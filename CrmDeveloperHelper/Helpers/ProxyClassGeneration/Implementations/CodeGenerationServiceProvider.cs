namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public sealed class CodeGenerationServiceProvider : ICodeGenerationServiceProvider
    {
        internal CodeGenerationServiceProvider(
            ITypeMappingService typeMappingService
            , ICodeGenerationService codeGenerationService
            , ICodeWriterFilterService codeFilterService
            , IMetadataProviderService metadataProviderService
            , INamingService namingService
        )
        {
            this.TypeMappingService = typeMappingService;
            this.CodeGenerationService = codeGenerationService;
            this.CodeWriterFilterService = codeFilterService;
            this.MetadataProviderService = metadataProviderService;
            this.NamingService = namingService;
        }

        public ITypeMappingService TypeMappingService { get; private set; }

        public ICodeGenerationService CodeGenerationService { get; private set; }

        public ICodeWriterFilterService CodeWriterFilterService { get; private set; }

        public IMetadataProviderService MetadataProviderService { get; private set; }

        public INamingService NamingService { get; private set; }

        //internal void InitializeServices(CrmSvcUtilParameters parameters)
        //{
        //    CodeWriterFilterService writerFilterService = new CodeWriterFilterService(parameters);
        //    _services.Add(typeof(ICodeWriterFilterService), (object)ServiceFactory.CreateInstance<ICodeWriterFilterService>((ICodeWriterFilterService)writerFilterService, parameters.CodeWriterFilterService, parameters));
        //    _services.Add(typeof(ICodeWriterMessageFilterService), (object)ServiceFactory.CreateInstance<ICodeWriterMessageFilterService>((ICodeWriterMessageFilterService)writerFilterService, parameters.CodeWriterMessageFilterService, parameters));
        //    _services.Add(typeof(IMetadataProviderService), (object)ServiceFactory.CreateInstance<IMetadataProviderService>((IMetadataProviderService)new SdkMetadataProviderService(parameters), parameters.MetadataProviderService, parameters));
        //    _services.Add(typeof(IMetadataProviderQueryService), (object)ServiceFactory.CreateInstance<IMetadataProviderQueryService>((IMetadataProviderQueryService)new MetadataProviderQueryService(parameters), parameters.MetadataQueryProvider, parameters));
        //    _services.Add(typeof(ICodeGenerationService), (object)ServiceFactory.CreateInstance<ICodeGenerationService>((ICodeGenerationService)new Microsoft.Crm.Services.Utility.CodeGenerationService(), parameters.CodeGenerationService, parameters));
        //    _services.Add(typeof(INamingService), (object)ServiceFactory.CreateInstance<INamingService>((INamingService)new Microsoft.Crm.Services.Utility.NamingService(parameters), parameters.NamingService, parameters));
        //    _services.Add(typeof(ICustomizeCodeDomService), (object)ServiceFactory.CreateInstance<ICustomizeCodeDomService>((ICustomizeCodeDomService)new CodeDomCustomizationService(), parameters.CodeCustomizationService, parameters));
        //    _services.Add(typeof(ITypeMappingService), (object)new Microsoft.Crm.Services.Utility.TypeMappingService(parameters));
        //}
    }
}
