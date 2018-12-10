using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaObjects.VisualStudio.Tools
{
    public static class VsSolutionFileExtention
    {
        internal static string[] GetHeader(this VsSolutionFile solutionFile)
        {
            var text = new List<string>();

            text.AddRange(GetVsSolutionHeaderFormatVS12());
            text.AddRange(GetVsSlutionHeaderExtention());

            return text.ToArray();
        }
        internal static string[] GetVsSolutionHeaderFormatVS12()
        {
            return new string[]
            {
                "",
                "Microsoft Visual Studio Solution File, Format Version 12.00"
            };
        }

        internal static string[] GetVsSlutionHeaderExtention()
        {
            return new string[]
            {
                "# Visual Studio 14",
                "VisualStudioVersion = 14.0.25420.1",
                "MinimumVisualStudioVersion = 10.0.40219.1"
            };
        }

        internal static string[] GetProjects(this VsSolutionFile sln)
        {
            var text = new List<string>();

            foreach (var p in sln.ProjectItems)
            {
                text.AddRange(p.Value.GetProjectItemLines());
            }

            return text.ToArray();
        }

        internal static string[] GetProjectItemLines(this VsSolutionFileProject projectItem)
        {
            var text = new List<string>();
            text.AddRange(projectItem.GetProjectItemHeader());
            if(projectItem.ProjectTypeId == VsSolutionProjectTypeIds.VsSolutionProjectTypeSolutionFolder)
            {
                text.AddRange(GetProjectItemContentFolderSolutionFiles(projectItem.Files));
            }
            text.AddRange(projectItem.GetProjectItemFooter());

            return text.ToArray();
        }

        internal static string[] GetProjectItemHeader(this VsSolutionFileProject projectItem)
        {
            return new string[]
            {
                "Project(\"{" + projectItem.ProjectTypeId.ToString().ToUpper() + "}\") = \"" + projectItem.ProjectName + "\", \"" + projectItem.ProjectPath + "\", \"{" + projectItem.ProjectId.ToString().ToUpper() + "}\""
            };
        }

        internal static string[] GetProjectItemContentFolderSolutionFiles(Dictionary<string, string> values)
        {
            if (values.Count > 0)
            {
                return GetProjectItemContent("SolutionItems", "preProject", values);
            }
            else
                return new string[] { };
        }

        internal static string[] GetProjectItemContent(string projectSection, string prePostProject, Dictionary<string, string> values)
        {
            var text = new List<string>();

            //"ProjectSection(SolutionItems) = preProject",
            text.Add("\tProjectSection(" + projectSection + ") = " + prePostProject);
            text.AddRange(values.Select(item => "\t\t" + item.Key + " = " + item.Value));
            text.Add("\tEndProjectSection");
            
            return text.ToArray();
        }

        internal static string[] GetProjectItemFooter(this VsSolutionFileProject projectItem)
        {
            return new string[]
            {
                "EndProject"
            };

        }

        internal static string[] GetGlobalSections(this VsSolutionFile sln)
        {
            var text = new List<string>();

            text.Add("Global");
            foreach (var item in sln.GlobalSections)
            {
                text.AddRange(item.Value.GetLines());
            }
            text.Add("EndGlobal");

            return text.ToArray();
        }

        internal static string[] GetLines(this VsSolutionFileGlobalSection gs)
        {
            var text = new List<string>();

            text.Add($"\tGlobalSection({gs.Name}) = {gs.SectionPrePost}");
            foreach(var item in gs.Items)
            {
                text.Add($"\t\t{item.Key} = {item.Value}");
            }
            text.Add($"\tEndGlobalSection");

            return text.ToArray();
        }

        internal static string[] GetSolutionsFileLines(this VsSolutionFile sln)
        {
            var text = new List<string>();

            text.AddRange(sln.GetHeader());
            text.AddRange(sln.GetProjects());
            text.AddRange(sln.GetGlobalSections());

            return text.ToArray();
        }


        public static string Save(this VsSolutionFile sln)
        {
            var filepath = sln.SolutionFolder + @"\" + sln.Filename;
            return SaveAs(sln, filepath);
        }

        public static string SaveAs(this VsSolutionFile sln, string filepath)
        {
            var text = new List<string>();

            System.IO.File.WriteAllLines(filepath, sln.GetSolutionsFileLines());

            return filepath;
        }


        internal static void Add(this Dictionary<string, VsSolutionFileGlobalSection> list, string name, PrePostSolution prePost, string[][] values = null)
        {
            var item = new VsSolutionFileGlobalSection(name, prePost);
            if (values != null)
            {
                foreach (var value in values)
                {
                    item.Items.Add(value[0], value[1]);
                }
            }

            list.Add(item.Name, item);
        }


    }
}
