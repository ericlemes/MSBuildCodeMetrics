namespace MSBuildCodeMetrics.Core
{
	public class ProviderMeasure
	{
		private readonly string _metricName;

		public string MetricName
		{
			get { return _metricName; }
		}


		private readonly string _measureName;
		public string MeasureName
		{
			get { return _measureName; }
		}

		private readonly int _value;
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
