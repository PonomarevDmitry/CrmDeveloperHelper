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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class CheckController : BaseController<IWriteToOutput>
    {
        public CheckController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
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
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var content = new StringBuilder();

                content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
                content.AppendLine(connectionData.GetConnectionDescription());
                content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

                var repositoryEntity = new EntityMetadataRepository(service);

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

                    content.AppendFormat(Properties.OutputStrings.EntitiesWithOwnershipFormat2, name, count).AppendLine();

                    gr.OrderBy(ent => ent.LogicalName).ToList().ForEach(ent => content.AppendLine(_tabSpacer + ent.LogicalName));
                }

                commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                string fileName = string.Format("{0}.Entities with Ownership at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CreatedFileWithEntitiesOwnershipFormat1, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
        }

        #endregion Проверка уровня собственности сущностей.

        #region Проверка кодировки файлов.

        internal void ExecuteCheckingFilesEncoding(List<SelectedFile> selectedFiles)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.CheckingFilesEncoding);

            try
            {
                CheckingFilesEncoding(null, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);
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

        public void ExecuteOpenFilesWithoutUTF8Encoding(IEnumerable<SelectedFile> selectedFiles)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.OpeningFilesWithoutUTF8Encoding);

            try
            {
                CheckingFilesEncoding(null, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

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
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

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
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var content = new StringBuilder();

                content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
                content.AppendLine(connectionData.GetConnectionDescription());
                content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

                var descriptor = new SolutionComponentDescriptor(service);
                descriptor.WithUrls = true;
                descriptor.WithManagedInfo = true;
                descriptor.WithSolutionsInfo = true;

                var descriptorHandler = new DependencyDescriptionHandler(descriptor);

                var dependencyRepository = new DependencyRepository(service);

                bool isconnectionDataDirty = false;

                var listNotExistsOnDisk = new List<string>();
                var listNotFoundedInCRMNoLink = new List<string>();
                var listLastLinkEqualByContent = new List<string>();

                var webResourceNames = new List<SolutionComponent>();

                var webResourceDescriptions = new Dictionary<SolutionComponent, string>();

                var repositoryWebResource = new WebResourceRepository(service);

                var tableWithoutDependenComponents = new FormatTextTableHandler();
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

                commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                string fileName = string.Format("{0}.WebResourceDependent at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CreatedFileWithWebResourcesDependentComponentsFormat1, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
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
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var content = new StringBuilder();

                content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
                content.AppendLine(connectionData.GetConnectionDescription());
                content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

                var entityMetadataSource = new SolutionComponentMetadataSource(service);

                var descriptor = new SolutionComponentDescriptor(service);
                descriptor.WithUrls = true;
                descriptor.WithManagedInfo = true;
                descriptor.WithSolutionsInfo = true;

                var dependencyRepository = new DependencyRepository(service);
                var descriptorHandler = new DependencyDescriptionHandler(descriptor);

                var request = new RetrieveAllOptionSetsRequest();

                var response = await service.ExecuteAsync<RetrieveAllOptionSetsResponse>(request);

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

                commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                string fileName = string.Format("{0}.Checking Global OptionSet Duplicates on Entity at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CreatedFileWithCheckingGlobalOptionSetDuplicatesOnEntityFormat1, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
        }

        #endregion Проверка глобальных OptionSet на дубликаты на сущности.

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
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var content = new StringBuilder();

                content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
                content.AppendLine(connectionData.GetConnectionDescription());
                content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

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
                            content.AppendFormat(_tabSpacer + item.Item1.ToString() + _tabSpacer + item.Item2.ToString()).AppendLine();
                        }
                    }

                    string fileName = string.Format("{0}.Checking ComponentType Enum at {1}.txt"
                        , connectionData.Name
                        , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                    );

                    commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

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
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var content = new StringBuilder();

                content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
                content.AppendLine(connectionData.GetConnectionDescription());
                content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

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

                    commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                    string fileName = string.Format(
                        "{0}.Dependency Nodes Description at {1}.txt"
                        , connectionData.Name
                        , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                    );

                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, "Dependency Nodes Description were exported to {0}", filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
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
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var content = new StringBuilder();

                content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
                content.AppendLine(connectionData.GetConnectionDescription());
                content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

                commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                string fileName = string.Format("{0}.Workflows Used Entities at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

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
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var content = new StringBuilder();

                content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
                content.AppendLine(connectionData.GetConnectionDescription());
                content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

                commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                string fileName = string.Format("{0}.Workflows Used Not Existing Entities at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

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
        }

        #endregion Поиск несуществующих используемых в БП сущностей

        public async Task ExecuteCheckingUnknownFormControlType(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingUnknownFormControlTypeFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckingUnknownFormControlType(connectionData, commonConfig);
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

        private async Task CheckingUnknownFormControlType(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var content = new StringBuilder();

                content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
                content.AppendLine(connectionData.GetConnectionDescription());
                content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

                var dictUnknownControls = new Dictionary<string, FormatTextTableHandler>(StringComparer.InvariantCultureIgnoreCase);

                var descriptor = new SolutionComponentDescriptor(service);
                var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

                var repositorySystemForm = new SystemFormRepository(service);

                var formList = await repositorySystemForm.GetListAsync(null, null, null, new ColumnSet(true));

                foreach (var systemForm in formList
                    .OrderBy(f => f.ObjectTypeCode)
                    .ThenBy(f => f.Type?.Value)
                    .ThenBy(f => f.Name)
                )
                {
                    string formXml = systemForm.FormXml;

                    if (!string.IsNullOrEmpty(formXml))
                    {
                        XElement doc = XElement.Parse(formXml);

                        var tabs = handler.GetFormTabs(doc);

                        var unknownControls = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType() == FormControl.FormControlType.UnknownControl);

                        foreach (var control in unknownControls)
                        {
                            if (!dictUnknownControls.ContainsKey(control.ClassId))
                            {
                                var tableUnknownControls = new FormatTextTableHandler();
                                tableUnknownControls.SetHeader("Entity", "FormType", "Form", "State", "Attribute", "Form Url");

                                dictUnknownControls[control.ClassId] = tableUnknownControls;
                            }

                            dictUnknownControls[control.ClassId].AddLine(
                                systemForm.ObjectTypeCode
                                , systemForm.FormattedValues[SystemForm.Schema.Attributes.type]
                                , systemForm.Name
                                , systemForm.FormattedValues[SystemForm.Schema.Attributes.formactivationstate]
                                , control.Attribute
                                , service.UrlGenerator.GetSolutionComponentUrl(ComponentType.SystemForm, systemForm.Id)
                            );
                        }
                    }
                }

                if (dictUnknownControls.Count > 0)
                {
                    content.AppendLine().AppendLine();

                    content.AppendFormat("Unknown Form Control Types: {0}", dictUnknownControls.Count);

                    foreach (var classId in dictUnknownControls.Keys.OrderBy(s => s))
                    {
                        content.AppendLine().AppendLine();

                        content.AppendLine(classId);

                        var tableUnknownControls = dictUnknownControls[classId];

                        foreach (var str in tableUnknownControls.GetFormatedLines(false))
                        {
                            content.AppendLine(_tabSpacer + str);
                        }
                    }

                    commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                    string fileName = string.Format("{0}.Checking Unknown Form Control Types at {1}.txt"
                        , connectionData.Name
                        , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                    );

                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, "Unknown Form Control Types were exported to {0}", filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "No Unknown Form Control Types in CRM were founded.");
                    this._iWriteToOutput.ActivateOutputWindow(connectionData);
                }
            }
        }
    }
}