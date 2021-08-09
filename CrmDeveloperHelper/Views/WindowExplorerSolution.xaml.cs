using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class WindowExplorerSolution : WindowWithSolutionComponentDescriptor
    {
        private readonly EnvDTE.SelectedItem _selectedItem;

        private Guid? _objectId;
        private int? _componentType;

        private readonly Popup _optionsPopup;

        private readonly Popup _optionsSolutionPopup;
        private readonly ExportSolutionOptionsControl _optionsExportSolutionOptionsControl;

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        public WindowExplorerSolution(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , int? componentType
            , Guid? objectId
            , EnvDTE.SelectedItem selectedItem
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            this._selectedItem = selectedItem;
            this._objectId = objectId;
            this._componentType = componentType;

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
                var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.SolutionComponentXmlOptions);
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

            FillExplorersMenuItems();

            this.DecreaseInit();

            FocusOnComboBoxTextBox(cmBFilter);

            var task = ShowExistingSolutions();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService, _selectedItem);

            explorersHelper.FillExplorers(miExplorers);
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
            base.OnClosed(e);

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
        }

        private ConnectionData GetSelectedConnection()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private Task<IOrganizationServiceExtented> GetService()
        {
            var connectionData = GetSelectedConnection();

            return GetOrganizationService(connectionData);
        }

        private async Task ShowExistingSolutions()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingSolutions);

            string textName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();

                textName = cmBFilter.Text?.Trim().ToLower();
            });

            IEnumerable<Solution> list = Enumerable.Empty<Solution>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var repository = new SolutionRepository(service);

                    list = await repository.GetSolutionsAllAsync(textName, _componentType, _objectId);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            LoadSolutions(list);

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingSolutionsCompletedFormat1, list.Count());
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

            public string Version => Solution.Version;

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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
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

                                    var menuItemSolutuion1 = new MenuItem()
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

                                        var menuItemSolutuion2 = new MenuItem()
                                        {
                                            Header = string.Format("Solution {0}", solution2.UniqueNameEscapeUnderscore),
                                        };

                                        menuItemSolutuion1.Items.Add(menuItemSolutuion2);

                                        var mICompareSolutions = new MenuItem()
                                        {
                                            Header = string.Format("Compare Solutions {0} and {1}", solution1.UniqueNameEscapeUnderscore, solution2.UniqueNameEscapeUnderscore),
                                            Tag = Tuple.Create(solution1, solution2),
                                        };
                                        mICompareSolutions.Click += mICompareSolutions_Click;

                                        var mIShowUniqueComponentsIn1 = new MenuItem()
                                        {
                                            Header = string.Format("Show Unique Components in {0} compare to {1}", solution1.UniqueNameEscapeUnderscore, solution2.UniqueNameEscapeUnderscore),
                                            Tag = Tuple.Create(solution1, solution2),
                                        };
                                        mIShowUniqueComponentsIn1.Click += mIShowUniqueComponentsInSolution_Click;

                                        var mIShowUniqueComponentsIn2 = new MenuItem()
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

                                        var menuItem = new MenuItem()
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

                                    var menuItem = new MenuItem()
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

                    var task = FillChangeComponentsToLastSelectedSolutionsAsync();
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
                var connectionData = GetSelectedConnection();

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

                        if (service != null)
                        {
                            var repository = new SolutionRepository(service);

                            var solutionsTask = await repository.GetSolutionsVisibleUnmanagedAsync(listSolutionNames);

                            var lastSolutions = solutionsTask.ToDictionary(s => s.UniqueName, StringComparer.InvariantCultureIgnoreCase);

                            var listTo = listSolutionNames.Where(s => lastSolutions.ContainsKey(s)).Select(s => lastSolutions[s]).ToList();

                            FillCopyComponentsMenuItems(tSDDBCopyComponentsLastSolution, listFrom, listTo);

                            FillRemoveComponentsMenuItems(tSDDBRemoveComponentsLastSolution, listFrom, listTo);
                        }
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

                var menuItemSolution = new MenuItem()
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
                    var menuItem = new MenuItem()
                    {
                        Header = string.Format("Copy Components from All Selected Solutions to {0}", solutionTo.UniqueNameEscapeUnderscore),
                        Tag = Tuple.Create(listFromFiltered.ToArray(), solutionTo),
                    };
                    menuItem.Click += mICopyComponentsFromSolutionCollectionToSolution_Click;

                    menuItemSolution.Items.Add(menuItem);
                }

                foreach (var solutionFrom in listFromFiltered)
                {
                    var menuItem = new MenuItem()
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

                var menuItemSolution = new MenuItem()
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
                    var menuItem = new MenuItem()
                    {
                        Header = string.Format("Remove Components owned by All Selected Solutions from {0}", solutionTo.UniqueNameEscapeUnderscore),
                        Tag = Tuple.Create(listFromFiltered.ToArray(), solutionTo),
                    };
                    menuItem.Click += mIRemoveComponentsOwnedSolutionCollectionFromSolution_Click;

                    menuItemSolution.Items.Add(menuItem);
                }

                foreach (var solutionFrom in listFromFiltered)
                {
                    var menuItem = new MenuItem()
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
            {
                var mIOpenComponentsInExplorer = new MenuItem()
                {
                    Header = string.Format("Open Solution Components in Explorer for {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                    FontWeight = FontWeights.Bold,
                };
                mIOpenComponentsInExplorer.Click += mIOpenComponentsInExplorer_Click;

                var mIOpenSolutionInWeb = new MenuItem()
                {
                    Header = string.Format("Open in Browser Solution {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mIOpenSolutionInWeb.Click += mIOpenSolutionInWeb_Click;

                var mICreateEntityDescription = new MenuItem()
                {
                    Header = "Create Entity Description",
                    Tag = solution,
                };
                mICreateEntityDescription.Click += mICreateEntityDescription_Click;

                itemCollection.Add(mIOpenComponentsInExplorer);
                itemCollection.Add(new Separator());
                itemCollection.Add(mIOpenSolutionInWeb);
                itemCollection.Add(new Separator());
                itemCollection.Add(mICreateEntityDescription);
            }

            {


                var mICopyUniqueName = new MenuItem()
                {
                    Header = "Copy to Clipboard Unique Name",
                    Tag = solution,
                };
                mICopyUniqueName.Click += mICopyUniqueName_Click;

                var mICopyFriendlyName = new MenuItem()
                {
                    Header = "Copy to Clipboard Friendly Name",
                    Tag = solution,
                };
                mICopyFriendlyName.Click += mICopyFriendlyName_Click;

                var mICopyVersion = new MenuItem()
                {
                    Header = "Copy to Clipboard Version",
                    Tag = solution,
                };
                mICopyVersion.Click += mICopyVersion_Click;

                var mICopyPublisherName = new MenuItem()
                {
                    Header = "Copy to Clipboard Publisher Name",
                    Tag = solution,
                };
                mICopyPublisherName.Click += mICopyPublisherName_Click;

                var mICopyPrefix = new MenuItem()
                {
                    Header = "Copy to Clipboard Prefix",
                    Tag = solution,
                };
                mICopyPrefix.Click += mICopyPrefix_Click;

                var mIClipboard = new MenuItem()
                {
                    Header = "Clipboard",

                    Items =
                    {
                        mICopyUniqueName,
                        mICopyFriendlyName,

                        new Separator(),
                        mICopyVersion,
                        mICopyPublisherName,
                        mICopyPrefix,
                    }
                };

                if (!string.IsNullOrEmpty(solution.Description))
                {
                    var mICopyDescription = new MenuItem()
                    {
                        Header = "Copy to Clipboard Description",
                        Tag = solution,
                    };
                    mICopyDescription.Click += mICopyDescription_Click;

                    mIClipboard.Items.Add(new Separator());
                    mIClipboard.Items.Add(mICopyDescription);
                }

                var mICopySolutionId = new MenuItem()
                {
                    Header = "Copy to Clipboard Solution Id",
                    Tag = solution,
                };
                mICopySolutionId.Click += mICopySolutionId_Click;

                mIClipboard.Items.Add(new Separator());
                mIClipboard.Items.Add(mICopySolutionId);

                itemCollection.Add(new Separator());
                itemCollection.Add(mIClipboard);
            }

            if (!string.IsNullOrEmpty(solution.Description))
            {
                var mIOpenSolutionDescription = new MenuItem()
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
                var mISelectSolutionAsLast = new MenuItem()
                {
                    Header = string.Format("Select Solution {0} as Last Selected", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mISelectSolutionAsLast.Click += miSelectSolutionAsLast_Click;


                var mIChangeSolutionInEditor = new MenuItem()
                {
                    Header = string.Format("Change Solution {0} in Editor", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mIChangeSolutionInEditor.Click += mIChangeSolutionInEditor_Click;

                itemCollection.Add(new Separator());
                itemCollection.Add(mISelectSolutionAsLast);

                itemCollection.Add(new Separator());
                itemCollection.Add(mIChangeSolutionInEditor);
            }

            {
                var mIUsedEntitiesInWorkflows = new MenuItem()
                {
                    Header = string.Format("Used Entities in Workflows in {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mIUsedEntitiesInWorkflows.Click += mIUsedEntitiesInWorkflows_Click;

                var mIUsedNotExistsEntitiesInWorkflows = new MenuItem()
                {
                    Header = string.Format("Used Not Exists Entities in Workflows in {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mIUsedNotExistsEntitiesInWorkflows.Click += mIUsedNotExistsEntitiesInWorkflows_Click;

                var mICreateSolutionImage = new MenuItem()
                {
                    Header = string.Format("Create Solution Image for {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mICreateSolutionImage.Click += mICreateSolutionImage_Click;

                var mICreateSolutionImageAndOpenOrganizationComparer = new MenuItem()
                {
                    Header = string.Format("Create Solution Image for {0} and Open Organization Comparer Window", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mICreateSolutionImageAndOpenOrganizationComparer.Click += mICreateSolutionImageAndOpenOrganizationComparer_Click;

                itemCollection.Add(new Separator());
                itemCollection.Add(mIUsedEntitiesInWorkflows);
                itemCollection.Add(mIUsedNotExistsEntitiesInWorkflows);
                itemCollection.Add(new Separator());
                itemCollection.Add(mICreateSolutionImage);
                itemCollection.Add(mICreateSolutionImageAndOpenOrganizationComparer);
            }

            if (solution.IsManaged.GetValueOrDefault() == false)
            {
                var mIExportSolution = new MenuItem()
                {
                    Header = string.Format("Export Solution {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mIExportSolution.Click += mIExportSolution_Click;

                itemCollection.Add(new Separator());
                itemCollection.Add(mIExportSolution);

                var connectionData = GetSelectedConnection();

                if (connectionData != null)
                {
                    var otherConnections = connectionData.ConnectionConfiguration.Connections.Where(c => c.ConnectionId != connectionData.ConnectionId).ToList();

                    if (otherConnections.Any())
                    {
                        var mICheckImportPossibility = new MenuItem()
                        {
                            Header = string.Format("Check Possibility to Import {0} into", solution.UniqueNameEscapeUnderscore),
                            Tag = solution,
                        };

                        foreach (var connection in otherConnections)
                        {
                            var menuItem = new MenuItem()
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

                        var mICheckImportPossibilityAndExportSolution = new MenuItem()
                        {
                            Header = string.Format("Check Possibility to Import {0} into Connection and Export Solution", solution.UniqueNameEscapeUnderscore),
                            Tag = solution,
                        };

                        foreach (var connection in otherConnections)
                        {
                            var menuItem = new MenuItem()
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

            {
                var mIOpenWebResourcesAll = new MenuItem()
                {
                    Header = "All",
                    Tag = Tuple.Create(solution, OpenFilesType.All),
                };
                mIOpenWebResourcesAll.Click += mIOpenWebResources_Click;

                var mIOpenWebResourcesNotEqualByText = new MenuItem()
                {
                    Header = "Not Equal By Text",
                    Tag = Tuple.Create(solution, OpenFilesType.NotEqualByText),
                };
                mIOpenWebResourcesNotEqualByText.Click += mIOpenWebResources_Click;

                var mIOpenWebResourcesEqualByText = new MenuItem()
                {
                    Header = "Equal By Text",
                    Tag = Tuple.Create(solution, OpenFilesType.EqualByText),
                };
                mIOpenWebResourcesEqualByText.Click += mIOpenWebResources_Click;

                var mIOpenWebResourcesWithInserts = new MenuItem()
                {
                    Header = "With Inserts",
                    Tag = Tuple.Create(solution, OpenFilesType.WithInserts),
                };
                mIOpenWebResourcesWithInserts.Click += mIOpenWebResources_Click;

                var mIOpenWebResourcesWithDeletes = new MenuItem()
                {
                    Header = "With Deletes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithDeletes),
                };
                mIOpenWebResourcesWithDeletes.Click += mIOpenWebResources_Click;

                var mIOpenWebResourcesWithComplexChanges = new MenuItem()
                {
                    Header = "With Complex Changes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithComplexChanges),
                };
                mIOpenWebResourcesWithComplexChanges.Click += mIOpenWebResources_Click;

                var mIOpenWebResourcesWithMirrorChanges = new MenuItem()
                {
                    Header = "With Mirror Changes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorChanges),
                };
                mIOpenWebResourcesWithMirrorChanges.Click += mIOpenWebResources_Click;

                var mIOpenWebResourcesWithMirrorInserts = new MenuItem()
                {
                    Header = "With Mirror Inserts",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorInserts),
                };
                mIOpenWebResourcesWithMirrorInserts.Click += mIOpenWebResources_Click;

                var mIOpenWebResourcesWithMirrorDeletes = new MenuItem()
                {
                    Header = "With Mirror Deletes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorDeletes),
                };
                mIOpenWebResourcesWithMirrorDeletes.Click += mIOpenWebResources_Click;

                var mIOpenWebResourcesWithMirrorComplexChanges = new MenuItem()
                {
                    Header = "With Mirror Complex Changes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorComplexChanges),
                };
                mIOpenWebResourcesWithMirrorComplexChanges.Click += mIOpenWebResources_Click;

                var mIOpenWebResourcesInTextEditorAll = new MenuItem()
                {
                    Header = "All",
                    Tag = Tuple.Create(solution, OpenFilesType.All),
                };
                mIOpenWebResourcesInTextEditorAll.Click += mIOpenWebResourcesInTextEditor_Click;

                var mIOpenWebResourcesInTextEditorNotEqualByText = new MenuItem()
                {
                    Header = "Not Equal By Text",
                    Tag = Tuple.Create(solution, OpenFilesType.NotEqualByText),
                };
                mIOpenWebResourcesInTextEditorNotEqualByText.Click += mIOpenWebResourcesInTextEditor_Click;

                var mIOpenWebResourcesInTextEditorEqualByText = new MenuItem()
                {
                    Header = "Equal By Text",
                    Tag = Tuple.Create(solution, OpenFilesType.EqualByText),
                };
                mIOpenWebResourcesInTextEditorEqualByText.Click += mIOpenWebResourcesInTextEditor_Click;

                var mIOpenWebResourcesInTextEditorWithInserts = new MenuItem()
                {
                    Header = "With Inserts",
                    Tag = Tuple.Create(solution, OpenFilesType.WithInserts),
                };
                mIOpenWebResourcesInTextEditorWithInserts.Click += mIOpenWebResourcesInTextEditor_Click;

                var mIOpenWebResourcesInTextEditorWithDeletes = new MenuItem()
                {
                    Header = "With Deletes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithDeletes),
                };
                mIOpenWebResourcesInTextEditorWithDeletes.Click += mIOpenWebResourcesInTextEditor_Click;

                var mIOpenWebResourcesInTextEditorWithComplexChanges = new MenuItem()
                {
                    Header = "With Complex Changes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithComplexChanges),
                };
                mIOpenWebResourcesInTextEditorWithComplexChanges.Click += mIOpenWebResourcesInTextEditor_Click;

                var mIOpenWebResourcesInTextEditorWithMirrorChanges = new MenuItem()
                {
                    Header = "With Mirror Changes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorChanges),
                };
                mIOpenWebResourcesInTextEditorWithMirrorChanges.Click += mIOpenWebResourcesInTextEditor_Click;

                var mIOpenWebResourcesInTextEditorWithMirrorInserts = new MenuItem()
                {
                    Header = "With Mirror Inserts",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorInserts),
                };
                mIOpenWebResourcesInTextEditorWithMirrorInserts.Click += mIOpenWebResourcesInTextEditor_Click;

                var mIOpenWebResourcesInTextEditorWithMirrorDeletes = new MenuItem()
                {
                    Header = "With Mirror Deletes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorDeletes),
                };
                mIOpenWebResourcesInTextEditorWithMirrorDeletes.Click += mIOpenWebResourcesInTextEditor_Click;

                var mIOpenWebResourcesInTextEditorWithMirrorComplexChanges = new MenuItem()
                {
                    Header = "With Mirror Complex Changes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorComplexChanges),
                };
                mIOpenWebResourcesInTextEditorWithMirrorComplexChanges.Click += mIOpenWebResourcesInTextEditor_Click;

                var mICompareWebResources = new MenuItem()
                {
                    Header = string.Format("Compare WebResources in {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mICompareWebResources.Click += mICompareWebResources_Click;

                var mICompareWithDetailsWebResources = new MenuItem()
                {
                    Header = string.Format("Compare with details WebResources in {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mICompareWithDetailsWebResources.Click += mICompareWithDetailsWebResources_Click;

                var mIShowDifferenceWebResourcesNotEqualByText = new MenuItem()
                {
                    Header = "Not Equal By Text",
                    Tag = Tuple.Create(solution, OpenFilesType.NotEqualByText),
                };
                mIShowDifferenceWebResourcesNotEqualByText.Click += mIShowDifferenceWebResources_Click;

                var mIShowDifferenceWebResourcesEqualByText = new MenuItem()
                {
                    Header = "Equal By Text",
                    Tag = Tuple.Create(solution, OpenFilesType.EqualByText),
                };
                mIShowDifferenceWebResourcesEqualByText.Click += mIShowDifferenceWebResources_Click;

                var mIShowDifferenceWebResourcesWithInserts = new MenuItem()
                {
                    Header = "With Inserts",
                    Tag = Tuple.Create(solution, OpenFilesType.WithInserts),
                };
                mIShowDifferenceWebResourcesWithInserts.Click += mIShowDifferenceWebResources_Click;

                var mIShowDifferenceWebResourcesWithDeletes = new MenuItem()
                {
                    Header = "With Deletes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithDeletes),
                };
                mIShowDifferenceWebResourcesWithDeletes.Click += mIShowDifferenceWebResources_Click;

                var mIShowDifferenceWebResourcesWithComplexChanges = new MenuItem()
                {
                    Header = "With Complex Changes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithComplexChanges),
                };
                mIShowDifferenceWebResourcesWithComplexChanges.Click += mIShowDifferenceWebResources_Click;

                var mIShowDifferenceWebResourcesWithMirrorChanges = new MenuItem()
                {
                    Header = "With Mirror Changes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorChanges),
                };
                mIShowDifferenceWebResourcesWithMirrorChanges.Click += mIShowDifferenceWebResources_Click;

                var mIShowDifferenceWebResourcesWithMirrorInserts = new MenuItem()
                {
                    Header = "With Mirror Inserts",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorInserts),
                };
                mIShowDifferenceWebResourcesWithMirrorInserts.Click += mIShowDifferenceWebResources_Click;

                var mIShowDifferenceWebResourcesWithMirrorDeletes = new MenuItem()
                {
                    Header = "With Mirror Deletes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorDeletes),
                };
                mIShowDifferenceWebResourcesWithMirrorDeletes.Click += mIShowDifferenceWebResources_Click;

                var mIShowDifferenceWebResourcesWithMirrorComplexChanges = new MenuItem()
                {
                    Header = "With Mirror Complex Changes",
                    Tag = Tuple.Create(solution, OpenFilesType.WithMirrorComplexChanges),
                };
                mIShowDifferenceWebResourcesWithMirrorComplexChanges.Click += mIShowDifferenceWebResources_Click;

                var mIOpenWebResources = new MenuItem()
                {
                    Header = string.Format("Open WebResources in {0}", solution.UniqueNameEscapeUnderscore),

                    Items =
                    {
                        mIOpenWebResourcesAll,
                        mIOpenWebResourcesNotEqualByText,
                        mIOpenWebResourcesEqualByText,

                        new Separator(),
                        mIOpenWebResourcesWithInserts,
                        mIOpenWebResourcesWithDeletes,
                        mIOpenWebResourcesWithComplexChanges,

                        new Separator(),
                        mIOpenWebResourcesWithMirrorChanges,
                        mIOpenWebResourcesWithMirrorInserts,
                        mIOpenWebResourcesWithMirrorDeletes,
                        mIOpenWebResourcesWithMirrorComplexChanges
                    },
                };

                var mIOpenWebResourcesInTextEditor = new MenuItem()
                {
                    Header = string.Format("Open WebResources in TextEditor in {0}", solution.UniqueNameEscapeUnderscore),

                    Items =
                    {
                        mIOpenWebResourcesInTextEditorAll,
                        mIOpenWebResourcesInTextEditorNotEqualByText,
                        mIOpenWebResourcesInTextEditorEqualByText,

                        new Separator(),
                        mIOpenWebResourcesInTextEditorWithInserts,
                        mIOpenWebResourcesInTextEditorWithDeletes,
                        mIOpenWebResourcesInTextEditorWithComplexChanges,

                        new Separator(),
                        mIOpenWebResourcesInTextEditorWithMirrorChanges,
                        mIOpenWebResourcesInTextEditorWithMirrorInserts,
                        mIOpenWebResourcesInTextEditorWithMirrorDeletes,
                        mIOpenWebResourcesInTextEditorWithMirrorComplexChanges
                    },
                };

                var mIShowDifferenceWebResources = new MenuItem()
                {
                    Header = string.Format("Show Difference WebResources in {0}", solution.UniqueNameEscapeUnderscore),

                    Items =
                    {
                        mIShowDifferenceWebResourcesNotEqualByText,
                        mIShowDifferenceWebResourcesEqualByText,

                        new Separator(),
                        mIShowDifferenceWebResourcesWithInserts,
                        mIShowDifferenceWebResourcesWithDeletes,
                        mIShowDifferenceWebResourcesWithComplexChanges,

                        new Separator(),
                        mIShowDifferenceWebResourcesWithMirrorChanges,
                        mIShowDifferenceWebResourcesWithMirrorInserts,
                        mIShowDifferenceWebResourcesWithMirrorDeletes,
                        mIShowDifferenceWebResourcesWithMirrorComplexChanges
                    },
                };

                var mIWebResources = new MenuItem()
                {
                    Header = string.Format("WebResources in {0}", solution.UniqueNameEscapeUnderscore),

                    Items =
                    {
                        mICompareWebResources,
                        mICompareWithDetailsWebResources,

                        new Separator(),
                        mIShowDifferenceWebResources
                    },
                };

                itemCollection.Add(new Separator());
                itemCollection.Add(mIOpenWebResources);
                itemCollection.Add(mIOpenWebResourcesInTextEditor);
                itemCollection.Add(mIWebResources);
            }

            {

                var mIComponents = new MenuItem()
                {
                    Header = string.Format("Components in {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mIComponents.Click += mIComponents_Click;

                var mIMissingComponents = new MenuItem()
                {
                    Header = string.Format("Missing Components for {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mIMissingComponents.Click += mIMissingComponents_Click;

                var mIUninstallComponents = new MenuItem()
                {
                    Header = string.Format("Uninstall Components for {0}", solution.UniqueNameEscapeUnderscore),
                    Tag = solution,
                };
                mIUninstallComponents.Click += mIUninstallComponents_Click;

                itemCollection.Add(new Separator());
                itemCollection.Add(mIComponents);
                itemCollection.Add(new Separator());
                itemCollection.Add(mIMissingComponents);
                itemCollection.Add(mIUninstallComponents);
            }
        }

        #region Showing Difference WebResources

        private async void mIShowDifferenceWebResources_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Tuple<Solution, OpenFilesType> tupleSolutionType
            )
            {
                await PerformShowingDifferenceSolutionWebResourcesAsync(tupleSolutionType.Item1, tupleSolutionType.Item2);
            }
        }

        private async Task PerformShowingDifferenceSolutionWebResourcesAsync(Solution solution, OpenFilesType openFilesType)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ComparingSolutionWebResourcesFormat1, solution.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                await SolutionController.ShowDifferenceSolutionWebResources(
                    _iWriteToOutput
                    , service
                    , descriptor
                    , _commonConfig
                    , solution.Id
                    , solution.UniqueName
                    , openFilesType
                );

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingSolutionWebResourcesCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingSolutionWebResourcesFailedFormat1, solution.UniqueName);
            }
        }

        #endregion Showing Difference WebResources

        #region Open WebResources

        private async void mIOpenWebResources_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Tuple<Solution, OpenFilesType> solutionWithType
            )
            {
                await PerformOpeningSolutionWebResourcesAsync(solutionWithType.Item1, false, solutionWithType.Item2);
            }
        }

        private async void mIOpenWebResourcesInTextEditor_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Tuple<Solution, OpenFilesType> solutionWithType
            )
            {
                await PerformOpeningSolutionWebResourcesAsync(solutionWithType.Item1, true, solutionWithType.Item2);
            }
        }

        private async Task PerformOpeningSolutionWebResourcesAsync(Solution solution, bool openInTextEditor, OpenFilesType openFilesType)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.OpeningSolutionWebResourcesFormat1, solution.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                await SolutionController.OpenSolutionWebResources(
                    _iWriteToOutput
                    , service
                    , descriptor
                    , _commonConfig
                    , solution.Id
                    , solution.UniqueName
                    , openInTextEditor
                    , openFilesType
                );

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.OpeningSolutionWebResourcesCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.OpeningSolutionWebResourcesFailedFormat1, solution.UniqueName);
            }
        }

        #endregion Open WebResources

        #region Compare WebResources

        private async void mICompareWebResources_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                await PerformComparingSolutionWebResourcesAsync(solution, false);
            }
        }

        private async void mICompareWithDetailsWebResources_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                await PerformComparingSolutionWebResourcesAsync(solution, true);
            }
        }

        private async Task PerformComparingSolutionWebResourcesAsync(Solution solution, bool withDetails)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ComparingSolutionWebResourcesFormat1, solution.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                await SolutionController.CompareSolutionWebResources(
                    _iWriteToOutput
                    , service
                    , descriptor
                    , _commonConfig
                    , solution.Id
                    , solution.UniqueName
                    , withDetails
                );

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingSolutionWebResourcesCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingSolutionWebResourcesFailedFormat1, solution.UniqueName);
            }
        }

        #endregion Compare WebResources

        #region Clipboard

        private void mICopyUniqueName_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                ClipboardHelper.SetText(solution.UniqueName);
            }
        }

        private void mICopyFriendlyName_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                ClipboardHelper.SetText(solution.FriendlyName);
            }
        }

        private void mICopyVersion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                ClipboardHelper.SetText(solution.Version);
            }
        }

        private void mICopyPublisherName_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                ClipboardHelper.SetText(solution.PublisherId?.Name);
            }
        }

        private void mICopyPrefix_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                ClipboardHelper.SetText(solution.PublisherCustomizationPrefix);
            }
        }

        private void mICopyDescription_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                ClipboardHelper.SetText(solution.Description);
            }
        }

        private void mICopySolutionId_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                ClipboardHelper.SetText(solution.Id.ToString());
            }
        }

        #endregion Clipboard

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

            if (service == null)
            {
                return false;
            }

            var descriptor = GetSolutionComponentDescriptor(service);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.GettingAllRequiredComponentsFormat1, solution.UniqueName);

            var repository = new DependencyRepository(service);

            var fullMissingComponents = await repository.GetSolutionAllRequiredComponentsAsync(solution.Id, solution.UniqueName);

            await descriptor.GetSolutionImageComponentsListAsync(fullMissingComponents);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.GettingAllRequiredComponentsCompletedFormat1, solution.UniqueName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.RemovingComponentsExistingInTargetFormat1, targetConnectionData.Name);

            var targetService = await GetOrganizationService(targetConnectionData);

            var targetDescription = GetSolutionComponentDescriptor(targetService);

            var existingComponentsInTarget = new List<SolutionComponent>();

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

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.RemovingComponentsExistingInTargetCompletedFormat1, targetConnectionData.Name);

            if (!fullMissingComponents.Any())
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.AllRequiredComponentsExistsInTargetFormat1, targetConnectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                return true;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerComponentsExplorer(_iWriteToOutput, service, descriptor, _commonConfig, fullMissingComponents, solution.UniqueName, "AllMissingDependencies", null);

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
            ConnectionData connectionData = GetSelectedConnection();
            ExportSolutionProfile exportSolutionProfile = null;

            this.Dispatcher.Invoke(() =>
            {
                exportSolutionProfile = cmBExportSolutionProfile.SelectedItem as ExportSolutionProfile;
            });

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.NoCRMConnection);
                _iWriteToOutput.ActivateOutputWindow(null, this);
                return;
            }

            if (exportSolutionProfile == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ExportSolutionProfileIsNotSelected);
                _iWriteToOutput.ActivateOutputWindow(connectionData, this);
                return;
            }

            string exportFolder = _optionsExportSolutionOptionsControl.GetExportFolder();

            if (string.IsNullOrEmpty(exportFolder))
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FolderForExportSolutionIsEmpty);
                _iWriteToOutput.ActivateOutputWindow(connectionData, this);
                return;
            }

            var service = await GetService();

            if (service == null)
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

            var solutionExportInfo = new ExportSolutionOverrideInformation(
                exportSolutionProfile.IsOverrideSolutionNameAndVersion
                , uniqueName
                , displayName
                , version
                , exportSolutionProfile.IsOverrideSolutionDescription
                , description
                );

            var config = new ExportSolutionConfig()
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

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingSolutionFormat1, solution.UniqueName);

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

                    var helper = new ExportSolutionHelper(service);

                    var filePath = await helper.ExportAsync(config, solutionExportInfo);

                    this.Dispatcher.Invoke(() =>
                    {
                        if (exportSolutionProfile.IsCopyFileToClipBoard)
                        {
                            ClipboardHelper.SetFileDropList(new System.Collections.Specialized.StringCollection() { filePath });
                        }
                    });

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution {0} exported to {1}", solution.UniqueName, filePath);

                    this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

                    if (this._selectedItem != null)
                    {
                        AddFileToVSProject(_selectedItem, filePath);
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

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingSolutionCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);


                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingSolutionFailedFormat1, solution.UniqueName);
            }
        }

        private void miSelectSolutionAsLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                && solution.IsManaged.GetValueOrDefault() == false
            )
            {
                this.Dispatcher.Invoke(() =>
                {
                    ConnectionData connectionData = GetSelectedConnection();

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

        private async void mIChangeSolutionInEditor_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                && solution.IsManaged.GetValueOrDefault() == false
            )
            {
                var service = await GetService();

                if (service == null)
                {
                    return;
                }

                WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, solution.LogicalName, solution.Id);
            }
        }

        private async void cmBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingSolutions();
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

        private async Task ExecuteActionOnSingleSolution(Solution solution, Func<string, Solution, Task> action)
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

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

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

        private async void mIComponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                await ExecuteActionOnSingleSolution(solution, PerformCreateFileWithSolutionComponents);
            }
        }

        private async void mICreateSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                await ExecuteActionOnSingleSolution(solution, PerformCreateSolutionImage);
            }
        }

        private async void mICreateSolutionImageAndOpenOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                await ExecuteActionOnSingleSolution(solution, PerformCreateSolutionImageAndOpenOrganizationComparer);
            }
        }

        private void mIOpenComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                ExecuteActionOnSingleSolutionWithoutFolderCheck(solution, PerformOpenSolutionComponentsInExplorer);
            }
        }

        private void mIOpenSolutionDescription_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
                && !string.IsNullOrEmpty(solution.Description)
            )
            {
                ExecuteActionOnSingleSolutionWithoutFolderCheck(solution, PerformOpenSolutionDescriptionInExplorerAsync);
            }
        }

        private async void mIMissingComponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                await ExecuteActionOnSingleSolution(solution, PerformShowingMissingDependencies);
            }
        }

        private async void mIUninstallComponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                await ExecuteActionOnSingleSolution(solution, PerformShowingDependenciesForUninstall);
            }
        }

        private async Task PerformCreateSolutionImage(string folder, Solution solution)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileWithSolutionImageFormat1, solution.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "SolutionImage"
                    , FileExtension.xml
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageAsync(solution.Id, solution.UniqueName);

                await solutionImage.SaveAsync(filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedSolutionImageFormat2, service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileWithSolutionImageCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileWithSolutionImageFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformCreateSolutionImageAndOpenOrganizationComparer(string folder, Solution solution)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileWithSolutionImageFormat1, solution.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageAsync(solution.Id, solution.UniqueName);

                _commonConfig.Save();

                WindowHelper.OpenOrganizationComparerWindow(_iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig, solutionImage);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileWithSolutionImageCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileWithSolutionImageFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformCreateFileWithSolutionComponents(string folder, Solution solution)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingTextFileWithComponentsFormat1, solution.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "Components"
                    , FileExtension.txt
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solution.Id);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution Components was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingTextFileWithComponentsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingTextFileWithComponentsFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformOpenSolutionComponentsInExplorer(string folder, Solution solution)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                _commonConfig.Save();
                var descriptor = GetSolutionComponentDescriptor(service);

                WindowHelper.OpenSolutionComponentsExplorer(this._iWriteToOutput, service, descriptor, _commonConfig, solution.UniqueName, null);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private Task PerformOpenSolutionDescriptionInExplorerAsync(string folder, Solution solution)
        {
            return Task.Run(() => PerformOpenSolutionDescriptionInExplorer(folder, solution));
        }

        private void PerformOpenSolutionDescriptionInExplorer(string folder, Solution solution)
        {
            if (solution == null || string.IsNullOrEmpty(solution.Description))
            {
                return;
            }

            var thread = new Thread(() =>
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

            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();
        }

        private async Task PerformShowingMissingDependencies(string folder, Solution solution)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingTextFileWithMissingDependenciesFormat1, solution.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

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
                    , FileExtension.txt
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionMissingDependenciesAsync(filePath, solution.Id, showComponents, showString);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution {0} was export into file '{1}'", showString, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingTextFileWithMissingDependenciesCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingTextFileWithMissingDependenciesFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformShowingDependenciesForUninstall(string folder, Solution solution)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingTextFileWithDependenciesForUninstallFormat1, solution.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

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
                    , FileExtension.txt
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionDependenciesForUninstallAsync(filePath, solution.Id, showComponents, showString);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution {0} was export into file '{1}'", showString, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingTextFileWithDependenciesForUninstallCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingTextFileWithDependenciesForUninstallFailedFormat1, solution.UniqueName);
            }
        }

        private async void mICompareSolutions_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Tuple<Solution, Solution> solutionPair
                && solutionPair.Item1 != null
                && solutionPair.Item2 != null
                && !string.Equals(solutionPair.Item1.UniqueName, solutionPair.Item2.UniqueName, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                await ExecuteActionOnSolutionPair(solutionPair.Item1, solutionPair.Item2, PerformCompareSolutions);
            }
        }

        private async void mIShowUniqueComponentsInSolution_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Tuple<Solution, Solution> solutionPair
                && solutionPair.Item1 != null
                && solutionPair.Item2 != null
                && !string.Equals(solutionPair.Item1.UniqueName, solutionPair.Item2.UniqueName, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                await ExecuteActionOnSolutionPair(solutionPair.Item1, solutionPair.Item2, PerformCompareSolutionsAndShowUnique);
            }
        }

        private async Task ExecuteActionOnSolutionPair(Solution solution1, Solution solution2, Func<string, Solution, Solution, Task> action)
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

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            ConnectionData connectionData = GetSelectedConnection();

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

        private async Task ExecuteActionOnSolutionAndSolutionCollection(Solution[] solutionsArray, Solution solution, Func<string, Solution[], Solution, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (solutionsArray == null || solution == null)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            try
            {
                await action(folder, solutionsArray, solution);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async Task PerformCompareSolutions(string folder, Solution solution1, Solution solution2)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ComparingSolutionsFormat2, solution1.UniqueName, solution2.UniqueName);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Comparing Solution Components '{0}' and '{1}'.", solution1.UniqueName, solution2.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                await solutionDescriptor.FindUniqueComponentsInSolutionsAsync(solution1.Id, solution2.Id);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingSolutionsCompletedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingSolutionsFailedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
        }

        private async Task PerformCompareSolutionsAndShowUnique(string folder, Solution solution1, Solution solution2)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ComparingSolutionsFormat2, solution1.UniqueName, solution2.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

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

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingSolutionsCompletedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingSolutionsFailedFormat2, solution1.UniqueName, solution2.UniqueName);
            }
        }

        private async void mICopyComponentsFromSolution1ToSolution2_Click(object sender, RoutedEventArgs e)
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

                await ExecuteActionOnSolutionPair(solutionPair.Item1, solutionPair.Item2, PerformCopyFromSolution1ToSolution2);
            }
        }

        private async void mICopyComponentsFromSolutionCollectionToSolution_Click(object sender, RoutedEventArgs e)
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

                await ExecuteActionOnSolutionAndSolutionCollection(solutionPair.Item1, solutionPair.Item2, PerformCopyFromSolutionCollectionToSolution);
            }
        }

        private async void mIRemoveComponentsOwnedSolution1FromSolution2_Click(object sender, RoutedEventArgs e)
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

                await ExecuteActionOnSolutionPair(solutionPair.Item1, solutionPair.Item2, PerformRemoveFromSolution1ToSolution2);
            }
        }

        private async void mIRemoveComponentsOwnedSolutionCollectionFromSolution_Click(object sender, RoutedEventArgs e)
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

                await ExecuteActionOnSolutionAndSolutionCollection(solutionPair.Item1, solutionPair.Item2, PerformRemoveFromSolutionCollectionToSolution);
            }
        }

        private async Task PerformCopyFromSolution1ToSolution2(string folder, Solution solutionSource, Solution solutionTarget)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CopingSolutionComponentsToFromFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                var repository = new SolutionComponentRepository(service);

                var columnSet = new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior);

                var componentsSource = await repository.GetSolutionComponentsAsync(solutionSource.Id, columnSet);

                var componentsTarget = await repository.GetSolutionComponentsAsync(solutionTarget.Id, columnSet);

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
                            , FileExtension.txt
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
                            , FileExtension.xml
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageAsync(solutionTarget.Id, solutionTarget.UniqueName);

                        await solutionImage.SaveAsync(filePath);
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

                        SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageWithComponentsAsync(solutionSource.UniqueName, componentesOnlyInSource);

                        await solutionImage.SaveAsync(filePath);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Coping Solution Components from '{0}' to '{1}'.", solutionSource.UniqueName, solutionTarget.UniqueName);

                    await Controllers.SolutionController.AddSolutionComponentsCollectionToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionTarget.UniqueName, componentesOnlyInSource, false);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Copied {0} components.", componentesOnlyInSource.Count.ToString());
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "All Solution Components '{0}' already added to '{1}'.", solutionSource.UniqueName, solutionTarget.UniqueName);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CopingSolutionComponentsToFromCompletedFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CopingSolutionComponentsToFromFailedFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);
            }
        }

        private async Task PerformCopyFromSolutionCollectionToSolution(string folder, Solution[] solutionSourceCollection, Solution solutionTarget)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            string sourceName = string.Join(",", solutionSourceCollection.Select(e => e.UniqueName).OrderBy(s => s));

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CopingSolutionComponentsToFromFormat2, solutionTarget.UniqueName, sourceName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                var repository = new SolutionComponentRepository(service);

                var componentsSource = new List<SolutionComponent>();

                var columnSet = new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior);

                {
                    var hash = new HashSet<Tuple<int, Guid>>();

                    var temp = await repository.GetSolutionComponentsForCollectionAsync(solutionSourceCollection.Select(e => e.Id), columnSet);

                    foreach (var item in temp)
                    {
                        if (hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value)))
                        {
                            componentsSource.Add(item);
                        }
                    }
                }

                var componentsTarget = await repository.GetSolutionComponentsAsync(solutionTarget.Id, columnSet);

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
                            , FileExtension.txt
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
                            , FileExtension.xml
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageAsync(solutionTarget.Id, solutionTarget.UniqueName);

                        await solutionImage.SaveAsync(filePath);
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
                            , FileExtension.xml
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageWithComponentsAsync(sourceName, componentesOnlyInSource);

                        await solutionImage.SaveAsync(filePath);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Coping Solution Components from '{0}' to '{1}'.", sourceName, solutionTarget.UniqueName);

                    await Controllers.SolutionController.AddSolutionComponentsCollectionToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionTarget.UniqueName, componentesOnlyInSource, false);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Copied {0} components.", componentesOnlyInSource.Count.ToString());
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "All Solution Components '{0}' already added to '{1}'.", sourceName, solutionTarget.UniqueName);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CopingSolutionComponentsToFromCompletedFormat2, solutionTarget.UniqueName, sourceName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CopingSolutionComponentsToFromFailedFormat2, solutionTarget.UniqueName, sourceName);
            }
        }

        private async Task PerformRemoveFromSolution1ToSolution2(string folder, Solution solutionSource, Solution solutionTarget)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.RemovingSolutionComponentsFromOwnedByFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                var repository = new SolutionComponentRepository(service);

                var columnSet = new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior);

                var componentsSource = await repository.GetSolutionComponentsAsync(solutionSource.Id, columnSet);

                var componentsTarget = await repository.GetSolutionComponentsAsync(solutionTarget.Id, columnSet);

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
                            , FileExtension.txt
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
                            , FileExtension.xml
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageAsync(solutionTarget.Id, solutionTarget.UniqueName);

                        await solutionImage.SaveAsync(filePath);
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

                        SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageWithComponentsAsync(solutionTarget.UniqueName, commonComponents);

                        await solutionImage.SaveAsync(filePath);
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

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.RemovingSolutionComponentsFromOwnedByCompletedFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.RemovingSolutionComponentsFromOwnedByFailedFormat2, solutionSource.UniqueName, solutionTarget.UniqueName);
            }
        }

        private async Task PerformRemoveFromSolutionCollectionToSolution(string folder, Solution[] solutionSourceCollection, Solution solutionTarget)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            string sourceName = string.Join(",", solutionSourceCollection.Select(e => e.UniqueName).OrderBy(s => s));

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.RemovingSolutionComponentsFromOwnedByFormat2, solutionTarget.UniqueName, sourceName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                var repository = new SolutionComponentRepository(service);

                var componentsSource = new List<SolutionComponent>();

                var columnSet = new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior);

                {
                    var hash = new HashSet<Tuple<int, Guid>>();

                    var temp = await repository.GetSolutionComponentsForCollectionAsync(solutionSourceCollection.Select(e => e.Id), columnSet);

                    foreach (var item in temp)
                    {
                        if (hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value)))
                        {
                            componentsSource.Add(item);
                        }
                    }
                }

                var componentsTarget = await repository.GetSolutionComponentsAsync(solutionTarget.Id, columnSet);

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
                            , FileExtension.txt
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
                            , FileExtension.xml
                        );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageAsync(solutionTarget.Id, solutionTarget.UniqueName);

                        await solutionImage.SaveAsync(filePath);
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

                        SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageWithComponentsAsync(solutionTarget.UniqueName, componentesOnlyInSource);

                        await solutionImage.SaveAsync(filePath);
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

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.RemovingSolutionComponentsFromOwnedByCompletedFormat2, solutionTarget.UniqueName, sourceName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.RemovingSolutionComponentsFromOwnedByFailedFormat2, solutionTarget.UniqueName, sourceName);
            }
        }

        private void miCreateNewSolutionInBrowser_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenSolutionCreateInWeb();
            }
        }

        private async void miCreateNewSolutionInEditor_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var repositoryPublisher = new PublisherRepository(service);

            var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

            var newSolution = new Solution()
            {
                FriendlyName = string.Empty,
                UniqueName = string.Empty,
                Version = "1.0.0.0",
                PublisherId = null,
            };

            if (publisherDefault != null)
            {
                newSolution.PublisherId = new Microsoft.Xrm.Sdk.EntityReference(publisherDefault.LogicalName, publisherDefault.Id)
                {
                    Name = publisherDefault.FriendlyName
                };
            }

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, Solution.EntityLogicalName, newSolution);
        }

        private void mIOpenSolutionListInWeb_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenCrmWebSite(OpenCrmWebSiteType.Solutions);
            }
        }

        private void mIOpenCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenCrmWebSite(OpenCrmWebSiteType.Customization);
            }
        }

        private async void mIOpenDefaultSolutionInWeb_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            service.ConnectionData.OpenSolutionInWeb(Solution.Schema.InstancesUniqueId.DefaultId);
        }

        private async void mIClearUnmanagedSolution_Click(object sender, RoutedEventArgs e)
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

                await ExecuteActionOnSingleSolution(solution, PerformClearUnmanagedSolution);
            }
        }

        private async Task PerformClearUnmanagedSolution(string folder, Solution solution)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var descriptor = GetSolutionComponentDescriptor(service);

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionClearingSolutionFormat2, service.ConnectionData.Name, solution.UniqueName);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Creating backup Solution Components in '{0}'.", solution.UniqueName);

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        service.ConnectionData.Name
                        , solution.UniqueName
                        , "Components Backup"
                        , FileExtension.txt
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
                        , FileExtension.xml
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageAsync(solution.Id, solution.UniqueName);

                    await solutionImage.SaveAsync(filePath);
                }

                var repository = new SolutionComponentRepository(service);

                await repository.ClearSolutionAsync(solution.UniqueName);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionClearingSolutionCompletedFormat2, service.ConnectionData.Name, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionClearingSolutionFailedFormat2, service.ConnectionData.Name, solution.UniqueName);
            }
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingSolutions();
        }

        protected override bool CanCloseWindow(KeyEventArgs e)
        {
            var popupArray = new Popup[] { _optionsPopup, _optionsSolutionPopup };

            foreach (var popup in popupArray)
            {
                if (popup.IsOpen)
                {
                    popup.IsOpen = false;
                    e.Handled = true;

                    return false;
                }
            }

            return true;
        }

        private void mIOpenSolutionInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                ConnectionData connectionData = GetSelectedConnection();

                if (connectionData != null)
                {
                    connectionData.OpenSolutionInWeb(solution.Id);
                }
            }
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                await ExecuteActionOnSingleSolution(solution, PerformCreateEntityDescription);
            }
        }

        private async void mIUsedEntitiesInWorkflows_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                await ExecuteActionOnSingleSolution(solution, PerformCreateFileWithUsedEntitiesInWorkflows);
            }
        }

        private async void mIUsedNotExistsEntitiesInWorkflows_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag is Solution solution
            )
            {
                await ExecuteActionOnSingleSolution(solution, PerformCreateFileWithUsedNotExistsEntitiesInWorkflows);
            }
        }

        private async Task PerformCreateEntityDescription(string folder, Solution solution)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                var repository = new SolutionRepository(service);

                var solutionFull = await repository.GetSolutionByIdAsync(solution.Id);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , EntityFileNameFormatter.Headers.EntityDescription
                    , FileExtension.txt
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, solutionFull, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData
                    , Properties.OutputStrings.InConnectionExportedEntityDescriptionFormat3
                    , service.ConnectionData.Name
                    , solutionFull.LogicalName
                    , filePath
                );

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionFailed);
            }
        }

        private async Task PerformCreateFileWithUsedEntitiesInWorkflows(string folder, Solution solution)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileWithUsedEntitiesInWorkflowsFormat1, solution.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "UsedEntitiesInWorkflows"
                    , FileExtension.txt
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var stringBuider = new StringBuilder();

                await workflowDescriptor.GetDescriptionWithUsedEntitiesInSolutionWorkflowsAsync(stringBuider, solution.Id);

                File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution Used Entities was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileWithUsedEntitiesInWorkflowsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileWithUsedEntitiesInWorkflowsFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformCreateFileWithUsedNotExistsEntitiesInWorkflows(string folder, Solution solution)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsFormat1, solution.UniqueName);

                var descriptor = GetSolutionComponentDescriptor(service);

                var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , solution.UniqueName
                    , "UsedNotExistsEntitiesInWorkflows"
                    , FileExtension.txt
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var stringBuider = new StringBuilder();

                await workflowDescriptor.GetDescriptionWithUsedNotExistsEntitiesInSolutionWorkflowsAsync(stringBuider, solution.Id);

                File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Solution Used Not Exists Entities was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsFailedFormat1, solution.UniqueName);
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

                            var menuItem = new MenuItem()
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

        private async void btnClearSolutionComponentFilter_Click(object sender, RoutedEventArgs e)
        {
            this._componentType = null;
            this._objectId = null;

            this.Dispatcher.Invoke(() =>
            {
                sepClearSolutionComponentFilter.IsEnabled = btnClearSolutionComponentFilter.IsEnabled = false;
                sepClearSolutionComponentFilter.Visibility = btnClearSolutionComponentFilter.Visibility = Visibility.Collapsed;
            });

            await ShowExistingSolutions();
        }

        private async void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

            ConnectionData connectionData = GetSelectedConnection();

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                BindCollections(connectionData);
            });

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (connectionData != null)
            {
                await FillChangeComponentsToLastSelectedSolutionsAsync();

                await ShowExistingSolutions();
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
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null && item.Solution != null)
                {
                    ExecuteActionOnSingleSolutionWithoutFolderCheck(item.Solution, PerformOpenSolutionComponentsInExplorer);
                }
            }
        }

        private void mIOpenSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _commonConfig.Save();

                WindowHelper.OpenSolutionImageWindow(this._iWriteToOutput, connectionData, _commonConfig);
            }
        }

        private void mIOpenSolutionDifferenceImage_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _commonConfig.Save();

                WindowHelper.OpenSolutionDifferenceImageWindow(this._iWriteToOutput, connectionData, _commonConfig);
            }
        }

        private void mIOpenOrganizationDifferenceImage_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _commonConfig.Save();

                WindowHelper.OpenOrganizationDifferenceImageWindow(this._iWriteToOutput, connectionData, _commonConfig);
            }
        }

        private async void mICreateSolutionImageFromZipFile_Click(object sender, RoutedEventArgs e)
        {
            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            string selectedPath = string.Empty;

            try
            {
                var thread = new Thread(() =>
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

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                thread.Join();

                if (string.IsNullOrEmpty(selectedPath) || !File.Exists(selectedPath))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FileNotExistsFormat1);
                    _iWriteToOutput.ActivateOutputWindow(null, this);
                    return;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                ToggleControls(service.ConnectionData, false, _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatingSolutionImageFromZipFile));

                var descriptor = GetSolutionComponentDescriptor(service);

                var components = await descriptor.LoadSolutionComponentsFromZipFileAsync(selectedPath);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, service, descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    service.ConnectionData.Name
                    , Path.GetFileNameWithoutExtension(selectedPath)
                    , "SolutionImage"
                    , FileExtension.xml
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageWithComponentsAsync(Path.GetFileNameWithoutExtension(selectedPath), components);

                await solutionImage.SaveAsync(filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedSolutionImageFormat2, service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingSolutionImageFromZipFileCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingSolutionImageFromZipFileFailed);
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