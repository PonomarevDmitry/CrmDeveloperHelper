using System;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class SolutionImageComponent
    {
        [DataMember]
        public int ComponentType { get; set; }

        [DataMember]
        public string ComponentTypeName
        {
            get
            {
                if (Entities.SolutionComponent.IsDefinedComponentType(this.ComponentType))
                {
                    return ((Entities.ComponentType)this.ComponentType).ToString();
                }

                return string.Empty;
            }

            set
            {

            }
        }

        [DataMember]
        public Guid? ObjectId { get; set; }

        [DataMember]
        public int? RootComponentBehavior { get; set; }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public string ParentSchemaName { get; set; }

        [DataMember]
        public string Description { get; set; }

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
            return this.ComponentType.GetHashCode()
                ^ this.ObjectId.GetHashCode()
                ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.SchemaName)
                ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.ParentSchemaName)
                ;
        }
    }
}