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

        public readonly Dictionary<Guid, VsSolutionFileProject> Projects;
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

            Projects = new Dictionary<Guid, Tools.VsSolutionFileProject>();

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
            var relativePath = GetRelativePathToSolutionFolder(filepath); //  Path.GetDirectoryName(filepath);
            var projectTypeId = VsSolutionProjectTypeIds.GetProjectTypeIdByFilepath(filepath);
            var projectId = GetProjectId(filepath);

            if(projectId == Guid.Empty)
            {
                projectId = Guid.NewGuid();
                // Warning 
            }

            return AddProject(projectTypeId, projectName, relativePath, projectId);
        }

        internal string GetRelativePathToSolutionFolder(string filepath)
        {
            var folder = this.SolutionFolder;

            Uri pathUri = new Uri(filepath);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
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
            Projects.Add(newProject.ProjectId, newProject);

            AddProjectConfigurationForProject(newProject);

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
}
