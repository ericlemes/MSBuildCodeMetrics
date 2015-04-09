using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core.XML
{
	/// <summary>
	/// This class represents the Summary section of the XML report. This class is used only for XML serialization purposes. 
	/// </summary>
	public class SummaryReport
	{
		private List<MetricReport> _metrics = new List<MetricReport>();
		/// <summary>
		/// The metrics of the report.
		/// </summary>
		public List<MetricReport> Metrics
		{
			get { return _metrics; }		
		}

		private MSBuildCodeMetricsReport _report;
		/// <summary>
		/// The parent report
		/// </summary>
		public MSBuildCodeMetricsReport Report
		{
			get { return _report; }
		}

		/// <summary>
		/// Adds a new metric and returns the metric
		/// </summary>
		/// <param name="providerName">Provider name</param>
		/// <param name="metricName">Metric name</param>
		/// <returns>The new metric</returns>
		public MetricReport AddMetric(string providerName, string metricName)
		{
			MetricReport m = new MetricReport(providerName, metricName);
			_metrics.Add(m);
			return m;
		}

		/// <summary>
		/// Creates a new summary report
		/// </summary>
		public SummaryReport()
		{
		}

		/// <summary>
		/// Creates a new summary report and hooks the parent report
		/// </summary>
		/// <param name="report">The parent report</param>
		public SummaryReport(MSBuildCodeMetricsReport report)
		{
			_report = report;
		}
	}
}
