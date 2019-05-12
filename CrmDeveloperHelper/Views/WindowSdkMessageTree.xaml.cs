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
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSdkMessageTree : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly ObservableCollection<PluginTreeViewItem> _messageTree = new ObservableCollection<PluginTreeViewItem>();

        private View _currentView = View.ByEntity;

        private BitmapImage _imageEntity;
        private BitmapImage _imageMessage;
        private BitmapImage _imageStage;

        private readonly CommonConfiguration _commonConfig;

        private enum View
        {
            ByEntity = 0,
            ByMessage = 1,
        }

        public WindowSdkMessageTree(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityFilter
            , string messageFilter
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput ?? throw new ArgumentNullException(nameof(iWriteToOutput));
            this._commonConfig = commonConfig ?? throw new ArgumentNullException(nameof(commonConfig));

            _connectionCache[service.ConnectionData.ConnectionId] = service ?? throw new ArgumentNullException(nameof(service));

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            LoadFromConfig();

            FillEntityNames(service.ConnectionData);

            LoadImages();

            LoadConfiguration();

            cmBEntityName.Text = entityFilter;
            txtBMessageFilter.Text = messageFilter;

            FocusOnComboBoxTextBox(cmBEntityName);

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            trVMessageTree.ItemsSource = _messageTree;

            this.DecreaseInit();

            ShowExistingMessages();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
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

        private const string paramEntityName = "EntityName";
        private const string paramMessage = "Message";

        private const string paramView = "View";

        private void LoadConfiguration()
        {
            WindowSettings winConfig = this.GetWindowsSettings();

            if (string.IsNullOrEmpty(this.cmBEntityName.Text)
                && string.IsNullOrEmpty(this.txtBMessageFilter.Text)
            )
            {
                this.cmBEntityName.Text = winConfig.GetValueString(paramEntityName);
                this.txtBMessageFilter.Text = winConfig.GetValueString(paramMessage);
            }

            {
                var temp = winConfig.GetValueString(paramView);

                if (!string.IsNullOrEmpty(temp))
                {
                    if (Enum.TryParse<View>(temp, out View tempView))
                    {
                        this._currentView = tempView;

                        this.IncreaseInit();

                        rBViewByEntity.IsChecked = rBViewByMessage.IsChecked = false;

                        switch (this._currentView)
                        {
                            case View.ByMessage:
                                rBViewByMessage.IsChecked = true;
                                break;
                            case View.ByEntity:
                            default:
                                rBViewByEntity.IsChecked = true;
                                break;
                        }

                        this.DecreaseInit();
                    }
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictString[paramEntityName] = this.cmBEntityName.Text?.Trim();
            winConfig.DictString[paramMessage] = this.txtBMessageFilter.Text.Trim();

            winConfig.DictString[paramView] = this._currentView.ToString();
        }

        private void LoadImages()
        {
            this._imageEntity = this.Resources["ImageEntity"] as BitmapImage;
            this._imageMessage = this.Resources["ImageMessage"] as BitmapImage;
            this._imageStage = this.Resources["ImageStage"] as BitmapImage;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingMessages();
            }

            base.OnKeyDown(e);
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

        private async Task ShowExistingMessages()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingSdkMessage);

            this.Dispatcher.Invoke(() =>
            {
                _messageTree.Clear();
            });

            string entityName = string.Empty;
            string messageName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                entityName = cmBEntityName.Text?.Trim();
                messageName = txtBMessageFilter.Text.Trim();
            });

            IEnumerable<SdkMessage> search = Enumerable.Empty<SdkMessage>();

            try
            {
                if (service != null)
                {
                    var repository = new SdkMessageRepository(service);

                    search = await repository.GetAllSdkMessageWithFiltersAsync(messageName, entityName);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            if (search != null)
            {
                this.trVMessageTree.Dispatcher.Invoke(() =>
                {
                    this.trVMessageTree.BeginInit();
                });

                switch (_currentView)
                {
                    case View.ByMessage:
                        FillTreeByMessage(search);
                        break;

                    case View.ByEntity:
                    default:
                        FillTreeByEntity(search);
                        break;
                }

                ExpandNodes(_messageTree);

                this.trVMessageTree.Dispatcher.Invoke(() =>
                {
                    this.trVMessageTree.EndInit();
                });
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingSdkMessageCompleted);
        }

        private void FillTreeByEntity(IEnumerable<SdkMessage> result)
        {
            var groupsByEnity = result.GroupBy(ent => ent.PrimaryObjectTypeCodeName).OrderBy(gr => gr.Key);

            foreach (var grEntity in groupsByEnity)
            {
                PluginTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);

                var groupsByMessages = grEntity.GroupBy(ent => ent.Name).OrderBy(mess => mess.Key, new MessageComparer());

                foreach (var mess in groupsByMessages)
                {
                    PluginTreeViewItem nodeMessage = CreateNodeMessage(mess.Key, mess);

                    AddTreeNode(nodeEntity, nodeMessage);
                    
                    ExpandNode(nodeMessage);
                }

                this.Dispatcher.Invoke(() =>
                {
                    _messageTree.Add(nodeEntity);
                });
            }
        }

        private void FillTreeByMessage(IEnumerable<SdkMessage> result)
        {
            var groupsByMessage = result.GroupBy(ent => ent.Name).OrderBy(mess => mess.Key, new MessageComparer());

            foreach (var grMessage in groupsByMessage)
            {
                var nodeMessage = CreateNodeMessage(grMessage.Key, grMessage);

                var groupsByEntity = grMessage.GroupBy(ent => ent.PrimaryObjectTypeCodeName).OrderBy(e => e.Key);

                foreach (var grEntity in groupsByEntity)
                {
                    PluginTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);

                    AddTreeNode(nodeMessage, nodeEntity);
                }

                this.Dispatcher.Invoke(() =>
                {
                    _messageTree.Add(nodeMessage);
                });
            }
        }

        private void AddTreeNode(PluginTreeViewItem node, PluginTreeViewItem childNode)
        {
            node.Items.Add(childNode);
            childNode.Parent = node;
        }

        private void ExpandNode(PluginTreeViewItem node)
        {
            node.IsExpanded = true;
        }

        private PluginTreeViewItem CreateNodeStage(int stage, int mode)
        {
            string name = SdkMessageProcessingStepRepository.GetStageName(stage, mode);

            var nodeStage = new PluginTreeViewItem(null)
            {
                Name = name,
                Image = _imageStage,
            };

            return nodeStage;
        }

        private PluginTreeViewItem CreateNodeMessage(string message, IEnumerable<SdkMessage> steps)
        {
            var nodeMessage = new PluginTreeViewItem(ComponentType.SdkMessage)
            {
                Name = message,
                Image = _imageMessage,

                MessageList = steps.Where(s => s.SdkMessageId.HasValue).Select(s => s.SdkMessageId.Value).Distinct().ToList(),

                MessageFilterList = steps.Where(s => s.SdkMessageFilterId.HasValue).Select(s => s.SdkMessageFilterId.Value).Distinct().ToList(),
            };

            return nodeMessage;
        }

        private PluginTreeViewItem CreateNodeEntity(string entityName, IEnumerable<SdkMessage> steps)
        {
            var nodeMessage = new PluginTreeViewItem(ComponentType.Entity)
            {
                Name = entityName,
                Image = _imageEntity,

                MessageFilterList = steps.Where(s => s.SdkMessageFilterId.HasValue).Select(s => s.SdkMessageFilterId.Value).Distinct().ToList(),
            };

            return nodeMessage;
        }

        private void ExpandNodes(ObservableCollection<PluginTreeViewItem> list)
        {
            if (list.Count == 1)
            {
                list[0].IsExpanded = true;

                if (list[0].Items.Count == 0)
                {
                    list[0].IsSelected = true;
                }
                else
                {
                    ExpandNodes(list[0].Items);
                }
            }
            else if (list.Count > 0)
            {
                list[0].IsSelected = true;
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

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, this.tSBCollapseAll, this.tSBExpandAll, this.menuView);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.trVMessageTree.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled
                                        && this.trVMessageTree.SelectedItem != null
                                        && this.trVMessageTree.SelectedItem is PluginTreeViewItem
                                        && CanCreateDescription(this.trVMessageTree.SelectedItem as PluginTreeViewItem);

                    UIElement[] list = { tSBCreateDescription };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }

                    tSBCreateDescription.Content = GetCreateDescription(this.trVMessageTree.SelectedItem as PluginTreeViewItem);
                }
                catch (Exception)
                {
                }
            });
        }

        private string GetCreateDescription(PluginTreeViewItem item)
        {
            if (item == null)
            {
                return "Create Description";
            }

            if (item.PluginAssembly.HasValue)
            {
                return "Create Plugin Assembly Description";
            }

            if (item.PluginType.HasValue)
            {
                return "Create Plugin Type Description";
            }

            if (item.MessageList != null && item.MessageList.Any())
            {
                return "Create Message Description";
            }

            if (item.MessageFilterList != null && item.MessageFilterList.Any())
            {
                return "Create Message Filter Description";
            }

            if (item.Step.HasValue)
            {
                return "Create Step Description";
            }

            if (item.StepImage.HasValue)
            {
                return "Create Image Description";
            }

            return "Create Description";
        }

        private bool CanCreateDescription(PluginTreeViewItem item)
        {
            return item.PluginAssembly.HasValue
                || item.PluginType.HasValue
                || (item.MessageList != null && item.MessageList.Any())
                || (item.MessageFilterList != null && item.MessageFilterList.Any())
                || item.Step.HasValue
                || item.StepImage.HasValue
                ;
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingMessages();
            }
        }

        private PluginTreeViewItem GetSelectedEntity()
        {
            PluginTreeViewItem result = null;

            if (this.trVMessageTree.SelectedItem != null
                && this.trVMessageTree.SelectedItem is PluginTreeViewItem
                )
            {
                result = this.trVMessageTree.SelectedItem as PluginTreeViewItem;
            }

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void trVMessageTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateButtonsEnable();
        }

        private void rBViewByEntity_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByEntity)
            {
                this._currentView = View.ByEntity;

                ShowExistingMessages();
            }
        }

        private void rBViewByMessage_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByMessage)
            {
                this._currentView = View.ByMessage;

                ShowExistingMessages();
            }
        }

        private void tSBCollapseAll_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(_messageTree, false);
        }

        private void tSBExpandAll_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(_messageTree, true);
        }

        private void mIExpandNodes_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, true);
        }

        private void mICollapseNodes_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, false);
        }

        private void ChangeExpandedInTreeViewItems(IEnumerable<PluginTreeViewItem> items, bool isExpanded)
        {
            if (items == null || !items.Any())
            {
                return;
            }

            foreach (var item in items)
            {
                item.IsExpanded = isExpanded;

                ChangeExpandedInTreeViewItems(item.Items, isExpanded);
            }
        }

        private async void tSBCreateDescription_Click(object sender, RoutedEventArgs e)
        {
            var node = GetSelectedEntity();

            if (node == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, _commonConfig.FolderForExport);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await CreateDescription(node);
        }

        private async Task CreateDescription(PluginTreeViewItem node)
        {
            if (!CanCreateDescription(node))
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingDescription);

            StringBuilder result = new StringBuilder();

            string fileName = string.Empty;

            if (node.MessageList != null && node.MessageList.Any())
            {
                var repository = new SdkMessageRepository(service);
                List<SdkMessage> listMessages = await repository.GetMessageByIdsAsync(node.MessageList.ToArray());

                fileName = EntityFileNameFormatter.GetMessageFileName(service.ConnectionData.Name, node.Name, "Description");

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                result.AppendLine(service.ConnectionData.GetConnectionInfo());

                foreach (var message in listMessages)
                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(message, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }
            }

            if (node.MessageFilterList != null && node.MessageFilterList.Any())
            {
                var repository = new SdkMessageFilterRepository(service);
                List<SdkMessageFilter> listMessages = await repository.GetMessageFiltersByIdsAsync(node.MessageFilterList.ToArray());

                fileName = EntityFileNameFormatter.GetMessageFilterFileName(service.ConnectionData.Name, node.Name, "Description");

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                result.AppendLine(service.ConnectionData.GetConnectionInfo());

                foreach (var message in listMessages)
                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(message, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, result.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingDescriptionCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1, service.ConnectionData.Name);
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConnectionData connectionData = null;

            this.Dispatcher.Invoke(() =>
            {
                trVMessageTree.ItemsSource = null;
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                FillEntityNames(connectionData);
                ShowExistingMessages();
            }
        }

        private void FillEntityNames(ConnectionData connectionData)
        {
            if (connectionData != null
                && connectionData.IntellisenseData != null
                && connectionData.IntellisenseData.Entities != null
                )
            {
                cmBEntityName.Dispatcher.Invoke(() =>
                {
                    string text = cmBEntityName.Text;

                    cmBEntityName.Items.Clear();

                    foreach (var item in connectionData.IntellisenseData.Entities.Keys.OrderBy(s => s))
                    {
                        cmBEntityName.Items.Add(item);
                    }

                    cmBEntityName.Text = text;
                });
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }
    }
}