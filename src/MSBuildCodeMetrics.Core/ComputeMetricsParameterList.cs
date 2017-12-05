using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	public class ComputeMetricsParameterList : List<ComputeMetricsParameter>
	{
		public static ComputeMetricsParameterList Create()
		{
			return new ComputeMetricsParameterList();
		}

		public ComputeMetricsParameterList Add(string providerName, string metricName, IEnumerable<string> files)
		{
			base.Add(new ComputeMetricsParameter(providerName, metricName, files));
			return this;
		}
	}

}
