using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.Providers.UnitTests.Resources;
using MSBuildCodeMetrics.Core.UnitTests.Mock;

namespace MSBuildCodeMetrics.Core.Providers.UnitTests
{
	[TestClass]
	public class CountProjectsByProjectTypeProviderTests
	{
	    [TestMethod]
	    public void WhenConstructingShouldInitializeProperly()
	    {
	        var p = new CountProjectsByProjectTypeProvider();
            Assert.AreEqual("CountProjectsByProjectTypeProvider", p.Name);
            Assert.AreEqual(1, p.GetMetrics().Count());
            Assert.AreEqual("ProjectTypeCount", p.GetMetrics().First());
	    }

		[TestMethod]
		public void TestCountProjectsByProjectType()
		{
			List<string> files = new List<string>().AddItem("file1.csproj").AddItem("file2.csproj");
			FileStreamFactoryMock streamFactory = new FileStreamFactoryMock();			
			streamFactory.AddFileMock("file1.csproj", TestResources.Project1);
			streamFactory.AddFileMock("file2.csproj", TestResources.Project2);

			CountProjectsByProjectTypeProvider provider = new CountProjectsByProjectTypeProvider(streamFactory);
			var measures = provider.ComputeMetrics(new List<string>().AddItem("ProjectTypeCount"), files).ToList();
			Assert.AreEqual("Library", measures[0].MeasureName);
			Assert.AreEqual(2, measures[0].Value);
		}
	}
}
