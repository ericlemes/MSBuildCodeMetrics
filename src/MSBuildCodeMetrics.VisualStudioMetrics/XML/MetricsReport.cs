using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			MaintainabilityIndex = ReturnValueOrZeroIfNull(l.Where(m => m.Name == "MaintainabilityIndex").FirstOrDefault());
			CyclomaticComplexity = ReturnValueOrZeroIfNull(l.Where(m => m.Name == "CyclomaticComplexity").FirstOrDefault());
			ClassCoupling = ReturnValueOrZeroIfNull(l.Where(m => m.Name == "ClassCoupling").FirstOrDefault());
			DepthOfInheritance = ReturnValueOrZeroIfNull(l.Where(m => m.Name == "DepthOfInheritance").FirstOrDefault());
			LinesOfCode = ReturnValueOrZeroIfNull(l.Where(m => m.Name == "LinesOfCode").FirstOrDefault());
		}
	}
}
