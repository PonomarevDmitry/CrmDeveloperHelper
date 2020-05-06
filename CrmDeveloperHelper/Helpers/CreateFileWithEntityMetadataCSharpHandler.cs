using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        private readonly CreateFileCSharpConfiguration _config;

        private Task<List<StringMap>> _listStringMap;
        private Task<List<AttributeMap>> _listAttributeMap;

        private readonly IWriteToOutput _iWriteToOutput;
        private Task _taskDownloadMetadata;
        private readonly ICodeGenerationServiceProvider _iCodeGenerationServiceProvider;

        private string _entityClassName;

        public CreateFileWithEntityMetadataCSharpHandler(
            TextWriter writer
            , CreateFileCSharpConfiguration config
            , IOrganizationServiceExtented service
            , IWriteToOutput iWriteToOutput
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        ) : base(writer, config.TabSpacer, config.AllDescriptions)
        {
            this._config = config;
            this._service = service;
            this._iWriteToOutput = iWriteToOutput;
            this._iCodeGenerationServiceProvider = iCodeGenerationServiceProvider;

            this._solutionComponentDescriptor = new SolutionComponentDescriptor(_service)
            {
                WithManagedInfo = _config.WithManagedInfo,
            };
            this._dependencyRepository = new DependencyRepository(_service);
            this._descriptorHandler = new DependencyDescriptionHandler(_solutionComponentDescriptor);
        }

        public Task CreateFileAsync(string entityLogicalName)
        {
            return Task.Run(() => CreateFile(entityLogicalName, null));
        }

        public Task CreateFileAsync(EntityMetadata entityMetadata)
        {
            return Task.Run(() => CreateFile(entityMetadata.LogicalName, entityMetadata));
        }

        private async Task CreateFile(string entityLogicalName, EntityMetadata entityMetadata)
        {
            if (entityMetadata == null)
            {
                this._entityMetadata = this._solutionComponentDescriptor.MetadataSource.GetEntityMetadata(entityLogicalName);
            }
            else
            {
                this._entityMetadata = entityMetadata;

                this._solutionComponentDescriptor.MetadataSource.StoreEntityMetadata(entityMetadata);
            }

            HashSet<string> hashSet = GetLinkedEntities(this._entityMetadata);

            hashSet.Remove(this._entityMetadata.LogicalName);

            this._taskDownloadMetadata = this._solutionComponentDescriptor.MetadataSource.DownloadEntityMetadataOnlyForNamesAsync(hashSet.ToArray(), new[] { "DisplayName", "DisplayCollectionName", "Description", "PrimaryIdAttribute", "PrimaryNameAttribute" }, true);

            var repositoryStringMap = new StringMapRepository(_service);
            this._listStringMap = repositoryStringMap.GetListAsync(this._entityMetadata.LogicalName);

            var repositoryAttributeMap = new AttributeMapRepository(_service);
            this._listAttributeMap = repositoryAttributeMap.GetListWithEntityMapAsync(this._entityMetadata.LogicalName);

            if (this._config.ConstantType == Model.ConstantType.ReadOnlyField)
            {
                _fieldHeader = "static readonly";
            }
            else
            {
                _fieldHeader = "const";
            }

            WriteLine();

            WriteLine("namespace {0}", _config.NamespaceClasses);
            WriteLine("{");

            if (!_config.GenerateSchemaIntoSchemaClass)
            {
                WriteSummaryEntity();
            }

            this._entityClassName = _iCodeGenerationServiceProvider.NamingService.GetNameForEntity(_entityMetadata);

            WriteLine("public partial class {0}", _entityClassName);
            WriteLine("{");

            if (_config.GenerateSchemaIntoSchemaClass)
            {
                WriteSummaryEntity();
                WriteLine("public static partial class Schema");
                WriteLine("{");
            }

            WriteLine("public {0} string EntityLogicalName = \"{1}\";", _fieldHeader, _entityMetadata.LogicalName);

            WriteLine();
            WriteLine("public {0} string EntitySchemaName = \"{1}\";", _fieldHeader, _entityMetadata.SchemaName);

            WriteLine();
            WriteLine("public {0} string EntityPrimaryIdAttribute = \"{1}\";", _fieldHeader, _entityMetadata.PrimaryIdAttribute);

            if (!string.IsNullOrEmpty(_entityMetadata.PrimaryNameAttribute))
            {
                WriteLine();

                WriteLine("public {0} string EntityPrimaryNameAttribute = \"{1}\";", _fieldHeader, _entityMetadata.PrimaryNameAttribute);
            }

            if (!string.IsNullOrEmpty(_entityMetadata.PrimaryImageAttribute))
            {
                WriteLine();

                WriteLine("public {0} string EntityPrimaryImageAttribute = \"{1}\";", _fieldHeader, _entityMetadata.PrimaryImageAttribute);
            }

            if (_entityMetadata.ObjectTypeCode.HasValue)
            {
                WriteLine();

                if (_entityMetadata.IsCustomEntity.GetValueOrDefault())
                {
                    WriteLine("// public {0} int EntityObjectTypeCode = {1};", _fieldHeader, _entityMetadata.ObjectTypeCode);
                }
                else
                {
                    WriteLine("public {0} int EntityObjectTypeCode = {1};", _fieldHeader, _entityMetadata.ObjectTypeCode);
                }
            }

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

            if (_config.GenerateSchemaIntoSchemaClass)
            {
                WriteLine("}");
            }

            Write("}");
        }

        private HashSet<string> GetLinkedEntities(EntityMetadata entityMetadata)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

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
            table.SetHeader("PropertyName", "Value");

            // ^([^\r\n]*)
            // table.AddEntityMetadataString("$1", _entityMetadata.$1);

            table.AddEntityMetadataString(nameof(EntityMetadata.ActivityTypeMask), _entityMetadata.ActivityTypeMask);
            table.AddEntityMetadataString(nameof(EntityMetadata.AutoCreateAccessTeams), _entityMetadata.AutoCreateAccessTeams);
            table.AddEntityMetadataString(nameof(EntityMetadata.AutoRouteToOwnerQueue), _entityMetadata.AutoRouteToOwnerQueue);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanBeInManyToMany), _entityMetadata.CanBeInManyToMany);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanBePrimaryEntityInRelationship), _entityMetadata.CanBePrimaryEntityInRelationship);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanBeRelatedEntityInRelationship), _entityMetadata.CanBeRelatedEntityInRelationship);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanChangeHierarchicalRelationship), _entityMetadata.CanChangeHierarchicalRelationship);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanChangeTrackingBeEnabled), _entityMetadata.CanChangeTrackingBeEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanCreateAttributes), _entityMetadata.CanCreateAttributes);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanCreateCharts), _entityMetadata.CanCreateCharts);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanCreateForms), _entityMetadata.CanCreateForms);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanCreateViews), _entityMetadata.CanCreateViews);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanEnableSyncToExternalSearchIndex), _entityMetadata.CanEnableSyncToExternalSearchIndex);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanModifyAdditionalSettings), _entityMetadata.CanModifyAdditionalSettings);
            table.AddEntityMetadataString(nameof(EntityMetadata.CanTriggerWorkflow), _entityMetadata.CanTriggerWorkflow);
            table.AddEntityMetadataString(nameof(EntityMetadata.ChangeTrackingEnabled), _entityMetadata.ChangeTrackingEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.CollectionSchemaName), _entityMetadata.CollectionSchemaName);
            table.AddEntityMetadataString(nameof(EntityMetadata.DataProviderId), _entityMetadata.DataProviderId);
            table.AddEntityMetadataString(nameof(EntityMetadata.DataSourceId), _entityMetadata.DataSourceId);
            table.AddEntityMetadataString(nameof(EntityMetadata.EnforceStateTransitions), _entityMetadata.EnforceStateTransitions);
            table.AddEntityMetadataString(nameof(EntityMetadata.EntityColor), _entityMetadata.EntityColor);
            table.AddEntityMetadataString(nameof(EntityMetadata.EntityHelpUrl), _entityMetadata.EntityHelpUrl);
            table.AddEntityMetadataString(nameof(EntityMetadata.EntityHelpUrlEnabled), _entityMetadata.EntityHelpUrlEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.EntitySetName), _entityMetadata.EntitySetName);
            table.AddEntityMetadataString(nameof(EntityMetadata.ExternalCollectionName), _entityMetadata.ExternalCollectionName);
            table.AddEntityMetadataString(nameof(EntityMetadata.ExternalName), _entityMetadata.ExternalName);
            table.AddEntityMetadataString(nameof(EntityMetadata.IconLargeName), _entityMetadata.IconLargeName);
            table.AddEntityMetadataString(nameof(EntityMetadata.IconMediumName), _entityMetadata.IconMediumName);
            table.AddEntityMetadataString(nameof(EntityMetadata.IconSmallName), _entityMetadata.IconSmallName);
            table.AddEntityMetadataString(nameof(EntityMetadata.IconVectorName), _entityMetadata.IconVectorName);
            table.AddEntityMetadataString(nameof(EntityMetadata.IntroducedVersion), _entityMetadata.IntroducedVersion);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsAIRUpdated), _entityMetadata.IsAIRUpdated);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsActivity), _entityMetadata.IsActivity);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsActivityParty), _entityMetadata.IsActivityParty);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsAvailableOffline), _entityMetadata.IsAvailableOffline);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsBPFEntity), _entityMetadata.IsBPFEntity);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsBusinessProcessEnabled), _entityMetadata.IsBusinessProcessEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsChildEntity), _entityMetadata.IsChildEntity);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsConnectionsEnabled), _entityMetadata.IsConnectionsEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsCustomEntity), _entityMetadata.IsCustomEntity);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsCustomizable), _entityMetadata.IsCustomizable);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsDocumentManagementEnabled), _entityMetadata.IsDocumentManagementEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsDocumentRecommendationsEnabled), _entityMetadata.IsDocumentRecommendationsEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsDuplicateDetectionEnabled), _entityMetadata.IsDuplicateDetectionEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsEnabledForCharts), _entityMetadata.IsEnabledForCharts);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsEnabledForExternalChannels), _entityMetadata.IsEnabledForExternalChannels);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsEnabledForTrace), _entityMetadata.IsEnabledForTrace);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsImportable), _entityMetadata.IsImportable);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsInteractionCentricEnabled), _entityMetadata.IsInteractionCentricEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsIntersect), _entityMetadata.IsIntersect);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsKnowledgeManagementEnabled), _entityMetadata.IsKnowledgeManagementEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsLogicalEntity), _entityMetadata.IsLogicalEntity);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsMailMergeEnabled), _entityMetadata.IsMailMergeEnabled);

            if (this._config.WithManagedInfo)
            {
                table.AddEntityMetadataString(nameof(EntityMetadata.IsManaged), _entityMetadata.IsManaged);
            }

            table.AddEntityMetadataString(nameof(EntityMetadata.IsMappable), _entityMetadata.IsMappable);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsOfflineInMobileClient), _entityMetadata.IsOfflineInMobileClient);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsOneNoteIntegrationEnabled), _entityMetadata.IsOneNoteIntegrationEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsOptimisticConcurrencyEnabled), _entityMetadata.IsOptimisticConcurrencyEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsPrivate), _entityMetadata.IsPrivate);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsQuickCreateEnabled), _entityMetadata.IsQuickCreateEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsReadOnlyInMobileClient), _entityMetadata.IsReadOnlyInMobileClient);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsReadingPaneEnabled), _entityMetadata.IsReadingPaneEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsRenameable), _entityMetadata.IsRenameable);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsSLAEnabled), _entityMetadata.IsSLAEnabled);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsStateModelAware), _entityMetadata.IsStateModelAware);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsValidForAdvancedFind), _entityMetadata.IsValidForAdvancedFind);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsValidForQueue), _entityMetadata.IsValidForQueue);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsVisibleInMobile), _entityMetadata.IsVisibleInMobile);
            table.AddEntityMetadataString(nameof(EntityMetadata.IsVisibleInMobileClient), _entityMetadata.IsVisibleInMobileClient);
            table.AddEntityMetadataString(nameof(EntityMetadata.LogicalCollectionName), _entityMetadata.LogicalCollectionName);
            table.AddEntityMetadataString(nameof(EntityMetadata.LogicalName), _entityMetadata.LogicalName);
            table.AddEntityMetadataString(nameof(EntityMetadata.MobileOfflineFilters), _entityMetadata.MobileOfflineFilters);
            table.AddEntityMetadataString(nameof(EntityMetadata.ObjectTypeCode), _entityMetadata.ObjectTypeCode);
            table.AddEntityMetadataString(nameof(EntityMetadata.OwnershipType), _entityMetadata.OwnershipType);
            table.AddEntityMetadataString(nameof(EntityMetadata.PrimaryIdAttribute), _entityMetadata.PrimaryIdAttribute);
            table.AddEntityMetadataString(nameof(EntityMetadata.PrimaryImageAttribute), _entityMetadata.PrimaryImageAttribute);
            table.AddEntityMetadataString(nameof(EntityMetadata.PrimaryNameAttribute), _entityMetadata.PrimaryNameAttribute);
            table.AddEntityMetadataString(nameof(EntityMetadata.RecurrenceBaseEntityLogicalName), _entityMetadata.RecurrenceBaseEntityLogicalName);
            table.AddEntityMetadataString(nameof(EntityMetadata.ReportViewName), _entityMetadata.ReportViewName);
            table.AddEntityMetadataString(nameof(EntityMetadata.SchemaName), _entityMetadata.SchemaName);
            table.AddEntityMetadataString(nameof(EntityMetadata.SyncToExternalSearchIndex), _entityMetadata.SyncToExternalSearchIndex);
            table.AddEntityMetadataString(nameof(EntityMetadata.UsesBusinessDataLabelTable), _entityMetadata.UsesBusinessDataLabelTable);

            List<string> result = table.GetFormatedLines(false);

            return result;
        }

        private async Task WriteAttributesToFile()
        {
            if (!this._config.GenerateAttributes
                || _entityMetadata.Attributes == null
                || !_entityMetadata.Attributes.Any()
            )
            {
                return;
            }

            WriteLine();
            WriteLine($"#region {Properties.CodeGenerationStrings.Attributes}");
            WriteLine();

            WriteLine("public static partial class Attributes");
            WriteLine("{");

            await this._taskDownloadMetadata;

            bool first = true;

            var primaryAttributes = _entityMetadata.Attributes.Where(a => string.Equals(a.LogicalName, _entityMetadata.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase)
                     || string.Equals(a.LogicalName, _entityMetadata.PrimaryNameAttribute, StringComparison.InvariantCultureIgnoreCase));

            if (primaryAttributes.Any())
            {
                bool regionCreated = false;

                if (!string.IsNullOrEmpty(_entityMetadata.PrimaryIdAttribute))
                {
                    AttributeMetadata attributeMetadata = _entityMetadata.Attributes.FirstOrDefault(e => string.Equals(e.LogicalName, _entityMetadata.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase));

                    if (attributeMetadata != null
                        && attributeMetadata.IsPrimaryId.GetValueOrDefault()
                    )
                    {
                        if (first) { first = false; } else { WriteLine(); }
                        if (!regionCreated)
                        {
                            regionCreated = true;
                            WriteLine($"#region {Properties.CodeGenerationStrings.PrimaryAttributes}");
                            WriteLine();
                        }

                        GenerateAttributeMetadata(attributeMetadata);
                    }
                }

                if (!string.IsNullOrEmpty(_entityMetadata.PrimaryNameAttribute))
                {
                    AttributeMetadata attributeMetadata = _entityMetadata.Attributes.FirstOrDefault(e => string.Equals(e.LogicalName, _entityMetadata.PrimaryNameAttribute, StringComparison.InvariantCultureIgnoreCase));

                    if (attributeMetadata != null
                        && attributeMetadata.IsPrimaryName.GetValueOrDefault()
                    )
                    {
                        if (first) { first = false; } else { WriteLine(); }
                        if (!regionCreated)
                        {
                            regionCreated = true;
                            WriteLine($"#region {Properties.CodeGenerationStrings.PrimaryAttributes}");
                            WriteLine();
                        }

                        GenerateAttributeMetadata(attributeMetadata);
                    }
                }

                WriteLine();
                WriteLine($"#endregion {Properties.CodeGenerationStrings.PrimaryAttributes}");
            }

            var notPrimaryAttributes = _entityMetadata.Attributes.Where(a => !string.Equals(a.LogicalName, _entityMetadata.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase)
                    && !string.Equals(a.LogicalName, _entityMetadata.PrimaryNameAttribute, StringComparison.InvariantCultureIgnoreCase));

            var oobAttributes = notPrimaryAttributes.Where(a => !a.IsCustomAttribute.GetValueOrDefault());
            var customAttributes = notPrimaryAttributes.Where(a => a.IsCustomAttribute.GetValueOrDefault());

            if (oobAttributes.Any())
            {
                if (first) { first = false; } else { WriteLine(); }
                WriteLine($"#region {Properties.CodeGenerationStrings.OOBAttributes}");

                foreach (AttributeMetadata attributeMetadata in oobAttributes.OrderBy(attr => attr.LogicalName))
                {
                    WriteLine();
                    GenerateAttributeMetadata(attributeMetadata);
                }

                WriteLine();
                WriteLine($"#endregion {Properties.CodeGenerationStrings.OOBAttributes}");
            }

            if (customAttributes.Any())
            {
                if (first) { first = false; } else { WriteLine(); }
                WriteLine($"#region {Properties.CodeGenerationStrings.CustomAttributes}");

                foreach (AttributeMetadata attributeMetadata in customAttributes.OrderBy(attr => attr.LogicalName))
                {
                    WriteLine();
                    GenerateAttributeMetadata(attributeMetadata);
                }

                WriteLine();
                WriteLine($"#endregion {Properties.CodeGenerationStrings.CustomAttributes}");
            }

            WriteLine("}");

            WriteLine();
            WriteLine($"#endregion {Properties.CodeGenerationStrings.Attributes}");
        }

        private void GenerateAttributeMetadata(AttributeMetadata attributeMetadata)
        {
            string attributeNameInSchemaAttributes = GetAttributeNameInSchemaAttributes(attributeMetadata);

            string attributeNameInProxy = GetAttributeNameInProxy(attributeMetadata);

            string proxyClassAttributeSeeLink = string.Empty;

            bool attributeGeneratedInProxyClass = _iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateAttribute(attributeMetadata);

            if (attributeGeneratedInProxyClass)
            {
                string proxyClassAttributeFullName = $"{_config.NamespaceClasses}.{_entityClassName}.{attributeNameInProxy}";
                proxyClassAttributeSeeLink = string.Format("Proxy Class Attribute <see cref=\"{0}\"/>", proxyClassAttributeFullName);
            }

            string optionSetSeeLink = GetOptionSetSeeLink(_config.NamespaceGlobalOptionSets, attributeMetadata);



            List<string> footers = GetAttributeDescription(
                attributeMetadata
                , _config.AllDescriptions
                , _config.WithManagedInfo
                , this._solutionComponentDescriptor
                , _tabSpacer
                , proxyClassAttributeSeeLink
                , optionSetSeeLink
            );

            footers.AddRange(GetAttributeMetadataDescription(attributeMetadata));

            WriteSummary(attributeMetadata.DisplayName, attributeMetadata.Description, null, footers);

            string str = string.Format("public {0} string {1} = \"{2}\";", _fieldHeader, attributeNameInSchemaAttributes, attributeMetadata.LogicalName);

            bool ignore =
            !(
                attributeGeneratedInProxyClass
                || attributeMetadata.IsValidForGrid.GetValueOrDefault()
                || attributeMetadata.IsValidForForm.GetValueOrDefault()
                || (attributeMetadata.IsValidForAdvancedFind?.Value).GetValueOrDefault()
                || string.Equals(attributeMetadata.LogicalName, SystemForm.Schema.Attributes.solutionid, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(attributeMetadata.LogicalName, SystemForm.Schema.Attributes.supportingsolutionid, StringComparison.InvariantCultureIgnoreCase)
            );

            if (ignore)
            {
                str = "//" + str;
            }

            if (!ignore)
            {
                if (_config.AddDescriptionAttribute)
                {
                    string description = CreateFileHandler.GetLocalizedLabel(attributeMetadata.DisplayName);

                    if (string.IsNullOrEmpty(description))
                    {
                        description = CreateFileHandler.GetLocalizedLabel(attributeMetadata.Description);
                    }

                    if (!string.IsNullOrEmpty(description))
                    {
                        WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(description));
                    }
                }
            }

            WriteLine(str);
        }

        private List<string> GetAttributeMetadataDescription(AttributeMetadata attributeMetadata)
        {
            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("PropertyName", "Value");

            // ^([^\r\n]*)
            // table.AddEntityMetadataString("$1", attributeMetadata.$1);

            table.AddEntityMetadataString(nameof(AttributeMetadata.AutoNumberFormat), attributeMetadata.AutoNumberFormat);
            table.AddEntityMetadataString(nameof(AttributeMetadata.CanBeSecuredForCreate), attributeMetadata.CanBeSecuredForCreate);
            table.AddEntityMetadataString(nameof(AttributeMetadata.CanBeSecuredForRead), attributeMetadata.CanBeSecuredForRead);
            table.AddEntityMetadataString(nameof(AttributeMetadata.CanBeSecuredForUpdate), attributeMetadata.CanBeSecuredForUpdate);
            table.AddEntityMetadataString(nameof(AttributeMetadata.CanModifyAdditionalSettings), attributeMetadata.CanModifyAdditionalSettings);
            table.AddEntityMetadataString(nameof(AttributeMetadata.DeprecatedVersion), attributeMetadata.DeprecatedVersion);
            table.AddEntityMetadataString(nameof(AttributeMetadata.ExternalName), attributeMetadata.ExternalName);
            table.AddEntityMetadataString(nameof(AttributeMetadata.InheritsFrom), attributeMetadata.InheritsFrom);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IntroducedVersion), attributeMetadata.IntroducedVersion);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsCustomizable), attributeMetadata.IsCustomizable);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsDataSourceSecret), attributeMetadata.IsDataSourceSecret);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsFilterable), attributeMetadata.IsFilterable);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsGlobalFilterEnabled), attributeMetadata.IsGlobalFilterEnabled);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsPrimaryId), attributeMetadata.IsPrimaryId);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsPrimaryName), attributeMetadata.IsPrimaryName);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsRenameable), attributeMetadata.IsRenameable);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsRequiredForForm), attributeMetadata.IsRequiredForForm);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsRetrievable), attributeMetadata.IsRetrievable);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsSearchable), attributeMetadata.IsSearchable);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsSortableEnabled), attributeMetadata.IsSortableEnabled);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsValidForForm), attributeMetadata.IsValidForForm);
            table.AddEntityMetadataString(nameof(AttributeMetadata.IsValidForGrid), attributeMetadata.IsValidForGrid);

            List<string> result = table.GetFormatedLines(false);

            return result;
        }

        private async Task WriteEnums()
        {
            if (!this._config.GenerateStateStatusOptionSet && !this._config.GenerateLocalOptionSet && !this._config.GenerateGlobalOptionSet)
            {
                return;
            }

            bool hasOptionSets = HasOptionSets();

            if (!hasOptionSets)
            {
                return;
            }

            WriteLine();
            WriteLine($"#region {Properties.CodeGenerationStrings.OptionSets}");

            WriteLine();
            WriteLine("public static partial class OptionSets");
            WriteLine("{");

            bool first = true;

            first = await WriteStateStatusOptionSets(first);

            first = await WriteRegularOptionSets(first);

            WriteLine("}");

            WriteLine();
            WriteLine($"#endregion {Properties.CodeGenerationStrings.OptionSets}");
        }

        private bool HasOptionSets()
        {
            if (_config.GenerateStateStatusOptionSet)
            {
                var stateAttr = _entityMetadata.Attributes.OfType<StateAttributeMetadata>().FirstOrDefault();
                var statusAttr = _entityMetadata.Attributes.OfType<StatusAttributeMetadata>().FirstOrDefault();

                if (stateAttr != null || statusAttr != null)
                {
                    return true;
                }
            }

            var picklists = _entityMetadata
                .Attributes
                .Where(a => a is PicklistAttributeMetadata || a is MultiSelectPicklistAttributeMetadata)
                .OfType<EnumAttributeMetadata>()
                .Where(p => p.OptionSet != null)
                .Where(p => (p.OptionSet.IsGlobal.GetValueOrDefault() && this._config.GenerateGlobalOptionSet)
                    || (!p.OptionSet.IsGlobal.GetValueOrDefault() && this._config.GenerateLocalOptionSet))
            ;

            if (picklists.Any())
            {
                return true;
            }

            return false;
        }

        private async Task<bool> WriteRegularOptionSets(bool first)
        {
            if (!this._config.GenerateLocalOptionSet && !this._config.GenerateGlobalOptionSet)
            {
                return first;
            }

            var picklists = _entityMetadata
                .Attributes
                .Where(a => a is PicklistAttributeMetadata || a is MultiSelectPicklistAttributeMetadata)
                .OfType<EnumAttributeMetadata>()
                .Where(p => p.OptionSet != null)
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
            WriteLine($"#region {Properties.CodeGenerationStrings.PicklistOptionSets}");

            if (this._config.GenerateLocalOptionSet)
            {
                foreach (var attrib in picklists
                    .Where(p => !p.OptionSet.IsGlobal.GetValueOrDefault())
                    .OrderBy(attr => attr.LogicalName)
                )
                {
                    if (first) { first = false; } else { WriteLine(); }

                    await GenerateOptionSetEnums(new[] { attrib }, attrib.OptionSet);
                }
            }

            if (this._config.GenerateGlobalOptionSet)
            {
                var groups = picklists.Where(p => p.OptionSet.IsGlobal.GetValueOrDefault()).GroupBy(p => p.OptionSet.MetadataId, (k, g) => new { g.First().OptionSet, Attributes = g }).OrderBy(e => e.OptionSet.Name);

                foreach (var optionSet in groups)
                {
                    if (first) { first = false; } else { WriteLine(); }

                    await GenerateOptionSetEnums(optionSet.Attributes.OrderBy(a => a.LogicalName), optionSet.OptionSet);
                }
            }

            WriteLine();
            WriteLine($"#endregion {Properties.CodeGenerationStrings.PicklistOptionSets}");

            return first;
        }

        private async Task<bool> WriteStateStatusOptionSets(bool first)
        {
            if (!this._config.GenerateStateStatusOptionSet)
            {
                return first;
            }

            var stateAttr = _entityMetadata.Attributes.OfType<StateAttributeMetadata>().FirstOrDefault();
            var statusAttr = _entityMetadata.Attributes.OfType<StatusAttributeMetadata>().FirstOrDefault();

            if (stateAttr != null && statusAttr != null)
            {
                if (first) { first = false; } else { WriteLine(); }

                WriteLine($"#region {Properties.CodeGenerationStrings.StateAndStatusOptionSets}");

                await GenerateStateOptionSet(stateAttr, statusAttr);

                await GenerateStatusOptionSet(statusAttr, stateAttr);

                WriteLine();
                WriteLine($"#endregion {Properties.CodeGenerationStrings.StateAndStatusOptionSets}");
            }

            return first;
        }

        private async Task GenerateStateOptionSet(StateAttributeMetadata stateAttr, StatusAttributeMetadata statusAttr)
        {
            WriteLine();

            string attributeNameInSchemaAttributes = GetAttributeNameInSchemaAttributes(statusAttr);

            var headers = new List<string>();

            if (_config.GenerateAttributes)
            {
                headers.Add(string.Format("Attribute: {0}{1}<see cref=\"Attributes.{2}\"/>", stateAttr.LogicalName, _tabSpacer, attributeNameInSchemaAttributes));
            }

            if (this._config.WithManagedInfo)
            {
                headers.Add(string.Format("IsManaged: {0}", stateAttr.OptionSet.IsManaged));
            }

            WriteSummary(stateAttr.DisplayName, stateAttr.Description, headers, null);

            if (_config.AddDescriptionAttribute)
            {
                string description = CreateFileHandler.GetLocalizedLabel(stateAttr.DisplayName);

                if (string.IsNullOrEmpty(description))
                {
                    description = CreateFileHandler.GetLocalizedLabel(stateAttr.Description);
                }

                if (!string.IsNullOrEmpty(description))
                {
                    WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(description));
                }
            }

            if (_config.OptionSetExportType == OptionSetExportType.Enums)
            {
                if (_config.AddTypeConverterAttributeForEnums && !string.IsNullOrEmpty(_config.TypeConverterName))
                {
                    WriteLine("[System.ComponentModel.TypeConverterAttribute({0})]", ToCSharpLiteral(_config.TypeConverterName));
                }

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

                if (_config.AddDescriptionAttribute)
                {
                    string description = CreateFileHandler.GetLocalizedLabel(item.Label);

                    if (string.IsNullOrEmpty(description))
                    {
                        description = CreateFileHandler.GetLocalizedLabel(item.Description);
                    }

                    if (!string.IsNullOrEmpty(description))
                    {
                        WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(description));
                    }
                }

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

        private async Task GenerateStatusOptionSet(StatusAttributeMetadata statusAttr, StateAttributeMetadata stateAttr)
        {
            WriteLine();

            string attributeNameInSchemaAttributes = GetAttributeNameInSchemaAttributes(statusAttr);

            var headers = new List<string>();

            if (_config.GenerateAttributes)
            {
                headers.Add(string.Format("Attribute: {0}{1}<see cref=\"Attributes.{2}\"/>", statusAttr.LogicalName, _tabSpacer, attributeNameInSchemaAttributes));
            }

            if (this._config.WithManagedInfo)
            {
                headers.Add(string.Format("IsManaged: {0}", statusAttr.OptionSet.IsManaged));
            }
            headers.Add("Value Format: Statecode_Statuscode");

            WriteSummary(stateAttr.DisplayName, stateAttr.Description, headers, null);

            if (_config.AddDescriptionAttribute)
            {
                string description = CreateFileHandler.GetLocalizedLabel(statusAttr.DisplayName);

                if (string.IsNullOrEmpty(description))
                {
                    description = CreateFileHandler.GetLocalizedLabel(statusAttr.Description);
                }

                if (!string.IsNullOrEmpty(description))
                {
                    WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(description));
                }
            }

            if (_config.OptionSetExportType == OptionSetExportType.Enums)
            {
                if (_config.AddTypeConverterAttributeForEnums && !string.IsNullOrEmpty(_config.TypeConverterName))
                {
                    WriteLine("[System.ComponentModel.TypeConverterAttribute({0})]", ToCSharpLiteral(_config.TypeConverterName));
                }

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

                if (_config.AddDescriptionAttribute)
                {
                    string description = CreateFileHandler.GetLocalizedLabel(item.Label);

                    if (string.IsNullOrEmpty(description))
                    {
                        description = CreateFileHandler.GetLocalizedLabel(item.Description);
                    }

                    if (!string.IsNullOrEmpty(description))
                    {
                        WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(description));
                    }
                }

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

        private string GetAttributeNameInSchemaAttributes(AttributeMetadata attributeMetadata)
        {
            return GetAttributeNameInProxy(attributeMetadata).ToLower();
        }

        private string GetAttributeNameInProxy(AttributeMetadata attributeMetadata)
        {
            var attributeName = _iCodeGenerationServiceProvider.NamingService.GetNameForAttribute(_entityMetadata, attributeMetadata);

            using (CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                if (!provider.IsValidIdentifier(attributeName))
                {
                    attributeName = "@" + attributeName;
                }
            }

            return attributeName;
        }

        private async Task GenerateOptionSetEnums(IEnumerable<AttributeMetadata> attributeList, OptionSetMetadata optionSet)
        {
            List<string> lines = new List<string>
            {
                "Attribute:"
            };

            if (_config.GenerateAttributes)
            {
                foreach (var attr in attributeList.OrderBy(a => a.LogicalName))
                {
                    string attributeName = GetAttributeNameInSchemaAttributes(attr);

                    var seeLink = string.Format("{0}<see cref=\"Attributes.{1}\"/>", _tabSpacer, attributeName);

                    lines.Add(seeLink);
                }
            }

            if (optionSet.IsGlobal.GetValueOrDefault())
            {
                CreateFileHandler.FillLabelDisplayNameAndDescription(lines, _config.AllDescriptions, optionSet.DisplayName, optionSet.Description, _config.TabSpacer);
            }
            else
            {
                CreateFileHandler.FillLabelDisplayNameAndDescription(lines, _config.AllDescriptions, attributeList.First().DisplayName, attributeList.First().Description, _config.TabSpacer);
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
                foreach (var attr in attributeList.OrderBy(a => a.LogicalName))
                {
                    CreateFileHandler.FillLabelDisplayNameAndDescription(lines, _config.AllDescriptions, attr.DisplayName, attr.Description, _config.TabSpacer);
                }
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

            var options = GetOptionItems(attributeList.First().EntityLogicalName, attributeList.First().LogicalName, optionSet, await this._listStringMap);

            if (!options.Any())
            {
                return;
            }

            WriteSummaryStrings(lines);

            {
                bool ignore = attributeList.Any(a => IgnoreAttribute(_entityMetadata.LogicalName, a.LogicalName));

                if (!ignore)
                {
                    if (_config.AddDescriptionAttribute)
                    {
                        string description = CreateFileHandler.GetLocalizedLabel(optionSet.DisplayName);

                        if (string.IsNullOrEmpty(description))
                        {
                            description = CreateFileHandler.GetLocalizedLabel(optionSet.Description);
                        }

                        if (!string.IsNullOrEmpty(description))
                        {
                            WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(description));
                        }
                    }

                    if (_config.OptionSetExportType == OptionSetExportType.Enums && _config.AddTypeConverterAttributeForEnums && !string.IsNullOrEmpty(_config.TypeConverterName))
                    {
                        WriteLine("[System.ComponentModel.TypeConverterAttribute({0})]", ToCSharpLiteral(_config.TypeConverterName));
                    }
                }

                StringBuilder str = new StringBuilder();

                if (ignore)
                {
                    str.Append("// ");
                }

                var enumName = string.Empty;

                if (optionSet.IsGlobal.GetValueOrDefault())
                {
                    enumName = optionSet.Name;
                }
                else
                {
                    enumName = attributeList.First().LogicalName;
                }

                if (_config.OptionSetExportType == OptionSetExportType.Enums)
                {
                    str.AppendFormat("public enum {0}", enumName);
                }
                else
                {
                    str.AppendFormat("public static partial class {0}", enumName);
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

                if (_config.AddDescriptionAttribute)
                {
                    string description = CreateFileHandler.GetLocalizedLabel(item.Label);

                    if (string.IsNullOrEmpty(description))
                    {
                        description = CreateFileHandler.GetLocalizedLabel(item.Description);
                    }

                    if (!string.IsNullOrEmpty(description))
                    {
                        WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(description));
                    }
                }

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

            string regionName = string.Format(Properties.CodeGenerationStrings.OneToManyRelationshipsFormat2, className, relationTypeName);

            WriteLine();
            WriteLine("#region {0}", regionName);

            WriteLine();
            WriteLine("public static partial class {0}", className);
            WriteLine("{");

            bool first = true;
            foreach (var relationship in relationshipColl.OrderBy(r => r.SchemaName))
            {
                if (first) { first = false; } else { WriteLine(); }

                string relationDescription = string.Format("{0} - Relationship {1}", relationTypeName, relationship.SchemaName);

                List<string> lines = new List<string>
                {
                    relationDescription
                };

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

                    if (!string.Equals(this._entityMetadata.LogicalName, relationship.ReferencedEntity, StringComparison.InvariantCultureIgnoreCase))
                    {
                        nameField = "ReferencedEntity";

                        await this._taskDownloadMetadata;

                        entityMetadata = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.ReferencedEntity);
                    }
                    else if (!string.Equals(this._entityMetadata.LogicalName, relationship.ReferencingEntity, StringComparison.InvariantCultureIgnoreCase))
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

                            lines.Add(string.Format("{0} {1}:", nameField, entityMetadata.LogicalName)
                                + _config.TabSpacer
                                + string.Format("PrimaryIdAttribute {0}", entityMetadata.PrimaryIdAttribute)
                                + (!string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute) ? _config.TabSpacer + string.Format("PrimaryNameAttribute {0}", entityMetadata.PrimaryNameAttribute) : string.Empty)
                            );

                            lineEntityDescription.ForEach(s => lines.Add(_config.TabSpacer + s));
                        }
                    }
                }

                {
                    var attributeMaps = (await _listAttributeMap).Where(a =>
                        string.Equals(a.EntityMapIdSourceEntityName, relationship.ReferencedEntity, StringComparison.InvariantCultureIgnoreCase)
                        && string.Equals(a.EntityMapIdTargetEntityName, relationship.ReferencingEntity, StringComparison.InvariantCultureIgnoreCase)
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

                if (_config.AddDescriptionAttribute)
                {
                    if (!string.IsNullOrEmpty(relationDescription))
                    {
                        WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(relationDescription));
                    }
                }

                WriteLine("public static partial class {0}", relationship.SchemaName.ToLower());
                WriteLine("{");

                WriteLine("public {0} string Name = \"{1}\";", _fieldHeader, relationship.SchemaName);

                WriteLine();
                WriteLine("public {0} string ReferencedEntity_{1} = \"{1}\";", _fieldHeader, relationship.ReferencedEntity);

                WriteLine();
                WriteLine("public {0} string ReferencedAttribute_{1} = \"{1}\";", _fieldHeader, relationship.ReferencedAttribute);

                if (!string.Equals(this._entityMetadata.LogicalName, relationship.ReferencedEntity, StringComparison.InvariantCultureIgnoreCase))
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

                if (!string.Equals(this._entityMetadata.LogicalName, relationship.ReferencingEntity, StringComparison.InvariantCultureIgnoreCase))
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
            WriteLine("#endregion {0}", regionName);
        }

        private List<string> GetRelationshipMetadataOneToManyDescription(OneToManyRelationshipMetadata relationship)
        {
            // ^([^\r\n]*)
            // table.AddEntityMetadataString("$1", _entityMetadata.$1);

            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("PropertyName", "Value");

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
                table.AddEntityMetadataString("CascadeConfiguration.RollupView", relationship.CascadeConfiguration.RollupView);
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
                        .OrderBy(s => s.LanguageCode, LocaleComparer.Comparer)
                        )
                    {
                        table.AddEntityMetadataString("AssociatedMenuConfiguration.Label", string.Format("{0}: {1}", LanguageLocale.GetLocaleName(label.LanguageCode), label.Label));
                    }
                }
            }

            List<string> result = table.GetFormatedLines(false);

            return result;
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
            WriteLine($"#region {Properties.CodeGenerationStrings.ManyToManyRelationships}");

            WriteLine();
            WriteLine("public static partial class ManyToMany");
            WriteLine("{");

            bool first = true;

            foreach (var relationship in relationshipColl.OrderBy(r => r.SchemaName))
            {
                if (first) { first = false; } else { WriteLine(); }

                string relationDescription = string.Format("N:N - Relationship {0}", relationship.SchemaName);

                List<string> lines = new List<string>
                {
                    relationDescription
                };

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

                    if (!string.Equals(this._entityMetadata.LogicalName, relationship.Entity1LogicalName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        nameField = "Entity1LogicalName";

                        await this._taskDownloadMetadata;

                        entityMetadata = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.Entity1LogicalName);
                    }
                    else if (!string.Equals(this._entityMetadata.LogicalName, relationship.Entity2LogicalName, StringComparison.InvariantCultureIgnoreCase))
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

                            lines.Add(string.Format("{0} {1}:", nameField, entityMetadata.LogicalName)
                                + _config.TabSpacer
                                + string.Format("PrimaryIdAttribute {0}", entityMetadata.PrimaryIdAttribute)
                                + (!string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute) ? _config.TabSpacer + string.Format("PrimaryNameAttribute {0}", entityMetadata.PrimaryNameAttribute) : string.Empty)
                            );
                            lineEntityDescription.ForEach(s => lines.Add(_config.TabSpacer + s));
                        }
                    }
                }

                //{
                //    var attributeMaps = _listAttributeMap.Where(a =>
                //        string.Equals(a.EntityMapIdSourceEntityName, relationship.Entity1LogicalName, StringComparison.InvariantCultureIgnoreCase)
                //        && string.Equals(a.EntityMapIdTargetEntityName, relationship.Entity2LogicalName, StringComparison.InvariantCultureIgnoreCase)
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

                if (_config.AddDescriptionAttribute)
                {
                    if (!string.IsNullOrEmpty(relationDescription))
                    {
                        WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(relationDescription));
                    }
                }

                WriteLine("public static partial class {0}", relationship.SchemaName.ToLower());
                WriteLine("{");

                WriteLine("public {0} string Name = \"{1}\";", _fieldHeader, relationship.SchemaName);

                WriteLine();
                WriteLine("public {0} string IntersectEntity_{1} = \"{1}\";", _fieldHeader, relationship.IntersectEntityName);

                WriteLine();
                WriteLine("public {0} string Entity1_{1} = \"{1}\";", _fieldHeader, relationship.Entity1LogicalName);

                WriteLine();
                WriteLine("public {0} string Entity1Attribute_{1} = \"{1}\";", _fieldHeader, relationship.Entity1IntersectAttribute);

                if (!string.Equals(this._entityMetadata.LogicalName, relationship.Entity1LogicalName, StringComparison.InvariantCultureIgnoreCase))
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

                if (!string.Equals(this._entityMetadata.LogicalName, relationship.Entity2LogicalName, StringComparison.InvariantCultureIgnoreCase))
                {
                    await this._taskDownloadMetadata;

                    var entity2LogicalName = _solutionComponentDescriptor.MetadataSource.GetEntityMetadata(relationship.Entity2LogicalName);

                    if (!string.IsNullOrEmpty(entity2LogicalName.PrimaryNameAttribute))
                    {
                        WriteLine();
                        WriteLine("public {0} string Entity2LogicalName_PrimaryNameAttribute_{1} = \"{1}\";", _fieldHeader, entity2LogicalName.PrimaryNameAttribute);
                    }
                }

                WriteLine("}");
            }

            WriteLine("}");

            WriteLine();
            WriteLine($"#endregion {Properties.CodeGenerationStrings.ManyToManyRelationships}");
        }

        private List<string> GetRelationshipMetadataManyToManyDescription(ManyToManyRelationshipMetadata relationship)
        {
            // ^([^\r\n]*)
            // table.AddEntityMetadataString("$1", _entityMetadata.$1);

            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("PropertyName", "Value");

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
                        .OrderBy(s => s.LanguageCode, LocaleComparer.Comparer)
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
                        .OrderBy(s => s.LanguageCode, LocaleComparer.Comparer)
                        )
                    {
                        table.AddEntityMetadataString("Entity2AssociatedMenuConfiguration.Label", string.Format("{0}: {1}", LanguageLocale.GetLocaleName(label.LanguageCode), label.Label));
                    }
                }
            }

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
            WriteLine($"#region {Properties.CodeGenerationStrings.EntityKeys}");

            WriteLine();
            WriteLine("public static partial class EntityKeys");
            WriteLine("{");

            bool first = true;
            foreach (var key in keysColl.OrderBy(r => r.LogicalName))
            {
                if (first) { first = false; } else { WriteLine(); }

                List<string> lines = new List<string>();

                CreateFileHandler.FillLabelDisplayNameAndDescription(lines, _config.AllDescriptions, key.DisplayName, new Label(), _config.TabSpacer);

                // ^([^\r\n]*)
                // table.AddEntityMetadataString("$1", _entityMetadata.$1);

                FormatTextTableHandler table = new FormatTextTableHandler();
                table.SetHeader("PropertyName", "Value");

                table.AddEntityMetadataString("SchemaName", key.SchemaName);
                if (this._config.WithManagedInfo)
                {
                    table.AddEntityMetadataString("IsManaged", key.IsManaged);
                }
                table.AddEntityMetadataString("IsCustomizable", key.IsCustomizable);
                table.AddEntityMetadataString("IntroducedVersion", key.IntroducedVersion);

                lines.Add(string.Empty);
                lines.AddRange(table.GetFormatedLines(false));

                WriteSummaryStrings(lines);

                if (_config.AddDescriptionAttribute)
                {
                    string description = CreateFileHandler.GetLocalizedLabel(key.DisplayName);

                    if (!string.IsNullOrEmpty(description))
                    {
                        WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(description));
                    }
                }

                WriteLine("public static partial class {0}", key.LogicalName.ToLower());
                WriteLine("{");

                WriteLine("public {0} string Name = \"{1}\";", _fieldHeader, key.LogicalName);

                WriteLine();
                WriteLine("public static readonly System.Collections.ObjectModel.ReadOnlyCollection<string> KeyAttributes = new System.Collections.ObjectModel.ReadOnlyCollection<string>(new string[] { " + string.Join(", ", key.KeyAttributes.OrderBy(s => s).Select(s => "\"" + s + "\"")) + " });");

                WriteLine("}");
            }

            WriteLine("}");

            WriteLine();
            WriteLine($"#endregion {Properties.CodeGenerationStrings.EntityKeys}");
        }

        public static string CreateFileNameForSchema(ConnectionData connectionData, string entitySchemaName, bool withoutConnectionName)
        {
            string fileName = string.Format("{0}.Schema.cs", entitySchemaName);

            if (!withoutConnectionName)
            {
                fileName = string.Format("{0}.{1}", connectionData.Name, fileName);
            }

            return fileName;
        }

        public static string CreateFileNameForProxy(ConnectionData connectionData, string entitySchemaName, bool withoutConnectionName)
        {
            string fileName = string.Format("{0}.Proxy.cs", entitySchemaName);

            if (!withoutConnectionName)
            {
                fileName = string.Format("{0}.{1}", connectionData.Name, fileName);
            }

            return fileName;
        }

        public static List<string> GetAttributeDescription(
            AttributeMetadata attrib
            , bool allDescription
            , bool withManagedInfo
            , SolutionComponentDescriptor descriptor
            , string tabSpacer = _defaultTabSpacer
            , string proxyClassAttributeSeeLink = null
            , string optionSetSeeLink = null
        )
        {
            List<string> result = new List<string>();

            AddStringIntoList(result, tabSpacer, string.Format("SchemaName: {0}", attrib.SchemaName));

            {
                var list = new List<string>();

                string attributeTypeName = string.Empty;
                string attrType = string.Empty;
                string requiredLevel = string.Empty;
                string attributeOf = string.Empty;

                if (attrib.AttributeType != null)
                {
                    attrType = string.Format("AttributeType: {0}", attrib.AttributeType);
                }

                if (attrib.AttributeTypeName != null && !string.IsNullOrEmpty(attrib.AttributeTypeName.Value))
                {
                    attributeTypeName = string.Format("AttributeTypeName: {0}", attrib.AttributeTypeName.Value);
                }

                if (attrib.RequiredLevel != null)
                {
                    requiredLevel = string.Format("RequiredLevel: {0}", attrib.RequiredLevel.Value);
                }

                if (!string.IsNullOrEmpty(attrib.AttributeOf))
                {
                    attributeOf = string.Format("AttributeOf '{0}'", attrib.AttributeOf);
                }

                string attributeType = attrib.GetType().Name;

                list.AddRange(new[] { attributeType, attrType, attributeTypeName, requiredLevel, attributeOf });

                if (withManagedInfo)
                {
                    string isManaged = string.Format("IsManaged {0}", attrib.IsManaged.ToString());

                    list.Add(isManaged);
                }

                AddStringIntoList(result, tabSpacer, list);
            }

            if (!string.IsNullOrEmpty(proxyClassAttributeSeeLink))
            {
                AddStringIntoList(result, tabSpacer, proxyClassAttributeSeeLink);
            }

            {
                var isValidForCreate = string.Format("IsValidForCreate: {0}", attrib.IsValidForCreate.GetValueOrDefault());
                var isValidForRead = string.Format("IsValidForRead: {0}", attrib.IsValidForRead.GetValueOrDefault());
                var isValidForUpdate = string.Format("IsValidForUpdate: {0}", attrib.IsValidForUpdate.GetValueOrDefault());

                var isValidForAdvancedFind = string.Format("IsValidForAdvancedFind: {0}", attrib.IsValidForAdvancedFind.Value);

                AddStringIntoList(result, tabSpacer, isValidForCreate, isValidForUpdate);
                AddStringIntoList(result, tabSpacer, isValidForRead, isValidForAdvancedFind);
            }

            {
                var isLogical = string.Format("IsLogical: {0}", attrib.IsLogical.GetValueOrDefault());
                var isSecured = string.Format("IsSecured: {0}", attrib.IsSecured.GetValueOrDefault());
                var isCustomAttribute = string.Format("IsCustomAttribute: {0}", attrib.IsCustomAttribute.GetValueOrDefault());
                var sourceType = string.Empty;

                if (attrib.SourceType == null)
                {
                    sourceType = "Simple";
                }
                else if (attrib.SourceType == 1)
                {
                    sourceType = "Calculated";
                }
                else if (attrib.SourceType == 2)
                {
                    sourceType = "Rollup";
                }
                else
                {
                    sourceType = attrib.SourceType.ToString();
                }

                sourceType = string.Format("SourceType: {0}", sourceType);

                AddStringIntoList(result, tabSpacer, isLogical, isSecured, isCustomAttribute, sourceType);
            }

            if (attrib is MemoAttributeMetadata memoAttrib)
            {
                string maxLength = string.Format("MaxLength = {0}", memoAttrib.MaxLength.HasValue ? memoAttrib.MaxLength.ToString() : "Null");
                string format = string.Format("Format = {0}", memoAttrib.Format.HasValue ? memoAttrib.Format.ToString() : "Null");
                string imeMode = string.Format("ImeMode = {0}", memoAttrib.ImeMode.HasValue ? memoAttrib.ImeMode.ToString() : "Null");
                string isLocalizable = string.Format("IsLocalizable = {0}", memoAttrib.IsLocalizable.HasValue ? memoAttrib.IsLocalizable.ToString() : "Null");

                AddStringIntoList(result, tabSpacer, maxLength);
                AddStringIntoList(result, tabSpacer, format, imeMode, isLocalizable);
            }

            if (attrib is StringAttributeMetadata stringAttrib)
            {
                string maxLength = string.Format("MaxLength = {0}", stringAttrib.MaxLength.HasValue ? stringAttrib.MaxLength.ToString() : "Null");
                string format = string.Format("Format = {0}", stringAttrib.Format.HasValue ? stringAttrib.Format.ToString() : "Null");
                string imeMode = string.Format("ImeMode = {0}", stringAttrib.ImeMode.HasValue ? stringAttrib.ImeMode.ToString() : "Null");
                string isLocalizable = string.Format("IsLocalizable = {0}", stringAttrib.IsLocalizable.HasValue ? stringAttrib.IsLocalizable.ToString() : "Null");

                string formulaDefinition = string.Empty;

                if (!string.IsNullOrEmpty(stringAttrib.FormulaDefinition))
                {
                    formulaDefinition = "FormulaDefinition is not null";
                }

                AddStringIntoList(result, tabSpacer, maxLength);
                AddStringIntoList(result, tabSpacer, format, imeMode, isLocalizable, formulaDefinition);
            }

            if (attrib is IntegerAttributeMetadata intAttrib)
            {
                string minValue = string.Format("MinValue = {0}", intAttrib.MinValue.HasValue ? intAttrib.MinValue.ToString() : "Null");
                string maxValue = string.Format("MaxValue = {0}", intAttrib.MaxValue.HasValue ? intAttrib.MaxValue.ToString() : "Null");

                string format = string.Format("Format = {0}", intAttrib.Format.HasValue ? intAttrib.Format.ToString() : "Null");

                string formulaDefinition = string.Empty;

                if (!string.IsNullOrEmpty(intAttrib.FormulaDefinition))
                {
                    formulaDefinition = "FormulaDefinition is not null";
                }

                AddStringIntoList(result, tabSpacer, minValue, maxValue);
                AddStringIntoList(result, tabSpacer, format, formulaDefinition);
            }

            if (attrib is BigIntAttributeMetadata bigIntAttrib)
            {
                string minValue = string.Format("MinValue = {0}", bigIntAttrib.MinValue.HasValue ? bigIntAttrib.MinValue.ToString() : "Null");
                string maxValue = string.Format("MaxValue = {0}", bigIntAttrib.MaxValue.HasValue ? bigIntAttrib.MaxValue.ToString() : "Null");

                AddStringIntoList(result, tabSpacer, minValue, maxValue);
            }

            if (attrib is ImageAttributeMetadata imageAttrib)
            {
                string isPrimaryImage = string.Format("IsPrimaryImage = {0}", imageAttrib.IsPrimaryImage.HasValue ? imageAttrib.IsPrimaryImage.ToString() : "Null");
                string maxHeight = string.Format("MaxHeight = {0}", imageAttrib.MaxHeight.HasValue ? imageAttrib.MaxHeight.ToString() : "Null");
                string maxValue = string.Format("MaxWidth = {0}", imageAttrib.MaxWidth.HasValue ? imageAttrib.MaxWidth.ToString() : "Null");

                AddStringIntoList(result, tabSpacer, isPrimaryImage, maxHeight, maxValue);
            }

            if (attrib is MoneyAttributeMetadata moneyAttrib)
            {
                string minValue = string.Format("MinValue = {0}", moneyAttrib.MinValue.HasValue ? moneyAttrib.MinValue.ToString() : "Null");
                string maxValue = string.Format("MaxValue = {0}", moneyAttrib.MaxValue.HasValue ? moneyAttrib.MaxValue.ToString() : "Null");

                string precision = string.Format("Precision = {0}", moneyAttrib.Precision.HasValue ? moneyAttrib.Precision.ToString() : "Null");
                string precisionSource = string.Format("PrecisionSource = {0}", moneyAttrib.PrecisionSource.HasValue ? moneyAttrib.PrecisionSource.ToString() : "Null");

                string isBaseCurrency = string.Format("IsBaseCurrency = {0}", moneyAttrib.IsBaseCurrency.HasValue ? moneyAttrib.IsBaseCurrency.ToString() : "Null");

                string imeMode = string.Format("ImeMode = {0}", moneyAttrib.ImeMode.HasValue ? moneyAttrib.ImeMode.ToString() : "Null");

                string formulaDefinition = string.Empty;
                string calculationOf = string.Empty;

                if (!string.IsNullOrEmpty(moneyAttrib.FormulaDefinition))
                {
                    formulaDefinition = "FormulaDefinition is not null";
                }

                if (!string.IsNullOrEmpty(moneyAttrib.CalculationOf))
                {
                    calculationOf = string.Format("CalculationOf = {0}", moneyAttrib.CalculationOf);
                }

                AddStringIntoList(result, tabSpacer, minValue, maxValue, precision, precisionSource);
                AddStringIntoList(result, tabSpacer, isBaseCurrency);
                AddStringIntoList(result, tabSpacer, imeMode, formulaDefinition, calculationOf);
            }

            if (attrib is DecimalAttributeMetadata decimalAttrib)
            {
                string minValue = string.Format("MinValue = {0}", decimalAttrib.MinValue.HasValue ? decimalAttrib.MinValue.ToString() : "Null");
                string maxValue = string.Format("MaxValue = {0}", decimalAttrib.MaxValue.HasValue ? decimalAttrib.MaxValue.ToString() : "Null");

                string precision = string.Format("Precision = {0}", decimalAttrib.Precision.HasValue ? decimalAttrib.Precision.ToString() : "Null");

                string imeMode = string.Format("ImeMode = {0}", decimalAttrib.ImeMode.HasValue ? decimalAttrib.ImeMode.ToString() : "Null");

                string formulaDefinition = string.Empty;

                if (!string.IsNullOrEmpty(decimalAttrib.FormulaDefinition))
                {
                    formulaDefinition = "FormulaDefinition is not null";
                }

                AddStringIntoList(result, tabSpacer, minValue, maxValue, precision);
                AddStringIntoList(result, tabSpacer, imeMode, formulaDefinition);
            }

            if (attrib is DoubleAttributeMetadata doubleAttrib)
            {
                string minValue = string.Format("MinValue = {0}", doubleAttrib.MinValue.HasValue ? doubleAttrib.MinValue.ToString() : "Null");
                string maxValue = string.Format("MaxValue = {0}", doubleAttrib.MaxValue.HasValue ? doubleAttrib.MaxValue.ToString() : "Null");

                string precision = string.Format("Precision = {0}", doubleAttrib.Precision.HasValue ? doubleAttrib.Precision.ToString() : "Null");

                string imeMode = string.Format("ImeMode = {0}", doubleAttrib.ImeMode.HasValue ? doubleAttrib.ImeMode.ToString() : "Null");

                AddStringIntoList(result, tabSpacer, minValue, maxValue, precision);
                AddStringIntoList(result, tabSpacer, imeMode);
            }

            if (attrib is BooleanAttributeMetadata boolAttrib)
            {
                result.Add(string.Format("DefaultValue = {0}", boolAttrib.DefaultValue.HasValue ? boolAttrib.DefaultValue.ToString() : "Null"));

                if (boolAttrib.OptionSet.FalseOption.Label != null || boolAttrib.OptionSet.FalseOption.Description != null)
                {
                    CreateFileHandler.FillLabelDisplayNameAndDescription(result, allDescription, boolAttrib.OptionSet.FalseOption.Label, boolAttrib.OptionSet.FalseOption.Description, tabSpacer);
                    result.Add(string.Format("FalseOption = {0}", boolAttrib.OptionSet.FalseOption.Value.HasValue ? boolAttrib.OptionSet.FalseOption.Value.Value.ToString() : "Null"));
                }

                if (boolAttrib.OptionSet.TrueOption.Label != null || boolAttrib.OptionSet.TrueOption.Description != null)
                {
                    CreateFileHandler.FillLabelDisplayNameAndDescription(result, allDescription, boolAttrib.OptionSet.TrueOption.Label, boolAttrib.OptionSet.TrueOption.Description, tabSpacer);
                    result.Add(string.Format("TrueOption = {0}", boolAttrib.OptionSet.TrueOption.Value.HasValue ? boolAttrib.OptionSet.TrueOption.Value.Value.ToString() : "Null"));
                }

                if (!string.IsNullOrEmpty(boolAttrib.FormulaDefinition))
                {
                    result.Add("FormulaDefinition is not null");
                }
            }

            if (attrib is PicklistAttributeMetadata picklistAttrib)
            {
                var optionSetDescription = GetOptionSetDescription(withManagedInfo, picklistAttrib.OptionSet);
                AddStringIntoList(result, tabSpacer, optionSetDescription);

                if (!string.IsNullOrEmpty(optionSetSeeLink))
                {
                    AddStringIntoList(result, tabSpacer, optionSetSeeLink);
                }

                string defaultFormValue = string.Format("DefaultFormValue = {0}", picklistAttrib.DefaultFormValue.HasValue ? picklistAttrib.DefaultFormValue.ToString() : "Null");
                AddStringIntoList(result, tabSpacer, defaultFormValue);

                if (!string.IsNullOrEmpty(picklistAttrib.FormulaDefinition))
                {
                    AddStringIntoList(result, tabSpacer, "FormulaDefinition is not null");
                }

                if (picklistAttrib.OptionSet.IsGlobal.GetValueOrDefault())
                {
                    List<string> lineOptionSetDescription = new List<string>();

                    FillLabelDisplayNameAndDescription(lineOptionSetDescription, allDescription, picklistAttrib.OptionSet.DisplayName, picklistAttrib.OptionSet.Description, tabSpacer);

                    if (lineOptionSetDescription.Any())
                    {
                        if (result.Any())
                        {
                            result.Add(string.Empty);
                        }

                        lineOptionSetDescription.ForEach(s => result.Add(tabSpacer + tabSpacer + s));
                    }
                }
            }

            if (attrib is MultiSelectPicklistAttributeMetadata multiSelectPicklistAttrib)
            {
                var optionSetDescription = GetOptionSetDescription(withManagedInfo, multiSelectPicklistAttrib.OptionSet);
                AddStringIntoList(result, tabSpacer, optionSetDescription);

                if (!string.IsNullOrEmpty(optionSetSeeLink))
                {
                    AddStringIntoList(result, tabSpacer, optionSetSeeLink);
                }

                string defaultFormValue = string.Format("DefaultFormValue = {0}", multiSelectPicklistAttrib.DefaultFormValue.HasValue ? multiSelectPicklistAttrib.DefaultFormValue.ToString() : "Null");
                AddStringIntoList(result, tabSpacer, defaultFormValue);

                if (!string.IsNullOrEmpty(multiSelectPicklistAttrib.FormulaDefinition))
                {
                    AddStringIntoList(result, tabSpacer, "FormulaDefinition is not null");
                }

                if (multiSelectPicklistAttrib.OptionSet.IsGlobal.GetValueOrDefault())
                {
                    List<string> lineOptionSetDescription = new List<string>();

                    FillLabelDisplayNameAndDescription(lineOptionSetDescription, allDescription, multiSelectPicklistAttrib.OptionSet.DisplayName, multiSelectPicklistAttrib.OptionSet.Description, tabSpacer);

                    if (lineOptionSetDescription.Any())
                    {
                        if (result.Any())
                        {
                            result.Add(string.Empty);
                        }

                        lineOptionSetDescription.ForEach(s => result.Add(tabSpacer + tabSpacer + s));
                    }
                }
            }

            if (attrib is StatusAttributeMetadata statusAttrib)
            {
                string defaultFormValue = string.Format("DefaultFormValue = {0}", statusAttrib.DefaultFormValue.HasValue ? statusAttrib.DefaultFormValue.ToString() : "Null");
                AddStringIntoList(result, tabSpacer, defaultFormValue);

                var optionSetDescription = GetOptionSetDescription(withManagedInfo, statusAttrib.OptionSet);
                AddStringIntoList(result, tabSpacer, optionSetDescription);

                if (!string.IsNullOrEmpty(optionSetSeeLink))
                {
                    AddStringIntoList(result, tabSpacer, optionSetSeeLink);
                }
            }

            if (attrib is StateAttributeMetadata stateAttrib)
            {
                string defaultFormValue = string.Format("DefaultFormValue = {0}", stateAttrib.DefaultFormValue.HasValue ? stateAttrib.DefaultFormValue.ToString() : "Null");
                AddStringIntoList(result, tabSpacer, defaultFormValue);

                var optionSetDescription = GetOptionSetDescription(withManagedInfo, stateAttrib.OptionSet);
                AddStringIntoList(result, tabSpacer, optionSetDescription);

                if (!string.IsNullOrEmpty(optionSetSeeLink))
                {
                    AddStringIntoList(result, tabSpacer, optionSetSeeLink);
                }
            }

            if (attrib is EntityNameAttributeMetadata entityNameAttrib)
            {
                if (entityNameAttrib.OptionSet != null)
                {
                    var optionSetDescription = GetOptionSetDescription(withManagedInfo, entityNameAttrib.OptionSet);

                    AddStringIntoList(result, tabSpacer, optionSetDescription);

                    string defaultFormValue = string.Format("DefaultFormValue = {0}", entityNameAttrib.DefaultFormValue.HasValue ? entityNameAttrib.DefaultFormValue.ToString() : "Null");

                    AddStringIntoList(result, tabSpacer, defaultFormValue);

                    if (entityNameAttrib.OptionSet.IsGlobal.GetValueOrDefault())
                    {
                        List<string> lineOptionSetDescription = new List<string>();

                        FillLabelDisplayNameAndDescription(lineOptionSetDescription, allDescription, entityNameAttrib.OptionSet.DisplayName, entityNameAttrib.OptionSet.Description, tabSpacer);

                        if (lineOptionSetDescription.Any())
                        {
                            if (result.Any())
                            {
                                result.Add(string.Empty);
                            }

                            lineOptionSetDescription.ForEach(s => result.Add(tabSpacer + tabSpacer + s));
                        }
                    }
                }
            }

            //{
            //    var uniqueIdentifierAttrib = (attrib as UniqueIdentifierAttributeMetadata);
            //    if (uniqueIdentifierAttrib != null)
            //    {

            //    }
            //}

            if (attrib is ManagedPropertyAttributeMetadata managedAttrib)
            {
                string managedPropertyLogicalName = string.Format("ManagedPropertyLogicalName = {0}", managedAttrib.ManagedPropertyLogicalName);
                string valueAttributeTypeCode = string.Format("ValueAttributeTypeCode = {0}", managedAttrib.ValueAttributeTypeCode);

                string parentAttributeName = string.Empty;
                string parentComponentType = string.Empty;
                string parentComponentTypeName = string.Empty;

                if (!string.IsNullOrEmpty(managedAttrib.ParentAttributeName))
                {
                    parentAttributeName = string.Format("ParentAttributeName = {0}", managedAttrib.ParentAttributeName);
                }

                if (managedAttrib.ParentComponentType.HasValue && managedAttrib.ParentComponentType != ManagedPropertyAttributeMetadata.EmptyParentComponentType)
                {
                    parentComponentType = string.Format("ParentComponentType = {0}", managedAttrib.ParentComponentType.ToString());
                    parentComponentTypeName = string.Format("ParentComponentTypeName = {0}", SolutionComponent.GetComponentTypeName(managedAttrib.ParentComponentType.GetValueOrDefault()));
                }

                AddStringIntoList(result, tabSpacer, managedPropertyLogicalName, valueAttributeTypeCode);
                AddStringIntoList(result, tabSpacer, parentAttributeName, parentComponentType, parentComponentTypeName);
            }

            if (attrib is LookupAttributeMetadata lookupAttrib)
            {
                if (lookupAttrib.Targets != null && lookupAttrib.Targets.Length > 0)
                {
                    string targets = string.Format("Targets: {0}", string.Join(",", lookupAttrib.Targets.OrderBy(s => s)));
                    AddStringIntoList(result, tabSpacer, targets);

                    if (lookupAttrib.Targets.Length <= 6)
                    {
                        foreach (var target in lookupAttrib.Targets)
                        {
                            var entityMetadata = descriptor.MetadataSource.GetEntityMetadata(target, new[] { "LogicalName", "DisplayName", "DisplayCollectionName", "Description", "PrimaryIdAttribute", "PrimaryNameAttribute" });

                            if (entityMetadata != null)
                            {
                                List<string> lineEntityDescription = new List<string>();

                                FillLabelEntity(lineEntityDescription, allDescription, entityMetadata.DisplayName, entityMetadata.DisplayCollectionName, entityMetadata.Description);

                                if (lineEntityDescription.Any())
                                {
                                    if (result.Any())
                                    {
                                        result.Add(string.Empty);
                                    }

                                    result.Add(tabSpacer
                                        + string.Format("Target {0}", target)
                                        + tabSpacer
                                        + string.Format("PrimaryIdAttribute {0}", entityMetadata.PrimaryIdAttribute)
                                        + (!string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute) ? tabSpacer + string.Format("PrimaryNameAttribute {0}", entityMetadata.PrimaryNameAttribute) : string.Empty)
                                    );

                                    lineEntityDescription.ForEach(s => result.Add(tabSpacer + tabSpacer + s));
                                }
                            }
                        }
                    }
                }
            }

            if (attrib is DateTimeAttributeMetadata dateTimeAttrib)
            {
                string dateTimeBehavior = string.Empty;
                string canChangeDateTimeBehavior = string.Empty;

                if (dateTimeAttrib.DateTimeBehavior != null)
                {
                    dateTimeBehavior = string.Format("DateTimeBehavior = {0}", dateTimeAttrib.DateTimeBehavior.Value);
                }

                if (dateTimeAttrib.CanChangeDateTimeBehavior != null)
                {
                    canChangeDateTimeBehavior = string.Format("CanChangeDateTimeBehavior = {0}", dateTimeAttrib.CanChangeDateTimeBehavior.Value);
                }

                string format = string.Format("Format = {0}", dateTimeAttrib.Format.HasValue ? dateTimeAttrib.Format.ToString() : "Null");
                string imeMode = string.Format("ImeMode = {0}", dateTimeAttrib.ImeMode.HasValue ? dateTimeAttrib.ImeMode.ToString() : "Null");

                string formulaDefinition = string.Empty;

                if (!string.IsNullOrEmpty(dateTimeAttrib.FormulaDefinition))
                {
                    formulaDefinition = "FormulaDefinition is not null";
                }

                AddStringIntoList(result, tabSpacer, dateTimeBehavior, canChangeDateTimeBehavior);

                AddStringIntoList(result, tabSpacer, imeMode, format, formulaDefinition);
            }

            return result;
        }

        private static string GetOptionSetDescription(bool withManagedInfo, OptionSetMetadata optionSet)
        {
            string managedStr = string.Empty;

            if (withManagedInfo)
            {
                managedStr = " " + (optionSet.IsManaged.GetValueOrDefault() ? "Managed" : "Unmanaged");
            }

            string result = string.Format("{0} {1} {2} OptionSet {3}"
                , optionSet.IsGlobal.GetValueOrDefault() ? "Global" : "Local"
                , optionSet.IsCustomOptionSet.GetValueOrDefault() ? "Custom" : "System"
                , managedStr
                , optionSet.Name
            );

            return result;
        }

        private string GetOptionSetSeeLink(string namespaceGlobalOptionSets, AttributeMetadata attributeMetadata)
        {
            string optionSetSeeLink = string.Empty;

            if (attributeMetadata is StateAttributeMetadata stateAttributeMetadata)
            {
                if (_config.GenerateStateStatusOptionSet)
                {
                    optionSetSeeLink = GetOptionSetSeeLink(stateAttributeMetadata.OptionSet.IsGlobal.GetValueOrDefault(), $"OptionSets.{stateAttributeMetadata.LogicalName}");
                }
            }
            else if (attributeMetadata is StatusAttributeMetadata statusAttributeMetadata)
            {
                if (_config.GenerateStateStatusOptionSet)
                {
                    optionSetSeeLink = GetOptionSetSeeLink(statusAttributeMetadata.OptionSet.IsGlobal.GetValueOrDefault(), $"OptionSets.{statusAttributeMetadata.LogicalName}");
                }
            }
            else if ((attributeMetadata is PicklistAttributeMetadata || attributeMetadata is MultiSelectPicklistAttributeMetadata)
                && attributeMetadata is EnumAttributeMetadata enumAttributeMetadata
            )
            {
                if (enumAttributeMetadata.OptionSet.IsGlobal.GetValueOrDefault())
                {
                    if (_config.GenerateGlobalOptionSet)
                    {
                        optionSetSeeLink = GetOptionSetSeeLink(enumAttributeMetadata.OptionSet.IsGlobal.GetValueOrDefault(), $"OptionSets.{enumAttributeMetadata.OptionSet.Name}");
                    }
                    else
                    {
                        string optionSetClassName = string.Empty;

                        if (!string.IsNullOrEmpty(_config.NamespaceGlobalOptionSets))
                        {
                            optionSetClassName = $"{_config.NamespaceGlobalOptionSets}.{enumAttributeMetadata.OptionSet.Name}";
                        }
                        else
                        {
                            optionSetClassName = enumAttributeMetadata.OptionSet.Name;
                        }

                        optionSetSeeLink = GetOptionSetSeeLink(enumAttributeMetadata.OptionSet.IsGlobal.GetValueOrDefault(), optionSetClassName);
                    }
                }
                else
                {
                    if (_config.GenerateLocalOptionSet)
                    {
                        optionSetSeeLink = GetOptionSetSeeLink(enumAttributeMetadata.OptionSet.IsGlobal.GetValueOrDefault(), $"OptionSets.{enumAttributeMetadata.LogicalName}");
                    }
                }
            }

            return optionSetSeeLink;
        }

        private static string GetOptionSetSeeLink(bool isGlobal, string optionSetClassName)
        {
            if (isGlobal)
            {
                return string.Format("Global OptionSet <see cref=\"{0}\"/>", optionSetClassName);
            }
            else
            {
                return string.Format("Local OptionSet <see cref=\"{0}\"/>", optionSetClassName);
            }
        }

        private static void AddStringIntoList(List<string> result, string separator, params string[] lines)
        {
            var filtered = lines.Where(s => !string.IsNullOrEmpty(s));

            if (filtered.Any())
            {
                result.Add(string.Join(separator, filtered));
            }
        }

        private static void AddStringIntoList(List<string> result, string separator, IEnumerable<string> lines)
        {
            var filtered = lines.Where(s => !string.IsNullOrEmpty(s));

            if (filtered.Any())
            {
                result.Add(string.Join(separator, filtered));
            }
        }
    }
}
