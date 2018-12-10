using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    /// <summary>
    /// Элемент для публикации в CRM
    /// </summary>
    public class ElementForPublish
    {
        /// <summary>
        /// Файл-источник контента для веб-ресурса
        /// </summary>
        public SelectedFile SelectedFile { get; private set; }

        /// <summary>
        /// Веб-ресурс для 
        /// </summary>
        public WebResource WebResource { get; private set; }

        public ElementForPublish(SelectedFile selectedFile, WebResource webResource)
        {
            if (selectedFile == null)
            {
                throw new ArgumentException("Не задан файл-источник.");
            }

            if (webResource == null)
            {
                throw new ArgumentException("Не задан веб-ресурс.");
            }

            this.SelectedFile = selectedFile;
            this.WebResource = webResource;
        }
    }
}