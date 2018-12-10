using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class OrganizationComparerSource : IOrganizationComparerSource
    {
        public ConnectionData Connection1 { get; private set; }

        public ConnectionData Connection2 { get; private set; }

        public IOrganizationServiceExtented Service1 { get; private set; }

        public IOrganizationServiceExtented Service2 { get; private set; }

        public OrganizationComparerSource(ConnectionData connection1, ConnectionData connection2)
        {
            this.Connection1 = connection1;
            this.Connection2 = connection2;
        }

        public async Task InitializeConnection(IWriteToOutput writeToOutput, StringBuilder content)
        {
            bool service1IsNull = this.Service1 == null;
            bool service2IsNull = this.Service2 == null;

            {
                var mess1 = Properties.OutputStrings.ConnectingToCRM;
                var mess2 = Connection1.GetConnectionDescription();

                if (service1IsNull)
                {
                    writeToOutput.WriteToOutput(mess1);
                    writeToOutput.WriteToOutput(mess2);

                    this.Service1 = await QuickConnection.ConnectAsync(Connection1);
                }

                var mess3 = string.Format(Properties.OutputStrings.CurrentServiceEndpointFormat1, this.Service1.CurrentServiceEndpoint);

                if (service1IsNull)
                {
                    writeToOutput.WriteToOutput(mess3);
                }

                if (content != null)
                {
                    content.AppendLine(mess1);
                    content.AppendLine(mess2);
                    content.AppendLine(mess3);
                }
            }

            if (service1IsNull)
            {
                writeToOutput.WriteToOutput(string.Empty);
            }

            if (content != null)
            {
                content.AppendLine();
            }

            {
                var mess1 = Properties.OutputStrings.ConnectingToCRM;
                var mess2 = Connection2.GetConnectionDescription();

                if (service2IsNull)
                {
                    writeToOutput.WriteToOutput(mess1);
                    writeToOutput.WriteToOutput(mess2);

                    this.Service2 = await QuickConnection.ConnectAsync(Connection2);
                }

                var mess3 = string.Format(Properties.OutputStrings.CurrentServiceEndpointFormat1, this.Service2.CurrentServiceEndpoint);

                if (service2IsNull)
                {
                    writeToOutput.WriteToOutput(mess3);
                }

                if (content != null)
                {
                    content.AppendLine(mess1);
                    content.AppendLine(mess2);
                    content.AppendLine(mess3);
                }
            }

            if (content != null)
            {
                content.AppendLine();
            }
        }

        private EntityMetadataCollection _EntityMetadataCollection1;
        public EntityMetadataCollection GetEntityMetadataCollection1()
        {
            if (this._EntityMetadataCollection1 == null)
            {
                if (this.Service1 == null)
                {
                    Task.WaitAll(InitializeConnection(null, null));
                }

                var entityQueryExpression = new EntityQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression() { AllProperties = true },
                    AttributeQuery = new AttributeQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                    RelationshipQuery = new RelationshipQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                    LabelQuery = new LabelQueryExpression(),
                };

                var isEntityKeyExists = this.Service1.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

                if (isEntityKeyExists)
                {
                    entityQueryExpression.KeyQuery = new EntityKeyQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } };
                }

                var response1 = (RetrieveMetadataChangesResponse)this.Service1.Execute(
                    new RetrieveMetadataChangesRequest()
                    {
                        ClientVersionStamp = null,
                        Query = entityQueryExpression,
                    }
                );

                this._EntityMetadataCollection1 = response1.EntityMetadata;
            }

            return this._EntityMetadataCollection1;
        }

        private EntityMetadataCollection _EntityMetadataCollection2;
        public EntityMetadataCollection GetEntityMetadataCollection2()
        {
            if (this._EntityMetadataCollection2 == null)
            {
                if (this.Service2 == null)
                {
                    Task.WaitAll(InitializeConnection(null, null));
                }

                var entityQueryExpression = new EntityQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression() { AllProperties = true },
                    AttributeQuery = new AttributeQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                    RelationshipQuery = new RelationshipQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                    LabelQuery = new LabelQueryExpression(),
                };

                var isEntityKeyExists = Service2.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

                if (isEntityKeyExists)
                {
                    entityQueryExpression.KeyQuery = new EntityKeyQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } };
                }

                var response2 = (RetrieveMetadataChangesResponse)Service2.Execute(
                    new RetrieveMetadataChangesRequest()
                    {
                        ClientVersionStamp = null,
                        Query = entityQueryExpression,
                    }
                );

                this._EntityMetadataCollection2 = response2.EntityMetadata;
            }

            return this._EntityMetadataCollection2;
        }

        public Task<List<EntityMap>> GetEntityMap1Async()
        {
            return GetEntityMapAsync(Service1);
        }

        public Task<List<EntityMap>> GetEntityMap2Async()
        {
            return GetEntityMapAsync(Service2);
        }

        protected virtual Task<List<EntityMap>> GetEntityMapAsync(IOrganizationServiceExtented service)
        {
            return new EntityMapRepository(service).GetListAsync();
        }

        public Task<List<AttributeMap>> GetAttributeMap1Async()
        {
            return GetAttributeMapAsync(Service1);
        }

        public Task<List<AttributeMap>> GetAttributeMap2Async()
        {
            return GetAttributeMapAsync(Service2);
        }

        protected virtual Task<List<AttributeMap>> GetAttributeMapAsync(IOrganizationServiceExtented service)
        {
            return new AttributeMapRepository(service).GetListAsync();
        }

        public Task<List<OptionSetMetadata>> GetOptionSetMetadata1Async()
        {
            return GetOptionSetMetadataAsync(Service1);
        }

        public Task<List<OptionSetMetadata>> GetOptionSetMetadata2Async()
        {
            return GetOptionSetMetadataAsync(Service2);
        }

        protected virtual Task<List<OptionSetMetadata>> GetOptionSetMetadataAsync(IOrganizationServiceExtented service)
        {
            return new OptionSetRepository(service).GetOptionSetsAsync();
        }

        public Task<List<WebResource>> GetWebResource1Async()
        {
            return GetWebResourceAsync(Service1);
        }

        public Task<List<WebResource>> GetWebResource2Async()
        {
            return GetWebResourceAsync(Service2);
        }

        protected virtual Task<List<WebResource>> GetWebResourceAsync(IOrganizationServiceExtented service)
        {
            return new WebResourceRepository(service).GetListAllWithContentAsync();
        }

        public Task<List<SiteMap>> GetSiteMap1Async(ColumnSet columnSet = null)
        {
            return GetSiteMapAsync(Service1, columnSet);
        }

        public Task<List<SiteMap>> GetSiteMap2Async(ColumnSet columnSet = null)
        {
            return GetSiteMapAsync(Service2, columnSet);
        }

        protected virtual Task<List<SiteMap>> GetSiteMapAsync(IOrganizationServiceExtented service, ColumnSet columnSet = null)
        {
            return new SitemapRepository(service).GetListAsync(columnSet);
        }

        public Task<Organization> GetOrganization1Async(ColumnSet columnSet)
        {
            if (Connection1.OrganizationId.HasValue)
            {
                return new OrganizationRepository(Service1).GetByIdAsync(Connection1.OrganizationId.Value, null);
            }

            return null;
        }

        public Task<Organization> GetOrganization2Async(ColumnSet columnSet)
        {
            if (Connection2.OrganizationId.HasValue)
            {
                return new OrganizationRepository(Service2).GetByIdAsync(Connection2.OrganizationId.Value, null);
            }

            return null;
        }

        public Task<List<Report>> GetReport1Async()
        {
            return GetReportAsync(Service1);
        }

        public Task<List<Report>> GetReport2Async()
        {
            return GetReportAsync(Service2);
        }

        protected virtual Task<List<Report>> GetReportAsync(IOrganizationServiceExtented service)
        {
            return new ReportRepository(service).GetListAllForCompareAsync();
        }

        public Task<List<Workflow>> GetWorkflow1Async()
        {
            return GetWorkflowAsync(Service1);
        }

        public Task<List<Workflow>> GetWorkflow2Async()
        {
            return GetWorkflowAsync(Service2);
        }

        protected virtual Task<List<Workflow>> GetWorkflowAsync(IOrganizationServiceExtented service)
        {
            return new WorkflowRepository(service).GetListAsync(null, null, null, null);
        }

        public Task<List<SystemForm>> GetSystemForm1Async()
        {
            return GetSystemFormAsync(Service1);
        }

        public Task<List<SystemForm>> GetSystemForm2Async()
        {
            return GetSystemFormAsync(Service2);
        }

        protected virtual Task<List<SystemForm>> GetSystemFormAsync(IOrganizationServiceExtented service)
        {
            return new SystemFormRepository(service).GetListAsync(null, null);
        }

        public Task<List<SavedQuery>> GetSavedQuery1Async()
        {
            return GetSavedQueryAsync(Service1);
        }

        public Task<List<SavedQuery>> GetSavedQuery2Async()
        {
            return GetSavedQueryAsync(Service2);
        }

        protected virtual Task<List<SavedQuery>> GetSavedQueryAsync(IOrganizationServiceExtented service)
        {
            return new SavedQueryRepository(service).GetListAsync(null, null);
        }

        public Task<List<SavedQueryVisualization>> GetSavedQueryVisualization1Async()
        {
            return GetSavedQueryVisualizationAsync(Service1);
        }

        public Task<List<SavedQueryVisualization>> GetSavedQueryVisualization2Async()
        {
            return GetSavedQueryVisualizationAsync(Service2);
        }

        protected virtual Task<List<SavedQueryVisualization>> GetSavedQueryVisualizationAsync(IOrganizationServiceExtented service)
        {
            return new SavedQueryVisualizationRepository(service).GetListAsync(null, null);
        }

        public Task<List<PluginAssembly>> GetPluginAssembly1Async()
        {
            return GetPluginAssemblyAsync(Service1);
        }

        public Task<List<PluginAssembly>> GetPluginAssembly2Async()
        {
            return GetPluginAssemblyAsync(Service2);
        }

        protected virtual Task<List<PluginAssembly>> GetPluginAssemblyAsync(IOrganizationServiceExtented service)
        {
            return new PluginAssemblyRepository(service).GetPluginAssembliesAsync(null, null);
        }

        public Task<List<PluginType>> GetPluginType1Async()
        {
            return GetPluginTypeAsync(Service1);
        }

        public Task<List<PluginType>> GetPluginType2Async()
        {
            return GetPluginTypeAsync(Service2);
        }

        protected virtual Task<List<PluginType>> GetPluginTypeAsync(IOrganizationServiceExtented service)
        {
            return new PluginTypeRepository(service).GetPluginTypesAsync(null, null);
        }

        public Task<List<SdkMessageProcessingStep>> GetSdkMessageProcessingStep1Async()
        {
            return GetSdkMessageProcessingStepAsync(Service1);
        }

        public Task<List<SdkMessageProcessingStep>> GetSdkMessageProcessingStep2Async()
        {
            return GetSdkMessageProcessingStepAsync(Service2);
        }

        protected virtual Task<List<SdkMessageProcessingStep>> GetSdkMessageProcessingStepAsync(IOrganizationServiceExtented service)
        {
            return new SdkMessageProcessingStepRepository(service).GetAllSdkMessageProcessingStepAsync(null, null, null);
        }

        public Task<List<SdkMessageProcessingStepImage>> GetSdkMessageProcessingStepImage1Async()
        {
            return GetSdkMessageProcessingStepImageAsync(Service1);
        }

        public Task<List<SdkMessageProcessingStepImage>> GetSdkMessageProcessingStepImage2Async()
        {
            return GetSdkMessageProcessingStepImageAsync(Service2);
        }

        protected virtual Task<List<SdkMessageProcessingStepImage>> GetSdkMessageProcessingStepImageAsync(IOrganizationServiceExtented service)
        {
            return new SdkMessageProcessingStepImageRepository(service).GetAllImagesAsync();
        }

        public Task<List<Role>> GetRole1Async()
        {
            return GetRoleAsync(Service1);
        }

        public Task<List<Role>> GetRole2Async()
        {
            return GetRoleAsync(Service2);
        }

        protected virtual Task<List<Role>> GetRoleAsync(IOrganizationServiceExtented service)
        {
            return new RoleRepository(service).GetListAsync(null, new ColumnSet(true));
        }

        public Task<List<FieldSecurityProfile>> GetFieldSecurityProfile1Async()
        {
            return GetFieldSecurityProfileAsync(Service1);
        }

        public Task<List<FieldSecurityProfile>> GetFieldSecurityProfile2Async()
        {
            return GetFieldSecurityProfileAsync(Service2);
        }

        protected virtual Task<List<FieldSecurityProfile>> GetFieldSecurityProfileAsync(IOrganizationServiceExtented service)
        {
            return new FieldSecurityProfileRepository(service).GetListAsync();
        }

        public Task<List<FieldPermission>> GetFieldPermission1Async()
        {
            return new FieldPermissionRepository(Service1).GetListAsync();
        }

        public Task<List<FieldPermission>> GetFieldPermission2Async()
        {
            return new FieldPermissionRepository(Service2).GetListAsync();
        }

        protected virtual Task<List<FieldSecurityProfile>> GetFieldPermissionAsync(IOrganizationServiceExtented service)
        {
            return new FieldSecurityProfileRepository(service).GetListAsync();
        }

        public Task<List<MailMergeTemplate>> GetMailMergeTemplate1Async()
        {
            return GetMailMergeTemplateAsync(Service1);
        }

        public Task<List<MailMergeTemplate>> GetMailMergeTemplate2Async()
        {
            return GetMailMergeTemplateAsync(Service2);
        }

        protected virtual Task<List<MailMergeTemplate>> GetMailMergeTemplateAsync(IOrganizationServiceExtented service)
        {
            return new MailMergeTemplateRepository(service).GetListAsync();
        }

        public Task<List<Template>> GetTemplate1Async()
        {
            return GetTemplateAsync(Service1);
        }

        public Task<List<Template>> GetTemplate2Async()
        {
            return GetTemplateAsync(Service2);
        }

        protected virtual Task<List<Template>> GetTemplateAsync(IOrganizationServiceExtented service)
        {
            return new TemplateRepository(service).GetListAsync();
        }

        public Task<List<KbArticleTemplate>> GetKbArticleTemplate1Async()
        {
            return GetKbArticleTemplateAsync(Service1);
        }

        public Task<List<KbArticleTemplate>> GetKbArticleTemplate2Async()
        {
            return GetKbArticleTemplateAsync(Service2);
        }

        protected virtual Task<List<KbArticleTemplate>> GetKbArticleTemplateAsync(IOrganizationServiceExtented service)
        {
            return new KbArticleTemplateRepository(service).GetListAsync();
        }

        public Task<List<ContractTemplate>> GetContractTemplate1Async()
        {
            return GetContractTemplateAsync(Service1);
        }

        public Task<List<ContractTemplate>> GetContractTemplate2Async()
        {
            return GetContractTemplateAsync(Service2);
        }

        protected virtual Task<List<ContractTemplate>> GetContractTemplateAsync(IOrganizationServiceExtented service)
        {
            return new ContractTemplateRepository(service).GetListAsync();
        }

        public Task<List<DisplayString>> GetDisplayString1Async()
        {
            return GetDisplayStringAsync(Service1);
        }

        public Task<List<DisplayString>> GetDisplayString2Async()
        {
            return GetDisplayStringAsync(Service2);
        }

        protected virtual Task<List<DisplayString>> GetDisplayStringAsync(IOrganizationServiceExtented service)
        {
            return new DisplayStringRepository(service).GetListAsync();
        }

        public Task<List<DisplayStringMap>> GetDisplayStringMap1Async()
        {
            return GetDisplayStringMapAsync(Service1);
        }

        public Task<List<DisplayStringMap>> GetDisplayStringMap2Async()
        {
            return GetDisplayStringMapAsync(Service2);
        }

        protected virtual Task<List<DisplayStringMap>> GetDisplayStringMapAsync(IOrganizationServiceExtented service)
        {
            return new DisplayStringMapRepository(service).GetListAsync();
        }

        public Task<HashSet<string>> GetEntitiesWithRibbonCustomization1Async()
        {
            return GetEntitiesWithRibbonCustomizationAsync(Service1);
        }

        public Task<HashSet<string>> GetEntitiesWithRibbonCustomization2Async()
        {
            return GetEntitiesWithRibbonCustomizationAsync(Service2);
        }

        protected virtual Task<HashSet<string>> GetEntitiesWithRibbonCustomizationAsync(IOrganizationServiceExtented service)
        {
            return new RibbonCustomizationRepository(service).GetEntitiesWithRibbonCustomizationAsync();
        }

        public Task<List<ConnectionRole>> GetConnectionRole1Async()
        {
            return GetConnectionRoleAsync(Service1);
        }

        public Task<List<ConnectionRole>> GetConnectionRole2Async()
        {
            return GetConnectionRoleAsync(Service2);
        }

        protected virtual Task<List<ConnectionRole>> GetConnectionRoleAsync(IOrganizationServiceExtented service)
        {
            return new ConnectionRoleRepository(service).GetListAsync();
        }

        public Task<List<ConnectionRoleAssociation>> GetConnectionRoleAssociation1Async()
        {
            return GetConnectionRoleAssociationAsync(Service1);
        }

        public Task<List<ConnectionRoleAssociation>> GetConnectionRoleAssociation2Async()
        {
            return GetConnectionRoleAssociationAsync(Service2);
        }

        protected virtual Task<List<ConnectionRoleAssociation>> GetConnectionRoleAssociationAsync(IOrganizationServiceExtented service)
        {
            return new ConnectionRoleAssociationRepository(service).GetListAsync();
        }

        public Task<List<ConnectionRoleObjectTypeCode>> GetConnectionRoleObjectTypeCode1Async()
        {
            return GetConnectionRoleObjectTypeCodeAsync(Service1);
        }

        public Task<List<ConnectionRoleObjectTypeCode>> GetConnectionRoleObjectTypeCode2Async()
        {
            return GetConnectionRoleObjectTypeCodeAsync(Service2);
        }

        protected virtual Task<List<ConnectionRoleObjectTypeCode>> GetConnectionRoleObjectTypeCodeAsync(IOrganizationServiceExtented service)
        {
            return new ConnectionRoleObjectTypeCodeRepository(service).GetListAsync();
        }
    }
}
