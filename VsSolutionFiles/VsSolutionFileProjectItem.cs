using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaObjects.VisualStudio.Tools
{
    public class VsSolutionFileProjectItem
    {
        public Guid ProjectTypeId { get; set; }
        public String ProjectName { get; set; }
        public String ProjectPath { get; set; }
        public Guid ProjectId { get; set; }
        public Dictionary<string, string> Files { get; set; }
        public string ParentPoject { get; internal set; }

        public VsSolutionFileProjectItem()
        {
            Files = new Dictionary<string, string>();
        }
    }
}
