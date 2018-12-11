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

        public static VsSolutionFile Open(string filePath)
        {
            var sln = new VsSolutionFile(filePath);

            sln.ReadSolutionFile();

            return sln;
        }
    }
}
