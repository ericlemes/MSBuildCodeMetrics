using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSBuildCodeMetrics.Core;
using Microsoft.Build.Utilities;

namespace MSBuildCodeMetrics.Tasks
{
	public class MSBuildLogger : ILogger
	{
		private TaskLoggingHelper _log;

		public MSBuildLogger(TaskLoggingHelper log)
		{
			_log = log;
		}

		public void LogMessage(string msg)
		{
			_log.LogMessage(msg);
		}

		public void LogError(string error)
		{
			_log.LogError(error);
		}
	}
}
