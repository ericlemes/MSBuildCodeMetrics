using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	public class ComputeMetricsParameter
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

		private readonly IEnumerable<string> _files;

		public IEnumerable<string> Files
		{
			get { return _files; }
		}

		public ComputeMetricsParameter(string providerName, string metricName, IEnumerable<string> files)
		{
			_providerName = providerName;
			_metricName = metricName;
			_files = files;
		}
	}
}
