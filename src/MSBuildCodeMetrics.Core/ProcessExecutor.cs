using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MSBuildCodeMetrics.Core
{
    /// <summary>
    /// Utility class to execute external processes
    /// </summary>
    public class ProcessExecutor : IProcessExecutor
    {
        private ILogger _logger;

        /// <summary>
        /// Creates new Process Executor instance
        /// </summary>
        /// <param name="logger"></param>
        public ProcessExecutor(ILogger logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Executes an external executable. Assumes current executing assembly path as working dir and throws exception on fail.
        /// Logs stdout as messages on logger and stderr as errors on logger.
        /// </summary>
        /// <param name="executable">Full path of executable</param>
        /// <param name="arguments">All the arguments as a string</param>
        public void ExecuteProcess(string executable, string arguments)
        {
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);

            Process p = CreateProcessInstanceForConsoleApp(executable, arguments, fi);            
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();

            if (p.ExitCode != 0)
                throw new Exception("Error running process: " + p.StartInfo.FileName + p.StartInfo.Arguments + ". Exit code " + p.ExitCode.ToString() +
                    Environment.NewLine);
        }

        private Process CreateProcessInstanceForConsoleApp(string executable, string arguments, FileInfo fi)
        {
            _logger.LogMessage("ExePath: " + executable);

            if (!File.Exists(executable))
                _logger.LogError("File not found: " + executable);

            Process p = new Process();
            p.ErrorDataReceived += (o, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                    _logger.LogError(e.Data);  
            };
            p.OutputDataReceived += (o, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                    _logger.LogMessage(e.Data);
            };
            p.StartInfo.FileName = "\"" + executable + "\"";
            p.StartInfo.Arguments = arguments;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = fi.DirectoryName;
            return p;
        }
    }
}
