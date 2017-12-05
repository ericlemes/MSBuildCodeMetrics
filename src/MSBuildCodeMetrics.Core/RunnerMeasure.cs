namespace MSBuildCodeMetrics.Core
{
	public class RunnerMeasure
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

		public RunnerMeasure(string providerName, ProviderMeasure providerMeasure)
		{
			_providerName = providerName;
			_metricName = providerMeasure.MetricName;
			_measureName = providerMeasure.MeasureName;
			_value = providerMeasure.Value;
		}
	}
}
