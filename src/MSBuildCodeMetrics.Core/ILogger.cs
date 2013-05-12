using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	public interface ILogger
	{
		void LogMessage(string msg);
		void LogError(string error);
	}
}
