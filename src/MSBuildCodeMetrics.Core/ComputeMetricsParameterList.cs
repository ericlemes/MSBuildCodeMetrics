using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This class is just a builder for List&lt;ComputeMetricsParameter&gt;. Syntax sugar.
	/// </summary>
	public class ComputeMetricsParameterList : List<ComputeMetricsParameter>
	{
		/// <summary>
		/// Creates a new ComputeMetricsParameterList and returns its reference
		/// </summary>
		/// <returns>The new ComputeMetricsParameterList</returns>
		public static ComputeMetricsParameterList Create()
		{
			return new ComputeMetricsParameterList();
		}

		/// <summary>
		/// Creates and adds a new ComputeMetricsParameter to the list
		/// </summary>
		/// <param name="providerName">Provider name</param>
		/// <param name="metricName">Metric name</param>
		/// <param name="files">Files</param>
		/// <returns>The compute metrics parameter list</returns>
		public ComputeMetricsParameterList Add(string providerName, string metricName, IEnumerable<string> files)
		{
			base.Add(new ComputeMetricsParameter(providerName, metricName, files));
			return this;
		}
	}

}
