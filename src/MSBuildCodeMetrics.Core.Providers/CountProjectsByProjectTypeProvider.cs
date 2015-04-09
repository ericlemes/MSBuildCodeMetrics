using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace MSBuildCodeMetrics.Core.Providers
{
	/// <summary>
	/// Implements a metric that computes number of project by project type
	/// </summary>
	public class CountProjectsByProjectTypeProvider : IMultiFileCodeMetricsProvider
	{
		private IFileStreamFactory _fileStreamFactory;

		/// <summary>
		/// Name of the provider (CountProjectsByProjectTypeProvider)
		/// </summary>
		public string Name
		{
			get { return "CountProjectsByProjectTypeProvider"; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public CountProjectsByProjectTypeProvider()
		{
			_fileStreamFactory = new FileStreamFactory();
		}

		/// <summary>
		/// Constructor used to inject dependencies
		/// </summary>
		/// <param name="fileStreamFactory">The file stream factory used to open files</param>
		public CountProjectsByProjectTypeProvider(IFileStreamFactory fileStreamFactory)
		{
			_fileStreamFactory = fileStreamFactory;
		}

		/// <summary>
		/// Get metrics computed by this provider
		/// </summary>
		/// <returns>set of metrics</returns>
		public IEnumerable<string> GetMetrics()
		{
			return new List<string>().AddItem("ProjectTypeCount");
		}

		/// <summary>
		/// Compute metrics
		/// </summary>
		/// <param name="metricsToCompute">Metrics</param>
		/// <param name="files">Files</param>
		/// <returns>set of measures</returns>
		public IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, IEnumerable<string> files)
		{
			Dictionary<string, int> counter = new Dictionary<string, int>();

			foreach (string fileName in files)
			{
				Stream fileStream = _fileStreamFactory.OpenFile(fileName);
				string projectType = GetProjectType(fileStream);

				if (!counter.ContainsKey(projectType))
					counter.Add(projectType, 0);
				counter[projectType] += 1;
			}

			return GenerateMeasures(counter);
		}

		private static List<ProviderMeasure> GenerateMeasures(Dictionary<string, int> counter)
		{
			List<ProviderMeasure> result = new List<ProviderMeasure>();
			foreach (KeyValuePair<string, int> p in counter)
				result.Add(new ProviderMeasure("ProjectTypeCount", p.Key, p.Value));
			return result;
		}

		/// <summary>
		/// Parses the input stream and return the associated project type. The stream should point to a project file (csproj or vbproj)
		/// </summary>
		/// <param name="st">The stream</param>
		/// <returns>the project type</returns>
		public static string GetProjectType(Stream st)
		{
			XmlTextReader rd = new XmlTextReader(st);
			using (rd)
			{
				rd.ReadStartElement("Project");
				rd.ReadToNextSibling("PropertyGroup");
				rd.ReadStartElement("PropertyGroup");
				rd.ReadToNextSibling("OutputType");
				return rd.ReadString();
			}
		}

	}
}
