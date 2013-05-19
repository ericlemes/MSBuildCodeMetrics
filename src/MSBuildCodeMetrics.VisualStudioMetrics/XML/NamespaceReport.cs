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
	[XmlType("Namespace")]
	public class NamespaceReport
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
		/// Types
		/// </summary>
		public List<TypeReport> Types
		{
			get;
			set;
		}

		/// <summary>
		/// Module
		/// </summary>
		public ModuleReport Module
		{
			get;
			set;
		}
	}	
}
