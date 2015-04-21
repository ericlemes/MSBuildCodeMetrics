using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MSBuildCodeMetrics.Core;
using MSBuildCodeMetrics.Core.UnitTests.Extensions;

namespace MSBuildCodeMetrics.JetBrains.UnitTests
{
    [TestClass]
    public class GivenADotCoverProvider
    {
        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new DotCoverProvider(fileStreamFactoryMock.Object);
            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void WhenGettingNameShouldReturnDotCover()
        {
            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new DotCoverProvider(fileStreamFactoryMock.Object);
            Assert.AreEqual("DotCover", p.Name);
        }

        [TestMethod]
        public void WhenGettingMetricsShouldReturnExpectedSet()
        {            
            var p = new DotCoverProvider();
            var metrics = p.GetMetrics().GetEnumerator();
            metrics.MoveNext();
            Assert.AreEqual("CodeCoverage", metrics.Current);
            metrics.MoveNext();
            Assert.AreEqual("CoveredStatements", metrics.Current);
            metrics.MoveNext();
            Assert.AreEqual("UncoveredStatements", metrics.Current);
            metrics.MoveNext();
            Assert.AreEqual("TotalStatements", metrics.Current);
            Assert.IsFalse(metrics.MoveNext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCallingComputeMetricsWithoutAddingDotCoverPathMetadataShouldThrow()
        {
            var p = new DotCoverProvider();                        
            p.AddMetadata("DotCoverTargetExecutableParam", "C:\\Path To\\MSTest.exe");
            p.AddMetadata("DotCoverTargetArgumentParam", "/testcontainer:{0}");
            p.AddMetadata("TempDir", "C:\\Some Dir With Spaces");
            var metricsToCompute = new List<string>()
            {
                "CodeCoverage"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { "C:\\PathTo\\TestProject1.dll", "C:\\Path To\\TestProject2.dll" });            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCallingComputeMetricsWithoutAddingDotCoverTargetExecutableParamMetadataShouldThrow()
        {
            var p = new DotCoverProvider();
            p.AddMetadata("DotCoverPath", "C:\\Path To\\MSTest.exe");
            p.AddMetadata("DotCoverTargetArgumentParam", "/testcontainer:{0}");
            p.AddMetadata("TempDir", "C:\\Some Dir With Spaces");
            var metricsToCompute = new List<string>()
            {
                "CodeCoverage"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { "C:\\PathTo\\TestProject1.dll", "C:\\Path To\\TestProject2.dll" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCallingComputeMetricsWithoutAddingDotCoverTargetArgumentParamMetadataShouldThrow()
        {
            var p = new DotCoverProvider();
            p.AddMetadata("DotCoverPath", "C:\\Path To\\MSTest.exe");
            p.AddMetadata("DotCoverTargetExecutableParam", "/testcontainer:{0}");
            p.AddMetadata("TempDir", "C:\\Some Dir With Spaces");
            var metricsToCompute = new List<string>()
            {
                "CodeCoverage"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { "C:\\PathTo\\TestProject1.dll", "C:\\Path To\\TestProject2.dll" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCallingComputeMetricsWithoutAddingTempDirMetadataShouldThrow()
        {
            var p = new DotCoverProvider();
            p.AddMetadata("DotCoverPath", "C:\\Path To\\MSTest.exe");
            p.AddMetadata("DotCoverTargetExecutableParam", "/testcontainer:{0}");
            p.AddMetadata("DotCoverTargetArgumentParam", "C:\\Some Dir With Spaces");
            var metricsToCompute = new List<string>()
            {
                "CodeCoverage"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { "C:\\PathTo\\TestProject1.dll", "C:\\Path To\\TestProject2.dll" });
        }

        [TestMethod]
        public void WhenComputingMetricsShouldRunDotCoverCodeWithRightParameters()
        {
            var processExecutorMock = new Mock<IProcessExecutor>();
            var executable = new List<string>();
            var arguments = new List<string>();
            processExecutorMock.Setup(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(
                (exe, args) =>
                {
                    executable.Add(exe);
                    arguments.Add(args);
                });

            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();            
            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile(@"C:\Some Dir With Spaces\TestProject1.dll.coverage.xml"))
                .Returns(
                    Resources.PartialCoverage1.ToMemoryStream());
            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile(@"C:\Some Dir With Spaces\TestProject2.dll.coverage.xml"))
                .Returns(
                    Resources.PartialCoverage2.ToMemoryStream());
            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile(@"C:\Some Dir With Spaces\MSBuildCodeMetricsMergedCoverage.report.xml"))
                .Returns(
                    Resources.PartialCoverage2.ToMemoryStream());
            var p = new DotCoverProvider(fileStreamFactoryMock.Object);

            p.ProcessExecutor = processExecutorMock.Object;
            p.AddMetadata("DotCoverPath", "C:\\Some Path\\dotCover.exe");
            p.AddMetadata("DotCoverTargetExecutableParam", "C:\\Path To\\MSTest.exe");
            p.AddMetadata("DotCoverTargetArgumentParam", "/testcontainer:{0}");
            p.AddMetadata("TempDir", "C:\\Some Dir With Spaces");

            var metricsToCompute = new List<string>()
            {
                "CodeCoverage"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { "C:\\PathTo\\TestProject1.dll", "C:\\Path To\\TestProject2.dll" });

            processExecutorMock.Verify(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(4));
            Assert.AreEqual("C:\\Some Path\\dotCover.exe", executable[0]);
            Assert.AreEqual("cover /TargetExecutable=\"C:\\Path To\\MSTest.exe\" /TargetArguments=\"/testcontainer:C:\\PathTo\\TestProject1.dll\" /Output=\"C:\\Some Dir With Spaces\\TestProject1.dll.dcvr\"", arguments[0]);
            Assert.AreEqual("C:\\Some Path\\dotCover.exe", executable[1]);
            Assert.AreEqual("cover /TargetExecutable=\"C:\\Path To\\MSTest.exe\" /TargetArguments=\"/testcontainer:C:\\Path To\\TestProject2.dll\" /Output=\"C:\\Some Dir With Spaces\\TestProject2.dll.dcvr\"", arguments[1]);
            Assert.AreEqual("C:\\Some Path\\dotCover.exe", executable[2]);
            Assert.AreEqual("merge /Source=\"C:\\Some Dir With Spaces\\TestProject1.dll.dcvr;C:\\Some Dir With Spaces\\TestProject2.dll.dcvr\" /Output=\"C:\\Some Dir With Spaces\\MSBuildCodeMetricsMergedCoverage.dcvr\"", arguments[2]);
            Assert.AreEqual("C:\\Some Path\\dotCover.exe", executable[3]);
            Assert.AreEqual("report /Source=\"C:\\Some Dir With Spaces\\MSBuildCodeMetricsMergedCoverage.dcvr\" /Output=\"C:\\Some Dir With Spaces\\MSBuildCodeMetricsMergedCoverage.report.xml\" /ReportType=XML", arguments[3]);
        }

        [TestMethod]
        public void WhenComputingMetricsWithFiltersShouldRunDotCoverCodeWithRightParameters()
        {
            var processExecutorMock = new Mock<IProcessExecutor>();
            var executable = new List<string>();
            var arguments = new List<string>();
            processExecutorMock.Setup(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(
                (exe, args) =>
                {
                    executable.Add(exe);
                    arguments.Add(args);
                });

            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile(@"C:\Some Dir With Spaces\TestProject1.dll.coverage.xml"))
                .Returns(
                    Resources.PartialCoverage1.ToMemoryStream());
            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile(@"C:\Some Dir With Spaces\TestProject2.dll.coverage.xml"))
                .Returns(
                    Resources.PartialCoverage2.ToMemoryStream());
            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile(@"C:\Some Dir With Spaces\MSBuildCodeMetricsMergedCoverage.report.xml"))
                .Returns(
                    Resources.PartialCoverage2.ToMemoryStream());
            var p = new DotCoverProvider(fileStreamFactoryMock.Object);

            p.ProcessExecutor = processExecutorMock.Object;
            p.AddMetadata("DotCoverPath", "C:\\Some Path\\dotCover.exe");
            p.AddMetadata("DotCoverTargetExecutableParam", "C:\\Path To\\MSTest.exe");
            p.AddMetadata("DotCoverTargetArgumentParam", "/testcontainer:{0}");
            p.AddMetadata("TempDir", "C:\\Some Dir With Spaces");
            p.AddMetadata("Filters", ":myassembly=*.UnitTests;myassembly=*.IntegrationTests");

            var metricsToCompute = new List<string>()
            {
                "CodeCoverage"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { "C:\\PathTo\\TestProject1.dll" });

            processExecutorMock.Verify(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));
            Assert.AreEqual("C:\\Some Path\\dotCover.exe", executable[0]);
            Assert.AreEqual("cover /TargetExecutable=\"C:\\Path To\\MSTest.exe\" /TargetArguments=\"/testcontainer:C:\\PathTo\\TestProject1.dll\" /Output=\"C:\\Some Dir With Spaces\\TestProject1.dll.dcvr\" /Filters=:myassembly=*.UnitTests;myassembly=*.IntegrationTests", arguments[0]);
        }

        [TestMethod]
        public void WhenComputingMetricsShouldReturnExpectedResults()
        {
            var processExecutorMock = new Mock<IProcessExecutor>();
            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile(@"C:\Some Dir With Spaces\MSBuildCodeMetricsMergedCoverage.report.xml"))
                .Returns(
                    Resources.MSBuildCodeMetrics_core_coverage.ToMemoryStream());
            var p = new DotCoverProvider(fileStreamFactoryMock.Object);

            p.ProcessExecutor = processExecutorMock.Object;
            p.AddMetadata("DotCoverPath", "C:\\Some Path\\dotCover.exe");
            p.AddMetadata("DotCoverTargetExecutableParam", "C:\\Path To\\MSTest.exe");
            p.AddMetadata("DotCoverTargetArgumentParam", "/testcontainer:{0}");
            p.AddMetadata("TempDir", "C:\\Some Dir With Spaces");

            var metricsToCompute = new List<string>()
            {
                "CodeCoverage",
                "CoveredStatements",
                "UncoveredStatements", 
                "TotalStatements"
            };

            var measures = p.ComputeMetrics(metricsToCompute, new List<string> { "C:\\PathTo\\TestProject1.dll" });
            Assert.AreEqual(173, measures.Where(m => m.MetricName == "CoveredStatements" && m.MeasureName == "MSBuildCodeMetrics.Core.CodeMetricsRunner").First().Value);
            Assert.AreEqual(173, measures.Where(m => m.MetricName == "TotalStatements" && m.MeasureName == "MSBuildCodeMetrics.Core.CodeMetricsRunner").First().Value);
            Assert.AreEqual(0, measures.Where(m => m.MetricName == "UncoveredStatements" && m.MeasureName == "MSBuildCodeMetrics.Core.CodeMetricsRunner").First().Value);
            Assert.AreEqual(100, measures.Where(m => m.MetricName == "CodeCoverage" && m.MeasureName == "MSBuildCodeMetrics.Core.CodeMetricsRunner").First().Value);            
            Assert.AreEqual(0, measures.Where(m => m.MetricName == "TotalStatements" && m.MeasureName == "MSBuildCodeMetrics.Core.NamespaceDoc").First().Value);
            Assert.AreEqual(0, measures.Where(m => m.MetricName == "CoveredStatements" && m.MeasureName == "MSBuildCodeMetrics.Core.NamespaceDoc").First().Value);
            Assert.AreEqual(100, measures.Where(m => m.MetricName == "CodeCoverage" && m.MeasureName == "MSBuildCodeMetrics.Core.NamespaceDoc").First().Value);
        }
    }
}
