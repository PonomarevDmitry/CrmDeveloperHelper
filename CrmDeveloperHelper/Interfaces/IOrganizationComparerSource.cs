using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IOrganizationComparerSource
    {
        ConnectionData Connection1 { get; }

        ConnectionData Connection2 { get; }

        IOrganizationServiceExtented Service1 { get; }

        IOrganizationServiceExtented Service2 { get; }

        Task InitializeConnection(IWriteToOutput writeToOutput, StringBuilder content);

        Task<List<EntityMap>> GetEntityMap1Async();
        Task<List<EntityMap>> GetEntityMap2Async();

        Task<List<OptionSetMetadata>> GetOptionSetMetadata1Async();
        Task<List<OptionSetMetadata>> GetOptionSetMetadata2Async();

        Task<List<WebResource>> GetWebResource1Async();
        Task<List<WebResource>> GetWebResource2Async();

        Task<List<SiteMap>> GetSiteMap1Async(ColumnSet columnSet = null);
        Task<List<SiteMap>> GetSiteMap2Async(ColumnSet columnSet = null);

        Task<Organization> GetOrganization1Async(ColumnSet columnSet);
        Task<Organization> GetOrganization2Async(ColumnSet columnSet);

        Task<List<Report>> GetReport1Async();
        Task<List<Report>> GetReport2Async();

        Task<List<Workflow>> GetWorkflow1Async();
        Task<List<Workflow>> GetWorkflow2Async();

        Task<List<SystemForm>> GetSystemForm1Async();
        Task<List<SystemForm>> GetSystemForm2Async();

        Task<List<SavedQuery>> GetSavedQuery1Async();
        Task<List<SavedQuery>> GetSavedQuery2Async();

        Task<List<SavedQueryVisualization>> GetSavedQueryVisualization1Async();
        Task<List<SavedQueryVisualization>> GetSavedQueryVisualization2Async();

        Task<List<PluginAssembly>> GetPluginAssembly1Async();
        Task<List<PluginAssembly>> GetPluginAssembly2Async();

        Task<List<PluginType>> GetPluginType1Async();
        Task<List<PluginType>> GetPluginType2Async();

        Task<List<SdkMessageProcessingStep>> GetSdkMessageProcessingStep1Async();
        Task<List<SdkMessageProcessingStep>> GetSdkMessageProcessingStep2Async();

        Task<List<SdkMessageProcessingStepImage>> GetSdkMessageProcessingStepImage1Async();
        Task<List<SdkMessageProcessingStepImage>> GetSdkMessageProcessingStepImage2Async();

        Task<List<Role>> GetRole1Async();
        Task<List<Role>> GetRole2Async();

        Task<List<FieldSecurityProfile>> GetFieldSecurityProfile1Async();
        Task<List<FieldSecurityProfile>> GetFieldSecurityProfile2Async();

        Task<List<FieldPermission>> GetFieldPermission1Async();
        Task<List<FieldPermission>> GetFieldPermission2Async();

        Task<List<MailMergeTemplate>> GetMailMergeTemplate1Async();
        Task<List<MailMergeTemplate>> GetMailMergeTemplate2Async();

        Task<List<Template>> GetTemplate1Async();
        Task<List<Template>> GetTemplate2Async();

        Task<List<KbArticleTemplate>> GetKbArticleTemplate1Async();
        Task<List<KbArticleTemplate>> GetKbArticleTemplate2Async();

        Task<List<ContractTemplate>> GetContractTemplate1Async();
        Task<List<ContractTemplate>> GetContractTemplate2Async();

        Task<List<DisplayString>> GetDisplayString1Async();
        Task<List<DisplayString>> GetDisplayString2Async();

        Task<List<DisplayStringMap>> GetDisplayStringMap1Async();
        Task<List<DisplayStringMap>> GetDisplayStringMap2Async();

        Task<List<AttributeMap>> GetAttributeMap1Async();
        Task<List<AttributeMap>> GetAttributeMap2Async();

        Task<HashSet<string>> GetEntitiesWithRibbonCustomization1Async();
        Task<HashSet<string>> GetEntitiesWithRibbonCustomization2Async();

        EntityMetadataCollection GetEntityMetadataCollection1();
        EntityMetadataCollection GetEntityMetadataCollection2();

        Task<List<ConnectionRole>> GetConnectionRole1Async();
        Task<List<ConnectionRole>> GetConnectionRole2Async();

        Task<List<ConnectionRoleAssociation>> GetConnectionRoleAssociation1Async();
        Task<List<ConnectionRoleAssociation>> GetConnectionRoleAssociation2Async();

        Task<List<ConnectionRoleObjectTypeCode>> GetConnectionRoleObjectTypeCode1Async();
        Task<List<ConnectionRoleObjectTypeCode>> GetConnectionRoleObjectTypeCode2Async();

        //Task<List<OptionSetMetadata>> Get1Async();
        //Task<List<OptionSetMetadata>> Get2Async();

        //Task<List<OptionSetMetadata>> Get1Async();
        //Task<List<OptionSetMetadata>> Get2Async();

        //Task<List<OptionSetMetadata>> Get1Async();
        //Task<List<OptionSetMetadata>> Get2Async();

        //Task<List<OptionSetMetadata>> Get1Async();
        //Task<List<OptionSetMetadata>> Get2Async();

    }
}
