namespace MSBuildCodeMetrics.Core.Ranges
{
	/// <summary>
	/// This class is used to represent the greater range in the summary report.
	/// </summary>
	public class GreaterThanRangeType : IRangeType
	{
		private readonly int _value;
		/// <summary>
		/// The value
		/// </summary>
		public int Value
		{
			get { return _value; }
		}

		/// <summary>
		/// The name of this range, readable by humans.
		/// </summary>
		public string Name
		{
			get { return "> " + _value.ToString(); }
		}

		/// <summary>
		/// Creates a new range
		/// </summary>
		/// <param name="value">value</param>
		public GreaterThanRangeType(int value)
		{
			_value = value;
		}
	}
}
