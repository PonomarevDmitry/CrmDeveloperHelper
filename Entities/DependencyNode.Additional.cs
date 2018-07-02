namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class DependencyNode
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
    }
}