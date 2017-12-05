using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	public interface ISingleFileCodeMetricsProvider : ICodeMetricsProvider
	{
		IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, string fileName);
	}
}
