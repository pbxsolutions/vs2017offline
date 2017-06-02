using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs2017offline.Analysis
{
    public class Analyzer
    {
        protected StreamReader _sr;
        protected StreamWriter _sw;
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

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
            if (_sw != null) _sw.WriteLine(text);
        }

        public void Pretty(string product, Dictionary<string, Workload> workloads, Dictionary<string, Component> unaffiliated)
        {
            int c = 0;

            foreach (var wl in workloads.Values)
            {
                WriteLine($"{wl.Id}");

                foreach (var component in wl.Components.Values)
                {
                    c += wl.Components.Count;
                    WriteLine($"\t{component.Id}");// - {wl?.Description} [{wl.Line}]");
                }

            }

            WriteLine($"Unaffiliated Components");

            foreach (var comp in unaffiliated.Values)
            {
                WriteLine($"\t{comp.Id}");// - {wl?.Description} [{wl.Line}]");
            }



            WriteLine($"{workloads.Count} workloads, with {c} components.");
            WriteLine($"{unaffiliated.Count} unaffiliated components.");
        }

        public void Csv(string product, Dictionary<string, Workload> workloads, Dictionary<string, Component> unaffiliated)
        {
            _sw = new StreamWriter($"{product}.csv");

            WriteLine($"Workload Id, Component Id, Version, Type");

            foreach (var wl in workloads.Values)
            {
                foreach (var component in wl.Components.Values)
                {
                    WriteLine($"{wl.Id}, {component.Id}, {component.Version}, {component.Dependency}");
                }

            }

            _sw.Close();
            _sw = null;
        }

        private class WL
        {
            public string Id;
            public bool Optional;
            public bool Recommended;
        }

        public void Analyze(string product, Dictionary<string, Workload> workloads, Dictionary<string, Component> unaffiliated, Dictionary<string, Component> components)
        {
            WL[] installWorkloads =
            {
                //new WL() { Id = "Microsoft.VisualStudio.Workload.CoreEditor", Recommended=true, Optional=true},
                //new WL() { Id = "Microsoft.VisualStudio.Workload.Azure"},
                //new WL() { Id = "Microsoft.VisualStudio.Workload.Data"},
                //new WL() { Id = "Microsoft.VisualStudio.Workload.DataScience"},
                //new WL() { Id = "Microsoft.VisualStudio.Workload.ManagedDesktop", Recommended=true, Optional=true}, //net windows
                //new WL() { Id = "Microsoft.VisualStudio.Workload.ManagedGame"}, //unity
                //new WL() { Id = "Microsoft.VisualStudio.Workload.NativeCrossPlat", Recommended=true, Optional=true}, //linux
                //new WL() { Id = "Microsoft.VisualStudio.Workload.NativeDesktop"}, //c++ windows
                //new WL() { Id = "Microsoft.VisualStudio.Workload.NativeGame"}, //c++ game
                //new WL() { Id = "Microsoft.VisualStudio.Workload.NativeMobile"}, //android ndk
                //new WL() { Id = "Microsoft.VisualStudio.Workload.NetCoreTools", Recommended=true, Optional=true},
                //new WL() { Id = "Microsoft.VisualStudio.Workload.NetCrossPlat"}, //xamarin
                //new WL() { Id = "Microsoft.VisualStudio.Workload.NetWeb", Recommended=true, Optional=true}, //asp
                //new WL() { Id = "Microsoft.VisualStudio.Workload.Node"},
                //new WL() { Id = "Microsoft.VisualStudio.Workload.Office", Recommended=true, Optional=true},
                //new WL() { Id = "Microsoft.VisualStudio.Workload.Python"},
                //new WL() { Id = "Microsoft.VisualStudio.Workload.Universal"},
                //new WL() { Id = "Microsoft.VisualStudio.Workload.VisualStudioExtension", Recommended=true, Optional=true},
                //new WL() { Id = "Microsoft.VisualStudio.Workload.WebCrossPlat"} //javascript
            };



            string[] installComponents =
            {
            /*
                //from Microsoft.VisualStudio.Workload.NativeDesktop
                "Microsoft.VisualStudio.Component.Graphics.Tools",
                "Microsoft.VisualStudio.Component.Graphics.Win81",
                "Microsoft.VisualStudio.Component.VC.CMake.Project",
                "Microsoft.VisualStudio.Component.VC.DiagnosticTools",
                "Microsoft.VisualStudio.Component.VC.Tools.x86.x64",
                "Component.Incredibuild",
                "Microsoft.VisualStudio.Component.VC.140",
                "Microsoft.VisualStudio.Component.VC.ATL",
                "Microsoft.VisualStudio.Component.VC.ATLMFC",
                "Microsoft.VisualStudio.Component.VC.ClangC2",
                "Microsoft.VisualStudio.Component.VC.CLI.Support",
                "Microsoft.VisualStudio.Component.VC.Modules.x86.x64",
                "Microsoft.VisualStudio.Component.WinXP",
                "Microsoft.VisualStudio.ComponentGroup.NativeDesktop.WinXP",
                "Microsoft.VisualStudio.ComponentGroup.NativeDesktop.Win81",
                "Microsoft.VisualStudio.Component.Windows10SDK.15063.Desktop",
                //native mobile
                "Component.MDD.Android",
                "Component.MDD.IOS",
                //managed mobile
                "Microsoft.Component.NetFX.Native",
                "Microsoft.VisualStudio.Component.Graphics",
                "Component.Xamarin",
                "Component.Xamarin.Inspector",
                "Component.Xamarin.Profiler",
                "Component.Xamarin.RemotedSimulator",
                //azure
                "Microsoft.VisualStudio.Component.Azure.AuthoringTools",
                "Microsoft.VisualStudio.Component.Azure.Compute.Emulator",
                "Microsoft.VisualStudio.Component.Azure.MobileAppsSdk",
                "Microsoft.VisualStudio.Component.Azure.ResourceManager.Tools",
                "Microsoft.VisualStudio.Component.Azure.ServiceFabric.Tools",
                "Microsoft.VisualStudio.Component.Azure.Storage.Emulator",
                "Microsoft.VisualStudio.Component.Azure.Waverton",
                "Microsoft.VisualStudio.ComponentGroup.Azure.CloudServices",
                "Microsoft.VisualStudio.ComponentGroup.Azure.ResourceManager.Tools",
                "Microsoft.VisualStudio.Component.Azure.Storage.AzCopy",
                "Microsoft.VisualStudio.Component.PowerShell.Tools",
                //data
                "Component.Redgate.ReadyRoll",
                "Component.Redgate.SQLPrompt.VsPackage",
                "Component.Redgate.SQLSearch.VSExtension",
                //web cross platform
                "Microsoft.VisualStudio.Component.Git",
                //python
                "Microsoft.Component.CookiecutterTools",
                "Microsoft.Component.PythonTools",
                "Microsoft.Component.PythonTools.Web",
                "Microsoft.Component.VC.Runtime.UCRTSDK",
                "Component.CPython2.x64",
                "Component.CPython2.x86",
                "Component.CPython3.x86",
                "Component.CPython3.x64",
                //universal
                "Microsoft.VisualStudio.Component.VC.Tools.ARM"

             */
            };



            string[] analyzeWorkloads =
            {
                //* "Microsoft.VisualStudio.Workload.CoreEditor",
                //* "Microsoft.VisualStudio.Workload.Azure",
                //* "Microsoft.VisualStudio.Workload.Data",
                //* "Microsoft.VisualStudio.Workload.DataScience",
               "Microsoft.VisualStudio.Workload.ManagedDesktop", //net windows
                //* "Microsoft.VisualStudio.Workload.ManagedGame", //unity
                //* "Microsoft.VisualStudio.Workload.NativeCrossPlat", //linux
                //* "Microsoft.VisualStudio.Workload.NativeDesktop", //c++ windows
                //* "Microsoft.VisualStudio.Workload.NativeGame", //c++ game
                //* "Microsoft.VisualStudio.Workload.NativeMobile", //android ndk
                //* "Microsoft.VisualStudio.Workload.NetCoreTools", 
                //* "Microsoft.VisualStudio.Workload.NetCrossPlat", //xamarin
                //* "Microsoft.VisualStudio.Workload.NetWeb", //asp
                //* "Microsoft.VisualStudio.Workload.Node",
                //* "Microsoft.VisualStudio.Workload.Office",
                //* "Microsoft.VisualStudio.Workload.Python",
                //"Microsoft.VisualStudio.Workload.Universal",
                //* "Microsoft.VisualStudio.Workload.VisualStudioExtension",
                //* "Microsoft.VisualStudio.Workload.WebCrossPlat" //javascript
            };


            //"install" required stuff for selected workloads & components
            Dictionary<string, Component> installed = new Dictionary<string, Component>();

            WriteLine("***********************************************************************");
            WriteLine("********* WHAT IS ALREADY INSTALLED");
            WriteLine("************************************************************************");


            foreach (var wl in installWorkloads)
            {
                WriteLine($"");
                WriteLine("---------------------------------------------------------");
                WriteLine($"--[{wl.Id}]");

                WriteLine($"");
                WriteLine($"\tRequired:");
                WriteLine($"\t--------------");
                foreach (var c in workloads[wl.Id].Components.Values.Where(p => p.Dependency == "Required"))
                {
                    if (!installed.ContainsKey(c.Id))
                    {
                        {
                            WriteLine($"\t\t{c.Id}");
                            installed.Add(c.Id, c);
                        }
                    }
                }

                WriteLine($"");
                WriteLine($"\tRecommended:");
                WriteLine($"\t--------------");
                foreach (var c in workloads[wl.Id].Components.Values.Where(p => p.Dependency == "Recommended"))
                {
                    if (!installed.ContainsKey(c.Id))
                    {
                        if (wl.Recommended)
                        {
                            WriteLine($"\t\t{c.Id}");
                            installed.Add(c.Id, c);
                        }
                    }
                }

                WriteLine($"");
                WriteLine($"\tOptional:");
                WriteLine($"\t--------------");
                foreach (var c in workloads[wl.Id].Components.Values.Where(p => p.Dependency == "Optional"))
                {
                    if (!installed.ContainsKey(c.Id))
                    {
                        if (wl.Optional)
                        {
                            WriteLine($"\t\t{c.Id}");
                            installed.Add(c.Id, c);
                        }
                    }
                }

            }

            //"install" components
            foreach (var cId in installComponents)
            {
                if (components.ContainsKey(cId))
                {
                    var c = components[cId];
                    if (!installed.ContainsKey(c.Id))
                    {
                        installed.Add(c.Id, c);
                    }
                }
            }

            //analyze selected
            WriteLine("");
            WriteLine("***********************************************************************");
            WriteLine("********* ANALYZE SELECTED WORKLOADS");
            WriteLine("************************************************************************");

            foreach (var wl in analyzeWorkloads)
            {
                WriteLine("");
                WriteLine("===============================================");
                WriteLine($"{wl}");

                string cmd = "\tvs_community.exe --layout ..\\community --lang en-US";

                foreach (var c in workloads[wl].Components.Values)
                {
                    if (!installed.ContainsKey(c.Id))
                    {

                        WriteLine($"\t{c.Id} ({c.Dependency})");

                        if (c.Dependency != "Required")
                        {
                            cmd += $" --add {c.Id}";
                        }

                    }

                }

                WriteLine("");
                WriteLine("to install selected workloads:");
                WriteLine("");
                if (installWorkloads.Where(p => p.Id == wl).Count() == 0)
                {
                    WriteLine($"\tvs_community.exe --layout ..\\community --lang en-US --add {wl}");
                }
                WriteLine(cmd);

            }


        }

    }
}
