using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	public class ComputeMetricsParameter
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

		private IEnumerable<string> _files;
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
