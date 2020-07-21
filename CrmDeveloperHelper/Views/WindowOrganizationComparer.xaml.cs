using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
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

        private readonly List<SolutionImage> _solutionImageList = new List<SolutionImage>();

        private readonly IWriteToOutput _iWriteToOutput;
        private ConnectionConfiguration _crmConfig;

        private readonly CommonConfiguration _commonConfig;

        private Dictionary<OrganizationComparerOperation, Func<OrganizationComparer, Task>> _operationHandlers;

        public WindowOrganizationComparer(
            IWriteToOutput iWriteToOutput
            , ConnectionConfiguration crmConfig
            , CommonConfiguration commonConfig
            , SolutionImage solutionImage
        )
        {
            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._crmConfig = crmConfig;
            this._commonConfig = commonConfig;

            BindingOperations.EnableCollectionSynchronization(this._crmConfig.Connections, sysObjectConnections);
            BindingOperations.EnableCollectionSynchronization(this._crmConfig.ArchiveConnections, sysObjectArchiveConnections);

            InitializeComponent();

            FillOperationHandlers();

            WindowCrmConnectionHandler.InitializeConnectionMenuItems(miConnection, this._iWriteToOutput, this._commonConfig, GetSelectedSingleConnection);

            LoadFromConfig();

            lstVwConnections.ItemsSource = this._crmConfig.Connections;

            if (solutionImage != null)
            {
                _solutionImageList.Insert(0, solutionImage);

                FillSolutionImages(solutionImage);
            }

            FillExplorersMenuItems();
        }

        private void FillExplorersMenuItems()
        {
            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, GetConnections);
            compareWindowsHelper.FillCompareWindows(tSDDBShowDifference);
        }

        private void FillOperationHandlers()
        {
            _operationHandlers = new Dictionary<OrganizationComparerOperation, Func<OrganizationComparer, Task>>()
            {
                { OrganizationComparerOperation.Organizations, AnalizeOrganizations },

                { OrganizationComparerOperation.Entities, AnalizeEntities },

                { OrganizationComparerOperation.EntitiesByAudit, AnalizeEntitiesByAudit },

                { OrganizationComparerOperation.EntityLabels, AnalizeEntityLabels },

                { OrganizationComparerOperation.EntityMaps, AnalizeEntityMaps },

                { OrganizationComparerOperation.EntityRibbons, AnalizeEntityRibbons },

                { OrganizationComparerOperation.EntityRibbonsWithDetails, AnalizeEntityRibbonsWithDetails },

                { OrganizationComparerOperation.ConnectionRoleCategories, AnalizeConnectionRoleCategories },

                { OrganizationComparerOperation.ConnectionRoles, AnalizeConnectionRoles },

                { OrganizationComparerOperation.ContractTemplates, AnalizeContractTemplates },

                { OrganizationComparerOperation.DisplayStrings, AnalizeDisplayStrings },

                { OrganizationComparerOperation.EmailTemplates, AnalizeEmailTemplates },

                { OrganizationComparerOperation.FieldSecurityProfiles, AnalizeFieldSecurityProfiles },

                { OrganizationComparerOperation.GlobalOptionSets, AnalizeGlobalOptionSets },

                { OrganizationComparerOperation.KBArticleTemplates, AnalizeKBArticleTemplates },

                { OrganizationComparerOperation.MailMergeTemplates, AnalizeMailMergeTemplates },

                { OrganizationComparerOperation.PluginAssemblies, AnalizePluginAssemblies },

                { OrganizationComparerOperation.PluginSteps, AnalizePluginSteps },

                { OrganizationComparerOperation.PluginStepsByStates, AnalizePluginStepsByStates },

                { OrganizationComparerOperation.PluginTypes, AnalizePluginTypes },

                { OrganizationComparerOperation.Reports, AnalizeReports },

                { OrganizationComparerOperation.SecurityRoles, AnalizeSecurityRoles },

                { OrganizationComparerOperation.SiteMaps, AnalizeSiteMaps },

                { OrganizationComparerOperation.SystemCharts, AnalizeSystemSavedQueryVisualizations },

                { OrganizationComparerOperation.SystemForms, AnalizeSystemForms },

                { OrganizationComparerOperation.SystemViews, AnalizeSystemSavedQueries },

                { OrganizationComparerOperation.WebResources, AnalizeWebResources },

                { OrganizationComparerOperation.WebResourcesWithDetails, AnalizeWebResourcesWithDetails },

                { OrganizationComparerOperation.WorkflowsAttributes, AnalizeWorkflowsAttributes },

                { OrganizationComparerOperation.WorkflowsAttributesWithDetails, AnalizeWorkflowsAttributesWithDetails },

                { OrganizationComparerOperation.WorkflowsStates, AnalizeWorkflowsStates },

                { OrganizationComparerOperation.ApplicationRibbons, AnalizeApplicationRibbons },

                { OrganizationComparerOperation.ApplicationRibbonsWithDetails, AnalizeApplicationRibbonsWithDetails },
            };
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

                        UIElement[] list = { tSBEdit, tSBCreateCopy, tSBMoveToArchive, tSBUp, tSBDown, miConnection, miLoadSolutionImageFromZipFile, miSelectSolutionImageFromConnectionSolution };

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

                        UIElement[] list = { miCompareOrganizations, tSDDBTransfer };

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

                CallEditConnection(connectionData);
            }
        }

        private void CallEditConnection(ConnectionData connectionData)
        {
            var form = new WindowCrmConnectionCard(_iWriteToOutput, connectionData, _crmConfig.Users);

            form.ShowDialog();

            connectionData.Save();
            this._crmConfig.Save();
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

            ToggleControls(null, false, Properties.OutputStrings.StartTestingConnectionFormat1, connectionData.Name);

            var testResult = await QuickConnection.TestConnectAsync(connectionData, this._iWriteToOutput, this);

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

                        _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
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

        private Tuple<ConnectionData, ConnectionData> GetConnections()
        {
            GetSelectedConnections(out ConnectionData connection1, out ConnectionData connection2);

            return Tuple.Create(connection1, connection2);
        }

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

        private IOrganizationComparerSource CreateOrganizationComparerSource(ConnectionData connection1, ConnectionData connection2)
        {
            if (cmBSolutionImage.SelectedItem is SolutionImage solutionImage)
            {
                return new OrganizationComparerSourceBySolutionImage(connection1, connection2, solutionImage);
            }

            return new OrganizationComparerSource(connection1, connection2);
        }

        #region Кнопки сравнения сред.

        private async void miCompareOrganizations_Click(object sender, RoutedEventArgs e)
        {
            var form = new WindowOrganizationComparerOperationMultiSelect();

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            List<OrganizationComparerOperation> operations = form.GetOperations();

            if (!operations.Any())
            {
                return;
            }

            await ExecuteListOperation(operations);
        }

        private async Task ExecuteListOperation(IEnumerable<OrganizationComparerOperation> operations)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (!operations.Any())
            {
                return;
            }

            var functions = operations.Where(o => _operationHandlers.ContainsKey(o)).Select(o => _operationHandlers[o]).ToList();

            if (!functions.Any())
            {
                return;
            }

            await ExecuteListOperation(functions);
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

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.ComparingConnectionsFormat2, connection1.Name, connection2.Name);

            try
            {
                var source = CreateOrganizationComparerSource(connection1, connection2);

                ToggleControls(null, false, Properties.OutputStrings.ComparingConnectionsFormat2, connection1.Name, connection2.Name);

                OrganizationComparer comparer = new OrganizationComparer(source, this._iWriteToOutput, folder);

                await MultipleAnalize(functions, comparer);

                ToggleControls(null, true, Properties.OutputStrings.ComparingConnectionsCompletedFormat2, connection1.Name, connection2.Name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(null, true, Properties.OutputStrings.ComparingConnectionsFailedFormat2, connection1.Name, connection2.Name);
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

        private async Task AnalizeGlobalOptionSets(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingGlobalOptionSetsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckGlobalOptionSetsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Global OptionSets in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeSystemForms(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingSystemFormsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckSystemFormsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check System Forms in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeSystemSavedQueries(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingSystemSavedQueriesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckSystemSavedQueriesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check System Saved Queries in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeSystemSavedQueryVisualizations(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingSystemChartsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckSystemSavedQueryVisualizationsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check System Saved Query Visualizations (Charts) in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeConnectionRoles(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingConnectionRolesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckConnectionRolesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Connection Roles in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeConnectionRoleCategories(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingConnectionRoleCategoriesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckConnectionRoleCategoriesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Connection Role Categories in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeEntityRibbons(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingEntityRibbonsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEntityRibbonsAsync(false);

                this._iWriteToOutput.WriteToOutput(null, "Check Entity Ribbons in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeEntityRibbonsWithDetails(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingEntityRibbonsWithDetailsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEntityRibbonsAsync(true);

                this._iWriteToOutput.WriteToOutput(null, "Check Entity Ribbons with details in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeApplicationRibbons(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingApplicationRibbonsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckApplicationRibbonsAsync(false);

                this._iWriteToOutput.WriteToOutput(null, "Check Application Ribbons in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeApplicationRibbonsWithDetails(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingApplicationRibbonsWithDetailsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckApplicationRibbonsAsync(true);

                this._iWriteToOutput.WriteToOutput(null, "Check Application Ribbons with details in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeDisplayStrings(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingDisplayStringsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckDisplayStringsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Display Strings in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeWebResources(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingWebResourcesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckWebResourcesAsync(false);

                this._iWriteToOutput.WriteToOutput(null, "Check WebResources in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeWebResourcesWithDetails(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingWebResourcesWithDetailsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckWebResourcesAsync(true);

                this._iWriteToOutput.WriteToOutput(null, "Check WebResources with details in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeSiteMaps(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingSiteMapsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckSiteMapsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check SiteMaps in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeSecurityRoles(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingSecurityRolesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckSecurityRolesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Security Roles in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeWorkflowsAttributes(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingWorkflowsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckWorkflowsAsync(false);

                this._iWriteToOutput.WriteToOutput(null, "Check Workflows in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeWorkflowsAttributesWithDetails(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingWorkflowsWithDetailsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckWorkflowsAsync(true);

                this._iWriteToOutput.WriteToOutput(null, "Check Workflows with details in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeWorkflowsStates(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingWorkflowsStatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckWorkflowsStatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Workflows states in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeReports(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingReportsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckReportsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Reports in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeEmailTemplates(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingEMailTemplatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEMailTemplatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check E-Mail Templates in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeMailMergeTemplates(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingMailMergeTemplatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckMailMergeTemplatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Mail Merge Templates in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeKBArticleTemplates(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingKBArticleTemplatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckKBArticleTemplatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check KB Article Templates in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeContractTemplates(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingContractTemplatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckContractTemplatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Contract Templates in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeFieldSecurityProfiles(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingFieldSecurityProfilesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckFieldSecurityProfilesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Field Security Profiles in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeEntities(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingEntitiesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEntitiesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Entities in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeEntitiesByAudit(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingEntitiesByAuditFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEntitiesByAuditAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Entities by Audit in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeEntityLabels(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingEntityLabelsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEntityLabelsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Entity Labels in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeEntityMaps(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingEntityMapsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckEntityMapsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Entity Maps in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizePluginAssemblies(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingPluginAssembliesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckPluginAssembliesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Plugin Assemblies in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizePluginTypes(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingPluginTypesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckPluginTypesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Plugin Types in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizePluginSteps(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingPluginStepsByIdsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckPluginStepsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Plugin Steps in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizePluginStepsByStates(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingPluginStepsStatesFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckPluginStepsStatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Plugin Steps States in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeOrganizations(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingOrganizationsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckOrganizationsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Organizations in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeDefaultTranslations(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingDefaultTranslationsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckDefaultTranslationsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Default Translations in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task AnalizeFieldTranslations(OrganizationComparer comparer)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.CheckingFieldTranslationsFormat2, comparer.Connection1.Name, comparer.Connection2.Name);

                string filePath = await comparer.CheckFieldTranslationsAsync();

                this._iWriteToOutput.WriteToOutput(null, "Check Field Translations in {0} and {1} exported into file {2}", comparer.Connection1.Name, comparer.Connection2.Name, filePath);

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

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

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

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await ExecuteTrasnferOperation(function, connection2, connection1, folder);
        }

        private async Task ExecuteTrasnferOperation(Func<OrganizationCustomizationTransfer, Task> function, ConnectionData connectionSource, ConnectionData connectionTarget, string folder)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.TransferingDataFormat2, connectionSource.Name, connectionTarget.Name);

            ToggleControls(null, false, Properties.OutputStrings.TransferingDataFormat2, connectionSource.Name, connectionTarget.Name);

            try
            {
                var source = CreateOrganizationComparerSource(connectionSource, connectionTarget);

                var trasnferHandler = new OrganizationCustomizationTransfer(source, this._iWriteToOutput, folder);

                await function(trasnferHandler);

                ToggleControls(null, true, Properties.OutputStrings.TransferingDataCompletedFormat2, connectionSource.Name, connectionTarget.Name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(null, true, Properties.OutputStrings.TransferingDataFailedFormat2, connectionSource.Name, connectionTarget.Name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.TransferingDataFormat2, connectionSource.Name, connectionTarget.Name);
        }

        private async void tSMITransferAuditFrom1To2_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteOperationFrom1To2Async(TrasnferAudit);
        }

        private async void tSMITransferAuditFrom2To1_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteOperationFrom2To1Async(TrasnferAudit);
        }

        private async Task TrasnferAudit(OrganizationCustomizationTransfer handler)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.TransferingAuditFormat2, handler.ConnectionSource.Name, handler.ConnectionTarget.Name);

                string filePath = await handler.TrasnferAuditAsync();

                this._iWriteToOutput.WriteToOutput(null, "Transfer Audit Log from {0} to {1} exported into file {2}", handler.ConnectionSource.Name, handler.ConnectionTarget.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async void tSMITransferWorkflowsStatesFrom1To2_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteOperationFrom1To2Async(TrasnferWorkflowsStates);
        }

        private async void tSMITransferWorkflowsStatesFrom2To1_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteOperationFrom2To1Async(TrasnferWorkflowsStates);
        }

        private async Task TrasnferWorkflowsStates(OrganizationCustomizationTransfer handler)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.TransferingWorkflowsStatesFormat2, handler.ConnectionSource.Name, handler.ConnectionTarget.Name);

                string filePath = await handler.TrasnferWorkflowsStatesAsync();

                this._iWriteToOutput.WriteToOutput(null, "Transfer Workflows States from {0} to {1} exported into file {2}", handler.ConnectionSource.Name, handler.ConnectionTarget.Name, filePath);

                this._iWriteToOutput.PerformAction(null, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async void tSMITransferPluginStepsStatesFrom1To2_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteOperationFrom1To2Async(TrasnferPluginStepsStates);
        }

        private async void tSMITransferPluginStepsStatesFrom2To1_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteOperationFrom2To1Async(TrasnferPluginStepsStates);
        }

        private async Task TrasnferPluginStepsStates(OrganizationCustomizationTransfer handler)
        {
            try
            {
                UpdateStatus(null, Properties.OutputStrings.TransferingPluginStepsStatesFormat2, handler.ConnectionSource.Name, handler.ConnectionTarget.Name);

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

        private async void tSBLoadSolutionImageFromFile_Click(object sender, RoutedEventArgs e)
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
                await LoadSolutionImage(selectedPath);
            }
        }

        private async void tSBLoadSolutionImageFromZipFile_Click(object sender, RoutedEventArgs e)
        {
            var connectionData = GetSelectedSingleConnection();

            if (connectionData == null)
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
                        Filter = "Solution (.zip)|*.zip",
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
                await LoadSolutionImageFromZip(connectionData, selectedPath);
            }
        }

        private async void miSelectSolutionImageFromConnectionSolution_Click(object sender, RoutedEventArgs e)
        {
            var connectionData = GetSelectedSingleConnection();

            if (connectionData == null)
            {
                return;
            }

            ToggleControls(connectionData, false, string.Empty);

            IOrganizationServiceExtented service = null;

            try
            {
                service = await QuickConnection.ConnectAndWriteToOutputAsync(_iWriteToOutput, connectionData);

                if (service == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                ToggleControls(connectionData, true, string.Empty);
            }

            using (service.Lock())
            {
                Solution solution = null;

                var t = new Thread(() =>
                {
                    try
                    {
                        var formSelectSolution = new WindowSolutionSelect(_iWriteToOutput, service);

                        formSelectSolution.ShowDialog().GetValueOrDefault();

                        solution = formSelectSolution.SelectedSolution;
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                    }
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (solution == null)
                {
                    return;
                }

                var descriptor = new SolutionComponentDescriptor(service);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                var solutionImage = await solutionDescriptor.CreateSolutionImageAsync(solution.Id, solution.UniqueName);

                if (solutionImage == null)
                {
                    return;
                }

                _solutionImageList.Insert(0, solutionImage);

                FillSolutionImages(solutionImage);
            }
        }

        private async Task LoadSolutionImage(string filePath)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            ToggleControls(null, false, Properties.OutputStrings.LoadingSolutionImage);

            SolutionImage solutionImage = null;

            try
            {
                solutionImage = await SolutionImage.LoadAsync(filePath);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                solutionImage = null;
            }

            if (solutionImage == null)
            {
                ToggleControls(null, true, Properties.OutputStrings.LoadingSolutionImageFailed);
                return;
            }

            _solutionImageList.Insert(0, solutionImage);

            ToggleControls(null, true, Properties.OutputStrings.LoadingSolutionImageCompleted);

            FillSolutionImages(solutionImage);
        }

        private async Task LoadSolutionImageFromZip(ConnectionData connectionData, string filePath)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            ToggleControls(connectionData, false, string.Empty);

            IOrganizationServiceExtented service = null;

            try
            {
                service = await QuickConnection.ConnectAndWriteToOutputAsync(_iWriteToOutput, connectionData);

                if (service == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                ToggleControls(connectionData, true, string.Empty);
            }

            using (service.Lock())
            {
                SolutionImage solutionImage = null;

                try
                {
                    ToggleControls(service.ConnectionData, false, _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatingSolutionImageFromZipFile));

                    var descriptor = new SolutionComponentDescriptor(service);

                    var components = await descriptor.LoadSolutionComponentsFromZipFileAsync(filePath);

                    SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                    solutionImage = await solutionDescriptor.CreateSolutionImageWithComponentsAsync(Path.GetFileNameWithoutExtension(filePath), components);

                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingSolutionImageFromZipFileCompleted);
                }
                catch (Exception ex)
                {
                    solutionImage = null;

                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingSolutionImageFromZipFileFailed);
                }

                if (solutionImage == null)
                {
                    return;
                }

                _solutionImageList.Insert(0, solutionImage);

                FillSolutionImages(solutionImage);
            }
        }

        private void tSBClearSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            _solutionImageList.Clear();

            FillSolutionImages();
        }

        private void FillSolutionImages(SolutionImage solutionImageForSelect = null)
        {
            ToggleControls(null, false, Properties.OutputStrings.LoadingSolutionImage);

            this.Dispatcher.Invoke(() =>
            {
                SolutionImage currentSelectedSolutionImage = cmBSolutionImage.SelectedItem as SolutionImage;

                cmBSolutionImage.Items.Clear();

                cmBSolutionImage.Items.Add(string.Empty);

                foreach (var item in _solutionImageList)
                {
                    cmBSolutionImage.Items.Add(item);
                }

                if (solutionImageForSelect != null && cmBSolutionImage.Items.Contains(solutionImageForSelect))
                {
                    cmBSolutionImage.SelectedItem = solutionImageForSelect;
                }
                else if (cmBSolutionImage.Items.Contains(currentSelectedSolutionImage))
                {
                    cmBSolutionImage.SelectedItem = currentSelectedSolutionImage;
                }
                else
                {
                    cmBSolutionImage.SelectedIndex = 0;
                }
            });

            ToggleControls(null, true, Properties.OutputStrings.LoadingSolutionImageCompleted);
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

        private void lstVwConnections_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwConnections_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            tSBMoveToArchive_Click(sender, e);
        }
    }
}