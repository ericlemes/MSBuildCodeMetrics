﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSBuildCodeMetrics.VisualStudioMetrics.UnitTests.Resources {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MSBuildCodeMetrics.VisualStudioMetrics.UnitTests.Resources.TestResources", typeof(TestResources).Assembly);
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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;CodeMetricsReport Version=&quot;10.0&quot;&gt;
        ///  &lt;Targets&gt;
        ///    &lt;Target Name=&quot;C:\SandBox\EricLemes.MSBuildTasks\src\EricLemes.MSBuildTasks.Tests.DummyAssembly\bin\Debug\EricLemes.MSBuildTasks.Tests.DummyAssembly.dll&quot;&gt;
        ///      &lt;Modules&gt;
        ///        &lt;Module Name=&quot;EricLemes.MSBuildTasks.Tests.DummyAssembly.dll&quot; AssemblyVersion=&quot;1.0.0.0&quot; FileVersion=&quot;1.0.0.0&quot;&gt;
        ///          &lt;Metrics&gt;
        ///            &lt;Metric Name=&quot;MaintainabilityIndex&quot; Value=&quot;89&quot; /&gt;
        ///            &lt;Metric Name=&quot;CyclomaticComplex [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MetricsOutput {
            get {
                return ResourceManager.GetString("MetricsOutput", resourceCulture);
            }
        }
    }
}
