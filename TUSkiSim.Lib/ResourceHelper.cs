using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public static class ResourceHelper
    {
        public static IEnumerable<string> GetEmbeddedResourceLines(Assembly assembly, string resourceName)
        {                       
            using (var stream = assembly.GetManifestResourceStream(resourceName))   
            using (var reader = new StreamReader(stream))                           
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return line;
            }
        }
    }
}
