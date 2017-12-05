# MSBuildCodeMetrics

----
## What is MSBuildCodeMetrics?

MSBuildCodeMetrics is a set of MSBuild tasks to help extracting code metrics.

The main goal is provide an open source alternative for some commercial tools that usually big and hard to plug on standard continuous integration tools.

Some metrics uses direct file parsing and other uses .NET managed code IL. Check case by case if the metrics are useful for you.

The reports are very simple, but useful. You can show a detailed report and a summary report.

### Detailed Report
This report supply each and every measure. For example, if you're using CountFilesByExtension, this report show you each extension and how many files you have.

For cyclomatic complexity (Visual Studio), you will have one line for each method, ranked by the most complex.

### Summary report
When handling big repositories (in the order of million LOC), it's very hard to handle detailed metrics.

The summary report, allows you to set up ranges to see how many measures you for each range.

Using the same example above, if you set ranges for 1000 and 500, this report will tell you how many extension group's has more than 1000 files, between 1000 and 500 and below 500.

The same logic is used for other measures. This can help you find how many methods has more than 30 in cyclomatic complexity


## Which code metrics are supported?

### Default Provider (MSBuildCodeMetrics.Core.Providers)

* **Files By Extension: **This isn't a usual code metric, but I built this to help understanding the different stuff contained by a given source code repository. It can help me find different kinds of compilers, IDE's and file types are used by the application
* **Lines of Code: **This is a interesting measure to know the size of an application. This metric means total LOC by file.
* **Projects by Project Type: ** This metric is useful to understand how many output projects exists in our repository. The idea is to parse the .csproj and .vbproj files and group the output by project type (Library, WinExe, Exe, etc.)

### Visual Studio Provider (MSBuildCodeMetrics.VisualStudioMetrics.VisualStudioCodeMetricsProvider)

Visual Studio provides the "Code Metrics Power Tool" which is a command line tool that can provide some metrics from managed code. You will need the tool installed in order to get the metrics, but MSBuildCodeMetrics does some of the dirty job of parsing the XML and make it easily readable for bigger code bases.

You can download the tool (hopefully) from [https://msdn.microsoft.com/en-us/library/bb385914.aspx](https://msdn.microsoft.com/en-us/library/bb385914.aspx)

When this was written, the metrics supported are:

* **Maintainability Index: **See VS reference.
* **Cyclomatic Complexity: **Very popular metric to detect how many paths are inside a method, giving an idea of complexity of the method.
* **Depth of Inheritance: ** See VS reference.
* **Class Coupling: ** See VS Reference.
* **Lines of Code per Method: ** Gets number of lines of code per method. 


## Extending MSBuildCodeMetrics

You can create your own providers extending the MSBuildCodeMetrics.Core.ICodeMetricsProvider interface. 

Of course, this is an open source project. If you have more ideas or want to contribute, feel free to contact me sending e-mail to ericlemes at gmail.com or just fork the repo on GitHub.

## Example

Code Metrics Task
The source code includes an example of CodeMetrics inspecting MSBuildCodeMetrics code. The example uses the following structure:


	<Target Name="demo" DependsOnTargets="build">		
		<ItemGroup>
			<Providers Include="MSBuildCodeMetrics.VisualStudioMetrics.VisualStudioCodeMetricsProvider, MSBuildCodeMetrics.VisualStudioMetrics">
				<TempDir>$(MSBuildProjectDirectory)\TempDir</TempDir>
			</Providers>		
			<Providers Include="MSBuildCodeMetrics.Core.Providers.CountFilesByExtensionProvider, MSBuildCodeMetrics.Core.Providers" />
			<Providers Include="MSBuildCodeMetrics.Core.Providers.CountLOCProvider, MSBuildCodeMetrics.Core.Providers">
				<FileTypes>.cs=C# Sources;.csproj=C# Projects</FileTypes>				
			</Providers>
			<Providers Include="MSBuildCodeMetrics.Core.Providers.CountProjectsByProjectTypeProvider, MSBuildCodeMetrics.Core.Providers" />
			<AllFiles Include="..\src\**\*.*" />
			<SourceFiles Include="..\src\**\*.cs;..\src\**\*.csproj" />
			<ProjectFiles Include="..\src\**\*.csproj" />
			<BinariesInOutputDir Include="$(BinariesOutputDir)\*.dll" />
			<Metrics Include="LinesOfCode">
				<ProviderName>VisualStudioMetrics</ProviderName>
				<Ranges>50;10;1</Ranges>			
				<Files>@(BinariesInOutputDir)</Files>
			</Metrics>				
			<Metrics Include="CyclomaticComplexity">
				<ProviderName>VisualStudioMetrics</ProviderName>
				<Ranges>10;5;4;3;2;1</Ranges>			
				<Files>@(BinariesInOutputDir)</Files>
			</Metrics>	
			<Metrics Include="CountFilesByExtension">
				<ProviderName>CountFilesByExtension</ProviderName>
				<Files>@(AllFiles)</Files>
				<Ranges>100</Ranges>
			</Metrics>
			<Metrics Include="CodeLOC">
				<ProviderName>LOC</ProviderName>
				<Files>@(SourceFiles)</Files>
				<Ranges>500</Ranges>
			</Metrics>			
			<Metrics Include="ProjectTypeCount">
				<ProviderName>CountProjectsByProjectTypeProvider</ProviderName>
				<Files>@(ProjectFiles)</Files>
				<Ranges>100</Ranges>
			</Metrics>
		</ItemGroup>
		<MakeDir Directories="$(MSBuildProjectDirectory)\TempDir" />
		<CodeMetrics Providers="@(Providers)" Metrics="@(Metrics)" ShowDetailsReport="true" FileOutput="true" OutputFileName="metrics.xml"  />
	</Target>	
