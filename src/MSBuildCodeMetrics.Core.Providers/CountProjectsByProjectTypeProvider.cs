using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace MSBuildCodeMetrics.Core.Providers
{
	public class CountProjectsByProjectTypeProvider : IMultiFileCodeMetricsProvider
	{
		private IFileStreamFactory _fileStreamFactory;

		public string Name
		{
			get { return "CountProjectsByProjectTypeProvider"; }
		}

		public CountProjectsByProjectTypeProvider()
		{
			_fileStreamFactory = new FileStreamFactory();
		}

		public CountProjectsByProjectTypeProvider(IFileStreamFactory fileStreamFactory)
		{
			_fileStreamFactory = fileStreamFactory;
		}

		public IEnumerable<string> GetMetrics()
		{
			return new List<string>().AddItem("ProjectTypeCount");
		}

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

		public static string GetProjectType(Stream st)
		{
			XmlTextReader rd = new XmlTextReader(st);
			using (rd)
			{
				if (!rd.IsStartElement())
					return "";
				rd.ReadStartElement("Project");
				rd.ReadToNextSibling("PropertyGroup");
				rd.ReadStartElement("PropertyGroup");
				rd.ReadToNextSibling("OutputType");
				return rd.ReadString();
			}
		}

	}
}
