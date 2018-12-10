using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class LocalizedLabel
    {
        [DataMember]
        public string EntityName { get; private set; }

        [DataMember]
        public Guid ObjectId { get; private set; }

        [DataMember]
        public string ColumnName { get; private set; }

        [DataMember]
        public List<LabelString> Labels { get; private set; }

        public LocalizedLabel()
        {
            this.Labels = new List<LabelString>();
        }

        public LocalizedLabel(string entityName, Guid objectId, string columnName) : this()
        {
            this.EntityName = entityName;
            this.ObjectId = objectId;
            this.ColumnName = columnName;
        }

        public override string ToString()
        {
            return string.Format("{0}; {1}; {2}; {3}", EntityName, ObjectId, ColumnName, this.Labels.Count);
        }
    }
}