using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	public class ProviderMeasure
	{
		private string _metricName;
		public string MetricName
		{
			get { return _metricName; }
		}

		private string _measureName;
		public string MeasureName
		{
			get { return _measureName; }
		}

		private int _value;
		public int Value
		{
			get { return _value; }
		}

		public ProviderMeasure(string metricName, string itemName, int value)
		{
			_metricName = metricName;
			_measureName = itemName;
			_value = value;
		}
	}
}
