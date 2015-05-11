using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    /// <summary>
    /// Represents an assembly in dotCover report
    /// </summary>
    public class Assembly
    {
        /// <summary>
        /// Name of this assembly
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Number of covered statements in this assembly
        /// </summary>
        [XmlAttribute]
        public int CoveredStatements { get; set; }

        /// <summary>
        /// Number of statements contained in this assembly
        /// </summary>
        [XmlAttribute]
        public int TotalStatements { get; set; }

        /// <summary>
        /// The % of coverage in this assembly
        /// </summary>
        [XmlAttribute]
        public int CoveragePercent { get; set; }

        /// <summary>
        /// List of namespaces contained in this assembly
        /// </summary>
        [XmlElement("Namespace")]
        public List<Namespace> Namespaces { get; set; }
    }
}
