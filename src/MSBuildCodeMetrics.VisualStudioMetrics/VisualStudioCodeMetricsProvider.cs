using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MSBuildCodeMetrics.VisualStudioMetrics.XML;
using MSBuildCodeMetrics.Core;

namespace MSBuildCodeMetrics.VisualStudioMetrics 
{
	public class VisualStudioCodeMetricsProvider : ISingleFileCodeMetricsProvider, ILoggableCodeMetricsProvider, IMetadataHandler, IProcessExecutorCodeMetricsProvider
	{		
		private string _metricsExePath;
		private string _tempDir;
		private ILogger _logger;
        private IProcessExecutor _processExecutor;

		public string Name
		{
			get { return "VisualStudioMetrics"; }
		}

        public IProcessExecutor ProcessExecutor
        {
            set { _processExecutor = value; }
        }

		public VisualStudioCodeMetricsProvider()
		{
		}

		public VisualStudioCodeMetricsProvider(string metricsExePath, string tempDir)
		{
			_metricsExePath = metricsExePath;
			_tempDir = tempDir;
		}

        public IEnumerable<string> GetMetrics()
		{
			yield return "MaintainabilityIndex";
            yield return "ClassCoupling";
            yield return "DepthOfInheritance";
            yield return "LinesOfCode";
            yield return "CyclomaticComplexity";		    
		}

		public IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, string fileName)
		{
			string tempFileName = GetTempFileFor(fileName);

            FindDefaultMetricsExeIfNotSpecified();

            string arguments = " /file:" + "\"" + fileName + "\" /out:\"" + tempFileName + "\" /gac ";
		    _processExecutor.ExecuteProcess(_metricsExePath, arguments);
			
			return GetMeasuresForFile(metricsToCompute, tempFileName);
		}

	    private void FindDefaultMetricsExeIfNotSpecified()
	    {
	        if (String.IsNullOrEmpty(_metricsExePath))
	        {
	            _logger.LogMessage("MetricsExePath not specified as metadata");
	            _metricsExePath = Environment.GetEnvironmentVariable("VS120COMNTOOLS") +
	                              @"..\..\Team Tools\Static Analysis Tools\FxCop\Metrics.exe";
	            _logger.LogMessage("Trying default: " + _metricsExePath);
	        }
	    }

	    private IEnumerable<ProviderMeasure> GetMeasuresForFile(IEnumerable<string> metricsToCompute, string tempFileName)
		{
			FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
			ModuleReport mr;
			using (fs)			
				mr = CodeMetricsReport.Deserialize(fs);

			List<MemberReport> allMembers = mr.GetAllMembers();
			List<ProviderMeasure> result = GetMeasures(metricsToCompute, allMembers);

			return result;			
		}

		private static List<ProviderMeasure> GetMeasures(IEnumerable<string> metricsToCompute, List<MemberReport> allMembers)
		{
			Dictionary<string, string> d = metricsToCompute.ToDictionary(s => s);

			List<ProviderMeasure> result = new List<ProviderMeasure>();
			foreach (MemberReport member in allMembers)
			{
				foreach (MetricReport metric in member.MetricsList)
				{
					if (!d.ContainsKey(metric.Name))
						continue;

					result.Add(new ProviderMeasure(metric.Name, member.Type.Namespace.Name + "." +
						member.Type.Name + "." + member.Name, Convert.ToInt32(metric.Value)));
				}
			}
			return result.OrderByDescending(m => m.Value).ToList();
		}

		private string GetTempFileFor(string fileName)
		{
			if (String.IsNullOrEmpty(_tempDir))
				throw new ArgumentNullException("TempDir", "VisualStudioMetricsProvider must receive a TempDir parameter as metadata");
			
			if (!Directory.Exists(_tempDir))
				Directory.CreateDirectory(_tempDir);

			FileInfo fi = new FileInfo(fileName);

			return _tempDir + "\\" + fi.Name + ".metrics.xml";
		}

		public ILogger Logger
		{
            set { _logger = value; }
		}

		public void AddMetadata(string name, string value)
		{
			if (name == "TempDir")
				_tempDir = value;
			else if (name == "MetricsExePath")
				_metricsExePath = value;
		}
    }
}
