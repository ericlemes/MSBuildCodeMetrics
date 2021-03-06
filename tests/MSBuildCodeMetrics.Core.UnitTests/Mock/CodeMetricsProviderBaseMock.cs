﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace MSBuildCodeMetrics.Core.UnitTests.Mock
{
	public abstract class CodeMetricsProviderBaseMock : ICodeMetricsProvider, IMetadataHandler
	{
        private ILogger _logger;
        public ILogger Logger
        {
            set { _logger = value; }
        }

        protected string _name;
		public string Name
		{
			get { return _name; }
		}

		protected List<string> _metrics = new List<string>();
		public IEnumerable<string> GetMetrics()
		{
			return _metrics;
		}

		protected List<ProviderMeasure> _measures = new List<ProviderMeasure>();
		public List<ProviderMeasure> Measures
		{
			get { return _measures; }
		}

		public void AddMetadata(string name, string value)
		{
			if (name == "ProviderName")
				_name = value;
			else if (name == "Data")
				ParseData(value);
			else if (name == "Metrics")
				_metrics = value.Split(';').ToList();
		}

		private CodeMetricsProviderBaseMock AddMetric(string metric)
		{
			_metrics.Add(metric);
			return this;
		}

		private CodeMetricsProviderBaseMock AddMeasure(string metric, string item, int value)
		{
			_measures.Add(new ProviderMeasure(metric, item, value));
			return this;
		}

		private void ParseData(string value)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(value);
			if (doc.DocumentElement.Name == "Metric")
			{
				AddMetric(doc.DocumentElement.GetAttribute("Name"));
				foreach (XmlNode n in doc.DocumentElement.ChildNodes)
				{
					if (n.Name == "Measure")					
						AddMeasure(doc.DocumentElement.GetAttribute("Name"),
							n.Attributes["Name"].Value, Convert.ToInt32(n.Attributes["Value"].Value));					
				}
			}			
		}
	}
}
