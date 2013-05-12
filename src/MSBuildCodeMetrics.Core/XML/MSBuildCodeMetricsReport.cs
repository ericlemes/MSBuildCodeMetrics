using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace MSBuildCodeMetrics.Core.XML
{	
	public class MSBuildCodeMetricsReport
	{	
		public SummaryReport Summary
		{
			get;
			set;
		}

		public DetailsReport Details
		{
			get;
			set;
		}

		public static MSBuildCodeMetricsReport Create()
		{
			return new MSBuildCodeMetricsReport();
		}

		public SummaryReport CreateSummary()
		{
			Summary = new SummaryReport(this);			
			return Summary;
		}

		public DetailsReport CreateDetails()
		{
			Details = new DetailsReport(this);
			return Details;
		}

		public MemoryStream SerializeToMemoryStream(bool seekToBegin)
		{
			MemoryStream serializedStream = new MemoryStream();
			XmlSerializer ser = new XmlSerializer(typeof(MSBuildCodeMetricsReport));
			ser.Serialize(serializedStream, this);

			if (seekToBegin)
				serializedStream.Seek(0, SeekOrigin.Begin);

			return serializedStream;
		}

		public static MSBuildCodeMetricsReport Deserialize(Stream stream)
		{
			XmlSerializer ser = new XmlSerializer(typeof(MSBuildCodeMetricsReport));
			return (MSBuildCodeMetricsReport)ser.Deserialize(stream);
		}

		public void DeserializeToStream(Stream outputReportStream)
		{
			XmlSerializer ser = new XmlSerializer(typeof(MSBuildCodeMetricsReport));
			ser.Serialize(outputReportStream, this);
		}
	}
}
