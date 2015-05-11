using MSBuildCodeMetrics.Core;
using Microsoft.Build.Utilities;

namespace MSBuildCodeMetrics.Tasks
{
	/// <summary>
	/// ILogger implementation for MSBuild default logger.
	/// </summary>
	public class MSBuildLogger : ILogger
	{
		private readonly TaskLoggingHelper _log;

		/// <summary>
		/// Creates a new MSBuildLogger
		/// </summary>
		/// <param name="log">The MSBuild logger</param>
		public MSBuildLogger(TaskLoggingHelper log)
		{
			_log = log;
		}

		/// <summary>
		/// Log a non-fatal message
		/// </summary>
		/// <param name="msg">Message</param>
		public void LogMessage(string msg)
		{
			_log.LogMessage(msg);
		}

		/// <summary>
		/// Logs a fatal error
		/// </summary>
		/// <param name="error">The error</param>
		public void LogError(string error)
		{
			_log.LogError(error);
		}
	}
}
