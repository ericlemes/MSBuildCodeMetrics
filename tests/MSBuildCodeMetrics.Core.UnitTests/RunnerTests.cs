using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.UnitTests.Mock;

namespace MSBuildCodeMetrics.Core.UnitTests
{
	[TestClass]
	public class RunnerTests
	{
		private readonly CodeMetricsRunner _runner = new CodeMetricsRunner(new LoggerMock());
		private readonly List<string> _fileList = new List<string>();
		private IList<ComputeMetricsParameter> _parameters;

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

			_parameters = ComputeMetricsParameterList.Create().
				Add("Provider1", "Metric1", _fileList).
				Add("Provider1", "Metric2", _fileList).
				Add("Provider2", "Metric3", _fileList).
				Add("Provider2", "Metric4", _fileList).
				Add("Provider3", "Metric5", _fileList);

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
			_runner.ComputeMetrics(_parameters);
			Assert.AreEqual(3, _runner.GetMeasuresByProvider("Provider1").Count());
			Assert.AreEqual(2, _runner.GetDetailedReport("Provider1", "Metric1").Count());
			Assert.AreEqual("Item1", _runner.GetDetailedReport("Provider2", "Metric3").ToList()[0].MeasureName);
			Assert.AreEqual(1, _runner.GetDetailedReport("Provider2", "Metric3").ToList()[0].Value);
			Assert.AreEqual(10, _runner.GetDetailedReport("Provider3", "Metric5").ToList()[0].Value);
		}
	}
}
