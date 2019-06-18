using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class CheckController
    {
        private const string tabSpacer = "    ";

        private readonly IWriteToOutput _iWriteToOutput = null;

        public CheckController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Проверка уровня собственности сущностей.

        public async Task ExecuteCheckingEntitiesOwnership(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingCRMEntityOwnershipFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckingEntitiesOwnership(connectionData, commonConfig);
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

        private async Task CheckingEntitiesOwnership(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

            var allEntities = await repositoryEntity.GetEntitiesDisplayNameAsync();

            var groups = allEntities.GroupBy(ent => ent.OwnershipType).OrderBy(gr => gr.Key);

            foreach (var gr in groups)
            {
                content.AppendLine();

                int count = gr.Count();

                string name = "Null";

                if (gr.Key.HasValue)
                {
                    name = gr.Key.Value.ToString();
                }

                content.AppendFormat("Entities with Ownership {0}: {1}", name, count).AppendLine();

                gr.OrderBy(ent => ent.LogicalName).ToList().ForEach(ent => content.AppendLine(tabSpacer + ent.LogicalName));
            }

            string fileName = string.Format("{0}.Entities with Ownership at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(connectionData, "Created file with Entities with Ownership: {0}", filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #endregion Проверка уровня собственности сущностей.

        #region Проверка кодировки файлов.

        internal void ExecuteCheckingFilesEncoding(List<SelectedFile> selectedFiles)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.CheckingFilesEncoding);

            try
            {
                CheckingFilesEncoding(this._iWriteToOutput, null, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.CheckingFilesEncoding);
            }
        }

        public static void CheckingFilesEncoding(IWriteToOutput iWriteToOutput, ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding)
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

                    var encodings = ContentCoparerHelper.GetFileEncoding(arrayFile);

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
                                listWrongEncoding.Add(string.Format("{0} has encoding {1}", selectedFile.FriendlyFilePath, encodings[0].EncodingName));

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
                                listMultipleEncodingHasUTF8.Add(string.Format("{0} has encoding {1}", selectedFile.FriendlyFilePath, str.ToString()));
                            }
                            else
                            {
                                listMultipleEncodingHasNotUTF8.Add(string.Format("{0} has encoding {1}", selectedFile.FriendlyFilePath, str.ToString()));
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
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File does not have encoding: {0}", listNotHaveBOM.Count);

                listNotHaveBOM.Sort();

                listNotHaveBOM.ForEach(item => iWriteToOutput.WriteToOutput(connectionData, item));
            }

            if (listWrongEncoding.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File has wrong Encoding: {0}", listWrongEncoding.Count);

                listWrongEncoding.Sort();

                listWrongEncoding.ForEach(item => iWriteToOutput.WriteToOutput(connectionData, item));
            }

            if (listMultipleEncodingHasUTF8.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File complies multiple Encoding with UTF8 in list: {0}", listMultipleEncodingHasUTF8.Count);

                listMultipleEncodingHasUTF8.Sort();

                listMultipleEncodingHasUTF8.ForEach(item => iWriteToOutput.WriteToOutput(connectionData, item));
            }

            if (listMultipleEncodingHasNotUTF8.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, "File complies multiple Encoding WITHOUT UTF8 in list: {0}", listMultipleEncodingHasNotUTF8.Count);

                listMultipleEncodingHasNotUTF8.Sort();

                listMultipleEncodingHasNotUTF8.ForEach(item => iWriteToOutput.WriteToOutput(connectionData, item));
            }

            if (listNotExistsOnDisk.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, listNotExistsOnDisk.Count);

                listNotExistsOnDisk.Sort();

                listNotExistsOnDisk.ForEach(item => iWriteToOutput.WriteToOutput(connectionData, item));
            }

            if (countWithUTF8Encoding > 0)
            {
                if (countWithUTF8Encoding == selectedFiles.Count())
                {
                    iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    iWriteToOutput.WriteToOutput(connectionData, "All Files has UTF8 Encoding: {0}", countWithUTF8Encoding);
                }
                else
                {
                    iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    iWriteToOutput.WriteToOutput(connectionData, "Files has UTF8 Encoding: {0}", countWithUTF8Encoding);
                }
            }
        }

        public void ExecuteOpenFilesWithoutUTF8Encoding(IEnumerable<SelectedFile> selectedFiles)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.OpeningFilesWithoutUTF8Encoding);

            try
            {
                CheckingFilesEncoding(this._iWriteToOutput, null, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                if (filesWithoutUTF8Encoding.Count > 0)
                {
                    foreach (var item in filesWithoutUTF8Encoding)
                    {
                        this._iWriteToOutput.WriteToOutputFilePathUri(null, item.FilePath);
                        this._iWriteToOutput.OpenFileInVisualStudio(null, item.FilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.OpeningFilesWithoutUTF8Encoding);
            }
        }

        #endregion Проверка кодировки файлов.

        #region Отображение зависимых компонентов веб-ресурсов.

        public async Task ExecuteShowingWebResourcesDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, Properties.OperationNames.CheckingCRMObjectsNamesAndShowDependentComponents);

            try
            {
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OperationNames.CheckingFilesEncoding);

                    CheckController.CheckingFilesEncoding(this._iWriteToOutput, connectionData, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                }

                await ShowingWebResourcesDependentComponents(connectionData, commonConfig, selectedFiles);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, Properties.OperationNames.CheckingCRMObjectsNamesAndShowDependentComponents);
            }
        }

        private async Task ShowingWebResourcesDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.WithUrls = true;
            descriptor.WithManagedInfo = true;
            descriptor.WithSolutionsInfo = true;

            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            var dependencyRepository = new DependencyRepository(service);

            bool isconnectionDataDirty = false;

            List<string> listNotExistsOnDisk = new List<string>();
            List<string> listNotFoundedInCRMNoLink = new List<string>();
            List<string> listLastLinkEqualByContent = new List<string>();

            List<SolutionComponent> webResourceNames = new List<SolutionComponent>();

            Dictionary<SolutionComponent, string> webResourceDescriptions = new Dictionary<SolutionComponent, string>();

            WebResourceRepository repositoryWebResource = new WebResourceRepository(service);

            FormatTextTableHandler tableWithoutDependenComponents = new FormatTextTableHandler();
            tableWithoutDependenComponents.SetHeader("FilePath", "Web Resource Name", "Web Resource Type");

            var groups = selectedFiles.GroupBy(sel => sel.Extension);

            foreach (var gr in groups)
            {
                var names = gr.Select(sel => sel.FriendlyFilePath).ToArray();

                var dict = repositoryWebResource.FindMultiple(gr.Key, names);

                foreach (var selectedFile in gr)
                {
                    if (File.Exists(selectedFile.FilePath))
                    {
                        string name = selectedFile.FriendlyFilePath.ToLower();

                        var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, name, gr.Key);

                        if (webresource == null)
                        {
                            Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                            if (webId.HasValue)
                            {
                                webresource = await repositoryWebResource.GetByIdAsync(webId.Value);

                                if (webresource != null)
                                {
                                    listLastLinkEqualByContent.Add(selectedFile.FriendlyFilePath);
                                }
                            }
                        }

                        if (webresource != null)
                        {
                            // Запоминается файл
                            isconnectionDataDirty = true;
                            connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                            var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.WebResource, webresource.Id);

                            var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                            if (!string.IsNullOrEmpty(desc))
                            {
                                var component = new SolutionComponent()
                                {
                                    ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                                    ObjectId = webresource.Id,
                                };

                                webResourceNames.Add(component);

                                webResourceDescriptions.Add(component, desc);
                            }
                            else
                            {
                                tableWithoutDependenComponents.AddLine(selectedFile.FriendlyFilePath, webresource.Name, "'" + webresource.FormattedValues[WebResource.Schema.Attributes.webresourcetype] + "'");
                            }
                        }
                        else
                        {
                            connectionData.RemoveMapping(selectedFile.FriendlyFilePath);

                            listNotFoundedInCRMNoLink.Add(selectedFile.FriendlyFilePath);
                        }
                    }
                    else
                    {
                        listNotExistsOnDisk.Add(selectedFile.FilePath);
                    }
                }
            }

            if (isconnectionDataDirty)
            {
                //Сохранение настроек после публикации
                connectionData.Save();
            }

            FindsController.WriteToContentList(listNotFoundedInCRMNoLink, content, "File NOT FOUNDED in CRM: {0}");

            FindsController.WriteToContentList(listLastLinkEqualByContent, content, "Files NOT FOUNDED in CRM, but has Last Link: {0}");

            FindsController.WriteToContentList(listNotExistsOnDisk, content, Properties.OutputStrings.FileNotExistsFormat1);

            FindsController.WriteToContentList(tableWithoutDependenComponents.GetFormatedLines(true), content, "Files without dependent components: {0}");

            FindsController.WriteToContentDictionary(descriptor, content, webResourceNames, webResourceDescriptions, "WebResource dependent components: {0}");

            string fileName = string.Format("{0}.WebResourceDependent at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(connectionData, "Created file with web-resources dependent components: {0}", filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #endregion Отображение зависимых компонентов веб-ресурсов.

        #region Проверка глобальных OptionSet на дубликаты на сущности.

        public async Task ExecuteCheckingGlobalOptionSetDuplicates(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingGlobalOptionSetDuplicatesOnEntityFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckingGlobalOptionSetDuplicates(connectionData, commonConfig);
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

        private async Task CheckingGlobalOptionSetDuplicates(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            var entityMetadataSource = new SolutionComponentMetadataSource(service);

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.WithUrls = true;
            descriptor.WithManagedInfo = true;
            descriptor.WithSolutionsInfo = true;

            var dependencyRepository = new DependencyRepository(service);
            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            RetrieveAllOptionSetsRequest request = new RetrieveAllOptionSetsRequest();

            RetrieveAllOptionSetsResponse response = (RetrieveAllOptionSetsResponse)service.Execute(request);

            bool hasInfo = false;

            foreach (var optionSet in response.OptionSetMetadata.OfType<OptionSetMetadata>().OrderBy(e => e.Name))
            {
                var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet.MetadataId.Value);

                if (coll.Any())
                {
                    var filter = coll
                        .Where(c => c.DependentComponentType.Value == (int)ComponentType.Attribute)
                        .Select(c => new { Dependency = c, Attribute = entityMetadataSource.GetAttributeMetadata(c.DependentComponentObjectId.Value) })
                        .Where(c => c.Attribute != null)
                        .GroupBy(c => c.Attribute.EntityLogicalName)
                        .Where(gr => gr.Count() > 1)
                        .SelectMany(gr => gr.Select(c => c.Dependency))
                        .ToList()
                        ;

                    if (filter.Any())
                    {
                        var desc = await descriptorHandler.GetDescriptionDependentAsync(filter);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            if (content.Length > 0)
                            {
                                content
                                    .AppendLine(new string('-', 150))
                                    .AppendLine();
                            }

                            hasInfo = true;

                            content.AppendFormat("Global OptionSet Name {0}       IsCustomOptionSet {1}      IsManaged {2}", optionSet.Name, optionSet.IsCustomOptionSet, optionSet.IsManaged).AppendLine();

                            content.AppendLine(desc);
                        }
                    }
                }
            }

            if (!hasInfo)
            {
                content.AppendLine("No duplicates were found.");
            }

            string fileName = string.Format("{0}.Checking Global OptionSet Duplicates on Entity at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(connectionData, "Created file with Checking Global OptionSet Duplicates on Entity: {0}", filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #endregion Проверка глобальных OptionSet на дубликаты на сущности.

        #region Редактирование элементов по идентификатору.

        public async Task ExecuteEditEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            string operation = string.Format(Properties.OperationNames.EditingCRMObjectsByIdEntityNameEntityTypeCodeFormat4, connectionData?.Name, entityId, entityName, entityTypeCode);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await EditEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
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

        private async Task EditEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
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
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            EntityMetadataRepository repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.GetEntitiesPropertiesAsync(entityName, entityTypeCode, "LogicalName", "PrimaryIdAttribute", "IsIntersect", "Attributes");

            List<EntityReference> listEntities = new List<EntityReference>();

            foreach (var item in entityMetadataList.OrderBy(e => e.LogicalName))
            {
                var primaryAttr = item.Attributes.FirstOrDefault(a => string.Equals(a.LogicalName, item.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase));

                if (primaryAttr != null && primaryAttr.AttributeType == AttributeTypeCode.Uniqueidentifier)
                {
                    var generalRepository = new GenericRepository(service, item);

                    Entity entity = await generalRepository.GetEntityByIdAsync(entityId, new ColumnSet(false));

                    if (entity != null)
                    {
                        listEntities.Add(entity.ToEntityReference());
                    }
                }
            }

            if (!listEntities.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoObjectsInCRMWereFounded);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ObjectsFoundedInCRMFormat1, listEntities.Count);

            foreach (var item in listEntities)
            {
                WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, item.LogicalName, item.Id);
            }
        }

        #endregion Редактирование элементов по идентификатору.

        #region Поиск элементов по любому Guid.

        public async Task ExecuteFindEntityByUniqueidentifier(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsByUniqueidentifierEntityNameEntityTypeCodeFormat4, connectionData?.Name, entityId, entityName, entityTypeCode);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindEntityByUniqueidentifier(connectionData, commonConfig, entityName, entityTypeCode, entityId);
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

        private async Task FindEntityByUniqueidentifier(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            EntityMetadataRepository repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.GetEntitiesPropertiesAsync(entityName, entityTypeCode, "LogicalName", "PrimaryIdAttribute", "IsIntersect", "Attributes");

            bool finded = false;

            foreach (var item in entityMetadataList.OrderBy(e => e.LogicalName))
            {
                foreach (var field in item.Attributes.Where(a => a.AttributeType == AttributeTypeCode.Uniqueidentifier).OrderBy(a => a.LogicalName))
                {
                    var generalRepository = new GenericRepository(service, item);

                    var entityList = await generalRepository.GetEntitiesByFieldAsync(field.LogicalName, entityId, new ColumnSet(true));

                    if (entityList != null)
                    {
                        foreach (var entity in entityList)
                        {
                            finded = true;

                            content
                                .AppendLine()
                                .AppendLine()
                                .AppendLine(new string('-', 150))
                                .AppendLine()
                                .AppendLine()
                                .AppendLine(await EntityDescriptionHandler.GetEntityDescriptionAsync(entity, null, connectionData))
                                ;
                        }
                    }
                }
            }

            if (finded)
            {
                string fileName = EntityFileNameFormatter.GetFindingCRMObjectsByUniqueidentifierFileName(connectionData.Name, entityId);

                if (string.IsNullOrEmpty(commonConfig.FolderForExport))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                    commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }
                else if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                    commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoObjectsInCRMWereFounded);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }
        }

        #endregion Поиск элементов по любому Guid.

        #region Поиск неизвестных типов компонентов.

        public async Task ExecuteCheckingComponentTypeEnum(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingComponentTypeEnumFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckingComponentTypeEnum(connectionData, commonConfig);
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

        private async Task CheckingComponentTypeEnum(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            var hash = new HashSet<Tuple<int, Guid>>();

            {
                var repository = new SolutionComponentRepository(service);

                var components = await repository.GetDistinctSolutionComponentsAsync();

                foreach (var item in components.Where(en => en.ComponentType != null && en.ObjectId.HasValue))
                {
                    if (!item.IsDefinedComponentType())
                    {
                        hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value));
                    }
                }
            }

            {
                var repository = new DependencyNodeRepository(service);

                var components = await repository.GetDistinctListUnknownComponentTypeAsync();

                foreach (var item in components.Where(en => en.ComponentType != null && en.ObjectId.HasValue))
                {
                    if (!item.IsDefinedComponentType())
                    {
                        hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value));
                    }
                }
            }

            {
                var repository = new InvalidDependencyRepository(service);

                var components = await repository.GetDistinctListAsync();

                foreach (var item in components.Where(en => en.MissingComponentType != null && en.MissingComponentId.HasValue))
                {
                    if (!item.IsDefinedMissingComponentType())
                    {
                        hash.Add(Tuple.Create(item.MissingComponentType.Value, item.MissingComponentId.Value));
                    }
                }

                foreach (var item in components.Where(en => en.ExistingComponentType != null && en.ExistingComponentId.HasValue))
                {
                    if (!item.IsDefinedExistingComponentType())
                    {
                        hash.Add(Tuple.Create(item.ExistingComponentType.Value, item.ExistingComponentId.Value));
                    }
                }
            }

            if (hash.Any())
            {
                var groups = hash.GroupBy(e => e.Item1);

                content.AppendLine().AppendLine();

                content.AppendFormat("ComponentTypes not founded in Enum: {0}", groups.Count());

                foreach (var gr in groups.OrderBy(e => e.Key))
                {
                    content.AppendLine().AppendLine();

                    foreach (var item in gr.OrderBy(e => e.Item2))
                    {
                        content.AppendFormat(tabSpacer + item.Item1.ToString() + tabSpacer + item.Item2.ToString()).AppendLine();
                    }
                }

                string fileName = string.Format("{0}.Checking ComponentType Enum at {1}.txt"
                    , connectionData.Name
                    , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

                if (string.IsNullOrEmpty(commonConfig.FolderForExport))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                    commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }
                else if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                    commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, "New ComponentTypes were exported to {0}", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "No New ComponentTypes in CRM were founded.");
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }
        }

        #endregion Поиск неизвестных типов компонентов.

        #region Описание всех dependencynode.

        public async Task ExecuteCreatingAllDependencyNodesDescription(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CreatingAllDependencyNodesDescriptionFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CreatingAllDependencyNodesDescription(connectionData, commonConfig);
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

        private async Task CreatingAllDependencyNodesDescription(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            var hash = new HashSet<Tuple<int, Guid>>();

            {
                var repository = new DependencyNodeRepository(service);

                var components = await repository.GetDistinctListAsync();

                foreach (var item in components.Where(en => en.ComponentType != null && en.ObjectId.HasValue))
                {
                    hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value));
                }
            }

            if (hash.Any())
            {
                content.AppendLine().AppendLine();

                var solutionComponents = hash.Select(e => new SolutionComponent
                {
                    ComponentType = new OptionSetValue(e.Item1),
                    ObjectId = e.Item2,
                }).ToList();

                var descriptor = new SolutionComponentDescriptor(service);
                descriptor.WithUrls = true;
                descriptor.WithManagedInfo = true;
                descriptor.WithSolutionsInfo = true;

                descriptor.MetadataSource.DownloadEntityMetadata();

                var desc = await descriptor.GetSolutionComponentsDescriptionAsync(solutionComponents);

                content.AppendLine(desc);

                string fileName = string.Format(
                    "{0}.Dependency Nodes Description at {1}.txt"
                    , connectionData.Name
                    , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

                if (string.IsNullOrEmpty(commonConfig.FolderForExport))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                    commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }
                else if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                    commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, "Dependency Nodes Description were exported to {0}", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
        }

        #endregion Описание всех dependencynode.

        #region Поиск используемых в БП сущностей

        public async Task ExecuteCheckingWorkflowsUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingWorkflowsUsedEntitiesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckingWorkflowsUsedEntities(connectionData, commonConfig);
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

        private async Task CheckingWorkflowsUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            string fileName = string.Format("{0}.Workflows Used Entities at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.WithUrls = true;
            descriptor.WithManagedInfo = true;
            descriptor.WithSolutionsInfo = true;

            var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, descriptor);

            var stringBuider = new StringBuilder();

            await workflowDescriptor.GetDescriptionWithUsedEntitiesInAllWorkflowsAsync(stringBuider);

            File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(connectionData, "Created file with Workflows Used Entities: {0}", filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #endregion Поиск используемых в БП сущностей

        #region Поиск несуществующих используемых в БП сущностей

        public async Task ExecuteCheckingWorkflowsNotExistingUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingWorkflowsUsedNotExistingEntitiesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckingWorkflowsNotExistingUsedEntities(connectionData, commonConfig);
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

        private async Task CheckingWorkflowsNotExistingUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            string fileName = string.Format("{0}.Workflows Used Not Existing Entities at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.WithUrls = true;
            descriptor.WithManagedInfo = true;
            descriptor.WithSolutionsInfo = true;

            var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, descriptor);

            var stringBuider = new StringBuilder();

            await workflowDescriptor.GetDescriptionWithUsedNotExistsEntitiesInAllWorkflowsAsync(stringBuider);

            File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(connectionData, "Created file with Workflows Used Not Existing Entities: {0}", filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #endregion Поиск несуществующих используемых в БП сущностей
    }
}