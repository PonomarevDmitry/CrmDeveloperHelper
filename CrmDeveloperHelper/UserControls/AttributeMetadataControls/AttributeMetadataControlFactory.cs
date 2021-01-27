using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public class AttributeMetadataControlFactory
    {
        public UserControl CreateControlForAttribute(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , bool fillAllways
            , EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , Entity entity
            , object value
        )
        {
            if (attributeMetadata is MemoAttributeMetadata memoAttrib)
            {
                string initialValue = null;

                if (value != null && value is string)
                {
                    initialValue = (string)value;
                }

                return new MemoAttributeMetadataControl(fillAllways, memoAttrib, initialValue);
            }

            if (attributeMetadata is StringAttributeMetadata stringAttrib)
            {
                string initialValue = null;

                if (value != null && value is string)
                {
                    initialValue = (string)value;
                }

                return new StringAttributeMetadataControl(fillAllways, stringAttrib, initialValue);
            }

            if (attributeMetadata is IntegerAttributeMetadata intAttrib)
            {
                int? initialValue = null;

                if (value != null && value is int)
                {
                    initialValue = (int)value;
                }

                return new IntegerAttributeMetadataControl(fillAllways, intAttrib, initialValue);
            }

            if (attributeMetadata is BigIntAttributeMetadata bigIntAttrib)
            {
                long? initialValue = null;

                if (value != null && value is long)
                {
                    initialValue = (long)value;
                }

                return new BigIntAttributeMetadataControl(fillAllways, bigIntAttrib, initialValue);
            }

            if (attributeMetadata is DecimalAttributeMetadata decimalAttrib)
            {
                decimal? initialValue = null;

                if (value != null && value is decimal)
                {
                    initialValue = (decimal)value;
                }

                return new DecimalAttributeMetadataControl(fillAllways, decimalAttrib, initialValue);
            }

            if (attributeMetadata is DoubleAttributeMetadata doubleAttrib)
            {
                double? initialValue = null;

                if (value != null && value is double)
                {
                    initialValue = (double)value;
                }

                return new DoubleAttributeMetadataControl(fillAllways, doubleAttrib, initialValue);
            }

            if (attributeMetadata is MoneyAttributeMetadata moneyAttrib)
            {
                Money initialValue = null;

                if (value != null && value is Money)
                {
                    initialValue = (Money)value;
                }

                return new MoneyAttributeMetadataControl(fillAllways, moneyAttrib, initialValue);
            }

            if (attributeMetadata is DateTimeAttributeMetadata dateTimeAttrib)
            {
                DateTime? initialValue = null;

                if (value != null && value is DateTime)
                {
                    initialValue = (DateTime)value;
                }

                return new DateTimeAttributeMetadataControl(fillAllways, dateTimeAttrib, initialValue);
            }

            if (attributeMetadata is BooleanAttributeMetadata boolAttrib)
            {
                bool? initialValue = null;

                if (value != null && value is bool boolValue)
                {
                    initialValue = boolValue;
                }

                return new BooleanAttributeMetadataControl(fillAllways, boolAttrib, initialValue);
            }

            if (attributeMetadata is ManagedPropertyAttributeMetadata managedPropertyAttributeMetadata)
            {
                if (managedPropertyAttributeMetadata.ValueAttributeTypeCode == AttributeTypeCode.Boolean)
                {
                    BooleanManagedProperty initialValue = null;

                    if (value != null && value is BooleanManagedProperty booleanManagedProperty)
                    {
                        initialValue = booleanManagedProperty;
                    }

                    return new BooleanManagedPropertyAttributeMetadataControl(fillAllways, managedPropertyAttributeMetadata, initialValue);
                }
            }

            if (attributeMetadata is PicklistAttributeMetadata picklistAttrib)
            {
                int? initialValue = null;

                if (value != null && value is OptionSetValue optionSetValue)
                {
                    initialValue = optionSetValue.Value;
                }

                string initialFormatedValue = string.Empty;

                if (entity != null
                    && entity.FormattedValues != null
                    && entity.FormattedValues.ContainsKey(picklistAttrib.LogicalName)
                    && !string.IsNullOrEmpty(entity.FormattedValues[picklistAttrib.LogicalName])
                )
                {
                    initialFormatedValue = entity.FormattedValues[picklistAttrib.LogicalName];
                }

                return new PicklistAttributeMetadataControl(fillAllways, picklistAttrib, initialValue, initialFormatedValue);
            }

            if (attributeMetadata is MultiSelectPicklistAttributeMetadata multiSelectPicklistAttributeMetadata)
            {
                OptionSetValueCollection initialValue = null;

                if (value != null && value is OptionSetValueCollection optionSetValueCollection)
                {
                    initialValue = optionSetValueCollection;
                }

                return new MultiSelectPicklistAttributeMetadataControl(fillAllways, multiSelectPicklistAttributeMetadata, initialValue);
            }

            if (attributeMetadata is StatusAttributeMetadata statusAttrib)
            {
                var stateAttrib = entityMetadata.Attributes.OfType<StateAttributeMetadata>().FirstOrDefault();

                if (stateAttrib != null)
                {
                    int? initialValueStatus = null;
                    int? initialValueState = null;

                    if (value != null && value is OptionSetValue optionSetValueStatus)
                    {
                        initialValueStatus = optionSetValueStatus.Value;
                    }

                    string statusFormatedValue = string.Empty;

                    if (entity != null)
                    {
                        if (entity.Attributes.ContainsKey(stateAttrib.LogicalName)
                            && entity.Attributes[stateAttrib.LogicalName] != null
                            && entity.Attributes[stateAttrib.LogicalName] is OptionSetValue optionSetValueState
                        )
                        {
                            initialValueState = optionSetValueState.Value;
                        }

                        if (entity.FormattedValues != null
                            && entity.FormattedValues.ContainsKey(statusAttrib.LogicalName)
                            && !string.IsNullOrEmpty(entity.FormattedValues[statusAttrib.LogicalName])
                        )
                        {
                            statusFormatedValue = entity.FormattedValues[statusAttrib.LogicalName];
                        }
                    }

                    return new StatusAttributeMetadataControl(fillAllways, statusAttrib, stateAttrib, initialValueStatus, initialValueState, statusFormatedValue);
                }
            }

            if (attributeMetadata is LookupAttributeMetadata lookupAttrib)
            {
                EntityReference initialValue = null;

                if (value != null && value is EntityReference)
                {
                    initialValue = (EntityReference)value;
                }

                return new LookupAttributeMetadataControl(iWriteToOutput, service, fillAllways, lookupAttrib, initialValue);
            }

            if (attributeMetadata is EntityNameAttributeMetadata entityNameAttrib)
            {
                string initialValue = null;

                if (value != null && value is string entityName)
                {
                    initialValue = entityName;
                }

                return new EntityNameAttributeMetadataControl(service, fillAllways, entityNameAttrib, initialValue);
            }

            if (attributeMetadata is UniqueIdentifierAttributeMetadata uniqueAttrib
                || attributeMetadata.AttributeType == AttributeTypeCode.Uniqueidentifier
            )
            {
                Guid? initialValue = null;

                if (value != null && value is Guid valueGuid)
                {
                    initialValue = valueGuid;
                }

                return new UniqueIdentifierAttributeMetadataControl(fillAllways, attributeMetadata, initialValue);
            }

            return null;
        }

        public static void SetGroupBoxNameByAttributeMetadata(GroupBox gbAttribute, AttributeMetadata attributeMetadata)
        {
            StringBuilder header = new StringBuilder(attributeMetadata.LogicalName);

            var displayName = CreateFileHandler.GetLocalizedLabel(attributeMetadata.DisplayName);
            var description = CreateFileHandler.GetLocalizedLabel(attributeMetadata.Description);

            if (attributeMetadata.AttributeTypeName != null && !string.IsNullOrEmpty(attributeMetadata.AttributeTypeName.Value))
            {
                header.AppendFormat(" - {0}", attributeMetadata.AttributeTypeName.Value);
            }
            else if (attributeMetadata.AttributeType.HasValue)
            {
                header.AppendFormat(" - {0}", attributeMetadata.AttributeType.ToString());
            }

            var attrType = attributeMetadata.GetType();

            if (attrType != typeof(AttributeMetadata))
            {
                header.AppendFormat(" - {0}", attrType.Name);
            }

            if (!string.IsNullOrEmpty(displayName))
            {
                header.AppendFormat(" - {0}", displayName);
            }
            else if (!string.IsNullOrEmpty(description))
            {
                header.AppendFormat(" - {0}", description);
            }

            header.Replace("_", "__");

            gbAttribute.Header = header.ToString();
        }
    }
}