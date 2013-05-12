using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	public class Metric
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

		private IEnumerable<int> _range;
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
