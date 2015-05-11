using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This class is just a builder for List&lt;Metric&gt;. Syntax sugar.
	/// </summary>
	public class MetricList : List<Metric>
	{
		/// <summary>
		/// Creates a new MetricList and return its reference
		/// </summary>
		/// <returns>The new MetricList</returns>
		public static MetricList Create()
		{
			return new MetricList();
		}

		/// <summary>
		/// Adds a new Metric to the list and returs the list.
		/// </summary>
		/// <param name="providerName">Provider name</param>
		/// <param name="metricName">Metric name</param>
		/// <param name="range">Range</param>
		/// <returns>The metric list</returns>
		public MetricList Add(string providerName, string metricName, IEnumerable<int> range)
		{
			base.Add(new Metric(providerName, metricName, range));
			return this;
		}

		/// <summary>
		/// Adds a new Metric to the list and returs the list.
		/// </summary>
		/// <param name="providerName">Provider name</param>
		/// <param name="metricName">Metric name</param>
		/// <returns>The metric list</returns>
		public MetricList Add(string providerName, string metricName)
		{
			base.Add(new Metric(providerName, metricName, null));
			return this;
		}
	}
}
