using Microsoft.Xrm.Sdk.Metadata;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class EntityMetadataListViewItem
    {
        public string EntityLogicalName { get; private set; }

        public string DisplayName { get; private set; }

        public EntityMetadata EntityMetadata { get; private set; }

        public EntityMetadataListViewItem(string logicalName, string displayName, EntityMetadata entityMetadata)
        {
            this.EntityLogicalName = logicalName;
            this.DisplayName = displayName;
            this.EntityMetadata = entityMetadata;
        }
    }
}