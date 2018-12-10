using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowCreateProxyClasses : WindowBase
    {
        private readonly object sysObjectUtils = new object();

        private CommonConfiguration _commonConfig;

        private ConnectionConfiguration _crmConfig;

        public WindowCreateProxyClasses(
            CommonConfiguration commonConfig
            , ConnectionConfiguration crmConfig
            , string fileName)
        {
            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._commonConfig = commonConfig;
            this._crmConfig = crmConfig;

            BindingOperations.EnableCollectionSynchronization(this._commonConfig.Utils, sysObjectUtils);

            InitializeComponent();

            this.tSSLblConnectionName.Content = crmConfig.CurrentConnectionData.Name;

            txtBFileName.Text = fileName;

            LoadFromConfig();
        }

        protected override void OnClosed(EventArgs e)
        {
            var connection = _crmConfig.CurrentConnectionData;

            if (connection != null)
            {
                if (cmBCrmSvcUtil.SelectedItem != null)
                {
                    connection.SelectedCrmSvcUtil = (cmBCrmSvcUtil.SelectedItem as CrmSvcUtil).Id;
                }
            }

            _commonConfig.Save();
            _crmConfig.Save();

            BindingOperations.ClearAllBindings(cmBCrmSvcUtil);

            cmBCrmSvcUtil.Items.DetachFromSourceCollection();

            cmBCrmSvcUtil.DataContext = null;
            cmBCrmSvcUtil.ItemsSource = null;

            base.OnClosed(e);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void LoadFromConfig()
        {
            FillUtils();

            var connection = _crmConfig.CurrentConnectionData;

            if (connection != null)
            {
                txtBNameSpace.DataContext = connection;
                txtBServiceContextName.DataContext = connection;
                chBInteractiveLogin.DataContext = connection;
                chBGenerateActions.DataContext = connection;

                txtBOrganizationService.Text = connection.OrganizationUrl;
                if (connection.User == null)
                {
                    txtBUser.Text = Properties.MessageBoxStrings.InteractiveLogin;
                }
                else
                {
                    txtBUser.Text = connection.GetUsername;
                }
                txtBVersion.Text = connection.OrganizationVersion;
            }
        }

        private void FillUtils()
        {
            cmBCrmSvcUtil.ItemsSource = _commonConfig.Utils;

            var connection = _crmConfig.CurrentConnectionData;

            if (connection != null)
            {
                if (connection.SelectedCrmSvcUtil.HasValue)
                {
                    var util = _commonConfig.Utils.FirstOrDefault(u => u.Id == connection.SelectedCrmSvcUtil);

                    if (util != null)
                    {
                        cmBCrmSvcUtil.SelectedItem = util;
                    }
                    else
                    {
                        connection.SelectedCrmSvcUtil = null;
                        SelectUtilByVersion(connection.OrganizationVersion);
                    }
                }
                else
                {
                    SelectUtilByVersion(connection.OrganizationVersion);
                }
            }
        }

        private void SelectUtilByVersion(string version)
        {
            var first = version[0];

            var util = _commonConfig.Utils
                .OrderByDescending(s => s.Version)
                .FirstOrDefault(s => s.Version[0] == first);

            if (util != null)
            {
                cmBCrmSvcUtil.SelectedItem = util;
            }
            else
            {
                cmBCrmSvcUtil.SelectedIndex = 0;
            }
        }

        private void btnUpdateProxyClasses_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            this.Close();
        }
    }
}
