using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	public class RangeList : List<int>
	{
		public static RangeList Create()
		{
			return new RangeList();
		}

		public new RangeList Add(int range)
		{
			base.Add(range);
			return this;
		}
	}

}
