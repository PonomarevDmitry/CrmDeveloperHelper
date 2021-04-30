using EnvDTE;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VSLangProj80;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class PropertiesHelper
    {
        public static string GetOutputPath(Project proj)
        {
            var prjPath = GetProjectPath(proj);

            if (string.IsNullOrWhiteSpace(prjPath))
            {
                return string.Empty;
            }

            EnvDTE.Properties prop = null;

            string probKey = string.Empty;
            if (proj.ConfigurationManager.ActiveConfiguration.Properties == null)
            {
                if (TryGetPropertyByName(proj.Properties, "ActiveConfiguration", out _) == false)
                {
                    return string.Empty;
                }

                prop = proj.Properties.Item("ActiveConfiguration").Value as EnvDTE.Properties;
                if (TryGetPropertyByName(prop, "PrimaryOutput", out _))
                {
                    probKey = "PrimaryOutput";
                }
            }
            else
            {
                prop = proj.ConfigurationManager.ActiveConfiguration.Properties;
                if (TryGetPropertyByName(prop, "OutputPath", out _))
                {
                    probKey = "OutputPath";
                }
            }

            if (TryGetPropertyByName(prop, probKey, out _) == false)
            {
                return string.Empty;
            }

            var filePath = prop.Item(probKey).Value.ToString();
            if (Path.IsPathRooted(filePath) == false)
            {
                filePath = Path.Combine(prjPath, filePath);
            }

            var attr = File.GetAttributes(filePath);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                return filePath;
            }
            else
            {
                return new FileInfo(filePath).Directory.FullName;
            }
        }

        public static string GetOutputFilePath(Project proj)
        {
            var outputPath = GetOutputPath(proj);

            if (string.IsNullOrEmpty(outputPath))
            {
                return string.Empty;
            }

            Property fileNameProp;

            TryGetPropertyByName(proj.Properties, "OutputFileName", out fileNameProp);

            if (fileNameProp == null)
            {
                return string.Empty;
            }

            var fileName = fileNameProp.Value.ToString();

            return Path.Combine(outputPath, fileName);
        }

        public static string GetProjectPath(Project proj)
        {
            if (TryGetPropertyByName(proj.Properties, "FullPath", out var propertyFullPath))
            {
                var filePath = propertyFullPath.Value.ToString();
                var fullPath = new FileInfo(filePath);
                return fullPath.Directory.FullName;
            }

            if (TryGetPropertyByName(proj.Properties, "ProjectFile", out var propertyProjectFile))
            {
                var filePath = propertyProjectFile.Value.ToString();
                var fullPath = new FileInfo(filePath);
                return fullPath.Directory.FullName;
            }

            return string.Empty;
        }

        public static bool TryGetPropertyByName(EnvDTE.Properties properties, string propertyName, out Property result)
        {
            result = null;

            if (properties != null)
            {
                var list = properties.OfType<Property>().ToList();

                //var temp = list.Select(e => new { e.Name }).ToList();

                foreach (Property item in list)
                {
                    if (item != null && string.Equals(item.Name, propertyName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = item;

                        return true;
                    }
                }
            }

            return false;
        }

        public static string GetAssemblyName(Project project)
        {
            if (TryGetPropertyByName(project.Properties, "AssemblyName", out var propertyAssemblyName))
            {
                return propertyAssemblyName.Value.ToString();
            }

            return project.Name;
        }
    }
}