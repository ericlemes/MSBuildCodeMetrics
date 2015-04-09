using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace MSBuildCodeMetrics.Tasks.UnitTests
{
    [TestClass]
    public class GivenAMSBuildCodeMetricsTaskException
    {
        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            var e = new MSBuildCodeMetricsTaskException("Message");
            Assert.IsNotNull(e);
            Assert.AreEqual("Message", e.Message);
        }
    }
}
