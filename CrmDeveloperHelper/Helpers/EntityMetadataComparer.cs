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

        private HashSet<string> _notExisting;

        private OptionSetComparer _optionSetComparer;

        public EntityMetadataComparer(string tabSpacer, string connectionName1, string connectionName2, OptionSetComparer optionSetComparer, HashSet<string> notExisting)
        {
            _tabSpacer = tabSpacer;
            _connectionName1 = connectionName1;
            _connectionName2 = connectionName2;
            _notExisting = notExisting;

            _optionSetComparer = optionSetComparer;
        }

        public async Task<List<string>> GetDifferenceAsync(OrganizationDifferenceImageBuilder imageBuilder, EntityMetadata entityMetadata1, EntityMetadata entityMetadata2)
        {
            var strDifference = new List<string>();

            {
                var table = new FormatTextTableHandler(true);
                table.SetHeader("Property", _connectionName1, _connectionName2);

                table.AddLineIfNotEqual(nameof(EntityMetadata.ActivityTypeMask), entityMetadata1.ActivityTypeMask, entityMetadata2.ActivityTypeMask);
                table.AddLineIfNotEqual(nameof(EntityMetadata.AutoCreateAccessTeams), entityMetadata1.AutoCreateAccessTeams, entityMetadata2.AutoCreateAccessTeams);
                table.AddLineIfNotEqual(nameof(EntityMetadata.AutoRouteToOwnerQueue), entityMetadata1.AutoRouteToOwnerQueue, entityMetadata2.AutoRouteToOwnerQueue);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanBeInManyToMany), entityMetadata1.CanBeInManyToMany, entityMetadata2.CanBeInManyToMany);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanBePrimaryEntityInRelationship), entityMetadata1.CanBePrimaryEntityInRelationship, entityMetadata2.CanBePrimaryEntityInRelationship);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanBeRelatedEntityInRelationship), entityMetadata1.CanBeRelatedEntityInRelationship, entityMetadata2.CanBeRelatedEntityInRelationship);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanChangeHierarchicalRelationship), entityMetadata1.CanChangeHierarchicalRelationship, entityMetadata2.CanChangeHierarchicalRelationship);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanChangeTrackingBeEnabled), entityMetadata1.CanChangeTrackingBeEnabled, entityMetadata2.CanChangeTrackingBeEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanCreateAttributes), entityMetadata1.CanCreateAttributes, entityMetadata2.CanCreateAttributes);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanCreateCharts), entityMetadata1.CanCreateCharts, entityMetadata2.CanCreateCharts);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanCreateForms), entityMetadata1.CanCreateForms, entityMetadata2.CanCreateForms);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanCreateViews), entityMetadata1.CanCreateViews, entityMetadata2.CanCreateViews);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanEnableSyncToExternalSearchIndex), entityMetadata1.CanEnableSyncToExternalSearchIndex, entityMetadata2.CanEnableSyncToExternalSearchIndex);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanModifyAdditionalSettings), entityMetadata1.CanModifyAdditionalSettings, entityMetadata2.CanModifyAdditionalSettings);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CanTriggerWorkflow), entityMetadata1.CanTriggerWorkflow, entityMetadata2.CanTriggerWorkflow);
                table.AddLineIfNotEqual(nameof(EntityMetadata.ChangeTrackingEnabled), entityMetadata1.ChangeTrackingEnabled, entityMetadata2.ChangeTrackingEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.CollectionSchemaName), entityMetadata1.CollectionSchemaName, entityMetadata2.CollectionSchemaName);
                table.AddLineIfNotEqual(nameof(EntityMetadata.DaysSinceRecordLastModified), entityMetadata1.DaysSinceRecordLastModified, entityMetadata2.DaysSinceRecordLastModified);
                table.AddLineIfNotEqual(nameof(EntityMetadata.EnforceStateTransitions), entityMetadata1.EnforceStateTransitions, entityMetadata2.EnforceStateTransitions);
                table.AddLineIfNotEqual(nameof(EntityMetadata.EntityColor), entityMetadata1.EntityColor, entityMetadata2.EntityColor);
                table.AddLineIfNotEqual(nameof(EntityMetadata.EntityHelpUrl), entityMetadata1.EntityHelpUrl, entityMetadata2.EntityHelpUrl);
                table.AddLineIfNotEqual(nameof(EntityMetadata.EntityHelpUrlEnabled), entityMetadata1.EntityHelpUrlEnabled, entityMetadata2.EntityHelpUrlEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.EntitySetName), entityMetadata1.EntitySetName, entityMetadata2.EntitySetName);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IconLargeName), entityMetadata1.IconLargeName, entityMetadata2.IconLargeName);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IconMediumName), entityMetadata1.IconMediumName, entityMetadata2.IconMediumName);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IconSmallName), entityMetadata1.IconSmallName, entityMetadata2.IconSmallName);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsActivity), entityMetadata1.IsActivity, entityMetadata2.IsActivity);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsActivityParty), entityMetadata1.IsActivityParty, entityMetadata2.IsActivityParty);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsAIRUpdated), entityMetadata1.IsAIRUpdated, entityMetadata2.IsAIRUpdated);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsAuditEnabled), entityMetadata1.IsAuditEnabled, entityMetadata2.IsAuditEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsAvailableOffline), entityMetadata1.IsAvailableOffline, entityMetadata2.IsAvailableOffline);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsBusinessProcessEnabled), entityMetadata1.IsBusinessProcessEnabled, entityMetadata2.IsBusinessProcessEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsConnectionsEnabled), entityMetadata1.IsConnectionsEnabled, entityMetadata2.IsConnectionsEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsCustomEntity), entityMetadata1.IsCustomEntity, entityMetadata2.IsCustomEntity);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsCustomizable), entityMetadata1.IsCustomizable, entityMetadata2.IsCustomizable);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsDocumentManagementEnabled), entityMetadata1.IsDocumentManagementEnabled, entityMetadata2.IsDocumentManagementEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsDuplicateDetectionEnabled), entityMetadata1.IsDuplicateDetectionEnabled, entityMetadata2.IsDuplicateDetectionEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsEnabledForCharts), entityMetadata1.IsEnabledForCharts, entityMetadata2.IsEnabledForCharts);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsEnabledForExternalChannels), entityMetadata1.IsEnabledForExternalChannels, entityMetadata2.IsEnabledForExternalChannels);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsEnabledForTrace), entityMetadata1.IsEnabledForTrace, entityMetadata2.IsEnabledForTrace);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsImportable), entityMetadata1.IsImportable, entityMetadata2.IsImportable);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsInteractionCentricEnabled), entityMetadata1.IsInteractionCentricEnabled, entityMetadata2.IsInteractionCentricEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsIntersect), entityMetadata1.IsIntersect, entityMetadata2.IsIntersect);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsKnowledgeManagementEnabled), entityMetadata1.IsKnowledgeManagementEnabled, entityMetadata2.IsKnowledgeManagementEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsMailMergeEnabled), entityMetadata1.IsMailMergeEnabled, entityMetadata2.IsMailMergeEnabled);
                //table.AddLineIfNotEqual(nameof(EntityMetadata.IsManaged), entityMetadata1.IsManaged, entityMetadata2.IsManaged);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsMappable), entityMetadata1.IsMappable, entityMetadata2.IsMappable);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsOfflineInMobileClient), entityMetadata1.IsOfflineInMobileClient, entityMetadata2.IsOfflineInMobileClient);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsOneNoteIntegrationEnabled), entityMetadata1.IsOneNoteIntegrationEnabled, entityMetadata2.IsOneNoteIntegrationEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsOptimisticConcurrencyEnabled), entityMetadata1.IsOptimisticConcurrencyEnabled, entityMetadata2.IsOptimisticConcurrencyEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsPrivate), entityMetadata1.IsPrivate, entityMetadata2.IsPrivate);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsQuickCreateEnabled), entityMetadata1.IsQuickCreateEnabled, entityMetadata2.IsQuickCreateEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsReadingPaneEnabled), entityMetadata1.IsReadingPaneEnabled, entityMetadata2.IsReadingPaneEnabled);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsReadOnlyInMobileClient), entityMetadata1.IsReadOnlyInMobileClient, entityMetadata2.IsReadOnlyInMobileClient);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsRenameable), entityMetadata1.IsRenameable, entityMetadata2.IsRenameable);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsStateModelAware), entityMetadata1.IsStateModelAware, entityMetadata2.IsStateModelAware);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsValidForAdvancedFind), entityMetadata1.IsValidForAdvancedFind, entityMetadata2.IsValidForAdvancedFind);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsValidForQueue), entityMetadata1.IsValidForQueue, entityMetadata2.IsValidForQueue);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsVisibleInMobile), entityMetadata1.IsVisibleInMobile, entityMetadata2.IsVisibleInMobile);
                table.AddLineIfNotEqual(nameof(EntityMetadata.IsVisibleInMobileClient), entityMetadata1.IsVisibleInMobileClient, entityMetadata2.IsVisibleInMobileClient);
                table.AddLineIfNotEqual(nameof(EntityMetadata.HasChanged), entityMetadata1.HasChanged, entityMetadata2.HasChanged);
                table.AddLineIfNotEqual(nameof(EntityMetadata.LogicalCollectionName), entityMetadata1.LogicalCollectionName, entityMetadata2.LogicalCollectionName);
                table.AddLineIfNotEqual(nameof(EntityMetadata.LogicalName), entityMetadata1.LogicalName, entityMetadata2.LogicalName);
                table.AddLineIfNotEqual(nameof(EntityMetadata.OwnershipType), entityMetadata1.OwnershipType, entityMetadata2.OwnershipType);
                table.AddLineIfNotEqual(nameof(EntityMetadata.RecurrenceBaseEntityLogicalName), entityMetadata1.RecurrenceBaseEntityLogicalName, entityMetadata2.RecurrenceBaseEntityLogicalName);
                table.AddLineIfNotEqual(nameof(EntityMetadata.ReportViewName), entityMetadata1.ReportViewName, entityMetadata2.ReportViewName);
                table.AddLineIfNotEqual(nameof(EntityMetadata.SchemaName), entityMetadata1.SchemaName, entityMetadata2.SchemaName);
                table.AddLineIfNotEqual(nameof(EntityMetadata.SyncToExternalSearchIndex), entityMetadata1.SyncToExternalSearchIndex, entityMetadata2.SyncToExternalSearchIndex);

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
            var listRelOnlyIn1 = new FormatTextTableHandler(true);
            var listRelOnlyIn2 = new FormatTextTableHandler(true);

            listRelOnlyIn1.SetHeader(
                nameof(OneToManyRelationshipMetadata.SchemaName)
                , nameof(OneToManyRelationshipMetadata.ReferencingEntity)
                , nameof(OneToManyRelationshipMetadata.ReferencingAttribute)
                , nameof(OneToManyRelationshipMetadata.ReferencedEntity)
                , nameof(OneToManyRelationshipMetadata.ReferencedAttribute)
                , nameof(OneToManyRelationshipMetadata.IsCustomRelationship)
                , nameof(OneToManyRelationshipMetadata.IsManaged)
            );
            listRelOnlyIn2.SetHeader(
                nameof(OneToManyRelationshipMetadata.SchemaName)
                , nameof(OneToManyRelationshipMetadata.ReferencingEntity)
                , nameof(OneToManyRelationshipMetadata.ReferencingAttribute)
                , nameof(OneToManyRelationshipMetadata.ReferencedEntity)
                , nameof(OneToManyRelationshipMetadata.ReferencedAttribute)
                , nameof(OneToManyRelationshipMetadata.IsCustomRelationship)
                , nameof(OneToManyRelationshipMetadata.IsManaged)
            );

            Dictionary<string, List<string>> dictDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (OneToManyRelationshipMetadata rel2 in listRel2.OrderBy(s => s.SchemaName))
            {
                if (_notExisting.Contains(rel2.ReferencedEntity) || _notExisting.Contains(rel2.ReferencingEntity))
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
                if (_notExisting.Contains(rel1.ReferencedEntity) || _notExisting.Contains(rel1.ReferencingEntity))
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

                if (_notExisting.Contains(rel1.ReferencedEntity) || _notExisting.Contains(rel1.ReferencingEntity))
                {
                    continue;
                }

                if (_notExisting.Contains(rel2.ReferencedEntity) || _notExisting.Contains(rel2.ReferencingEntity))
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


            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.IsHierarchical), rel1.IsHierarchical, rel2.IsHierarchical);

            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.ReferencedEntity), rel1.ReferencedEntity, rel2.ReferencedEntity);
            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.ReferencedAttribute), rel1.ReferencedAttribute, rel2.ReferencedAttribute);
            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.ReferencedEntityNavigationPropertyName), rel1.ReferencedEntityNavigationPropertyName, rel2.ReferencedEntityNavigationPropertyName);

            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.ReferencingEntity), rel1.ReferencingEntity, rel2.ReferencingEntity);
            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.ReferencingAttribute), rel1.ReferencingAttribute, rel2.ReferencingAttribute);
            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.ReferencingEntityNavigationPropertyName), rel1.ReferencingEntityNavigationPropertyName, rel2.ReferencingEntityNavigationPropertyName);

            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.IsCustomizable), rel1.IsCustomizable, rel2.IsCustomizable);
            //table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.IsManaged), rel1.IsManaged, rel2.IsManaged);

            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.IsCustomRelationship), rel1.IsCustomRelationship, rel2.IsCustomRelationship);
            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.IsValidForAdvancedFind), rel1.IsValidForAdvancedFind, rel2.IsValidForAdvancedFind);

            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.IntroducedVersion), rel1.IntroducedVersion, rel2.IntroducedVersion);

            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.RelationshipType), rel1.RelationshipType, rel2.RelationshipType);
            table.AddLineIfNotEqual(nameof(OneToManyRelationshipMetadata.SecurityTypes), rel1.SecurityTypes, rel2.SecurityTypes);

            AddAssociatedMenuConfigurationDifference(table, nameof(OneToManyRelationshipMetadata.AssociatedMenuConfiguration), rel1.AssociatedMenuConfiguration, rel2.AssociatedMenuConfiguration);

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
                table.AddLineIfNotEqual(nameof(CascadeConfiguration), "not null", "null");
            }
            else if (config1 == null && config2 != null)
            {
                table.AddLineIfNotEqual(nameof(CascadeConfiguration), "null", "not null");
            }
            else if (config1 != null && config2 != null)
            {
                table.AddLineIfNotEqual($"{nameof(CascadeConfiguration)}.{nameof(CascadeConfiguration.Assign)}", config1.Assign, config2.Assign);
                table.AddLineIfNotEqual($"{nameof(CascadeConfiguration)}.{nameof(CascadeConfiguration.Delete)}", config1.Delete, config2.Delete);
                table.AddLineIfNotEqual($"{nameof(CascadeConfiguration)}.{nameof(CascadeConfiguration.Merge)}", config1.Merge, config2.Merge);
                table.AddLineIfNotEqual($"{nameof(CascadeConfiguration)}.{nameof(CascadeConfiguration.Reparent)}", config1.Reparent, config2.Reparent);
                table.AddLineIfNotEqual($"{nameof(CascadeConfiguration)}.{nameof(CascadeConfiguration.Share)}", config1.Share, config2.Share);
                table.AddLineIfNotEqual($"{nameof(CascadeConfiguration)}.{nameof(CascadeConfiguration.Unshare)}", config1.Unshare, config2.Unshare);
                table.AddLineIfNotEqual($"{nameof(CascadeConfiguration)}.{nameof(CascadeConfiguration.RollupView)}", config1.RollupView, config2.RollupView);
            }
        }

        private void CompareManyToMany(OrganizationDifferenceImageBuilder imageBuilder, List<string> strDifference, string entityName, IEnumerable<ManyToManyRelationshipMetadata> listRel1, IEnumerable<ManyToManyRelationshipMetadata> listRel2)
        {
            var listRelOnlyIn1 = new FormatTextTableHandler(true);
            var listRelOnlyIn2 = new FormatTextTableHandler(true);

            listRelOnlyIn1.SetHeader(
                nameof(ManyToManyRelationshipMetadata.SchemaName)
                , nameof(ManyToManyRelationshipMetadata.IntersectEntityName)
                , nameof(ManyToManyRelationshipMetadata.Entity1LogicalName)
                , nameof(ManyToManyRelationshipMetadata.Entity1IntersectAttribute)
                , nameof(ManyToManyRelationshipMetadata.Entity2LogicalName)
                , nameof(ManyToManyRelationshipMetadata.Entity2IntersectAttribute)
                , nameof(ManyToManyRelationshipMetadata.IsCustomRelationship)
                , nameof(ManyToManyRelationshipMetadata.IsManaged)
            );
            listRelOnlyIn2.SetHeader(
                nameof(ManyToManyRelationshipMetadata.SchemaName)
                , nameof(ManyToManyRelationshipMetadata.IntersectEntityName)
                , nameof(ManyToManyRelationshipMetadata.Entity1LogicalName)
                , nameof(ManyToManyRelationshipMetadata.Entity1IntersectAttribute)
                , nameof(ManyToManyRelationshipMetadata.Entity2LogicalName)
                , nameof(ManyToManyRelationshipMetadata.Entity2IntersectAttribute)
                , nameof(ManyToManyRelationshipMetadata.IsCustomRelationship)
                , nameof(ManyToManyRelationshipMetadata.IsManaged)
            );

            Dictionary<string, List<string>> dictDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (ManyToManyRelationshipMetadata rel2 in listRel2.OrderBy(s => s.SchemaName))
            {
                if (_notExisting.Contains(rel2.Entity1LogicalName) || _notExisting.Contains(rel2.Entity2LogicalName))
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
                if (_notExisting.Contains(rel1.Entity1LogicalName) || _notExisting.Contains(rel1.Entity2LogicalName))
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

                if (_notExisting.Contains(rel1.Entity1LogicalName) || _notExisting.Contains(rel1.Entity2LogicalName))
                {
                    continue;
                }

                if (_notExisting.Contains(rel2.Entity1LogicalName) || _notExisting.Contains(rel2.Entity2LogicalName))
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

            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.IntersectEntityName), rel1.IntersectEntityName, rel2.IntersectEntityName);

            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.Entity1LogicalName), rel1.Entity1LogicalName, rel2.Entity1LogicalName);
            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.Entity1IntersectAttribute), rel1.Entity1IntersectAttribute, rel2.Entity1IntersectAttribute);
            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.Entity1NavigationPropertyName), rel1.Entity1NavigationPropertyName, rel2.Entity1NavigationPropertyName);

            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.Entity2LogicalName), rel1.Entity2LogicalName, rel2.Entity2LogicalName);
            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.Entity2IntersectAttribute), rel1.Entity2IntersectAttribute, rel2.Entity2IntersectAttribute);
            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.Entity2NavigationPropertyName), rel1.Entity2NavigationPropertyName, rel2.Entity2NavigationPropertyName);

            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.IsCustomizable), rel1.IsCustomizable, rel2.IsCustomizable);
            //table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.IsManaged), rel1.IsManaged, rel2.IsManaged);

            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.IsCustomRelationship), rel1.IsCustomRelationship, rel2.IsCustomRelationship);
            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.IsValidForAdvancedFind), rel1.IsValidForAdvancedFind, rel2.IsValidForAdvancedFind);

            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.IntroducedVersion), rel1.IntroducedVersion, rel2.IntroducedVersion);

            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.RelationshipType), rel1.RelationshipType, rel2.RelationshipType);
            table.AddLineIfNotEqual(nameof(ManyToManyRelationshipMetadata.SecurityTypes), rel1.SecurityTypes, rel2.SecurityTypes);

            //AddLine(table, "IsCustomizable", key1.IsCustomizable, key2.IsCustomizable);
            //AddEntityMetadataString(table, "IsManaged", relationship.IsManaged);

            AddAssociatedMenuConfigurationDifference(table, nameof(ManyToManyRelationshipMetadata.Entity1AssociatedMenuConfiguration), rel1.Entity1AssociatedMenuConfiguration, rel2.Entity1AssociatedMenuConfiguration);
            AddAssociatedMenuConfigurationDifference(table, nameof(ManyToManyRelationshipMetadata.Entity2AssociatedMenuConfiguration), rel1.Entity2AssociatedMenuConfiguration, rel2.Entity2AssociatedMenuConfiguration);

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
            var listKeysOnlyIn1 = new FormatTextTableHandler(true);
            var listKeysOnlyIn2 = new FormatTextTableHandler(true);

            listKeysOnlyIn1.SetHeader(
                nameof(EntityKeyMetadata.LogicalName)
                , nameof(EntityKeyMetadata.SchemaName)
                , nameof(EntityKeyMetadata.IsManaged)
                , nameof(EntityKeyMetadata.KeyAttributes)
            );
            listKeysOnlyIn2.SetHeader(
                nameof(EntityKeyMetadata.LogicalName)
                , nameof(EntityKeyMetadata.SchemaName)
                , nameof(EntityKeyMetadata.IsManaged)
            );

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

                table.AddLineIfNotEqual(nameof(EntityKeyMetadata.SchemaName), key1.SchemaName, key2.SchemaName);
                table.AddLineIfNotEqual(nameof(EntityKeyMetadata.IsCustomizable), key1.IsCustomizable, key2.IsCustomizable);
                table.AddLineIfNotEqual(nameof(EntityKeyMetadata.IntroducedVersion), key1.IntroducedVersion, key2.IntroducedVersion);
                table.AddLineIfNotEqual(nameof(EntityKeyMetadata.EntityLogicalName), key1.EntityLogicalName, key2.EntityLogicalName);
                table.AddLineIfNotEqual(nameof(EntityKeyMetadata.EntityKeyIndexStatus), key1.EntityKeyIndexStatus, key2.EntityKeyIndexStatus);
                table.AddLineIfNotEqual(nameof(EntityKeyMetadata.KeyAttributes), string.Join(",", key1.KeyAttributes.OrderBy(s => s)), string.Join(",", key2.KeyAttributes.OrderBy(s => s)));

                if (table.Count > 0)
                {
                    strDifference.AddRange(table.GetFormatedLines(false));
                }
            }

            return strDifference;
        }

        private async Task CompareAttributesAsync(OrganizationDifferenceImageBuilder imageBuilder, List<string> strDifference, string entityName, IEnumerable<AttributeMetadata> attributes1, IEnumerable<AttributeMetadata> attributes2)
        {
            var listAttributesOnlyIn1 = new FormatTextTableHandler(true);
            var listAttributesOnlyIn2 = new FormatTextTableHandler(true);

            listAttributesOnlyIn1.SetHeader(
                nameof(AttributeMetadata.LogicalName)
                , "TypeName"
                , nameof(AttributeMetadata.AttributeType)
                , nameof(AttributeMetadata.IsCustomAttribute)
                , nameof(AttributeMetadata.IsManaged)
                , "Target"
            );
            listAttributesOnlyIn2.SetHeader(
                nameof(AttributeMetadata.LogicalName)
                , "TypeName"
                , nameof(AttributeMetadata.AttributeType)
                , nameof(AttributeMetadata.IsCustomAttribute)
                , nameof(AttributeMetadata.IsManaged)
                , "Target"
            );

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

                table.AddLineIfNotEqual(nameof(AttributeMetadata.AttributeOf), attr1.AttributeOf, attr2.AttributeOf);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.AttributeType), attr1.AttributeType, attr2.AttributeType);

                table.AddLineIfNotEqual(nameof(AttributeMetadata.AttributeTypeName)
                    , attr1.AttributeTypeName != null ? attr1.AttributeTypeName.Value : "null"
                    , attr2.AttributeTypeName != null ? attr2.AttributeTypeName.Value : "null"
                );

                table.AddLineIfNotEqual(nameof(AttributeMetadata.CanBeSecuredForCreate), attr1.CanBeSecuredForCreate, attr2.CanBeSecuredForCreate);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.CanBeSecuredForRead), attr1.CanBeSecuredForRead, attr2.CanBeSecuredForRead);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.CanBeSecuredForUpdate), attr1.CanBeSecuredForUpdate, attr2.CanBeSecuredForUpdate);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.CanModifyAdditionalSettings), attr1.CanModifyAdditionalSettings, attr2.CanModifyAdditionalSettings);
                //EntityValuesComparer.AddLineIfNotEqual("ColumnNumber), attr1.ColumnNumber, attr2.ColumnNumber);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.InheritsFrom), attr1.InheritsFrom, attr2.InheritsFrom);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsAuditEnabled), attr1.IsAuditEnabled, attr2.IsAuditEnabled);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsCustomAttribute), attr1.IsCustomAttribute, attr2.IsCustomAttribute);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsCustomizable), attr1.IsCustomizable, attr2.IsCustomizable);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsFilterable), attr1.IsFilterable, attr2.IsFilterable);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsGlobalFilterEnabled), attr1.IsGlobalFilterEnabled, attr2.IsGlobalFilterEnabled);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsLogical), attr1.IsLogical, attr2.IsLogical);
                //table.AddLineIfNotEqual(nameof(AttributeMetadata.IsManaged), attr1.IsManaged, attr2.IsManaged);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsPrimaryId), attr1.IsPrimaryId, attr2.IsPrimaryId);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsPrimaryName), attr1.IsPrimaryName, attr2.IsPrimaryName);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsRenameable), attr1.IsRenameable, attr2.IsRenameable);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsRetrievable), attr1.IsRetrievable, attr2.IsRetrievable);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsSearchable), attr1.IsSearchable, attr2.IsSearchable);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsSecured), attr1.IsSecured, attr2.IsSecured);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsSortableEnabled), attr1.IsSortableEnabled, attr2.IsSortableEnabled);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsValidForAdvancedFind), attr1.IsValidForAdvancedFind, attr2.IsValidForAdvancedFind);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsValidForCreate), attr1.IsValidForCreate, attr2.IsValidForCreate);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsValidForRead), attr1.IsValidForRead, attr2.IsValidForRead);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.IsValidForUpdate), attr1.IsValidForUpdate, attr2.IsValidForUpdate);

                table.AddLineIfNotEqual(nameof(AttributeMetadata.RequiredLevel), attr1.RequiredLevel, attr2.RequiredLevel);

                table.AddLineIfNotEqual(nameof(AttributeMetadata.SchemaName), attr1.SchemaName, attr2.SchemaName);
                table.AddLineIfNotEqual(nameof(AttributeMetadata.SourceType), attr1.SourceType, attr2.SourceType);

                List<string> additionalDifference = new List<string>();

                if (attr1.GetType().Name != attr2.GetType().Name)
                {
                    table.AddLineIfNotEqual("Type", attr1.GetType().Name, attr2.GetType().Name);
                }
                else
                {
                    if (attr1 is MemoAttributeMetadata)
                    {
                        MemoAttributeMetadata memoAttrib1 = attr1 as MemoAttributeMetadata;
                        MemoAttributeMetadata memoAttrib2 = attr2 as MemoAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(MemoAttributeMetadata.MaxLength), memoAttrib1.MaxLength, memoAttrib2.MaxLength);
                        table.AddLineIfNotEqual(nameof(MemoAttributeMetadata.Format), memoAttrib1.Format, memoAttrib2.Format);
                        table.AddLineIfNotEqual(nameof(MemoAttributeMetadata.ImeMode), memoAttrib1.ImeMode, memoAttrib2.ImeMode);
                        table.AddLineIfNotEqual(nameof(MemoAttributeMetadata.IsLocalizable), memoAttrib1.IsLocalizable, memoAttrib2.IsLocalizable);
                    }

                    if (attr1 is StringAttributeMetadata)
                    {
                        StringAttributeMetadata stringAttrib1 = attr1 as StringAttributeMetadata;
                        StringAttributeMetadata stringAttrib2 = attr2 as StringAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(StringAttributeMetadata.MaxLength), stringAttrib1.MaxLength, stringAttrib2.MaxLength);
                        table.AddLineIfNotEqual(nameof(StringAttributeMetadata.Format), stringAttrib1.Format, stringAttrib2.Format);
                        table.AddLineIfNotEqual(nameof(StringAttributeMetadata.ImeMode), stringAttrib1.ImeMode, stringAttrib2.ImeMode);
                        table.AddLineIfNotEqual(nameof(StringAttributeMetadata.IsLocalizable), stringAttrib1.IsLocalizable, stringAttrib2.IsLocalizable);

                        if (stringAttrib1.FormulaDefinition != stringAttrib2.FormulaDefinition)
                        {
                            table.AddLine(nameof(StringAttributeMetadata.FormulaDefinition));
                        }
                    }

                    if (attr1 is IntegerAttributeMetadata)
                    {
                        IntegerAttributeMetadata intAttrib1 = attr1 as IntegerAttributeMetadata;
                        IntegerAttributeMetadata intAttrib2 = attr2 as IntegerAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(IntegerAttributeMetadata.MinValue), intAttrib1.MinValue, intAttrib2.MinValue);
                        table.AddLineIfNotEqual(nameof(IntegerAttributeMetadata.MaxValue), intAttrib1.MaxValue, intAttrib2.MaxValue);
                        table.AddLineIfNotEqual(nameof(IntegerAttributeMetadata.Format), intAttrib1.Format, intAttrib2.Format);

                        if (intAttrib1.FormulaDefinition != intAttrib2.FormulaDefinition)
                        {
                            table.AddLine(nameof(IntegerAttributeMetadata.FormulaDefinition));
                        }
                    }

                    if (attr1 is BigIntAttributeMetadata)
                    {
                        BigIntAttributeMetadata bigIntAttrib1 = attr1 as BigIntAttributeMetadata;
                        BigIntAttributeMetadata bigIntAttrib2 = attr2 as BigIntAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(BigIntAttributeMetadata.MinValue), bigIntAttrib1.MinValue, bigIntAttrib2.MinValue);
                        table.AddLineIfNotEqual(nameof(BigIntAttributeMetadata.MaxValue), bigIntAttrib1.MaxValue, bigIntAttrib2.MaxValue);
                    }

                    if (attr1 is ImageAttributeMetadata)
                    {
                        ImageAttributeMetadata imageAttrib1 = attr1 as ImageAttributeMetadata;
                        ImageAttributeMetadata imageAttrib2 = attr1 as ImageAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(ImageAttributeMetadata.IsPrimaryImage), imageAttrib1.IsPrimaryImage, imageAttrib2.IsPrimaryImage);
                        table.AddLineIfNotEqual(nameof(ImageAttributeMetadata.MaxHeight), imageAttrib1.MaxHeight, imageAttrib2.MaxHeight);
                        table.AddLineIfNotEqual(nameof(ImageAttributeMetadata.MaxWidth), imageAttrib1.MaxWidth, imageAttrib2.MaxWidth);
                    }

                    if (attr1 is MoneyAttributeMetadata)
                    {
                        MoneyAttributeMetadata moneyAttrib1 = attr1 as MoneyAttributeMetadata;
                        MoneyAttributeMetadata moneyAttrib2 = attr2 as MoneyAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(MoneyAttributeMetadata.MinValue), moneyAttrib1.MinValue, moneyAttrib2.MinValue);
                        table.AddLineIfNotEqual(nameof(MoneyAttributeMetadata.MaxValue), moneyAttrib1.MaxValue, moneyAttrib2.MaxValue);
                        table.AddLineIfNotEqual(nameof(MoneyAttributeMetadata.Precision), moneyAttrib1.Precision, moneyAttrib2.Precision);
                        table.AddLineIfNotEqual(nameof(MoneyAttributeMetadata.PrecisionSource), moneyAttrib1.PrecisionSource, moneyAttrib2.PrecisionSource);
                        table.AddLineIfNotEqual(nameof(MoneyAttributeMetadata.IsBaseCurrency), moneyAttrib1.IsBaseCurrency, moneyAttrib2.IsBaseCurrency);
                        table.AddLineIfNotEqual(nameof(MoneyAttributeMetadata.ImeMode), moneyAttrib1.ImeMode, moneyAttrib2.ImeMode);
                        table.AddLineIfNotEqual(nameof(MoneyAttributeMetadata.CalculationOf), moneyAttrib1.CalculationOf, moneyAttrib2.CalculationOf);

                        if (moneyAttrib1.FormulaDefinition != moneyAttrib2.FormulaDefinition)
                        {
                            table.AddLine(nameof(MoneyAttributeMetadata.FormulaDefinition));
                        }
                    }

                    if (attr1 is DecimalAttributeMetadata)
                    {
                        DecimalAttributeMetadata decimalAttrib1 = attr1 as DecimalAttributeMetadata;
                        DecimalAttributeMetadata decimalAttrib2 = attr2 as DecimalAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(DecimalAttributeMetadata.MinValue), decimalAttrib1.MinValue, decimalAttrib2.MinValue);
                        table.AddLineIfNotEqual(nameof(DecimalAttributeMetadata.MaxValue), decimalAttrib1.MaxValue, decimalAttrib2.MaxValue);
                        table.AddLineIfNotEqual(nameof(DecimalAttributeMetadata.Precision), decimalAttrib1.Precision, decimalAttrib2.Precision);
                        table.AddLineIfNotEqual(nameof(DecimalAttributeMetadata.ImeMode), decimalAttrib1.ImeMode, decimalAttrib2.ImeMode);

                        if (decimalAttrib1.FormulaDefinition != decimalAttrib2.FormulaDefinition)
                        {
                            table.AddLine(nameof(DecimalAttributeMetadata.FormulaDefinition));
                        }
                    }

                    if (attr1 is DoubleAttributeMetadata)
                    {
                        DoubleAttributeMetadata doubleAttrib1 = attr1 as DoubleAttributeMetadata;
                        DoubleAttributeMetadata doubleAttrib2 = attr2 as DoubleAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(DoubleAttributeMetadata.MinValue), doubleAttrib1.MinValue, doubleAttrib2.MinValue);
                        table.AddLineIfNotEqual(nameof(DoubleAttributeMetadata.MaxValue), doubleAttrib1.MaxValue, doubleAttrib2.MaxValue);
                        table.AddLineIfNotEqual(nameof(DoubleAttributeMetadata.Precision), doubleAttrib1.Precision, doubleAttrib2.Precision);
                        table.AddLineIfNotEqual(nameof(DoubleAttributeMetadata.ImeMode), doubleAttrib1.ImeMode, doubleAttrib2.ImeMode);
                    }

                    if (attr1 is BooleanAttributeMetadata)
                    {
                        BooleanAttributeMetadata boolAttrib1 = attr1 as BooleanAttributeMetadata;
                        BooleanAttributeMetadata boolAttrib2 = attr2 as BooleanAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(BooleanAttributeMetadata.OptionSet.FalseOption), boolAttrib1.OptionSet.FalseOption.Value, boolAttrib2.OptionSet.FalseOption.Value);
                        table.AddLineIfNotEqual(nameof(BooleanAttributeMetadata.OptionSet.TrueOption), boolAttrib1.OptionSet.TrueOption.Value, boolAttrib2.OptionSet.TrueOption.Value);

                        if (boolAttrib1.FormulaDefinition != boolAttrib2.FormulaDefinition)
                        {
                            table.AddLine(nameof(BooleanAttributeMetadata.FormulaDefinition));
                        }

                        if (boolAttrib1.OptionSet != null && boolAttrib2.OptionSet == null)
                        {
                            table.AddLine(nameof(BooleanAttributeMetadata.OptionSet), "not null", "null");
                        }
                        else if (boolAttrib1.OptionSet == null && boolAttrib2.OptionSet != null)
                        {
                            table.AddLine(nameof(BooleanAttributeMetadata.OptionSet), "null", "not null");
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

                    if (attr1 is EnumAttributeMetadata
                        && (attr1 is PicklistAttributeMetadata || attr1 is MultiSelectPicklistAttributeMetadata)
                    )
                    {
                        var enumAttrib1 = attr1 as EnumAttributeMetadata;
                        var enumAttrib2 = attr2 as EnumAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(EnumAttributeMetadata.DefaultFormValue), enumAttrib1.DefaultFormValue, enumAttrib2.DefaultFormValue);

                        if (attr1 is PicklistAttributeMetadata pickList1
                            && attr2 is PicklistAttributeMetadata pickList2
                        )
                        {
                            if (pickList1.FormulaDefinition != pickList2.FormulaDefinition)
                            {
                                table.AddLine(nameof(PicklistAttributeMetadata.FormulaDefinition));
                            }
                        }

                        if (attr1 is MultiSelectPicklistAttributeMetadata multiPickList1
                            && attr2 is MultiSelectPicklistAttributeMetadata multiPickList2
                        )
                        {
                            if (multiPickList1.FormulaDefinition != multiPickList2.FormulaDefinition)
                            {
                                table.AddLine(nameof(MultiSelectPicklistAttributeMetadata.FormulaDefinition));
                            }
                        }

                        if (enumAttrib1.OptionSet != null && enumAttrib2.OptionSet == null)
                        {
                            table.AddLine(nameof(EnumAttributeMetadata.OptionSet), "not null", "null");
                        }
                        else if (enumAttrib1.OptionSet == null && enumAttrib2.OptionSet != null)
                        {
                            table.AddLine(nameof(EnumAttributeMetadata.OptionSet), "null", "not null");
                        }
                        else if (enumAttrib1.OptionSet != null && enumAttrib2.OptionSet != null)
                        {
                            if (!CreateFileHandler.IgnoreAttribute(enumAttrib1.EntityLogicalName, enumAttrib1.LogicalName))
                            {
                                List<string> diffenrenceOptionSet = await _optionSetComparer.GetDifference(enumAttrib1.OptionSet, enumAttrib2.OptionSet, enumAttrib1.EntityLogicalName, enumAttrib1.LogicalName);

                                if (diffenrenceOptionSet.Count > 0)
                                {
                                    additionalDifference.Add(
                                        string.Format("Difference in OptionSet {0} and {1}"
                                        , enumAttrib1.OptionSet.Name + (enumAttrib1.OptionSet.IsGlobal.GetValueOrDefault() ? "(Global)" : "(Local)")
                                        , enumAttrib2.OptionSet.Name + (enumAttrib2.OptionSet.IsGlobal.GetValueOrDefault() ? "(Global)" : "(Local)")
                                    ));
                                    diffenrenceOptionSet.ForEach(s => additionalDifference.Add(_tabSpacer + s));
                                }
                            }
                        }
                    }

                    if (attr1 is StatusAttributeMetadata)
                    {
                        StatusAttributeMetadata statusAttrib1 = attr1 as StatusAttributeMetadata;
                        StatusAttributeMetadata statusAttrib2 = attr2 as StatusAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(StatusAttributeMetadata.DefaultFormValue), statusAttrib1.DefaultFormValue, statusAttrib2.DefaultFormValue);
                    }

                    if (attr1 is StateAttributeMetadata)
                    {
                        StateAttributeMetadata stateAttrib1 = attr1 as StateAttributeMetadata;
                        StateAttributeMetadata stateAttrib2 = attr2 as StateAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(StateAttributeMetadata.DefaultFormValue), stateAttrib1.DefaultFormValue, stateAttrib2.DefaultFormValue);
                    }

                    if (attr1 is EntityNameAttributeMetadata)
                    {
                        EntityNameAttributeMetadata entityNameAttrib1 = attr1 as EntityNameAttributeMetadata;
                        EntityNameAttributeMetadata entityNameAttrib2 = attr2 as EntityNameAttributeMetadata;

                        table.AddLineIfNotEqual(nameof(EntityNameAttributeMetadata.DefaultFormValue), entityNameAttrib1.DefaultFormValue, entityNameAttrib2.DefaultFormValue);

                        //if (entityNameAttrib1.OptionSet != null && entityNameAttrib2.OptionSet != null)
                        //{

                        //}
                    }

                    if (attr1 is LookupAttributeMetadata)
                    {
                        LookupAttributeMetadata lookupAttrib1 = attr1 as LookupAttributeMetadata;
                        LookupAttributeMetadata lookupAttrib2 = attr2 as LookupAttributeMetadata;

                        string targets1 = string.Join(",", lookupAttrib1.Targets.OrderBy(s => s));
                        string targets2 = string.Join(",", lookupAttrib2.Targets.OrderBy(s => s));

                        if (targets1 != targets2)
                        {
                            string t1 = string.Join(",", lookupAttrib1.Targets.Except(lookupAttrib2.Targets).OrderBy(s => s));
                            string t2 = string.Join(",", lookupAttrib2.Targets.Except(lookupAttrib1.Targets).OrderBy(s => s));

                            table.AddLine(nameof(LookupAttributeMetadata.Targets), t1, t2);
                        }
                    }

                    if (attr1 is DateTimeAttributeMetadata)
                    {
                        DateTimeAttributeMetadata dateTimeAttrib1 = attr1 as DateTimeAttributeMetadata;
                        DateTimeAttributeMetadata dateTimeAttrib2 = attr2 as DateTimeAttributeMetadata;

                        table.AddLineIfNotEqual(
                            nameof(DateTimeAttributeMetadata.DateTimeBehavior)
                            , dateTimeAttrib1.DateTimeBehavior != null ? dateTimeAttrib1.DateTimeBehavior.Value : "null"
                            , dateTimeAttrib2.DateTimeBehavior != null ? dateTimeAttrib2.DateTimeBehavior.Value : "null"
                        );

                        table.AddLineIfNotEqual(nameof(DateTimeAttributeMetadata.CanChangeDateTimeBehavior), dateTimeAttrib1.CanChangeDateTimeBehavior, dateTimeAttrib2.CanChangeDateTimeBehavior);
                        table.AddLineIfNotEqual(nameof(DateTimeAttributeMetadata.Format), dateTimeAttrib1.Format, dateTimeAttrib2.Format);
                        table.AddLineIfNotEqual(nameof(DateTimeAttributeMetadata.ImeMode), dateTimeAttrib1.ImeMode, dateTimeAttrib2.ImeMode);

                        if (dateTimeAttrib1.FormulaDefinition != dateTimeAttrib2.FormulaDefinition)
                        {
                            table.AddLine(nameof(DateTimeAttributeMetadata.FormulaDefinition));
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