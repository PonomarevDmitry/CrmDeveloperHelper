using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public sealed class PluginsAndWorkflowLoader
    {
        private static readonly string[] AssemblyProbeSubdirectories = new string[4]
        {
            string.Empty,
            "amd64",
            "i386",
            "$(VSCRMTFDEVTOOLSROOT)\\..\\private\\lib"
        };

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

            Type[] loadedAssemblyTypes = assembly.GetExportedTypes();

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
            var message = new StringBuilder();

            message.AppendFormat("Resolve Assembly {0}", args.Name);

            if (args.RequestingAssembly != null)
            {
                message
                    .AppendLine()
                    .AppendFormat("Requesting Assembly {0}", args.RequestingAssembly.FullName)
                    ;
            }

            Assembly result = null;

            var assemblyName = new AssemblyName(args.Name);

            foreach (var knownedAssemblyName in _knownCrmAssemblies)
            {
                if (string.Equals(assemblyName.Name, knownedAssemblyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    var temp = Assembly.Load(knownedAssemblyName + ", Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL");

                    message
                        .AppendLine()
                        .AppendFormat("Resolving from Knowed Assembly {0}", temp.FullName)
                        .AppendLine()
                        .AppendFormat("Resolving from path {0}", temp.CodeBase)
                        ;

                    result = Assembly.ReflectionOnlyLoadFrom(temp.CodeBase);
                }
            }

            if (result == null)
            {
                string fileName = assemblyName.Name + ".dll";

                foreach (string probeSubdirectory in AssemblyProbeSubdirectories)
                {
                    string filePath = Path.Combine(Path.Combine(_assemblyDirectory, probeSubdirectory), fileName);

                    if (File.Exists(filePath))
                    {
                        message
                            .AppendLine()
                            .AppendFormat("Resolving from File {0}", filePath)
                            ;

                        Assembly assembly2 = Assembly.Load(AssemblyName.GetAssemblyName(filePath));

                        if (assembly2 != null)
                        {
                            message
                                .AppendLine()
                                .AppendFormat("Resolving from CodeBase : {0}", assembly2.CodeBase)
                                ;

                            result = Assembly.ReflectionOnlyLoadFrom(assembly2.CodeBase);
                        }
                    }
                }
            }

            if (result == null)
            {
                message
                    .AppendLine()
                    .Append("Resolving by Default")
                    ;

                var temp = Assembly.Load(args.Name);

                if (temp != null)
                {
                    message
                        .AppendLine()
                        .AppendFormat("Resolving by Default from : {0}", temp.CodeBase)
                        ;

                    result = Assembly.ReflectionOnlyLoadFrom(temp.CodeBase);
                }
            }

            message.AppendLine().AppendLine();

            if (result != null)
            {
                message.Append("Assembly resolved.");
            }
            else
            {
                message.Append("Assembly NOT RESOLVED.");
            }

            DTEHelper.WriteToLog(message.ToString());

            return result;
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