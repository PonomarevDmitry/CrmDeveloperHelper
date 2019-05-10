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

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly EnvDTE.SelectedItem _selectedItem;

        private Guid? _objectId;
        private int? _componentType;

        private readonly Popup _optionsPopup;

        private readonly Popup _optionsSolutionPopup;
        private readonly ExportSolutionOptionsControl _optionsExportSolutionOptionsControl;

        private readonly XmlOptionsControls _xmlOptions = XmlOptionsControls.SolutionComponentSettings;

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private readonly Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        public WindowExplorerSolution(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , int? componentType
            , Guid? objectId
            , EnvDTE.SelectedItem selectedItem
            )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._selectedItem = selectedItem;
            this._objectId = objectId;
            this._componentType = componentType;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            InitializeComponent();

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            this._optionsExportSolutionOptionsControl = new ExportSolutionOptionsControl(cmBCurrentConnection, cmBExportSolutionProfile);
            this._optionsExportSolutionOptionsControl.CloseClicked += Child_CloseClicked;
            this._optionsSolutionPopup = new Popup
            {
                Child = this._optionsExportSolutionOptionsControl,

                PlacementTarget = gridExportProfile,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            {
                var child = new ExportXmlOptionsControl(_commonConfig, _xmlOptions);
                child.CloseClicked += Child_CloseClicked;
                this._optionsPopup = new Popup
                {
                    Child = child,

                    PlacementTarget = toolBarHeader,
                    Placement = PlacementMode.Bottom,
                    StaysOpen = false,
                    Focusable = true,
                };
            }

            LoadFromConfig();

            cmBFilter.Text = service.ConnectionData.ExplorerSolutionFilter;

            sepClearSolutionComponentFilter.IsEnabled = btnClearSolutionComponentFilter.IsEnabled = _objectId.HasValue && _componentType.HasValue;
            sepClearSolutionComponentFilter.Visibility = btnClearSolutionComponentFilter.Visibility = (_objectId.HasValue && _componentType.HasValue) ? Visibility.Visible : Visibility.Collapsed;

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSolutions.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            service.ConnectionData.LastSelectedSolutionsUniqueName.CollectionChanged -= LastSelectedSolutionsUniqueName_CollectionChanged;
            service.ConnectionData.LastSelectedSolutionsUniqueName.CollectionChanged += LastSelectedSolutionsUniqueName_CollectionChanged;

            BindCollections(service.ConnectionData);

            this.DecreaseInit();

            FocusOnComboBoxTextBox(cmBFilter);

            if (service != null)
            {
                ShowExistingSolutions();
            }
        }

        private void LastSelectedSolutionsUniqueName_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(FillChangeComponentsToLastSelectedSolutionsAsync);
        }

        private void LoadFromConfig()
        {
            txtBFolder.DataContext = _commonConfig;

            cmBFileAction.DataContext = _commonConfig;

            cmBGroupBy.DataContext = _commonConfig;

            cmBFilter.DataContext = cmBCurrentConnection;

            cmBExportSolutionProfile.DataContext = cmBCurrentConnection;

            if (this._selectedItem != null)
            {
                string exportFolder = string.Empty;

                if (_selectedItem.ProjectItem != null)
                {
                    exportFolder = _selectedItem.ProjectItem.FileNames[1];
                }
                else if (_selectedItem.Project != null)
                {
                    string relativePath = DTEHelper.GetRelativePath(_selectedItem.Project);

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

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();

            _optionsExportSolutionOptionsControl.DetachCollections();

            (cmBCurrentConnection.SelectedItem as ConnectionData)?.Save();

            cmBCurrentConnection.DataContext = null;
            cmBFilter.DataContext = null;
            cmBExportSolutionProfile.DataContext = null;

            BindingOperations.ClearAllBindings(cmBCurrentConnection);
            BindingOperations.ClearAllBindings(cmBFilter);
            BindingOperations.ClearAllBindings(cmBExportSolutionProfile);

            cmBFilter.Items.DetachFromSourceCollection();
            cmBCurrentConnection.Items.DetachFromSourceCollection();
            cmBExportSolutionProfile.Items.DetachFromSourceCollection();

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

            return await GetService(connectionData);
        }

        private async Task<IOrganizationServiceExtented> GetService(ConnectionData connectionData)
        {
            if (connectionData != null)
            {
                if (!_connectionCache.ContainsKey(connectionData.ConnectionId))
                {
                    ToggleControls(connectionData, false, string.Empty);

                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;

                    ToggleControls(connectionData, true, string.Empty);
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

            return await GetDescriptor(connectionData);
        }

        private async Task<SolutionComponentDescriptor> GetDescriptor(ConnectionData connectionData)
        {
            if (connectionData != null)
            {
                if (!_descriptorCache.ContainsKey(connectionData.ConnectionId))
                {
                    var service = await GetService(connectionData);

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(service);
                }

                _descriptorCache[connectionData.ConnectionId].SetSettings(_commonConfig);

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingSolutions()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingSolutions);

            this._itemsSource.Clear();

            string textName = string.Empty;

            cmBFilter.Dispatcher.Invoke(() =>
            {
                textName = cmBFilter.Text?.Trim().ToLower();
            });

            IEnumerable<Solution> list = Enumerable.Empty<Solution>();

            try
            {
                if (service != null)
                {
                    SolutionRepository repository = new SolutionRepository(service);

                    list = await repository.GetSolutionsAllAsync(textName, _componentType, _objectId);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadSolutions(list);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingSolutionsCompletedFormat1, list.Count());
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

            public string Description => Solution.Description;

            public bool HasDescription => !string.IsNullOrEmpty(Solution.Description);

            public EntityViewItem(Solution solution)
            {
                this.Solution = solution;
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

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwSolutions.Dispatcher.Invoke(() =>
            {
                try
                {
                    {
                        bool enabled = this.IsControlsEnabled;

                        menuCompareSolutions.IsEnabled = tSDDBCompareSolutions.IsEnabled = enabled;

                        tSDDBCompareSolutions.Items.Clear();

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
                                    if (tSDDBCompareSolutions.Items.Count > 0)
                                    {
                                        tSDDBCompareSolutions.Items.Add(new Separator());
                                    }

                                    MenuItem menuItemSolutuion1 = new MenuItem()
                                    {
                                        Header = string.Format("Solution {0}", solution1.UniqueNameEscapeUnderscore),
                                    };

                                    tSDDBCompareSolutions.Items.Add(menuItemSolutuion1);

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

                                        MenuItem mICompareSolutions = new MenuItem()
                                        {
                                            Header = string.Format("Compare Solutions {0} and {1}", solution1.UniqueNameEscapeUnderscore, solution2.UniqueNameEscapeUnderscore),
                                            Tag = Tuple.Create(solution1, solution2),
                                        };
                                        mICompareSolutions.Click += mICompareSolutions_Click;

                                        MenuItem mIShowUniqueComponentsIn1 = new MenuItem()
                                        {
                                            Header = string.Format("Show Unique Components in {0} compare to {1}", solution1.UniqueNameEscapeUnderscore, solution2.UniqueNameEscapeUnderscore),
                                            Tag = Tuple.Create(solution1, solution2),
                                        };
                                        mIShowUniqueComponentsIn1.Click += mIShowUniqueComponentsInSolution_Click;

                                        MenuItem mIShowUniqueComponentsIn2 = new MenuItem()
                                        {
                                            Header = string.Format("Show Unique Components in {0} compare to {1}", solution2.UniqueNameEscapeUnderscore, solution1.UniqueNameEscapeUnderscore),
                                            Tag = Tuple.Create(solution2, solution1),
                                        };
                                        mIShowUniqueComponentsIn2.Click += mIShowUniqueComponentsInSolution_Click;

                                        menuItemSolutuion2.Items.Add(mICompareSolutions);
                                        menuItemSolutuion2.Items.Add(new Separator());
                                        menuItemSolutuion2.Items.Add(mIShowUniqueComponentsIn1);
                                        menuItemSolutuion2.Items.Add(new Separator());
                                        menuItemSolutuion2.Items.Add(mIShowUniqueComponentsIn2);
                                    }
                                }
                            }
                            else
                            {
                                menuCompareSolutions.IsEnabled = tSDDBCompareSolutions.IsEnabled = false;
                            }
                        }
                    }

                    {
                        bool enabled = this.IsControlsEnabled;

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
                        bool enabled = this.IsControlsEnabled;

                        tSDDBCopyComponents.IsEnabled = enabled;
                        tSDDBCopyComponents.Items.Clear();

                        tSDDBRemoveComponents.IsEnabled = enabled;
                        tSDDBRemoveComponents.Items.Clear();

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

                                FillRemoveComponentsMenuItems(tSDDBRemoveComponents, listFrom, listTo);
                            }
                        }

                        tSDDBCopyComponents.IsEnabled = tSDDBCopyComponents.Items.Count > 0;
                        tSDDBRemoveComponents.IsEnabled = tSDDBRemoveComponents.Items.Count > 0;
                    }

                    menuItemChangeComponents.IsEnabled =
                        tSDDBCopyComponents.Items.Count > 0
                        || tSDDBCopyComponentsLastSolution.Items.Count > 0
                        || tSDDBRemoveComponents.Items.Count > 0
                        || tSDDBRemoveComponentsLastSolution.Items.Count > 0
                        ;

                    {
                        bool enabled = this.IsControlsEnabled;

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

                    FillChangeComponentsToLastSelectedSolutionsAsync();
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(null, ex);
                }
            });
        }

        private async Task FillChangeComponentsToLastSelectedSolutionsAsync()
        {
            bool enabled = this.IsControlsEnabled;

            tSDDBCopyComponentsLastSolution.IsEnabled = enabled;
            tSDDBCopyComponentsLastSolution.Items.Clear();

            tSDDBRemoveComponentsLastSolution.IsEnabled = enabled;
            tSDDBRemoveComponentsLastSolution.Items.Clear();

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

                        FillRemoveComponentsMenuItems(tSDDBRemoveComponentsLastSolution, listFrom, listTo);
                    }
                }
            }

            tSDDBCopyComponentsLastSolution.IsEnabled = tSDDBCopyComponentsLastSolution.Items.Count > 0;
            tSDDBRemoveComponentsLastSolution.IsEnabled = tSDDBRemoveComponentsLastSolution.Items.Count > 0;

            menuItemChangeComponents.IsEnabled =
                tSDDBCopyComponents.Items.Count > 0
                || tSDDBCopyComponentsLastSolution.Items.Count > 0
                || tSDDBRemoveComponents.Items.Count > 0
                || tSDDBRemoveComponentsLastSolution.Items.Count > 0
                ;
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

        private void FillRemoveComponentsMenuItems(MenuItem parentMenuItem, List<Solution> listFrom, List<Solution> listTo)
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
                        Header = string.Format("Remove Components owned by All Selected Solutions from {0}", solutionTo.UniqueNameEscapeUnderscore),
                        Tag = Tuple.Create(listFromFiltered.ToArray(), solutionTo),
                    };
                    menuItem.Click += mIRemoveComponentsOwnedSolutionCollectionFromSolution_Click;

                    menuItemSolution.Items.Add(menuItem);
                }

                foreach (var solutionFrom in listFromFiltered)
                {
                    MenuItem menuItem = new MenuItem()
                    {
                        Header = string.Format("Remove Components owned by {0} from {1}", solutionFrom.UniqueNameEscapeUnderscore, solutionTo.UniqueNameEscapeUnderscore),
                        Tag = Tuple.Create(solutionFrom, solutionTo),
                    };
                    menuItem.Click += mIRemoveComponentsOwnedSolution1FromSolution2_Click;

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

            MenuItem mICreateEntityDescription = new MenuItem()
            {
                Header = "Create Entity Description",
                Tag = solution,
            };
            mICreateEntityDescription.Click += mICreateEntityDescription_Click;

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

            MenuItem mICreateSolutionImage = new MenuItem()
            {
                Header = string.Format("Create Solution Image for {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mICreateSolutionImage.Click += mICreateSolutionImage_Click;

            MenuItem mICreateSolutionImageAndOpenOrganizationComparer = new MenuItem()
            {
                Header = string.Format("Create Solution Image for {0} and Open Organization Comparer Window", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mICreateSolutionImageAndOpenOrganizationComparer.Click += mICreateSolutionImageAndOpenOrganizationComparer_Click;

            MenuItem mIComponents = new MenuItem()
            {
                Header = string.Format("Components in {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mIComponents.Click += mIComponents_Click;




            MenuItem mIMissingComponents = new MenuItem()
            {
                Header = string.Format("Missing Components for {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mIMissingComponents.Click += mIMissingComponents_Click;

            MenuItem mIUninstallComponents = new MenuItem()
            {
                Header = string.Format("Uninstall Components for {0}", solution.UniqueNameEscapeUnderscore),
                Tag = solution,
            };
            mIUninstallComponents.Click += mIUninstallComponents_Click;




            itemCollection.Add(mIOpenComponentsInWindow);
            itemCollection.Add(new Separator());
            itemCollection.Add(mIOpenSolutionInWeb);

            if (!string.IsNullOrEmpty(solution.Description))
            {
                MenuItem mIOpenSolutionDescription = new MenuItem()
                {
                    Header = string.Format("Open Solution Description for {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mIOpenSolutionDescription.Click += mIOpenSolutionDescription_Click;

                itemCollection.Add(new Separator());
                itemCollection.Add(mIOpenSolutionDescription);
            }

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
            itemCollection.Add(mICreateSolutionImage);
            itemCollection.Add(mICreateSolutionImageAndOpenOrganizationComparer);

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

                var connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                if (connectionData != null)
                {
                    var otherConnections = connectionData.ConnectionConfiguration.Connections.Where(c => c.ConnectionId != connectionData.ConnectionId).ToList();

                    if (otherConnections.Any())
                    {
                        MenuItem mICheckImportPossibility = new MenuItem()
                        {
                            Header = string.Format("Check Possibility to Import {0} into", solution.UniqueNameEscapeUnderscore),
                            Tag = solution,
                        };

                        foreach (var connection in otherConnections)
                        {
                            MenuItem menuItem = new MenuItem()
                            {
                                Header = connection.Name,
                                Tag = connection,
                            };
                            menuItem.Click += mICheckImportPossibility_Click;

                            if (mICheckImportPossibility.Items.Count > 0)
                            {
                                mICheckImportPossibility.Items.Add(new Separator());
                            }

                            mICheckImportPossibility.Items.Add(menuItem);
                        }

                        MenuItem mICheckImportPossibilityAndExportSolution = new MenuItem()
                        {
                            Header = string.Format("Check Possibility to Import {0} into Connection and Export Solution", solution.UniqueNameEscapeUnderscore),
                            Tag = solution,
                        };

                        foreach (var connection in otherConnections)
                        {
                            MenuItem menuItem = new MenuItem()
                            {
                                Header = connection.Name,
                                Tag = connection,
                            };
                            menuItem.Click += mICheckImportPossibilityAndExportSolution_Click;

                            if (mICheckImportPossibilityAndExportSolution.Items.Count > 0)
                            {
                                mICheckImportPossibilityAndExportSolution.Items.Add(new Separator());
                            }

                            mICheckImportPossibilityAndExportSolution.Items.Add(menuItem);
                        }

                        itemCollection.Add(new Separator());
                        itemCollection.Add(mICheckImportPossibility);

                        itemCollection.Add(new Separator());
                        itemCollection.Add(mICheckImportPossibilityAndExportSolution);
                    }
                }
            }

            itemCollection.Add(new Separator());
            itemCollection.Add(mIComponents);
            itemCollection.Add(new Separator());
            itemCollection.Add(mIMissingComponents);
            itemCollection.Add(mIUninstallComponents);
        }

        private async void mICheckImportPossibility_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (!(sender is MenuItem menuItem)
                || menuItem.Tag == null
                || !(menuItem.Tag is ConnectionData targetConnectionData)
                || menuItem.Parent == null
                || !(menuItem.Parent is MenuItem parentMenuItem)
                || parentMenuItem.Tag == null
                || !(parentMenuItem.Tag is Solution solution)
                || solution.IsManaged.GetValueOrDefault()
            )
            {
                return;
            }

            await CheckImportPossibility(targetConnectionData, solution);
        }

        private async void mICheckImportPossibilityAndExportSolution_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (!(sender is MenuItem menuItem)
                || menuItem.Tag == null
                || !(menuItem.Tag is ConnectionData targetConnectionData)
                || menuItem.Parent == null
                || !(menuItem.Parent is MenuItem parentMenuItem)
                || parentMenuItem.Tag == null
                || !(parentMenuItem.Tag is Solution solution)
                || solution.IsManaged.GetValueOrDefault()
            )
            {
                return;
            }

            if (await CheckImportPossibility(targetConnectionData, solution))
            {
                await PerformExportSolution(solution);
            }
        }

        private async Task<bool> CheckImportPossibility(ConnectionData targetConnectionData, Solution solution)
        {
            var service = await GetService();
            var descriptor = await GetDescriptor();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.GettingAllRequiredComponentsFormat1, solution.UniqueName);

            DependencyRepository repository = new DependencyRepository(service);

            var fullMissingComponents = await repository.GetSolutionAllRequiredComponentsAsync(solution.Id, solution.UniqueName);

            await descriptor.GetSolutionImageComponentsListAsync(fullMissingComponents);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.GettingAllRequiredComponentsCompletedFormat1, solution.UniqueName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.RemovingComponentsExistingInTargetFormat1, targetConnectionData.Name);

            var targetService = await GetService(targetConnectionData);
            var targetDescription = await GetDescriptor(targetConnectionData);

            List<SolutionComponent> existingComponentsInTarget = new List<SolutionComponent>();

            foreach (var item in fullMissingComponents)
            {
                var imageList = await descriptor.GetSolutionImageComponentsListAsync(new[] { item });

                bool allExists = true;

                foreach (var imageItem in imageList)
                {
                    var targetComponents = await targetDescription.GetSolutionComponentsListAsync(new[] { imageItem });

                    if (!targetComponents.Any())
                    {
                        allExists = false;
                        break;
                    }
                }

                if (allExists)
                {
                    existingComponentsInTarget.Add(item);
                }
            }

            foreach (var item in existingComponentsInTarget)
            {
                fullMissingComponents.Remove(item);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.RemovingComponentsExistingInTargetCompletedFormat1, targetConnectionData.Name);

            if (!fullMissingComponents.Any())
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.WindowStatusStrings.AllRequiredComponentsExistsInTargetFormat1, targetConnectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                return true;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerComponentsWindow(_iWriteToOutput, service, descriptor, _commonConfig, fullMissingComponents, solution.UniqueName, "AllMissingDependencies", null);

            return false;
        }

        private async void mIExportSolution_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (!(sender is MenuItem menuItem)
                || menuItem.Tag == null
                || !(menuItem.Tag is Solution solution)
                || solution.IsManaged.GetValueOrDefault()
            )
            {
                return;
            }

            await PerformExportSolution(solution);
        }

        private async Task PerformExportSolution(Solution solution)
        {
            ConnectionData connectionData = null;
            ExportSolutionProfile exportSolutionProfile = null;

            this.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                exportSolutionProfile = cmBExportSolutionProfile.SelectedItem as ExportSolutionProfile;
            });

            if (connectionData == null)
            {
                return;
            }

            if (exportSolutionProfile == null)
            {
                return;
            }

            string exportFolder = _optionsExportSolutionOptionsControl.GetExportFolder();

            if (string.IsNullOrEmpty(exportFolder))
            {
                return;
            }

            var fileExportFolder = exportFolder;

            var uniqueName = exportSolutionProfile.OverrideUniqueName?.Trim() ?? string.Empty;
            var displayName = exportSolutionProfile.OverrideDisplayName?.Trim() ?? string.Empty;
            var version = exportSolutionProfile.OverrideVersion?.Trim() ?? string.Empty;

            var description = exportSolutionProfile.OverrideDescription?.Trim() ?? string.Empty;

            if (exportSolutionProfile.IsOverrideSolutionNameAndVersion)
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

            if (exportSolutionProfile.IsCreateFolderForVersion)
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
                exportSolutionProfile.IsOverrideSolutionNameAndVersion
                , uniqueName
                , displayName
                , version
                , exportSolutionProfile.IsOverrideSolutionDescription
                , description
                );

            ExportSolutionConfig config = new ExportSolutionConfig()
            {
                ExportAutoNumberingSettings = exportSolutionProfile.ExportAutoNumberingSettings,
                ExportCalendarSettings = exportSolutionProfile.ExportCalendarSettings,
                ExportCustomizationSettings = exportSolutionProfile.ExportCustomizationSettings,
                ExportEmailTrackingSettings = exportSolutionProfile.ExportEmailTrackingSettings,
                ExportExternalApplications = exportSolutionProfile.ExportExternalApplications,
                ExportGeneralSettings = exportSolutionProfile.ExportGeneralSettings,
                ExportIsvConfig = exportSolutionProfile.ExportIsvConfig,
                ExportMarketingSettings = exportSolutionProfile.ExportMarketingSettings,
                ExportOutlookSynchronizationSettings = exportSolutionProfile.ExportOutlookSynchronizationSettings,
                ExportRelationshipRoles = exportSolutionProfile.ExportRelationshipRoles,
                ExportSales = exportSolutionProfile.ExportSales,

                Managed = exportSolutionProfile.IsManaged,

                IdSolution = solution.Id,

                ConnectionName = connectionData.Name,

                ExportFolder = fileExportFolder,
            };

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingSolutionFormat1, solution.UniqueName);

            try
            {
                if (service != null)
                {
                    _iWriteToOutput.WriteToOutputSolutionUri(service.ConnectionData, solution.UniqueName, solution.Id);

                    if (solutionExportInfo.OverrideNameAndVersion)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            _optionsExportSolutionOptionsControl.StoreTextValues();

                            exportSolutionProfile.AddLastOverrideUniqueName(solutionExportInfo.UniqueName);
                            exportSolutionProfile.AddLastOverrideDisplayName(solutionExportInfo.DisplayName);

                            _optionsExportSolutionOptionsControl.RestoreTextValues();

                            if (!string.IsNullOrEmpty(solutionExportInfo.Version) && Version.TryParse(solutionExportInfo.Version, out Version ver))
                            {
                                var oldVersion = ver.ToString();
                                var newVersion = new Version(ver.Major, ver.Minor, ver.Build, ver.Revision + 1).ToString();

                                exportSolutionProfile.AddLastOverrideVersion(newVersion, oldVersion);

                                _optionsExportSolutionOptionsControl.SetNewVersion(newVersion);
                            }
                        });
                    }

                    ExportSolutionHelper helper = new ExportSolutionHelper(service);

                    var filePath = await helper.ExportAsync(config, solutionExportInfo);

                    this.Dispatcher.Invoke(() =>
                    {
                        if (exportSolutionProfile.IsCopyFileToClipBoard)
                        {
                            Clipboard.SetFileDropList(new System.Collections.Specialized.StringCollection() { filePath });
                        }
                    });

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution {0} exported to {1}", solution.UniqueName, filePath);

                    this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

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

                    this._iWriteToOutput.SelectFileInFolder(service.ConnectionData, filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Cannot get Service.");
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingSolutionCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);


                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingSolutionFailedFormat1, solution.UniqueName);
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

        private async void ExecuteActionOnSingleSolution(Solution solution, Func<string, Solution, Task> action)
        {
            if (!this.IsControlsEnabled)
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
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            try
            {
                await action(folder, solution);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void ExecuteActionOnSingleSolutionWithoutFolderCheck(Solution solution, Func<string, Solution, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (solution == null)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            try
            {
                action(folder, solution);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void mIComponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ExecuteActionOnSingleSolution(solution, PerformCreateFileWithSolutionComponents);
            }
        }

        private void mICreateSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ExecuteActionOnSingleSolution(solution, PerformCreateSolutionImage);
            }
        }

        private void mICreateSolutionImageAndOpenOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ExecuteActionOnSingleSolution(solution, PerformCreateSolutionImageAndOpenOrganizationComparer);
            }
        }

        private void mIOpenComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ExecuteActionOnSingleSolutionWithoutFolderCheck(solution, PerformOpenSolutionComponentsInWindow);
            }
        }

        private void mIOpenSolutionDescription_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                && !string.IsNullOrEmpty(solution.Description)
                )
            {
                ExecuteActionOnSingleSolutionWithoutFolderCheck(solution, PerformOpenSolutionDescriptionInWindow);
            }
        }

        private void mIMissingComponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag is Solution solution
               )
            {
                ExecuteActionOnSingleSolution(solution, PerformShowingMissingDependencies);
            }
        }

        private void mIUninstallComponents_Click(object sender, RoutedEventArgs e)
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
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileWithSolutionImageFormat1, solution.UniqueName);
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "SolutionImage"
                    , "xml"
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solution.Id, solution.UniqueName);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedSolutionImageForConnectionFormat2, service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformCreateSolutionImageAndOpenOrganizationComparer(string folder, Solution solution)
        {
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileWithSolutionImageFormat1, solution.UniqueName);
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "SolutionImage"
                    , "xml"
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solution.Id, solution.UniqueName);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedSolutionImageForConnectionFormat2, service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                _commonConfig.Save();

                WindowHelper.OpenOrganizationComparerWindow(_iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformCreateFileWithSolutionComponents(string folder, Solution solution)
        {
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingTextFileWithComponentsFormat1, solution.UniqueName);
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "Components"
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solution.Id);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution Components was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingTextFileWithComponentsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingTextFileWithComponentsFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformOpenSolutionComponentsInWindow(string folder, Solution solution)
        {
            var service = await GetService();

            try
            {
                _commonConfig.Save();
                var descriptor = await GetDescriptor();

                WindowHelper.OpenSolutionComponentDependenciesWindow(this._iWriteToOutput, service, descriptor, _commonConfig, solution.UniqueName, null);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async Task PerformOpenSolutionDescriptionInWindow(string folder, Solution solution)
        {
            if (solution == null || string.IsNullOrEmpty(solution.Description))
            {
                return;
            }

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var title = solution.UniqueName + " Description";

                    var form = new WindowTextField(title, title, solution.Description, true);

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        private async Task PerformShowingMissingDependencies(string folder, Solution solution)
        {
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingTextFileWithMissingDependenciesFormat1, solution.UniqueName);
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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution {0} was export into file '{1}'", showString, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingTextFileWithMissingDependenciesCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingTextFileWithMissingDependenciesFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformShowingDependenciesForUninstall(string folder, Solution solution)
        {
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingTextFileWithDependenciesForUninstallFormat1, solution.UniqueName);
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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution {0} was export into file '{1}'", showString, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingTextFileWithDependenciesForUninstallCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingTextFileWithDependenciesForUninstallFailedFormat1, solution.UniqueName);
            }
        }

        private void mICompareSolutions_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag is Tuple<Solution, Solution> solutionPair
               && solutionPair.Item1 != null
               && solutionPair.Item2 != null
               && !string.Equals(solutionPair.Item1.UniqueName, solutionPair.Item2.UniqueName, StringComparison.InvariantCultureIgnoreCase)
               )
            {
                ExecuteActionOnSolutionPair(solutionPair.Item1, solutionPair.Item2, PerformCompareSolutions);
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
                ExecuteActionOnSolutionPair(solutionPair.Item1, solutionPair.Item2, PerformCompareSolutionsAndShowUnique);
            }
        }

        private async void ExecuteActionOnSolutionPair(Solution solution1, Solution solution2, Func<string, Solution, Solution, Task> action)
        {
            if (!this.IsControlsEnabled)
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
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                try
                {
                    await action(folder, solution1, solution2);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        private async void ExecuteActionOnSolutionAndSolutionCollection(Solution[] solutions, Solution solution, Func<string, Solution[], Solution, Task> action)
        {
            if (!this.IsControlsEnabled)
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
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            try
            {
                await action(folder, solutions, solution);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task PerformCompareSolutions(string folder, Solution solution1, Solution solution2)
        {
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ComparingSolutionsFormat2, solution1.UniqueName, solution2.UniqueName);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Comparing Solution Components '{0}' and '{1}'.", solution1.UniqueName, solution2.UniqueName);
                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                await solutionDescriptor.FindUniqueComponentsInSolutionsAsync(solution1.Id, solution2.Id);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ComparingSolutionsCompletedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ComparingSolutionsFailedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
        }

        private async Task PerformCompareSolutionsAndShowUnique(string folder, Solution solution1, Solution solution2)
        {
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ComparingSolutionsFormat2, solution1.UniqueName, solution2.UniqueName);

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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Unique Solution Components '{0}' was export into file '{1}'", solution1.UniqueName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ComparingSolutionsCompletedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ComparingSolutionsFailedFormat2, solution1.UniqueName, solution2.UniqueName);
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

        private void mIRemoveComponentsOwnedSolution1FromSolution2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag is Tuple<Solution, Solution> solutionPair
               && solutionPair.Item1 != null
               && solutionPair.Item2 != null
               && !string.Equals(solutionPair.Item1.UniqueName, solutionPair.Item2.UniqueName, StringComparison.InvariantCultureIgnoreCase)
               && solutionPair.Item2.IsManaged.GetValueOrDefault() == false
               )
            {
                string question = string.Format(Properties.MessageBoxStrings.RemoveSolutionComponentsFromToFormat2, solutionPair.Item1.UniqueName, solutionPair.Item2.UniqueName);

                if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                {
                    return;
                }

                ExecuteActionOnSolutionPair(solutionPair.Item1, solutionPair.Item2, PerformRemoveFromSolution1ToSolution2);
            }
        }

        private void mIRemoveComponentsOwnedSolutionCollectionFromSolution_Click(object sender, RoutedEventArgs e)
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

                string question = string.Format(Properties.MessageBoxStrings.RemoveSolutionComponentsFromToFormat2, sourceName, solutionPair.Item2.UniqueName);

                if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                {
                    return;
                }

                ExecuteActionOnSolutionAndSolutionCollection(solutionPair.Item1, solutionPair.Item2, PerformRemoveFromSolutionCollectionToSolution);
            }
        }

        private async Task PerformCopyFromSolution1ToSolution2(string folder, Solution solutionSource, Solution solutionTarget)
        {
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CopingSolutionComponentsToFromFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);

                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                SolutionComponentRepository repository = new SolutionComponentRepository(service);

                var componentsSource = await repository.GetSolutionComponentsAsync(solutionSource.Id, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                var componentsTarget = await repository.GetSolutionComponentsAsync(solutionTarget.Id, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                var componentesOnlyInSource = SolutionDescriptor.GetComponentsInFirstNotSecond(componentsSource, componentsTarget);

                if (componentesOnlyInSource.Count > 0)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Creating backup Solution Components in '{0}'.", solutionTarget.UniqueName);

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , $"Components Backup before Adding from {solutionSource.UniqueName}"
                        );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solutionTarget.Id);

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Created backup Solution Components in '{0}': {1}", solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);
                    }

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , $"SolutionImage Components Backup before Adding from {solutionSource.UniqueName}"
                            , "xml"
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solutionTarget.Id, solutionTarget.UniqueName);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Showing Unique Solution Components in '{0}'.", solutionSource.UniqueName);

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionMultipleFileName(
                            service.ConnectionData.Name
                            , solutionSource.UniqueName
                            , solutionTarget.UniqueName
                            , $"Unique Components for Adding from {solutionSource.UniqueName}"
                        );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithUniqueComponentsInSolution1Async(filePath, solutionSource.Id, solutionTarget.Id);

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Created file with Unique Components in '{0}' for Adding to '{1}': {2}", solutionSource.UniqueName, solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);
                    }

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionMultipleFileName(
                            service.ConnectionData.Name
                            , solutionSource.UniqueName
                            , solutionTarget.UniqueName
                            , $"SolutionImage Unique Components for Adding from {solutionSource.UniqueName}"
                        );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateSolutionImageWithComponentsAsync(filePath, solutionSource.UniqueName, componentesOnlyInSource);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Coping Solution Components from '{0}' into '{1}'.", solutionSource.UniqueName, solutionTarget.UniqueName);

                    await Controllers.SolutionController.AddSolutionComponentsCollectionIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionTarget.UniqueName, componentesOnlyInSource, false);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Copied {0} components.", componentesOnlyInSource.Count.ToString());
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "All Solution Components '{0}' already added into '{1}'.", solutionSource.UniqueName, solutionTarget.UniqueName);
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CopingSolutionComponentsToFromCompletedFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CopingSolutionComponentsToFromFailedFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);
            }
        }

        private async Task PerformCopyFromSolutionCollectionToSolution(string folder, Solution[] solutionSourceCollection, Solution solutionTarget)
        {
            var service = await GetService();

            string sourceName = string.Join(",", solutionSourceCollection.Select(e => e.UniqueName).OrderBy(s => s));

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CopingSolutionComponentsToFromFormat2, solutionTarget.UniqueName, sourceName);

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
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Creating backup Solution Components in '{0}'.", solutionTarget.UniqueName);


                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , $"Components Backup before Adding from {sourceName}"
                            , "txt"
                        );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solutionTarget.Id);

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Created backup Solution Components in '{0}': {1}", solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);
                    }

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , $"SolutionImage Components Backup before Adding from {sourceName}"
                            , "xml"
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solutionTarget.Id, solutionTarget.UniqueName);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Showing Unique Solution Components in '{0}'.", sourceName);

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionMultipleFileName(
                            service.ConnectionData.Name
                            , sourceName
                            , solutionTarget.UniqueName
                            , $"Unique Components for Adding from {sourceName}"
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

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Created file with Unique Components in '{0}' for Adding to '{1}': {2}", sourceName, solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);
                    }

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , $"SolutionImage Unique Components for Adding from {sourceName}"
                            , "xml"
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateSolutionImageWithComponentsAsync(filePath, sourceName, componentesOnlyInSource);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Coping Solution Components from '{0}' into '{1}'.", sourceName, solutionTarget.UniqueName);

                    await Controllers.SolutionController.AddSolutionComponentsCollectionIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionTarget.UniqueName, componentesOnlyInSource, false);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Copied {0} components.", componentesOnlyInSource.Count.ToString());
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "All Solution Components '{0}' already added into '{1}'.", sourceName, solutionTarget.UniqueName);
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CopingSolutionComponentsToFromCompletedFormat2, solutionTarget.UniqueName, sourceName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CopingSolutionComponentsToFromFailedFormat2, solutionTarget.UniqueName, sourceName);
            }
        }

        private async Task PerformRemoveFromSolution1ToSolution2(string folder, Solution solutionSource, Solution solutionTarget)
        {
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.RemovingSolutionComponentsFromOwnedByFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);

                var descriptor = await GetDescriptor();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                SolutionComponentRepository repository = new SolutionComponentRepository(service);

                var componentsSource = await repository.GetSolutionComponentsAsync(solutionSource.Id, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                var componentsTarget = await repository.GetSolutionComponentsAsync(solutionTarget.Id, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

                var commonComponents = SolutionDescriptor.GetCommonComponents(componentsSource, componentsTarget);

                if (commonComponents.Count > 0)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Creating backup Solution Components in '{0}'.", solutionTarget.UniqueName);

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , $"Components Backup before removing {solutionSource.UniqueName}"
                        );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solutionTarget.Id);

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Created backup Solution Components in '{0}': {1}", solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);
                    }

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , $"SolutionImage Components Backup before removing {solutionSource.UniqueName}"
                            , "xml"
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solutionTarget.Id, solutionTarget.UniqueName);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Showing Unique Solution Components in '{0}'.", solutionSource.UniqueName);

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionMultipleFileName(
                            service.ConnectionData.Name
                            , solutionSource.UniqueName
                            , solutionTarget.UniqueName
                            , $"Unique Components for Removing from {solutionSource.UniqueName}"
                            );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithUniqueComponentsInSolution1Async(filePath, solutionSource.Id, solutionTarget.Id);

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Created file with Unique Components in '{0}' for Removing from '{1}': {2}", solutionSource.UniqueName, solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);
                    }

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionMultipleFileName(
                            service.ConnectionData.Name
                            , solutionSource.UniqueName
                            , solutionTarget.UniqueName
                            , $"SolutionImage Unique Components for Removing from {solutionSource.UniqueName}"
                            );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateSolutionImageWithComponentsAsync(filePath, solutionTarget.UniqueName, commonComponents);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Removing Solution Components owned by '{0}' from '{1}'.", solutionSource.UniqueName, solutionTarget.UniqueName);

                    await Controllers.SolutionController.RemoveSolutionComponentsCollectionFromSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionTarget.UniqueName, commonComponents, false);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Removed {0} components.", commonComponents.Count.ToString());
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "There are No Common Solution Components in '{0}' and '{1}'.", solutionSource.UniqueName, solutionTarget.UniqueName);
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.RemovingSolutionComponentsFromOwnedByCompletedFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.RemovingSolutionComponentsFromOwnedByFailedFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);
            }
        }

        private async Task PerformRemoveFromSolutionCollectionToSolution(string folder, Solution[] solutionSourceCollection, Solution solutionTarget)
        {
            string sourceName = string.Join(",", solutionSourceCollection.Select(e => e.UniqueName).OrderBy(s => s));

            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.RemovingSolutionComponentsFromOwnedByFormat2, solutionTarget.UniqueName, sourceName);

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
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Creating backup Solution Components in '{0}'.", solutionTarget.UniqueName);


                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , $"Components Backup before removing {sourceName}"
                            , "txt"
                        );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solutionTarget.Id);

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Created backup Solution Components in '{0}': {1}", solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);
                    }

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionFileName(
                            service.ConnectionData.Name
                            , solutionTarget.UniqueName
                            , $"SolutionImage Components Backup before removing {sourceName}"
                            , "xml"
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solutionTarget.Id, solutionTarget.UniqueName);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Showing Unique Solution Components in '{0}'.", sourceName);

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionMultipleFileName(
                            service.ConnectionData.Name
                            , sourceName
                            , solutionTarget.UniqueName
                            , $"SolutionImage Unique Components for Removing from {sourceName}"
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

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Created file with Unique Components in '{0}' for Removing from '{1}': {2}", sourceName, solutionTarget.UniqueName, filePath);
                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);
                    }

                    {
                        string fileName = EntityFileNameFormatter.GetSolutionMultipleFileName(
                            service.ConnectionData.Name
                            , sourceName
                            , solutionTarget.UniqueName
                            , $"SolutionImage Unique Components for Removing from {sourceName}"
                            );

                        string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                        await solutionDescriptor.CreateSolutionImageWithComponentsAsync(filePath, solutionTarget.UniqueName, componentesOnlyInSource);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Removing Solution Components owned by '{0}' from '{1}'.", sourceName, solutionTarget.UniqueName);

                    await Controllers.SolutionController.RemoveSolutionComponentsCollectionFromSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionTarget.UniqueName, componentesOnlyInSource, false);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Removed {0} components.", componentesOnlyInSource.Count.ToString());
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "There are No Common Solution Components in '{0}' and '{1}'.", sourceName, solutionTarget.UniqueName);
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.RemovingSolutionComponentsFromOwnedByCompletedFormat2, solutionTarget.UniqueName, sourceName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.RemovingSolutionComponentsFromOwnedByFailedFormat2, solutionTarget.UniqueName, sourceName);
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

        private async void mIOpenDefaultSolutionInWeb_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            service.ConnectionData.OpenSolutionInWeb(Solution.Schema.InstancesUniqueId.DefaultId);
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
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ClearingSolutionFormat2, service.ConnectionData.Name, solution.UniqueName);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Creating backup Solution Components in '{0}'.", solution.UniqueName);

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        service.ConnectionData.Name
                        , solution.UniqueName
                        , "Components Backup"
                        , "txt"
                    );

                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solution.Id);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Created backup Solution Components in '{0}': {1}", solution.UniqueName, filePath);
                    this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);
                }

                {

                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        service.ConnectionData.Name
                        , solution.UniqueName
                        , "SolutionImage Backup before Clearing"
                        , "xml"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solution.Id, solution.UniqueName);
                }

                SolutionComponentRepository repository = new SolutionComponentRepository(service);
                await repository.ClearSolutionAsync(solution.UniqueName);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ClearingSolutionCompletedFormat2, service.ConnectionData.Name, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ClearingSolutionFailedFormat2, service.ConnectionData.Name, solution.UniqueName);
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
                    if (_optionsSolutionPopup.IsOpen)
                    {
                        _optionsSolutionPopup.IsOpen = false;
                        e.Handled = true;
                    }

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

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                )
            {
                ExecuteActionOnSingleSolution(solution, PerformCreateEntityDescription);
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

        private async Task PerformCreateEntityDescription(string folder, Solution solution)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingEntityDescription);

            try
            {
                SolutionRepository repository = new SolutionRepository(service);

                var solutionFull = await repository.GetSolutionByIdAsync(solution.Id);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , EntityFileNameFormatter.Headers.EntityDescription
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, solutionFull, EntityFileNameFormatter.WebResourceIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData
                    , Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , solutionFull.LogicalName
                    , filePath
                );

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingEntityDescriptionFailed);
            }
        }

        private async Task PerformCreateFileWithUsedEntitiesInWorkflows(string folder, Solution solution)
        {
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileWithUsedEntitiesInWorkflowsFormat1, solution.UniqueName);

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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution Used Entities was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileWithUsedEntitiesInWorkflowsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileWithUsedEntitiesInWorkflowsFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformCreateFileWithUsedNotExistsEntitiesInWorkflows(string folder, Solution solution)
        {
            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsFormat1, solution.UniqueName);

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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution Used Not Exists Entities was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsFailedFormat1, solution.UniqueName);
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
                removed.Save();

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
                FillChangeComponentsToLastSelectedSolutionsAsync();

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
                    ExecuteActionOnSingleSolutionWithoutFolderCheck(item.Solution, PerformOpenSolutionComponentsInWindow);
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

        private void mIOpenSolutionDifferenceImage_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                _commonConfig.Save();

                WindowHelper.OpenSolutionDifferenceImageWindow(this._iWriteToOutput, connectionData, _commonConfig);
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
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string selectedPath = string.Empty;

            try
            {
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
                        DTEHelper.WriteExceptionToOutput(null, ex);
                    }
                });

                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (string.IsNullOrEmpty(selectedPath) || !File.Exists(selectedPath))
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }

            var service = await GetService();

            try
            {
                ToggleControls(service.ConnectionData, false, _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.WindowStatusStrings.CreatingSolutionImageFromZipFile));

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

                await solutionDescriptor.CreateSolutionImageWithComponentsAsync(filePath, Path.GetFileNameWithoutExtension(selectedPath), components);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedSolutionImageForConnectionFormat2, service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingSolutionImageFromZipFileCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingSolutionImageFromZipFileFailed);
            }
        }

        private void miExportSolutionOptions_Click(object sender, RoutedEventArgs e)
        {
            this._optionsSolutionPopup.IsOpen = true;
            this._optionsSolutionPopup.Child.Focus();
        }

        private void miDescriptionOptions_Click(object sender, RoutedEventArgs e)
        {
            this._optionsPopup.IsOpen = true;
            this._optionsPopup.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_optionsSolutionPopup.IsOpen)
            {
                _optionsSolutionPopup.IsOpen = false;
            }

            if (_optionsPopup.IsOpen)
            {
                _optionsPopup.IsOpen = false;
            }

            this.Focus();
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void btnCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void miEntitySecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void miSecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
            );
        }

        private async void btnSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty, string.Empty);
        }

        private async void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnSiteMap_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenExportSiteMapWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnWebResources_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenWebResourceExplorerWindow(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void btnExportReport_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenReportExplorerWindow(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void btnPluginAssembly_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenPluginAssemblyWindow(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnPluginType_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenPluginTypeWindow(this._iWriteToOutput, service, _commonConfig, null);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }

        private void btnNewProfile_Click(object sender, RoutedEventArgs e)
        {
            if (cmBCurrentConnection.SelectedItem != null
                && cmBCurrentConnection.SelectedItem is ConnectionData connectionData
            )
            {
                var dialog = new WindowSelectPrefix("Enter New Profile Name", "New Profile Name");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData.ExportSolutionProfileList.Add(new ExportSolutionProfile()
                    {
                        Name = dialog.Prefix,
                    });
                }
            }
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            if (cmBExportSolutionProfile.SelectedItem != null
                && cmBExportSolutionProfile.SelectedItem is ExportSolutionProfile exportSolutionProfile
                && cmBCurrentConnection.SelectedItem != null
                && cmBCurrentConnection.SelectedItem is ConnectionData connectionData
            )
            {
                var dialog = new WindowSelectPrefix("Enter Profile Name", "Profile Name")
                {
                    Prefix = exportSolutionProfile.Name,
                };

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    exportSolutionProfile.Name = dialog.Prefix;
                }
            }
        }

        private void btnDeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            if (cmBExportSolutionProfile.Items.Count > 1)
            {
                if (cmBExportSolutionProfile.SelectedItem != null
                    && cmBExportSolutionProfile.SelectedItem is ExportSolutionProfile exportSolutionProfile
                    && cmBCurrentConnection.SelectedItem != null
                    && cmBCurrentConnection.SelectedItem is ConnectionData connectionData
                )
                {
                    connectionData.ExportSolutionProfileList.Remove(exportSolutionProfile);
                }
            }
        }
    }
}