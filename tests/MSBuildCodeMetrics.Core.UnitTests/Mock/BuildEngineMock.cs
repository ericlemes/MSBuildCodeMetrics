using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;

namespace MSBuildCodeMetrics.Core.UnitTests.Mock
{
	public class BuildEngineMock : IBuildEngine
	{
		public bool BuildProjectFile(string projectFileName, string[] targetNames, System.Collections.IDictionary globalProperties, System.Collections.IDictionary targetOutputs)
		{
			throw new NotImplementedException();
		}

		public int ColumnNumberOfTaskNode
		{
			get { return 0; }
		}

		public bool ContinueOnError
		{
			get { throw new NotImplementedException(); }
		}

		public int LineNumberOfTaskNode
		{
			get { return 0; }
		}

		public void LogCustomEvent(CustomBuildEventArgs e)
		{
			throw new NotImplementedException();
		}

		public void LogErrorEvent(BuildErrorEventArgs e)
		{
			
		}

		public void LogMessageEvent(BuildMessageEventArgs e)
		{
			Console.WriteLine(e.Message);
		}

		public void LogWarningEvent(BuildWarningEventArgs e)
		{
			throw new NotImplementedException();
		}

		public string ProjectFileOfTaskNode
		{
			get { return ""; }
		}
	}
}
