using System;

namespace MSBuildCodeMetrics.Tasks
{
	public class MSBuildCodeMetricsTaskException : Exception
	{
		public MSBuildCodeMetricsTaskException(string msg) : base(msg)
		{
		}
	}
}
