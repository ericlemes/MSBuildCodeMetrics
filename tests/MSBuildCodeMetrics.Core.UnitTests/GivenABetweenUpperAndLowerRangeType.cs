using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.Ranges;

namespace MSBuildCodeMetrics.Core.UnitTests
{
    [TestClass]
    public class GivenABetweenUpperAndLowerRangeType
    {
        [TestMethod]
        public void WhenConstructingShouldInitializeProperly()
        {
            var r = new BetweenUpperAndLowerRangeType(100, 0);
            Assert.AreEqual(0, r.LowerValue);
            Assert.AreEqual(100, r.UpperValue);            
        }
    }
}
