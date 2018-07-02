using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
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
    public partial class SolutionComponentDescriptor
    {
        private const string formatSpacer = "    {0}";

        private const string unknowedMessage = "Default Description for Unknowned Solution Components:";

        private readonly IWriteToOutput _iWriteToOutput;
        private readonly IOrganizationServiceExtented _service;
        private readonly bool _withUrls;

        /// <summary>
        /// Конструктор репозитория функция по поиску решений.
        /// </summary>
        /// <param name="service"></param>
        public SolutionComponentDescriptor(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, bool withUrls)
        {
            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._withUrls = withUrls;
        }

        public Task<string> GetSolutionComponentsDescriptionAsync(IEnumerable<SolutionComponent> components)
        {
            return Task.Run(() => GetSolutionComponentsDescription(components));
        }

        private string GetSolutionComponentsDescription(IEnumerable<SolutionComponent> components)
        {
            StringBuilder builder = new StringBuilder();

            var groups = components.GroupBy(comp => comp.ComponentType.Value).OrderBy(gr => gr.Key);

            foreach (var gr in groups)
            {
                try
                {
                    GetSolutionComponentsDescriptionForGroup(builder, gr.Key, gr.ToList());
                }
                catch (Exception ex)
                {
                    builder.AppendLine().AppendLine("Exception");

                    builder.AppendLine().AppendLine(DTEHelper.GetExceptionDescription(ex)).AppendLine();

                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }

            return builder.ToString();
        }

        private void GetSolutionComponentsDescriptionForGroup(StringBuilder builder, int type, List<SolutionComponent> components)
        {
            if (components.Count == 0)
            {
                return;
            }

            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            string name = components.First().ComponentTypeName;

            builder.AppendFormat("ComponentType:   {0} ({1})            Count: {2}"
                , name
                , type.ToString()
                , components.Count.ToString()
                ).AppendLine();

            bool useDefault = true;

            if (SolutionComponent.IsDefinedComponentType(type))
            {
                ComponentType componentType = (ComponentType)type;

                useDefault = false;

                switch (componentType)
                {
                    case ComponentType.WebResource:
                        GenerateDescriptionWebResources(builder, components);
                        break;

                    case ComponentType.Role:
                        GenerateDescriptionRoles(builder, components);
                        break;

                    case ComponentType.RolePrivileges:
                        GenerateDescriptionRolePrivileges(builder, components);
                        break;

                    case ComponentType.FieldSecurityProfile:
                        GenerateDescriptionFieldSecurityProfile(builder, components);
                        break;

                    case ComponentType.FieldPermission:
                        GenerateDescriptionFieldPermission(builder, components);
                        break;

                    case ComponentType.PluginAssembly:
                        GenerateDescriptionPluginAssembly(builder, components);
                        break;

                    case ComponentType.PluginType:
                        GenerateDescriptionPluginType(builder, components);
                        break;

                    case ComponentType.SDKMessageProcessingStep:
                        GenerateDescriptionSDKMessageProcessingStep(builder, components);
                        break;

                    case ComponentType.SDKMessageProcessingStepImage:
                        GenerateDescriptionSDKMessageProcessingStepImage(builder, components);
                        break;

                    case ComponentType.Workflow:
                        GenerateDescriptionWorkflow(builder, components);
                        break;

                    case ComponentType.ConnectionRole:
                        GenerateDescriptionConnectionRole(builder, components);
                        break;

                    case ComponentType.ConvertRule:
                        GenerateDescriptionConvertRule(builder, components);
                        break;

                    case ComponentType.ConvertRuleItem:
                        GenerateDescriptionConvertRuleItem(builder, components);
                        break;

                    case ComponentType.RoutingRule:
                        GenerateDescriptionRoutingRule(builder, components);
                        break;

                    case ComponentType.RoutingRuleItem:
                        GenerateDescriptionRoutingRuleItem(builder, components);
                        break;

                    case ComponentType.HierarchyRule:
                        GenerateDescriptionHierarchyRule(builder, components);
                        break;

                    case ComponentType.Report:
                        GenerateDescriptionReport(builder, components);
                        break;

                    case ComponentType.ReportEntity:
                        GenerateDescriptionReportEntity(builder, components);
                        break;

                    case ComponentType.ReportCategory:
                        GenerateDescriptionReportCategory(builder, components);
                        break;

                    case ComponentType.ReportVisibility:
                        GenerateDescriptionReportVisibility(builder, components);
                        break;

                    case ComponentType.EmailTemplate:
                        GenerateDescriptionEmailTemplate(builder, components);
                        break;

                    case ComponentType.MailMergeTemplate:
                        GenerateDescriptionMailMergeTemplate(builder, components);
                        break;

                    case ComponentType.KBArticleTemplate:
                        GenerateDescriptionKBArticleTemplate(builder, components);
                        break;

                    case ComponentType.ContractTemplate:
                        GenerateDescriptionContractTemplate(builder, components);
                        break;

                    case ComponentType.RibbonCustomization:
                        GenerateDescriptionRibbonCustomization(builder, components);
                        break;

                    case ComponentType.RibbonCommand:
                        GenerateDescriptionRibbonCommand(builder, components);
                        break;

                    case ComponentType.RibbonContextGroup:
                        GenerateDescriptionRibbonContextGroup(builder, components);
                        break;

                    case ComponentType.RibbonDiff:
                        GenerateDescriptionRibbonDiff(builder, components);
                        break;

                    case ComponentType.RibbonRule:
                        GenerateDescriptionRibbonRule(builder, components);
                        break;

                    case ComponentType.RibbonTabToCommandMap:
                        GenerateDescriptionRibbonTabToCommandMap(builder, components);
                        break;

                    case ComponentType.DisplayString:
                        GenerateDescriptionDisplayString(builder, components);
                        break;

                    case ComponentType.DisplayStringMap:
                        GenerateDescriptionDisplayStringMap(builder, components);
                        break;

                    case ComponentType.Entity:
                        GenerateDescriptionEntity(builder, components);
                        break;

                    case ComponentType.Attribute:
                        GenerateDescriptionAttribute(builder, components);
                        break;

                    case ComponentType.EntityRelationship:
                        GenerateDescriptionEntityRelationship(builder, components);
                        break;

                    case ComponentType.EntityKey:
                        GenerateDescriptionEntityKey(builder, components);
                        break;

                    case ComponentType.OptionSet:
                        GenerateDescriptionOptionSet(builder, components);
                        break;

                    case ComponentType.ManagedProperty:
                        GenerateDescriptionManagedProperty(builder, components);
                        break;

                    case ComponentType.SystemForm:
                        GenerateDescriptionSystemForm(builder, components);
                        break;

                    case ComponentType.SavedQuery:
                        GenerateDescriptionSavedQuery(builder, components);
                        break;

                    case ComponentType.SavedQueryVisualization:
                        GenerateDescriptionSavedQueryVisualization(builder, components);
                        break;

                    case ComponentType.SiteMap:
                        GenerateDescriptionSiteMap(builder, components);
                        break;

                    case ComponentType.CustomControl:
                        GenerateDescriptionCustomControl(builder, components);
                        break;

                    case ComponentType.CustomControlDefaultConfig:
                        GenerateDescriptionCustomControlDefaultConfig(builder, components);
                        break;

                    case ComponentType.CustomControlResource:
                        GenerateDescriptionCustomControlResource(builder, components);
                        break;

                    case ComponentType.SLA:
                        GenerateDescriptionSLA(builder, components);
                        break;

                    case ComponentType.SLAItem:
                        GenerateDescriptionSLAItem(builder, components);
                        break;

                    case ComponentType.SimilarityRule:
                        GenerateDescriptionSimilarityRule(builder, components);
                        break;

                    case ComponentType.ServiceEndpoint:
                        GenerateDescriptionServiceEndpoint(builder, components);
                        break;

                    case ComponentType.MobileOfflineProfile:
                        GenerateDescriptionMobileOfflineProfile(builder, components);
                        break;

                    case ComponentType.MobileOfflineProfileItem:
                        GenerateDescriptionMobileOfflineProfileItem(builder, components);
                        break;

                    case ComponentType.EntityMap:
                        GenerateDescriptionEntityMap(builder, components);
                        break;

                    case ComponentType.AttributeMap:
                        GenerateDescriptionAttributeMap(builder, components);
                        break;

                    case ComponentType.AppModule:
                        GenerateDescriptionAppModule(builder, components);
                        break;

                    case ComponentType.AppModuleRoles:
                        GenerateDescriptionAppModuleRoles(builder, components);
                        break;

                    case ComponentType.ChannelAccessProfile:
                        GenerateDescriptionChannelAccessProfile(builder, components);
                        break;

                    case ComponentType.ChannelAccessProfileEntityAccessLevel:
                        GenerateDescriptionChannelAccessProfileEntityAccessLevel(builder, components);
                        break;

                    case ComponentType.ProcessTrigger:
                        GenerateDescriptionProcessTrigger(builder, components);
                        break;

                    case ComponentType.DependencyFeature:
                        GenerateDescriptionDependencyFeature(builder, components);
                        break;

                    //case ComponentType.Relationship:
                    //    break;
                    //case ComponentType.ViewAttribute:
                    //    break;
                    //case ComponentType.LocalizedLabel:
                    //    break;
                    //case ComponentType.RelationshipExtraCondition:
                    //    break;
                    //case ComponentType.EntityRelationshipRole:
                    //    break;
                    //case ComponentType.EntityRelationshipRelationships:
                    //    break;
                    //case ComponentType.Form:
                    //    break;
                    //case ComponentType.Organization:
                    //    break;
                    //case ComponentType.Attachment:
                    //    break;
                    //case ComponentType.DuplicateRule:
                    //    break;
                    //case ComponentType.DuplicateRuleCondition:
                    //    break;

                    default:
                        useDefault = true;
                        break;
                }
            }

            if (useDefault)
            {
                builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();

                foreach (var item in components)
                {
                    builder.AppendFormat(formatSpacer, item.ToString()).AppendLine();
                }
            }
        }

        public string GetComponentDescription(int type, Guid idEntity)
        {
            var component = new SolutionComponent()
            {
                ObjectId = idEntity,
                ComponentType = new OptionSetValue(type),
            };

            var listEntities = GetEntities<Entity>(type, new Guid?[] { idEntity });

            var entity = listEntities.FirstOrDefault();

            if (SolutionComponent.IsDefinedComponentType(type))
            {
                ComponentType componentType = (ComponentType)type;

                switch (componentType)
                {
                    case ComponentType.Entity:
                        return GenerateDescriptionEntitySingle(component);

                    case ComponentType.Attribute:
                    case ComponentType.AttributePicklistValue:
                    case ComponentType.AttributeLookupValue:
                    case ComponentType.EntityKeyAttribute:
                        return GenerateDescriptionAttributeSingle(component);

                    case ComponentType.EntityRelationship:
                        return GenerateDescriptionEntityRelationshipSingle(component);

                    case ComponentType.EntityKey:
                        return GenerateDescriptionEntityKeySingle(component);

                    case ComponentType.OptionSet:
                        return GenerateDescriptionOptionSetSingle(component);

                    case ComponentType.ManagedProperty:
                        return GenerateDescriptionManagedPropertySingle(component);

                    case ComponentType.WebResource:
                        return GenerateDescriptionWebResourcesSingle(entity.ToEntity<WebResource>(), component);

                    case ComponentType.Role:
                        return GenerateDescriptionRolesSingle(entity.ToEntity<Role>(), component);

                    case ComponentType.RolePrivileges:
                        return GenerateDescriptionRolePrivilegesSinge(entity.ToEntity<RolePrivileges>(), component);

                    case ComponentType.SavedQuery:
                        return GenerateDescriptionSavedQuerySingle(entity.ToEntity<SavedQuery>(), component);

                    case ComponentType.Workflow:
                        return GenerateDescriptionWorkflowSingle(entity.ToEntity<Workflow>(), component);

                    case ComponentType.ConnectionRole:
                        return GenerateDescriptionConnectionRoleSingle(entity.ToEntity<ConnectionRole>(), component);

                    case ComponentType.SystemForm:
                        return GenerateDescriptionSystemFormSingle(entity.ToEntity<SystemForm>(), component);

                    case ComponentType.SavedQueryVisualization:
                        return GenerateDescriptionSavedQueryVisualizationSingle(entity.ToEntity<SavedQueryVisualization>(), component);

                    case ComponentType.Report:
                        return GenerateDescriptionReportSingle(entity.ToEntity<Report>(), component);

                    case ComponentType.ReportEntity:
                        return GenerateDescriptionReportEntitySingle(entity.ToEntity<ReportEntity>(), component);

                    case ComponentType.ReportCategory:
                        return GenerateDescriptionReportCategorySingle(entity.ToEntity<ReportCategory>(), component);

                    case ComponentType.ReportVisibility:
                        return GenerateDescriptionReportVisibilitySingle(entity.ToEntity<ReportVisibility>(), component);

                    case ComponentType.ConvertRule:
                        return GenerateDescriptionConvertRuleSingle(entity.ToEntity<ConvertRule>(), component);

                    case ComponentType.ConvertRuleItem:
                        return GenerateDescriptionConvertRuleItemSingle(entity.ToEntity<ConvertRuleItem>(), component);

                    case ComponentType.EmailTemplate:
                        return GenerateDescriptionEmailTemplateSingle(entity.ToEntity<Template>(), component);

                    case ComponentType.MailMergeTemplate:
                        return GenerateDescriptionMailMergeTemplateSingle(entity.ToEntity<MailMergeTemplate>(), component);

                    case ComponentType.KBArticleTemplate:
                        return GenerateDescriptionKBArticleTemplateSingle(entity.ToEntity<KbArticleTemplate>(), component);

                    case ComponentType.ContractTemplate:
                        return GenerateDescriptionContractTemplateSingle(entity.ToEntity<ContractTemplate>(), component);

                    case ComponentType.RibbonCustomization:
                        return GenerateDescriptionRibbonCustomizationSingle(entity.ToEntity<RibbonCustomization>(), component);

                    case ComponentType.RibbonCommand:
                        return GenerateDescriptionRibbonCommandSingle(entity.ToEntity<RibbonCommand>(), component);

                    case ComponentType.RibbonContextGroup:
                        return GenerateDescriptionRibbonContextGroupSingle(entity.ToEntity<RibbonContextGroup>(), component);

                    case ComponentType.RibbonDiff:
                        return GenerateDescriptionRibbonDiffSingle(entity.ToEntity<RibbonDiff>(), component);

                    case ComponentType.RibbonRule:
                        return GenerateDescriptionRibbonRuleSingle(entity.ToEntity<RibbonRule>(), component);

                    case ComponentType.RibbonTabToCommandMap:
                        return GenerateDescriptionRibbonTabToCommandMapSingle(entity.ToEntity<RibbonTabToCommandMap>(), component);

                    case ComponentType.FieldSecurityProfile:
                        return GenerateDescriptionFieldSecurityProfileSingle(entity.ToEntity<FieldSecurityProfile>(), component);

                    case ComponentType.FieldPermission:
                        return GenerateDescriptionFieldPermissionSingle(entity.ToEntity<FieldPermission>(), component);

                    case ComponentType.RoutingRule:
                        return GenerateDescriptionRoutingRuleSingle(entity.ToEntity<RoutingRule>(), component);

                    case ComponentType.RoutingRuleItem:
                        return GenerateDescriptionRoutingRuleItemSingle(entity.ToEntity<RoutingRuleItem>(), component);

                    case ComponentType.HierarchyRule:
                        return GenerateDescriptionHierarchyRuleSingle(entity.ToEntity<HierarchyRule>(), component);

                    case ComponentType.DisplayString:
                        return GenerateDescriptionDisplayStringSingle(entity.ToEntity<DisplayString>(), component);

                    case ComponentType.DisplayStringMap:
                        return GenerateDescriptionDisplayStringMapSingle(entity.ToEntity<DisplayStringMap>(), component);

                    case ComponentType.PluginAssembly:
                        return GenerateDescriptionPluginAssemblySingle(entity.ToEntity<PluginAssembly>(), component);

                    case ComponentType.PluginType:
                        return GenerateDescriptionPluginTypeSingle(entity.ToEntity<PluginType>(), component);

                    case ComponentType.SDKMessageProcessingStep:
                        return GenerateDescriptionSDKMessageProcessingStepSingle(entity.ToEntity<SdkMessageProcessingStep>(), component);

                    case ComponentType.SDKMessageProcessingStepImage:
                        return GenerateDescriptionSDKMessageProcessingStepImageSingle(entity.ToEntity<SdkMessageProcessingStepImage>(), component);

                    case ComponentType.SiteMap:
                        return GenerateDescriptionSiteMapSingle(entity.ToEntity<SiteMap>(), component);

                    case ComponentType.CustomControl:
                        return GenerateDescriptionCustomControlSingle(entity.ToEntity<CustomControl>(), component);

                    case ComponentType.CustomControlDefaultConfig:
                        return GenerateDescriptionCustomControlDefaultConfigSingle(entity.ToEntity<CustomControlDefaultConfig>(), component);

                    case ComponentType.CustomControlResource:
                        return GenerateDescriptionCustomControlResourceSingle(entity.ToEntity<CustomControlResource>(), component);

                    case ComponentType.SLA:
                        return GenerateDescriptionSLASingle(entity.ToEntity<SLA>(), component);

                    case ComponentType.SLAItem:
                        return GenerateDescriptionSLAItemSingle(entity.ToEntity<SLAItem>(), component);

                    case ComponentType.SimilarityRule:
                        return GenerateDescriptionSimilarityRuleSingle(entity.ToEntity<SimilarityRule>(), component);

                    case ComponentType.ServiceEndpoint:
                        return GenerateDescriptionServiceEndpointSingle(entity.ToEntity<ServiceEndpoint>(), component);

                    case ComponentType.MobileOfflineProfile:
                        return GenerateDescriptionMobileOfflineProfileSingle(entity.ToEntity<MobileOfflineProfile>(), component);

                    case ComponentType.MobileOfflineProfileItem:
                        return GenerateDescriptionMobileOfflineProfileItemSingle(entity.ToEntity<MobileOfflineProfileItem>(), component);

                    case ComponentType.EntityMap:
                        return GenerateDescriptionEntityMapSingle(entity.ToEntity<EntityMap>(), component);

                    case ComponentType.AttributeMap:
                        return GenerateDescriptionAttributeMapSingle(entity.ToEntity<AttributeMap>(), component);

                    case ComponentType.AppModule:
                        return GenerateDescriptionAppModuleSingle(entity.ToEntity<AppModule>(), component);

                    case ComponentType.AppModuleRoles:
                        return GenerateDescriptionAppModuleRolesSingle(entity.ToEntity<AppModuleRoles>(), component);

                    case ComponentType.ChannelAccessProfile:
                        return GenerateDescriptionChannelAccessProfileSingle(entity.ToEntity<ChannelAccessProfile>(), component);

                    case ComponentType.ChannelAccessProfileEntityAccessLevel:
                        return GenerateDescriptionChannelAccessProfileEntityAccessLevelSingle(entity.ToEntity<ChannelAccessProfileEntityAccessLevel>(), component);

                    case ComponentType.ProcessTrigger:
                        return GenerateDescriptionProcessTriggerSingle(entity.ToEntity<ProcessTrigger>(), component);

                    case ComponentType.DependencyFeature:
                        return GenerateDescriptionDependencyFeatureSingle(component);
                }
            }

            return component.ToString();
        }

        #region Generate Description.

        private void GenerateDescriptionDisplayString(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<DisplayString>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Key", "LanguageCode", "Published", "Custom", "CustomComment", "FormatParameters", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var displayStringKey = entity.DisplayStringKey;
                var languageCode = entity.LanguageCode;
                var publishedDisplayString = entity.PublishedDisplayString;
                var customDisplayString = entity.CustomDisplayString;
                var customComment = entity.CustomComment;
                var formatParameters = entity.FormatParameters;

                handler.AddLine(displayStringKey
                    , LanguageLocale.GetLocaleName(languageCode.Value)
                    , publishedDisplayString
                    , customDisplayString
                    , customComment
                    , formatParameters.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionDisplayStringSingle(DisplayString entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string displaystringkey = entity.DisplayStringKey;

                string customdisplaystring = entity.CustomDisplayString;

                StringBuilder str = new StringBuilder();

                str.AppendFormat("DisplayString {0}", displaystringkey);

                if (!string.IsNullOrEmpty(customdisplaystring))
                {
                    str.AppendFormat(" - {0}", customdisplaystring);
                }

                str.AppendFormat(" - IsManaged {0}", entity.IsManaged.ToString());

                str.AppendFormat(" - SolutionName {0}", EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename"));

                return str.ToString();
            }

            return component.ToString();
        }

        private void GenerateDescriptionDisplayStringMap(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<DisplayStringMap>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("DisplayStringKey", "ObjectTypeCode", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var displayStringKey = entity.GetAttributeValue<AliasedValue>("displaystring.displaystringkey").Value.ToString();

                handler.AddLine(displayStringKey
                    , entity.ObjectTypeCode
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionDisplayStringMapSingle(DisplayStringMap entity, SolutionComponent component)
        {
            if (entity != null)
            {
                var displayStringKey = entity.GetAttributeValue<AliasedValue>("displaystring.displaystringkey").Value.ToString();

                return string.Format("DisplayStringKey {0}    ObjectTypeCode {1}    IsManaged {2}    SolutionName {3}"
                    , displayStringKey
                    , entity.ObjectTypeCode
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionWorkflow(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<Workflow>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();

            handler.SetHeader("Entity", "Category", "Name", "Type", "Scope", "Mode", "StatusCode", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Url");

            foreach (var entity in list
                    .OrderBy(entity => entity.PrimaryEntity)
                    .ThenBy(entity => entity.Category?.Value)
                    .ThenBy(entity => entity.Name)
                    .ThenBy(entity =>
                    {
                        var op = entity.BusinessProcessType;
                        return (op != null) ? (int?)op.Value : null;
                    })
                )
            {
                CreateOneWorkFlowDescription(entity, handler);
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionWorkflowSingle(Workflow entity, SolutionComponent component)
        {
            if (entity != null)
            {
                StringBuilder builder = new StringBuilder();

                string primaryentity = entity.PrimaryEntity;
                string name = entity.Name;
                string category = string.Empty;
                string businessprocesstype = string.Empty;
                string scope = string.Empty;
                string mode = string.Empty;

                builder.AppendFormat("Workflow {0}", primaryentity);

                if (entity.Contains(Workflow.Schema.Attributes.category) && entity.Attributes[Workflow.Schema.Attributes.category] != null)
                {
                    category = entity.FormattedValues[Workflow.Schema.Attributes.category];

                    builder.AppendFormat(" {0}", category);
                }

                builder.AppendFormat("    {0}", name);

                var uniqueName = entity.UniqueName;

                if (!string.IsNullOrEmpty(uniqueName))
                {
                    builder.AppendFormat("    UniqueName {0}", uniqueName);
                }

                if (entity.Contains(Workflow.Schema.Attributes.businessprocesstype) && entity.Attributes[Workflow.Schema.Attributes.businessprocesstype] != null)
                {
                    businessprocesstype = entity.FormattedValues[Workflow.Schema.Attributes.businessprocesstype];

                    builder.AppendFormat("    {0}", businessprocesstype);
                }

                if (entity.Contains(Workflow.Schema.Attributes.scope) && entity.Attributes[Workflow.Schema.Attributes.scope] != null)
                {
                    scope = entity.FormattedValues[Workflow.Schema.Attributes.scope];

                    builder.AppendFormat("    {0}", scope);
                }

                if (entity.Contains(Workflow.Schema.Attributes.mode) && entity.Attributes[Workflow.Schema.Attributes.mode] != null)
                {
                    mode = entity.FormattedValues[Workflow.Schema.Attributes.mode];

                    builder.AppendFormat("    {0}", mode);
                }

                builder.AppendFormat("    {0}", entity.FormattedValues[Workflow.Schema.Attributes.statuscode]);

                builder.AppendFormat("    IsManaged {0}", entity.IsManaged.ToString());

                builder.AppendFormat("    SolutionName {0}", EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename"));

                if (_withUrls)
                {
                    builder.AppendFormat("    Url {0}", _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.Workflow, entity.Id, null, null));
                }

                return builder.ToString();
            }

            return component.ToString();
        }

        private void CreateOneWorkFlowDescription(Workflow entity, FormatTextTableHandler handler)
        {
            //"workflowid", "name", "uniquename", "businessprocesstype", "category", "mode", "primaryentity", "scope"

            string name = entity.Name;

            var uniqueName = entity.UniqueName;

            if (!string.IsNullOrEmpty(uniqueName))
            {
                name += string.Format("    (UniqueName \"{0}\")", uniqueName);
            }

            string primaryentity = entity.PrimaryEntity;
            string category = string.Empty;
            string businessprocesstype = string.Empty;
            string scope = string.Empty;
            string mode = string.Empty;
            string statusCodeString = entity.FormattedValues[Workflow.Schema.Attributes.statuscode];

            if (entity.Contains(Workflow.Schema.Attributes.category) && entity.Attributes[Workflow.Schema.Attributes.category] != null)
            {
                //var option = entity.Category.Value;

                //category = GetWorkFlowCategory(option);

                category = entity.FormattedValues[Workflow.Schema.Attributes.category];
            }

            //if (entity.Contains("uniquename") && entity.Attributes["uniquename"] != null)
            //{
            //    string uniquename = entity.UniqueName;

            //    if (!string.IsNullOrEmpty(uniquename))
            //    {
            //        builder.AppendFormat("; UniqueName: '{0}'", uniquename);
            //    }
            //}

            if (entity.Contains(Workflow.Schema.Attributes.businessprocesstype) && entity.Attributes[Workflow.Schema.Attributes.businessprocesstype] != null)
            {
                //var option = entity.BusinessProcessType.Value;

                //businessprocesstype = GetWorkFlowBusinessProcessType(option);

                businessprocesstype = entity.FormattedValues[Workflow.Schema.Attributes.businessprocesstype];
            }

            if (entity.Contains(Workflow.Schema.Attributes.scope) && entity.Attributes[Workflow.Schema.Attributes.scope] != null)
            {
                scope = entity.FormattedValues[Workflow.Schema.Attributes.scope];
            }

            if (entity.Contains(Workflow.Schema.Attributes.mode) && entity.Attributes[Workflow.Schema.Attributes.mode] != null)
            {
                mode = entity.FormattedValues[Workflow.Schema.Attributes.mode];
            }

            handler.AddLine(primaryentity
                , category
                , name
                , businessprocesstype
                , scope
                , mode
                , statusCodeString
                , entity.IsManaged.ToString()
                , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                , _withUrls ? _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.Workflow, entity.Id, null, null) : string.Empty
            );
        }

        private void GenerateDescriptionWebResources(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<WebResource>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("WebResourceType", "Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Url");

            foreach (var webResource in list)
            {
                string webTypeName = string.Format("'{0}'", webResource.FormattedValues[WebResource.Schema.Attributes.webresourcetype]);

                string name = webResource.Name;

                handler.AddLine(webTypeName
                    , name
                    , webResource.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(webResource, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(webResource, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(webResource, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(webResource, "suppsolution.ismanaged")
                    , _withUrls ? _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.WebResource, webResource.Id, null, null) : string.Empty
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionWebResourcesSingle(WebResource webResource, SolutionComponent component)
        {
            if (webResource != null)
            {
                string webTypeName = string.Format("'{0}'", webResource.FormattedValues[WebResource.Schema.Attributes.webresourcetype]);

                return string.Format("WebResource     '{0}'    WebResourceType '{1}'    IsManaged {2}    SolutionName {3}{4}"
                    , webResource.Name
                    , webTypeName
                    , webResource.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(webResource, "solution.uniquename")
                    , _withUrls ? string.Format("    Url {0}", _service.ConnectionData.GetSolutionComponentUrl(ComponentType.WebResource, webResource.Id, null, null)) : string.Empty
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionSiteMap(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<SiteMap>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("SiteMapName", "Id", "IsAppAware", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var sitemap in list)
            {
                string name = sitemap.SiteMapName;

                handler.AddLine(
                    name
                    , sitemap.Id.ToString()
                    , sitemap.IsAppAware.ToString()
                    , sitemap.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(sitemap, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(sitemap, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(sitemap, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(sitemap, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionSiteMapSingle(SiteMap entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string name = entity.SiteMapName;

                return string.Format("SiteMapName {0}    Id {1}    IsAppAware {2}    IsManaged {3}    SolutionName {4}"
                    , name
                    , entity.Id.ToString()
                    , entity.IsAppAware.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionCustomControl(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<CustomControl>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "CompatibleDataTypes", "Manifest", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.CompatibleDataTypes
                    , entity.Manifest
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionCustomControlSingle(CustomControl entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("Name {0}    CompatibleDataTypes {1}    Manifest {2}    IsManaged {3}    SolutionName {4}"
                    , entity.Name
                    , entity.CompatibleDataTypes
                    , entity.Manifest
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionCustomControlDefaultConfig(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<CustomControlDefaultConfig>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("EntityName", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.PrimaryEntityTypeCode
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionCustomControlDefaultConfigSingle(CustomControlDefaultConfig entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("EntityName {0}    Id {1}    IsManaged {2}    SolutionName {3}"
                    , entity.PrimaryEntityTypeCode
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionSLA(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<SLA>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Entity", "Name", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.FormattedValues.ContainsKey(SLA.Schema.Attributes.objecttypecode) ? entity.FormattedValues[SLA.Schema.Attributes.objecttypecode] : entity.ObjectTypeCode?.Value.ToString()
                    , entity.Name
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionSLASingle(SLA entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("Entity {0}    Name {1}    Id {2}    IsManaged {3}    SolutionName {4}"
                    , entity.FormattedValues.ContainsKey(SLA.Schema.Attributes.objecttypecode) ? entity.FormattedValues[SLA.Schema.Attributes.objecttypecode] : entity.ObjectTypeCode?.Value.ToString()
                    , entity.Name
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionSLAItem(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<SLAItem>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("SLA", "Name", "RelatedField", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.SLAId.Name
                    , entity.Name
                    , entity.RelatedField
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionSLAItemSingle(SLAItem entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("SLA {0}    Name {1}    RelatedField {2}    Id {3}    IsManaged {4}    SolutionName {5}"
                    , entity.SLAId.Name
                    , entity.Name
                    , entity.RelatedField
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionServiceEndpoint(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<ServiceEndpoint>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "ConnectionMode", "Contract", "MessageFormat", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.connectionmode]
                    , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.contract]
                    , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.messageformat]
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionServiceEndpointSingle(ServiceEndpoint entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("Name {0}    ConnectionMode {1}    Contract {2}    MessageFormat {3}    IsManaged {4}    SolutionName {5}"
                    , entity.Name
                    , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.connectionmode]
                    , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.contract]
                    , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.messageformat]
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionMobileOfflineProfile(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<MobileOfflineProfile>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "SelectedEntityMetadata", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.SelectedEntityMetadata
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionMobileOfflineProfileSingle(MobileOfflineProfile entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("Name {0}    SelectedEntityMetadata {1}    Id {2}    IsManaged {3}    SolutionName {4}"
                    , entity.Name
                    , entity.SelectedEntityMetadata
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionMobileOfflineProfileItem(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<MobileOfflineProfileItem>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "SelectedEntityMetadata", "EntityObjectTypeCode", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.SelectedEntityTypeCode
                    , entity.EntityObjectTypeCode.ToString()
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionMobileOfflineProfileItemSingle(MobileOfflineProfileItem entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("Name {0}    SelectedEntityMetadata {1}    EntityObjectTypeCode {2}    Id {3}    IsManaged {4}    SolutionName {5}"
                    , entity.Name
                    , entity.SelectedEntityTypeCode
                    , entity.EntityObjectTypeCode.ToString()
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionAppModule(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<AppModule>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "UniqueName", "URL", "AppModuleVersion", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.UniqueName
                    , entity.URL
                    , entity.AppModuleVersion
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionAppModuleSingle(AppModule entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("Name {0}    UniqueName {1}    URL {2}    AppModuleVersion {3}    Id {4}    IsManaged {5}    SolutionName {6}"
                    , entity.Name
                    , entity.UniqueName
                    , entity.URL
                    , entity.AppModuleVersion
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionChannelAccessProfile(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<ChannelAccessProfile>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "StatusCode", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.FormattedValues[ChannelAccessProfile.Schema.Attributes.statuscode]
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionChannelAccessProfileSingle(ChannelAccessProfile entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("Name {0}    StatusCode {1}    Id {2}    IsManaged {3}    SolutionName {4}"
                    , entity.Name
                    , entity.FormattedValues[ChannelAccessProfile.Schema.Attributes.statuscode]
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionChannelAccessProfileEntityAccessLevel(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<ChannelAccessProfileEntityAccessLevel>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("ChannelAccessProfileName", "EntityAccessLevelName", "EntityAccessLevelDepthMask", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(entity, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid + "." + ChannelAccessProfile.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid + "." + Privilege.Schema.Attributes.name)
                    , entity.EntityAccessLevelDepthMask.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionChannelAccessProfileEntityAccessLevelSingle(ChannelAccessProfileEntityAccessLevel entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("ChannelAccessProfileName {0}    EntityAccessLevelName {1}    EntityAccessLevelDepthMask {2}    IsManaged {3}    SolutionName {4}"
                    , EntityDescriptionHandler.GetAttributeString(entity, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid + "." + ChannelAccessProfile.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid + "." + Privilege.Schema.Attributes.name)
                    , entity.EntityAccessLevelDepthMask
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionAppModuleRoles(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<AppModuleRoles>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("AppModuleName", "RoleName", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(entity, AppModuleRoles.Schema.Attributes.appmoduleid + "." + AppModule.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, AppModuleRoles.Schema.Attributes.roleid + "." + Role.Schema.Attributes.name)
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionAppModuleRolesSingle(AppModuleRoles entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("AppModuleName {0}    RoleName {1}    IsManaged {2}    SolutionName {3}"
                    , EntityDescriptionHandler.GetAttributeString(entity, AppModuleRoles.Schema.Attributes.appmoduleid + "." + AppModule.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, AppModuleRoles.Schema.Attributes.roleid + "." + Role.Schema.Attributes.name)
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionProcessTrigger(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<ProcessTrigger>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("PrimaryEntityTypeCode", "ProcessName", "Event", "PipelineStage", "FormName", "Scope", "MethodId", "ControlName", "ControlType", "IsCustomizable", "IsManaged"
                , "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string pipelinestage = null;
                entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.pipelinestage, out pipelinestage);

                string scope = null;
                entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.scope, out scope);

                string controltype = null;
                entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.controltype, out controltype);

                handler.AddLine(
                    entity?.PrimaryEntityTypeCode
                    , entity?.ProcessId?.Name
                    , entity?.Event
                    , pipelinestage
                    , entity?.FormId?.Name
                    , scope
                    , entity?.MethodId?.ToString()
                    , entity?.ControlName
                    , controltype
                    , entity?.IsCustomizable?.Value.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionProcessTriggerSingle(ProcessTrigger entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string pipelinestage = null;
                entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.pipelinestage, out pipelinestage);

                string scope = null;
                entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.scope, out scope);

                string controltype = null;
                entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.controltype, out controltype);

                return string.Format("PrimaryEntityTypeCode {0}    ProcessName {1}    Event {2}    PipelineStage {3}        FormName {4}        Scope {5}        MethodId {6}        ControlName {7}        ControlType {8}        IsManaged {9}        SolutionName {10}"
                    , entity?.PrimaryEntityTypeCode
                    , entity?.ProcessId?.Name
                    , entity?.Event
                    , pipelinestage
                    , entity?.FormId?.Name
                    , scope
                    , entity?.MethodId?.ToString()
                    , entity?.ControlName
                    , controltype
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionCustomControlResource(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<CustomControlResource>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("ControlName", "Name", "WebResourceName", "WebResourceType", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.customcontrolid + "." + AppModule.Schema.Attributes.name)
                    , entity.Name
                    , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.webresourcetype)
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionCustomControlResourceSingle(CustomControlResource entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("ControlName {0}    Name {1}    WebResourceName {2}    WebResourceType {3}    Id {4}    IsManaged {5}    SolutionName {6}"
                    , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.customcontrolid + "." + AppModule.Schema.Attributes.name)
                    , entity.Name
                    , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.webresourcetype)
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionDependencyFeature(StringBuilder builder, List<SolutionComponent> components)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("DependencyFeatureId", "Behaviour");

            foreach (var comp in components)
            {
                string behaviorName = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behaviorName = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                handler.AddLine(comp.ObjectId.ToString(), string.Empty, behaviorName);
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionDependencyFeatureSingle(SolutionComponent component)
        {
            return string.Format("DependencyFeatureId {0}"
                        , component.ObjectId.Value.ToString()
                        );
        }

        #endregion Generate Description.

        internal string GetCustomizableName(SolutionComponent component)
        {
            if (component.ComponentType == null || !component.ObjectId.HasValue)
            {
                return null;
            }

            var type = component.ComponentType.Value;

            if (SolutionComponent.IsDefinedComponentType(type))
            {
                ComponentType componentType = (ComponentType)type;

                switch (componentType)
                {
                    case ComponentType.Entity:
                        {
                            EntityMetadata metaData = GetEntityMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.IsCustomizable.Value.ToString();
                            }
                        }
                        break;

                    case ComponentType.Attribute:
                    case ComponentType.AttributePicklistValue:
                    case ComponentType.AttributeLookupValue:
                    case ComponentType.EntityKeyAttribute:
                        {
                            AttributeMetadata metaData = GetAttributeMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.IsCustomizable.Value.ToString();
                            }

                        }
                        break;

                    case ComponentType.EntityRelationship:
                        {
                            RelationshipMetadataBase metaData = GetRelationshipMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.IsCustomizable.Value.ToString();
                            }
                        }
                        break;

                    case ComponentType.EntityKey:
                        {
                            EntityKeyMetadata metaData = GetEntityKeyMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.IsCustomizable.Value.ToString();
                            }
                        }
                        break;

                    case ComponentType.OptionSet:
                        {
                            if (this.AllOptionSetMetadata.Any())
                            {
                                if (this.AllOptionSetMetadata.ContainsKey(component.ObjectId.Value))
                                {
                                    var optionSet = this.AllOptionSetMetadata[component.ObjectId.Value];

                                    return optionSet.IsCustomizable.Value.ToString();
                                }
                            }
                        }
                        break;

                    //case ComponentType.ManagedProperty:
                    //    if (this.AllManagedProperties.Any())
                    //    {
                    //        if (this.AllManagedProperties.ContainsKey(component.ObjectId.Value))
                    //        {
                    //            var managedProperty = this.AllManagedProperties[component.ObjectId.Value];

                    //            return managedProperty.IsCustomizable.Value.ToString();
                    //        }
                    //    }
                    //    break;

                    case ComponentType.DependencyFeature:
                        return null;
                }
            }

            var listEntities = GetEntities<Entity>(component.ComponentType.Value, new Guid?[] { component.ObjectId.Value });

            var entity = listEntities.FirstOrDefault();

            if (entity != null)
            {
                if (entity.Contains("iscustomizable") && entity.Attributes["iscustomizable"] is BooleanManagedProperty booleanManagedProperty)
                {
                    return booleanManagedProperty.Value.ToString();
                }

                if (entity.Contains("iscustomizable") && entity.Attributes["iscustomizable"] is bool boolValue)
                {
                    return boolValue.ToString();
                }
            }

            return null;
        }

        internal string GetManagedName(SolutionComponent component)
        {
            if (component.ComponentType == null || !component.ObjectId.HasValue)
            {
                return null;
            }

            var type = component.ComponentType.Value;

            if (SolutionComponent.IsDefinedComponentType(type))
            {
                ComponentType componentType = (ComponentType)type;

                switch (componentType)
                {
                    case ComponentType.Entity:
                        {
                            EntityMetadata metaData = GetEntityMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.IsManaged.ToString();
                            }
                        }
                        break;

                    case ComponentType.Attribute:
                    case ComponentType.AttributePicklistValue:
                    case ComponentType.AttributeLookupValue:
                    case ComponentType.EntityKeyAttribute:
                        {
                            AttributeMetadata metaData = GetAttributeMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.IsManaged.ToString();
                            }

                        }
                        break;

                    case ComponentType.EntityRelationship:
                        {
                            RelationshipMetadataBase metaData = GetRelationshipMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.IsManaged.ToString();
                            }
                        }
                        break;

                    case ComponentType.EntityKey:
                        {
                            EntityKeyMetadata metaData = GetEntityKeyMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.IsManaged.ToString();
                            }
                        }
                        break;

                    case ComponentType.OptionSet:
                        {
                            if (this.AllOptionSetMetadata.Any())
                            {
                                if (this.AllOptionSetMetadata.ContainsKey(component.ObjectId.Value))
                                {
                                    var optionSet = this.AllOptionSetMetadata[component.ObjectId.Value];

                                    return optionSet.IsManaged.ToString();
                                }
                            }
                        }
                        break;

                    //case ComponentType.ManagedProperty:
                    //    if (this.AllManagedProperties.Any())
                    //    {
                    //        if (this.AllManagedProperties.ContainsKey(component.ObjectId.Value))
                    //        {
                    //            var managedProperty = this.AllManagedProperties[component.ObjectId.Value];

                    //            return managedProperty.ManagedPropertyType;
                    //        }
                    //    }
                    //    break;

                    case ComponentType.DependencyFeature:
                        return null;
                }
            }

            var listEntities = GetEntities<Entity>(component.ComponentType.Value, new Guid?[] { component.ObjectId.Value });

            var entity = listEntities.FirstOrDefault();

            if (entity != null)
            {
                if (entity.Contains("ismanaged"))
                {
                    if (entity.FormattedValues.ContainsKey("ismanaged"))
                    {
                        return entity.FormattedValues["ismanaged"];
                    }
                    else
                    {
                        return entity.GetAttributeValue<bool?>("ismanaged").ToString();
                    }
                }
            }

            return null;
        }

        internal string GetDisplayName(SolutionComponent component)
        {
            if (component.ComponentType == null || !component.ObjectId.HasValue)
            {
                return null;
            }

            var type = component.ComponentType.Value;

            if (SolutionComponent.IsDefinedComponentType(type))
            {
                ComponentType componentType = (ComponentType)type;

                switch (componentType)
                {
                    case ComponentType.Entity:
                        {
                            EntityMetadata metaData = GetEntityMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.DisplayName?.UserLocalizedLabel?.Label;
                            }
                        }
                        break;

                    case ComponentType.Attribute:
                    case ComponentType.AttributePicklistValue:
                    case ComponentType.AttributeLookupValue:
                    case ComponentType.EntityKeyAttribute:
                        {
                            AttributeMetadata metaData = GetAttributeMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.DisplayName?.UserLocalizedLabel?.Label;
                            }

                        }
                        break;

                    case ComponentType.EntityRelationship:
                        {
                            RelationshipMetadataBase metaData = GetRelationshipMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                if (metaData is OneToManyRelationshipMetadata)
                                {
                                    var relationship = metaData as OneToManyRelationshipMetadata;

                                    return relationship.AssociatedMenuConfiguration?.Label?.UserLocalizedLabel?.Label;
                                }
                                else if (metaData is ManyToManyRelationshipMetadata)
                                {
                                    var relationship = metaData as ManyToManyRelationshipMetadata;

                                    return (relationship.Entity1AssociatedMenuConfiguration ?? relationship.Entity1AssociatedMenuConfiguration)?.Label?.UserLocalizedLabel?.Label;
                                }
                            }
                        }
                        break;

                    case ComponentType.EntityKey:
                        {
                            EntityKeyMetadata metaData = GetEntityKeyMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.DisplayName?.UserLocalizedLabel?.Label;
                            }
                        }
                        break;

                    case ComponentType.OptionSet:
                        {
                            if (this.AllOptionSetMetadata.Any())
                            {
                                if (this.AllOptionSetMetadata.ContainsKey(component.ObjectId.Value))
                                {
                                    var optionSet = this.AllOptionSetMetadata[component.ObjectId.Value];

                                    return optionSet.DisplayName?.UserLocalizedLabel?.Label;
                                }
                            }
                        }
                        break;

                    case ComponentType.ManagedProperty:
                        if (this.AllManagedProperties.Any())
                        {
                            if (this.AllManagedProperties.ContainsKey(component.ObjectId.Value))
                            {
                                var managedProperty = this.AllManagedProperties[component.ObjectId.Value];

                                return managedProperty.DisplayName?.UserLocalizedLabel?.Label;
                            }
                        }
                        break;

                    case ComponentType.DependencyFeature:
                        return null;
                }
            }

            var listEntities = GetEntities<Entity>(component.ComponentType.Value, new Guid?[] { component.ObjectId.Value });

            var entity = listEntities.FirstOrDefault();

            if (entity != null)
            {
                if (SolutionComponent.IsDefinedComponentType(type))
                {
                    ComponentType componentType = (ComponentType)type;

                    switch (componentType)
                    {
                        case ComponentType.WebResource:
                            return entity.ToEntity<WebResource>().DisplayName;

                        case ComponentType.Workflow:
                            return entity.ToEntity<Workflow>().UniqueName;

                        case ComponentType.RolePrivileges:
                            return SecurityRolePrivilegesRepository.GetPrivilegeDepthMaskName(entity.ToEntity<RolePrivileges>().PrivilegeDepthMask.Value);

                        case ComponentType.SystemForm:
                            {
                                var ent = entity.ToEntity<SystemForm>();

                                return ent.FormattedValues[SystemForm.Schema.Attributes.type];
                            }

                        case ComponentType.SavedQuery:
                            {
                                var ent = entity.ToEntity<SavedQuery>();

                                return SavedQueryRepository.GetQueryTypeName(ent.QueryType.Value);
                            }

                        case ComponentType.Report:
                            return entity.ToEntity<Report>().FileName;
                    }
                }
            }

            return null;
        }

        internal string GetName(SolutionComponent component)
        {
            if (component.ComponentType == null || !component.ObjectId.HasValue)
            {
                return null;
            }

            var type = component.ComponentType.Value;

            if (SolutionComponent.IsDefinedComponentType(type))
            {
                ComponentType componentType = (ComponentType)type;

                switch (componentType)
                {
                    case ComponentType.Entity:
                        {
                            EntityMetadata metaData = GetEntityMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return metaData.LogicalName;
                            }
                        }
                        break;

                    case ComponentType.Attribute:
                    case ComponentType.AttributePicklistValue:
                    case ComponentType.AttributeLookupValue:
                    case ComponentType.EntityKeyAttribute:
                        {
                            AttributeMetadata metaData = GetAttributeMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName);
                            }

                        }
                        break;

                    case ComponentType.EntityRelationship:
                        {
                            RelationshipMetadataBase metaData = GetRelationshipMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                if (metaData is OneToManyRelationshipMetadata)
                                {
                                    var relationship = metaData as OneToManyRelationshipMetadata;

                                    return string.Format("{0}.{1}.{2}"
                                        , relationship.ReferencingEntity
                                        , relationship.ReferencingAttribute
                                        , relationship.SchemaName
                                        );
                                }
                                else if (metaData is ManyToManyRelationshipMetadata)
                                {
                                    var relationship = metaData as ManyToManyRelationshipMetadata;

                                    return string.Format("{0} - {1}.{2}", relationship.Entity1LogicalName, relationship.Entity2LogicalName, relationship.SchemaName);
                                }
                            }
                        }
                        break;

                    case ComponentType.EntityKey:
                        {
                            EntityKeyMetadata metaData = GetEntityKeyMetadata(component.ObjectId.Value);

                            if (metaData != null)
                            {
                                return string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName);
                            }
                        }
                        break;

                    case ComponentType.OptionSet:
                        {
                            if (this.AllOptionSetMetadata.Any())
                            {
                                if (this.AllOptionSetMetadata.ContainsKey(component.ObjectId.Value))
                                {
                                    var optionSet = this.AllOptionSetMetadata[component.ObjectId.Value];

                                    return optionSet.Name;
                                }
                            }
                        }
                        break;

                    case ComponentType.ManagedProperty:
                        if (this.AllManagedProperties.Any())
                        {
                            if (this.AllManagedProperties.ContainsKey(component.ObjectId.Value))
                            {
                                var managedProperty = this.AllManagedProperties[component.ObjectId.Value];

                                return managedProperty.LogicalName;
                            }
                        }
                        break;

                    case ComponentType.DependencyFeature:
                        return component.ObjectId.Value.ToString();
                }
            }

            if (SolutionComponent.IsDefinedComponentType(type))
            {
                var listEntities = GetEntities<Entity>(component.ComponentType.Value, new Guid?[] { component.ObjectId.Value });

                var entity = listEntities.FirstOrDefault();

                if (entity != null)
                {
                    ComponentType componentType = (ComponentType)type;

                    switch (componentType)
                    {
                        case ComponentType.WebResource:
                            return entity.ToEntity<WebResource>().Name;

                        case ComponentType.Role:
                            return entity.ToEntity<Role>().Name;

                        case ComponentType.RolePrivileges:
                            return entity.ToEntity<RolePrivileges>().GetAttributeValue<AliasedValue>("privilege.name")?.Value?.ToString();

                        case ComponentType.Workflow:
                            return entity.ToEntity<Workflow>().Name;

                        case ComponentType.ConnectionRole:
                            return entity.ToEntity<ConnectionRole>().Name;

                        case ComponentType.SystemForm:
                            {
                                var ent = entity.ToEntity<SystemForm>();

                                return string.Format("{0} - {1}", ent.ObjectTypeCode, ent.Name);
                            }

                        case ComponentType.SavedQuery:
                            {
                                var ent = entity.ToEntity<SavedQuery>();

                                return string.Format("{0} - {1}", ent.ReturnedTypeCode, ent.Name);
                            }

                        case ComponentType.SavedQueryVisualization:
                            {
                                var ent = entity.ToEntity<SavedQueryVisualization>();

                                return string.Format("{0} - {1}", ent.PrimaryEntityTypeCode, ent.Name);
                            }

                        case ComponentType.Report:
                            return entity.ToEntity<Report>().Name;

                        case ComponentType.ReportEntity:
                            {
                                var ent = entity.ToEntity<ReportEntity>();

                                return string.Format("{0} - {1}", ent.ReportId?.Name, ent.ObjectTypeCode);
                            }

                        case ComponentType.ReportCategory:
                            {
                                var ent = entity.ToEntity<ReportCategory>();

                                return string.Format("{0} - {1}", ent.ReportId?.Name, ent.FormattedValues.ContainsKey(ReportCategory.Schema.Attributes.categorycode) ? ent.FormattedValues[ReportCategory.Schema.Attributes.categorycode] : ent.CategoryCode?.Value.ToString());
                            }

                        case ComponentType.ReportVisibility:
                            {
                                var ent = entity.ToEntity<ReportVisibility>();

                                return string.Format("{0} - {1}", ent.ReportId?.Name, ent.FormattedValues.ContainsKey(ReportVisibility.Schema.Attributes.visibilitycode) ? ent.FormattedValues[ReportVisibility.Schema.Attributes.visibilitycode] : ent.VisibilityCode?.Value.ToString());
                            }

                        case ComponentType.ConvertRule:
                            return entity.ToEntity<ConvertRule>().Name;

                        case ComponentType.ConvertRuleItem:
                            return entity.ToEntity<ConvertRuleItem>().Name;

                        case ComponentType.EmailTemplate:
                            return entity.ToEntity<Template>().Title;

                        case ComponentType.MailMergeTemplate:
                            return entity.ToEntity<MailMergeTemplate>().Name;

                        case ComponentType.KBArticleTemplate:
                            return entity.ToEntity<KbArticleTemplate>().Title;

                        case ComponentType.ContractTemplate:
                            return entity.ToEntity<ContractTemplate>().Name;

                        case ComponentType.RibbonCustomization:
                            return entity.ToEntity<RibbonCustomization>().Entity ?? "ApplicationRibbon";

                        case ComponentType.RibbonCommand:
                            {
                                var ent = entity.ToEntity<RibbonCommand>();

                                return string.Format("{0} - {1}", ent.Entity ?? "ApplicationRibbon", ent.Command);
                            }

                        case ComponentType.RibbonContextGroup:
                            {
                                var ent = entity.ToEntity<RibbonContextGroup>();

                                return string.Format("{0} - {1}", ent.Entity ?? "ApplicationRibbon", ent.ContextGroupId);
                            }

                        case ComponentType.RibbonDiff:
                            {
                                var ent = entity.ToEntity<RibbonDiff>();

                                return string.Format("{0} - {1}", ent.Entity ?? "ApplicationRibbon", ent.DiffId);
                            }

                        case ComponentType.RibbonRule:
                            {
                                var ent = entity.ToEntity<RibbonRule>();

                                return string.Format("{0} - {1}", ent.Entity ?? "ApplicationRibbon", ent.RuleId);
                            }

                        case ComponentType.RibbonTabToCommandMap:
                            {
                                var ent = entity.ToEntity<RibbonTabToCommandMap>();

                                return string.Format("{0} - {1}", ent.Entity ?? "ApplicationRibbon", ent.TabId);
                            }

                        case ComponentType.FieldSecurityProfile:
                            return entity.ToEntity<FieldSecurityProfile>().Name;

                        case ComponentType.FieldPermission:
                            {
                                var ent = entity.ToEntity<FieldPermission>();

                                return string.Format("{0}.{1}", ent.EntityName, ent.AttributeLogicalName);
                            }

                        case ComponentType.RoutingRule:
                            return entity.ToEntity<RoutingRule>().Name;

                        case ComponentType.RoutingRuleItem:
                            return entity.ToEntity<RoutingRuleItem>().Name;

                        case ComponentType.HierarchyRule:
                            return entity.ToEntity<HierarchyRule>().Name;

                        case ComponentType.DisplayString:
                            return entity.ToEntity<DisplayString>().DisplayStringKey;

                        case ComponentType.DisplayStringMap:
                            return entity.ToEntity<DisplayStringMap>().Id.ToString();

                        case ComponentType.PluginAssembly:
                            return entity.ToEntity<PluginAssembly>().Name;

                        case ComponentType.PluginType:
                            return entity.ToEntity<PluginType>().TypeName;

                        case ComponentType.SDKMessageProcessingStep:
                            return entity.ToEntity<SdkMessageProcessingStep>().Name;

                        case ComponentType.SDKMessageProcessingStepImage:
                            return entity.ToEntity<SdkMessageProcessingStepImage>().EntityAlias;

                        case ComponentType.SiteMap:
                            return entity.ToEntity<SiteMap>().SiteMapNameUnique ?? entity.ToEntity<SiteMap>().Id.ToString();

                        case ComponentType.CustomControl:
                            return entity.ToEntity<CustomControl>().Name;

                        case ComponentType.CustomControlDefaultConfig:
                            return entity.ToEntity<CustomControlDefaultConfig>().Id.ToString();

                        case ComponentType.CustomControlResource:
                            {
                                var ent = entity.ToEntity<CustomControlResource>();

                                return string.Format("{0} - {1} - {2} - {3}"
                                    , EntityDescriptionHandler.GetAttributeString(ent, CustomControlResource.Schema.Attributes.customcontrolid + "." + CustomControl.Schema.Attributes.name)
                                    , ent.Name
                                    , EntityDescriptionHandler.GetAttributeString(ent, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.name)
                                    , EntityDescriptionHandler.GetAttributeString(ent, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.webresourcetype)
                                );
                            }

                        case ComponentType.SLA:
                            return entity.ToEntity<SLA>().Name;

                        case ComponentType.SLAItem:
                            return entity.ToEntity<SLAItem>().Name;

                        case ComponentType.SimilarityRule:
                            return entity.ToEntity<SimilarityRule>().Name;

                        case ComponentType.ServiceEndpoint:
                            return entity.ToEntity<ServiceEndpoint>().Name;

                        case ComponentType.MobileOfflineProfile:
                            return entity.ToEntity<MobileOfflineProfile>().Name;

                        case ComponentType.MobileOfflineProfileItem:
                            return entity.ToEntity<MobileOfflineProfileItem>().Name;

                        case ComponentType.EntityMap:
                            {
                                var ent = entity.ToEntity<EntityMap>();

                                return string.Format("{0} -> {1}"
                                    , ent.SourceEntityName
                                    , ent.TargetEntityName
                                    );
                            }

                        case ComponentType.AttributeMap:
                            {
                                var ent = entity.ToEntity<AttributeMap>();

                                return string.Format("{0}.{1} -> {2}.{3}"
                                    , ent.GetAttributeValue<AliasedValue>("entitymap.sourceentityname")?.Value?.ToString()
                                    , ent.SourceAttributeName
                                    , ent.GetAttributeValue<AliasedValue>("entitymap.targetentityname")?.Value?.ToString()
                                    , ent.TargetAttributeName
                                    );
                            }

                        case ComponentType.AppModule:
                            {
                                var ent = entity.ToEntity<AppModule>();

                                return ent.UniqueName ?? ent.Name ?? ent.Id.ToString();
                            }

                        case ComponentType.AppModuleRoles:
                            {
                                var ent = entity.ToEntity<AppModuleRoles>();

                                return string.Format("{0} - {1}"
                                    , ent.GetAttributeValue<AliasedValue>("entitymap.sourceentityname")?.Value?.ToString()
                                    , ent.GetAttributeValue<AliasedValue>("entitymap.targetentityname")?.Value?.ToString()
                                    );
                            }

                        case ComponentType.ChannelAccessProfile:
                            {
                                var ent = entity.ToEntity<ChannelAccessProfile>();

                                return ent.Name;
                            }

                        case ComponentType.ChannelAccessProfileEntityAccessLevel:
                            {
                                var ent = entity.ToEntity<ChannelAccessProfileEntityAccessLevel>();

                                return string.Format("{0} - {1} - {2}"
                                    , EntityDescriptionHandler.GetAttributeString(ent, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid + "." + ChannelAccessProfile.Schema.Attributes.name)
                                    , EntityDescriptionHandler.GetAttributeString(ent, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid + "." + Privilege.Schema.Attributes.name)
                                    , ent.EntityAccessLevelDepthMask
                                );
                            }

                        case ComponentType.ProcessTrigger:
                            {
                                var ent = entity.ToEntity<ProcessTrigger>();

                                return string.Format("{0} {1}"
                                    , ent.PrimaryEntityTypeCode
                                    , ent.ProcessId?.Name
                                    );
                            }
                    }
                }
            }

            return "Unknown";
        }

        public string GetFileName(string connectionName, int type, Guid objectId, string fieldTitle, string extension)
        {
            if (SolutionComponent.IsDefinedComponentType(type))
            {
                ComponentType componentType = (ComponentType)type;

                switch (componentType)
                {
                    case ComponentType.Entity:
                        {
                            EntityMetadata metaData = GetEntityMetadata(objectId);

                            if (metaData != null)
                            {
                                return metaData.LogicalName;
                            }
                        }
                        break;

                    case ComponentType.Attribute:
                    case ComponentType.AttributePicklistValue:
                    case ComponentType.AttributeLookupValue:
                    case ComponentType.EntityKeyAttribute:
                        {
                            AttributeMetadata metaData = GetAttributeMetadata(objectId);

                            if (metaData != null)
                            {
                                return string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName);
                            }

                        }
                        break;

                    case ComponentType.EntityRelationship:
                        {
                            RelationshipMetadataBase metaData = GetRelationshipMetadata(objectId);

                            if (metaData != null)
                            {
                                if (metaData is OneToManyRelationshipMetadata)
                                {
                                    var relationship = metaData as OneToManyRelationshipMetadata;

                                    return string.Format("{0}.{1}.{2}"
                                        , relationship.ReferencingEntity
                                        , relationship.ReferencingAttribute
                                        , relationship.SchemaName
                                        );
                                }
                                else if (metaData is ManyToManyRelationshipMetadata)
                                {
                                    var relationship = metaData as ManyToManyRelationshipMetadata;

                                    return string.Format("{0} - {1}.{2}", relationship.Entity1LogicalName, relationship.Entity2LogicalName, relationship.SchemaName);
                                }
                            }
                        }
                        break;

                    case ComponentType.EntityKey:
                        {
                            EntityKeyMetadata metaData = GetEntityKeyMetadata(objectId);

                            if (metaData != null)
                            {
                                return string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName);
                            }
                        }
                        break;

                    case ComponentType.OptionSet:
                        {
                            if (this.AllOptionSetMetadata.Any())
                            {
                                if (this.AllOptionSetMetadata.ContainsKey(objectId))
                                {
                                    var optionSet = this.AllOptionSetMetadata[objectId];

                                    return optionSet.Name;
                                }
                            }
                        }
                        break;

                    case ComponentType.ManagedProperty:
                        if (this.AllManagedProperties.Any())
                        {
                            if (this.AllManagedProperties.ContainsKey(objectId))
                            {
                                var managedProperty = this.AllManagedProperties[objectId];

                                return managedProperty.LogicalName;
                            }
                        }
                        break;
                }
            }

            var listEntities = GetEntities<Entity>(type, new Guid?[] { objectId });

            var entity = listEntities.FirstOrDefault();

            if (entity != null)
            {
                return EntityFileNameFormatter.GetEntityName(connectionName, entity, fieldTitle, extension);
            }

            return string.Format("{0}.ComponentType {1} - {2} - {3}.{4}", connectionName, type, objectId, fieldTitle, extension);
        }

        public static TupleList<string, string> GetComponentColumns(int componentType)
        {
            if (SolutionComponent.IsDefinedComponentType(componentType))
            {
                ComponentType componentTypeEnum = (ComponentType)componentType;

                if (SolutionComponent.IsComponentTypeMetadata(componentTypeEnum))
                {
                    return new TupleList<string, string>();
                }

                switch (componentTypeEnum)
                {
                    case ComponentType.Role:
                        return new TupleList<string, string>
                        {
                            {  Role.Schema.Attributes.name, "Name" }
                            , { Role.Schema.Attributes.businessunitid, "BusinessUnit" }
                            , { Role.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { Role.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.RolePrivileges:
                        return new TupleList<string, string>
                        {
                            { "role.name", "Name" }
                            , { "role.businessunitid", "BusinessUnit" }
                            , { "privilege.name", "Privilege" }
                            , { RolePrivileges.Schema.Attributes.privilegedepthmask, "PrivilegeDepthMask" }
                            , { RolePrivileges.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.SavedQuery:
                        return new TupleList<string, string>
                        {
                            {  SavedQuery.Schema.Attributes.returnedtypecode, "EntityName" }
                            , { SavedQuery.Schema.Attributes.name, "Name" }
                            , { SavedQuery.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { SavedQuery.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.SavedQueryVisualization:
                        return new TupleList<string, string>
                        {
                            {  SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, "PrimaryEntityTypeCode" }
                            , { SavedQueryVisualization.Schema.Attributes.name, "Name" }
                            , { SavedQueryVisualization.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { SavedQueryVisualization.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.Workflow:
                        return new TupleList<string, string>
                        {
                            { Workflow.Schema.Attributes.primaryentity, "EntityName" }
                            , { Workflow.Schema.Attributes.category, "Category" }
                            , { Workflow.Schema.Attributes.name, "Name" }
                            , { Workflow.Schema.Attributes.uniquename, "UniqueName" }
                            , { Workflow.Schema.Attributes.businessprocesstype, "Type" }
                            , { Workflow.Schema.Attributes.scope, "Scope" }
                            , { Workflow.Schema.Attributes.mode, "Mode" }
                            , { Workflow.Schema.Attributes.statuscode, "StatusCode" }
                            , { Workflow.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { Workflow.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ConnectionRole:
                        return new TupleList<string, string>
                        {
                            {  ConnectionRole.Schema.Attributes.name, "Name" }
                            , { ConnectionRole.Schema.Attributes.category, "Category" }
                            , { ConnectionRole.Schema.Attributes.statuscode, "StatusCode" }
                            , { ConnectionRole.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { ConnectionRole.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.SystemForm:
                        return new TupleList<string, string>
                        {
                            {  SystemForm.Schema.Attributes.objecttypecode, "EntityName" }
                            , { SystemForm.Schema.Attributes.type, "FormType" }
                            , { SystemForm.Schema.Attributes.name, "Name" }
                            , { SystemForm.Schema.Attributes.uniquename, "UniqueName" }
                            , { SystemForm.Schema.Attributes.formactivationstate, "State" }
                            , { SystemForm.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { SystemForm.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.Report:
                        return new TupleList<string, string>
                        {
                            { Report.Schema.Attributes.name, "Name" }
                            , { Report.Schema.Attributes.filename, "FileName" }
                            , { Report.Schema.Attributes.reporttypecode, "ReportType" }
                            , { Report.Schema.Attributes.ispersonal, "ViewableBy" }
                            , { Report.Schema.Attributes.ownerid, "Owner" }
                            , { Report.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { Report.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ReportEntity:
                        return new TupleList<string, string>
                        {
                            { ReportEntity.Schema.Attributes.reportid, "ReportName" }
                            , { ReportEntity.Schema.Attributes.objecttypecode, "ReportRelatedEntity" }
                            , { ReportEntity.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { ReportEntity.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ReportCategory:
                        return new TupleList<string, string>
                        {
                            { ReportCategory.Schema.Attributes.reportid, "ReportName" }
                            , { ReportCategory.Schema.Attributes.categorycode, "Category" }
                            , { ReportCategory.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { ReportCategory.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ReportVisibility:
                        return new TupleList<string, string>
                        {
                            {  ReportVisibility.Schema.Attributes.reportid, "ReportName" }
                            , { ReportVisibility.Schema.Attributes.visibilitycode, "Visibility" }
                            , { ReportVisibility.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { ReportVisibility.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ConvertRule:
                        return new TupleList<string, string>
                        {
                            { ConvertRule.Schema.Attributes.name, "Name" }
                            , { ConvertRule.Schema.Attributes.statuscode, "StatusCode" }
                            , { ConvertRule.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ConvertRuleItem:
                        return new TupleList<string, string>
                        {
                            { ConvertRuleItem.Schema.Attributes.convertruleid, "ConvertRuleName" }
                            , { ConvertRuleItem.Schema.Attributes.name, "Name" }
                            , { ConvertRuleItem.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.EmailTemplate:
                        return new TupleList<string, string>
                        {
                            { Template.Schema.Attributes.templatetypecode, "TemplateTypeCode" }
                            , { Template.Schema.Attributes.title, "Title" }
                            , { Template.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { Template.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.MailMergeTemplate:
                        return new TupleList<string, string>
                        {
                            { MailMergeTemplate.Schema.Attributes.templatetypecode, "TemplateTypeCode" }
                            , { MailMergeTemplate.Schema.Attributes.name, "Name" }
                            , { MailMergeTemplate.Schema.Attributes.mailmergetype, "MailMergeType" }
                            , { MailMergeTemplate.Schema.Attributes.statuscode, "StatusCode" }
                            , { MailMergeTemplate.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { MailMergeTemplate.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.KBArticleTemplate:
                        return new TupleList<string, string>
                        {
                            { KbArticleTemplate.Schema.Attributes.title, "Title" }
                            , { KbArticleTemplate.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { KbArticleTemplate.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ContractTemplate:
                        return new TupleList<string, string>
                        {
                            { ContractTemplate.Schema.Attributes.name, "Name" }
                            , { ContractTemplate.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { ContractTemplate.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.RibbonCustomization:
                        return new TupleList<string, string>
                        {
                            { RibbonCustomization.Schema.Attributes.entity, "Entity" }
                            , { RibbonCustomization.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.RibbonCommand:
                        return new TupleList<string, string>
                        {
                            { RibbonCommand.Schema.Attributes.entity, "Entity" }
                            , { RibbonCommand.Schema.Attributes.command, "Command" }
                            , { RibbonCommand.Schema.Attributes.ismanaged, "IsManaged" }
                            , { RibbonCommand.Schema.EntityPrimaryIdAttribute, "" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.RibbonContextGroup:
                        return new TupleList<string, string>
                        {
                            { RibbonContextGroup.Schema.Attributes.entity, "Entity" }
                            , { RibbonContextGroup.Schema.Attributes.contextgroupid, "ContextGroupId" }
                            , { RibbonContextGroup.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.RibbonDiff:
                        return new TupleList<string, string>
                        {
                            { RibbonDiff.Schema.Attributes.entity, "Entity" }
                            , { RibbonDiff.Schema.Attributes.diffid, "DiffId" }
                            , { RibbonDiff.Schema.Attributes.difftype, "DiffType" }
                            , { RibbonDiff.Schema.Attributes.tabid, "TabId" }
                            , { RibbonDiff.Schema.Attributes.contextgroupid, "ContextGroupId" }
                            , { RibbonDiff.Schema.Attributes.sequence, "Sequence" }
                            , { RibbonDiff.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.RibbonRule:
                        return new TupleList<string, string>
                        {
                            { RibbonRule.Schema.Attributes.entity, "Entity" }
                            , { RibbonRule.Schema.Attributes.ruletype, "RuleType" }
                            , { RibbonRule.Schema.Attributes.ruleid, "RuleId" }
                            , { RibbonRule.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.RibbonTabToCommandMap:
                        return new TupleList<string, string>
                        {
                            { RibbonTabToCommandMap.Schema.Attributes.entity, "Entity" }
                            , { RibbonTabToCommandMap.Schema.Attributes.tabid, "TabId" }
                            , { RibbonTabToCommandMap.Schema.Attributes.controlid, "ControlId" }
                            , { RibbonTabToCommandMap.Schema.Attributes.command, "Command" }
                            , { RibbonTabToCommandMap.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.FieldSecurityProfile:
                        return new TupleList<string, string>
                        {
                            { FieldSecurityProfile.Schema.Attributes.name, "Name" }
                            , { FieldSecurityProfile.Schema.Attributes.description, "Description" }
                            , { FieldSecurityProfile.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.FieldPermission:
                        return new TupleList<string, string>
                        {
                            { "fieldsecurityprofile.name", "FieldSecurityProfile" }
                            , { FieldPermission.Schema.Attributes.entityname, "Entity" }
                            , { FieldPermission.Schema.Attributes.attributelogicalname, "Attribute" }
                            , { FieldPermission.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.RoutingRule:
                        return new TupleList<string, string>
                        {
                            { RoutingRule.Schema.Attributes.name, "Name" }
                            , { RoutingRule.Schema.Attributes.workflowid, "Workflow" }
                            , { RoutingRule.Schema.Attributes.statuscode, "StatusCode" }
                            , { RoutingRule.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.RoutingRuleItem:
                        return new TupleList<string, string>
                        {
                            {  RoutingRuleItem.Schema.Attributes.routingruleid, "RoutingRuleName" }
                            , { RoutingRuleItem.Schema.Attributes.name, "Name" }
                            , { RoutingRuleItem.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.HierarchyRule:
                        return new TupleList<string, string>
                        {
                            { HierarchyRule.Schema.Attributes.primaryentitylogicalname, "Entity" }
                            , { HierarchyRule.Schema.Attributes.name, "Name" }
                            , { HierarchyRule.Schema.Attributes.description, "Description" }
                            , { HierarchyRule.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { HierarchyRule.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.DisplayString:
                        return new TupleList<string, string>
                        {
                            { DisplayString.Schema.Attributes.displaystringkey, "Key" }
                            , { DisplayString.Schema.Attributes.languagecode, "LanguageCode" }
                            , { DisplayString.Schema.Attributes.publisheddisplaystring, "Published" }
                            , { DisplayString.Schema.Attributes.customdisplaystring, "Custom" }
                            , { DisplayString.Schema.Attributes.customcomment, "CustomComment" }
                            , { DisplayString.Schema.Attributes.formatparameters, "FormatParameters" }
                            , { DisplayString.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.DisplayStringMap:
                        return new TupleList<string, string>
                        {
                            { "displaystring.displaystringkey", "DisplayStringKey" }
                            , { DisplayStringMap.Schema.Attributes.objecttypecode, "Entity" }
                            , { DisplayStringMap.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.PluginAssembly:
                        return new TupleList<string, string>
                        {
                            { PluginAssembly.Schema.Attributes.name, "Name" }
                            , { PluginAssembly.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { PluginAssembly.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.PluginType:
                        return new TupleList<string, string>
                        {
                            { PluginType.Schema.Attributes.assemblyname, "AssemblyName" }
                            , { PluginType.Schema.Attributes.typename, "TypeName" }
                            , { PluginType.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.SDKMessageProcessingStep:
                        return new TupleList<string, string>
                        {
                            { SdkMessageProcessingStep.Schema.Attributes.eventhandler, "TypeName" }
                            , { SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode, "PrimaryObjectTypeCode" }
                            , { SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode, "SecondaryObjectTypeCode" }
                            , { SdkMessageProcessingStep.Schema.Attributes.sdkmessageid, "Message" }
                            , { SdkMessageProcessingStep.Schema.Attributes.stage, "Stage" }
                            , { SdkMessageProcessingStep.Schema.Attributes.mode, "Mode" }
                            , { SdkMessageProcessingStep.Schema.Attributes.rank, "Rank" }
                            , { SdkMessageProcessingStep.Schema.Attributes.statuscode, "StatusCode" }
                            , { SdkMessageProcessingStep.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { SdkMessageProcessingStep.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                            , { SdkMessageProcessingStep.Schema.Attributes.filteringattributes, "" }
                        };

                    case ComponentType.SDKMessageProcessingStepImage:
                        return new TupleList<string, string>
                        {
                            { "sdkmessageprocessingstep.eventhandler", "TypeName" }
                            , { SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode, "PrimaryObjectTypeCode" }
                            , { SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode, "SecondaryObjectTypeCode" }
                            , { "sdkmessageprocessingstep.sdkmessageid", "Message" }
                            , { "sdkmessageprocessingstep.stage", "Stage" }
                            , { "sdkmessageprocessingstep.mode", "Mode" }
                            , { "sdkmessageprocessingstep.rank", "Rank" }
                            , { "sdkmessageprocessingstep.statuscode", "StatusCode" }
                            , { SdkMessageProcessingStepImage.Schema.Attributes.imagetype, "ImageType" }
                            , { SdkMessageProcessingStepImage.Schema.Attributes.name, "Name" }
                            , { SdkMessageProcessingStepImage.Schema.Attributes.entityalias, "EntityAlias" }
                            , { SdkMessageProcessingStepImage.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { SdkMessageProcessingStepImage.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                            , { SdkMessageProcessingStepImage.Schema.Attributes.attributes, "" }
                        };

                    case ComponentType.SiteMap:
                        return new TupleList<string, string>
                        {
                            { SiteMap.Schema.EntityPrimaryIdAttribute, "Id" }
                            , { SiteMap.Schema.Attributes.sitemapname, "Name" }
                            , { SiteMap.Schema.Attributes.sitemapnameunique, "NameUnique" }
                            , { SiteMap.Schema.Attributes.isappaware, "IsAppware" }
                            , { SiteMap.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.CustomControl:
                        return new TupleList<string, string>
                        {
                            { CustomControl.Schema.Attributes.name, "Name" }
                            , { CustomControl.Schema.Attributes.compatibledatatypes, "CompatibleDataTypes" }
                            , { CustomControl.Schema.Attributes.manifest, "Manifest" }
                            , { CustomControl.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.CustomControlDefaultConfig:
                        return new TupleList<string, string>
                        {
                            { CustomControlDefaultConfig.Schema.Attributes.primaryentitytypecode, "PrimaryEntityTypeCode" }
                            , { CustomControlDefaultConfig.Schema.EntityPrimaryIdAttribute, "Id" }
                            , { CustomControlDefaultConfig.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.CustomControlResource:
                        return new TupleList<string, string>
                        {
                            { CustomControlResource.Schema.Attributes.customcontrolid + "." + CustomControl.Schema.Attributes.name, "ControlName" }
                            , { CustomControlResource.Schema.Attributes.name, "Name" }
                            , { CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.name, "WebResourceName" }
                            , { CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.webresourcetype, "WebResourceType" }
                            , { CustomControlResource.Schema.EntityPrimaryIdAttribute, "Id" }
                            , { CustomControlResource.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.SLA:
                        return new TupleList<string, string>
                        {
                            { SLA.Schema.Attributes.objecttypecode, "Entity" }
                            , { SLA.Schema.Attributes.name, "Name" }
                            , { SLA.Schema.EntityPrimaryIdAttribute, "Id" }
                            , { SLA.Schema.Attributes.statuscode, "StatusCode" }
                            , { SLA.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.SLAItem:
                        return new TupleList<string, string>
                        {
                            { SLAItem.Schema.Attributes.slaid, "SLA" }
                            , { SLAItem.Schema.Attributes.name, "Name" }
                            , { SLAItem.Schema.Attributes.relatedfield, "RelatedField" }
                            , { SLAItem.Schema.EntityPrimaryIdAttribute, "Id" }
                            , { SLAItem.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.SimilarityRule:
                        return new TupleList<string, string>
                        {
                            { SimilarityRule.Schema.Attributes.baseentityname, "Entity" }
                            , { SimilarityRule.Schema.Attributes.name, "Name" }
                            , { SimilarityRule.Schema.Attributes.matchingentityname, "MatchingEntityName" }
                            , { SimilarityRule.Schema.EntityPrimaryIdAttribute, "Id" }
                            , { SimilarityRule.Schema.Attributes.statuscode, "StatusCode" }
                            , { SimilarityRule.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ServiceEndpoint:
                        return new TupleList<string, string>
                        {
                            { ServiceEndpoint.Schema.Attributes.name, "Name" }
                            , { ServiceEndpoint.Schema.Attributes.connectionmode, "ConnectionMode" }
                            , { ServiceEndpoint.Schema.Attributes.contract, "Contract" }
                            , { ServiceEndpoint.Schema.Attributes.messageformat, "MessageFormat" }
                            , { ServiceEndpoint.Schema.Attributes.namespaceformat, "NamespaceFormat" }
                            , { ServiceEndpoint.Schema.Attributes.namespaceaddress, "NamespaceAddress" }
                            , { ServiceEndpoint.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { ServiceEndpoint.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.MobileOfflineProfile:
                        return new TupleList<string, string>
                        {
                            { MobileOfflineProfile.Schema.Attributes.name, "Name" }
                            , { MobileOfflineProfile.Schema.Attributes.selectedentitymetadata, "SelectedEntityMetadata" }
                            , { MobileOfflineProfile.Schema.EntityPrimaryIdAttribute, "Id" }
                            , { MobileOfflineProfile.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.MobileOfflineProfileItem:
                        return new TupleList<string, string>
                        {
                            { MobileOfflineProfileItem.Schema.Attributes.name, "Name" }
                            , { MobileOfflineProfileItem.Schema.Attributes.selectedentitytypecode, "SelectedEntityTypeCode" }
                            , { MobileOfflineProfileItem.Schema.Attributes.entityobjecttypecode, "EntityObjectTypeCode" }
                            , { MobileOfflineProfileItem.Schema.EntityPrimaryIdAttribute, "Id" }
                            , { MobileOfflineProfileItem.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.EntityMap:
                        return new TupleList<string, string>
                        {
                            { EntityMap.Schema.Attributes.sourceentityname, "SourceEntityName" }
                            , { EntityMap.Schema.Attributes.targetentityname, "TargetEntityName" }
                            , { EntityMap.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.AttributeMap:
                        return new TupleList<string, string>
                        {
                            { "entitymap.sourceentityname", "SourceEntityName" }
                            , { AttributeMap.Schema.Attributes.sourceattributename, "SourceAttributeName" }
                            , { "entitymap.targetentityname", "TargetEntityName" }
                            , { AttributeMap.Schema.Attributes.targetattributename, "TargetAttributeName" }
                            , { AttributeMap.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.WebResource:
                        return new TupleList<string, string>
                        {
                            { WebResource.Schema.Attributes.name, "Name" }
                            , { WebResource.Schema.Attributes.webresourcetype, "Type" }
                            , { WebResource.Schema.Attributes.languagecode, "LanguageCode" }
                            , { WebResource.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { WebResource.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.AppModule:
                        return new TupleList<string, string>
                        {
                            { AppModule.Schema.Attributes.name, "Name" }
                            , { AppModule.Schema.Attributes.uniquename, "UniqueName" }
                            , { AppModule.Schema.Attributes.url, "URL" }
                            , { AppModule.Schema.Attributes.appmoduleversion, "AppModuleVersion" }
                            , { AppModule.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.AppModuleRoles:
                        return new TupleList<string, string>
                        {
                            { AppModuleRoles.Schema.Attributes.appmoduleid + "." + AppModule.Schema.Attributes.name, "AppModuleName" }
                            , { AppModuleRoles.Schema.Attributes.roleid + "." + Role.Schema.Attributes.name, "RoleName" }
                            , { AppModuleRoles.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ChannelAccessProfile:
                        return new TupleList<string, string>
                        {
                            { ChannelAccessProfile.Schema.Attributes.name, "Name" }
                            , { ChannelAccessProfile.Schema.Attributes.statuscode, "StatusCode" }
                            , { ChannelAccessProfile.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ChannelAccessProfileEntityAccessLevel:
                        return new TupleList<string, string>
                        {
                            { ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid + "." + ChannelAccessProfile.Schema.Attributes.name, "ChannelAccessProfileName" }
                            , { ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid + "." + Privilege.Schema.Attributes.name, "EntityAccessLevel" }
                            , { ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccessleveldepthmask, "EntityAccessLevelDepthMask" }
                            , { ChannelAccessProfileEntityAccessLevel.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };

                    case ComponentType.ProcessTrigger:
                        return new TupleList<string, string>
                        {
                            { ProcessTrigger.Schema.Attributes.primaryentitytypecode, "PrimaryEntityTypeCode" }
                            , { ProcessTrigger.Schema.Attributes.processid, "ProcessName" }
                            , { ProcessTrigger.Schema.Attributes.Event, "Event" }
                            , { ProcessTrigger.Schema.Attributes.pipelinestage, "PipelineStage" }
                            , { ProcessTrigger.Schema.Attributes.formid, "FormName" }
                            , { ProcessTrigger.Schema.Attributes.scope, "Scope" }
                            , { ProcessTrigger.Schema.Attributes.methodid, "MethodId" }
                            , { ProcessTrigger.Schema.Attributes.controlname, "ControlName" }
                            , { ProcessTrigger.Schema.Attributes.controltype, "ControlType" }
                            , { ProcessTrigger.Schema.Attributes.iscustomizable, "IsCustomizable" }
                            , { ProcessTrigger.Schema.Attributes.ismanaged, "IsManaged" }
                            , { "solution.uniquename", "SolutionName" }
                            , { "solution.ismanaged", "SolutionIsManaged" }
                            , { "suppsolution.uniquename", "SupportingName" }
                            , { "suppsolution.ismanaged", "SupportingIsManaged" }
                        };
                }
            }

            return new TupleList<string, string>();
        }
    }
}