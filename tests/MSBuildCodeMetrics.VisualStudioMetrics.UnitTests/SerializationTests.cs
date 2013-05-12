using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MSBuildCodeMetrics.VisualStudioCodeMetrics.XML;
using MSBuildCodeMetrics.VisualStudioMetrics;
using MSBuildCodeMetrics.VisualStudioMetrics.UnitTests.Resources;

namespace MSBuildCodeMetrics.Core.VisualStudioMetrics.Tests
{
	[TestClass]
	public class SerializationTests
	{
		private Stream _metricsOutputStream;
		private ModuleReport _moduleReport;

		[TestInitialize]
		public void Initialize()
		{
			_metricsOutputStream = new MemoryStream();
			StreamWriter sw = new StreamWriter(_metricsOutputStream);
			sw.Write(TestResources.MetricsOutput);
			sw.Flush();
			_metricsOutputStream.Seek(0, SeekOrigin.Begin);

			_moduleReport = CodeMetricsReport.Deserialize(_metricsOutputStream);
		}

		[TestMethod]
		public void TestModuleReportSerialization()
		{
			Assert.AreEqual("EricLemes.MSBuildTasks.Tests.DummyAssembly.dll", _moduleReport.Name);
			Assert.AreEqual("1.0.0.0", _moduleReport.AssemblyVersion);
			Assert.AreEqual("1.0.0.0", _moduleReport.FileVersion);
			Assert.AreEqual(89, _moduleReport.Metrics.MaintainabilityIndex);
			Assert.AreEqual(7, _moduleReport.Metrics.CyclomaticComplexity);
			Assert.AreEqual(1, _moduleReport.Metrics.ClassCoupling);
			Assert.AreEqual(1, _moduleReport.Metrics.DepthOfInheritance);
			Assert.AreEqual(12, _moduleReport.Metrics.LinesOfCode);
		}

		[TestMethod]
		public void TestNamespaceReportSerialization()
		{
			Assert.AreEqual(1, _moduleReport.Namespaces.Count);

			NamespaceReport namespaceReport = _moduleReport.Namespaces[0];

			Assert.AreEqual("EricLemes.MSBuildTasks.Tests.DummyAssembly", namespaceReport.Name);
			Assert.AreEqual(89, namespaceReport.Metrics.MaintainabilityIndex);
			Assert.AreEqual(7, namespaceReport.Metrics.CyclomaticComplexity);
			Assert.AreEqual(1, namespaceReport.Metrics.ClassCoupling);
			Assert.AreEqual(1, namespaceReport.Metrics.DepthOfInheritance);
			Assert.AreEqual(12, namespaceReport.Metrics.LinesOfCode);
		}

		[TestMethod]
		public void TestTypeReportSerialization()
		{
			NamespaceReport namespaceReport = _moduleReport.Namespaces[0];
			Assert.AreEqual(2, namespaceReport.Types.Count);

			TypeReport dummyClassTypeReport = namespaceReport.Types[0];
			Assert.AreEqual("DummyClass", dummyClassTypeReport.Name);
			Assert.AreEqual("EricLemes.MSBuildTasks.Tests.DummyAssembly", dummyClassTypeReport.Namespace.Name);
			Assert.AreEqual(81, dummyClassTypeReport.Metrics.MaintainabilityIndex);
			Assert.AreEqual(4, dummyClassTypeReport.Metrics.CyclomaticComplexity);
			Assert.AreEqual(1, dummyClassTypeReport.Metrics.ClassCoupling);
			Assert.AreEqual(1, dummyClassTypeReport.Metrics.DepthOfInheritance);
			Assert.AreEqual(9, dummyClassTypeReport.Metrics.LinesOfCode);
		}

		[TestMethod]
		public void TestSimpleMethodReportXMLSerialization()
		{
			NamespaceReport namespaceReport = _moduleReport.Namespaces[0];
			TypeReport dummyClassTypeReport = namespaceReport.Types[0];
			MemberReport simpleMethodReport = dummyClassTypeReport.Members[0];

			Assert.AreEqual("SimpleMethod() : void", simpleMethodReport.Name);
			Assert.AreEqual(@"C:\SandBox\EricLemes.MSBuildTasks\src\EricLemes.MSBuildTasks.Tests.DummyAssembly\DummyClass.cs", simpleMethodReport.File);
			Assert.AreEqual("DummyClass", simpleMethodReport.Type.Name);
			Assert.AreEqual(11, simpleMethodReport.Line);
			Assert.AreEqual(78, simpleMethodReport.Metrics.MaintainabilityIndex);
			Assert.AreEqual(1, simpleMethodReport.Metrics.CyclomaticComplexity);
			Assert.AreEqual(1, simpleMethodReport.Metrics.ClassCoupling);
			Assert.AreEqual(5, simpleMethodReport.Metrics.LinesOfCode);
		}

		[TestMethod]
		public void TestSimpleMethod2ReportSerialization()
		{
			NamespaceReport namespaceReport = _moduleReport.Namespaces[0];
			TypeReport dummyClassTypeReport = namespaceReport.Types[0];			
			MemberReport simpleMethod2Report = dummyClassTypeReport.Members[1];

			Assert.AreEqual("SimpleMethod2(bool) : void", simpleMethod2Report.Name);
			Assert.AreEqual(@"C:\SandBox\EricLemes.MSBuildTasks\src\EricLemes.MSBuildTasks.Tests.DummyAssembly\DummyClass.cs", simpleMethod2Report.File);
			Assert.AreEqual(20, simpleMethod2Report.Line);
			Assert.AreEqual(81, simpleMethod2Report.Metrics.MaintainabilityIndex);
			Assert.AreEqual(2, simpleMethod2Report.Metrics.CyclomaticComplexity);
			Assert.AreEqual(1, simpleMethod2Report.Metrics.ClassCoupling);
			Assert.AreEqual(3, simpleMethod2Report.Metrics.LinesOfCode);
		}

		[TestMethod]
		public void TestDummyClassConstructorMethodReportSerialization()
		{
			NamespaceReport namespaceReport = _moduleReport.Namespaces[0];
			TypeReport dummyClassTypeReport = namespaceReport.Types[0];			
			MemberReport dummyClassConstructorReport = dummyClassTypeReport.Members[2];

			Assert.AreEqual("DummyClass()", dummyClassConstructorReport.Name);
			Assert.AreEqual(null, dummyClassConstructorReport.File);
			Assert.AreEqual(0, dummyClassConstructorReport.Line);
			Assert.AreEqual(100, dummyClassConstructorReport.Metrics.MaintainabilityIndex);
			Assert.AreEqual(1, dummyClassConstructorReport.Metrics.CyclomaticComplexity);
			Assert.AreEqual(0, dummyClassConstructorReport.Metrics.ClassCoupling);
			Assert.AreEqual(1, dummyClassConstructorReport.Metrics.LinesOfCode);
		}
	}
}
