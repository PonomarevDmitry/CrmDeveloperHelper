using Microsoft.CSharp;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class CSharpCodeHelper
    {
        private static AppDomain CreateChildDomain()
        {
            string path = typeof(CSharpCodeHelper).Assembly.Location;

            string directory = Path.GetDirectoryName(path);

            var setup = new AppDomainSetup
            {
                ApplicationBase = directory,
                CachePath = directory,
                LoaderOptimization = LoaderOptimization.MultiDomain,
            };

            AppDomain childDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), AppDomain.CurrentDomain.Evidence.Clone(), setup);

            return childDomain;
        }

        public static Task<string[]> GetTypeFullNameListAsync(string[] pluginTypesNotCompiled, VSProject2Info[] projectInfos)
        {
            return Task.Run(() => GetTypeFullNameList(pluginTypesNotCompiled, projectInfos));
        }

        private static string[] GetTypeFullNameList(string[] pluginTypesNotCompiled, VSProject2Info[] projectInfos)
        {
            var result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var item in pluginTypesNotCompiled)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    result.Add(item);
                }
            }

            var compiledTypes = GetFileTypeFullName(projectInfos);

            foreach (var item in compiledTypes)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    result.Add(item);
                }
            }

            return result.OrderBy(s => s).ToArray();
        }

        public static async Task<string> GetSingleFileTypeFullNameAsync(string[] pluginTypesNotCompiled, VSProject2Info[] projectInfos)
        {
            if (pluginTypesNotCompiled.Any())
            {
                return pluginTypesNotCompiled.First();
            }
            else
            {
                var compiledFileTypes = await CSharpCodeHelper.GetFileTypeFullNameAsync(projectInfos);

                if (compiledFileTypes.Any())
                {
                    return compiledFileTypes.First();
                }
            }

            return string.Empty;
        }

        public static Task<string[]> GetFileTypeFullNameAsync(IEnumerable<VSProject2Info> projInfoEnum)
        {
            return Task.Run(() => GetFileTypeFullName(projInfoEnum));
        }

        private static string[] GetFileTypeFullName(IEnumerable<VSProject2Info> projInfoEnum)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var projInfo in projInfoEnum)
            {
                AppDomain childDomainWithoutAssemblies = CreateChildDomain();
                AppDomain childDomainWithAssemblies = CreateChildDomain();

                childDomainWithoutAssemblies.SetData(configAssembliesProjects, projInfo.AssembliesProjects.ToArray());

                childDomainWithAssemblies.SetData(configAssembliesProjects, projInfo.AssembliesProjects.ToArray());
                childDomainWithAssemblies.SetData(configAssemblies, projInfo.AssembliesReferences.ToArray());

                try
                {
                    childDomainWithoutAssemblies.DoCallBack(new CrossAppDomainDelegate(FillRefferenceListInAppDomain));
                    childDomainWithAssemblies.DoCallBack(new CrossAppDomainDelegate(FillRefferenceListInAppDomain));

                    foreach (var filePath in projInfo.CSharpFiles)
                    {
                        string fileType = string.Empty;

                        if (string.IsNullOrEmpty(fileType))
                        {
                            childDomainWithoutAssemblies.SetData(configFilePath, filePath);
                            childDomainWithoutAssemblies.DoCallBack(new CrossAppDomainDelegate(GetFileTypeFullNameInAppDomain));

                            fileType = childDomainWithoutAssemblies.GetData(configResult)?.ToString();
                        }

                        if (string.IsNullOrEmpty(fileType))
                        {
                            childDomainWithAssemblies.SetData(configFilePath, filePath);
                            childDomainWithAssemblies.DoCallBack(new CrossAppDomainDelegate(GetFileTypeFullNameInAppDomain));

                            fileType = childDomainWithAssemblies.GetData(configResult)?.ToString();
                        }

                        if (!string.IsNullOrEmpty(fileType))
                        {
                            result.Add(fileType);
                        }
                        else
                        {
                            result.Add(Path.GetFileNameWithoutExtension(filePath).Split('.').FirstOrDefault());
                        }
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
                finally
                {
                    AppDomain.Unload(childDomainWithoutAssemblies);
                    AppDomain.Unload(childDomainWithAssemblies);
                }
            }

            return result.OrderBy(s => s).ToArray();
        }

        private const string configResult = "Result";
        private const string configFilePath = "FilePath";
        private const string configAssembliesProjects = "AssembliesProjects";
        private const string configAssemblies = "Assemblies";

        private static string[] _refferencedAssemblies = null;

        public static void FillRefferenceListInAppDomain()
        {
            try
            {
                var assembliesProjects = (string[])AppDomain.CurrentDomain.GetData(configAssembliesProjects);

                var resolver = new AssemblyResolver(assembliesProjects.Select(e => Path.GetDirectoryName(e)).ToArray());

                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolver.Domain_ReflectionOnlyAssemblyResolve;
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolver.Domain_ReflectionOnlyAssemblyResolve;
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolver.Domain_ReflectionOnlyAssemblyResolve;
                AppDomain.CurrentDomain.AssemblyResolve -= resolver.Domain_AssemblyResolve;
                AppDomain.CurrentDomain.AssemblyResolve -= resolver.Domain_AssemblyResolve;
                AppDomain.CurrentDomain.AssemblyResolve += resolver.Domain_AssemblyResolve;

                HashSet<string> list = new HashSet<string>(assembliesProjects, StringComparer.InvariantCultureIgnoreCase);

                var hash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

                var configAssembliesObject = AppDomain.CurrentDomain.GetData(configAssemblies);

                if (configAssembliesObject != null && configAssembliesObject is string[] assemblies)
                {
                    foreach (var item in assemblies)
                    {
                        Assembly assembly = Assembly.ReflectionOnlyLoadFrom(item);

                        if (hash.Add(Path.GetFileName(item)))
                        {
                            list.Add(item);
                        }
                    }

                    foreach (var item in assemblies)
                    {
                        Assembly assembly = Assembly.ReflectionOnlyLoadFrom(item);

                        var files = assembly.GetReferencedAssemblies();

                        foreach (var reference in files)
                        {
                            if (reference.Name != "mscorlib")
                            {
                                var temp = Assembly.ReflectionOnlyLoad(reference.FullName);

                                if (!string.IsNullOrEmpty(temp.CodeBase))
                                {
                                    var uri = new Uri(temp.CodeBase);

                                    if (hash.Add(Path.GetFileName(uri.LocalPath)))
                                    {
                                        list.Add(uri.LocalPath);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var item in assembliesProjects)
                {
                    Assembly assembly = Assembly.ReflectionOnlyLoadFrom(item);

                    if (hash.Add(Path.GetFileName(item)))
                    {
                        list.Add(item);
                    }
                }

                foreach (var item in assembliesProjects)
                {
                    Assembly assembly = Assembly.ReflectionOnlyLoadFrom(item);

                    var files = assembly.GetReferencedAssemblies();

                    foreach (var reference in files)
                    {
                        if (reference.Name != "mscorlib")
                        {
                            var temp = Assembly.ReflectionOnlyLoad(reference.FullName);

                            if (!string.IsNullOrEmpty(temp.CodeBase))
                            {
                                var uri = new Uri(temp.CodeBase);

                                if (hash.Add(Path.GetFileName(uri.LocalPath)))
                                {
                                    list.Add(uri.LocalPath);
                                }
                            }
                        }
                    }
                }

                _refferencedAssemblies = list.ToArray();
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        public static void GetFileTypeFullNameInAppDomain()
        {
            try
            {
                AppDomain.CurrentDomain.SetData(configResult, string.Empty);

                var filePath = (string)AppDomain.CurrentDomain.GetData(configFilePath);

                string code = File.ReadAllText(filePath);

                CompilerParameters parameters = new CompilerParameters()
                {
                    GenerateInMemory = true,
                    GenerateExecutable = false,
                };

                parameters.ReferencedAssemblies.AddRange(_refferencedAssemblies);

                using (CSharpCodeProvider provider = new CSharpCodeProvider())
                {
                    var compilerResults = provider.CompileAssemblyFromSource(parameters, code);

                    if (compilerResults.Output.Count > 0)
                    {
                        DTEHelper.WriteToLog(string.Join(Environment.NewLine, compilerResults.Output.OfType<string>()));
                    }

                    foreach (var error in compilerResults.Errors.OfType<CompilerError>())
                    {
                        WriteErrorToLog(filePath, error);
                    }

                    if (!compilerResults.Errors.HasErrors)
                    {
                        var types = compilerResults.CompiledAssembly.DefinedTypes;

                        AppDomain.CurrentDomain.SetData(configResult, types.FirstOrDefault()?.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        private static void WriteErrorToLog(string filePath, CompilerError error)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"FilePath     : {filePath}");
            stringBuilder.AppendLine($"FileName     : {error.FileName}");
            stringBuilder.AppendLine($"Line         : {error.Line}");
            stringBuilder.AppendLine($"Column       : {error.Column}");
            stringBuilder.AppendLine($"IsWarning    : {error.IsWarning}");
            stringBuilder.AppendLine($"ErrorNumber  : {error.ErrorNumber}");
            stringBuilder.AppendLine("ErrorText    :");
            stringBuilder.AppendLine(error.ErrorText);

            DTEHelper.WriteErrorToLog(stringBuilder.ToString());
        }

        private class AssemblyResolver
        {
            private string[] _paths;

            public AssemblyResolver(string[] paths)
            {
                this._paths = paths;
            }

            public Assembly Domain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
            {
                var assemblyName = new AssemblyName(args.Name);

                if (this._paths != null)
                {
                    foreach (var item in this._paths)
                    {
                        var filePath = Path.Combine(item, assemblyName.Name + ".dll");

                        if (File.Exists(filePath))
                        {
                            return Assembly.ReflectionOnlyLoadFrom(filePath);
                        }
                    }
                }

                return Assembly.ReflectionOnlyLoadFrom(Assembly.Load(args.Name).CodeBase);
            }

            public Assembly Domain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                var assemblyName = new AssemblyName(args.Name);

                if (this._paths != null)
                {
                    foreach (var item in this._paths)
                    {
                        var filePath = Path.Combine(item, assemblyName.Name + ".dll");

                        if (File.Exists(filePath))
                        {
                            return Assembly.LoadFrom(filePath);
                        }
                    }
                }

                return Assembly.Load(args.Name);
            }
        }
    }
}