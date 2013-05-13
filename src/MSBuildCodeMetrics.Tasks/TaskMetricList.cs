using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSBuildCodeMetrics.Core;

namespace MSBuildCodeMetrics.Tasks
{
	public class TaskMetricList : List<TaskMetric>
	{
		public static TaskMetricList Create()
		{
			return new TaskMetricList();
		}

		public TaskMetricList Add(string providerName, string metricName, IEnumerable<int> ranges, IEnumerable<string> files)
		{
			base.Add(new TaskMetric(providerName, metricName, ranges, files));
			return this;
		}

		public MetricList ToMetricList()
		{
			MetricList ml = new MetricList();
			foreach (TaskMetric t in this)			
				ml.Add(t.ProviderName, t.MetricName, t.Ranges);
			return ml;
		}

		public ComputeMetricsParameterList ToComputeMetricsParameterList()
		{
			ComputeMetricsParameterList pl = new ComputeMetricsParameterList();
			foreach (TaskMetric t in this)
				pl.Add(t.ProviderName, t.MetricName, t.Files);
			return pl;
		}
	}
}
