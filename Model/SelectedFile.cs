
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense;
using System;
using System.IO;
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    /// <summary>
    /// Описывает выбранный для публикации файл 
    /// </summary>
    public class SelectedFile
    {
        /// <summary>
        /// Имя файла без расширения
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string Extension { get; private set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Полный путь к файлу
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Относительный путь к файлу
        /// </summary>
        public string FriendlyFilePath { get; private set; }

        public string UrlFriendlyFilePath { get; private set; }

        /// <summary>
        /// Конструктор выбранного файла
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="friendlyFilePath"></param>
        public SelectedFile(string filePath, string friendlyFilePath)
        {
            this.FilePath = filePath;
            this.FileName = Path.GetFileName(filePath);
            this.Name = Path.GetFileNameWithoutExtension(filePath);
            this.FriendlyFilePath = friendlyFilePath;

            UrlFriendlyFilePath = string.Format("{0}:///{1}", UrlCommandFilter.PrefixOpenInVisualStudioRelativePath, friendlyFilePath.Replace('\\', '/').TrimStart('/'));

            this.Extension = Path.GetExtension(filePath).ToLower();
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.FriendlyFilePath))
            {
                return this.FriendlyFilePath;
            }
            else
            {
                return base.ToString();
            }
        }
    }
}