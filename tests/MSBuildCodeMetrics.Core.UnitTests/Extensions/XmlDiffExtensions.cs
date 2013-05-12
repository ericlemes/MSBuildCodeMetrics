using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.XmlDiffPatch;
using System.IO;
using System.Xml;

namespace Microsoft.XmlDiffPatch
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
