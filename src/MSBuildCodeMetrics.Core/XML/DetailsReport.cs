using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core.XML
{
	/// <summary>
	/// This class represent the Details section of the report XML. This is used only for XML serialization purpose.
	/// </summary>
	public class DetailsReport
	{
		private MSBuildCodeMetricsReport _report;
		/// <summary>
		/// The parent report
		/// </summary>
		public MSBuildCodeMetricsReport Report
		{
			get { return _report; }
		}

		private List<MetricReport> _metrics = new List<MetricReport>();
		/// <summary>
		/// The metrics
		/// </summary>
		public List<MetricReport> Metrics
		{
			get { return _metrics; }			
		}

		/// <summary>
		/// Creates a new metric, adds it to the report and return the reference of the new metric.
		/// </summary>
		/// <param name="providerName">The provider name</param>
		/// <param name="metricName">The metric name</param>
		/// <returns>The metric report</returns>
		public MetricReport AddMetric(string providerName, string metricName)
		{
			MetricReport m = new MetricReport(providerName, metricName);
			_metrics.Add(m);
			return m;
		}

		/// <summary>
		/// Creates a new details report
		/// </summary>
		public DetailsReport()
		{
		}

		/// <summary>
		/// Creates a new details report and hooks the parent report.
		/// </summary>
		/// <param name="report">The parent report</param>
		public DetailsReport(MSBuildCodeMetricsReport report)
		{
			_report = report;
		}
	}
}
