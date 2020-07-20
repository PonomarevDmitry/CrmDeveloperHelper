using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
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
        protected const string _tabSpacer = "      ";

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

        protected async Task CheckAttributeValidateGetEntityExecuteAction<T>(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , XDocument doc
            , string filePath
            , Func<XDocument, string> primaryIdGetter
            , Func<ConnectionData, string, string, bool> validatorAttribute
            , Func<ConnectionData, XDocument, Task<bool>> validatorDocument
            , Func<IOrganizationServiceExtented, CommonConfiguration, string, Task<Tuple<bool, T>>> entityGetter
            , Func<IOrganizationServiceExtented, CommonConfiguration, XDocument, string, T, Task> continueAction
        )
        {
            var primaryAttribute = primaryIdGetter(doc);

            if (!validatorAttribute(connectionData, filePath, primaryAttribute))
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

            var getResult = await entityGetter(service, commonConfig, primaryAttribute);

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
            , params string[] args
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
            , params string[] args
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
            , params string[] args
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

        protected async Task CheckEncodingConnectFindWebResourceExecuteActionAsync(
            ConnectionData connectionData
            , string operationNameFormat
            , IEnumerable<SelectedFile> selectedFiles
            , OpenFilesType openFilesType
            , Action<ConnectionData, IOrganizationServiceExtented, TupleList<SelectedFile, WebResource>> action
            , params string[] args
        )
        {
            var operation = FormatOperationName(connectionData, operationNameFormat, args);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

                if (connectionData == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                    return;
                }

                var compareResult = await GetWebResourcesWithType(connectionData, selectedFiles, openFilesType);

                if (compareResult == null)
                {
                    return;
                }

                action(connectionData, compareResult.Item1, compareResult.Item2);
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

        protected async Task CheckEncodingConnectFindWebResourceExecuteActionTaskAsync(
            ConnectionData connectionData
            , string operationNameFormat
            , IEnumerable<SelectedFile> selectedFiles
            , OpenFilesType openFilesType
            , Func<ConnectionData, IOrganizationServiceExtented, TupleList<SelectedFile, WebResource>, Task> action
            , params string[] args
        )
        {
            var operation = FormatOperationName(connectionData, operationNameFormat, args);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

                if (connectionData == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                    return;
                }

                var compareResult = await GetWebResourcesWithType(connectionData, selectedFiles, openFilesType);

                if (compareResult == null)
                {
                    return;
                }

                await action(connectionData, compareResult.Item1, compareResult.Item2);
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

        private static string FormatOperationName(ConnectionData connectionData, string operationNameFormat, string[] args)
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
                if (!File.Exists(selectedFile.FilePath))
                {
                    listNotExistsOnDisk.Add(selectedFile.FilePath);
                    continue;
                }

                if (!FileOperations.SupportsWebResourceTextType(selectedFile.FilePath))
                {
                    continue;
                }

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

        protected async Task<Tuple<IOrganizationServiceExtented, TupleList<SelectedFile, WebResource>>> GetWebResourcesWithType(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            IOrganizationServiceExtented service = null;

            TupleList<SelectedFile, WebResource> filesToOpen = new TupleList<SelectedFile, WebResource>();

            if (openFilesType == OpenFilesType.All)
            {
                foreach (var item in selectedFiles)
                {
                    filesToOpen.Add(item, null);
                }
            }
            else if (openFilesType == OpenFilesType.NotExistsInCrmWithoutLink || openFilesType == OpenFilesType.NotExistsInCrmWithLink)
            {
                var compareResult = await FindFilesNotExistsInCrmAsync(connectionData, selectedFiles);

                if (compareResult != null)
                {
                    service = compareResult.Item1;

                    if (openFilesType == OpenFilesType.NotExistsInCrmWithoutLink)
                    {
                        filesToOpen.AddRange(compareResult.Item2.Select(f => Tuple.Create(f, (WebResource)null)));
                    }
                    else if (openFilesType == OpenFilesType.NotExistsInCrmWithLink)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                }
            }
            else if (openFilesType == OpenFilesType.EqualByText
                    || openFilesType == OpenFilesType.NotEqualByText
            )
            {
                var compareResult = await ComparingFilesAndWebResourcesAsync(connectionData, selectedFiles, false);

                if (compareResult != null)
                {
                    service = compareResult.Item1;

                    if (openFilesType == OpenFilesType.EqualByText)
                    {
                        filesToOpen.AddRange(compareResult.Item2);
                    }
                    else if (openFilesType == OpenFilesType.NotEqualByText)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                }
            }
            else if (openFilesType == OpenFilesType.WithInserts
                    || openFilesType == OpenFilesType.WithDeletes
                    || openFilesType == OpenFilesType.WithComplexChanges
                    || openFilesType == OpenFilesType.WithMirrorChanges
                    || openFilesType == OpenFilesType.WithMirrorInserts
                    || openFilesType == OpenFilesType.WithMirrorDeletes
                    || openFilesType == OpenFilesType.WithMirrorComplexChanges
                )
            {
                var compareResult = await ComparingFilesAndWebResourcesAsync(connectionData, selectedFiles, true);

                if (compareResult != null)
                {
                    service = compareResult.Item1;

                    if (openFilesType == OpenFilesType.WithInserts)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsOnlyInserts).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithDeletes)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsOnlyDeletes).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithComplexChanges)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsComplexChanges).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithMirrorChanges)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsMirror).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithMirrorInserts)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsMirrorWithInserts).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithMirrorDeletes)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsMirrorWithDeletes).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                    else if (openFilesType == OpenFilesType.WithMirrorComplexChanges)
                    {
                        filesToOpen.AddRange(compareResult.Item3.Where(s => s.Item3.IsMirrorWithComplex).Select(f => Tuple.Create(f.Item1, f.Item2)));
                    }
                }
            }

            return Tuple.Create(service, filesToOpen);
        }

        private async Task<Tuple<IOrganizationServiceExtented
                , List<SelectedFile>
                , List<Tuple<SelectedFile, WebResource>>
                >> FindFilesNotExistsInCrmAsync(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles)
        {
            var service = await QuickConnection.ConnectAndWriteToOutputAsync(this._iWriteToOutput, connectionData);

            if (service == null)
            {
                return null;
            }

            bool isconnectionDataDirty = false;

            var listNotFoundedInCrmNoLink = new List<SelectedFile>();
            var listNotFoundedInCrmWithLink = new List<Tuple<SelectedFile, WebResource>>();

            List<string> listNotExistsOnDisk = new List<string>();

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            var groups = selectedFiles.GroupBy(sel => sel.Extension);

            foreach (var gr in groups)
            {
                var names = gr.Select(sel => sel.FriendlyFilePath).ToArray();

                var dict = webResourceRepository.FindMultiple(gr.Key, names
                    , new ColumnSet
                    (
                        WebResource.Schema.EntityPrimaryIdAttribute
                        , WebResource.Schema.Attributes.name
                        , WebResource.Schema.Attributes.webresourcetype
                    )
                );

                foreach (var selectedFile in gr)
                {
                    if (!File.Exists(selectedFile.FilePath))
                    {
                        listNotExistsOnDisk.Add(selectedFile.FilePath);
                        continue;
                    }

                    string name = selectedFile.FriendlyFilePath.ToLower();

                    var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, name, gr.Key);

                    if (webresource != null)
                    {
                        // Запоминается файл
                        isconnectionDataDirty = true;
                        connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);
                    }
                    else
                    {
                        Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                        if (webId.HasValue)
                        {
                            webresource = await webResourceRepository.GetByIdAsync(webId.Value, new ColumnSet(true));

                            if (webresource != null)
                            {
                                listNotFoundedInCrmWithLink.Add(Tuple.Create(selectedFile, webresource));

                                isconnectionDataDirty = true;
                                connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);
                            }
                            else
                            {
                                connectionData.RemoveMapping(selectedFile.FriendlyFilePath);

                                listNotFoundedInCrmNoLink.Add(selectedFile);
                            }
                        }
                        else
                        {
                            listNotFoundedInCrmNoLink.Add(selectedFile);
                        }
                    }
                }
            }

            if (isconnectionDataDirty)
            {
                //Сохранение настроек после публикации
                connectionData.Save();
            }

            var tabSpacer = "    ";

            if (listNotFoundedInCrmNoLink.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM: {0}", listNotFoundedInCrmNoLink.Count);

                listNotFoundedInCrmNoLink.Sort();

                listNotFoundedInCrmNoLink.ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listNotFoundedInCrmWithLink.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link: {0}", listNotFoundedInCrmWithLink.Count);

                FormatTextTableHandler tableLastLinkDifferent = new FormatTextTableHandler();
                tableLastLinkDifferent.SetHeader("FriendlyFilePath", "WebResourceName");

                listNotFoundedInCrmWithLink.ForEach(i => tableLastLinkDifferent.AddLine(i.Item1.FriendlyFilePath, i.Item2.Name));

                tableLastLinkDifferent.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listNotExistsOnDisk.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, listNotExistsOnDisk.Count);

                listNotExistsOnDisk.Sort();

                listNotExistsOnDisk.ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listNotFoundedInCrmNoLink.Count + listNotFoundedInCrmWithLink.Count == 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "No files not exists in Crm");
            }

            return Tuple.Create(service, listNotFoundedInCrmNoLink, listNotFoundedInCrmWithLink);
        }

        protected async Task<Tuple<IOrganizationServiceExtented
                , List<Tuple<SelectedFile, WebResource>>
                , List<Tuple<SelectedFile, WebResource, ContentCopareResult>>
                >> ComparingFilesAndWebResourcesAsync(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles, bool withDetails)
        {
            var service = await QuickConnection.ConnectAndWriteToOutputAsync(this._iWriteToOutput, connectionData);

            if (service == null)
            {
                return null;
            }

            bool isconnectionDataDirty = false;

            List<string> listNotExistsOnDisk = new List<string>();

            List<string> listNotFoundedInCRMNoLink = new List<string>();

            var dictFilesEqualByTextNotContent = new List<Tuple<SelectedFile, WebResource>>();
            var dictFilesNotEqualByText = new List<Tuple<SelectedFile, WebResource, ContentCopareResult>>();

            int countEqualByContent = 0;

            FormatTextTableHandler tableEqualByText = new FormatTextTableHandler();
            tableEqualByText.SetHeader("FriendlyFilePath", "WebResourceName");

            FormatTextTableHandler tableDifferent = new FormatTextTableHandler();

            if (withDetails)
            {
                tableDifferent.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");
            }
            else
            {
                tableDifferent.SetHeader("FriendlyFilePath", "WebResourceName");
            }

            FormatTextTableHandler tableDifferentOnlyInserts = new FormatTextTableHandler();
            tableDifferentOnlyInserts.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)");

            FormatTextTableHandler tableDifferentOnlyDeletes = new FormatTextTableHandler();
            tableDifferentOnlyDeletes.SetHeader("FriendlyFilePath", "WebResourceName", "-Deletes", "(-Length)");

            FormatTextTableHandler tableDifferentComplexChanges = new FormatTextTableHandler();
            tableDifferentComplexChanges.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableDifferentMirror = new FormatTextTableHandler();
            tableDifferentMirror.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableDifferentMirrorWithInserts = new FormatTextTableHandler();
            tableDifferentMirrorWithInserts.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableDifferentMirrorWithDeletes = new FormatTextTableHandler();
            tableDifferentMirrorWithDeletes.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableLastLinkEqualByContent = new FormatTextTableHandler();
            tableLastLinkEqualByContent.SetHeader("FriendlyFilePath", "WebResourceName");

            FormatTextTableHandler listLastLinkEqualByText = new FormatTextTableHandler();
            listLastLinkEqualByText.SetHeader("FriendlyFilePath", "WebResourceName");

            FormatTextTableHandler tableLastLinkDifferent = new FormatTextTableHandler();
            if (withDetails)
            {
                tableLastLinkDifferent.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");
            }
            else
            {
                tableLastLinkDifferent.SetHeader("FriendlyFilePath", "WebResourceName");
            }

            FormatTextTableHandler tableLastLinkDifferentOnlyInserts = new FormatTextTableHandler();
            tableLastLinkDifferentOnlyInserts.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)");

            FormatTextTableHandler tableLastLinkDifferentOnlyDeletes = new FormatTextTableHandler();
            tableLastLinkDifferentOnlyDeletes.SetHeader("FriendlyFilePath", "WebResourceName", "-Deletes", "(-Length)");

            FormatTextTableHandler tableLastLinkDifferentComplexChanges = new FormatTextTableHandler();
            tableLastLinkDifferentComplexChanges.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableLastLinkDifferentMirror = new FormatTextTableHandler();
            tableLastLinkDifferentMirror.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableLastLinkDifferentMirrorWithInserts = new FormatTextTableHandler();
            tableLastLinkDifferentMirrorWithInserts.SetHeader("FriendlyFilePath", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            FormatTextTableHandler tableLastLinkDifferentMirrorWithDeletes = new FormatTextTableHandler();
            tableLastLinkDifferentMirrorWithDeletes.SetHeader("FriendlyFilePath", "WebResourceName", "+Inserts", "(+Length)", "-Deletes", "(-Length)");

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            var groups = selectedFiles.GroupBy(sel => sel.Extension);

            foreach (var gr in groups)
            {
                var names = gr.Select(sel => sel.FriendlyFilePath).ToArray();

                var dict = webResourceRepository.FindMultiple(gr.Key, names
                    , new ColumnSet(
                        WebResource.Schema.EntityPrimaryIdAttribute
                        , WebResource.Schema.Attributes.name
                        , WebResource.Schema.Attributes.webresourcetype
                        , WebResource.Schema.Attributes.content
                    ));

                foreach (var selectedFile in gr)
                {
                    if (!File.Exists(selectedFile.FilePath))
                    {
                        listNotExistsOnDisk.Add(selectedFile.FilePath);
                        continue;
                    }

                    string urlShowDifference = string.Format("{0}:///{1}?ConnectionId={2}", UrlCommandFilter.PrefixShowDifference, selectedFile.FilePath.Replace('\\', '/'), connectionData.ConnectionId.ToString());

                    string name = selectedFile.FriendlyFilePath.ToLower();

                    var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, name, gr.Key);

                    if (webresource != null)
                    {
                        // Запоминается файл
                        isconnectionDataDirty = true;
                        connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                        var contentWebResource = webresource.Content ?? string.Empty;

                        var arrayFile = File.ReadAllBytes(selectedFile.FilePath);

                        var contentFile = Convert.ToBase64String(arrayFile);

                        if (string.Equals(contentFile, contentWebResource))
                        {
                            countEqualByContent++;
                        }
                        else
                        {
                            var arrayWebResource = Convert.FromBase64String(contentWebResource);

                            var nameWebResource = webresource.Name;

                            var compare = ContentComparerHelper.CompareByteArrays(selectedFile.Extension, arrayFile, arrayWebResource, withDetails);

                            if (compare.IsEqual)
                            {
                                tableEqualByText.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource);

                                dictFilesEqualByTextNotContent.Add(Tuple.Create(selectedFile, webresource));
                            }
                            else
                            {
                                dictFilesNotEqualByText.Add(Tuple.Create(selectedFile, webresource, compare));

                                if (withDetails)
                                {
                                    string[] values = new string[]
                                    {
                                            selectedFile.UrlFriendlyFilePath, nameWebResource
                                                , string.Format("+{0}", compare.Inserts)
                                                , string.Format("(+{0})", compare.InsertLength)
                                                , string.Format("-{0}", compare.Deletes)
                                                , string.Format("(-{0})", compare.DeleteLength)
                                                , urlShowDifference
                                    };

                                    tableDifferent.AddLine(values);

                                    if (compare.IsOnlyInserts)
                                    {
                                        tableDifferentOnlyInserts.AddLine(selectedFile.UrlFriendlyFilePath
                                            , string.Format("+{0}", compare.Inserts)
                                            , string.Format("(+{0})", compare.InsertLength)
                                            , urlShowDifference
                                        );
                                    }

                                    if (compare.IsOnlyDeletes)
                                    {
                                        tableDifferentOnlyDeletes.AddLine(selectedFile.UrlFriendlyFilePath
                                            , string.Format("-{0}", compare.Deletes)
                                            , string.Format("(-{0})", compare.DeleteLength)
                                            , urlShowDifference
                                        );
                                    }

                                    if (compare.IsComplexChanges)
                                    {
                                        tableDifferentComplexChanges.AddLine(values);
                                    }

                                    if (compare.IsMirror)
                                    {
                                        tableDifferentMirror.AddLine(values);
                                    }

                                    if (compare.IsMirrorWithInserts)
                                    {
                                        tableDifferentMirrorWithInserts.AddLine(values);
                                    }

                                    if (compare.IsMirrorWithDeletes)
                                    {
                                        tableDifferentMirrorWithDeletes.AddLine(values);
                                    }
                                }
                                else
                                {
                                    tableDifferent.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource, urlShowDifference);
                                }
                            }
                        }
                    }
                    else
                    {
                        Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                        if (webId.HasValue)
                        {
                            webresource = await webResourceRepository.GetByIdAsync(webId.Value);

                            if (webresource != null)
                            {
                                // Запоминается файл
                                isconnectionDataDirty = true;
                                connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                                var contentWebResource = webresource.Content ?? string.Empty;
                                var nameWebResource = webresource.Name;

                                var arrayFile = File.ReadAllBytes(selectedFile.FilePath);

                                var contentFile = Convert.ToBase64String(arrayFile);

                                if (string.Equals(contentFile, contentWebResource))
                                {
                                    tableLastLinkEqualByContent.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource);
                                }
                                else
                                {
                                    var arrayWebResource = Convert.FromBase64String(contentWebResource);

                                    var compare = ContentComparerHelper.CompareByteArrays(selectedFile.Extension, arrayFile, arrayWebResource);

                                    if (compare.IsEqual)
                                    {
                                        listLastLinkEqualByText.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource);

                                        dictFilesEqualByTextNotContent.Add(Tuple.Create(selectedFile, webresource));
                                    }
                                    else
                                    {
                                        dictFilesNotEqualByText.Add(Tuple.Create(selectedFile, webresource, compare));

                                        if (withDetails)
                                        {
                                            string[] values = new string[]
                                            {
                                                    selectedFile.UrlFriendlyFilePath, nameWebResource
                                                        , string.Format("+{0}", compare.Inserts)
                                                        , string.Format("(+{0})", compare.InsertLength)
                                                        , string.Format("-{0}", compare.Deletes)
                                                        , string.Format("(-{0})", compare.DeleteLength)
                                                        , urlShowDifference
                                            };

                                            tableLastLinkDifferent.AddLine(values);


                                            if (compare.IsOnlyInserts)
                                            {
                                                tableLastLinkDifferentOnlyInserts.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource
                                                    , string.Format("+{0}", compare.Inserts)
                                                    , string.Format("(+{0})", compare.InsertLength)
                                                    , urlShowDifference
                                                    );
                                            }

                                            if (compare.IsOnlyDeletes)
                                            {
                                                tableLastLinkDifferentOnlyDeletes.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource
                                                    , string.Format("-{0}", compare.Deletes)
                                                    , string.Format("(-{0})", compare.DeleteLength)
                                                    , urlShowDifference
                                                    );
                                            }

                                            if (compare.IsComplexChanges)
                                            {
                                                tableLastLinkDifferentComplexChanges.AddLine(values);
                                            }

                                            if (compare.IsMirror)
                                            {
                                                tableLastLinkDifferentMirror.AddLine(values);
                                            }

                                            if (compare.IsMirrorWithInserts)
                                            {
                                                tableLastLinkDifferentMirrorWithInserts.AddLine(values);
                                            }

                                            if (compare.IsMirrorWithDeletes)
                                            {
                                                tableLastLinkDifferentMirrorWithDeletes.AddLine(values);
                                            }
                                        }
                                        else
                                        {
                                            tableLastLinkDifferent.AddLine(selectedFile.UrlFriendlyFilePath, nameWebResource, urlShowDifference);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                connectionData.RemoveMapping(selectedFile.FriendlyFilePath);

                                listNotFoundedInCRMNoLink.Add(selectedFile.UrlFriendlyFilePath);
                            }
                        }
                        else
                        {
                            listNotFoundedInCRMNoLink.Add(selectedFile.UrlFriendlyFilePath);
                        }
                    }
                }
            }

            if (isconnectionDataDirty)
            {
                //Сохранение настроек после публикации
                connectionData.Save();
            }

            var tabSpacer = "    ";

            if (tableDifferent.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content: {0}", tableDifferent.Count);

                tableDifferent.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentOnlyInserts.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH ONLY INSERTS: {0}", tableDifferentOnlyInserts.Count);

                tableDifferentOnlyInserts.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentOnlyDeletes.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH ONLY DELETES: {0}", tableDifferentOnlyDeletes.Count);

                tableDifferentOnlyDeletes.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentComplexChanges.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH COMPLEX CHANGES: {0}", tableDifferentComplexChanges.Count);

                tableDifferentComplexChanges.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentMirror.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH MIRROR CHANGES: {0}", tableDifferentMirror.Count);

                tableDifferentMirror.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentMirrorWithInserts.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH MIRROR CHANGES AND INSERTS: {0}", tableDifferentMirrorWithInserts.Count);

                tableDifferentMirrorWithInserts.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableDifferentMirrorWithDeletes.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File and web-resource are DIFFERENT by content WITH MIRROR CHANGES AND DELETES: {0}", tableDifferentMirrorWithDeletes.Count);

                tableDifferentMirrorWithDeletes.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listNotFoundedInCRMNoLink.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM: {0}", listNotFoundedInCRMNoLink.Count);

                listNotFoundedInCRMNoLink.Sort();

                listNotFoundedInCRMNoLink.ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferent.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT: {0}", tableLastLinkDifferent.Count);

                tableLastLinkDifferent.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }


            if (tableLastLinkDifferentOnlyInserts.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH ONLY INSERTS: {0}", tableLastLinkDifferentOnlyInserts.Count);

                tableLastLinkDifferentOnlyInserts.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferentOnlyDeletes.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH ONLY DELETES: {0}", tableLastLinkDifferentOnlyDeletes.Count);

                tableLastLinkDifferentOnlyDeletes.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferentComplexChanges.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH COMPLEX CHANGES: {0}", tableLastLinkDifferentComplexChanges.Count);

                tableLastLinkDifferentComplexChanges.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferentMirror.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH MIRROR CHANGES: {0}", tableLastLinkDifferentMirror.Count);

                tableLastLinkDifferentMirror.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferentMirrorWithInserts.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH MIRROR CHANGES AND INSERTS: {0}", tableLastLinkDifferentMirrorWithInserts.Count);

                tableLastLinkDifferentMirrorWithInserts.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkDifferentMirrorWithDeletes.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files DIFFERENT WITH MIRROR CHANGES AND DELETES: {0}", tableLastLinkDifferentMirrorWithDeletes.Count);

                tableLastLinkDifferentMirrorWithDeletes.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listLastLinkEqualByText.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files EQUALS BY TEXT: {0}", listLastLinkEqualByText.Count);

                listLastLinkEqualByText.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableLastLinkEqualByContent.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File NOT FOUNDED in CRM, but has Last Link, files EQUALS BY CONTENT: {0}", tableLastLinkEqualByContent.Count);

                tableLastLinkEqualByContent.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (listNotExistsOnDisk.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, listNotExistsOnDisk.Count);

                listNotExistsOnDisk.Sort();

                listNotExistsOnDisk.ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (tableEqualByText.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(connectionData, "File and web-resource EQUALS BY TEXT: {0}", tableEqualByText.Count);

                tableEqualByText.GetFormatedLines(true).ForEach(item => this._iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));
            }

            if (countEqualByContent > 0)
            {
                if (countEqualByContent == selectedFiles.Count())
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, "All files and web-resources EQUALS BY CONTENT: {0}", countEqualByContent);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, "File and web-resource EQUALS BY CONTENT: {0}", countEqualByContent);
                }
            }

            return Tuple.Create(service, dictFilesEqualByTextNotContent, dictFilesNotEqualByText);
        }

        protected async void OpenWindowForUnknownPluginTypes(ConnectionData connectionData, CommonConfiguration commonConfig, List<string> unknownPluginTypes)
        {
            if (!unknownPluginTypes.Any())
            {
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.PluginTypesNotFoundedByNameFormat1, unknownPluginTypes.Count);

            foreach (var pluginTypeName in unknownPluginTypes)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "{0}{1}", _tabSpacer, pluginTypeName);
            }

            var service = await QuickConnection.ConnectAsync(connectionData);

            WindowHelper.OpenPluginTypeExplorer(
                this._iWriteToOutput
                , service
                , commonConfig
                , unknownPluginTypes.FirstOrDefault()
            );
        }
    }
}
