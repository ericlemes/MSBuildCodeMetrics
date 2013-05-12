using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	public static class ListExtensions
	{
		public static List<T> AddItem<T>(this List<T> list, T value)
		{
			list.Add(value);
			return list;
		}
	}
}
