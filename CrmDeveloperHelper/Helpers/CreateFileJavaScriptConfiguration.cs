namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class CreateFileJavaScriptConfiguration
    {
        public string TabSpacer { get; }

        public bool WithDependentComponents { get; }

        public bool GenerateSchemaIntoSchemaClass { get; }

        public bool GenerateGlobalOptionSets { get; }

        public string NamespaceClassesJavaScript { get; }

        public CreateFileJavaScriptConfiguration(
            string tabSpacer
            , bool withDependentComponents
            , bool generateSchemaIntoSchemaClass
            , bool generateGlobalOptionSets
            , string namespaceClassesJavaScript
        )
        {
            this.TabSpacer = tabSpacer;
            this.WithDependentComponents = withDependentComponents;
            this.GenerateSchemaIntoSchemaClass = generateSchemaIntoSchemaClass;
            this.GenerateGlobalOptionSets = generateGlobalOptionSets;
            this.NamespaceClassesJavaScript = namespaceClassesJavaScript;
        }
    }
}
