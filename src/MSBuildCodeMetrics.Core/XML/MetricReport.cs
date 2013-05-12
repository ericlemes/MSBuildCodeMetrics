using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MSBuildCodeMetrics.Core.Ranges;

namespace MSBuildCodeMetrics.Core.XML
{
	[XmlType("Metric")]
	public class MetricReport
	{
		[XmlAttribute]
		public string ProviderName
		{
			get;
			set;
		}

		[XmlAttribute]
		public string MetricName
		{
			get;
			set;
		}
		
		public List<SummaryRangeReport> Ranges
		{
			get;
			set;
		}

		public List<MeasureReport> Measures
		{
			get;
			set;
		}

		public MetricReport()
		{
		}

		public MetricReport(string providerName, string metricName)
		{
			this.ProviderName = providerName;
			this.MetricName = metricName;
		}

		public MetricReport CreateRanges()
		{
			this.Ranges = new List<SummaryRangeReport>();
			return this;
		}
		
		public MetricReport AddRange(string rangeName, int methodCount)
		{
			SummaryRangeReport sr = new SummaryRangeReport(rangeName, methodCount);
			this.Ranges.Add(sr);
			return this;
		}

		public MetricReport CreateMeasures()
		{
			this.Measures = new List<MeasureReport>();
			return this;
		}

		public MetricReport AddMeasure(string measureName, int value)
		{
			MeasureReport m = new MeasureReport(measureName, value);
			this.Measures.Add(m);
			return this;
		}	
	}
}
