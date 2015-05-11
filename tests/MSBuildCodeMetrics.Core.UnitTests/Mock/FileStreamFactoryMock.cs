using System.Collections.Generic;
using System.IO;

namespace MSBuildCodeMetrics.Core.UnitTests.Mock
{
	public class FileStreamFactoryMock : IFileStreamFactory
	{
		private readonly Dictionary<string, Stream> _fileMocks = new Dictionary<string, Stream>();
		private Stream _outputStream;
		public Stream OutputStream
		{
			get { return _outputStream; }			
		}

		public Stream GetStreamForTempFile(string tempFileName)
		{
			return new MemoryStream();
		}

		public Stream CreateStream(string outputReportFileName)
		{
			_outputStream = new MemoryStream();
			return _outputStream;
		}

		public void CloseStream(Stream outputReportStream)
		{
			//Do nothing because I use memory streams. 
		}

		public Stream OpenFile(string fileName)
		{
			return _fileMocks[fileName];
		}

		public void AddFileMock(string fileName, string fileContent)
		{
			MemoryStream ms = new MemoryStream();
			StreamWriter sw = new StreamWriter(ms);
			sw.Write(fileContent);
			sw.Flush();
			ms.Seek(0, SeekOrigin.Begin);
			_fileMocks.Add(fileName, ms);
		}
	}
}
