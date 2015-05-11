using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Build.Framework;
using MSBuildCodeMetrics.Core.UnitTests.Mock;
using System.IO;
using Moq;
using MSBuildCodeMetrics.Core;
using MSBuildCodeMetrics.Core.XML;

namespace MSBuildCodeMetrics.Tasks.UnitTests
{
	[TestClass]
	public class GivenACodeMetricsTask
	{
		private CodeMetrics _task;
		private TaskItemMock[] _metrics;
		private TaskItemMock[] _providers;
		private BuildEngineMock _buildEngine;

		[TestInitialize]
		public void Initialize()
		{
			_buildEngine = new BuildEngineMock();
			_task = new CodeMetrics();
			_task.BuildEngine = _buildEngine;

			_metrics = new TaskItemMock[1];
			_metrics[0] = new TaskItemMock("LinesOfCode").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "foo");

			_providers = new TaskItemMock[1];
			_providers[0] = new TaskItemMock(
				"MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Metrics", "LinesOfCode");
		}

	    [TestMethod]
	    public void WhenConstructingShouldNotThrow()
	    {
	        var fileStreamFactoryMock = new Mock<IFileStreamFactory>();
	        new CodeMetrics(fileStreamFactoryMock.Object);
	    }

		[TestMethod]
		public void WhenRunningTaskShouldComputeMetrics()
		{
			ITaskItem[] providers = new TaskItemMock[1];
			providers[0] = new TaskItemMock("MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests").				
				AddMetadata("Data", "<Metric Name=\"LinesOfCode\"><Measure " + 
					"Name=\"Method1\" Value=\"1\" /><Measure Name=\"Method2\" Value=\"2\" /><Measure " +
					"Name=\"Method3\" Value=\"5\" /></Metric>").
				AddMetadata("ProviderName", "CodeMetricsProviderMock");

			ITaskItem[] metrics = new TaskItemMock[1];
			metrics[0] = new TaskItemMock("LinesOfCode").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "C:\\foo.txt");			

			FileStreamFactoryMock streamFactory = new FileStreamFactoryMock();			

			CodeMetrics task = new CodeMetrics(streamFactory);
			task.BuildEngine = new BuildEngineMock();
			task.Providers = providers;			
			task.OutputFileName = "report.xml";
			task.ShowDetailsReport = true;
			task.ShowSummaryReport = true;
			task.Metrics = metrics;
			task.FileOutput = true;
			Assert.AreEqual(true, task.Execute());

