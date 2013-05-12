using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.UnitTests.Mock;
using MSBuildCodeMetrics.Core.XML;

namespace MSBuildCodeMetrics.Core.UnitTests
{
	[TestClass]
	public class RunnerTests
	{
		CodeMetricsRunner _runner = new CodeMetricsRunner(new LoggerMock());
		List<string> _fileList = new List<string>();

		[TestInitialize]
		public void Initialize()
		{			
			_runner.RegisterProvider(CodeMetricsProviderSingleFileMock.Create("Provider1").AddMetric("Metric1").AddMetric("Metric2").
				AddMeasure("Metric1", "Item1", 1).
				AddMeasure("Metric1", "Item2", 2).
				AddMeasure("Metric2", "Item1", 3));
			_runner.RegisterProvider(CodeMetricsProviderSingleFileMock.Create("Provider2").AddMetric("Metric3").AddMetric("Metric4").
				AddMeasure("Metric3", "Item1", 1));
			_runner.RegisterProvider(CodeMetricsProviderMultiFileMock.Create("Provider3").AddMetric("Metric5").
				AddMeasure("Metric5", "Item1", 10));

			_fileList.Add("foo");
		}

		[TestMethod]
		public void TestRegisterProvider()
		{			
			Assert.IsNotNull(_runner.GetProvider("Provider1"));
			Assert.IsTrue(_runner.IsMetricRegistered("Provider1", "Metric1"));
			Assert.IsTrue(_runner.IsMetricRegistered("Provider1", "Metric2"));
		}

		[TestMethod]
		public void TestComputeMetrics()
		{
			_runner.ComputeMetrics(_fileList);
			Assert.AreEqual(3, _runner.GetMeasuresByProvider("Provider1").Count());
			Assert.AreEqual(2, _runner.GetMeasuresByMetric("Provider1", "Metric1").Count());
			Assert.AreEqual("Item1", _runner.GetMeasuresByMetric("Provider2", "Metric3")[0].MeasureName);
			Assert.AreEqual(1, _runner.GetMeasuresByMetric("Provider2", "Metric3")[0].Value);
			Assert.AreEqual(10, _runner.GetMeasuresByMetric("Provider3", "Metric5")[0].Value);
		}
	}
}
