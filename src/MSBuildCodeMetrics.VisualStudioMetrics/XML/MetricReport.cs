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
	[XmlType("Metric")]
	public class MetricReport
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
		/// Value
		/// </summary>
		[XmlAttribute]
		public string Value
		{
			get;
			set;
		}
	}
}
