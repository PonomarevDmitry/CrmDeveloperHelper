using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class SystemFormIntellisenseData
    {
        [DataMember]
        public Guid? FormId { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public string ObjectTypeCode { get; private set; }

        public void LoadData(SystemForm systemForm)
        {
            if (systemForm.FormId.HasValue)
            {
                this.FormId = systemForm.FormId;
            }

            this.Name = systemForm.Name;
            this.Description = systemForm.Description;
            this.ObjectTypeCode = systemForm.ObjectTypeCode;
        }

        public void MergeDataFromDisk(SystemFormIntellisenseData systemForm)
        {
            if (systemForm == null)
            {
                return;
            }

            if (!this.FormId.HasValue
                && systemForm.FormId.HasValue
            )
            {
                this.FormId = systemForm.FormId;
            }

            if (string.IsNullOrEmpty(this.Name)
                && !string.IsNullOrEmpty(systemForm.Name)
            )
            {
                this.Name = systemForm.Name;
            }

            if (string.IsNullOrEmpty(this.Description)
                && !string.IsNullOrEmpty(systemForm.Description)
            )
            {
                this.Description = systemForm.Description;
            }

            if (string.IsNullOrEmpty(this.ObjectTypeCode)
                && !string.IsNullOrEmpty(systemForm.ObjectTypeCode)
            )
            {
                this.ObjectTypeCode = systemForm.ObjectTypeCode;
            }
        }
    }
}