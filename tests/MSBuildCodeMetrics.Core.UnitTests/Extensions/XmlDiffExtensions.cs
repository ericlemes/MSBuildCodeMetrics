using System.IO;
using System.Xml;
using Microsoft.XmlDiffPatch;

namespace MSBuildCodeMetrics.Core.UnitTests.Extensions
{
	public static class XmlDiffExtensions
	{
		public static bool Compare(this XmlDiff xmlDiff, Stream srcStream, Stream diffStream)
		{			
			XmlTextReader src = new XmlTextReader(srcStream);
			XmlTextReader dest = new XmlTextReader(diffStream);
			xmlDiff.IgnoreWhitespace = true;
			return xmlDiff.Compare(src, dest);
		}
	}
}
