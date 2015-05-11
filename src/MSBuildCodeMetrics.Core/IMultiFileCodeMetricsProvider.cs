using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This interface should be implemented for providers that needs to compute metrics for a set of files.
	/// </summary>
	public interface IMultiFileCodeMetricsProvider : ICodeMetricsProvider
	{
		/// <summary>
		/// Called from CodeMetricsRunner. Computes the metrics
		/// </summary>
		/// <param name="metricsToCompute">Each item in the list is a metric to compute.</param>
		/// <param name="files">The list of files that will have its metrics computed.</param>
		/// <returns>a set of measures</returns>
		IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, IEnumerable<string> files);
	}
}
