using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaObjects.VisualStudio.Tools
{
    public enum PrePostSolution
    {
        preSolution,
        postSolution
    }

    public class VsSolutionFileGlobalSection
    {
        public readonly string Name;
        public readonly PrePostSolution SectionPrePost;
        public readonly Dictionary<string, string> Items; 
        
        public VsSolutionFileGlobalSection(string name, PrePostSolution prePost)
        {
            Name = name;
            SectionPrePost = prePost;
            Items = new Dictionary<string, string>();
        }
    }
}
