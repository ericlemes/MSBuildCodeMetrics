namespace MSBuildCodeMetrics.Core
{
    /// <summary>
    /// Interface to be implemented by providers that wants IProcessExecutor to be injected.
    /// </summary>
    public interface IProcessExecutorCodeMetricsProvider
    {
        /// <summary>
        /// Setter for process executor
        /// </summary>
        IProcessExecutor ProcessExecutor { set; }
    }
}
