using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MSBuildCodeMetrics.Core;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This class is a File Stream implementation for FileStreamFactory. It is the default implementation for the providers.
	/// </summary>
	public class FileStreamFactory : IFileStreamFactory
	{
		/// <summary>
		/// Creates a new file stream, associated with a new file
		/// </summary>
		/// <param name="fileName">file name</param>
		/// <returns>The new stream</returns>
		public Stream CreateStream(string fileName)
		{
			return new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
		}

		/// <summary>
		/// Closes the specified stream
		/// </summary>
		/// <param name="outputReportStream">The stream</param>
		public void CloseStream(System.IO.Stream outputReportStream)
		{
			outputReportStream.Close();
		}

		/// <summary>
		/// Open the specified existing file, for read.
		/// </summary>
		/// <param name="fileName">The file</param>
		/// <returns>Stream</returns>
		public Stream OpenFile(string fileName)
		{
			return new FileStream(fileName, FileMode.Open, FileAccess.Read);
		}
	}
}
