using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Microsoft.XmlDiffPatch;
using MSBuildCodeMetrics.Core.XML;
using MSBuildCodeMetrics.Core.UnitTests.Extensions;

namespace MSBuildCodeMetrics.Core.UnitTests
{
	[TestClass]
	public class SerializationTests
	{
		private Stream _expectedReportStream;
		private const string _cyclomaticComplexity = "CyclomaticComplexity";
		private const string _linesOfCode = "LinesOfCode";

		[TestInitialize]
		public void Initialize()
		{
			_expectedReportStream = new MemoryStream();
			StreamWriter sw = new StreamWriter(_expectedReportStream);
			sw.Write(Resources.TestResources.ExpectedOutputReportXML);
			sw.Flush();
			_expectedReportStream.Seek(0, SeekOrigin.Begin);
		}

		[TestMethod]
		public void TestSerialization()
		{
			MSBuildCodeMetricsReport report = MSBuildCodeMetricsReport.Create().CreateSummary().Report.CreateDetails().Report;

			report.Summary.AddMetric("VisualStudioMetrics", _cyclomaticComplexity).CreateRanges().
				AddRange("> 10", 5).
				AddRange("<= 10 and > 5", 3).
				AddRange("<= 5", 1);

			report.Summary.AddMetric("VisualStudioMetrics", _linesOfCode).CreateRanges().
				AddRange("> 100", 5).
				AddRange("<= 100 and > 50", 3).
				AddRange("<= 50", 1);			

			report.Details.AddMetric("VisualStudioMetrics", _cyclomaticComplexity).CreateMeasures().
				AddMeasure("Method1() : void", 100).
				AddMeasure("Method2() : void", 50);

			report.Details.AddMetric("VisualStudioMetrics", _linesOfCode).CreateMeasures().
				AddMeasure("Method1() : void", 1000).
				AddMeasure("Method2() : void", 500);

			Stream ms = report.SerializeToMemoryStream(true);
			XmlDiff diff = new XmlDiff();
			Assert.IsTrue(diff.Compare(_expectedReportStream, ms));
		}
	}
}
