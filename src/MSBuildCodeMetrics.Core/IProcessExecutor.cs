using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBuildCodeMetrics.Core
{
    public interface IProcessExecutor
    {
        void ExecuteProcess(string executable, string arguments);
    }
}
