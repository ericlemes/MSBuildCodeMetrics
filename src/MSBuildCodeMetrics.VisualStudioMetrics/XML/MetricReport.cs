using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.VisualStudioCodeMetrics.XML
{
	[XmlType("Metric")]
	public class MetricReport
	{
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Value
		{
			get;
			set;
		}
	}
}
