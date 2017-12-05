using System.Collections.Generic;
using System.Linq;

namespace MSBuildCodeMetrics.Core.UnitTests.Mock
{
	public class CodeMetricsProviderMultiFileMock : CodeMetricsProviderBaseMock, IMultiFileCodeMetricsProvider
	{

        private ILogger _logger;
        public ILogger Logger
        {
            set { _logger = value; }
        }

        public CodeMetricsProviderMultiFileMock(string name)
		{
			_name = name;
		}		

		public IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, IEnumerable<string> files)
		{
			return
				(from ProviderMeasure m in Measures
				 where metricsToCompute.Contains(m.MetricName)
				 select m);
		}

		public static CodeMetricsProviderMultiFileMock Create(string providerName)
		{
			return new CodeMetricsProviderMultiFileMock(providerName);			
		}

		public CodeMetricsProviderMultiFileMock AddMetric(string metric)
		{
			_metrics.Add(metric);
			return this;
		}

		public CodeMetricsProviderMultiFileMock AddMeasure(string metric, string item, int value)
		{
			_measures.Add(new ProviderMeasure(metric, item, value));
			return this;
		}
	}
}
