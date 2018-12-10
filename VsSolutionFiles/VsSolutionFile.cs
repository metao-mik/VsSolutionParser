using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaObjects.VisualStudio.Tools
{
    public class VsSolutionFile
    {
        public readonly Guid SolutionId;
        public readonly string SolutionFolder;
        public readonly string Filename;

        public readonly List<VsSolutionFileProjectItem> ProjectItems;
        public readonly Dictionary<string, VsSolutionFileGlobalSection> GlobalSections;

        internal static VsSolutionFile OpenSolutionFile(string filename)
        {
            throw new NotImplementedException();
        }

        public VsSolutionFile(string filePath) : this(filePath, Guid.NewGuid()) { }

        public VsSolutionFile(string filePath, Guid solutionId)
        {
            SolutionFolder = System.IO.Path.GetDirectoryName(filePath);
            Filename = System.IO.Path.GetFileName(filePath);
            SolutionId = solutionId;

            ProjectItems = new List<Tools.VsSolutionFileProjectItem>();

            GlobalSections = new Dictionary<string, VsSolutionFileGlobalSection>();
            GlobalSections.Add("SolutionConfigurationPlatforms", PrePostSolution.preSolution,
                new string[][] {
                    new string[] { "Debug|Any CPU", "Debug|Any CPU" },
                    new string[] { "Release|Any CPU", "Release|Any CPU" }});
            GlobalSections.Add("ProjectConfigurationPlatforms", PrePostSolution.postSolution);
            GlobalSections.Add("SolutionProperties", PrePostSolution.preSolution, 
                new string[][] { new string[] { "HideSolutionNode", "FALSE"} });
            GlobalSections.Add("ExtensibilityGlobals", PrePostSolution.postSolution,
                new string[][] { new string[] { "SolutionGuid", "{" + solutionId.ToString().ToUpper() + "}" } });
        }

        public void AddProjectFileCsproj(string csprojFile)
        {

        }

        public VsSolutionFileProjectItem AddProject(Guid projectTypeId, string projectName, string projectFolder, Guid projectId)
        {
            var newProject = new Tools.VsSolutionFileProjectItem()
            {
                ProjectTypeId = projectTypeId,
                ProjectName = projectName,
                ProjectPath = projectFolder,
                ProjectId = projectId
            };
            ProjectItems.Add(newProject);

            
            return newProject;
        }

        private void AddProjectConfigurationForProject(VsSolutionFileProjectItem project)
        {
            if (project.ProjectTypeId == VsSolutionProjectTypeIds.VsSolutionProjectTypeCSharp)
            {
                foreach (var item in this.GlobalSections["SolutionConfigurationPlatforms"].Items)
                {
                    this.GlobalSections["ProjectConfigurationPlatforms"].Items.Add(
                        "{" + project.ProjectId.ToString().ToUpper() + "." + item.Key + ".ActiveCfg", item.Value);
                    this.GlobalSections["ProjectConfigurationPlatforms"].Items.Add(
                        "{" + project.ProjectId.ToString().ToUpper() + "." + item.Key + ".Build.0", item.Value);
                }
            }
        }

        public VsSolutionFile ReadSolutionFile(string filePath)
        {
            var lines = System.IO.File.ReadAllLines(filePath);

            var line = 0; 
            while (line < lines.Length)
            {
                if (lines[line].StartsWith("Project(\"{"))
                {
                    while (line < lines.Length || lines[line].StartsWith("EndProject"))
                    {
                        if (lines[line].StartsWith("EndProject"))
                        {
                            // add Object
                        }
                        line++;
                    }
                }
                line++;
            }

            return this; 
        }
    }

    public class VsSolutionProjectTypeIds
    {
        public static Guid VsSolutionProjectTypeCSharp = Guid.Parse("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC");
        public static Guid VsSolutionProjectTypeSolutionFolder = Guid.Parse("2150E333-8FDC-42A3-9474-1A3956D46DE8");
    }
}
