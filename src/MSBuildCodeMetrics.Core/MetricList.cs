using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	public class MetricList : List<Metric>
	{
		public static MetricList Create()
		{
			return new MetricList();
		}

		public MetricList Add(string providerName, string metricName, IEnumerable<int> range)
		{
			base.Add(new Metric(providerName, metricName, range));
			return this;
		}

		public MetricList Add(string providerName, string metricName)
		{
			base.Add(new Metric(providerName, metricName, null));
			return this;
		}
	}
}
