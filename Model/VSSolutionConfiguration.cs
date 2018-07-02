using System;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class VSSolutionConfiguration
    {
        [DataMember]
        public string SolutionPath { get; set; }

        /// <summary>
        /// Выбранное подключение к CRM
        /// </summary>
        [DataMember]
        public Guid? SelectedConnectionId { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public VSSolutionConfiguration()
        {

        }
    }
}