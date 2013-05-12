using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSBuildCodeMetrics.Core.Ranges;

namespace MSBuildCodeMetrics.Core
{	
	internal class SummaryReportRange
	{
		private IRangeType _range;
		public IRangeType Range
		{
			get { return _range; }
		}

		private int _count;
		public int Count
		{
			get { return _count; }
		}

		public SummaryReportRange(IRangeType range, int methodCount)
		{
			_range = range;
			_count = methodCount;
		}
	}
}
