using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.VisualStudioCodeMetrics.XML
{	
	public class MetricsReport
	{		
		public int MaintainabilityIndex
		{
			get;
			set;
		}

		public int CyclomaticComplexity
		{
			get;
			set;
		}

		public int ClassCoupling
		{
			get;
			set;
		}

		public int DepthOfInheritance
		{
			get;
			set;
		}

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
