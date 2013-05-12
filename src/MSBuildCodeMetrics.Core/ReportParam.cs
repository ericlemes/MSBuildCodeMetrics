using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	public class ReportParam
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

		public ReportParam(string providerName, string metricName, IEnumerable<int> range)
		{
			_providerName = providerName;
			_metricName = metricName;
			_range = range;
		}
	}

	public class ReportParamList : List<ReportParam>
	{
		public static ReportParamList Create()
		{
			return new ReportParamList();
		}

		public ReportParamList Add(string providerName, string metricName, IEnumerable<int> range)
		{
			base.Add(new ReportParam(providerName, metricName, range));
			return this;
		}

		public ReportParamList Add(string providerName, string metricName)
		{
			base.Add(new ReportParam(providerName, metricName, null));
			return this;
		}
	}
}
