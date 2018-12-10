using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class SolutionComponent
    {
        public string ComponentTypeName
        {
            get
            {
                if (this.ComponentType == null)
                {
                    return string.Empty;
                }

                if (this.IsDefinedComponentType())
                {
                    return SolutionComponent.GetComponentTypeName(this.ComponentType.Value);
                }

                if (this.FormattedValues != null
                    && this.FormattedValues.ContainsKey(Schema.Attributes.componenttype)
                    && !string.IsNullOrEmpty(this.FormattedValues[Schema.Attributes.componenttype])
                    )
                {
                    return this.FormattedValues[Schema.Attributes.componenttype];
                }

                return "Unknown";
            }
        }

        public bool IsDefinedComponentType()
        {
            return SolutionComponent.IsDefinedComponentType(this.ComponentType?.Value);
        }

        public override string ToString()
        {
            string componentTypeName = string.Empty;

            if (this.ComponentType != null)
            {
                if (Enum.IsDefined(typeof(ComponentType), this.ComponentType.Value))
                {
                    ComponentType componentType = (ComponentType)this.ComponentType.Value;

                    componentTypeName = componentType.ToString();
                }
            }

            return string.Format("{0} - {1} - {2} - {3}"
                , this.ComponentType?.Value

                , this.ComponentTypeName

                , this.ObjectId?.ToString()

                , GetRootComponentBehaviorName(this.RootComponentBehavior?.Value)
            );
        }

        public static bool IsDefinedComponentType(int? componentType)
        {
            if (!componentType.HasValue)
            {
                return false;
            }

            return Enum.IsDefined(typeof(ComponentType), componentType.Value);
        }

        public static string GetComponentTypeName(int componentType)
        {
            if (Enum.IsDefined(typeof(ComponentType), componentType))
            {
                return ((ComponentType)componentType).ToString();
            }

            return "Unknown";
        }

        public static string GetRootComponentBehaviorName(int? behavior)
        {
            string result = Entities.RootComponentBehavior.IncludeSubcomponents.ToString();

            if (behavior.HasValue && Enum.IsDefined(typeof(RootComponentBehavior), behavior.Value))
            {
                RootComponentBehavior componentBehavior = (RootComponentBehavior)behavior.Value;

                result = componentBehavior.ToString();
            }

            return result;
        }

        internal static bool IsComponentTypeMetadata(Entities.ComponentType componentType)
        {
            switch (componentType)
            {
                case Entities.ComponentType.Entity:
                case Entities.ComponentType.Attribute:
                case Entities.ComponentType.Relationship:
                case Entities.ComponentType.AttributePicklistValue:
                case Entities.ComponentType.AttributeLookupValue:
                case Entities.ComponentType.ViewAttribute:
                case Entities.ComponentType.LocalizedLabel:
                case Entities.ComponentType.RelationshipExtraCondition:
                case Entities.ComponentType.OptionSet:
                case Entities.ComponentType.EntityRelationship:
                case Entities.ComponentType.EntityRelationshipRole:
                case Entities.ComponentType.EntityRelationshipRelationships:
                case Entities.ComponentType.ManagedProperty:
                case Entities.ComponentType.EntityKey:
                case Entities.ComponentType.EntityKeyAttribute:

                case Entities.ComponentType.DependencyFeature:
                    return true;
            }

            return false;
        }

        public static int? GetComponentTypeObjectTypeCode(ComponentType componentType)
        {
            switch (componentType)
            {
                case Entities.ComponentType.Entity:
                    return 9801;

                case Entities.ComponentType.Attribute:
                    return 9802;

                case Entities.ComponentType.OptionSet:
                    return 9804;

                case Entities.ComponentType.PluginAssembly:
                    return PluginAssembly.EntityTypeCode;

                case Entities.ComponentType.PluginType:
                    return PluginType.EntityTypeCode;

                case Entities.ComponentType.Report:
                    return Report.EntityTypeCode;

                case Entities.ComponentType.SavedQuery:
                    return SavedQuery.EntityTypeCode;

                case Entities.ComponentType.SavedQueryVisualization:
                    return SavedQueryVisualization.EntityTypeCode;

                case Entities.ComponentType.SiteMap:
                    return SiteMap.EntityTypeCode;

                case Entities.ComponentType.SystemForm:
                    return SystemForm.EntityTypeCode;

                case Entities.ComponentType.WebResource:
                    return WebResource.EntityTypeCode;

                case Entities.ComponentType.Workflow:
                    return Workflow.EntityTypeCode;

                case Entities.ComponentType.AppModule:
                    return AppModule.EntityTypeCode;

                case Entities.ComponentType.AppModuleRoles:
                    return AppModuleRoles.EntityTypeCode;

                case Entities.ComponentType.Privilege:
                    return Privilege.EntityTypeCode;

                case Entities.ComponentType.PrivilegeObjectTypeCode:
                    return PrivilegeObjectTypeCodes.EntityTypeCode;

                case Entities.ComponentType.Role:
                    return Role.EntityTypeCode;

                case Entities.ComponentType.RolePrivileges:
                    return RolePrivileges.EntityTypeCode;

                case Entities.ComponentType.DisplayString:
                    return DisplayString.EntityTypeCode;

                case Entities.ComponentType.DisplayStringMap:
                    return DisplayStringMap.EntityTypeCode;

                case Entities.ComponentType.Organization:
                    return Organization.EntityTypeCode;

                case Entities.ComponentType.ProcessTrigger:
                    return ProcessTrigger.EntityTypeCode;

                case Entities.ComponentType.ReportEntity:
                    return ReportEntity.EntityTypeCode;

                case Entities.ComponentType.ReportCategory:
                    return ReportCategory.EntityTypeCode;

                case Entities.ComponentType.ReportVisibility:
                    return ReportVisibility.EntityTypeCode;

                case Entities.ComponentType.EmailTemplate:
                    return Template.EntityTypeCode;

                case Entities.ComponentType.ContractTemplate:
                    return ContractTemplate.EntityTypeCode;

                case Entities.ComponentType.KbArticleTemplate:
                    return KbArticleTemplate.EntityTypeCode;

                case Entities.ComponentType.MailMergeTemplate:
                    return MailMergeTemplate.EntityTypeCode;

                case Entities.ComponentType.DuplicateRule:
                    return DuplicateRule.EntityTypeCode;

                case Entities.ComponentType.DuplicateRuleCondition:
                    return DuplicateRuleCondition.EntityTypeCode;

                case Entities.ComponentType.EntityMap:
                    return EntityMap.EntityTypeCode;

                case Entities.ComponentType.AttributeMap:
                    return AttributeMap.EntityTypeCode;

                case Entities.ComponentType.RibbonCommand:
                    return RibbonCommand.EntityTypeCode;

                case Entities.ComponentType.RibbonContextGroup:
                    return RibbonContextGroup.EntityTypeCode;

                case Entities.ComponentType.RibbonCustomization:
                    return RibbonCustomization.EntityTypeCode;

                case Entities.ComponentType.RibbonRule:
                    return RibbonRule.EntityTypeCode;

                case Entities.ComponentType.RibbonTabToCommandMap:
                    return RibbonTabToCommandMap.EntityTypeCode;

                case Entities.ComponentType.RibbonDiff:
                    return RibbonDiff.EntityTypeCode;

                case Entities.ComponentType.ConnectionRole:
                    return ConnectionRole.EntityTypeCode;

                case Entities.ComponentType.FieldSecurityProfile:
                    return FieldSecurityProfile.EntityTypeCode;

                case Entities.ComponentType.FieldPermission:
                    return FieldPermission.EntityTypeCode;

                case Entities.ComponentType.SdkMessageProcessingStep:
                    return SdkMessageProcessingStep.EntityTypeCode;

                case Entities.ComponentType.SdkMessageProcessingStepImage:
                    return SdkMessageProcessingStepImage.EntityTypeCode;

                case Entities.ComponentType.ServiceEndpoint:
                    return ServiceEndpoint.EntityTypeCode;

                case Entities.ComponentType.RoutingRule:
                    return RoutingRule.EntityTypeCode;

                case Entities.ComponentType.RoutingRuleItem:
                    return RoutingRuleItem.EntityTypeCode;

                case Entities.ComponentType.SLA:
                    return SLA.EntityTypeCode;

                case Entities.ComponentType.SLAItem:
                    return SLAItem.EntityTypeCode;

                case Entities.ComponentType.ConvertRule:
                    return ConvertRule.EntityTypeCode;

                case Entities.ComponentType.ConvertRuleItem:
                    return ConvertRuleItem.EntityTypeCode;

                case Entities.ComponentType.HierarchyRule:
                    return HierarchyRule.EntityTypeCode;

                case Entities.ComponentType.MobileOfflineProfile:
                    return MobileOfflineProfile.EntityTypeCode;

                case Entities.ComponentType.MobileOfflineProfileItem:
                    return MobileOfflineProfileItem.EntityTypeCode;

                case Entities.ComponentType.SimilarityRule:
                    return SimilarityRule.EntityTypeCode;

                case Entities.ComponentType.CustomControl:
                    return CustomControl.EntityTypeCode;

                case Entities.ComponentType.CustomControlDefaultConfig:
                    return CustomControlDefaultConfig.EntityTypeCode;

                case Entities.ComponentType.CustomControlResource:
                    return CustomControlResource.EntityTypeCode;

                case Entities.ComponentType.ChannelAccessProfileEntityAccessLevel:
                    return ChannelAccessProfileEntityAccessLevel.EntityTypeCode;

                case Entities.ComponentType.ChannelAccessProfile:
                    return ChannelAccessProfile.EntityTypeCode;

                case Entities.ComponentType.SdkMessage:
                    return SdkMessage.EntityTypeCode;

                case Entities.ComponentType.SdkMessageFilter:
                    return SdkMessageFilter.EntityTypeCode;

                case Entities.ComponentType.SdkMessagePair:
                    return SdkMessagePair.EntityTypeCode;

                case Entities.ComponentType.SdkMessageRequest:
                    return SdkMessageRequest.EntityTypeCode;

                case Entities.ComponentType.SdkMessageRequestField:
                    return SdkMessageRequestField.EntityTypeCode;

                case Entities.ComponentType.SdkMessageResponse:
                    return SdkMessageResponse.EntityTypeCode;

                case Entities.ComponentType.SdkMessageResponseField:
                    return SdkMessageResponseField.EntityTypeCode;

                case Entities.ComponentType.ImportMap:
                    return ImportMap.EntityTypeCode;

                case Entities.ComponentType.ImportEntityMapping:
                    return ImportEntityMapping.EntityTypeCode;

                case Entities.ComponentType.Attachment:
                    return Attachment.Schema.EntityTypeCode;

                case Entities.ComponentType.DependencyFeature:
                    return DependencyFeature.Schema.EntityTypeCode;

                case Entities.ComponentType.ComplexControl:
                    return ComplexControl.Schema.EntityTypeCode;

                case Entities.ComponentType.WebWizard:
                    return WebWizard.Schema.EntityTypeCode;

                case Entities.ComponentType.AttributePicklistValue:
                case Entities.ComponentType.AttributeLookupValue:
                case Entities.ComponentType.ViewAttribute:
                case Entities.ComponentType.EntityKeyAttribute:

                case Entities.ComponentType.Relationship:
                case Entities.ComponentType.LocalizedLabel:

                case Entities.ComponentType.RelationshipExtraCondition:

                case Entities.ComponentType.EntityRelationship:
                case Entities.ComponentType.EntityRelationshipRole:
                case Entities.ComponentType.EntityRelationshipRelationships:

                case Entities.ComponentType.ManagedProperty:
                case Entities.ComponentType.EntityKey:

                case Entities.ComponentType.Form:

                case Entities.ComponentType.Index:
                    break;
            }

            return null;
        }

        public static void GetComponentTypeEntityName(int type, out string entityName, out string entityIdName)
        {
            entityName = string.Empty;
            entityIdName = string.Empty;

            if (!IsDefinedComponentType(type))
            {
                return;
            }

            ComponentType componentType = (ComponentType)type;

            if (IsComponentTypeMetadata(componentType))
            {
                return;
            }

            switch (componentType)
            {
                case Entities.ComponentType.EmailTemplate:
                    entityName = Template.EntityLogicalName;
                    entityIdName = Template.PrimaryIdAttribute;
                    break;

                case Entities.ComponentType.RolePrivileges:
                    entityName = RolePrivileges.EntityLogicalName;
                    entityIdName = RolePrivileges.PrimaryIdAttribute;
                    break;

                case Entities.ComponentType.SystemForm:
                    entityName = SystemForm.EntityLogicalName;
                    entityIdName = SystemForm.PrimaryIdAttribute;
                    break;

                case Entities.ComponentType.DependencyFeature:
                    break;

                default:
                    entityName = componentType.ToString().ToLower();
                    entityIdName = entityName + "id";
                    break;
            }
        }
    }
}