namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This interface is used to allow providers to log its activity. 
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// This method is used for log a message. It's used only for non-fatal errors or non-standard conditions.
		/// </summary>
		/// <param name="msg">The message</param>
		void LogMessage(string msg);

		/// <summary>
		/// This method is used to log fatal errors.
		/// </summary>
		/// <param name="error">The error</param>
		void LogError(string error);
	}
}
