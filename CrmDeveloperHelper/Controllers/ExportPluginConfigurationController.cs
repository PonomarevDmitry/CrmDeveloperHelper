using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class ExportPluginConfigurationController : BaseController<IWriteToOutput>
    {
        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public ExportPluginConfigurationController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        public async Task ExecuteExportingPluginConfigurationXml(Model.ConnectionData connectionData, Model.CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingPluginConfigurationXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await ExportingPluginConfigurationXml(connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task ExportingPluginConfigurationXml(Model.ConnectionData connectionData, Model.CommonConfiguration commonConfig)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var filePath = await CreatePluginDescription(connectionData, service, commonConfig);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            service.TryDispose();
        }

        private async Task<string> CreatePluginDescription(Model.ConnectionData connectionData, IOrganizationServiceExtented service, Model.CommonConfiguration commonConfig)
        {
            Model.Backup.PluginDescription description = await GetPluginDescription(service);

            if (connectionData.User != null)
            {
                description.CRMConnectionUserName = connectionData.User.Username;
            }
            else if (CredentialCache.DefaultNetworkCredentials != null)
            {
                description.CRMConnectionUserName =
                    (!string.IsNullOrEmpty(CredentialCache.DefaultNetworkCredentials.Domain) ? CredentialCache.DefaultNetworkCredentials.Domain + "\\" : "")
                    + CredentialCache.DefaultNetworkCredentials.UserName
                    ;
            }

            description.Organization = connectionData.UniqueOrgName;
            description.DiscoveryService = connectionData.DiscoveryUrl;
            description.OrganizationService = connectionData.OrganizationUrl;
            description.PublicUrl = connectionData.PublicUrl;
            description.MachineName = Environment.MachineName;
            description.ExecuteUserDomainName = Environment.UserDomainName;
            description.ExecuteUserName = Environment.UserName;

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string fileName = string.Format("{0}.{1} {2}.xml"
                , connectionData.Name
                , commonConfig.PluginConfigurationFileName.Trim()
                , EntityFileNameFormatter.GetDateString()
            );

            string filePath = Path.Combine(commonConfig.FolderForExport, fileName);

            description.Save(filePath);

            this._iWriteToOutput.WriteToOutput(connectionData, "Plugin Description exported to {0}", filePath);
            this._iWriteToOutput.WriteToOutputFilePathUri(connectionData, filePath);

            return filePath;
        }

        private async Task<Model.Backup.PluginDescription> GetPluginDescription(IOrganizationServiceExtented service)
        {
            var repositoryAssembly = new PluginAssemblyRepository(service);
            var repositoryType = new PluginTypeRepository(service);

            var repositoryMessage = new SdkMessageRepository(service);
            var repositoryFilter = new SdkMessageFilterRepository(service);
            var repositorySecure = new SdkMessageProcessingStepSecureConfigRepository(service);
            var repositoryImage = new SdkMessageProcessingStepImageRepository(service);

            var repositoryStep = new SdkMessageProcessingStepRepository(service);

            var result = new Model.Backup.PluginDescription();

            result.CreatedOn = DateTime.Now;

            var listAssemblies = await repositoryAssembly.GetAllPluginAssemblisWithStepsAsync();
            var listMessage = await repositoryMessage.GetAllSdkMessageWithStepsAsync();
            var listFilter = await repositoryFilter.GetAllSdkMessageFilterWithStepsAsync();
            var listSecure = await repositorySecure.GetAllSdkMessageProcessingStepSecureConfigAsync();

            foreach (var entAssembly in listAssemblies)
            {
                var assembly = Model.Backup.PluginAssembly.GetObject(entAssembly);

                result.PluginAssemblies.Add(assembly);

                var listTypes = await repositoryType.GetPluginTypesAsync(entAssembly.Id);

                foreach (var entPluginType in listTypes)
                {
                    var pluginType = Model.Backup.PluginType.GetObject(entPluginType);

                    assembly.PluginTypes.Add(pluginType);

                    var listSteps = await repositoryStep.GetPluginStepsByPluginTypeIdAsync(entPluginType.Id);

                    var listStepsToAdd = new List<Model.Backup.PluginStep>();

                    foreach (var entStep in listSteps)
                    {
                        Entities.SdkMessage entMessage = null;
                        Entities.SdkMessageFilter entFilter = null;
                        Entities.SdkMessageProcessingStepSecureConfig entSecure = null;

                        var refMessage = entStep.SdkMessageId;
                        if (refMessage != null)
                        {
                            entMessage = listMessage.FirstOrDefault(m => m.SdkMessageId == refMessage.Id);
                        }

                        var refFilter = entStep.SdkMessageFilterId;
                        if (refFilter != null)
                        {
                            entFilter = listFilter.FirstOrDefault(f => f.SdkMessageFilterId == refFilter.Id);
                        }

                        var refSecure = entStep.SdkMessageProcessingStepSecureConfigId;
                        if (refSecure != null)
                        {
                            entSecure = listSecure.FirstOrDefault(s => s.SdkMessageProcessingStepSecureConfigId == refSecure.Id);
                        }

                        var step = Model.Backup.PluginStep.GetObject(entStep, entMessage, entFilter, entSecure);

                        listStepsToAdd.Add(step);

                        var listImages = await repositoryImage.GetStepImagesAsync(entStep.Id);

                        foreach (var entImage in listImages)
                        {
                            var image = Model.Backup.PluginImage.GetObject(entImage);

                            step.PluginImages.Add(image);
                        }
                    }

                    pluginType.PluginSteps.AddRange(
                        listStepsToAdd
                        .OrderBy(step => step.PrimaryEntity)
                        .ThenBy(step => step.SecondaryEntity)
                        .ThenBy(step => step.Message, MessageComparer.Comparer)
                        .ThenBy(step => step.Stage)
                        .ThenBy(step => step.ExecutionOrder)
                        .ThenBy(step => step.Name)
                        .ThenBy(step => step.CreatedOn)
                    );
                }
            }

            return result;
        }

        public async Task ExecuteExportingPluginConfigurationIntoFolder(Model.ConnectionData connectionData, Model.CommonConfiguration commonConfig, EnvDTE.SelectedItem selectedItem)
        {
            string operation = string.Format(Properties.OperationNames.ExportingPluginConfigurationXmlIntoFolderFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await ExportingPluginConfigurationIntoFolder(selectedItem, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task ExportingPluginConfigurationIntoFolder(EnvDTE.SelectedItem selectedItem, Model.ConnectionData connectionData, Model.CommonConfiguration commonConfig)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            string filePath = string.Empty;

            //filePath = CreatePluginDescription(connectionData, service, commonConfig.FolderForExport, commonConfig.PluginConfigurationFileName, connectionData.Name);

            if (selectedItem.ProjectItem != null)
            {
                string folder = selectedItem.ProjectItem.FileNames[1];

                filePath = await CreatePluginDescription(connectionData, service, commonConfig);

                File.SetAttributes(filePath, FileAttributes.ReadOnly);

                selectedItem.ProjectItem.ProjectItems.AddFromFileCopy(filePath);

                selectedItem.ProjectItem.ContainingProject.Save();
            }
            else if (selectedItem.Project != null)
            {
                string relativePath = DTEHelper.GetRelativePath(selectedItem.Project);

                string solutionPath = Path.GetDirectoryName(selectedItem.DTE.Solution.FullName);

                string folder = Path.Combine(solutionPath, relativePath);

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                filePath = await CreatePluginDescription(connectionData, service, commonConfig);

                File.SetAttributes(filePath, FileAttributes.ReadOnly);

                selectedItem.Project.ProjectItems.AddFromFile(filePath);
            }

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            service.TryDispose();
        }
    }
}
