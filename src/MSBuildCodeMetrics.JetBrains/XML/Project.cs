using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    /// <summary>
    /// Represents one project with violations
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// List of issues of this project
        /// </summary>
        [XmlElement("Issue")]
        public List<Issue> Issues { get; set; }
    }
}
