using System;
using System.Collections.Generic;
using System.IO;
using MSBuildCodeMetrics.Core;
using MSBuildCodeMetrics.JetBrains.XML;

namespace MSBuildCodeMetrics.JetBrains
{
    public class DotCoverProvider : IMultiFileCodeMetricsProvider, IMetadataHandler, IProcessExecutorCodeMetricsProvider
    {
        private IProcessExecutor _processExecutor;
        private string _dotCoverPath;
        private string _dotCoverTargetExecutable;
        private string _dotCoverTargetArgument;
        private string _tempDir;
        private readonly IFileStreamFactory _fileStreamFactory;        
        private string _filters;

        public string Name
        {
            get { return "DotCover"; }
        }

        public IEnumerable<string> GetMetrics()
        {
            yield return "CodeCoverage";
            yield return "CoveredStatements";
            yield return "UncoveredStatements";
            yield return "TotalStatements";            
        }

        private ILogger _logger;
        public ILogger Logger
        {
            set { _logger = value; }
        }

        public DotCoverProvider(IFileStreamFactory fileStreamFactory)
        {
            _fileStreamFactory = fileStreamFactory;
        }

        public DotCoverProvider()
        {
            _fileStreamFactory = new FileStreamFactory();
        }

        public void AddMetadata(string name, string value)
        {
            if (name == "DotCoverPath")
                _dotCoverPath = value;
            else if (name == "DotCoverTargetExecutableParam")
                _dotCoverTargetExecutable = value;
            else if (name == "DotCoverTargetArgumentParam")
                _dotCoverTargetArgument = value;
            else if (name == "TempDir")
                _tempDir = value;      
            else if (name == "Filters")
                _filters = value;
        }

        public IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, IEnumerable<string> files)
        {
            if (String.IsNullOrEmpty(_dotCoverPath))
                throw new ArgumentNullException("DotCoverPath", "DotCoverPath must be specified using AddMetadata");
            if (String.IsNullOrEmpty(_dotCoverTargetExecutable))
                throw new ArgumentNullException("DotCoverTargetExecutableParam", "DotCoverTargetExecutableParam must be specified using AddMetadata");
            if (String.IsNullOrEmpty(_dotCoverTargetArgument))
                throw new ArgumentNullException("DotCoverTargetArgumentParam", "DotCoverTargetArgumentParam must be specified using AddMetadata");
            if (String.IsNullOrEmpty(_tempDir))
                throw new ArgumentNullException("TempDir", "TempDir must be specified using AddMetadata");
                        
            var outputFiles = new List<string>();
            foreach (var file in files)
            {
                var targetArguments = String.Format(_dotCoverTargetArgument, file);
                var fileInfo = new FileInfo(file);
                var outputFile = _tempDir + "\\" + fileInfo.Name + ".dcvr";
                var arguments = "cover /TargetExecutable=\"" + _dotCoverTargetExecutable +
                                "\" /TargetArguments=\"" + targetArguments + "\" /Output=\"" + outputFile + "\"";
                if (!String.IsNullOrEmpty(_filters))
                    arguments += " /Filters=" + _filters;
                _processExecutor.ExecuteProcess(_dotCoverPath, arguments);
                outputFiles.Add(outputFile);                
            }

            var argumentsMerge = String.Empty;
            foreach (var outputFile in outputFiles)
            {
                if (argumentsMerge != string.Empty)
                    argumentsMerge += ";";
                argumentsMerge += outputFile;
            }
            argumentsMerge = "merge /Source=\"" + argumentsMerge + "\" /Output=\"" + _tempDir + "\\MSBuildCodeMetricsMergedCoverage.dcvr\"";
            _processExecutor.ExecuteProcess(_dotCoverPath, argumentsMerge);

            var argumentsReport = "report /Source=\"" + _tempDir + "\\MSBuildCodeMetricsMergedCoverage.dcvr" +
                                  "\" /Output=\"" + _tempDir + "\\MSBuildCodeMetricsMergedCoverage.report.xml\" /ReportType=XML";

            _processExecutor.ExecuteProcess(_dotCoverPath, argumentsReport);

            var stream = _fileStreamFactory.OpenFile(_tempDir + "\\MSBuildCodeMetricsMergedCoverage.report.xml");                
            return GetMetricsForReport(stream);                                   
        }

        private IEnumerable<ProviderMeasure> GetMetricsForReport(Stream stream)
        {            
            var report = DotCoverReport.Deserialize(stream);
            stream.Close();

            var measures = new List<ProviderMeasure>();

            report.Assemblies.ForEach(a =>
            {
                a.Namespaces.ForEach(n =>
                {
                    n.Types.ForEach(t =>
                    {
                        var fullTypeName = n.Name + "." + t.Name;
                        if (t.TotalStatements == 0)
                            measures.Add(new ProviderMeasure("CodeCoverage", fullTypeName, 100));
                        else
                            measures.Add(new ProviderMeasure("CodeCoverage", fullTypeName, t.CoveragePercent));
                        measures.Add(new ProviderMeasure("CoveredStatements", fullTypeName, t.CoveredStatements));
                        measures.Add(new ProviderMeasure("TotalStatements", fullTypeName, t.TotalStatements));
                        measures.Add(new ProviderMeasure("UncoveredStatements", fullTypeName, t.TotalStatements - t.CoveredStatements));
                    });
                });
            });

            return measures;
        }

        public IProcessExecutor ProcessExecutor
        {
            set { _processExecutor = value; }
        }

    }
}
