using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class SolutionImageComponent
    {
        [DataMember]
        public int ComponentType { get; set; }

        [DataMember]
        public Guid? ObjectId { get; set; }

        [DataMember]
        public int? RootComponentBehavior { get; set; }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public string ParentSchemaName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SolutionImageComponent component))
            {
                return false;
            }

            return this.ComponentType == component.ComponentType
                    && this.ObjectId == component.ObjectId
                    && string.Equals(this.SchemaName, component.SchemaName, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(this.ParentSchemaName, component.ParentSchemaName, StringComparison.InvariantCultureIgnoreCase)
                ;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
