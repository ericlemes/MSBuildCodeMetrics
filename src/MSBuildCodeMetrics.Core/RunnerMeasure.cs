using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This class represent one measure from the Runner perspective. It also includes the provider for each metric.
	/// </summary>
	public class RunnerMeasure
	{
		private string _providerName;
		/// <summary>
		/// The provider that generated this measure
		/// </summary>
		public string ProviderName
		{
			get { return _providerName; }
		}

		private string _metricName;
		/// <summary>
		/// Name of the metric
		/// </summary>
		public string MetricName
		{
			get { return _metricName; }
		}

		private string _measureName;
		/// <summary>
		/// Name of the measure or item.
		/// </summary>
		public string MeasureName
		{
			get { return _measureName; }
		}

		private int _value;
		/// <summary>
		/// The value of the measure or item
		/// </summary>
		public int Value
		{
			get { return _value; }
		}

		/// <summary>
		/// Creates a new runner measure
		/// </summary>
		/// <param name="providerName">The name of the provider</param>
		/// <param name="providerMeasure">The provider measure that originated this runner measure</param>
		public RunnerMeasure(string providerName, ProviderMeasure providerMeasure)
		{
			_providerName = providerName;
			_metricName = providerMeasure.MetricName;
			_measureName = providerMeasure.MeasureName;
			_value = providerMeasure.Value;
		}
	}
}
