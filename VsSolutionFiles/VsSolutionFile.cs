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

        public readonly List<VsSolutionFileProjectItem> projectItems; 

        internal static VsSolutionFile OpenSolutionFile(string filename)
        {
            throw new NotImplementedException();
        }

        public VsSolutionFile(string filePath) : this(filePath, Guid.NewGuid()) { }

        public VsSolutionFile(string filePath, Guid solutionId) 
        {
            SolutionFolder = System.IO.Path.GetPathRoot(filePath);
            Filename = System.IO.Path.GetFileName(filePath);
            SolutionId = solutionId;

            projectItems = new List<Tools.VsSolutionFileProjectItem>();
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
            projectItems.Add(newProject);
            return newProject;
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
