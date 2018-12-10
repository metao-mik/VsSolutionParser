using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaObjects.VisualStudio.Tools;

namespace VsSolutionUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Create_New_Solution()
        {
            var solutionFilePath = @"c:\users\mklei\documents\visual studio 2015\Projects\VsSolutionUnitTests\VsSolutionUnitTests_test.sln";
            var project1Path = @"c:\users\mklei\documents\visual studio 2015\Projects\VsSolutionUnitTests\VsSolutionFiles\VsSolutionFilesLib.csproj";
            var project2Path = @"c:\users\mklei\documents\visual studio 2015\Projects\VsSolutionUnitTests\VsSolutionUnitTests\VsSolutionUnitTests.csproj";


            var sln = VsSolutionFiles.Create(solutionFilePath);
            var p1 = sln.AddProject(VsSolutionProjectTypeIds.VsSolutionProjectTypeCSharp, "VsSolutionFilesLib", @"VsSolutionFiles\VsSolutionFilesLib.csproj", Guid.Parse("5479DAB1-6B5E-4757-9375-B6B099D4160B"));
            var p2 = sln.AddProject(VsSolutionProjectTypeIds.VsSolutionProjectTypeCSharp, "VsSolutionUnitTests", @"VsSolutionUnitTests\VsSolutionUnitTests.csproj", Guid.Parse("5479DAB1-6B5E-4757-9375-B6B099D4160B"));
            //            sln.AddProjectFileCsproj(project1Path);
            //            sln.AddProjectFileCsproj(project2Path);

            sln.SaveSoltionfile();

            Assert.IsNotNull(sln);
        }

        [TestMethod]
        public void Test_Read_Solution_1()
        {
            var solutionFilePath = @"c:\users\mklei\documents\visual studio 2015\Projects\VsSolutionUnitTests\VsSolutionUnitTests_test.sln";

            var sln = VsSolutionFiles.ReadSolutionFile(solutionFilePath);

        }
    }
}
