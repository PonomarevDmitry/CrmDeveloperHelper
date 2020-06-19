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
            // Менеджер для публикации в CRM.
            PublishManager publishHelper = new PublishManager(this._iWriteToOutput, service);

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            bool allForOther = false;

            var groups = selectedFiles.GroupBy(sel => sel.Extension);

            foreach (var gr in groups)
            {
                var names = gr.Select(sel => sel.FriendlyFilePath).ToArray();

                var dict = webResourceRepository.FindMultiple(gr.Key, names);

                foreach (var selectedFile in gr)
                {
                    if (File.Exists(selectedFile.FilePath))
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Try to find web-resource by name: {0}. Searching...", selectedFile.Name);

                        string key = selectedFile.FriendlyFilePath.ToLower();

                        var contentFile = Convert.ToBase64String(File.ReadAllBytes(selectedFile.FilePath));

                        var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, key, gr.Key);

                        if (webresource != null)
                        {
                            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "WebResource founded by name. WebResourceId: {0} Name: {1}", webresource.Id, webresource.Name);
                        }

                        if (webresource == null)
                        {
                            if (selectedFile.FileName.StartsWith(service.ConnectionData.Name + "."))
                            {
                                string newFileName = selectedFile.FileName.Replace(service.ConnectionData.Name + ".", string.Empty);

                                string newFilePath = Path.Combine(Path.GetDirectoryName(selectedFile.FilePath), newFileName);

                                var newSelectedFile = new SelectedFile(newFilePath, selectedFile.SolutionDirectoryPath);

                                var newDict = webResourceRepository.FindMultiple(newSelectedFile.Extension, new[] { newSelectedFile.FriendlyFilePath });

                                webresource = WebResourceRepository.FindWebResourceInDictionary(newDict, newSelectedFile.FriendlyFilePath.ToLower(), newSelectedFile.Extension);

                                if (webresource != null)
                                {
                                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "WebResource founded by name with Connection Prefix. WebResourceId: {0} Name: {1}", webresource.Id, webresource.Name);
                                }
                            }
                        }

                        if (webresource == null)
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
                                    webresource = await webResourceRepository.GetByIdAsync(selectedWebResourceId.Value);
                                }
                                else
                                {
                                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "!Warning. WebResource not linked. name: {0}.", selectedFile.Name);
                                }
                            }
                            else
                            {
                                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Updating Content and Publishing was cancelled.");
                                return;
                            }
                        }

                        if (webresource != null)
                        {
                            // Запоминается файл
                            service.ConnectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                            publishHelper.Add(new ElementForPublish(selectedFile, webresource));
                        }
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "File not founded: {0}", selectedFile.FilePath);
                    }
                }
            }

            //Сохранение настроек после публикации
            service.ConnectionData.Save();

            publishHelper.UpdateContentAndPublish();
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
            await CheckEncodingConnectFindWebResourceExecuteActionAsync(connectionData
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
        private void UpdatingContentAndPublishEqualByText(Tuple<IOrganizationServiceExtented, TupleList<SelectedFile, WebResource>> compareResult)
        {
            IOrganizationServiceExtented service = compareResult.Item1;

            var filesToPublish = compareResult.Item2.Where(f => f.Item2 != null);

            if (!filesToPublish.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            // Менеджер для публикации в CRM.
            PublishManager publishHelper = new PublishManager(this._iWriteToOutput, compareResult.Item1);

            foreach (var item in filesToPublish)
            {
                publishHelper.Add(new ElementForPublish(item.Item1, item.Item2));
            }

            publishHelper.UpdateContentAndPublish();
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
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.PublishingAllFailedFormat1, service.ConnectionData.Name);
            }
        }

        #endregion Публикация всего.

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
            TupleList<SelectedFile, WebResource> list = new TupleList<SelectedFile, WebResource>();

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

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

                    var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, key, gr.Key);

                    if (webresource != null)
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "WebResource founded by name. WebResourceId: {0} Name: {1}", webresource.Id, webresource.Name);
                    }

                    if (webresource == null)
                    {
                        if (selectedFile.FileName.StartsWith(service.ConnectionData.Name + "."))
                        {
                            string newFileName = selectedFile.FileName.Replace(service.ConnectionData.Name + ".", string.Empty);

                            string newFilePath = Path.Combine(Path.GetDirectoryName(selectedFile.FilePath), newFileName);

                            var newSelectedFile = new SelectedFile(newFilePath, selectedFile.SolutionDirectoryPath);

                            var newDict = webResourceRepository.FindMultiple(newSelectedFile.Extension, new[] { newSelectedFile.FriendlyFilePath });

                            webresource = WebResourceRepository.FindWebResourceInDictionary(newDict, newSelectedFile.FriendlyFilePath.ToLower(), newSelectedFile.Extension);

                            if (webresource != null)
                            {
                                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "WebResource founded by name with Connection Prefix. WebResourceId: {0} Name: {1}", webresource.Id, webresource.Name);
                            }
                        }
                    }

                    if (webresource == null)
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
                                webresource = await webResourceRepository.GetByIdAsync(selectedWebResourceId.Value);
                            }
                            else
                            {
                                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "!Warning. WebResource not linked. name: {0}.", selectedFile.Name);
                            }
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Updating Content and Publishing was cancelled.");
                            return;
                        }
                    }

                    if (webresource != null)
                    {
                        // Запоминается файл
                        service.ConnectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                        list.Add(selectedFile, webresource);
                    }
                }
            }

            //Сохранение настроек после публикации
            service.ConnectionData.Save();

            if (!list.Any())
            {
                return;
            }

            Dictionary<string, WebResource> knownWebResources = new Dictionary<string, WebResource>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var tuple in list)
            {
                knownWebResources.Add(tuple.Item1.FilePath, tuple.Item2);
            }

            var webResourceToPublish = new TupleList<string, WebResource, string, int, string>();

            foreach (var tuple in list)
            {
                var selectedFile = tuple.Item1;
                var webResource = tuple.Item2;

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
                                , schemaName: Commands.AbstractDynamicCommandXsdSchemas.SchemaDependencyXml
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

                var newDependencyXmlBuilder = new StringBuilder();

                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                using (var writer = new StringWriter(newDependencyXmlBuilder))
                {
                    serializer.Serialize(writer, webResourceDependencies, namespaces);
                }

                string newDependencyXml = newDependencyXmlBuilder.ToString();

                if (ContentComparerHelper.TryParseXml(newDependencyXml, out XElement doc))
                {
                    ContentComparerHelper.ClearRoot(doc);

                    newDependencyXml = doc.ToString(SaveOptions.DisableFormatting);
                }

                webResourceToPublish.Add(selectedFile.FilePath, webResource, newDependencyXml, newDependencies.Count, string.Join(", ", newDependencies.OrderBy(s => s)));
            }

            if (!webResourceToPublish.Any())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NothingToPublish);
                return;
            }

            {
                FormatTextTableHandler tableUpdated = new FormatTextTableHandler();
                tableUpdated.SetHeader("FileName", "Dependencies", "WebResourceName", "WebResourceType", "New Dependencies");

                foreach (var tuple in webResourceToPublish.OrderBy(e => e.Item1))
                {
                    var filePath = tuple.Item1;
                    var webResource = tuple.Item2;

                    webResource.FormattedValues.TryGetValue(WebResource.Schema.Attributes.webresourcetype, out var webresourcetype);
                    tableUpdated.AddLine(filePath, tuple.Item4.ToString(), webResource.Name, webresourcetype, tuple.Item5);
                }

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Updating WebResources DependencyXml and Publishing...");

                tableUpdated.GetFormatedLines(false).ForEach(item => _iWriteToOutput.WriteToOutput(service.ConnectionData, "{0}{1}", _tabSpacer, item));
            }

            foreach (var tuple in webResourceToPublish.OrderBy(e => e.Item1))
            {
                var newDependencyXml = tuple.Item3;
                var webResource = tuple.Item2;

                var updateWebResource = new WebResource()
                {
                    Id = webResource.Id,
                    WebResourceId = webResource.Id,
                    DependencyXml = newDependencyXml,
                };

                await service.UpdateAsync(updateWebResource);
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Update WebResources Completed.");
            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Publishing WebResources...");

            var repository = new PublishActionsRepository(service);
            await repository.PublishWebResourcesAsync(webResourceToPublish.Select(e => e.Item2.Id));

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Published web-resources: {0}", webResourceToPublish.Count);
        }

        #endregion Including References to WebResources DependencyXml

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

        private static Regex _regexReference = new Regex(@"^[\/]{2,}[\s]+<reference[\s]+path=\""(?<path>.+)\""[\s]+\/>[\s]*\r?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
            Dictionary<string, WebResource> knownWebResources = new Dictionary<string, WebResource>(StringComparer.InvariantCultureIgnoreCase);

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
                                    , schemaName: Commands.AbstractDynamicCommandXsdSchemas.SchemaFormXml
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