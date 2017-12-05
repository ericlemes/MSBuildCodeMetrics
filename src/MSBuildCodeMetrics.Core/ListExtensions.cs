using System.Collections.Generic;

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
