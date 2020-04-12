using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public abstract class BaseController<TWriteToOutput> where TWriteToOutput : IWriteToOutput
    {
        protected readonly TWriteToOutput _iWriteToOutput = default(TWriteToOutput);

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public BaseController(TWriteToOutput iWriteToOutput)
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

        protected Task<IOrganizationServiceExtented> ConnectAndWriteToOutputAsync(ConnectionData connectionData)
        {
            return QuickConnection.ConnectAndWriteToOutputAsync(this._iWriteToOutput, connectionData);
        }

        protected async Task<Tuple<IOrganizationServiceExtented, IOrganizationServiceExtented>> ConnectAndWriteToOutputDoubleConnectionAsync(ConnectionData connectionData1, ConnectionData connectionData2)
        {
            if (connectionData1 == null)
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.NoCRMConnection1);
                return null;
            }

            if (connectionData2 == null)
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.NoCRMConnection2);
                return null;
            }

            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.ConnectingToCRM);
            this._iWriteToOutput.WriteToOutput(null, string.Empty);
            this._iWriteToOutput.WriteToOutput(null, connectionData1.GetConnectionDescription());
            this._iWriteToOutput.WriteToOutput(null, string.Empty);
            this._iWriteToOutput.WriteToOutput(null, connectionData2.GetConnectionDescription());

            var task1 = QuickConnection.ConnectAsync(connectionData1);
            var task2 = QuickConnection.ConnectAsync(connectionData2);

            var service1 = await task1;
            var service2 = await task2;

            if (service1 == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData1, Properties.OutputStrings.ConnectionFailedFormat1, connectionData1.Name);
                return null;
            }

            if (service2 == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData2, Properties.OutputStrings.ConnectionFailedFormat1, connectionData2.Name);
                return null;
            }

            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CurrentServiceEndpointConnectionFormat2, connectionData1.Name, service1.CurrentServiceEndpoint);
            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CurrentServiceEndpointConnectionFormat2, connectionData2.Name, service2.CurrentServiceEndpoint);

            return Tuple.Create(service1, service2);
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

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var getResult = await entityGetter(service, commonConfig, attribute.Value);

            if (!getResult.Item1)
            {
                return;
            }

            await continueAction(service, commonConfig, doc, filePath, getResult.Item2);
        }

        protected async Task ConnectAndExecuteActionAsync(
            ConnectionData connectionData
            , string operationNameFormat
            , Action<IOrganizationServiceExtented> action
            , params object[] args
        )
        {
            var operation = FormatOperationName(connectionData, operationNameFormat, args);

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

        protected async Task ConnectAndExecuteActionAsync(
            ConnectionData connectionData
            , string operationNameFormat
            , Func<IOrganizationServiceExtented, Task> action
            , params object[] args
        )
        {
            var operation = FormatOperationName(connectionData, operationNameFormat, args);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                var service = await ConnectAndWriteToOutputAsync(connectionData);

                if (service == null)
                {
                    return;
                }

                await action(service);
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

        protected void GetFileGenerationConfigurationAndOpenExplorer(string operation, Action<FileGenerationConfiguration> action)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, operation);

            try
            {
                var fileGenerationConfiguration = FileGenerationConfiguration.GetConfiguration();

                action(fileGenerationConfiguration);
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

        protected async Task CheckEncodingCheckReadOnlyConnectExecuteActionAsync(
            ConnectionData connectionData
            , string operationNameFormat
            , IEnumerable<SelectedFile> selectedFiles
            , bool checkReadOnly
            , Func<IOrganizationServiceExtented, Task> action
            , params object[] args
        )
        {
            var operation = FormatOperationName(connectionData, operationNameFormat, args);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

                if (checkReadOnly)
                {
                    if (connectionData == null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                        return;
                    }

                    if (connectionData.IsReadOnly)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                        return;
                    }
                }

                var service = await ConnectAndWriteToOutputAsync(connectionData);

                if (service == null)
                {
                    return;
                }

                await action(service);
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

        private static string FormatOperationName(ConnectionData connectionData, string operationNameFormat, object[] args)
        {
            var formatArgs = new List<object>() { connectionData?.Name };

            formatArgs.AddRange(args);

            string operation = string.Format(operationNameFormat, formatArgs.ToArray());

            return operation;
        }

        protected void CheckingFilesEncodingAndWriteEmptyLines(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding)
        {
            filesWithoutUTF8Encoding = null;

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OperationNames.CheckingFilesEncoding);

            CheckingFilesEncoding(connectionData, selectedFiles, out filesWithoutUTF8Encoding);

            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
        }

        protected void CheckingFilesEncoding(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding)
        {
            filesWithoutUTF8Encoding = new List<SelectedFile>();

            int countWithUTF8Encoding = 0;

            List<string> listNotExistsOnDisk = new List<string>();

            List<string> listNotHaveBOM = new List<string>();

            List<string> listWrongEncoding = new List<string>();

            List<string> listMultipleEncodingHasUTF8 = new List<string>();

            List<string> listMultipleEncodingHasNotUTF8 = new List<string>();

            foreach (var selectedFile in selectedFiles)
            {
                if (File.Exists(selectedFile.FilePath))
                {
                    var arrayFile = File.ReadAllBytes(selectedFile.FilePath);

                    var encodings = ContentComparerHelper.GetFileEncoding(arrayFile);

                    if (encodings.Count > 0)
                    {
                        if (encodings.Count == 1)
                        {
                            if (encodings[0].CodePage == Encoding.UTF8.CodePage)
                            {
                                countWithUTF8Encoding++;
                            }
                            else
                            {
                                listWrongEncoding.Add(string.Format(Properties.OutputStrings.FileHasEncodingFormat2, selectedFile.FriendlyFilePath, encodings[0].EncodingName));

                                filesWithoutUTF8Encoding.Add(selectedFile);
                            }
                        }
                        else
                        {
                            filesWithoutUTF8Encoding.Add(selectedFile);

                            bool hasUTF8 = false;

                            StringBuilder str = new StringBuilder();

                            foreach (var enc in encodings)
                            {
                                if (enc.CodePage == Encoding.UTF8.CodePage)
                                {
                                    hasUTF8 = true;
                                }

                                if (str.Length > 0)
                                {
                                    str.Append(", ");
                                    str.Append(enc.EncodingName);
                                }
                            }

                            if (hasUTF8)
                            {
                                listMultipleEncodingHasUTF8.Add(string.Format(Properties.OutputStrings.FileHasEncodingFormat2, selectedFile.FriendlyFilePath, str.ToString()));
                            }
                            else
                            {
                                listMultipleEncodingHasNotUTF8.Add(string.Format(Properties.OutputStrings.FileHasEncodingFormat2, selectedFile.FriendlyFilePath, str.ToString()));
                            }
                        }
                    }
                    else
                    {
                        listNotHaveBOM.Add(selectedFile.FriendlyFilePath);

                        filesWithoutUTF8Encoding.Add(selectedFile);
                    }
                }
                else
                {
                    listNotExistsOnDisk.Add(selectedFile.FilePath);
                }
            }

            if (listNotHaveBOM.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FilesDoesNotHaveEncodingFormat1, listNotHaveBOM.Count);

                listNotHaveBOM.Sort();

                listNotHaveBOM.ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, item));
            }

            if (listWrongEncoding.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FilesHasWrongEncodingFormat1, listWrongEncoding.Count);

                listWrongEncoding.Sort();

                listWrongEncoding.ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, item));
            }

            if (listMultipleEncodingHasUTF8.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FilesCompliesMultipleEncodingWithUTF8Format1, listMultipleEncodingHasUTF8.Count);

                listMultipleEncodingHasUTF8.Sort();

                listMultipleEncodingHasUTF8.ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, item));
            }

            if (listMultipleEncodingHasNotUTF8.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FilesCompliesMultipleEncodingWithoutUTF8Format1, listMultipleEncodingHasNotUTF8.Count);

                listMultipleEncodingHasNotUTF8.Sort();

                listMultipleEncodingHasNotUTF8.ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, item));
            }

            if (listNotExistsOnDisk.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, listNotExistsOnDisk.Count);

                listNotExistsOnDisk.Sort();

                listNotExistsOnDisk.ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, item));
            }

            if (countWithUTF8Encoding > 0)
            {
                if (countWithUTF8Encoding == selectedFiles.Count())
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.AllFilesHasUTF8EncodingFormat1, countWithUTF8Encoding);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FilesHasUTF8EncodingFormat1, countWithUTF8Encoding);
                }
            }
        }
    }
}
