using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.VisualStudioCodeMetrics.XML
{
	[XmlType("Type")]
	public class TypeReport
	{
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}
		
		[XmlArray("Metrics")]
		public List<MetricReport> MetricsList
		{
			get;
			set;
		}
		
		private MetricsReport _metrics;
		[XmlIgnore]
		public MetricsReport Metrics
		{
			get
			{
				if (_metrics == null)
					_metrics = new MetricsReport(MetricsList);
				return _metrics;
			}
		}

		public List<MemberReport> Members
		{
			get;
			set;
		}

		public NamespaceReport Namespace
		{
			get;
			set;
		}

	}
}
