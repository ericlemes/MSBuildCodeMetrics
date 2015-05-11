using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.XML;
using System.IO;

namespace MSBuildCodeMetrics.Core.UnitTests
{
	[TestClass]
	public class DeserializationTests
	{
		private Stream _expectedReportStream;
		private const string _cyclomaticComplexity = "CyclomaticComplexity";
		private const string _linesOfCode = "LinesOfCode";
		private MSBuildCodeMetricsReport _report;

		[TestInitialize]
		public void Initialize()
		{			
			_expectedReportStream = new MemoryStream();
			StreamWriter sw = new StreamWriter(_expectedReportStream);
			sw.Write(Resources.TestResources.ExpectedOutputReportXML);
			sw.Flush();
			_expectedReportStream.Seek(0, SeekOrigin.Begin);

			_report = MSBuildCodeMetricsReport.Deserialize(_expectedReportStream);
		}

		[TestMethod]
		public void TestSummaryDeserialization()
		{
			Assert.AreEqual(2, _report.Summary.Metrics.Count);
			Assert.AreEqual("VisualStudioMetrics", _report.Summary.Metrics[0].ProviderName);
			Assert.AreEqual(_cyclomaticComplexity, _report.Summary.Metrics[0].MetricName);
			Assert.AreEqual("> 10", _report.Summary.Metrics[0].Ranges[0].Name);
			Assert.AreEqual(5, _report.Summary.Metrics[0].Ranges[0].Count);
			Assert.AreEqual("<= 10 and > 5", _report.Summary.Metrics[0].Ranges[1].Name);
			Assert.AreEqual(3, _report.Summary.Metrics[0].Ranges[1].Count);
		}

		[TestMethod]
		public void TestDetailsDeserialization()
		{
			Assert.AreEqual(2, _report.Details.Metrics.Count);
			Assert.AreEqual("VisualStudioMetrics", _report.Details.Metrics[1].ProviderName);
			Assert.AreEqual(_linesOfCode, _report.Details.Metrics[1].MetricName);
			Assert.AreEqual(2, _report.Details.Metrics[1].Measures.Count);
			Assert.AreEqual("Method1() : void", _report.Details.Metrics[1].Measures[0].MeasureName);
			Assert.AreEqual(1000, _report.Details.Metrics[1].Measures[0].Value);
		}


	}
}
