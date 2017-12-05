using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	public interface IMultiFileCodeMetricsProvider : ICodeMetricsProvider
	{
		IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, IEnumerable<string> files);
	}
}
