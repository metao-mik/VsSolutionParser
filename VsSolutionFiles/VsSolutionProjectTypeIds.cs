using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaObjects.VisualStudio.Tools
{
    public static class VsSolutionProjectTypeIds
    {
        public static Guid VsSolutionProjectTypeCSharp = Guid.Parse("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC");
        public static Guid VsSolutionProjectTypeSolutionFolder = Guid.Parse("2150E333-8FDC-42A3-9474-1A3956D46DE8");

        public static Guid GetProjectTypeIdByFileExtention(string extention)
        {
            extention = extention.ToLower();
            if(!extention.StartsWith(".")) { extention = "." + extention;  }

            switch (extention)
            {
                case ".csproj":
                    return VsSolutionProjectTypeIds.VsSolutionProjectTypeCSharp;
                    
                default:
                    return Guid.Empty;
            }
        }

        public static Guid GetProjectTypeIdByFilepath(string filepath)
        {
            var extention = Path.GetExtension(filepath);
            return GetProjectTypeIdByFileExtention(extention);
        }

    }
}
