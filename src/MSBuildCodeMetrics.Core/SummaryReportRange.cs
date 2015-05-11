using MSBuildCodeMetrics.Core.Ranges;

namespace MSBuildCodeMetrics.Core
{	
	internal class SummaryReportRange
	{
		private readonly IRangeType _range;
		public IRangeType Range
		{
			get { return _range; }
		}

		private readonly int _count;
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
