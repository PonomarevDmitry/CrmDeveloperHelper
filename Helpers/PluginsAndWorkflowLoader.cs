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

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= Domain_ReflectionOnlyAssemblyResolve;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= Domain_ReflectionOnlyAssemblyResolve;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += Domain_ReflectionOnlyAssemblyResolve;

            var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);

            Type[] loadedAssemblyTypes = assembly.GetTypes();

            if (loadedAssemblyTypes != null)
            {
                foreach (var assemblyType in loadedAssemblyTypes)
                {
                    if (assemblyType.IsAbstract || !assemblyType.IsVisible || assemblyType.FullName.Contains("<>c"))
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

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= Domain_ReflectionOnlyAssemblyResolve;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= Domain_ReflectionOnlyAssemblyResolve;

            return Tuple.Create(assemblyPlugins, assemblyWorkflow);
        }

        private static readonly string[] _knownCrmAssemblies = { "Microsoft.Xrm.Sdk", "Microsoft.Xrm.Sdk.Workflow", "Microsoft.Crm.Sdk.Proxy", "Microsoft.Xrm.Sdk.Data", "Microsoft.Xrm.Sdk.Deployment" };

        private Assembly Domain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);

            foreach (var knownedAssemblyName in _knownCrmAssemblies)
            {
                if (string.Equals(assemblyName.Name, knownedAssemblyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    var temp = Assembly.Load(knownedAssemblyName + ", Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL");

                    return Assembly.ReflectionOnlyLoadFrom(temp.CodeBase);
                }
            }

            {
                var filePath = Path.Combine(_assemblyDirectory, assemblyName.Name + ".dll");

                if (File.Exists(filePath))
                {
                    return Assembly.ReflectionOnlyLoadFrom(filePath);
                }
            }

            return Assembly.ReflectionOnlyLoadFrom(Assembly.Load(args.Name).CodeBase);
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