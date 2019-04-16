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
    public partial class WindowExportSavedQueryVisualization : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private string _filterEntity;

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly Popup _optionsPopup;

        private readonly XmlOptionsControls _xmlOptions = XmlOptionsControls.SetXmlSchemas;

        public WindowExportSavedQueryVisualization(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntity
            , string selection
            )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._filterEntity = filterEntity;

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

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            if (string.IsNullOrEmpty(_filterEntity))
            {
                btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
                btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwCharts.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this.DecreaseInit();

            if (service != null)
            {
                ShowExistingCharts();
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

            if (connectionData != null)
            {
                if (!_connectionCache.ContainsKey(connectionData.ConnectionId))
                {
                    ToggleControls(connectionData, false, string.Empty);

                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;

                    ToggleControls(connectionData, true, string.Empty);
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingCharts()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingCharts);

            this._itemsSource.Clear();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<SavedQueryVisualization> list = Enumerable.Empty<SavedQueryVisualization>();

            try
            {
                if (service != null)
                {
                    var repository = new SavedQueryVisualizationRepository(service);
                    list = await repository.GetListAsync(this._filterEntity
                        , new ColumnSet(
                            SavedQueryVisualization.Schema.Attributes.savedqueryvisualizationid
                            , SavedQueryVisualization.Schema.Attributes.primaryentitytypecode
                            , SavedQueryVisualization.Schema.Attributes.name
                            , SavedQueryVisualization.Schema.Attributes.iscustomizable
                            ));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            list = FilterList(list, textName);

            LoadSystemCharts(list);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingChartsCompletedFormat1, list.Count());
        }

        private static IEnumerable<SavedQueryVisualization> FilterList(IEnumerable<SavedQueryVisualization> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.Id == tempGuid || ent.SavedQueryVisualizationIdUnique == tempGuid);
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        var type = ent.PrimaryEntityTypeCode.ToLower();
                        var name = ent.Name.ToLower();

                        return type.Contains(textName) || name.Contains(textName);
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string EntityName { get; private set; }

            public string ChartName { get; private set; }

            public SavedQueryVisualization SavedQueryVisualization { get; private set; }

            public EntityViewItem(string entityName, string chartName, SavedQueryVisualization savedQueryVisualization)
            {
                this.EntityName = entityName;
                this.ChartName = chartName;
                this.SavedQueryVisualization = savedQueryVisualization;
            }
        }

        private void LoadSystemCharts(IEnumerable<SavedQueryVisualization> results)
        {
            this.lstVwCharts.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(ent => ent.PrimaryEntityTypeCode)
                    .ThenBy(ent => ent.Name)
                )
                {
                    var item = new EntityViewItem(entity.PrimaryEntityTypeCode, entity.Name, entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwCharts.Items.Count == 1)
                {
                    this.lstVwCharts.SelectedItem = this.lstVwCharts.Items[0];
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
            this.lstVwCharts.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwCharts.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportChart, btnExportAll };

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
                ShowExistingCharts();
            }
        }

        private SavedQueryVisualization GetSelectedEntity()
        {
            return this.lstVwCharts.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwCharts.SelectedItems.OfType<EntityViewItem>().Select(e => e.SavedQueryVisualization).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

                if (item != null)
                {
                    var service = await GetService();

                    if (service != null)
                    {
                        service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQueryVisualization, item.SavedQueryVisualization.Id);
                    }
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idSavedQueryVisualization, string entityName, string name)
        {
            await PerformExportXmlToFile(folder, idSavedQueryVisualization, entityName, name, SavedQueryVisualization.Schema.Attributes.datadescription, SavedQueryVisualization.Schema.Headers.datadescription);

            await PerformExportXmlToFile(folder, idSavedQueryVisualization, entityName, name, SavedQueryVisualization.Schema.Attributes.presentationdescription, SavedQueryVisualization.Schema.Headers.presentationdescription);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idSavedQueryVisualization, string entityName, string name, Func<string, Guid, string, string, Task> action)
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

            await action(folder, idSavedQueryVisualization, entityName, name);
        }

        private Task<string> CreateFileAsync(string folder, string entityName, string name, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, entityName, name, fieldTitle, xmlContent));
        }

        private string CreateFile(string folder, string entityName, string name, string fieldTitle, string xmlContent)
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

            string fileName = EntityFileNameFormatter.GetSavedQueryVisualizationFileName(connectionData.Name, entityName, name, fieldTitle, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    //if (_commonConfig.SetXmlSchemasDuringExport)
                    //{
                    //    var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaVisualizationDataDescription);

                    //    if (schemasResources != null)
                    //    {
                    //        xmlContent = ContentCoparerHelper.SetXsdSchema(xmlContent, schemasResources);
                    //    }
                    //}

                    //if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    //{
                    //    xmlContent = doc.ToString();
                    //}

                    xmlContent = ContentCoparerHelper.FormatXmlByConfiguration(xmlContent, _commonConfig, _xmlOptions
                          , schemaName: CommonExportXsdSchemasCommand.SchemaVisualizationDataDescription
                          );

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SavedQueryVisualization.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SavedQueryVisualization.Schema.EntityLogicalName, name, fieldTitle);
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

            ExecuteAction(entity.Id, entity.PrimaryEntityTypeCode, entity.Name, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Guid idSavedQueryVisualization, string entityName, string name)
        {
            await PerformExportEntityDescription(folder, idSavedQueryVisualization, entityName, name);

            await PerformExportXmlToFile(folder, idSavedQueryVisualization, entityName, name, SavedQueryVisualization.Schema.Attributes.datadescription, SavedQueryVisualization.Schema.Headers.datadescription);

            await PerformExportXmlToFile(folder, idSavedQueryVisualization, entityName, name, SavedQueryVisualization.Schema.Attributes.presentationdescription, SavedQueryVisualization.Schema.Headers.presentationdescription);
        }

        private async Task ExecuteActionEntity(Guid idSavedQueryVisualization, string entityName, string name, string fieldName, string fieldTitle, Func<string, Guid, string, string, string, string, Task> action)
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

            await action(folder, idSavedQueryVisualization, entityName, name, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idSavedQueryVisualization, string entityName, string name, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new SavedQueryVisualizationRepository(service);

                var savedQueryVisualization = await repository.GetByIdAsync(idSavedQueryVisualization, new ColumnSet(fieldName));

                string xmlContent = savedQueryVisualization.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, entityName, name, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idSavedQueryVisualization, string entityName, string name, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                var repository = new SavedQueryVisualizationRepository(service);

                var savedQueryVisualization = await repository.GetByIdAsync(idSavedQueryVisualization, new ColumnSet(fieldName));

                string xmlContent = savedQueryVisualization.GetAttributeValue<string>(fieldName);

                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }
                }

                string filePath = await CreateFileAsync(folder, entityName, name, fieldTitle + " BackUp", xmlContent);

                var newText = string.Empty;
                bool? dialogResult = false;

                this.Dispatcher.Invoke(() =>
                {
                    var form = new WindowTextField("Enter " + fieldTitle, fieldTitle, xmlContent);

                    dialogResult = form.ShowDialog();

                    newText = form.FieldText;
                });

                if (dialogResult.GetValueOrDefault() == false)
                {
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                newText = ContentCoparerHelper.RemoveAllCustomXmlAttributesAndNamespaces(newText);

                {
                    if (ContentCoparerHelper.TryParseXml(newText, out var doc))
                    {
                        newText = doc.ToString(SaveOptions.DisableFormatting);
                    }
                }

                var updateEntity = new SavedQueryVisualization
                {
                    Id = idSavedQueryVisualization
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

                UpdateStatus(service.ConnectionData, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

                {
                    var repositoryPublish = new PublishActionsRepository(service);

                    await repositoryPublish.PublishEntitiesAsync(new[] { entityName });
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private void mIExportSystemChartDataDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntityTypeCode, entity.Name, SavedQueryVisualization.Schema.Attributes.datadescription, SavedQueryVisualization.Schema.Headers.datadescription, PerformExportXmlToFile);
        }

        private void mIExportSystemChartPresentationDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntityTypeCode, entity.Name, SavedQueryVisualization.Schema.Attributes.presentationdescription, SavedQueryVisualization.Schema.Headers.presentationdescription, PerformExportXmlToFile);
        }

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.PrimaryEntityTypeCode, entity.Name, PerformExportEntityDescription);
        }

        private void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.PrimaryEntityTypeCode, entity.Name, PerformEntityEditor);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idSavedQueryVisualization, string entityName, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetSavedQueryVisualizationFileName(service.ConnectionData.Name, entityName, name, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new SavedQueryVisualizationRepository(service);

                var savedQueryVisualization = await repository.GetByIdAsync(idSavedQueryVisualization, new ColumnSet(true));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, savedQueryVisualization, EntityFileNameFormatter.SavedQueryVisualizationIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , savedQueryVisualization.LogicalName
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

        private async Task PerformEntityEditor(string folder, Guid idSavedQueryVisualization, string entityName, string name)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, SavedQueryVisualization.EntityLogicalName, idSavedQueryVisualization);
        }

        private void btnClearEntityFilter_Click(object sender, RoutedEventArgs e)
        {
            this._filterEntity = null;

            btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
            btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;

            ShowExistingCharts();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingCharts();
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SavedQueryVisualization, entity.Id);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQueryVisualization, entity.Id);
            }
        }

        private async void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddIntoSolution(true, null);
        }

        private async void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddIntoSolution(bool withSelect, string solutionUniqueName)
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

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SavedQueryVisualization, new[] { entity.Id }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void mIAddEntityIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null);
        }

        private async void mIAddEntityIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddEntityIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || string.IsNullOrEmpty(entity.PrimaryEntityTypeCode)
                || string.Equals(entity.PrimaryEntityTypeCode, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                var entityMetadataId = connectionData.GetEntityMetadataId(entity.PrimaryEntityTypeCode);

                if (entityMetadataId.HasValue)
                {
                    _commonConfig.Save();

                    var service = await GetService();

                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                        await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entityMetadataId.Value }, null, withSelect);
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

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            FillLastSolutionItems(connectionData, items, true, AddIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

            ActivateControls(items, (nodeItem.SavedQueryVisualization.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");

            bool hasEntity = !string.IsNullOrEmpty(nodeItem.SavedQueryVisualization.PrimaryEntityTypeCode) && !string.Equals(nodeItem.SavedQueryVisualization.PrimaryEntityTypeCode, "none", StringComparison.InvariantCultureIgnoreCase);
            ActivateControls(items, hasEntity, "contMnEntity");
            FillLastSolutionItems(connectionData, items, hasEntity, mIAddEntityIntoCrmSolutionLast_Click, "contMnAddEntityIntoSolutionLast");
        }

        private void tSDDBExportChart_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

            ActivateControls(tSDDBExportChart.Items.OfType<Control>(), (nodeItem.SavedQueryVisualization.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void btnCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, null, entity?.PrimaryEntityTypeCode, null);
        }

        private async void btnEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode);
        }

        private async void btnEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode);
        }

        private async void btnEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode);
        }

        private async void btnEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode);
        }

        private async void miEntitySecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode);
        }

        private async void miSecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode, string.Empty);
        }

        private async void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode, string.Empty);
        }

        private async void btnWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode, string.Empty);
        }

        private async void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode, string.Empty, string.Empty);
        }

        private async void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode, string.Empty);
        }

        private async void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntityTypeCode, string.Empty);
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntityTypeCode);
        }

        private async void btnCompareApplicationRibbons_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntityTypeCode);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntityTypeCode);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntityTypeCode, entity?.Name ?? txtBFilter.Text);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntityTypeCode);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        private async void mIOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
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
                , (int)ComponentType.SavedQueryVisualization
                , entity.Id
                , null
                );
        }

        private async void mIOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
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
                , (int)ComponentType.SavedQueryVisualization
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
                ShowExistingCharts();
            }
        }

        private void mIUpdateSystemChartDataDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntityTypeCode, entity.Name, SavedQueryVisualization.Schema.Attributes.datadescription, SavedQueryVisualization.Schema.Headers.datadescription, PerformUpdateEntityField);
        }

        private void mIUpdateSystemChartPresentationDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntityTypeCode, entity.Name, SavedQueryVisualization.Schema.Attributes.presentationdescription, SavedQueryVisualization.Schema.Headers.presentationdescription, PerformUpdateEntityField);
        }

        private void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                && !string.IsNullOrEmpty(entity.PrimaryEntityTypeCode)
                && !string.Equals(entity.PrimaryEntityTypeCode, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ExecuteAction(entity.Id, entity.PrimaryEntityTypeCode, entity.Name, PerformPublishEntityAsync);
        }

        private async Task PerformPublishEntityAsync(string folder, Guid idSavedQueryVisualization, string entityName, string name)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);
        }

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || string.IsNullOrEmpty(entity.PrimaryEntityTypeCode)
                || string.Equals(entity.PrimaryEntityTypeCode, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.PrimaryEntityTypeCode);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || string.IsNullOrEmpty(entity.PrimaryEntityTypeCode)
                || string.Equals(entity.PrimaryEntityTypeCode, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(entity.PrimaryEntityTypeCode);
            }
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