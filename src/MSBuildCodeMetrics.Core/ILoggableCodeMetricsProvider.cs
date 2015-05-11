﻿namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This interface must be implemented for each provider that needs to log its activity. 
	/// </summary>
	public interface ILoggableCodeMetricsProvider
	{
	    /// <summary>
	    /// This method is called from CodeMetricsRunner to send the logger to the providers.
	    /// </summary>	    
	    ILogger Logger { set; }	
	}
}
