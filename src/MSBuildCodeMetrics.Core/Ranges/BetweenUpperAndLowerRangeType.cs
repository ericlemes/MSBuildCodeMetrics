using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core.Ranges
{
	public class BetweenUpperAndLowerRangeType : IRangeType
	{
		public string Name
		{
			get { return "<= " + _upperValue.ToString() + " and > " + _lowerValue.ToString(); }
		}

		private int _upperValue;
		public int UpperValue
		{
			get { return _upperValue; }
		}		

		private int _lowerValue;
		public int LowerValue
		{
			get { return _lowerValue; }
		}

		public BetweenUpperAndLowerRangeType(int upperValue, int lowerValue)
		{
			_upperValue = upperValue;
			_lowerValue = lowerValue;
		}
	}
}
