using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    public class Issue
    {
        [XmlAttribute]
        public string TypeId { get; set; }

        [XmlAttribute]
        public string File { get; set; }

        [XmlAttribute]
        public string Offset { get; set; }

        [XmlAttribute]
        public int Line { get; set; }

        [XmlAttribute]
        public string Message { get; set; }        
    }
}
