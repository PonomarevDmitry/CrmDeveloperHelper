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

        private static readonly HashSet<string> _SupportedFileTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".html", ".htm", ".js", ".css",
            ".gif", ".jpg", ".png", ".ico",
            ".xml", ".xsl", ".xslt",
            ".xap"
        };

        private const string _SupportedReportType = ".rdl";
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

                result = _SupportedFileTypes.Contains(ext);
            }

            return result;
        }

        public static bool SupportsWebResourceTextType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                string ext = Path.GetExtension(path.ToLower());

                result = ContentCoparerHelper.SupportsText(ext);
            }

            return result;
        }

        public static bool SupportsReportType(string path)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(path))
            {
                result = path.EndsWith(_SupportedReportType, StringComparison.OrdinalIgnoreCase);
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
    }
}