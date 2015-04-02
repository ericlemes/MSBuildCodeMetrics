using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MSBuildCodeMetrics.Core.UnitTests.Mock;
using System.Reflection;
using MSBuildCodeMetrics.VisualStudioMetrics;
using MSBuildCodeMetrics.Core;

namespace MSBuildCodeMetrics.VisualStudioMetrics.IntegrationTests
{
	[TestClass]
	public class VisualStudioCodeMetricsProviderTests
	{
		[TestMethod]
		[DeploymentItem(@"MSBuildCodeMetrics.Core.dll")]
		public void RunTest()
		{
			string metricsExePath =  
				Environment.GetEnvironmentVariable("VS120COMNTOOLS") + @"..\..\Team Tools\Static Analysis Tools\FxCop\Metrics.exe";
			string currentPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

			List<string> l = new List<string>();
			l.Add("CyclomaticComplexity");
			l.Add("LinesOfCode");

			VisualStudioCodeMetricsProvider provider = new VisualStudioCodeMetricsProvider(metricsExePath, currentPath);
			provider.SetLogger(new LoggerMock());
			List<ProviderMeasure> measures = provider.ComputeMetrics(l, @"MSBuildCodeMetrics.Core.dll").ToList<ProviderMeasure>();

			List<ProviderMeasure> registerProviderMeasures = measures.Where(
				m => m.MeasureName == "MSBuildCodeMetrics.Core.CodeMetricsRunner.RegisterProvider(ICodeMetricsProvider) : void").
				ToList<ProviderMeasure>();
			Assert.AreEqual(2, registerProviderMeasures.Count);
		}
	}
}
