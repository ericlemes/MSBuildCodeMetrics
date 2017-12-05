using System.IO;

namespace MSBuildCodeMetrics.Core
{
	public class FileStreamFactory : IFileStreamFactory
	{
		public Stream CreateStream(string fileName)
		{
			return new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
		}

		public void CloseStream(Stream outputReportStream)
		{
			outputReportStream.Close();
		}

		public Stream OpenFile(string fileName)
		{
			return new FileStream(fileName, FileMode.Open, FileAccess.Read);
		}
	}
}
