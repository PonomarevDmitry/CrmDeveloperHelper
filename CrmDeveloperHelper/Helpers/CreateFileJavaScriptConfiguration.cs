using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class CreateFileJavaScriptConfiguration
    {
        public string TabSpacer { get; }

        public bool WithDependentComponents { get; }

        public bool GenerateSchemaIntoSchemaClass { get; }

        public bool GenerateGlobalOptionSets { get; }

        public CreateFileJavaScriptConfiguration(
            string tabSpacer
            , bool withDependentComponents
            , bool generateSchemaIntoSchemaClass
            , bool generateGlobalOptionSets
        )
        {
            this.TabSpacer = tabSpacer;
            this.WithDependentComponents = withDependentComponents;
            this.GenerateSchemaIntoSchemaClass = generateSchemaIntoSchemaClass;
            this.GenerateGlobalOptionSets = generateGlobalOptionSets;
        }
    }
}
