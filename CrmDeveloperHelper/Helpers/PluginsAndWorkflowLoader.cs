using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public sealed class PluginsAndWorkflowLoader
    {
        private string _assemblyDirectory;

        public PluginsAndWorkflowLoader()
        {

        }

        public AssemblyReaderResult LoadAssembly(string assemblyPath)
        {
            this._assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= Domain_ReflectionOnlyAssemblyResolve;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= Domain_ReflectionOnlyAssemblyResolve;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += Domain_ReflectionOnlyAssemblyResolve;

            var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);

            var assemblyName = assembly.GetName();

            AssemblyReaderResult result = new AssemblyReaderResult()
            {
                FilePath = assemblyPath,
                FileName = Path.GetFileName(assemblyPath),

                Name = assemblyName.Name,
                FullName = assemblyName.FullName,

                Version = assemblyName.Version.ToString(),
            };

            if (assemblyName.CultureInfo.LCID == System.Globalization.CultureInfo.InvariantCulture.LCID)
            {
                result.Culture = "neutral";
            }
            else
            {
                result.Culture = assemblyName.CultureInfo.Name;
            }

            byte[] tokenBytes = assemblyName.GetPublicKeyToken();
            if (tokenBytes == null || tokenBytes.Length == 0)
            {
                result.PublicKeyToken = null;
            }
            else
            {
                result.PublicKeyToken = string.Join(string.Empty, tokenBytes.Select(b => b.ToString("X2")));
            }

            HashSet<string> assemblyPlugins = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            HashSet<string> assemblyWorkflow = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            Type[] loadedAssemblyTypes = assembly.GetTypes();

            if (loadedAssemblyTypes != null)
            {
                foreach (var assemblyType in loadedAssemblyTypes)
                {
                    if (assemblyType.IsAbstract || !assemblyType.IsVisible || assemblyType.FullName.Contains("<>c"))
                    {
                        continue;
                    }

                    if (IsPluginClass(assemblyType))
                    {
                        assemblyPlugins.Add(assemblyType.FullName);
                    }
                    else if (IsSubClassOfCodeActivity(assemblyType))
                    {
                        assemblyWorkflow.Add(assemblyType.FullName);
                    }
                }
            }

            result.Plugins = new List<string>(assemblyPlugins.OrderBy(s => s));
            result.Workflows = new List<string>(assemblyWorkflow.OrderBy(s => s));

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= Domain_ReflectionOnlyAssemblyResolve;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= Domain_ReflectionOnlyAssemblyResolve;

            return result;
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

        private static bool IsPluginClass(Type assemblyType)
        {
            var interfaces = assemblyType.GetInterfaces();

            foreach (var item in interfaces)
            {
                if (string.Equals(item.FullName, "Microsoft.Xrm.Sdk.IPlugin")
                    || string.Equals(item.FullName, "Microsoft.Xrm.Sdk.IPluginExecutionContext")
                )
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSubClassOfCodeActivity(Type assemblyType)
        {
            if (assemblyType.BaseType != null)
            {
                if (string.Equals(assemblyType.BaseType.FullName, "System.Activities.CodeActivity"))
                {
                    return true;
                }

                return IsSubClassOfCodeActivity(assemblyType.BaseType);
            }

            return false;
        }
    }
}