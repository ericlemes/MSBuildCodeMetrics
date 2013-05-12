using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.UnitTests;
using MSBuildCodeMetrics.Core.Providers.UnitTests.Resources;

namespace MSBuildCodeMetrics.Core.Providers.UnitTests
{
	[TestClass]
	public class CountLOCProviderTests
	{
		[TestMethod]
		public void TestCountLOCProvider()
		{
			List<string> files = new List<string>().AddItem("file1.cs");
			FileStreamFactoryMock streamFactory = new FileStreamFactoryMock();
			streamFactory.AddFileMock("file1.cs", TestResources.SourceFile1);

			CountLOCProvider provider = new CountLOCProvider(streamFactory);
			provider.AddMetadata(".cs", "C#");
			List<ProviderMeasure> measures = provider.ComputeMetrics(new List<string>().AddItem("FileLOC").AddItem("EmptyLOC").AddItem("CommentLOC"), files).ToList<ProviderMeasure>();
			Assert.AreEqual("TotalLOC", measures[0].MetricName);
			Assert.AreEqual("C#", measures[0].MeasureName);
			Assert.AreEqual(33, measures[0].Value);
			Assert.AreEqual("EmptyLOC", measures[2].MetricName);
			Assert.AreEqual("C#", measures[2].MeasureName);
			Assert.AreEqual(3, measures[2].Value);
			Assert.AreEqual("CommentLOC", measures[3].MetricName);
			Assert.AreEqual("C#", measures[3].MeasureName);
			Assert.AreEqual(6, measures[3].Value);
			Assert.AreEqual("CodeLOC", measures[1].MetricName);
			Assert.AreEqual("C#", measures[1].MeasureName);
			Assert.AreEqual(24, measures[1].Value);
		}
	}
}
