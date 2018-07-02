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
            string result = string.Empty;

            if (behavior.HasValue)
            {
                if (Enum.IsDefined(typeof(RootComponentBehavior), behavior.Value))
                {
                    RootComponentBehavior componentBehavior = (RootComponentBehavior)behavior.Value;

                    result = componentBehavior.ToString();
                }
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

        internal static int GetComponentTypeObjectTypeCode(ComponentType componentType)
        {
            switch (componentType)
            {
                case Entities.ComponentType.Entity:
                    return 9801;

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

                //case Entities.ComponentType.AppModuleRoles:
                //    return AppModuleRoles.EntityTypeCode;

                case Entities.ComponentType.Attribute:
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
                    break;
            }

            return 0;
        }
    }
}