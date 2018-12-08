using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerSolution : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private EnvDTE.SelectedItem _selectedItem;

        private Guid? _objectId;
        private int? _componentType;

        private bool _controlsEnabled = true;

        private Popup _optionsPopup;
        private ExportSolutionOptionsControl _optionsExportSolutionOptionsControl;

        private ObservableCollection<EntityViewItem> _itemsSource;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        private int _init = 0;

        public WindowExplorerSolution(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , int? componentType
            , Guid? objectId
            , EnvDTE.SelectedItem selectedItem
            )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._selectedItem = selectedItem;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;
            this._objectId = objectId;
            this._componentType = componentType;

            var descriptor = new SolutionComponentDescriptor(service, true);

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _descriptorCache[service.ConnectionData.ConnectionId] = descriptor;

            InitializeComponent();

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            this._optionsExportSolutionOptionsControl = new ExportSolutionOptionsControl(_commonConfig, cmBCurrentConnection);
            this._optionsExportSolutionOptionsControl.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = this._optionsExportSolutionOptionsControl,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            LoadFromConfig();

            cmBFilter.Text = service.ConnectionData.ExplorerSolutionFilter;

            sepClearSolutionComponentFilter.IsEnabled = btnClearSolutionComponentFilter.IsEnabled = _objectId.HasValue && _componentType.HasValue;
            sepClearSolutionComponentFilter.Visibility = btnClearSolutionComponentFilter.Visibility = (_objectId.HasValue && _componentType.HasValue) ? Visibility.Visible : Visibility.Collapsed;

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSolutions.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            service.ConnectionData.LastSelectedSolutionsUniqueName.CollectionChanged -= LastSelectedSolutionsUniqueName_CollectionChanged;
            service.ConnectionData.LastSelectedSolutionsUniqueName.CollectionChanged += LastSelectedSolutionsUniqueName_CollectionChanged;

            BindCollections(service.ConnectionData);

            _init--;

            FocusOnComboBoxTextBox(cmBFilter);

            if (service != null)
            {
                ShowExistingSolutions();
            }
        }

        private void LastSelectedSolutionsUniqueName_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(FillCopyComponentsToLastSelectedSolutionsAsync);
        }

        private void LoadFromConfig()
        {
            txtBFolder.DataContext = _commonConfig;

            cmBFileAction.DataContext = _commonConfig;

            cmBGroupBy.DataContext = _commonConfig;

            cmBFilter.DataContext = cmBCurrentConnection;

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

                _optionsExportSolutionOptionsControl.SetExportFolderReadOnly(exportFolder);
            }
            else
            {
                _optionsExportSolutionOptionsControl.BindExportFolder();
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

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

            _optionsExportSolutionOptionsControl.DetachCollections();

            cmBCurrentConnection.DataContext = null;
            cmBFilter.DataContext = null;

            BindingOperations.ClearAllBindings(cmBCurrentConnection);
            BindingOperations.ClearAllBindings(cmBFilter);

            cmBFilter.Items.DetachFromSourceCollection();
            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBFilter.DataContext = null;
            cmBCurrentConnection.DataContext = null;

            cmBFilter.ItemsSource = null;
            cmBCurrentConnection.ItemsSource = null;

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
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task<SolutionComponentDescriptor> GetDescriptor()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_descriptorCache.ContainsKey(connectionData.ConnectionId))
                {
                    var service = await GetService();

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);
                }

                return _descriptorCache[connectionData.ConnectionId];
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

            cmBFilter.Dispatcher.Invoke(() =>
            {
                textName = cmBFilter.Text?.Trim().ToLower();
            });

            IEnumerable<Solution> list = Enumerable.Empty<Solution>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    SolutionRepository repository = new SolutionRepository(service);

                    list = await repository.GetSolutionsAllAsync(textName, _componentType, _objectId);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            LoadSolutions(list);

            ToggleControls(true, Properties.WindowStatusStrings.LoadingSolutionsCompletedFormat1, list.Count());
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
            this.lstVwSolutions.Dispatcher.Invoke(() =>
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

            ToggleControl(cmBCurrentConnection, enabled);

            ToggleProgressBar(enabled);

            UpdateButtonsEnable();
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
                    {
                        bool enabled = this._controlsEnabled;

                        menuAnalyzeSolutions.IsEnabled = tSDDBAnalyzeSolutions.IsEnabled = enabled;

                        tSDDBAnalyzeSolutions.Items.Clear();

                        if (enabled)
                        {
                            var list1 = lstVwSolutions.SelectedItems
                                .OfType<EntityViewItem>()
                                .Select(e => e.Solution)
                                .OrderBy(e => e.UniqueName)
                                .ToList();

                            if (list1.Count > 1)
                            {
                                foreach (var solution1 in list1)
                                {
                                    if (tSDDBAnalyzeSolutions.Items.Count > 0)
                                    {
                                        tSDDBAnalyzeSolutions.Items.Add(new Separator());
                                    }

                                    MenuItem menuItemSolutuion1 = new MenuItem()
                                    {
                                        Header = string.Format("Solution {0}", solution1.UniqueNameEscapeUnderscore),
                                    };

                                    tSDDBAnalyzeSolutions.Items.Add(menuItemSolutuion1);

                                    var list2 = list1.Where(en => !string.Equals(en.UniqueName, solution1.UniqueName, StringComparison.InvariantCultureIgnoreCase));

                                    foreach (var solution2 in list2)
                                    {
                                        if (menuItemSolutuion1.Items.Count > 0)
                                        {
                                            menuItemSolutuion1.Items.Add(new Separator());
                                        }

                                        MenuItem menuItemSolutuion2 = new MenuItem()
                                        {
                                            Header = string.Format("Solution {0}", solution2.UniqueNameEscapeUnderscore),
                                        };

                                        menuItemSolutuion1.Items.Add(menuItemSolutuion2);

                                        MenuItem mIAnalyzeSolutions = new MenuItem()
                                        {
                                            Header = string.Format("Analyze Solutions {0} and {1}", solution1.UniqueNameEscapeUnderscore, solution2.UniqueNameEscapeUnderscore),
                                            Tag = Tuple.Create(solution1, solution2),
                                        };
                                        mIAnalyzeSolutions.Click += mIAnalyzeSolutions_Click;

                                        MenuItem mIShowUniqueComponentsIn1 = new MenuItem()
                                        {
                                            Header = string.Format("Show Unique Components in {0} compared to {1}", solution1.UniqueNameEscapeUnderscore, solution2.UniqueNameEscapeUnderscore),
                                            Tag = Tuple.Create(solution1, solution2),
                                        };
                                        mIShowUniqueComponentsIn1.Click += mIShowUniqueComponentsInSolution_Click;

                                        MenuItem mIShowUniqueComponentsIn2 = new MenuItem()
                                        {
                                            Header = string.Format("Show Unique Components in {0} compared to {1}", solution2.UniqueNameEscapeUnderscore, solution1.UniqueNameEscapeUnderscore),
                                            Tag = Tuple.Create(solution2, solution1),
                                        };
                                        mIShowUniqueComponentsIn2.Click += mIShowUniqueComponentsInSolution_Click;

                                        menuItemSolutuion2.Items.Add(mIAnalyzeSolutions);
                                        menuItemSolutuion2.Items.Add(new Separator());
                                        menuItemSolutuion2.Items.Add(mIShowUniqueComponentsIn1);
                                        menuItemSolutuion2.Items.Add(new Separator());
                                        menuItemSolutuion2.Items.Add(mIShowUniqueComponentsIn2);
                                    }
                                }
                            }
                            else
                            {
                                menuAnalyzeSolutions.IsEnabled = tSDDBAnalyzeSolutions.IsEnabled = false;
                            }
                        }
                    }

                    {
                        bool enabled = this._controlsEnabled;

                        menuShow.IsEnabled = tSDDBShow.IsEnabled = enabled;

                        tSDDBShow.Items.Clear();

                        if (enabled)
                        {
                            var list = lstVwSolutions.SelectedItems
                                .OfType<EntityViewItem>()
                                .Select(e => e.Solution)
                                .OrderBy(e => e.UniqueName)
                                .ToList();

                            if (list.Any())
                            {
                                if (list.Count == 1)
                                {
                                    var solution = list.First();

                                    FillSolutionButtons(solution, tSDDBShow.Items);
                                }
                                else
                                {
                                    foreach (var solution in list)
                                    {
                                        if (tSDDBShow.Items.Count > 0)
                                        {
                                            tSDDBShow.Items.Add(new Separator());
                                        }

                                        MenuItem menuItem = new MenuItem()
                                        {
                                            Header = string.Format("Solution {0}", solution.UniqueNameEscapeUnderscore),
                                        };

                                        tSDDBShow.Items.Add(menuItem);

                                        FillSolutionButtons(solution, menuItem.Items);
                                    }
                                }
                            }
                            else
                            {
                                menuShow.IsEnabled = tSDDBShow.IsEnabled = false;
                            }
                        }
                    }

                    {
                        bool enabled = this._controlsEnabled;

                        tSDDBCopyComponents.IsEnabled = enabled;

                        tSDDBCopyComponents.Items.Clear();

                        if (enabled)
                        {
                            var listFrom = lstVwSolutions.SelectedItems
                                .OfType<EntityViewItem>()
                                .Where(e => e.Solution != null)
                                .Select(e => e.Solution)
                                .OrderBy(e => e.UniqueName)
                                .ToList();

                            var listTo = listFrom.Where(e => e.IsManaged.GetValueOrDefault() == false).OrderBy(e => e.UniqueName).ToList();

                            var total = listTo.Count * (listFrom.Count - 1);

                            if (total > 0)
                            {
                                FillCopyComponentsMenuItems(tSDDBCopyComponents, listFrom, listTo);
                            }
                        }

                        tSDDBCopyComponents.IsEnabled = tSDDBCopyComponents.Items.Count > 0;

                        menuItemCopyComponents.IsEnabled = tSDDBCopyComponents.Items.Count > 0 || tSDDBCopyComponentsLastSolution.Items.Count > 0;
                    }

                    {
                        bool enabled = this._controlsEnabled;

                        tSDDBClearUnmanagedSolution.IsEnabled = enabled;

                        tSDDBClearUnmanagedSolution.Items.Clear();

                        if (enabled)
                        {
                            var list = lstVwSolutions.SelectedItems
                                .OfType<EntityViewItem>()
                                .Where(e => e.Solution.IsManaged.GetValueOrDefault() == false)
                                .Select(e => e.Solution)
                                .OrderBy(e => e.UniqueName)
                                .ToList();

                            if (list.Any())
                            {
                                foreach (var solution in list)
                                {
                                    if (tSDDBClearUnmanagedSolution.Items.Count > 0)
                                    {
                                        tSDDBClearUnmanagedSolution.Items.Add(new Separator());
                                    }

                                    MenuItem menuItem = new MenuItem()
                                    {
                                        Header = string.Format("Clear solution {0}", solution.UniqueNameEscapeUnderscore),
                                        Tag = solution,
                                    };

                                    menuItem.Click += mIClearUnmanagedSolution_Click;

                                    tSDDBClearUnmanagedSolution.Items.Add(menuItem);
                                }
                            }
                        }

                        tSDDBClearUnmanagedSolution.IsEnabled = tSDDBClearUnmanagedSolution.Items.Count > 0;
                    }

                    FillCopyComponentsToLastSelectedSolutionsAsync();
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            });
        }

        private async Task FillCopyComponentsToLastSelectedSolutionsAsync()
        {
            bool enabled = this._controlsEnabled;

            tSDDBCopyComponentsLastSolution.IsEnabled = enabled;

            tSDDBCopyComponentsLastSolution.Items.Clear();

            if (enabled)
            {
                var connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                if (connectionData != null
                    && connectionData.LastSelectedSolutionsUniqueName != null
                    )
                {
                    var listSolutionNames = connectionData.LastSelectedSolutionsUniqueName.ToList();

                    var listFrom = lstVwSolutions.SelectedItems
                            .OfType<EntityViewItem>()
                            .Where(e => e.Solution != null)
                            .Select(e => e.Solution)
                            .OrderBy(e => e.UniqueName)
                            .ToList();

                    if (listSolutionNames.Any() && listFrom.Any())
                    {
                        var service = await GetService();

                        SolutionRepository repository = new SolutionRepository(service);

                        var solutionsTask = await repository.GetSolutionsVisibleUnmanagedAsync(listSolutionNames);

                        var lastSolutions = solutionsTask.ToDictionary(s => s.UniqueName, StringComparer.InvariantCultureIgnoreCase);

                        var listTo = listSolutionNames.Where(s => lastSolutions.ContainsKey(s)).Select(s => lastSolutions[s]).ToList();

                        FillCopyComponentsMenuItems(tSDDBCopyComponentsLastSolution, listFrom, listTo);
                    }
                }
            }

            tSDDBCopyComponentsLastSolution.IsEnabled = tSDDBCopyComponentsLastSolution.Items.Count > 0;

            menuItemCopyComponents.IsEnabled = tSDDBCopyComponents.Items.Count > 0 || tSDDBCopyComponentsLastSolution.Items.Count > 0;
        }

        private void FillCopyComponentsMenuItems(MenuItem parentMenuItem, List<Solution> listFrom, List<Solution> listTo)
        {
            foreach (var solutionTo in listTo)
            {
                var listFromFiltered = listFrom.Where(en => !string.Equals(en.UniqueName, solutionTo.UniqueName, StringComparison.InvariantCultureIgnoreCase));

                if (!listFromFiltered.Any())
                {
                    continue;
                }

                MenuItem menuItemSolution = new MenuItem()
                {
                    Header = string.Format("Solution {0}", solutionTo.UniqueNameEscapeUnderscore),
                };

                if (parentMenuItem.Items.Count > 0)
                {
                    parentMenuItem.Items.Add(new Separator());
                }

                parentMenuItem.Items.Add(menuItemSolution);

                if (listFromFiltered.Count() > 1)
                {
                    MenuItem menuItem = new MenuItem()
                    {
                        Header = string.Format("Copy Components from All Selected Solutions to {0}", solutionTo.UniqueNameEscapeUnderscore),
                        Tag = Tuple.Create(listFromFiltered.ToArray(), solutionTo),
                    };
                    menuItem.Click += mICopyComponentsFromSolutionCollectionToSolution_Click;

                    menuItemSolution.Items.Add(menuItem);
                }

                foreach (var solutionFrom in listFromFiltered)
                {
                    MenuItem menuItem = new MenuItem()
                    {
                        Header = string.Format("Copy Components from {0} to {1}", solutionFrom.UniqueNameEscapeUnderscore, solutionTo.UniqueNameEscapeUnderscore),
                        Tag = Tuple.Create(solutionFrom, solutionTo),
                    };
                    menuItem.Click += mICopyComponentsFromSolution1ToSolution2_Click;

                    if (menuItemSolution.Items.Count > 0)
                    {
                        menuItemSolution.Items.Add(new Separator());
                    }

                    menuItemSolution.Items.Add(menuItem);
                }
            }
        }

        private void FillSolutionButtons(Solution solution, ItemCollection itemCollection)
        {
            MenuItem mIOpenSolutionInWeb = new MenuItem()
            {
                Header = string.Format("Open in Web Solution {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mIOpenSolutionInWeb.Click += mIOpenSolutionInWeb_Click;

            MenuItem mIOpenComponentsInWindow = new MenuItem()
            {
                Header = string.Format("Open Solution Components in Window for {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mIOpenComponentsInWindow.Click += mIOpenComponentsInWindow_Click;

            MenuItem mIUsedEntitiesInWorkflows = new MenuItem()
            {
                Header = string.Format("Used Entities in Workflows in {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mIUsedEntitiesInWorkflows.Click += mIUsedEntitiesInWorkflows_Click;

            MenuItem mIUsedNotExistsEntitiesInWorkflows = new MenuItem()
            {
                Header = string.Format("Used Not Exists Entities in Workflows in {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mIUsedNotExistsEntitiesInWorkflows.Click += mIUsedNotExistsEntitiesInWorkflows_Click;

            MenuItem mICreateSolutionImageIn = new MenuItem()
            {
                Header = string.Format("Create Solution Image for {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mICreateSolutionImageIn.Click += mICreateSolutionImageIn_Click;

            MenuItem mIComponentsIn = new MenuItem()
            {
                Header = string.Format("Components in {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mIComponentsIn.Click += mIComponentsIn_Click;

            MenuItem mIMissingComponentsIn = new MenuItem()
            {
                Header = string.Format("Missing Components for {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mIMissingComponentsIn.Click += mIMissingComponentsIn_Click;

            MenuItem mIUninstallComponentsIn = new MenuItem()
            {
                Header = string.Format("Uninstall Components for {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mIUninstallComponentsIn.Click += mIUninstallComponentsIn_Click;




            itemCollection.Add(mIOpenComponentsInWindow);
            itemCollection.Add(new Separator());
            itemCollection.Add(mIOpenSolutionInWeb);

            if (solution.IsManaged.GetValueOrDefault() == false)
            {
                MenuItem mISelectSolutionAsLast = new MenuItem()
                {
                    Header = string.Format("Select Solution {0} as Last Selected", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mISelectSolutionAsLast.Click += miSelectSolutionAsLast_Click;

                itemCollection.Add(new Separator());
                itemCollection.Add(mISelectSolutionAsLast);
            }

            itemCollection.Add(new Separator());
            itemCollection.Add(mIUsedEntitiesInWorkflows);
            itemCollection.Add(mIUsedNotExistsEntitiesInWorkflows);
            itemCollection.Add(new Separator());
            itemCollection.Add(mICreateSolutionImageIn);

            if (solution.IsManaged.GetValueOrDefault() == false)
            {
                MenuItem mIExportSolution = new MenuItem()
                {
                    Header = string.Format("Export Solution {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mIExportSolution.Click += mIExportSolution_Click;

                itemCollection.Add(new Separator());
                itemCollection.Add(mIExportSolution);
            }

            itemCollection.Add(new Separator());
            itemCollection.Add(mIComponentsIn);
            itemCollection.Add(new Separator());
            itemCollection.Add(mIMissingComponentsIn);
            itemCollection.Add(mIUninstallComponentsIn);
        }

        private async void mIExportSolution_Click(object sender, RoutedEventArgs e)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (!(sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                && solution.IsManaged.GetValueOrDefault() == false
                ))
            {
                return;
            }

            if (solution == null)
            {
                return;
            }

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData == null)
            {
                return;
            }

            string exportFolder = string.Empty;

            exportFolder = _optionsExportSolutionOptionsControl.GetExportFolder();

            if (string.IsNullOrEmpty(exportFolder))
            {
                return;
            }

            var fileExportFolder = exportFolder;

            var uniqueName = connectionData.ExportSolutionOverrideUniqueName?.Trim() ?? string.Empty;
            var displayName = connectionData.ExportSolutionOverrideDisplayName?.Trim() ?? string.Empty;
            var version = connectionData.ExportSolutionOverrideVersion?.Trim() ?? string.Empty;

            var description = connectionData.ExportSolutionOverrideDescription?.Trim() ?? string.Empty;

            if (connectionData.ExportSolutionIsOverrideSolutionNameAndVersion)
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

            if (connectionData.ExportSolutionIsCreateFolderForVersion)
            {
                if (!string.IsNullOrEmpty(version))
                {
                    fileExportFolder = Path.Combine(fileExportFolder, version);
                }
                else if (!string.IsNullOrEmpty(solution.Version))
                {
                    fileExportFolder = Path.Combine(fileExportFolder, solution.Version);
                }
            }

            if (!Directory.Exists(fileExportFolder))
            {
                Directory.CreateDirectory(fileExportFolder);
            }

            if (connectionData == null)
            {
                return;
            }

            ExportSolutionOverrideInformation solutionExportInfo = new ExportSolutionOverrideInformation(
                connectionData.ExportSolutionIsOverrideSolutionNameAndVersion
                , uniqueName
                , displayName
                , version
                , connectionData.ExportSolutionIsOverrideSolutionDescription
                , description
                );

            ExportSolutionConfig config = new ExportSolutionConfig()
            {
                ExportAutoNumberingSettings = _commonConfig.ExportSolutionExportAutoNumberingSettings,
                ExportCalendarSettings = _commonConfig.ExportSolutionExportCalendarSettings,
                ExportCustomizationSettings = _commonConfig.ExportSolutionExportCustomizationSettings,
                ExportEmailTrackingSettings = _commonConfig.ExportSolutionExportEmailTrackingSettings,
                ExportExternalApplications = _commonConfig.ExportSolutionExportExternalApplications,
                ExportGeneralSettings = _commonConfig.ExportSolutionExportGeneralSettings,
                ExportIsvConfig = _commonConfig.ExportSolutionExportIsvConfig,
                ExportMarketingSettings = _commonConfig.ExportSolutionExportMarketingSettings,
                ExportOutlookSynchronizationSettings = _commonConfig.ExportSolutionExportOutlookSynchronizationSettings,
                ExportRelationshipRoles = _commonConfig.ExportSolutionExportRelationshipRoles,
                ExportSales = _commonConfig.ExportSolutionExportSales,

                Managed = connectionData.ExportSolutionManaged,

                IdSolution = solution.Id,

                ConnectionName = connectionData.Name,

                ExportFolder = fileExportFolder,
            };

            ToggleControls(false, Properties.WindowStatusStrings.ExportingSolutionFormat1, solution.UniqueName);

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    _iWriteToOutput.WriteToOutputSolutionUri(service.ConnectionData, solution.UniqueName, solution.Id);

                    if (solutionExportInfo.OverrideNameAndVersion)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            _optionsExportSolutionOptionsControl.StoreTextValues();

                            service.ConnectionData.AddLastExportSolutionOverrideUniqueName(solutionExportInfo.UniqueName);
                            service.ConnectionData.AddLastExportSolutionOverrideDisplayName(solutionExportInfo.DisplayName);

                            _optionsExportSolutionOptionsControl.RestoreTextValues();

                            if (!string.IsNullOrEmpty(solutionExportInfo.Version) && Version.TryParse(solutionExportInfo.Version, out Version ver))
                            {
                                var oldVersion = ver.ToString();
                                var newVersion = new Version(ver.Major, ver.Minor, ver.Build, ver.Revision + 1).ToString();

                                service.ConnectionData.AddLastExportSolutionOverrideVersion(newVersion, oldVersion);

                                _optionsExportSolutionOptionsControl.SetNewVersion(newVersion);
                            }
                        });
                    }

                    ExportSolutionHelper helper = new ExportSolutionHelper(service);

                    var filePath = await helper.ExportAsync(config, solutionExportInfo);

                    this.Dispatcher.Invoke(() =>
                    {
                        if (service.ConnectionData.ExportSolutionIsCopyFileToClipBoard)
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
                            _optionsExportSolutionOptionsControl.StoreTextValues();

                            service.ConnectionData.AddLastSolutionExportFolder(exportFolder);

                            _optionsExportSolutionOptionsControl.RestoreTextValues();
                        });
                    }

                    this._iWriteToOutput.SelectFileInFolder(filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput("Cannot get Service.");
                }

                ToggleControls(true, Properties.WindowStatusStrings.ExportingSolutionCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);


                ToggleControls(true, Properties.WindowStatusStrings.ExportingSolutionFailedFormat1, solution.UniqueName);
            }
        }

        private void miSelectSolutionAsLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                && solution.IsManaged.GetValueOrDefault() == false
                )
            {
                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                    if (connectionData != null)
                    {
                        var text = cmBFilter.Text;

                        connectionData.AddLastSelectedSolution(solution.UniqueName);

                        _iWriteToOutput.WriteToOutputSolutionUri(connectionData, solution.UniqueName, solution.Id);

                        cmBFilter.Text = text;
                    }
                });
            }
        }

        private void cmBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingSolutions();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteActionOnSingleSolution(Solution solution, Func<string, Solution, Task> action)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (solution == null)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            try
            {
                action(folder, solution);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        private void mIComponentsIn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ExecuteActionOnSingleSolution(solution, PerformCreateFileWithSolutionComponents);
            }
        }

        private void mICreateSolutionImageIn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ExecuteActionOnSingleSolution(solution, PerformCreateSolutionImage);
            }
        }

        private void mIOpenComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ExecuteActionOnSingleSolution(solution, PerformOpenSolutionComponentsInWindow);
            }
        }

        private void mIMissingComponentsIn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag is Solution solution
               )
            {
                ExecuteActionOnSingleSolution(solution, PerformShowingMissingDependencies);
            }
        }

        private void mIUninstallComponentsIn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag is Solution solution
               )
            {
                ExecuteActionOnSingleSolution(solution, PerformShowingDependenciesForUninstall);
            }
        }

        private async Task PerformCreateSolutionImage(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingFileWithSolutionImageFormat1, solution.UniqueName);

                var service = await GetService();
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "SolutionImage"
                    , "xml"
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solution.Id);

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedSolutionImageForConnectionFormat2, service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformCreateFileWithSolutionComponents(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingTextFileWithComponentsFormat1, solution.UniqueName);

                var service = await GetService();
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "Components"
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solution.Id);

                this._iWriteToOutput.WriteToOutput("Solution Components was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithComponentsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithComponentsFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformOpenSolutionComponentsInWindow(string folder, Solution solution)
        {
            try
            {
                _commonConfig.Save();

                var service = await GetService();
                var descriptor = await GetDescriptor();

                WindowHelper.OpenSolutionComponentDependenciesWindow(this._iWriteToOutput, service, descriptor, _commonConfig, solution.UniqueName, null);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        private async Task PerformShowingMissingDependencies(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingTextFileWithMissingDependenciesFormat1, solution.UniqueName);

                var service = await GetService();
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                ComponentsGroupBy showComponents = _commonConfig.ComponentsGroupBy;

                string showString = null;

                if (showComponents == ComponentsGroupBy.DependentComponents)
                {
                    showString = "dependent";
                }
                else
                {
                    showString = "required";
                }

                showString = string.Format("Missing Dependencies {0}", showString);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , showString
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionMissingDependenciesAsync(filePath, solution.Id, showComponents, showString);

                this._iWriteToOutput.WriteToOutput("Solution {0} was export into file '{1}'", showString, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithMissingDependenciesCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithMissingDependenciesFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformShowingDependenciesForUninstall(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingTextFileWithDependenciesForUninstallFormat1, solution.UniqueName);

                var service = await GetService();
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                ComponentsGroupBy showComponents = _commonConfig.ComponentsGroupBy;

                string showString = null;

                if (showComponents == ComponentsGroupBy.DependentComponents)
                {
                    showString = "dependent";
                }
                else
                {
                    showString = "required";
                }

                showString = string.Format("Dependencies for Uninstall {0}", showString);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , showString
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionDependenciesForUninstallAsync(filePath, solution.Id, showComponents, showString);

                this._iWriteToOutput.WriteToOutput("Solution {0} was export into file '{1}'", showString, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithDependenciesForUninstallCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithDependenciesForUninstallFailedFormat1, solution.UniqueName);
            }
        }

        private void mIAnalyzeSolutions_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag is Tuple<Solution, Solution> solutionPair
               && solutionPair.Item1 != null
               && solutionPair.Item2 != null
               && !string.Equals(solutionPair.Item1.UniqueName, solutionPair.Item2.UniqueName, StringComparison.InvariantCultureIgnoreCase)
               )
            {
                ExecuteActionOnSolutionPair(solutionPair.Item1, solutionPair.Item2, PerformAnalizeSolutions);
            }
        }

        private void mIShowUniqueComponentsInSolution_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Tuple<Solution, Solution> solutionPair
                && solutionPair.Item1 != null
                && solutionPair.Item2 != null
                && !string.Equals(solutionPair.Item1.UniqueName, solutionPair.Item2.UniqueName, StringComparison.InvariantCultureIgnoreCase)
                )
            {
                ExecuteActionOnSolutionPair(solutionPair.Item1, solutionPair.Item2, PerformAnalizeSolutionsAndShowUnique);
            }
        }

        private void ExecuteActionOnSolutionPair(Solution solution1, Solution solution2, Func<string, Solution, Solution, Task> action)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (solution1 == null || solution2 == null)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            try
            {
                action(folder, solution1, solution2);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        private void ExecuteActionOnSolutionAndSolutionCollection(Solution[] solutions, Solution solution, Func<string, Solution[], Solution, Task> action)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (solutions == null || solution == null)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            try
            {
                action(folder, solutions, solution);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        private async Task PerformAnalizeSolutions(string folder, Solution solution1, Solution solution2)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.AnalizingSolutionsFormat2, solution1.UniqueName, solution2.UniqueName);

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput("Analyzing Solution Components '{0}' and '{1}'.", solution1.UniqueName, solution2.UniqueName);

                var service = await GetService();
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                await solutionDescriptor.FindUniqueComponentsInSolutionsAsync(solution1.Id, solution2.Id);

                ToggleControls(true, Properties.WindowStatusStrings.AnalizingSolutionsCompletedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.AnalizingSolutionsFailedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
        }

        private async Task PerformAnalizeSolutionsAndShowUnique(string folder, Solution solution1, Solution solution2)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.AnalizingSolutionsFormat2, solution1.UniqueName, solution2.UniqueName);

                var service = await GetService();
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionMultipleFileName(
                    service.ConnectionData.Name
                    , solution1.UniqueName
                    , solution2.UniqueName
                    , "Unique Components"
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithUniqueComponentsInSolution1Async(filePath, solution1.Id, solution2.Id);

                this._iWriteToOutput.WriteToOutput("Unique Solution Components '{0}' was export into file '{1}'", solution1.UniqueName, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.AnalizingSolutionsCompletedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.AnalizingSolutionsFailedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
        }

        private void mICopyComponentsFromSolution1ToSolution2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag is Tuple<Solution, Solution> solutionPair
               && solutionPair.Item1 != null
               && solutionPair.Item2 != null
               && !string.Equals(solutionPair.Item1.UniqueName, solutionPair.Item2.UniqueName, StringComparison.InvariantCultureIgnoreCase)
               && solutionPair.Item2.IsManaged.GetValueOrDefault() == false
               )
            {
                string question = string.Format(Properties.MessageBoxStrings.CopySolutionComponentsFromToFormat2, solutionPair.Item1.UniqueName, solutionPair.Item2.UniqueName);

                if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                {
                    return;
                }

                ExecuteActionOnSolutionPair(solutionPair.Item1, solutionPair.Item2, PerformCopyFromSolution1ToSolution2);
            }
        }

        private void mICopyComponentsFromSolutionCollectionToSolution_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag is Tuple<Solution[], Solution> solutionPair
               && solutionPair.Item1 != null
               && solutionPair.Item2 != null
               && solutionPair.Item2.IsManaged.GetValueOrDefault() == false
               )
            {
                foreach (var item in solutionPair.Item1)
                {
                    if (string.Equals(item.UniqueName, solutionPair.Item2.UniqueName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return;
                    }
                }

                string sourceName = string.Join(",", solutionPair.Item1.Select(en => en.UniqueName).OrderBy(s => s));

                string question = string.Format(Properties.MessageBoxStrings.CopySolutionComponentsFromToFormat2, sourceName, solutionPair.Item2.UniqueName);

                if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                {
                    return;
                }

                ExecuteActionOnSolutionAndSolutionCollection(solutionPair.Item1, solutionPair.Item2, PerformCopyFromSolutionCollectionToSolution);
            }
        }

        private async Task PerformCopyFromSolution1ToSolution2(string folder, Solution solutionSource, Solution solutionTarget)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CopingSolutionComponentsToFromFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);

                var service = await GetService();
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                SolutionComponentRepository repository = new SolutionComponentRepository(service);

                var componentsSource = await repository.GetSolutionComponentsAsync(solutionSource.Id, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                var componentsTarget = await repository.GetSolutionComponentsAsync(solutionTarget.Id, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                var componentesOnlyInSource = SolutionDescriptor.GetComponentsInFirstNotSecond(componentsSource, componentsTarget);

                if (componentesOnlyInSource.Count > 0)
                {
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput("Creating backup Solution Components in '{0}'.", solutionTarget.UniqueName);

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , "Components Backup"
                        );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solutionTarget.Id);

                        this._iWriteToOutput.WriteToOutput("Created backup Solution Components in '{0}': {1}", solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                    }

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , "SolutionImage Backup"
                        );

                        fileName = fileName.Replace(".txt", ".xml");

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solutionTarget.Id);
                    }

                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput("Showing Unique Solution Components in '{0}'.", solutionSource.UniqueName);

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionMultipleFileName(
                            service.ConnectionData.Name
                            , solutionSource.UniqueName
                            , solutionTarget.UniqueName
                            , "Unique Components for Adding"
                            );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithUniqueComponentsInSolution1Async(filePath, solutionSource.Id, solutionTarget.Id);

                        this._iWriteToOutput.WriteToOutput("Created file with Unique Components in '{0}' for Adding to '{1}': {2}", solutionSource.UniqueName, solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                    }


                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput("Coping Solution Components from '{0}' into '{1}'.", solutionSource.UniqueName, solutionTarget.UniqueName);

                    await Controllers.SolutionController.AddSolutionComponentsCollectionIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionTarget.UniqueName, componentesOnlyInSource, false);

                    this._iWriteToOutput.WriteToOutput("Copied {0} components.", componentesOnlyInSource.Count.ToString());
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput("All Solution Components '{0}' already added into '{1}'.", solutionSource.UniqueName, solutionTarget.UniqueName);
                }

                ToggleControls(true, Properties.WindowStatusStrings.CopingSolutionComponentsToFromCompletedFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CopingSolutionComponentsToFromFailedFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);
            }
        }
        private async Task PerformCopyFromSolutionCollectionToSolution(string folder, Solution[] solutionSourceCollection, Solution solutionTarget)
        {
            string sourceName = string.Join(",", solutionSourceCollection.Select(e => e.UniqueName).OrderBy(s => s));

            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CopingSolutionComponentsToFromFormat2, solutionTarget.UniqueName, sourceName);

                var service = await GetService();
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                SolutionComponentRepository repository = new SolutionComponentRepository(service);

                List<SolutionComponent> componentsSource = new List<SolutionComponent>();

                {
                    var hash = new HashSet<Tuple<int, Guid>>();

                    var temp = await repository.GetSolutionComponentsForCollectionAsync(solutionSourceCollection.Select(e => e.Id), new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                    foreach (var item in temp)
                    {
                        if (hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value)))
                        {
                            componentsSource.Add(item);
                        }
                    }
                }

                var componentsTarget = await repository.GetSolutionComponentsAsync(solutionTarget.Id, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                var componentesOnlyInSource = SolutionDescriptor.GetComponentsInFirstNotSecond(componentsSource, componentsTarget);

                var componentesOnlyInTarget = SolutionDescriptor.GetComponentsInFirstNotSecond(componentsTarget, componentsSource);

                if (componentesOnlyInSource.Count > 0)
                {
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput("Creating backup Solution Components in '{0}'.", solutionTarget.UniqueName);


                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , "Components Backup"
                            , "txt"
                        );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solutionTarget.Id);

                        this._iWriteToOutput.WriteToOutput("Created backup Solution Components in '{0}': {1}", solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                    }

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , "SolutionImage Backup"
                            , "xml"
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solutionTarget.Id);
                    }

                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput("Showing Unique Solution Components in '{0}'.", sourceName);

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionMultipleFileName(
                            service.ConnectionData.Name
                            , sourceName
                            , solutionTarget.UniqueName
                            , "Unique Components for Adding"
                            );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithComponentsInSolution1Async(
                            filePath
                            , sourceName
                            , solutionTarget.UniqueName
                            , componentsSource.Count
                            , componentsTarget.Count
                            , componentesOnlyInSource
                            , componentesOnlyInTarget.Count
                            );

                        this._iWriteToOutput.WriteToOutput("Created file with Unique Components in '{0}' for Adding to '{1}': {2}", sourceName, solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                    }

                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput("Coping Solution Components from '{0}' into '{1}'.", sourceName, solutionTarget.UniqueName);

                    await Controllers.SolutionController.AddSolutionComponentsCollectionIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionTarget.UniqueName, componentesOnlyInSource, false);

                    this._iWriteToOutput.WriteToOutput("Copied {0} components.", componentesOnlyInSource.Count.ToString());
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput("All Solution Components '{0}' already added into '{1}'.", sourceName, solutionTarget.UniqueName);
                }

                ToggleControls(true, Properties.WindowStatusStrings.CopingSolutionComponentsToFromCompletedFormat2, solutionTarget.UniqueName, sourceName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CopingSolutionComponentsToFromFailedFormat2, solutionTarget.UniqueName, sourceName);
            }
        }

        private void miCreateNewSolution_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionCreateInWeb();
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

        private void mIClearUnmanagedSolution_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                && solution.IsManaged.GetValueOrDefault() == false
                )
            {
                string question = string.Format(Properties.MessageBoxStrings.ClearSolutionFormat1, solution.UniqueName);

                if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                {
                    return;
                }

                ExecuteActionOnSingleSolution(solution, PerformClearUnmanagedSolution);
            }
        }

        private async Task PerformClearUnmanagedSolution(string folder, Solution solution)
        {
            var service = await GetService();
            var descriptor = await GetDescriptor();

            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.ClearingSolutionFormat2, service.ConnectionData.Name, solution.UniqueName);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput("Creating backup Solution Components in '{0}'.", solution.UniqueName);

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        service.ConnectionData.Name
                        , solution.UniqueName
                        , "Components Backup"
                        , "txt"
                    );

                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

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

                ToggleControls(true, Properties.WindowStatusStrings.ClearingSolutionCompletedFormat2, service.ConnectionData.Name, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ClearingSolutionFailedFormat2, service.ConnectionData.Name, solution.UniqueName);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSolutions();
            }

            if (!e.Handled)
            {
                if (e.Key == Key.Escape
                    || (e.Key == Key.W && e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    )
                {
                    if (_optionsPopup.IsOpen)
                    {
                        _optionsPopup.IsOpen = false;
                        e.Handled = true;
                    }
                }
            }

            base.OnKeyDown(e);
        }

        private void mIOpenSolutionInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                if (connectionData != null)
                {
                    connectionData.OpenSolutionInWeb(solution.Id);
                }
            }
        }

        private void mIUsedEntitiesInWorkflows_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ExecuteActionOnSingleSolution(solution, PerformCreateFileWithUsedEntitiesInWorkflows);
            }
        }

        private void mIUsedNotExistsEntitiesInWorkflows_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ExecuteActionOnSingleSolution(solution, PerformCreateFileWithUsedNotExistsEntitiesInWorkflows);
            }
        }

        private async Task PerformCreateFileWithUsedEntitiesInWorkflows(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingFileWithUsedEntitiesInWorkflowsFormat1, solution.UniqueName);

                var service = await GetService();
                var descriptor = await GetDescriptor();

                var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "UsedEntitiesInWorkflows"
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var stringBuider = new StringBuilder();

                await workflowDescriptor.GetDescriptionWithUsedEntitiesInSolutionWorkflowsAsync(stringBuider, solution.Id);

                File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("Solution Used Entities was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithUsedEntitiesInWorkflowsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithUsedEntitiesInWorkflowsFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformCreateFileWithUsedNotExistsEntitiesInWorkflows(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsFormat1, solution.UniqueName);

                var service = await GetService();
                var descriptor = await GetDescriptor();

                var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "UsedNotExistsEntitiesInWorkflows"
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var stringBuider = new StringBuilder();

                await workflowDescriptor.GetDescriptionWithUsedNotExistsEntitiesInSolutionWorkflowsAsync(stringBuider, solution.Id);

                File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("Solution Used Not Exists Entities was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsFailedFormat1, solution.UniqueName);
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                contextMenu.Items.Clear();

                var list = lstVwSolutions.SelectedItems
                                 .OfType<EntityViewItem>()
                                 .Select(en => en.Solution)
                                 .OrderBy(en => en.UniqueName)
                                 .ToList();

                if (list.Any())
                {
                    if (list.Count == 1)
                    {
                        var solution = list.First();

                        FillSolutionButtons(solution, contextMenu.Items);
                    }
                    else
                    {
                        foreach (var solution in list)
                        {
                            if (contextMenu.Items.Count > 0)
                            {
                                contextMenu.Items.Add(new Separator());
                            }

                            MenuItem menuItem = new MenuItem()
                            {
                                Header = string.Format("Solution {0}", solution.UniqueNameEscapeUnderscore),
                            };

                            contextMenu.Items.Add(menuItem);

                            FillSolutionButtons(solution, menuItem.Items);
                        }
                    }
                }
            }
        }

        private void btnClearSolutionComponentFilter_Click(object sender, RoutedEventArgs e)
        {
            this._componentType = null;
            this._objectId = null;

            this.Dispatcher.Invoke(() =>
            {
                sepClearSolutionComponentFilter.IsEnabled = btnClearSolutionComponentFilter.IsEnabled = false;
                sepClearSolutionComponentFilter.Visibility = btnClearSolutionComponentFilter.Visibility = Visibility.Collapsed;
            });

            ShowExistingSolutions();
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems.OfType<ConnectionData>())
            {
                removed.LastSelectedSolutionsUniqueName.CollectionChanged -= LastSelectedSolutionsUniqueName_CollectionChanged;
                removed.LastSelectedSolutionsUniqueName.CollectionChanged -= LastSelectedSolutionsUniqueName_CollectionChanged;
            }

            foreach (var added in e.AddedItems.OfType<ConnectionData>())
            {
                added.LastSelectedSolutionsUniqueName.CollectionChanged -= LastSelectedSolutionsUniqueName_CollectionChanged;
                added.LastSelectedSolutionsUniqueName.CollectionChanged -= LastSelectedSolutionsUniqueName_CollectionChanged;
                added.LastSelectedSolutionsUniqueName.CollectionChanged += LastSelectedSolutionsUniqueName_CollectionChanged;
            }

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                BindCollections(connectionData);
            });

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (connectionData != null)
            {
                FillCopyComponentsToLastSelectedSolutionsAsync();

                ShowExistingSolutions();
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

                BindingOperations.EnableCollectionSynchronization(connectionData.LastSelectedSolutionsUniqueName, _syncCacheObjects[connectionData.ConnectionId]);

                BindingOperations.EnableCollectionSynchronization(connectionData.LastSolutionExportFolders, _syncCacheObjects[connectionData.ConnectionId]);
                BindingOperations.EnableCollectionSynchronization(connectionData.LastExportSolutionOverrideUniqueName, _syncCacheObjects[connectionData.ConnectionId]);
                BindingOperations.EnableCollectionSynchronization(connectionData.LastExportSolutionOverrideDisplayName, _syncCacheObjects[connectionData.ConnectionId]);
                BindingOperations.EnableCollectionSynchronization(connectionData.LastExportSolutionOverrideVersion, _syncCacheObjects[connectionData.ConnectionId]);
            }

            _optionsExportSolutionOptionsControl.StoreTextValues();

            connectionData.AddLastSolutionExportFolder(_commonConfig.FolderForExport);

            _optionsExportSolutionOptionsControl.RestoreTextValues();
        }

        private void lstVwSolutions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

                if (item != null && item.Solution != null)
                {
                    ExecuteActionOnSingleSolution(item.Solution, PerformOpenSolutionComponentsInWindow);
                }
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

        private async void mICreateSolutionImageFromZipFile_Click(object sender, RoutedEventArgs e)
        {
            string folder = txtBFolder.Text.Trim();

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            try
            {
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
                        DTEHelper.WriteExceptionToOutput(ex);
                    }
                });

                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (string.IsNullOrEmpty(selectedPath) || !File.Exists(selectedPath))
                {
                    return;
                }

                ToggleControls(false, _iWriteToOutput.WriteToOutput(Properties.WindowStatusStrings.CreatingSolutionImageFromZipFile));

                var service = await GetService();
                var descriptor = await GetDescriptor();

                var components = await descriptor.LoadSolutionComponentsFromZipFileAsync(selectedPath);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , Path.GetFileNameWithoutExtension(selectedPath)
                    , "SolutionImage"
                    , "xml"
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateSolutionImageWithComponentsAsync(filePath, components);

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedSolutionImageForConnectionFormat2, service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingSolutionImageFromZipFileCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingSolutionImageFromZipFileFailed);
            }
        }

        private void miExportSolutionOptions_Click(object sender, RoutedEventArgs e)
        {
            this._optionsPopup.IsOpen = true;
            this._optionsPopup.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_optionsPopup.IsOpen)
            {
                _optionsPopup.IsOpen = false;
                this.Focus();
            }
        }
    }
}