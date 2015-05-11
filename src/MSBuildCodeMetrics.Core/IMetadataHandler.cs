namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This interface should be implemented for providers that needs metadata. 
	/// </summary>
	public interface IMetadataHandler
	{
		/// <summary>
		/// This method is called from CodeMetricsRunner for each provider, to send metadata information
		/// </summary>
		/// <param name="name">Metadata name</param>
		/// <param name="value">Metadata value</param>
		void AddMetadata(string name, string value);
	}
}
