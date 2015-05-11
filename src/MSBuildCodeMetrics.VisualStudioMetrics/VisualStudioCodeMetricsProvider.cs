using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MSBuildCodeMetrics.VisualStudioMetrics.XML;
using MSBuildCodeMetrics.Core;

namespace MSBuildCodeMetrics.VisualStudioMetrics 
{
	/// <summary>
	/// Code metrics provider to handle Visual Studio Metrics
	/// </summary>
	/// <remarks>
	/// This provider expects TempDir (temporary dir to run Metrics.exe) and MetricsExePath (the Metrics.exe full path).
	/// If MetricsExePath isn't provided, the default location is %VS120COMNTOOLS")\..\..\Team Tools\Static Analysis Tools\FxCop\Metrics.exe
	/// </remarks>
	public class VisualStudioCodeMetricsProvider : ISingleFileCodeMetricsProvider, ILoggableCodeMetricsProvider, IMetadataHandler, IProcessExecutorCodeMetricsProvider
	{		
		private string _metricsExePath;
		private string _tempDir;
		private ILogger _logger;
        private IProcessExecutor _processExecutor;

		/// <summary>
		/// Name
		/// </summary>
		public string Name
		{
			get { return "VisualStudioMetrics"; }
		}

        /// <summary>
        /// Setter for process executor
        /// </summary>
        public IProcessExecutor ProcessExecutor
        {
            set { _processExecutor = value; }
        }

		/// <summary>
		/// Default constructor
		/// </summary>
		public VisualStudioCodeMetricsProvider()
		{
		}

		/// <summary>
		/// Constructor receiving dependencies
		/// </summary>
		/// <param name="metricsExePath">Metrics.exe full path</param>
		/// <param name="tempDir">Temporary dir used in the execution of metrics.exe</param>
		public VisualStudioCodeMetricsProvider(string metricsExePath, string tempDir)
		{
			_metricsExePath = metricsExePath;
			_tempDir = tempDir;
		}

		/// <summary>
		/// Gets metrics computed by this provider: MaintainabilityIndex, ClassCoupling, DepthOfInheritance, LinesOfCode and CyclomaticComplexity
		/// </summary>
		/// <returns>a set of metrics</returns>
		public IEnumerable<string> GetMetrics()
		{
			yield return "MaintainabilityIndex";
            yield return "ClassCoupling";
            yield return "DepthOfInheritance";
            yield return "LinesOfCode";
            yield return "CyclomaticComplexity";		    
		}

		/// <summary>
		/// Compute metrics
		/// </summary>
		/// <param name="metricsToCompute">Metrics</param>
		/// <param name="fileName">File name</param>
		/// <returns>a set of measures</returns>
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

		/// <summary>
		/// Sets the logger 
		/// </summary>		
		public ILogger Logger
		{
            set { _logger = value; }
		}

		/// <summary>
		/// Adds metadata
		/// </summary>
		/// <param name="name">Metadata name</param>
		/// <param name="value">Metadata value</param>
		public void AddMetadata(string name, string value)
		{
			if (name == "TempDir")
				_tempDir = value;
			else if (name == "MetricsExePath")
				_metricsExePath = value;
		}
    }
}
