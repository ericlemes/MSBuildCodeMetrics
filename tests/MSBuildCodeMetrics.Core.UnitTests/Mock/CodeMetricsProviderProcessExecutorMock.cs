using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBuildCodeMetrics.Core.UnitTests.Mock
{
    public class CodeMetricsProviderProcessExecutorMock : CodeMetricsProviderBaseMock, IProcessExecutorCodeMetricsProvider
    {
        public static IProcessExecutor LastProcessExecutorSet { get; set; }

        public IProcessExecutor ProcessExecutor
        {
            get { return LastProcessExecutorSet; }
            set { LastProcessExecutorSet = value; }
        }        
    }
}
