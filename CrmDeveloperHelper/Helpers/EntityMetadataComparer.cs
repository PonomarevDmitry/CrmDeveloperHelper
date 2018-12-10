using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    internal class EntityMetadataComparer
    {
        private readonly string _tabSpacer;
        private readonly string _connectionName1;
        private readonly string _connectionName2;

        private HashSet<string> _notExising;

        private OptionSetComparer _optionSetComparer;

        public EntityMetadataComparer(string tabSpacer, string connectionName1, string connectionName2, OptionSetComparer optionSetComparer, HashSet<string> notExising)
        {
            _tabSpacer = tabSpacer;
            _connectionName1 = connectionName1;
            _connectionName2 = connectionName2;
            _notExising = notExising;

            _optionSetComparer = optionSetComparer;
        }

        public async Task<List<string>> GetDifferenceAsync(OrganizationDifferenceImageBuilder imageBuilder, EntityMetadata entityMetadata1, EntityMetadata entityMetadata2)
        {
            List<string> strDifference = new List<string>();

            {
                FormatTextTableHandler table = new FormatTextTableHandler(true);
                table.SetHeader("Property", _connectionName1, _connectionName2);

                table.AddLineIfNotEqual("ActivityTypeMask", entityMetadata1.ActivityTypeMask, entityMetadata2.ActivityTypeMask);
                table.AddLineIfNotEqual("AutoCreateAccessTeams", entityMetadata1.AutoCreateAccessTeams, entityMetadata2.AutoCreateAccessTeams);
                table.AddLineIfNotEqual("AutoRouteToOwnerQueue", entityMetadata1.AutoRouteToOwnerQueue, entityMetadata2.AutoRouteToOwnerQueue);
                table.AddLineIfNotEqual("CanBeInManyToMany", entityMetadata1.CanBeInManyToMany, entityMetadata2.CanBeInManyToMany);
                table.AddLineIfNotEqual("CanBePrimaryEntityInRelationship", entityMetadata1.CanBePrimaryEntityInRelationship, entityMetadata2.CanBePrimaryEntityInRelationship);
                table.AddLineIfNotEqual("CanBeRelatedEntityInRelationship", entityMetadata1.CanBeRelatedEntityInRelationship, entityMetadata2.CanBeRelatedEntityInRelationship);
                table.AddLineIfNotEqual("CanChangeHierarchicalRelationship", entityMetadata1.CanChangeHierarchicalRelationship, entityMetadata2.CanChangeHierarchicalRelationship);
                table.AddLineIfNotEqual("CanChangeTrackingBeEnabled", entityMetadata1.CanChangeTrackingBeEnabled, entityMetadata2.CanChangeTrackingBeEnabled);
                table.AddLineIfNotEqual("CanCreateAttributes", entityMetadata1.CanCreateAttributes, entityMetadata2.CanCreateAttributes);
                table.AddLineIfNotEqual("CanCreateCharts", entityMetadata1.CanCreateCharts, entityMetadata2.CanCreateCharts);
                table.AddLineIfNotEqual("CanCreateForms", entityMetadata1.CanCreateForms, entityMetadata2.CanCreateForms);
                table.AddLineIfNotEqual("CanCreateViews", entityMetadata1.CanCreateViews, entityMetadata2.CanCreateViews);
                table.AddLineIfNotEqual("CanEnableSyncToExternalSearchIndex", entityMetadata1.CanEnableSyncToExternalSearchIndex, entityMetadata2.CanEnableSyncToExternalSearchIndex);
                table.AddLineIfNotEqual("CanModifyAdditionalSettings", entityMetadata1.CanModifyAdditionalSettings, entityMetadata2.CanModifyAdditionalSettings);
                table.AddLineIfNotEqual("CanTriggerWorkflow", entityMetadata1.CanTriggerWorkflow, entityMetadata2.CanTriggerWorkflow);
                table.AddLineIfNotEqual("ChangeTrackingEnabled", entityMetadata1.ChangeTrackingEnabled, entityMetadata2.ChangeTrackingEnabled);
                table.AddLineIfNotEqual("CollectionSchemaName", entityMetadata1.CollectionSchemaName, entityMetadata2.CollectionSchemaName);
                table.AddLineIfNotEqual("DaysSinceRecordLastModified", entityMetadata1.DaysSinceRecordLastModified, entityMetadata2.DaysSinceRecordLastModified);
                table.AddLineIfNotEqual("EnforceStateTransitions", entityMetadata1.EnforceStateTransitions, entityMetadata2.EnforceStateTransitions);
                table.AddLineIfNotEqual("EntityColor", entityMetadata1.EntityColor, entityMetadata2.EntityColor);
                table.AddLineIfNotEqual("EntityHelpUrl", entityMetadata1.EntityHelpUrl, entityMetadata2.EntityHelpUrl);
                table.AddLineIfNotEqual("EntityHelpUrlEnabled", entityMetadata1.EntityHelpUrlEnabled, entityMetadata2.EntityHelpUrlEnabled);
                table.AddLineIfNotEqual("EntitySetName", entityMetadata1.EntitySetName, entityMetadata2.EntitySetName);
                table.AddLineIfNotEqual("IconLargeName", entityMetadata1.IconLargeName, entityMetadata2.IconLargeName);
                table.AddLineIfNotEqual("IconMediumName", entityMetadata1.IconMediumName, entityMetadata2.IconMediumName);
                table.AddLineIfNotEqual("IconSmallName", entityMetadata1.IconSmallName, entityMetadata2.IconSmallName);
                table.AddLineIfNotEqual("IsActivity", entityMetadata1.IsActivity, entityMetadata2.IsActivity);
                table.AddLineIfNotEqual("IsActivityParty", entityMetadata1.IsActivityParty, entityMetadata2.IsActivityParty);
                table.AddLineIfNotEqual("IsAIRUpdated", entityMetadata1.IsAIRUpdated, entityMetadata2.IsAIRUpdated);
                table.AddLineIfNotEqual("IsAuditEnabled", entityMetadata1.IsAuditEnabled, entityMetadata2.IsAuditEnabled);
                table.AddLineIfNotEqual("IsAvailableOffline", entityMetadata1.IsAvailableOffline, entityMetadata2.IsAvailableOffline);
                table.AddLineIfNotEqual("IsBusinessProcessEnabled", entityMetadata1.IsBusinessProcessEnabled, entityMetadata2.IsBusinessProcessEnabled);
                table.AddLineIfNotEqual("IsConnectionsEnabled", entityMetadata1.IsConnectionsEnabled, entityMetadata2.IsConnectionsEnabled);
                table.AddLineIfNotEqual("IsCustomEntity", entityMetadata1.IsCustomEntity, entityMetadata2.IsCustomEntity);
                table.AddLineIfNotEqual("IsCustomizable", entityMetadata1.IsCustomizable, entityMetadata2.IsCustomizable);
                table.AddLineIfNotEqual("IsDocumentManagementEnabled", entityMetadata1.IsDocumentManagementEnabled, entityMetadata2.IsDocumentManagementEnabled);
                table.AddLineIfNotEqual("IsDuplicateDetectionEnabled", entityMetadata1.IsDuplicateDetectionEnabled, entityMetadata2.IsDuplicateDetectionEnabled);
                table.AddLineIfNotEqual("IsEnabledForCharts", entityMetadata1.IsEnabledForCharts, entityMetadata2.IsEnabledForCharts);
                table.AddLineIfNotEqual("IsEnabledForExternalChannels", entityMetadata1.IsEnabledForExternalChannels, entityMetadata2.IsEnabledForExternalChannels);
                table.AddLineIfNotEqual("IsEnabledForTrace", entityMetadata1.IsEnabledForTrace, entityMetadata2.IsEnabledForTrace);
                table.AddLineIfNotEqual("IsImportable", entityMetadata1.IsImportable, entityMetadata2.IsImportable);
                table.AddLineIfNotEqual("IsInteractionCentricEnabled", entityMetadata1.IsInteractionCentricEnabled, entityMetadata2.IsInteractionCentricEnabled);
                table.AddLineIfNotEqual("IsIntersect", entityMetadata1.IsIntersect, entityMetadata2.IsIntersect);
                table.AddLineIfNotEqual("IsKnowledgeManagementEnabled", entityMetadata1.IsKnowledgeManagementEnabled, entityMetadata2.IsKnowledgeManagementEnabled);
                table.AddLineIfNotEqual("IsMailMergeEnabled", entityMetadata1.IsMailMergeEnabled, entityMetadata2.IsMailMergeEnabled);
                //table.AddLineIfNotEqual("IsManaged", entityMetadata1.IsManaged, entityMetadata2.IsManaged);
                table.AddLineIfNotEqual("IsMappable", entityMetadata1.IsMappable, entityMetadata2.IsMappable);
                table.AddLineIfNotEqual("IsOfflineInMobileClient", entityMetadata1.IsOfflineInMobileClient, entityMetadata2.IsOfflineInMobileClient);
                table.AddLineIfNotEqual("IsOneNoteIntegrationEnabled", entityMetadata1.IsOneNoteIntegrationEnabled, entityMetadata2.IsOneNoteIntegrationEnabled);
                table.AddLineIfNotEqual("IsOptimisticConcurrencyEnabled", entityMetadata1.IsOptimisticConcurrencyEnabled, entityMetadata2.IsOptimisticConcurrencyEnabled);
                table.AddLineIfNotEqual("IsPrivate", entityMetadata1.IsPrivate, entityMetadata2.IsPrivate);
                table.AddLineIfNotEqual("IsQuickCreateEnabled", entityMetadata1.IsQuickCreateEnabled, entityMetadata2.IsQuickCreateEnabled);
                table.AddLineIfNotEqual("IsReadingPaneEnabled", entityMetadata1.IsReadingPaneEnabled, entityMetadata2.IsReadingPaneEnabled);
                table.AddLineIfNotEqual("IsReadOnlyInMobileClient", entityMetadata1.IsReadOnlyInMobileClient, entityMetadata2.IsReadOnlyInMobileClient);
                table.AddLineIfNotEqual("IsRenameable", entityMetadata1.IsRenameable, entityMetadata2.IsRenameable);
                table.AddLineIfNotEqual("IsStateModelAware", entityMetadata1.IsStateModelAware, entityMetadata2.IsStateModelAware);
                table.AddLineIfNotEqual("IsValidForAdvancedFind", entityMetadata1.IsValidForAdvancedFind, entityMetadata2.IsValidForAdvancedFind);
                table.AddLineIfNotEqual("IsValidForQueue", entityMetadata1.IsValidForQueue, entityMetadata2.IsValidForQueue);
                table.AddLineIfNotEqual("IsVisibleInMobile", entityMetadata1.IsVisibleInMobile, entityMetadata2.IsVisibleInMobile);
                table.AddLineIfNotEqual("IsVisibleInMobileClient", entityMetadata1.IsVisibleInMobileClient, entityMetadata2.IsVisibleInMobileClient);
                table.AddLineIfNotEqual("HasChanged", entityMetadata1.HasChanged, entityMetadata2.HasChanged);
                table.AddLineIfNotEqual("LogicalCollectionName", entityMetadata1.LogicalCollectionName, entityMetadata2.LogicalCollectionName);
                table.AddLineIfNotEqual("LogicalName", entityMetadata1.LogicalName, entityMetadata2.LogicalName);
                table.AddLineIfNotEqual("OwnershipType", entityMetadata1.OwnershipType, entityMetadata2.OwnershipType);
                table.AddLineIfNotEqual("RecurrenceBaseEntityLogicalName", entityMetadata1.RecurrenceBaseEntityLogicalName, entityMetadata2.RecurrenceBaseEntityLogicalName);
                table.AddLineIfNotEqual("ReportViewName", entityMetadata1.ReportViewName, entityMetadata2.ReportViewName);
                table.AddLineIfNotEqual("SchemaName", entityMetadata1.SchemaName, entityMetadata2.SchemaName);
                table.AddLineIfNotEqual("SyncToExternalSearchIndex", entityMetadata1.SyncToExternalSearchIndex, entityMetadata2.SyncToExternalSearchIndex);

                if (table.Count > 0)
                {
                    strDifference.AddRange(table.GetFormatedLines(true));
                }
            }

            await CompareAttributesAsync(imageBuilder, strDifference, entityMetadata1.LogicalName, entityMetadata1.Attributes, entityMetadata2.Attributes);

            CompareKeys(imageBuilder, strDifference, entityMetadata1.LogicalName, entityMetadata1.Keys ?? Enumerable.Empty<EntityKeyMetadata>(), entityMetadata2.Keys ?? Enumerable.Empty<EntityKeyMetadata>());

            CompareOneToMany(imageBuilder, strDifference, entityMetadata1.LogicalName, "N:1", "ManyToOne", entityMetadata1.ManyToOneRelationships, entityMetadata2.ManyToOneRelationships);

            CompareOneToMany(imageBuilder, strDifference, entityMetadata1.LogicalName, "1:N", "OneToMany", entityMetadata1.OneToManyRelationships, entityMetadata2.OneToManyRelationships);

            CompareManyToMany(imageBuilder, strDifference, entityMetadata1.LogicalName, entityMetadata1.ManyToManyRelationships, entityMetadata2.ManyToManyRelationships);

            return strDifference;
        }

        private void CompareOneToMany(OrganizationDifferenceImageBuilder imageBuilder, List<string> strDifference, string entityName, string className, string relationTypeName, IEnumerable<OneToManyRelationshipMetadata> listRel1, IEnumerable<OneToManyRelationshipMetadata> listRel2)
        {
            FormatTextTableHandler listRelOnlyIn1 = new FormatTextTableHandler(true);
            FormatTextTableHandler listRelOnlyIn2 = new FormatTextTableHandler(true);

            listRelOnlyIn1.SetHeader("SchemaName", "ReferencingEntity", "ReferencingAttribute", "ReferencedEntity", "ReferencedAttribute", "IsCustomRelationship", "IsManaged");
            listRelOnlyIn2.SetHeader("SchemaName", "ReferencingEntity", "ReferencingAttribute", "ReferencedEntity", "ReferencedAttribute", "IsCustomRelationship", "IsManaged");

            Dictionary<string, List<string>> dictDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (OneToManyRelationshipMetadata rel2 in listRel2.OrderBy(s => s.SchemaName))
            {
                if (_notExising.Contains(rel2.ReferencedEntity) || _notExising.Contains(rel2.ReferencingEntity))
                {
                    continue;
                }

                {
                    OneToManyRelationshipMetadata rel1 = listRel1.FirstOrDefault(s => string.Equals(s.SchemaName, rel2.SchemaName, StringComparison.InvariantCultureIgnoreCase));

                    if (rel1 != null)
                    {
                        continue;
                    }
                }

                listRelOnlyIn2.AddLine(
                    rel2.SchemaName
                    , rel2.ReferencingEntity
                    , rel2.ReferencingAttribute
                    , rel2.ReferencedEntity
                    , rel2.ReferencedAttribute
                    , rel2.IsCustomRelationship.ToString()
                    , rel2.IsManaged.ToString()
                    );

                imageBuilder.AddComponentSolution2((int)ComponentType.EntityRelationship, rel2.MetadataId.Value);
            }

            foreach (OneToManyRelationshipMetadata rel1 in listRel1.OrderBy(s => s.SchemaName))
            {
                if (_notExising.Contains(rel1.ReferencedEntity) || _notExising.Contains(rel1.ReferencingEntity))
                {
                    continue;
                }

                {
                    OneToManyRelationshipMetadata rel2 = listRel2.FirstOrDefault(s => string.Equals(s.SchemaName, rel1.SchemaName, StringComparison.InvariantCultureIgnoreCase));

                    if (rel2 != null)
                    {
                        continue;
                    }
                }

                listRelOnlyIn1.AddLine(
                    rel1.SchemaName
                    , rel1.ReferencingEntity
                    , rel1.ReferencingAttribute
                    , rel1.ReferencedEntity
                    , rel1.ReferencedAttribute
                    , rel1.IsCustomRelationship.ToString()
                    , rel1.IsManaged.ToString()
                    );

                imageBuilder.AddComponentSolution1((int)ComponentType.EntityRelationship, rel1.MetadataId.Value);
            }

            foreach (OneToManyRelationshipMetadata rel1 in listRel1.OrderBy(s => s.SchemaName))
            {
                OneToManyRelationshipMetadata rel2 = listRel2.FirstOrDefault(s => string.Equals(s.SchemaName, rel1.SchemaName, StringComparison.InvariantCultureIgnoreCase));

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

                    imageBuilder.AddComponentDifferent((int)ComponentType.EntityRelationship, rel1.MetadataId.Value, rel2.MetadataId.Value, string.Join(Environment.NewLine, strDifference));
                }
            }

            if (listRelOnlyIn1.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("{0} - {1} ONLY EXISTS in {2}: {3}", className, relationTypeName, _connectionName1, listRelOnlyIn1.Count));

                listRelOnlyIn1.GetFormatedLines(false).ForEach(s => strDifference.Add(_tabSpacer + s));
            }

            if (listRelOnlyIn2.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("{0} - {1} ONLY EXISTS in {2}: {3}", className, relationTypeName, _connectionName2, listRelOnlyIn2.Count));

                listRelOnlyIn2.GetFormatedLines(false).ForEach(s => strDifference.Add(_tabSpacer + s));
            }

            if (dictDifference.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("{0} - {1} DIFFERENT in {2} and {3}: {4}", className, relationTypeName, _connectionName1, _connectionName2, dictDifference.Count));

                foreach (KeyValuePair<string, List<string>> item in dictDifference.OrderBy(e => e.Key))
                {
                    if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                    strDifference.Add(string.Format("Different {0} - {1} {2}", className, relationTypeName, item.Key));

                    item.Value.ForEach(s => strDifference.Add(_tabSpacer + s));
                }
            }
        }

        private List<string> GetDifferenceOneToMany(OneToManyRelationshipMetadata rel1, OneToManyRelationshipMetadata rel2)
        {
            FormatTextTableHandler table = new FormatTextTableHandler(true);
            table.SetHeader("Property", _connectionName1, _connectionName2);

            //public bool? IsHierarchical { get; set; }
            //public string ReferencedAttribute { get; set; }
            //public string ReferencedEntity { get; set; }
            //public string ReferencedEntityNavigationPropertyName { get; set; }
            //public string ReferencingAttribute { get; set; }
            //public string ReferencingEntity { get; set; }
            //public string ReferencingEntityNavigationPropertyName { get; set; }

            //public string IntroducedVersion { get; }
            //public BooleanManagedProperty IsCustomizable { get; set; }
            //public bool? IsCustomRelationship { get; set; }
            //public bool? IsManaged { get; }
            //public bool? IsValidForAdvancedFind { get; set; }
            //public RelationshipType RelationshipType { get; }
            //public string SchemaName { get; set; }
            //public SecurityTypes? SecurityTypes { get; set; }


            table.AddLineIfNotEqual("IsHierarchical", rel1.IsHierarchical, rel2.IsHierarchical);

            table.AddLineIfNotEqual("ReferencedEntity", rel1.ReferencedEntity, rel2.ReferencedEntity);
            table.AddLineIfNotEqual("ReferencedAttribute", rel1.ReferencedAttribute, rel2.ReferencedAttribute);
            table.AddLineIfNotEqual("ReferencedEntityNavigationPropertyName", rel1.ReferencedEntityNavigationPropertyName, rel2.ReferencedEntityNavigationPropertyName);

            table.AddLineIfNotEqual("ReferencingEntity", rel1.ReferencingEntity, rel2.ReferencingEntity);
            table.AddLineIfNotEqual("ReferencingAttribute", rel1.ReferencingAttribute, rel2.ReferencingAttribute);
            table.AddLineIfNotEqual("ReferencingEntityNavigationPropertyName", rel1.ReferencingEntityNavigationPropertyName, rel2.ReferencingEntityNavigationPropertyName);

            table.AddLineIfNotEqual("IsCustomizable", rel1.IsCustomizable, rel2.IsCustomizable);
            //table.AddLineIfNotEqual("IsManaged", rel1.IsManaged, rel2.IsManaged);

            table.AddLineIfNotEqual("IsCustomRelationship", rel1.IsCustomRelationship, rel2.IsCustomRelationship);
            table.AddLineIfNotEqual("IsValidForAdvancedFind", rel1.IsValidForAdvancedFind, rel2.IsValidForAdvancedFind);

            table.AddLineIfNotEqual("IntroducedVersion", rel1.IntroducedVersion, rel2.IntroducedVersion);

            table.AddLineIfNotEqual("RelationshipType", rel1.RelationshipType, rel2.RelationshipType);
            table.AddLineIfNotEqual("SecurityTypes", rel1.SecurityTypes, rel2.SecurityTypes);

            AddAssociatedMenuConfigurationDifference(table, "AssociatedMenuConfiguration", rel1.AssociatedMenuConfiguration, rel2.AssociatedMenuConfiguration);

            AddCascadeConfigurationDifference(table, rel1.CascadeConfiguration, rel2.CascadeConfiguration);

            List<string> result = new List<string>();

            if (table.Count > 0)
            {
                result.AddRange(table.GetFormatedLines(false));
            }

            return result;
        }

        private void AddCascadeConfigurationDifference(FormatTextTableHandler table, CascadeConfiguration config1, CascadeConfiguration config2)
        {
            if (config1 != null && config2 == null)
            {
                table.AddLineIfNotEqual("CascadeConfiguration", "not null", "null");
            }
            else if (config1 == null && config2 != null)
            {
                table.AddLineIfNotEqual("CascadeConfiguration", "null", "not null");
            }
            else if (config1 != null && config2 != null)
            {
                table.AddLineIfNotEqual("CascadeConfiguration.Assign", config1.Assign, config2.Assign);
                table.AddLineIfNotEqual("CascadeConfiguration.Delete", config1.Delete, config2.Delete);
                table.AddLineIfNotEqual("CascadeConfiguration.Merge", config1.Merge, config2.Merge);
                table.AddLineIfNotEqual("CascadeConfiguration.Reparent", config1.Reparent, config2.Reparent);
                table.AddLineIfNotEqual("CascadeConfiguration.Share", config1.Share, config2.Share);
                table.AddLineIfNotEqual("CascadeConfiguration.Unshare", config1.Unshare, config2.Unshare);
            }
        }

        private void CompareManyToMany(OrganizationDifferenceImageBuilder imageBuilder, List<string> strDifference, string entityName, IEnumerable<ManyToManyRelationshipMetadata> listRel1, IEnumerable<ManyToManyRelationshipMetadata> listRel2)
        {
            FormatTextTableHandler listRelOnlyIn1 = new FormatTextTableHandler(true);
            FormatTextTableHandler listRelOnlyIn2 = new FormatTextTableHandler(true);

            listRelOnlyIn1.SetHeader("SchemaName", "IntersectEntityName", "Entity1LogicalName", "Entity1IntersectAttribute", "Entity2LogicalName", "Entity2IntersectAttribute", "IsCustomRelationship", "IsManaged");
            listRelOnlyIn2.SetHeader("SchemaName", "IntersectEntityName", "Entity1LogicalName", "Entity1IntersectAttribute", "Entity2LogicalName", "Entity2IntersectAttribute", "IsCustomRelationship", "IsManaged");

            Dictionary<string, List<string>> dictDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (ManyToManyRelationshipMetadata rel2 in listRel2.OrderBy(s => s.SchemaName))
            {
                if (_notExising.Contains(rel2.Entity1LogicalName) || _notExising.Contains(rel2.Entity2LogicalName))
                {
                    continue;
                }

                {
                    ManyToManyRelationshipMetadata rel1 = listRel1.FirstOrDefault(s => string.Equals(s.SchemaName, rel2.SchemaName, StringComparison.InvariantCultureIgnoreCase));

                    if (rel1 != null)
                    {
                        continue;
                    }
                }

                listRelOnlyIn2.AddLine(
                    rel2.SchemaName
                    , rel2.IntersectEntityName
                    , rel2.Entity1LogicalName
                    , rel2.Entity1IntersectAttribute
                    , rel2.Entity2LogicalName
                    , rel2.Entity2IntersectAttribute
                    , rel2.IsCustomRelationship.ToString()
                    , rel2.IsManaged.ToString()
                    );

                imageBuilder.AddComponentSolution2((int)ComponentType.EntityRelationship, rel2.MetadataId.Value);
            }

            foreach (ManyToManyRelationshipMetadata rel1 in listRel1.OrderBy(s => s.SchemaName))
            {
                if (_notExising.Contains(rel1.Entity1LogicalName) || _notExising.Contains(rel1.Entity2LogicalName))
                {
                    continue;
                }

                {
                    ManyToManyRelationshipMetadata rel2 = listRel2.FirstOrDefault(s => string.Equals(s.SchemaName, rel1.SchemaName, StringComparison.InvariantCultureIgnoreCase));

                    if (rel2 != null)
                    {
                        continue;
                    }
                }

                listRelOnlyIn1.AddLine(
                    rel1.SchemaName
                    , rel1.IntersectEntityName
                    , rel1.Entity1LogicalName
                    , rel1.Entity1IntersectAttribute
                    , rel1.Entity2LogicalName
                    , rel1.Entity2IntersectAttribute
                    , rel1.IsCustomRelationship.ToString()
                    , rel1.IsManaged.ToString()
                    );

                imageBuilder.AddComponentSolution1((int)ComponentType.EntityRelationship, rel1.MetadataId.Value);             
            }

            foreach (ManyToManyRelationshipMetadata rel1 in listRel1.OrderBy(s => s.SchemaName))
            {
                ManyToManyRelationshipMetadata rel2 = listRel2.FirstOrDefault(s => string.Equals(s.SchemaName, rel1.SchemaName, StringComparison.InvariantCultureIgnoreCase));

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

                    imageBuilder.AddComponentDifferent((int)ComponentType.EntityRelationship, rel1.MetadataId.Value, rel2.MetadataId.Value, string.Join(Environment.NewLine, strDifference));
                }
            }

            if (listRelOnlyIn1.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("ManyToMany ONLY EXISTS in {0}: {1}", _connectionName1, listRelOnlyIn1.Count));

                listRelOnlyIn1.GetFormatedLines(false).ForEach(s => strDifference.Add(_tabSpacer + s));
            }

            if (listRelOnlyIn2.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("ManyToMany ONLY EXISTS in {0}: {1}", _connectionName2, listRelOnlyIn2.Count));

                listRelOnlyIn2.GetFormatedLines(false).ForEach(s => strDifference.Add(_tabSpacer + s));
            }

            if (dictDifference.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("ManyToMany DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, dictDifference.Count));

                foreach (KeyValuePair<string, List<string>> item in dictDifference.OrderBy(e => e.Key))
                {
                    if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                    strDifference.Add(string.Format("Different ManyToMany {0}", item.Key));

                    item.Value.ForEach(s => strDifference.Add(_tabSpacer + s));
                }
            }
        }

        private List<string> GetDifferenceManyToMany(ManyToManyRelationshipMetadata rel1, ManyToManyRelationshipMetadata rel2)
        {
            FormatTextTableHandler table = new FormatTextTableHandler(true);
            table.SetHeader("Property", _connectionName1, _connectionName2);

            //public AssociatedMenuConfiguration Entity1AssociatedMenuConfiguration { get; set; }
            //public string Entity1IntersectAttribute { get; set; }
            //public string Entity1LogicalName { get; set; }
            //public string Entity1NavigationPropertyName { get; set; }
            //public AssociatedMenuConfiguration Entity2AssociatedMenuConfiguration { get; set; }
            //public string Entity2IntersectAttribute { get; set; }
            //public string Entity2LogicalName { get; set; }
            //public string Entity2NavigationPropertyName { get; set; }
            //public string IntersectEntityName { get; set; }

            //public string IntroducedVersion { get; }
            //public BooleanManagedProperty IsCustomizable { get; set; }
            //public bool? IsCustomRelationship { get; set; }
            //public bool? IsManaged { get; }
            //public bool? IsValidForAdvancedFind { get; set; }
            //public RelationshipType RelationshipType { get; }
            //public string SchemaName { get; set; }
            //public SecurityTypes? SecurityTypes { get; set; }

            table.AddLineIfNotEqual("IntersectEntityName", rel1.IntersectEntityName, rel2.IntersectEntityName);

            table.AddLineIfNotEqual("Entity1LogicalName", rel1.Entity1LogicalName, rel2.Entity1LogicalName);
            table.AddLineIfNotEqual("Entity1IntersectAttribute", rel1.Entity1IntersectAttribute, rel2.Entity1IntersectAttribute);
            table.AddLineIfNotEqual("Entity1NavigationPropertyName", rel1.Entity1NavigationPropertyName, rel2.Entity1NavigationPropertyName);

            table.AddLineIfNotEqual("Entity2LogicalName", rel1.Entity2LogicalName, rel2.Entity2LogicalName);
            table.AddLineIfNotEqual("Entity2IntersectAttribute", rel1.Entity2IntersectAttribute, rel2.Entity2IntersectAttribute);
            table.AddLineIfNotEqual("Entity2NavigationPropertyName", rel1.Entity2NavigationPropertyName, rel2.Entity2NavigationPropertyName);

            table.AddLineIfNotEqual("IsCustomizable", rel1.IsCustomizable, rel2.IsCustomizable);
            //table.AddLineIfNotEqual("IsManaged", rel1.IsManaged, rel2.IsManaged);

            table.AddLineIfNotEqual("IsCustomRelationship", rel1.IsCustomRelationship, rel2.IsCustomRelationship);
            table.AddLineIfNotEqual("IsValidForAdvancedFind", rel1.IsValidForAdvancedFind, rel2.IsValidForAdvancedFind);

            table.AddLineIfNotEqual("IntroducedVersion", rel1.IntroducedVersion, rel2.IntroducedVersion);

            table.AddLineIfNotEqual("RelationshipType", rel1.RelationshipType, rel2.RelationshipType);
            table.AddLineIfNotEqual("SecurityTypes", rel1.SecurityTypes, rel2.SecurityTypes);

            //AddLine(table, "IsCustomizable", key1.IsCustomizable, key2.IsCustomizable);
            //AddEntityMetadataString(table, "IsManaged", relationship.IsManaged);

            AddAssociatedMenuConfigurationDifference(table, "Entity1AssociatedMenuConfiguration", rel1.Entity1AssociatedMenuConfiguration, rel2.Entity1AssociatedMenuConfiguration);
            AddAssociatedMenuConfigurationDifference(table, "Entity2AssociatedMenuConfiguration", rel1.Entity2AssociatedMenuConfiguration, rel2.Entity2AssociatedMenuConfiguration);

            //"SchemaName", "IntersectEntityName", "Entity1LogicalName", "Entity1IntersectAttribute", "Entity2LogicalName", "Entity2IntersectAttribute"

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
                table.AddLineIfNotEqual(name + ".Behavior", config1.Behavior, config2.Behavior);
                table.AddLineIfNotEqual(name + ".Group", config1.Group, config2.Group);
                table.AddLineIfNotEqual(name + ".Order", config1.Order, config2.Order);

                if (config1.Label != null && config2.Label == null)
                {
                    table.AddLineIfNotEqual(name + ".Label", "not null", "null");
                }
                else if (config1.Label == null && config2.Label != null)
                {
                    table.AddLineIfNotEqual(name + ".Label", "null", "not null");
                }
            }
        }

        private void CompareKeys(OrganizationDifferenceImageBuilder imageBuilder, List<string> strDifference, string entityName, IEnumerable<EntityKeyMetadata> keys1, IEnumerable<EntityKeyMetadata> keys2)
        {
            FormatTextTableHandler listKeysOnlyIn1 = new FormatTextTableHandler(true);
            FormatTextTableHandler listKeysOnlyIn2 = new FormatTextTableHandler(true);

            listKeysOnlyIn1.SetHeader("LogicalName", "SchemaName", "IsManaged", "KeyAttributes");
            listKeysOnlyIn2.SetHeader("LogicalName", "SchemaName", "IsManaged", "KeyAttributes");

            Dictionary<string, List<string>> dictDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (EntityKeyMetadata key2 in keys2.OrderBy(s => s.LogicalName))
            {
                {
                    EntityKeyMetadata key1 = keys1.FirstOrDefault(s => string.Equals(s.LogicalName, key2.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                    if (key1 != null)
                    {
                        continue;
                    }
                }

                listKeysOnlyIn2.AddLine(key2.LogicalName, key2.SchemaName, key2.IsManaged.ToString(), string.Join(",", key2.KeyAttributes.OrderBy(s => s)));

                imageBuilder.AddComponentSolution2((int)ComponentType.EntityKey, key2.MetadataId.Value);
            }

            foreach (EntityKeyMetadata key1 in keys1.OrderBy(s => s.LogicalName))
            {
                {
                    EntityKeyMetadata key2 = keys2.FirstOrDefault(s => string.Equals(s.LogicalName, key1.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                    if (key2 != null)
                    {
                        continue;
                    }
                }

                listKeysOnlyIn1.AddLine(key1.LogicalName, key1.SchemaName, key1.IsManaged.ToString(), string.Join(",", key1.KeyAttributes.OrderBy(s => s)));

                imageBuilder.AddComponentSolution1((int)ComponentType.EntityKey, key1.MetadataId.Value);
            }

            foreach (EntityKeyMetadata key1 in keys1.OrderBy(s => s.LogicalName))
            {
                EntityKeyMetadata key2 = keys2.FirstOrDefault(s => string.Equals(s.LogicalName, key1.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                if (key2 == null)
                {
                    continue;
                }

                List<string> diff = GetDifferenceKey(key1, key2);

                if (diff.Count > 0)
                {
                    dictDifference.Add(key1.LogicalName, diff.Select(s => _tabSpacer + s).ToList());

                    imageBuilder.AddComponentDifferent((int)ComponentType.EntityKey, key1.MetadataId.Value, key2.MetadataId.Value, string.Join(Environment.NewLine, strDifference));
                }
            }

            if (listKeysOnlyIn1.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("Keys ONLY EXISTS in {0}: {1}", _connectionName1, listKeysOnlyIn1.Count));

                listKeysOnlyIn1.GetFormatedLines(false).ForEach(s => strDifference.Add(_tabSpacer + s));
            }

            if (listKeysOnlyIn2.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("Keys ONLY EXISTS in {0}: {1}", _connectionName2, listKeysOnlyIn2.Count));

                listKeysOnlyIn2.GetFormatedLines(false).ForEach(s => strDifference.Add(_tabSpacer + s));
            }

            if (dictDifference.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("Keys DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, dictDifference.Count));

                foreach (KeyValuePair<string, List<string>> item in dictDifference.OrderBy(e => e.Key))
                {
                    if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                    strDifference.Add(string.Format("Different Key {0}", item.Key));

                    strDifference.AddRange(item.Value);
                }
            }
        }

        private List<string> GetDifferenceKey(EntityKeyMetadata key1, EntityKeyMetadata key2)
        {
            List<string> strDifference = new List<string>();

            {
                FormatTextTableHandler table = new FormatTextTableHandler(true);
                table.SetHeader("Property", _connectionName1, _connectionName2);

                //EntityReference AsyncJob { get; }
                //Label DisplayName { get; set; }
                //EntityKeyIndexStatus EntityKeyIndexStatus { get; }
                //string EntityLogicalName { get; }
                //string IntroducedVersion { get; }
                //BooleanManagedProperty IsCustomizable { get; }
                //bool? IsManaged { get; }
                //string[] KeyAttributes { get; set; }
                //string LogicalName { get; set; }
                //string SchemaName { get; set; }

                table.AddLineIfNotEqual("SchemaName", key1.SchemaName, key2.SchemaName);
                table.AddLineIfNotEqual("IsCustomizable", key1.IsCustomizable, key2.IsCustomizable);
                table.AddLineIfNotEqual("IntroducedVersion", key1.IntroducedVersion, key2.IntroducedVersion);
                table.AddLineIfNotEqual("EntityLogicalName", key1.EntityLogicalName, key2.EntityLogicalName);
                table.AddLineIfNotEqual("EntityKeyIndexStatus", key1.EntityKeyIndexStatus, key2.EntityKeyIndexStatus);
                table.AddLineIfNotEqual("KeyAttributes", string.Join(",", key1.KeyAttributes.OrderBy(s => s)), string.Join(",", key2.KeyAttributes.OrderBy(s => s)));

                if (table.Count > 0)
                {
                    strDifference.AddRange(table.GetFormatedLines(false));
                }
            }

            return strDifference;
        }

        private async Task CompareAttributesAsync(OrganizationDifferenceImageBuilder imageBuilder, List<string> strDifference, string entityName, IEnumerable<AttributeMetadata> attributes1, IEnumerable<AttributeMetadata> attributes2)
        {
            FormatTextTableHandler listAttributesOnlyIn1 = new FormatTextTableHandler(true);
            FormatTextTableHandler listAttributesOnlyIn2 = new FormatTextTableHandler(true);

            listAttributesOnlyIn1.SetHeader("LogicalName", "TypeName", "AttributeType", "IsCustomAttribute", "IsManaged", "Target");
            listAttributesOnlyIn2.SetHeader("LogicalName", "TypeName", "AttributeType", "IsCustomAttribute", "IsManaged", "Target");

            Dictionary<string, List<string>> dictDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (AttributeMetadata attr2 in attributes2.OrderBy(s => s.LogicalName))
            {
                {
                    AttributeMetadata attr1 = attributes1.FirstOrDefault(s => string.Equals(s.LogicalName, attr2.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                    if (attr1 != null)
                    {
                        continue;
                    }
                }

                List<string> listStr = new List<string>() { attr2.LogicalName, attr2.GetType().Name, attr2.AttributeType.ToString(), attr2.IsManaged.ToString(), attr2.IsCustomAttribute.ToString() };

                if (attr2 is LookupAttributeMetadata)
                {
                    listStr.Add(string.Join(",", (attr2 as LookupAttributeMetadata)?.Targets?.OrderBy(s => s)));
                }

                listAttributesOnlyIn1.CalculateLineLengths(listStr.ToArray());
                listAttributesOnlyIn2.AddLine(listStr.ToArray());

                imageBuilder.AddComponentSolution2((int)ComponentType.Attribute, attr2.MetadataId.Value);
            }

            foreach (AttributeMetadata attr1 in attributes1.OrderBy(s => s.LogicalName))
            {
                {
                    AttributeMetadata attr2 = attributes2.FirstOrDefault(s => string.Equals(s.LogicalName, attr1.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                    if (attr2 != null)
                    {
                        continue;
                    }
                }

                List<string> listStr = new List<string>() { attr1.LogicalName, attr1.GetType().Name, attr1.AttributeType.ToString(), attr1.IsManaged.ToString(), attr1.IsCustomAttribute.ToString() };

                if (attr1 is LookupAttributeMetadata)
                {
                    listStr.Add(string.Join(",", (attr1 as LookupAttributeMetadata)?.Targets?.OrderBy(s => s)));
                }

                listAttributesOnlyIn1.AddLine(listStr.ToArray());
                listAttributesOnlyIn2.CalculateLineLengths(listStr.ToArray());

                imageBuilder.AddComponentSolution1((int)ComponentType.Attribute, attr1.MetadataId.Value);
            }

            foreach (AttributeMetadata attr1 in attributes1.OrderBy(s => s.LogicalName))
            {
                AttributeMetadata attr2 = attributes2.FirstOrDefault(s => string.Equals(s.LogicalName, attr1.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                if (attr2 == null)
                {
                    continue;
                }

                List<string> diff = await GetDifferenceAttribute(attr1, attr2);

                if (diff.Count > 0)
                {
                    dictDifference.Add(attr1.LogicalName, diff);

                    imageBuilder.AddComponentDifferent((int)ComponentType.Attribute, attr1.MetadataId.Value, attr2.MetadataId.Value, string.Join(Environment.NewLine, strDifference));
                }
            }

            if (listAttributesOnlyIn1.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("Attributes ONLY EXISTS in {0}: {1}", _connectionName1, listAttributesOnlyIn1.Count));

                listAttributesOnlyIn1.GetFormatedLines(false).ForEach(s => strDifference.Add(_tabSpacer + s));
            }

            if (listAttributesOnlyIn2.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("Attributes ONLY EXISTS in {0}: {1}", _connectionName2, listAttributesOnlyIn2.Count));

                listAttributesOnlyIn2.GetFormatedLines(false).ForEach(s => strDifference.Add(_tabSpacer + s));
            }

            if (dictDifference.Count > 0)
            {
                if (strDifference.Count > 0) { strDifference.Add(string.Empty); }

                strDifference.Add(string.Format("Attributes DIFFERENT in {0} and {1}: {2}", _connectionName1, _connectionName2, dictDifference.Count));

                foreach (KeyValuePair<string, List<string>> item in dictDifference.OrderBy(e => e.Key))
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
                table.SetHeader("Property", _connectionName1, _connectionName2);

                //public string AttributeOf { get; internal set; }
                //public AttributeTypeCode? AttributeType { get; internal set; }
                //public AttributeTypeDisplayName AttributeTypeName { get; internal set; }
                //public bool? CanBeSecuredForCreate { get; internal set; }
                //public bool? CanBeSecuredForRead { get; internal set; }
                //public bool? CanBeSecuredForUpdate { get; internal set; }
                //public BooleanManagedProperty CanModifyAdditionalSettings { get; set; }
                //public int? ColumnNumber { get; internal set; }
                //public string DeprecatedVersion { get; internal set; }
                //public string InheritsFrom { get; internal set; }
                //public string IntroducedVersion { get; internal set; }
                //public BooleanManagedProperty IsAuditEnabled { get; set; }
                //public bool? IsCustomAttribute { get; internal set; }
                //public BooleanManagedProperty IsCustomizable { get; set; }
                //public bool? IsFilterable { get; internal set; }
                //public BooleanManagedProperty IsGlobalFilterEnabled { get; set; }
                //public bool? IsLogical { get; internal set; }
                //public bool? IsManaged { get; internal set; }
                //public bool? IsPrimaryId { get; internal set; }
                //public bool? IsPrimaryName { get; internal set; }
                //public BooleanManagedProperty IsRenameable { get; set; }
                //public bool? IsRetrievable { get; internal set; }
                //public bool? IsSearchable { get; internal set; }
                //public bool? IsSecured { get; set; }
                //public BooleanManagedProperty IsSortableEnabled { get; set; }
                //public BooleanManagedProperty IsValidForAdvancedFind { get; set; }
                //public bool? IsValidForCreate { get; internal set; }
                //public bool? IsValidForRead { get; internal set; }
                //public bool? IsValidForUpdate { get; internal set; }
                //public AttributeRequiredLevelManagedProperty RequiredLevel { get; set; }
                //public string SchemaName { get; set; }
                //public int? SourceType { get; set; }

                table.AddLineIfNotEqual("AttributeOf", attr1.AttributeOf, attr2.AttributeOf);
                table.AddLineIfNotEqual("AttributeType", attr1.AttributeType, attr2.AttributeType);

                table.AddLineIfNotEqual("AttributeTypeName"
                    , attr1.AttributeTypeName != null ? attr1.AttributeTypeName.Value : "null"
                    , attr2.AttributeTypeName != null ? attr2.AttributeTypeName.Value : "null"
                    );
                table.AddLineIfNotEqual("CanBeSecuredForCreate", attr1.CanBeSecuredForCreate, attr2.CanBeSecuredForCreate);
                table.AddLineIfNotEqual("CanBeSecuredForRead", attr1.CanBeSecuredForRead, attr2.CanBeSecuredForRead);
                table.AddLineIfNotEqual("CanBeSecuredForUpdate", attr1.CanBeSecuredForUpdate, attr2.CanBeSecuredForUpdate);
                table.AddLineIfNotEqual("CanModifyAdditionalSettings", attr1.CanModifyAdditionalSettings, attr2.CanModifyAdditionalSettings);
                //EntityValuesComparer.AddLineIfNotEqual("ColumnNumber", attr1.ColumnNumber, attr2.ColumnNumber);
                table.AddLineIfNotEqual("InheritsFrom", attr1.InheritsFrom, attr2.InheritsFrom);
                table.AddLineIfNotEqual("IsAuditEnabled", attr1.IsAuditEnabled, attr2.IsAuditEnabled);
                table.AddLineIfNotEqual("IsCustomAttribute", attr1.IsCustomAttribute, attr2.IsCustomAttribute);
                table.AddLineIfNotEqual("IsCustomizable", attr1.IsCustomizable, attr2.IsCustomizable);
                table.AddLineIfNotEqual("IsFilterable", attr1.IsFilterable, attr2.IsFilterable);
                table.AddLineIfNotEqual("IsGlobalFilterEnabled", attr1.IsGlobalFilterEnabled, attr2.IsGlobalFilterEnabled);
                table.AddLineIfNotEqual("IsLogical", attr1.IsLogical, attr2.IsLogical);
                //table.AddLineIfNotEqual("IsManaged", attr1.IsManaged, attr2.IsManaged);
                table.AddLineIfNotEqual("IsPrimaryId", attr1.IsPrimaryId, attr2.IsPrimaryId);
                table.AddLineIfNotEqual("IsPrimaryName", attr1.IsPrimaryName, attr2.IsPrimaryName);
                table.AddLineIfNotEqual("IsRenameable", attr1.IsRenameable, attr2.IsRenameable);
                table.AddLineIfNotEqual("IsRetrievable", attr1.IsRetrievable, attr2.IsRetrievable);
                table.AddLineIfNotEqual("IsSearchable", attr1.IsSearchable, attr2.IsSearchable);
                table.AddLineIfNotEqual("IsSecured", attr1.IsSecured, attr2.IsSecured);
                table.AddLineIfNotEqual("IsSortableEnabled", attr1.IsSortableEnabled, attr2.IsSortableEnabled);
                table.AddLineIfNotEqual("IsValidForAdvancedFind", attr1.IsValidForAdvancedFind, attr2.IsValidForAdvancedFind);
                table.AddLineIfNotEqual("IsValidForCreate", attr1.IsValidForCreate, attr2.IsValidForCreate);
                table.AddLineIfNotEqual("IsValidForRead", attr1.IsValidForRead, attr2.IsValidForRead);
                table.AddLineIfNotEqual("IsValidForUpdate", attr1.IsValidForUpdate, attr2.IsValidForUpdate);

                table.AddLineIfNotEqual("RequiredLevel", attr1.RequiredLevel, attr2.RequiredLevel);

                table.AddLineIfNotEqual("SchemaName", attr1.SchemaName, attr2.SchemaName);
                table.AddLineIfNotEqual("SourceType", attr1.SourceType, attr2.SourceType);

                List<string> additionalDifference = new List<string>();

                if (attr1.GetType().Name != attr2.GetType().Name)
                {
                    table.AddLineIfNotEqual("Type", attr1.GetType().Name, attr2.GetType().Name);
                }
                else
                {
                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.MemoAttributeMetadata)
                    {
                        MemoAttributeMetadata memoAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.MemoAttributeMetadata;
                        MemoAttributeMetadata memoAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.MemoAttributeMetadata;

                        table.AddLineIfNotEqual("MaxLength", memoAttrib1.MaxLength, memoAttrib2.MaxLength);
                        table.AddLineIfNotEqual("Format", memoAttrib1.Format, memoAttrib2.Format);
                        table.AddLineIfNotEqual("ImeMode", memoAttrib1.ImeMode, memoAttrib2.ImeMode);
                        table.AddLineIfNotEqual("IsLocalizable", memoAttrib1.IsLocalizable, memoAttrib2.IsLocalizable);
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.StringAttributeMetadata)
                    {
                        StringAttributeMetadata stringAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.StringAttributeMetadata;
                        StringAttributeMetadata stringAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.StringAttributeMetadata;

                        table.AddLineIfNotEqual("MaxLength", stringAttrib1.MaxLength, stringAttrib2.MaxLength);
                        table.AddLineIfNotEqual("Format", stringAttrib1.Format, stringAttrib2.Format);
                        table.AddLineIfNotEqual("ImeMode", stringAttrib1.ImeMode, stringAttrib2.ImeMode);
                        table.AddLineIfNotEqual("IsLocalizable", stringAttrib1.IsLocalizable, stringAttrib2.IsLocalizable);

                        if (stringAttrib1.FormulaDefinition != stringAttrib2.FormulaDefinition)
                        {
                            table.AddLine("FormulaDefinition");
                        }
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.IntegerAttributeMetadata)
                    {
                        IntegerAttributeMetadata intAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.IntegerAttributeMetadata;
                        IntegerAttributeMetadata intAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.IntegerAttributeMetadata;

                        table.AddLineIfNotEqual("MinValue", intAttrib1.MinValue, intAttrib2.MinValue);
                        table.AddLineIfNotEqual("MaxValue", intAttrib1.MaxValue, intAttrib2.MaxValue);
                        table.AddLineIfNotEqual("Format", intAttrib1.Format, intAttrib2.Format);

                        if (intAttrib1.FormulaDefinition != intAttrib2.FormulaDefinition)
                        {
                            table.AddLine("FormulaDefinition");
                        }
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.BigIntAttributeMetadata)
                    {
                        BigIntAttributeMetadata bigIntAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.BigIntAttributeMetadata;
                        BigIntAttributeMetadata bigIntAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.BigIntAttributeMetadata;

                        table.AddLineIfNotEqual("MinValue", bigIntAttrib1.MinValue, bigIntAttrib2.MinValue);
                        table.AddLineIfNotEqual("MaxValue", bigIntAttrib1.MaxValue, bigIntAttrib2.MaxValue);
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.ImageAttributeMetadata)
                    {
                        ImageAttributeMetadata imageAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.ImageAttributeMetadata;
                        ImageAttributeMetadata imageAttrib2 = attr1 as Microsoft.Xrm.Sdk.Metadata.ImageAttributeMetadata;

                        table.AddLineIfNotEqual("IsPrimaryImage", imageAttrib1.IsPrimaryImage, imageAttrib2.IsPrimaryImage);
                        table.AddLineIfNotEqual("MaxHeight", imageAttrib1.MaxHeight, imageAttrib2.MaxHeight);
                        table.AddLineIfNotEqual("MaxWidth", imageAttrib1.MaxWidth, imageAttrib2.MaxWidth);
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.MoneyAttributeMetadata)
                    {
                        MoneyAttributeMetadata moneyAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.MoneyAttributeMetadata;
                        MoneyAttributeMetadata moneyAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.MoneyAttributeMetadata;

                        table.AddLineIfNotEqual("MinValue", moneyAttrib1.MinValue, moneyAttrib2.MinValue);
                        table.AddLineIfNotEqual("MaxValue", moneyAttrib1.MaxValue, moneyAttrib2.MaxValue);
                        table.AddLineIfNotEqual("Precision", moneyAttrib1.Precision, moneyAttrib2.Precision);
                        table.AddLineIfNotEqual("PrecisionSource", moneyAttrib1.PrecisionSource, moneyAttrib2.PrecisionSource);
                        table.AddLineIfNotEqual("IsBaseCurrency", moneyAttrib1.IsBaseCurrency, moneyAttrib2.IsBaseCurrency);
                        table.AddLineIfNotEqual("ImeMode", moneyAttrib1.ImeMode, moneyAttrib2.ImeMode);
                        table.AddLineIfNotEqual("CalculationOf", moneyAttrib1.CalculationOf, moneyAttrib2.CalculationOf);

                        if (moneyAttrib1.FormulaDefinition != moneyAttrib2.FormulaDefinition)
                        {
                            table.AddLine("FormulaDefinition");
                        }
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.DecimalAttributeMetadata)
                    {
                        DecimalAttributeMetadata decimalAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.DecimalAttributeMetadata;
                        DecimalAttributeMetadata decimalAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.DecimalAttributeMetadata;

                        table.AddLineIfNotEqual("MinValue", decimalAttrib1.MinValue, decimalAttrib2.MinValue);
                        table.AddLineIfNotEqual("MaxValue", decimalAttrib1.MaxValue, decimalAttrib2.MaxValue);
                        table.AddLineIfNotEqual("Precision", decimalAttrib1.Precision, decimalAttrib2.Precision);
                        table.AddLineIfNotEqual("ImeMode", decimalAttrib1.ImeMode, decimalAttrib2.ImeMode);

                        if (decimalAttrib1.FormulaDefinition != decimalAttrib2.FormulaDefinition)
                        {
                            table.AddLine("FormulaDefinition");
                        }
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.DoubleAttributeMetadata)
                    {
                        DoubleAttributeMetadata doubleAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.DoubleAttributeMetadata;
                        DoubleAttributeMetadata doubleAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.DoubleAttributeMetadata;

                        table.AddLineIfNotEqual("MinValue", doubleAttrib1.MinValue, doubleAttrib2.MinValue);
                        table.AddLineIfNotEqual("MaxValue", doubleAttrib1.MaxValue, doubleAttrib2.MaxValue);
                        table.AddLineIfNotEqual("Precision", doubleAttrib1.Precision, doubleAttrib2.Precision);
                        table.AddLineIfNotEqual("ImeMode", doubleAttrib1.ImeMode, doubleAttrib2.ImeMode);
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata)
                    {
                        BooleanAttributeMetadata boolAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata;
                        BooleanAttributeMetadata boolAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata;

                        table.AddLineIfNotEqual("FalseOption", boolAttrib1.OptionSet.FalseOption.Value, boolAttrib2.OptionSet.FalseOption.Value);
                        table.AddLineIfNotEqual("TrueOption", boolAttrib1.OptionSet.TrueOption.Value, boolAttrib2.OptionSet.TrueOption.Value);

                        if (boolAttrib1.FormulaDefinition != boolAttrib2.FormulaDefinition)
                        {
                            table.AddLine("FormulaDefinition");
                        }

                        if (boolAttrib1.OptionSet != null && boolAttrib2.OptionSet == null)
                        {
                            table.AddLine("OptionSet", "not null", "null");
                        }
                        else if (boolAttrib1.OptionSet == null && boolAttrib2.OptionSet != null)
                        {
                            table.AddLine("OptionSet", "null", "not null");
                        }
                        else if (boolAttrib1.OptionSet != null && boolAttrib2.OptionSet != null)
                        {
                            if (!CreateFileHandler.IgnoreAttribute(boolAttrib1.EntityLogicalName, boolAttrib1.LogicalName))
                            {
                                List<string> diffenrenceOptionSet = await _optionSetComparer.GetDifference(boolAttrib1.OptionSet, boolAttrib2.OptionSet, attr1.EntityLogicalName, attr2.LogicalName);

                                if (diffenrenceOptionSet.Count > 0)
                                {
                                    additionalDifference.Add(string.Format("Difference in OptionSet {0} and {1}", boolAttrib1.OptionSet.Name, boolAttrib2.OptionSet.Name));
                                    diffenrenceOptionSet.ForEach(s => additionalDifference.Add(_tabSpacer + s));
                                }
                            }
                        }
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.PicklistAttributeMetadata)
                    {
                        PicklistAttributeMetadata picklistAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.PicklistAttributeMetadata;
                        PicklistAttributeMetadata picklistAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.PicklistAttributeMetadata;

                        table.AddLineIfNotEqual("DefaultFormValue", picklistAttrib1.DefaultFormValue, picklistAttrib2.DefaultFormValue);

                        if (picklistAttrib1.FormulaDefinition != picklistAttrib2.FormulaDefinition)
                        {
                            table.AddLine("FormulaDefinition");
                        }

                        if (picklistAttrib1.OptionSet != null && picklistAttrib2.OptionSet == null)
                        {
                            table.AddLine("OptionSet", "not null", "null");
                        }
                        else if (picklistAttrib1.OptionSet == null && picklistAttrib2.OptionSet != null)
                        {
                            table.AddLine("OptionSet", "null", "not null");
                        }
                        else if (picklistAttrib1.OptionSet != null && picklistAttrib2.OptionSet != null)
                        {
                            if (!CreateFileHandler.IgnoreAttribute(picklistAttrib1.EntityLogicalName, picklistAttrib1.LogicalName))
                            {
                                List<string> diffenrenceOptionSet = await _optionSetComparer.GetDifference(picklistAttrib1.OptionSet, picklistAttrib2.OptionSet, picklistAttrib1.EntityLogicalName, picklistAttrib1.LogicalName);

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

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.StatusAttributeMetadata)
                    {
                        StatusAttributeMetadata statusAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.StatusAttributeMetadata;
                        StatusAttributeMetadata statusAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.StatusAttributeMetadata;

                        table.AddLineIfNotEqual("DefaultFormValue", statusAttrib1.DefaultFormValue, statusAttrib2.DefaultFormValue);
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.StateAttributeMetadata)
                    {
                        StateAttributeMetadata stateAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.StateAttributeMetadata;
                        StateAttributeMetadata stateAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.StateAttributeMetadata;

                        table.AddLineIfNotEqual("DefaultFormValue", stateAttrib1.DefaultFormValue, stateAttrib2.DefaultFormValue);
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.EntityNameAttributeMetadata)
                    {
                        EntityNameAttributeMetadata entityNameAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.EntityNameAttributeMetadata;
                        EntityNameAttributeMetadata entityNameAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.EntityNameAttributeMetadata;

                        table.AddLineIfNotEqual("DefaultFormValue", entityNameAttrib1.DefaultFormValue, entityNameAttrib2.DefaultFormValue);

                        //if (entityNameAttrib1.OptionSet != null && entityNameAttrib2.OptionSet != null)
                        //{

                        //}
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.LookupAttributeMetadata)
                    {
                        LookupAttributeMetadata lookupAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.LookupAttributeMetadata;
                        LookupAttributeMetadata lookupAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.LookupAttributeMetadata;

                        string targets1 = string.Join(",", lookupAttrib1.Targets.OrderBy(s => s));
                        string targets2 = string.Join(",", lookupAttrib2.Targets.OrderBy(s => s));

                        if (targets1 != targets2)
                        {
                            string t1 = string.Join(",", lookupAttrib1.Targets.Except(lookupAttrib2.Targets).OrderBy(s => s));
                            string t2 = string.Join(",", lookupAttrib2.Targets.Except(lookupAttrib1.Targets).OrderBy(s => s));

                            table.AddLine("Targets", t1, t2);
                        }
                    }

                    if (attr1 is Microsoft.Xrm.Sdk.Metadata.DateTimeAttributeMetadata)
                    {
                        DateTimeAttributeMetadata dateTimeAttrib1 = attr1 as Microsoft.Xrm.Sdk.Metadata.DateTimeAttributeMetadata;
                        DateTimeAttributeMetadata dateTimeAttrib2 = attr2 as Microsoft.Xrm.Sdk.Metadata.DateTimeAttributeMetadata;

                        table.AddLineIfNotEqual("DateTimeBehavior"
                            , dateTimeAttrib1.DateTimeBehavior != null ? dateTimeAttrib1.DateTimeBehavior.Value : "null"
                            , dateTimeAttrib2.DateTimeBehavior != null ? dateTimeAttrib2.DateTimeBehavior.Value : "null"
                            );

                        table.AddLineIfNotEqual("CanChangeDateTimeBehavior", dateTimeAttrib1.CanChangeDateTimeBehavior, dateTimeAttrib2.CanChangeDateTimeBehavior);
                        table.AddLineIfNotEqual("Format", dateTimeAttrib1.Format, dateTimeAttrib2.Format);
                        table.AddLineIfNotEqual("ImeMode", dateTimeAttrib1.ImeMode, dateTimeAttrib2.ImeMode);

                        if (dateTimeAttrib1.FormulaDefinition != dateTimeAttrib2.FormulaDefinition)
                        {
                            table.AddLine("FormulaDefinition");
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
            if (string.Equals(attributeName, "regardingobjectid", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            if (attributeName.Equals("objectid", StringComparison.InvariantCultureIgnoreCase))
            {
                if (entityName.Equals("queueitem", StringComparison.InvariantCultureIgnoreCase)
                    || entityName.Equals("principalobjectattributeaccess", StringComparison.InvariantCultureIgnoreCase)
                    || entityName.Equals("annotation", StringComparison.InvariantCultureIgnoreCase)
                    || entityName.Equals("userentityinstancedata", StringComparison.InvariantCultureIgnoreCase)

                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("duplicaterecord", StringComparison.InvariantCultureIgnoreCase))
            {
                if (attributeName.Equals("baserecordid", StringComparison.InvariantCultureIgnoreCase)
                    || attributeName.Equals("duplicaterecordid", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    return true;
                }
            }

            if (entityName.Equals("connection", StringComparison.InvariantCultureIgnoreCase))
            {
                if (attributeName.Equals("record1id", StringComparison.InvariantCultureIgnoreCase)
                    || attributeName.Equals("record2id", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    return true;
                }
            }

            return false;
        }
    }
}