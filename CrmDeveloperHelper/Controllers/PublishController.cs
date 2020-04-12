using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                , Properties.OperationNames.OpeningEntityMetadataExplorerFormat1
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
            string operation = string.Format(Properties.OperationNames.UpdatingContentWebResourcesEqualByTextAndPublishingFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

                await UpdatingContentAndPublishEqualByText(connectionData, selectedFiles);
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

        /// <summary>
        /// Запуск публикации веб-ресурсов. 
        /// Определяет ид веб-ресурсов
        /// 1. в файле привязок
        /// 2. по имени веб-ресурса
        /// 3. ручное связывание
        /// </summary>
        private async Task UpdatingContentAndPublishEqualByText(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            var compareResult = await CompareController.GetWebResourcesWithType(this._iWriteToOutput, selectedFiles, OpenFilesType.EqualByText, connectionData);

            if (compareResult == null || compareResult.Item1 == null)
            {
                return;
            }

            var filesToPublish = compareResult.Item2.Where(f => f.Item2 != null);

            if (!filesToPublish.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NothingToPublish);
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
    }
}