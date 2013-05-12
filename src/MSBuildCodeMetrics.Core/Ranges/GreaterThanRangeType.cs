using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core.Ranges
{
	public class GreaterThanRangeType : IRangeType
	{
		private int _value;
		public int Value
		{
			get { return _value; }
		}

		public string Name
		{
			get { return "> " + _value.ToString(); }
		}

		public GreaterThanRangeType(int value)
		{
			_value = value;
		}
	}
}
