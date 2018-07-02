using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class LinkedEntityMetadata
    {
        public string LogicalName { get; private set; }

        public EntityMetadata EntityMetadata1 { get; private set; }

        public EntityMetadata EntityMetadata2 { get; private set; }

        public string DisplayName1 { get; private set; }

        public string DisplayName2 { get; private set; }

        public LinkedEntityMetadata(string logicalName, EntityMetadata entityMetadata1, EntityMetadata entityMetadata2)
        {
            this.LogicalName = logicalName;

            this.EntityMetadata1 = entityMetadata1;
            this.EntityMetadata2 = entityMetadata2;

            if (entityMetadata1 != null)
            {
                this.DisplayName1 = CreateFileHandler.GetLocalizedLabel(entityMetadata1.DisplayName);
            }

            if (entityMetadata2 != null)
            {
                this.DisplayName2 = CreateFileHandler.GetLocalizedLabel(entityMetadata2.DisplayName);
            }
        }
    }
}