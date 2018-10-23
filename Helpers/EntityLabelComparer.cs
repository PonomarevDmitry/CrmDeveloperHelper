using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    class EntityLabelComparer
    {
        private string _tabSpacer;
        private string _connectionName1;
        private string _connectionName2;

        private HashSet<string> _notExising;

        private OptionSetComparer _optionSetComparer;

        public EntityLabelComparer(string tabSpacer, string connectionName1, string connectionName2, OptionSetComparer optionSetComparer, HashSet<string> notExising)
        {
            this._tabSpacer = tabSpacer;
            this._connectionName1 = connectionName1;
            this._connectionName2 = connectionName2;
            this._notExising = notExising;
            this._optionSetComparer = optionSetComparer;
        }

        public async Task<List<string>> GetDifference(OrganizationDifferenceImageBuilder imageBuilder, EntityMetadata entityMetadata1, EntityMetadata entityMetadata2)
        {
            List<string> strDifference = new List<string>();

            {
                FormatTextTableHandler table = new FormatTextTableHandler(true);

                table.CalculateLineLengths("LanguageCode", "Value");
                table.CalculateLineLengths("LanguageCode", "Organization", "Value");

                var isDifferentDisplayName = LabelComparer.GetDifference(entityMetadata1.DisplayName, entityMetadata2.DisplayName);
                var isDifferentDescription = LabelComparer.GetDifference(entityMetadata1.Description, entityMetadata2.Description);

                isDifferentDisplayName.LabelsOnlyIn1.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayName.LabelsOnlyIn2.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayName.LabelDifference.ForEach(i =>
                {
                    table.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    table.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                isDifferentDescription.LabelsOnlyIn1.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDescription.LabelsOnlyIn2.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDescription.LabelDifference.ForEach(i =>
                {
                    table.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    table.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                if (!isDifferentDisplayName.IsEmpty)
                {
                    if (isDifferentDisplayName.LabelsOnlyIn1.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames ONLY in {0}: {1}", _connectionName1, isDifferentDisplayName.LabelsOnlyIn1.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayName.LabelsOnlyIn1.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayName.LabelsOnlyIn2.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames ONLY in {0}: {1}", _connectionName2, isDifferentDisplayName.LabelsOnlyIn2.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayName.LabelsOnlyIn2.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayName.LabelDifference.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDisplayName.LabelDifference.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDisplayName.LabelDifference.ForEach(e =>
                        {
                            strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, _connectionName1, e.Value1));
                            strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, _connectionName2, e.Value2));
                        });
                    }
                }

                if (!isDifferentDescription.IsEmpty)
                {
                    if (isDifferentDescription.LabelsOnlyIn1.Count > 0)
                    {
                        strDifference.Add(string.Format("Descriptions ONLY in {0}: {1}", _connectionName1, isDifferentDescription.LabelsOnlyIn1.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDescription.LabelsOnlyIn1.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDescription.LabelsOnlyIn2.Count > 0)
                    {
                        strDifference.Add(string.Format("Descriptions ONLY in {0}: {1}", _connectionName2, isDifferentDescription.LabelsOnlyIn2.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDescription.LabelsOnlyIn2.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDescription.LabelDifference.Count > 0)
                    {
                        strDifference.Add(string.Format("Descriptions DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDescription.LabelDifference.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDescription.LabelDifference.ForEach(e =>
                        {
                            strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, _connectionName1, e.Value1));
                            strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, _connectionName2, e.Value2));
                        });
                    }
                }
            }

            {
                FormatTextTableHandler table = new FormatTextTableHandler(true);

                table.CalculateLineLengths("LanguageCode", "Value");
                table.CalculateLineLengths("LanguageCode", "Organization", "Value");

                var isDifferentDisplayCollectionName = LabelComparer.GetDifference(entityMetadata1.DisplayCollectionName, entityMetadata2.DisplayCollectionName);

                isDifferentDisplayCollectionName.LabelsOnlyIn1.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayCollectionName.LabelsOnlyIn2.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayCollectionName.LabelDifference.ForEach(i =>
                {
                    table.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    table.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                if (!isDifferentDisplayCollectionName.IsEmpty)
                {
                    if (isDifferentDisplayCollectionName.LabelsOnlyIn1.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayCollectionNames ONLY in {0}: {1}", _connectionName1, isDifferentDisplayCollectionName.LabelsOnlyIn1.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayCollectionName.LabelsOnlyIn1.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayCollectionName.LabelsOnlyIn2.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayCollectionNames ONLY in {0}: {1}", _connectionName2, isDifferentDisplayCollectionName.LabelsOnlyIn2.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayCollectionName.LabelsOnlyIn2.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayCollectionName.LabelDifference.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayCollectionNames DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDisplayCollectionName.LabelDifference.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDisplayCollectionName.LabelDifference.ForEach(e =>
                        {
                            strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, _connectionName1, e.Value1));
                            strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, _connectionName2, e.Value2));
                        });
                    }
                }
            }

            await CompareAttributes(imageBuilder, strDifference, entityMetadata1.LogicalName, entityMetadata1.Attributes, entityMetadata2.Attributes);

            CompareKeys(imageBuilder, strDifference, entityMetadata1.LogicalName, entityMetadata1.Keys ?? Enumerable.Empty<EntityKeyMetadata>(), entityMetadata2.Keys ?? Enumerable.Empty<EntityKeyMetadata>());

            CompareOneToMany(imageBuilder, strDifference, entityMetadata1.LogicalName, "N:1", "ManyToOne", entityMetadata1.ManyToOneRelationships, entityMetadata2.ManyToOneRelationships);

            CompareOneToMany(imageBuilder, strDifference, entityMetadata1.LogicalName, "1:N", "OneToMany", entityMetadata1.OneToManyRelationships, entityMetadata2.OneToManyRelationships);

            CompareManyToMany(imageBuilder, strDifference, entityMetadata1.LogicalName, entityMetadata1.ManyToManyRelationships, entityMetadata2.ManyToManyRelationships);

            return strDifference;
        }

        private void CompareOneToMany(OrganizationDifferenceImageBuilder imageBuilder, List<string> strDifference, string entityName, string className, string relationTypeName, IEnumerable<OneToManyRelationshipMetadata> listRel1, IEnumerable<OneToManyRelationshipMetadata> listRel2)
        {
            Dictionary<string, List<string>> dictDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var rel1 in listRel1.OrderBy(s => s.SchemaName))
            {
                var rel2 = listRel2.FirstOrDefault(s => s.SchemaName == rel1.SchemaName);

                if (rel2 == null)
                {
                    continue;
                }

                if (_notExising.Contains(rel1.ReferencedEntity) || _notExising.Contains(rel1.ReferencingEntity))
                {
                    continue;
                }

                if (_notExising.Contains(rel2.ReferencedEntity) || _notExising.Contains(rel2.ReferencingEntity))
                {
                    continue;
                }

                List<string> diff = GetDifferenceOneToMany(rel1, rel2);

                if (diff.Count > 0)
                {
                    dictDifference.Add(rel1.SchemaName, diff);

                    imageBuilder.AddComponentDifferent((int)ComponentType.EntityRelationship, rel1.MetadataId.Value, rel2.MetadataId.Value, string.Join(Environment.NewLine, diff));
                }
            }

            if (dictDifference.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("{0} - {1} DIFFERENT in {2} and {3}: {4}", className, relationTypeName, _connectionName1, _connectionName2, dictDifference.Count));

                foreach (var item in dictDifference.OrderBy(e => e.Key))
                {
                    if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                    strDifference.Add(string.Format("Different {0} - {1} {2}", className, relationTypeName, item.Key));

                    item.Value.ForEach(s => strDifference.Add(_tabSpacer + s));
                }
            }
        }

        private List<string> GetDifferenceOneToMany(OneToManyRelationshipMetadata rel1, OneToManyRelationshipMetadata rel2)
        {
            var table = new FormatTextTableHandler(true);
            table.SetHeader("Property", _connectionName1, _connectionName2);

            AddAssociatedMenuConfigurationDifference(table, "AssociatedMenuConfiguration", rel1.AssociatedMenuConfiguration, rel2.AssociatedMenuConfiguration);

            List<string> result = new List<string>();

            if (table.Count > 0)
            {
                result.AddRange(table.GetFormatedLines(false));
            }

            return result;
        }

        private void CompareManyToMany(OrganizationDifferenceImageBuilder imageBuilder, List<string> strDifference, string entityName, IEnumerable<ManyToManyRelationshipMetadata> listRel1, IEnumerable<ManyToManyRelationshipMetadata> listRel2)
        {
            Dictionary<string, List<string>> dictDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var rel1 in listRel1.OrderBy(s => s.SchemaName))
            {
                var rel2 = listRel2.FirstOrDefault(s => s.SchemaName == rel1.SchemaName);

                if (rel2 == null)
                {
                    continue;
                }

                if (_notExising.Contains(rel1.Entity1LogicalName) || _notExising.Contains(rel1.Entity2LogicalName))
                {
                    continue;
                }

                if (_notExising.Contains(rel2.Entity1LogicalName) || _notExising.Contains(rel2.Entity2LogicalName))
                {
                    continue;
                }

                List<string> diff = GetDifferenceManyToMany(rel1, rel2);

                if (diff.Count > 0)
                {
                    dictDifference.Add(rel1.SchemaName, diff);

                    imageBuilder.AddComponentDifferent((int)ComponentType.EntityRelationship, rel1.MetadataId.Value, rel2.MetadataId.Value, string.Join(Environment.NewLine, diff));
                }
            }

            if (dictDifference.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("ManyToMany DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, dictDifference.Count));

                foreach (var item in dictDifference.OrderBy(e => e.Key))
                {
                    if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                    strDifference.Add(string.Format("Different ManyToMany {0}", item.Key));

                    item.Value.ForEach(s => strDifference.Add(_tabSpacer + s));
                }
            }
        }

        private List<string> GetDifferenceManyToMany(ManyToManyRelationshipMetadata rel1, ManyToManyRelationshipMetadata rel2)
        {
            var table = new FormatTextTableHandler(true);
            table.SetHeader("Property", _connectionName1, _connectionName2);

            AddAssociatedMenuConfigurationDifference(table, "Entity1AssociatedMenuConfiguration", rel1.Entity1AssociatedMenuConfiguration, rel2.Entity1AssociatedMenuConfiguration);
            AddAssociatedMenuConfigurationDifference(table, "Entity2AssociatedMenuConfiguration", rel1.Entity2AssociatedMenuConfiguration, rel2.Entity2AssociatedMenuConfiguration);

            List<string> result = new List<string>();

            if (table.Count > 0)
            {
                result.AddRange(table.GetFormatedLines(false));
            }

            return result;
        }

        private void AddAssociatedMenuConfigurationDifference(FormatTextTableHandler table, string name, AssociatedMenuConfiguration config1, AssociatedMenuConfiguration config2)
        {
            if (config1 != null && config2 == null)
            {
                table.AddLineIfNotEqual(name, "not null", "null");
            }
            else if (config1 == null && config2 != null)
            {
                table.AddLineIfNotEqual(name, "null", "not null");
            }
            else if (config1 != null && config2 != null)
            {
                if (config1.Label != null && config2.Label == null)
                {
                    table.AddLineIfNotEqual(name + ".Label", "not null", "null");
                }
                else if (config1.Label == null && config2.Label != null)
                {
                    table.AddLineIfNotEqual(name + ".Label", "null", "not null");
                }
                else if (config1.Label != null && config2.Label != null)
                {
                    var isDifferentLabel = LabelComparer.GetDifference(config1.Label, config2.Label);

                    if (!isDifferentLabel.IsEmpty)
                    {
                        table.AddLine(name + ".Label");

                        if (isDifferentLabel.LabelsOnlyIn1.Count > 0)
                        {
                            table.AddLine(string.Format("Labels ONLY in {0}: {1}", _connectionName1, isDifferentLabel.LabelsOnlyIn1.Count));
                            table.AddLine("LanguageCode", "Value");
                            isDifferentLabel.LabelsOnlyIn1.ForEach(e => table.AddLine(e.Locale, e.Value));
                        }

                        if (isDifferentLabel.LabelsOnlyIn2.Count > 0)
                        {
                            table.AddLine(string.Format("Labels ONLY in {0}: {1}", _connectionName2, isDifferentLabel.LabelsOnlyIn2.Count));
                            table.AddLine("LanguageCode", "Value");
                            isDifferentLabel.LabelsOnlyIn2.ForEach(e => table.AddLine(e.Locale, e.Value));
                        }

                        if (isDifferentLabel.LabelDifference.Count > 0)
                        {
                            table.AddLine(string.Format("Labels DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentLabel.LabelDifference.Count));
                            table.AddLine("LanguageCode", "Organization", "Value");
                            isDifferentLabel.LabelDifference.ForEach(e => table.AddLine(e.Locale, e.Value1, e.Value2));
                        }
                    }
                }
            }
        }

        private void CompareKeys(OrganizationDifferenceImageBuilder imageBuilder, List<string> strDifference, string entityName, IEnumerable<EntityKeyMetadata> keys1, IEnumerable<EntityKeyMetadata> keys2)
        {
            Dictionary<string, List<string>> dictDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var key1 in keys1.OrderBy(s => s.LogicalName))
            {
                var key2 = keys2.FirstOrDefault(s => s.LogicalName == key1.LogicalName);

                if (key2 == null)
                {
                    continue;
                }

                List<string> diff = GetDifferenceKey(key1, key2);

                if (diff.Count > 0)
                {
                    dictDifference.Add(key1.LogicalName, diff.Select(s => _tabSpacer + s).ToList());

                    imageBuilder.AddComponentDifferent((int)ComponentType.EntityKey, key1.MetadataId.Value, key2.MetadataId.Value, string.Join(Environment.NewLine, diff));
                }
            }

            if (dictDifference.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("Keys DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, dictDifference.Count));

                foreach (var item in dictDifference.OrderBy(e => e.Key))
                {
                    if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                    strDifference.Add(string.Format("Different Keys {0}", item.Key));

                    strDifference.AddRange(item.Value);
                }
            }
        }

        private List<string> GetDifferenceKey(EntityKeyMetadata key1, EntityKeyMetadata key2)
        {
            List<string> strDifference = new List<string>();

            {
                FormatTextTableHandler tableFormatter = new FormatTextTableHandler(true);

                tableFormatter.CalculateLineLengths("LanguageCode", "Value");
                tableFormatter.CalculateLineLengths("LanguageCode", "Organization", "Value");

                var isDifferentDisplayName = LabelComparer.GetDifference(key1.DisplayName, key2.DisplayName);

                isDifferentDisplayName.LabelsOnlyIn1.ForEach(i => tableFormatter.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayName.LabelsOnlyIn2.ForEach(i => tableFormatter.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayName.LabelDifference.ForEach(i =>
                {
                    tableFormatter.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    tableFormatter.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                if (!isDifferentDisplayName.IsEmpty)
                {
                    if (isDifferentDisplayName.LabelsOnlyIn1.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames ONLY in {0}: {1}", _connectionName1, isDifferentDisplayName.LabelsOnlyIn1.Count));
                        strDifference.Add(_tabSpacer + tableFormatter.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayName.LabelsOnlyIn1.ForEach(e => strDifference.Add(_tabSpacer + tableFormatter.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayName.LabelsOnlyIn2.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames ONLY in {0}: {1}", _connectionName2, isDifferentDisplayName.LabelsOnlyIn2.Count));
                        strDifference.Add(_tabSpacer + tableFormatter.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayName.LabelsOnlyIn2.ForEach(e => strDifference.Add(_tabSpacer + tableFormatter.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayName.LabelDifference.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDisplayName.LabelDifference.Count));
                        strDifference.Add(_tabSpacer + tableFormatter.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDisplayName.LabelDifference.ForEach(i =>
                        {
                            strDifference.Add(_tabSpacer + tableFormatter.FormatLine(i.Locale, _connectionName1, i.Value1));
                            strDifference.Add(_tabSpacer + tableFormatter.FormatLine(i.Locale, _connectionName2, i.Value2));
                        });
                    }
                }
            }

            return strDifference;
        }

        private async Task CompareAttributes(OrganizationDifferenceImageBuilder imageBuilder, List<string> strDifference, string entityName, IEnumerable<AttributeMetadata> attributes1, IEnumerable<AttributeMetadata> attributes2)
        {
            Dictionary<string, List<string>> dictDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var attr1 in attributes1.OrderBy(s => s.LogicalName))
            {
                var attr2 = attributes2.FirstOrDefault(s => s.LogicalName == attr1.LogicalName);

                if (attr2 == null)
                {
                    continue;
                }

                List<string> diff = await GetDifferenceAttribute(attr1, attr2);

                if (diff.Count > 0)
                {
                    dictDifference.Add(attr1.LogicalName, diff);

                    imageBuilder.AddComponentDifferent((int)ComponentType.Attribute, attr1.MetadataId.Value, attr2.MetadataId.Value, string.Join(Environment.NewLine, diff));
                }
            }

            if (dictDifference.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("Attributes DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, dictDifference.Count));

                foreach (var item in dictDifference.OrderBy(e => e.Key))
                {
                    if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                    strDifference.Add(string.Format("Different Attribute {0}", item.Key));

                    item.Value.ForEach(s => strDifference.Add(_tabSpacer + s));
                }
            }
        }

        private async Task<List<string>> GetDifferenceAttribute(AttributeMetadata attr1, AttributeMetadata attr2)
        {
            List<string> strDifference = new List<string>();

            {
                FormatTextTableHandler table = new FormatTextTableHandler(true);

                table.CalculateLineLengths("LanguageCode", "Value");
                table.CalculateLineLengths("LanguageCode", "Organization", "Value");

                var isDifferentDisplayName = LabelComparer.GetDifference(attr1.DisplayName, attr2.DisplayName);
                var isDifferentDescription = LabelComparer.GetDifference(attr1.Description, attr2.Description);

                isDifferentDisplayName.LabelsOnlyIn1.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayName.LabelsOnlyIn2.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDisplayName.LabelDifference.ForEach(i =>
                {
                    table.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    table.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                isDifferentDescription.LabelsOnlyIn1.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDescription.LabelsOnlyIn2.ForEach(i => table.CalculateLineLengths(i.Locale, i.Value));
                isDifferentDescription.LabelDifference.ForEach(i =>
                {
                    table.CalculateLineLengths(i.Locale, _connectionName1, i.Value1);
                    table.CalculateLineLengths(i.Locale, _connectionName2, i.Value2);
                });

                if (!isDifferentDisplayName.IsEmpty)
                {
                    if (isDifferentDisplayName.LabelsOnlyIn1.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames ONLY in {0}: {1}", _connectionName1, isDifferentDisplayName.LabelsOnlyIn1.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayName.LabelsOnlyIn1.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayName.LabelsOnlyIn2.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames ONLY in {0}: {1}", _connectionName2, isDifferentDisplayName.LabelsOnlyIn2.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDisplayName.LabelsOnlyIn2.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDisplayName.LabelDifference.Count > 0)
                    {
                        strDifference.Add(string.Format("DisplayNames DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDisplayName.LabelDifference.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDisplayName.LabelDifference.ForEach(i =>
                        {
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName1, i.Value1));
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName2, i.Value2));
                        });
                    }
                }

                if (!isDifferentDescription.IsEmpty)
                {
                    if (isDifferentDescription.LabelsOnlyIn1.Count > 0)
                    {
                        strDifference.Add(string.Format("Descriptions ONLY in {0}: {1}", _connectionName1, isDifferentDescription.LabelsOnlyIn1.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDescription.LabelsOnlyIn1.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDescription.LabelsOnlyIn2.Count > 0)
                    {
                        strDifference.Add(string.Format("Descriptions ONLY in {0}: {1}", _connectionName2, isDifferentDescription.LabelsOnlyIn2.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Value"));
                        isDifferentDescription.LabelsOnlyIn2.ForEach(e => strDifference.Add(_tabSpacer + table.FormatLine(e.Locale, e.Value)));
                    }

                    if (isDifferentDescription.LabelDifference.Count > 0)
                    {
                        strDifference.Add(string.Format("Descriptions DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, isDifferentDescription.LabelDifference.Count));
                        strDifference.Add(_tabSpacer + table.FormatLine("LanguageCode", "Organization", "Value"));
                        isDifferentDescription.LabelDifference.ForEach(i =>
                        {
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName1, i.Value1));
                            strDifference.Add(_tabSpacer + table.FormatLine(i.Locale, _connectionName2, i.Value2));
                        });
                    }
                }
            }

            {
                var table = new FormatTextTableHandler(true);
                table.SetHeader("Property", _connectionName1, _connectionName2);

                List<string> additionalDifference = new List<string>();

                if (attr1.GetType().Name == attr2.GetType().Name)
                {
                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.PicklistAttributeMetadata)
                    {
                        var picklistAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.PicklistAttributeMetadata;
                        var picklistAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.PicklistAttributeMetadata;

                        if (picklistAttrib1.OptionSet != null && picklistAttrib2.OptionSet != null)
                        {
                            if (!CreateFileHandler.IgnoreAttribute(picklistAttrib1.EntityLogicalName, picklistAttrib1.LogicalName))
                            {
                                var diffenrenceOptionSet = await _optionSetComparer.GetDifference(picklistAttrib1.OptionSet, picklistAttrib2.OptionSet, attr1.EntityLogicalName, attr1.LogicalName);

                                if (diffenrenceOptionSet.Count > 0)
                                {
                                    additionalDifference.Add(string.Format("Difference in OptionSet {0} and {1}"
                                        , picklistAttrib1.OptionSet.Name + (picklistAttrib1.OptionSet.IsGlobal.GetValueOrDefault() ? "(Global)" : "(Local)")
                                        , picklistAttrib2.OptionSet.Name + (picklistAttrib2.OptionSet.IsGlobal.GetValueOrDefault() ? "(Global)" : "(Local)")
                                        )
                                        );
                                    diffenrenceOptionSet.ForEach(s => additionalDifference.Add(_tabSpacer + s));
                                }
                            }
                        }
                    }
                }

                if (table.Count > 0)
                {
                    strDifference.AddRange(table.GetFormatedLines(true));
                }

                if (additionalDifference.Count > 0)
                {
                    strDifference.AddRange(additionalDifference);
                }
            }

            return strDifference;
        }

        private bool IgnoreTargetsAttr(string entityName, string attributeName)
        {
            if (attributeName == "regardingobjectid")
            {
                return true;
            }


            if (attributeName.Equals("objectid", StringComparison.OrdinalIgnoreCase))
            {
                if (entityName.Equals("queueitem", StringComparison.OrdinalIgnoreCase)
                    || entityName.Equals("principalobjectattributeaccess", StringComparison.OrdinalIgnoreCase)
                    || entityName.Equals("annotation", StringComparison.OrdinalIgnoreCase)
                    || entityName.Equals("userentityinstancedata", StringComparison.OrdinalIgnoreCase)

                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("duplicaterecord", StringComparison.OrdinalIgnoreCase))
            {
                if (attributeName.Equals("baserecordid", StringComparison.OrdinalIgnoreCase)
                    || attributeName.Equals("duplicaterecordid", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("connection", StringComparison.OrdinalIgnoreCase))
            {
                if (attributeName.Equals("record1id", StringComparison.OrdinalIgnoreCase)
                    || attributeName.Equals("record2id", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
