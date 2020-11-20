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
        public async Task ExecuteDifferenceSystemForm(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceSystemFormFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSystemFormExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, DifferenceSystemFormXml);
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

        public async Task ExecuteDifferenceSystemForm(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceSystemFormFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetSystemFormExecuteAction(connectionData, commonConfig, doc, filePath, null, DifferenceSystemFormXml);
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

        public async Task ExecuteUpdateSystemForm(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingSystemFormFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSystemFormExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, ValidateDocumentSystemFormXml, UpdateSystemFormXml);
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

        public async Task ExecuteUpdateSystemForm(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingSystemFormFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetSystemFormExecuteAction(connectionData, commonConfig, doc, filePath, ValidateDocumentSystemFormXml, UpdateSystemFormXml);
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

        public async Task ExecuteOpenInWebSystemForm(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , SystemForm.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(ActionOnComponent.OpenInWeb)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSystemFormExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, OpenInWebSystemFormXml);
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

        public async Task ExecuteGetSystemFormCurrentXml(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.GettingSystemFormCurrentXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSystemFormExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, GetCurrentSystemFormXmlAsync);
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

        public async Task ExecuteGetSystemFormCurrentAttribute(ConnectionData connectionData, CommonConfiguration commonConfig, Guid formId, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.GettingSystemFormCurrentAttributeFormat2
                , (service) => GettingSystemFormCurrentAttribute(service, commonConfig, formId, actionOnComponent, fieldName, fieldTitle)
                , fieldTitle
            );
        }

        private async Task GettingSystemFormCurrentAttribute(IOrganizationServiceExtented service, CommonConfiguration commonConfig, Guid formId, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
        {
            var repositorySystemForm = new SystemFormRepository(service);

            var systemForm = await repositorySystemForm.GetByIdAsync(formId, new ColumnSet(true));

            if (actionOnComponent == ActionOnComponent.SingleXmlField)
            {
                if (string.Equals(fieldName, SystemForm.Schema.Attributes.formxml, StringComparison.InvariantCultureIgnoreCase))
                {
                    GetCurrentSystemFormXml(service, commonConfig, systemForm);
                }
            }
            else if (actionOnComponent == ActionOnComponent.SingleField)
            {
                if (string.Equals(fieldName, SystemForm.Schema.Attributes.formjson, StringComparison.InvariantCultureIgnoreCase))
                {
                    GetCurrentSystemFormJson(service, commonConfig, systemForm);
                }
            }
            else if (actionOnComponent == ActionOnComponent.EntityDescription)
            {
                await GetCurrentEntityDescription(service, commonConfig, systemForm);
            }
            else if (actionOnComponent == ActionOnComponent.Description)
            {
                await GetCurrentFormDescription(service, commonConfig, systemForm);
            }
        }

        public async Task ExecuteOpeningLinkedSystemForm(ConnectionData connectionData, CommonConfiguration commonConfig, ActionOnComponent actionOnComponent, string entityName, Guid formId, int formType)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ActionOnComponentFormat3
                , (service) => OpeningLinkedSystemForm(service, commonConfig, actionOnComponent, entityName, formId, formType)
                , "Linked " + SystemForm.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(ActionOnComponent.OpenInWeb)
            );
        }

        private void OpeningLinkedSystemForm(IOrganizationServiceExtented service, CommonConfiguration commonConfig, ActionOnComponent actionOnComponent, string entityName, Guid formId, int formType)
        {
            if (actionOnComponent == ActionOnComponent.OpenInWeb)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SystemForm, formId);
                service.TryDispose();
            }
            else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInWeb)
            {
                service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SystemForm, formId);
                service.TryDispose();
            }
            else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInExplorer)
            {
                WindowHelper.OpenSolutionComponentDependenciesExplorer(
                    _iWriteToOutput
                    , service
                    , null
                    , commonConfig
                    , (int)ComponentType.SystemForm
                    , formId
                    , null
                );
            }
            else if (actionOnComponent == ActionOnComponent.OpenSolutionsListWithComponentInExplorer)
            {
                WindowHelper.OpenExplorerSolutionExplorer(
                    _iWriteToOutput
                    , service
                    , commonConfig
                    , (int)ComponentType.SystemForm
                    , formId
                    , null
                );
            }
        }

        public async Task ExecuteChangingLinkedSystemFormInEntityEditor(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, Guid formId, int formType)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ChangingLinkedSystemFormInEntityEditorFormat1
                , (service) => WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, SystemForm.EntityLogicalName, formId)
            );
        }

        public async Task ExecuteCopingToClipboardSystemFormCurrentTabsAndSections(ConnectionData connectionData, CommonConfiguration commonConfig, JavaScriptObjectType jsObjectType, string entityName, Guid formId, int formType)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ChangingLinkedSystemFormInEntityEditorFormat1
                , (service) => CopingToClipboardSystemFormCurrentTabsAndSections(service, commonConfig, jsObjectType, entityName, formId, formType)
            );
        }

        private async Task CopingToClipboardSystemFormCurrentTabsAndSections(IOrganizationServiceExtented service, CommonConfiguration commonConfig, JavaScriptObjectType jsObjectType, string entityName, Guid formId, int formType)
        {
            var repositorySystemForm = new SystemFormRepository(service);

            var systemForm = await repositorySystemForm.GetByIdAsync(formId, new ColumnSet(true));

            string formXml = systemForm.FormXml;

            if (string.IsNullOrEmpty(formXml))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            try
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                var config = new CreateFileJavaScriptConfiguration(fileGenerationOptions);

                var doc = XElement.Parse(formXml);

                var descriptor = new SolutionComponentDescriptor(service);

                descriptor.SetSettings(commonConfig);

                var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

                var tabs = handler.GetFormTabs(doc);

                var stringBuilder = new StringBuilder();

                using (var writer = new StringWriter(stringBuilder))
                {
                    var handlerCreate = new CreateFormTabsJavaScriptHandler(writer, config, jsObjectType, service);

                    handlerCreate.WriteContentOnlyForm(tabs);
                }

                ClipboardHelper.SetText(stringBuilder.ToString().Trim(' ', '\r', '\n'));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CopyingEntityJavaScriptContentOnFormCompletedFormat2, systemForm.ObjectTypeCode, systemForm.Name);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                service.TryDispose();
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private Task CheckAttributeValidateGetSystemFormExecuteAction(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , XDocument doc
            , string filePath
            , Func<ConnectionData, XDocument, Task<bool>> validatorDocument
            , Func<IOrganizationServiceExtented, CommonConfiguration, XDocument, string, SystemForm, Task> continueAction
        )
        {
            return CheckAttributeValidateGetEntityExecuteAction(
                connectionData
                , commonConfig
                , doc
                , filePath
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId
                , ValidateAttributeSystemFormXml
                , validatorDocument
                , GetSystemFormByAttribute
                , continueAction
            );
        }

        private async Task<Tuple<bool, SystemForm>> GetSystemFormByAttribute(IOrganizationServiceExtented service, CommonConfiguration commonConfig, string systemFormId)
        {
            var repositorySystemForm = new SystemFormRepository(service);

            var systemForm = await repositorySystemForm.GetByIdAsync(Guid.Parse(systemFormId), new ColumnSet(true));

            if (systemForm == null)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionSystemFormWasNotFoundFormat2, service.ConnectionData.Name, systemFormId);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                WindowHelper.OpenSystemFormExplorer(_iWriteToOutput, service, commonConfig, systemFormId);
            }

            return Tuple.Create(systemForm != null, systemForm);
        }

        private bool ValidateAttributeSystemFormXml(ConnectionData connectionData, string filePath, XAttribute attribute)
        {
            if (string.IsNullOrEmpty(attribute.Value))
            {
                this._iWriteToOutput.WriteToOutput(
                    connectionData
                    , Properties.OutputStrings.XmlAttributeIsEmptyFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId.ToString()
                    , filePath
                );

                return false;
            }

            if (!Guid.TryParse(attribute.Value, out _))
            {
                this._iWriteToOutput.WriteToOutput(
                    connectionData
                    , Properties.OutputStrings.XmlAttributeNotValidGuidFormat3
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId.ToString()
                    , attribute.Value
                    , filePath
                );

                return false;
            }

            return true;
        }

        private async Task DifferenceSystemFormXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SystemForm systemForm)
        {
            string formXml = systemForm.FormXml;

            string fieldTitle = SystemForm.Schema.Headers.formxml;

            string fileTitle2 = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, fieldTitle, FileExtension.xml);
            string filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

            if (string.IsNullOrEmpty(formXml))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            try
            {
                formXml = ContentComparerHelper.FormatXmlByConfiguration(
                    formXml
                    , commonConfig
                    , XmlOptionsControls.FormXmlOptions
                    , schemaName: AbstractDynamicCommandXsdSchemas.FormXmlSchema
                    , entityName: systemForm.ObjectTypeCode
                    , formId: systemForm.Id
                );

                File.WriteAllText(filePath2, formXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, fieldTitle, filePath2);
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

        private async Task UpdateSystemFormXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SystemForm systemForm)
        {
            {
                string fieldTitle = SystemForm.Schema.Headers.formxml;

                string formXml = systemForm.FormXml;

                if (!string.IsNullOrEmpty(formXml))
                {
                    commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                    string fileNameBackUp = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, fieldTitle + " BackUp", FileExtension.xml);
                    string filePathBackUp = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileNameBackUp));

                    try
                    {
                        formXml = ContentComparerHelper.FormatXmlByConfiguration(
                            formXml
                            , commonConfig
                            , XmlOptionsControls.FormXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.FormXmlSchema
                            , formId: systemForm.Id
                            , entityName: systemForm.ObjectTypeCode
                        );

                        File.WriteAllText(filePathBackUp, formXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntitySchemaName, systemForm.Name, fieldTitle, filePathBackUp);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntitySchemaName, systemForm.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }

            var newText = doc.ToString(SaveOptions.DisableFormatting);

            var updateEntity = new SystemForm
            {
                Id = systemForm.Id,
            };
            updateEntity.Attributes[SystemForm.Schema.Attributes.formxml] = newText;

            await service.UpdateAsync(updateEntity);

            var repositoryPublish = new PublishActionsRepository(service);

            _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingSystemFormFormat3, service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name);
            await repositoryPublish.PublishDashboardsAsync(new[] { systemForm.Id });

            if (systemForm.ObjectTypeCode.IsValidEntityName())
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingEntitiesFormat2, service.ConnectionData.Name, systemForm.ObjectTypeCode);
                await repositoryPublish.PublishEntitiesAsync(new[] { systemForm.ObjectTypeCode });
            }

            service.TryDispose();
        }

        private async Task<bool> ValidateDocumentSystemFormXml(ConnectionData connectionData, XDocument doc)
        {
            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, SystemForm.Schema.Attributes.formxml);

            ContentComparerHelper.ClearRoot(doc);

            bool validateResult = await SystemFormRepository.ValidateXmlDocumentAsync(connectionData, _iWriteToOutput, doc);

            if (!validateResult)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingXmlForFieldFailedFormat1, SystemForm.Schema.Attributes.formxml);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                var dialogResult = MessageBoxResult.Cancel;

                var t = new Thread(() =>
                {
                    dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (dialogResult != MessageBoxResult.OK)
                {
                    return false;
                }
            }

            return true;
        }

        private Task OpenInWebSystemFormXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SystemForm systemForm)
        {
            return Task.Run(() =>
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SystemForm, systemForm.Id);
                service.TryDispose();
            });
        }

        private Task GetCurrentSystemFormXmlAsync(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SystemForm systemForm)
        {
            return Task.Run(() => GetCurrentSystemFormXml(service, commonConfig, systemForm));
        }

        private void GetCurrentSystemFormXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, SystemForm systemForm)
        {
            string formXml = systemForm.FormXml;

            if (string.IsNullOrEmpty(formXml))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            formXml = ContentComparerHelper.FormatXmlByConfiguration(
                formXml
                , commonConfig
                , XmlOptionsControls.FormXmlOptions
                , schemaName: AbstractDynamicCommandXsdSchemas.FormXmlSchema
                , formId: systemForm.Id
                , entityName: systemForm.ObjectTypeCode
            );

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string currentFileName = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, SystemForm.Schema.Headers.formxml, FileExtension.xml);
            string currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(currentFileName));

            try
            {
                File.WriteAllText(currentFilePath, formXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, SystemForm.Schema.Headers.formxml, currentFilePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            this._iWriteToOutput.PerformAction(service.ConnectionData, currentFilePath);

            service.TryDispose();
        }

        private void GetCurrentSystemFormJson(IOrganizationServiceExtented service, CommonConfiguration commonConfig, SystemForm systemForm)
        {
            string formJson = systemForm.FormJson;

            if (string.IsNullOrEmpty(formJson))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, SystemForm.Schema.Headers.formjson);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            formJson = ContentComparerHelper.FormatJson(formJson);

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string currentFileName = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, SystemForm.Schema.Headers.formjson, FileExtension.json);
            string currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(currentFileName));

            try
            {
                File.WriteAllText(currentFilePath, formJson, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, SystemForm.Schema.Headers.formjson, currentFilePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            this._iWriteToOutput.PerformAction(service.ConnectionData, currentFilePath);

            service.TryDispose();
        }

        private async Task GetCurrentEntityDescription(IOrganizationServiceExtented service, CommonConfiguration commonConfig, SystemForm systemForm)
        {
            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string fileName = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, systemForm, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityDescriptionFormat3
                    , service.ConnectionData.Name
                    , systemForm.LogicalName
                    , filePath
                );
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            service.TryDispose();
        }

        private async Task GetCurrentFormDescription(IOrganizationServiceExtented service, CommonConfiguration commonConfig, SystemForm systemForm)
        {
            string formXml = systemForm.FormXml;

            if (string.IsNullOrEmpty(formXml))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.SetSettings(commonConfig);

            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            string fileName = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, "FormDescription", FileExtension.txt);
            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                XElement doc = XElement.Parse(formXml);

                string desc = await handler.GetFormDescriptionAsync(doc, systemForm.ObjectTypeCode, systemForm.Id, systemForm.Name, systemForm.FormattedValues[SystemForm.Schema.Attributes.type]);

                File.WriteAllText(filePath, desc, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, "FormDescription", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                service.TryDispose();
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }
    }
}
