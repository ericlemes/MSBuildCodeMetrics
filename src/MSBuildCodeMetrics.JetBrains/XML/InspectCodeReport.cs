using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    [XmlType("Report")]
    public class InspectCodeReport
    {        
        public List<IssueType> IssueTypes
        {
            get; set;
        }

        public List<Project> Issues
        {
            get; set;
        }

        public static InspectCodeReport Deserialize(Stream stream)
        {
            XmlSerializer s = new XmlSerializer(typeof(InspectCodeReport));
            object o = s.Deserialize(stream);
            return (InspectCodeReport) o;            
        }
    }
}
