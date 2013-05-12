using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core.XML
{
	public class SummaryReport
	{
		private List<MetricReport> _metrics = new List<MetricReport>();
		public List<MetricReport> Metrics
		{
			get { return _metrics; }
			set { _metrics = value; }
		}

		private MSBuildCodeMetricsReport _report;
		public MSBuildCodeMetricsReport Report
		{
			get { return _report; }
		}

		public MetricReport AddMetric(string providerName, string metricName)
		{
			MetricReport m = new MetricReport(providerName, metricName);
			_metrics.Add(m);
			return m;
		}

		public SummaryReport()
		{
		}

		public SummaryReport(MSBuildCodeMetricsReport report)
		{
			_report = report;
		}
	}
}
