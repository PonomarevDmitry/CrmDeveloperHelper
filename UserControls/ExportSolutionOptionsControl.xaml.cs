using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    public partial class ExportSolutionOptionsControl : UserControl
    {
        private CommonConfiguration _commonConfig;
        private ComboBox _cmBCurrentConnection;
        private Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        private string _textUniqueName;
        private string _textDisplayName;

        public ExportSolutionOptionsControl(CommonConfiguration commonConfig, ComboBox cmBCurrentConnection)
        {
            InitializeComponent();

            this._commonConfig = commonConfig;
            this._cmBCurrentConnection = cmBCurrentConnection;

            if (_cmBCurrentConnection.SelectedItem is ConnectionData connectionData)
            {
                BindCollections(connectionData);

                cmBUniqueName.Text = connectionData.ExportSolutionOverrideUniqueName;
                cmBDisplayName.Text = connectionData.ExportSolutionOverrideDisplayName;
                cmBVersion.Text = connectionData.ExportSolutionOverrideVersion;
            }

            cmBCurrentConnection.SelectionChanged += CmBCurrentConnection_SelectionChanged;

            LoadFromConfig();
        }

        private void CmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindCollections(_cmBCurrentConnection.SelectedItem as ConnectionData);
        }

        private void BindCollections(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return;
            }

            if (!_syncCacheObjects.ContainsKey(connectionData.ConnectionId))
            {
                _syncCacheObjects.Add(connectionData.ConnectionId, new object());

                BindingOperations.EnableCollectionSynchronization(connectionData.LastExportSolutionOverrideUniqueName, _syncCacheObjects[connectionData.ConnectionId]);
                BindingOperations.EnableCollectionSynchronization(connectionData.LastExportSolutionOverrideDisplayName, _syncCacheObjects[connectionData.ConnectionId]);
                BindingOperations.EnableCollectionSynchronization(connectionData.LastExportSolutionOverrideVersion, _syncCacheObjects[connectionData.ConnectionId]);
            }
        }

        private void LoadFromConfig()
        {
            chBAutoNumbering.DataContext = _commonConfig;
            chBCalendar.DataContext = _commonConfig;
            chBCustomization.DataContext = _commonConfig;
            chBEmailTracking.DataContext = _commonConfig;
            chBExternalApplications.DataContext = _commonConfig;
            chBGeneral.DataContext = _commonConfig;
            chBISVConfig.DataContext = _commonConfig;
            chBMarketing.DataContext = _commonConfig;
            chBOutlookSynchronization.DataContext = _commonConfig;
            chBRelashionshipRoles.DataContext = _commonConfig;
            chBSales.DataContext = _commonConfig;

            chBManaged.DataContext = _cmBCurrentConnection;

            chBOverrideSolutionNameAndVersion.DataContext = _cmBCurrentConnection;
            chBOverrideSolutionDescription.DataContext = _cmBCurrentConnection;

            chBCreateFolderForVersion.DataContext = _cmBCurrentConnection;
            chBCopyFileToClipBoard.DataContext = _cmBCurrentConnection;

            cmBUniqueName.DataContext = _cmBCurrentConnection;
            cmBDisplayName.DataContext = _cmBCurrentConnection;
            cmBVersion.DataContext = _cmBCurrentConnection;
            txtBDescription.DataContext = _cmBCurrentConnection;
        }

        public void DetachCollections()
        {
            BindingOperations.ClearAllBindings(cmBUniqueName);
            BindingOperations.ClearAllBindings(cmBDisplayName);
            BindingOperations.ClearAllBindings(cmBVersion);

            cmBUniqueName.Items.DetachFromSourceCollection();
            cmBDisplayName.Items.DetachFromSourceCollection();
            cmBVersion.Items.DetachFromSourceCollection();

            cmBUniqueName.ItemsSource = null;
            cmBDisplayName.ItemsSource = null;
            cmBVersion.ItemsSource = null;
        }

        public void StoreTextValues()
        {
            this._textUniqueName = cmBUniqueName.Text;
            this._textDisplayName = cmBDisplayName.Text;
        }

        public void RestoreTextValues()
        {
            cmBUniqueName.Text = _textUniqueName;
            cmBDisplayName.Text = _textDisplayName;
        }

        public void SetNewVersion(string newVersion)
        {
            cmBVersion.Text = newVersion;
        }

        public event EventHandler<EventArgs> CloseClicked;

        public void OnCloseClicked()
        {
            CloseClicked?.Invoke(this, new EventArgs());
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                if (e.Key == Key.Escape
                    || (e.Key == Key.W && e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    )
                {
                    e.Handled = true;

                    OnCloseClicked();

                    return;
                }
            }

            base.OnKeyDown(e);
        }
    }
}