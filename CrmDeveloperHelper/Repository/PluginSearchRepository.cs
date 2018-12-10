using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class PluginSearchRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        private SdkMessageProcessingStepImageRepository _repositoryImage;
        private SdkMessageProcessingStepRepository _repositoryStep;

        /// <summary>
        /// Конструктор репозитория функция по поиску решений.
        /// </summary>
        /// <param name="service"></param>
        public PluginSearchRepository(IOrganizationServiceExtented service)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));

            this._repositoryImage = new SdkMessageProcessingStepImageRepository(service);
            this._repositoryStep = new SdkMessageProcessingStepRepository(service);
        }

        public async Task<PluginSearchResult> FindAllAsync(List<PluginStage> list, string pluginName, string messageName, string entityName)
        {
            PluginSearchResult result = new PluginSearchResult();

            //result.PluginAssembly = GetAllPluginAssembly();
            //result.PluginType = GetAllPluginType();
            //result.SdkMessage = GetAllSdkMessage();
            //result.SdkMessageFilter = GetAllSdkMessageFilter();
            //result.SdkMessageProcessingStepSecureConfig = GetAllSdkMessageProcessingStepSecureConfig();

            var listSteps = await _repositoryStep.GetAllSdkMessageProcessingStepAsync(list, pluginName, messageName);

            if (string.IsNullOrEmpty(entityName))
            {
                result.SdkMessageProcessingStep = listSteps;
            }
            else
            {
                entityName = entityName.ToLower();

                result.SdkMessageProcessingStep = listSteps.Where(ent => ent.PrimaryObjectTypeCodeName.ToLower().Contains(entityName)).ToList();
            }

            result.SdkMessageProcessingStepImage = await this._repositoryImage.GetAllSdkMessageProcessingStepImageAsync();

            return result;
        }
    }
}
