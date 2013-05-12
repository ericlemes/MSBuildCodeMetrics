using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.VisualStudioCodeMetrics.XML
{	
	[XmlType("Target")]
	public class TargetReport
	{
		public string Name
		{
			get;
			set;
		}
	
		public List<ModuleReport> Modules
		{
			get;
			set;
		}
	}
}
