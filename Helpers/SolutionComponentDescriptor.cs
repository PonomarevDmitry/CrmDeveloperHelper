using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
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

        private ConcurrentDictionary<int, ISolutionComponentDescriptionBuilder> _cacheBuilders = new ConcurrentDictionary<int, ISolutionComponentDescriptionBuilder>();

        private readonly IWriteToOutput _iWriteToOutput;
        private readonly IOrganizationServiceExtented _service;
        private readonly bool _withUrls;

        public SolutionComponentMetadataSource MetadataSource { get; private set; }

        /// <summary>
        /// Конструктор репозитория функция по поиску решений.
        /// </summary>
        /// <param name="service"></param>
        public SolutionComponentDescriptor(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, bool withUrls)
            : this(iWriteToOutput, service, withUrls, null)
        {

        }

        public SolutionComponentDescriptor(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, bool withUrls, SolutionComponentMetadataSource metadataSource)
        {
            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._withUrls = withUrls;
            this.MetadataSource = metadataSource;

            if (this.MetadataSource == null)
            {
                this.MetadataSource = new SolutionComponentMetadataSource(_service);
            }
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
                    if (builder.Length > 0)
                    {
                        builder.AppendLine();
                    }

                    string name = components.First().ComponentTypeName;

                    builder.AppendFormat("ComponentType:   {0} ({1})            Count: {2}"
                        , name
                        , gr.Key.ToString()
                        , gr.Count().ToString()
                        ).AppendLine();

                    var descriptionBuilder = GetDescriptionBuilder(gr.Key);

                    descriptionBuilder.GenerateDescription(builder, gr, _withUrls);
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

        public Task<List<SolutionImageComponent>> GetSolutionImageComponentsAsync(IEnumerable<SolutionComponent> components)
        {
            return Task.Run(() => GetSolutionImageComponents(components));
        }

        private List<SolutionImageComponent> GetSolutionImageComponents(IEnumerable<SolutionComponent> components)
        {
            List<SolutionImageComponent> result = new List<SolutionImageComponent>();

            var groups = components.GroupBy(comp => comp.ComponentType.Value).OrderBy(gr => gr.Key);

            foreach (var gr in groups)
            {
                try
                {
                    var descriptionBuilder = GetDescriptionBuilder(gr.Key);

                    foreach (var item in gr)
                    {
                        descriptionBuilder.FillSolutionImageComponent(result, item);
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }

            return result;
        }

        public string GetComponentDescription(int type, Guid idEntity)
        {
            var solutionComponent = new SolutionComponent()
            {
                ObjectId = idEntity,
                ComponentType = new OptionSetValue(type),
            };

            var descriptionBuilder = GetDescriptionBuilder(type);

            return descriptionBuilder.GenerateDescriptionSingle(solutionComponent, _withUrls);
        }

        public string GetCustomizableName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || solutionComponent.ComponentType == null || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            var descriptionBuilder = GetDescriptionBuilder(solutionComponent.ComponentType.Value);

            return descriptionBuilder.GetCustomizableName(solutionComponent);
        }

        public string GetManagedName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || solutionComponent.ComponentType == null || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            var descriptionBuilder = GetDescriptionBuilder(solutionComponent.ComponentType.Value);

            return descriptionBuilder.GetManagedName(solutionComponent);
        }

        public string GetName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || !solutionComponent.ObjectId.HasValue)
            {
                return "Unknown";
            }

            var descriptionBuilder = GetDescriptionBuilder(solutionComponent.ComponentType.Value);

            return descriptionBuilder.GetName(solutionComponent);
        }

        public string GetDisplayName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || solutionComponent.ComponentType == null || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            var descriptionBuilder = GetDescriptionBuilder(solutionComponent.ComponentType.Value);

            return descriptionBuilder.GetDisplayName(solutionComponent);
        }

        public string GetFileName(string connectionName, int type, Guid objectId, string fieldTitle, string extension)
        {
            var descriptionBuilder = GetDescriptionBuilder(type);

            return descriptionBuilder.GetFileName(connectionName, objectId, fieldTitle, extension);
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

                    case ComponentType.KbArticleTemplate:
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

                    case ComponentType.SdkMessageProcessingStep:
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

                    case ComponentType.SdkMessageProcessingStepImage:
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

        private ISolutionComponentDescriptionBuilder GetDescriptionBuilder(int componentType)
        {
            if (!_cacheBuilders.ContainsKey(componentType))
            {
                var builder = new SolutionComponentDescriptionBuilderFactory().CreateBuilder(_service, this.MetadataSource, componentType);

                _cacheBuilders.TryAdd(componentType, builder);
            }

            return _cacheBuilders[componentType];
        }

        public List<T> GetEntities<T>(int componentType, IEnumerable<Guid> components) where T : Entity
        {
            return GetEntities<T>(componentType, components.Select(id => (Guid?)id));
        }

        public List<T> GetEntities<T>(int componentType, IEnumerable<Guid?> components) where T : Entity
        {
            var descriptionBuilder = GetDescriptionBuilder(componentType);

            return descriptionBuilder.GetEntities<T>(components);
        }

        public T GetEntity<T>(int componentType, Guid idEntity) where T : Entity
        {
            var descriptionBuilder = GetDescriptionBuilder(componentType);

            return descriptionBuilder.GetEntity<T>(idEntity);
        }
    }
}