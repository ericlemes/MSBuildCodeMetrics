﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSBuildCodeMetrics.Core.UnitTests.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class TestResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TestResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MSBuildCodeMetrics.Core.UnitTests.Resources.TestResources", typeof(TestResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;CodeMetricsReport xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xmlns:xsd=&quot;http://www.w3.org/2001/XMLSchema&quot;&gt;
        ///	&lt;Summary&gt;
        ///		&lt;Metrics&gt;
        ///			&lt;Metric Name=&quot;CyclomaticComplexity&quot;&gt;
        ///				&lt;Ranges&gt;
        ///					&lt;Range Name=&quot;&amp;gt; 10&quot; MethodCount=&quot;5&quot; /&gt;
        ///					&lt;Range Name=&quot;&amp;lt;= 10 and &amp;gt; 5&quot; MethodCount=&quot;3&quot; /&gt;
        ///					&lt;Range Name=&quot;&amp;lt;= 5&quot; MethodCount=&quot;1&quot; /&gt;
        ///				&lt;/Ranges&gt;
        ///			&lt;/Metric&gt;
        ///			&lt;Metric Name=&quot;LinesOfCode&quot;&gt;
        ///				&lt;Ranges&gt;
        ///					&lt;Range Name=&quot;&amp;gt; 100&quot; MethodCount=&quot;5&quot; /&gt;
        ///					&lt;Range N [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExpectedOutputReportXML {
            get {
                return ResourceManager.GetString("ExpectedOutputReportXML", resourceCulture);
            }
        }
    }
}