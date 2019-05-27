using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    public partial class ExportSolutionOptionsControl : UserControl
    {
        private readonly ComboBox _cmBCurrentConnection;
        private readonly ComboBox _cmBExportSolutionProfile;

        private readonly Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        private readonly Dictionary<ExportSolutionProfile, object> _syncCacheProfiles = new Dictionary<ExportSolutionProfile, object>();

        private string _textUniqueName;
        private string _textDisplayName;
        private string _textExportFolder;
        private bool _bindLastSolutionExportFolders;

        public ExportSolutionOptionsControl(ComboBox cmBCurrentConnection, ComboBox cmBExportSolutionProfile)
        {
            InitializeComponent();

            this._cmBCurrentConnection = cmBCurrentConnection;
            this._cmBExportSolutionProfile = cmBExportSolutionProfile;

            BindCollections(_cmBCurrentConnection.SelectedItem as ConnectionData);

            LoadFromConfig();

            if (_cmBExportSolutionProfile.SelectedItem is ExportSolutionProfile exportSolutionProfile)
            {
                cmBUniqueName.Text = exportSolutionProfile.OverrideUniqueName;
                cmBDisplayName.Text = exportSolutionProfile.OverrideDisplayName;
                cmBVersion.Text = exportSolutionProfile.OverrideVersion;

                //cmBExportFolder.Text = exportSolutionProfile.ExportFolder;
            }

            cmBCurrentConnection.SelectionChanged += cmBCurrentConnection_SelectionChanged;
            cmBExportSolutionProfile.SelectionChanged += cmBExportSolutionProfile_SelectionChanged;
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems.OfType<ConnectionData>())
            {
                removed.Save();
            }

            if (_cmBCurrentConnection.SelectedItem is ConnectionData connectionData)
            {
                BindCollections(connectionData);

                if (this._bindLastSolutionExportFolders)
                {
                    string text = cmBExportFolder.Text;

                    cmBExportFolder.ItemsSource = connectionData.LastSolutionExportFolders;

                    cmBExportFolder.Text = text;
                }
            }
        }

        private void cmBExportSolutionProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cmBExportSolutionProfile.SelectedItem is ExportSolutionProfile exportSolutionProfile)
            {
                BindCollections(exportSolutionProfile);
            }
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

                BindingOperations.EnableCollectionSynchronization(connectionData.LastSolutionExportFolders, _syncCacheObjects[connectionData.ConnectionId]);
                BindingOperations.EnableCollectionSynchronization(connectionData.ExportSolutionProfileList, _syncCacheObjects[connectionData.ConnectionId]);
            }
        }

        private void BindCollections(ExportSolutionProfile exportSolutionProfile)
        {
            if (exportSolutionProfile == null)
            {
                return;
            }

            if (!_syncCacheProfiles.ContainsKey(exportSolutionProfile))
            {
                _syncCacheProfiles.Add(exportSolutionProfile, new object());

                BindingOperations.EnableCollectionSynchronization(exportSolutionProfile.LastOverrideUniqueName, _syncCacheProfiles[exportSolutionProfile]);
                BindingOperations.EnableCollectionSynchronization(exportSolutionProfile.LastOverrideDisplayName, _syncCacheProfiles[exportSolutionProfile]);
                BindingOperations.EnableCollectionSynchronization(exportSolutionProfile.LastOverrideVersion, _syncCacheProfiles[exportSolutionProfile]);
            }
        }

        private void LoadFromConfig()
        {
            chBAutoNumbering.DataContext = _cmBExportSolutionProfile;
            chBCalendar.DataContext = _cmBExportSolutionProfile;
            chBCustomization.DataContext = _cmBExportSolutionProfile;
            chBEmailTracking.DataContext = _cmBExportSolutionProfile;
            chBExternalApplications.DataContext = _cmBExportSolutionProfile;
            chBGeneral.DataContext = _cmBExportSolutionProfile;
            chBISVConfig.DataContext = _cmBExportSolutionProfile;
            chBMarketing.DataContext = _cmBExportSolutionProfile;
            chBOutlookSynchronization.DataContext = _cmBExportSolutionProfile;
            chBRelashionshipRoles.DataContext = _cmBExportSolutionProfile;
            chBSales.DataContext = _cmBExportSolutionProfile;

            chBIsManaged.DataContext = _cmBExportSolutionProfile;

            chBOverrideSolutionNameAndVersion.DataContext = _cmBExportSolutionProfile;
            chBOverrideSolutionDescription.DataContext = _cmBExportSolutionProfile;

            chBCreateFolderForVersion.DataContext = _cmBExportSolutionProfile;
            chBCopyFileToClipBoard.DataContext = _cmBExportSolutionProfile;

            cmBUniqueName.DataContext = _cmBExportSolutionProfile;
            cmBDisplayName.DataContext = _cmBExportSolutionProfile;
            cmBVersion.DataContext = _cmBExportSolutionProfile;

            txtBDescription.DataContext = _cmBExportSolutionProfile;
        }

        public void DetachCollections()
        {
            cmBUniqueName.DataContext = null;
            cmBDisplayName.DataContext = null;
            cmBVersion.DataContext = null;
            cmBExportFolder.DataContext = null;

            BindingOperations.ClearAllBindings(cmBUniqueName);
            BindingOperations.ClearAllBindings(cmBDisplayName);
            BindingOperations.ClearAllBindings(cmBVersion);
            BindingOperations.ClearAllBindings(cmBExportFolder);

            cmBUniqueName.Items.DetachFromSourceCollection();
            cmBDisplayName.Items.DetachFromSourceCollection();
            cmBVersion.Items.DetachFromSourceCollection();
            cmBExportFolder.Items.DetachFromSourceCollection();

            cmBUniqueName.ItemsSource = null;
            cmBDisplayName.ItemsSource = null;
            cmBVersion.ItemsSource = null;
            cmBExportFolder.ItemsSource = null;
        }

        public void StoreTextValues()
        {
            this._textUniqueName = cmBUniqueName.Text;
            this._textDisplayName = cmBDisplayName.Text;
            this._textExportFolder = cmBExportFolder.Text;
        }

        public void RestoreTextValues()
        {
            cmBUniqueName.Text = _textUniqueName;
            cmBDisplayName.Text = _textDisplayName;
            cmBExportFolder.Text = _textExportFolder;
        }

        public void SetNewVersion(string newVersion)
        {
            if (_cmBExportSolutionProfile.SelectedItem is ExportSolutionProfile exportSolutionProfile)
            {
                exportSolutionProfile.OverrideVersion = newVersion;
            }

            cmBVersion.Text = newVersion;
        }

        public event EventHandler<EventArgs> CloseClicked;

        private void OnCloseClicked()
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

        public void SetExportFolderReadOnly(string exportFolder)
        {
            cmBExportFolder.IsReadOnly = true;
            cmBExportFolder.Text = exportFolder;
        }

        public void BindExportFolder()
        {
            //Text="{Binding Path=SelectedItem.ExportSolutionFolder}" ItemsSource="{Binding Path=SelectedItem.LastSolutionExportFolders}" 
            {
                Binding binding = new Binding
                {
                    Path = new PropertyPath(string.Format("{0}.{1}", nameof(ComboBox.SelectedItem), nameof(ExportSolutionConfig.ExportFolder))),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                };
                BindingOperations.SetBinding(cmBExportFolder, ComboBox.TextProperty, binding);
            }

            cmBExportFolder.DataContext = _cmBExportSolutionProfile;

            this._bindLastSolutionExportFolders = true;
        }

        public string GetExportFolder()
        {
            return cmBExportFolder.Text?.Trim();
        }
    }
}