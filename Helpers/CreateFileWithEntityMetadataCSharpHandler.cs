using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class CreateFileWithEntityMetadataCSharpHandler : CreateFileHandler
    {
        private EntityMetadata _entityMetadata;

        private readonly DependencyDescriptionHandler _descriptorHandler;
        private readonly DependencyRepository _dependencyRepository;
        private readonly SolutionComponentDescriptor _solutionComponentDescriptor;

        private string _fieldHeader;

        public IOrganizationServiceExtented _service;

        private CreateFileWithEntityMetadataCSharpConfiguration _config;

        private Task<List<StringMap>> _listStringMap;
        private Task<List<AttributeMap>> _listAttributeMap;

        private readonly IWriteToOutput _iWriteToOutput;
        private Task _taskDownloadMetadata;

        public CreateFileWithEntityMetadataCSharpHandler(
            CreateFileWithEntityMetadataCSharpConfiguration config
            , IOrganizationServiceExtented service
            , IWriteToOutput iWriteToOutput
            ) : base(config.TabSpacer, config.AllDescriptions)
        {
            this._config = config;
            this._service = service;
            this._iWriteToOutput = iWriteToOutput;

            this._solutionComponentDescriptor = new SolutionComponentDescriptor(_service, false);
            this._dependencyRepository = new DependencyRepository(_service);
            this._descriptorHandler = new DependencyDescriptionHandler(_solutionComponentDescriptor);
        }

        public Task<string> CreateFileAsync(string fileName = null)
        {
            return Task.Run(async () => await CreateFile(fileName));
        }

        private async Task<string> CreateFile(string fileName = null)
        {
            if (_config.EntityMetadata == null)
            {
                this._entityMetadata = this._solutionComponentDescriptor.MetadataSource.GetEntityMetadata(_config.EntityName);
            }
            else
            {
                this._entityMetadata = _config.EntityMetadata;

                this._solutionComponentDescriptor.MetadataSource.StoreEntityMetadata(_config.EntityMetadata);
            }

            HashSet<string> hashSet = GetLinkedEntities(this._entityMetadata);

            hashSet.Remove(this._entityMetadata.LogicalName);

            this._taskDownloadMetadata = this._solutionComponentDescriptor.MetadataSource.DownloadEntityMetadataOnlyForNamesAsync(hashSet.ToArray(), new[] { "DisplayName", "DisplayCollectionName", "Description", "PrimaryIdAttribute", "PrimaryNameAttribute" }, true);

            var repositoryStringMap = new StringMapRepository(_service);
            this._listStringMap = repositoryStringMap.GetListAsync(this._entityMetadata.LogicalName);

            var repositoryAttributeMap = new AttributeMapRepository(_service);
            this._listAttributeMap = repositoryAttributeMap.GetListWithEntityMapAsync(this._entityMetadata.LogicalName);

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = string.Format("{0}.{1}.Generated.cs", this._service.ConnectionData.Name, _entityMetadata.SchemaName);
            }

            var fileFilePath = Path.Combine(this._config.Folder, fileName);

            if (this._config.ConstantType == Model.ConstantType.ReadOnlyField)
            {
                _fieldHeader = "static readonly";
            }
            else
            {
                _fieldHeader = "const";
            }

            StartWriting(fileFilePath);

            WriteLine();

            WriteLine("namespace {0}", this._service.ConnectionData.NameSpaceClasses);
            WriteLine("{");

            WriteSummaryEntity();

            WriteLine("public partial class {0}", _entityMetadata.SchemaName);
            WriteLine("{");

            if (_config.GenerateIntoSchemaClass)
            {
                WriteLine("public static partial class Schema");
                WriteLine("{");
            }

            WriteLine("public {0} string EntityLogicalName = \"{1}\";", _fieldHeader, _entityMetadata.LogicalName);

            WriteLine();

            WriteLine("public {0} string EntitySchemaName = \"{1}\";", _fieldHeader, _entityMetadata.SchemaName);

            if (!string.IsNullOrEmpty(_entityMetadata.PrimaryNameAttribute))
            {
                WriteLine();

                WriteLine("public {0} string EntityPrimaryNameAttribute = \"{1}\";", _fieldHeader, _entityMetadata.PrimaryNameAttribute);
            }

            WriteLine();
            WriteLine("public {0} string EntityPrimaryIdAttribute = \"{1}\";", _fieldHeader, _entityMetadata.PrimaryIdAttribute);

            await WriteAttributesToFile();

            await WriteEnums();

            WriteKeys(_entityMetadata.Keys);

            if (this._config.GenerateManyToOne)
            {
                await WriteOneToMany(_entityMetadata.ManyToOneRelationships, "ManyToOne", "N:1");
            }

            if (this._config.GenerateOneToMany)
            {
                await WriteOneToMany(_entityMetadata.OneToManyRelationships, "OneToMany", "1:N");
            }

            await WriteManyToMany(_entityMetadata.ManyToManyRelationships);

            WriteLine("}");
            if (_config.GenerateIntoSchemaClass)
            {
                WriteLine("}");
            }
            Write("}");

            EndWriting();

            return fileFilePath;
        }

        private HashSet<string> GetLinkedEntities(EntityMetadata entityMetadata)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (entityMetadata.Attributes != null)
            {
                var attrs = entityMetadata.Attributes.OfType<LookupAttributeMetadata>();

                foreach (var attr in attrs)
                {
                    if (attr.Targets != null)
                    {
                        foreach (var item in attr.Targets)
                        {
                            result.Add(item);
                        }
                    }
                }
            }

            if (entityMetadata.OneToManyRelationships != null)
            {
                foreach (var rel in entityMetadata.OneToManyRelationships)
                {
                    result.Add(rel.ReferencedEntity);
                    result.Add(rel.ReferencingEntity);
                }
            }

            if (entityMetadata.ManyToOneRelationships != null)
            {
                foreach (var rel in entityMetadata.ManyToOneRelationships)
                {
                    result.Add(rel.ReferencedEntity);
                    result.Add(rel.ReferencingEntity);
                }
            }

            if (entityMetadata.ManyToManyRelationships != null)
            {
                foreach (var rel in entityMetadata.ManyToManyRelationships)
                {
                    result.Add(rel.Entity1LogicalName);
                    result.Add(rel.Entity2LogicalName);
                }
            }

            return result;
        }

        private async Task WriteEnums()
        {
            if (!this._config.GenerateStatus && !this._config.GenerateLocalOptionSet && !this._config.GenerateGlobalOptionSet)
            {
                return;
            }

            bool hasOptionSets = HasOptionSets();

            if (!hasOptionSets)
            {
                return;
            }

            WriteLine();
            WriteLine("#region OptionSets.");

            WriteLine();
            WriteLine("public static partial class OptionSets");
            WriteLine("{");

            bool first = true;

            first = await WriteStateStatusOptionSets(first);

            first = await WriteRegularOptionSets(first);

            WriteLine("}");

            WriteLine();
            WriteLine("#endregion OptionSets.");
        }

        private bool HasOptionSets()
        {
            var stateAttr = _entityMetadata.Attributes.OfType<StateAttributeMetadata>().FirstOrDefault();
            var statusAttr = _entityMetadata.Attributes.OfType<StatusAttributeMetadata>().FirstOrDefault();

            var picklists = _entityMetadata
                .Attributes.OfType<PicklistAttributeMetadata>()
                .Where(p => (p.OptionSet.IsGlobal.GetValueOrDefault() && this._config.GenerateGlobalOptionSet)
                    || (!p.OptionSet.IsGlobal.GetValueOrDefault() && this._config.GenerateLocalOptionSet))
                ;

            return stateAttr != null || statusAttr != null || picklists.Any();
        }

        private void WriteSummaryEntity()
        {
            List<string> summary = new List<string>();

            CreateFileHandler.FillLabelEntity(summary, _config.AllDescriptions, _entityMetadata.DisplayName, _entityMetadata.DisplayCollectionName, _entityMetadata.Description, _tabSpacer);

            List<string> footers = GetEntityMetadataDescription();

            if (footers.Count > 0)
            {
                if (summary.Count > 0)
                {
                    summary.Add(string.Empty);
                }

                summary.AddRange(footers);
            }

            WriteSummaryStrings(summary);
        }

        private List<string> GetEntityMetadataDescription()
        {
            FormatTextTableHandler table = new FormatTextTableHandler();

            table.AddEntityMetadataString("ActivityTypeMask", _entityMetadata.ActivityTypeMask);
            table.AddEntityMetadataString("AutoCreateAccessTeams", _entityMetadata.AutoCreateAccessTeams);
            table.AddEntityMetadataString("AutoRouteToOwnerQueue", _entityMetadata.AutoRouteToOwnerQueue);
            table.AddEntityMetadataString("CanBeInManyToMany", _entityMetadata.CanBeInManyToMany);
            table.AddEntityMetadataString("CanBePrimaryEntityInRelationship", _entityMetadata.CanBePrimaryEntityInRelationship);
            table.AddEntityMetadataString("CanBeRelatedEntityInRelationship", _entityMetadata.CanBeRelatedEntityInRelationship);
            table.AddEntityMetadataString("CanChangeHierarchicalRelationship", _entityMetadata.CanChangeHierarchicalRelationship);
            table.AddEntityMetadataString("CanChangeTrackingBeEnabled", _entityMetadata.CanChangeTrackingBeEnabled);
            table.AddEntityMetadataString("CanCreateAttributes", _entityMetadata.CanCreateAttributes);
            table.AddEntityMetadataString("CanCreateCharts", _entityMetadata.CanCreateCharts);
            table.AddEntityMetadataString("CanCreateForms", _entityMetadata.CanCreateForms);
            table.AddEntityMetadataString("CanCreateViews", _entityMetadata.CanCreateViews);
            table.AddEntityMetadataString("CanEnableSyncToExternalSearchIndex", _entityMetadata.CanEnableSyncToExternalSearchIndex);
            table.AddEntityMetadataString("CanModifyAdditionalSettings", _entityMetadata.CanModifyAdditionalSettings);
            table.AddEntityMetadataString("CanTriggerWorkflow", _entityMetadata.CanTriggerWorkflow);
            table.AddEntityMetadataString("ChangeTrackingEnabled", _entityMetadata.ChangeTrackingEnabled);
            table.AddEntityMetadataString("CollectionSchemaName", _entityMetadata.CollectionSchemaName);
            table.AddEntityMetadataString("DaysSinceRecordLastModified", _entityMetadata.DaysSinceRecordLastModified);
            table.AddEntityMetadataString("EnforceStateTransitions", _entityMetadata.EnforceStateTransitions);
            table.AddEntityMetadataString("EntityColor", _entityMetadata.EntityColor);
            table.AddEntityMetadataString("EntityHelpUrl", _entityMetadata.EntityHelpUrl);
            table.AddEntityMetadataString("EntityHelpUrlEnabled", _entityMetadata.EntityHelpUrlEnabled);
            table.AddEntityMetadataString("EntitySetName", _entityMetadata.EntitySetName);
            table.AddEntityMetadataString("IconLargeName", _entityMetadata.IconLargeName);
            table.AddEntityMetadataString("IconMediumName", _entityMetadata.IconMediumName);
            table.AddEntityMetadataString("IconSmallName", _entityMetadata.IconSmallName);
            table.AddEntityMetadataString("IsActivity", _entityMetadata.IsActivity);
            table.AddEntityMetadataString("IsActivityParty", _entityMetadata.IsActivityParty);
            table.AddEntityMetadataString("IsAIRUpdated", _entityMetadata.IsAIRUpdated);
            table.AddEntityMetadataString("IsAuditEnabled", _entityMetadata.IsAuditEnabled);
            table.AddEntityMetadataString("IsAvailableOffline", _entityMetadata.IsAvailableOffline);
            table.AddEntityMetadataString("IsBusinessProcessEnabled", _entityMetadata.IsBusinessProcessEnabled);
            table.AddEntityMetadataString("IsChildEntity", _entityMetadata.IsChildEntity);
            table.AddEntityMetadataString("IsConnectionsEnabled", _entityMetadata.IsConnectionsEnabled);
            table.AddEntityMetadataString("IsCustomEntity", _entityMetadata.IsCustomEntity);
            table.AddEntityMetadataString("IsCustomizable", _entityMetadata.IsCustomizable);
            table.AddEntityMetadataString("IsDocumentManagementEnabled", _entityMetadata.IsDocumentManagementEnabled);
            table.AddEntityMetadataString("IsDuplicateDetectionEnabled", _entityMetadata.IsDuplicateDetectionEnabled);
            table.AddEntityMetadataString("IsEnabledForCharts", _entityMetadata.IsEnabledForCharts);
            table.AddEntityMetadataString("IsEnabledForExternalChannels", _entityMetadata.IsEnabledForExternalChannels);
            table.AddEntityMetadataString("IsEnabledForTrace", _entityMetadata.IsEnabledForTrace);
            table.AddEntityMetadataString("IsImportable", _entityMetadata.IsImportable);
            table.AddEntityMetadataString("IsInteractionCentricEnabled", _entityMetadata.IsInteractionCentricEnabled);
            table.AddEntityMetadataString("IsIntersect", _entityMetadata.IsIntersect);
            table.AddEntityMetadataString("IsKnowledgeManagementEnabled", _entityMetadata.IsKnowledgeManagementEnabled);
            table.AddEntityMetadataString("IsMailMergeEnabled", _entityMetadata.IsMailMergeEnabled);

            if (this._config.WithManagedInfo)
            {
                table.AddEntityMetadataString("IsManaged", _entityMetadata.IsManaged);
            }

            table.AddEntityMetadataString("IsMappable", _entityMetadata.IsMappable);
            table.AddEntityMetadataString("IsOfflineInMobileClient", _entityMetadata.IsOfflineInMobileClient);
            table.AddEntityMetadataString("IsOneNoteIntegrationEnabled", _entityMetadata.IsOneNoteIntegrationEnabled);
            table.AddEntityMetadataString("IsOptimisticConcurrencyEnabled", _entityMetadata.IsOptimisticConcurrencyEnabled);
            table.AddEntityMetadataString("IsPrivate", _entityMetadata.IsPrivate);
            table.AddEntityMetadataString("IsQuickCreateEnabled", _entityMetadata.IsQuickCreateEnabled);
            table.AddEntityMetadataString("IsReadingPaneEnabled", _entityMetadata.IsReadingPaneEnabled);
            table.AddEntityMetadataString("IsReadOnlyInMobileClient", _entityMetadata.IsReadOnlyInMobileClient);
            table.AddEntityMetadataString("IsRenameable", _entityMetadata.IsRenameable);
            table.AddEntityMetadataString("IsStateModelAware", _entityMetadata.IsStateModelAware);
            table.AddEntityMetadataString("IsValidForAdvancedFind", _entityMetadata.IsValidForAdvancedFind);
            table.AddEntityMetadataString("IsValidForQueue", _entityMetadata.IsValidForQueue);
            table.AddEntityMetadataString("IsVisibleInMobile", _entityMetadata.IsVisibleInMobile);
            table.AddEntityMetadataString("IsVisibleInMobileClient", _entityMetadata.IsVisibleInMobileClient);
            table.AddEntityMetadataString("LogicalCollectionName", _entityMetadata.LogicalCollectionName);
            table.AddEntityMetadataString("LogicalName", _entityMetadata.LogicalName);
            table.AddEntityMetadataString("ObjectTypeCode", _entityMetadata.ObjectTypeCode);
            table.AddEntityMetadataString("OwnershipType", _entityMetadata.OwnershipType);
            table.AddEntityMetadataString("RecurrenceBaseEntityLogicalName", _entityMetadata.RecurrenceBaseEntityLogicalName);
            table.AddEntityMetadataString("ReportViewName", _entityMetadata.ReportViewName);
            table.AddEntityMetadataString("SchemaName", _entityMetadata.SchemaName);
            table.AddEntityMetadataString("SyncToExternalSearchIndex", _entityMetadata.SyncToExternalSearchIndex);

            //AddEntityMetadataString(table, "SyncToExternalSearchIndex", _entityMetadata.SyncToExternalSearchIndex);

            table.SetHeader("PropertyName", "Value");

            List<string> result = table.GetFormatedLines(false);

            return result;
        }

        private async Task<bool> WriteRegularOptionSets(bool first)
        {
            if (!this._config.GenerateLocalOptionSet && !this._config.GenerateGlobalOptionSet)
            {
                return first;
            }

            var picklists = _entityMetadata.Attributes
                .OfType<PicklistAttributeMetadata>()
                .Where(p => (p.OptionSet.IsGlobal.GetValueOrDefault() && this._config.GenerateGlobalOptionSet)
                        || (!p.OptionSet.IsGlobal.GetValueOrDefault() && this._config.GenerateLocalOptionSet)
                        )
                ;

            picklists = picklists.Where(e => e.OptionSet.Options.Any(o => o.Value.HasValue));

            if (!picklists.Any())
            {
                return first;
            }

            WriteLine();
            WriteLine("#region Picklist OptionSet OptionSets.");

            foreach (var attrib in picklists
                .Where(p => (!p.OptionSet.IsGlobal.GetValueOrDefault() && this._config.GenerateLocalOptionSet))
                .OrderBy(attr => attr.LogicalName))
            {
                if (first) { first = false; } else { WriteLine(); }

                await GenerateOptionSetEnums(attrib, attrib.OptionSet);
            }

            foreach (var attrib in picklists
               .Where(p => (p.OptionSet.IsGlobal.GetValueOrDefault() && this._config.GenerateGlobalOptionSet))
               .OrderBy(attr => attr.LogicalName))
            {
                if (first) { first = false; } else { WriteLine(); }

                await GenerateOptionSetEnums(attrib, attrib.OptionSet);
            }

            WriteLine();
            WriteLine("#endregion Picklist OptionSets.");

            return first;
        }

        private async Task<bool> WriteStateStatusOptionSets(bool first)
        {
            if (!this._config.GenerateStatus)
            {
                return first;
            }

            var stateAttr = _entityMetadata.Attributes.OfType<StateAttributeMetadata>().FirstOrDefault();
            var statusAttr = _entityMetadata.Attributes.OfType<StatusAttributeMetadata>().FirstOrDefault();

            if (stateAttr != null && statusAttr != null)
            {
                if (first) { first = false; } else { WriteLine(); }

                WriteLine("#region State and Status OptionSets.");

                await GenerateStateOptionSet(stateAttr, statusAttr);

                await GenerateStatusOptionSet(statusAttr, stateAttr);

                WriteLine();
                WriteLine("#endregion State and Status OptionSets.");
            }

            return first;
        }

        private async Task GenerateStatusOptionSet(StatusAttributeMetadata statusAttr, StateAttributeMetadata stateAttr)
        {
            WriteLine();

            var headers = new List<string> { string.Format("Attribute: {0}", statusAttr.LogicalName) };
            if (this._config.WithManagedInfo)
            {
                headers.Add(string.Format("IsManaged: {0}", statusAttr.OptionSet.IsManaged));
            }
            headers.Add("Value Format: Statecode_Statuscode");

            WriteSummary(stateAttr.DisplayName, stateAttr.Description, headers, null);
            if (_config.OptionSetExportType == OptionSetExportType.Enums)
            {
                WriteLine("public enum statuscode");
            }
            else
            {
                WriteLine("public static partial class statuscode");
            }
            WriteLine("{");

            var options = CreateFileHandler.GetStatusOptionItems(statusAttr, stateAttr, await this._listStringMap);

            bool first = true;

            // Формируем значения
            foreach (var item in options)
            {
                if (first) { first = false; } else { WriteLine(); }

                var header = new List<string> { string.Format("Linked Statecode: {0}, {1}", item.LinkedStateCodeName, item.LinkedStateCode.ToString()) };

                if (item.DisplayOrder.HasValue)
                {
                    header.Add(string.Format("DisplayOrder: {0}", item.DisplayOrder.Value));
                }

                if (this._config.WithManagedInfo)
                {
                    header.Add(string.Format("IsManaged: {0}", item.OptionMetadata.IsManaged.GetValueOrDefault()));
                }

                WriteSummary(item.Label, item.Description, header, null);

                var str = item.MakeStrings();

                if (_config.OptionSetExportType == OptionSetExportType.Enums)
                {
                    WriteLine("[System.Runtime.Serialization.EnumMemberAttribute()]");
                    WriteLine(str + ",");
                }
                else
                {
                    WriteLine("public {0} int {1};", _fieldHeader, str);
                }
            }

            WriteLine("}");
        }

        private async Task GenerateStateOptionSet(StateAttributeMetadata stateAttr, StatusAttributeMetadata statusAttr)
        {
            WriteLine();

            var headers = new List<string> { string.Format("Attribute: {0}", stateAttr.LogicalName) };
            if (this._config.WithManagedInfo)
            {
                headers.Add(string.Format("IsManaged: {0}", stateAttr.OptionSet.IsManaged));
            }

            WriteSummary(stateAttr.DisplayName, stateAttr.Description, headers, null);

            if (_config.OptionSetExportType == OptionSetExportType.Enums)
            {
                WriteLine("public enum statecode");
            }
            else
            {
                WriteLine("public static partial class statecode");
            }
            WriteLine("{");

            var options = CreateFileHandler.GetStateOptionItems(statusAttr, stateAttr, await this._listStringMap);

            bool first = true;

            // Формируем значения
            foreach (var item in options)
            {
                if (first) { first = false; } else { WriteLine(); }

                var header = new List<string> { string.Format("Default statuscode: {0}, {1}", item.DefaultStatusCodeName, item.DefaultStatusCode.ToString()), string.Format("InvariantName: {0}", item.InvariantName) };

                if (item.DisplayOrder.HasValue)
                {
                    header.Add(string.Format("DisplayOrder: {0}", item.DisplayOrder.Value));
                }

                if (this._config.WithManagedInfo)
                {
                    header.Add(string.Format("IsManaged: {0}", item.OptionMetadata.IsManaged.GetValueOrDefault()));
                }

                WriteSummary(item.Label, item.Description, header, null);

                var str = item.MakeStrings();

                if (_config.OptionSetExportType == OptionSetExportType.Enums)
                {
                    WriteLine("[System.Runtime.Serialization.EnumMemberAttribute()]");
                    WriteLine(str + ",");
                }
                else
                {
                    WriteLine("public {0} int {1};", _fieldHeader, str);
                }
            }

            WriteLine("}");
        }

        private async Task WriteAttributesToFile()
        {
            if (!this._config.GenerateAttributes)
            {
                return;
            }

            if (_entityMetadata.Attributes == null)
            {
                return;
            }

            if (!_entityMetadata.Attributes.Any())
            {
                return;
            }

            var attributes = _entityMetadata.Attributes;

            if (!attributes.Any())
            {
                return;
            }

            WriteLine();
            WriteLine("#region Attributes.");

            WriteLine();
            WriteLine("public static partial class Attributes");
            WriteLine("{");

            bool first = true;

            foreach (AttributeMetadata attrib in attributes.OrderBy(attr => attr.LogicalName))
            {
                if (first) { first = false; } else { WriteLine(); }

                await this._taskDownloadMetadata;

                List<string> footers = GetAttributeDescription(attrib, _config.AllDescriptions, _config.WithManagedInfo, this._solutionComponentDescriptor);

                WriteSummary(attrib.DisplayName, attrib.Description, null, footers);

                string str = string.Format("public {0} string {1} = \"{2}\";", _fieldHeader, attrib.LogicalName.ToLower(), attrib.LogicalName);

                bool ignore = !string.IsNullOrEmpty(attrib.AttributeOf);

                if (ignore)
                {
                    str = "//" + str;
                }

                WriteLine(str);
            }

            WriteLine("}");

            WriteLine();
            WriteLine("#endregion Attributes.");
        }

        private async Task GenerateOptionSetEnums(AttributeMetadata attrib, OptionSetMetadata optionSet)
        {
            List<string> lines = new List<string>();

            lines.Add(string.Format("Attribute: {0}", attrib.LogicalName));

            if (optionSet.IsGlobal.GetValueOrDefault())
            {
                CreateFileHandler.FillLabelDisplayNameAndDescription(lines, _config.AllDescriptions, optionSet.DisplayName, optionSet.Description, _config.TabSpacer);
            }
            else
            {
                CreateFileHandler.FillLabelDisplayNameAndDescription(lines, _config.AllDescriptions, attrib.DisplayName, attrib.Description, _config.TabSpacer);
            }

            lines.Add(string.Empty);

            string managedStr = string.Empty;

            if (this._config.WithManagedInfo)
            {
                managedStr = " " + (optionSet.IsManaged.GetValueOrDefault() ? "Managed" : "Unmanaged");
            }

            string temp = string.Format("{0} {1} {2} OptionSet {3}"
                , optionSet.IsGlobal.GetValueOrDefault() ? "Global" : "Local"
                , optionSet.IsCustomOptionSet.GetValueOrDefault() ? "Custom" : "System"
                , managedStr
                , optionSet.Name
            );

            lines.Add(temp);

            if (optionSet.IsGlobal.GetValueOrDefault())
            {
                CreateFileHandler.FillLabelDisplayNameAndDescription(lines, _config.AllDescriptions, attrib.DisplayName, attrib.Description, _config.TabSpacer);
            }
            else
            {
                CreateFileHandler.FillLabelDisplayNameAndDescription(lines, _config.AllDescriptions, optionSet.DisplayName, optionSet.Description, _config.TabSpacer);
            }

            if (optionSet.IsGlobal.GetValueOrDefault() && this._config.WithDependentComponents)
            {
                var coll = await _dependencyRepository.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet.MetadataId.Value);

                await this._taskDownloadMetadata;

                var desc = await _descriptorHandler.GetDescriptionDependentAsync(coll);

                var split = desc.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Any())
                {
                    lines.Add(string.Empty);

                    foreach (var item in split)
                    {
                        lines.Add(item);
                    }
                }
            }

            var options = GetOptionItems(attrib.EntityLogicalName, attrib.LogicalName, optionSet, await this._listStringMap);

            if (!options.Any())
            {
                return;
            }

            WriteSummaryStrings(lines);

            {
                bool ignore = IgnoreAttribute(_entityMetadata.LogicalName, attrib.LogicalName);

                string str = "";

                if (ignore)
                {
                    str += "//";
                }

                var enumName = attrib.LogicalName;

                if (optionSet.IsGlobal.GetValueOrDefault())
                {
                    enumName = optionSet.Name;
                }

                if (_config.OptionSetExportType == OptionSetExportType.Enums)
                {
                    str += string.Format("public enum {0}", enumName);
                }
                else
                {
                    str += string.Format("public static partial class {0}", enumName);
                }

                WriteLine(str);

                if (ignore)
                {
                    return;
                }
            }

            WriteLine("{");

            bool first = true;

            // Формируем значения
            foreach (var item in options)
            {
                if (first) { first = false; } else { WriteLine(); }

                List<string> header = new List<string>() { item.Value.ToString() };

                if (item.DisplayOrder.HasValue)
                {
                    header.Add(string.Format("DisplayOrder: {0}", item.DisplayOrder.Value));
                }

                if (this._config.WithManagedInfo)
                {
                    header.Add(string.Format("IsManaged: {0}", item.OptionMetadata.IsManaged.GetValueOrDefault()));
                }

                WriteSummary(item.Label, item.Description, header, null);

                var str = item.MakeStrings();

                if (_config.OptionSetExportType == OptionSetExportType.Enums)
                {
                    WriteLine("[System.Runtime.Serialization.EnumMemberAttribute()]");
                    WriteLine(str + ",");
                }
                else
                {
                    WriteLine("public {0} int {1};", _fieldHeader, str);
                }
            }

            WriteLine("}");
        }

        private async Task WriteOneToMany(OneToManyRelationshipMetadata[] metadata, string className, string relationTypeName)
        {
            var relationshipColl = metadata.Where(rel => !string.IsNullOrEmpty(rel.SchemaName));

            if (!relationshipColl.Any())
            {
                return;
            }

            WriteLine();
            WriteLine("#region Relationship {0} - {1}.", className, relationTypeName);

            WriteLine();
            WriteLine("public static partial class {0}", className);
            WriteLine("{");

            bool first = true;
            foreach (var relationship in relationshipColl.OrderBy(r => r.SchemaName))
            {
                if (first) { first = false; } else { WriteLine(); }

                List<string> lines = new List<string>();
                lines.Add(string.Format("{0} - Relationship {1}", relationTypeName, relationship.SchemaName));

                {
                    List<string> footers = GetRelationshipMetadataOneToManyDescription(relationship);

                    if (footers.Any())
                    {
                        lines.Add(string.Empty);
                        lines.AddRange(footers);
                    }
                }

                {
                    EntityMetadata entityMetadata = null;
                    string nameField = string.Empty;

                    if (!string.Equals(this._entityMetadata.LogicalName, relationship.ReferencedEntity, StringComparison.OrdinalIgnoreCase))
                    {
                        nameField = "ReferencedEntity";

                        await this._taskDownloadMetadata;

                        entityMetadata = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.ReferencedEntity);
                    }
                    else if (!string.Equals(this._entityMetadata.LogicalName, relationship.ReferencingEntity, StringComparison.OrdinalIgnoreCase))
                    {
                        nameField = "ReferencingEntity";

                        await this._taskDownloadMetadata;

                        entityMetadata = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.ReferencingEntity);
                    }

                    if (entityMetadata != null)
                    {
                        List<string> lineEntityDescription = new List<string>();

                        CreateFileHandler.FillLabelEntity(lineEntityDescription, _config.AllDescriptions, entityMetadata.DisplayName, entityMetadata.DisplayCollectionName, entityMetadata.Description, _tabSpacer);

                        if (lineEntityDescription.Any())
                        {
                            if (lines.Any())
                            {
                                lines.Add(string.Empty);
                            }

                            lines.Add(string.Format("{0} {1}:", nameField, entityMetadata.LogicalName));
                            lineEntityDescription.ForEach(s => lines.Add(_config.TabSpacer + s));
                        }
                    }
                }

                {
                    var attributeMaps = (await _listAttributeMap).Where(a =>
                        string.Equals(a.EntityMapIdSourceEntityName, relationship.ReferencedEntity, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(a.EntityMapIdTargetEntityName, relationship.ReferencingEntity, StringComparison.OrdinalIgnoreCase)
                    );

                    if (attributeMaps.Any())
                    {
                        FormatTextTableHandler table = new FormatTextTableHandler();
                        table.AddLine("SourceEntity", "", "TargetEntity");
                        table.AddLine(relationship.ReferencedEntity, "->", relationship.ReferencingEntity);

                        table.AddLine(string.Empty);
                        table.AddLine("SourceAttribute", "", "TargetAttribute");

                        foreach (var item in attributeMaps.OrderBy(a => a.SourceAttributeName).ThenBy(a => a.TargetAttributeName))
                        {
                            table.AddLine(item.SourceAttributeName, "->", item.TargetAttributeName);
                        }

                        if (lines.Any())
                        {
                            lines.Add(string.Empty);
                        }

                        lines.Add("AttributeMaps:");

                        table.GetFormatedLines(false).ForEach(s => lines.Add(_config.TabSpacer + s));
                    }
                }

                WriteSummaryStrings(lines);

                WriteLine("public static partial class {0}", relationship.SchemaName.ToLower());
                WriteLine("{");

                WriteLine("public {0} string Name = \"{1}\";", _fieldHeader, relationship.SchemaName);

                WriteLine();
                WriteLine("public {0} string ReferencedEntity_{1} = \"{1}\";", _fieldHeader, relationship.ReferencedEntity);

                WriteLine();
                WriteLine("public {0} string ReferencedAttribute_{1} = \"{1}\";", _fieldHeader, relationship.ReferencedAttribute);

                if (!string.Equals(this._entityMetadata.LogicalName, relationship.ReferencedEntity, StringComparison.OrdinalIgnoreCase))
                {
                    await this._taskDownloadMetadata;

                    var referencedEntityMetadata = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.ReferencedEntity);

                    if (!string.IsNullOrEmpty(referencedEntityMetadata.PrimaryNameAttribute))
                    {
                        WriteLine();
                        WriteLine("public {0} string ReferencedEntity_PrimaryNameAttribute_{1} = \"{1}\";", _fieldHeader, referencedEntityMetadata.PrimaryNameAttribute);
                    }
                }

                WriteLine();
                WriteLine("public {0} string ReferencingEntity_{1} = \"{1}\";", _fieldHeader, relationship.ReferencingEntity);

                WriteLine();
                WriteLine("public {0} string ReferencingAttribute_{1} = \"{1}\";", _fieldHeader, relationship.ReferencingAttribute);

                if (!string.Equals(this._entityMetadata.LogicalName, relationship.ReferencingEntity, StringComparison.OrdinalIgnoreCase))
                {
                    await this._taskDownloadMetadata;

                    var referencingEntityMetadata = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.ReferencingEntity);

                    if (!string.IsNullOrEmpty(referencingEntityMetadata.PrimaryNameAttribute))
                    {
                        WriteLine();
                        WriteLine("public {0} string ReferencingEntity_PrimaryNameAttribute_{1} = \"{1}\";", _fieldHeader, referencingEntityMetadata.PrimaryNameAttribute);
                    }
                }

                WriteLine("}");
            }

            WriteLine("}");

            WriteLine();
            WriteLine("#endregion Relationship {0} - {1}.", className, relationTypeName);
        }

        private List<string> GetRelationshipMetadataOneToManyDescription(OneToManyRelationshipMetadata relationship)
        {
            FormatTextTableHandler table = new FormatTextTableHandler();

            table.AddEntityMetadataString("IsHierarchical", relationship.IsHierarchical);
            table.AddEntityMetadataString("ReferencedEntityNavigationPropertyName", relationship.ReferencedEntityNavigationPropertyName);
            table.AddEntityMetadataString("ReferencingEntityNavigationPropertyName", relationship.ReferencingEntityNavigationPropertyName);
            table.AddEntityMetadataString("IsCustomizable", relationship.IsCustomizable);
            table.AddEntityMetadataString("IsCustomRelationship", relationship.IsCustomRelationship);
            if (this._config.WithManagedInfo)
            {
                table.AddEntityMetadataString("IsManaged", relationship.IsManaged);
            }
            table.AddEntityMetadataString("IsValidForAdvancedFind", relationship.IsValidForAdvancedFind);
            table.AddEntityMetadataString("RelationshipType", relationship.RelationshipType);
            table.AddEntityMetadataString("SecurityTypes", relationship.SecurityTypes);

            if (relationship.CascadeConfiguration != null)
            {
                table.AddEntityMetadataString("CascadeConfiguration.Assign", relationship.CascadeConfiguration.Assign);
                table.AddEntityMetadataString("CascadeConfiguration.Delete", relationship.CascadeConfiguration.Delete);
                table.AddEntityMetadataString("CascadeConfiguration.Merge", relationship.CascadeConfiguration.Merge);
                table.AddEntityMetadataString("CascadeConfiguration.Reparent", relationship.CascadeConfiguration.Reparent);
                table.AddEntityMetadataString("CascadeConfiguration.Share", relationship.CascadeConfiguration.Share);
                table.AddEntityMetadataString("CascadeConfiguration.Unshare", relationship.CascadeConfiguration.Unshare);
            }

            if (relationship.AssociatedMenuConfiguration != null)
            {
                table.AddEntityMetadataString("AssociatedMenuConfiguration.Behavior", relationship.AssociatedMenuConfiguration.Behavior);
                table.AddEntityMetadataString("AssociatedMenuConfiguration.Group", relationship.AssociatedMenuConfiguration.Group);
                table.AddEntityMetadataString("AssociatedMenuConfiguration.Order", relationship.AssociatedMenuConfiguration.Order);

                if (relationship.AssociatedMenuConfiguration.Label != null
                    && relationship.AssociatedMenuConfiguration.Label.LocalizedLabels != null
                    && relationship.AssociatedMenuConfiguration.Label.LocalizedLabels.Where(s => !string.IsNullOrEmpty(s.Label)).Any()
                    )
                {
                    foreach (var label in relationship.AssociatedMenuConfiguration.Label.LocalizedLabels
                        .Where(s => !string.IsNullOrEmpty(s.Label))
                        .OrderBy(s => s.LanguageCode, new LocaleComparer())
                        )
                    {
                        table.AddEntityMetadataString("AssociatedMenuConfiguration.Label", string.Format("{0}: {1}", LanguageLocale.GetLocaleName(label.LanguageCode), label.Label));
                    }
                }
            }

            table.SetHeader("PropertyName", "Value");

            List<string> result = table.GetFormatedLines(false);

            return result;
        }

        private void WriteKeys(EntityKeyMetadata[] keys)
        {
            if (!this._config.GenerateKeys)
            {
                return;
            }

            if (keys == null)
            {
                return;
            }

            var keysColl = keys.Where(key => !string.IsNullOrEmpty(key.LogicalName));

            if (!keysColl.Any())
            {
                return;
            }

            WriteLine();
            WriteLine("#region EntityKeys.");

            WriteLine();
            WriteLine("public static partial class EntityKeys");
            WriteLine("{");

            bool first = true;
            foreach (var key in keysColl.OrderBy(r => r.LogicalName))
            {
                if (first) { first = false; } else { WriteLine(); }

                List<string> lines = new List<string>();

                CreateFileHandler.FillLabelDisplayNameAndDescription(lines, _config.AllDescriptions, key.DisplayName, new Label(), _config.TabSpacer);

                FormatTextTableHandler table = new FormatTextTableHandler();
                table.SetHeader("PropertyName", "Value");

                table.AddEntityMetadataString("SchemaName", key.SchemaName);
                if (this._config.WithManagedInfo)
                {
                    table.AddEntityMetadataString("IsManaged", key.IsManaged);
                }
                table.AddEntityMetadataString("IsCustomizable", key.IsCustomizable);

                lines.Add(string.Empty);
                lines.AddRange(table.GetFormatedLines(false));

                WriteSummaryStrings(lines);

                WriteLine("public static partial class {0}", key.LogicalName.ToLower());
                WriteLine("{");

                WriteLine("public {0} string Name = \"{1}\";", _fieldHeader, key.LogicalName);

                WriteLine();
                WriteLine("public static readonly System.Collections.ObjectModel.ReadOnlyCollection<string> KeyAttributes = new System.Collections.ObjectModel.ReadOnlyCollection<string>(new string[] { " + string.Join(", ", key.KeyAttributes.OrderBy(s => s).Select(s => "\"" + s + "\"")) + " });");

                WriteLine("}");
            }

            WriteLine("}");

            WriteLine();
            WriteLine("#endregion EntityKeys.");
        }

        private async Task WriteManyToMany(ManyToManyRelationshipMetadata[] metadata)
        {
            if (!this._config.GenerateManyToMany)
            {
                return;
            }

            var relationshipColl = metadata.Where(rel => !string.IsNullOrEmpty(rel.SchemaName));

            if (!relationshipColl.Any())
            {
                return;
            }

            WriteLine();
            WriteLine("#region Relationship ManyToMany - N:N.");

            WriteLine();
            WriteLine("public static partial class ManyToMany");
            WriteLine("{");

            bool first = true;

            foreach (var relationship in relationshipColl.OrderBy(r => r.SchemaName))
            {
                if (first) { first = false; } else { WriteLine(); }

                List<string> lines = new List<string>();
                lines.Add(string.Format("N:N - Relationship {0}", relationship.SchemaName));

                {
                    List<string> footers = GetRelationshipMetadataManyToManyDescription(relationship);

                    if (footers.Any())
                    {
                        lines.Add(string.Empty);
                        lines.AddRange(footers);
                    }
                }

                {
                    EntityMetadata entityMetadata = null;
                    string nameField = string.Empty;

                    if (!string.Equals(this._entityMetadata.LogicalName, relationship.Entity1LogicalName, StringComparison.OrdinalIgnoreCase))
                    {
                        nameField = "Entity1LogicalName";

                        await this._taskDownloadMetadata;

                        entityMetadata = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.Entity1LogicalName);
                    }
                    else if (!string.Equals(this._entityMetadata.LogicalName, relationship.Entity2LogicalName, StringComparison.OrdinalIgnoreCase))
                    {
                        nameField = "Entity2LogicalName";

                        await this._taskDownloadMetadata;

                        entityMetadata = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.Entity2LogicalName);
                    }

                    if (entityMetadata != null)
                    {
                        List<string> lineEntityDescription = new List<string>();

                        CreateFileHandler.FillLabelEntity(lineEntityDescription, _config.AllDescriptions, entityMetadata.DisplayName, entityMetadata.DisplayCollectionName, entityMetadata.Description, _tabSpacer);

                        if (lineEntityDescription.Any())
                        {
                            if (lines.Any())
                            {
                                lines.Add(string.Empty);
                            }

                            lines.Add(string.Format("{0} {1}:", nameField, entityMetadata.LogicalName));
                            lineEntityDescription.ForEach(s => lines.Add(_config.TabSpacer + s));
                        }
                    }
                }

                //{
                //    var attributeMaps = _listAttributeMap.Where(a =>
                //        string.Equals(a.EntityMapIdSourceEntityName, relationship.Entity1LogicalName, StringComparison.OrdinalIgnoreCase)
                //        && string.Equals(a.EntityMapIdTargetEntityName, relationship.Entity2LogicalName, StringComparison.OrdinalIgnoreCase)
                //    );

                //    if (attributeMaps.Any())
                //    {
                //        FormatTextTableHandler table = new FormatTextTableHandler();
                //        table.AddLine("SourceEntity", "", "TargetEntity");
                //        table.AddLine(relationship.Entity1LogicalName, "->", relationship.Entity2LogicalName);

                //        table.AddLine("SourceAttribute", "", "TargetAttribute");

                //        foreach (var item in attributeMaps.OrderBy(a => a.SourceAttributeName).ThenBy(a => a.TargetAttributeName))
                //        {
                //            table.AddLine(item.SourceAttributeName, "->", item.TargetAttributeName);
                //        }

                //        if (lines.Any())
                //        {
                //            lines.Add(string.Empty);
                //        }

                //        lines.Add("AttributeMaps:");

                //        table.GetFormatedLines(false).ForEach(s => lines.Add(_config.TabSpacer + s));
                //    }
                //}

                WriteSummaryStrings(lines);

                WriteLine("public static partial class {0}", relationship.SchemaName.ToLower());
                WriteLine("{");

                WriteLine("public {0} string Name = \"{1}\";", _fieldHeader, relationship.SchemaName);

                WriteLine();
                WriteLine("public {0} string IntersectEntity_{1} = \"{1}\";", _fieldHeader, relationship.IntersectEntityName);

                WriteLine();
                WriteLine("public {0} string Entity1_{1} = \"{1}\";", _fieldHeader, relationship.Entity1LogicalName);

                WriteLine();
                WriteLine("public {0} string Entity1Attribute_{1} = \"{1}\";", _fieldHeader, relationship.Entity1IntersectAttribute);

                if (!string.Equals(this._entityMetadata.LogicalName, relationship.Entity1LogicalName, StringComparison.OrdinalIgnoreCase))
                {
                    await this._taskDownloadMetadata;

                    var entity1LogicalName = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.Entity1LogicalName);

                    if (!string.IsNullOrEmpty(entity1LogicalName.PrimaryNameAttribute))
                    {
                        WriteLine();
                        WriteLine("public {0} string Entity1LogicalName_PrimaryNameAttribute_{1} = \"{1}\";", _fieldHeader, entity1LogicalName.PrimaryNameAttribute);
                    }
                }

                WriteLine();
                WriteLine("public {0} string Entity2_{1} = \"{1}\";", _fieldHeader, relationship.Entity2LogicalName);

                WriteLine();
                WriteLine("public {0} string Entity2Attribute_{1} = \"{1}\";", _fieldHeader, relationship.Entity2IntersectAttribute);

                if (!string.Equals(this._entityMetadata.LogicalName, relationship.Entity2LogicalName, StringComparison.OrdinalIgnoreCase))
                {
                    await this._taskDownloadMetadata;

                    var entity2LogicalName = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.Entity2LogicalName);

                    if (!string.IsNullOrEmpty(entity2LogicalName.PrimaryNameAttribute))
                    {
                        WriteLine();
                        WriteLine("public {0} string Entity2LogicalName{1} = \"{1}\";", _fieldHeader, entity2LogicalName.PrimaryNameAttribute);
                    }
                }

                WriteLine("}");
            }

            WriteLine("}");

            WriteLine();
            WriteLine("#endregion Relationship ManyToMany - N:N.");
        }

        private List<string> GetRelationshipMetadataManyToManyDescription(ManyToManyRelationshipMetadata relationship)
        {
            FormatTextTableHandler table = new FormatTextTableHandler();

            table.AddEntityMetadataString("Entity1NavigationPropertyName", relationship.Entity1NavigationPropertyName);
            table.AddEntityMetadataString("Entity2NavigationPropertyName", relationship.Entity2NavigationPropertyName);
            table.AddEntityMetadataString("IsCustomizable", relationship.IsCustomizable);
            table.AddEntityMetadataString("IsCustomRelationship", relationship.IsCustomRelationship);
            if (this._config.WithManagedInfo)
            {
                table.AddEntityMetadataString("IsManaged", relationship.IsManaged);
            }
            table.AddEntityMetadataString("IsValidForAdvancedFind", relationship.IsValidForAdvancedFind);
            table.AddEntityMetadataString("RelationshipType", relationship.RelationshipType);
            table.AddEntityMetadataString("SecurityTypes", relationship.SecurityTypes);

            if (relationship.Entity1AssociatedMenuConfiguration != null)
            {
                var config = relationship.Entity1AssociatedMenuConfiguration;

                table.AddEntityMetadataString("Entity1AssociatedMenuConfiguration.Behavior", config.Behavior);
                table.AddEntityMetadataString("Entity1AssociatedMenuConfiguration.Group", config.Group);
                table.AddEntityMetadataString("Entity1AssociatedMenuConfiguration.Order", config.Order);

                if (config.Label != null
                    && config.Label.LocalizedLabels != null
                    && config.Label.LocalizedLabels.Where(s => !string.IsNullOrEmpty(s.Label)).Any()
                    )
                {
                    foreach (var label in config.Label.LocalizedLabels
                        .Where(s => !string.IsNullOrEmpty(s.Label))
                        .OrderBy(s => s.LanguageCode, new LocaleComparer())
                        )
                    {
                        table.AddEntityMetadataString("Entity1AssociatedMenuConfiguration.Label", string.Format("{0}: {1}", LanguageLocale.GetLocaleName(label.LanguageCode), label.Label));
                    }
                }
            }

            if (relationship.Entity2AssociatedMenuConfiguration != null)
            {
                var config = relationship.Entity2AssociatedMenuConfiguration;

                table.AddEntityMetadataString("Entity2AssociatedMenuConfiguration.Behavior", config.Behavior);
                table.AddEntityMetadataString("Entity2AssociatedMenuConfiguration.Group", config.Group);
                table.AddEntityMetadataString("Entity2AssociatedMenuConfiguration.Order", config.Order);

                if (config.Label != null
                    && config.Label.LocalizedLabels != null
                    && config.Label.LocalizedLabels.Where(s => !string.IsNullOrEmpty(s.Label)).Any()
                    )
                {
                    foreach (var label in config.Label.LocalizedLabels
                        .Where(s => !string.IsNullOrEmpty(s.Label))
                        .OrderBy(s => s.LanguageCode, new LocaleComparer())
                        )
                    {
                        table.AddEntityMetadataString("Entity2AssociatedMenuConfiguration.Label", string.Format("{0}: {1}", LanguageLocale.GetLocaleName(label.LanguageCode), label.Label));
                    }
                }
            }

            table.SetHeader("PropertyName", "Value");

            List<string> result = table.GetFormatedLines(false);

            return result;
        }
    }
}
