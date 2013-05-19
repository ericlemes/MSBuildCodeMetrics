using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

namespace MSBuildCodeMetrics.VisualStudioMetrics.XML
{
	/// <summary>
	/// Used to parse Visual Studio Metrics XML
	/// </summary>
	public class CodeMetricsReport
	{
		/// <summary>
		/// Version
		/// </summary>
		public string Version
		{
			get;
			set;
		}

		/// <summary>
		/// Targets
		/// </summary>
		public List<TargetReport> Targets
		{
			get;
			set;
		}

		/// <summary>
		/// Deserialize the report
		/// </summary>
		/// <param name="inputStream">input stream</param>
		/// <returns>new ModuleReport</returns>
		public static ModuleReport Deserialize(Stream inputStream)
		{
			XmlSerializer s = new XmlSerializer(typeof(CodeMetricsReport));
			object o = s.Deserialize(inputStream);
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
