using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public partial class ExportXmlController
    {
        public async Task ExecuteDifferencePluginTypeCustomWorkflowActivityInfo(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.DifferencePluginTypeCustomWorkflowActivityInfoFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetPluginTypeExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, DifferencePluginTypeCustomWorkflowActivityInfo);
                }
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

        public async Task ExecuteDifferencePluginTypeCustomWorkflowActivityInfo(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.DifferencePluginTypeCustomWorkflowActivityInfoFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetPluginTypeExecuteAction(connectionData, commonConfig, doc, filePath, null, DifferencePluginTypeCustomWorkflowActivityInfo);
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

        public async Task ExecuteGetPluginTypeCurrentCustomWorkflowActivityInfo(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.GettingPluginTypeCurrentCustomWorkflowActivityInfoFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetPluginTypeExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, GetCurrentPluginTypeCustomWorkflowActivityInfoAsync);
                }
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

        private Task CheckAttributeValidateGetPluginTypeExecuteAction(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , XDocument doc
            , string filePath
            , Func<ConnectionData, XDocument, Task<bool>> validatorDocument
            , Func<IOrganizationServiceExtented, CommonConfiguration, XDocument, string, PluginType, Task> continueAction
        )
        {
            return CheckAttributeValidateGetEntityExecuteAction(
                connectionData
                , commonConfig
                , doc
                , filePath
                , GetPluginTypeNameFromCustomWorkflowActivityInfo
                , ValidateAttributePluginTypeCustomWorkflowActivityInfo
                , validatorDocument
                , GetPluginTypeByAttribute
                , continueAction
            );
        }

        private static string GetPluginTypeNameFromCustomWorkflowActivityInfo(XDocument doc)
        {
            var pluginTypeName = doc.XPathSelectElements("SandboxCustomActivityInfo/CustomActivityInfo/TypeName").Where(e => !string.IsNullOrEmpty(e.Value)).Select(e => e.Value).FirstOrDefault();

            return pluginTypeName;
        }

        private bool ValidateAttributePluginTypeCustomWorkflowActivityInfo(ConnectionData connectionData, string filePath, string pluginTypeName)
        {
            if (string.IsNullOrEmpty(pluginTypeName))
            {
                this._iWriteToOutput.WriteToOutput(
                    connectionData
                    , Properties.OutputStrings.XmlNodeIsEmptyOrNotFoundedFormat2
                    , "SandboxCustomActivityInfo/CustomActivityInfo/TypeName"
                    , filePath
                );

                return false;
            }

            return true;
        }

        private async Task<Tuple<bool, PluginType>> GetPluginTypeByAttribute(IOrganizationServiceExtented service, CommonConfiguration commonConfig, string pluginTypeName)
        {
            var repositoryPluginType = new PluginTypeRepository(service);

            var pluginType = await repositoryPluginType.FindPluginTypeAsync(pluginTypeName);

            if (pluginType == null)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PluginTypeNotFoundedInConnectionFormat2, service.ConnectionData.Name, pluginTypeName);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                WindowHelper.OpenPluginTypeExplorer(_iWriteToOutput, service, commonConfig);
            }

            return Tuple.Create(pluginType != null, pluginType);
        }

        private async Task DifferencePluginTypeCustomWorkflowActivityInfo(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, PluginType pluginType)
        {
            string customWorkflowActivityInfo = pluginType.CustomWorkflowActivityInfo;

            string fieldTitle = PluginType.Schema.Headers.customworkflowactivityinfo;

            string fileTitle2 = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, pluginType.TypeName, fieldTitle, FileExtension.xml);
            string filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

            if (string.IsNullOrEmpty(customWorkflowActivityInfo))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, PluginType.Schema.EntitySchemaName, pluginType.TypeName, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            try
            {
                customWorkflowActivityInfo = ContentComparerHelper.FormatXmlByConfiguration(
                    customWorkflowActivityInfo
                    , commonConfig
                    , XmlOptionsControls.PluginTypeCustomWorkflowActivityInfoXmlOptions
                    , schemaName: AbstractDynamicCommandXsdSchemas.PluginTypeCustomWorkflowActivityInfoSchema
                );

                File.WriteAllText(filePath2, customWorkflowActivityInfo, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, PluginType.Schema.EntitySchemaName, pluginType.TypeName, fieldTitle, filePath2);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            string fileLocalPath = filePath;
            string fileLocalTitle = Path.GetFileName(filePath);

            await this._iWriteToOutput.ProcessStartProgramComparerAsync(service.ConnectionData, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);

            service.TryDispose();
        }

        private Task GetCurrentPluginTypeCustomWorkflowActivityInfoAsync(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, PluginType pluginType)
        {
            return Task.Run(() => GetCurrentPluginTypeCustomWorkflowActivityInfo(service, commonConfig, doc, filePath, pluginType));
        }

        private void GetCurrentPluginTypeCustomWorkflowActivityInfo(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, PluginType pluginType)
        {
            string customWorkflowActivityInfo = pluginType.CustomWorkflowActivityInfo;

            if (string.IsNullOrEmpty(customWorkflowActivityInfo))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, PluginType.Schema.EntitySchemaName, pluginType.TypeName, PluginType.Schema.Headers.customworkflowactivityinfo);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            customWorkflowActivityInfo = ContentComparerHelper.FormatXmlByConfiguration(
                customWorkflowActivityInfo
                , commonConfig
                , XmlOptionsControls.PluginTypeCustomWorkflowActivityInfoXmlOptions
                , schemaName: AbstractDynamicCommandXsdSchemas.PluginTypeCustomWorkflowActivityInfoSchema
            );

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string currentFileName = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, pluginType.TypeName, PluginType.Schema.Headers.customworkflowactivityinfo, FileExtension.xml);
            string currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(currentFileName));

            try
            {
                File.WriteAllText(currentFilePath, customWorkflowActivityInfo, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, PluginType.Schema.EntitySchemaName, pluginType.TypeName, PluginType.Schema.Headers.customworkflowactivityinfo, currentFilePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, currentFilePath);

                service.TryDispose();
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }
    }
}
