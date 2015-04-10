using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.UnitTests.Extensions;
using MSBuildCodeMetrics.JetBrains.XML;

namespace MSBuildCodeMetrics.JetBrains.UnitTests
{
    [TestClass]
    public class GivenAnInspectCodeReport
    {
        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            var p = new InspectCodeProvider();
            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void WhenDeserializeReportShouldGetExpectedResults()
        {
            var stream = Resources.MSBuildCodeMetrics_Metrics.ToMemoryStream();

            var report = InspectCodeReport.Deserialize(stream);
            Assert.AreEqual(45, report.IssueTypes.Count);
            Assert.AreEqual("AccessToForEachVariableInClosure", report.IssueTypes[0].Id);
            Assert.AreEqual("Potential Code Quality Issues", report.IssueTypes[0].Category);
            Assert.AreEqual("Access to foreach variable in closure", report.IssueTypes[0].Description);
            Assert.AreEqual("WARNING", report.IssueTypes[0].Severity);
            Assert.AreEqual(11, report.Issues.Count);
            Assert.AreEqual("MSBuildCodeMetrics.Core", report.Issues[0].Name);
            Assert.AreEqual(24, report.Issues[1].Issues.Count);
            Assert.AreEqual("ConvertPropertyToExpressionBody", report.Issues[1].Issues[0].TypeId);
            Assert.AreEqual(@"MSBuildCodeMetrics.Core.Providers\CountFilesByExtensionProvider.cs", report.Issues[1].Issues[0].File);
            Assert.AreEqual("444-450", report.Issues[1].Issues[0].Offset);
            Assert.AreEqual(17, report.Issues[1].Issues[0].Line);
            Assert.AreEqual("Use expression body", report.Issues[1].Issues[0].Message);
        }
    }
}
