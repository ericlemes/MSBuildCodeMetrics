using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	public interface ISingleFileCodeMetricsProvider : ICodeMetricsProvider
	{
		IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, string fileName);
	}
}
