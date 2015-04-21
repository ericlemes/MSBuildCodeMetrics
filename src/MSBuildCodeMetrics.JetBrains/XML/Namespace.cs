using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    /// <summary>
    /// Represents a namespace in a dotCover report
    /// </summary>
    public class Namespace
    {
        /// <summary>
        /// Name of this namespace
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Number of covered statements in this namespace
        /// </summary>
        [XmlAttribute]
        public int CoveredStatements { get; set; }

        /// <summary>
        /// Number of statements contained in this namespace
        /// </summary>
        [XmlAttribute]
        public int TotalStatements { get; set; }

        /// <summary>
        /// The % of coverage in this namespace
        /// </summary>
        [XmlAttribute]
        public int CoveragePercent { get; set; }

        /// <summary>
        /// Types contained in this assembly
        /// </summary>
        [XmlElement("Type")]
        public List<Type> Types { get; set; }
    }
}
