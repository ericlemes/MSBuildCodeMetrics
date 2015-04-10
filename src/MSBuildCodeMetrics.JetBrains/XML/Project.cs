﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuildCodeMetrics.JetBrains.XML
{
    public class Project
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement("Issue")]
        public List<Issue> Issues { get; set; }
    }
}
