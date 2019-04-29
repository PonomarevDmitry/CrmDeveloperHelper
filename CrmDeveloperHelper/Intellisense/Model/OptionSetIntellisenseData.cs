using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class OptionSetIntellisenseData
    {
        [DataMember]
        public bool IsStateCode { get; private set; }

        [DataMember]
        public bool IsStatusCode { get; private set; }

        [DataMember]
        public bool IsBoolean { get; private set; }

        [DataMember]
        public OptionSetMetadataBase OptionSetMetadata { get; private set; }

        public OptionSetIntellisenseData()
        {

        }

        public OptionSetIntellisenseData(BooleanAttributeMetadata boolMetadata)
        {
            this.IsBoolean = true;

            this.OptionSetMetadata = boolMetadata.OptionSet;
        }

        public OptionSetIntellisenseData(EnumAttributeMetadata enumAttributeMetadata)
        {
            this.IsStateCode = enumAttributeMetadata is StateAttributeMetadata;
            this.IsStatusCode = enumAttributeMetadata is StatusAttributeMetadata;

            this.OptionSetMetadata = enumAttributeMetadata.OptionSet;
        }
    }
}
