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
    public class PublishController
    {
        private IWriteToOutput _iWriteToOutput = null;

        /// <summary>
        /// Конструктор контроллера для публикации
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public PublishController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Обновление веб-ресурсов и публикация.

        /// <summary>
        /// Запуск публикации ресурсов
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public async Task ExecuteUpdateContentAndPublish(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingContentAndPublishingFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OperationNames.CheckingFilesEncoding);

                    CheckController.CheckingFilesEncoding(this._iWriteToOutput, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                }

                await UpdateContentAndPublish(selectedFiles, connectionData);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task UpdateContentAndPublish(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }
            
            if (connectionData.IsReadOnly)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

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
                        this._iWriteToOutput.WriteToOutput("Try to find web-resource by name: {0}. Searching...", selectedFile.Name);

                        string key = selectedFile.FriendlyFilePath.ToLower();

                        var contentFile = Convert.ToBase64String(File.ReadAllBytes(selectedFile.FilePath));

                        var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, key, gr.Key);

                        if (webresource != null)
                        {
                            var webName = webresource.Name;

                            this._iWriteToOutput.WriteToOutput("WebResource founded by name. WebResourceId: {0} Name: {1}", webresource.Id, webName);
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput("WebResource not founded by name. FileName: {0}. Open linking dialog...", selectedFile.Name);

                            Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                            bool? dialogResult = null;
                            Guid? selectedWebResourceId = null;

                            var t = new Thread(() =>
                            {
                                try
                                {
                                    var form = new Views.WindowWebResourceSelectOrCreate(this._iWriteToOutput, service, connectionData, selectedFile, webId);
                                    form.ShowCreateButton(allForOther);

                                    dialogResult = form.ShowDialog();

                                    allForOther = form.ForAllOther;

                                    selectedWebResourceId = form.SelectedWebResourceId;
                                }
                                catch (Exception ex)
                                {
                                    DTEHelper.WriteExceptionToOutput(ex);
                                }
                            });
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();

                            t.Join();

                            if (string.IsNullOrEmpty(connectionData?.LastSelectedSolutionsUniqueName?.FirstOrDefault()))
                            {
                                allForOther = false;
                            }

                            connectionData.ConnectionConfiguration.Save();

                            if (dialogResult.GetValueOrDefault())
                            {
                                if (selectedWebResourceId.HasValue)
                                {
                                    webresource = await webResourceRepository.FindByIdAsync(selectedWebResourceId.Value);
                                }
                                else
                                {
                                    this._iWriteToOutput.WriteToOutput("!Warning. WebResource not linked. name: {0}.", selectedFile.Name);
                                }
                            }
                            else
                            {
                                this._iWriteToOutput.WriteToOutput("Updating Content and Publishing was cancelled.");
                                return;
                            }
                        }

                        if (webresource != null)
                        {
                            // Запоминается файл
                            connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                            publishHelper.Add(new ElementForPublish(selectedFile, webresource));
                        }
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput("File not founded: {0}", selectedFile.FilePath);
                    }
                }
            }

            //Сохранение настроек после публикации
            connectionData.ConnectionConfiguration.Save();

            publishHelper.UpdateContentAndPublish();
        }

        #endregion Обновление веб-ресурсов и публикация.

        #region Публикация веб-ресурсов разных по содержанию, но одинаковых по тексту.

        /// <summary>
        /// Запуск публикации ресурсов
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public async Task ExecuteUpdateContentAndPublishEqualByText(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingContentWebResourcesEqualByTextAndPublishingFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OperationNames.CheckingFilesEncoding);

                    CheckController.CheckingFilesEncoding(this._iWriteToOutput, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                }

                await UpdateContentAndPublishEqualByText(selectedFiles, connectionData);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        /// <summary>
        /// Запуск публикации веб-ресурсов. 
        /// Определяет ид веб-ресурсов
        /// 1. в файле привязок
        /// 2. по имени веб-ресурса
        /// 3. ручное связывание
        /// </summary>
        private async Task UpdateContentAndPublishEqualByText(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            var compareResult = await CompareController.GetWebResourcesWithType(this._iWriteToOutput, selectedFiles, OpenFilesType.EqualByText, connectionData);

            var filesToPublish = compareResult.Item2.Where(f => f.Item2 != null);            

            if (!filesToPublish.Any())
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NothingToPublish);
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
            string operation = string.Format(Properties.OperationNames.PublishingAllCustomizationFormat1, connectionData.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await PublishingAllAsync(connectionData);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task PublishingAllAsync(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishAllXmlAsync();

                _iWriteToOutput.WriteToOutput(Properties.OutputStrings.PublishingAllCompletedFormat1, connectionData.Name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                _iWriteToOutput.WriteToOutput(Properties.OutputStrings.PublishingAllFailedFormat1, connectionData.Name);
            }
        }

        #endregion Публикация всего.
    }
}