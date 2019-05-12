using Microsoft.Xrm.Sdk.Query;
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
    public partial class WindowSdkMessageRequestTree : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly ObservableCollection<SdkMessageRequestTreeViewItem> _messageTree = new ObservableCollection<SdkMessageRequestTreeViewItem>();

        private View _currentView = View.ByEntityMessageNamespace;

        private BitmapImage _imageEntity;
        private BitmapImage _imageMessage;
        private BitmapImage _imageField;
        private BitmapImage _imageRequest;
        private BitmapImage _imageResponse;
        private BitmapImage _imageNamespace;

        private readonly CommonConfiguration _commonConfig;

        private enum View
        {
            ByEntityMessageNamespace = 0,
            ByMessageEntityNamespace = 1,
            ByNamespaceMessageEntity = 2,
            ByNamespaceEntityMessage = 3,
        }

        public WindowSdkMessageRequestTree(
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

            LoadImages();

            LoadConfiguration();

            cmBEntityName.Text = entityFilter;
            txtBMessageFilter.Text = messageFilter;

            FocusOnComboBoxTextBox(cmBEntityName);

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            trVSdkMessageRequestTree.ItemsSource = _messageTree;

            this.DecreaseInit();

            ShowExistingSdkMessageRequests();
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

                        rBViewByEntityMessageNamespace.IsChecked
                            = rBViewByNamespaceEntityMessage.IsChecked
                            = rBViewByNamespaceMessageEntity.IsChecked
                            = rBViewByMessageEntityNamespace.IsChecked = false;

                        switch (this._currentView)
                        {
                            case View.ByNamespaceEntityMessage:
                                rBViewByNamespaceEntityMessage.IsChecked = true;
                                break;

                            case View.ByNamespaceMessageEntity:
                                rBViewByNamespaceMessageEntity.IsChecked = true;
                                break;

                            case View.ByMessageEntityNamespace:
                                rBViewByMessageEntityNamespace.IsChecked = true;
                                break;

                            case View.ByEntityMessageNamespace:
                            default:
                                rBViewByEntityMessageNamespace.IsChecked = true;
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
            this._imageField = this.Resources["ImageField"] as BitmapImage;
            this._imageRequest = this.Resources["ImageRequest"] as BitmapImage;
            this._imageResponse = this.Resources["ImageResponse"] as BitmapImage;
            this._imageNamespace = this.Resources["ImageNamespace"] as BitmapImage;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSdkMessageRequests();
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

        private async Task ShowExistingSdkMessageRequests()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingSdkMessageRequests);

            this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
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

            SdkMessageSearchResult search = new SdkMessageSearchResult();

            try
            {
                search.Requests = await new SdkMessageRequestRepository(service).GetListAsync(entityName, messageName, new ColumnSet(SdkMessageRequest.Schema.Attributes.name, SdkMessageRequest.Schema.Attributes.primaryobjecttypecode));
                search.RequestFields = await new SdkMessageRequestFieldRepository(service).GetListAsync(new ColumnSet(SdkMessageRequestField.Schema.Attributes.name, SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid, SdkMessageRequestField.Schema.Attributes.clrparser, SdkMessageRequestField.Schema.Attributes.position));

                search.Responses = await new SdkMessageResponseRepository(service).GetListAsync(new ColumnSet(SdkMessageResponse.Schema.Attributes.sdkmessagerequestid));
                search.ResponseFields = await new SdkMessageResponseFieldRepository(service).GetListAsync(new ColumnSet(SdkMessageResponseField.Schema.Attributes.name, SdkMessageResponseField.Schema.Attributes.sdkmessageresponseid, SdkMessageResponseField.Schema.Attributes.clrformatter, SdkMessageResponseField.Schema.Attributes.position));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            if (search != null)
            {
                this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
                {
                    this.trVSdkMessageRequestTree.BeginInit();
                });

                switch (_currentView)
                {
                    case View.ByNamespaceEntityMessage:
                        FillTreeByNamespaceEntityMessage(search);
                        break;

                    case View.ByNamespaceMessageEntity:
                        FillTreeByNamespaceMessageEntity(search);
                        break;

                    case View.ByMessageEntityNamespace:
                        FillTreeByMessageEntityNamespace(search);
                        break;

                    case View.ByEntityMessageNamespace:
                    default:
                        FillTreeByEntityMessageNamespace(search);
                        break;
                }

                ExpandNodes(_messageTree);

                this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
                {
                    this.trVSdkMessageRequestTree.EndInit();
                });
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingSdkMessageRequestsCompleted);
        }

        private void FillTreeByEntityMessageNamespace(SdkMessageSearchResult search)
        {
            var groupsByEnity = search.Requests.GroupBy(ent => ent.PrimaryObjectTypeCode ?? "none").OrderBy(gr => gr.Key);

            foreach (var grEntity in groupsByEnity)
            {
                SdkMessageRequestTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);

                var groupsByMessages = grEntity.GroupBy(ent => ent.SdkMessageName).OrderBy(mess => mess.Key, new MessageComparer());

                foreach (var mess in groupsByMessages)
                {
                    SdkMessageRequestTreeViewItem nodeMessage = CreateNodeMessage(mess.Key, mess);
                    AddTreeNode(nodeEntity, nodeMessage);

                    var groupsByNamespace = mess.GroupBy(ent => ent.Namespace).OrderBy(e => e.Key);

                    foreach (var grNamespace in groupsByNamespace)
                    {
                        SdkMessageRequestTreeViewItem nodeNamespace = CreateNodeNamespace(grNamespace.Key);
                        AddTreeNode(nodeMessage, nodeNamespace);

                        FillRequests(search, nodeNamespace, grNamespace);

                        ExpandNode(nodeNamespace);
                    }

                    ExpandNode(nodeMessage);
                }

                this.Dispatcher.Invoke(() =>
                {
                    _messageTree.Add(nodeEntity);
                });
            }
        }

        private void FillTreeByMessageEntityNamespace(SdkMessageSearchResult search)
        {
            var groupsByMessage = search.Requests.GroupBy(ent => ent.SdkMessageName).OrderBy(mess => mess.Key, new MessageComparer());

            foreach (var grMessage in groupsByMessage)
            {
                var nodeMessage = CreateNodeMessage(grMessage.Key, grMessage);

                var groupsByEntity = grMessage.GroupBy(ent => ent.PrimaryObjectTypeCode ?? "none").OrderBy(e => e.Key);

                foreach (var grEntity in groupsByEntity)
                {
                    SdkMessageRequestTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);
                    AddTreeNode(nodeMessage, nodeEntity);

                    var groupsByNamespace = grEntity.GroupBy(ent => ent.Namespace).OrderBy(e => e.Key);

                    foreach (var grNamespace in groupsByNamespace)
                    {
                        SdkMessageRequestTreeViewItem nodeNamespace = CreateNodeNamespace(grNamespace.Key);
                        AddTreeNode(nodeEntity, nodeNamespace);

                        FillRequests(search, nodeNamespace, grNamespace);

                        ExpandNode(nodeNamespace);
                    }
                }

                this.Dispatcher.Invoke(() =>
                {
                    _messageTree.Add(nodeMessage);
                });
            }
        }

        private void FillTreeByNamespaceEntityMessage(SdkMessageSearchResult search)
        {
            var groupsByNamespace = search.Requests.GroupBy(ent => ent.Namespace).OrderBy(e => e.Key);

            foreach (var grNamespace in groupsByNamespace)
            {
                SdkMessageRequestTreeViewItem nodeNamespace = CreateNodeNamespace(grNamespace.Key);

                var groupsByEntity = grNamespace.GroupBy(ent => ent.PrimaryObjectTypeCode ?? "none").OrderBy(e => e.Key);

                foreach (var grEntity in groupsByEntity)
                {
                    SdkMessageRequestTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);
                    AddTreeNode(nodeNamespace, nodeEntity);

                    var groupsByMessage = grEntity.GroupBy(ent => ent.SdkMessageName).OrderBy(mess => mess.Key, new MessageComparer());

                    foreach (var grMessage in groupsByMessage)
                    {
                        var nodeMessage = CreateNodeMessage(grMessage.Key, grMessage);
                        AddTreeNode(nodeEntity, nodeMessage);

                        FillRequests(search, nodeMessage, grMessage);

                        ExpandNode(nodeMessage);
                    }

                    ExpandNode(nodeEntity);
                }

                this.Dispatcher.Invoke(() =>
                {
                    _messageTree.Add(nodeNamespace);
                });
            }
        }

        private void FillTreeByNamespaceMessageEntity(SdkMessageSearchResult search)
        {
            var groupsByNamespace = search.Requests.GroupBy(ent => ent.Namespace).OrderBy(e => e.Key);

            foreach (var grNamespace in groupsByNamespace)
            {
                SdkMessageRequestTreeViewItem nodeNamespace = CreateNodeNamespace(grNamespace.Key);

                var groupsByMessage = grNamespace.GroupBy(ent => ent.SdkMessageName).OrderBy(mess => mess.Key, new MessageComparer());

                foreach (var grMessage in groupsByMessage)
                {
                    var nodeMessage = CreateNodeMessage(grMessage.Key, grMessage);
                    AddTreeNode(nodeNamespace, nodeMessage);

                    var groupsByEntity = grMessage.GroupBy(ent => ent.PrimaryObjectTypeCode ?? "none").OrderBy(e => e.Key);

                    foreach (var grEntity in groupsByEntity)
                    {
                        SdkMessageRequestTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);
                        AddTreeNode(nodeMessage, nodeEntity);

                        FillRequests(search, nodeEntity, grEntity);

                        ExpandNode(nodeEntity);
                    }

                    ExpandNode(nodeMessage);
                }

                this.Dispatcher.Invoke(() =>
                {
                    _messageTree.Add(nodeNamespace);
                });
            }
        }

        private void FillRequests(SdkMessageSearchResult search, SdkMessageRequestTreeViewItem nodeParent, IEnumerable<SdkMessageRequest> requests)
        {
            foreach (var sdkRequest in requests.OrderBy(s => s.Name))
            {
                SdkMessageRequestTreeViewItem nodeRequest = CreateNodeRequest(sdkRequest);
                AddTreeNode(nodeParent, nodeRequest);

                {
                    var fields = search.RequestFields.Where(f => f.SdkMessageRequestId?.Id == sdkRequest.SdkMessageRequestId).OrderBy(f => f.Position).ThenBy(f => f.Name);

                    foreach (var field in fields)
                    {
                        SdkMessageRequestTreeViewItem nodeRequestField = CreateNodeRequestField(field);
                        AddTreeNode(nodeRequest, nodeRequestField);
                    }
                }

                var responses = search.Responses.Where(r => r.SdkMessageRequestId?.Id == sdkRequest.SdkMessageRequestId);

                foreach (var response in responses)
                {
                    SdkMessageRequestTreeViewItem nodeRespose = CreateNodeResponse(sdkRequest.Name + " - Response", response);
                    AddTreeNode(nodeParent, nodeRespose);

                    var fields = search.ResponseFields.Where(f => f.SdkMessageResponseId?.Id == response.SdkMessageResponseId).OrderBy(f => f.Position).ThenBy(f => f.Name);

                    foreach (var field in fields)
                    {
                        SdkMessageRequestTreeViewItem nodeRequestField = CreateNodeResponseField(field);
                        AddTreeNode(nodeRespose, nodeRequestField);
                    }

                    ExpandNode(nodeRespose);
                }

                ExpandNode(nodeRequest);
            }
        }

        private void AddTreeNode(SdkMessageRequestTreeViewItem node, SdkMessageRequestTreeViewItem childNode)
        {
            node.Items.Add(childNode);
        }

        private void ExpandNode(SdkMessageRequestTreeViewItem node)
        {
            node.IsExpanded = true;
        }

        private SdkMessageRequestTreeViewItem CreateNodeMessage(string message, IEnumerable<SdkMessageRequest> steps)
        {
            var nodeMessage = new SdkMessageRequestTreeViewItem()
            {
                Name = message,
                Image = _imageMessage,

                Message = steps.Where(s => s.SdkMessageId != null).Select(s => s.SdkMessageId.Value).Distinct().ToList(),
            };

            return nodeMessage;
        }

        private SdkMessageRequestTreeViewItem CreateNodeEntity(string entityName, IEnumerable<SdkMessageRequest> steps)
        {
            var node = new SdkMessageRequestTreeViewItem()
            {
                Name = entityName,
                Image = _imageEntity,
            };

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeNamespace(string name)
        {
            var node = new SdkMessageRequestTreeViewItem()
            {
                Name = name,
                Image = _imageNamespace,
            };

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeResponseField(SdkMessageResponseField field)
        {
            var node = new SdkMessageRequestTreeViewItem()
            {
                Name = string.Format("{0}. {1}      ({2})", field.Position, field.Name, field.ClrFormatter),
                Image = _imageField,
                SdkMessageResponse = field.SdkMessageResponseId?.Id
            };

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeResponse(string name, SdkMessageResponse response)
        {
            var node = new SdkMessageRequestTreeViewItem()
            {
                Name = name,
                Image = _imageResponse,
                SdkMessageResponse = response.SdkMessageResponseId
            };

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeRequestField(SdkMessageRequestField field)
        {
            var node = new SdkMessageRequestTreeViewItem()
            {
                Name = string.Format("{0}. {1}      ({2})", field.Position, field.Name, field.ClrParser),
                Image = _imageField,
                SdkMessageRequest = field.SdkMessageRequestId?.Id
            };

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeRequest(SdkMessageRequest sdkRequest)
        {
            var node = new SdkMessageRequestTreeViewItem()
            {
                Name = sdkRequest.Name + " - Request",
                Image = _imageRequest,
                SdkMessageRequest = sdkRequest.SdkMessageRequestId
            };

            return node;
        }

        private void ExpandNodes(ObservableCollection<SdkMessageRequestTreeViewItem> list)
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
            this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled
                                        && this.trVSdkMessageRequestTree.SelectedItem != null
                                        && this.trVSdkMessageRequestTree.SelectedItem is SdkMessageRequestTreeViewItem
                                        && CanCreateDescription(this.trVSdkMessageRequestTree.SelectedItem as SdkMessageRequestTreeViewItem);

                    UIElement[] list = { tSBCreateDescription };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }

                    tSBCreateDescription.Content = GetCreateDescriptionName(this.trVSdkMessageRequestTree.SelectedItem as SdkMessageRequestTreeViewItem);
                }
                catch (Exception)
                {
                }
            });
        }

        private string GetCreateDescriptionName(SdkMessageRequestTreeViewItem item)
        {
            if (item == null)
            {
                return "Create Description";
            }

            if (item.Message != null && item.Message.Any())
            {
                return "Create Message Description";
            }

            if (item.SdkMessageRequest.HasValue)
            {
                return "Create SdkMessageRequest Description";
            }

            if (item.SdkMessageResponse.HasValue)
            {
                return "Create SdkMessageResponse Description";
            }

            return "Create Description";
        }

        private bool CanCreateDescription(SdkMessageRequestTreeViewItem item)
        {
            return
                (item.Message != null && item.Message.Any())
                || item.SdkMessageRequest.HasValue
                || item.SdkMessageResponse.HasValue
                ;
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingSdkMessageRequests();
            }
        }

        private SdkMessageRequestTreeViewItem GetSelectedEntity()
        {
            SdkMessageRequestTreeViewItem result = null;

            if (this.trVSdkMessageRequestTree.SelectedItem != null
                && this.trVSdkMessageRequestTree.SelectedItem is SdkMessageRequestTreeViewItem
                )
            {
                result = this.trVSdkMessageRequestTree.SelectedItem as SdkMessageRequestTreeViewItem;
            }

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void trVSdkMessageRequestTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateButtonsEnable();
        }

        private void rBViewByEntityMessageNamespace_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByEntityMessageNamespace)
            {
                this._currentView = View.ByEntityMessageNamespace;

                ShowExistingSdkMessageRequests();
            }
        }

        private void rBViewByMessageEntityNamespace_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByMessageEntityNamespace)
            {
                this._currentView = View.ByMessageEntityNamespace;

                ShowExistingSdkMessageRequests();
            }
        }

        private void rBViewByNamespaceEntityMessage_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByNamespaceEntityMessage)
            {
                this._currentView = View.ByNamespaceEntityMessage;

                ShowExistingSdkMessageRequests();
            }
        }

        private void rBViewByNamespaceMessageEntity_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByNamespaceMessageEntity)
            {
                this._currentView = View.ByNamespaceMessageEntity;

                ShowExistingSdkMessageRequests();
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
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, true);
        }

        private void mICollapseNodes_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, false);
        }

        private void ChangeExpandedInTreeViewItems(IEnumerable<SdkMessageRequestTreeViewItem> items, bool isExpanded)
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

        private async Task CreateDescription(SdkMessageRequestTreeViewItem node)
        {
            if (!CanCreateDescription(node))
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1, service.ConnectionData.Name);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingDescription);

            StringBuilder result = new StringBuilder();

            string fileName = string.Empty;

            if (node.SdkMessageRequest.HasValue)
            {
                var repository = new SdkMessageRequestRepository(service);
                var request = await repository.GetByIdAsync(node.SdkMessageRequest.Value, new ColumnSet(true));

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetSdkMessageRequestFileName(service.ConnectionData.Name, node.Name, "Description");
                }

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                result.AppendLine(service.ConnectionData.GetConnectionInfo());

                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(request, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }

                {
                    var repFields = new SdkMessageRequestFieldRepository(service);

                    var queryFields = await repFields.GetListByRequestIdAsync(request.Id, new ColumnSet(true));

                    foreach (var field in queryFields.OrderBy(f => f.Position).ThenBy(f => f.Name))
                    {
                        var desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(field, null, service.ConnectionData);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                            result.AppendLine(desc);
                        }
                    }
                }
            }

            if (node.SdkMessageResponse.HasValue)
            {
                var repository = new SdkMessageResponseRepository(service);
                var Response = await repository.GetByIdAsync(node.SdkMessageResponse.Value, new ColumnSet(true));

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetSdkMessageResponseFileName(service.ConnectionData.Name, node.Name, "Description");
                }

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                result.AppendLine(service.ConnectionData.GetConnectionInfo());

                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(Response, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }

                {
                    var repFields = new SdkMessageResponseFieldRepository(service);

                    var queryFields = await repFields.GetListByResponseIdAsync(Response.Id, new ColumnSet(true));

                    foreach (var field in queryFields.OrderBy(f => f.Position).ThenBy(f => f.Name))
                    {
                        var desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(field, null, service.ConnectionData);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                            result.AppendLine(desc);
                        }
                    }
                }
            }

            if (node.Message != null && node.Message.Any())
            {
                var repository = new SdkMessageRepository(service);
                List<SdkMessage> listMessages = await repository.GetMessageByIdsAsync(node.Message.ToArray());

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetMessageFileName(service.ConnectionData.Name, node.Name, "Description");
                }

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
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                trVSdkMessageRequestTree.ItemsSource = null;
            });

            if (connectionData != null)
            {
                FillEntityNames(connectionData);

                ShowExistingSdkMessageRequests();
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