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
            return GetVsSolutionHeaderFormatVS12();
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

        internal static string[] GetProjectItemHeader(this VsSolutionFileProjectItem projectItem)
        {
            return new string[]
            {
                "Project(\"{" + projectItem.ProjectTypeId + "}\") = \"" + projectItem.ProjectName + "\", \"" + projectItem.ProjectPath + "\", \"{" + projectItem.ProjectId + "}\""
            };
        }

        internal static string[] GetProjectItemContentFolderSolutionFiles(Dictionary<string, string> values)
        {
            return GetProjectItemContent("SolutionsItems", "preProject", values);
        }

        internal static string[] GetProjectItemContent(string projectSection, string prePostProject, Dictionary<string, string> values)
        {
            var text = new List<string>();

            //"ProjectSection(SolutionItems) = preProject",
            text.Add("ProjectSection(" + projectSection + ") = " + prePostProject);
            text.AddRange(values.Select(item => "\t" + item.Key + " = " + item.Value));
            text.Add("EndProjectSection");
            
            return text.ToArray();
        }

        internal static string[] GetProjectItemFooter(this VsSolutionFileProjectItem projectItem)
        {
            return new string[]
            {
                "EndProject"
            };

        }


    }
}
