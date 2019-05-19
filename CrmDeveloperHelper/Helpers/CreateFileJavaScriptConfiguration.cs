using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class CreateFileJavaScriptConfiguration
    {
        public string TabSpacer { get; private set; }

        public bool WithDependentComponents { get; private set; }

        public bool GenerateIntoSchemaClass { get; private set; }

        public CreateFileJavaScriptConfiguration(
            string tabSpacer
            , bool withDependentComponents
            , bool generateIntoSchemaClass
        )
        {
            this.TabSpacer = tabSpacer;
            this.WithDependentComponents = withDependentComponents;
            this.GenerateIntoSchemaClass = generateIntoSchemaClass;
        }
    }
}
