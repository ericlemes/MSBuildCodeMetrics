using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MSBuildCodeMetrics.Core;

namespace MSBuildCodeMetrics.Core
{
	public class FileStreamFactory : IFileStreamFactory
	{
		public Stream GetStreamForOutputReport(string outputReportFileName)
		{
			return new FileStream(outputReportFileName, FileMode.Create, FileAccess.ReadWrite);
		}

		public void CloseStream(System.IO.Stream outputReportStream)
		{
			outputReportStream.Close();
		}

		public Stream OpenFile(string fileName)
		{
			return new FileStream(fileName, FileMode.Open, FileAccess.Read);
		}
	}
}
