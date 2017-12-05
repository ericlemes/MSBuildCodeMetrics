using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;

namespace MSBuildCodeMetrics.Core.Providers
{
	public class CountProjectsByProjectTypeProvider : IMultiFileCodeMetricsProvider
	{
		private readonly IFileStreamFactory _fileStreamFactory;

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

        private ILogger _logger;
        public ILogger Logger
        {
            set { _logger = value; }
        }

        public IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, IEnumerable<string> files)
		{
			Dictionary<string, int> counter = new Dictionary<string, int>();

			foreach (string fileName in files)
			{
				Stream fileStream = _fileStreamFactory.OpenFile(fileName);
				string projectType = GetProjectType(fileName, fileStream);

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

		public string GetProjectType(string fileName, Stream st)
		{
            try
            {
                XmlTextReader rd = new XmlTextReader(st);
                using (rd)
                {
                    rd.ReadStartElement("Project");
                    rd.ReadToNextSibling("PropertyGroup");
                    rd.ReadStartElement("PropertyGroup");
                    rd.ReadToNextSibling("OutputType");
                    string projectType = rd.ReadString();

                    if (string.IsNullOrEmpty(projectType))
                        throw new Exception("Unknown project type");

                    return projectType;
                }
            }
            catch
            {
                _logger.LogMessage("Unknown project type for file: " + fileName);
                return "Unknown";
            }
		}

	}
}
