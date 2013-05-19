using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This is the most important interface for MSBuildCodeMetrics. Each new provider must implement this interface. There's also the
	/// ISingleFileCodeMetricsProvider and IMultiFileCodeMetricsProvider that extends this interface behavior.
	/// </summary>
	public interface ICodeMetricsProvider
	{
		/// <summary>
		/// Name of the provider
		/// </summary>
		string Name
		{
			get;
		}		
		/// <summary>
		/// Returns the metrics that the provider knows how to compute.
		/// </summary>
		/// <returns>A set of metrics</returns>
		IEnumerable<string> GetMetrics();				
	}
}
