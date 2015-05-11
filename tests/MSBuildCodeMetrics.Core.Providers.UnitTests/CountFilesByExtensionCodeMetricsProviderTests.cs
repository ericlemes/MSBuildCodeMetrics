using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSBuildCodeMetrics.Core.Providers.UnitTests
{
	[TestClass]
	public class CountFilesByExtensionCodeMetricsProviderTests
	{
	    [TestMethod]
	    public void WhenConstructingShouldNotThrowAndInitializeProperly()
	    {
	        var p = new CountFilesByExtensionProvider();
            Assert.AreEqual("CountFilesByExtension", p.Name);
            Assert.AreEqual(1, p.GetMetrics().Count());
            Assert.AreEqual("CountFilesByExtension", p.GetMetrics().First());
	    }

		[TestMethod]
		public void TestCountFilesByExtension()
		{
			List<string> files = new List<string>().
				AddItem(@"C:\Teste.txt").
				AddItem(@"C:\outracoisa.txt").
				AddItem(@"C:\teste.csproj").
				AddItem(@"outrocsproj.csproj");
			CountFilesByExtensionProvider provider = new CountFilesByExtensionProvider();
			var measures = provider.ComputeMetrics(new List<string>().AddItem("CountFilesByExtension"), files);
			Assert.AreEqual(2, measures.FirstOrDefault(m => m.MeasureName == ".txt").Value);
			Assert.AreEqual(2, measures.FirstOrDefault(m => m.MeasureName == ".csproj").Value);
		}
	}
}
