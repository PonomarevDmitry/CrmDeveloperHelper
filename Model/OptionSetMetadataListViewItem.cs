using Microsoft.Xrm.Sdk.Metadata;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class OptionSetMetadataListViewItem
    {
        public string LogicalName { get; private set; }

        public string DisplayName { get; private set; }

        public OptionSetMetadata OptionSetMetadata { get; private set; }

        public OptionSetMetadataListViewItem(string logicalName, string displayName, OptionSetMetadata optionSetMetadata)
        {
            this.LogicalName = logicalName;
            this.DisplayName = displayName;
            this.OptionSetMetadata = optionSetMetadata;
        }
    }
}