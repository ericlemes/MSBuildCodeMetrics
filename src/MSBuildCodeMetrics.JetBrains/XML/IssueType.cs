using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    /// <summary>
    /// Issue Type
    /// </summary>
    public class IssueType
    {
        /// <summary>
        /// Id of the issue type
        /// </summary>
        [XmlAttribute]
        public string Id { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        [XmlAttribute]
        public string Category { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [XmlAttribute]
        public string Description { get; set; }

        /// <summary>
        /// Severity
        /// </summary>
        [XmlAttribute]
        public string Severity { get; set; }
    }
}
