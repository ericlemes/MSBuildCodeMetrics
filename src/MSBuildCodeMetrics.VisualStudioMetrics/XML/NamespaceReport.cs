using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.VisualStudioCodeMetrics.XML
{
	[XmlType("Namespace")]
	public class NamespaceReport
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

		public List<TypeReport> Types
		{
			get;
			set;
		}

		public ModuleReport Module
		{
			get;
			set;
		}
	}	
}
