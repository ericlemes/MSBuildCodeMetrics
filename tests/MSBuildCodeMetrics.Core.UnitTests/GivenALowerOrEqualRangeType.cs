using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.Ranges;

namespace MSBuildCodeMetrics.Core.UnitTests
{
    [TestClass]
    public class GivenALowerOrEqualRangeType
    {
        [TestMethod]
        public void WhenConstructingShouldInitializeProperly()
        {
            var r = new LowerOrEqualRangeType(50);
            Assert.AreEqual(50, r.Value);
        }
    }
}
