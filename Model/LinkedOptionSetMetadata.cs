using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class LinkedOptionSetMetadata
    {
        public string Name { get; private set; }

        public OptionSetMetadata OptionSetMetadata1 { get; private set; }

        public OptionSetMetadata OptionSetMetadata2 { get; private set; }

        public string DisplayName1 { get; private set; }

        public string DisplayName2 { get; private set; }

        public LinkedOptionSetMetadata(string name, OptionSetMetadata optionSetMetadata1, OptionSetMetadata optionSetMetadata2)
        {
            this.Name = name;

            this.OptionSetMetadata1 = optionSetMetadata1;
            this.OptionSetMetadata2 = optionSetMetadata2;

            if (optionSetMetadata1 != null)
            {
                this.DisplayName1 = CreateFileHandler.GetLocalizedLabel(optionSetMetadata1.DisplayName);
            }

            if (optionSetMetadata2 != null)
            {
                this.DisplayName2 = CreateFileHandler.GetLocalizedLabel(optionSetMetadata2.DisplayName);
            }
        }
    }
}