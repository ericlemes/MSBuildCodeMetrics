using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    /// <summary>
    /// Represents a dotCover XML report
    /// </summary>
    [XmlType("Root")]
    public class DotCoverReport
    {
        /// <summary>
        /// Assemblies contained in this report
        /// </summary>
        [XmlElement("Assembly")]
        public List<Assembly> Assemblies { get; set; }

        /// <summary>
        /// Deserializes a stream in the DotCoverReport format
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static DotCoverReport Deserialize(Stream stream)
        {
            XmlSerializer s = new XmlSerializer(typeof(DotCoverReport));
            object o = s.Deserialize(stream);
            return (DotCoverReport)o;      
        }
    }
}
