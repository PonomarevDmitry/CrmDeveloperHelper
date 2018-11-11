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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportSolution : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private EnvDTE.SelectedItem _selectedItem;

        private bool _controlsEnabled = true;

        private ObservableCollection<EntityViewItem> _itemsSource;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        private int _init = 0;

        public WindowExportSolution(
            IWriteToOutput outputWindow
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , EnvDTE.SelectedItem selectedItem
            , string filter
            )
        {
            _init++;

            this._iWriteToOutput = outputWindow;
            this._commonConfig = commonConfig;
            this._selectedItem = selectedItem;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _syncCacheObjects.Add(service.ConnectionData.ConnectionId, new object());

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);
            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.LastSolutionExportFolders, _syncCacheObjects[service.ConnectionData.ConnectionId]);
            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.LastSelectedSolutionsUniqueName, _syncCacheObjects[service.ConnectionData.ConnectionId]);
            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.LastExportSolutionOverrideUniqueName, _syncCacheObjects[service.ConnectionData.ConnectionId]);
            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.LastExportSolutionOverrideDisplayName, _syncCacheObjects[service.ConnectionData.ConnectionId]);
            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.LastExportSolutionOverrideVersion, _syncCacheObjects[service.ConnectionData.ConnectionId]);

            service.ConnectionData.AddLastSolutionExportFolder(_commonConfig.FolderForExport);

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            LoadFromConfig();

            if (!string.IsNullOrEmpty(filter))
            {
                service.ConnectionData.ExportSolutionFilter = filter;
            }

            cmBFilter.Text = service.ConnectionData.ExportSolutionFilter;

            cmBExportFolder.Text = service.ConnectionData.ExportSolutionFolder;

            cmBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSolutions.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            if (service != null)
            {
                ShowExistingSolutions();
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

            rBManaged.DataContext = _commonConfig;
            rBUnmanaged.DataContext = _commonConfig;

            cmBFilter.DataContext = cmBCurrentConnection;

            chBOverrideSolutionNameAndVersion.DataContext = cmBCurrentConnection;
            chBOverrideSolutionDescription.DataContext = cmBCurrentConnection;

            chBCreateFolderForVersion.DataContext = cmBCurrentConnection;
            chBCopyFileToClipBoard.DataContext = cmBCurrentConnection;

            cmBUniqueName.DataContext = cmBCurrentConnection;
            cmBDisplayName.DataContext = cmBCurrentConnection;
            cmBVersion.DataContext = cmBCurrentConnection;
            txtBDescription.DataContext = cmBCurrentConnection;

            if (this._selectedItem != null)
            {
                string exportFolder = string.Empty;

                if (_selectedItem.ProjectItem != null)
                {
                    exportFolder = _selectedItem.ProjectItem.FileNames[1];
                }
                else if (_selectedItem.Project != null)
                {
                    string relativePath = GetRelativePath(_selectedItem.Project);

                    string solutionPath = Path.GetDirectoryName(_selectedItem.DTE.Solution.FullName);

                    exportFolder = Path.Combine(solutionPath, relativePath);
                }

                cmBExportFolder.IsReadOnly = true;
                cmBExportFolder.Text = exportFolder;
            }
            else
            {
                //Text="{Binding Path=SelectedItem.ExportSolutionFolder}" ItemsSource="{Binding Path=SelectedItem.LastSolutionExportFolders}" 
                {
                    Binding binding = new Binding
                    {
                        Path = new PropertyPath("SelectedItem.ExportSolutionFolder")
                    };
                    BindingOperations.SetBinding(cmBExportFolder, ComboBox.TextProperty, binding);
                }

                {
                    Binding binding = new Binding
                    {
                        Path = new PropertyPath("SelectedItem.LastSolutionExportFolders")
                    };
                    BindingOperations.SetBinding(cmBExportFolder, ComboBox.ItemsSourceProperty, binding);
                }

                cmBExportFolder.DataContext = cmBCurrentConnection;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);
            BindingOperations.ClearAllBindings(cmBExportFolder);
            BindingOperations.ClearAllBindings(cmBFilter);

            cmBFilter.Items.DetachFromSourceCollection();
            cmBExportFolder.Items.DetachFromSourceCollection();
            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBFilter.ItemsSource = null;
            cmBExportFolder.ItemsSource = null;
            cmBCurrentConnection.ItemsSource = null;

            BindingOperations.ClearAllBindings(cmBUniqueName);
            BindingOperations.ClearAllBindings(cmBDisplayName);
            BindingOperations.ClearAllBindings(cmBVersion);

            cmBUniqueName.Items.DetachFromSourceCollection();
            cmBDisplayName.Items.DetachFromSourceCollection();
            cmBVersion.Items.DetachFromSourceCollection();

            cmBUniqueName.ItemsSource = null;
            cmBDisplayName.ItemsSource = null;
            cmBVersion.ItemsSource = null;

            base.OnClosed(e);
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
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingSolutions()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingSolutions);

            this._itemsSource.Clear();

            string textName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                textName = cmBFilter.Text?.Trim()?.ToLower() ?? string.Empty;
            });

            List<Solution> list = null;

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    SolutionRepository repository = new SolutionRepository(service);

                    list = await repository.GetListSolutionsUnmanagedAsync(textName);
                }
                else
                {
                    list = new List<Solution>();
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                list = new List<Solution>();
            }

            LoadSolutions(list);

            ToggleControls(true, Properties.WindowStatusStrings.LoadingSolutionsCompletedFormat, list.Count());
        }

        private class EntityViewItem
        {
            public Solution Solution { get; private set; }

            public string SolutionName => Solution.UniqueName;

            public string DisplayName => Solution.FriendlyName;

            public string SolutionType => Solution.FormattedValues[Solution.Schema.Attributes.ismanaged];

            public string Visible => Solution.FormattedValues[Solution.Schema.Attributes.isvisible];

            public DateTime? InstalledOn => Solution.InstalledOn?.ToLocalTime();

            public string PublisherName => Solution.PublisherId?.Name;

            public string Prefix => Solution.PublisherCustomizationPrefix;

            public EntityViewItem(Solution Solution)
            {
                this.Solution = Solution;
            }
        }

        private void LoadSolutions(IEnumerable<Solution> results)
        {
            this.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results.OrderByDescending(ent => ent.InstalledOn).ThenBy(ent => ent.UniqueName))
                {
                    var item = new EntityViewItem(entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwSolutions.Items.Count == 1)
                {
                    this.lstVwSolutions.SelectedItem = this.lstVwSolutions.Items[0];
                }
            });
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

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(this.btnExportSolution, enabled);

            ToggleControl(cmBCurrentConnection, enabled);

            ToggleProgressBar(enabled);

            if (enabled)
            {
                UpdateButtonsEnable();
            }
        }

        private void ToggleProgressBar(bool enabled)
        {
            if (tSProgressBar == null)
            {
                return;
            }

            this.tSProgressBar.Dispatcher.Invoke(() =>
            {
                tSProgressBar.IsIndeterminate = !enabled;
            });
        }

        private void ToggleControl(Control c, bool enabled)
        {
            c.Dispatcher.Invoke(() =>
            {
                if (c is TextBox)
                {
                    ((TextBox)c).IsReadOnly = !enabled;
                }
                else
                {
                    c.IsEnabled = enabled;
                }
            });
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwSolutions.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwSolutions.SelectedItems.Count > 0;

                    UIElement[] list = { btnExportSolution, btnOpenSolutionInWeb };

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

        private void cmBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingSolutions();
            }
        }

        private Solution GetSelectedEntity()
        {
            return this.lstVwSolutions.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwSolutions.SelectedItems.OfType<EntityViewItem>().Select(e => e.Solution).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

                if (item != null)
                {
                    string message = string.Format(Properties.MessageBoxStrings.ExportSolutionFormat, item.SolutionName);

                    if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        ExecuteAction(item.Solution, PerformExportSolution);
                    }
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteAction(Solution solution, Func<string, Solution, ExportSolutionConfig, ExportSolutionOverrideInformation, Task> action)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (solution == null)
            {
                return;
            }

            string exportFolder = string.Empty;

            cmBExportFolder.Dispatcher.Invoke(() =>
            {
                exportFolder = cmBExportFolder.Text?.Trim();
            });

            if (string.IsNullOrEmpty(exportFolder))
            {
                return;
            }

            var fileExportFolder = exportFolder;

            var overrideSolutionNameAndVersion = chBOverrideSolutionNameAndVersion.IsChecked.GetValueOrDefault();

            var overrideSolutionDescription = chBOverrideSolutionDescription.IsChecked.GetValueOrDefault();

            var uniqueName = cmBUniqueName.Text?.Trim();
            var displayName = cmBDisplayName.Text?.Trim();
            var version = cmBVersion.Text?.Trim();

            var description = txtBDescription.Text?.Trim();

            if (overrideSolutionNameAndVersion)
            {
                if (!string.IsNullOrEmpty(version))
                {
                    if (!Version.TryParse(version, out Version ver))
                    {
                        MessageBox.Show(Properties.MessageBoxStrings.SolutionVersionTextIsNotValidVersion, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    version = ver.ToString();
                }
            }

            if (chBCreateFolderForVersion.IsChecked.GetValueOrDefault())
            {
                if (!string.IsNullOrEmpty(version))
                {
                    fileExportFolder = Path.Combine(fileExportFolder, version);
                }
                else if(!string.IsNullOrEmpty(solution.Version))
                {
                    fileExportFolder = Path.Combine(fileExportFolder, solution.Version);
                }
            }

            if (!Directory.Exists(fileExportFolder))
            {
                Directory.CreateDirectory(fileExportFolder);
            }

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                ExportSolutionConfig config = null;
                ExportSolutionOverrideInformation solutionInfo = new ExportSolutionOverrideInformation(overrideSolutionNameAndVersion, uniqueName, displayName, version, overrideSolutionDescription, description);

                config = new ExportSolutionConfig()
                {
                    ExportAutoNumberingSettings = chBAutoNumbering.IsChecked.GetValueOrDefault(),
                    ExportCalendarSettings = chBCalendar.IsChecked.GetValueOrDefault(),
                    ExportCustomizationSettings = chBCustomization.IsChecked.GetValueOrDefault(),
                    ExportEmailTrackingSettings = chBEmailTracking.IsChecked.GetValueOrDefault(),
                    ExportExternalApplications = chBExternalApplications.IsChecked.GetValueOrDefault(),
                    ExportGeneralSettings = chBGeneral.IsChecked.GetValueOrDefault(),
                    ExportIsvConfig = chBISVConfig.IsChecked.GetValueOrDefault(),
                    ExportMarketingSettings = chBMarketing.IsChecked.GetValueOrDefault(),
                    ExportOutlookSynchronizationSettings = chBOutlookSynchronization.IsChecked.GetValueOrDefault(),
                    ExportRelationshipRoles = chBRelashionshipRoles.IsChecked.GetValueOrDefault(),
                    ExportSales = chBSales.IsChecked.GetValueOrDefault(),

                    Managed = rBManaged.IsChecked.GetValueOrDefault(),

                    IdSolution = solution.Id,

                    ConnectionName = connectionData.Name,

                    ExportFolder = fileExportFolder,
                };

                action(exportFolder, solution, config, solutionInfo);
            }
        }

        private string GetRelativePath(EnvDTE.Project project)
        {
            List<string> names = new List<string>();

            if (project != null)
            {
                AddNamesRecursive(names, project);
            }

            names.Reverse();

            return string.Join(@"\", names);
        }

        private void AddNamesRecursive(List<string> names, EnvDTE.Project project)
        {
            if (project != null)
            {
                names.Add(project.Name);

                if (project.ParentProjectItem != null && project.ParentProjectItem.ContainingProject != null)
                {
                    AddNamesRecursive(names, project.ParentProjectItem.ContainingProject);
                }
            }
        }

        private void btnExportSolution_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformExportSolution);
        }

        private async Task PerformExportSolution(string folder, Solution solution, ExportSolutionConfig config, ExportSolutionOverrideInformation solutionExportInfo)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }
            
            ToggleControls(false, Properties.WindowStatusStrings.ExportingSolutionFormat, solution.UniqueName);

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    _iWriteToOutput.WriteToOutputSolutionUri(service.ConnectionData.ConnectionId, solution.UniqueName, service.ConnectionData.GetSolutionUrl(solution.Id));

                    if (solutionExportInfo.OverrideNameAndVersion)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            string text = null;

                            text = cmBUniqueName.Text;
                            service.ConnectionData.AddLastExportSolutionOverrideUniqueName(solutionExportInfo.UniqueName);
                            cmBUniqueName.Text = text;

                            text = cmBDisplayName.Text;
                            service.ConnectionData.AddLastExportSolutionOverrideDisplayName(solutionExportInfo.DisplayName);
                            cmBDisplayName.Text = text;

                            if (!string.IsNullOrEmpty(solutionExportInfo.Version) && Version.TryParse(solutionExportInfo.Version, out Version ver))
                            {
                                var oldVersion = ver.ToString();
                                var newVersion = new Version(ver.Major, ver.Minor, ver.Build, ver.Revision + 1).ToString();

                                service.ConnectionData.AddLastExportSolutionOverrideVersion(newVersion, oldVersion);
                                cmBVersion.Text = newVersion;
                            }
                        });
                    }

                    ExportSolutionHelper helper = new ExportSolutionHelper(service);

                    var filePath = await helper.ExportAsync(config, solutionExportInfo);

                    this.Dispatcher.Invoke(() =>
                    {
                        if (chBCopyFileToClipBoard.IsChecked.GetValueOrDefault())
                        {
                            Clipboard.SetFileDropList(new System.Collections.Specialized.StringCollection() { filePath });
                        }
                    });

                    this._iWriteToOutput.WriteToOutput("Solution {0} exported to {1}", solution.UniqueName, filePath);

                    if (this._selectedItem != null)
                    {
                        if (_selectedItem.ProjectItem != null)
                        {
                            _selectedItem.ProjectItem.ProjectItems.AddFromFileCopy(filePath);

                            _selectedItem.ProjectItem.ContainingProject.Save();
                        }
                        else if (_selectedItem.Project != null)
                        {
                            _selectedItem.Project.ProjectItems.AddFromFile(filePath);

                            _selectedItem.Project.Save();
                        }
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var text = cmBExportFolder.Text;

                            service.ConnectionData.AddLastSolutionExportFolder(folder);

                            cmBExportFolder.Text = text;
                        });
                    }

                    this._iWriteToOutput.SelectFileInFolder(filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput("Cannot get Service.");
                }

                ToggleControls(true, Properties.WindowStatusStrings.ExportingSolutionCompletedFormat, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);


                ToggleControls(true, Properties.WindowStatusStrings.ExportingSolutionFailedFormat, solution.UniqueName);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSolutions();
            }

            base.OnKeyDown(e);
        }

        private void btnOpenSolutionInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionInWeb(entity.Id);
            }
        }

        private void mIOpenSolutionListInWeb_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenCrmWebSite(OpenCrmWebSiteType.Solutions);
            }
        }

        private void mIOpenCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenCrmWebSite(OpenCrmWebSiteType.Customization);
            }
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();

                var connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                if (connectionData != null)
                {
                    if (!_syncCacheObjects.ContainsKey(connectionData.ConnectionId))
                    {
                        _syncCacheObjects.Add(connectionData.ConnectionId, new object());

                        BindingOperations.EnableCollectionSynchronization(connectionData.LastSolutionExportFolders, _syncCacheObjects[connectionData.ConnectionId]);
                        BindingOperations.EnableCollectionSynchronization(connectionData.LastSelectedSolutionsUniqueName, _syncCacheObjects[connectionData.ConnectionId]);
                        BindingOperations.EnableCollectionSynchronization(connectionData.LastExportSolutionOverrideUniqueName, _syncCacheObjects[connectionData.ConnectionId]);
                        BindingOperations.EnableCollectionSynchronization(connectionData.LastExportSolutionOverrideDisplayName, _syncCacheObjects[connectionData.ConnectionId]);
                        BindingOperations.EnableCollectionSynchronization(connectionData.LastExportSolutionOverrideVersion, _syncCacheObjects[connectionData.ConnectionId]);
                    }

                    var text = cmBExportFolder.Text;

                    connectionData.AddLastSolutionExportFolder(_commonConfig.FolderForExport);

                    cmBExportFolder.Text = text;
                }
            });

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (cmBCurrentConnection.SelectedItem != null)
            {
                ShowExistingSolutions();
            }
        }

        private void mICreateNewSolution_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionCreateInWeb();
            }
        }

        private void mIOpenSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                _commonConfig.Save();

                WindowHelper.OpenSolutionImageWindow(this._iWriteToOutput, connectionData, _commonConfig);
            }
        }

        private void mIOpenOrganizationDifferenceImage_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                _commonConfig.Save();

                WindowHelper.OpenOrganizationDifferenceImageWindow(this._iWriteToOutput, connectionData, _commonConfig);
            }
        }

        private async void btnOpenComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenSolutionComponentDependenciesWindow(this._iWriteToOutput, service, null, commonConfig, entity.UniqueName, null);
        }

        private async void ClearUnmanagedSolution_Click(object sender, RoutedEventArgs e)
        {
            var solution = GetSelectedEntity();

            if (solution == null)
            {
                return;
            }

            string question = string.Format(Properties.MessageBoxStrings.ClearSolutionFormat, solution.UniqueName);

            if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.ClearingSolutionFormat);

                var service = await GetService();

                var descriptor = new SolutionComponentDescriptor(service, true);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput("Creating backup Solution Components in '{0}'.", solution.UniqueName);

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                       service.ConnectionData.Name
                       , solution.UniqueName
                       , "Components Backup"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solution.Id);

                    this._iWriteToOutput.WriteToOutput("Created backup Solution Components in '{0}': {1}", solution.UniqueName, filePath);
                    this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                }

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                       service.ConnectionData.Name
                       , solution.UniqueName
                       , "SolutionImage Backup"
                       , "xml"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solution.Id);
                }

                SolutionComponentRepository repository = new SolutionComponentRepository(service);
                await repository.ClearSolutionAsync(solution.UniqueName);

                ToggleControls(true, Properties.WindowStatusStrings.ClearingSolutionCompletedFormat);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ClearingSolutionFailedFormat);
            }
        }

        private void miSelectSolutionAsLast_Click(object sender, RoutedEventArgs e)
        {
            var solution = GetSelectedEntity();

            if (solution == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                var text = cmBFilter.Text;

                connectionData.AddLastSelectedSolution(solution.UniqueName);

                _iWriteToOutput.WriteToOutputSolutionUri(connectionData.ConnectionId, solution.UniqueName, connectionData.GetSolutionUrl(solution.Id));

                cmBFilter.Text = text;
            }
        }
    }
}