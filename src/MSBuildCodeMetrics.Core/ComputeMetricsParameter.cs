using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This class represents the input parameters for each metric computation. Since each metric can have it's own
	/// files, this is the way the input parameters are passed in the moment of the metrics computation.
	/// </summary>
	public class ComputeMetricsParameter
	{
        private readonly string _providerName;
		/// <summary>
		/// The provider name
		/// </summary>
		public string ProviderName
		{
			get { return _providerName; }
		}

		private readonly string _metricName;
		/// <summary>
		/// Metric name
		/// </summary>
		public string MetricName
		{
			get { return _metricName; }
		}

		private readonly IEnumerable<string> _files;
		/// <summary>
		/// List of files to compute metrics from
		/// </summary>
		public IEnumerable<string> Files
		{
			get { return _files; }
		}

		/// <summary>
		/// Creates a new ComputeMetricsParameter for the specified provider, metric and files.
		/// </summary>
		/// <param name="providerName">Provider name</param>
		/// <param name="metricName">Metric name</param>
		/// <param name="files">Files</param>
		public ComputeMetricsParameter(string providerName, string metricName, IEnumerable<string> files)
		{
			_providerName = providerName;
			_metricName = metricName;
			_files = files;
		}
	}
}
