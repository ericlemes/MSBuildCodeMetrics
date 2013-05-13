using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MSBuildCodeMetrics.Core.UnitTests.Mock;
using MSBuildCodeMetrics.Core.XML;

namespace MSBuildCodeMetrics.Core.UnitTests
{
	[TestClass]
	public class ReportTests
	{		
		private CodeMetricsRunner _runner;
		private List<string> _inputFiles;
		private IList<ComputeMetricsParameter> _parameters;

		[TestInitialize]
		public void Initialize()
		{
			CodeMetricsProviderSingleFileMock runnerMock = CodeMetricsProviderSingleFileMock.Create("Provider1").AddMetric("CyclomaticComplexity").
				AddMetric("LinesOfCode").
				AddMeasure("CyclomaticComplexity", "Method1", 1).
				AddMeasure("CyclomaticComplexity", "Method2", 1).
				AddMeasure("CyclomaticComplexity", "Method3", 5).
				AddMeasure("CyclomaticComplexity", "Method4", 10).
				AddMeasure("CyclomaticComplexity", "Method5", 20).
				AddMeasure("LinesOfCode", "Method1", 1).
				AddMeasure("LinesOfCode", "Method2", 1).
				AddMeasure("LinesOfCode", "Method3", 20).
				AddMeasure("LinesOfCode", "Method4", 25).
				AddMeasure("LinesOfCode", "Method5", 40).
				AddMeasure("LinesOfCode", "Method6", 250);

			CodeMetricsProviderSingleFileMock runnerMock2 = CodeMetricsProviderSingleFileMock.Create("Provider2").AddMetric("CyclomaticComplexity").
				AddMetric("LinesOfCode").
				AddMeasure("CyclomaticComplexity", "Method1", 1).
				AddMeasure("CyclomaticComplexity", "Method2", 1).
				AddMeasure("CyclomaticComplexity", "Method3", 5).
				AddMeasure("CyclomaticComplexity", "Method4", 10).
				AddMeasure("CyclomaticComplexity", "Method5", 20).
				AddMeasure("LinesOfCode", "Method1", 1).
				AddMeasure("LinesOfCode", "Method2", 1).
				AddMeasure("LinesOfCode", "Method3", 20).
				AddMeasure("LinesOfCode", "Method4", 25).
				AddMeasure("LinesOfCode", "Method5", 40).
				AddMeasure("LinesOfCode", "Method6", 250);			

			_runner = new CodeMetricsRunner(new LoggerMock());
			_runner.RegisterProvider(runnerMock);
			_runner.RegisterProvider(runnerMock2);

			_inputFiles = new List<string>();
			_inputFiles.Add("foo");

			_parameters = ComputeMetricsParameterList.Create().
				Add("Provider1", "CyclomaticComplexity", _inputFiles).
				Add("Provider1", "LinesOfCode", _inputFiles).
				Add("Provider2", "CyclomaticComplexity", _inputFiles).
				Add("Provider2", "LinesOfCode", _inputFiles);

			_runner.ComputeMetrics(_parameters);
		}

		[TestMethod]
		public void TestSummaryReport()
		{			
			MSBuildCodeMetricsReport report = _runner.GenerateReport(MetricList.Create().
				Add("Provider1", "CyclomaticComplexity", RangeList.Create().Add(5).Add(10)).
				Add("Provider1", "LinesOfCode", RangeList.Create().Add(20).Add(50)), true, false);			

			Assert.AreEqual(2, report.Summary.Metrics.Count);
			Assert.AreEqual("Provider1", report.Summary.Metrics[0].ProviderName);
			Assert.AreEqual("CyclomaticComplexity", report.Summary.Metrics[0].MetricName);
			Assert.AreEqual(3, report.Summary.Metrics[0].Ranges.Count);
			Assert.AreEqual("> 10", report.Summary.Metrics[0].Ranges[0].Name);
			Assert.AreEqual(1, report.Summary.Metrics[0].Ranges[0].Count);
		}

		[TestMethod]
		public void DetailedReportTest()
		{
			MSBuildCodeMetricsReport report = _runner.GenerateReport(MetricList.Create().
				Add("Provider1", "CyclomaticComplexity", RangeList.Create().Add(5).Add(10)).
				Add("Provider1", "LinesOfCode", RangeList.Create().Add(20).Add(50)), false, true);			
			
			Assert.AreEqual(2, report.Details.Metrics.Count);
			Assert.AreEqual("Provider1", report.Details.Metrics[0].ProviderName);
			Assert.AreEqual("CyclomaticComplexity", report.Details.Metrics[0].MetricName);
			Assert.AreEqual(5, report.Details.Metrics[0].Measures.Count);
			Assert.AreEqual("Method1", report.Details.Metrics[0].Measures[0].MeasureName);
			Assert.AreEqual(1, report.Details.Metrics[0].Measures[0].Value);
		}

		[TestMethod]
		public void TestReportWithBothReports()
		{
			MSBuildCodeMetricsReport report = _runner.GenerateReport(MetricList.Create().
				Add("Provider1", "CyclomaticComplexity", RangeList.Create().Add(5).Add(10)).
				Add("Provider1", "LinesOfCode", RangeList.Create().Add(20).Add(50)), true, true);						

			Assert.AreEqual(3, report.Summary.Metrics[0].Ranges.Count);
			Assert.AreEqual(5, report.Details.Metrics[0].Measures.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestReportWithNullMetricList()
		{
			MSBuildCodeMetricsReport report = _runner.GenerateReport(MetricList.Create().
				Add("Provider1", "CyclomaticComplexity").
				Add("Provider1", "LinesOfCode"), true, false);						
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestReportWithEmptyMetricList()
		{			
			MSBuildCodeMetricsReport report = _runner.GenerateReport(MetricList.Create(), true, false);						
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestReportWithNoReport()
		{
			MSBuildCodeMetricsReport report = _runner.GenerateReport(MetricList.Create().
				Add("Provider1", "CyclomaticComplexity", RangeList.Create().Add(5).Add(10)).
				Add("Provider1", "LinesOfCode", RangeList.Create().Add(20).Add(50)), false, false);			
		}
	}
}
