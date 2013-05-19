using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSBuildCodeMetrics.Core;

namespace MSBuildCodeMetrics.Tasks
{
	/// <summary>
	/// Builder to List&lt;TaskMetric&gt;. Syntax sugar.
	/// </summary>
	public class TaskMetricList : List<TaskMetric>
	{
		/// <summary>
		/// Creates a new TaskMetricList
		/// </summary>
		/// <returns>the new TaskMetricsList</returns>
		public static TaskMetricList Create()
		{
			return new TaskMetricList();
		}

		/// <summary>
		/// Creates a new TaskMetric and adds to internal list
		/// </summary>
		/// <param name="providerName">Provider name</param>
		/// <param name="metricName">Metric name</param>
		/// <param name="ranges">Ranges</param>
		/// <param name="files">Files</param>
		/// <returns>The task metric list</returns>
		public TaskMetricList Add(string providerName, string metricName, IEnumerable<int> ranges, IEnumerable<string> files)
		{
			base.Add(new TaskMetric(providerName, metricName, ranges, files));
			return this;
		}

		/// <summary>
		/// Converts this TaskMetricsList to a MetricsList
		/// </summary>
		/// <returns>a new MetricsList</returns>
		public MetricList ToMetricList()
		{
			MetricList ml = new MetricList();
			foreach (TaskMetric t in this)			
				ml.Add(t.ProviderName, t.MetricName, t.Ranges);
			return ml;
		}

		/// <summary>
		/// Converts this TaskMetricList to a ComputeMetricsParameterList
		/// </summary>
		/// <returns>The compute metrics parameter list</returns>
		public ComputeMetricsParameterList ToComputeMetricsParameterList()
		{
			ComputeMetricsParameterList pl = new ComputeMetricsParameterList();
			foreach (TaskMetric t in this)
				pl.Add(t.ProviderName, t.MetricName, t.Files);
			return pl;
		}
	}
}
