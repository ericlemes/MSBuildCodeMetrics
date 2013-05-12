using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MSBuildCodeMetrics.Core
{
	public interface IFileStreamFactory
	{		
		Stream GetStreamForOutputReport(string outputReportFileName);
		void CloseStream(Stream outputReportStream);
		Stream OpenFile(string fileName);
	}
}
