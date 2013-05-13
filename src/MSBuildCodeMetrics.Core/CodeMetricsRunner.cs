using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSBuildCodeMetrics.Core.XML;
using MSBuildCodeMetrics.Core.Ranges;

namespace MSBuildCodeMetrics.Core
{
	public class CodeMetricsRunner
	{
		private List<ICodeMetricsProvider> _providers = new List<ICodeMetricsProvider>();
		private Dictionary<string, ICodeMetricsProvider> _providerRegistry = new Dictionary<string, ICodeMetricsProvider>();
		private Dictionary<ICodeMetricsProvider, List<string>> _metrics = new Dictionary<ICodeMetricsProvider, List<string>>();
		private List<RunnerMeasure> _measures = new List<RunnerMeasure>();
		private ILogger _logger;

		public CodeMetricsRunner(ILogger logger)
		{
			_logger = logger;
		}

		public void RegisterProvider(ICodeMetricsProvider provider)
		{
			_providers.Add(provider);
			_metrics.Add(provider, provider.GetMetrics().ToList<string>());
			_providerRegistry.Add(provider.Name, provider);
		}

		public ICodeMetricsProvider GetProvider(string name)
		{
			return _providers.Where(p => p.Name == name).FirstOrDefault();
		}

		public bool IsMetricRegistered(string providerName, string metricName)
		{
			return _metrics[GetProvider(providerName)].IndexOf(metricName) >= 0;
		}

		public void ComputeMetrics(IEnumerable<ComputeMetricsParameter> parameters)
		{
			foreach (ComputeMetricsParameter p in parameters)
				if (p.Files.Count() <= 0)
					throw new ArgumentOutOfRangeException("InputFiles shouldn't be empty.");

			var providers =
				(from ComputeMetricsParameter p in parameters
				 select p.ProviderName).Distinct();

			foreach (string providerName in providers)
			{
				if (!_providerRegistry.ContainsKey(providerName))
					throw new ArgumentOutOfRangeException("providerName", "ProviderName informed in parameters isn't registered");
				var metrics =
					(from ComputeMetricsParameter p in parameters
					 where p.ProviderName == providerName
					 select p.MetricName).Distinct();

				List<string> files = new List<string>();
				foreach (string metricName in metrics)
				{				
					var fileLists =
						(from ComputeMetricsParameter p in parameters
						 where p.ProviderName == providerName && p.MetricName == metricName
						 select p.Files);
					foreach (List<string> fileList in fileLists)
						files.AddRange(fileList);
				}

				if (_providerRegistry[providerName] is ISingleFileCodeMetricsProvider)
				{
					foreach (string file in files.Distinct())
						foreach (ProviderMeasure pm in ((ISingleFileCodeMetricsProvider)_providerRegistry[providerName]).ComputeMetrics(metrics, file))
							_measures.Add(new RunnerMeasure(providerName, pm));
				}
				else if (_providerRegistry[providerName] is IMultiFileCodeMetricsProvider)
				{
					foreach (ProviderMeasure pm in ((IMultiFileCodeMetricsProvider)_providerRegistry[providerName]).ComputeMetrics(metrics, files.Distinct()))
						_measures.Add(new RunnerMeasure(providerName, pm));
				}

			}
		}

		public IEnumerable<RunnerMeasure> GetMeasuresByProvider(string providerName)
		{
			return
				(from RunnerMeasure rm in _measures
				 where rm.ProviderName == providerName
				 select rm);
		}

		public IList<RunnerMeasure> GetMeasuresByMetric(string providerName, string metricName)
		{
			return
				(from RunnerMeasure rm in _measures
				 where rm.ProviderName == providerName && rm.MetricName == metricName
				 select rm).ToList<RunnerMeasure>();
		}

		public MSBuildCodeMetricsReport GenerateReport(List<Metric> metricList, bool summary, bool details)
		{
			if (metricList == null)
				throw new ArgumentNullException("metricList");
			if (metricList.Count <= 0)
				throw new ArgumentOutOfRangeException("metricList");
			if (!summary && !details)
				throw new ArgumentOutOfRangeException("", "At least one report must be set to true");

			MSBuildCodeMetricsReport result = new MSBuildCodeMetricsReport();

			foreach (Metric p in metricList)
			{
				if (summary)
					AppendSummaryReport(result, p.ProviderName, p.MetricName, GetSummaryReport(p.ProviderName, p.MetricName, p.Range));
				if (details)
					AppendDetailsReport(result, p.ProviderName, p.MetricName, GetDetailedReport(p.ProviderName, p.MetricName));
			}

			return result;
		}

