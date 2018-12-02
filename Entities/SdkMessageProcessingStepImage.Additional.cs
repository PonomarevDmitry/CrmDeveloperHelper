using System.Linq;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class SdkMessageProcessingStepImage
    {
        public string PrimaryObjectTypeCodeName
        {
            get
            {
                if (this.Attributes.ContainsKey(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode)
                    && this.Attributes[SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode] != null
                    && this.Attributes[SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode] is AliasedValue aliasedValue
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return "none";
            }
        }

        public string SecondaryObjectTypeCodeName
        {
            get
            {
                if (this.Attributes.ContainsKey(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode)
                    && this.Attributes[SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode] != null
                    && this.Attributes[SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode] is AliasedValue aliasedValue
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return "none";
            }
        }

        public IEnumerable<string> Attributes1Strings
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Attributes1))
                {
                    foreach (var item in this.Attributes1.Split(',').OrderBy(s => s))
                    {
                        yield return item;
                    }
                }
            }
        }

        public string Attributes1StringsSorted
        {
            get
            {
                return string.Join(",", this.Attributes1Strings);
            }
        }
    }
}
