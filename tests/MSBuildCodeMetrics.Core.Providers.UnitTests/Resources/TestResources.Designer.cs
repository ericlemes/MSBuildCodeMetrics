﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSBuildCodeMetrics.Core.Providers.UnitTests.Resources {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MSBuildCodeMetrics.Core.Providers.UnitTests.Resources.TestResources", typeof(TestResources).Assembly);
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
        ///&lt;Project ToolsVersion=&quot;4.0&quot; DefaultTargets=&quot;Build&quot; xmlns=&quot;http://schemas.microsoft.com/developer/msbuild/2003&quot;&gt;
        ///  &lt;PropertyGroup&gt;
        ///    &lt;Configuration Condition=&quot; &apos;$(Configuration)&apos; == &apos;&apos; &quot;&gt;Debug&lt;/Configuration&gt;
        ///    &lt;Platform Condition=&quot; &apos;$(Platform)&apos; == &apos;&apos; &quot;&gt;AnyCPU&lt;/Platform&gt;
        ///    &lt;ProductVersion&gt;8.0.30703&lt;/ProductVersion&gt;
        ///    &lt;SchemaVersion&gt;2.0&lt;/SchemaVersion&gt;
        ///    &lt;ProjectGuid&gt;{13E1FB9D-8405-485B-844E-ED421E95F77F}&lt;/ProjectGuid&gt;
        ///    &lt;OutputType&gt;Library&lt;/OutputTy [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Project1 {
            get {
                return ResourceManager.GetString("Project1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;Project ToolsVersion=&quot;4.0&quot; DefaultTargets=&quot;Build&quot; xmlns=&quot;http://schemas.microsoft.com/developer/msbuild/2003&quot;&gt;
        ///  &lt;PropertyGroup&gt;
        ///    &lt;Configuration Condition=&quot; &apos;$(Configuration)&apos; == &apos;&apos; &quot;&gt;Debug&lt;/Configuration&gt;
        ///    &lt;Platform Condition=&quot; &apos;$(Platform)&apos; == &apos;&apos; &quot;&gt;AnyCPU&lt;/Platform&gt;
        ///    &lt;ProductVersion&gt;
        ///    &lt;/ProductVersion&gt;
        ///    &lt;SchemaVersion&gt;2.0&lt;/SchemaVersion&gt;
        ///    &lt;ProjectGuid&gt;{43B4F89B-4D31-4EC9-AAFA-70DAF3701674}&lt;/ProjectGuid&gt;
        ///    &lt;OutputType&gt;Library&lt;/OutputType&gt; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Project2 {
            get {
                return ResourceManager.GetString("Project2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Collections.Generic;
        ///using System.Linq;
        ///using System.Text;
        ///
        ///namespace MSBuildCodeMetrics.Core
        ///{
        ///	public class MetricList : List&lt;ReportParam&gt;
        ///	{
        ///		public static MetricList Create()
        ///		{
        ///			return new MetricList();
        ///		}
        ///
        ///		public MetricList Add(string providerName, string metricName, List&lt;int&gt; range)
        ///		{
        ///			/* 
        ///			Comment 
        ///			Comment
        ///			Comment
        ///			*/
        ///			base.Add(new ReportParam(providerName, metricName, range));
        ///			return this;
        ///		}
        ///
        ///		public MetricList Add(str [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SourceFile1 {
            get {
                return ResourceManager.GetString("SourceFile1", resourceCulture);
            }
        }
    }
}
