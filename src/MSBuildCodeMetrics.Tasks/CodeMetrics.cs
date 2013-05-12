using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Xml.Serialization;
using MSBuildCodeMetrics.Core;
using System.Reflection;
using MSBuildCodeMetrics.Core.XML;

namespace MSBuildCodeMetrics.Tasks
{
	public class CodeMetrics : Task
	{		
		private IFileStreamFactory _streamFactory;
		private Dictionary<string, ICodeMetricsProvider> _providers = new Dictionary<string, ICodeMetricsProvider>();

		[Required]
		public string MetricsExePath
		{
			get;
			set;
		}

		[Required]
		public ITaskItem[] InputFiles
		{
			get;
			set;
		}

		[Required]
		public ITaskItem[] Providers 
		{ 
			get; 
			set; 
		}
		
		public string OutputFileName
		{
			get;
			set;
		}

		[Required]
		public ITaskItem[] Metrics
		{
			get;
			set;
		}

		private bool _showSummaryReport = true;
		public bool ShowSummaryReport
		{
			get { return _showSummaryReport; }
			set { _showSummaryReport = value; }
		}

		public bool ShowDetailsReport
		{
			get;
			set;
		}

		private bool _showConsoleOutput = true;
		public bool ShowConsoleOutput
		{
			get { return _showConsoleOutput; }
			set { _showConsoleOutput = value; }
		}

		private bool _fileOutput = false;
		public bool FileOutput
		{
			get { return _fileOutput; }
			set { _fileOutput = value; }
		}

		public CodeMetrics()
		{
			_streamFactory = new FileStreamFactory();
		}

		public CodeMetrics(IFileStreamFactory streamFactory)
		{
			_streamFactory = streamFactory;
		}		

		public override bool Execute()
		{
			try
			{
				MetricList ml = ParseMetrics();
				List<string> files = GetInputFileList();

				MSBuildCodeMetrics.Core.ILogger logger = new MSBuildLogger(Log);
				CodeMetricsRunner runner = new CodeMetricsRunner(logger);
				RegisterProviders(runner, logger);
				ValidateMetricsList(runner, ml);

				runner.ComputeMetrics(files);
				GenerateReport(runner, ml);
				return true;
			}
			catch (ArgumentException e)
			{
				Log.LogError(e.Message);
				return false;
			}			
		}

		private void ValidateMetricsList(CodeMetricsRunner runner,  MetricList ml)
		{
			foreach (ReportParam p in ml)
			{
				if (String.IsNullOrEmpty(p.ProviderName))
					throw new ArgumentNullException("ProviderName", "ProviderName must be informed in Metrics property");
				if (!_providers.ContainsKey(p.ProviderName))
					throw new ArgumentException("Invalid provider name in Metrics property: ProviderName: " + p.ProviderName);
				if (String.IsNullOrEmpty(p.MetricName))
					throw new ArgumentNullException("MetricName", "Metric name in property Metrics can't be null (ProviderName: " + p.ProviderName);
				if (!runner.IsMetricRegistered(p.ProviderName, p.MetricName))
					throw new ArgumentOutOfRangeException("MetricName", "Provider " + p.ProviderName + " doesn't know how to handle metric " + p.MetricName);
			}
		}

		private List<string> GetInputFileList()
		{
			List<string> files = new List<string>();
			foreach (ITaskItem i in InputFiles)
				files.Add(i.GetMetadata("FullPath"));
			return files;
		}

		private void RegisterProviders(CodeMetricsRunner runner, MSBuildCodeMetrics.Core.ILogger logger)
		{
			if (Providers.Length <= 0)
				throw new ArgumentOutOfRangeException("At least one Provider must me informed in Providers property");							

			foreach (ITaskItem item in Providers)
			{
				string typeName = item.ItemSpec;				
				ICodeMetricsProvider provider = CreateProviderInstance(typeName, item);								
				_providers.Add(provider.Name, provider);				

				runner.RegisterProvider(provider);
				if (provider is ILoggableCodeMetricsProvider)
					((ILoggableCodeMetricsProvider)provider).SetLogger(logger);
			}
		}

