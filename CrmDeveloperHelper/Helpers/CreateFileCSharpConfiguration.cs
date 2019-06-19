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

        public bool GenerateStatus { get; private set; }

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

        public static CreateFileCSharpConfiguration CreateForSchemaGlobalOptionSet(
            string namespaceClasses
            , string namespaceGlobalOptionSets
            , string typeConverterName
            , CommonConfiguration commonConfig
        )
        {
            var result = CreateForSchemaEntity(namespaceClasses, namespaceGlobalOptionSets, typeConverterName, commonConfig);

            result.WithDependentComponents = commonConfig.GenerateSchemaGlobalOptionSetsWithDependentComponents;

            //result.AddDescriptionAttribute = commonConfig.GenerateSchemaAddDescriptionAttribute;

            return result;
        }

        public static CreateFileCSharpConfiguration CreateForSchemaEntity(
            string namespaceClasses
            , string namespaceGlobalOptionSets
            , string typeConverterName
            , CommonConfiguration commonConfig
        )
        {
            var result = new CreateFileCSharpConfiguration
            {
                TabSpacer = commonConfig.GetTabSpacer(),

                NamespaceClasses = namespaceClasses,
                NamespaceGlobalOptionSets = namespaceGlobalOptionSets,

                TypeConverterName = typeConverterName,

                GenerateAttributes = commonConfig.GenerateSchemaAttributes,
                GenerateStatus = commonConfig.GenerateSchemaStatusOptionSet,
                GenerateLocalOptionSet = commonConfig.GenerateSchemaLocalOptionSet,
                GenerateGlobalOptionSet = commonConfig.GenerateSchemaGlobalOptionSet,
                GenerateOneToMany = commonConfig.GenerateSchemaOneToMany,
                GenerateManyToOne = commonConfig.GenerateSchemaManyToOne,
                GenerateManyToMany = commonConfig.GenerateSchemaManyToMany,
                GenerateKeys = commonConfig.GenerateSchemaKeys,

                AllDescriptions = commonConfig.GenerateCommonAllDescriptions,
                WithDependentComponents = commonConfig.GenerateSchemaEntityOptionSetsWithDependentComponents,
                ConstantType = commonConfig.GenerateSchemaConstantType,
                OptionSetExportType = commonConfig.GenerateSchemaOptionSetExportType,

                GenerateSchemaIntoSchemaClass = commonConfig.GenerateSchemaIntoSchemaClass,
                WithManagedInfo = commonConfig.SolutionComponentWithManagedInfo,

                AddDescriptionAttribute = commonConfig.GenerateSchemaAddDescriptionAttribute,

                AddTypeConverterAttributeForEnums = commonConfig.GenerateSchemaAddTypeConverterAttributeForEnums,
            };

            return result;
        }

        public static CreateFileCSharpConfiguration CreateForProxyClass(
            string namespaceClasses
            , string namespaceGlobalOptionSets
            , CommonConfiguration commonConfig
        )
        {
            var result = new CreateFileCSharpConfiguration
            {
                TabSpacer = commonConfig.GetTabSpacer(),

                NamespaceClasses = namespaceClasses,
                NamespaceGlobalOptionSets = namespaceGlobalOptionSets,

                GenerateAttributes = commonConfig.GenerateProxyClassesAttributes,
                GenerateAttributesWithNameOf = commonConfig.GenerateProxyClassesAttributesWithNameOf,
                GenerateStatus = commonConfig.GenerateProxyClassesStatusOptionSet,
                GenerateLocalOptionSet = commonConfig.GenerateProxyClassesLocalOptionSet,
                GenerateGlobalOptionSet = commonConfig.GenerateProxyClassesGlobalOptionSet,
                GenerateOneToMany = commonConfig.GenerateProxyClassesOneToMany,
                GenerateManyToOne = commonConfig.GenerateProxyClassesManyToOne,
                GenerateManyToMany = commonConfig.GenerateProxyClassesManyToMany,

                AllDescriptions = commonConfig.GenerateCommonAllDescriptions,
                WithDependentComponents = commonConfig.GenerateSchemaEntityOptionSetsWithDependentComponents,

                WithManagedInfo = commonConfig.SolutionComponentWithManagedInfo,

                GenerateWithDebuggerNonUserCode = commonConfig.GenerateProxyClassesWithDebuggerNonUserCode,
                UseSchemaConstInCSharpAttributes = commonConfig.GenerateProxyClassesUseSchemaConstInCSharpAttributes,
                WithoutObsoleteAttribute = commonConfig.GenerateProxyClassesWithoutObsoleteAttribute,
                MakeAllPropertiesEditable = commonConfig.GenerateProxyClassesMakeAllPropertiesEditable,
                AddConstructorWithAnonymousTypeObject = commonConfig.GenerateProxyClassesAddConstructorWithAnonymousTypeObject,

                GenerateAttributesEnumsStateStatus = commonConfig.GenerateProxyClassesAttributesEnumsStateStatus,
                GenerateAttributesEnumsLocal = commonConfig.GenerateProxyClassesAttributesEnumsLocal,
                GenerateAttributesEnumsGlobal = commonConfig.GenerateProxyClassesAttributesEnumsGlobal,

                GenerateAttributesEnumsStateStatusUseSchemaEnum = commonConfig.GenerateProxyClassesAttributesEnumsUseSchemaStateStatusEnum,
                GenerateAttributesEnumsLocalUseSchemaEnum = commonConfig.GenerateProxyClassesAttributesEnumsUseSchemaLocalEnum,
                GenerateAttributesEnumsGlobalUseSchemaEnum = commonConfig.GenerateProxyClassesAttributesEnumsUseSchemaGlobalEnum,

                AddDescriptionAttribute = commonConfig.GenerateProxyClassesAddDescriptionAttribute,
            };

            return result;
        }

        public static CreateFileCSharpConfiguration CreateForSdkMessageRequest(
            string namespaceClasses
            , string namespaceGlobalOptionSets
            , CommonConfiguration commonConfig
        )
        {
            var result = new CreateFileCSharpConfiguration
            {
                TabSpacer = commonConfig.GetTabSpacer(),

                NamespaceClasses = namespaceClasses,
                NamespaceGlobalOptionSets = namespaceGlobalOptionSets,

                GenerateAttributesWithNameOf = commonConfig.GenerateSdkMessageRequestAttributesWithNameOf,

                GenerateWithDebuggerNonUserCode = commonConfig.GenerateSdkMessageRequestWithDebuggerNonUserCode,

                MakeAllPropertiesEditable = commonConfig.GenerateSdkMessageRequestMakeAllPropertiesEditable,
            };

            return result;
        }
    }
}
