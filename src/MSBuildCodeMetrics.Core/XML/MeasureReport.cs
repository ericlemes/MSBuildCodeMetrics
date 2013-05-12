using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.Core.XML
{
	[XmlType("Measure")]
	public class MeasureReport
	{
		[XmlAttribute]
		public string MeasureName
		{
			get;
			set;
		}

		[XmlAttribute]
		public int Value
		{
			get;
			set;
		}

		public MeasureReport()
		{
		}

		public MeasureReport(string measureName, int value)
		{
			this.MeasureName = measureName;
			this.Value = value;
		}
	}
}
