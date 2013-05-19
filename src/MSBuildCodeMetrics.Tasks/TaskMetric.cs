using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Tasks
{
	/// <summary>
	/// Represents an input parameter for a task metric. Used to parse the MSBuild TaskItem.
	/// </summary>
	public class TaskMetric
	{
		private string _providerName;
		/// <summary>
		/// Provider name
		/// </summary>
		public string ProviderName
		{
			get { return _providerName; }
		}

		private string _metricName;
		/// <summary>
		/// Metric name
		/// </summary>
		public string MetricName
		{
			get { return _metricName; }
		}

		private IEnumerable<int> _ranges;
		/// <summary>
		/// Ranges
		/// </summary>
		public IEnumerable<int> Ranges
		{
			get { return _ranges; }
		}

		private IEnumerable<string> _files;
		/// <summary>
		/// The files that will be processed in this metric
		/// </summary>
		public IEnumerable<string> Files
		{
			get { return _files; }
		}

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
