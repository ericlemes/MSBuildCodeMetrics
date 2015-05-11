namespace MSBuildCodeMetrics.Core.Ranges
{	
	/// <summary>
	/// This interface is an abstraction for Ranges used in the summary report.
	/// </summary>
	public interface IRangeType
	{
		/// <summary>
		/// The name of the range, readable by humans
		/// </summary>
		string Name
		{
			get;			
		}
	}
}
