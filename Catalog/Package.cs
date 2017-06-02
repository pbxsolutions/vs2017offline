using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs2017offline.Catalog
{
    public class Package
    {
        public string Id;
        public string Version;
        public string Name;
        public string Type;
        public string Language;
        public long InstallSize;
        public List<Payload> Payloads;
        public Dictionary<string, Dependency> Dependencies;

        public static string GetSize(float size)
        {
            size = size / 1024f;
            string sufix = "Kb";

            if (size > 1024)
            {
                size = size / 1024f;
                sufix = "Mb";
            }

            if (size > 1024)
            {
                size = size / 1024f;
                sufix = "Gb";
            }

            return $"{size} {sufix}";
        }
    }
}
