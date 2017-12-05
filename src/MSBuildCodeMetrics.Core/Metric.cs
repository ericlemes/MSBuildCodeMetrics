using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	public class Metric
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

		private readonly IEnumerable<int> _range;
		public IEnumerable<int> Range
		{
			get { return _range; }
		}

		public Metric(string providerName, string metricName, IEnumerable<int> range)
		{
			_providerName = providerName;
			_metricName = metricName;
			_range = range;			
		}
	}
}
