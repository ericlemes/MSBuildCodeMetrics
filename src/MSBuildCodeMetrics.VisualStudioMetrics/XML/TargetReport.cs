using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.VisualStudioMetrics.XML
{	
	/// <summary>
	/// Used to parse Visual Studio Metrics XML
	/// </summary>
	[XmlType("Target")]
	public class TargetReport
	{
		/// <summary>
		/// Name
		/// </summary>
		public string Name
		{
			get;
			set;
		}
	
		/// <summary>
		/// Modules
		/// </summary>
		public List<ModuleReport> Modules
		{
			get;
			set;
		}
	}
}
