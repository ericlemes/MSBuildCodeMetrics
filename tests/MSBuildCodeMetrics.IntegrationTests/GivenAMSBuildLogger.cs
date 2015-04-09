using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Moq;
using MSBuildCodeMetrics.Tasks;

namespace MSBuildCodeMetrics.IntegrationTests
{
    [TestClass]
    public class GivenAMSBuildLogger
    {
        [TestMethod]
        public void WhenCallingLogErrorShouldCallMSBuildLogError()
        {
            var buildEngineMock = new Mock<IBuildEngine>();

            var taskMock = new Mock<ITask>();
            taskMock.Setup(t => t.BuildEngine).Returns(buildEngineMock.Object);
            var helper = new TaskLoggingHelper(taskMock.Object);
            var logger = new MSBuildLogger(helper);

            logger.LogError("Error");
        }
    }
}
