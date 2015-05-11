namespace MSBuildCodeMetrics.Core
{
    /// <summary>
    /// Interface for ProcessExecutor (utility class to execute external processes).
    /// </summary>
    public interface IProcessExecutor
    {
        /// <summary>
        /// Executes an external process, assuming current assembly dir, the working dir.
        /// </summary>
        /// <param name="executable"></param>
        /// <param name="arguments"></param>
        void ExecuteProcess(string executable, string arguments);
    }
}
