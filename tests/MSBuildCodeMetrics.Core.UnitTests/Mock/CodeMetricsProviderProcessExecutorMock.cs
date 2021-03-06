﻿namespace MSBuildCodeMetrics.Core.UnitTests.Mock
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
