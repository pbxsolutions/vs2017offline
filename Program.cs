using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vs2017offline.Analysis;

namespace vs2017offline
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Workload> workloads = new Dictionary<string, Workload>();
            Dictionary<string, Component> unaffiliated = new Dictionary<string, Component>();
            Dictionary<string, Component> components = new Dictionary<string, Component>();

            /*
                Products:
                    Microsoft.VisualStudio.Product.BuildTools
                    Microsoft.VisualStudio.Product.Community
                    Microsoft.VisualStudio.Product.Enterprise
                    Microsoft.VisualStudio.Product.FeedbackClient
                    Microsoft.VisualStudio.Product.Professional
                    Microsoft.VisualStudio.Product.TeamExplorer
                    Microsoft.VisualStudio.Product.TestAgent
                    Microsoft.VisualStudio.Product.TestController
                    Microsoft.VisualStudio.Product.TestProfessional

            
                Community Componenst:
                   Microsoft.VisualStudio.Branding.Community
                   Microsoft.VisualStudio.Workload.CoreEditor
                   Microsoft.VisualStudio.Workload.Universal
                   Microsoft.VisualStudio.Workload.ManagedDesktop
                   Microsoft.VisualStudio.Workload.NativeDesktop
                   Microsoft.VisualStudio.Workload.NetWeb
                   Microsoft.VisualStudio.Workload.Azure
                   Microsoft.VisualStudio.Workload.Python
                   Microsoft.VisualStudio.Workload.Node
                   Microsoft.VisualStudio.Workload.Data
                   Microsoft.VisualStudio.Workload.DataScience
                   Microsoft.VisualStudio.Workload.Office
                   Microsoft.VisualStudio.Workload.NetCrossPlat
                   Microsoft.VisualStudio.Workload.ManagedGame
                   Microsoft.VisualStudio.Workload.WebCrossPlat
                   Microsoft.VisualStudio.Workload.NativeMobile
                   Microsoft.VisualStudio.Workload.NativeGame
                   Microsoft.VisualStudio.Workload.VisualStudioExtension
                   Microsoft.VisualStudio.Workload.NativeCrossPlat
                   Microsoft.VisualStudio.Workload.NetCoreTools

            */

            var product = "Microsoft.VisualStudio.Product.Community";

            //var reader = new Catalog.JsonReader();
            var reader = new Web.WebReader();

            reader.Populate(product, workloads, unaffiliated, components);

            var analyzer = new Analyzer();
            analyzer.Analyze(product, workloads, unaffiliated, components);


        }
    }
}
