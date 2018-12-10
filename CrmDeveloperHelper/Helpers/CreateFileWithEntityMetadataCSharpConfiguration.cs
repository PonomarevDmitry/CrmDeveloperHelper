using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class CreateFileWithEntityMetadataCSharpConfiguration
    {
        public string EntityName { get; private set; }

        public string Folder { get; private set; }

        public string TabSpacer { get; private set; }

        public bool GenerateAttributes { get; private set; }

        public bool GenerateStatus { get; private set; }

        public bool GenerateLocalOptionSet { get; private set; }

        public bool GenerateGlobalOptionSet { get; private set; }

        public bool GenerateOneToMany { get; private set; }

        public bool GenerateManyToOne { get; private set; }

        public bool GenerateManyToMany { get; private set; }

        public bool GenerateKeys { get; private set; }

        public bool GenerateIntoSchemaClass { get; private set; }

        public bool AllDescriptions { get; private set; }

        public bool WithDependentComponents { get; private set; }

        public ConstantType ConstantType { get; private set; }

        public OptionSetExportType OptionSetExportType { get; private set; }

        public EntityMetadata EntityMetadata { get; set; }

        public bool WithManagedInfo { get; private set; }

        public CreateFileWithEntityMetadataCSharpConfiguration(
            string entityName
            , string folder
            , string tabSpacer
            , bool generateAttributes
            , bool generateStatus
            , bool generateLocalOptionSet
            , bool generateGlobalOptionSet
            , bool generateOneToMany
            , bool generateManyToOne
            , bool generateManyToMany
            , bool generateKeys
            , bool allDescriptions
            , bool withDependentComponents
            , bool intoSchemaClass
            , bool withManagedInfo
            , ConstantType constantType
            , OptionSetExportType optionSetExportType
            )
        {
            this.EntityName = entityName.ToLower();
            this.Folder = folder;
            this.TabSpacer = tabSpacer;
            this.GenerateAttributes = generateAttributes;
            this.GenerateStatus = generateStatus;
            this.GenerateLocalOptionSet = generateLocalOptionSet;
            this.GenerateGlobalOptionSet = generateGlobalOptionSet;
            this.GenerateOneToMany = generateOneToMany;
            this.GenerateManyToOne = generateManyToOne;
            this.GenerateManyToMany = generateManyToMany;
            this.AllDescriptions = allDescriptions;
            this.WithDependentComponents = withDependentComponents;
            this.ConstantType = constantType;
            this.GenerateKeys = generateKeys;
            this.OptionSetExportType = optionSetExportType;

            this.GenerateIntoSchemaClass = intoSchemaClass;
            this.WithManagedInfo = withManagedInfo;
        }
    }
}
