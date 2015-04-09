using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace MSBuildCodeMetrics.Tasks.UnitTests
{
    [TestClass]
    public class GivenATaskMetricList
    {
        [TestMethod]
        public void WhenCreatingShouldNotThrow()
        {
            var l = TaskMetricList.Create();
            Assert.IsNotNull(l);
        }
    }
}
