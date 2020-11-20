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
        public async Task ExecuteDifferenceSiteMap(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceSiteMapFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSiteMapExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, DifferenceSiteMapXml);
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

        public async Task ExecuteDifferenceSiteMap(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceSiteMapFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetSiteMapExecuteAction(connectionData, commonConfig, doc, filePath, null, DifferenceSiteMapXml);
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

        public async Task ExecuteUpdateSiteMap(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingSiteMapFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSiteMapExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, ValidateDocumentSiteMapXml, UpdateSiteMapXml);
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

        public async Task ExecuteUpdateSiteMap(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingSiteMapFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetSiteMapExecuteAction(connectionData, commonConfig, doc, filePath, ValidateDocumentSiteMapXml, UpdateSiteMapXml);
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

        public async Task ExecuteOpenInWebSiteMap(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , SiteMap.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(ActionOnComponent.OpenInWeb)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSiteMapExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, OpenInWebSiteMapXml);
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

        public async Task ExecuteGetSiteMapCurrentXml(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.GettingSiteMapCurrentXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetSiteMapExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, GetCurrentSiteMapXmlAsync);
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

        private Task CheckAttributeValidateGetSiteMapExecuteAction(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , XDocument doc
            , string filePath
            , Func<ConnectionData, XDocument, Task<bool>> validatorDocument
            , Func<IOrganizationServiceExtented, CommonConfiguration, XDocument, string, SiteMap, Task> continueAction
        )
        {
            return CheckAttributeValidateGetEntityExecuteAction(
                connectionData
                , commonConfig
                , doc
                , filePath
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique
                , null
                , validatorDocument
                , GetSiteMapByAttribute
                , continueAction
            );
        }

        private async Task<Tuple<bool, SiteMap>> GetSiteMapByAttribute(IOrganizationServiceExtented service, CommonConfiguration commonConfig, string siteMapNameUnique)
        {
            var repositorySiteMap = new SiteMapRepository(service);

            var siteMap = await repositorySiteMap.FindByExactNameAsync(siteMapNameUnique ?? string.Empty, new ColumnSet(true));

            if (siteMap == null)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionSiteMapWasNotFoundFormat2, service.ConnectionData.Name, SiteMap.Schema.EntityLogicalName, siteMapNameUnique);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                WindowHelper.OpenExportSiteMapExplorer(_iWriteToOutput, service, commonConfig, siteMapNameUnique);
            }

            return Tuple.Create(siteMap != null, siteMap);
        }

        private async Task DifferenceSiteMapXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SiteMap siteMap)
        {
            string siteMapXml = siteMap.SiteMapXml;

            string fieldTitle = SiteMap.Schema.Headers.sitemapxml;

            string fileTitle2 = EntityFileNameFormatter.GetSiteMapFileName(service.ConnectionData.Name, siteMap.SiteMapNameUnique, siteMap.Id, fieldTitle, FileExtension.xml);
            string filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

            if (string.IsNullOrEmpty(siteMapXml))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SiteMap.Schema.EntityLogicalName, siteMap.SiteMapNameUnique, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            try
            {
                siteMapXml = ContentComparerHelper.FormatXmlByConfiguration(
                    siteMapXml
                    , commonConfig
                    , XmlOptionsControls.SiteMapXmlOptions
                    , schemaName: AbstractDynamicCommandXsdSchemas.SiteMapXmlSchema
                    , siteMapUniqueName: siteMap.SiteMapNameUnique ?? string.Empty
                );

                File.WriteAllText(filePath2, siteMapXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SiteMap.Schema.EntityLogicalName, siteMap.SiteMapNameUnique, fieldTitle, filePath2);
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

        private async Task UpdateSiteMapXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SiteMap siteMap)
        {
            string fieldTitle = SiteMap.Schema.Headers.sitemapxml;

            {
                string siteMapXml = siteMap.SiteMapXml;

                if (!string.IsNullOrEmpty(siteMapXml))
                {
                    commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                    string fileNameBackUp = EntityFileNameFormatter.GetSiteMapFileName(service.ConnectionData.Name, siteMap.SiteMapNameUnique, siteMap.Id, fieldTitle + " BackUp", FileExtension.xml);
                    string filePathBackUp = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileNameBackUp));

                    try
                    {
                        siteMapXml = ContentComparerHelper.FormatXmlByConfiguration(
                            siteMapXml
                            , commonConfig
                            , XmlOptionsControls.SiteMapXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.SiteMapXmlSchema
                            , siteMapUniqueName: siteMap.SiteMapNameUnique ?? string.Empty
                        );

                        File.WriteAllText(filePathBackUp, siteMapXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SiteMap.Schema.EntityLogicalName, siteMap.SiteMapNameUnique, fieldTitle, filePathBackUp);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SiteMap.Schema.EntityLogicalName, siteMap.SiteMapNameUnique, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }

            var newText = doc.ToString(SaveOptions.DisableFormatting);

            var updateEntity = new SiteMap
            {
                Id = siteMap.Id
            };
            updateEntity.Attributes[SiteMap.Schema.Attributes.sitemapxml] = newText;

            await service.UpdateAsync(updateEntity);

            _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingSiteMapFormat3, service.ConnectionData.Name, siteMap.SiteMapName, siteMap.Id.ToString());

            var repositoryPublish = new PublishActionsRepository(service);

            await repositoryPublish.PublishSiteMapsAsync(new[] { siteMap.Id });

            service.TryDispose();
        }

        private async Task<bool> ValidateDocumentSiteMapXml(ConnectionData connectionData, XDocument doc)
        {
            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, SiteMap.Schema.Attributes.sitemapxml);

            ContentComparerHelper.ClearRoot(doc);

            bool validateResult = await SiteMapRepository.ValidateXmlDocumentAsync(connectionData, _iWriteToOutput, doc);

            if (!validateResult)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingXmlForFieldFailedFormat1, SiteMap.Schema.Attributes.sitemapxml);
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

        private Task OpenInWebSiteMapXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SiteMap siteMap)
        {
            return Task.Run(() =>
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SiteMap, siteMap.Id);
                service.TryDispose();
            });
        }

        private Task GetCurrentSiteMapXmlAsync(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SiteMap siteMap)
        {
            return Task.Run(() => GetCurrentSiteMapXml(service, commonConfig, doc, filePath, siteMap));
        }

        private void GetCurrentSiteMapXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, SiteMap siteMap)
        {
            string siteMapXml = siteMap.SiteMapXml;

            if (string.IsNullOrEmpty(siteMapXml))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SiteMap.Schema.EntityLogicalName, siteMap.SiteMapNameUnique, SiteMap.Schema.Headers.sitemapxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            siteMapXml = ContentComparerHelper.FormatXmlByConfiguration(
                siteMapXml
                , commonConfig
                , XmlOptionsControls.SiteMapXmlOptions
                , schemaName: AbstractDynamicCommandXsdSchemas.SiteMapXmlSchema
                , siteMapUniqueName: siteMap.SiteMapNameUnique ?? string.Empty
            );

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string currentFileName = EntityFileNameFormatter.GetSiteMapFileName(service.ConnectionData.Name, siteMap.SiteMapNameUnique, siteMap.Id, SiteMap.Schema.Headers.sitemapxml, FileExtension.xml);
            string currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(currentFileName));

            try
            {
                File.WriteAllText(currentFilePath, siteMapXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SiteMap.Schema.EntityLogicalName, siteMap.SiteMapNameUnique, SiteMap.Schema.Headers.sitemapxml, currentFilePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            this._iWriteToOutput.PerformAction(service.ConnectionData, currentFilePath);

            service.TryDispose();
        }
    }
}
