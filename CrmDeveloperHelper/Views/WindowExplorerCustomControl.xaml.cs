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
    public partial class WindowExplorerCustomControl : WindowWithSolutionComponentDescriptor
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Popup _optionsPopup;

        public WindowExplorerCustomControl(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string filter
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.CustomControlXmlOptions);
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

            this.lstVwCustomControls.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingCustomControls();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection()));

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

        private async Task ShowExistingCustomControls()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingCustomControls);

            string textName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();

                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<CustomControl> list = Enumerable.Empty<CustomControl>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var repository = new CustomControlRepository(service);
                    list = await repository.GetListAsync(textName
                        , new ColumnSet
                        (
                            CustomControl.Schema.Attributes.customcontrolid
                            , CustomControl.Schema.Attributes.name
                            , CustomControl.Schema.Attributes.compatibledatatypes
                            , CustomControl.Schema.Attributes.ismanaged
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            this.lstVwCustomControls.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list
                    .OrderBy(ent => ent.Name)
                    .ThenBy(ent => ent.CompatibleDataTypes)
                )
                {
                    var item = new EntityViewItem(entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwCustomControls.Items.Count == 1)
                {
                    this.lstVwCustomControls.SelectedItem = this.lstVwCustomControls.Items[0];
                }
            });

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingCustomControlsCompletedFormat1, list.Count());
        }

        private class EntityViewItem
        {
            public string Name => CustomControl.Name;

            public string CompatibleDataTypes => CustomControl.CompatibleDataTypes;

            public bool IsManaged => CustomControl.IsManaged.GetValueOrDefault();

            public CustomControl CustomControl { get; private set; }

            public EntityViewItem(CustomControl CustomControl)
            {
                this.CustomControl = CustomControl;
            }
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
            this.lstVwCustomControls.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwCustomControls.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportCustomControl, btnExportAll };

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
                await ShowExistingCustomControls();
            }
        }

        private CustomControl GetSelectedEntity()
        {
            return this.lstVwCustomControls.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwCustomControls.SelectedItems.OfType<EntityViewItem>().Select(e => e.CustomControl).SingleOrDefault() : null;
        }

        private List<CustomControl> GetSelectedEntitiesList()
        {
            return this.lstVwCustomControls.SelectedItems.OfType<EntityViewItem>().Select(e => e.CustomControl).ToList();
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
                        service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.CustomControl, item.CustomControl.Id);
                    }
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteActionAsync(Guid idCustomControl, string name, Func<string, Guid, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idCustomControl, name);
        }

        private Task<string> CreateFileAsync(string folder, Guid idCustomControl, string name, string fieldTitle, FileExtension extension, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, idCustomControl, name, fieldTitle, extension, xmlContent));
        }

        private string CreateFile(string folder, Guid idCustomControl, string name, string fieldTitle, FileExtension extension, string xmlContent)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetCustomControlFileName(connectionData.Name, name, fieldTitle, extension);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, connectionData.Name, CustomControl.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, connectionData.Name, CustomControl.Schema.EntityLogicalName, name, fieldTitle);
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

            await ExecuteActionAsync(entity.Id, entity.Name, PerformExportAllXmlAsync);
        }

        private async Task PerformExportAllXmlAsync(string folder, Guid idCustomControl, string name)
        {
            await PerformExportEntityDescriptionAsync(folder, idCustomControl, name);

            await PerformExportXmlToFileAsync(folder, idCustomControl, name, CustomControl.Schema.Attributes.manifest, CustomControl.Schema.Headers.manifest, FileExtension.xml);

            await PerformExportXmlToFileAsync(folder, idCustomControl, name, CustomControl.Schema.Attributes.clientjson, CustomControl.Schema.Headers.clientjson, FileExtension.json);
        }

        private async Task ExecuteActionEntityAsync(Guid idCustomControl, string name, string fieldName, string fieldTitle, FileExtension extension, Func<string, Guid, string, string, string, FileExtension, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idCustomControl, name, fieldName, fieldTitle, extension);
        }

        private async Task PerformExportXmlToFileAsync(string folder, Guid idCustomControl, string name, string fieldName, string fieldTitle, FileExtension extension)
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
                var repository = new CustomControlRepository(service);

                var customControl = await repository.GetByIdAsync(idCustomControl, new ColumnSet(fieldName));

                string xmlContent = customControl.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(xmlContent))
                {
                    if (string.Equals(fieldName, CustomControl.Schema.Attributes.manifest, StringComparison.InvariantCultureIgnoreCase))
                    {
                        xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            xmlContent
                            , _commonConfig
                            , XmlOptionsControls.CustomControlXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.ManifestSchema
                            , customControlId: idCustomControl
                        );
                    }
                    else if (string.Equals(fieldName, CustomControl.Schema.Attributes.clientjson, StringComparison.InvariantCultureIgnoreCase))
                    {
                        xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                    }
                }

                string filePath = await CreateFileAsync(folder, idCustomControl, name, fieldTitle, extension, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idCustomControl, string name, string fieldName, string fieldTitle, FileExtension extension)
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
                var repository = new CustomControlRepository(service);

                var customControl = await repository.GetByIdAsync(idCustomControl, new ColumnSet(fieldName));

                string xmlContent = customControl.GetAttributeValue<string>(fieldName);

                if (string.Equals(fieldName, CustomControl.Schema.Attributes.manifest, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (ContentComparerHelper.TryParseXml(xmlContent, out var tempDoc))
                    {
                        xmlContent = tempDoc.ToString();
                    }
                }
                else if (string.Equals(fieldName, CustomControl.Schema.Attributes.clientjson, StringComparison.InvariantCultureIgnoreCase))
                {
                    xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                }

                {
                    string backUpXmlContent = xmlContent;

                    if (string.Equals(fieldName, CustomControl.Schema.Attributes.manifest, StringComparison.InvariantCultureIgnoreCase))
                    {
                        backUpXmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            backUpXmlContent
                            , _commonConfig
                            , XmlOptionsControls.CustomControlXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.ManifestSchema
                            , customControlId: idCustomControl
                        );
                    }
                    else if (string.Equals(fieldName, CustomControl.Schema.Attributes.clientjson, StringComparison.InvariantCultureIgnoreCase))
                    {
                        backUpXmlContent = ContentComparerHelper.FormatJson(backUpXmlContent);
                    }

                    await CreateFileAsync(folder, idCustomControl, name, fieldTitle + " BackUp", extension, backUpXmlContent);
                }

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
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionUpdatingFieldCanceledFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                newText = ContentComparerHelper.RemoveInTextAllCustomXmlAttributesAndNamespaces(newText);

                UpdateStatus(service.ConnectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, fieldName);

                if (!ContentComparerHelper.TryParseXmlDocument(newText, out var doc))
                {
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.TextIsNotValidXml);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }

                newText = doc.ToString(SaveOptions.DisableFormatting);

                var updateEntity = new CustomControl
                {
                    Id = idCustomControl
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

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

            await ExecuteActionAsync(entity.Id, entity.Name, PerformExportEntityDescriptionAsync);
        }

        private async void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.Name, PerformEntityEditor);
        }

        private async void mIDeleteCustomControl_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.Name, PerformDeleteEntity);
        }

        private async Task PerformExportEntityDescriptionAsync(string folder, Guid idCustomControl, string name)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetCustomControlFileName(service.ConnectionData.Name, name, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new CustomControlRepository(service);

                var customControl = await repository.GetByIdAsync(idCustomControl, ColumnSetInstances.AllColumns);

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, customControl, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityDescriptionFormat3
                    , service.ConnectionData.Name
                    , customControl.LogicalName
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

        private async Task PerformEntityEditor(string folder, Guid idCustomControl, string name)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, CustomControl.EntityLogicalName, idCustomControl);
        }

        private async Task PerformDeleteEntity(string folder, Guid idCustomControl, string name)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, CustomControl.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionDeletingEntityFormat2, service.ConnectionData.Name, CustomControl.EntityLogicalName);

            try
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, CustomControl.EntityLogicalName, idCustomControl);

                await service.DeleteAsync(CustomControl.EntityLogicalName, idCustomControl);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionDeletingEntityCompletedFormat2, service.ConnectionData.Name, CustomControl.EntityLogicalName);

            await ShowExistingCustomControls();
        }

        private async void mIExportCustomControlManifest_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.manifest, CustomControl.Schema.Headers.manifest, FileExtension.xml, PerformExportXmlToFileAsync);
        }

        private async void mIExportCustomControlClientJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.clientjson, CustomControl.Schema.Headers.clientjson, FileExtension.json, PerformExportXmlToFileAsync);
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingCustomControls();
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.CustomControl, entity.Id);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.CustomControl, entity.Id);
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

            _commonConfig.Save();

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var descriptor = GetSolutionComponentDescriptor(service);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.CustomControl, entitiesList, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
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

            if (service == null)
            {
                return;
            }

            var descriptor = GetSolutionComponentDescriptor(service);

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , service
                , descriptor
                , _commonConfig
                , (int)ComponentType.CustomControl
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

            if (service == null)
            {
                return;
            }

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.CustomControl
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
                await ShowExistingCustomControls();
            }
        }

        private async void mIUpdateCustomControlManifest_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.manifest, CustomControl.Schema.Headers.manifest, FileExtension.xml, PerformUpdateEntityField);
        }

        private async void mIUpdateCustomControlClientJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.clientjson, CustomControl.Schema.Headers.clientjson, FileExtension.json, PerformUpdateEntityField);
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

        private async void hyperlinkManifest_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.CustomControl;

            await ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.manifest, CustomControl.Schema.Headers.manifest, FileExtension.xml, PerformExportXmlToFileAsync);
        }

        private async void hyperlinkClientJson_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.CustomControl;

            await ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.clientjson, CustomControl.Schema.Headers.clientjson, FileExtension.json, PerformExportXmlToFileAsync);
        }

        private void lstVwCustomControls_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwCustomControls_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeleteCustomControl_Click(sender, e);
        }

        #region Clipboard

        private void mIClipboardCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.Name);
        }

        private void mIClipboardCopyCompatibleDataTypes_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.CompatibleDataTypes);
        }

        private void mIClipboardCopyCustomControlId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.CustomControl.Id.ToString());
        }

        #endregion Clipboard
    }
}