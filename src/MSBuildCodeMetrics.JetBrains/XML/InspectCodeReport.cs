using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    /// <summary>
    /// Class for XML serialization of InspectCode output
    /// </summary>
    [XmlType("Report")]
    public class InspectCodeReport
    {        
        /// <summary>
        /// Issue types
        /// </summary>
        public List<IssueType> IssueTypes
        {
            get; set;
        }

        /// <summary>
        /// Projects with issues
        /// </summary>
        public List<Project> Issues
        {
            get; set;
        }

        /// <summary>
        /// Deserializes a stream, expecting a InspectCode output file in the stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static InspectCodeReport Deserialize(Stream stream)
        {
            XmlSerializer s = new XmlSerializer(typeof(InspectCodeReport));
            object o = s.Deserialize(stream);
            return (InspectCodeReport) o;            
        }
    }
}
