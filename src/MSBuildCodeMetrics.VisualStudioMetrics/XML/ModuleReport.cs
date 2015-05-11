using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.VisualStudioMetrics.XML
{
	/// <summary>
	/// Used to parse Visual Studio Metrics XML
	/// </summary>
	[XmlType("Module")]
	public class ModuleReport
	{
		/// <summary>
		/// Name
		/// </summary>
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Assembly version
		/// </summary>
		[XmlAttribute]
		public string AssemblyVersion
		{
			get;
			set;
		}

		/// <summary>
		/// File version
		/// </summary>
		[XmlAttribute]
		public string FileVersion
		{
			get;
			set;
		}
		
		/// <summary>
		/// Metrics list
		/// </summary>
		[XmlArray("Metrics")]
		public List<MetricReport> MetricsList
		{
			get;
			set;
		}
		
		private MetricsReport _metrics;
		/// <summary>
		/// Metrics
		/// </summary>
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

		/// <summary>
		/// Namespace
		/// </summary>
		public List<NamespaceReport> Namespaces
		{
			get;
			set;
		}

		/// <summary>
		/// Returns a list o all members (the lowest level of the tree)
		/// </summary>
		/// <returns>A list of all members</returns>
		public List<MemberReport> GetAllMembers()
		{
			List<MemberReport> allMembers = new List<MemberReport>();
			foreach (NamespaceReport nr in Namespaces)
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
