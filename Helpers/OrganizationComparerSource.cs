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

        public virtual Task<List<EntityMap>> GetEntityMap1Async()
        {
            return new EntityMapRepository(Service1).GetListAsync();
        }

        public virtual Task<List<EntityMap>> GetEntityMap2Async()
        {
            return new EntityMapRepository(Service2).GetListAsync();
        }

        public virtual Task<List<AttributeMap>> GetAttributeMap1Async()
        {
            return new AttributeMapRepository(Service1).GetListAsync();
        }

        public virtual Task<List<AttributeMap>> GetAttributeMap2Async()
        {
            return new AttributeMapRepository(Service2).GetListAsync();
        }

        public virtual Task<List<OptionSetMetadata>> GetOptionSetMetadata1Async()
        {
            return new OptionSetRepository(Service1).GetOptionSetsAsync();
        }

        public virtual Task<List<OptionSetMetadata>> GetOptionSetMetadata2Async()
        {
            return new OptionSetRepository(Service2).GetOptionSetsAsync();
        }

        public virtual Task<List<WebResource>> GetWebResource1Async()
        {
            return new WebResourceRepository(Service1).GetListAllWithContentAsync();
        }

        public virtual Task<List<WebResource>> GetWebResource2Async()
        {
            return new WebResourceRepository(Service2).GetListAllWithContentAsync();
        }

        public virtual Task<List<SiteMap>> GetSiteMap1Async(ColumnSet columnSet = null)
        {
            return new SitemapRepository(Service1).GetListAsync(columnSet);
        }

        public virtual Task<List<SiteMap>> GetSiteMap2Async(ColumnSet columnSet = null)
        {
            return new SitemapRepository(Service2).GetListAsync(columnSet);
        }

        public virtual Task<Organization> GetOrganization1Async(ColumnSet columnSet)
        {
            if (Connection1.OrganizationId.HasValue)
            {
                return new OrganizationRepository(Service1).GetByIdAsync(Connection1.OrganizationId.Value, null);
            }

            return null;
        }

        public virtual Task<Organization> GetOrganization2Async(ColumnSet columnSet)
        {
            if (Connection2.OrganizationId.HasValue)
            {
                return new OrganizationRepository(Service2).GetByIdAsync(Connection2.OrganizationId.Value, null);
            }

            return null;
        }

        public virtual Task<List<Report>> GetReport1Async()
        {
            return new ReportRepository(Service1).GetListAllForCompareAsync();
        }

        public virtual Task<List<Report>> GetReport2Async()
        {
            return new ReportRepository(Service2).GetListAllForCompareAsync();
        }

        public virtual Task<List<Workflow>> GetWorkflow1Async()
        {
            return new WorkflowRepository(Service1).GetListAsync(null, null, null, null);
        }

        public virtual Task<List<Workflow>> GetWorkflow2Async()
        {
            return new WorkflowRepository(Service2).GetListAsync(null, null, null, null);
        }

        public virtual Task<List<SystemForm>> GetSystemForm1Async()
        {
            return new SystemFormRepository(Service1).GetListAsync(null, null);
        }

        public virtual Task<List<SystemForm>> GetSystemForm2Async()
        {
            return new SystemFormRepository(Service2).GetListAsync(null, null);
        }

        public virtual Task<List<SavedQuery>> GetSavedQuery1Async()
        {
            return new SavedQueryRepository(Service1).GetListAsync(null, null);
        }

        public virtual Task<List<SavedQuery>> GetSavedQuery2Async()
        {
            return new SavedQueryRepository(Service2).GetListAsync(null, null);
        }

        public virtual Task<List<SavedQueryVisualization>> GetSavedQueryVisualization1Async()
        {
            return new SavedQueryVisualizationRepository(Service1).GetListAsync(null, null);
        }

        public virtual Task<List<SavedQueryVisualization>> GetSavedQueryVisualization2Async()
        {
            return new SavedQueryVisualizationRepository(Service2).GetListAsync(null, null);
        }

        public virtual Task<List<PluginAssembly>> GetPluginAssembly1Async()
        {
            return new PluginAssemblyRepository(Service1).GetPluginAssembliesAsync(null, null);
        }

        public virtual Task<List<PluginAssembly>> GetPluginAssembly2Async()
        {
            return new PluginAssemblyRepository(Service2).GetPluginAssembliesAsync(null, null);
        }

        public virtual Task<List<PluginType>> GetPluginType1Async()
        {
            return new PluginTypeRepository(Service1).GetPluginTypesAsync(null, null);
        }

        public virtual Task<List<PluginType>> GetPluginType2Async()
        {
            return new PluginTypeRepository(Service2).GetPluginTypesAsync(null, null);
        }

        public virtual Task<List<SdkMessageProcessingStep>> GetSdkMessageProcessingStep1Async()
        {
            return new SdkMessageProcessingStepRepository(Service1).GetAllSdkMessageProcessingStepAsync(null, null, null);
        }

        public virtual Task<List<SdkMessageProcessingStep>> GetSdkMessageProcessingStep2Async()
        {
            return new SdkMessageProcessingStepRepository(Service2).GetAllSdkMessageProcessingStepAsync(null, null, null);
        }

        public virtual Task<List<SdkMessageProcessingStepImage>> GetSdkMessageProcessingStepImage1Async()
        {
            return new SdkMessageProcessingStepImageRepository(Service1).GetAllImagesAsync();
        }

        public virtual Task<List<SdkMessageProcessingStepImage>> GetSdkMessageProcessingStepImage2Async()
        {
            return new SdkMessageProcessingStepImageRepository(Service2).GetAllImagesAsync();
        }

        public virtual Task<List<Role>> GetRole1Async()
        {
            return new SecurityRoleRepository(Service1).GetListParentRolesAsync();
        }

        public virtual Task<List<Role>> GetRole2Async()
        {
            return new SecurityRoleRepository(Service2).GetListParentRolesAsync();
        }

        public virtual Task<List<FieldSecurityProfile>> GetFieldSecurityProfile1Async()
        {
            return new FieldSecurityProfileRepository(Service1).GetListAsync();
        }

        public virtual Task<List<FieldSecurityProfile>> GetFieldSecurityProfile2Async()
        {
            return new FieldSecurityProfileRepository(Service2).GetListAsync();
        }

        public virtual Task<List<FieldPermission>> GetFieldPermission1Async()
        {
            return new FieldPermissionRepository(Service1).GetListAsync();
        }

        public virtual Task<List<FieldPermission>> GetFieldPermission2Async()
        {
            return new FieldPermissionRepository(Service2).GetListAsync();
        }

        public virtual Task<List<MailMergeTemplate>> GetMailMergeTemplate1Async()
        {
            return new MailMergeTemplateRepository(Service1).GetListAsync();
        }

        public virtual Task<List<MailMergeTemplate>> GetMailMergeTemplate2Async()
        {
            return new MailMergeTemplateRepository(Service2).GetListAsync();
        }

        public virtual Task<List<Template>> GetTemplate1Async()
        {
            return new TemplateRepository(Service1).GetListAsync();
        }

        public virtual Task<List<Template>> GetTemplate2Async()
        {
            return new TemplateRepository(Service2).GetListAsync();
        }

        public virtual Task<List<KbArticleTemplate>> GetKbArticleTemplate1Async()
        {
            return new KbArticleTemplateRepository(Service1).GetListAsync();
        }

        public virtual Task<List<KbArticleTemplate>> GetKbArticleTemplate2Async()
        {
            return new KbArticleTemplateRepository(Service2).GetListAsync();
        }

        public virtual Task<List<ContractTemplate>> GetContractTemplate1Async()
        {
            return new ContractTemplateRepository(Service1).GetListAsync();
        }

        public virtual Task<List<ContractTemplate>> GetContractTemplate2Async()
        {
            return new ContractTemplateRepository(Service2).GetListAsync();
        }

        public virtual Task<List<DisplayString>> GetDisplayString1Async()
        {
            return new DisplayStringRepository(Service1).GetListAsync();
        }

        public virtual Task<List<DisplayString>> GetDisplayString2Async()
        {
            return new DisplayStringRepository(Service2).GetListAsync();
        }

        public virtual Task<List<DisplayStringMap>> GetDisplayStringMap1Async()
        {
            return new DisplayStringMapRepository(Service1).GetListAsync();
        }

        public virtual Task<List<DisplayStringMap>> GetDisplayStringMap2Async()
        {
            return new DisplayStringMapRepository(Service2).GetListAsync();
        }

        public virtual Task<HashSet<string>> GetEntitiesWithRibbonCustomization1Async()
        {
            return new RibbonCustomizationRepository(Service1).GetEntitiesWithRibbonCustomizationAsync();
        }

        public virtual Task<HashSet<string>> GetEntitiesWithRibbonCustomization2Async()
        {
            return new RibbonCustomizationRepository(Service2).GetEntitiesWithRibbonCustomizationAsync();
        }

        public virtual Task<List<ConnectionRole>> GetConnectionRole1Async()
        {
            return new ConnectionRoleRepository(Service1).GetListAsync();
        }

        public virtual Task<List<ConnectionRole>> GetConnectionRole2Async()
        {
            return new ConnectionRoleRepository(Service2).GetListAsync();
        }

        public virtual Task<List<ConnectionRoleAssociation>> GetConnectionRoleAssociation1Async()
        {
            return new ConnectionRoleAssociationRepository(Service1).GetListAsync();
        }

        public virtual Task<List<ConnectionRoleAssociation>> GetConnectionRoleAssociation2Async()
        {
            return new ConnectionRoleAssociationRepository(Service2).GetListAsync();
        }

        public virtual Task<List<ConnectionRoleObjectTypeCode>> GetConnectionRoleObjectTypeCode1Async()
        {
            return new ConnectionRoleObjectTypeCodeRepository(Service1).GetListAsync();
        }

        public virtual Task<List<ConnectionRoleObjectTypeCode>> GetConnectionRoleObjectTypeCode2Async()
        {
            return new ConnectionRoleObjectTypeCodeRepository(Service2).GetListAsync();
        }
    }
}
