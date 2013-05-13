using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Tasks
{
	public class MSBuildCodeMetricsTaskException : Exception
	{
		public MSBuildCodeMetricsTaskException(string msg) : base(msg)
		{
		}
	}
}
