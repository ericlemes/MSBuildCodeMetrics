using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This is a builder for generic List&lt;T&gt;. Syntax sugar.
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Adds an item to the list and returns the list pointer
		/// </summary>
		/// <typeparam name="T">The list type</typeparam>
		/// <param name="list">The list</param>
		/// <param name="value">Value to add in the list</param>
		/// <returns>the list</returns>
		public static List<T> AddItem<T>(this List<T> list, T value)
		{
			list.Add(value);
			return list;
		}
	}
}
