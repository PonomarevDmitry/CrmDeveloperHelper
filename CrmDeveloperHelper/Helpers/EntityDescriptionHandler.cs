using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class EntityDescriptionHandler
    {
        private const string _defaultTabSpacer = "    ";

        public static Task ExportEntityDescriptionAsync(string filePath, Entity entity, ConnectionData connectionData = null)
        {
            return Task.Run(() => ExportEntityDescription(filePath, entity, connectionData));
        }

        private static void ExportEntityDescription(string filePath, Entity entity, ConnectionData connectionData)
        {
            string content = GetEntityDescription(entity, connectionData);

            File.WriteAllText(filePath, content, new UTF8Encoding(false));
        }

        public static Task<string> GetEntityDescriptionAsync(Entity entity, ConnectionData connectionData = null)
        {
            return Task.Run(() => GetEntityDescription(entity, connectionData));
        }

        private static string GetEntityDescription(Entity entity, ConnectionData connectionData)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("Entity : {0}", entity.LogicalName).AppendLine();
            result.AppendFormat("Id     : {0}", entity.Id).AppendLine();

            if (connectionData != null)
            {
                var url = connectionData.GetEntityInstanceUrl(entity.LogicalName, entity.Id);

                if (!string.IsNullOrEmpty(url))
                {
                    result.AppendFormat("Url    : {0}", url).AppendLine();
                }
            }

            result.AppendLine();

            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("Field Name", "Type", "Value");

            IEnumerable<string> attributeToIgnore = GetAttributesToIgnore(entity.LogicalName);

            HashSet<string> hashAttributeToIgnore = new HashSet<string>(attributeToIgnore, StringComparer.InvariantCultureIgnoreCase);

            foreach (var attr in entity.Attributes.OrderBy(s => s.Key))
            {
                if (hashAttributeToIgnore.Any())
                {
                    if (hashAttributeToIgnore.Contains(attr.Key))
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

        public static string GetAttributeStringShortEntityReference(Entity entity, string key)
        {
            return GetAttributeStringInternal(entity, key, GetValueStringShortEntityReference, null);
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

            if (value is OptionSetValueCollection valueOptionSetValueCollection)
            {
                string valuesString = valueOptionSetValueCollection.Any() ? string.Join(",", valueOptionSetValueCollection.Select(o => o.Value).OrderBy(o => o)) : "none";

                return valuesString + (formattedValues.ContainsKey(key) ? string.Format(" - {0}", formattedValues[key]) : string.Empty);
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

            if (value is EntityCollection entityCollection)
            {
                StringBuilder result = new StringBuilder();

                result.AppendFormat("EnitityCollection {0}: {1}", entityCollection.EntityName, (entityCollection.Entities?.Count).GetValueOrDefault()).AppendLine();

                foreach (var item in entityCollection.Entities)
                {
                    string entityDesc = GetEntityDescription(item, connectionData);

                    if (!string.IsNullOrEmpty(entityDesc))
                    {
                        var split = entityDesc.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var str in split)
                        {
                            result.AppendLine(_defaultTabSpacer + _defaultTabSpacer + str);
                        }
                    }
                }

                return result.ToString();
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

            if (value is OptionSetValueCollection valueOptionSetValueCollection)
            {
                string valuesString = valueOptionSetValueCollection.Any() ? string.Join(",", valueOptionSetValueCollection.Select(o => o.Value).OrderBy(o => o)) : "none";

                return formattedValues.ContainsKey(key) && !string.IsNullOrEmpty(formattedValues[key]) ? formattedValues[key] : valuesString;
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

            if (value is EntityCollection entityCollection)
            {
                return string.Format("EnitityCollection {0}: {1}", entityCollection.EntityName, (entityCollection.Entities?.Count).GetValueOrDefault());
            }

            return value.ToString();
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

            if (value is OptionSetValueCollection valueOptionSetValueCollection)
            {
                string valuesString = valueOptionSetValueCollection.Any() ? string.Join(",", valueOptionSetValueCollection.Select(o => o.Value).OrderBy(o => o)) : "none";

                return valuesString + (formattedValues.ContainsKey(key) ? string.Format(" - {0}", formattedValues[key]) : string.Empty);
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

            if (value is EntityCollection entityCollection)
            {
                return string.Format("EnitityCollection {0}: {1}", entityCollection.EntityName, (entityCollection.Entities?.Count).GetValueOrDefault());
            }

            return value.ToString();
        }

        public const string ColumnOriginalEntity = "columnOriginalEntity______";

        public static DataTable ConvertEntityCollectionToDataTable(ConnectionData connectionData, EntityCollection entityCollection, out Dictionary<string, string> columnMapping)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(ColumnOriginalEntity, typeof(Entity));

            columnMapping = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var entity in entityCollection.Entities)
            {
                DataRow row = dataTable.NewRow();

                row[ColumnOriginalEntity] = entity;

                foreach (string attributeName in entity.Attributes.Keys)
                {
                    var value = entity.Attributes[attributeName];

                    if (value == null || EntityDescriptionHandler.GetUnderlyingValue(value) == null)
                    {
                        continue;
                    }

                    string entityPrimaryIdAttribute = connectionData.GetEntityPrimaryIdAttribute(entity.LogicalName);

                    if (value is Guid idValue
                        && string.Equals(entityPrimaryIdAttribute, attributeName, StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        value = new PrimaryGuidView(connectionData, entity.LogicalName, idValue);
                    }

                    string columnName = string.Format("{0}___{1}", entity.LogicalName, attributeName.Replace(".", "_"));

                    if (value is AliasedValue aliasedValue)
                    {
                        columnName = string.Format("{0}___{1}___{2}", attributeName.Replace(".", "_"), aliasedValue.EntityLogicalName, aliasedValue.AttributeLogicalName);

                        string aliasedValuePrimaryIdAttribute = connectionData.GetEntityPrimaryIdAttribute(aliasedValue.EntityLogicalName);

                        if (aliasedValue.Value is Guid refIdValue
                            && string.Equals(aliasedValuePrimaryIdAttribute, aliasedValue.AttributeLogicalName, StringComparison.InvariantCultureIgnoreCase)
                        )
                        {
                            value = new PrimaryGuidView(connectionData, aliasedValue.EntityLogicalName, refIdValue);
                        }
                    }

                    value = EntityDescriptionHandler.GetUnderlyingValue(value);

                    if (value is EntityReference entityReference)
                    {
                        value = new EntityReferenceView(connectionData, entityReference.LogicalName, entityReference.Id, entityReference.Name);
                    }

                    if (value is Money money)
                    {
                        value = money.Value;
                    }

                    if (value is DateTime dateTime)
                    {
                        value = dateTime.ToLocalTime();
                    }

                    if (value is OptionSetValue optionSetValue)
                    {
                        value = (entity.FormattedValues != null && entity.FormattedValues.ContainsKey(attributeName) ? string.Format("{0} - ", entity.FormattedValues[attributeName]) : string.Empty) + optionSetValue.Value.ToString();
                    }

                    if (value is OptionSetValueCollection valueOptionSetValueCollection)
                    {
                        string valuesString = valueOptionSetValueCollection.Any() ? string.Join(",", valueOptionSetValueCollection.Select(o => o.Value).OrderBy(o => o)) : "none";

                        value = (entity.FormattedValues != null && entity.FormattedValues.ContainsKey(attributeName) ? string.Format("{0} - ", entity.FormattedValues[attributeName]) : string.Empty) + valuesString;
                    }

                    if (value is BooleanManagedProperty booleanManagedProperty)
                    {
                        value = string.Format("{0,-5}        CanBeChanged = {1,-5}", booleanManagedProperty.Value, booleanManagedProperty.CanBeChanged);
                    }

                    if (value is EntityCollection valueEntityCollection)
                    {
                        value = string.Format("EnitityCollection {0}: {1}", valueEntityCollection.EntityName, (valueEntityCollection.Entities?.Count).GetValueOrDefault());
                    }

                    if (dataTable.Columns.IndexOf(columnName) == -1)
                    {
                        if (!columnMapping.ContainsKey(attributeName))
                        {
                            columnMapping.Add(attributeName, columnName);
                        }

                        dataTable.Columns.Add(columnName, value.GetType());
                    }

                    row[columnName] = value;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public static List<string> GetColumnsFromFetch(ConnectionData connectionData, XElement fetchXml)
        {
            var hashColumns = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            var result = new List<string>();

            var linkElements = fetchXml.DescendantsAndSelf().Where(n => IsLinkElement(n)).ToList();

            {
                var entityElements = fetchXml.DescendantsAndSelf().Where(IsAttributeOrAllElement).ToList();

                foreach (var attributeNode in entityElements)
                {
                    if (IsAttributeElement(attributeNode))
                    {
                        var attrAlias = attributeNode.Attribute("alias");

                        if (attrAlias != null && !string.IsNullOrEmpty(attrAlias.Value))
                        {
                            if (hashColumns.Add(attrAlias.Value))
                            {
                                result.Add(attrAlias.Value);
                            }
                        }
                        else
                        {
                            string name = attributeNode.Attribute("name").Value;

                            var parentElement = attributeNode.Ancestors().FirstOrDefault(IsEntityOrLinkElement);

                            if (parentElement != null)
                            {
                                var parentAlias = parentElement.Attribute("alias");

                                if (parentAlias != null && !string.IsNullOrEmpty(parentAlias.Value))
                                {
                                    name = parentAlias.Value + "." + name;
                                }
                                else if (IsLinkElement(parentElement))
                                {
                                    if (linkElements.Contains(parentElement))
                                    {
                                        var index = linkElements.IndexOf(parentElement) + 1;

                                        name = parentElement.Attribute("name")?.Value + index.ToString() + "." + name;
                                    }
                                }
                            }

                            if (hashColumns.Add(name))
                            {
                                result.Add(name);
                            }
                        }
                    }
                    else if (IsAllAttributesElement(attributeNode))
                    {
                        var parentElement = attributeNode.Ancestors().FirstOrDefault(IsEntityOrLinkElement);

                        if (parentElement != null)
                        {
                            var parentAlias = parentElement.Attribute("alias");
                            var parentEntityName = parentElement.Attribute("name");

                            string parentPrefixName = string.Empty;

                            if (parentAlias != null && !string.IsNullOrEmpty(parentAlias.Value))
                            {
                                parentPrefixName = parentAlias.Value + ".";
                            }
                            else if (IsLinkElement(parentElement))
                            {
                                if (linkElements.Contains(parentElement))
                                {
                                    var index = linkElements.IndexOf(parentElement) + 1;

                                    parentPrefixName = parentElement.Attribute("name")?.Value + index.ToString();
                                }
                            }

                            var intellisenseData = connectionData.EntitiesIntellisenseData;

                            if (intellisenseData != null
                                && intellisenseData.Entities != null
                                && intellisenseData.Entities.ContainsKey(parentEntityName.Value)
                                && intellisenseData.Entities[parentEntityName.Value].Attributes != null
                            )
                            {
                                foreach (var attrName in intellisenseData.Entities[parentEntityName.Value].Attributes.Keys.OrderBy(s => s))
                                {
                                    string name = parentPrefixName + attrName;

                                    if (hashColumns.Add(name))
                                    {
                                        result.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsEntityOrLinkElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "entity", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(element.Name.LocalName, "link-entity", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsLinkElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "link-entity", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsAttributeOrAllElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "attribute", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(element.Name.LocalName, "all-attributes", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsAttributeElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "attribute", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsAllAttributesElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "all-attributes", StringComparison.InvariantCultureIgnoreCase);
        }

        private static IEnumerable<string> GetAttributesToIgnore(string entityLogicalName)
        {
            entityLogicalName = entityLogicalName.ToLower();

            switch (entityLogicalName)
            {
                case CustomControl.EntityLogicalName:
                    return CustomControlIgnoreFields;

                case WebResource.EntityLogicalName:
                    return WebResourceIgnoreFields;

                case SystemForm.EntityLogicalName:
                    return SystemFormIgnoreFields;

                case SavedQuery.EntityLogicalName:
                    return SavedQueryIgnoreFields;

                case SavedQueryVisualization.EntityLogicalName:
                    return SavedQueryVisualizationIgnoreFields;

                case SiteMap.EntityLogicalName:
                    return SiteMapIgnoreFields;

                case Report.EntityLogicalName:
                    return ReportIgnoreFields;

                case PluginAssembly.EntityLogicalName:
                    return PluginAssemblyIgnoreFields;

                case PluginType.EntityLogicalName:
                    return PluginTypeIgnoreFields;

                case Workflow.EntityLogicalName:
                    return WorkflowIgnoreFields;

                case Organization.EntityLogicalName:
                    return OrganizationIgnoreFields;

                case ImportJob.EntityLogicalName:
                    return ImportJobIgnoreFields;
            }

            return Enumerable.Empty<string>();
        }

        #region IgnoreAttributes

        private static readonly IReadOnlyCollection<string> CustomControlIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            CustomControl.Schema.Attributes.manifest
            , CustomControl.Schema.Attributes.clientjson
        });

        private static readonly IReadOnlyCollection<string> WebResourceIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            WebResource.Schema.Attributes.content
            , WebResource.Schema.Attributes.contentjson
            , WebResource.Schema.Attributes.dependencyxml
        });

        private static readonly IReadOnlyCollection<string> SystemFormIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            SystemForm.Schema.Attributes.formxml
            , SystemForm.Schema.Attributes.formjson
        });

        private static readonly IReadOnlyCollection<string> SavedQueryIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            SavedQuery.Schema.Attributes.fetchxml
            , SavedQuery.Schema.Attributes.layoutxml
            , SavedQuery.Schema.Attributes.columnsetxml
            , SavedQuery.Schema.Attributes.layoutjson
            , SavedQuery.Schema.Attributes.offlinesqlquery
        });

        private static readonly IReadOnlyCollection<string> SavedQueryVisualizationIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            SavedQueryVisualization.Schema.Attributes.datadescription
            , SavedQueryVisualization.Schema.Attributes.presentationdescription
        });

        private static readonly IReadOnlyCollection<string> SiteMapIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            SiteMap.Schema.Attributes.sitemapxml
        });

        private static readonly IReadOnlyCollection<string> ReportIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            Report.Schema.Attributes.bodytext
            , Report.Schema.Attributes.originalbodytext
            , Report.Schema.Attributes.bodybinary
            , Report.Schema.Attributes.defaultfilter
            , Report.Schema.Attributes.customreportxml
            , Report.Schema.Attributes.schedulexml
            , Report.Schema.Attributes.queryinfo
        });

        private static readonly IReadOnlyCollection<string> PluginAssemblyIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            PluginAssembly.Schema.Attributes.content
        });

        private static readonly IReadOnlyCollection<string> PluginTypeIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            PluginType.Schema.Attributes.customworkflowactivityinfo
        });

        private static readonly IReadOnlyCollection<string> WorkflowIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            Workflow.Schema.Attributes.xaml
            , Workflow.Schema.Attributes.inputparameters
            , Workflow.Schema.Attributes.clientdata
            , Workflow.Schema.Attributes.uidata
        });

        private static readonly IReadOnlyCollection<string> OrganizationIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            Organization.Schema.Attributes.defaultemailsettings
            , Organization.Schema.Attributes.externalpartycorrelationkeys
            , Organization.Schema.Attributes.externalpartyentitysettings
            , Organization.Schema.Attributes.featureset
            , Organization.Schema.Attributes.kmsettings
            , Organization.Schema.Attributes.referencesitemapxml
            , Organization.Schema.Attributes.sitemapxml
            , Organization.Schema.Attributes.defaultthemedata
            , Organization.Schema.Attributes.highcontrastthemedata
            , Organization.Schema.Attributes.slapausestates
        });

        private static readonly IReadOnlyCollection<string> ImportJobIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            ImportJob.Schema.Attributes.data
        });

        #endregion IgnoreAttributes
    }
}
