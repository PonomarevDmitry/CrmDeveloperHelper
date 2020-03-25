using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class EntityMetadataListViewItem
    {
        public string LogicalName => EntityMetadata.LogicalName;

        public string DisplayName { get; private set; }

        public EntityMetadata EntityMetadata { get; private set; }

        public bool IsIntersect => EntityMetadata.IsIntersect.GetValueOrDefault();

        public EntityMetadataListViewItem(EntityMetadata entityMetadata)
        {
            this.DisplayName = CreateFileHandler.GetLocalizedLabel(entityMetadata.DisplayName);
            this.EntityMetadata = entityMetadata;
        }
    }
}