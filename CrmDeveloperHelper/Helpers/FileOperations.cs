using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class FileOperations
    {
        private const string _fileNameConnectionDataFormat1 = "ConnectionData.{0}.xml";

        private const string _fileNameConnectionName = "ConnectionName.txt";

        private const string _fileNameConnectionConfig = "ConnectionConfiguration.xml";
        private const string _fileNameCommonConfig = "CommonConfiguration.xml";
        private const string _fileNameFileGenerationConfiguration = "FileGenerationConfiguration.xml";

#if DEBUG
        private const string _folderConfiguratonSubdirectoryName = "CrmDeveloperHelperDEBUG";
#else
        private const string _folderConfiguratonSubdirectoryName = "CrmDeveloperHelper";
#endif

        public static string RemoveWrongSymbols(string name)
        {
            var result = new StringBuilder(name);

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

        public static string GetNewTempFilePath(string fileName, string extension)
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
            using (var memStream = new MemoryStream())
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
            using (var memStream = new MemoryStream())
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

        private static readonly HashSet<string> _supportedExtensionsWebResource = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            ".html", ".htm", ".js", ".css", ".resx"
            , ".gif", ".jpg", ".png", ".ico", ".svg"
            , ".xml", ".xsl", ".xslt"
            , ".xap"
        };

        private static HashSet<string> _supportedExtensionsWebResourceText = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            ".htm", ".html", ".css", ".js", ".xml", ".xsl, .xslt", ".svg", ".resx"
        };

        private static HashSet<string> _supportedReportType = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { ".rdl", ".rdlc" };

        private const string _supportedCSharpFile = ".cs";
        private const string _supportedJavaScriptFile = ".js";
        private const string _supportedXmlFile = ".xml";

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

                result = _supportedExtensionsWebResource.Contains(ext);
            }

            return result;
        }

        public static bool SupportsWebResourceTextType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                string ext = Path.GetExtension(path);

                result = _supportedExtensionsWebResourceText.Contains(ext);
            }

            return result;
        }

        public static bool SupportsReportType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                string ext = Path.GetExtension(path);

                result = _supportedReportType.Contains(ext);
            }

            return result;
        }

        public static bool SupportsCSharpType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                result = path.EndsWith(_supportedCSharpFile, StringComparison.InvariantCultureIgnoreCase);
            }

            return result;
        }

        public static bool SupportsJavaScriptType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                result = path.EndsWith(_supportedJavaScriptFile, StringComparison.InvariantCultureIgnoreCase);
            }

            return result;
        }

        public static bool SupportsXmlType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                result = path.EndsWith(_supportedXmlFile, StringComparison.InvariantCultureIgnoreCase);
            }

            return result;
        }

        #endregion Проверка расширений файлов.

        #region Common Configs

        public static string GetConfigurationFolder()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _folderConfiguratonSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        private static string GetConfigurationFilePath(string fileName)
        {
            return Path.Combine(GetConfigurationFolder(), fileName);
        }

        public static string GetCommonConfigFilePath()
        {
            return GetConfigurationFilePath(_fileNameCommonConfig);
        }

        public static string GetConnectionConfigurationFilePath()
        {
            return GetConfigurationFilePath(_fileNameConnectionConfig);
        }

        public static string GetFileGenerationConfigurationFilePath()
        {
            return GetConfigurationFilePath(_fileNameFileGenerationConfiguration);
        }

        private const string _folderLogsSubdirectoryName = "Logs";

        public static string GetLogsFilePath()
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderLogsSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        public static string GetOutputFolderPath()
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderOutputSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        private const string _folderForExportSubdirectoryName = "FolderForExport";

        public static string GetDefaultFolderForExportFilePath()
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderForExportSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        private const string _folderXsdSchemasSubdirectoryName = "XsdSchemas";

        public static string GetSchemaXsdFolder()
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderXsdSchemasSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        private const string _fileNameWindowConfigFormat1 = "{0}.xml";

        private const string _folderWindowSettingsSubdirectoryName = "WindowSettings";

        public static WindowSettings GetWindowConfiguration(string windowName)
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderWindowSettingsSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fileName = string.Format(_fileNameWindowConfigFormat1, windowName);

            string oldFilePath = Path.Combine(GetConfigurationFolder(), fileName);
            string filePath = Path.Combine(directory, fileName);

            MoveOldFile(oldFilePath, filePath);

            var result = WindowSettings.Get(filePath);

            return result;
        }

        private static void MoveOldFile(string oldFilePath, string newFilePath)
        {
            if (File.Exists(oldFilePath))
            {
                File.Move(oldFilePath, newFilePath);
            }
        }

        private const string _folderUserControlSettingsSubdirectoryName = "UserControlSettings";

        public static UserControlSettings GetUserControlSettings(string controlName)
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderUserControlSettingsSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fileName = string.Format(_fileNameWindowConfigFormat1, controlName);

            string oldFilePath = Path.Combine(GetConfigurationFolder(), fileName);
            string filePath = Path.Combine(directory, fileName);

            MoveOldFile(oldFilePath, filePath);

            var result = UserControlSettings.Get(filePath);

            return result;
        }

        #endregion Common Configs

        #region ConnectionData

        private const string _folderNameConnectionDataCollection = "ConnectionDataCollection";

        public static string GetConnectionDataFilePath(Guid connectionId)
        {
            string directory = Path.Combine(GetConfigurationFolder(), _folderNameConnectionDataCollection);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fileName = string.Format(_fileNameConnectionDataFormat1, connectionId.ToString());

            string filePath = Path.Combine(directory, fileName);

            return filePath;
        }

        private const string _folderNameConnectionsInfo = "ConnectionsInfo";
        private const string _folderNameConnectionDataFormat1 = "Connection.{0}";

        public static string GetConnectionInformationFolderPath(Guid connectionId)
        {
            string configurationFolder = GetConfigurationFolder();

            var folderName = string.Format(_folderNameConnectionDataFormat1, connectionId.ToString());

            var result = Path.Combine(configurationFolder, _folderNameConnectionsInfo, folderName);

            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        private const string _folderOutputSubdirectoryName = "Output";

        public static string GetConnectionOutputFolderPath(Guid connectionId)
        {
            string directory = Path.Combine(GetConnectionInformationFolderPath(connectionId), _folderOutputSubdirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        private const string _folderIntellisenseDataSubdirectoryName = "IntellisenseData";

        public static string GetConnectionIntellisenseDataFolderPath(Guid connectionId)
        {
            string directory = GetConnectionInformationFolderPath(connectionId);

            var result = Path.Combine(directory, _folderIntellisenseDataSubdirectoryName);

            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        private const string _folderIntellisenseDataEntitiesSubdirectoryName = "Entities";

        public static string GetConnectionIntellisenseDataFolderPathEntities(Guid connectionId)
        {
            var result = Path.Combine(GetConnectionIntellisenseDataFolderPath(connectionId), _folderIntellisenseDataEntitiesSubdirectoryName);

            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        private const string _folderIntellisenseDataRibbonsSubdirectoryName = "Ribbons";

        public static string GetConnectionIntellisenseDataFolderPathRibbons(Guid connectionId)
        {
            string directory = GetConnectionIntellisenseDataFolderPath(connectionId);

            var result = Path.Combine(directory, _folderIntellisenseDataRibbonsSubdirectoryName);

            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        private const string _folderFetchXmlSubdirectoryName = "FetchXml";

        public static string GetConnectionFetchXmlFolderPath(Guid connectionId)
        {
            string directory = GetConnectionInformationFolderPath(connectionId);

            var result = Path.Combine(directory, _folderFetchXmlSubdirectoryName);

            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        private const string _folderTranslationCacheSubdirectoryName = "TranslationCache";

        public static string GetTranslationLocalCacheFolder(Guid connectionId)
        {
            string directory = GetConnectionInformationFolderPath(connectionId);

            var result = Path.Combine(directory, _folderTranslationCacheSubdirectoryName);

            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        internal static string GetConnectionNameFilePath(Guid connectionId)
        {
            string directory = GetConnectionInformationFolderPath(connectionId);

            var result = Path.Combine(directory, _fileNameConnectionName);

            return result;
        }

        #endregion ConnectionData

        private static void ClearTranslationLocalCacheThread(string directory)
        {
            if (Directory.Exists(directory))
            {
                try
                {
                    var dir = new DirectoryInfo(directory);

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

        public static Uri GetResourceUri(string fileName)
        {
            return new Uri(string.Format("pack://application:,,,/Nav.Common.VSPackages.CrmDeveloperHelper;component/Resources/{0}", fileName));
        }

        public static Uri GetSchemaResourceUri(string fileName)
        {
            return new Uri(string.Format("pack://application:,,,/Nav.Common.VSPackages.CrmDeveloperHelper;component/Resources/Schemas/{0}", fileName));
        }

        public static Uri GetSiteMapResourceUri(string version)
        {
            return new Uri(string.Format("pack://application:,,,/Nav.Common.VSPackages.CrmDeveloperHelper;component/Resources/SiteMaps/SiteMap.{0}.xml", version));
        }

        public static string GetMutexName(string filePath)
        {
            var result = new StringBuilder("Nav.Common.VSPackages.CrmDeveloperHelper." + filePath.ToLower());

            result.Replace(@"\", "_");
            result.Replace(":", "_");

            return result.ToString();
        }

        internal static string CheckFilePathUnique(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return filePath;
            }

            string folder = Path.GetDirectoryName(filePath);
            string ext = Path.GetExtension(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            var index = 1;

            do
            {
                filePath = Path.Combine(folder, string.Format("{0}_{1}{2}", fileName, index, ext));

                index++;
            } while (File.Exists(filePath));

            return filePath;
        }
    }
}