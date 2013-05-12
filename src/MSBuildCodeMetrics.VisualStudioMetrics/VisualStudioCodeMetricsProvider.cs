using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using MSBuildCodeMetrics.VisualStudioCodeMetrics.XML;
using System.Diagnostics;
using MSBuildCodeMetrics.Core;

namespace MSBuildCodeMetrics.VisualStudioMetrics 
{
	public class VisualStudioCodeMetricsProvider : ISingleFileCodeMetricsProvider, ILoggableCodeMetricsProvider, IMetadataHandler
	{
		private List<ModuleReport> _reports = new List<ModuleReport>();
		private string _metricsExePath;
		private string _tempDir;
		private ILogger _logger;

		public string Name
		{
			get { return "VisualStudioMetrics"; }
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
			List<string> l = new List<string>();
			l.Add("MaintainabilityIndex");
			l.Add("ClassCoupling");
			l.Add("DepthOfInheritance");
			l.Add("LinesOfCode");
			l.Add("CyclomaticComplexity");
			return l;
		}

		public IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, string fileName)
		{
			string tempFileName = GetTempFileFor(fileName);
			RunExternalExecutable(fileName, tempFileName);
			return GetMeasuresForFile(metricsToCompute, tempFileName);
		}

		private void RunExternalExecutable(string fileName, string tempFileName)
		{
			FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);

			Process p = CreateProcessInstanceForConsoleApp(fileName, tempFileName, fi);
			if (!p.Start())
				throw new Exception("Error starting process: " + p.StartInfo.FileName + p.StartInfo.Arguments);
			p.WaitForExit();

			while (!p.StandardOutput.EndOfStream)
				_logger.LogMessage(p.StandardOutput.ReadLine());
			while (!p.StandardError.EndOfStream)
				_logger.LogError(p.StandardError.ReadLine());				

			if (p.ExitCode != 0)
				throw new Exception("Error running process: " + p.StartInfo.FileName + p.StartInfo.Arguments + ". Exit code " + p.ExitCode.ToString() +
					Environment.NewLine);
		}

		private Process CreateProcessInstanceForConsoleApp(string fileName, string tempFileName, FileInfo fi)
		{
			if (String.IsNullOrEmpty(_metricsExePath))
			{
				_logger.LogMessage("MetricsExePath not specified as metadata");
				_metricsExePath = Environment.GetEnvironmentVariable("VS100COMNTOOLS") + @"..\..\Team Tools\Static Analysis Tools\FxCop\Metrics.exe";
				_logger.LogMessage("Trying default: " + _metricsExePath);
			}
			else
				_logger.LogMessage("MetricsExePath: " + _metricsExePath);

			if (!File.Exists(_metricsExePath))
				_logger.LogError("File not found: " + _metricsExePath);

			Process p = new Process();
			p.StartInfo.FileName = "\"" + _metricsExePath + "\"";
			p.StartInfo.Arguments = " /file:" + "\"" + fileName + "\" /out:\"" + tempFileName + "\" /gac ";
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.WorkingDirectory = fi.DirectoryName;
			return p;
		}

		private IEnumerable<ProviderMeasure> GetMeasuresForFile(IEnumerable<string> metricsToCompute, string tempFileName)
		{
			FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
			ModuleReport mr = null;
			using (fs)			
				mr = CodeMetricsReport.Deserialize(fs);

			List<MemberReport> allMembers = mr.GetAllMembers();
			List<ProviderMeasure> result = GetMeasures(metricsToCompute, allMembers);

			return result;			
		}

		private static List<ProviderMeasure> GetMeasures(IEnumerable<string> metricsToCompute, List<MemberReport> allMembers)
		{
			Dictionary<string, string> d = metricsToCompute.ToDictionary<string, string>(s => s);

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
			return result;
		}

		private string GetTempFileFor(string fileName)
		{
			if (String.IsNullOrEmpty(_tempDir))
				throw new ArgumentNullException("TempDir", "VisualStudioMetricsProvider must receive a TempDir parameter as metadata");

			DirectoryInfo di = new DirectoryInfo(_tempDir);
			if (!Directory.Exists(_tempDir))
				Directory.CreateDirectory(_tempDir);

			FileInfo fi = new FileInfo(fileName);

			return _tempDir + "\\" + fi.Name + ".metrics.xml";
		}

		public void SetLogger(ILogger logger)
		{
			_logger = logger;
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
