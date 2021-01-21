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
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerSiteMap : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Popup _optionsPopup;

        public WindowExplorerSiteMap(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string filter
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.SiteMapXmlOptions);
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

            if (!string.IsNullOrEmpty(filter))
            {
                txtBFilter.Text = filter;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSiteMaps.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingSiteMaps();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getSiteMapName: GetSiteMapName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getSiteMapName: GetSiteMapName
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

        private string GetSiteMapName()
        {
            var entity = GetSelectedEntity();

            return entity?.SiteMapName ?? txtBFilter.Text.Trim();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;
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

        private async Task ShowExistingSiteMaps()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingSiteMaps);

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();
            });

            IEnumerable<SiteMap> list = Enumerable.Empty<SiteMap>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var repository = new SiteMapRepository(service);

                    list = await repository.GetListAsync(new ColumnSet(SiteMap.EntityPrimaryIdAttribute, SiteMap.Schema.Attributes.sitemapname, SiteMap.Schema.Attributes.sitemapnameunique));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadSiteMaps(list);

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingSiteMapsCompletedFormat1, list.Count());
        }

        private static IEnumerable<SiteMap> FilterList(IEnumerable<SiteMap> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.Id == tempGuid || ent.SiteMapIdUnique == tempGuid);
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        return (ent.SiteMapName ?? string.Empty).IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                                || (ent.SiteMapNameUnique ?? string.Empty).IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                                ;
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public Guid SiteMapId => SiteMap.Id;

            public string SiteMapName => SiteMap.SiteMapName;

            public string SiteMapNameUnique => SiteMap.SiteMapNameUnique;

            public SiteMap SiteMap { get; }

            public EntityViewItem(SiteMap siteMap)
            {
                this.SiteMap = siteMap;
            }
        }

        private void LoadSiteMaps(IEnumerable<SiteMap> results)
        {
            this.lstVwSiteMaps.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(ent => ent.SiteMapNameUnique)
                    .ThenBy(ent => ent.SiteMapName)
                    .ThenBy(ent => ent.Id)
                )
                {
                    var item = new EntityViewItem(entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwSiteMaps.Items.Count == 1)
                {
                    this.lstVwSiteMaps.SelectedItem = this.lstVwSiteMaps.Items[0];
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
            this.lstVwSiteMaps.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwSiteMaps.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportSiteMap, btnExportAll };

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
                await ShowExistingSiteMaps();
            }
        }

        private SiteMap GetSelectedEntity()
        {
            return this.lstVwSiteMaps.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwSiteMaps.SelectedItems.OfType<EntityViewItem>().Select(e => e.SiteMap).SingleOrDefault() : null;
        }

        private List<SiteMap> GetSelectedEntitiesList()
        {
            return this.lstVwSiteMaps.SelectedItems.OfType<EntityViewItem>().Select(e => e.SiteMap).ToList();
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
                    await ExecuteAction(item.SiteMap.Id, item.SiteMap.SiteMapName, item.SiteMap.SiteMapNameUnique, PerformExportMouseDoubleClick);
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idSiteMap, string name, string nameUnique)
        {
            var service = await GetService();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SiteMap, idSiteMap);
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idSiteMap, string name, string nameUnique, Func<string, Guid, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idSiteMap, name, nameUnique);
        }

        private Task<string> CreateFileAsync(string folder, string name, string nameUnique, Guid id, string fieldTitle, string siteMapXml)
        {
            return Task.Run(() => CreateFile(folder, name, nameUnique, id, fieldTitle, siteMapXml));
        }

        private string CreateFile(string folder, string name, string nameUnique, Guid id, string fieldTitle, string siteMapXml)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetSiteMapFileName(connectionData.Name, name, id, fieldTitle, FileExtension.xml);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(siteMapXml))
            {
                try
                {
                    siteMapXml = ContentComparerHelper.FormatXmlByConfiguration(
                        siteMapXml
                        , _commonConfig
                        , XmlOptionsControls.SiteMapXmlOptions
                        , schemaName: AbstractDynamicCommandXsdSchemas.SiteMapXmlSchema
                        , siteMapUniqueName: nameUnique ?? string.Empty
                    );

                    File.WriteAllText(filePath, siteMapXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, connectionData.Name, SiteMap.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, connectionData.Name, SiteMap.Schema.EntityLogicalName, name, fieldTitle);
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

            await ExecuteAction(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Guid idSiteMap, string name, string nameUnique)
        {
            await PerformExportEntityDescription(folder, idSiteMap, name, nameUnique);

            await PerformExportXmlToFile(folder, idSiteMap, name, nameUnique, SiteMap.Schema.Attributes.sitemapxml, SiteMap.Schema.Headers.sitemapxml);
        }

        private async Task ExecuteActionEntity(Guid idSiteMap, string name, string nameUnique, string fieldName, string fieldTitle, Func<string, Guid, string, string, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idSiteMap, name, nameUnique, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idSiteMap, string name, string nameUnique, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new SiteMapRepository(service);

                var sitemap = await repository.GetByIdAsync(idSiteMap, new ColumnSet(fieldName));

                string xmlContent = sitemap.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, name, nameUnique, idSiteMap, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idSiteMap, string name, string nameUnique, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionUpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                var repository = new SiteMapRepository(service);

                var sitemap = await repository.GetByIdAsync(idSiteMap, new ColumnSet(fieldName));

                string xmlContent = sitemap.GetAttributeValue<string>(fieldName);

                await CreateFileAsync(folder, name, nameUnique, idSiteMap, fieldTitle + " BackUp", xmlContent);

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
                        ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionUpdatingFieldCanceledFormat2, service.ConnectionData.Name, fieldName);
                        return;
                    }
                }

                newText = ContentComparerHelper.RemoveInTextAllCustomXmlAttributesAndNamespaces(newText);

                UpdateStatus(service.ConnectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, fieldName);

                if (!ContentComparerHelper.TryParseXmlDocument(newText, out var doc))
                {
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.TextIsNotValidXml);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }

                bool validateResult = await SiteMapRepository.ValidateXmlDocumentAsync(service.ConnectionData, _iWriteToOutput, doc);

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

                newText = doc.ToString(SaveOptions.DisableFormatting);

                var updateEntity = new SiteMap
                {
                    Id = idSiteMap
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

                UpdateStatus(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingSiteMapFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());

                {
                    var repositoryPublish = new PublishActionsRepository(service);

                    await repositoryPublish.PublishSiteMapsAsync(new[] { idSiteMap });
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionUpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionUpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, PerformExportEntityDescription);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idSiteMap, string name, string nameUnique)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetSiteMapFileName(service.ConnectionData.Name, name, idSiteMap, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new SiteMapRepository(service);

                var sitemap = await repository.GetByIdAsync(idSiteMap, ColumnSetInstances.AllColumns);

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, sitemap, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityDescriptionFormat3
                    , service.ConnectionData.Name
                    , sitemap.LogicalName
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

        private async void mIExportSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, SiteMap.Schema.Attributes.sitemapxml, SiteMap.Schema.Headers.sitemapxml, PerformExportXmlToFile);
        }

        private async void btnPublishSiteMap_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, PerformPublishSiteMap);
        }

        private async Task PerformPublishSiteMap(string folder, Guid idSiteMap, string name, string nameUnique)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingSiteMapFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionPublishingSiteMapFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishSiteMapsAsync(new[] { idSiteMap });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingSiteMapCompletedFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingSiteMapFailedFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingSiteMapFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingSiteMaps();
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SiteMap, entity.Id);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SiteMap, entity.Id);
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
            var entitiesList = GetSelectedEntitiesList()
                .Select(e => e.Id);

            if (!entitiesList.Any())
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SiteMap, entitiesList, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = GetSelectedConnection();

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");
            }
        }

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , service
                , null
                , _commonConfig
                , (int)ComponentType.SiteMap
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

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.SiteMap
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

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                await ShowExistingSiteMaps();
            }
        }

        private async void mIUpdateSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, SiteMap.Schema.Attributes.sitemapxml, SiteMap.Schema.Headers.sitemapxml, PerformUpdateEntityField);
        }

        private async void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, PerformEntityEditor);
        }

        private async Task PerformEntityEditor(string folder, Guid idSiteMap, string name, string nameUnique)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, SiteMap.EntityLogicalName, idSiteMap);
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

        #region Clipboard

        private void mIClipboardCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.SiteMapName);
        }

        private void mIClipboardCopyNameUnique_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.SiteMapNameUnique);
        }

        private void mIClipboardCopySiteMapId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.SiteMap.Id.ToString());
        }

        #endregion Clipboard
    }
}