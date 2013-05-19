using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This class is just a Builder for List&lt;int&gt;. Syntax sugar.
	/// </summary>
	public class RangeList : List<int>
	{
		/// <summary>
		/// Creates a new RangeList instance and returns it
		/// </summary>
		/// <returns>The new range list</returns>
		public static RangeList Create()
		{
			return new RangeList();
		}

		/// <summary>
		/// Adds a new range.
		/// </summary>
		/// <param name="range">The range</param>
		/// <returns>Range list</returns>
		public new RangeList Add(int range)
		{
			base.Add(range);
			return this;
		}
	}

}
