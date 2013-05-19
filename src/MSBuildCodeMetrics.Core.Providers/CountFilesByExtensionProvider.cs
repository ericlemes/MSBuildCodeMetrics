using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MSBuildCodeMetrics.Core.Providers
{
	/// <summary>
	/// Implements a metric that computes how many files existing for each extension.
	/// </summary>
	public class CountFilesByExtensionProvider : IMultiFileCodeMetricsProvider
	{
		/// <summary>
		/// Name of the provider (CountFilesByExtension)
		/// </summary>
		public string Name
		{
			get { return "CountFilesByExtension"; }
		}

		/// <summary>
		/// Returns metrics computed by this provider: CountFilesByExtension
		/// </summary>
		/// <returns>A set of metrics</returns>
		public IEnumerable<string> GetMetrics()
		{
			List<string> l = new List<string>();
			l.Add("CountFilesByExtension");
			return l;
		}

		/// <summary>
		/// Compute metrics 
		/// </summary>
		/// <param name="metricsToCompute">list of metrics to compute</param>
		/// <param name="files">files to parse</param>
		/// <returns>a set of measures</returns>
		public IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, IEnumerable<string> files)
		{
			Dictionary<string, int> counter = CountFilesByExtension(files);
			List<ProviderMeasure> result = GenerateMeasures(counter);
			return result;
		}

		private static List<ProviderMeasure> GenerateMeasures(Dictionary<string, int> counter)
		{
			List<ProviderMeasure> result = new List<ProviderMeasure>();
			foreach (KeyValuePair<string, int> p in counter)
				result.Add(new ProviderMeasure("CountFilesByExtension", p.Key, p.Value));
			return result.OrderByDescending(m => m.Value).ToList<ProviderMeasure>();
		}

		private Dictionary<string, int> CountFilesByExtension(IEnumerable<string> files)
		{
			Dictionary<string, int> counter = new Dictionary<string, int>();
			foreach (string fileName in files)
			{
				FileInfo fi = new FileInfo(fileName);
				if (!counter.ContainsKey(fi.Extension))
					counter.Add(fi.Extension, 0);
				counter[fi.Extension] += 1;
			}
			return counter;
		}
	}
}
