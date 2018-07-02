namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class Dependency
    {
        public string DependentComponentTypeName
        {
            get
            {
                if (this.DependentComponentType == null)
                {
                    return string.Empty;
                }

                if (this.IsDefinedDependentComponentType())
                {
                    return SolutionComponent.GetComponentTypeName(this.DependentComponentType.Value);
                }

                if (this.FormattedValues != null
                    && this.FormattedValues.ContainsKey(Schema.Attributes.dependentcomponenttype)
                    && !string.IsNullOrEmpty(this.FormattedValues[Schema.Attributes.dependentcomponenttype])
                    )
                {
                    return this.FormattedValues[Schema.Attributes.dependentcomponenttype];
                }

                return "Unknown";
            }
        }

        public bool IsDefinedDependentComponentType()
        {
            return SolutionComponent.IsDefinedComponentType(this.DependentComponentType?.Value);
        }

        public string RequiredComponentTypeName
        {
            get
            {
                if (this.RequiredComponentType == null)
                {
                    return string.Empty;
                }

                if (this.IsDefinedRequiredComponentType())
                {
                    return SolutionComponent.GetComponentTypeName(this.RequiredComponentType.Value);
                }

                if (this.FormattedValues != null
                    && this.FormattedValues.ContainsKey(Schema.Attributes.requiredcomponenttype)
                    && !string.IsNullOrEmpty(this.FormattedValues[Schema.Attributes.requiredcomponenttype])
                    )
                {
                    return this.FormattedValues[Schema.Attributes.requiredcomponenttype];
                }

                return "Unknown";
            }
        }

        public bool IsDefinedRequiredComponentType()
        {
            return SolutionComponent.IsDefinedComponentType(this.RequiredComponentType?.Value);
        }

        public SolutionComponent DependentToSolutionComponent()
        {
            var result = new SolutionComponent()
            {
                ObjectId = this.DependentComponentObjectId.Value,
                ComponentType = this.DependentComponentType,
            };

            if (this.FormattedValues.ContainsKey(Dependency.Schema.Attributes.dependentcomponenttype))
            {
                result.FormattedValues[SolutionComponent.Schema.Attributes.componenttype] = this.FormattedValues[Dependency.Schema.Attributes.dependentcomponenttype];
            }

            return result;
        }

        public SolutionComponent RequiredToSolutionComponent()
        {
            var result = new SolutionComponent()
            {
                ObjectId = this.RequiredComponentObjectId.Value,
                ComponentType = this.RequiredComponentType
            };

            if (this.FormattedValues.ContainsKey(Dependency.Schema.Attributes.requiredcomponenttype))
            {
                result.FormattedValues[SolutionComponent.Schema.Attributes.componenttype] = this.FormattedValues[Dependency.Schema.Attributes.requiredcomponenttype];
            }

            return result;
        }
    }
}