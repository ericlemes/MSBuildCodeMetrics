using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This class is used as parameter in the reports. Each Metric has the input information needed to generate the reports.
	/// </summary>
	public class Metric
	{
		private readonly string _providerName;
		/// <summary>
		/// The name of the provider
		/// </summary>
		public string ProviderName
		{
			get { return _providerName; }
		}

		private readonly string _metricName;
		/// <summary>
		/// The metric name
		/// </summary>
		public string MetricName
		{
			get { return _metricName; }
		}

		private readonly IEnumerable<int> _range;
		/// <summary>
		/// The ranges. Example: if 10 and 5 is specified, the following ranges will be computed: x &gt; 10, 10 &gt;= x &gt; 5, x &lt; 5
		/// </summary>
		public IEnumerable<int> Range
		{
			get { return _range; }
		}

		/// <summary>
		/// Creates a new metric
		/// </summary>
		/// <param name="providerName">Provider name</param>
		/// <param name="metricName">Metric name</param>
		/// <param name="range">The ranges (ex: 10, 5)</param>
		public Metric(string providerName, string metricName, IEnumerable<int> range)
		{
			_providerName = providerName;
			_metricName = metricName;
			_range = range;			
		}
	}
}
