using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VSLangProj;
using VSLangProj80;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class VSProject2Info
    {
        public string FileName { get; private set; }

        public List<string> AssembliesProjects { get; private set; } = new List<string>();

        public List<string> AssembliesReferences { get; private set; } = new List<string>();

        public List<string> CSharpFiles { get; private set; } = new List<string>();

        private HashSet<string> _hash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        private VSProject2Info()
        {

        }

        public static void GetPluginTypes(IEnumerable<EnvDTE.Document> documentsEnum, out string[] pluginTypesNotCompiled, out VSProject2Info[] projectInfos)
        {
            var notCompiledList = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            var dictionary = new Dictionary<string, VSProject2Info>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var document in documentsEnum.Where(d => d != null))
            {
                if (document.ProjectItem?.ContainingProject != null)
                {
                    var project = document.ProjectItem?.ContainingProject;

                    if (dictionary.ContainsKey(project.FileName))
                    {
                        dictionary[project.FileName].CSharpFiles.Add(document.FullName);

                        continue;
                    }
                    else if (GetProjectInfo(project, out var projectInfo))
                    {
                        dictionary[project.FileName] = projectInfo;

                        dictionary[project.FileName].CSharpFiles.Add(document.FullName);

                        continue;
                    }
                }

                notCompiledList.Add(Path.GetFileNameWithoutExtension(document.FullName).Split('.').FirstOrDefault());
            }

            pluginTypesNotCompiled = notCompiledList.ToArray();
            projectInfos = dictionary.Values.ToArray();
        }

        public static void GetPluginTypes(IEnumerable<EnvDTE.ProjectItem> projectItems, out string[] pluginTypesNotCompiled, out VSProject2Info[] projectInfos)
        {
            var notCompiledList = new List<string>();
            var dictionary = new Dictionary<string, VSProject2Info>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var projectItem in projectItems.Where(d => d != null))
            {
                if (projectItem.ContainingProject != null)
                {
                    var project = projectItem.ContainingProject;

                    if (dictionary.ContainsKey(project.FileName))
                    {
                        dictionary[project.FileName].CSharpFiles.Add(projectItem.FileNames[1]);

                        continue;
                    }
                    else if (GetProjectInfo(project, out var projectInfo))
                    {
                        dictionary[project.FileName] = projectInfo;

                        dictionary[project.FileName].CSharpFiles.Add(projectItem.FileNames[1]);

                        continue;
                    }
                }

                notCompiledList.Add(Path.GetFileNameWithoutExtension(projectItem.FileNames[1]).Split('.').FirstOrDefault());
            }


            pluginTypesNotCompiled = notCompiledList.ToArray();
            projectInfos = dictionary.Values.ToArray();
        }

        private static bool GetProjectInfo(EnvDTE.Project project, out VSProject2Info projectInfo)
        {
            projectInfo = null;

            if (project != null
                && project.Object != null
                && project.Object is VSProject2 vsProject
            )
            {
                projectInfo = new VSProject2Info()
                {
                    FileName = project.FileName,
                };

                projectInfo.FillProjectReferences(vsProject);

                return true;
            }

            return false;
        }

        private void FillProjectReferences(VSProject2 proj)
        {
            if (proj.Project != null)
            {
                var outputFilePath = PropertiesHelper.GetOutputFilePath(proj.Project);

                if (!string.IsNullOrEmpty(outputFilePath))
                {
                    if (_hash.Add(Path.GetFileName(outputFilePath))
                        && outputFilePath.IndexOf("mscorlib", StringComparison.InvariantCultureIgnoreCase) == -1
                    )
                    {
                        AssembliesProjects.Add(outputFilePath);
                    }
                }
            }

            foreach (var item in proj.References.OfType<Reference>())
            {
                if (item.Type == prjReferenceType.prjReferenceTypeAssembly)
                {
                    if (_hash.Add(Path.GetFileName(item.Path))
                        && item.Path.IndexOf("mscorlib", StringComparison.InvariantCultureIgnoreCase) == -1
                    )
                    {
                        AssembliesReferences.Add(item.Path);
                    }

                    if (item.SourceProject != null
                        && item.SourceProject.Object != null
                        && item.SourceProject.Object is VSProject2 project2
                        )
                    {
                        FillProjectReferences(project2);
                    }
                }
            }
        }
    }
}