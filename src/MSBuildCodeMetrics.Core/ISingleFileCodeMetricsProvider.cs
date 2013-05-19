using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This interface must be implemented for providers that need to compute metric for each file perspective.
	/// </summary>
	public interface ISingleFileCodeMetricsProvider : ICodeMetricsProvider
	{
		/// <summary>
		/// Called from CodeMetricsRunner. Computes the metrics.
		/// </summary>
		/// <param name="metricsToCompute">Each item in the list is a metric to compute</param>
		/// <param name="fileName">The file that will have its metrics extracted</param>
		/// <returns>a set of measures</returns>
		IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, string fileName);
	}
}
