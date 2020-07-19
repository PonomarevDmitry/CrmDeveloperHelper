using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.XsdModels;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    /// <summary>
    /// Контроллер для публикации
    /// </summary>
    public class PublishController : BaseController<IWriteToOutput>
    {
        /// <summary>
        /// Конструктор контроллера для публикации
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public PublishController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        /// <summary>
        /// Элемент для публикации в CRM
        /// </summary>
        private class ElementForPublish
        {
            public SelectedFile SelectedFile { get; private set; }

            public WebResource WebResource { get; private set; }

            public string NewDependencyXml { get; internal set; }

            public string NewDependencies { get; internal set; }

            public int NewDependenciesCount { get; internal set; }

            public ElementForPublish(SelectedFile selectedFile, WebResource webResource)
            {
                this.WebResource = webResource ?? throw new ArgumentException("Не задан веб-ресурс.");
                this.SelectedFile = selectedFile ?? throw new ArgumentException("Не задан файл-источник.");
            }
        }

        /// <summary>
        /// Выполнение обновления содержания и публикация.
        /// </summary>
        private async Task UpdateContentAndPublishAsync(IOrganizationServiceExtented service, Dictionary<Guid, ElementForPublish> elements)
        {
            if (elements.Count == 0)
            {
                return;
            }

            var listToUpdate = GetEntitesToUpdateContent(service.ConnectionData, elements.Values);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Updating WebResources Content...");

            foreach (var entity in listToUpdate)
            {
                await service.UpdateAsync(entity);
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Publishing WebResources...");

            var repositoryPublish = new PublishActionsRepository(service);
            await repositoryPublish.PublishWebResourcesAsync(elements.Keys);

            WriteToConsolePublishedWebResources(service.ConnectionData, elements.Values.Select(e => e.WebResource));
        }

        private void WriteToConsolePublishedWebResources(ConnectionData connectionData, IEnumerable<WebResource> elements)
        {
            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("WebResourceName", "WebResourceType");

            foreach (var webResource in elements.OrderBy(e => e.Name))
            {
                webResource.FormattedValues.TryGetValue(WebResource.Schema.Attributes.webresourcetype, out var webresourcetype);
                table.AddLine(webResource.Name, webresourcetype);
            }

            this._iWriteToOutput.WriteToOutput(connectionData, "Published WebResources: {0}", table.Count);

            var lines = table.GetFormatedLines(false);
            lines.ForEach(item => _iWriteToOutput.WriteToOutput(connectionData, "{0}{1}", _tabSpacer, item));
        }

        /// <summary>
        /// Обновление контента веб-ресурсов
        /// </summary>
        /// <returns></returns>
        private List<WebResource> GetEntitesToUpdateContent(ConnectionData connectionData, IEnumerable<ElementForPublish> elements)
        {
            var result = new List<WebResource>();

            var list = elements.OrderBy(element => element.SelectedFile.FriendlyFilePath).ToList();

            var tableNotCustomizable = new FormatTextTableHandler("FileName", "WebResourceName", "WebResourceType");

            var tableUpdatedContent = new FormatTextTableHandler("FileName", "WebResourceName", "WebResourceType");

            var tableEqual = new FormatTextTableHandler("FileName", "WebResourceName", "WebResourceType");

            var tableDependencyUpdated = new FormatTextTableHandler("FileName", "New Dependencies Count", "WebResourceName", "WebResourceType", "New Dependencies");

            foreach (var element in list)
            {
                var contentFile = Convert.ToBase64String(File.ReadAllBytes(element.SelectedFile.FilePath));
                var contentWebResource = element.WebResource.Content ?? string.Empty;

                element.WebResource.FormattedValues.TryGetValue(WebResource.Schema.Attributes.webresourcetype, out var webResourceTypeName);

                var isCustomizable = (element.WebResource.IsCustomizable?.Value).GetValueOrDefault(true);

                if (contentFile == contentWebResource)
                {
                    tableEqual.AddLine(element.SelectedFile.FileName, element.WebResource.Name, webResourceTypeName);
                }

                if (isCustomizable)
                {
                    WebResource resource = null;

                    if (contentFile != contentWebResource)
                    {
                        if (resource == null)
                        {
                            resource = new WebResource();
                            resource.WebResourceId = resource.Id = element.WebResource.Id;
                        }

                        resource.Content = contentFile;

                        tableUpdatedContent.AddLine(element.SelectedFile.FileName, element.WebResource.Name, webResourceTypeName);
                    }

                    if (element.NewDependenciesCount > 0)
                    {
                        if (resource == null)
                        {
                            resource = new WebResource();
                            resource.WebResourceId = resource.Id = element.WebResource.Id;
                        }

                        resource.DependencyXml = element.NewDependencyXml;

                        tableDependencyUpdated.AddLine(element.SelectedFile.FileName, element.NewDependenciesCount.ToString(), element.WebResource.Name, webResourceTypeName, element.NewDependencies);
                    }

                    if (resource != null)
                    {
                        result.Add(resource);
                    }
                }
                else
                {
                    tableNotCustomizable.AddLine(element.SelectedFile.FileName, element.WebResource.Name, webResourceTypeName);
                }
            }

            if (tableNotCustomizable.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResources are NOT Customizable, can't change WebResource's content: {0}", tableNotCustomizable.Count.ToString());

                var lines = tableNotCustomizable.GetFormatedLines(false);

                lines.ForEach(item => _iWriteToOutput.WriteToOutput(connectionData, "{0}{1}", _tabSpacer, item));
            }

            if (tableEqual.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResources equal to file content: {0}", tableEqual.Count.ToString());

                var lines = tableEqual.GetFormatedLines(false);

                lines.ForEach(item => _iWriteToOutput.WriteToOutput(connectionData, "{0}{1}", _tabSpacer, item));
            }

            if (tableUpdatedContent.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "Updated WebResources: {0}", tableUpdatedContent.Count.ToString());

                tableUpdatedContent.GetFormatedLines(false).ForEach(item => _iWriteToOutput.WriteToOutput(connectionData, "{0}{1}", _tabSpacer, item));
            }

            if (tableDependencyUpdated.Count > 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "Updated WebResources DependencyXml: {0}", tableDependencyUpdated.Count.ToString());

                tableDependencyUpdated.GetFormatedLines(false).ForEach(item => _iWriteToOutput.WriteToOutput(connectionData, "{0}{1}", _tabSpacer, item));
            }

            return result;
        }

        #region Обновление веб-ресурсов и публикация.

        /// <summary>
        /// Запуск публикации ресурсов
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public async Task ExecuteUpdateContentAndPublish(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            await CheckEncodingCheckReadOnlyConnectExecuteActionAsync(connectionData
                , Properties.OperationNames.UpdatingContentAndPublishingFormat1
                , selectedFiles
                , true
                , (service) => UpdatingContentAndPublish(service, selectedFiles)
            );
        }

        private async Task UpdatingContentAndPublish(IOrganizationServiceExtented service, List<SelectedFile> selectedFiles)
        {
            // Репозиторий для работы с веб-ресурсами
            var webResourceRepository = new WebResourceRepository(service);

            var findResult = await FindWebResources(service, webResourceRepository, selectedFiles);

            if (!findResult.Item1)
            {
                return;
            }

            var elements = findResult.Item2;

            if (!elements.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            await UpdateContentAndPublishAsync(service, elements);
        }

        private async Task<Tuple<bool, Dictionary<Guid, ElementForPublish>>> FindWebResources(IOrganizationServiceExtented service, WebResourceRepository webResourceRepository, List<SelectedFile> selectedFiles)
        {
            var elements = new Dictionary<Guid, ElementForPublish>();

            bool allForOther = false;

            var groups = selectedFiles.GroupBy(sel => sel.Extension);

            foreach (var gr in groups)
            {
                var names = gr.Select(sel => sel.FriendlyFilePath).ToArray();

                var dict = webResourceRepository.FindMultiple(gr.Key, names);

                foreach (var selectedFile in gr)
                {
                    if (!File.Exists(selectedFile.FilePath))
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "File not founded: {0}", selectedFile.FilePath);
                        continue;
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Try to find web-resource by name: {0}. Searching...", selectedFile.Name);

                    string key = selectedFile.FriendlyFilePath.ToLower();

                    var webResource = WebResourceRepository.FindWebResourceInDictionary(dict, key, gr.Key);

                    if (webResource != null)
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "WebResource founded by name. WebResourceId: {0} Name: {1}", webResource.Id, webResource.Name);
                    }

                    if (webResource == null)
                    {
                        if (selectedFile.FileName.StartsWith(service.ConnectionData.Name + "."))
                        {
                            string newFileName = selectedFile.FileName.Replace(service.ConnectionData.Name + ".", string.Empty);

                            string newFilePath = Path.Combine(Path.GetDirectoryName(selectedFile.FilePath), newFileName);

                            var newSelectedFile = new SelectedFile(newFilePath, selectedFile.SolutionDirectoryPath);

                            var newDict = webResourceRepository.FindMultiple(newSelectedFile.Extension, new[] { newSelectedFile.FriendlyFilePath });

                            webResource = WebResourceRepository.FindWebResourceInDictionary(newDict, newSelectedFile.FriendlyFilePath.ToLower(), newSelectedFile.Extension);

                            if (webResource != null)
                            {
                                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "WebResource founded by name with Connection Prefix. WebResourceId: {0} Name: {1}", webResource.Id, webResource.Name);
                            }
                        }
                    }

                    if (webResource == null)
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "WebResource not founded by name. FileName: {0}. Open linking dialog...", selectedFile.Name);

                        Guid? webId = service.ConnectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                        bool? dialogResult = null;
                        Guid? selectedWebResourceId = null;

                        var t = new Thread(() =>
                        {
                            try
                            {
                                var form = new Views.WindowWebResourceSelectOrCreate(this._iWriteToOutput, service, selectedFile, webId);
                                form.ShowCreateButton(allForOther);

                                dialogResult = form.ShowDialog();

                                allForOther = form.ForAllOther;

                                selectedWebResourceId = form.SelectedWebResourceId;
                            }
                            catch (Exception ex)
                            {
                                DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                            }
                        });
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();

                        t.Join();

                        if (string.IsNullOrEmpty(service.ConnectionData?.LastSelectedSolutionsUniqueName?.FirstOrDefault()))
                        {
                            allForOther = false;
                        }

                        service.ConnectionData.Save();

                        if (dialogResult.GetValueOrDefault())
                        {
                            if (selectedWebResourceId.HasValue)
                            {
                                webResource = await webResourceRepository.GetByIdAsync(selectedWebResourceId.Value);
                            }
                            else
                            {
                                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "!Warning. WebResource not linked. name: {0}.", selectedFile.Name);
                            }
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Updating Content and Publishing was cancelled.");
                            return Tuple.Create(false, (Dictionary<Guid, ElementForPublish>)null);
                        }
                    }

                    if (webResource != null)
                    {
                        // Запоминается файл
                        service.ConnectionData.AddMapping(webResource.Id, selectedFile.FriendlyFilePath);

                        if (!elements.ContainsKey(webResource.Id))
                        {
                            elements.Add(webResource.Id, new ElementForPublish(selectedFile, webResource));
                        }
                    }
                }
            }

            //Сохранение настроек после публикации
            service.ConnectionData.Save();

            return Tuple.Create(true, elements);
        }

        #endregion Обновление веб-ресурсов и публикация.

        #region Публикация веб-ресурсов разных по содержанию, но одинаковых по тексту.

        /// <summary>
        /// Запуск публикации ресурсов
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public async Task ExecuteUpdateContentAndPublishEqualByText(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            await CheckEncodingConnectFindWebResourceExecuteActionTaskAsync(connectionData
                , Properties.OperationNames.UpdatingContentWebResourcesEqualByTextAndPublishingFormat1
                , selectedFiles
                , OpenFilesType.EqualByText
                , UpdatingContentAndPublishEqualByText
            );
        }

        /// <summary>
        /// Запуск публикации веб-ресурсов. 
        /// Определяет ид веб-ресурсов
        /// 1. в файле привязок
        /// 2. по имени веб-ресурса
        /// 3. ручное связывание
        /// </summary>
        private async Task UpdatingContentAndPublishEqualByText(ConnectionData connectionData, IOrganizationServiceExtented service, TupleList<SelectedFile, WebResource> filesToPublish)
        {
            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            if (!filesToPublish.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            // Менеджер для публикации в CRM.
            Dictionary<Guid, ElementForPublish> elements = new Dictionary<Guid, ElementForPublish>();

            foreach (var item in filesToPublish)
            {
                if (!elements.ContainsKey(item.Item2.Id))
                {
                    elements.Add(item.Item2.Id, new ElementForPublish(item.Item1, item.Item2));
                }
            }

            await UpdateContentAndPublishAsync(service, elements);
        }

        #endregion Публикация веб-ресурсов разных по содержанию, но одинаковых по тексту.

        #region Публикация всего.

        public async Task ExecutePublishingAll(ConnectionData connectionData)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.PublishingAllCustomizationFormat1
                , (service) => PublishingAllAsync(service)
            );
        }

        private async Task PublishingAllAsync(IOrganizationServiceExtented service)
        {
            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishAllXmlAsync();

                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PublishingAllCompletedFormat1, service.ConnectionData.Name);

                service.TryDispose();
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PublishingAllFailedFormat1, service.ConnectionData.Name);
            }
        }

        #endregion Публикация всего.

        private static Regex _regexReference = new Regex(@"^[\/]{3,}[\s]+<reference[\s]+path=\""(?<path>.+)\""[\s]+\/>[\s]*\r?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private static HashSet<string> GetFileReferencesFilePaths(string javaScriptCode, string selectedFileFolder, string solutionDirectoryPath)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            var matches = _regexReference.Matches(javaScriptCode);

            foreach (var match in matches.OfType<Match>())
            {
                if (match.Success && match.Groups["path"] != null)
                {
                    var path = match.Groups["path"].Value;

                    if (!string.IsNullOrEmpty(path))
                    {
                        string referenceFilePath = path;

                        if (path.StartsWith("~"))
                        {
                            referenceFilePath = Path.Combine(solutionDirectoryPath, path.Substring(1));
                        }

                        if (path.StartsWith(".."))
                        {
                            referenceFilePath = Path.Combine(selectedFileFolder, path);
                        }
                        else if (!File.Exists(referenceFilePath))
                        {
                            referenceFilePath = Path.Combine(selectedFileFolder, path);
                        }

                        FileInfo fileInfo = new FileInfo(referenceFilePath);

                        if (fileInfo.Exists)
                        {
                            result.Add(fileInfo.FullName);
                        }
                    }
                }
            }

            return result;
        }

        private static Dictionary<string, WebResource> GetRefernecedWebResources(
            ConnectionData connectionData
            , WebResourceRepository webResourceRepository
            , Dictionary<string, WebResource> knownWebResources
            , string SolutionDirectoryPath
            , IEnumerable<string> referenceFilePathList
        )
        {
            var result = new Dictionary<string, WebResource>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var filePath in referenceFilePathList)
            {
                string extension = Path.GetExtension(filePath).ToLower();

                if (!WebResourceRepository.IsSupportedExtension(extension))
                {
                    continue;
                }

                if (!knownWebResources.ContainsKey(filePath))
                {
                    string fileName = Path.GetFileName(filePath);
                    string friendlyFilePath = SelectedFile.GetFriendlyPath(filePath, SolutionDirectoryPath);

                    var dict = webResourceRepository.FindMultiple(extension, new[] { friendlyFilePath });

                    var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, friendlyFilePath, extension);

                    if (webresource == null)
                    {
                        if (fileName.StartsWith(connectionData.Name + "."))
                        {
                            string newFileName = fileName.Replace(connectionData.Name + ".", string.Empty);

                            string newFilePath = Path.Combine(Path.GetDirectoryName(friendlyFilePath), newFileName);

                            var newSelectedFile = new SelectedFile(newFilePath, SolutionDirectoryPath);

                            var newDict = webResourceRepository.FindMultiple(newSelectedFile.Extension, new[] { newSelectedFile.FriendlyFilePath });

                            webresource = WebResourceRepository.FindWebResourceInDictionary(newDict, newSelectedFile.FriendlyFilePath.ToLower(), newSelectedFile.Extension);
                        }
                    }

                    if (webresource != null)
                    {
                        // Запоминается файл
                        connectionData.AddMapping(webresource.Id, friendlyFilePath);

                        knownWebResources.Add(filePath, webresource);
                    }
                }

                if (knownWebResources.ContainsKey(filePath))
                {
                    var knowWebResource = knownWebResources[filePath];

                    result.Add(knowWebResource.Name, knowWebResource);
                }
            }

            return result;
        }

        #region Including References to WebResources DependencyXml

        public async Task ExecuteIncludeReferencesToDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            await CheckEncodingCheckReadOnlyConnectExecuteActionAsync(connectionData
                , Properties.OperationNames.IncludeReferencesToDependencyXmlAndPublishingFormat1
                , selectedFiles
                , true
                , (service) => IncludingReferencesToDependencyXml(service, commonConfig, selectedFiles)
            );
        }

        private async Task IncludingReferencesToDependencyXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            var webResourceRepository = new WebResourceRepository(service);

            var findResult = await FindWebResources(service, webResourceRepository, selectedFiles);

            if (!findResult.Item1)
            {
                return;
            }

            var elements = findResult.Item2;

            if (!elements.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            FillNewDependenciesInfo(service, webResourceRepository, elements);

            var webResourceToPublish = elements.Values.Where(e => e.NewDependenciesCount > 0).ToList();

            if (!webResourceToPublish.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            foreach (var tuple in webResourceToPublish.OrderBy(e => e.SelectedFile.FileName))
            {
                var newDependencyXml = tuple.NewDependencyXml;
                var webResource = tuple.WebResource;

                {
                    string fieldTitle = WebResource.Schema.Headers.dependencyxml;

                    string dependencyXml = webResource.DependencyXml;

                    if (!string.IsNullOrEmpty(dependencyXml))
                    {
                        commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                        string fileNameBackUp = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, webResource.Name, fieldTitle + " BackUp", "xml");
                        string filePathBackUp = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileNameBackUp));

                        try
                        {
                            dependencyXml = ContentComparerHelper.FormatXmlByConfiguration(
                                dependencyXml
                                , commonConfig
                                , XmlOptionsControls.WebResourceDependencyXmlOptions
                                , schemaName: Commands.AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlSchema
                                , webResourceName: webResource.Name
                            );

                            File.WriteAllText(filePathBackUp, dependencyXml, new UTF8Encoding(false));

                            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, WebResource.Schema.EntitySchemaName, webResource.Name, fieldTitle, filePathBackUp);
                        }
                        catch (Exception ex)
                        {
                            this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                        }
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, WebResource.Schema.EntitySchemaName, webResource.Name, fieldTitle);
                        this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    }
                }

                var updateWebResource = new WebResource()
                {
                    Id = webResource.Id,
                    WebResourceId = webResource.Id,
                    DependencyXml = newDependencyXml,
                };

                await service.UpdateAsync(updateWebResource);
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Updating WebResources Completed.");

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Publishing WebResources...");

            var repositoryPublish = new PublishActionsRepository(service);
            await repositoryPublish.PublishWebResourcesAsync(webResourceToPublish.Select(e => e.WebResource.Id));

            WriteToConsolePublishedWebResources(service.ConnectionData, webResourceToPublish.Select(e => e.WebResource));
        }

        private static void FillNewDependenciesInfo(IOrganizationServiceExtented service, WebResourceRepository webResourceRepository, Dictionary<Guid, ElementForPublish> elements)
        {
            var knownWebResources = new Dictionary<string, WebResource>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var element in elements.Values)
            {
                knownWebResources.Add(element.SelectedFile.FilePath, element.WebResource);
            }

            foreach (var element in elements.Values)
            {
                var selectedFile = element.SelectedFile;
                var webResource = element.WebResource;

                var serializer = new XmlSerializer(typeof(Dependencies));

                Dependencies webResourceDependencies = null;

                if (!string.IsNullOrEmpty(webResource.DependencyXml))
                {
                    using (TextReader reader = new StringReader(webResource.DependencyXml))
                    {
                        webResourceDependencies = serializer.Deserialize(reader) as Dependencies;
                    }
                }
                else
                {
                    webResourceDependencies = new Dependencies();
                }

                string javaScriptCode = File.ReadAllText(selectedFile.FilePath);
                string selectedFileFolder = Path.GetDirectoryName(selectedFile.FilePath);

                var referenceFilePathList = GetFileReferencesFilePaths(javaScriptCode, selectedFileFolder, selectedFile.SolutionDirectoryPath);

                var referenceWebResourceDictionary = GetRefernecedWebResources(service.ConnectionData, webResourceRepository, knownWebResources, selectedFile.SolutionDirectoryPath, referenceFilePathList);

                IEnumerable<string> currentWebResourceDependenciesStrings = Enumerable.Empty<string>();

                if (webResourceDependencies.Dependency != null)
                {
                    currentWebResourceDependenciesStrings = from dep in webResourceDependencies.Dependency
                                                            where dep.componentType == componentType.WebResource && dep.Library != null
                                                            from lib in dep.Library
                                                            select lib.name
                                                            ;
                }

                HashSet<string> currentWebResourceDependencies = new HashSet<string>(currentWebResourceDependenciesStrings, StringComparer.InvariantCultureIgnoreCase);

                var fileDependencies = new HashSet<string>(referenceWebResourceDictionary.Keys, StringComparer.InvariantCultureIgnoreCase);

                var newDependencies = fileDependencies.Except(currentWebResourceDependencies, StringComparer.InvariantCultureIgnoreCase).ToList();

                if (!newDependencies.Any())
                {
                    continue;
                }

                DependenciesDependency dependenciesWebResource = null;

                var librariesWebResource = new List<DependenciesDependencyLibrary>();

                if (webResourceDependencies.Dependency != null)
                {
                    dependenciesWebResource = webResourceDependencies.Dependency.FirstOrDefault(d => d.componentType == componentType.WebResource);

                    if (dependenciesWebResource != null)
                    {
                        if (dependenciesWebResource.Library != null)
                        {
                            librariesWebResource.AddRange(dependenciesWebResource.Library);
                        }
                    }
                    else
                    {
                        dependenciesWebResource = new DependenciesDependency()
                        {
                            componentType = componentType.WebResource,
                        };

                        List<DependenciesDependency> dependenciesDependencies = new List<DependenciesDependency>();

                        dependenciesDependencies.AddRange(webResourceDependencies.Dependency);
                        dependenciesDependencies.Add(dependenciesWebResource);

                        webResourceDependencies.Dependency = dependenciesDependencies.ToArray();
                    }
                }
                else
                {
                    dependenciesWebResource = new DependenciesDependency()
                    {
                        componentType = componentType.WebResource,
                    };

                    webResourceDependencies.Dependency = new[] { dependenciesWebResource };
                }

                foreach (var item in newDependencies)
                {
                    var newWebResource = referenceWebResourceDictionary[item];

                    librariesWebResource.Add(new DependenciesDependencyLibrary()
                    {
                        libraryUniqueId = Guid.NewGuid().ToString("B").ToLower(),
                        name = item,
                        displayName = newWebResource.DisplayName ?? string.Empty,
                        description = newWebResource.Description ?? string.Empty,
                        languagecode = newWebResource.LanguageCode.ToString(),
                    });
                }

                dependenciesWebResource.Library = librariesWebResource.ToArray();

                var newDependencyXmlStringBuilder = new StringBuilder();

                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                using (var writer = new StringWriter(newDependencyXmlStringBuilder))
                {
                    serializer.Serialize(writer, webResourceDependencies, namespaces);
                }

                string newDependencyXml = newDependencyXmlStringBuilder.ToString();

                if (ContentComparerHelper.TryParseXml(newDependencyXml, out XElement doc))
                {
                    ContentComparerHelper.ClearRoot(doc);

                    newDependencyXml = doc.ToString(SaveOptions.DisableFormatting);
                }

                element.NewDependencyXml = newDependencyXml;
                element.NewDependenciesCount = newDependencies.Count;
                element.NewDependencies = string.Join(", ", newDependencies.OrderBy(s => s));
            }
        }

        #endregion Including References to WebResources DependencyXml

        public async Task ExecuteUpdateContentIncludeReferencesToDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            await CheckEncodingCheckReadOnlyConnectExecuteActionAsync(connectionData
                , Properties.OperationNames.UpdateContentIncludeReferencesToDependencyXmlFormat1
                , selectedFiles
                , true
                , (service) => UpdatingContentIncludingReferencesToDependencyXml(service, commonConfig, selectedFiles)
            );
        }

        private async Task UpdatingContentIncludingReferencesToDependencyXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            var webResourceRepository = new WebResourceRepository(service);

            var findResult = await FindWebResources(service, webResourceRepository, selectedFiles);

            if (!findResult.Item1)
            {
                return;
            }

            var elements = findResult.Item2;

            if (!elements.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            FillNewDependenciesInfo(service, webResourceRepository, elements);

            if (!elements.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            foreach (var element in elements.Values.Where(e => e.NewDependenciesCount > 0).OrderBy(e => e.SelectedFile.FileName))
            {
                var webResource = element.WebResource;

                string fieldTitle = WebResource.Schema.Headers.dependencyxml;

                string dependencyXml = webResource.DependencyXml;

                if (!string.IsNullOrEmpty(dependencyXml))
                {
                    commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                    string fileNameBackUp = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, webResource.Name, fieldTitle + " BackUp", "xml");
                    string filePathBackUp = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileNameBackUp));

                    try
                    {
                        dependencyXml = ContentComparerHelper.FormatXmlByConfiguration(
                            dependencyXml
                            , commonConfig
                            , XmlOptionsControls.WebResourceDependencyXmlOptions
                            , schemaName: Commands.AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlSchema
                            , webResourceName: webResource.Name
                        );

                        File.WriteAllText(filePathBackUp, dependencyXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, WebResource.Schema.EntitySchemaName, webResource.Name, fieldTitle, filePathBackUp);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, WebResource.Schema.EntitySchemaName, webResource.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }

            await UpdateContentAndPublishAsync(service, elements);
        }

        public async Task ExecuteUpdateEqualByTextContentIncludeReferencesToDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            await CheckEncodingConnectFindWebResourceExecuteActionTaskAsync(connectionData
                , Properties.OperationNames.UpdateEqualByTextContentIncludeReferencesToDependencyXmlFormat1
                , selectedFiles
                , OpenFilesType.EqualByText
                , (conn, service, filesToPublish) => UpdateEqualByTextContentIncludeReferencesToDependencyXml(conn, service, commonConfig, filesToPublish)
            );
        }

        private async Task UpdateEqualByTextContentIncludeReferencesToDependencyXml(ConnectionData connectionData, IOrganizationServiceExtented service, CommonConfiguration commonConfig, TupleList<SelectedFile, WebResource> filesToPublish)
        {
            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            if (!filesToPublish.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            // Менеджер для публикации в CRM.
            Dictionary<Guid, ElementForPublish> elements = new Dictionary<Guid, ElementForPublish>();

            foreach (var item in filesToPublish)
            {
                if (!elements.ContainsKey(item.Item2.Id))
                {
                    elements.Add(item.Item2.Id, new ElementForPublish(item.Item1, item.Item2));
                }
            }

            var webResourceRepository = new WebResourceRepository(service);

            FillNewDependenciesInfo(service, webResourceRepository, elements);

            if (!elements.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            foreach (var element in elements.Values.Where(e => e.NewDependenciesCount > 0).OrderBy(e => e.SelectedFile.FileName))
            {
                var webResource = element.WebResource;

                string fieldTitle = WebResource.Schema.Headers.dependencyxml;

                string dependencyXml = webResource.DependencyXml;

                if (!string.IsNullOrEmpty(dependencyXml))
                {
                    commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                    string fileNameBackUp = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, webResource.Name, fieldTitle + " BackUp", "xml");
                    string filePathBackUp = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileNameBackUp));

                    try
                    {
                        dependencyXml = ContentComparerHelper.FormatXmlByConfiguration(
                            dependencyXml
                            , commonConfig
                            , XmlOptionsControls.WebResourceDependencyXmlOptions
                            , schemaName: Commands.AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlSchema
                            , webResourceName: webResource.Name
                        );

                        File.WriteAllText(filePathBackUp, dependencyXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, WebResource.Schema.EntitySchemaName, webResource.Name, fieldTitle, filePathBackUp);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, WebResource.Schema.EntitySchemaName, webResource.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }

            await UpdateContentAndPublishAsync(service, elements);
        }

        #region Including References to Linked SystemForms Libraries

        public async Task ExecuteIncludeReferencesToLinkedSystemFormsLibraries(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            await CheckEncodingCheckReadOnlyConnectExecuteActionAsync(connectionData
                , Properties.OperationNames.IncludeReferencesToLinkedSystemFormsLibrariesFormat1
                , selectedFiles
                , true
                , (service) => IncludingReferencesToLinkedSystemFormsLibraries(service, commonConfig, selectedFiles)
            );
        }

        private async Task IncludingReferencesToLinkedSystemFormsLibraries(IOrganizationServiceExtented service, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            var knownWebResources = new Dictionary<string, WebResource>(StringComparer.InvariantCultureIgnoreCase);

            var systemFormRepository = new SystemFormRepository(service);

            // Репозиторий для работы с веб-ресурсами
            var webResourceRepository = new WebResourceRepository(service);

            var changedSystemForms = new TupleList<SelectedFile, SystemForm, string, int, string>();

            foreach (var selectedFile in selectedFiles)
            {
                if (!File.Exists(selectedFile.FilePath))
                {
                    continue;
                }

                string javaScriptCode = File.ReadAllText(selectedFile.FilePath);

                if (!CommonHandlers.GetLinkedSystemForm(javaScriptCode, out string entityName, out Guid formId, out int formType))
                {
                    continue;
                }

                var systemForm = await systemFormRepository.GetByIdAsync(formId, new ColumnSet(true));

                if (systemForm == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(systemForm.FormXml))
                {
                    continue;
                }

                if (!ContentComparerHelper.TryParseXml(systemForm.FormXml, out var docFormXml))
                {
                    continue;
                }

                string selectedFileFolder = Path.GetDirectoryName(selectedFile.FilePath);

                var referenceFilePathList = GetFileReferencesFilePaths(javaScriptCode, selectedFileFolder, selectedFile.SolutionDirectoryPath);

                var referenceWebResourceDictionary = GetRefernecedWebResources(service.ConnectionData, webResourceRepository, knownWebResources, selectedFile.SolutionDirectoryPath, referenceFilePathList);

                var formLibraries = docFormXml.Element("formLibraries");

                if (formLibraries != null)
                {
                    HashSet<string> currentWebResourceDependenciesStrings = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

                    var allLibraries = formLibraries.Descendants("Library");

                    foreach (var nodeLibrary in allLibraries)
                    {
                        var name = (string)nodeLibrary.Attribute("name");

                        if (!string.IsNullOrEmpty(name))
                        {
                            currentWebResourceDependenciesStrings.Add(name);
                        }
                    }

                    var fileDependencies = new HashSet<string>(referenceWebResourceDictionary.Values.Where(e => e.WebResourceTypeEnum == WebResource.Schema.OptionSets.webresourcetype.Script_JScript_3).Select(e => e.Name), StringComparer.InvariantCultureIgnoreCase);

                    var newDependencies = fileDependencies.Except(currentWebResourceDependenciesStrings, StringComparer.InvariantCultureIgnoreCase).ToList();

                    if (!newDependencies.Any())
                    {
                        continue;
                    }

                    {
                        string fieldTitle = SystemForm.Schema.Headers.formxml;

                        string formXml = systemForm.FormXml;

                        if (!string.IsNullOrEmpty(formXml))
                        {
                            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                            string fileNameBackUp = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, fieldTitle + " BackUp", "xml");
                            string filePathBackUp = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileNameBackUp));

                            try
                            {
                                formXml = ContentComparerHelper.FormatXmlByConfiguration(
                                    formXml
                                    , commonConfig
                                    , XmlOptionsControls.FormXmlOptions
                                    , schemaName: Commands.AbstractDynamicCommandXsdSchemas.FormXmlSchema
                                    , formId: systemForm.Id
                                    , entityName: systemForm.ObjectTypeCode
                                );

                                File.WriteAllText(filePathBackUp, formXml, new UTF8Encoding(false));

                                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntitySchemaName, systemForm.Name, fieldTitle, filePathBackUp);
                            }
                            catch (Exception ex)
                            {
                                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                            }
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntitySchemaName, systemForm.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                        }
                    }

                    foreach (var webResourceName in newDependencies)
                    {
                        formLibraries.Add(
                            new XElement("Library"
                                , new XAttribute("name", webResourceName)
                                , new XAttribute("libraryUniqueId", Guid.NewGuid().ToString("B").ToLower())
                            )
                        );
                    }

                    ContentComparerHelper.ClearRoot(docFormXml);

                    var newFormXml = docFormXml.ToString(SaveOptions.DisableFormatting);

                    changedSystemForms.Add(selectedFile, systemForm, newFormXml, newDependencies.Count, string.Join(", ", newDependencies.OrderBy(s => s)));
                }
            }

            if (!changedSystemForms.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            {
                FormatTextTableHandler tableUpdated = new FormatTextTableHandler();
                tableUpdated.SetHeader("FileName", "Dependencies", "Form Entity", "FormType", "FormName", "New Dependencies");

                foreach (var tuple in changedSystemForms
                    .OrderBy(e => e.Item2.ObjectTypeCode)
                    .ThenBy(e => (int?)e.Item2.TypeEnum)
                    .ThenBy(e => e.Item2.Name)
                )
                {
                    var filePath = tuple.Item1.FilePath;
                    var systemForm = tuple.Item2;

                    systemForm.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.type, out var formTypeName);

                    tableUpdated.AddLine(filePath, tuple.Item4.ToString(), systemForm.ObjectTypeCode, formTypeName, systemForm.Name, tuple.Item5);
                }

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Updating SystemForm FormXml and Publishing...");

                tableUpdated.GetFormatedLines(false).ForEach(item => _iWriteToOutput.WriteToOutput(service.ConnectionData, "{0}{1}", _tabSpacer, item));
            }

            foreach (var tuple in changedSystemForms
                .OrderBy(e => e.Item2.ObjectTypeCode)
                .ThenBy(e => (int?)e.Item2.TypeEnum)
                .ThenBy(e => e.Item2.Name)
            )
            {
                var systemForm = tuple.Item2;
                var newFormXml = tuple.Item3;

                var updateSystemForm = new SystemForm()
                {
                    Id = systemForm.Id,
                    FormId = systemForm.Id,

                    FormXml = newFormXml,
                };

                await service.UpdateAsync(updateSystemForm);
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Update SystemForms FormXml Completed.");

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Publishing SystemForms...");

            var repositoryPublish = new PublishActionsRepository(service);
            await repositoryPublish.PublishDashboardsAsync(changedSystemForms.Select(e => e.Item2.Id));

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Published SystemForms: {0}", changedSystemForms.Count);

            HashSet<string> formsEntityNames = new HashSet<string>(changedSystemForms.Where(e => e.Item2.ObjectTypeCode.IsValidEntityName()).Select(e => e.Item2.ObjectTypeCode), StringComparer.InvariantCultureIgnoreCase);

            if (formsEntityNames.Any())
            {
                var entityNamesOrdered = string.Join(",", formsEntityNames.OrderBy(s => s));

                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);
                await repositoryPublish.PublishEntitiesAsync(formsEntityNames);
            }
        }

        #endregion Including References to Linked SystemForms Libraries
    }
}