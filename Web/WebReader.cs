using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vs2017offline.Analysis;

namespace vs2017offline.Web
{
    public class WebReader
    {
        protected StreamReader _sr;
        protected int _n;
        protected string _text = "";

        public void ReadLine()
        {
            while (!_sr.EndOfStream)
            {
                ++_n;
                _text = _sr.ReadLine().Trim();

                if (_text != "") break;

            }
        }

        public Dictionary<string, string> _sources = new Dictionary<string, string>();

                    

        public WebReader()
        {
            //VS 2017 15.2
            _sources.Add("Microsoft.VisualStudio.Product.Enterprise", "enterprise workloads.txt");
            _sources.Add("Microsoft.VisualStudio.Product.Professional", "professional workloads.txt");
            _sources.Add("Microsoft.VisualStudio.Product.Community", "community workloads.txt"); 
            _sources.Add("Microsoft.VisualStudio.Product.TeamExplorer", "team explorer workloads.txt");
            _sources.Add("Microsoft.VisualStudio.Product.BuildTools", "build tools workloads.txt");
            _sources.Add("Microsoft.VisualStudio.Product.TestAgent", "test agent workloads.txt");
            _sources.Add("Microsoft.VisualStudio.Product.TestController", "test controller workloads.txt");
            _sources.Add("Microsoft.VisualStudio.Product.TestProfessional", "test professional workloads.txt");
            _sources.Add("Microsoft.VisualStudio.Product.FeedbackClient", "feedback client workloads.txt");

        }

        public void Populate(string product, Dictionary<string, Workload> workloads, Dictionary<string, Component> unaffiliated, Dictionary<string, Component> components)
        {
            string basePath = @"..\..\Data\";

            _sr = new StreamReader($"{basePath}{_sources[product]}");


            Workload workload = null;

            while (!_sr.EndOfStream)
            {
                ReadLine();

                if ((_text.StartsWith("ID: ") && _text.Contains(".Workload.")))
                {

                    workload = new Workload();
                    workload.Id = _text.Replace("ID: ", "").Trim();

                    ReadLine();
                    string descrription = _text.Replace("Description: ", "").Trim();

                    workloads.Add(workload.Id, workload);

                }
                else
                {
                    var parts = _text.Split('\t');

                    Component component = null;

                    if (parts.Length == 4 && parts[0].Contains(".") && IsDependency(parts[3]))
                    {
                        component = new Component();
                        component.Id = parts[0].Trim();
                        string escription = parts[1].Trim();
                        component.Version = parts[2].Trim();
                        component.Dependency = parts[3].Trim();

                        workload.Components.Add(component.Id, component);
                    }
                    else if (parts.Length == 3 && parts[0].Contains("."))
                    {
                        component = new Component();
                        component.Id = parts[0].Trim();
                        string description = parts[1].Trim();
                        component.Version = parts[2].Trim();
                        component.Dependency = "None";

                        unaffiliated.Add(component.Id, component);
                    }

                    if (component != null && !components.ContainsKey(component.Id))
                    {
                        components.Add(component.Id, component);
                    }


                }



            }

            _sr.Close();

        }

            public bool IsDependency(string text)
            {
                if (text == "Required" || text == "Recommended" || text == "Optional")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
}
