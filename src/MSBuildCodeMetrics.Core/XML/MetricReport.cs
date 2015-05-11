using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.Core.XML
{
	/// <summary>
	/// This class represent a metric in XML report. This is used only for XML serialization purpose.
	/// </summary>
	[XmlType("Metric")]
	public class MetricReport
	{
		/// <summary>
		/// Provider name
		/// </summary>
		[XmlAttribute]
		public string ProviderName
		{
			get;
			set;
		}

		/// <summary>
		/// Metric name
		/// </summary>
		[XmlAttribute]
		public string MetricName
		{
			get;
			set;
		}
		
		/// <summary>
		/// Ranges of the report
		/// </summary>
		public List<SummaryRangeReport> Ranges
		{
			get;
			set;
		}

		/// <summary>
		/// Measures
		/// </summary>
		public List<MeasureReport> Measures
		{
			get;
			set;
		}

		/// <summary>
		/// Creates a new MetricReport
		/// </summary>
		public MetricReport()
		{
		}

		/// <summary>
		/// Creates a new MetricReport
		/// </summary>
		/// <param name="providerName">Provider name</param>
		/// <param name="metricName">Metric name</param>
		public MetricReport(string providerName, string metricName)
		{
			ProviderName = providerName;
			MetricName = metricName;
		}

		/// <summary>
		/// Create a new SummaryRangeReport and hooks the Ranges property
		/// </summary>
		/// <returns>the report</returns>
		public MetricReport CreateRanges()
		{
			Ranges = new List<SummaryRangeReport>();
			return this;
		}
		
		/// <summary>
		/// Create a new SummaryRangeReport and adds it to the Ranges property
		/// </summary>
		/// <param name="rangeName">Range name</param>
		/// <param name="count">count</param>
		/// <returns>the report</returns>
		public MetricReport AddRange(string rangeName, int count)
		{
			SummaryRangeReport sr = new SummaryRangeReport(rangeName, count);
			Ranges.Add(sr);
			return this;
		}

		/// <summary>
		/// Create a new list of MeasureReport and hooks the Measures property
		/// </summary>
		/// <returns>the report</returns>
		public MetricReport CreateMeasures()
		{
			Measures = new List<MeasureReport>();
			return this;
		}

		/// <summary>
		/// Create a new MetricReport and add it to the Measures list
		/// </summary>
		/// <param name="measureName">Measure name</param>
		/// <param name="value">Value</param>
		/// <returns>The new metric report</returns>
		public MetricReport AddMeasure(string measureName, int value)
		{
			MeasureReport m = new MeasureReport(measureName, value);
			Measures.Add(m);
			return this;
		}	
	}
}
