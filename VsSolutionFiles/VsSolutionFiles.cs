using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaObjects.VisualStudio.Tools
{
    public static class VsSolutionFiles
    {
        public static VsSolutionFile Create(string path)
        {
            return new VsSolutionFile(path);
        }

        public static VsSolutionFile ReadSolutionFile(string filePath)
        {
            var sln = new VsSolutionFile(filePath);


            var lines = System.IO.File.ReadAllLines(filePath);
            var block = new List<string>();

            var isHeader = true;
            var isProject = false;
            var isGlobalSection = false;
            var line = 0;

            while (line < lines.Length)
            {
                // Header
                if (isHeader && lines[line].StartsWith("Project(\"{"))
                {
                    sln.EvaluateHeader(block.ToArray());
                    isHeader = false;
                }
                // projects 
                if (!isHeader && lines[line].StartsWith("Project(\"{"))
                {
                    block = new List<string>();
                    isProject = true;
                }
                if (isProject && lines[line].StartsWith("EndProject"))
                {
                    block.Add(lines[line]);
                    sln.EvaluateProject(block.ToArray());
                    isProject = false;
                }
                // Global Section 
                if (!isHeader && lines[line].StartsWith("\tGlobalSection("))
                {
                    block = new List<string>();
                    isGlobalSection = true;
                }
                if (isGlobalSection && lines[line].StartsWith("\tEndGlobalSection"))
                {
                    block.Add(lines[line]);
                    sln.EvaluateGlobalSection(block.ToArray());
                    isGlobalSection = false;
                }

                block.Add(lines[line]);
                line++;
            }
            return sln;
        }

        private static void EvaluateHeader(this VsSolutionFile sln, string[] block)
        {
            //VsVersionHeader = block.Concat("\n");
        }

        private static void EvaluateProject(this VsSolutionFile sln, string[] block)
        {
            var items = block[0].Split(',');
            var projectId = Guid.Parse(items[2].Substring(3, 36));
            var projectTypeId = Guid.Parse(items[0].Substring(10, 36));
            var projectName = items[0].Substring(53, items[0].Length - 53 - 1);
            var projectPath = items[1].Trim().Substring(1, items[1].Trim().Length - 2);

            var proj = sln.AddProject(projectTypeId, projectName, projectPath, projectId);
            
            // Solution-Files 
            if(proj.ProjectTypeId == VsSolutionProjectTypeIds.VsSolutionProjectTypeSolutionFolder && 
                    block[1].StartsWith("\tProjectSection(SolutionItems)"))
            {
                for (int line = 2; line < block.Length - 2; line++)
                {
                    var values = block[line].Split('=');
                    var name = values[0].Trim();
                    var path = values[1].Trim();
                    proj.Files.Add(name, path);
                }
            }
        }

        private static void EvaluateGlobalSection(this VsSolutionFile sln, string[] block)
        {
            var sectionNameAndPrePost = block[0].Split('=');
            var section = new VsSolutionFileGlobalSection(
                sectionNameAndPrePost[0].Substring(15, sectionNameAndPrePost[0].Length - 15 - 2),
                sectionNameAndPrePost[1] == " preSolution" ? PrePostSolution.preSolution : PrePostSolution.postSolution);

            for (int i = 1; i < block.Length - 1; i++)
            {
                var values = block[i].Split('=');
                section.Items.Add(values[0].Substring(2).Trim(), values[1].Trim());
            }

            if(section.Name == "ExtensibilityGlobals")
            {
                sln.SolutionId = Guid.Parse(section.Items["SolutionGuid"]);
            }

            if(section.Name == "NestedProjects")
            {
                foreach (var item in section.Items)
                {
                    sln.ProjectItems[Guid.Parse(item.Key)].ParentPoject = item.Value;
                }
            }


            if (sln.GlobalSections.ContainsKey(section.Name))
            {
                sln.GlobalSections.Remove(section.Name);
            }
            sln.GlobalSections.Add(section.Name, section);
        }

    }
}
