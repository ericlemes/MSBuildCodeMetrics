using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    /// <summary>
    /// Represents a type in dotCover XML Report
    /// </summary>
    public class Type
    {
        /// <summary>
        /// Name of this type
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Represents the number of covered statements in this type
        /// </summary>
        [XmlAttribute]
        public int CoveredStatements { get; set; }

        /// <summary>
        /// Number of statements in this type
        /// </summary>
        [XmlAttribute]
        public int TotalStatements { get; set; }

        /// <summary>
        /// Represents the % of coverage in this type
        /// </summary>
        [XmlAttribute]
        public int CoveragePercent { get; set; }
    }
}
