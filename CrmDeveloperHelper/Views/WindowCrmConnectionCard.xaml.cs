﻿using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowCrmConnectionCard : WindowBase
    {
        /// <summary>
        /// Подключение к CRM
        /// </summary>
        public ConnectionData ConnectionData { get; private set; }

        private readonly ObservableCollection<ConnectionUserData> _listUsers;

        private readonly IWriteToOutput _iWriteToOutput;

        public WindowCrmConnectionCard(IWriteToOutput iWriteToOutput, ConnectionData connectionData, ObservableCollection<ConnectionUserData> listUsers)
        {
            InitializeComponent();

            SetInputLanguageEnglish();

            this.ConnectionData = connectionData;
            this._iWriteToOutput = iWriteToOutput;
            this._listUsers = listUsers;

            if (this.ConnectionData == null)
            {
                this.ConnectionData = new ConnectionData();
            }

            if (this.ConnectionData.ConnectionId == Guid.Empty)
            {
                this.ConnectionData.ConnectionId = Guid.NewGuid();
            }

            RefreshUserList();

            LoadConnectionData(connectionData);

            txtBName.SelectionLength = 0;
            txtBName.SelectionStart = txtBName.Text.Length;

            txtBName.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void RefreshUserList()
        {
            cmBUser.Dispatcher.Invoke(() =>
            {
                cmBUser.Items.Clear();

                cmBUser.Items.Add(string.Empty);

                foreach (var item in this._listUsers)
                {
                    cmBUser.Items.Add(item);
                }

                if (ConnectionData.User != null && cmBUser.Items.Contains(ConnectionData.User))
                {
                    cmBUser.SelectedItem = ConnectionData.User;
                }
                else
                {
                    cmBUser.SelectedIndex = 0;
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

        private bool FieldsIsOk(out string message)
        {
            bool result = true;
            message = string.Empty;

            var builder = new StringBuilder();

            if (string.IsNullOrEmpty(this.txtBName.Text.Trim()))
            {
                result = false;

                if (builder.Length > 0) { builder.AppendLine(); }

                builder.Append(Properties.MessageBoxStrings.NameIsEmpty);
            }

            if (!string.IsNullOrEmpty(txtBConnectionPoolCacheTimeSpan.Text))
            {
                if (!TimeSpan.TryParse(txtBConnectionPoolCacheTimeSpan.Text, out _))
                {
                    result = false;

                    if (builder.Length > 0) { builder.AppendLine(); }

                    builder.Append(Properties.MessageBoxStrings.CannotParseConnectionPoolCacheTimeSpan);
                }
            }

            message = builder.ToString();
            return result;
        }

        private void btnTemplateDiscovery_Click(object sender, RoutedEventArgs e)
        {
            txtBDiscoveryUrl.Text = Properties.MessageBoxStrings.DefaultDiscoveryServiceUrlTemplate;
            txtBDiscoveryUrl.SelectionLength = 0;
            txtBDiscoveryUrl.SelectionStart = 7;
            txtBDiscoveryUrl.Focus();
        }

        private void btnTemplateOrganization_Click(object sender, RoutedEventArgs e)
        {
            txtBOrganizationServiceUrl.Text = Properties.MessageBoxStrings.DefaultOrganizationServiceUrlTemplate;
            txtBOrganizationServiceUrl.SelectionLength = 0;
            txtBOrganizationServiceUrl.SelectionStart = 7;
            txtBOrganizationServiceUrl.Focus();
        }

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            var user = new ConnectionUserData();

            var form = new WindowCrmConnectionUserCard(user);

            if (form.ShowDialog().GetValueOrDefault())
            {
                this._listUsers.Add(user);
            }

            RefreshUserList();
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (cmBUser.SelectedItem is ConnectionUserData)
            {
                var user = cmBUser.SelectedItem as ConnectionUserData;

                var form = new WindowCrmConnectionUserCard(user);

                form.ShowDialog();

                RefreshUserList();
            }
        }

        private void cmBUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditUser.IsEnabled = cmBUser.SelectedItem is ConnectionUserData;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string message = string.Empty;
            bool fieldsIsOk = FieldsIsOk(out message);

            if (!fieldsIsOk)
            {
                MessageBox.Show(this, message, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveConnectionInformation(this.ConnectionData);

            this.ConnectionData.Save();
            this.DialogResult = true;
            this.Close();
        }

        private void SaveConnectionInformation(ConnectionData connectionData)
        {
            connectionData.Name = this.txtBName.Text.Trim();

            connectionData.GroupName = this.txtBGroupName.Text.Trim();

            connectionData.DiscoveryUrl = txtBDiscoveryUrl.Text.Trim();
            connectionData.OrganizationUrl = txtBOrganizationServiceUrl.Text.Trim();
            connectionData.UniqueOrgName = txtBUniqueOrganizationName.Text.Trim();

            connectionData.PublicUrl = txtBPublicUrl.Text.Trim();

            connectionData.User = cmBUser.SelectedItem as ConnectionUserData;

            connectionData.IsReadOnly = chBIsReadOnly.IsChecked.GetValueOrDefault();

            connectionData.IsWriteConnectionPoolActionsToOutput = chBIsWriteConnectionPoolActionsToOutput.IsChecked.GetValueOrDefault();

            if (!string.IsNullOrEmpty(txtBConnectionPoolCacheTimeSpan.Text))
            {
                connectionData.ConnectionPoolCacheTimeSpan = TimeSpan.Parse(txtBConnectionPoolCacheTimeSpan.Text);
            }
        }

        private void LoadConnectionData(ConnectionData connectionData)
        {
            txtBName.Text = connectionData.Name;
            txtBGroupName.Text = connectionData.GroupName;
            txtBDiscoveryUrl.Text = connectionData.DiscoveryUrl;
            txtBOrganizationServiceUrl.Text = connectionData.OrganizationUrl;
            txtBUniqueOrganizationName.Text = connectionData.UniqueOrgName;

            chBIsReadOnly.IsChecked = connectionData.IsReadOnly;
            chBIsWriteConnectionPoolActionsToOutput.IsChecked = connectionData.IsWriteConnectionPoolActionsToOutput;

            txtBConnectionPoolCacheTimeSpan.Text = connectionData.ConnectionPoolCacheTimeSpan.ToString();

            LoadReadOnlyInformation(connectionData);
        }

        private void LoadReadOnlyInformation(ConnectionData connectionData)
        {
            txtBPublicUrl.Dispatcher.Invoke(() =>
            {
                if (string.IsNullOrEmpty(txtBPublicUrl.Text)
                    && !string.IsNullOrEmpty(connectionData.PublicUrl)
                )
                {
                    txtBPublicUrl.Text = connectionData.PublicUrl;
                }

                txtBVersion.Text = connectionData.OrganizationVersion;
                txtBFriendlyName.Text = connectionData.FriendlyName;
                txtBOrganizationId.Text = connectionData.OrganizationId.ToString();
                txtBOrganizationState.Text = connectionData.OrganizationState;
                txtBUrlName.Text = connectionData.UrlName;

                txtBBaseCurrency.Text = connectionData.BaseCurrency;
                txtBDefaultLanguage.Text = connectionData.DefaultLanguage;
                txtBInstalledLanguagePacks.Text = connectionData.InstalledLanguagePacks;
            });
        }

        private async void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            ToggleControls(this.ConnectionData, false, Properties.OutputStrings.StartTestingConnectionFormat1, this.ConnectionData.Name);

            SaveConnectionInformation(this.ConnectionData);

            var testResult = await QuickConnection.TestConnectAsync(this.ConnectionData, this._iWriteToOutput, this);

            if (testResult)
            {
                LoadConnectionData(this.ConnectionData);

                ToggleControls(this.ConnectionData, true, Properties.OutputStrings.ConnectedSuccessfullyFormat1, this.ConnectionData.Name);
            }
            else
            {
                ToggleControls(this.ConnectionData, true, Properties.OutputStrings.ConnectionFailedFormat1, this.ConnectionData.Name);
            }
        }

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, this.btnSave, this.btnTestConnection);
        }
    }
}