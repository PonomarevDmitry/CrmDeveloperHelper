using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
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
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerSdkMessage : WindowWithSolutionComponentDescriptor
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        public WindowExplorerSdkMessage(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string filterMessage
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            LoadFromConfig();

            if (!string.IsNullOrEmpty(filterMessage))
            {
                txtBFilter.Text = filterMessage;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwMessages.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingMessages();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getMessageName: GetMessageName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            mIOpenMessageFilterExplorer.Click += explorersHelper.miMessageFilterExplorer_Click;

            mIOpenPluginTree.Click += explorersHelper.miPluginTree_Click;
            mIOpenMessageFilterTree.Click += explorersHelper.miMessageFilterTree_Click;
            mIOpenMessageRequestTree.Click += explorersHelper.miMessageRequestTree_Click;

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu listContextMenu
            )
            {
                explorersHelper.FillExplorers(listContextMenu, nameof(miExplorers));

                compareWindowsHelper.FillCompareWindows(listContextMenu, nameof(miCompareOrganizations));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageFilterExplorer_Click, nameof(mIOpenMessageFilterExplorer));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miPluginTree_Click, nameof(mIOpenPluginTree));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageFilterTree_Click, nameof(mIOpenMessageFilterTree));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageRequestTree_Click, nameof(mIOpenMessageRequestTree));
            }
        }

        private string GetMessageName()
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

        private async Task ShowExistingMessages()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingSdkMessage);

            this._itemsSource.Clear();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<SdkMessage> list = Enumerable.Empty<SdkMessage>();

            try
            {
                if (service != null)
                {
                    var repository = new SdkMessageRepository(service);

                    list = await repository.GetMessagesAsync(textName
                        , new ColumnSet
                        (
                            SdkMessage.Schema.Attributes.name
                            , SdkMessage.Schema.Attributes.categoryname
                            , SdkMessage.Schema.Attributes.autotransact
                            , SdkMessage.Schema.Attributes.availability
                            , SdkMessage.Schema.Attributes.customizationlevel
                            , SdkMessage.Schema.Attributes.ismanaged
                            , SdkMessage.Schema.Attributes.expand
                            , SdkMessage.Schema.Attributes.isactive
                            , SdkMessage.Schema.Attributes.isprivate
                            , SdkMessage.Schema.Attributes.isreadonly
                            , SdkMessage.Schema.Attributes.isvalidforexecuteasync
                            , SdkMessage.Schema.Attributes.workflowsdkstepenabled
                            , SdkMessage.Schema.Attributes.throttlesettings
                            , SdkMessage.Schema.Attributes.template
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadSdkMessages(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingSdkMessageCompletedFormat1, list.Count());
        }

        private class EntityViewItem
        {
            public string Name => SdkMessage.Name;

            public string CategoryName => SdkMessage.CategoryName;

            public bool AutoTransact => SdkMessage.AutoTransact.GetValueOrDefault();

            public int? Availability => SdkMessage.Availability;

            public Entities.GlobalOptionSets.AvailabilityEnum? AvailabilityEnum => SdkMessage.AvailabilityEnum;

            public int? CustomizationLevel => SdkMessage.CustomizationLevel;

            public bool IsManaged => SdkMessage.IsManaged.GetValueOrDefault();

            public bool Expand => SdkMessage.Expand.GetValueOrDefault();

            public bool IsActive => SdkMessage.IsActive.GetValueOrDefault();

            public bool IsPrivate => SdkMessage.IsPrivate.GetValueOrDefault();

            public bool IsReadOnly => SdkMessage.IsReadOnly.GetValueOrDefault();

            public bool IsValidForExecuteAsync => SdkMessage.IsValidForExecuteAsync.GetValueOrDefault();

            public bool WorkflowSdkStepEnabled => SdkMessage.WorkflowSdkStepEnabled.GetValueOrDefault();

            public bool Template => SdkMessage.Template.GetValueOrDefault();

            public string ThrottleSettings => SdkMessage.ThrottleSettings;

            public bool HasThrottleSettings => !string.IsNullOrEmpty(SdkMessage.ThrottleSettings);

            public SdkMessage SdkMessage { get; }

            public EntityViewItem(SdkMessage entity)
            {
                this.SdkMessage = entity;
            }
        }

        private void LoadSdkMessages(IEnumerable<SdkMessage> results)
        {
            this.lstVwMessages.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(ent => ent.CategoryName, MessageComparer.Comparer)
                    .ThenBy(ent => ent.Name, MessageComparer.Comparer)
                )
                {
                    var item = new EntityViewItem(entity);

                    _itemsSource.Add(item);
                }

                if (this.lstVwMessages.Items.Count == 1)
                {
                    this.lstVwMessages.SelectedItem = this.lstVwMessages.Items[0];
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
            this.lstVwMessages.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwMessages.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportMessage };

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
                await ShowExistingMessages();
            }
        }

        private SdkMessage GetSelectedEntity()
        {
            return this.lstVwMessages.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwMessages.SelectedItems.OfType<EntityViewItem>().Select(e => e.SdkMessage).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void lstVwMessages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    await ExecuteAction(item.SdkMessage.Id, item.SdkMessage.Name, PerformExportMouseDoubleClick);
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idSdkMessage, string name)
        {
            await PerformExportEntityDescription(folder, idSdkMessage, name);
        }

        private void lstVwMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idAssembly, string name, Func<string, Guid, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            try
            {
                await action(folder, idAssembly, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.Name, PerformExportEntityDescription);
        }

        private async void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.Name, PerformEntityEditor);
        }

        private async void mIDeleteMessage_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.Name, PerformDeleteEntity);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idSdkMessage, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            var repository = new SdkMessageRepository(service);

            var message = await repository.GetByIdAsync(idSdkMessage, new ColumnSet(true));

            string fileName = EntityFileNameFormatter.GetMessageFileName(service.ConnectionData.Name, name, EntityFileNameFormatter.Headers.EntityDescription);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, message, service.ConnectionData);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                , service.ConnectionData.Name
                , message.LogicalName
                , filePath
            );

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
        }

        private async Task PerformEntityEditor(string folder, Guid idSdkMessage, string name)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, SdkMessage.EntityLogicalName, idSdkMessage);
        }

        private async Task PerformDeleteEntity(string folder, Guid idSdkMessage, string name)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, SdkMessage.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DeletingEntityFormat2, service.ConnectionData.Name, SdkMessage.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, SdkMessage.EntityLogicalName, idSdkMessage);

                    await service.DeleteAsync(SdkMessage.EntityLogicalName, idSdkMessage);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DeletingEntityCompletedFormat2, service.ConnectionData.Name, SdkMessage.EntityLogicalName);

                await ShowExistingMessages();
            }
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingMessages();
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SdkMessage, entity.Id);
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
            var descriptor = GetSolutionComponentDescriptor(service);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.SdkMessage, new[] { entity.Id }, null, withSelect);
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

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , service
                , null
                , _commonConfig
                , (int)ComponentType.SdkMessage
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
                , (int)ComponentType.SdkMessage
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
                await ShowExistingMessages();
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private void lstVwMessages_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwMessages_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeleteMessage_Click(sender, e);
        }

        private async void mIExportThrottleSettingsXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.Name, SdkMessage.Schema.Attributes.throttlesettings, nameof(SdkMessage.ThrottleSettings), FileExtension.xml, PerformExportXmlToFile);
        }

        private async Task ExecuteActionEntity(Guid idMessage, string name, string fieldName, string fieldTitle, FileExtension extension, Func<string, Guid, string, string, string, FileExtension, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idMessage, name, fieldName, fieldTitle, extension);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idMessage, string name, string fieldName, string fieldTitle, FileExtension extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new SdkMessageRepository(service);

                var messageEntity = await repository.GetByIdAsync(idMessage, new ColumnSet(fieldName));

                string xmlContent = messageEntity.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, idMessage, name, fieldTitle, extension, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private Task<string> CreateFileAsync(string folder, Guid idMessage, string name, string fieldTitle, FileExtension extension, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, idMessage, name, fieldTitle, extension, xmlContent));
        }

        private string CreateFile(string folder, Guid idMessage, string name, string fieldTitle, FileExtension extension, string xmlContent)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(xmlContent))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SdkMessage.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);

                return null;
            }

            string fileName = EntityFileNameFormatter.GetMessageFileName(connectionData.Name, name, fieldTitle, extension);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                    xmlContent
                    , _commonConfig
                    , XmlOptionsControls.None
                );

                File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SdkMessage.Schema.EntityLogicalName, name, fieldTitle, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            return filePath;
        }

        #region Clipboard

        private void mIClipboardCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.Name);
        }

        private void mIClipboardCopyCategoryName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.CategoryName);
        }

        private void mIClipboardCopyMessageId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.SdkMessage.Id.ToString());
        }

        #endregion Clipboard
    }
}