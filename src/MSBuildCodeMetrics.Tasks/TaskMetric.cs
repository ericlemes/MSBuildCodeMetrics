using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Tasks
{
	public class TaskMetric
	{
		private string _providerName;
		public string ProviderName
		{
			get { return _providerName; }
		}

		private string _metricName;
		public string MetricName
		{
			get { return _metricName; }
		}

		private IEnumerable<int> _ranges;
		public IEnumerable<int> Ranges
		{
			get { return _ranges; }
		}

		private IEnumerable<string> _files;
		public IEnumerable<string> Files
		{
			get { return _files; }
		}

		public TaskMetric(string providerName, string metricName, IEnumerable<int> ranges, IEnumerable<string> files)
		{
			_providerName = providerName;
			_metricName = metricName;
			_ranges = ranges;
			_files = files;
		}
	}

}
