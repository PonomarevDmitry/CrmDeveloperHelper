using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public partial class ExportXmlController
    {
        public async Task ExecuteDifferenceWebResourceDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceWebResourceDependencyXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetWebResourceExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, DifferenceWebResourceDependencyXml);
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

        public async Task ExecuteDifferenceWebResourceDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceWebResourceDependencyXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetWebResourceExecuteAction(connectionData, commonConfig, doc, filePath, null, DifferenceWebResourceDependencyXml);
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

        public async Task ExecuteUpdateWebResourceDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingWebResourceDependencyXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetWebResourceExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, ValidateDocumentWebResourceDependencyXml, UpdateWebResourceDependencyXml);
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

        public async Task ExecuteUpdateWebResourceDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingWebResourceDependencyXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetWebResourceExecuteAction(connectionData, commonConfig, doc, filePath, ValidateDocumentWebResourceDependencyXml, UpdateWebResourceDependencyXml);
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

        public async Task ExecuteOpenInWebWebResourceDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , WebResource.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(ActionOnComponent.OpenInWeb)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetWebResourceExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, OpenInWebWebResourceDependencyXml);
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

        public async Task ExecuteGetWebResourceCurrentDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.GettingWebResourceCurrentDependencyXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetWebResourceExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, GetCurrentWebResourceDependencyXmlAsync);
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

        private Task CheckAttributeValidateGetWebResourceExecuteAction(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , XDocument doc
            , string filePath
            , Func<ConnectionData, XDocument, Task<bool>> validatorDocument
            , Func<IOrganizationServiceExtented, CommonConfiguration, XDocument, string, WebResource, Task> continueAction
        )
        {
            return CheckAttributeValidateGetEntityExecuteAction(
                connectionData
                , commonConfig
                , doc
                , filePath
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWebResourceName
                , ValidateAttributeWebResourceDependencyXml
                , validatorDocument
                , GetWebResourceByAttribute
                , continueAction
            );
        }

        private async Task<Tuple<bool, WebResource>> GetWebResourceByAttribute(IOrganizationServiceExtented service, CommonConfiguration commonConfig, string webResourceName)
        {
            var repositoryWebResource = new WebResourceRepository(service);

            var webResource = await repositoryWebResource.FindByExactNameAsync(webResourceName, ColumnSetInstances.AllColumns);

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionWebResourceWasNotFoundFormat2, service.ConnectionData.Name, webResourceName);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                WindowHelper.OpenWebResourceExplorer(_iWriteToOutput, service, commonConfig);
            }

            return Tuple.Create(webResource != null, webResource);
        }

        private bool ValidateAttributeWebResourceDependencyXml(ConnectionData connectionData, string filePath, XAttribute attribute)
        {
            if (string.IsNullOrEmpty(attribute.Value))
            {
                this._iWriteToOutput.WriteToOutput(
                    connectionData
                    , Properties.OutputStrings.XmlAttributeIsEmptyFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWebResourceName.ToString()
                    , filePath
                );

                return false;
            }

            return true;
        }

        private async Task DifferenceWebResourceDependencyXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, WebResource webResource)
        {
            string dependencyXml = webResource.DependencyXml;

            string fieldTitle = WebResource.Schema.Headers.dependencyxml;

            string fileTitle2 = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, webResource.Name, fieldTitle, FileExtension.xml);
            string filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

            if (string.IsNullOrEmpty(dependencyXml))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, WebResource.Schema.EntityLogicalName, webResource.Name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            try
            {
                dependencyXml = ContentComparerHelper.FormatXmlByConfiguration(
                    dependencyXml
                    , commonConfig
                    , XmlOptionsControls.WebResourceDependencyXmlOptions
                    , schemaName: AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlSchema
                    , webResourceName: webResource.Name
                );

                File.WriteAllText(filePath2, dependencyXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, WebResource.Schema.EntityLogicalName, webResource.Name, fieldTitle, filePath2);
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

        private async Task UpdateWebResourceDependencyXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, WebResource webResource)
        {
            {
                string fieldTitle = WebResource.Schema.Headers.dependencyxml;

                string dependencyXml = webResource.DependencyXml;

                if (!string.IsNullOrEmpty(dependencyXml))
                {
                    commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                    string fileNameBackUp = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, webResource.Name, fieldTitle + " BackUp", FileExtension.xml);
                    string filePathBackUp = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileNameBackUp));

                    try
                    {
                        dependencyXml = ContentComparerHelper.FormatXmlByConfiguration(
                            dependencyXml
                            , commonConfig
                            , XmlOptionsControls.WebResourceDependencyXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlSchema
                            , webResourceName: webResource.Name
                        );

                        File.WriteAllText(filePathBackUp, dependencyXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, WebResource.Schema.EntityLogicalName, webResource.Name, fieldTitle, filePathBackUp);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, WebResource.Schema.EntityLogicalName, webResource.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }

            var newText = doc.ToString(SaveOptions.DisableFormatting);

            var updateEntity = new WebResource
            {
                Id = webResource.Id
            };
            updateEntity.Attributes[WebResource.Schema.Attributes.dependencyxml] = newText;

            await service.UpdateAsync(updateEntity);

            var repositoryPublish = new PublishActionsRepository(service);

            _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingWebResourceFormat2, service.ConnectionData.Name, webResource.Name);

            await repositoryPublish.PublishWebResourcesAsync(new[] { webResource.Id });

            service.TryDispose();
        }

        private async Task<bool> ValidateDocumentWebResourceDependencyXml(ConnectionData connectionData, XDocument doc)
        {
            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, WebResource.Schema.Attributes.dependencyxml);

            ContentComparerHelper.ClearRoot(doc);

            bool validateResult = await WebResourceRepository.ValidateXmlDocumentAsync(connectionData, _iWriteToOutput, doc);

            if (!validateResult)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingXmlForFieldFailedFormat1, WebResource.Schema.Attributes.dependencyxml);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                var dialogResult = MessageBoxResult.Cancel;

                var thread = new Thread(() =>
                {
                    dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                thread.Join();

                if (dialogResult != MessageBoxResult.OK)
                {
                    return false;
                }
            }

            return true;
        }

        private Task OpenInWebWebResourceDependencyXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, WebResource webResource)
        {
            return Task.Run(() =>
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.WebResource, webResource.Id);

                service.TryDispose();
            });
        }

        private Task GetCurrentWebResourceDependencyXmlAsync(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, WebResource webResource)
        {
            return Task.Run(() => GetCurrentWebResourceDependencyXml(service, commonConfig, doc, filePath, webResource));
        }

        private void GetCurrentWebResourceDependencyXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, WebResource webResource)
        {
            string dependencyXml = webResource.DependencyXml;

            if (string.IsNullOrEmpty(dependencyXml))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, WebResource.Schema.EntityLogicalName, webResource.Name, WebResource.Schema.Headers.dependencyxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            dependencyXml = ContentComparerHelper.FormatXmlByConfiguration(
                dependencyXml
                , commonConfig
                , XmlOptionsControls.WebResourceDependencyXmlOptions
                , schemaName: AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlSchema
                , webResourceName: webResource.Name
            );

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string currentFileName = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, webResource.Name, WebResource.Schema.Headers.dependencyxml, FileExtension.xml);
            string currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(currentFileName));

            try
            {
                File.WriteAllText(currentFilePath, dependencyXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, WebResource.Schema.EntityLogicalName, webResource.Name, WebResource.Schema.Headers.dependencyxml, currentFilePath);

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
