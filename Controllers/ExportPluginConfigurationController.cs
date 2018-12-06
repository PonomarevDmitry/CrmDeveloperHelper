using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
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
    public class ExportPluginConfigurationController
    {
        private IWriteToOutput _iWriteToOutput = null;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public ExportPluginConfigurationController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        public async Task ExecuteExportingPluginConfigurationXml(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingPluginConfigurationXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ExportingPluginConfigurationXml(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ExportingPluginConfigurationXml(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var filePath = await CreatePluginDescription(connectionData, service, commonConfig.FolderForExport, commonConfig.PluginConfigurationFileName, connectionData.Name);

            this._iWriteToOutput.PerformAction(filePath);
        }

        private async Task<string> CreatePluginDescription(ConnectionData connectionData, IOrganizationServiceExtented service, string fileFolder, string fileNameFormat, string connectionDataName)
        {
            Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction.PluginDescription description = await GetPluginDescription(service);

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

            string fileName = string.Format("{0}{1} {2}.xml"
                , (!string.IsNullOrEmpty(connectionDataName) ? connectionDataName + "." : string.Empty)
                , fileNameFormat.Trim()
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(fileFolder, fileName);

            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }

            description.Save(filePath);

            this._iWriteToOutput.WriteToOutput("Plugin Description exported to {0}", filePath);
            this._iWriteToOutput.WriteToOutputFilePathUri(filePath);

            return filePath;
        }

        private async Task<Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction.PluginDescription> GetPluginDescription(IOrganizationServiceExtented service)
        {
            var repositoryAssembly = new PluginAssemblyRepository(service);
            var repositoryType = new PluginTypeRepository(service);

            var repositoryMessage = new SdkMessageRepository(service);
            var repositoryFilter = new SdkMessageFilterRepository(service);
            var repositorySecure = new SdkMessageProcessingStepSecureConfigRepository(service);
            var repositoryImage = new SdkMessageProcessingStepImageRepository(service);

            var repositoryStep = new SdkMessageProcessingStepRepository(service);

            var result = new Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction.PluginDescription();

            result.CreatedOn = DateTime.Now;

            var listAssemblies = await repositoryAssembly.GetAllPluginAssemblisWithStepsAsync();
            var listMessage = await repositoryMessage.GetAllSdkMessageWithStepsAsync();
            var listFilter = await repositoryFilter.GetAllSdkMessageFilterWithStepsAsync();
            var listSecure = await repositorySecure.GetAllSdkMessageProcessingStepSecureConfigAsync();

            foreach (var entAssembly in listAssemblies)
            {
                var assembly = Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction.PluginAssembly.GetObject(entAssembly);

                result.PluginAssemblies.Add(assembly);

                var listTypes = await repositoryType.GetPluginTypesAsync(entAssembly.Id);

                foreach (var entPluginType in listTypes)
                {
                    var pluginType = Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction.PluginType.GetObject(entPluginType);

                    assembly.PluginTypes.Add(pluginType);

                    var listSteps = await repositoryStep.GetPluginStepsByPluginTypeIdAsync(entPluginType.Id);

                    var listStepsToAdd = new List<Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction.PluginStep>();

                    foreach (var entStep in listSteps)
                    {
                        Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessage entMessage = null;
                        Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageFilter entFilter = null;
                        Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageProcessingStepSecureConfig entSecure = null;

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

                        var step = Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction.PluginStep.GetObject(entStep, entMessage, entFilter, entSecure);

                        listStepsToAdd.Add(step);

                        var listImages = await repositoryImage.GetStepImagesAsync(entStep.Id);

                        foreach (var entImage in listImages)
                        {
                            var image = Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction.PluginImage.GetObject(entImage);

                            step.PluginImages.Add(image);
                        }
                    }

                    pluginType.PluginSteps.AddRange(
                        listStepsToAdd
                        .OrderBy(step => step.PrimaryEntity)
                        .ThenBy(step => step.SecondaryEntity)
                        .ThenBy(step => step.Message, new MessageComparer())
                        .ThenBy(step => step.Stage)
                        .ThenBy(step => step.ExecutionOrder)
                        .ThenBy(step => step.Name)
                        .ThenBy(step => step.CreatedOn)
                    );
                }
            }

            return result;
        }

        public async Task ExecuteExportingPluginConfigurationIntoFolder(EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingPluginConfigurationXmlIntoFolderFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ExportingPluginConfigurationIntoFolder(selectedItem, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ExportingPluginConfigurationIntoFolder(EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            string filePath = string.Empty;

            //filePath = CreatePluginDescription(connectionData, service, commonConfig.FolderForExport, commonConfig.PluginConfigurationFileName, connectionData.Name);

            if (selectedItem.ProjectItem != null)
            {
                string folder = selectedItem.ProjectItem.FileNames[1];

                filePath = await CreatePluginDescription(connectionData, service, folder, commonConfig.PluginConfigurationFileName, connectionData.Name);

                File.SetAttributes(filePath, FileAttributes.ReadOnly);

                selectedItem.ProjectItem.ProjectItems.AddFromFileCopy(filePath);

                selectedItem.ProjectItem.ContainingProject.Save();
            }
            else if (selectedItem.Project != null)
            {
                string relativePath = GetRelativePath(selectedItem.Project);

                string solutionPath = Path.GetDirectoryName(selectedItem.DTE.Solution.FullName);

                string folder = Path.Combine(solutionPath, relativePath);

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                filePath = await CreatePluginDescription(connectionData, service, folder, commonConfig.PluginConfigurationFileName, connectionData.Name);

                File.SetAttributes(filePath, FileAttributes.ReadOnly);

                selectedItem.Project.ProjectItems.AddFromFile(filePath);
            }

            this._iWriteToOutput.PerformAction(filePath);
        }

        private string GetRelativePath(EnvDTE.Project project)
        {
            List<string> names = new List<string>();

            if (project != null)
            {
                AddNamesRecursive(names, project);
            }

            names.Reverse();

            return string.Join(@"\", names);
        }

        private void AddNamesRecursive(List<string> names, EnvDTE.Project project)
        {
            if (project != null)
            {
                names.Add(project.Name);

                if (project.ParentProjectItem != null && project.ParentProjectItem.ContainingProject != null)
                {
                    AddNamesRecursive(names, project.ParentProjectItem.ContainingProject);
                }
            }
        }
    }
}
