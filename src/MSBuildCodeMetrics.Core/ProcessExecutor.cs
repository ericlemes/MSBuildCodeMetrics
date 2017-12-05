using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MSBuildCodeMetrics.Core
{
    public class ProcessExecutor : IProcessExecutor
    {
        private readonly ILogger _logger;

        public ProcessExecutor(ILogger logger)
        {
            _logger = logger;
        }

        public void ExecuteProcess(string executable, string arguments)
        {
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);

            Process p = CreateProcessInstanceForConsoleApp(executable, arguments, fi);            
            _logger.LogMessage("Starting process " + executable + " " + arguments);
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
            p.StartInfo.WorkingDirectory = fi.DirectoryName ?? "";
            return p;
        }
    }
}
