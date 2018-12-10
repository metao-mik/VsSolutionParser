using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaObjects.VisualStudio.Tools;

namespace VsSolutionUnitTests
{
    [TestClass]
    public class SampleUsageTest
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

            sln.Save();

            Assert.IsNotNull(sln);
        }

        [TestMethod]
        public void Test_Read_This_Solution()
        {
            var solutionFilePath = @"c:\users\mklei\documents\visual studio 2015\Projects\VsSolutionUnitTests\VsSolutionUnitTests.sln";

            var sln = VsSolutionFiles.ReadSolutionFile(solutionFilePath);
        
            Assert.AreEqual(4, sln.ProjectItems.Count);
        }

    
        [TestMethod]
        public void Test_Read_Solution_and_save_under_different_name()
        {
            var sourceSolutionFilePath = @"c:\users\mklei\documents\visual studio 2015\Projects\VsSolutionUnitTests\VsSolutionUnitTests.sln";
            var destSolutionFilePath = @"c:\users\mklei\documents\visual studio 2015\Projects\VsSolutionUnitTests\VsSolutionUnitTests_test2.sln";

            var sln = VsSolutionFiles.ReadSolutionFile(sourceSolutionFilePath);

            Assert.AreEqual(4, sln.ProjectItems.Count);

            sln.SaveAs(destSolutionFilePath);

            Assert.IsTrue(1 == 1); 
            // die Dateien sollten Binär fast gleich sein (Bis auf die Solution-ID) 
        }

        [TestMethod]
        public void Test_Create_Solutionfile_From_Scratch()
        {
            var solFolder = @"c: \users\mklei\documents\visual studio 2015\Projects\VsSolutionUnitTests";
            var solPath = solFolder + @"\VsSolutionUnitTests_test3.sln";
            var projPath = @"C:\Users\mklei\Documents\Visual Studio 2015\Projects\VsSolutionUnitTests\VsSolutionFiles\VsSolutionFileLib.csproj";
            var testPath = @"C:\Users\mklei\Documents\Visual Studio 2015\Projects\VsSolutionUnitTests\VsSolutionUnitTests\VsSolutionIntegrationUnitTests.csproj";

            var sln = VsSolutionFiles.Create(solPath);
            // Dokumentation mit readme.md
            var doku = sln.AddSolutionFolder("Dokumentation");
            doku.AddFile("./Dokumenation/Readme.md");

            // Tests mit Unit-Test-Projet 
            var tests = sln.AddSolutionFolder("Tests");
            var proj = tests.AddProject(testPath);
            //var proj = tests.AddProject(solPath + @"\Tests\VsSolutionIntegrationUnitTests.csproj");

            // die Klassenbiliothek
            proj = sln.AddProjectFile(projPath);
            //proj = sln.AddProjectFile(solPath + @"\VsSolutionFilesLib.csproj");

            sln.Save();
        }
    }
}
