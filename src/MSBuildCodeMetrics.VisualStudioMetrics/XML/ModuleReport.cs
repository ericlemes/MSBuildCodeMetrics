using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.VisualStudioCodeMetrics.XML
{
	[XmlType("Module")]
	public class ModuleReport
	{
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string AssemblyVersion
		{
			get;
			set;
		}

		[XmlAttribute]
		public string FileVersion
		{
			get;
			set;
		}
		
		[XmlArray("Metrics")]
		public List<MetricReport> MetricsList
		{
			get;
			set;
		}
		
		private MetricsReport _metrics;
		[XmlIgnore]
		public MetricsReport Metrics
		{
			get
			{
				if (_metrics == null)
					_metrics = new MetricsReport(MetricsList);
				return _metrics;
			}
		}

		public List<NamespaceReport> Namespaces
		{
			get;
			set;
		}

		public List<MemberReport> GetAllMembers()
		{
			List<MemberReport> allMembers = new List<MemberReport>();
			foreach (NamespaceReport nr in this.Namespaces)
			{
				foreach (TypeReport tr in nr.Types)
				{
					foreach (MemberReport mr in tr.Members)
						allMembers.Add(mr);
				}
			}
			return allMembers;
		}
	}
}
