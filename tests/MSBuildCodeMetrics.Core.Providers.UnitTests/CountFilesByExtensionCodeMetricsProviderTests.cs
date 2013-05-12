using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSBuildCodeMetrics.Core.Providers.UnitTests
{
	[TestClass]
	public class CountFilesByExtensionCodeMetricsProviderTests
	{
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
			Assert.AreEqual(2, measures.Where(m => m.MeasureName == ".txt").FirstOrDefault().Value);
			Assert.AreEqual(2, measures.Where(m => m.MeasureName == ".csproj").FirstOrDefault().Value);
		}
	}
}
