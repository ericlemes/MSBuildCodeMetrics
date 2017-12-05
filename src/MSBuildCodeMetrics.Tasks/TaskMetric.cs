using System.Collections.Generic;

namespace MSBuildCodeMetrics.Tasks
{
	public class TaskMetric
	{
		private readonly string _providerName;
		public string ProviderName
		{
			get { return _providerName; }
		}

		private readonly string _metricName;
		public string MetricName
		{
			get { return _metricName; }
		}

		private readonly IEnumerable<int> _ranges;
		public IEnumerable<int> Ranges
		{
			get { return _ranges; }
		}

		private readonly IEnumerable<string> _files;
		public IEnumerable<string> Files
		{
			get { return _files; }
		}

        public string HigherRangeFailMessage { get; set; }

        public string LowerRangeFailMessage { get; set; }

		public TaskMetric(string providerName, string metricName, IEnumerable<int> ranges, IEnumerable<string> files)
		{
			_providerName = providerName;
			_metricName = metricName;
			_ranges = ranges;
			_files = files;
		}
    }

}
