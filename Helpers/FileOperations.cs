using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class FileOperations
    {
        public static string RemoveWrongSymbols(string name)
        {
            StringBuilder result = new StringBuilder(name);

            foreach (var c in Path.GetInvalidPathChars())
            {
                result.Replace(c, '_');
            }

            foreach (var c in Path.GetInvalidFileNameChars())
            {
                result.Replace(c, '_');
            }

            return result.ToString();
        }

        private const string _tempFileFolder = "CrmDeveloperHelperTemp";

        public static string GetTempFileFolder()
        {
            return Path.Combine(Path.GetTempPath(), _tempFileFolder);
        }

        public static string GetNewTempFile(string fileName, string extension)
        {
            string path = string.Empty;

            string directory = GetTempFileFolder();

            fileName = RemoveWrongSymbols(fileName);

            do
            {
                path = string.Format("{0}_{1}{2}", fileName, Guid.NewGuid().ToString(), extension);

                path = Path.Combine(directory, path);

            } while (File.Exists(path));

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return path;
        }

        public static byte[] UnzipRibbon(byte[] data)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(data, 0, data.Length);

                using (ZipPackage package = (ZipPackage)ZipPackage.Open(memStream, FileMode.Open))
                {
                    ZipPackagePart part = (ZipPackagePart)package.GetPart(new Uri("/RibbonXml.xml", UriKind.Relative));

                    using (Stream strm = part.GetStream())
                    {
                        long len = strm.Length;
                        byte[] buff = new byte[len];
                        strm.Read(buff, 0, (int)len);
                        return buff;
                    }
                }
            }
        }

        public static byte[] UnzipCrmTranslations(byte[] data, string filepath)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(data, 0, data.Length);

                using (ZipPackage package = (ZipPackage)ZipPackage.Open(memStream, FileMode.Open))
                {
                    ZipPackagePart part = (ZipPackagePart)package.GetPart(new Uri(filepath, UriKind.Relative));

                    using (Stream strm = part.GetStream())
                    {
                        long len = strm.Length;
                        byte[] buff = new byte[len];
                        strm.Read(buff, 0, (int)len);
                        return buff;
                    }
                }
            }
        }

        #region Проверка расширений файлов.

        private static readonly HashSet<string> _SupportedExtensionsWebResource = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".html", ".htm", ".js", ".css",
            ".gif", ".jpg", ".png", ".ico", ".svg",
            ".xml", ".xsl", ".xslt",
            ".xap"
        };

        private static HashSet<string> _SupportedExtensionsWebResourceText = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".htm", ".html", ".css", ".js", ".xml", ".xsl, .xslt", ".svg" };

        private static HashSet<string> _SupportedReportType = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".rdl", ".rdlc" };

        private const string _SupportedCSharpFile = ".cs";
        private const string _SupportedXmlFile = ".xml";

        /// <summary>
        /// Имеет ли файл правильное расширение.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool SupportsWebResourceType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                string ext = Path.GetExtension(path);

                result = _SupportedExtensionsWebResource.Contains(ext);
            }

            return result;
        }

        public static bool SupportsWebResourceTextType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                string ext = Path.GetExtension(path);

                result = _SupportedExtensionsWebResourceText.Contains(ext);
            }

            return result;
        }

        public static bool SupportsReportType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                string ext = Path.GetExtension(path);

                result = _SupportedReportType.Contains(ext);
            }

            return result;
        }

        public static bool SupportsCSharpType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                result = path.EndsWith(_SupportedCSharpFile, StringComparison.OrdinalIgnoreCase);
            }

            return result;
        }

        public static bool SupportsXmlType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                result = path.EndsWith(_SupportedXmlFile, StringComparison.OrdinalIgnoreCase);
            }

            return result;
        }

        #endregion Проверка расширений файлов.

        public static string GetConnectionConfigFilePath()
        {
            return GetConfigurationFilePath(_programConnectionConfigFileName);
        }

        public static Translation GetTranslationLocalCache(string fileName)
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderTranslationCacheSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string pathConfig = Path.Combine(directory, fileName);

            Translation result = Translation.Get(pathConfig);

            return result;
        }

        public static void SaveTranslationLocalCache(string fileName, Translation translation)
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderTranslationCacheSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string pathConfig = Path.Combine(directory, fileName);

            Translation.Save(pathConfig, translation);
        }

        public static string GetSchemaXsdFolder()
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderXsdSchemasSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        public static string GetConnectionIntellisenseDataFullFilePath(string fileName)
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderIntellisenseCacheCacheSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return Path.Combine(directory, fileName);
        }

        public static void ClearTranslationLocalCache()
        {
            System.Threading.Thread clearTempFiles = new System.Threading.Thread(ClearTranslationLocalCacheThread);

            clearTempFiles.Start();
        }

        private static void ClearTranslationLocalCacheThread()
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderTranslationCacheSubdirectoryName);

            if (Directory.Exists(directory))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(directory);

                    var files = dir.GetFiles();

                    foreach (var item in files)
                    {
                        try
                        {
                            item.Delete();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public static WindowSettings GetWindowConfiguration(string windowName)
        {
            string fileName = string.Format(_formatWindowConfigFileName, windowName);

            string pathConfig = GetConfigurationFilePath(fileName);

            WindowSettings result = WindowSettings.Get(pathConfig);

            return result;
        }

        public static UserControlSettings GetUserControlSettings(string controlName)
        {
            string fileName = string.Format(_formatWindowConfigFileName, controlName);

            string pathConfig = GetConfigurationFilePath(fileName);

            UserControlSettings result = UserControlSettings.Get(pathConfig);

            return result;
        }

        private const string _formatWindowConfigFileName = "{0}.xml";

        private const string _programConnectionConfigFileName = "ConnectionConfiguration.xml";
        private const string _programCommonConfigFileName = "CommonConfiguration.xml";

#if DEBUG
        private const string _folderConfiguratonSubdirectoryName = "CrmDeveloperHelperDEBUG";
#else
        private const string _folderConfiguratonSubdirectoryName = "CrmDeveloperHelper";
#endif

        private const string _folderTranslationCacheSubdirectoryName = "TranslationCache";

        private const string _folderXsdSchemasSubdirectoryName = "XsdSchemas";

        private const string _folderIntellisenseCacheCacheSubdirectoryName = "IntellisenseCache";

        private const string _folderOutputSubdirectoryName = "Output";

        private const string _folderLogsSubdirectoryName = "Logs";

        private static string GetConfigurationFilePath(string fileName)
        {
            return Path.Combine(GetConfigurationFolder(), fileName);
        }

        public static string GetConfigurationFolder()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _folderConfiguratonSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        public static string GetCommonConfigPath()
        {
            return GetConfigurationFilePath(_programCommonConfigFileName);
        }

        public static string GetOutputPath()
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderOutputSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        public static string GetLogsPath()
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderLogsSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        public static void CreateBackUpFile(string filePath, Exception ex)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            DateTime date = DateTime.Now;

            var folder = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);

            {
                var fileNewName = string.Format("{0} DataContractReadObject Crush at {1:yyyy.MM.dd HH-mm-ss}{2}", fileName, date, extension);
                var fileNewPath = Path.Combine(folder, fileNewName);

                File.Move(filePath, fileNewPath);
            }

            {
                var description = DTEHelper.GetExceptionDescription(ex);

                var fileNewName = string.Format("{0} DataContractReadObject Crush at {1:yyyy.MM.dd HH-mm-ss} Exception Description.txt", fileName, date);
                var fileNewPath = Path.Combine(folder, fileNewName);

                File.WriteAllText(fileNewPath, description, new UTF8Encoding(false));
            }
        }

        public static Uri GetSchemaResourceUri(string fileName)
        {
            return new Uri(string.Format("pack://application:,,,/Nav.Common.VSPackages.CrmDeveloperHelper;component/Resources/Schemas/{0}", fileName));
        }

        public static Uri GetSiteMapResourceUri(string version)
        {
            return new Uri(string.Format("pack://application:,,,/Nav.Common.VSPackages.CrmDeveloperHelper;component/Resources/SiteMaps/SiteMap.{0}.xml", version));
        }
    }
}