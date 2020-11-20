using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
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
        public async Task ExecuteDifferenceWorkflow(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceWorkflowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetWorkflowExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, DifferenceWorkflowXaml);
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

        public async Task ExecuteDifferenceWorkflow(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceWorkflowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetWorkflowExecuteAction(connectionData, commonConfig, doc, filePath, null, DifferenceWorkflowXaml);
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

        public async Task ExecuteUpdateWorkflow(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingWorkflowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetWorkflowExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, ValidateDocumentWorkflowXaml, UpdateWorkflowXaml);
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

        public async Task ExecuteUpdateWorkflow(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingWorkflowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetWorkflowExecuteAction(connectionData, commonConfig, doc, filePath, ValidateDocumentWorkflowXaml, UpdateWorkflowXaml);
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

        public async Task ExecuteOpenInWebWorkflow(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , Workflow.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(ActionOnComponent.OpenInWeb)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetWorkflowExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, OpenInWebWorkflowXaml);
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

        public async Task ExecuteGetWorkflowCurrentXaml(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.GettingWorkflowCurrentXamlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetWorkflowExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, GetCurrentWorkflowXamlAsync);
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

        private Task CheckAttributeValidateGetWorkflowExecuteAction(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , XDocument doc
            , string filePath
            , Func<ConnectionData, XDocument, Task<bool>> validatorDocument
            , Func<IOrganizationServiceExtented, CommonConfiguration, XDocument, string, Workflow, Task> continueAction
        )
        {
            return CheckAttributeValidateGetEntityExecuteAction(
                connectionData
                , commonConfig
                , doc
                , filePath
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWorkflowId
                , ValidateAttributeWorkflowXaml
                , validatorDocument
                , GetWorkflowByAttribute
                , continueAction
            );
        }

        private async Task<Tuple<bool, Workflow>> GetWorkflowByAttribute(IOrganizationServiceExtented service, CommonConfiguration commonConfig, string workflowId)
        {
            var repositoryWorkflow = new WorkflowRepository(service);

            var workflow = await repositoryWorkflow.GetByIdAsync(Guid.Parse(workflowId), new ColumnSet(true));

            if (workflow == null)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionWorkflowWasNotFoundFormat2, service.ConnectionData.Name, workflowId);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                WindowHelper.OpenWorkflowExplorer(_iWriteToOutput, service, commonConfig, workflowId);
            }

            return Tuple.Create(workflow != null, workflow);
        }

        private bool ValidateAttributeWorkflowXaml(ConnectionData connectionData, string filePath, XAttribute attribute)
        {
            if (string.IsNullOrEmpty(attribute.Value))
            {
                this._iWriteToOutput.WriteToOutput(
                    connectionData
                    , Properties.OutputStrings.XmlAttributeIsEmptyFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWorkflowId.ToString()
                    , filePath
                );

                return false;
            }

            if (!Guid.TryParse(attribute.Value, out _)
            )
            {
                this._iWriteToOutput.WriteToOutput(
                    connectionData
                    , Properties.OutputStrings.XmlAttributeNotValidGuidFormat3
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWorkflowId.ToString()
                    , attribute.Value
                    , filePath
                );

                return false;
            }

            return true;
        }

        private async Task DifferenceWorkflowXaml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, Workflow workflow)
        {
            string workflowXaml = workflow.Xaml;

            string fieldTitle = Workflow.Schema.Headers.xaml;

            string fileTitle2 = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, workflow.PrimaryEntity, workflow.FormattedValues[Workflow.Schema.Attributes.category], workflow.Name, fieldTitle, FileExtension.xml);
            string filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

            if (string.IsNullOrEmpty(workflowXaml))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow.Name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            try
            {
                workflowXaml = ContentComparerHelper.FormatXmlByConfiguration(
                    workflowXaml
                    , commonConfig
                    , XmlOptionsControls.WorkflowXmlOptions
                    , workflowId: workflow.Id
                );

                File.WriteAllText(filePath2, workflowXaml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow.Name, fieldTitle, filePath2);
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

        private async Task UpdateWorkflowXaml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, Workflow workflow)
        {
            string fieldTitle = Workflow.Schema.Headers.xaml;

            {
                string workflowXaml = workflow.Xaml;

                if (!string.IsNullOrEmpty(workflowXaml))
                {
                    commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                    string fileNameBackUp = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, workflow.PrimaryEntity, workflow.FormattedValues[Workflow.Schema.Attributes.category], workflow.Name, fieldTitle + " BackUp", FileExtension.xml);
                    string filePathBackUp = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileNameBackUp));

                    try
                    {
                        workflowXaml = ContentComparerHelper.FormatXmlByConfiguration(
                            workflowXaml
                            , commonConfig
                            , XmlOptionsControls.WorkflowXmlOptions
                            , workflowId: workflow.Id
                        );

                        File.WriteAllText(filePathBackUp, workflowXaml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow.Name, fieldTitle, filePathBackUp);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }

            if (workflow.StateCodeEnum == Workflow.Schema.OptionSets.statecode.Activated_1)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionDeactivatingWorkflowFormat2, service.ConnectionData.Name, workflow.Name);

                await service.ExecuteAsync<SetStateResponse>(new SetStateRequest()
                {
                    EntityMoniker = workflow.ToEntityReference(),
                    State = new OptionSetValue((int)Workflow.Schema.OptionSets.statecode.Draft_0),
                    Status = new OptionSetValue((int)Workflow.Schema.OptionSets.statuscode.Draft_0_Draft_1),
                });
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionUpdatingFieldFormat2, service.ConnectionData.Name, Workflow.Schema.Headers.xaml);

            var newText = doc.ToString(SaveOptions.DisableFormatting);

            var updateEntity = new Workflow
            {
                Id = workflow.Id,
            };
            updateEntity.Attributes[Workflow.Schema.Attributes.xaml] = newText;

            await service.UpdateAsync(updateEntity);

            if (workflow.StateCodeEnum == Workflow.Schema.OptionSets.statecode.Activated_1)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionActivatingWorkflowFormat2, service.ConnectionData.Name, workflow.Name);

                await service.ExecuteAsync<SetStateResponse>(new SetStateRequest()
                {
                    EntityMoniker = workflow.ToEntityReference(),
                    State = new OptionSetValue((int)Workflow.Schema.OptionSets.statecode.Activated_1),
                    Status = new OptionSetValue((int)Workflow.Schema.OptionSets.statuscode.Activated_1_Activated_2),
                });
            }

            service.TryDispose();
        }

        private async Task<bool> ValidateDocumentWorkflowXaml(ConnectionData connectionData, XDocument doc)
        {
            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, Workflow.Schema.Attributes.xaml);

            ContentComparerHelper.ClearRoot(doc);

            bool validateResult = await SiteMapRepository.ValidateXmlDocumentAsync(connectionData, _iWriteToOutput, doc);

            if (!validateResult)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingXmlForFieldFailedFormat1, Workflow.Schema.Attributes.xaml);
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

        private Task OpenInWebWorkflowXaml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, Workflow workflow)
        {
            return Task.Run(() =>
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Workflow, workflow.Id);
                service.TryDispose();
            });
        }

        private Task GetCurrentWorkflowXamlAsync(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, Workflow workflow)
        {
            return Task.Run(() => GetCurrentWorkflowXaml(service, commonConfig, doc, filePath, workflow));
        }

        private void GetCurrentWorkflowXaml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, Workflow workflow)
        {
            string workflowXaml = workflow.Xaml;

            if (string.IsNullOrEmpty(workflowXaml))
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow.Name, Workflow.Schema.Headers.xaml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            workflowXaml = ContentComparerHelper.FormatXmlByConfiguration(
                workflowXaml
                , commonConfig
                , XmlOptionsControls.WorkflowXmlOptions
                , workflowId: workflow.Id
            );

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string fieldTitle = Workflow.Schema.Headers.xaml;

            string currentFileName = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, workflow.PrimaryEntity, workflow.FormattedValues[Workflow.Schema.Attributes.category], workflow.Name, fieldTitle, FileExtension.xml);
            string currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(currentFileName));

            try
            {
                File.WriteAllText(currentFilePath, workflowXaml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow.Name, Workflow.Schema.Headers.xaml, currentFilePath);

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