		private void AppendSummaryReport(MSBuildCodeMetricsReport result, string providerName, string metricName,
			IList<SummaryReportRange> summaryReport)
		{
			if (result.Summary == null)
				result.CreateSummary();

			MetricReport metricSummaryReport = result.Summary.AddMetric(providerName, metricName).CreateRanges();

			foreach (SummaryReportRange r in summaryReport)
				metricSummaryReport.AddRange(r.Range.Name, r.Count);
		}

		private void AppendDetailsReport(MSBuildCodeMetricsReport result, string providerName, string metricName, IEnumerable<RunnerMeasure> detailedReport)
		{
			if (result.Details == null)
				result.CreateDetails();

			MetricReport detailsReport = result.Details.AddMetric(providerName, metricName).CreateMeasures();

			foreach (RunnerMeasure rm in detailedReport)
				detailsReport.AddMeasure(rm.MeasureName, rm.Value);
		}

		private IList<SummaryReportRange> GetSummaryReport(string providerName, string metricName, IEnumerable<int> rangeList)
		{
			if (rangeList == null)
				throw new ArgumentNullException("rangeList");
			if (rangeList.ToList<int>().Count <= 0)
				throw new ArgumentOutOfRangeException("rangeList", "Can't be empty");

			List<int> orderedRangeList = rangeList.Distinct().OrderByDescending(i => i).ToList();

			if (rangeList.ToList<int>().Count == 1)
				return GetReportWithTwoRanges(providerName, metricName, orderedRangeList);
			else
				return GetReportWithMoreThanTwoRanges(providerName, metricName, orderedRangeList);
		}

		private List<SummaryReportRange> GetReportWithTwoRanges(string providerName, string metricName, IList<int> rangeList)
		{
			List<SummaryReportRange> result = new List<SummaryReportRange>();
			AppendGreaterThanRange(providerName, metricName, result, rangeList[0]);
			AppendLowerOrEqualRange(providerName, metricName, result, rangeList[0]);
			return result;
		}

		private IList<SummaryReportRange> GetReportWithMoreThanTwoRanges(string providerName, string metricName, IList<int> rangeList)
		{
			List<SummaryReportRange> result = new List<SummaryReportRange>();
			for (int i = 0; i < rangeList.Count; i++)
			{
				if (i == 0)
					AppendGreaterThanRange(providerName, metricName, result, rangeList[i]);
				else
				{
					AppendBetweenUpperAndLowerRange(providerName, metricName, result, rangeList[i - 1], rangeList[i]);
					if (i == rangeList.Count - 1)
						AppendLowerOrEqualRange(providerName, metricName, result, rangeList[i]);
				}
			}
			return result;
		}

		public IEnumerable<RunnerMeasure> GetDetailedReport(string providerName, string metricName)
		{
			return
				(from RunnerMeasure rm in _measures
				 where rm.ProviderName == providerName && rm.MetricName == metricName
				 select rm).OrderByDescending(m => m.Value);
		}

		private void AppendGreaterThanRange(string providerName, string metricName, List<SummaryReportRange> result, int rangeValue)
		{
			int count =
				(from RunnerMeasure rm in _measures
				 where rm.Value > rangeValue && rm.MetricName == metricName && rm.ProviderName == providerName
				 select rm).Count();

			result.Add(new SummaryReportRange(new GreaterThanRangeType(rangeValue), count));
		}

		private void AppendLowerOrEqualRange(string providerName, string metricName, List<SummaryReportRange> result, int rangeValue)
		{
			int count =
				(from RunnerMeasure rm in _measures
				 where rm.Value <= rangeValue && rm.MetricName == metricName && rm.ProviderName == providerName
				 select rm).Count();

			result.Add(new SummaryReportRange(new LowerOrEqualRangeType(rangeValue), count));
		}

		private void AppendBetweenUpperAndLowerRange(string providerName, string metricName,
			List<SummaryReportRange> result, int upperValue, int lowerValue)
		{
			int count =
				(from RunnerMeasure rm in _measures
				 where rm.Value <= upperValue && rm.Value > lowerValue && rm.ProviderName == providerName && rm.MetricName == metricName
				 select rm).Count();

			result.Add(new SummaryReportRange(new BetweenUpperAndLowerRangeType(upperValue, lowerValue), count));
		}

	}
}
