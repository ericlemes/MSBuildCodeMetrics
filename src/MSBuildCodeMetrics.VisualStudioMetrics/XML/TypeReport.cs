using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.VisualStudioMetrics.XML
{
	/// <summary>
	/// Used to parse Visual Studio Metrics XML
	/// </summary>
	[XmlType("Type")]
	public class TypeReport
	{
		/// <summary>
		/// Name
		/// </summary>
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}
		
		/// <summary>
		/// MetricsList
		/// </summary>
		[XmlArray("Metrics")]
		public List<MetricReport> MetricsList
		{
			get;
			set;
		}
		
		private MetricsReport _metrics;
		/// <summary>
		/// Metrics
		/// </summary>
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

		/// <summary>
		/// Members
		/// </summary>
		public List<MemberReport> Members
		{
			get;
			set;
		}

		/// <summary>
		/// Namespace
		/// </summary>
		public NamespaceReport Namespace
		{
			get;
			set;
		}

	}
}
