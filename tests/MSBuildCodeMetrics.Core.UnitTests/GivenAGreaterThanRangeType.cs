using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.Ranges;

namespace MSBuildCodeMetrics.Core.UnitTests
{
    [TestClass]
    public class GivenAGreaterThanRangeType
    {
        [TestMethod]
        public void WhenConstructingShouldInitializeProperly()
        {
            var r = new GreaterThanRangeType(10);
            Assert.AreEqual(10, r.Value);
        }
    }
}
