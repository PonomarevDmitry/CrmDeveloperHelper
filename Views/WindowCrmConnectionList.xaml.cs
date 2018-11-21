using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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

        public WindowCrmConnectionList(IWriteToOutput iWriteToOutput, ConnectionConfiguration crmConfig)
        {
            this._crmConfig = crmConfig;
            this._iWriteToOutput = iWriteToOutput;

            BindingOperations.EnableCollectionSynchronization(this._crmConfig.Connections, sysObjectConnections);
            BindingOperations.EnableCollectionSynchronization(this._crmConfig.ArchiveConnections, sysObjectArchiveConnections);
            BindingOperations.EnableCollectionSynchronization(this._crmConfig.Users, sysObjectUsers);

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            lstVwConnections.ItemsSource = this._crmConfig.Connections;

            lstVwArchiveConnections.ItemsSource = this._crmConfig.ArchiveConnections;

            lstVwUsers.ItemsSource = this._crmConfig.Users;

            UpdateCurrentConnectionInfo();
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControl(Control c, bool enabled)
        {
            c.Dispatcher.Invoke(() =>
            {
                try
                {
                    if (c is TextBox)
                    {
                        ((TextBox)c).IsReadOnly = !enabled;
                    }
                    else
                    {
                        c.IsEnabled = enabled;
                    }
                }
                catch (Exception)
                {
                }
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
                    bool enabled = this.lstVwConnections.SelectedItems.Count > 0;

                    UIElement[] list = { tSBEdit, tSBCreateCopy, tSBMoveToArchive, tSBUp, tSBDown, tSBTestConnection };

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
                    bool enabled = this.lstVwArchiveConnections.SelectedItems.Count > 0;

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
                    bool enabled = this.lstVwUsers.SelectedItems.Count > 0;

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
        }

        private void tSBCreate_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = new ConnectionData();

            var form = new WindowCrmConnectionCard(_iWriteToOutput, connectionData, _crmConfig.Users);

            if (form.ShowDialog().GetValueOrDefault())
            {
                this.Dispatcher.Invoke(() =>
                {
                    connectionData.ConnectionConfiguration = this._crmConfig;
                    this._crmConfig.Connections.Add(connectionData);
                });
            }

            UpdateCurrentConnectionInfo();

            this._crmConfig.Save();
        }

        private void tSBEdit_Click(object sender, RoutedEventArgs e)
        {
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

            this._crmConfig.Save();
        }

        private void tSBCreateCopy_Click(object sender, RoutedEventArgs e)
        {
            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData oldconnectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                ConnectionData connectionData = oldconnectionData.CreateCopy();

                var form = new WindowCrmConnectionCard(_iWriteToOutput, connectionData, _crmConfig.Users);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        connectionData.ConnectionConfiguration = this._crmConfig;
                        this._crmConfig.Connections.Add(connectionData);
                    });
                }

                UpdateCurrentConnectionInfo();

                this._crmConfig.Save();
            }
        }

        private async void tSBTestConnection_Click(object sender, RoutedEventArgs e)
        {
            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                UpdateStatus(Properties.WindowStatusStrings.StartTestingConnectionFormat1, connectionData.Name);

                try
                {
                    await QuickConnection.TestConnectAsync(connectionData, this._iWriteToOutput);

                    UpdateCurrentConnectionInfo();

                    this._crmConfig.Save();

                    UpdateStatus(Properties.WindowStatusStrings.ConnectedSuccessfullyFormat1, connectionData.Name);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(ex);

                    UpdateStatus(Properties.WindowStatusStrings.ConnectionFailedFormat1, connectionData.Name);
                }
            }
        }

        private void tSBUp_Click(object sender, RoutedEventArgs e)
        {
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

            if (!string.IsNullOrEmpty(directory))
            {
                if (Directory.Exists(directory))
                {
                    try
                    {
                        Process.Start(directory);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        private void tSBSelectConnection_Click(object sender, RoutedEventArgs e)
        {
            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                this._crmConfig.SetCurrentConnection(connectionData.ConnectionId);

                this._crmConfig.Save();

                _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);
                _iWriteToOutput.ActivateOutputWindow();

                this.DialogResult = true;

                this.Close();
            }
        }

        private void tSBMoveToArchive_Click(object sender, RoutedEventArgs e)
        {
            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                string message = string.Format(Properties.MessageBoxStrings.MoveConnectionToArchiveFormat1, connectionData.Name);

                if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    if (connectionData.ConnectionId == _crmConfig.CurrentConnectionData?.ConnectionId)
                    {
                        _crmConfig.SetCurrentConnection(null);

                        _iWriteToOutput.WriteToOutput(Properties.WindowStatusStrings.ConnectionIsNotSelected);
                        _iWriteToOutput.ActivateOutputWindow();
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
                var item = ((FrameworkElement)e.OriginalSource).DataContext as ConnectionData;

                if (item != null)
                {
                    CallEditConnection(item);
                }
            }
        }

        private void tSBCreateUser_Click(object sender, RoutedEventArgs e)
        {
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
                if (((FrameworkElement)e.OriginalSource).DataContext is ConnectionUserData user)
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
            if (lstVwArchiveConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwArchiveConnections.SelectedItems[0] as ConnectionData;

                UpdateStatus(Properties.WindowStatusStrings.StartTestingConnectionFormat1, connectionData.Name);

                try
                {
                    await QuickConnection.TestConnectAsync(connectionData, this._iWriteToOutput);

                    this._crmConfig.Save();

                    UpdateStatus(Properties.WindowStatusStrings.ConnectedSuccessfullyFormat1, connectionData.Name);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(ex);

                    UpdateStatus(Properties.WindowStatusStrings.ConnectionFailedFormat1, connectionData.Name);
                }
            }
        }

        private void tSBArchiveUp_Click(object sender, RoutedEventArgs e)
        {
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
                if (((FrameworkElement)e.OriginalSource).DataContext is ConnectionData item)
                {
                    CallEditArchiveConnection(item);
                }
            }
        }

        private void tSBArchiveReturnToConnectionList_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = lstVwArchiveConnections.SelectedItems[0] as ConnectionData;

            string message = string.Format(Properties.MessageBoxStrings.ReturnCRMConnectionToConnectionListFormat1, connectionData.Name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.Dispatcher.Invoke(() =>
                {
                    connectionData.ConnectionConfiguration = this._crmConfig;
                    _crmConfig.Connections.Add(connectionData);
                    _crmConfig.ArchiveConnections.Remove(connectionData);
                });
            }

            UpdateCurrentConnectionInfo();

            this._crmConfig.Save();
        }

        private void bntSelect_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (((FrameworkElement)e.OriginalSource).DataContext is ConnectionData item)
                {
                    this._crmConfig.SetCurrentConnection(item.ConnectionId);

                    this._crmConfig.Save();

                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentConnectionFormat1, item.Name);
                    _iWriteToOutput.ActivateOutputWindow();

                    this.DialogResult = true;

                    this.Close();
                }
            }
        }
    }
}