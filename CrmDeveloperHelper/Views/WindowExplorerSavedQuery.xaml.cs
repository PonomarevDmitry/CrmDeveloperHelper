using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerSavedQuery : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Popup _optionsPopup;

        public WindowExplorerSavedQuery(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string filterEntity
            , string selection
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            LoadEntityNames(cmBEntityName, service.ConnectionData);

            cmBStatusCode.ItemsSource = new EnumBindingSourceExtension(typeof(SavedQuery.Schema.OptionSets.statuscode?)).ProvideValue(null) as IEnumerable;
            cmBStatusCode.SelectedItem = SavedQuery.Schema.OptionSets.statuscode.Active_0_Active_1;

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.SavedQueryXmlOptions);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            LoadFromConfig();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            cmBEntityName.Text = filterEntity;

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSavedQueries.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingSavedQueries();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getEntityName: GetEntityName
                , getSavedQueryName: GetSavedQueryName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getEntityName: GetEntityName
                , getSavedQueryName: GetSavedQueryName
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu listContextMenu
            )
            {
                explorersHelper.FillExplorers(listContextMenu, nameof(miExplorers));

                compareWindowsHelper.FillCompareWindows(listContextMenu, nameof(miCompareOrganizations));
            }
        }

        private string GetEntityName()
        {
            var entity = GetSelectedEntity();

            return entity?.ReturnedTypeCode;
        }

        private string GetSavedQueryName()
        {
            var entity = GetSelectedEntity();

            return entity?.Name ?? txtBFilter.Text.Trim();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
        }

        private ConnectionData GetSelectedConnection()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private Task<IOrganizationServiceExtented> GetService()
        {
            return GetOrganizationService(GetSelectedConnection());
        }

        private async Task ShowExistingSavedQueries()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingSavedQueries);

            this._itemsSource.Clear();

            string entityName = string.Empty;
            SavedQuery.Schema.OptionSets.statuscode? statuscode = null;

            this.Dispatcher.Invoke(() =>
            {
                if (!string.IsNullOrEmpty(cmBEntityName.Text)
                    && cmBEntityName.Items.Contains(cmBEntityName.Text)
                )
                {
                    entityName = cmBEntityName.Text.Trim().ToLower();
                }

                if (cmBStatusCode.SelectedItem is SavedQuery.Schema.OptionSets.statuscode comboBoxItem)
                {
                    statuscode = comboBoxItem;
                }
            });

            string filterEntity = null;

            if (service.ConnectionData.IsValidEntityName(entityName))
            {
                filterEntity = entityName;
            }

            IEnumerable<SavedQuery> list = Enumerable.Empty<SavedQuery>();

            try
            {
                if (service != null)
                {
                    var repository = new SavedQueryRepository(service);

                    list = await repository.GetListAsync(filterEntity
                        , statuscode
                        , new ColumnSet
                        (
                            SavedQuery.Schema.Attributes.name
                            , SavedQuery.Schema.Attributes.returnedtypecode
                            , SavedQuery.Schema.Attributes.querytype
                            , SavedQuery.Schema.Attributes.iscustomizable
                            , SavedQuery.Schema.Attributes.statuscode
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadSavedQueries(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingSavedQueriesCompletedFormat1, list.Count());
        }

        private static IEnumerable<SavedQuery> FilterList(IEnumerable<SavedQuery> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.Id == tempGuid || ent.SavedQueryIdUnique == tempGuid);
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        return ent.ReturnedTypeCode.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1 || ent.Name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1;
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string ReturnedTypeCode => SavedQuery.ReturnedTypeCode;

            public string Name => SavedQuery.Name;

            public string QueryType { get; }

            public string StatusCode { get; }

            public SavedQuery SavedQuery { get; }

            public EntityViewItem(SavedQuery savedQuery)
            {
                savedQuery.FormattedValues.TryGetValue(SavedQuery.Schema.Attributes.statuscode, out var statuscode);

                this.StatusCode = statuscode;
                this.QueryType = SavedQueryRepository.GetQueryTypeName(savedQuery.QueryType.GetValueOrDefault());

                this.SavedQuery = savedQuery;
            }
        }

        private void LoadSavedQueries(IEnumerable<SavedQuery> results)
        {
            this.lstVwSavedQueries.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(ent => ent.ReturnedTypeCode)
                    .ThenBy(ent => ent.QueryType)
                    .ThenBy(ent => ent.Name)
                )
                {
                    var item = new EntityViewItem(entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwSavedQueries.Items.Count == 1)
                {
                    this.lstVwSavedQueries.SelectedItem = this.lstVwSavedQueries.Items[0];
                }
            });
        }

        private void UpdateStatus(ConnectionData connectionData, string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(connectionData, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwSavedQueries.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwSavedQueries.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportSavedQuery, btnExportAll };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private async void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingSavedQueries();
            }
        }

        private async void cmBStatusCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ShowExistingSavedQueries();
        }

        private SavedQuery GetSelectedEntity()
        {
            return this.lstVwSavedQueries.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwSavedQueries.SelectedItems.OfType<EntityViewItem>().Select(e => e.SavedQuery).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    var service = await GetService();

                    if (service != null)
                    {
                        service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQuery, item.SavedQuery.Id);
                    }
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idSavedQuery, string entityName, string name, Func<string, Guid, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idSavedQuery, entityName, name);
        }

        private Task<string> CreateFileAsync(string folder, Guid savedQueryId, string entityName, string name, string fieldTitle, FileExtension extension, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, savedQueryId, entityName, name, fieldTitle, extension, xmlContent));
        }

        private string CreateFile(string folder, Guid savedQueryId, string entityName, string name, string fieldTitle, FileExtension extension, string xmlContent)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetSavedQueryFileName(connectionData.Name, entityName, name, fieldTitle, extension);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (extension == FileExtension.xml)
                    {
                        xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            xmlContent
                            , _commonConfig
                            , XmlOptionsControls.SavedQueryXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.FetchSchema
                            , savedQueryId: savedQueryId
                            , entityName: entityName
                        );
                    }
                    else if (extension == FileExtension.json)
                    {
                        xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                    }

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SavedQuery.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SavedQuery.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private async void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.ReturnedTypeCode, entity.Name, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Guid idSavedQuery, string entityName, string name)
        {
            await PerformExportEntityDescription(folder, idSavedQuery, entityName, name);

            await PerformExportXmlToFile(folder, idSavedQuery, entityName, name, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, FileExtension.xml);

            await PerformExportXmlToFile(folder, idSavedQuery, entityName, name, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, FileExtension.xml);

            await PerformExportXmlToFile(folder, idSavedQuery, entityName, name, SavedQuery.Schema.Attributes.columnsetxml, SavedQuery.Schema.Headers.columnsetxml, FileExtension.xml);

            await PerformExportXmlToFile(folder, idSavedQuery, entityName, name, SavedQuery.Schema.Attributes.layoutjson, SavedQuery.Schema.Headers.layoutjson, FileExtension.json);

            await PerformExportXmlToFile(folder, idSavedQuery, entityName, name, SavedQuery.Schema.Attributes.offlinesqlquery, SavedQuery.Schema.Headers.offlinesqlquery, FileExtension.sql);
        }

        private async Task ExecuteActionEntity(Guid idSavedQuery, string entityName, string name, string fieldName, string fieldTitle, FileExtension extension, Func<string, Guid, string, string, string, string, FileExtension, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idSavedQuery, entityName, name, fieldName, fieldTitle, extension);
        }

        private async Task ExecuteActionEntity(Guid idSavedQuery, string entityName, string name, string fieldName, string fieldTitle, string variableName, FileExtension extension, Func<string, Guid, string, string, string, string, string, FileExtension, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idSavedQuery, entityName, name, fieldName, fieldTitle, variableName, extension);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idSavedQuery, string entityName, string name, string fieldName, string fieldTitle, FileExtension extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new SavedQueryRepository(service);

                var savedQuery = await repository.GetByIdAsync(idSavedQuery, new ColumnSet(fieldName));

                string xmlContent = savedQuery.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, idSavedQuery, entityName, name, fieldTitle, extension, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformCopyToClipboardXmlToFileJavaScript(string folder, Guid idSavedQuery, string entityName, string name, string fieldName, string fieldTitle, string variableName, FileExtension extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CopingXmlFieldToClipboardFormat1, fieldTitle);

            try
            {
                var repository = new SavedQueryRepository(service);

                var savedQuery = await repository.GetByIdAsync(idSavedQuery, new ColumnSet(fieldName));

                string xmlContent = savedQuery.GetAttributeValue<string>(fieldName);

                if (extension == FileExtension.xml)
                {
                    if (ContentComparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();

                        xmlContent = ContentComparerHelper.FormatToJavaScript(variableName, xmlContent);
                    }
                }
                else if (extension == FileExtension.json)
                {
                    xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                }

                ClipboardHelper.SetText(xmlContent);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CopingXmlFieldToClipboardCompletedFormat1, fieldTitle);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CopingXmlFieldToClipboardFailedFormat1, fieldTitle);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idSavedQuery, string entityName, string name, string fieldName, string fieldTitle, FileExtension extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                var repository = new SavedQueryRepository(service);

                var savedQuery = await repository.GetByIdAsync(idSavedQuery, new ColumnSet(fieldName, SavedQuery.Schema.Attributes.querytype));

                string xmlContent = savedQuery.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, idSavedQuery, entityName, name, fieldTitle + " BackUp", extension, xmlContent);

                var newText = string.Empty;
                {
                    bool? dialogResult = false;

                    this.Dispatcher.Invoke(() =>
                    {
                        var form = new WindowTextField("Enter " + fieldTitle, fieldTitle, xmlContent);

                        dialogResult = form.ShowDialog();

                        newText = form.FieldText;
                    });

                    if (dialogResult.GetValueOrDefault() == false)
                    {
                        ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldCanceledFormat2, service.ConnectionData.Name, fieldName);
                        return;
                    }
                }

                newText = ContentComparerHelper.RemoveInTextAllCustomXmlAttributesAndNamespaces(newText);

                UpdateStatus(service.ConnectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, fieldTitle);

                if (!ContentComparerHelper.TryParseXmlDocument(newText, out var doc))
                {
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.TextIsNotValidXml);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }

                bool validateResult = await SavedQueryRepository.ValidateXmlDocumentAsync(service.ConnectionData, _iWriteToOutput, doc, fieldTitle);

                if (!validateResult)
                {
                    var dialogResult = MessageBoxResult.Cancel;

                    this.Dispatcher.Invoke(() =>
                    {
                        dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    });

                    if (dialogResult != MessageBoxResult.OK)
                    {
                        ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ValidatingXmlForFieldFailedFormat1, fieldName);
                        _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                        return;
                    }
                }

                if (string.Equals(fieldName, SavedQuery.Schema.Attributes.fetchxml, StringComparison.InvariantCulture))
                {
                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.ExecutingValidateSavedQueryRequest);

                    var request = new ValidateSavedQueryRequest()
                    {
                        FetchXml = newText,
                        QueryType = savedQuery.QueryType.GetValueOrDefault()
                    };

                    service.Execute(request);
                }

                var updateEntity = new SavedQuery
                {
                    Id = idSavedQuery
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

                UpdateStatus(service.ConnectionData, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

                {
                    var repositoryPublish = new PublishActionsRepository(service);

                    await repositoryPublish.PublishEntitiesAsync(new[] { entityName });
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private async void mIExportSavedQueryFetchXmlAndLayoutXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, FileExtension.xml, PerformExportXmlToFile);

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, FileExtension.xml, PerformExportXmlToFile);
        }

        private async void mIExportSavedQueryFetchXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, FileExtension.xml, PerformExportXmlToFile);
        }

        private async void mIExportSavedQueryFetchXmlIntoJavaScript_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, SavedQuery.Schema.Variables.fetchxml, FileExtension.xml, PerformCopyToClipboardXmlToFileJavaScript);
        }

        private async void mIExportSavedQueryLayoutXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, FileExtension.xml, PerformExportXmlToFile);
        }

        private async void mIExportSavedQueryLayoutXmlIntoJavaScript_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, SavedQuery.Schema.Variables.layoutxml, FileExtension.xml, PerformCopyToClipboardXmlToFileJavaScript);
        }

        private async void mIExportSavedQueryColumnSetXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.columnsetxml, SavedQuery.Schema.Headers.columnsetxml, FileExtension.xml, PerformExportXmlToFile);
        }

        private async void mIExportSavedQueryColumnSetXmlIntoJavaScript_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.columnsetxml, SavedQuery.Schema.Headers.columnsetxml, SavedQuery.Schema.Variables.columnsetxml, FileExtension.xml, PerformCopyToClipboardXmlToFileJavaScript);
        }

        private async void mIExportSavedQueryLayoutJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.layoutjson, SavedQuery.Schema.Headers.layoutjson, FileExtension.json, PerformExportXmlToFile);
        }

        private async void mICopyToClipboardLayoutJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.layoutjson, SavedQuery.Schema.Headers.layoutjson, string.Empty, FileExtension.json, PerformCopyToClipboardXmlToFileJavaScript);
        }

        private async void mIExportSavedQueryOfflineSqlQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.offlinesqlquery, SavedQuery.Schema.Headers.offlinesqlquery, FileExtension.sql, PerformExportXmlToFile);
        }

        private async void mICopyToClipboardOfflineSqlQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.offlinesqlquery, SavedQuery.Schema.Headers.offlinesqlquery, string.Empty, FileExtension.sql, PerformCopyToClipboardXmlToFileJavaScript);
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.ReturnedTypeCode, entity.Name, PerformExportEntityDescription);
        }

        private async void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.ReturnedTypeCode, entity.Name, PerformEntityEditor);
        }

        private async void mIChangeStateSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.ReturnedTypeCode, entity.Name, PerformChangeStateSavedQuery);
        }

        private async void mIDeleteSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.ReturnedTypeCode, entity.Name, PerformDeleteEntity);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idSavedQuery, string entityName, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetSavedQueryFileName(service.ConnectionData.Name, entityName, name, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new SavedQueryRepository(service);

                var savedQuery = await repository.GetByIdAsync(idSavedQuery, new ColumnSet(true));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, savedQuery, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , savedQuery.LogicalName
                    , filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionFailed);
            }
        }

        private async Task PerformEntityEditor(string folder, Guid idSavedQuery, string entityName, string name)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, SavedQuery.EntityLogicalName, idSavedQuery);
        }

        private async Task PerformChangeStateSavedQuery(string folder, Guid idSavedQuery, string entityName, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ChangingEntityStateFormat1, SavedQuery.EntityLogicalName);

            var repository = new SavedQueryRepository(service);

            var savedQuery = await repository.GetByIdAsync(idSavedQuery, new ColumnSet(true));

            int state = savedQuery.StatusCodeEnum == SavedQuery.Schema.OptionSets.statuscode.Active_0_Active_1 ? (int)SavedQuery.Schema.OptionSets.statecode.Inactive_1 : (int)SavedQuery.Schema.OptionSets.statecode.Active_0;
            int status = savedQuery.StatusCodeEnum == SavedQuery.Schema.OptionSets.statuscode.Active_0_Active_1 ? (int)SavedQuery.Schema.OptionSets.statuscode.Inactive_1_Inactive_2 : (int)SavedQuery.Schema.OptionSets.statuscode.Active_0_Active_1;

            try
            {
                await service.ExecuteAsync<SetStateResponse>(new SetStateRequest()
                {
                    EntityMoniker = new EntityReference(SavedQuery.EntityLogicalName, idSavedQuery),
                    State = new OptionSetValue(state),
                    Status = new OptionSetValue(status),
                });

                var repositoryPublish = new PublishActionsRepository(service);

                await repositoryPublish.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ChangingEntityStateCompletedFormat1, SavedQuery.EntityLogicalName);
            }
            catch (Exception ex)
            {
                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ChangingEntityStateFailedFormat1, SavedQuery.EntityLogicalName);

                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            await ShowExistingSavedQueries();
        }

        private async Task PerformDeleteEntity(string folder, Guid idSavedQuery, string entityName, string name)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, SavedQuery.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DeletingEntityFormat2, service.ConnectionData.Name, SavedQuery.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, SavedQuery.EntityLogicalName, idSavedQuery);

                    await service.DeleteAsync(SavedQuery.EntityLogicalName, idSavedQuery);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DeletingEntityCompletedFormat2, service.ConnectionData.Name, SavedQuery.EntityLogicalName);

                await ShowExistingSavedQueries();
            }
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingSavedQueries();
        }

        protected override bool CanCloseWindow(KeyEventArgs e)
        {
            Popup[] _popupArray = new Popup[] { _optionsPopup };

            foreach (var popup in _popupArray)
            {
                if (popup.IsOpen)
                {
                    popup.IsOpen = false;
                    e.Handled = true;

                    return false;
                }
            }

            return true;
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SavedQuery, entity.Id);
            }
        }

        private async void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQuery, entity.Id);
            }
        }

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ReturnedTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.ReturnedTypeCode);
            }
        }

        private void mIOpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ReturnedTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                this._iWriteToOutput.OpenFetchXmlFile(connectionData, _commonConfig, entity.ReturnedTypeCode);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ReturnedTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(entity.ReturnedTypeCode);
            }
        }

        private void mIOpenSavedQueryInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ReturnedTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(entity.ReturnedTypeCode, entity.Id);
            }
        }

        private async void AddToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null);
        }

        private async void AddToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddToSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SavedQuery, new[] { entity.Id }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void AddToCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void AddToCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
        }

        private async void AddToCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
        }

        private async void AddToCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
            }
        }

        private async void AddToCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
            }
        }

        private async void AddToCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
            }
        }

        private async Task AddEntityToSolution(bool withSelect, string solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ReturnedTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                var entityMetadataId = connectionData.GetEntityMetadataId(entity.ReturnedTypeCode);

                if (entityMetadataId.HasValue)
                {
                    _commonConfig.Save();

                    var service = await GetService();

                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                        await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entityMetadataId.Value }, rootComponentBehavior, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            ConnectionData connectionData = GetSelectedConnection();

            FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");

            EntityViewItem nodeItem = GetItemFromRoutedDataContext<EntityViewItem>(e);

            ActivateControls(items, (nodeItem.SavedQuery.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");

            bool hasEntity = nodeItem.SavedQuery.ReturnedTypeCode.IsValidEntityName();
            ActivateControls(items, hasEntity, "contMnEntity");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastIncludeSubcomponents");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastDoNotIncludeSubcomponents");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddEntityToSolutionLastIncludeAsShellOnly");

            ActivateControls(items, hasEntity && connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddEntityToSolutionLast");

            SetControlsName(items, GetChangeStateName(nodeItem.SavedQuery), "contMnChangeState");
        }

        private string GetChangeStateName(SavedQuery savedQuery)
        {
            if (savedQuery == null)
            {
                return "ChangeState";
            }

            return savedQuery.StatusCodeEnum == SavedQuery.Schema.OptionSets.statuscode.Active_0_Active_1 ? "Deactivate SavedQuery" : "Activate SavedQuery";
        }

        private void tSDDBExportSavedQuery_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            var savedQuery = GetSelectedEntity();

            ActivateControls(tSDDBExportSavedQuery.Items.OfType<Control>(), (savedQuery?.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");

            SetControlsName(tSDDBExportSavedQuery.Items.OfType<Control>(), GetChangeStateName(savedQuery), "contMnChangeState");
        }

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , service
                , null
                , _commonConfig
                , (int)ComponentType.SavedQuery
                , entity.Id
                , null
                );
        }

        private async void mIOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.SavedQuery
                , entity.Id
                , null
            );
        }

        private async void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (!this.IsControlsEnabled)
            {
                return;
            }

            var connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                LoadEntityNames(cmBEntityName, connectionData);

                await ShowExistingSavedQueries();
            }
        }

        private async void mIUpdateSavedQueryFetchXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, FileExtension.xml, PerformUpdateEntityField);
        }

        private async void mIUpdateSavedQueryLayoutXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, FileExtension.xml, PerformUpdateEntityField);
        }

        private async void mIUpdateSavedQueryColumnSetXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.columnsetxml, SavedQuery.Schema.Headers.columnsetxml, FileExtension.xml, PerformUpdateEntityField);
        }

        private async void mIUpdateSavedQueryLayoutJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.layoutjson, SavedQuery.Schema.Headers.layoutjson, FileExtension.json, PerformUpdateEntityField);
        }

        private async void mIUpdateSavedQueryOfflineSqlQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.offlinesqlquery, SavedQuery.Schema.Headers.offlinesqlquery, FileExtension.sql, PerformUpdateEntityField);
        }

        private async void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ReturnedTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.ReturnedTypeCode, entity.Name, PerformPublishEntityAsync);
        }

        private async Task PerformPublishEntityAsync(string folder, Guid idSavedQuery, string entityName, string name)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);
        }

        private void miOptions_Click(object sender, RoutedEventArgs e)
        {
            this._optionsPopup.IsOpen = true;
            this._optionsPopup.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_optionsPopup.IsOpen)
            {
                _optionsPopup.IsOpen = false;
                this.Focus();
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private async void hyperlinkFetchXmlAndLayoutXml_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SavedQuery;

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, FileExtension.xml, PerformExportXmlToFile);

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, FileExtension.xml, PerformExportXmlToFile);
        }

        private async void hyperlinkFetchXml_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SavedQuery;

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, FileExtension.xml, PerformExportXmlToFile);
        }

        private async void hyperlinkLayoutXml_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SavedQuery;

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, FileExtension.xml, PerformExportXmlToFile);
        }

        private async void hyperlinkColumnSetXml_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SavedQuery;

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.columnsetxml, SavedQuery.Schema.Headers.columnsetxml, FileExtension.xml, PerformExportXmlToFile);
        }

        private async void hyperlinkLayoutJson_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SavedQuery;

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.layoutjson, SavedQuery.Schema.Headers.layoutjson, FileExtension.json, PerformExportXmlToFile);
        }

        private async void hyperlinkOfflineSqlQuery_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SavedQuery;

            await ExecuteActionEntity(entity.Id, entity.ReturnedTypeCode, entity.Name, SavedQuery.Schema.Attributes.offlinesqlquery, SavedQuery.Schema.Headers.offlinesqlquery, FileExtension.sql, PerformExportXmlToFile);
        }

        private async void hyperlinkPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SavedQuery;

            if (entity == null
                || !entity.ReturnedTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.ReturnedTypeCode, entity.Name, PerformPublishEntityAsync);
        }

        private void lstVwSavedQueries_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwSavedQueries_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeleteSavedQuery_Click(sender, e);
        }

        #region Clipboard

        private void mIClipboardCopyEntityName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.ReturnedTypeCode);
        }

        private void mIClipboardCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.Name);
        }

        private void mIClipboardCopyQueryTypeCode_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.SavedQuery.QueryType.ToString());
        }

        private void mIClipboardCopyQueryTypeName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.QueryType);
        }

        private void mIClipboardCopySavedQueryId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.SavedQuery.Id.ToString());
        }

        #endregion Clipboard
    }
}