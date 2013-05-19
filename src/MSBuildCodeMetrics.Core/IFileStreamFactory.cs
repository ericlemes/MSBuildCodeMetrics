using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MSBuildCodeMetrics.Core
{
	/// <summary>
	/// This interface is used for dependency inversion. It helps the testability of the task. 
	/// </summary>
	public interface IFileStreamFactory
	{		
		/// <summary>
		/// Creates a new stream for the specified file name
		/// </summary>
		/// <param name="fileName">The name of the file to create the stream</param>
		/// <returns>The new stream</returns>		
		Stream CreateStream(string fileName);

		/// <summary>
		/// Close the stream
		/// </summary>
		/// <param name="stream">Stream to close</param>
		void CloseStream(Stream stream);

		/// <summary>
		/// Open the file and return the associated file stream.
		/// </summary>
		/// <param name="fileName">The file name</param>
		/// <returns>The stream</returns>
		Stream OpenFile(string fileName);
	}
}
