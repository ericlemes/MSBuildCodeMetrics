using System.Collections.Generic;

namespace MSBuildCodeMetrics.Core
{
	public interface ICodeMetricsProvider
	{
		string Name
		{
			get;
		}		

		IEnumerable<string> GetMetrics();

        ILogger Logger { set; }
	}
}
