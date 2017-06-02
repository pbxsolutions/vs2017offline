using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vs2017offline.Analysis;

namespace vs2017offline.Catalog
{
    public class JsonReader 
    {
        public List<string> _products = new List<string>();
        public List<string> Products => _products;

        public Dictionary<string, Package> _packages = new Dictionary<string, Package>();
        public Dictionary<string, Package> Packages => _packages;

        public void Read(string file, string language= "en-US")
        {

            var sr = new StreamReader(file);
            string json = sr.ReadToEnd();
            sr.Close();

            var catalog = new
            {
                info = new { productName = "" },
                packages = new[] { new { id = "", version = "", type="", installSize = 0L, name = "", relativePath="", language="",
                    payloads =new[] { new {fileName="",size=0L,url="" } },
                    dependencies=new Dictionary<string, object>()

                }
                }
            };

            catalog = JsonConvert.DeserializeAnonymousType(json, catalog);
            sr.Close();

            foreach (var package in catalog.packages)
            {

                if (package.language == null || package.language == language)
                {

                    var p = new Package() { Id = package.id, Version = package.version, Language = package.language, Name = package.name, InstallSize = package.installSize, Type = package.type };

                    if (p.Type == "Product")
                    {
                        _products.Add(p.Id);
                    }


                    if (package.payloads != null)
                    {
                        p.Payloads = new List<Payload>();
                        foreach (var payload in package.payloads)
                        {
                            p.Payloads.Add(new Payload() { FileName = payload.fileName, Url = payload.url, Size = payload.size });
                        }
                    }

                    if (package.dependencies != null)
                    {
                        p.Dependencies = new Dictionary<string, Dependency>();

                        foreach (var dep in package.dependencies.Keys)
                        {
                            object o = package.dependencies[dep];
                            Type t = o.GetType();

                            string version = null;
                            string type = null;

                            if (o is string)
                            {
                                version = o as string;
                            }
                            else if (o is JObject)
                            {
                                JObject jobj = o as JObject;
                                version = jobj.GetValue("version")?.ToString();
                                type = jobj.GetValue("type")?.ToString();
                            }

                            p.Dependencies.Add(dep, new Dependency() { Id = dep, Version = version, Type = type });

                        }
                    }

                    if (!_packages.ContainsKey(p.Id))
                    {
                        _packages.Add(p.Id, p);
                    }

                }
            } //end cache

           


        }

        public void Populate(string productName, Dictionary<string, Workload> workloads, Dictionary<string, Component> unaffiliated, Dictionary<string, Component> components)
        {
            Read(@"..\..\Data\catalog.json");


            var product = Packages[productName];

            foreach (var dependency in product.Dependencies.Values)
            {
                var package = Packages[dependency.Id];

                if (package.Type == "Workload")
                {
                    var workload = new Workload();
                    workload.Id = package.Id;
                    workloads.Add(workload.Id, workload);
                }
                else if (package.Type == "Component")
                {
                    var component = new Component() { Id = package.Id, Version = package.Version, Type = package.Type, Dependency = null };
                    components.Add(component.Id, component);
                }
            }



            foreach (var workload in workloads.Values)
            {
                var wlPackage = Packages[workload.Id];

                foreach (var dependency in wlPackage.Dependencies.Values)
                {
                    if (Packages.ContainsKey(dependency.Id))
                    {
                        var cPackage = Packages[dependency.Id];
                        if (cPackage.Type == "Component")
                        {
                            var dep = string.IsNullOrEmpty(dependency.Type?.Trim()) ? "Required" : dependency.Type;
                            var component = new Component() { Id = cPackage.Id, Version = cPackage.Version, Type = cPackage.Type, Dependency = dep };
                            workload.Components.Add(component.Id, component);

                            if (!components.ContainsKey(component.Id))
                            {
                                components.Add(component.Id, component);
                            }

                        }
                    }

                }


            }
        }

    }
}
