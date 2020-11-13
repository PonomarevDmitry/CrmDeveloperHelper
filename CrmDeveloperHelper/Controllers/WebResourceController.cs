using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.XsdModels;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
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
    public partial class WebResourceController : BaseController<IWriteToOutputAndPublishList>
    {
        public WebResourceController(IWriteToOutputAndPublishList iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

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
            using (service.Lock())
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

            using (service.Lock())
            {
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
        }

        #endregion Публикация веб-ресурсов разных по содержанию, но одинаковых по тексту.

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
            using (service.Lock())
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

                FillNewDependenciesInfo(service.ConnectionData, webResourceRepository, elements);

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

                            string fileNameBackUp = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, webResource.Name, fieldTitle + " BackUp", FileExtension.xml);
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
        }

        private static void FillNewDependenciesInfo(ConnectionData connectionData, WebResourceRepository webResourceRepository, Dictionary<Guid, ElementForPublish> elements)
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

                if (!FileOperations.SupportsJavaScriptType(selectedFile.FilePath))
                {
                    continue;
                }

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

                var referenceWebResourceDictionary = GetRefernecedWebResources(connectionData, webResourceRepository, knownWebResources, selectedFile.SolutionDirectoryPath, referenceFilePathList);

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

        #region Update Content and Include References to DependencyXml

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
            using (service.Lock())
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

                FillNewDependenciesInfo(service.ConnectionData, webResourceRepository, elements);

                foreach (var element in elements.Values.Where(e => e.NewDependenciesCount > 0).OrderBy(e => e.SelectedFile.FileName))
                {
                    var webResource = element.WebResource;

                    string fieldTitle = WebResource.Schema.Headers.dependencyxml;

                    string dependencyXml = webResource.DependencyXml;

                    if (!string.IsNullOrEmpty(dependencyXml))
                    {
                        commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                        string fileNameBackUp = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, webResource.Name, fieldTitle + " BackUp", FileExtension.xml);
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
        }

        #endregion Update Content and Include References to DependencyXml

        #region Update Equal by Text Content and Include References to DependencyXml

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

            using (service.Lock())
            {
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

                FillNewDependenciesInfo(service.ConnectionData, webResourceRepository, elements);

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

                        string fileNameBackUp = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, webResource.Name, fieldTitle + " BackUp", FileExtension.xml);
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
        }

        #endregion Update Equal by Text Content and Include References to DependencyXml

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
            using (service.Lock())
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

                                string fileNameBackUp = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, fieldTitle + " BackUp", FileExtension.xml);
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

                var formsEntityNames = new HashSet<string>(changedSystemForms.Where(e => e.Item2.ObjectTypeCode.IsValidEntityName()).Select(e => e.Item2.ObjectTypeCode), StringComparer.InvariantCultureIgnoreCase);

                if (formsEntityNames.Any())
                {
                    var entityNamesOrdered = string.Join(",", formsEntityNames.OrderBy(s => s));

                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);
                    await repositoryPublish.PublishEntitiesAsync(formsEntityNames);
                }
            }
        }

        #endregion Including References to Linked SystemForms Libraries

        #region Сравнение с веб-ресурсами.

        public async Task ExecuteComparingFilesAndWebResources(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
        {
            string operation = string.Format(Properties.OperationNames.ComparingFilesAndWebResourcesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

                await ComparingFilesAndWebResourcesAsync(connectionData, selectedFiles, withDetails);
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

        #endregion Сравнение с веб-ресурсами.

        #region Добавление в список на публикацию идентичных по тексту, но не по содержанию файлов.

        public async Task ExecuteAddingIntoPublishListFilesByType(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            await CheckEncodingConnectFindWebResourceExecuteActionAsync(connectionData
                , Properties.OperationNames.AddingIntoPublishListFilesFormat2
                , selectedFiles
                , openFilesType
                , AddingIntoPublishListFilesByType
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(openFilesType)
            );
        }

        private void AddingIntoPublishListFilesByType(ConnectionData connectionData, IOrganizationServiceExtented service, TupleList<SelectedFile, WebResource> listFilesToDifference)
        {
            if (listFilesToDifference.Any())
            {
                this._iWriteToOutput.AddToListForPublish(connectionData, listFilesToDifference.Select(f => f.Item1).OrderBy(f => f.FriendlyFilePath));
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "No files for adding to Publish List.");
            }
        }

        public async Task ExecuteRemovingIntoPublishListFilesByType(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            await CheckEncodingConnectFindWebResourceExecuteActionAsync(connectionData
                , Properties.OperationNames.AddingIntoPublishListFilesFormat2
                , selectedFiles
                , openFilesType
                , RemovingIntoPublishListFilesByType
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(openFilesType)
            );
        }

        private void RemovingIntoPublishListFilesByType(ConnectionData connectionData, IOrganizationServiceExtented service, TupleList<SelectedFile, WebResource> listFilesToDifference)
        {
            if (listFilesToDifference.Any())
            {
                this._iWriteToOutput.RemoveFromListForPublish(connectionData, listFilesToDifference.Select(f => f.Item1).OrderBy(f => f.FriendlyFilePath));
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "No files for removing from Publish List.");
            }
        }

        #endregion Добавление в список на публикацию идентичных по тексту, но не по содержанию файлов.

        public async Task ExecuteComparingFilesWithWrongEncoding(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
        {
            string operation = string.Format(Properties.OperationNames.ComparingFilesWithWrongEncodingAndWebResourcesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncoding(connectionData, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);

                await ComparingFilesAndWebResourcesAsync(connectionData, filesWithoutUTF8Encoding, withDetails);
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

        private bool SelecteWebResourceInWindow(IOrganizationServiceExtented service, SelectedFile selectedFile, Guid? lastLinkedWebResourceId, out Guid selectedWebResourceId)
        {
            selectedWebResourceId = Guid.Empty;

            bool? dialogResult = null;

            Guid? webResourceId = null;

            var t = new Thread(() =>
            {
                try
                {
                    var form = new WindowWebResourceSelectOrCreate(this._iWriteToOutput, service, selectedFile, lastLinkedWebResourceId);

                    dialogResult = form.ShowDialog();

                    webResourceId = form.SelectedWebResourceId;
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (dialogResult.GetValueOrDefault() == false)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DifferenceWasCancelled);
                return false;
            }

            if (!webResourceId.HasValue)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "!Warning. WebResource not exists. name: {0}.", selectedFile.Name);
                return false;
            }

            selectedWebResourceId = webResourceId.Value;

            return true;
        }

        #region Различия файла и веб-ресурса.

        public async Task ExecuteDifferenceWebResources(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceWebResourceFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, new[] { selectedFile }, out _);

                await DifferenceWebResources(connectionData, commonConfig, selectedFile, withSelect);
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

        private async Task DifferenceWebResources(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, bool withSelect)
        {
            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            WebResource webResource = null;
            Guid? lastLinkedWebResourceId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

            using (service.Lock())
            {
                // Репозиторий для работы с веб-ресурсами
                var webResourceRepository = new WebResourceRepository(service);

                if (!withSelect)
                {
                    webResource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);

                    if (webResource != null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "WebResource founded by name.");
                    }
                    else if (lastLinkedWebResourceId.HasValue)
                    {
                        if (webResource != null)
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name. Last link WebResource is selected for difference.");
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name and has not Last link.");
                            this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom WebResource selection form.");
                        }
                    }
                }

                if (webResource == null)
                {
                    if (SelecteWebResourceInWindow(service, selectedFile, lastLinkedWebResourceId, out Guid selectedWebResourceId))
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "Custom WebResource is selected.");

                        webResource = await webResourceRepository.GetByIdAsync(selectedWebResourceId);
                    }
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded in CRM: {0}", selectedFile.FileName);
                return;
            }

            connectionData.AddMapping(webResource.Id, selectedFile.FriendlyFilePath);
            connectionData.Save();

            string fileLocalPath = selectedFile.FilePath;
            string fileLocalTitle = selectedFile.FileName;

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            {
                var contentWebResource = webResource.Content ?? string.Empty;

                var array = Convert.FromBase64String(contentWebResource);

                filePath2 = FileOperations.GetNewTempFilePath(webResource.Name, selectedFile.Extension);

                File.WriteAllBytes(filePath2, array);

                fileTitle2 = connectionData.Name + "." + selectedFile.FileName + " - " + filePath2;
            }

            await this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);
        }

        #endregion Различия файла и веб-ресурса.

        #region Сравнение трех файлов вебресурсов.

        public async Task ExecuteThreeFileDifferenceWebResources(ConnectionData connectionData1, ConnectionData connectionData2, CommonConfiguration commonConfig, SelectedFile selectedFile, ShowDifferenceThreeFileType differenceType)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceLocalFileAndTwoWebResourcesFormat3, differenceType, connectionData1?.Name, connectionData2?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(null, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(null, new[] { selectedFile }, out _);

                await ThreeFileDifferenceWebResources(connectionData1, connectionData2, commonConfig, selectedFile, differenceType);
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

        private async Task ThreeFileDifferenceWebResources(ConnectionData connectionData1, ConnectionData connectionData2, CommonConfiguration commonConfig, SelectedFile selectedFile, ShowDifferenceThreeFileType differenceType)
        {
            if (differenceType == ShowDifferenceThreeFileType.ThreeWay)
            {
                if (!File.Exists(selectedFile.FilePath))
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                    return;
                }
            }

            var doubleConnection = await ConnectAndWriteToOutputDoubleConnectionAsync(connectionData1, connectionData2);

            if (doubleConnection == null)
            {
                return;
            }

            var service1 = doubleConnection.Item1;
            var service2 = doubleConnection.Item2;

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository1 = new WebResourceRepository(service1);
            WebResourceRepository webResourceRepository2 = new WebResourceRepository(service2);

            var taskWebResource1 = webResourceRepository1.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);
            var taskWebResource2 = webResourceRepository2.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);

            WebResource webresource1 = await taskWebResource1;
            WebResource webresource2 = await taskWebResource2;

            if (webresource1 != null)
            {
                this._iWriteToOutput.WriteToOutput(null, "{0}: WebResource founded by name.", connectionData1.Name);

                connectionData1.AddMapping(webresource1.Id, selectedFile.FriendlyFilePath);

                connectionData1.Save();
            }
            else
            {
                Guid? webId = connectionData1.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (webId.HasValue)
                {
                    webresource1 = await webResourceRepository1.GetByIdAsync(webId.Value);

                    if (webresource1 != null)
                    {
                        this._iWriteToOutput.WriteToOutput(null, "{0}: WebResource not founded by name. Last link WebResource is selected for difference.", connectionData1.Name);

                        connectionData1.AddMapping(webresource1.Id, selectedFile.FriendlyFilePath);

                        connectionData1.Save();
                    }
                }
            }

            if (webresource2 != null)
            {
                this._iWriteToOutput.WriteToOutput(null, "{0}: WebResource founded by name.", connectionData2.Name);

                connectionData2.AddMapping(webresource2.Id, selectedFile.FriendlyFilePath);

                connectionData2.Save();
            }
            else
            {
                Guid? webId = connectionData2.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (webId.HasValue)
                {
                    webresource2 = await webResourceRepository2.GetByIdAsync(webId.Value);

                    if (webresource2 != null)
                    {
                        this._iWriteToOutput.WriteToOutput(null, "{0}: WebResource not founded by name. Last link WebResource is selected for difference.", connectionData2.Name);

                        connectionData2.AddMapping(webresource2.Id, selectedFile.FriendlyFilePath);

                        connectionData2.Save();
                    }
                }
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
            }

            if (webresource1 == null)
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.WebResourceNotFoundedInConnectionFormat2, connectionData1.Name, selectedFile.FileName);
            }

            if (webresource2 == null)
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.WebResourceNotFoundedInConnectionFormat2, connectionData2.Name, selectedFile.FileName);
            }

            // string fileLocalPath, string fileLocalTitle, string filePath1, string fileTitle1, string filePath2, string fileTitle2,
            string fileLocalPath = selectedFile.FilePath;
            string fileLocalTitle = selectedFile.FileName;

            string filePath1 = string.Empty;
            string fileTitle1 = string.Empty;

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            if (webresource1 != null)
            {
                var contentWebResource1 = webresource1.Content ?? string.Empty;

                var array = Convert.FromBase64String(contentWebResource1);

                filePath1 = FileOperations.GetNewTempFilePath(webresource1.Name, selectedFile.Extension);
                fileTitle1 = connectionData1.Name + "." + selectedFile.FileName + " - " + filePath1;

                File.WriteAllBytes(filePath1, array);
            }

            if (webresource2 != null)
            {
                var contentWebResource2 = webresource2.Content ?? string.Empty;

                var array = Convert.FromBase64String(contentWebResource2);

                filePath2 = FileOperations.GetNewTempFilePath(webresource2.Name, selectedFile.Extension);
                fileTitle2 = connectionData2.Name + "." + selectedFile.FileName + " - " + filePath2;

                File.WriteAllBytes(filePath2, array);
            }

            switch (differenceType)
            {
                case ShowDifferenceThreeFileType.OneByOne:
                    ShowDifferenceOneByOne(commonConfig, connectionData1, connectionData2, fileLocalPath, fileLocalTitle, filePath1, fileTitle1, filePath2, fileTitle2);
                    break;

                case ShowDifferenceThreeFileType.TwoConnections:
                    await this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData1, filePath1, filePath2, fileTitle1, fileTitle2, connectionData2);
                    break;

                case ShowDifferenceThreeFileType.ThreeWay:
                    ShowDifferenceThreeWay(commonConfig, connectionData1, connectionData2, fileLocalPath, fileLocalTitle, filePath1, fileTitle1, filePath2, fileTitle2);
                    break;
                default:
                    break;
            }
        }

        #endregion Сравнение трех файлов вебресурсов.

        #region Сравнение файлов и веб-ресурсов по разным параметрам.

        public async Task ExecuteWebResourceMultiDifferenceFiles(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            await CheckEncodingConnectFindWebResourceExecuteActionTaskAsync(connectionData
                , Properties.OperationNames.MultiDifferenceFormat2
                , selectedFiles
                , openFilesType
                , MultiDifferenceFiles
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(openFilesType)
            );
        }

        private async Task MultiDifferenceFiles(ConnectionData connectionData, IOrganizationServiceExtented service, TupleList<SelectedFile, WebResource> listFilesToDifference)
        {
            if (!listFilesToDifference.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoFilesForDifference);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.StartingCompareProgramForCountFilesFormat1, listFilesToDifference.Count());

            foreach (var item in listFilesToDifference.OrderBy(file => file.Item1.FilePath))
            {
                var selectedFile = item.Item1;
                var webresource = item.Item2;

                if (webresource != null)
                {
                    var contentWebResource = webresource.Content;

                    var webResourceName = webresource.Name;

                    var array = Convert.FromBase64String(contentWebResource);

                    string tempFilePath = FileOperations.GetNewTempFilePath(webResourceName, selectedFile.Extension);

                    File.WriteAllBytes(tempFilePath, array);

                    string fileLocalPath = selectedFile.FilePath;
                    string fileLocalTitle = selectedFile.FileName;

                    string filePath2 = tempFilePath;
                    string fileTitle2 = connectionData.Name + "." + selectedFile.FileName + " - " + tempFilePath;

                    await this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);
                }
            }
        }

        #endregion Сравнение файлов и веб-ресурсов по разным параметрам.

        #region Creating WebResources EntityDescription

        public async Task ExecuteCreatingWebResourceEntityDescription(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.CreatingWebResourceEntityDescriptionFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, new[] { selectedFile }, out _);

                await CreatingWebResourceEntityDescription(selectedFile, connectionData, commonConfig);
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

        private async Task CreatingWebResourceEntityDescription(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            WebResource webResource = null;

            using (service.Lock())
            {
                // Репозиторий для работы с веб-ресурсами
                var webResourceRepository = new WebResourceRepository(service);

                webResource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);
                Guid? lastLinkedWebResourceId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (webResource != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "WebResource founded by name.");
                }
                else if (lastLinkedWebResourceId.HasValue)
                {
                    webResource = await webResourceRepository.GetByIdAsync(lastLinkedWebResourceId.Value);

                    if (webResource != null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name. Last link WebResource is selected for difference.");
                    }
                }

                if (webResource == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name and has not Last link.");
                    this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom WebResource selection form.");

                    if (SelecteWebResourceInWindow(service, selectedFile, lastLinkedWebResourceId, out Guid selectedWebResourceId))
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "Custom WebResource is selected.");

                        webResource = await webResourceRepository.GetByIdAsync(selectedWebResourceId);
                    }
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded in CRM: {0}", selectedFile.FileName);
                return;
            }

            connectionData.AddMapping(webResource.Id, selectedFile.FriendlyFilePath);
            connectionData.Save();

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string fileName = EntityFileNameFormatter.GetWebResourceFileName(connectionData.Name, webResource.Name, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, webResource, connectionData);

            this._iWriteToOutput.WriteToOutput(connectionData
                , Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                , connectionData.Name
                , webResource.LogicalName
                , filePath
            );

            this._iWriteToOutput.PerformAction(connectionData, filePath);
        }

        #endregion Creating WebResources EntityDescription

        #region Changing WebResource in EntityEditor

        public async Task ExecuteChangingWebResourceInEntityEditor(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.ChangingWebResourceInEntityEditorFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, new[] { selectedFile }, out _);

                await ChangingWebResourceInEntityEditor(selectedFile, connectionData, commonConfig);
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

        private async Task ChangingWebResourceInEntityEditor(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            WebResource webResource = null;

            // Репозиторий для работы с веб-ресурсами
            var webResourceRepository = new WebResourceRepository(service);

            webResource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);
            Guid? lastLinkedWebResourceId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

            if (webResource != null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource founded by name.");
            }
            else if (lastLinkedWebResourceId.HasValue)
            {
                webResource = await webResourceRepository.GetByIdAsync(lastLinkedWebResourceId.Value);

                if (webResource != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name. Last link WebResource is selected for difference.");
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name and has not Last link.");
                this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom WebResource selection form.");

                if (SelecteWebResourceInWindow(service, selectedFile, lastLinkedWebResourceId, out Guid selectedWebResourceId))
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Custom WebResource is selected.");

                    webResource = await webResourceRepository.GetByIdAsync(selectedWebResourceId);
                }
            }

            if (webResource == null)
            {
                service.TryDispose();

                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded in CRM: {0}", selectedFile.FileName);
                return;
            }

            connectionData.AddMapping(webResource.Id, selectedFile.FriendlyFilePath);
            connectionData.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, WebResource.EntityLogicalName, webResource.Id);
        }

        #endregion Changing WebResource in EntityEditor

        #region Getting WebResource Attribute

        public async Task ExecuteWebResourceGettingAttribute(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle)
        {
            string operation = string.Format(Properties.OperationNames.GettingWebResourceAttributeFormat2, connectionData?.Name, fieldTitle);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, new[] { selectedFile }, out _);

                await WebResourceGettingAttribute(selectedFile, fieldName, fieldTitle, connectionData, commonConfig);
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

        private async Task WebResourceGettingAttribute(SelectedFile selectedFile, string fieldName, string fieldTitle, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            WebResource webResource = null;

            using (service.Lock())
            {
                // Репозиторий для работы с веб-ресурсами
                var webResourceRepository = new WebResourceRepository(service);

                webResource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);
                Guid? lastLinkedWebResourceId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (webResource != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "WebResource founded by name.");
                }
                else if (lastLinkedWebResourceId.HasValue)
                {
                    webResource = await webResourceRepository.GetByIdAsync(lastLinkedWebResourceId.Value);

                    if (webResource != null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name. Last link WebResource is selected for difference.");
                    }
                }

                if (webResource == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name and has not Last link.");
                    this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom WebResource selection form.");

                    if (SelecteWebResourceInWindow(service, selectedFile, lastLinkedWebResourceId, out Guid selectedWebResourceId))
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "Custom WebResource is selected.");

                        webResource = await webResourceRepository.GetByIdAsync(selectedWebResourceId);
                    }
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded in CRM: {0}", selectedFile.FileName);
                return;
            }

            connectionData.AddMapping(webResource.Id, selectedFile.FriendlyFilePath);
            connectionData.Save();

            string xmlContent = webResource.GetAttributeValue<string>(fieldName);

            if (string.IsNullOrEmpty(xmlContent))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, WebResource.Schema.EntityLogicalName, webResource.Name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            if (string.Equals(fieldName, WebResource.Schema.Attributes.dependencyxml, StringComparison.InvariantCultureIgnoreCase))
            {
                xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                    xmlContent
                    , commonConfig
                    , XmlOptionsControls.WebResourceDependencyXmlOptions
                    , schemaName: AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlSchema
                    , webResourceName: webResource.Name
                );
            }
            else if (string.Equals(fieldName, WebResource.Schema.Attributes.contentjson, StringComparison.InvariantCultureIgnoreCase))
            {
                xmlContent = ContentComparerHelper.FormatJson(xmlContent);
            }

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string fileName = EntityFileNameFormatter.GetWebResourceFileName(connectionData.Name, webResource.Name, fieldTitle, FileExtension.xml);
            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, WebResource.Schema.EntityLogicalName, webResource.Name, fieldTitle, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            this._iWriteToOutput.PerformAction(connectionData, filePath);
        }

        #endregion Getting WebResource Attribute

        #region Getting WebResource Current Content

        public async Task ExecuteGettingContent(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles)
        {
            string operation = string.Format(Properties.OperationNames.GettingWebResourceContentFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await GettingContent(selectedFiles, connectionData, commonConfig);

                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);
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

        private async Task GettingContent(IEnumerable<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                foreach (var selectedFile in selectedFiles)
                {
                    if (!File.Exists(selectedFile.FilePath))
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                        continue;
                    }

                    // Репозиторий для работы с веб-ресурсами
                    var webResourceRepository = new WebResourceRepository(service);

                    WebResource webResource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);
                    Guid? lastLinkedWebResourceId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                    if (webResource != null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "WebResource founded by name.");
                    }
                    else if (lastLinkedWebResourceId.HasValue)
                    {
                        webResource = await webResourceRepository.GetByIdAsync(lastLinkedWebResourceId.Value);

                        if (webResource != null)
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name. Last link WebResource is selected for getting content.");
                        }
                    }

                    if (webResource == null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name and has not Last link.");
                        this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom WebResource selection form.");

                        if (SelecteWebResourceInWindow(service, selectedFile, lastLinkedWebResourceId, out Guid selectedWebResourceId))
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "Custom WebResource is selected.");

                            webResource = await webResourceRepository.GetByIdAsync(selectedWebResourceId);
                        }
                    }

                    if (webResource == null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded in CRM: {0}", selectedFile.FileName);
                        continue;
                    }

                    var webResourceContentArray = Convert.FromBase64String(webResource.Content);

                    File.WriteAllBytes(selectedFile.FilePath, webResourceContentArray);

                    connectionData.AddMapping(webResource.Id, selectedFile.FriendlyFilePath);
                    connectionData.Save();
                }
            }
        }

        #endregion Getting WebResource Current Content

        #region Очищение связей.

        internal void ExecuteClearingWebResourcesLinks(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            string operation = string.Format(Properties.OperationNames.ClearingLastLinkFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

                ClearingWebResourcesLinks(selectedFiles, connectionData);
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

        private void ClearingWebResourcesLinks(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            int count = 0;

            foreach (SelectedFile selectedFile in selectedFiles)
            {
                if (connectionData.RemoveMapping(selectedFile.FriendlyFilePath))
                {
                    count++;
                }
            }

            if (count > 0)
            {
                //Сохранение настроек после публикации
                connectionData.Save();
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.DeletedLastLinksFormat1, count);
        }

        #endregion Очищение связей.

        #region Создание связи веб-ресурсов.

        public async Task ExecuteCreatingLastLinkWebResourceMultiple(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            await CheckEncodingCheckReadOnlyConnectExecuteActionAsync(connectionData
                , Properties.OperationNames.CreatingLastLinkForWebResourcesFormat1
                , selectedFiles
                , false
                , (service) => CreatingLastLinkWebResourceMultiple(service, selectedFiles)
            );
        }

        private async Task CreatingLastLinkWebResourceMultiple(IOrganizationServiceExtented service, List<SelectedFile> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            foreach (var selectedFile in selectedFiles)
            {
                if (!File.Exists(selectedFile.FilePath))
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                    continue;
                }

                var idLastLink = service.ConnectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                bool? dialogResult = null;
                Guid? selectedWebResourceId = null;

                bool showNext = false;

                var t = new Thread((ThreadStart)(() =>
                {
                    try
                    {
                        var form = new Views.WindowWebResourceSelectOrCreate(this._iWriteToOutput, service, selectedFile, idLastLink);
                        form.ShowSkipButton();

                        dialogResult = form.ShowDialog();
                        selectedWebResourceId = form.SelectedWebResourceId;
                        showNext = form.ShowNext;
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                    }
                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (dialogResult.GetValueOrDefault())
                {
                    if (selectedWebResourceId.HasValue)
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.WebResourceIsSelected);

                        var webresource = await webResourceRepository.GetByIdAsync(selectedWebResourceId.Value);

                        service.ConnectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                        service.ConnectionData.Save();
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.WebResourceNotFoundedByNameFormat1, selectedFile.Name);
                    }
                }
                else if (!showNext)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatingLastLinkWasCanceled);
                    return;
                }
            }
        }

        #endregion Создание связи веб-ресурсов.

        #region Открытие веб-ресурсов.

        public async Task ExecuteOpeningWebResource(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, ActionOnComponent actionOnComponent)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , WebResource.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(actionOnComponent)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, new[] { selectedFile }, out _);

                await OpeningWebResource(commonConfig, connectionData, selectedFile, actionOnComponent);
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

        private async Task OpeningWebResource(CommonConfiguration commonConfig, ConnectionData connectionData, SelectedFile selectedFile, ActionOnComponent actionOnComponent)
        {
            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            // Репозиторий для работы с веб-ресурсами
            var webResourceRepository = new WebResourceRepository(service);

            WebResource webresource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);

            if (webresource != null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WebResourceFoundedByNameFormat2, webresource.Id.ToString(), webresource.Name);

                connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                connectionData.Save();
            }
            else
            {
                Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (webId.HasValue)
                {
                    webresource = await webResourceRepository.GetByIdAsync(webId.Value);
                }

                if (webresource != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Web-resource not founded by name. Last link web-resource is selected for opening.");

                    connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                    connectionData.Save();
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Web-resource not founded by name and has not Last link.");
                    this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom Web-resource selection form.");

                    bool? dialogResult = null;
                    Guid? selectedWebResourceId = null;

                    string selectedPath = string.Empty;
                    var t = new Thread((ThreadStart)(() =>
                    {
                        try
                        {
                            var form = new Views.WindowWebResourceSelectOrCreate(this._iWriteToOutput, service, selectedFile, webId);

                            dialogResult = form.ShowDialog();
                            selectedWebResourceId = form.SelectedWebResourceId;
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(connectionData, ex);
                        }
                    }));
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    t.Join();

                    if (dialogResult.GetValueOrDefault())
                    {
                        if (selectedWebResourceId.HasValue)
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "Custom Web-resource is selected.");

                            webresource = await webResourceRepository.GetByIdAsync(selectedWebResourceId.Value);

                            connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                            connectionData.Save();
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "!Warning. WebResource not exists. name: {0}.", selectedFile.Name);
                        }
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "Opening was cancelled.");
                        return;
                    }
                }
            }

            if (webresource == null)
            {
                service.TryDispose();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WebResourceNotFoundedByNameFormat1, selectedFile.FileName);
                return;
            }

            if (actionOnComponent == ActionOnComponent.OpenInWeb)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(Entities.ComponentType.WebResource, webresource.Id);
                service.TryDispose();
            }
            else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInWeb)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.WebResource, webresource.Id);
                service.TryDispose();
            }
            else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInExplorer)
            {
                WindowHelper.OpenSolutionComponentDependenciesExplorer(
                    _iWriteToOutput
                    , service
                    , null
                    , commonConfig
                    , (int)ComponentType.WebResource
                    , webresource.Id
                    , null
                );
            }
            else if (actionOnComponent == ActionOnComponent.OpenSolutionsListWithComponentInExplorer)
            {
                WindowHelper.OpenExplorerSolutionExplorer(
                    _iWriteToOutput
                    , service
                    , commonConfig
                    , (int)ComponentType.WebResource
                    , webresource.Id
                    , null
                );
            }
        }

        #endregion Открытие веб-ресурсов.

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

        #region OpenFiles

        public async Task ExecuteOpenFiles(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool inTextEditor)
        {
            await CheckEncodingConnectFindWebResourceExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningFilesFormat2
                , selectedFiles
                , openFilesType
                , (conn, service, filesToOpen) => OpenFiles(conn, service, filesToOpen, inTextEditor)
                , Helpers.EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(openFilesType)
            );
        }

        private void OpenFiles(ConnectionData connectionData, IOrganizationServiceExtented service, TupleList<SelectedFile, WebResource> filesToOpen, bool inTextEditor)
        {
            if (!filesToOpen.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "No files for open.");
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            var orderEnumrator = filesToOpen.Select(s => s.Item1).OrderBy(s => s.FriendlyFilePath);

            if (inTextEditor)
            {
                foreach (var item in orderEnumrator)
                {
                    this._iWriteToOutput.WriteToOutputFilePathUri(connectionData, item.FilePath);
                    this._iWriteToOutput.OpenFileInTextEditor(connectionData, item.FilePath);
                }
            }
            else
            {
                foreach (var item in orderEnumrator)
                {
                    this._iWriteToOutput.WriteToOutputFilePathUri(connectionData, item.FilePath);
                    this._iWriteToOutput.OpenFileInVisualStudio(connectionData, item.FilePath);
                }
            }
        }

        #endregion OpenFiles
    }
}
