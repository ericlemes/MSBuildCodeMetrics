using System.Collections.Generic;

namespace MSBuildCodeMetrics.Tasks
{
	/// <summary>
	/// Represents an input parameter for a task metric. Used to parse the MSBuild TaskItem.
	/// </summary>
	public class TaskMetric
	{
		private readonly string _providerName;
		/// <summary>
		/// Provider name
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

		private readonly IEnumerable<int> _ranges;
		/// <summary>
		/// Ranges
		/// </summary>
		public IEnumerable<int> Ranges
		{
			get { return _ranges; }
		}

		private readonly IEnumerable<string> _files;
		/// <summary>
		/// The files that will be processed in this metric
		/// </summary>
		public IEnumerable<string> Files
		{
			get { return _files; }
		}

        /// <summary>
        /// Error message to be used in build failure if some condition in the higher band happens.
        /// Useful to break the build if some metric is unacceptable.
        /// </summary>
        public string HigherRangeFailMessage { get; set; }

        /// <summary>
        /// Error message to be used in build failure if some condidtion in the lower band happens
        /// Useful to break the build if some metric is unacepptable for metrics which lower numbers are not good.
        /// For example, code coverage.
        /// </summary>
        public string LowerRangeFailMessage { get; set; }

		/// <summary>
		/// Creates a new task metric
		/// </summary>
		/// <param name="providerName">Provider name</param>
		/// <param name="metricName">Metric name</param>
		/// <param name="ranges">Ranges</param>
		/// <param name="files">Files</param>
		public TaskMetric(string providerName, string metricName, IEnumerable<int> ranges, IEnumerable<string> files)
		{
			_providerName = providerName;
			_metricName = metricName;
			_ranges = ranges;
			_files = files;
		}
    }

}
