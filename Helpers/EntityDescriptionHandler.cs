using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class EntityDescriptionHandler
    {
        public static Task<string> GetEntityDescriptionAsync(Entity entity, ICollection<string> attributeToIgnore, ConnectionData connectionData = null)
        {
            return Task.Run(() => GetEntityDescription(entity, attributeToIgnore, connectionData));
        }

        private static string GetEntityDescription(Entity entity, ICollection<string> attributeToIgnore, ConnectionData connectionData)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("Entity: {0}", entity.LogicalName).AppendLine();
            result.AppendFormat("Id: {0}", entity.Id).AppendLine();

            if (connectionData != null)
            {
                var url = connectionData.GetEntityInstanceUrl(entity.LogicalName, entity.Id);

                if (!string.IsNullOrEmpty(url))
                {
                    result.AppendFormat("Url: {0}", url).AppendLine();
                }
            }

            result.AppendLine();

            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("Field Name", "Type", "Value");

            foreach (var attr in entity.Attributes.OrderBy(s => s.Key))
            {
                if (attributeToIgnore != null)
                {
                    if (attributeToIgnore.Contains(attr.Key))
                    {
                        continue;
                    }
                }

                table.AddLine(attr.Key, GetAttributeType(attr.Value), GetAttributeString(entity, attr.Key, connectionData));
            }

            table.GetFormatedLines(false).ForEach(s => result.AppendLine(s));

            return result.ToString();
        }

        private static string GetAttributeType(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is AliasedValue)
            {
                var aliased = value as AliasedValue;

                if (aliased.Value != null)
                {
                    return value.GetType().Name + "." + GetAttributeType(aliased.Value);
                }
            }

            return value.GetType().Name;
        }

        public static object GetUnderlyingValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is AliasedValue aliased)
            {
                if (aliased.Value == null)
                {
                    return null;
                }

                return GetUnderlyingValue(aliased.Value);
            }

            return value;
        }

        private static string GetAttributeStringInternal(Entity entity, string key, Func<FormattedValueCollection, string, object, ConnectionData, string> getterString, ConnectionData connectionData)
        {
            if (!entity.Attributes.Contains(key))
            {
                return string.Empty;
            }

            var value = entity.Attributes[key];

            if (value is EntityReference)
            {
                var reference = (EntityReference)value;

                if (reference.Id == Guid.Empty)
                {
                    entity.Attributes.Remove(key);
                    return string.Empty;
                }
            }

            if (value == null)
            {
                return string.Empty;
            }

            return getterString(entity.FormattedValues, key, value, connectionData);
        }

        public static string GetAttributeString(Entity entity, string key, ConnectionData connectionData = null)
        {
            return GetAttributeStringInternal(entity, key, GetValueStringFull, connectionData);
        }

        public static string GetAttributeStringShortEntityReferenceAndPicklist(Entity entity, string key, ConnectionData connectionData = null)
        {
            return GetAttributeStringInternal(entity, key, GetValueStringShortEntityReferenceAndPicklist, connectionData);
        }

        private static string GetValueStringFull(FormattedValueCollection formattedValues, string key, object value, ConnectionData connectionData)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is EntityReference entityReference)
            {
                StringBuilder result = new StringBuilder();

                if (!string.IsNullOrEmpty(entityReference.Name))
                {
                    result.AppendFormat("{0} - ", entityReference.Name);
                }

                result.AppendFormat("{0} - {1}", entityReference.LogicalName, entityReference.Id.ToString());

                if (connectionData != null)
                {
                    var url = connectionData.GetEntityInstanceUrl(entityReference.LogicalName, entityReference.Id);

                    if (!string.IsNullOrEmpty(url))
                    {
                        result.AppendFormat(" - {0}", url);
                    }
                }

                return result.ToString();
            }

            if (value is BooleanManagedProperty booleanManaged)
            {
                return string.Format("{0,-5}        CanBeChanged = {1,-5}", booleanManaged.Value, booleanManaged.CanBeChanged);
            }

            if (value is OptionSetValue optionSetValue)
            {
                return optionSetValue.Value.ToString() + (formattedValues.ContainsKey(key) ? string.Format(" - {0}", formattedValues[key]) : string.Empty);
            }

            if (value is Money money)
            {
                return money.Value.ToString();
            }

            if (value is DateTime dateTime)
            {
                return dateTime.ToLocalTime().ToString("G", System.Globalization.CultureInfo.CurrentCulture);
            }

            if (value is AliasedValue aliasedValue)
            {
                return GetValueStringFull(formattedValues, key, aliasedValue.Value, connectionData);
            }

            return value.ToString();
        }

        private static string GetValueStringShortEntityReferenceAndPicklist(FormattedValueCollection formattedValues, string key, object value, ConnectionData connectionData)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is EntityReference entityReference)
            {
                if (!string.IsNullOrEmpty(entityReference.Name))
                {
                    return entityReference.Name;
                }
                else
                {
                    return string.Format("{0} - {1}", entityReference.LogicalName, entityReference.Id.ToString());
                }
            }

            if (value is BooleanManagedProperty booleanManaged)
            {
                return booleanManaged.Value.ToString();
            }

            if (value is OptionSetValue optionSetValue)
            {
                return formattedValues.ContainsKey(key) && !string.IsNullOrEmpty(formattedValues[key]) ? formattedValues[key] : optionSetValue.Value.ToString();
            }

            if (value is Money money)
            {
                return money.Value.ToString();
            }

            if (value is DateTime dateTime)
            {
                return dateTime.ToLocalTime().ToString("G", System.Globalization.CultureInfo.CurrentCulture);
            }

            if (value is AliasedValue aliasedValue)
            {
                return GetValueStringShortEntityReferenceAndPicklist(formattedValues, key, aliasedValue.Value, connectionData);
            }

            return value.ToString();
        }

        public static string GetAttributeStringShortEntityReference(Entity entity, string key)
        {
            return GetAttributeStringInternal(entity, key, GetValueStringShortEntityReference, null);
        }

        private static string GetValueStringShortEntityReference(FormattedValueCollection formattedValues, string key, object value, ConnectionData connectionData)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is EntityReference entityReference)
            {
                if (!string.IsNullOrEmpty(entityReference.Name))
                {
                    return entityReference.Name;
                }
                else
                {
                    return string.Format("{0} - {1}", entityReference.LogicalName, entityReference.Id.ToString());
                }
            }

            if (value is BooleanManagedProperty booleanManagedboolean)
            {
                return string.Format("{0,-5}    CanBeChanged = {1,-5}", booleanManagedboolean.Value, booleanManagedboolean.CanBeChanged);
            }

            if (value is OptionSetValue optionSetValue)
            {
                return optionSetValue.Value.ToString() + (formattedValues.ContainsKey(key) ? string.Format(" - {0}", formattedValues[key]) : string.Empty);
            }

            if (value is Money money)
            {
                return money.Value.ToString();
            }

            if (value is DateTime dateTime)
            {
                return dateTime.ToLocalTime().ToString("G", System.Globalization.CultureInfo.CurrentCulture);
            }

            if (value is AliasedValue aliasedValue)
            {
                return GetValueStringShortEntityReference(formattedValues, key, aliasedValue.Value, connectionData);
            }

            return value.ToString();
        }

        public static Task ExportEntityDescriptionAsync(string filePath, Entity entity, ICollection<string> list, ConnectionData connectionData = null)
        {
            return Task.Run(() => ExportEntityDescription(filePath, entity, list, connectionData));
        }

        private static void ExportEntityDescription(string filePath, Entity entity, ICollection<string> list, ConnectionData connectionData)
        {
            string content = GetEntityDescription(entity, list, connectionData);

            File.WriteAllText(filePath, content, new UTF8Encoding(false));
        }
    }
}
