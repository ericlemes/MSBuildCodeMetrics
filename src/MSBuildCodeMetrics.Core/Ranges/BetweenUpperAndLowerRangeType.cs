using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core.Ranges
{
	/// <summary>
	/// This class represents the inner range of summary reports.
	/// </summary>
	public class BetweenUpperAndLowerRangeType : IRangeType
	{
		/// <summary>
		/// Name of the range, readable by humans
		/// </summary>
		public string Name
		{
			get { return "<= " + _upperValue.ToString() + " and > " + _lowerValue.ToString(); }
		}

		private int _upperValue;
		/// <summary>
		/// The upper value of the range
		/// </summary>
		public int UpperValue
		{
			get { return _upperValue; }
		}		

		private int _lowerValue;
		/// <summary>
		/// The lower value of the range
		/// </summary>
		public int LowerValue
		{
			get { return _lowerValue; }
		}

		/// <summary>
		/// Creates a new range
		/// </summary>
		/// <param name="upperValue">Upper value</param>
		/// <param name="lowerValue">Lower value</param>
		public BetweenUpperAndLowerRangeType(int upperValue, int lowerValue)
		{
			_upperValue = upperValue;
			_lowerValue = lowerValue;
		}
	}
}
