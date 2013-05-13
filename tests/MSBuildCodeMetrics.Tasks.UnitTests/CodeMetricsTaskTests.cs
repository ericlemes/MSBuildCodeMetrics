using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Build.Framework;
using MSBuildCodeMetrics.Core.UnitTests.Mock;
using MSBuildCodeMetrics.Core.UnitTests;
using System.IO;
using MSBuildCodeMetrics.Core.XML;

namespace MSBuildCodeMetrics.Tasks.UnitTests
{
	[TestClass]
	public class CodeMetricsTaskTests
	{
		private CodeMetrics _task;
		private TaskItemMock[] _metrics;
		private TaskItemMock[] _providers;
		private BuildEngineMock _buildEngine;

		[TestInitialize]
		public void Initialize()
		{
			_buildEngine = new BuildEngineMock();
			_task = new CodeMetrics();
			_task.BuildEngine = _buildEngine;

			_metrics = new TaskItemMock[1];
			_metrics[0] = new TaskItemMock("LinesOfCode").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "foo");

			_providers = new TaskItemMock[1];
			_providers[0] = new TaskItemMock(
				"MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Metrics", "LinesOfCode");
		}

		[TestMethod]
		public void ExecuteTest()
		{
			ITaskItem[] providers = new TaskItemMock[1];
			providers[0] = new TaskItemMock("MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests").				
				AddMetadata("Data", "<Metric Name=\"LinesOfCode\"><Measure " + 
					"Name=\"Method1\" Value=\"1\" /><Measure Name=\"Method2\" Value=\"2\" /><Measure " +
					"Name=\"Method3\" Value=\"5\" /></Metric>").
				AddMetadata("ProviderName", "CodeMetricsProviderMock");

			ITaskItem[] metrics = new TaskItemMock[1];
			metrics[0] = new TaskItemMock("LinesOfCode").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "C:\foo.txt");			

			FileStreamFactoryMock streamFactory = new FileStreamFactoryMock();			

			CodeMetrics task = new CodeMetrics(streamFactory);
			task.BuildEngine = new BuildEngineMock();
			task.Providers = providers;			
			task.OutputFileName = "report.xml";
			task.ShowDetailsReport = true;
			task.ShowSummaryReport = true;
			task.Metrics = metrics;
			task.FileOutput = true;
			Assert.AreEqual(true, task.Execute());

			streamFactory.OutputStream.Seek(0, SeekOrigin.Begin);
			MSBuildCodeMetricsReport report = MSBuildCodeMetricsReport.Deserialize(streamFactory.OutputStream);
			Assert.AreEqual(1, report.Details.Metrics.Count);
			Assert.AreEqual("LinesOfCode", report.Details.Metrics[0].MetricName);
			Assert.AreEqual("Method3", report.Details.Metrics[0].Measures[0].MeasureName);
			Assert.AreEqual(5, report.Details.Metrics[0].Measures[0].Value);
		}

		[TestMethod]		
		public void TestEmptyMetrics()
		{
			_task.Metrics = new TaskItemMock[0];
			_task.Providers = _providers;			
			
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("At least one Metrics must be informed in Metrics property", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void TestEmptyProviders()
		{
			_task.Metrics = _metrics;			
			_task.Providers = new TaskItemMock[0];
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("At least one Provider must me informed in Providers property", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void TestInvalidTypeProviders()
		{
			_task.Metrics = _metrics;			
			_task.Providers = new TaskItemMock[1];
			_task.Providers[0] = new TaskItemMock("foo");
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Invalid provider: foo. Couldn't create instance of this type", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void TestInvalidTypeProviders2()
		{
			_task.Metrics = _metrics;			
			_task.Providers = new TaskItemMock[1];
			_task.Providers[0] = new TaskItemMock("MSBuildCodeMetrics.Tasks.CodeMetrics, MSBuildCodeMetrics.Tasks");
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Type MSBuildCodeMetrics.Tasks.CodeMetrics, MSBuildCodeMetrics.Tasks doesn't implements ICodeMetricsProvider", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void TestProviderWithNoName()
		{
			_task.Metrics = _metrics;			
			_task.Providers = new TaskItemMock[1];
			_task.Providers[0] = new TaskItemMock("MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests");
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Provider MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests doesn't implement property Name correctly", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void TestEmptyInputFiles()
		{
			_task.Metrics = _metrics;			
			_task.Providers = _providers;
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock("LinesOfCode").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Ranges", "2;3");
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Files must be informed in Metrics property. ProviderName: CodeMetricsProviderMock, Metric: LinesOfCode", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void TestMetricWithNullProviderName()
		{
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock("InvalidMetric").
				AddMetadata("ProviderName", null).
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "foo");
			_task.Providers = _providers;
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("ProviderName must be informed in Metrics property", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void TestMetricWithInvalidProviderName()
		{
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock("InvalidMetric").
				AddMetadata("ProviderName", "InvalidProvider").
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "foo");
			_task.Providers = _providers;
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Invalid provider name in Metrics property: ProviderName: InvalidProvider", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void TestMetricWithNullMetric()
		{
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock(null).
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "foo");
			_task.Providers = _providers;
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Metric name in property Metrics can't be null (ProviderName: CodeMetricsProviderMock", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void TestMetricWithInvalidMetric()
		{
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock("InvalidMetric").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "foo");
			_task.Providers = _providers;
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Provider CodeMetricsProviderMock doesn't know how to handle metric InvalidMetric", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void TestMetricWithNoRanges()
		{
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock("LinesOfCode").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Files", "foo");
			_task.Providers = _providers;
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Ranges can't be null if you need a summary report. ProviderName: CodeMetricsProviderMock, MetricName: LinesOfCode", _buildEngine.ErrorMessage);
		}

	}
}
