namespace MSBuildCodeMetrics.Core
{
    public interface IProcessExecutor
    {
        void ExecuteProcess(string executable, string arguments);
    }
}
