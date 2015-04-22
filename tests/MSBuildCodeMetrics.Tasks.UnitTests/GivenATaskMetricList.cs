using System.Collections.Generic;
using System.Linq;
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

        [TestMethod]
        public void WhenCallingAddShouldCreateAndAddNewTaskMetricInstance()
        {
            var l = TaskMetricList.Create().Add("Provider", "Metric", new List<int> {1}, new List<string> {"file1"});
            Assert.AreEqual("Provider", l[0].ProviderName);
            Assert.AreEqual("Metric", l[0].MetricName);
            Assert.AreEqual(1, l[0].Ranges.First());
            Assert.AreEqual("file1", l[0].Files.First());
        }
    }
}
