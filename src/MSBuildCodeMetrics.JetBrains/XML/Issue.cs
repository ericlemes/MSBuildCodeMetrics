using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    /// <summary>
    /// Represents one violation of resharper rules
    /// </summary>
    public class Issue
    {
        /// <summary>
        /// Id of the violation. Can be referenced with Id on IssueType to find severity
        /// </summary>
        [XmlAttribute]
        public string TypeId { get; set; }

        /// <summary>
        /// File where the violation happened
        /// </summary>
        [XmlAttribute]
        public string File { get; set; }

        /// <summary>
        /// Offset
        /// </summary>
        [XmlAttribute]
        public string Offset { get; set; }

        /// <summary>
        /// Line
        /// </summary>
        [XmlAttribute]
        public int Line { get; set; }

        /// <summary>
        /// Description of the violation
        /// </summary>
        [XmlAttribute]
        public string Message { get; set; }        
    }
}
