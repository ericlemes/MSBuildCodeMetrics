﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This class represents one measure generated by one code metrics provider. 
	/// </summary>
	public class ProviderMeasure
	{
		private string _metricName;
		/// <summary>
		/// The metric name
		/// </summary>
		public string MetricName
		{
			get { return _metricName; }
		}


		private string _measureName;
		/// <summary>
		/// The measure name. The item that have had its measure computed.
		/// </summary>
		public string MeasureName
		{
			get { return _measureName; }
		}

		private int _value;
		/// <summary>
		/// The value of the measure
		/// </summary>
		public int Value
		{
			get { return _value; }
		}

		/// <summary>
		/// Creates a new provider measure
		/// </summary>
		/// <param name="metricName">The metric</param>
		/// <param name="itemName">The measure or item</param>
		/// <param name="value">The value</param>
		public ProviderMeasure(string metricName, string itemName, int value)
		{
			_metricName = metricName;
			_measureName = itemName;
			_value = value;
		}
	}
}
