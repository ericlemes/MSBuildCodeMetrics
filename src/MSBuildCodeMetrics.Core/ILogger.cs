namespace MSBuildCodeMetrics.Core
{
	public interface ILogger
	{
		void LogMessage(string msg);

		void LogError(string error);
	}
}
