using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core.Ranges
{
	/// <summary>
	/// This class represents the lower range of the summary report.
	/// </summary>
	public class LowerOrEqualRangeType : IRangeType
	{
		private int _value;
		/// <summary>
		/// The value
		/// </summary>
		public int Value
		{
			get { return _value; }
		}

		/// <summary>
		/// The name of the range, readable by humans
		/// </summary>
		public string Name
		{
			get { return "<= " + _value.ToString(); }
		}

		/// <summary>
		/// Creates a new range
		/// </summary>
		/// <param name="value">The value</param>
		public LowerOrEqualRangeType(int value)
		{
			_value = value;
		}
	}
}
