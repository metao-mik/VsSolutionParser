using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaObjects.VisualStudio.Tools
{
    public static class VsSolutionFolderExtentions
    {
        public static VsSolutionFileProject AddSolutionFolder(this VsSolutionFile sln, string foldername)
        {
            var proj = sln.AddProject(VsSolutionProjectTypeIds.VsSolutionProjectTypeSolutionFolder, foldername, foldername, Guid.NewGuid());
            return proj;
        }

        public static string AddFile(this VsSolutionFileProject proj, string filepath)
        {
            if( proj.ProjectTypeId != VsSolutionProjectTypeIds.VsSolutionProjectTypeSolutionFolder)
            {
                throw new NotImplementedException("Diese methode ist nur für SolutionFodler vorgesehen.");
            }

            var filename = Path.GetFileName(filepath);
            var relativeFilepath = filepath;
            if (filepath.ToLower().StartsWith(proj.Solution.SolutionFolder))
            {
                relativeFilepath = filepath.Substring(proj.Solution.SolutionFolder.Length);
            }
            relativeFilepath = proj.Solution.GetRelativePathToSolutionFolder(filepath);
            proj.Files.Add(filename, relativeFilepath);

            return relativeFilepath;
        }

        public static VsSolutionFileProject AddProject(this VsSolutionFileProject proj, string filepath)
        {
            var addedProj = proj.Solution.AddProjectFile(filepath);
            addedProj.ParentPoject = proj.ProjectId;

            return addedProj;
        }

        public static String ToStingVSFormat(this Guid guid)
        {
            return "{" + guid.ToString().ToUpper() + "}";
        }
    }
}
