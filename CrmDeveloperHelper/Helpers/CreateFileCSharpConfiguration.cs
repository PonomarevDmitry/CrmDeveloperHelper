using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class CreateFileCSharpConfiguration
    {
        public string TabSpacer { get; }

        public string NamespaceClasses { get; }

        public string NamespaceGlobalOptionSets { get; }

        public bool UseSchemaConstInCSharpAttributes { get; }

        public bool GenerateAttributes { get; }

        public bool GenerateAttributesWithNameOf { get; }

        public ProxyClassAttributeEnums GenerateAttributesEnumsStateStatus { get; }

        public ProxyClassAttributeEnums GenerateAttributesEnumsLocal { get; }

        public ProxyClassAttributeEnums GenerateAttributesEnumsGlobal { get; }

        public bool GenerateAttributesEnumsStateStatusUseSchemaEnum { get; }

        public bool GenerateAttributesEnumsLocalUseSchemaEnum { get; }

        public ProxyClassAttributeEnumsGlobalOptionSetLocation GenerateAttributesEnumsGlobalUseSchemaEnum { get; }

        public bool GenerateStatus { get; }

        public bool GenerateLocalOptionSet { get; }

        public bool GenerateGlobalOptionSet { get; }

        public bool GenerateOneToMany { get; }

        public bool GenerateManyToOne { get; }

        public bool GenerateManyToMany { get; }

        public bool GenerateKeys { get; }

        public bool GenerateSchemaIntoSchemaClass { get; }

        public bool GenerateWithDebuggerNonUserCode { get; }

        public bool AllDescriptions { get; }

        public bool WithDependentComponents { get; }

        public ConstantType ConstantType { get; }

        public OptionSetExportType OptionSetExportType { get; }

        public bool WithManagedInfo { get; }

        public bool WithoutObsoleteAttribute { get; }

        public bool MakeAllPropertiesEditable { get; }

        public bool AddConstructorWithAnonymousTypeObject { get; }

        public bool GenerateServiceContext { get; }

        public bool AddDescriptionAttribute { get; }

        public CreateFileCSharpConfiguration(
            string tabSpacer
            , string namespaceClasses
            , string namespaceGlobalOptionSets
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
            , bool generateAttributesWithNameOf
            , bool generateWithDebuggerNonUserCode
            , bool useSchemaConstInCSharpAttributes
            , bool withoutObsoleteAttribute
            , bool makeAllPropertiesEditable
            , bool addConstructorWithAnonymousTypeObject
            , ProxyClassAttributeEnums generateAttributesEnumsStateStatus

            , ProxyClassAttributeEnums generateAttributesEnumsLocal
            , ProxyClassAttributeEnums generateAttributesEnumsGlobal
            , bool generateAttributesEnumsStateStatusUseSchemaEnum

            , bool generateAttributesEnumsLocalUseSchemaEnum
            , ProxyClassAttributeEnumsGlobalOptionSetLocation generateAttributesEnumsGlobalUseSchemaEnum
            , bool addDescriptionAttribute
        )
        {
            this.TabSpacer = tabSpacer;

            this.NamespaceClasses = namespaceClasses;
            this.NamespaceGlobalOptionSets = namespaceGlobalOptionSets;

            this.GenerateAttributes = generateAttributes;
            this.GenerateAttributesWithNameOf = generateAttributesWithNameOf;
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

            this.GenerateSchemaIntoSchemaClass = intoSchemaClass;
            this.WithManagedInfo = withManagedInfo;

            this.GenerateWithDebuggerNonUserCode = generateWithDebuggerNonUserCode;
            this.UseSchemaConstInCSharpAttributes = useSchemaConstInCSharpAttributes;
            this.WithoutObsoleteAttribute = withoutObsoleteAttribute;
            this.MakeAllPropertiesEditable = makeAllPropertiesEditable;
            this.AddConstructorWithAnonymousTypeObject = addConstructorWithAnonymousTypeObject;

            this.GenerateAttributesEnumsStateStatus = generateAttributesEnumsStateStatus;
            this.GenerateAttributesEnumsLocal = generateAttributesEnumsLocal;
            this.GenerateAttributesEnumsGlobal = generateAttributesEnumsGlobal;

            this.GenerateAttributesEnumsStateStatusUseSchemaEnum = generateAttributesEnumsStateStatusUseSchemaEnum;
            this.GenerateAttributesEnumsLocalUseSchemaEnum = generateAttributesEnumsLocalUseSchemaEnum;
            this.GenerateAttributesEnumsGlobalUseSchemaEnum = generateAttributesEnumsGlobalUseSchemaEnum;

            this.AddDescriptionAttribute = addDescriptionAttribute;
        }
    }
}
