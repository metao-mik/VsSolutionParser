using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MetaObjects.VisualStudio.Tools
{

    public class VsSolutionFile
    {
        private Guid _solutionId;

        public Guid SolutionId {
            get { return _solutionId;
            }
            internal set {
                _solutionId = value;
            }
        }

        public readonly string SolutionFolder;
        public readonly string Filename;

        public readonly Dictionary<Guid, VsSolutionFileProject> ProjectItems;
        public readonly Dictionary<string, VsSolutionFileGlobalSection> GlobalSections;

        private string VsVersionHeader; 


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

            ProjectItems = new Dictionary<Guid, Tools.VsSolutionFileProject>();

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

        internal void SetSolutionId(Guid solutionId)
        {
            SolutionId = solutionId;
            if(GlobalSections.ContainsKey("ExtensibilityGlobals"))
            {
                GlobalSections["ExtensibilityGlobals"].Items["SolutionId"] = solutionId.ToString();
            }
        }

        public VsSolutionFileProject AddProjectFile(string filepath)
        {
            if(!File.Exists(filepath))
            {
                // Warning, Datei existiert nicht
            }

            var projectName = Path.GetFileNameWithoutExtension(filepath);
            var relativePath = Path.GetDirectoryName(filepath);
            var projectTypeId = GetProjectTypeId(filepath);
            var projectId = GetProjectId(filepath);

            if(projectId == Guid.Empty)
            {
                projectId = Guid.NewGuid();
                // Warning 
            }

            return AddProject(projectTypeId, projectName, relativePath, projectId);
        }

        private static Guid GetProjectId(string filepath)
        {
            var doc = XDocument.Load(filepath);
            var elem = doc.Root.Descendants().Where(p => p.Name.LocalName == "ProjectGuid").FirstOrDefault();
            if (elem != null)
            {
                return Guid.Parse(elem.Value);
            }
            else
                return Guid.Empty;
        }

        private static Guid GetProjectTypeId(string filepath)
        {
            if (Path.GetExtension(filepath).ToLower() == ".csproj")
            {
                return VsSolutionProjectTypeIds.VsSolutionProjectTypeCSharp;
            }
            else
                return Guid.Empty;
        }

        public VsSolutionFileProject AddProject(Guid projectTypeId, string projectName, string projectFolder, Guid projectId)
        {
            var newProject = new Tools.VsSolutionFileProject()
            {
                ProjectTypeId = projectTypeId,
                ProjectName = projectName,
                ProjectPath = projectFolder,
                ProjectId = projectId, 
                Solution = this
            };
            ProjectItems.Add(newProject.ProjectId, newProject);
            
            return newProject;
        }

        private void AddProjectConfigurationForProject(VsSolutionFileProject project)
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
    }

    public class VsSolutionProjectTypeIds
    {
        public static Guid VsSolutionProjectTypeCSharp = Guid.Parse("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC");
        public static Guid VsSolutionProjectTypeSolutionFolder = Guid.Parse("2150E333-8FDC-42A3-9474-1A3956D46DE8");
    }
}
