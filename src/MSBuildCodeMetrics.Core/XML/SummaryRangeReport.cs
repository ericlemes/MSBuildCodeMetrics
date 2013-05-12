using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.Core.XML
{
	[XmlType("Range")]
	public class SummaryRangeReport
	{
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		[XmlAttribute]
		public int Count
		{
			get;
			set;
		}

		public SummaryRangeReport()
		{
		}

		public SummaryRangeReport(string rangeName, int methodCount)
		{
			this.Name = rangeName;
			this.Count = methodCount;
		}
	}
}
