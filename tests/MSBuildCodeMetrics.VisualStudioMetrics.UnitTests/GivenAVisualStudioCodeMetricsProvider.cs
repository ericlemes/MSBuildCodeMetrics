using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MSBuildCodeMetrics.Core;
using System.Reflection;

namespace MSBuildCodeMetrics.VisualStudioMetrics.UnitTests
{
    [TestClass]
    public class GivenAVisualStudioCodeMetricsProvider
    {        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCallingComputeMetricsWithoutTempDirShouldThrow()
        {
            VisualStudioCodeMetricsProvider p = new VisualStudioCodeMetricsProvider("C:\\InvalidExecutable.exe", null);
            p.ComputeMetrics(new List<string> { "CyclomaticComplexity" }, "C:\\outputfile.xml");
        }

        [TestMethod]
        [DeploymentItem(@"MSBuildCodeMetrics.Core.dll")]
        public void WhenCallingComputeMetricsAndTempDirDoesnotExistsShouldCreateTempDir()
        {
            var loggerMock = new Mock<ILogger>();            

            string tempDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName + "\\NewDir";

            if (Directory.Exists(tempDir))
                throw new Exception("This directory should not exists");

            var p = new VisualStudioCodeMetricsProvider(null, tempDir);            
            p.Logger = loggerMock.Object;
            p.ProcessExecutor = new ProcessExecutor(loggerMock.Object);
            p.ComputeMetrics(new List<string> { "CyclomaticComplexity" }, @"MSBuildCodeMetrics.Core.dll");

            Assert.IsTrue(Directory.Exists(tempDir));
        }

        [TestMethod]
        [ExpectedException(typeof(Win32Exception))]
        public void WhenFailsRunningExternalProcessShouldThrow()
        {
            var loggerMock = new Mock<ILogger>();

            var p = new VisualStudioCodeMetricsProvider("C:\\InvalidExecutable.exe", "C:\\Temp");
            p.Logger = loggerMock.Object;
            p.ProcessExecutor = new ProcessExecutor(loggerMock.Object);
            p.ComputeMetrics(new List<string> {"CyclomaticComplexity"}, "C:\\outputfile.xml");
        }

        [TestMethod]
        [DeploymentItem(@"MSBuildCodeMetrics.Core.dll")]
        public void WhenCallingComputeMetricsWithoutMetricsExeShouldTryDefaultLocation()
        {
            var loggerMock = new Mock<ILogger>();
            var currentDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

            var p = new VisualStudioCodeMetricsProvider(null, currentDir);
            p.Logger = loggerMock.Object;
            p.ProcessExecutor = new ProcessExecutor(loggerMock.Object);
            p.ComputeMetrics(new List<string> { "CyclomaticComplexity" }, @"MSBuildCodeMetrics.Core.dll");
            
            loggerMock.Verify(l => l.LogMessage(It.Is<string>(s => s.StartsWith("Trying default: "))), Times.Once());
        }

        [TestMethod]        
        [ExpectedException(typeof(Exception))]
        public void WhenExecutingAndReturnErrorShouldWriteErrorMessage()
        {
            var loggerMock = new Mock<ILogger>();
            var errorMessage = String.Empty;
            loggerMock.Setup(l => l.LogError(It.IsAny<string>())).Callback<string>(
                s => { errorMessage = s; });
            var currentDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

            var p = new VisualStudioCodeMetricsProvider(null, currentDir);
            p.Logger = loggerMock.Object;
            p.ProcessExecutor = new ProcessExecutor(loggerMock.Object);
            p.ComputeMetrics(new List<string> { "CyclomaticComplexity" }, "wrongfile");

            Assert.IsNotNull(errorMessage);
        }
    }
}
