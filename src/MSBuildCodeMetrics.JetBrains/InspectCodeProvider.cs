﻿using MSBuildCodeMetrics.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MSBuildCodeMetrics.JetBrains.XML;

namespace MSBuildCodeMetrics.JetBrains
{

    /// <summary>
    /// Provider for JetBrains InspectCode.exe (Resharper Command Line). Get the number of violations from a specified solution.
    /// Expects metadatas:
    /// - InspectCodePath: FullPathToInspectCode    
    /// - DotSettingsFile: Path to DotSettingsFile
    /// - TempDir: Directory to output .metrics.xml files
    /// </summary>
    public class InspectCodeProvider : IMultiFileCodeMetricsProvider, ILoggableCodeMetricsProvider, IProcessExecutorCodeMetricsProvider, IMetadataHandler
    {
        private IProcessExecutor _processExecutor;
        private string _inspectCodePath;        
        private string _dotSettingsFile;
        private string _tempDir;
        private IFileStreamFactory _fileStreamFactory;

        public string Name
        {
            get { return "InspectCode"; }
        }

        public IEnumerable<string> GetMetrics()
        {
            yield return "AllViolations";
            yield return "Warnings";
            yield return "Suggestions";
            yield return "Errors";
            yield break;            
        }

        public ILogger Logger
        {
            set { }
        }

        public IProcessExecutor ProcessExecutor
        {
            set { _processExecutor = value; }
        }

        public void AddMetadata(string name, string value)
        {
            if (name == "InspectCodePath")
                _inspectCodePath = value;            
            else if (name == "DotSettingsFile")
                _dotSettingsFile = value;            
            else if (name == "TempDir")
                _tempDir = value;
        }

        public InspectCodeProvider()
        {
            _fileStreamFactory = new FileStreamFactory();            
        }

        public InspectCodeProvider(IFileStreamFactory fileStreamFactory)
        {
            _fileStreamFactory = fileStreamFactory;
        }

        public IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, IEnumerable<string> files)
        {
            if (String.IsNullOrEmpty(_inspectCodePath))
                throw new ArgumentNullException("InspectCodePath", "InspectCodePath should be informed used AddMetadata method");
            if (String.IsNullOrEmpty(_tempDir))
                throw new ArgumentNullException("TempDir", "TempDir should be informed used AddMetadata method");

            var measures = new List<ProviderMeasure>();

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);                
                var outputFile = _tempDir + "\\" + fi.Name + ".metrics.xml";
                var arguments = "\"" + file + "\" /o=\"" + outputFile + "\"";
                if (!String.IsNullOrEmpty(_dotSettingsFile))
                    arguments += " /p=\"" + _dotSettingsFile + "\"";

                _processExecutor.ExecuteProcess(_inspectCodePath, arguments);

                var fileStream = _fileStreamFactory.OpenFile(outputFile);
                measures.AddRange(ParseMetricsReport(fileStream));
            }                        

            return measures;
        }

        private IEnumerable<ProviderMeasure> ParseMetricsReport(Stream fileStream)
        {
            var report = InspectCodeReport.Deserialize(fileStream);
            var typeSeverityDict = GetTypeSeverityDict(report);
            var measures = new List<ProviderMeasure>();
                
            report.Issues.ForEach(p =>
            {
                var severityCount = new Dictionary<string, int>
                {
                    {"WARNING", 0},
                    {"ERROR", 0},
                    {"SUGGESTION", 0}
                };
                p.Issues.ForEach(i => { severityCount[typeSeverityDict[i.TypeId].Severity]++; });

                CreateMeasures(measures, p, severityCount);
            });

            return measures;
        }

        private void CreateMeasures(List<ProviderMeasure> measures, Project p, Dictionary<string, int> severityCount)
        {
            measures.Add(
                new ProviderMeasure("AllViolations", p.Name,
                    severityCount["WARNING"] + severityCount["ERROR"] + severityCount["SUGGESTION"])
                );
            measures.Add(
                new ProviderMeasure("Warnings", p.Name, severityCount["WARNING"])
                );
            measures.Add(
                new ProviderMeasure("Errors", p.Name, severityCount["ERROR"])
                );
            measures.Add(
                new ProviderMeasure("Suggestions", p.Name, severityCount["SUGGESTION"])
                );
        }

        private Dictionary<string, IssueType> GetTypeSeverityDict(InspectCodeReport report)
        {
            var typeSeverityDict = new Dictionary<string, IssueType>();
            report.IssueTypes.ForEach(it =>
            {
                typeSeverityDict.Add(it.Id, it);
            });
            return typeSeverityDict;
        } 
    }
}