			streamFactory.OutputStream.Seek(0, SeekOrigin.Begin);
			MSBuildCodeMetricsReport report = MSBuildCodeMetricsReport.Deserialize(streamFactory.OutputStream);
			Assert.AreEqual(1, report.Details.Metrics.Count);
			Assert.AreEqual("LinesOfCode", report.Details.Metrics[0].MetricName);
			Assert.AreEqual("Method3", report.Details.Metrics[0].Measures[0].MeasureName);
			Assert.AreEqual(5, report.Details.Metrics[0].Measures[0].Value);
		}

		[TestMethod]		
		public void WhenRunningWithoutMetricsShouldReturnFalseAndSetError()
		{
			_task.Metrics = new TaskItemMock[0];
			_task.Providers = _providers;			
			
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("At least one Metrics must be informed in Metrics property", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void WhenRunningWithoutProvidersShouldReturnFalseAndLogError()
		{
			_task.Metrics = _metrics;			
			_task.Providers = new TaskItemMock[0];
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("At least one Provider must me informed in Providers property", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void WhenRunningWithInvalidProviderShouldReturnFalseAndLogError()
		{
			_task.Metrics = _metrics;			
			_task.Providers = new TaskItemMock[1];
			_task.Providers[0] = new TaskItemMock("foo");
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Invalid provider: foo. Couldn't create instance of this type", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void WhenRunningWithClassThatIsNotAProviderShouldReturnFalseAndLogError()
		{
			_task.Metrics = _metrics;			
			_task.Providers = new TaskItemMock[1];
			_task.Providers[0] = new TaskItemMock("MSBuildCodeMetrics.Tasks.CodeMetrics, MSBuildCodeMetrics.Tasks");
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Type MSBuildCodeMetrics.Tasks.CodeMetrics, MSBuildCodeMetrics.Tasks doesn't implements ICodeMetricsProvider", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void WhenRunningWithProviderThatDoesNotImplementsNameShouldReturnFalseAndLogError()
		{
			_task.Metrics = _metrics;			
			_task.Providers = new TaskItemMock[1];
			_task.Providers[0] = new TaskItemMock("MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests");
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Provider MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests doesn't implement property Name correctly", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void WhenRunningWithoutFilesShouldReturnFalseAndLogError()
		{
			_task.Metrics = _metrics;			
			_task.Providers = _providers;
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock("LinesOfCode").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Ranges", "2;3");
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Files must be informed in Metrics property. ProviderName: CodeMetricsProviderMock, Metric: LinesOfCode", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void WhenRunningWithoutProviderNameShouldReturnFalseAndLogError()
		{
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock("InvalidMetric").
				AddMetadata("ProviderName", null).
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "foo");
			_task.Providers = _providers;
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("ProviderName must be informed in Metrics property", _buildEngine.ErrorMessage);
		}


        [TestMethod]
        public void WhenRunningWithNoProivdersShouldReturnFalseAndLogError()
        {
            _task.Metrics = new TaskItemMock[1];
            _task.Metrics[0] = new TaskItemMock("InvalidMetric").
                AddMetadata("ProviderName", "Provider").
                AddMetadata("Ranges", "2;3").
                AddMetadata("Files", "foo");
            _task.Providers = new ITaskItem[0];
            Assert.AreEqual(false, _task.Execute());
            Assert.AreEqual("At least one Provider must me informed in Providers property", _buildEngine.ErrorMessage);
        }

        [TestMethod]
        public void WhenRunningWithoutMetricNameShouldReturnFalseAndLogError()
        {
            _task.Metrics = new TaskItemMock[1];
            _task.Metrics[0] = new TaskItemMock(null).
                AddMetadata("ProviderName", "CodeMetricsProviderMock").
                AddMetadata("Ranges", "2;3").
                AddMetadata("Files", "foo");
            _task.Providers = _providers;
            Assert.AreEqual(false, _task.Execute());
            Assert.AreEqual("Metric name in property Metrics can't be null (ProviderName: CodeMetricsProviderMock", _buildEngine.ErrorMessage);
        }

		[TestMethod]
		public void WhenRunningWithInvalidProviderNameShouldReturnFalseAndLogError()
		{
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock("InvalidMetric").
				AddMetadata("ProviderName", "InvalidProvider").
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "foo");
			_task.Providers = _providers;
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Invalid provider name in Metrics property: ProviderName: InvalidProvider", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void WhenRunningWithoutMetricsShouldReturnFalseAndLogError()
		{
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock(null).
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "foo");
			_task.Providers = _providers;
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Metric name in property Metrics can't be null (ProviderName: CodeMetricsProviderMock", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void WhenRunningWithInvalidMetricShouldReturnFalseAndLogError()
		{
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock("InvalidMetric").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Ranges", "2;3").
				AddMetadata("Files", "foo");
			_task.Providers = _providers;
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Provider CodeMetricsProviderMock doesn't know how to handle metric InvalidMetric", _buildEngine.ErrorMessage);
		}

		[TestMethod]
		public void WhenRunningWithoutRangesShouldReturnFalseAndLogError()
		{
			_task.Metrics = new TaskItemMock[1];
			_task.Metrics[0] = new TaskItemMock("LinesOfCode").
				AddMetadata("ProviderName", "CodeMetricsProviderMock").
				AddMetadata("Files", "foo");
			_task.Providers = _providers;
			Assert.AreEqual(false, _task.Execute());
			Assert.AreEqual("Ranges can't be null if you need a summary report. ProviderName: CodeMetricsProviderMock, MetricName: LinesOfCode", _buildEngine.ErrorMessage);
		}

	    [TestMethod]
	    public void WhenSettingShowConsoleOutputShouldStoreProperly()
	    {
	        _task.ShowConsoleOutput = false;
	        Assert.AreEqual(false, _task.ShowConsoleOutput);
	    }

	    [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
	    public void WhenSettingRangeWithInvalidValueShouldReturnFalseAndSetError()
	    {
	        var buildEngineMock = new Mock<IBuildEngine>();
	        var task = new CodeMetrics()
	        {
	            BuildEngine = buildEngineMock.Object,
	            Metrics = new ITaskItem[]
	            {
	                _metrics[0] = new TaskItemMock("LinesOfCode").
	                    AddMetadata("ProviderName", "CodeMetricsProviderMock").
	                    AddMetadata("Ranges", "abc;abc").
	                    AddMetadata("Files", "foo")
	            }
	        };
	        task.Execute();
	    }

        [TestMethod]     
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenRunningWithoutMetricsShouldReturnFalseAndSetErrorMessage()
        {
            var buildEngineMock = new Mock<IBuildEngine>();
            var task = new CodeMetrics()
            {
                BuildEngine = buildEngineMock.Object                
            };
            task.Execute();            
        }

        [TestMethod]        
        public void WhenRunningWithMetricsWithEmptyRangesShouldReturnFalseAndSetErrorMessage()
        {
            var buildEngineMock = new Mock<IBuildEngine>();
            var errorMessage = String.Empty;
            buildEngineMock.Setup(be => be.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).
                Callback<BuildErrorEventArgs>(
                    e =>
                    {
                        errorMessage = e.Message;
                    });
            var task = new CodeMetrics()
            {
                BuildEngine = buildEngineMock.Object,
                Metrics = new ITaskItem[]
                {
                    new TaskItemMock("Metric").
                        AddMetadata("ProviderName", "CodeMetricsProviderMock").
                        AddMetadata("Ranges", null).
                        AddMetadata("Files", "foo")
                }								
            };
            Assert.IsFalse(task.Execute());
            Assert.IsTrue(errorMessage.StartsWith("Ranges can't be null if you need a summary report."));
        }

	    [TestMethod]
	    public void WhenRunningWithProcessExecutorCodeMetricsShouldSetProcessExecutor()
	    {
            var buildEngineMock = new Mock<IBuildEngine>();	        

            var task = new CodeMetrics()
            {
                BuildEngine = buildEngineMock.Object,
                Providers = new ITaskItem[]
                {
                    new TaskItemMock("MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderProcessExecutorMock, MSBuildCodeMetrics.Core.UnitTests").
                    AddMetadata("ProviderName", "CodeMetricsProviderProcessExecutorMock")                    
                },
                Metrics = new ITaskItem[]
                {
                    new TaskItemMock("Metric").
                        AddMetadata("ProviderName", "DummyProvider").
                        AddMetadata("Ranges", "1").
                        AddMetadata("Files", "foo")
                }
            };
            Assert.IsNull(CodeMetricsProviderProcessExecutorMock.LastProcessExecutorSet);
	        task.Execute();
            Assert.IsNotNull(CodeMetricsProviderProcessExecutorMock.LastProcessExecutorSet);            
	    }

	    [TestMethod]
	    public void WhenRunningWithHigherRangeFailMessageAndHasValueOnHigherBandShouldFail()
	    {
            var buildEngineMock = new Mock<IBuildEngine>();
            var errorMessage = String.Empty;
            buildEngineMock.Setup(be => be.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).
                Callback<BuildErrorEventArgs>(
                    e =>
                    {
                        errorMessage = e.Message;
                    });

            var task = new CodeMetrics()
            {
                BuildEngine = buildEngineMock.Object,
                Providers = new ITaskItem[]
                {
                    new TaskItemMock("MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests").
                        AddMetadata("Data", "<Metric Name=\"LinesOfCode\"><Measure " +                            
                                    "Name=\"Method1\" Value=\"1\" /><Measure Name=\"Method2\" Value=\"2\" /><Measure " + 
                                    "Name=\"Method3\" Value=\"5\" /></Metric>").
                        AddMetadata("ProviderName", "CodeMetricsProviderMock")
                },
                Metrics = new ITaskItem[]
                {
                    new TaskItemMock("LinesOfCode").
                        AddMetadata("ProviderName", "CodeMetricsProviderMock").
                        AddMetadata("Ranges", "4").
                        AddMetadata("Files", "foo").
                        AddMetadata("HigherRangeFailMessage", "There are methods with more than 4 lines of code")
                }
            };

	        Assert.AreEqual(false, task.Execute());
            Assert.AreEqual("There are methods with more than 4 lines of code", errorMessage);
	    }

        [TestMethod]
        public void WhenRunningWithLowerRangeFailMessageAndDoesnotHaveValueOnLowerBandShouldNotFail()
        {
            var buildEngineMock = new Mock<IBuildEngine>();
            var errorMessage = String.Empty;
            buildEngineMock.Setup(be => be.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).
                Callback<BuildErrorEventArgs>(
                    e =>
                    {
                        errorMessage = e.Message;
                    });

            var task = new CodeMetrics()
            {
                BuildEngine = buildEngineMock.Object,
                Providers = new ITaskItem[]
                {
                    new TaskItemMock("MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests").
                        AddMetadata("Data", "<Metric Name=\"CodeCoverage\"><Measure " +                            
                                    "Name=\"Assembly1\" Value=\"100\" /><Measure Name=\"Assembly2\" Value=\"100\" /><Measure " + 
                                    "Name=\"Assembly3\" Value=\"100\" /></Metric>").
                        AddMetadata("ProviderName", "CodeMetricsProviderMock")
                },
                Metrics = new ITaskItem[]
                {
                    new TaskItemMock("CodeCoverage").
                        AddMetadata("ProviderName", "CodeMetricsProviderMock").
                        AddMetadata("Ranges", "90").
                        AddMetadata("Files", "foo").
                        AddMetadata("LowerRangeFailMessage", "There are assemblies with less than 90% of coverage")
                }
            };

            Assert.AreEqual(true, task.Execute());            
        }        

        [TestMethod]
        public void WhenRunningWithLowerRangeFailMessageAndHasValueOnLowerBandShouldFail()
        {
            var buildEngineMock = new Mock<IBuildEngine>();
            var errorMessage = String.Empty;
            buildEngineMock.Setup(be => be.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).
                Callback<BuildErrorEventArgs>(
                    e =>
                    {
                        errorMessage = e.Message;
                    });

            var task = new CodeMetrics()
            {
                BuildEngine = buildEngineMock.Object,
                Providers = new ITaskItem[]
                {
                    new TaskItemMock("MSBuildCodeMetrics.Core.UnitTests.Mock.CodeMetricsProviderSingleFileMock, MSBuildCodeMetrics.Core.UnitTests").
                        AddMetadata("Data", "<Metric Name=\"CodeCoverage\"><Measure " +                            
                                    "Name=\"Assembly1\" Value=\"75\" /><Measure Name=\"Assembly2\" Value=\"100\" /><Measure " + 
                                    "Name=\"Assembly3\" Value=\"100\" /></Metric>").
                        AddMetadata("ProviderName", "CodeMetricsProviderMock")
                },
                Metrics = new ITaskItem[]
                {
                    new TaskItemMock("CodeCoverage").
                        AddMetadata("ProviderName", "CodeMetricsProviderMock").
                        AddMetadata("Ranges", "90").
                        AddMetadata("Files", "foo").
                        AddMetadata("LowerRangeFailMessage", "There are assemblies with less than 90% of coverage")
                }
            };

            Assert.AreEqual(false, task.Execute());
            Assert.AreEqual("There are assemblies with less than 90% of coverage", errorMessage);
        }        

	}
}
