using System.Xml.Serialization;

namespace MSBuildCodeMetrics.Core.XML
{
	/// <summary>
	/// This class represents a measure in XML report. This is used only for XML serialization purpose.
	/// </summary>
	[XmlType("Measure")]
	public class MeasureReport
	{
		/// <summary>
		/// Measure name
		/// </summary>
		[XmlAttribute]
		public string MeasureName
		{
			get;
			set;
		}

		/// <summary>
		/// Value
		/// </summary>
		[XmlAttribute]
		public int Value
		{
			get;
			set;
		}

		/// <summary>
		/// Creates a new MeasureReport
		/// </summary>
		public MeasureReport()
		{
		}

		/// <summary>
		/// Creates a new MeasureReport
		/// </summary>
		/// <param name="measureName">Measure name</param>
		/// <param name="value">Value</param>
		public MeasureReport(string measureName, int value)
		{
			MeasureName = measureName;
			Value = value;
		}
	}
}
