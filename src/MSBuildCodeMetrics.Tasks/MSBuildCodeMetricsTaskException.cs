using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Tasks
{
	/// <summary>
	/// Exception generated when errors occurs when executing the task.
	/// </summary>
	public class MSBuildCodeMetricsTaskException : Exception
	{
		/// <summary>
		/// Creates a new exception
		/// </summary>
		/// <param name="msg">Message</param>
		public MSBuildCodeMetricsTaskException(string msg) : base(msg)
		{
		}
	}
}
