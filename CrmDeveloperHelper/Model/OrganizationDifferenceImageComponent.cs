using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class OrganizationDifferenceImageComponent : SolutionImageComponent
    {
        public OrganizationDifferenceImageComponent()
        {

        }

        public OrganizationDifferenceImageComponent(SolutionImageComponent component)
        {
            this.ComponentType = component.ComponentType;
            this.ObjectId = component.ObjectId;
            this.RootComponentBehavior = component.RootComponentBehavior;
            this.SchemaName = component.SchemaName;
            this.ParentSchemaName = component.ParentSchemaName;
            this.Description = component.Description;
        }

        [DataMember]
        public string DescriptionSecond { get; set; }

        [DataMember]
        public string DescriptionDifference { get; set; }
    }
}
