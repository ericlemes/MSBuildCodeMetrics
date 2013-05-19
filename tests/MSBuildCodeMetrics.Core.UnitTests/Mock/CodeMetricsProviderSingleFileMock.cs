﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MSBuildCodeMetrics.Core.UnitTests.Mock
{
	public class CodeMetricsProviderSingleFileMock : CodeMetricsProviderBaseMock, ISingleFileCodeMetricsProvider
	{
		
		public static CodeMetricsProviderSingleFileMock Create(string name)
		{
			return new CodeMetricsProviderSingleFileMock(name);
		}

		
		public IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, string fileName)
		{
			return
				(from ProviderMeasure m in Measures
				 where metricsToCompute.Contains(m.MetricName)
				 select m);
		}

		public CodeMetricsProviderSingleFileMock()
		{
		}

		public CodeMetricsProviderSingleFileMock(string name)
		{
			_name = name;
		}

		public CodeMetricsProviderSingleFileMock AddMetric(string metric)
		{
			_metrics.Add(metric);
			return this;
		}

		public CodeMetricsProviderSingleFileMock AddMeasure(string metric, string item, int value)
		{
			_measures.Add(new ProviderMeasure(metric, item, value));
			return this;
		}


	}
}