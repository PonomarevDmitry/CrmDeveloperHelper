using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription
{
    public class SolutionComponentDescriptionBuilderFactory
    {
        public ISolutionComponentDescriptionBuilder CreateBuilder(IOrganizationServiceExtented service, SolutionComponentMetadataSource metadataSource, int componentType)
        {
            if (SolutionComponent.IsDefinedComponentType(componentType))
            {
                var componentTypeEnum = (ComponentType)componentType;

                switch (componentTypeEnum)
                {
                    case ComponentType.Entity:
                        return new EntityMetadataDescriptionBuilder(metadataSource);
                    case ComponentType.Attribute:
                        return new AttributeMetadataDescriptionBuilder(metadataSource);
                    case ComponentType.OptionSet:
                        return new OptionSetMetadataDescriptionBuilder(metadataSource);
                    case ComponentType.EntityRelationship:
                        return new RelationshipMetadataDescriptionBuilder(metadataSource);
                    case ComponentType.EntityKey:
                        return new EntityKeyMetadataDescriptionBuilder(metadataSource);
                    case ComponentType.ManagedProperty:
                        return new ManagedPropertyDescriptionBuilder(service);
                    case ComponentType.Role:
                        return new RoleDescriptionBuilder(service);
                    case ComponentType.RolePrivileges:
                        return new RolePrivilegesDescriptionBuilder(service);
                    case ComponentType.DisplayString:
                        return new DisplayStringDescriptionBuilder(service);
                    case ComponentType.DisplayStringMap:
                        return new DisplayStringMapDescriptionBuilder(service);                    
                    case ComponentType.SavedQuery:
                        return new SavedQueryDescriptionBuilder(service);
                    case ComponentType.Workflow:
                        return new WorkflowDescriptionBuilder(service);
                    case ComponentType.ProcessTrigger:
                        return new ProcessTriggerDescriptionBuilder(service);
                    case ComponentType.Report:
                        return new ReportDescriptionBuilder(service);
                    case ComponentType.ReportEntity:
                        return new ReportEntityDescriptionBuilder(service);
                    case ComponentType.ReportCategory:
                        return new ReportCategoryDescriptionBuilder(service);
                    case ComponentType.ReportVisibility:
                        return new ReportVisibilityDescriptionBuilder(service);
                    case ComponentType.EmailTemplate:
                        return new EmailTemplateDescriptionBuilder(service);
                    case ComponentType.ContractTemplate:
                        return new ContractTemplateDescriptionBuilder(service);
                    case ComponentType.KbArticleTemplate:
                        return new KbArticleTemplateDescriptionBuilder(service);
                    case ComponentType.MailMergeTemplate:
                        return new MailMergeTemplateDescriptionBuilder(service);
                    case ComponentType.DuplicateRule:
                        return new DuplicateRuleDescriptionBuilder(service);
                    case ComponentType.DuplicateRuleCondition:
                        return new DuplicateRuleConditionDescriptionBuilder(service);
                    case ComponentType.EntityMap:
                        return new EntityMapDescriptionBuilder(service);
                    case ComponentType.AttributeMap:
                        return new AttributeMapDescriptionBuilder(service);
                    case ComponentType.RibbonCommand:
                        return new RibbonCommandDescriptionBuilder(service);
                    case ComponentType.RibbonContextGroup:
                        return new RibbonContextGroupDescriptionBuilder(service);
                    case ComponentType.RibbonCustomization:
                        return new RibbonCustomizationDescriptionBuilder(service);
                    case ComponentType.RibbonRule:
                        return new RibbonRuleDescriptionBuilder(service);
                    case ComponentType.RibbonTabToCommandMap:
                        return new RibbonTabToCommandMapDescriptionBuilder(service);
                    case ComponentType.RibbonDiff:
                        return new RibbonDiffDescriptionBuilder(service);
                    case ComponentType.SavedQueryVisualization:
                        return new SavedQueryVisualizationDescriptionBuilder(service);
                    case ComponentType.SystemForm:
                        return new SystemFormDescriptionBuilder(service);
                    case ComponentType.WebResource:
                        return new WebResourceDescriptionBuilder(service);
                    case ComponentType.SiteMap:
                        return new SiteMapDescriptionBuilder(service);
                    case ComponentType.ConnectionRole:
                        return new ConnectionRoleDescriptionBuilder(service);
                    case ComponentType.FieldSecurityProfile:
                        return new FieldSecurityProfileDescriptionBuilder(service);
                    case ComponentType.FieldPermission:
                        return new FieldPermissionDescriptionBuilder(service);
                    case ComponentType.AppModule:
                        return new AppModuleDescriptionBuilder(service);
                    case ComponentType.AppModuleRoles:
                        return new AppModuleRolesDescriptionBuilder(service);
                    case ComponentType.PluginType:
                        return new PluginTypeDescriptionBuilder(service);
                    case ComponentType.PluginAssembly:
                        return new PluginAssemblyDescriptionBuilder(service);
                    case ComponentType.SdkMessageProcessingStep:
                        return new SdkMessageProcessingStepDescriptionBuilder(service);
                    case ComponentType.SdkMessageProcessingStepImage:
                        return new SdkMessageProcessingStepImageDescriptionBuilder(service);
                    case ComponentType.ServiceEndpoint:
                        return new ServiceEndpointDescriptionBuilder(service);
                    case ComponentType.RoutingRule:
                        return new RoutingRuleDescriptionBuilder(service);
                    case ComponentType.RoutingRuleItem:
                        return new RoutingRuleItemDescriptionBuilder(service);
                    case ComponentType.SLA:
                        return new SLADescriptionBuilder(service);
                    case ComponentType.SLAItem:
                        return new SLAItemDescriptionBuilder(service);
                    case ComponentType.ConvertRule:
                        return new ConvertRuleDescriptionBuilder(service);
                    case ComponentType.ConvertRuleItem:
                        return new ConvertRuleItemDescriptionBuilder(service);
                    case ComponentType.HierarchyRule:
                        return new HierarchyRuleDescriptionBuilder(service);
                    case ComponentType.MobileOfflineProfile:
                        return new MobileOfflineProfileDescriptionBuilder(service);
                    case ComponentType.MobileOfflineProfileItem:
                        return new MobileOfflineProfileItemDescriptionBuilder(service);
                    case ComponentType.SimilarityRule:
                        return new SimilarityRuleDescriptionBuilder(service);
                    case ComponentType.CustomControl:
                        return new CustomControlDescriptionBuilder(service);
                    case ComponentType.CustomControlDefaultConfig:
                        return new CustomControlDefaultConfigDescriptionBuilder(service);
                    case ComponentType.CustomControlResource:
                        return new CustomControlResourceDescriptionBuilder(service);
                    case ComponentType.ChannelAccessProfileEntityAccessLevel:
                        return new ChannelAccessProfileEntityAccessLevelDescriptionBuilder(service);
                    case ComponentType.ChannelAccessProfile:
                        return new ChannelAccessProfileDescriptionBuilder(service);
                    case ComponentType.DependencyFeature:
                        return new DependencyFeatureDescriptionBuilder(service);
                    case ComponentType.SdkMessage:
                        return new SdkMessageDescriptionBuilder(service);
                    case ComponentType.SdkMessageFilter:
                        return new SdkMessageFilterDescriptionBuilder(service);
                    case ComponentType.SdkMessagePair:
                        return new SdkMessagePairDescriptionBuilder(service);
                    case ComponentType.SdkMessageRequest:
                        return new SdkMessageRequestDescriptionBuilder(service);
                    case ComponentType.SdkMessageRequestField:
                        return new SdkMessageRequestFieldDescriptionBuilder(service);
                    case ComponentType.SdkMessageResponse:
                        return new SdkMessageResponseDescriptionBuilder(service);
                    case ComponentType.SdkMessageResponseField:
                        return new SdkMessageResponseFieldDescriptionBuilder(service);

                        //case ComponentType.Form:
                        //    return new DescriptionBuilder(service);
                        //case ComponentType.Organization:
                        //    return new DescriptionBuilder(service);
                        //case ComponentType.Attachment:
                        //    return new DescriptionBuilder(service);
                        //case ComponentType.ComplexControl:
                        //    return new DescriptionBuilder(service);
                        //case ComponentType.ImportMap:
                        //    return new DescriptionBuilder(service);
                        //case ComponentType.WebWizard:
                        //    return new DescriptionBuilder(service);
                        //case ComponentType.ImportEntityMapping:
                        //    return new DescriptionBuilder(service);

                }
            }

            return new DefaultSolutionComponentDescriptionBuilder(service, componentType);
        }
    }
}