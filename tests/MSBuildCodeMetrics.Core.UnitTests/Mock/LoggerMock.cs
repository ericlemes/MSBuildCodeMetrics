using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core.UnitTests.Mock
{
	public class LoggerMock : ILogger
	{
		public void LogMessage(string msg)
		{
			Console.WriteLine(msg);
		}

		public void LogError(string error)
		{
			Console.WriteLine(error);
		}
	}
}
