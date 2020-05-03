using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class CreateFileJavaScriptConfiguration
    {
        public string TabSpacer { get; }

        public bool WithDependentComponents { get; }

        public bool GenerateSchemaIntoSchemaClass { get; }

        public bool GenerateGlobalOptionSets { get; }

        public string NamespaceClassesJavaScript { get; }

        public bool AddFormTypeEnum { get; }

        public bool AddRequiredLevelEnum { get; }

        public bool AddSubmitModeEnum { get; }

        public bool AddConsoleFunctions { get; }

        public CreateFileJavaScriptConfiguration(FileGenerationOptions fileGenerationOptions)
        {
            this.TabSpacer = fileGenerationOptions.GetTabSpacer();

            this.WithDependentComponents = fileGenerationOptions.GenerateSchemaEntityOptionSetsWithDependentComponents;

            this.GenerateSchemaIntoSchemaClass = fileGenerationOptions.GenerateJavaScriptIntoSchemaClass;
            this.GenerateGlobalOptionSets = fileGenerationOptions.GenerateJavaScriptGlobalOptionSet;

            this.NamespaceClassesJavaScript = fileGenerationOptions.NamespaceClassesJavaScript;

            this.AddFormTypeEnum = fileGenerationOptions.JavaScriptAddFormTypeEnum;
            this.AddRequiredLevelEnum = fileGenerationOptions.JavaScriptAddRequiredLevelEnum;
            this.AddSubmitModeEnum = fileGenerationOptions.JavaScriptAddSubmitModeEnum;
            this.AddConsoleFunctions = fileGenerationOptions.JavaScriptAddConsoleFunctions;
        }
    }
}