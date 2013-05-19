using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MSBuildCodeMetrics.Core.Providers
{
	/// <summary>
	/// Implements the File LOC Metric
	/// </summary>
	/// <remarks>
	/// This expects a metadata named "Files" and expects a list of extension and labels in the format: .cs=C# Sources;.csproj=C# Projects
	/// </remarks>
	public class CountLOCProvider : IMultiFileCodeMetricsProvider, IMetadataHandler
	{
		private const string CommentDblSlash = "//";
		private const string CommentSingleQuote = "'";
		private const string CommentSlashStar = "/*";
		private const string CommentStarSlash = "*/";
		private Dictionary<string, string> _extensions = new Dictionary<string, string>();
		private IFileStreamFactory _fileStreamFactory;

		/// <summary>
		/// Provider name (LOC)
		/// </summary>
		public string Name
		{
			get { return "LOC"; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public CountLOCProvider()
		{
			_fileStreamFactory = new FileStreamFactory();
		}

		/// <summary>
		/// Constructor used to inject dependencies
		/// </summary>
		/// <param name="fileStreamFactory">The file stream factory used to open files</param>
		public CountLOCProvider(IFileStreamFactory fileStreamFactory)
		{
			_fileStreamFactory = fileStreamFactory;
		}

		/// <summary>
		/// Get Metrics computed by this provider: CodeLOC, EmptyLOC, CommentLOC
		/// </summary>
		/// <returns>A set of metrics</returns>
		public IEnumerable<string> GetMetrics()
		{
			return new List<string>().AddItem("CodeLOC").AddItem("EmptyLOC").AddItem("CommentLOC");
		}

		/// <summary>
		/// Adds metadata to the provider
		/// </summary>
		/// <param name="name">Metadata name</param>
		/// <param name="value">Metadata value</param>
		public void AddMetadata(string name, string value)
		{
			if (name == "FileTypes")
			{
				string[] items = value.Split(';');
				foreach (string item in items)
				{
					string[] pair = item.Split('=');
					if (pair.Length != 2)
						throw new Exception("Value must be in format key=value");
					_extensions.Add(pair[0], pair[1]);
				}
			}
		}

		private FileLOCCount CountFile(string fileName)
		{
			int fileLineCount = 0;
			int fileCommentLineCount = 0;
			int fileEmptyLineCount = 0;
			bool inComment = false;

			Stream stream = _fileStreamFactory.OpenFile(fileName);
			StreamReader sr = new StreamReader(stream);

			using (sr)
			{
				while (sr.Peek() != -1)
				{
					string line = sr.ReadLine().Trim();
					fileLineCount++;

					if (line.Length == 0)
						fileEmptyLineCount++;						

				if (line.StartsWith(CommentSlashStar, StringComparison.OrdinalIgnoreCase))
					{
						fileCommentLineCount++;
						if (line.IndexOf(CommentStarSlash, StringComparison.OrdinalIgnoreCase) == -1)
						{
							inComment = true;
						}
					}
					else if (inComment)
					{
						fileCommentLineCount++;						
						if (line.IndexOf(CommentStarSlash, StringComparison.OrdinalIgnoreCase) != -1)						
							inComment = false;						
					}
					else if (line.StartsWith(CommentDblSlash, StringComparison.OrdinalIgnoreCase))
					{
						fileCommentLineCount++;
					}
					else if (line.StartsWith(CommentSingleQuote, StringComparison.OrdinalIgnoreCase))
					{
						fileCommentLineCount++;
					}							
				}				
				sr.Close();
			}

			return new FileLOCCount(fileCommentLineCount, fileEmptyLineCount, fileLineCount);
		}

		/// <summary>
		/// Compute metrics
		/// </summary>
		/// <param name="metricsToCompute">The input metrics</param>
		/// <param name="files">The files</param>
		/// <returns>a set of measures</returns>
		public IEnumerable<ProviderMeasure> ComputeMetrics(IEnumerable<string> metricsToCompute, IEnumerable<string> files)
		{
			if (_extensions.Count <= 0)
				throw new Exception("Extensions must be informed as metadata");

			Dictionary<string, FileLOCCount> counter = new Dictionary<string, FileLOCCount>();
			CountAllFiles(files, counter);
			return GenerateMeasures(counter);
		}

		private List<ProviderMeasure> GenerateMeasures(Dictionary<string, FileLOCCount> counter)
		{
			List<ProviderMeasure> result = new List<ProviderMeasure>();
			foreach (KeyValuePair<string, FileLOCCount> p in counter)
			{
				result.Add(new ProviderMeasure("TotalLOC", p.Key, p.Value.TotalLineCount));
				result.Add(new ProviderMeasure("CodeLOC", p.Key, p.Value.CodeLineCount));
				result.Add(new ProviderMeasure("EmptyLOC", p.Key, p.Value.EmptyLineCount));
				result.Add(new ProviderMeasure("CommentLOC", p.Key, p.Value.CommentLineCount));
			}
			return result;
		}

		private void CountAllFiles(IEnumerable<string> files, Dictionary<string, FileLOCCount> counter)
		{
			foreach (string fileName in files)
			{
				FileInfo fi = new FileInfo(fileName);
				if (!_extensions.ContainsKey(fi.Extension.ToLower()))
					continue;

				if (!counter.ContainsKey(_extensions[fi.Extension.ToLower()]))
					counter.Add(_extensions[fi.Extension.ToLower()], new FileLOCCount(0, 0, 0));

				counter[_extensions[fi.Extension.ToLower()]].Sum(CountFile(fileName));
			}
		}
	}
}
