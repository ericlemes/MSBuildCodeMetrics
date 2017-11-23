using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core.XML;
using MSBuildCodeMetrics.Tasks;
using MSBuildCodeMetrics.Core.UnitTests.Mock;
using Microsoft.Build.Framework;
using System.IO;
using System.Reflection;

namespace MSBuildCodeMetrics.IntegrationTests
{
	[TestClass]
	public class CodeMetricsVisualStudioMetricsTaskTest
	{
		/*[TestMethod]
		[DeploymentItem(@"MSBuildCodeMetrics.Core.dll")]
		public void ExecuteTest()
		{
			string metricsExePath =
				Environment.GetEnvironmentVariable("VS140COMNTOOLS") + @"..\..\Team Tools\Static Analysis Tools\FxCop\Metrics.exe";
			string currentPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

			ITaskItem[] providers = new TaskItemMock[1];
			providers[0] = new TaskItemMock("MSBuildCodeMetrics.VisualStudioMetrics.VisualStudioCodeMetricsProvider, MSBuildCodeMetrics.VisualStudioMetrics").
				AddMetadata("TempDir", currentPath).
				AddMetadata("MetricsExePath", metricsExePath);

			ITaskItem[] metrics = new TaskItemMock[2];
			metrics[0] = new TaskItemMock("CyclomaticComplexity").
				AddMetadata("ProviderName", "VisualStudioMetrics").
				AddMetadata("Ranges", "5;10").
				AddMetadata("Files", currentPath + @"\MSBuildCodeMetrics.Core.dll");
			metrics[1] = new TaskItemMock("LinesOfCode").
				AddMetadata("ProviderName", "VisualStudioMetrics").
				AddMetadata("Ranges", "50;100").
				AddMetadata("Files", currentPath + @"\MSBuildCodeMetrics.Core.dll");

			ITaskItem[] inputFiles = new TaskItemMock[1];
			inputFiles[0] = new TaskItemMock("MSBuildCodeMetrics.Core.dll").AddMetadata("FullPath", currentPath + @"\MSBuildCodeMetrics.Core.dll");

			CodeMetrics task = new CodeMetrics();
			task.BuildEngine = new BuildEngineMock();
			task.Providers = providers;			
			task.OutputFileName = "report.xml";
			task.ShowDetailsReport = true;
			task.ShowSummaryReport = true;
			task.Metrics = metrics;
			task.FileOutput = true;
			Assert.IsTrue(task.Execute());			

			FileStream fs = new FileStream("report.xml", FileMode.Open, FileAccess.Read);
			MSBuildCodeMetricsReport report;
			using (fs)			
				report = MSBuildCodeMetricsReport.Deserialize(fs);

			Assert.AreEqual("CyclomaticComplexity", report.Details.Metrics[0].MetricName);
			var measures = report.Details.Metrics[0].Measures.Where(me => me.MeasureName == "MSBuildCodeMetrics.Core.CodeMetricsRunner.RegisterProvider(ICodeMetricsProvider) : void");
			Assert.AreEqual(1, measures.Count());
			Assert.AreEqual(1, measures.ToList()[0].Value);
		}*/
	}
}
