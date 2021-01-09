using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public partial class ExportXmlController
    {
        public async Task ExecuteDifferenceSavedQuery(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceSavedQueryFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSavedQueryExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, DifferenceSavedQueryXml);
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

        public async Task ExecuteDifferenceSavedQuery(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceSavedQueryFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetSavedQueryExecuteAction(connectionData, commonConfig, doc, filePath, null, DifferenceSavedQueryXml);
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

        public async Task ExecuteUpdateSavedQuery(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingSavedQueryFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSavedQueryExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, ValidateDocumentSavedQueryXml, UpdateSavedQueryXml);
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

        public async Task ExecuteUpdateSavedQuery(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingSavedQueryFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetSavedQueryExecuteAction(connectionData, commonConfig, doc, filePath, ValidateDocumentSavedQueryXml, UpdateSavedQueryXml);
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

        public async Task ExecuteOpenInWebSavedQuery(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , SavedQuery.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(ActionOnComponent.OpenInWeb)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSavedQueryExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, OpenInWebSavedQueryXml);
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

        public async Task ExecuteGetSavedQueryCurrentXml(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.GettingSavedQueryCurrentXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSavedQueryExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, GetCurrentSavedQueryXmlAsync);
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

        private Task CheckAttributeValidateGetSavedQueryExecuteAction(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , XDocument doc
            , string filePath
            , Func<ConnectionData, XDocument, Task<bool>> validatorDocument
            , Func<IOrganizationServiceExtented, CommonConfiguration, XDocument, string, SavedQuery, Task> continueAction
        )
        {
            return CheckAttributeValidateGetEntityExecuteAction(
                connectionData
                , commonConfig
                , doc
                , filePath
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId
                , ValidateAttributeSavedQueryXml
                , validatorDocument
                , GetSavedQueryByAttribute
                , continueAction
            );
        }

        private async Task<Tuple<bool, SavedQuery>> GetSavedQueryByAttribute(IOrganizationServiceExtented service, CommonConfiguration commonConfig, string savedQueryId)
        {
            var repositorySavedQuery = new SavedQueryRepository(service);

            var savedQuery = await repositorySavedQuery.GetByIdAsync(Guid.Parse(savedQueryId), ColumnSetInstances.AllColumns);

            if (savedQuery == null)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionSavedQueryWasNotFoundFormat2, service.ConnectionData.Name, savedQueryId);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                WindowHelper.OpenSavedQueryExplorer(_iWriteToOutput, service, commonConfig, savedQueryId);
            }

            return Tuple.Create(savedQuery != null, savedQuery);
        }

        private bool ValidateAttributeSavedQueryXml(ConnectionData connectionData, string filePath, XAttribute attribute)
        {
            if (string.IsNullOrEmpty(attribute.Value))
            {
                this._iWriteToOutput.WriteToOutput(
                    connectionData
                    , Properties.OutputStrings.XmlAttributeIsEmptyFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId.ToString()
                    , filePath
                );

                return false;
            }

            if (!Guid.TryParse(attribute.Value, out _))
            {
                this._iWriteToOutput.WriteToOutput(
                    connectionData
                    , Properties.OutputStrings.XmlAttributeNotValidGuidFormat3
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId.ToString()
                    , attribute.Value
                    , filePath
                );

                return false;
            }

            return true;
        }

        private async Task DifferenceSavedQueryXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SavedQuery savedQuery)
        {
            string fieldName = SavedQueryRepository.GetFieldNameByXmlRoot(doc.Root.Name.ToString());
            string fieldTitle = SavedQueryRepository.GetFieldTitleByXmlRoot(doc.Root.Name.ToString());

            string xmlContent = savedQuery.GetAttributeValue<string>(fieldName);

            string fileTitle2 = EntityFileNameFormatter.GetSavedQueryFileName(service.ConnectionData.Name, savedQuery.ReturnedTypeCode, savedQuery.Name, fieldTitle, FileExtension.xml);
            string filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

            if (string.IsNullOrEmpty(xmlContent))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            try
            {
                xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                    xmlContent
                    , commonConfig
                    , XmlOptionsControls.SavedQueryXmlOptions
                    , schemaName: AbstractDynamicCommandXsdSchemas.FetchSchema
                    , savedQueryId: savedQuery.Id
                    , entityName: savedQuery.ReturnedTypeCode
                );

                File.WriteAllText(filePath2, xmlContent, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle, filePath2);
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

        private async Task UpdateSavedQueryXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SavedQuery savedQuery)
        {
            string fieldName = SavedQueryRepository.GetFieldNameByXmlRoot(doc.Root.Name.ToString());
            string fieldTitle = SavedQueryRepository.GetFieldTitleByXmlRoot(doc.Root.Name.ToString());

            if (string.Equals(fieldName, SavedQuery.Schema.Attributes.layoutxml, StringComparison.InvariantCulture)
                && !string.IsNullOrEmpty(savedQuery.ReturnedTypeCode)
            )
            {
                var entityData = service.ConnectionData.GetEntityIntellisenseData(savedQuery.ReturnedTypeCode);

                if (entityData != null && entityData.ObjectTypeCode.HasValue)
                {
                    XAttribute attr = doc.Root.Attribute("object");

                    if (attr != null)
                    {
                        attr.Value = entityData.ObjectTypeCode.ToString();
                    }
                }
            }

            {
                string xmlContent = savedQuery.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(xmlContent))
                {
                    commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                    string fileNameBackUp = EntityFileNameFormatter.GetSavedQueryFileName(service.ConnectionData.Name, savedQuery.ReturnedTypeCode, savedQuery.Name, fieldTitle + " BackUp", FileExtension.xml);
                    string filePathBackUp = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileNameBackUp));

                    try
                    {
                        xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            xmlContent
                            , commonConfig
                            , XmlOptionsControls.SavedQueryXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.FetchSchema
                            , savedQueryId: savedQuery.Id
                            , entityName: savedQuery.ReturnedTypeCode
                        );

                        File.WriteAllText(filePathBackUp, xmlContent, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle, filePathBackUp);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }

            var newText = doc.ToString(SaveOptions.DisableFormatting);

            if (string.Equals(fieldName, SavedQuery.Schema.Attributes.fetchxml, StringComparison.InvariantCulture))
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExecutingValidateSavedQueryRequest);

                var request = new ValidateSavedQueryRequest()
                {
                    FetchXml = newText,
                    QueryType = savedQuery.QueryType.GetValueOrDefault()
                };

                service.Execute(request);
            }

            var updateEntity = new SavedQuery
            {
                Id = savedQuery.Id,
            };
            updateEntity.Attributes[fieldName] = newText;

            await service.UpdateAsync(updateEntity);

            _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingEntitiesFormat2, service.ConnectionData.Name, savedQuery.ReturnedTypeCode);

            {
                var repositoryPublish = new PublishActionsRepository(service);

                await repositoryPublish.PublishEntitiesAsync(new[] { savedQuery.ReturnedTypeCode });
            }

            service.TryDispose();
        }

        private async Task<bool> ValidateDocumentSavedQueryXml(ConnectionData connectionData, XDocument doc)
        {
            string fieldName = SavedQueryRepository.GetFieldNameByXmlRoot(doc.Root.Name.ToString());
            string fieldTitle = SavedQueryRepository.GetFieldTitleByXmlRoot(doc.Root.Name.ToString());

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, fieldTitle);

            ContentComparerHelper.ClearRoot(doc);

            bool validateResult = await SavedQueryRepository.ValidateXmlDocumentAsync(connectionData, _iWriteToOutput, doc, fieldTitle);

            if (!validateResult)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingXmlForFieldFailedFormat1, fieldTitle);
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

        private Task OpenInWebSavedQueryXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SavedQuery savedQuery)
        {
            return Task.Run(() =>
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQuery, savedQuery.Id);
                service.TryDispose();
            });
        }

        private Task GetCurrentSavedQueryXmlAsync(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SavedQuery savedQuery)
        {
            return Task.Run(() => GetCurrentSavedQueryXml(service, commonConfig, doc, filePath, savedQuery));
        }

        private void GetCurrentSavedQueryXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SavedQuery savedQuery)
        {
            string fieldName = SavedQueryRepository.GetFieldNameByXmlRoot(doc.Root.Name.ToString());
            string fieldTitle = SavedQueryRepository.GetFieldTitleByXmlRoot(doc.Root.Name.ToString());

            string xmlContent = savedQuery.GetAttributeValue<string>(fieldName);

            if (string.IsNullOrEmpty(xmlContent))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string currentFileName = EntityFileNameFormatter.GetSavedQueryFileName(service.ConnectionData.Name, savedQuery.ReturnedTypeCode, savedQuery.Name, fieldTitle, FileExtension.xml);
            string currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(currentFileName));

            try
            {
                File.WriteAllText(currentFilePath, xmlContent, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle, currentFilePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, currentFilePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                service.TryDispose();
            }
        }
    }
}
