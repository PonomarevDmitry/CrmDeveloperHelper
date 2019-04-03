using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Text;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public class AttributeMetadataControlFactory
    {
        public UserControl CreateControlForAttribute(IOrganizationServiceExtented service, AttributeMetadata attributeMetadata, Entity entity, object value)
        {
            if (attributeMetadata is MemoAttributeMetadata memoAttrib)
            {
                string initialValue = null;

                if (value != null && value is string)
                {
                    initialValue = (string)value;
                }

                return new MemoAttributeMetadataControl(memoAttrib, initialValue);
            }

            if (attributeMetadata is StringAttributeMetadata stringAttrib)
            {
                string initialValue = null;

                if (value != null && value is string)
                {
                    initialValue = (string)value;
                }

                return new StringAttributeMetadataControl(stringAttrib, initialValue);
            }

            if (attributeMetadata is IntegerAttributeMetadata intAttrib)
            {
                int? initialValue = null;

                if (value != null && value is int)
                {
                    initialValue = (int)value;
                }

                return new IntegerAttributeMetadataControl(intAttrib, initialValue);
            }

            if (attributeMetadata is BigIntAttributeMetadata bigIntAttrib)
            {
                long? initialValue = null;

                if (value != null && value is long)
                {
                    initialValue = (long)value;
                }

                return new BigIntAttributeMetadataControl(bigIntAttrib, initialValue);
            }

            if (attributeMetadata is DecimalAttributeMetadata decimalAttrib)
            {
                decimal? initialValue = null;

                if (value != null && value is decimal)
                {
                    initialValue = (decimal)value;
                }

                return new DecimalAttributeMetadataControl(decimalAttrib, initialValue);
            }

            if (attributeMetadata is DoubleAttributeMetadata doubleAttrib)
            {
                double? initialValue = null;

                if (value != null && value is double)
                {
                    initialValue = (double)value;
                }

                return new DoubleAttributeMetadataControl(doubleAttrib, initialValue);
            }

            if (attributeMetadata is MoneyAttributeMetadata moneyAttrib)
            {
                Money initialValue = null;

                if (value != null && value is Money)
                {
                    initialValue = (Money)value;
                }

                return new MoneyAttributeMetadataControl(moneyAttrib, initialValue);
            }

            if (attributeMetadata is DateTimeAttributeMetadata dateTimeAttrib)
            {
                DateTime? initialValue = null;

                if (value != null && value is DateTime)
                {
                    initialValue = (DateTime)value;
                }

                return new DateTimeAttributeMetadataControl(dateTimeAttrib, initialValue);
            }

            if (attributeMetadata is BooleanAttributeMetadata boolAttrib)
            {
                bool? initialValue = null;

                if (value != null && value is bool boolValue)
                {
                    initialValue = boolValue;
                }

                return new BooleanAttributeMetadataControl(boolAttrib, initialValue);
            }

            if (attributeMetadata is PicklistAttributeMetadata picklistAttrib)
            {
                int? initialValue = null;

                if (value != null && value is OptionSetValue optionSetValue)
                {
                    initialValue = optionSetValue.Value;
                }

                return new PicklistAttributeMetadataControl(service, entity, picklistAttrib, initialValue);
            }

            if (attributeMetadata is StatusAttributeMetadata statusAttrib)
            {
                int? initialValue = null;

                if (value != null && value is OptionSetValue optionSetValue)
                {
                    initialValue = optionSetValue.Value;
                }

                return new StatusAttributeMetadataControl(service, entity, statusAttrib, initialValue);
            }

            if (attributeMetadata is EntityNameAttributeMetadata entityNameAttrib)
            {

            }

            if (attributeMetadata is LookupAttributeMetadata lookupAttrib)
            {
                EntityReference initialValue = null;

                if (value != null && value is EntityReference)
                {
                    initialValue = (EntityReference)value;
                }

                return new LookupAttributeMetadataControl(lookupAttrib, initialValue);
            }

            return null;
        }

        public static void SetGroupBoxNameByAttributeMetadata(GroupBox gbAttribute, AttributeMetadata attributeMetadata)
        {
            StringBuilder header = new StringBuilder();
            header.Append(attributeMetadata.LogicalName.Replace("_", "__"));

            var displayName = CreateFileHandler.GetLocalizedLabel(attributeMetadata.DisplayName);
            var description = CreateFileHandler.GetLocalizedLabel(attributeMetadata.Description);

            if (!string.IsNullOrEmpty(displayName))
            {
                header.AppendFormat(" - {0}", displayName.Replace("_", "__"));
            }
            else if (!string.IsNullOrEmpty(description))
            {
                header.AppendFormat(" - {0}", description.Replace("_", "__"));
            }

            gbAttribute.Header = header.ToString();
        }
    }
}