using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core.XML
{
	public class DetailsReport
	{
		private MSBuildCodeMetricsReport _report;
		public MSBuildCodeMetricsReport Report
		{
			get { return _report; }
		}

		private List<MetricReport> _metrics = new List<MetricReport>();
		public List<MetricReport> Metrics
		{
			get { return _metrics; }
			set { _metrics = value; }
		}

		public MetricReport AddMetric(string providerName, string metricName)
		{
			MetricReport m = new MetricReport(providerName, metricName);
			_metrics.Add(m);
			return m;
		}

		public DetailsReport()
		{
		}

		public DetailsReport(MSBuildCodeMetricsReport report)
		{
			_report = report;
		}
	}
}
