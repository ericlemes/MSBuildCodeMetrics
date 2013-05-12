using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

namespace MSBuildCodeMetrics.VisualStudioCodeMetrics.XML
{
	public class CodeMetricsReport
	{
		public string Version
		{
			get;
			set;
		}

		public List<TargetReport> Targets
		{
			get;
			set;
		}

		public static ModuleReport Deserialize(Stream metricsOutputStream)
		{
			XmlSerializer s = new XmlSerializer(typeof(CodeMetricsReport));
			object o = s.Deserialize(metricsOutputStream);
			ModuleReport result = ((CodeMetricsReport)o).Targets[0].Modules[0];
			CreateParentRelationship(result);
			return result;
		}

		private static void CreateParentRelationship(ModuleReport result)
		{
			foreach (NamespaceReport n in result.Namespaces)
			{
				n.Module = result;
				foreach (TypeReport t in n.Types)
				{
					t.Namespace = n;
					foreach (MemberReport m in t.Members)
						m.Type = t;
				}
			}
		}
	}
}
