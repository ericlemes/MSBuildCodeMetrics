using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildCodeMetrics.Core;

namespace MSBuildCodeMetrics.IntegrationTests
{
    [TestClass]
    public class GivenAFileStreamFactory
    {
        [TestMethod]
        public void WhenOpeningFileShouldReturnValidStream()
        {
            var currentPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            var fsf = new FileStreamFactory();
            var fileName = currentPath + "\\dummyfile.txt";
            var fileStream = fsf.CreateStream(fileName);            
            fileStream.Flush();
            fileStream.Close();

            fileStream = fsf.OpenFile(fileName);
            Assert.IsNotNull(fileStream);
            Assert.IsTrue(fileStream.CanRead);
        }
    }
}
