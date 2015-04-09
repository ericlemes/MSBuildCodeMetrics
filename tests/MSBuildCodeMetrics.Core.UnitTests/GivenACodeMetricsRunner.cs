using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MSBuildCodeMetrics.Core.UnitTests
{
    [TestClass]
    public class GivenACodeMetricsRunner
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenGeneratingReportWithoutMetricListShouldThrow()
        {
            var loggerMock = new Mock<ILogger>();
            var cmr = new CodeMetricsRunner(loggerMock.Object);
            cmr.GenerateReport(null, true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenGeneratingReportWithEmptyRangeListShouldThrow()
        {
            var loggerMock = new Mock<ILogger>();
            var cmr = new CodeMetricsRunner(loggerMock.Object);
            var metricList = new List<Metric>()
            {
                new Metric("Provider1", "Metric1", new List<int>()) 
            };
            cmr.GenerateReport(metricList, true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenGeneratingReportWithNullRangeListShouldThrow()
        {
            var loggerMock = new Mock<ILogger>();
            var cmr = new CodeMetricsRunner(loggerMock.Object);
            var metricList = new List<Metric>()
            {
                new Metric("Provider1", "Metric1", null) 
            };
            cmr.GenerateReport(metricList, true, true);
        }

        [TestMethod]        
        public void WhenGeneratingReportWithTwoRangesShouldGenerateReportWithTwoRanges()
        {
            var loggerMock = new Mock<ILogger>();
            var cmr = new CodeMetricsRunner(loggerMock.Object);
            var metricList = new List<Metric>()
            {
                new Metric("Provider1", "Metric1", new List<int>() { 10 }) 
            };
            var report = cmr.GenerateReport(metricList, true, true);
            Assert.AreEqual(2, report.Summary.Metrics[0].Ranges.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenComputingMetricsWithoutFilesShouldThrow()
        {
            var loggerMock = new Mock<ILogger>();
            var cmr = new CodeMetricsRunner(loggerMock.Object);
            var cmp = new List<ComputeMetricsParameter>()
            {
                new ComputeMetricsParameter("Provider1", "Metric1", new List<string>())                
            };
            cmr.ComputeMetrics(cmp);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenComputingMetricsWithUnregeisteredProviderShouldThrow()
        {
            var loggerMock = new Mock<ILogger>();
            var cmr = new CodeMetricsRunner(loggerMock.Object);
            var cmp = new List<ComputeMetricsParameter>()
            {
                new ComputeMetricsParameter("Provider1", "Metric1", new List<string>() { "file1.txt" })                
            };
            cmr.ComputeMetrics(cmp);
        }
    }
}
