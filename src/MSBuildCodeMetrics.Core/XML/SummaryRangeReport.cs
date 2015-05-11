using System.Xml.Serialization;

namespace MSBuildCodeMetrics.Core.XML
{
	/// <summary>
	/// This class represents a Range in XML report. This is used only for XML serialization purpose.
	/// </summary>
	[XmlType("Range")]
	public class SummaryRangeReport
	{
		/// <summary>
		/// Name of the range
		/// </summary>
		[XmlAttribute]	
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Count of measures in this range
		/// </summary>
		[XmlAttribute]
		public int Count
		{
			get;
			set;
		}

		/// <summary>
		/// Creates a new SummaryRangeReport
		/// </summary>
		public SummaryRangeReport()
		{
		}

		/// <summary>
		/// Creates a new SummaryRangeReport
		/// </summary>
		/// <param name="rangeName">Range name</param>
		/// <param name="count">Count</param>
		public SummaryRangeReport(string rangeName, int count)
		{
			Name = rangeName;
			Count = count;
		}
	}
}
