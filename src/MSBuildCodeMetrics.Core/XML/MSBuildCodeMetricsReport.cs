using System.Xml.Serialization;
using System.IO;

namespace MSBuildCodeMetrics.Core.XML
{	
	/// <summary>
	/// This class represents the XML output report. This is used only for XML serialization purpose.
	/// </summary>
	public class MSBuildCodeMetricsReport
	{	
		/// <summary>
		/// The summary section of the report
		/// </summary>
		public SummaryReport Summary
		{
			get;
			set;
		}

		/// <summary>
		/// The details section of the report
		/// </summary>
		public DetailsReport Details
		{
			get;
			set;
		}

		/// <summary>
		/// Creates a new report.
		/// </summary>
		/// <returns>The new report</returns>
		public static MSBuildCodeMetricsReport Create()
		{
			return new MSBuildCodeMetricsReport();
		}

		/// <summary>
		/// Creates a new SummaryReport and hooks the Summary property.
		/// </summary>
		/// <returns>the new SummaryReport</returns>
		public SummaryReport CreateSummary()
		{
			Summary = new SummaryReport(this);			
			return Summary;
		}

		/// <summary>
		/// Creates a new DetailsReport and hooks the Details property.
		/// </summary>
		/// <returns>The new details section</returns>
		public DetailsReport CreateDetails()
		{
			Details = new DetailsReport(this);
			return Details;
		}

		/// <summary>
		/// Serialize this report to a new memory stream.
		/// </summary>
		/// <param name="seekToBegin">True if you want the stream in its start position</param>
		/// <returns>The new memory stream</returns>
		public MemoryStream SerializeToMemoryStream(bool seekToBegin)
		{
			MemoryStream serializedStream = new MemoryStream();
			XmlSerializer ser = new XmlSerializer(typeof(MSBuildCodeMetricsReport));
			ser.Serialize(serializedStream, this);

			if (seekToBegin)
				serializedStream.Seek(0, SeekOrigin.Begin);

			return serializedStream;
		}

		/// <summary>
		/// Deserializes the report
		/// </summary>
		/// <param name="stream">Stream</param>
		/// <returns>a new MSBuildCodeMetricsReport</returns>
		public static MSBuildCodeMetricsReport Deserialize(Stream stream)
		{
			XmlSerializer ser = new XmlSerializer(typeof(MSBuildCodeMetricsReport));
			return (MSBuildCodeMetricsReport)ser.Deserialize(stream);
		}

		/// <summary>
		/// Serializes the report.
		/// </summary>
		/// <param name="outputStream">The output stream</param>
		public void Serialize(Stream outputStream)
		{
			XmlSerializer ser = new XmlSerializer(typeof(MSBuildCodeMetricsReport));
			ser.Serialize(outputStream, this);
		}
	}
}
