using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaObjects.VisualStudio.Tools
{
    public class VsSolutionFileProject
    {
        public Guid ProjectTypeId { get; set; }
        public String ProjectName { get; set; }
        public String ProjectPath { get; set; }
        public Guid ProjectId { get; set; }
        public Dictionary<string, string> Files { get; set; }
        public Guid ParentPoject { get; internal set; }
        public VsSolutionFile Solution { get; internal set; }

        public VsSolutionFileProject()
        {
            Files = new Dictionary<string, string>();
        }
    }
}
