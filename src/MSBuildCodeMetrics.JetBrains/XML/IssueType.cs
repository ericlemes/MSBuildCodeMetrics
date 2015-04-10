using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{

    public class IssueType
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Category { get; set; }

        [XmlAttribute]
        public string Description { get; set; }

        [XmlAttribute]
        public string Severity { get; set; }
    }
}
