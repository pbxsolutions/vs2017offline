using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs2017offline.Analysis
{
    public class Workload
    {
        public string Id;
        public Dictionary<string, Component> Components = new Dictionary<string, Component>();
    }
}
