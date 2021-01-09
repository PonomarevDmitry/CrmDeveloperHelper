using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class OrganizationComparerSourceBySolutionImage : OrganizationComparerSource
    {
        private SolutionImage _solutionImage;

        public OrganizationComparerSourceBySolutionImage(ConnectionData connection1, ConnectionData connection2, SolutionImage solutionImage)
            : base(connection1, connection2)
        {
            this._solutionImage = solutionImage;
        }

        protected override Task<IEnumerable<Role>> GetRoleAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Role);

            if (!imageComponents.Any())
            {
                return Task.FromResult(Enumerable.Empty<Role>());
            }

            return Task.Run(() => GetRoles(service, imageComponents));
        }

        private static async Task<IEnumerable<Role>> GetRoles(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<Role>();
            }

            return await new RoleRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override async Task<IEnumerable<Workflow>> GetWorkflowAsync(IOrganizationServiceExtented service, ColumnSet columnSet)
        {
            List<Workflow> result = new List<Workflow>();

            var descriptor = new SolutionComponentDescriptor(service);
            var repository = new WorkflowRepository(service);

            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Workflow);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var tempList = await repository.GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), columnSet);

                    result.AddRange(tempList);
                }
            }

            var hashSet = new HashSet<Guid>(result.Select(c => c.Id));

            imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Entity);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var entities = solutionComponents
                        .Where(c => c.RootComponentBehaviorEnum.GetValueOrDefault(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0) == SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0
                            && c.ObjectId.HasValue)
                        .Select(e => descriptor.MetadataSource.GetEntityMetadata(e.ObjectId.Value))
                        .Where(e => e != null)
                        .Select(e => e.LogicalName)
                        .ToArray();

                    if (entities.Any())
                    {
                        var tempList = await repository.GetListForEntitiesAsync(entities, columnSet);

                        foreach (var item in tempList)
                        {
                            if (hashSet.Add(item.Id))
                            {
                                result.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        protected override Task<List<WebResource>> GetWebResourceAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.WebResource);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<WebResource>());
            }

            return Task.Run(() => GetWebResources(service, imageComponents));
        }

        private static async Task<List<WebResource>> GetWebResources(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<WebResource>();
            }

            return await new WebResourceRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override Task<List<ConnectionRole>> GetConnectionRoleAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.ConnectionRole);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<ConnectionRole>());
            }

            return Task.Run(() => GetConnectionRoles(service, imageComponents));
        }

        private static async Task<List<ConnectionRole>> GetConnectionRoles(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<ConnectionRole>();
            }

            return await new ConnectionRoleRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override Task<List<ContractTemplate>> GetContractTemplateAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.ContractTemplate);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<ContractTemplate>());
            }

            return Task.Run(() => GetContractTemplates(service, imageComponents));
        }

        private static async Task<List<ContractTemplate>> GetContractTemplates(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<ContractTemplate>();
            }

            return await new ContractTemplateRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override Task<List<KbArticleTemplate>> GetKbArticleTemplateAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.KbArticleTemplate);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<KbArticleTemplate>());
            }

            return Task.Run(() => GetKbArticleTemplate(service, imageComponents));
        }

        private static async Task<List<KbArticleTemplate>> GetKbArticleTemplate(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<KbArticleTemplate>();
            }

            return await new KbArticleTemplateRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override Task<List<MailMergeTemplate>> GetMailMergeTemplateAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.MailMergeTemplate);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<MailMergeTemplate>());
            }

            return Task.Run(() => GetMailMergeTemplate(service, imageComponents));
        }

        private static async Task<List<MailMergeTemplate>> GetMailMergeTemplate(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<MailMergeTemplate>();
            }

            return await new MailMergeTemplateRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override Task<List<Template>> GetTemplateAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.EmailTemplate);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<Template>());
            }

            return Task.Run(() => GetTemplate(service, imageComponents));
        }

        private static async Task<List<Template>> GetTemplate(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<Template>();
            }

            return await new TemplateRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override async Task<List<DisplayString>> GetDisplayStringAsync(IOrganizationServiceExtented service)
        {
            List<DisplayString> result = new List<DisplayString>();

            var descriptor = new SolutionComponentDescriptor(service);
            var repository = new DisplayStringRepository(service);

            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.DisplayString);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var tempList = await repository.GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);

                    result.AddRange(tempList);
                }
            }

            var hashSet = new HashSet<Guid>(result.Select(c => c.Id));

            imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Entity);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var entities = solutionComponents
                        .Where(c => c.RootComponentBehaviorEnum.GetValueOrDefault(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0) == SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0
                            && c.ObjectId.HasValue)
                        .Select(e => descriptor.MetadataSource.GetEntityMetadata(e.ObjectId.Value))
                        .Where(e => e != null)
                        .Select(e => e.LogicalName)
                        .ToArray();

                    if (entities.Any())
                    {
                        var tempList = await repository.GetListForEntitiesAsync(entities, ColumnSetInstances.AllColumns);

                        foreach (var item in tempList)
                        {
                            if (hashSet.Add(item.Id))
                            {
                                result.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        protected override Task<List<FieldSecurityProfile>> GetFieldSecurityProfileAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.FieldSecurityProfile);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<FieldSecurityProfile>());
            }

            return Task.Run(() => GetFieldSecurityProfile(service, imageComponents));
        }

        private static async Task<List<FieldSecurityProfile>> GetFieldSecurityProfile(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<FieldSecurityProfile>();
            }

            return await new FieldSecurityProfileRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override async Task<List<EntityMap>> GetEntityMapAsync(IOrganizationServiceExtented service)
        {
            List<EntityMap> result = new List<EntityMap>();

            var descriptor = new SolutionComponentDescriptor(service);
            var repository = new EntityMapRepository(service);

            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.EntityMap);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var tempList = await repository.GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);

                    result.AddRange(tempList);
                }
            }

            var hashSet = new HashSet<Guid>(result.Select(c => c.Id));

            imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Entity);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var entities = solutionComponents
                        .Where(c => c.RootComponentBehaviorEnum.GetValueOrDefault(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0) == SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0
                            && c.ObjectId.HasValue)
                        .Select(e => descriptor.MetadataSource.GetEntityMetadata(e.ObjectId.Value))
                        .Where(e => e != null)
                        .Select(e => e.LogicalName)
                        .ToArray();

                    if (entities.Any())
                    {
                        var tempList = await repository.GetListForEntitiesAsync(entities, ColumnSetInstances.AllColumns);

                        foreach (var item in tempList)
                        {
                            if (hashSet.Add(item.Id))
                            {
                                result.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        protected override Task<List<PluginAssembly>> GetPluginAssemblyAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.PluginAssembly);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<PluginAssembly>());
            }

            return Task.Run(() => GetPluginAssembly(service, imageComponents));
        }

        private static async Task<List<PluginAssembly>> GetPluginAssembly(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<PluginAssembly>();
            }

            return await new PluginAssemblyRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), null);
        }

        protected override Task<List<PluginType>> GetPluginTypeAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.PluginType);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<PluginType>());
            }

            return Task.Run(() => GetPluginType(service, imageComponents));
        }

        private static async Task<List<PluginType>> GetPluginType(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<PluginType>();
            }

            return await new PluginTypeRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override Task<List<Report>> GetReportAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Report);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<Report>());
            }

            return Task.Run(() => GetReport(service, imageComponents));
        }

        private static async Task<List<Report>> GetReport(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<Report>();
            }

            return await new ReportRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override Task<List<SdkMessageProcessingStep>> GetSdkMessageProcessingStepAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.SdkMessageProcessingStep);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<SdkMessageProcessingStep>());
            }

            return Task.Run(() => GetSdkMessageProcessingStep(service, imageComponents));
        }

        private static async Task<List<SdkMessageProcessingStep>> GetSdkMessageProcessingStep(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<SdkMessageProcessingStep>();
            }

            return await new SdkMessageProcessingStepRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override Task<List<SiteMap>> GetSiteMapAsync(IOrganizationServiceExtented service, ColumnSet columnSet = null)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.SiteMap);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<SiteMap>());
            }

            return Task.Run(() => GetSiteMap(service, imageComponents));
        }

        private static async Task<List<SiteMap>> GetSiteMap(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<SiteMap>();
            }

            return await new SiteMapRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);
        }

        protected override async Task<List<SavedQuery>> GetSavedQueryAsync(IOrganizationServiceExtented service)
        {
            List<SavedQuery> result = new List<SavedQuery>();

            var descriptor = new SolutionComponentDescriptor(service);
            var repository = new SavedQueryRepository(service);

            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.SavedQuery);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var tempList = await repository.GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);

                    result.AddRange(tempList);
                }
            }

            var hashSet = new HashSet<Guid>(result.Select(c => c.Id));

            imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Entity);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var entities = solutionComponents
                        .Where(c => c.RootComponentBehaviorEnum.GetValueOrDefault(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0) == (int)SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0
                            && c.ObjectId.HasValue)
                        .Select(e => descriptor.MetadataSource.GetEntityMetadata(e.ObjectId.Value))
                        .Where(e => e != null)
                        .Select(e => e.LogicalName)
                        .ToArray();

                    if (entities.Any())
                    {
                        var tempList = await repository.GetListForEntitiesAsync(entities, ColumnSetInstances.AllColumns);

                        foreach (var item in tempList)
                        {
                            if (hashSet.Add(item.Id))
                            {
                                result.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        protected override async Task<List<SavedQueryVisualization>> GetSavedQueryVisualizationAsync(IOrganizationServiceExtented service)
        {
            List<SavedQueryVisualization> result = new List<SavedQueryVisualization>();

            var descriptor = new SolutionComponentDescriptor(service);
            var repository = new SavedQueryVisualizationRepository(service);

            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.SavedQueryVisualization);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var tempList = await repository.GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);

                    result.AddRange(tempList);
                }
            }

            var hashSet = new HashSet<Guid>(result.Select(c => c.Id));

            imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Entity);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var entities = solutionComponents
                        .Where(c => c.RootComponentBehaviorEnum.GetValueOrDefault(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0) == SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0
                            && c.ObjectId.HasValue)
                        .Select(e => descriptor.MetadataSource.GetEntityMetadata(e.ObjectId.Value))
                        .Where(e => e != null)
                        .Select(e => e.LogicalName)
                        .ToArray();

                    if (entities.Any())
                    {
                        var tempList = await repository.GetListForEntitiesAsync(entities, ColumnSetInstances.AllColumns);

                        foreach (var item in tempList)
                        {
                            if (hashSet.Add(item.Id))
                            {
                                result.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        protected override async Task<List<SystemForm>> GetSystemFormAsync(IOrganizationServiceExtented service)
        {
            List<SystemForm> result = new List<SystemForm>();

            var descriptor = new SolutionComponentDescriptor(service);
            var repository = new SystemFormRepository(service);

            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.SystemForm);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var tempList = await repository.GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), ColumnSetInstances.AllColumns);

                    result.AddRange(tempList);
                }
            }

            var hashSet = new HashSet<Guid>(result.Select(c => c.Id));

            imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Entity);

            if (imageComponents.Any())
            {
                var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

                if (solutionComponents.Any())
                {
                    var entities = solutionComponents
                        .Where(c => c.RootComponentBehaviorEnum.GetValueOrDefault(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0) == SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0
                            && c.ObjectId.HasValue)
                        .Select(e => descriptor.MetadataSource.GetEntityMetadata(e.ObjectId.Value))
                        .Where(e => e != null)
                        .Select(e => e.LogicalName)
                        .ToArray();

                    if (entities.Any())
                    {
                        var tempList = await repository.GetListForEntitiesAsync(entities, ColumnSetInstances.AllColumns);

                        foreach (var item in tempList)
                        {
                            if (hashSet.Add(item.Id))
                            {
                                result.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        protected override Task<List<OptionSetMetadata>> GetOptionSetMetadataAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.OptionSet);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<OptionSetMetadata>());
            }

            return Task.Run(() => GetOptionSet(service, imageComponents));
        }

        private static async Task<List<OptionSetMetadata>> GetOptionSet(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> imageComponents)
        {
            var descriptor = new SolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                return new List<OptionSetMetadata>();
            }

            return await new OptionSetRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value));
        }

        protected override async Task<List<EntityMetadata>> GetEntityMetadataCollection1InternalAsync()
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Entity);

            if (!imageComponents.Any())
            {
                this._EntityMetadataCollection1 = new List<EntityMetadata>();

                return this._EntityMetadataCollection1;
            }

            await InitializeConnection(null, null);

            var descriptor = new SolutionComponentDescriptor(this.Service1);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                this._EntityMetadataCollection1 = new List<EntityMetadata>();

                return this._EntityMetadataCollection1;
            }

            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression() { AllProperties = true },
                AttributeQuery = new AttributeQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                RelationshipQuery = new RelationshipQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                LabelQuery = new LabelQueryExpression(),

                Criteria =
                {
                    Conditions =
                    {
                        new MetadataConditionExpression("MetadataId", MetadataConditionOperator.In, solutionComponents.Select(c => c.ObjectId.Value).ToArray()),
                    },
                },
            };

            var isEntityKeyExists = this.Service1.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

            if (isEntityKeyExists)
            {
                entityQueryExpression.KeyQuery = new EntityKeyQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } };
            }

            var response = (RetrieveMetadataChangesResponse)this.Service1.Execute(
                new RetrieveMetadataChangesRequest()
                {
                    ClientVersionStamp = null,
                    Query = entityQueryExpression,
                }
            );

            this._EntityMetadataCollection1 = response.EntityMetadata.ToList();

            return this._EntityMetadataCollection1;
        }

        protected override async Task<List<EntityMetadata>> GetEntityMetadataCollection2InternalAsync()
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Entity);

            if (!imageComponents.Any())
            {
                this._EntityMetadataCollection2 = new List<EntityMetadata>();

                return this._EntityMetadataCollection2;
            }

            await InitializeConnection(null, null);

            var descriptor = new SolutionComponentDescriptor(this.Service2);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(imageComponents);

            if (!solutionComponents.Any())
            {
                this._EntityMetadataCollection2 = new List<EntityMetadata>();

                return this._EntityMetadataCollection2;
            }

            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression() { AllProperties = true },
                AttributeQuery = new AttributeQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                RelationshipQuery = new RelationshipQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                LabelQuery = new LabelQueryExpression(),

                Criteria =
                {
                    Conditions =
                    {
                        new MetadataConditionExpression("MetadataId", MetadataConditionOperator.In, solutionComponents.Select(c => c.ObjectId.Value).ToArray()),
                    },
                },
            };

            var isEntityKeyExists = this.Service2.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

            if (isEntityKeyExists)
            {
                entityQueryExpression.KeyQuery = new EntityKeyQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } };
            }

            var response = (RetrieveMetadataChangesResponse)this.Service2.Execute(
                new RetrieveMetadataChangesRequest()
                {
                    ClientVersionStamp = null,
                    Query = entityQueryExpression,
                }
            );

            this._EntityMetadataCollection2 = response.EntityMetadata.ToList();

            return this._EntityMetadataCollection2;
        }
    }
}
