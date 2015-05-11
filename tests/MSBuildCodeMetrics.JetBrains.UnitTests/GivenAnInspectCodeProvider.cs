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
    public class GivenAnInspectCodeProvider
    {
        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void WhenGettingNameShouldReturnInspectCode()
        {
            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            Assert.AreEqual("InspectCode", p.Name);
        }

        [TestMethod]
        public void WhenGettingMetricsShouldReturnExpectedSet()
        {
            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            var metrics = p.GetMetrics().GetEnumerator();
            metrics.MoveNext();            
            Assert.AreEqual("AllViolations", metrics.Current);
            metrics.MoveNext();
            Assert.AreEqual("Warnings", metrics.Current);
            metrics.MoveNext();
            Assert.AreEqual("Suggestions", metrics.Current);
            metrics.MoveNext();
            Assert.AreEqual("Errors", metrics.Current);
            metrics.MoveNext();
            Assert.AreEqual("AllViolationsAllFiles", metrics.Current);
            metrics.MoveNext();
            Assert.AreEqual("WarningsAllFiles", metrics.Current);
            metrics.MoveNext();
            Assert.AreEqual("SuggestionsAllFiles", metrics.Current);
            metrics.MoveNext();
            Assert.AreEqual("ErrorsAllFiles", metrics.Current);
            Assert.IsFalse(metrics.MoveNext());
        }

        [TestMethod]
        public void WhenComputingMetricsShouldRunInspectCodeWithRightParameters()
        {
            var processExecutorMock = new Mock<IProcessExecutor>();
            var executable = String.Empty;
            var arguments = String.Empty;
            processExecutorMock.Setup(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(
                (exe, args) =>
                {
                    executable = exe;
                    arguments = args;
                });

            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile("C:\\Some Dir With Spaces\\SomeSolution.sln.metrics.xml"))
                .Returns(Resources.MSBuildCodeMetrics_Metrics.ToMemoryStream());

            p.ProcessExecutor = processExecutorMock.Object;
            p.AddMetadata("InspectCodePath", "InspectCode.exe");            
            p.AddMetadata("DotSettingsFile", "SomeSolution.sln.DotSettings");
            p.AddMetadata("TempDir", "C:\\Some Dir With Spaces");

            var metricsToCompute = new List<string>()
            {
                "AllViolations",
                "Warnings",
                "Suggestions",
                "Errors"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> {"SomeSolution.sln" } );

            processExecutorMock.Verify(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            Assert.AreEqual("InspectCode.exe", executable);
            Assert.AreEqual("\"SomeSolution.sln\" /o=\"C:\\Some Dir With Spaces\\SomeSolution.sln.metrics.xml\" /p=\"SomeSolution.sln.DotSettings\"", arguments);
        }

        [TestMethod]
        public void WhenComputingMetricsWithFullSolutionPathShouldRunInspectCodeWithRightParameters()
        {
            var processExecutorMock = new Mock<IProcessExecutor>();
            var executable = String.Empty;
            var arguments = String.Empty;
            processExecutorMock.Setup(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(
                (exe, args) =>
                {
                    executable = exe;
                    arguments = args;
                });

            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile("C:\\Some Dir With Spaces\\SomeSolution.sln.metrics.xml"))
                .Returns(Resources.MSBuildCodeMetrics_Metrics.ToMemoryStream());

            p.ProcessExecutor = processExecutorMock.Object;
            p.AddMetadata("InspectCodePath", "InspectCode.exe");
            p.AddMetadata("DotSettingsFile", "SomeSolution.sln.DotSettings");
            p.AddMetadata("TempDir", "C:\\Some Dir With Spaces");

            var metricsToCompute = new List<string>()
            {
                "AllViolations",
                "Warnings",
                "Suggestions",
                "Errors"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { @"C:\FullPath\To\SomeSolution.sln" });

            processExecutorMock.Verify(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            Assert.AreEqual("InspectCode.exe", executable);
            Assert.AreEqual("\"C:\\FullPath\\To\\SomeSolution.sln\" /o=\"C:\\Some Dir With Spaces\\SomeSolution.sln.metrics.xml\" /p=\"SomeSolution.sln.DotSettings\"", arguments);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenComputingMetricsAndNoInspectCodeExeMetadataSpecifiedShouldThrow()
        {
            var processExecutorMock = new Mock<IProcessExecutor>();

            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            p.ProcessExecutor = processExecutorMock.Object;
            p.AddMetadata("DotSettingsFile", "SomeSolution.sln.DotSettings");

            var metricsToCompute = new List<string>()
            {
                "AllViolations"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { "SomeSolution.sln" });
        }

        [TestMethod]
        public void WhenComputingMetricsWithoutDotSettingsFileShouldRunInspectCodeWithRightParameters()
        {
            var processExecutorMock = new Mock<IProcessExecutor>();
            var executable = String.Empty;
            var arguments = String.Empty;
            processExecutorMock.Setup(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(
                (exe, args) =>
                {
                    executable = exe;
                    arguments = args;
                });

            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();

            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile("C:\\Temp\\SomeSolution.sln.metrics.xml"))
                .Returns(Resources.MSBuildCodeMetrics_Metrics.ToMemoryStream());

            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            p.ProcessExecutor = processExecutorMock.Object;
            p.AddMetadata("InspectCodePath", "InspectCode.exe");            
            p.AddMetadata("TempDir", "C:\\Temp");

            var metricsToCompute = new List<string>()
            {
                "AllViolations"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { "SomeSolution.sln" });

            processExecutorMock.Verify(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            Assert.AreEqual("InspectCode.exe", executable);
            Assert.AreEqual("\"SomeSolution.sln\" /o=\"C:\\Temp\\SomeSolution.sln.metrics.xml\"", arguments);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenComputingMetricsAndNoTempDirMetadataSpecifiedShouldThrow()
        {
            var processExecutorMock = new Mock<IProcessExecutor>();

            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            p.ProcessExecutor = processExecutorMock.Object;
            p.AddMetadata("DotSettingsFile", "SomeSolution.sln.DotSettings");
            p.AddMetadata("InspectCodePath", "SomeSolution.sln.DotSettings");

            var metricsToCompute = new List<string>()
            {
                "AllViolations"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { "SomeSolution.sln" });
        }

        [TestMethod]
        public void WhenSettingLoggerShouldNotThrow()
        {
            var loggerMock = new Mock<ILogger>();

            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            p.Logger = loggerMock.Object;
        }

        [TestMethod]
        public void WhenEnumeratingMetricsShouldNotThrow()
        {
            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            p.GetMetrics().ToList();            
        }

        [TestMethod]
        public void WhenComputingMetricsShouldReadStreamFromOutputFile()
        {
            var processExecutorMock = new Mock<IProcessExecutor>();
            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();

            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile("C:\\Temp\\SomeSolution.sln.metrics.xml"))
                .Returns(Resources.MSBuildCodeMetrics_Metrics.ToMemoryStream());
            
            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            p.ProcessExecutor = processExecutorMock.Object;
            p.AddMetadata("InspectCodePath", "InspectCode.exe");            
            p.AddMetadata("DotSettingsFile", "SomeSolution.sln.DotSettings");
            p.AddMetadata("TempDir", "C:\\Temp");

            var metricsToCompute = new List<string>()
            {
                "AllViolations"
            };

            p.ComputeMetrics(metricsToCompute, new List<string> { "SomeSolution.sln" });

            processExecutorMock.Verify(pe => pe.ExecuteProcess(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            fileStreamFactoryMock.Verify(fsf => fsf.OpenFile("C:\\Temp\\SomeSolution.sln.metrics.xml"), Times.Once());
        }

        [TestMethod]
        public void WhenComputingMetricsShouldGenerateProviderMeasures()
        {
            var processExecutorMock = new Mock<IProcessExecutor>();
            var fileStreamFactoryMock = new Mock<IFileStreamFactory>();

            fileStreamFactoryMock.Setup(fsf => fsf.OpenFile("C:\\Temp\\SomeSolution.sln.metrics.xml"))
                .Returns(Resources.MSBuildCodeMetrics_Metrics.ToMemoryStream());

            var p = new InspectCodeProvider(fileStreamFactoryMock.Object);
            p.ProcessExecutor = processExecutorMock.Object;
            p.AddMetadata("InspectCodePath", "InspectCode.exe");
            p.AddMetadata("DotSettingsFile", "SomeSolution.sln.DotSettings");
            p.AddMetadata("TempDir", "C:\\Temp");

            var metricsToCompute = new List<string>()
            {
                "AllViolations",
                "Warnings",
                "Suggestions",
                "Errors",
                "AllViolationsAllFiles",
                "WarningsAllFiles",
                "SuggestionsAllFiles",
                "ErrorsAllFiles",
            };

            var measures = p.ComputeMetrics(metricsToCompute, new List<string> { "SomeSolution.sln" });
            Assert.AreEqual(11, measures.Count(m => m.MetricName == "AllViolations"));
            Assert.AreEqual(24, measures.Where(m => m.MetricName == "AllViolations" && m.MeasureName == "MSBuildCodeMetrics.Core.Providers").Sum(m => m.Value));
            Assert.AreEqual(12, measures.Where(m => m.MetricName == "Suggestions" && m.MeasureName == "MSBuildCodeMetrics.Core.Providers").Sum(m => m.Value));
            Assert.AreEqual(9, measures.Where(m => m.MetricName == "Warnings" && m.MeasureName == "MSBuildCodeMetrics.Core.Providers").Sum(m => m.Value));
            Assert.AreEqual(3, measures.Where(m => m.MetricName == "Errors" && m.MeasureName == "MSBuildCodeMetrics.Core.Providers").Sum(m => m.Value));
            Assert.AreEqual(11, measures.Count(m => m.MetricName == "AllViolations"));
            Assert.AreEqual(594, measures.First(m => m.MetricName == "AllViolationsAllFiles" && m.MeasureName == "AllFiles").Value);
            Assert.AreEqual(176, measures.First(m => m.MetricName == "SuggestionsAllFiles" && m.MeasureName == "AllFiles").Value);
            Assert.AreEqual(396, measures.First(m => m.MetricName == "WarningsAllFiles" && m.MeasureName == "AllFiles").Value);
            Assert.AreEqual(22, measures.First(m => m.MetricName == "ErrorsAllFiles" && m.MeasureName == "AllFiles").Value);            
        }        
    }
}
