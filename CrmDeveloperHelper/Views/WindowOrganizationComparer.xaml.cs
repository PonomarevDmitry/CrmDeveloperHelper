using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
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
    public partial class WindowOrganizationComparer : WindowBase
    {
        private readonly object sysObjectConnections = new object();
        private readonly object sysObjectArchiveConnections = new object();

        private SolutionImage _solutionImage = null;

        private readonly IWriteToOutput _iWriteToOutput;
        private ConnectionConfiguration _crmConfig;

        private readonly CommonConfiguration _commonConfig;

        public WindowOrganizationComparer(
            IWriteToOutput iWriteToOutput
            , ConnectionConfiguration crmConfig
            , CommonConfiguration commonConfig
            , string solutionImageFilePath
        )
        {
            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._crmConfig = crmConfig;
            this._commonConfig = commonConfig;

            BindingOperations.EnableCollectionSynchronization(this._crmConfig.Connections, sysObjectConnections);
            BindingOperations.EnableCollectionSynchronization(this._crmConfig.ArchiveConnections, sysObjectArchiveConnections);

            InitializeComponent();

            WindowCrmConnectionHandler.InitializeConnectionMenuItems(miConnection, this._iWriteToOutput, this._commonConfig, GetSelectedSingleConnection);

            LoadFromConfig();

            lstVwConnections.ItemsSource = this._crmConfig.Connections;

            if (!string.IsNullOrEmpty(solutionImageFilePath) && File.Exists(solutionImageFilePath))
            {
                LoadSolutionImage(solutionImageFilePath);
            }
        }

        private void LoadFromConfig()
        {
            txtBFolder.DataContext = _commonConfig;

            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _crmConfig.Save();

            BindingOperations.ClearAllBindings(lstVwConnections);

            lstVwConnections.Items.DetachFromSourceCollection();

            lstVwConnections.ItemsSource = null;

            base.OnClosed(e);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(tSProgressBar);

            UpdateButtonsConnection();
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

                    GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

                    {
                        bool enabled = connection1 != null && connection2 == null;

                        UIElement[] list = { tSBEdit, tSBCreateCopy, tSBMoveToArchive, tSBUp, tSBDown, miConnection };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled;
                        }
                    }

                    {
                        bool enabled = this.IsControlsEnabled && connection1 != null && connection2 == null;

                        UIElement[] list = { tSBTestConnection };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled;
                        }
                    }

                    {
                        bool enabled = this.IsControlsEnabled && connection1 != null && connection2 != null;

                        UIElement[] list = { tSDDBCompareOrganizations, tSDDBTransfer };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled;
                        }

                        if (enabled)
                        {
                            tSMITransferAuditFrom1To2.Header = string.Format("Audit from {0} to {1}", connection1.Name, connection2.Name);
                            tSMITransferAuditFrom2To1.Header = string.Format("Audit from {1} to {0}", connection1.Name, connection2.Name);

                            tSMITransferWorkflowsStatesFrom1To2.Header = string.Format("Workflows States from {0} to {1}", connection1.Name, connection2.Name);
                            tSMITransferWorkflowsStatesFrom2To1.Header = string.Format("Workflows States from {1} to {0}", connection1.Name, connection2.Name);

                            tSMITransferPluginStepsStatesFrom1To2.Header = string.Format("Plugin Steps States from {0} to {1}", connection1.Name, connection2.Name);
                            tSMITransferPluginStepsStatesFrom2To1.Header = string.Format("Plugin Steps States from {1} to {0}", connection1.Name, connection2.Name);
                        }
                    }

                    {
                        bool enabled = connection1 != null && connection2 != null;

                        UIElement[] list = { tSDDBShowDifference };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled;
                        }
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        #region Кнопки управления подключениями.

        private void tSBCreate_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = new ConnectionData();

            var form = new WindowCrmConnectionCard(this._iWriteToOutput, connectionData, _crmConfig.Users);

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

            this._crmConfig.Save();
        }

        private void tSBEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                var form = new WindowCrmConnectionCard(this._iWriteToOutput, connectionData, _crmConfig.Users);

                form.ShowDialog();

                connectionData.Save();
                this._crmConfig.Save();
            }
        }

        private void tSBCreateCopy_Click(object sender, RoutedEventArgs e)
        {
            if (lstVwConnections.SelectedItems.Count == 1)
            {
                ConnectionData oldconnectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

                ConnectionData connectionData = oldconnectionData.CreateCopy();

                var form = new WindowCrmConnectionCard(this._iWriteToOutput, connectionData, _crmConfig.Users);

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

                this._crmConfig.Save();
            }
        }

        private void tSBLoadFromFile_Click(object sender, RoutedEventArgs e)
        {
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
                    _iWriteToOutput.ActivateOutputWindow(null);
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
                _iWriteToOutput.ActivateOutputWindow(null);

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

            this._crmConfig.Save();
        }

        private async void tSBTestConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (lstVwConnections.SelectedItems.Count != 1)
            {
                return;
            }

            ConnectionData connectionData = lstVwConnections.SelectedItems[0] as ConnectionData;

            ToggleControls(null, false, Properties.WindowStatusStrings.StartTestingConnectionFormat1, connectionData.Name);

            var testResult = await QuickConnection.TestConnectAsync(connectionData, this._iWriteToOutput);

            if (testResult)
            {
                connectionData.Save();
                this._crmConfig.Save();

                ToggleControls(connectionData, true, Properties.WindowStatusStrings.ConnectedSuccessfullyFormat1, connectionData.Name);
            }
            else
            {
                ToggleControls(connectionData, true, Properties.WindowStatusStrings.ConnectionFailedFormat1, connectionData.Name);
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

                    this._crmConfig.Save();
                }
            }
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
            }
        }

        private void tSBSelectConnectionFileInFolder_Click(object sender, RoutedEventArgs e)
        {
            var connectionData = ((FrameworkElement)e.OriginalSource).DataContext as ConnectionData;

            if (connectionData == null)
            {
                return;
            }

            _iWriteToOutput.SelectFileInFolder(connectionData, connectionData.Path);
        }

        private void tSBCopyConnectionId_Click(object sender, RoutedEventArgs e)
        {
            var connectionData = ((FrameworkElement)e.OriginalSource).DataContext as ConnectionData;

            if (connectionData == null)
            {
                return;
            }

            Clipboard.SetText(connectionData.ConnectionId.ToString());
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

                        _iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.ConnectionIsNotSelected);
                        _iWriteToOutput.ActivateOutputWindow(null);
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        connectionData.ConnectionConfiguration = this._crmConfig;

                        _crmConfig.ArchiveConnections.Add(connectionData);
                        _crmConfig.Connections.Remove(connectionData);
                    });
                }

                this._crmConfig.Save();
            }
        }

        #endregion Кнопки управления подключениями.

        public void GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2)
        {
            connection1 = null;
            connection2 = null;

            if (lstVwConnections.SelectedItems.Count == 1)
            {
                if (lstVwConnections.SelectedItems[0] != null
                    && lstVwConnections.SelectedItems[0] is ConnectionData
                    )
                {
                    connection1 = lstVwConnections.SelectedItems[0] as ConnectionData;
                }
            }

            if (lstVwConnections.SelectedItems.Count == 2)
            {
                var list = lstVwConnections.SelectedItems.OfType<ConnectionData>().OrderBy(c => lstVwConnections.Items.IndexOf(c)).ToList();

                connection1 = list[0];
                connection2 = list[1];
            }
        }

        public ConnectionData GetSelectedSingleConnection()
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 != null && connection2 == null)
            {
                return connection1;
            }

            return null;
        }

        #region Формы сравнения.

        private void tSMIDifferenceSystemSavedQueries_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                , null
                );
        }

        private void tSMIDifferenceSystemCharts_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            string folder = string.Empty;
            txtBFolder.Dispatcher.Invoke(() =>
            {
                folder = txtBFolder.Text.Trim();
            });

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                , null
                );
        }

        private void tSMIDifferenceGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                , null
                );
        }

        private void tSMIDifferenceSystemForms_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                , null
                );
        }

        private void tSMIDifferenceWebResources_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerWebResourcesWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                );
        }

        private void tSMIDifferenceEntityMetadata_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                , null
                );
        }

        private void tSMIDifferenceApplicationRibbons_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                );
        }

        private void tSMIDifferenceSitemaps_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerSiteMapWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                );
        }

        private void tSMIDifferenceWorkflows_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                , null
                );
        }

        private void tSMIDifferenceReports_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerReportWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                );
        }

        private void tSMIDifferencePluginAssemblies_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenOrganizationComparerPluginAssemblyWindow(
                this._iWriteToOutput
                , _commonConfig
                , connection1
                , connection2
                );
        }

        #endregion Формы сравнения.

        private async Task ExecuteOperation(Func<OrganizationComparer, Task> function)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            if (MessageBox.Show(Properties.MessageBoxStrings.ExecuteOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel) != MessageBoxResult.OK)
            {
                return;
            }

            string folder = string.Empty;
            txtBFolder.Dispatcher.Invoke(() =>
            {
                folder = txtBFolder.Text.Trim();
            });

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.ComparingConnectionsFormat2, connection1.Name, connection2.Name);

            ToggleControls(null, false, Properties.WindowStatusStrings.ComparingConnectionsFormat2, connection1.Name, connection2.Name);

            try
            {
                var source = CreateOrganizationComparerSource(connection1, connection2);

                OrganizationComparer comparer = new OrganizationComparer(source, this._iWriteToOutput, folder);

                await function(comparer);

                ToggleControls(null, true, Properties.WindowStatusStrings.ComparingConnectionsFormat2, connection1.Name, connection2.Name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(null, true, Properties.WindowStatusStrings.ComparingConnectionsFormat2, connection1.Name, connection2.Name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.ComparingConnectionsFormat2, connection1.Name, connection2.Name);
        }

        private IOrganizationComparerSource CreateOrganizationComparerSource(ConnectionData connection1, ConnectionData connection2)
        {
            if (_solutionImage != null && _solutionImage.Components.Any())
            {
                return new OrganizationComparerSourceBySolutionImage(connection1, connection2, _solutionImage);
            }

            return new OrganizationComparerSource(connection1, connection2);
        }

        #region Кнопки сравнения сред.

        #region Кнопки сравнения сред несколькими процедурами.

        private void tSMICompareAll_Click(object sender, RoutedEventArgs e)
        {
            var functions = new List<Func<OrganizationComparer, Task>>()
            {
                AnalizeOrganizations

                , AnalizeGlobalOptionSets
                , AnalizeEntities
                , AnalizeEntityLabels
                , AnalizeEntityMaps

                , AnalizeSystemForms
                , AnalizeSystemSavedQueries
                , AnalizeSystemSavedQueryVisualizations

                , AnalizeDisplayStrings

                , AnalizeConnectionRoles
                , AnalizeConnectionRoleCategories

                , AnalizeWebResourcesWithDetails

                , AnalizeSitemaps

                , AnalizeDefaultTranslations
                , AnalizeFieldTranslations

                , AnalizeWorkflowsStates
                , AnalizeWorkflowsWithDetails

                , AnalizeReports

                , AnalizeEmailTemplate
                , AnalizeMailMergeTemplates
                , AnalizeKBArticleTemplates
                , AnalizeContractTemplate

                , AnalizeSecurityRoles
                , AnalizeFieldSecurityProfiles

                , AnalizePluginAssemblies
                , AnalizePluginTypes

                , AnalizePluginStepsStates

                , AnalizePluginStepsByPluginTypeNames
                , AnalizePluginStepsByIds

                , AnalizeRibbonsWithDetails
            };

            ExecuteListOperation(functions);
        }

        private void tSMICompareAllEntityMetadata_Click(object sender, RoutedEventArgs e)
        {
            var functions = new List<Func<OrganizationComparer, Task>>()
            {
                AnalizeEntities
                , AnalizeEntityLabels
                , AnalizeEntityMaps
            };

            ExecuteListOperation(functions);
        }

        private void tSMICompareAllEntityObjects_Click(object sender, RoutedEventArgs e)
        {
            var functions = new List<Func<OrganizationComparer, Task>>()
            {
                AnalizeSystemForms
                , AnalizeSystemSavedQueries
                , AnalizeSystemSavedQueryVisualizations
            };

            ExecuteListOperation(functions);
        }

        private void tSMICompareAllEntityInformation_Click(object sender, RoutedEventArgs e)
        {
            var functions = new List<Func<OrganizationComparer, Task>>()
            {
                AnalizeEntities
                , AnalizeEntityLabels
                , AnalizeEntityMaps

                , AnalizeSystemForms
                , AnalizeSystemSavedQueries
                , AnalizeSystemSavedQueryVisualizations

                , AnalizeDisplayStrings

                , AnalizeRibbonsWithDetails
            };

            ExecuteListOperation(functions);
        }

        private void tSMICompareAllTranslations_Click(object sender, RoutedEventArgs e)
        {
            var functions = new List<Func<OrganizationComparer, Task>>()
            {
                AnalizeDefaultTranslations
                , AnalizeFieldTranslations
            };

            ExecuteListOperation(functions);
        }

        private void tSMICompareAllTemplates_Click(object sender, RoutedEventArgs e)
        {
            var functions = new List<Func<OrganizationComparer, Task>>()
            {
                AnalizeEmailTemplate
                , AnalizeMailMergeTemplates
                , AnalizeKBArticleTemplates
                , AnalizeContractTemplate
            };

            ExecuteListOperation(functions);
        }

        private void tSMICompareAllConnectionInformation_Click(object sender, RoutedEventArgs e)
        {
            var functions = new List<Func<OrganizationComparer, Task>>()
            {
                AnalizeConnectionRoles
                , AnalizeConnectionRoleCategories
            };

            ExecuteListOperation(functions);
        }

        private void tSMICompareAllPluginInformation_Click(object sender, RoutedEventArgs e)
        {
            var functions = new List<Func<OrganizationComparer, Task>>()
            {
                AnalizePluginAssemblies
                , AnalizePluginTypes

                , AnalizePluginStepsStates

                , AnalizePluginStepsByPluginTypeNames
                , AnalizePluginStepsByIds
            };

            ExecuteListOperation(functions);
        }

        private void tSMICompareAllSecurity_Click(object sender, RoutedEventArgs e)
        {
            var functions = new List<Func<OrganizationComparer, Task>>()
            {
                AnalizeSecurityRoles
                , AnalizeFieldSecurityProfiles
            };

            ExecuteListOperation(functions);
        }

        private async Task ExecuteListOperation(List<Func<OrganizationComparer, Task>> functions)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            if (MessageBox.Show(Properties.MessageBoxStrings.ExecuteOperations, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel) != MessageBoxResult.OK)
            {
                return;
            }

            string folder = string.Empty;
            txtBFolder.Dispatcher.Invoke(() =>
            {
                folder = txtBFolder.Text.Trim();
            });

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.ComparingConnectionsFormat2, connection1.Name, connection2.Name);

            try
            {
                var source = CreateOrganizationComparerSource(connection1, connection2);

                ToggleControls(null, false, Properties.WindowStatusStrings.ComparingConnectionsFormat2, connection1.Name, connection2.Name);

                OrganizationComparer comparer = new OrganizationComparer(source, this._iWriteToOutput, folder);

                await MultipleAnalize(functions, comparer);

                ToggleControls(null, true, Properties.WindowStatusStrings.ComparingConnectionsCompletedFormat2, connection1.Name, connection2.Name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(null, true, Properties.WindowStatusStrings.ComparingConnectionsFailedFormat2, connection1.Name, connection2.Name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.ComparingConnectionsFormat2, connection1.Name, connection2.Name);
        }

        private async Task MultipleAnalize(List<Func<OrganizationComparer, Task>> functions, OrganizationComparer comparer)
        {
            foreach (var function in functions)
            {
                try
                {
                    await function(comparer);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(null, ex);
                }
            }
        }

        #endregion Кнопки сравнения сред несколькими процедурами.

        private void tSMIGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeGlobalOptionSets);
        }

        private async Task AnalizeGlobalOptionSets(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingGlobalOptionSetsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckGlobalOptionSetsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Global OptionSets in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMISystemForms_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeSystemForms);
        }

        private async Task AnalizeSystemForms(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingSystemFormsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckSystemFormsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check System Forms in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMISystemSavedQueries_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeSystemSavedQueries);
        }

        private async Task AnalizeSystemSavedQueries(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingSystemSavedQueriesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckSystemSavedQueriesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check System Saved Queries in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMISystemCharts_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeSystemSavedQueryVisualizations);
        }

        private async Task AnalizeSystemSavedQueryVisualizations(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingSystemChartsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckSystemSavedQueryVisualizationsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check System Saved Query Visualizations (Charts) in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIConnectionRoles_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeConnectionRoles);
        }

        private async Task AnalizeConnectionRoles(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingConnectionRolesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckConnectionRolesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Connection Roles in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIConnectionRoleCategories_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeConnectionRoleCategories);
        }

        private async Task AnalizeConnectionRoleCategories(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingConnectionRoleCategoriesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckConnectionRoleCategoriesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Connection Role Categories in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIRibbons_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeRibbons);
        }

        private async Task AnalizeRibbons(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingRibbonsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckRibbonsAsync(false);

                this._iWriteToOutput.WriteToOutput(null, "Check Ribbons in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIRibbonsWithDetails_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeRibbonsWithDetails);
        }

        private async Task AnalizeRibbonsWithDetails(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingRibbonsWithDetailsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckRibbonsAsync(true);

                this._iWriteToOutput.WriteToOutput(null, "Check Ribbons with details in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIDisplayStrings_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeDisplayStrings);
        }

        private async Task AnalizeDisplayStrings(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingDisplayStringsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckDisplayStringsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Display Strings in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIWebResources_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeWebResources);
        }

        private async Task AnalizeWebResources(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingWebResourcesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckWebResourcesAsync(false);

                this._iWriteToOutput.WriteToOutput(null, "Check WebResources in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIWebResourcesWithDetails_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeWebResourcesWithDetails);
        }

        private async Task AnalizeWebResourcesWithDetails(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingWebResourcesWithDetailsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckWebResourcesAsync(true);

                this._iWriteToOutput.WriteToOutput(null, "Check WebResources with details in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMISitemaps_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeSitemaps);
        }

        private async Task AnalizeSitemaps(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingSitemapsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckSiteMapsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Sitemaps in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMISecurityRoles_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeSecurityRoles);
        }

        private async Task AnalizeSecurityRoles(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingSecurityRolesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckSecurityRolesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Security Roles in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIWorkflows_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeWorkflows);
        }

        private async Task AnalizeWorkflows(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingWorkflowsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckWorkflowsAsync(false);

                this._iWriteToOutput.WriteToOutput(null, "Check Workflows in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIWorkflowsWithDetails_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeWorkflowsWithDetails);
        }

        private async Task AnalizeWorkflowsWithDetails(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingWorkflowsWithDetailsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckWorkflowsAsync(true);

                this._iWriteToOutput.WriteToOutput(null, "Check Workflows with details in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIWorkflowsStates_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeWorkflowsStates);
        }

        private async Task AnalizeWorkflowsStates(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingWorkflowsStatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckWorkflowsStatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Workflows states in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIReports_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeReports);
        }

        private async Task AnalizeReports(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingReportsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckReportsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Reports in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIEmailTemplate_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeEmailTemplate);
        }

        private async Task AnalizeEmailTemplate(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingEMailTemplatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEMailTemplatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check E-Mail Templates in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIMailMergeTemplate_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeMailMergeTemplates);
        }

        private async Task AnalizeMailMergeTemplates(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingMailMergeTemplatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckMailMergeTemplatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Mail Merge Templates in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIKBArticleTemplate_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeKBArticleTemplates);
        }

        private async Task AnalizeKBArticleTemplates(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingKBArticleTemplatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckKBArticleTemplatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check KB Article Templates in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIContractTemplate_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeContractTemplate);
        }

        private async Task AnalizeContractTemplate(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingContractTemplatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckContractTemplatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Contract Templates in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIFieldSecurityProfiles_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeFieldSecurityProfiles);
        }

        private async Task AnalizeFieldSecurityProfiles(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingFieldSecurityProfilesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckFieldSecurityProfilesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Field Security Profiles in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIEntities_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeEntities);
        }

        private async Task AnalizeEntities(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingEntitiesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEntitiesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Entities in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIEntitiesByAudit_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeEntitiesByAudit);
        }

        private async Task AnalizeEntitiesByAudit(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingEntitiesByAuditFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEntitiesByAuditAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Entities by Audit in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIEntityLabels_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeEntityLabels);
        }

        private async Task AnalizeEntityLabels(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingEntityLabelsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEntityLabelsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Entity Labels in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIEntityMaps_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeEntityMaps);
        }

        private async Task AnalizeEntityMaps(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingEntityMapsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEntityMapsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Entity Maps in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIPluginAssemblies_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizePluginAssemblies);
        }

        private async Task AnalizePluginAssemblies(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingPluginAssembliesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckPluginAssembliesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Plugin Assemblies in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIPluginTypes_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizePluginTypes);
        }

        private async Task AnalizePluginTypes(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingPluginTypesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckPluginTypesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Plugin Types in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIPluginStepsByPluginTypeNames_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizePluginStepsByPluginTypeNames);
        }

        private async Task AnalizePluginStepsByPluginTypeNames(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingPluginStepsByPluginTypeNamesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckPluginStepsByPluginTypeNamesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Plugin Steps by Plugin Type Names in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIPluginStepsByIds_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizePluginStepsByIds);
        }

        private async Task AnalizePluginStepsByIds(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingPluginStepsByIdsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckPluginStepsByIdsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Plugin Steps by Ids in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIPluginStepsStates_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizePluginStepsStates);
        }

        private async Task AnalizePluginStepsStates(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingPluginStepsStatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckPluginStepsStatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Plugin Steps States in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIDefaultTranslations_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeDefaultTranslations);
        }

        private async Task AnalizeDefaultTranslations(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingDefaultTranslationsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckDefaultTranslationsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Default Translations in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIFieldTranslations_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeFieldTranslations);
        }

        private async Task AnalizeFieldTranslations(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingFieldTranslationsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckFieldTranslationsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Field Translations in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMIOrganizations_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperation(AnalizeOrganizations);
        }

        private async Task AnalizeOrganizations(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.CheckingOrganizationsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckOrganizationsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Organizations in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        #endregion Кнопки сравнения сред.

        #region Transfer Buttons.

        private async Task ExecuteOperationFrom1To2Async(Func<OrganizationCustomizationTransfer, Task> function)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            string message = string.Format(Properties.MessageBoxStrings.ExecuteTransferOperationFormat2, connection1.Name, connection2.Name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel) != MessageBoxResult.OK)
            {
                return;
            }

            string folder = string.Empty;
            txtBFolder.Dispatcher.Invoke(() =>
            {
                folder = txtBFolder.Text.Trim();
            });

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await ExecuteTrasnferOperation(function, connection1, connection2, folder);
        }

        private async Task ExecuteOperationFrom2To1Async(Func<OrganizationCustomizationTransfer, Task> function)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            if (connection1 == null || connection2 == null)
            {
                return;
            }

            string message = string.Format(Properties.MessageBoxStrings.ExecuteTransferOperationFormat2, connection2.Name, connection1.Name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel) != MessageBoxResult.OK)
            {
                return;
            }

            string folder = string.Empty;
            txtBFolder.Dispatcher.Invoke(() =>
            {
                folder = txtBFolder.Text.Trim();
            });

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await ExecuteTrasnferOperation(function, connection2, connection1, folder);
        }

        private async Task ExecuteTrasnferOperation(Func<OrganizationCustomizationTransfer, Task> function, ConnectionData connectionSource, ConnectionData connectionTarget, string folder)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.TransferingDataFormat2, connectionSource.Name, connectionTarget.Name);

            ToggleControls(null, false, Properties.WindowStatusStrings.TransferingDataFormat2, connectionSource.Name, connectionTarget.Name);

            try
            {
                var source = CreateOrganizationComparerSource(connectionSource, connectionTarget);

                var trasnferHandler = new OrganizationCustomizationTransfer(source, this._iWriteToOutput, folder);

                await function(trasnferHandler);

                ToggleControls(null, true, Properties.WindowStatusStrings.TransferingDataCompletedFormat2, connectionSource.Name, connectionTarget.Name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(null, true, Properties.WindowStatusStrings.TransferingDataFailedFormat2, connectionSource.Name, connectionTarget.Name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.TransferingDataFormat2, connectionSource.Name, connectionTarget.Name);
        }

        private void tSMITransferAuditFrom1To2_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperationFrom1To2Async(TrasnferAudit);
        }

        private void tSMITransferAuditFrom2To1_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperationFrom2To1Async(TrasnferAudit);
        }

        private async Task TrasnferAudit(OrganizationCustomizationTransfer handler)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.TransferingAuditFormat2, handler.ConnectionSource.Name, handler.ConnectionTarget.Name);

                string filePath = await handler.TrasnferAuditAsync();

                this._iWriteToOutput.WriteToOutput(null, "Transfer Audit Log from {0} to {1} exported into file {2}", handler.ConnectionSource.Name, handler.ConnectionTarget.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMITransferWorkflowsStatesFrom1To2_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperationFrom1To2Async(TrasnferWorkflowsStates);
        }

        private void tSMITransferWorkflowsStatesFrom2To1_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperationFrom2To1Async(TrasnferWorkflowsStates);
        }

        private async Task TrasnferWorkflowsStates(OrganizationCustomizationTransfer handler)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.TransferingWorkflowsStatesFormat2, handler.ConnectionSource.Name, handler.ConnectionTarget.Name);

                string filePath = await handler.TrasnferWorkflowsStatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Transfer Workflows States from {0} to {1} exported into file {2}", handler.ConnectionSource.Name, handler.ConnectionTarget.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void tSMITransferPluginStepsStatesFrom1To2_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperationFrom1To2Async(TrasnferPluginStepsStates);
        }

        private void tSMITransferPluginStepsStatesFrom2To1_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOperationFrom2To1Async(TrasnferPluginStepsStates);
        }

        private async Task TrasnferPluginStepsStates(OrganizationCustomizationTransfer handler)
        {
            try
            {
                UpdateStatus(null, Properties.WindowStatusStrings.TransferingPluginStepsStatesFormat2, handler.ConnectionSource.Name, handler.ConnectionTarget.Name);

                string filePath = await handler.TrasnferPluginStepsStatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Transfer Plugin Steps States from {0} to {1} exported into file {2}", handler.ConnectionSource.Name, handler.ConnectionTarget.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        #endregion Transfer Buttons.

        private void tSBLoadSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = string.Empty;
            var t = new Thread(() =>
            {
                try
                {
                    var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
                    {
                        Filter = "SolutionImage (.xml)|*.xml",
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
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (!string.IsNullOrEmpty(selectedPath))
            {
                LoadSolutionImage(selectedPath);
            }
        }

        private async Task LoadSolutionImage(string filePath)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this._solutionImage = null;
            txtBFilePath.Text = string.Empty;

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            ToggleControls(null, false, Properties.WindowStatusStrings.LoadingSolutionImage);

            try
            {
                this._solutionImage = await SolutionImage.LoadAsync(filePath);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                this._solutionImage = null;
            }

            txtBFilePath.Dispatcher.Invoke(() =>
            {
                if (this._solutionImage != null)
                {
                    txtBFilePath.Text = filePath;
                }
                else
                {
                    txtBFilePath.Text = string.Empty;
                }
            });

            if (this._solutionImage == null)
            {
                ToggleControls(null, true, Properties.WindowStatusStrings.LoadingSolutionImageFailed);
                return;
            }

            ToggleControls(null, true, Properties.WindowStatusStrings.LoadingSolutionImageCompleted);
        }

        private void tSBClearSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            this._solutionImage = null;
            txtBFilePath.Text = string.Empty;
        }
    }
}