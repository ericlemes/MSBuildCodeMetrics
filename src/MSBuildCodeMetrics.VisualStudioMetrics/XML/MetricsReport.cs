using System;
using System.Collections.Generic;
using System.Linq;

namespace MSBuildCodeMetrics.VisualStudioMetrics.XML
{	
	/// <summary>
	/// Used to parse Visual Studio Metrics XML
	/// </summary>
	public class MetricsReport
	{		
		/// <summary>
		/// Maintainability index
		/// </summary>
		public int MaintainabilityIndex
		{
			get;
			set;
		}

		/// <summary>
		/// Cyclomatic complexity
		/// </summary>
		public int CyclomaticComplexity
		{
			get;
			set;
		}

		/// <summary>
		/// Class coupling
		/// </summary>
		public int ClassCoupling
		{
			get;
			set;
		}

		/// <summary>
		/// Depth of inheritance
		/// </summary>
		public int DepthOfInheritance
		{
			get;
			set;
		}

		/// <summary>
		/// Lines of code
		/// </summary>
		public int LinesOfCode
		{
			get;
			set;
		}

		private int ReturnValueOrZeroIfNull(MetricReport m)
		{
			if (m == null)
				return 0;
			return Convert.ToInt32(m.Value);
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="l">A list of MetricReports that wil be parsed to this members properties</param>
		public MetricsReport(List<MetricReport> l)
		{
            MaintainabilityIndex = ReturnValueOrZeroIfNull(l.FirstOrDefault(m => m.Name == "MaintainabilityIndex"));
            CyclomaticComplexity = ReturnValueOrZeroIfNull(l.FirstOrDefault(m => m.Name == "CyclomaticComplexity"));
            ClassCoupling = ReturnValueOrZeroIfNull(l.FirstOrDefault(m => m.Name == "ClassCoupling"));
			DepthOfInheritance = ReturnValueOrZeroIfNull(l.FirstOrDefault(m => m.Name == "DepthOfInheritance"));
            LinesOfCode = ReturnValueOrZeroIfNull(l.FirstOrDefault(m => m.Name == "LinesOfCode"));
		}
	}
}
