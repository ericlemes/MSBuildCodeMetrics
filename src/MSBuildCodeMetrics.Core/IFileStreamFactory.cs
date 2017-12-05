using System.IO;

namespace MSBuildCodeMetrics.Core
{
	public interface IFileStreamFactory
	{		
		Stream CreateStream(string fileName);

		void CloseStream(Stream stream);

		Stream OpenFile(string fileName);
	}
}
