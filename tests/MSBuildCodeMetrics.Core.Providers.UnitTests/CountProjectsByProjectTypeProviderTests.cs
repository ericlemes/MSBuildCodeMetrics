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
	public class CountProjectsByProjectTypeProviderTests
	{
		[TestMethod]
		public void TestCountProjectsByProjectType()
		{
			List<string> files = new List<string>().AddItem("file1.csproj").AddItem("file2.csproj");
			FileStreamFactoryMock streamFactory = new FileStreamFactoryMock();			
			streamFactory.AddFileMock("file1.csproj", TestResources.Project1);
			streamFactory.AddFileMock("file2.csproj", TestResources.Project2);

			CountProjectsByProjectTypeProvider provider = new CountProjectsByProjectTypeProvider(streamFactory);
			List<ProviderMeasure> measures = provider.ComputeMetrics(new List<string>().AddItem("ProjectTypeCount"), files).ToList<ProviderMeasure>();
			Assert.AreEqual("Library", measures[0].MeasureName);
			Assert.AreEqual(2, measures[0].Value);
		}
	}
}
