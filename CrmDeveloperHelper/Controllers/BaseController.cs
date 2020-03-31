using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public abstract class BaseController
    {
        protected readonly IWriteToOutput _iWriteToOutput = null;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public BaseController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        protected bool ParseXmlDocument(ConnectionData connectionData, SelectedFile selectedFile, out XDocument doc)
        {
            doc = null;

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return false;
            }

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentComparerHelper.TryParseXmlDocument(fileText, out doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                return false;
            }

            return true;
        }

        protected async Task CheckAttributeValidateGetEntityExecuteAction<T>(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , XDocument doc
            , string filePath
            , XName attributeName
            , Func<ConnectionData, string, XAttribute, bool> validatorAttribute
            , Func<ConnectionData, XDocument, Task<bool>> validatorDocument
            , Func<IOrganizationServiceExtented, CommonConfiguration, string, Task<Tuple<bool, T>>> entityGetter
            , Func<IOrganizationServiceExtented, CommonConfiguration, XDocument, string, T, Task> continueAction
        )
        {
            var attribute = doc.Root.Attribute(attributeName);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(
                    connectionData
                    , Properties.OutputStrings.FileNotContainsXmlAttributeFormat2
                    , attributeName.ToString()
                    , filePath
                );

                return;
            }

            if (validatorAttribute != null && !validatorAttribute(connectionData, filePath, attribute))
            {
                return;
            }

            if (validatorDocument != null && !await validatorDocument(connectionData, doc))
            {
                return;
            }

            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var getResult = await entityGetter(service, commonConfig, attribute.Value);

            if (!getResult.Item1)
            {
                return;
            }

            await continueAction(service, commonConfig, doc, filePath, getResult.Item2);
        }

        protected async Task<IOrganizationServiceExtented> ConnectAndWriteToOutputAsync(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return null;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return null;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            return service;
        }

        protected async Task ConnectAndOpenExplorerAsync(ConnectionData connectionData, string operationNameFormat1, Action<IOrganizationServiceExtented> action)
        {
            string operation = string.Format(operationNameFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                var service = await ConnectAndWriteToOutputAsync(connectionData);

                if (service == null)
                {
                    return;
                }

                action(service);
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

        protected void GetFileGenerationOptionsAndOpenExplorer(string operation, Action<FileGenerationOptions> action)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, operation);

            try
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                action(fileGenerationOptions);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, operation);
            }
        }
    }
}
