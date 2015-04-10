using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This interface must be implemented for each provider that needs to log its activity. 
	/// </summary>
	public interface ILoggableCodeMetricsProvider
	{
	    /// <summary>
	    /// This method is called from CodeMetricsRunner to send the logger to the providers.
	    /// </summary>
	    /// <param name="logger">The logger</param>
	    ILogger Logger { set; }	
	}
}
