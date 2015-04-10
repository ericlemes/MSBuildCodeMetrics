using System.IO;

namespace MSBuildCodeMetrics.Core.UnitTests.Extensions
{
    public static class StringExtensions
    {
        public static MemoryStream ToMemoryStream(this string src)
        {
            var ms = new MemoryStream();
            var streamWriter = new StreamWriter(ms);
            streamWriter.Write(src);
            streamWriter.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}
