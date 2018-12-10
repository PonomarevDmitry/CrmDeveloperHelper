using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using VSLangProj;
using VSLangProj80;

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

        public static string GetFileTypeFullName(string filePath, VSProject2 proj)
        {
            List<string> assembliesProjects = new List<string>();
            List<string> assembliesReferences = new List<string>();

            var hash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            FillProjectReferences(proj, assembliesProjects, assembliesReferences, hash);

            AppDomain childDomain = CreateChildDomain();

            try
            {
                childDomain.SetData(configFilePath, filePath);
                childDomain.SetData(configAssembliesProjects, assembliesProjects.ToArray());
                childDomain.SetData(configAssemblies, assembliesReferences.ToArray());
                childDomain.DoCallBack(new CrossAppDomainDelegate(GetFileTypeFullNameInAppDomain));

                return childDomain.GetData(configResult).ToString();
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
            finally
            {
                AppDomain.Unload(childDomain);
            }

            return string.Empty;
        }

        private static void FillProjectReferences(VSProject2 proj, List<string> assembliesProjects, List<string> assembliesReferences, HashSet<string> hash)
        {
            if (proj.Project != null)
            {
                var outputFilePath = PropertiesHelper.GetOutputFilePath(proj.Project);

                if (!string.IsNullOrEmpty(outputFilePath))
                {
                    if (hash.Add(Path.GetFileName(outputFilePath))
                        && outputFilePath.IndexOf("mscorlib", StringComparison.InvariantCultureIgnoreCase) == -1
                        )
                    {
                        assembliesProjects.Add(outputFilePath);
                    }
                }
            }

            foreach (var item in proj.References.OfType<Reference>())
            {
                if (item.Type == prjReferenceType.prjReferenceTypeAssembly)
                {
                    if (hash.Add(Path.GetFileName(item.Path))
                        && item.Path.IndexOf("mscorlib", StringComparison.InvariantCultureIgnoreCase) == -1
                        )
                    {
                        assembliesReferences.Add(item.Path);
                    }

                    if (item.SourceProject != null
                        && item.SourceProject.Object != null
                        && item.SourceProject.Object is VSProject2 project2
                        )
                    {
                        FillProjectReferences(project2, assembliesProjects, assembliesReferences, hash);
                    }
                }
            }
        }

        private const string configResult = "Result";
        private const string configFilePath = "FilePath";
        private const string configAssembliesProjects = "AssembliesProjects";
        private const string configAssemblies = "Assemblies";

        public static void GetFileTypeFullNameInAppDomain()
        {
            try
            {
                var filePath = (string)AppDomain.CurrentDomain.GetData(configFilePath);

                var assembliesProjects = (string[])AppDomain.CurrentDomain.GetData(configAssembliesProjects);
                var assemblies = (string[])AppDomain.CurrentDomain.GetData(configAssemblies);

                AppDomain.CurrentDomain.SetData(configResult, string.Empty);

                HashSet<string> list = new HashSet<string>(assembliesProjects, StringComparer.InvariantCultureIgnoreCase);

                var resolver = new AssemblyResolver(assembliesProjects.Select(e => Path.GetDirectoryName(e)).ToArray());

                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolver.Domain_ReflectionOnlyAssemblyResolve;
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolver.Domain_ReflectionOnlyAssemblyResolve;
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolver.Domain_ReflectionOnlyAssemblyResolve;
                AppDomain.CurrentDomain.AssemblyResolve -= resolver.Domain_AssemblyResolve;
                AppDomain.CurrentDomain.AssemblyResolve -= resolver.Domain_AssemblyResolve;
                AppDomain.CurrentDomain.AssemblyResolve += resolver.Domain_AssemblyResolve;

                var hash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

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

                string code = File.ReadAllText(filePath);

                CompilerParameters parameters = new CompilerParameters()
                {
                    GenerateInMemory = true,
                    GenerateExecutable = false,
                };

                parameters.ReferencedAssemblies.AddRange(list.ToArray());

                using (CSharpCodeProvider provider = new CSharpCodeProvider())
                {
                    var compilerResults = provider.CompileAssemblyFromSource(parameters, code);

                    if (compilerResults.Errors.Count == 0)
                    {
                        var types = compilerResults.CompiledAssembly.DefinedTypes;

                        AppDomain.CurrentDomain.SetData(configResult, types.FirstOrDefault()?.FullName);
                    }

                    AppDomain.CurrentDomain.AssemblyResolve -= resolver.Domain_AssemblyResolve;
                    AppDomain.CurrentDomain.AssemblyResolve -= resolver.Domain_AssemblyResolve;
                    AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolver.Domain_ReflectionOnlyAssemblyResolve;
                    AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolver.Domain_ReflectionOnlyAssemblyResolve;
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
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