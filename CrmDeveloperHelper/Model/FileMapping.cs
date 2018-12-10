using System;
using System.Runtime.Serialization;
using System.Text;


namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    /// <summary>
    /// Параметры веб-ресурса
    /// </summary>
    [DataContract]
    public class FileMapping
    {
        /// <summary>
        /// Путь к файлу ресурса
        /// </summary>
        [DataMember]
        public string SourceFilePath { get; set; }

        /// <summary>
        /// Идентификатор объекта в CRM
        /// </summary>
        [DataMember]
        public Guid CRMObjectId { get; set; }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            if (!string.IsNullOrEmpty(this.SourceFilePath))
            {
                str.Append(this.SourceFilePath);
            }

            if (str.Length > 0) { str.Append(" - "); }

            str.Append(this.CRMObjectId.ToString());

            if (str.Length > 0)
            {
                return str.ToString();
            }
            else
            {
                return base.ToString();
            }
        }
    }
}