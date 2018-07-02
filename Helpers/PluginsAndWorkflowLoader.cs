using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class PluginsAndWorkflowLoader
    {
        private string _assemblyPath;
        private string _assemblyDirectory;

        public Tuple<HashSet<string>, HashSet<string>> LoadAssembly(string assemblyPath)
        {
            this._assemblyPath = assemblyPath;
            this._assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            HashSet<string> assemblyPlugins = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            HashSet<string> assemblyWorkflow = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += Domain_ReflectionOnlyAssemblyResolve;

            var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);

            Type[] loadedAssemblyTypes = assembly.GetTypes();

            if (loadedAssemblyTypes != null)
            {
                foreach (var assemblyType in loadedAssemblyTypes)
                {
                    if (assemblyType.IsAbstract)
                    {
                        continue;
                    }

                    if (assemblyType.FullName.Contains("<>c"))
                    {
                        continue;
                    }

                    var interfaces = assemblyType.GetInterfaces();

                    foreach (var item in interfaces)
                    {
                        if (item.FullName.Equals("Microsoft.Xrm.Sdk.IPlugin")
                            || item.FullName.Equals("Microsoft.Xrm.Sdk.IPluginExecutionContext")
                            )
                        {
                            assemblyPlugins.Add(assemblyType.FullName);
                            break;
                        }
                    }

                    if (IsSubClassOfCodeActivity(assemblyType))
                    {
                        assemblyWorkflow.Add(assemblyType.FullName);
                    }
                }
            }

            return Tuple.Create(assemblyPlugins, assemblyWorkflow);
        }

        private Assembly Domain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly result = null;

            if (args.Name.Contains("Microsoft.Xrm.Sdk"))
            {
                var temp = Assembly.Load("Microsoft.Xrm.Sdk, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL");

                result = Assembly.ReflectionOnlyLoadFrom(temp.CodeBase);
            }
            else if (args.Name.Contains("Microsoft.Xrm.Sdk.Workflow"))
            {
                var temp = Assembly.Load("Microsoft.Xrm.Sdk.Workflow, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL");

                result = Assembly.ReflectionOnlyLoadFrom(temp.CodeBase);
            }
            else if (args.Name.Contains("Microsoft.Crm.Sdk.Proxy"))
            {
                var temp = Assembly.Load("Microsoft.Crm.Sdk.Proxy, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL");

                result = Assembly.ReflectionOnlyLoadFrom(temp.CodeBase);
            }
            else
            {
                var assemblyName = new AssemblyName(args.Name);

                var filePath = Path.Combine(_assemblyDirectory, assemblyName.Name + ".dll");

                if (File.Exists(filePath))
                {
                    result = Assembly.ReflectionOnlyLoadFrom(filePath);
                }
            }


            if (result == null)
            {
                var temp = Assembly.Load(args.Name);

                result = Assembly.ReflectionOnlyLoadFrom(temp.CodeBase);
            }

            return result;
        }

        private bool IsSubClassOfCodeActivity(Type item)
        {
            if (item.BaseType != null)
            {
                if (item.BaseType.FullName.Equals("System.Activities.CodeActivity"))
                {
                    return true;
                }

                return IsSubClassOfCodeActivity(item.BaseType);
            }

            return false;
        }
    }
}