		private ICodeMetricsProvider CreateProviderInstance(string typeName, ITaskItem item)
		{
			Type providerType = Type.GetType(typeName);

			if (providerType == null)
				throw new ArgumentOutOfRangeException("Invalid provider: " + typeName + ". Couldn't create instance of this type");

			if (!(typeof(ICodeMetricsProvider).IsAssignableFrom(providerType)))
				throw new ArgumentOutOfRangeException("Type " + typeName + " doesn't implements ICodeMetricsProvider");		

			ICodeMetricsProvider provider = (ICodeMetricsProvider)providerType.GetConstructor(new Type[0]).Invoke(new object[0]);

			foreach (string metadataName in item.MetadataNames)
			{
				if (provider is IMetadataHandler)
					((IMetadataHandler)provider).AddMetadata(metadataName, item.GetMetadata(metadataName));
			}

			if (String.IsNullOrEmpty(provider.Name))
				throw new ArgumentNullException("Provider " + typeName + " doesn't implement property Name correctly");

			return provider;
		}

		private MetricList ParseMetrics()
		{
			if (Metrics == null)
				throw new ArgumentNullException("Metrics");

			if (Metrics.Length <= 0)
				throw new ArgumentOutOfRangeException("At least one Metrics must be informed in Metrics property");			

			MetricList ml = new MetricList();
			foreach (ITaskItem i in Metrics)
			{
				string metricName = i.ItemSpec;
				string providerName = i.GetMetadata("ProviderName");
				List<int> ranges = new List<int>();
				ParseRanges(i, ranges);
				ml.Add(providerName, metricName, ranges);
			}
			return ml;
		}

		private void ParseRanges(ITaskItem i, List<int> ranges)
		{
			bool hasRangesMetadata = (i.MetadataNames.Cast<string>().Where(metadataName => metadataName == "Ranges").Count() > 0);
			if (!hasRangesMetadata)
				return;

			if (String.IsNullOrEmpty(i.GetMetadata("Ranges")))
				return;
			
			foreach (string s in i.GetMetadata("Ranges").Split(';'))			
				ranges.Add(ParseRange(s));						
		}

		private int ParseRange(string s)
		{
			try
			{
				return Convert.ToInt32(s);
			}
			catch (FormatException)
			{
				throw new ArgumentOutOfRangeException("Ranges", "Ranges must be numbers separated by semicolon (Ex.: 10;5;1)");
			}
		}

		private void GenerateReport(CodeMetricsRunner runner, MetricList ml)
		{			
			MSBuildCodeMetricsReport report = runner.GenerateReport(ml, ShowSummaryReport, ShowDetailsReport);
			if (FileOutput)							
				GenerateFileOutput(report);
			if (ShowConsoleOutput)
				GenerateConsoleOutput(report);
		}

		private void GenerateConsoleOutput(MSBuildCodeMetricsReport report)
		{			
			if (ShowSummaryReport)							
				GenerateSummaryReportConsoleOutput(report);
			
			if (ShowDetailsReport)			
				GenerateDetailsReportConsoleOutput(report);	
		}

		private void GenerateDetailsReportConsoleOutput(MSBuildCodeMetricsReport report)
		{
			foreach (MetricReport m in report.Details.Metrics)
			{
				Log.LogMessage("");
				Log.LogMessage(m.ProviderName + ": " + m.MetricName);
				foreach (MeasureReport measure in m.Measures)
					Log.LogMessage(measure.MeasureName + ": " + measure.Value.ToString());
			}
		}

		private void GenerateSummaryReportConsoleOutput(MSBuildCodeMetricsReport report)
		{
			foreach (MetricReport m in report.Summary.Metrics)
			{
				Log.LogMessage("");
				Log.LogMessage(m.ProviderName + ": " + m.MetricName);
				foreach (SummaryRangeReport r in m.Ranges)
				{
					Log.LogMessage(r.Name + ": " + r.Count.ToString());
				}
			}
		}

		private void GenerateFileOutput(MSBuildCodeMetricsReport report)
		{		
			Stream outputReportStream = _streamFactory.GetStreamForOutputReport(OutputFileName);
			report.DeserializeToStream(outputReportStream);
			_streamFactory.CloseStream(outputReportStream);			
		}


	}
}
