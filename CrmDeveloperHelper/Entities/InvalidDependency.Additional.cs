namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class InvalidDependency
    {
        public string MissingComponentTypeName
        {
            get
            {
                if (this.MissingComponentType == null)
                {
                    return string.Empty;
                }

                if (this.IsDefinedMissingComponentType())
                {
                    return SolutionComponent.GetComponentTypeName(this.MissingComponentType.Value);
                }

                if (this.FormattedValues != null
                    && this.FormattedValues.ContainsKey(Schema.Attributes.missingcomponenttype)
                    && !string.IsNullOrEmpty(this.FormattedValues[Schema.Attributes.missingcomponenttype])
                    )
                {
                    return this.FormattedValues[Schema.Attributes.missingcomponenttype];
                }

                return "Unknown";
            }
        }

        public bool IsDefinedMissingComponentType()
        {
            return SolutionComponent.IsDefinedComponentType(this.MissingComponentType?.Value);
        }

        public string ExistingComponentTypeName
        {
            get
            {
                if (this.ExistingComponentType == null)
                {
                    return string.Empty;
                }

                if (this.IsDefinedExistingComponentType())
                {
                    return SolutionComponent.GetComponentTypeName(this.ExistingComponentType.Value);
                }

                if (this.FormattedValues != null
                    && this.FormattedValues.ContainsKey(Schema.Attributes.existingcomponenttype)
                    && !string.IsNullOrEmpty(this.FormattedValues[Schema.Attributes.existingcomponenttype])
                    )
                {
                    return this.FormattedValues[Schema.Attributes.existingcomponenttype];
                }

                return "Unknown";
            }
        }

        public bool IsDefinedExistingComponentType()
        {
            return SolutionComponent.IsDefinedComponentType(this.ExistingComponentType?.Value);
        }
    }
}