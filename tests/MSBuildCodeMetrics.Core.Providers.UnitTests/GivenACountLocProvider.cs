using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.Providers.UnitTests.Resources;
using MSBuildCodeMetrics.Core.UnitTests.Mock;

namespace MSBuildCodeMetrics.Core.Providers.UnitTests
{
	[TestClass]
	public class GivenACountLOCProvider
	{
		private CountLOCProvider _provider;
		private List<string> _files;

		[TestInitialize]
		public void Initialize()
		{
			_files = new List<string>().AddItem("file1.cs");
			FileStreamFactoryMock streamFactory = new FileStreamFactoryMock();
			streamFactory.AddFileMock("file1.cs", TestResources.SourceFile1);

			_provider = new CountLOCProvider(streamFactory);
		}

	    [TestMethod]
	    public void WhenConstructingShouldInitializeProperly()
	    {
	        var p = new CountLOCProvider();
            Assert.AreEqual("LOC", p.Name);
            Assert.AreEqual(3, p.GetMetrics().Count());
	        var metrics = p.GetMetrics().GetEnumerator();
	        metrics.MoveNext();
            Assert.AreEqual("CodeLOC", metrics.Current);
	        metrics.MoveNext();
            Assert.AreEqual("EmptyLOC", metrics.Current);
	        metrics.MoveNext();
            Assert.AreEqual("CommentLOC", metrics.Current);            
	    }

		[TestMethod]
		public void WhenCallingComputeMetricsShouldReturnMeasures()
		{
			List<string> files = new List<string>().AddItem("file1.cs");
			FileStreamFactoryMock streamFactory = new FileStreamFactoryMock();
			streamFactory.AddFileMock("file1.cs", TestResources.SourceFile1);

            CountLOCProvider provider = new CountLOCProvider(streamFactory);
			provider.AddMetadata("FileTypes", ".cs=C#");
			var measures = provider.ComputeMetrics(new List<string>().AddItem("FileLOC").AddItem("EmptyLOC").AddItem("CommentLOC"), files).ToList();
			Assert.AreEqual("TotalLOC", measures[0].MetricName);
			Assert.AreEqual("C#", measures[0].MeasureName);
			Assert.AreEqual(34, measures[0].Value);
			Assert.AreEqual("EmptyLOC", measures[2].MetricName);
			Assert.AreEqual("C#", measures[2].MeasureName);
			Assert.AreEqual(3, measures[2].Value);
			Assert.AreEqual("CommentLOC", measures[3].MetricName);
			Assert.AreEqual("C#", measures[3].MeasureName);
			Assert.AreEqual(7, measures[3].Value);
			Assert.AreEqual("CodeLOC", measures[1].MetricName);
			Assert.AreEqual("C#", measures[1].MeasureName);
			Assert.AreEqual(24, measures[1].Value);
		}

        [TestMethod]
        public void WhenCallingComputeMetricsWithoutSpecifyingFileTypeShouldIgnoreFile()
        {
            List<string> files = new List<string>().AddItem("file1.cs");
            FileStreamFactoryMock streamFactory = new FileStreamFactoryMock();
            streamFactory.AddFileMock("file1.cs", TestResources.SourceFile1);

            CountLOCProvider provider = new CountLOCProvider(streamFactory);
            provider.AddMetadata("FileTypes", ".vb=VB.NET");
            var measures = provider.ComputeMetrics(new List<string>().AddItem("FileLOC").AddItem("EmptyLOC").AddItem("CommentLOC"), files).ToList();
            Assert.AreEqual(0, measures.Count);            
        }

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void TestCountLocProviderWithoutExtensions()
		{
		    _provider.ComputeMetrics(new List<string>().AddItem("FileLOC").AddItem("EmptyLOC").AddItem("CommentLOC"), _files);
		}

	    [TestMethod]
        [ExpectedException(typeof(Exception))]
	    public void WhenAddingFileTypesInWrongSyntaxShouldThrow()
	    {
            _provider.AddMetadata("FileTypes", "C#");
	    }
	}
}
