using MSBuildCodeMetrics.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// Gets the name of this provider
        /// </summary>
        public string Name
        {
            get { return "InspectCode"; }
        }

        /// <summary>
        /// Get available metrics for this provider. Returns AllViolations (all warnings, errors and suggestions, assembly by assembly),
        /// Warnings (all warnings, assembly by assembly), Suggestions (all suggestions, assembly by assembly), Errors (all errors, 
        /// assembly by assembly, AllViolationsAllFiles (all warnings, errors and suggestions, aggregated for all solutions inspected),
        /// WarningsAllFiles (all warnings, aggregated by all solutions inspected), AllSuggestions (all suggestions, aggregated by all
        /// solutions inspected) and AllErrors (all errors, aggregated by all solutions inspected). The purpose of the AllFiles metrics
        /// is to break the build when a given number of violations is reached.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetMetrics()
        {
            yield return "AllViolations";
            yield return "Warnings";
            yield return "Suggestions";
            yield return "Errors";
            yield return "AllViolationsAllFiles";
            yield return "WarningsAllFiles";
            yield return "SuggestionsAllFiles";
            yield return "ErrorsAllFiles";
            yield break;            
        }

        /// <summary>
        /// Used by core to inject logger to provider
        /// </summary>
        public ILogger Logger
        {
            set { }
        }

        /// <summary>
        /// Used by core to inject process executor to provider
        /// </summary>
        public IProcessExecutor ProcessExecutor
        {
            set { _processExecutor = value; }
        }

        /// <summary>
        /// Invoked from core on providers to set properties sent from MSBuild engine.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddMetadata(string name, string value)
        {
            if (name == "InspectCodePath")
                _inspectCodePath = value;            
            else if (name == "DotSettingsFile")
                _dotSettingsFile = value;            
            else if (name == "TempDir")
                _tempDir = value;
        }

        /// <summary>
        /// Constructor for production code.
        /// </summary>
        public InspectCodeProvider()
        {
            _fileStreamFactory = new FileStreamFactory();            
        }

        /// <summary>
        /// Constructor used in tests for mocking IFileStreamFactory
        /// </summary>
        /// <param name="fileStreamFactory"></param>
        public InspectCodeProvider(IFileStreamFactory fileStreamFactory)
        {
            _fileStreamFactory = fileStreamFactory;
        }

        /// <summary>
        /// Computes de metrics. Executes the InspectCode executable and parses xml response.
        /// </summary>
        /// <param name="metricsToCompute"></param>
        /// <param name="files"></param>
        /// <returns></returns>
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

            ConsolidateMetricsForAllFiles(measures);

            return measures;
        }

        private void ConsolidateMetricsForAllFiles(List<ProviderMeasure> measures)
        {
            var warningsAllFiles = measures.Where(m => m.MetricName == "Warnings").Sum(m => m.Value);
            var errorsAllFiles = measures.Where(m => m.MetricName == "Errors").Sum(m => m.Value);
            var suggestionsAllFiles = measures.Where(m => m.MetricName == "Suggestions").Sum(m => m.Value);

            measures.Add(new ProviderMeasure("WarningsAllFiles", "AllFiles", warningsAllFiles));
            measures.Add(new ProviderMeasure("ErrorsAllFiles", "AllFiles", errorsAllFiles));
            measures.Add(new ProviderMeasure("SuggestionsAllFiles", "AllFiles", suggestionsAllFiles));
            measures.Add(new ProviderMeasure("AllViolationsAllFiles", "AllFiles", warningsAllFiles + errorsAllFiles + suggestionsAllFiles));
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
