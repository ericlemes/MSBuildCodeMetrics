using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.UnitTests.Extensions;
using MSBuildCodeMetrics.JetBrains.XML;

namespace MSBuildCodeMetrics.JetBrains.UnitTests
{
    [TestClass]
    public class GivenADotCoverReport
    {
        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            var r = new DotCoverReport();
            Assert.IsNotNull(r);
        }

        [TestMethod]
        public void WhenDeserializeReportShouldReturnExpectedResult()
        {
            var stream = Resources.MSBuildCodeMetrics_core_coverage.ToMemoryStream();

            var report = DotCoverReport.Deserialize(stream);
            Assert.AreEqual(2, report.Assemblies.Count);
            Assert.AreEqual("MSBuildCodeMetrics.Core", report.Assemblies[0].Name);
            Assert.AreEqual(412, report.Assemblies[0].CoveredStatements);
            Assert.AreEqual(466, report.Assemblies[0].TotalStatements);
            Assert.AreEqual(88, report.Assemblies[0].CoveragePercent);
            Assert.AreEqual(3, report.Assemblies[0].Namespaces.Count);
            Assert.AreEqual("MSBuildCodeMetrics.Core", report.Assemblies[0].Namespaces[0].Name);
            Assert.AreEqual(273, report.Assemblies[0].Namespaces[0].CoveredStatements);
            Assert.AreEqual(323, report.Assemblies[0].Namespaces[0].TotalStatements);
            Assert.AreEqual(85, report.Assemblies[0].Namespaces[0].CoveragePercent);
            Assert.AreEqual(13, report.Assemblies[0].Namespaces[0].Types.Count);
            Assert.AreEqual("CodeMetricsRunner", report.Assemblies[0].Namespaces[0].Types[0].Name);
            Assert.AreEqual(173, report.Assemblies[0].Namespaces[0].Types[0].CoveredStatements);
            Assert.AreEqual(173, report.Assemblies[0].Namespaces[0].Types[0].TotalStatements);
            Assert.AreEqual(100, report.Assemblies[0].Namespaces[0].Types[0].CoveragePercent);
        }
    }
}
