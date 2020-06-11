using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowCrmConnectionList : WindowBase
    {
        private readonly object sysObjectConnections = new object();
        private readonly object sysObjectArchiveConnections = new object();
        private readonly object sysObjectUsers = new object();

        private readonly ConnectionConfiguration _crmConfig;
        private readonly IWriteToOutput _iWriteToOutput;

        private readonly bool _currentConnectionChoosing;

        private readonly ObservableCollection<OrganizationDetailViewItem> _itemsSource;

        public WindowCrmConnectionList(IWriteToOutput iWriteToOutput, ConnectionConfiguration crmConfig, bool currentConnectionChoosing = false)
        {
            this._crmConfig = crmConfig;
            this._iWriteToOutput = iWriteToOutput;
            this._currentConnectionChoosing = currentConnectionChoosing;

            BindingOperations.EnableCollectionSynchronization(this._crmConfig.Connections, sysObjectConnections);
            BindingOperations.EnableCollectionSynchronization(this._crmConfig.ArchiveConnections, sysObjectArchiveConnections);
            BindingOperations.EnableCollectionSynchronization(this._crmConfig.Users, sysObjectUsers);

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            var commonConfig = CommonConfiguration.Get();

            WindowCrmConnectionHandler.InitializeConnectionMenuItems(miConnection, this._iWriteToOutput, commonConfig, GetSelectedConnection);

            lstVwConnections.ItemsSource = this._crmConfig.Connections;

            lstVwArchiveConnections.ItemsSource = this._crmConfig.ArchiveConnections;

            lstVwUsers.ItemsSource = this._crmConfig.Users;

            lstVwOrganizations.ItemsSource = this._itemsSource = new ObservableCollection<OrganizationDetailViewItem>();

            UpdateCurrentConnectionInfo();

            LoadConfiguration();

            UpdateButtonsConnection();
            UpdateButtonsConnectionArchive();
            UpdateButtonsUsers();
            UpdateButtonsDiscoveryOrganization();
        }

        protected override void OnClosed(EventArgs e)
        {
            this._crmConfig.Save();

            BindingOperations.ClearAllBindings(lstVwConnections);
            BindingOperations.ClearAllBindings(lstVwArchiveConnections);
            BindingOperations.ClearAllBindings(lstVwUsers);

            lstVwConnections.Items.DetachFromSourceCollection();
            lstVwArchiveConnections.Items.DetachFromSourceCollection();
            lstVwUsers.Items.DetachFromSourceCollection();

            lstVwConnections.ItemsSource = null;
            lstVwArchiveConnections.ItemsSource = null;
            lstVwUsers.ItemsSource = null;

            base.OnClosed(e);
        }

        private const string paramDiscoveryServiceUrl = "DiscoveryServiceUrl";
        private const string paramDiscoveryServiceUser = "DiscoveryServiceUser";

        private void LoadConfiguration()
        {
            WindowSettings winConfig = this.GetWindowsSettings();

            this.cmBDiscoveryServiceUrl.Text = winConfig.GetValueString(paramDiscoveryServiceUrl);

            {
                var temp = winConfig.GetValueString(paramDiscoveryServiceUser);

                if (!string.IsNullOrEmpty(temp)
                    && Guid.TryParse(temp, out Guid userId)
                )
                {
                    var user = this._crmConfig.Users.FirstOrDefault(u => u.UserId == userId);

                    if (user != null)
                    {
                        if (cmBDiscoveryServiceUser.Items.Contains(user))
                        {
                            cmBDiscoveryServiceUser.SelectedItem = user;
                        }
                    }
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictString[paramDiscoveryServiceUrl] = this.cmBDiscoveryServiceUrl.Text?.Trim();
            winConfig.DictString[paramDiscoveryServiceUser] = (this.cmBDiscoveryServiceUser.SelectedItem as ConnectionUserData)?.UserId.ToString();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(tSProgressBar
                , mICreate
            );

            UpdateButtonsConnection();
            UpdateButtonsUsers();
            UpdateButtonsConnectionArchive();
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

        private void lstVwConnections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsConnection();
        }

        private void UpdateButtonsConnection()
        {
            this.lstVwConnections.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = IsControlsEnabled && this.lstVwConnections.SelectedItems.Count > 0;

                    UIElement[] list = { tSBEdit, tSBCreateCopy, tSBMoveToArchive, tSBUp, tSBDown, tSBTestConnection, miConnection };

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

        private void lstVwArchiveConnections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsConnectionArchive();
        }

        private void UpdateButtonsConnectionArchive()
        {
            this.lstVwArchiveConnections.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = IsControlsEnabled && this.lstVwArchiveConnections.SelectedItems.Count > 0;

                    UIElement[] list = { tSBArchiveEdit, tSBArchiveTestConnection, tSBArchiveUp, tSBArchiveDown, tSBArchiveDelete, tSBArchiveReturnToConnectionList };

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

        private void lstVwUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsUsers();
        }

        private void UpdateButtonsUsers()
        {
            this.lstVwUsers.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = IsControlsEnabled && this.lstVwUsers.SelectedItems.Count > 0;

                    UIElement[] list = { tSBDeleteUser, tSBEditUser, tSBUserDown, tSBUserUp };

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

        private void UpdateCurrentConnectionInfo()
        {
            ConnectionData connectionData = this._crmConfig.CurrentConnectionData;

            string name = "None";

            if (connectionData != null)
            {
                name = connectionData.GetDescription();
            }

            txtBCurrentConnection.Dispatcher.Invoke(() =>
            {
                txtBCurrentConnection.Text = name;
            });

            UpdateDiscoveryServiceUrls();

            UpdateDiscoveryUsers();
        }

        private void UpdateDiscoveryServiceUrls()
        {
            var services = new HashSet<string>(this._crmConfig.Connections.Where(c => !string.IsNullOrEmpty(c.DiscoveryUrl)).Select(c => c.DiscoveryUrl), StringComparer.InvariantCultureIgnoreCase);

            foreach (var item in this._crmConfig.ArchiveConnections)
            {
                if (!string.IsNullOrEmpty(item.DiscoveryUrl))
                {
                    services.Add(item.DiscoveryUrl);
                }
            }

            cmBDiscoveryServiceUrl.Dispatcher.Invoke(() =>
            {
                var text = cmBDiscoveryServiceUrl.Text;

                cmBDiscoveryServiceUrl.Items.Clear();

                foreach (var item in services.OrderBy(s => s))
                {
                    cmBDiscoveryServiceUrl.Items.Add(item);
                }

                cmBDiscoveryServiceUrl.Text = text;
            });
        }

        private void UpdateDiscoveryUsers()
        {
            cmBDiscoveryServiceUser.Dispatcher.Invoke(() =>
            {
                var selectedUser = cmBDiscoveryServiceUser.SelectedItem as ConnectionUserData;

                cmBDiscoveryServiceUser.Items.Clear();

                cmBDiscoveryServiceUser.Items.Add(string.Empty);

                foreach (var item in this._crmConfig.Users)
                {
                    cmBDiscoveryServiceUser.Items.Add(item);
                }

                if (selectedUser != null
                    && cmBDiscoveryServiceUser.Items.Contains(selectedUser)
                )
                {
                    cmBDiscoveryServiceUser.SelectedItem = selectedUser;
                }
                else
                {
                    cmBDiscoveryServiceUser.SelectedIndex = 0;
                }
            });
        }

        private void tSBCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = new ConnectionData()
            {
                ConnectionConfiguration = _crmConfig,
            };

            var form = new WindowCrmConnectionCard(_iWriteToOutput, connectionData, _crmConfig.Users);

            if (form.ShowDialog().GetValueOrDefault())
            {
                this.Dispatcher.Invoke(() =>
                {
                    connectionData.Save();
                    connectionData.ConnectionConfiguration = this._crmConfig;
                    connectionData.LoadIntellisenseAsync();
                    connectionData.StartWatchFile();
                    this._crmConfig.Connections.Add(connectionData);
                });
            }

            UpdateCurrentConnectionInfo();

            this._crmConfig.Save();
        }

        private void tSBEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                CallEditConnection(connectionData);
            }
        }

        private void CallEditConnection(ConnectionData connectionData)
        {
            var form = new WindowCrmConnectionCard(_iWriteToOutput, connectionData, _crmConfig.Users);

            form.ShowDialog();

            UpdateCurrentConnectionInfo();

            connectionData.Save();
            this._crmConfig.Save();
        }

        private void tSBCreateCopy_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData oldconnectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                ConnectionData connectionData = oldconnectionData.CreateCopy();

                var form = new WindowCrmConnectionCard(_iWriteToOutput, connectionData, _crmConfig.Users);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        connectionData.Save();
                        connectionData.ConnectionConfiguration = this._crmConfig;
                        connectionData.LoadIntellisenseAsync();
                        connectionData.StartWatchFile();
                        this._crmConfig.Connections.Add(connectionData);
                    });
                }

                UpdateCurrentConnectionInfo();

                this._crmConfig.Save();
            }
        }

        private void tSBLoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            string selectedPath = string.Empty;
            var t = new Thread(() =>
            {
                try
                {
                    var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
                    {
                        Filter = "ConnectionData (.xml)|*.xml",
                        FilterIndex = 1,
                        RestoreDirectory = true
                    };

                    if (openFileDialog1.ShowDialog().GetValueOrDefault())
                    {
                        selectedPath = openFileDialog1.FileName;
                    }
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(null, ex);
                    _iWriteToOutput.ActivateOutputWindow(null, this);
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (string.IsNullOrEmpty(selectedPath))
            {
                return;
            }

            ConnectionData connectionData = null;

            try
            {
                connectionData = ConnectionData.GetFromPath(selectedPath);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null, this);

                connectionData = null;
            }

            if (connectionData == null)
            {
                return;
            }

            connectionData.User = this._crmConfig.Users.FirstOrDefault(u => u.UserId == connectionData.UserId);

            if (this._crmConfig.Connections.Any(c => c.ConnectionId == connectionData.ConnectionId)
                || this._crmConfig.ArchiveConnections.Any(c => c.ConnectionId == connectionData.ConnectionId)
                || connectionData.ConnectionId == Guid.Empty
                )
            {
                connectionData.ConnectionId = Guid.NewGuid();
            }

            var form = new WindowCrmConnectionCard(_iWriteToOutput, connectionData, _crmConfig.Users);

            if (form.ShowDialog().GetValueOrDefault())
            {
                this.Dispatcher.Invoke(() =>
                {
                    connectionData.Save();
                    connectionData.ConnectionConfiguration = this._crmConfig;
                    connectionData.LoadIntellisenseAsync();
                    connectionData.StartWatchFile();
                    this._crmConfig.Connections.Add(connectionData);
                });
            }

            UpdateCurrentConnectionInfo();

            this._crmConfig.Save();
        }

        private async void tSBTestConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwConnections.SelectedItems.Count != 1)
            {
                return;
            }

            ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

            ToggleControls(connectionData, false, Properties.OutputStrings.StartTestingConnectionFormat1, connectionData.Name);

            var task = QuickConnection.TestConnectAsync(connectionData, this._iWriteToOutput, this);

            this.Focus();
            this.Activate();

            var testResult = await task;

            if (testResult)
            {
                UpdateCurrentConnectionInfo();

                connectionData.Save();
                this._crmConfig.Save();

                ToggleControls(connectionData, true, Properties.OutputStrings.ConnectedSuccessfullyFormat1, connectionData.Name);
            }
            else
            {
                ToggleControls(connectionData, true, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
            }
        }

        private void tSBUp_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                connectionData.ConnectionConfiguration = this._crmConfig;
                int index = _crmConfig.Connections.IndexOf(connectionData);

                if (index != 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        _crmConfig.Connections.Move(index, index - 1);

                        lstVwConnections.SelectedItem = connectionData;

                        UpdateCurrentConnectionInfo();
                    });

                    this._crmConfig.Save();
                }
            }
        }

        private void tSBDown_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                connectionData.ConnectionConfiguration = this._crmConfig;
                int index = _crmConfig.Connections.IndexOf(connectionData);

                if (index != _crmConfig.Connections.Count - 1)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        _crmConfig.Connections.Move(index, index + 1);

                        lstVwConnections.SelectedItem = connectionData;
                    });

                    UpdateCurrentConnectionInfo();

                    this._crmConfig.Save();
                }
            }
        }

        private void tSBOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            var directory = FileOperations.GetConfigurationFolder();

            this._iWriteToOutput.OpenFolder(null, directory);
        }

        private void tSBSelectConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                this._crmConfig.SetCurrentConnection(connectionData.ConnectionId);

                this._crmConfig.Save();

                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);

                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(connectionData, this);

                if (this._currentConnectionChoosing)
                {
                    this.DialogResult = true;

                    this.Close();
                }
            }
        }

        private void tSBMoveToArchive_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                string message = string.Format(Properties.MessageBoxStrings.MoveConnectionToArchiveFormat1, connectionData.Name);

                if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    if (connectionData.ConnectionId == _crmConfig.CurrentConnectionData?.ConnectionId)
                    {
                        _crmConfig.SetCurrentConnection(null);

                        _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.ConnectionIsNotSelected);
                        _iWriteToOutput.ActivateOutputWindow(null, this);
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        connectionData.ConnectionConfiguration = this._crmConfig;
                        _crmConfig.ArchiveConnections.Add(connectionData);
                        _crmConfig.Connections.Remove(connectionData);
                    });
                }

                UpdateCurrentConnectionInfo();

                this._crmConfig.Save();
            }
        }

        private void lstVwConnections_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ConnectionData item = GetItemFromRoutedDataContext<ConnectionData>(e);

                if (item != null)
                {
                    CallEditConnection(item);
                }
            }
        }

        private void tSBCreateUser_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var user = new ConnectionUserData();

            var form = new WindowCrmConnectionUserCard(user);

            if (form.ShowDialog().GetValueOrDefault())
            {
                this._crmConfig.Users.Add(user);
            }

            UpdateCurrentConnectionInfo();

            this._crmConfig.Save();
        }

        private void tSBEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwUsers.SelectedItems.Count == 1)
            {
                ConnectionUserData user = lstVwUsers.SelectedItems[0] as ConnectionUserData;

                CallUserEditForm(user);
            }
        }

        private void lstVwUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ConnectionUserData user = GetItemFromRoutedDataContext<ConnectionUserData>(e);

                if (user != null)
                {
                    CallUserEditForm(user);
                }
            }
        }

        private void CallUserEditForm(ConnectionUserData user)
        {
            var form = new WindowCrmConnectionUserCard(user);

            form.ShowDialog();

            UpdateCurrentConnectionInfo();

            this._crmConfig.Save();
        }

        private void tSBDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwUsers.SelectedItems.Count == 1)
            {
                ConnectionUserData user = lstVwUsers.SelectedItems[0] as ConnectionUserData;

                string message = string.Format(Properties.MessageBoxStrings.DeleteCRMConnectionUserFormat1, user.Username);

                if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    _crmConfig.Users.Remove(user);
                }

                UpdateCurrentConnectionInfo();

                this._crmConfig.Save();
            }
        }

        private void tSBUserUp_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwUsers.SelectedItems.Count == 1)
            {
                ConnectionUserData user = lstVwUsers.SelectedItems[0] as ConnectionUserData;

                int index = _crmConfig.Users.IndexOf(user);

                if (index != 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        _crmConfig.Users.Move(index, index - 1);

                        lstVwUsers.SelectedItem = user;
                    });

                    this._crmConfig.Save();
                }
            }
        }

        private void tSBUserDown_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwUsers.SelectedItems.Count == 1)
            {
                ConnectionUserData user = lstVwUsers.SelectedItems[0] as ConnectionUserData;

                int index = _crmConfig.Users.IndexOf(user);

                if (index != _crmConfig.Users.Count - 1)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        _crmConfig.Users.Move(index, index + 1);

                        lstVwUsers.SelectedItem = user;
                    });

                    this._crmConfig.Save();
                }
            }
        }

        private void tSBArchiveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwArchiveConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwArchiveConnections.SelectedItems[0] as ConnectionData;

                CallEditArchiveConnection(connectionData);
            }
        }

        private void CallEditArchiveConnection(ConnectionData connectionData)
        {
            var form = new WindowCrmConnectionCard(_iWriteToOutput, connectionData, _crmConfig.Users);

            form.ShowDialog();

            this._crmConfig.Save();
        }

        private async void tSBArchiveTestConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwArchiveConnections.SelectedItems.Count != 1)
            {
                return;
            }

            ConnectionData connectionData = lstVwArchiveConnections.SelectedItems[0] as ConnectionData;

            ToggleControls(connectionData, false, Properties.OutputStrings.StartTestingConnectionFormat1, connectionData.Name);

            var task = QuickConnection.TestConnectAsync(connectionData, this._iWriteToOutput, this);

            this.Focus();
            this.Activate();

            var testResult = await task;

            if (testResult)
            {
                connectionData.Save();
                this._crmConfig.Save();

                ToggleControls(connectionData, true, Properties.OutputStrings.ConnectedSuccessfullyFormat1, connectionData.Name);
            }
            else
            {
                ToggleControls(connectionData, true, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
            }
        }

        private void tSBArchiveUp_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwArchiveConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwArchiveConnections.SelectedItems[0] as ConnectionData;

                int index = _crmConfig.ArchiveConnections.IndexOf(connectionData);

                if (index != 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        _crmConfig.ArchiveConnections.Move(index, index - 1);

                        lstVwArchiveConnections.SelectedItem = connectionData;
                    });

                    this._crmConfig.Save();
                }
            }
        }

        private void tSBArchiveDown_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwArchiveConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwArchiveConnections.SelectedItems[0] as ConnectionData;

                int index = _crmConfig.ArchiveConnections.IndexOf(connectionData);

                if (index != _crmConfig.ArchiveConnections.Count - 1)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        _crmConfig.ArchiveConnections.Move(index, index + 1);

                        lstVwArchiveConnections.SelectedItem = connectionData;
                    });

                    this._crmConfig.Save();
                }
            }
        }

        private void tSBArchiveDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (lstVwArchiveConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwArchiveConnections.SelectedItems[0] as ConnectionData;

                string message = string.Format(Properties.MessageBoxStrings.DeleteCRMConnectionFormat1, connectionData.Name);

                if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        _crmConfig.ArchiveConnections.Remove(connectionData);
                    });
                }

                this._crmConfig.Save();
            }
        }

        private void lstVwArchiveConnections_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ConnectionData item = GetItemFromRoutedDataContext<ConnectionData>(e);

                if (item != null)
                {
                    CallEditArchiveConnection(item);
                }
            }
        }

        private void tSBArchiveReturnToConnectionList_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = lstVwArchiveConnections.SelectedItems[0] as ConnectionData;

            string message = string.Format(Properties.MessageBoxStrings.ReturnCRMConnectionToConnectionListFormat1, connectionData.Name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.Dispatcher.Invoke(() =>
                {
                    connectionData.ConnectionConfiguration = this._crmConfig;
                    connectionData.LoadIntellisenseAsync();
                    connectionData.StartWatchFile();
                    _crmConfig.Connections.Add(connectionData);
                    _crmConfig.ArchiveConnections.Remove(connectionData);
                });
            }

            UpdateCurrentConnectionInfo();

            this._crmConfig.Save();
        }

        private void bntSelect_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (sender is Button button)
            {
                ConnectionData connectionData = GetItemFromRoutedDataContext<ConnectionData>(e);

                if (connectionData != null)
                {
                    this._crmConfig.SetCurrentConnection(connectionData.ConnectionId);

                    this._crmConfig.Save();

                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);

                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);

                    _iWriteToOutput.ActivateOutputWindow(connectionData, this);

                    if (this._currentConnectionChoosing)
                    {
                        this.DialogResult = true;

                        this.Close();
                    }
                }
            }
        }

        private void lstVwOrganizations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.lstVwOrganizations.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwOrganizations.SelectedItems.Count > 0;

                    UIElement[] list = { tSBCreateConnectionFromOrganization };

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

        private void tSBCreateConnectionFromOrganization_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            UpdateButtonsDiscoveryOrganization();
        }

        private void UpdateButtonsDiscoveryOrganization()
        {
            if (lstVwOrganizations.SelectedItems.Count == 1)
            {
                OrganizationDetailViewItem detail = lstVwOrganizations.SelectedItems[0] as OrganizationDetailViewItem;

                if (detail != null)
                {
                    OpenConnectionFormForOrganizationDetail(detail);
                }
            }
        }

        private void bntCreateConnectionFromOrganization_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (sender is Button button)
            {
                OrganizationDetailViewItem detail = GetItemFromRoutedDataContext<OrganizationDetailViewItem>(e);

                if (detail != null)
                {
                    OpenConnectionFormForOrganizationDetail(detail);
                }
            }
        }

        private void OpenConnectionFormForOrganizationDetail(OrganizationDetailViewItem detail)
        {
            ConnectionData connectionData = new ConnectionData()
            {
                ConnectionId = Guid.NewGuid(),

                ConnectionConfiguration = _crmConfig,

                DiscoveryUrl = detail.DiscoveryUri.ToString(),
                OrganizationUrl = detail.OrganizationServiceEndpoint,
                PublicUrl = detail.WebApplicationEndpoint,
                UniqueOrgName = detail.UniqueName,

                User = detail.User,

                OrganizationId = detail.OrganizationId,
                FriendlyName = detail.FriendlyName,
                OrganizationVersion = detail.OrganizationVersion,
                OrganizationState = detail.State.ToString(),
                UrlName = detail.UrlName,
            };

            var form = new WindowCrmConnectionCard(_iWriteToOutput, connectionData, _crmConfig.Users);

            if (form.ShowDialog().GetValueOrDefault())
            {
                this.Dispatcher.Invoke(() =>
                {
                    connectionData.Save();
                    connectionData.ConnectionConfiguration = this._crmConfig;
                    connectionData.LoadIntellisenseAsync();
                    connectionData.StartWatchFile();
                    this._crmConfig.Connections.Add(connectionData);
                });
            }

            UpdateCurrentConnectionInfo();

            this._crmConfig.Save();
        }

        private async void CmBDiscoveryServiceUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (e.Key != Key.Enter)
            {
                return;
            }

            string discoveryServiceUrl = cmBDiscoveryServiceUrl.Text?.Trim();

            if (string.IsNullOrEmpty(discoveryServiceUrl))
            {
                return;
            }

            if (!discoveryServiceUrl.EndsWith(Properties.MessageBoxStrings.DefaultDiscoveryServiceUrlSuffix, StringComparison.InvariantCultureIgnoreCase))
            {
                discoveryServiceUrl = discoveryServiceUrl.TrimEnd('/') + Properties.MessageBoxStrings.DefaultDiscoveryServiceUrlSuffix;
            }

            if (!Uri.TryCreate(discoveryServiceUrl, UriKind.Absolute, out Uri discoveryServiceUri))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.DiscoveryServiceCouldNotBeReceived);
                _iWriteToOutput.ActivateOutputWindow(null, this);
                return;
            }

            ToggleControls(null, false, Properties.OutputStrings.DiscoveringOrganizations);

            try
            {
                this._itemsSource.Clear();

                var serviceManagement = await CreateManagementAsync(discoveryServiceUri);

                if (serviceManagement == null)
                {
                    ToggleControls(null, true, Properties.OutputStrings.DiscoveryServiceConfigurationCouldNotBeReceived);
                    _iWriteToOutput.ActivateOutputWindow(null, this);
                    return;
                }

                var user = cmBDiscoveryServiceUser.SelectedItem as ConnectionUserData;

                var discoveryService = QuickConnection.CreateDiscoveryService(serviceManagement, user?.Username, user?.Password);

                if (discoveryService == null)
                {
                    ToggleControls(null, true, Properties.OutputStrings.DiscoveryServiceCouldNotBeReceived);
                    _iWriteToOutput.ActivateOutputWindow(null, this);
                    return;
                }

                var repository = new DiscoveryServiceRepository(discoveryService);

                var orgs = await repository.DiscoverOrganizationsAsync();

                this._itemsSource.Clear();

                foreach (var org in orgs.OrderBy(o => o.UniqueName).ThenBy(o => o.OrganizationId))
                {
                    var viewItem = new OrganizationDetailViewItem(discoveryServiceUri, user, org);

                    this._itemsSource.Add(viewItem);
                }

                ToggleControls(null, true, Properties.OutputStrings.DiscoveringOrganizationsCompleted);
            }
            catch (Exception ex)
            {
                ToggleControls(null, true, Properties.OutputStrings.DiscoveringOrganizationsFailed);
                _iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private Task<IServiceManagement<IDiscoveryService>> CreateManagementAsync(Uri discoveryServiceUri)
        {
            return Task.Run(() => CreateManagement(discoveryServiceUri));
        }

        private IServiceManagement<IDiscoveryService> CreateManagement(Uri discoveryServiceUri)
        {
            return ServiceConfigurationFactory.CreateManagement<IDiscoveryService>(discoveryServiceUri);
        }

        private void tSBSelectConnectionFileInFolder_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetItemFromRoutedDataContext<ConnectionData>(e);

            if (connectionData == null)
            {
                return;
            }

            _iWriteToOutput.SelectFileInFolder(connectionData, connectionData.Path);
        }

        private void tSBCopyConnectionId_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetItemFromRoutedDataContext<ConnectionData>(e);

            if (connectionData == null)
            {
                return;
            }

            ClipboardHelper.SetText(connectionData.ConnectionId.ToString());
        }

        private ConnectionData GetSelectedConnection()
        {
            return this.lstVwConnections.SelectedItems.OfType<ConnectionData>().Count() == 1
                ? this.lstVwConnections.SelectedItems.OfType<ConnectionData>().SingleOrDefault() : null;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwConnections_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            tSBMoveToArchive_Click(sender, e);
        }

        private void lstVwUsers_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            tSBDeleteUser_Click(sender, e);
        }

        private void lstVwArchiveConnections_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            tSBArchiveDelete_Click(sender, e);
        }
    }
}