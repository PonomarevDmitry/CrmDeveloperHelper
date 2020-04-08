using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class CreateFileCSharpConfiguration
    {
        public string TabSpacer { get; private set; }

        public string NamespaceClasses { get; private set; }

        public string NamespaceGlobalOptionSets { get; private set; }

        public bool UseSchemaConstInCSharpAttributes { get; private set; }

        public bool GenerateAttributes { get; private set; }

        public bool GenerateAttributesWithNameOf { get; private set; }

        public ProxyClassAttributeEnums GenerateAttributesEnumsStateStatus { get; private set; }

        public ProxyClassAttributeEnums GenerateAttributesEnumsLocal { get; private set; }

        public ProxyClassAttributeEnums GenerateAttributesEnumsGlobal { get; private set; }

        public bool GenerateAttributesEnumsStateStatusUseSchemaEnum { get; private set; }

        public bool GenerateAttributesEnumsLocalUseSchemaEnum { get; private set; }

        public ProxyClassAttributeEnumsGlobalOptionSetLocation GenerateAttributesEnumsGlobalUseSchemaEnum { get; private set; }

        public bool GenerateStateStatusOptionSet { get; private set; }

        public bool GenerateLocalOptionSet { get; private set; }

        public bool GenerateGlobalOptionSet { get; private set; }

        public bool GenerateOneToMany { get; private set; }

        public bool GenerateManyToOne { get; private set; }

        public bool GenerateManyToMany { get; private set; }

        public bool GenerateKeys { get; private set; }

        public bool GenerateSchemaIntoSchemaClass { get; private set; }

        public bool GenerateWithDebuggerNonUserCode { get; private set; }

        public bool AllDescriptions { get; private set; }

        public bool WithDependentComponents { get; private set; }

        public ConstantType ConstantType { get; private set; }

        public OptionSetExportType OptionSetExportType { get; private set; }

        public bool WithManagedInfo { get; private set; }

        public bool WithoutObsoleteAttribute { get; private set; }

        public bool MakeAllPropertiesEditable { get; private set; }

        public bool AddConstructorWithAnonymousTypeObject { get; private set; }

        public bool GenerateServiceContext { get; private set; }

        public bool AddDescriptionAttribute { get; private set; }

        public bool AddTypeConverterAttributeForEnums { get; private set; }

        public string TypeConverterName { get; private set; }

        private CreateFileCSharpConfiguration()
        {

        }

        public static CreateFileCSharpConfiguration CreateForSchemaGlobalOptionSet(FileGenerationOptions fileGenerationOptions)
        {
            var result = CreateForSchemaEntity(fileGenerationOptions);

            result.WithDependentComponents = fileGenerationOptions.GenerateSchemaGlobalOptionSetsWithDependentComponents;

            //result.AddDescriptionAttribute = commonConfig.GenerateSchemaAddDescriptionAttribute;

            return result;
        }

        public static CreateFileCSharpConfiguration CreateForSchemaEntity(FileGenerationOptions fileGenerationOptions)
        {
            var result = new CreateFileCSharpConfiguration
            {
                TabSpacer = fileGenerationOptions.GetTabSpacer(),

                NamespaceClasses = fileGenerationOptions.NamespaceClassesCSharp,
                NamespaceGlobalOptionSets = fileGenerationOptions.NamespaceGlobalOptionSetsCSharp,

                TypeConverterName = fileGenerationOptions.TypeConverterName,

                GenerateAttributes = fileGenerationOptions.GenerateSchemaAttributes,
                GenerateStateStatusOptionSet = fileGenerationOptions.GenerateSchemaStatusOptionSet,
                GenerateLocalOptionSet = fileGenerationOptions.GenerateSchemaLocalOptionSet,
                GenerateGlobalOptionSet = fileGenerationOptions.GenerateSchemaGlobalOptionSet,
                GenerateOneToMany = fileGenerationOptions.GenerateSchemaOneToMany,
                GenerateManyToOne = fileGenerationOptions.GenerateSchemaManyToOne,
                GenerateManyToMany = fileGenerationOptions.GenerateSchemaManyToMany,
                GenerateKeys = fileGenerationOptions.GenerateSchemaKeys,

                AllDescriptions = fileGenerationOptions.GenerateCommonAllDescriptions,
                WithDependentComponents = fileGenerationOptions.GenerateSchemaEntityOptionSetsWithDependentComponents,
                ConstantType = fileGenerationOptions.GenerateSchemaConstantType,
                OptionSetExportType = fileGenerationOptions.GenerateSchemaOptionSetExportType,

                GenerateSchemaIntoSchemaClass = fileGenerationOptions.GenerateSchemaIntoSchemaClass,

                WithManagedInfo = fileGenerationOptions.SolutionComponentWithManagedInfo,

                AddDescriptionAttribute = fileGenerationOptions.GenerateSchemaAddDescriptionAttribute,

                AddTypeConverterAttributeForEnums = fileGenerationOptions.GenerateSchemaAddTypeConverterAttributeForEnums,
            };

            return result;
        }

        public static CreateFileCSharpConfiguration CreateForProxyClass(FileGenerationOptions fileGenerationOptions)
        {
            var result = new CreateFileCSharpConfiguration
            {
                TabSpacer = fileGenerationOptions.GetTabSpacer(),

                NamespaceClasses = fileGenerationOptions.NamespaceClassesCSharp,
                NamespaceGlobalOptionSets = fileGenerationOptions.NamespaceGlobalOptionSetsCSharp,

                GenerateAttributes = fileGenerationOptions.GenerateProxyClassesAttributes,
                GenerateAttributesWithNameOf = fileGenerationOptions.GenerateProxyClassesAttributesWithNameOf,
                GenerateStateStatusOptionSet = fileGenerationOptions.GenerateProxyClassesStatusOptionSet,
                GenerateLocalOptionSet = fileGenerationOptions.GenerateProxyClassesLocalOptionSet,
                GenerateGlobalOptionSet = fileGenerationOptions.GenerateProxyClassesGlobalOptionSet,
                GenerateOneToMany = fileGenerationOptions.GenerateProxyClassesOneToMany,
                GenerateManyToOne = fileGenerationOptions.GenerateProxyClassesManyToOne,
                GenerateManyToMany = fileGenerationOptions.GenerateProxyClassesManyToMany,

                AllDescriptions = fileGenerationOptions.GenerateCommonAllDescriptions,
                WithDependentComponents = fileGenerationOptions.GenerateSchemaEntityOptionSetsWithDependentComponents,

                WithManagedInfo = fileGenerationOptions.SolutionComponentWithManagedInfo,

                GenerateWithDebuggerNonUserCode = fileGenerationOptions.GenerateProxyClassesWithDebuggerNonUserCode,
                UseSchemaConstInCSharpAttributes = fileGenerationOptions.GenerateProxyClassesUseSchemaConstInCSharpAttributes,
                WithoutObsoleteAttribute = fileGenerationOptions.GenerateProxyClassesWithoutObsoleteAttribute,
                MakeAllPropertiesEditable = fileGenerationOptions.GenerateProxyClassesMakeAllPropertiesEditable,
                AddConstructorWithAnonymousTypeObject = fileGenerationOptions.GenerateProxyClassesAddConstructorWithAnonymousTypeObject,

                GenerateAttributesEnumsStateStatus = fileGenerationOptions.GenerateProxyClassesAttributesEnumsStateStatus,
                GenerateAttributesEnumsLocal = fileGenerationOptions.GenerateProxyClassesAttributesEnumsLocal,
                GenerateAttributesEnumsGlobal = fileGenerationOptions.GenerateProxyClassesAttributesEnumsGlobal,

                GenerateAttributesEnumsStateStatusUseSchemaEnum = fileGenerationOptions.GenerateProxyClassesAttributesEnumsUseSchemaStateStatusEnum,
                GenerateAttributesEnumsLocalUseSchemaEnum = fileGenerationOptions.GenerateProxyClassesAttributesEnumsUseSchemaLocalEnum,
                GenerateAttributesEnumsGlobalUseSchemaEnum = fileGenerationOptions.GenerateProxyClassesAttributesEnumsUseSchemaGlobalEnum,

                AddDescriptionAttribute = fileGenerationOptions.GenerateProxyClassesAddDescriptionAttribute,
            };

            return result;
        }

        public static CreateFileCSharpConfiguration CreateForSdkMessageRequest(FileGenerationOptions fileGenerationOptions)
        {
            var result = new CreateFileCSharpConfiguration
            {
                TabSpacer = fileGenerationOptions.GetTabSpacer(),

                NamespaceClasses = fileGenerationOptions.NamespaceSdkMessagesCSharp,
                NamespaceGlobalOptionSets = fileGenerationOptions.NamespaceGlobalOptionSetsCSharp,

                GenerateAttributesWithNameOf = fileGenerationOptions.GenerateSdkMessageRequestAttributesWithNameOf,

                GenerateWithDebuggerNonUserCode = fileGenerationOptions.GenerateSdkMessageRequestWithDebuggerNonUserCode,

                MakeAllPropertiesEditable = fileGenerationOptions.GenerateSdkMessageRequestMakeAllPropertiesEditable,
            };

            return result;
        }
    }
}
