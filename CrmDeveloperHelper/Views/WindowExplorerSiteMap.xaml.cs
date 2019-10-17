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
    public partial class WindowExplorerSiteMap : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly Popup _optionsPopup;

        public static readonly XmlOptionsControls _xmlOptions = XmlOptionsControls.XmlFull;

        public WindowExplorerSiteMap(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            var child = new ExportXmlOptionsControl(_commonConfig, _xmlOptions);
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

            this.DecreaseInit();

            if (service != null)
            {
                ShowExistingSiteMaps();
            }
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

        private async Task<IOrganizationServiceExtented> GetService()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData == null)
            {
                return null;
            }

            if (_connectionCache.ContainsKey(connectionData.ConnectionId))
            {
                return _connectionCache[connectionData.ConnectionId];
            }

            ToggleControls(connectionData, false, string.Empty);

            _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
            _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            try
            {
                var service = await QuickConnection.ConnectAsync(connectionData);

                if (service != null)
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;

                    return service;
                }
                else
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                }
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                ToggleControls(connectionData, true, string.Empty);
            }

            return null;
        }

        private async Task ShowExistingSiteMaps()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingSiteMaps);

            this._itemsSource.Clear();

            IEnumerable<SiteMap> list = Enumerable.Empty<SiteMap>();

            try
            {
                if (service != null)
                {
                    var repository = new SitemapRepository(service);
                    list = await repository.GetListAsync(new ColumnSet(SiteMap.EntityPrimaryIdAttribute, SiteMap.Schema.Attributes.sitemapname, SiteMap.Schema.Attributes.sitemapnameunique));
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

            LoadSiteMaps(list);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingSiteMapsCompletedFormat1, list.Count());
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

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
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

        private void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingSiteMaps();
            }
        }

        private SiteMap GetSelectedEntity()
        {
            return this.lstVwSiteMaps.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwSiteMaps.SelectedItems.OfType<EntityViewItem>().Select(e => e.SiteMap).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    ExecuteAction(item.SiteMap.Id, item.SiteMap.SiteMapName, item.SiteMap.SiteMapNameUnique, PerformExportMouseDoubleClick);
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
            string folder = txtBFolder.Text.Trim();

            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await action(folder, idSiteMap, name, nameUnique);
        }

        private Task<string> CreateFileAsync(string folder, string name, string nameUnique, Guid id, string fieldTitle, string siteMapXml)
        {
            return Task.Run(() => CreateFile(folder, name, nameUnique, id, fieldTitle, siteMapXml));
        }

        private string CreateFile(string folder, string name, string nameUnique, Guid id, string fieldTitle, string siteMapXml)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetSiteMapFileName(connectionData.Name, name, id, fieldTitle, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(siteMapXml))
            {
                try
                {
                    //if (_commonConfig.SetXmlSchemasDuringExport)
                    //{
                    //    var schemasResources = AbstractDynamicCommandXsdSchemas.GetXsdSchemas(AbstractDynamicCommandXsdSchemas.SchemaSiteMapXml);

                    //    if (schemasResources != null)
                    //    {
                    //        siteMapXml = ContentCoparerHelper.SetXsdSchema(siteMapXml, schemasResources);
                    //    }
                    //}

                    //if (_commonConfig.SetIntellisenseContext)
                    //{
                    //    siteMapXml = ContentCoparerHelper.SetIntellisenseContextSiteMapNameUnique(siteMapXml, nameUnique);
                    //}

                    //if (_commonConfig.SortXmlAttributes)
                    //{
                    //    siteMapXml = ContentCoparerHelper.SortXmlAttributes(siteMapXml);
                    //}

                    //siteMapXml = ContentCoparerHelper.FormatXml(siteMapXml, _commonConfig.ExportXmlAttributeOnNewLine);

                    siteMapXml = ContentCoparerHelper.FormatXmlByConfiguration(siteMapXml, _commonConfig, _xmlOptions
                       , schemaName: AbstractDynamicCommandXsdSchemas.SchemaSiteMapXml
                       , siteMapUniqueName: nameUnique ?? string.Empty
                       );

                    File.WriteAllText(filePath, siteMapXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SiteMap.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SiteMap.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, PerformExportAllXml);
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

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await action(folder, idSiteMap, name, nameUnique, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idSiteMap, string name, string nameUnique, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new SitemapRepository(service);

                var sitemap = await repository.GetByIdAsync(idSiteMap, new ColumnSet(fieldName));

                string xmlContent = sitemap.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, name, nameUnique, idSiteMap, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idSiteMap, string name, string nameUnique, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                var repository = new SitemapRepository(service);

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
                        ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldCanceledFormat2, service.ConnectionData.Name, fieldName);
                        return;
                    }
                }

                newText = ContentCoparerHelper.RemoveAllCustomXmlAttributesAndNamespaces(newText);

                UpdateStatus(service.ConnectionData, Properties.WindowStatusStrings.ValidatingXmlForFieldFormat1, fieldName);

                if (!ContentCoparerHelper.TryParseXmlDocument(newText, out var doc))
                {
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.TextIsNotValidXml);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }

                bool validateResult = await SitemapRepository.ValidateXmlDocumentAsync(service.ConnectionData, _iWriteToOutput, doc);

                if (!validateResult)
                {
                    var dialogResult = MessageBoxResult.Cancel;

                    this.Dispatcher.Invoke(() =>
                    {
                        dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    });

                    if (dialogResult != MessageBoxResult.OK)
                    {
                        ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ValidatingXmlForFieldFailedFormat1, fieldName);
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

                UpdateStatus(service.ConnectionData, Properties.WindowStatusStrings.PublishingSiteMapFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());

                {
                    var repositoryPublish = new PublishActionsRepository(service);

                    await repositoryPublish.PublishSiteMapsAsync(new[] { idSiteMap });
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, PerformExportEntityDescription);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idSiteMap, string name, string nameUnique)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetSiteMapFileName(service.ConnectionData.Name, name, idSiteMap, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new SitemapRepository(service);

                var sitemap = await repository.GetByIdAsync(idSiteMap, new ColumnSet(true));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, sitemap, EntityFileNameFormatter.SiteMapIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , sitemap.LogicalName
                    , filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingEntityDescriptionFailed);
            }
        }

        private void mIExportSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, SiteMap.Schema.Attributes.sitemapxml, SiteMap.Schema.Headers.sitemapxml, PerformExportXmlToFile);
        }

        private void btnPublishSiteMap_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, PerformPublishSiteMap);
        }

        private async Task PerformPublishSiteMap(string folder, Guid idSiteMap, string name, string nameUnique)
        {
            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingSiteMapFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.PublishingSiteMapFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishSiteMapsAsync(new[] { idSiteMap });

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingSiteMapCompletedFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingSiteMapFailedFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingSiteMapFormat3, service.ConnectionData.Name, name, idSiteMap.ToString());
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSiteMaps();
            }

            if (!e.Handled)
            {
                if (e.Key == Key.Escape
                    || (e.Key == Key.W && e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    )
                {
                    if (_optionsPopup.IsOpen)
                    {
                        _optionsPopup.IsOpen = false;
                        e.Handled = true;
                    }
                }
            }

            base.OnKeyDown(e);
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

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

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SiteMap, new[] { entity.Id }, null, withSelect);
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

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

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

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesWindow(
                _iWriteToOutput
                , service
                , null
                , _commonConfig
                , (int)ComponentType.SiteMap
                , entity.Id
                , null
                );
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareSiteMaps_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSiteMapWindow(
                _iWriteToOutput
                , _commonConfig
                , service.ConnectionData
                , service.ConnectionData
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

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.SiteMap
                , entity.Id
                , null
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                ShowExistingSiteMaps();
            }
        }

        private void mIUpdateSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, SiteMap.Schema.Attributes.sitemapxml, SiteMap.Schema.Headers.sitemapxml, PerformUpdateEntityField);
        }

        private void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.SiteMapName, entity.SiteMapNameUnique, PerformEntityEditor);
        }

        private async Task PerformEntityEditor(string folder, Guid idSiteMap, string name, string nameUnique)
        {
            var service = await GetService();

            var repositoryPublish = new PublishActionsRepository(service);

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
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }
    }
}