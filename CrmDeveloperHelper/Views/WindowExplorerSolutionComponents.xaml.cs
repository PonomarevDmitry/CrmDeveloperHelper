using Microsoft.Xrm.Sdk;
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
    public partial class WindowExplorerSolutionComponents : WindowBase
    {
        private readonly IWriteToOutput _iWriteToOutput;

        private readonly IOrganizationServiceExtented _service;
        private readonly SolutionComponentDescriptor _descriptor;

        private readonly SolutionComponentConverter _converter;

        private readonly CommonConfiguration _commonConfig;

        private readonly Popup _optionsPopup;

        public static readonly XmlOptionsControls _xmlOptions = XmlOptionsControls.SolutionComponentSettings;

        private Solution _solution;

        private readonly ObservableCollection<SolutionComponentViewItem> _itemsSource;

        public WindowExplorerSolutionComponents(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , string selection
            )
        {
            IncreaseInit();

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._commonConfig = commonConfig;
            this._descriptor = descriptor;

            this.Title = string.Format("{0} Solution Components", solutionUniqueName);

            if (this._descriptor == null)
            {
                this._descriptor = new SolutionComponentDescriptor(_service);
            }

            this._converter = new SolutionComponentConverter(this._descriptor);

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

            this.tSSLblConnectionName.Content = _service.ConnectionData.Name;

            FillDataGridColumns();

            LoadFromConfig();

            LoadConfiguration();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<SolutionComponentViewItem>();

            this.lstVSolutionComponents.ItemsSource = _itemsSource;

            DecreaseInit();

            if (_service != null)
            {
                ShowExistingSolutionComponents(solutionUniqueName);
            }
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        private const string paramComponentType = "ComponentType";

        private void LoadConfiguration()
        {
            WindowSettings winConfig = this.GetWindowsSettings();

            {
                var categoryValue = winConfig.GetValueInt(paramComponentType);

                if (categoryValue != -1)
                {
                    var item = cmBComponentType.Items.OfType<ComponentType?>().FirstOrDefault(e => (int)e == categoryValue);
                    if (item.HasValue)
                    {
                        cmBComponentType.SelectedItem = item.Value;

                        FillDataGridColumns();
                    }
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            var categoryValue = -1;

            if (cmBComponentType.SelectedItem is ComponentType selected)
            {
                categoryValue = (int)selected;
            }

            winConfig.DictInt[paramComponentType] = categoryValue;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _commonConfig.Save();
        }

        private enum SolutionComponentsType
        {
            SolutionComponents = 0,
            MissingComponents = 1,
            UninstallComponents = 2,
        }

        private SolutionComponentsType GetSolutionComponentsType()
        {
            var result = SolutionComponentsType.SolutionComponents;

            cmBSolutionComponentsType.Dispatcher.Invoke(() =>
            {
                if (cmBSolutionComponentsType.SelectedIndex != -1)
                {
                    result = (SolutionComponentsType)cmBSolutionComponentsType.SelectedIndex;
                }
            });

            return result;
        }

        private async Task ShowExistingSolutionComponents(string solutionUniqueName = null)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this._itemsSource.Clear();

            int? category = null;

            cmBComponentType.Dispatcher.Invoke(() =>
            {
                if (cmBComponentType.SelectedItem is ComponentType comp)
                {
                    category = (int)comp;
                }
            });

            var solutionComponents = GetSolutionComponentsType();

            var list = new List<SolutionComponent>();

            string formatResult = Properties.WindowStatusStrings.LoadingRequiredComponentsCompletedFormat1;

            try
            {
                if (this._solution == null && !string.IsNullOrEmpty(solutionUniqueName))
                {
                    var rep = new SolutionRepository(this._service);

                    this._solution = await rep.GetSolutionByUniqueNameAsync(solutionUniqueName);

                    var isManaged = this._solution?.IsManaged ?? false;

                    var description = this._solution?.Description;

                    var hasDescription = !string.IsNullOrEmpty(description);

                    this.Dispatcher.Invoke(() =>
                    {
                        miClearUnManagedSolution.IsEnabled = sepClearUnManagedSolution.IsEnabled = !isManaged;
                        miClearUnManagedSolution.Visibility = sepClearUnManagedSolution.Visibility = !isManaged ? Visibility.Visible : Visibility.Collapsed;

                        miSelectAsLastSelected.IsEnabled = sepClearUnManagedSolution2.IsEnabled = !isManaged;
                        miSelectAsLastSelected.Visibility = sepClearUnManagedSolution2.Visibility = !isManaged ? Visibility.Visible : Visibility.Collapsed;

                        miSolutionDescription.IsEnabled = sepSolutionDescription.IsEnabled = hasDescription;
                        miSolutionDescription.Visibility = sepSolutionDescription.Visibility = hasDescription ? Visibility.Visible : Visibility.Collapsed;

                        miSolutionDescription.ToolTip = description;
                    });
                }

                if (this._solution != null)
                {
                    switch (solutionComponents)
                    {
                        case SolutionComponentsType.SolutionComponents:
                        default:
                            {
                                ToggleControls(false, Properties.WindowStatusStrings.LoadingSolutionComponents);
                                formatResult = Properties.WindowStatusStrings.LoadingSolutionComponentsCompletedFormat1;

                                var repository = new SolutionComponentRepository(this._service);

                                list = await repository.GetSolutionComponentsByTypeAsync(_solution.Id, category, new ColumnSet(SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.rootcomponentbehavior));
                            }
                            break;

                        case SolutionComponentsType.MissingComponents:
                            {
                                ToggleControls(false, Properties.WindowStatusStrings.LoadingMissingComponents);
                                formatResult = Properties.WindowStatusStrings.LoadingMissingComponentsCompletedFormat1;

                                var repository = new DependencyRepository(this._service);

                                var temp = (await repository.GetSolutionMissingDependenciesAsync(_solution.UniqueName)).Select(e => e.RequiredToSolutionComponent());

                                temp = temp.Where(en => en.ComponentType != null && en.ObjectId.HasValue);

                                if (category.HasValue)
                                {
                                    temp = temp.Where(en => en.ComponentType?.Value == category.Value);
                                }

                                var hash = new HashSet<Tuple<int, Guid>>();

                                foreach (var item in temp)
                                {
                                    if (hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value)))
                                    {
                                        list.Add(item);
                                    }
                                }
                            }
                            break;

                        case SolutionComponentsType.UninstallComponents:
                            {
                                ToggleControls(false, Properties.WindowStatusStrings.LoadingUninstallComponents);
                                formatResult = Properties.WindowStatusStrings.LoadingUninstallComponentsCompletedFormat1;

                                var repository = new DependencyRepository(this._service);

                                var temp = (await repository.GetSolutionDependenciesForUninstallAsync(_solution.UniqueName)).Select(en => en.RequiredToSolutionComponent());

                                temp = temp.Where(en => en.ComponentType != null && en.ObjectId.HasValue);

                                if (category.HasValue)
                                {
                                    temp = temp.Where(en => en.ComponentType?.Value == category.Value);
                                }

                                var hash = new HashSet<Tuple<int, Guid>>();

                                foreach (var item in temp)
                                {
                                    if (hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value)))
                                    {
                                        list.Add(item);
                                    }
                                }
                            }
                            break;
                    }

                    await _descriptor.GetSolutionComponentsDescriptionAsync(list);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }

            var convertedList = new List<SolutionComponentViewItem>();

            foreach (var entity in list
                    .OrderBy(ent => ent.ComponentType.Value)
                    .ThenBy(ent => ent.ObjectId)
                )
            {
                string name = _descriptor.GetName(entity);
                string displayName = _descriptor.GetDisplayName(entity);
                string managed = _descriptor.GetManagedName(entity);
                string customizable = _descriptor.GetCustomizableName(entity);
                string behavior = _descriptor.GetRootComponentBehaviorName(entity);

                var item = new SolutionComponentViewItem(entity, name, displayName, entity.ComponentTypeName, managed, customizable, behavior);

                convertedList.Add(item);
            }

            var enumerable = convertedList.AsEnumerable();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            enumerable = FilterList(enumerable, textName);

            LoadSolutionComponents(enumerable);

            ToggleControls(true, formatResult, enumerable.Count());
        }

        private static IEnumerable<SolutionComponentViewItem> FilterList(IEnumerable<SolutionComponentViewItem> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (int.TryParse(textName, out int tempInt))
                {
                    list = list.Where(ent => ent.SolutionComponent.ComponentType?.Value == tempInt);
                }
                else
                {
                    if (Guid.TryParse(textName, out Guid tempGuid))
                    {
                        list = list.Where(ent =>
                            ent.SolutionComponent.ObjectId == tempGuid
                        );
                    }
                    else
                    {
                        list = list.Where(ent =>
                        {
                            var name = ent.Name?.ToLower() ?? string.Empty;
                            var nameUnique = ent.DisplayName?.ToLower() ?? string.Empty;

                            return name.Contains(textName) || nameUnique.Contains(textName);
                        });
                    }
                }
            }

            return list;
        }

        private void LoadSolutionComponents(IEnumerable<SolutionComponentViewItem> results)
        {
            this.lstVSolutionComponents.Dispatcher.Invoke(() =>
            {
                foreach (var item in results.OrderBy(en => en.SolutionComponent.ComponentType.Value).ThenBy(en => en.Name))
                {
                    _itemsSource.Add(item);
                }

                if (this.lstVSolutionComponents.Items.Count == 1)
                {
                    this.lstVSolutionComponents.SelectedItem = this.lstVSolutionComponents.Items[0];
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

            _iWriteToOutput.WriteToOutput(_service.ConnectionData, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.btnExportAll, this.tSDDBExportSolutionComponent, this.cmBComponentType, this.mISolutionInformation, this.cmBSolutionComponentsType);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVSolutionComponents.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVSolutionComponents.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportSolutionComponent, btnExportAll };

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

        private void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingSolutionComponents();
            }
        }

        private SolutionComponentViewItem GetSelectedSolutionComponent()
        {
            return this.lstVSolutionComponents.SelectedItems.OfType<SolutionComponentViewItem>().Count() == 1
                ? this.lstVSolutionComponents.SelectedItems.OfType<SolutionComponentViewItem>().SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (((FrameworkElement)e.OriginalSource).DataContext is SolutionComponentViewItem item)
                {
                    ExecuteAction(item, PerformExportMouseDoubleClick);
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, SolutionComponentViewItem item)
        {
            await PerformExportEntityDescription(folder, item);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(SolutionComponentViewItem item, Func<string, SolutionComponentViewItem, Task> action)
        {
            string folder = _commonConfig.FolderForExport;

            if (!this.IsControlsEnabled)
            {
                return;
            }

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

            await action(folder, item);
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            await PerformExportEntityDescription(folder, solutionComponentViewItem);
        }

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformExportEntityDescription);
        }

        private async Task PerformExportEntityDescription(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityDescription);

            string fileName = _descriptor.GetFileName(_service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, EntityFileNameFormatter.Headers.EntityDescription, "txt");

            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var stringBuilder = new StringBuilder();

            var desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(solutionComponentViewItem.SolutionComponent, null, _service.ConnectionData);

            if (!string.IsNullOrEmpty(desc))
            {
                if (stringBuilder.Length > 0) { stringBuilder.AppendLine().AppendLine().AppendLine().AppendLine(); }

                stringBuilder.AppendLine(desc);
            }

            var entity = _descriptor.GetEntity<Entity>(solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

            if (entity != null)
            {
                desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(entity, null, _service.ConnectionData);

                if (!string.IsNullOrEmpty(desc))
                {
                    if (stringBuilder.Length > 0) { stringBuilder.AppendLine().AppendLine().AppendLine().AppendLine(); }

                    stringBuilder.AppendLine(desc);
                }
            }

            File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "{0} {1} Entity Description exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

            this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

            ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSolutionComponents();
            }

            base.OnKeyDown(e);
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            this._service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb((ComponentType)entity.SolutionComponent.ComponentType.Value, entity.SolutionComponent.ObjectId.Value);
        }

        private void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            _service.UrlGenerator.OpenSolutionComponentInWeb((ComponentType)entity.SolutionComponent.ComponentType.Value, entity.SolutionComponent.ObjectId.Value);
        }

        private async void AddToSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null);
        }

        private async void AddToSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
            )
            {
                await AddToSolution(false, solutionUniqueName);
            }
        }

        private async void AddToCurrentSolution_Click(object sender, RoutedEventArgs e)
        {
            if (GetSolutionComponentsType() == SolutionComponentsType.SolutionComponents
                || _solution == null
                || _solution.IsManaged.GetValueOrDefault()
                )
            {
                return;
            }

            await AddToSolution(false, _solution.UniqueName);
        }

        private async Task AddToSolution(bool withSelect, string solutionUniqueName)
        {
            var solutionComponents = lstVSolutionComponents.SelectedItems.OfType<SolutionComponentViewItem>().Select(en => en.SolutionComponent).ToList();

            if (!solutionComponents.Any())
            {
                return;
            }

            await AddComponentsToSolution(withSelect, solutionUniqueName, solutionComponents);
        }

        private async Task AddComponentsToSolution(bool withSelect, string solutionUniqueName, IEnumerable<SolutionComponent> solutionComponents)
        {
            if (!solutionComponents.Any())
            {
                return;
            }

            _commonConfig.Save();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);

                await SolutionController.AddSolutionComponentsCollectionToSolution(_iWriteToOutput, _service, _descriptor, _commonConfig, solutionUniqueName, solutionComponents, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private async void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            {
                var enabledAdd = GetSolutionComponentsType() != SolutionComponentsType.SolutionComponents && this._solution != null && !this._solution.IsManaged.GetValueOrDefault();
                var enabledRemove = GetSolutionComponentsType() == SolutionComponentsType.SolutionComponents && this._solution != null && !this._solution.IsManaged.GetValueOrDefault();

                ActivateControls(items, enabledRemove, "contMnRemoveFromSolution");

                ActivateControls(items, enabledAdd, "contMnAddToCurrentSolution");
            }

            FillLastSolutionItems(_service.ConnectionData, items, true, AddToSolutionLast_Click, "contMnAddToSolutionLast");

            var selectedSolutionComponent = GetSelectedSolutionComponent();

            var hasExplorer = false;

            if (selectedSolutionComponent != null)
            {
                hasExplorer = HasExplorer(selectedSolutionComponent.SolutionComponent.ComponentType?.Value);

                if (hasExplorer)
                {
                    var componentType = (ComponentType)selectedSolutionComponent.SolutionComponent.ComponentType.Value;

                    string componentName = string.Format("{0} Explorer", componentType.ToString());

                    SetControlsName(items, componentName, "contMnExplorer");
                }
            }

            ActivateControls(items, hasExplorer, "contMnExplorer");

            var menuItemLinkedComponent = items.OfType<MenuItem>().FirstOrDefault(i => string.Equals(i.Uid, "contMnLinkedComponents", StringComparison.InvariantCultureIgnoreCase));

            if (menuItemLinkedComponent != null)
            {
                menuItemLinkedComponent.Items.Clear();

                if (selectedSolutionComponent != null)
                {
                    var linkedComponents = _descriptor.GetLinkedComponents(selectedSolutionComponent.SolutionComponent);

                    if (linkedComponents != null && linkedComponents.Any())
                    {
                        var componentsWithNames = linkedComponents.Select(c => new { Component = c, Name = _descriptor.GetName(c) }).OrderBy(c => c.Component.ComponentType?.Value).ThenBy(c => c.Name);

                        foreach (var item in componentsWithNames)
                        {
                            var menuItem = new MenuItem()
                            {
                                Header = string.Format("{0} - {1}", item.Component.ComponentTypeName, item.Name).Replace("_", "__"),
                            };

                            FillLinkedSolutionComponentActions(menuItem.Items, item.Component);

                            menuItemLinkedComponent.Items.Add(menuItem);
                        }
                    }
                }

                ActivateControls(items, menuItemLinkedComponent.Items.Count > 0, "contMnLinkedComponents");
            }
        }

        private void FillLinkedSolutionComponentActions(ItemCollection itemCollection, SolutionComponent solutionComponent)
        {
            MenuItem mILinkedComponentOpenInWeb = new MenuItem()
            {
                Header = "Open in Web",
                Tag = solutionComponent,
            };
            mILinkedComponentOpenInWeb.Click += MILinkedComponentOpenInWeb_Click;

            MenuItem mILinkedComponentOpenInstanceListInWindow = new MenuItem()
            {
                Header = "Open Entity List in Web",
                Tag = solutionComponent,
            };
            mILinkedComponentOpenInstanceListInWindow.Click += MILinkedComponentOpenInstanceListInWindow_Click;

            MenuItem mILinkedComponentOpenExplorer = new MenuItem()
            {
                Header = "Open Explorer",
                Tag = solutionComponent,
            };
            mILinkedComponentOpenExplorer.Click += MILinkedComponentOpenExplorer_Click;

            MenuItem mILinkedComponentAddToCurrentSolution = new MenuItem()
            {
                Header = "Add to Current Solution",
                Tag = solutionComponent,
            };
            mILinkedComponentAddToCurrentSolution.Click += MILinkedComponentAddToCurrentSolution_Click;

            MenuItem mILinkedComponentAddToSolutionLast = new MenuItem()
            {
                Header = "Add to Last Crm Solution",
                Tag = solutionComponent,
                Uid = "mILinkedComponentAddToSolutionLast",
            };

            FillLastSolutionItems(_service.ConnectionData, new[] { mILinkedComponentAddToSolutionLast }, true, MILinkedComponentAddToSolutionLast_Click, "mILinkedComponentAddToSolutionLast");

            MenuItem mILinkedComponentAddToSolution = new MenuItem()
            {
                Header = "Add to Crm Solution",
                Tag = solutionComponent,
            };
            mILinkedComponentAddToSolution.Click += MILinkedComponentAddToSolution_Click;

            MenuItem mILinkedComponentOpenSolutionsContainingComponentInWindow = new MenuItem()
            {
                Header = "Open Solutions Containing Component in Window",
                Tag = solutionComponent,
            };
            mILinkedComponentOpenSolutionsContainingComponentInWindow.Click += MILinkedComponentOpenSolutionsContainingComponentInWindow_Click;

            MenuItem mILinkedComponentOpenDependentComponentsInWeb = new MenuItem()
            {
                Header = "Open Dependent Components in Web",
                Tag = solutionComponent,
            };
            mILinkedComponentOpenDependentComponentsInWeb.Click += MILinkedComponentOpenDependentComponentsInWeb_Click;

            MenuItem mILinkedComponentOpenDependentComponentsInWindow = new MenuItem()
            {
                Header = "Open Dependent Components in Window",
                Tag = solutionComponent,
            };
            mILinkedComponentOpenDependentComponentsInWindow.Click += MILinkedComponentOpenDependentComponentsInWindow_Click;

            //MenuItem mILinkedComponent = new MenuItem()
            //{
            //    Header = "Open Entity List in Web",
            //    Tag = solutionComponent,
            //};

            itemCollection.Add(mILinkedComponentOpenInWeb);

            if (solutionComponent.ComponentType?.Value == (int)ComponentType.Entity)
            {
                itemCollection.Add(new Separator());
                itemCollection.Add(mILinkedComponentOpenInstanceListInWindow);
            }

            if (HasExplorer(solutionComponent.ComponentType?.Value))
            {
                itemCollection.Add(new Separator());
                itemCollection.Add(mILinkedComponentOpenExplorer);
            }

            itemCollection.Add(new Separator());

            if (this._solution != null && !this._solution.IsManaged.GetValueOrDefault())
            {
                itemCollection.Add(mILinkedComponentAddToCurrentSolution);
            }

            itemCollection.Add(mILinkedComponentAddToSolutionLast);
            itemCollection.Add(mILinkedComponentAddToSolution);

            itemCollection.Add(new Separator());
            itemCollection.Add(mILinkedComponentOpenSolutionsContainingComponentInWindow);

            itemCollection.Add(new Separator());
            itemCollection.Add(mILinkedComponentOpenDependentComponentsInWeb);
            itemCollection.Add(mILinkedComponentOpenDependentComponentsInWindow);
        }

        private void MILinkedComponentOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
                || menuItem.Tag == null
                || !(menuItem.Tag is SolutionComponent solutionComponent)
                )
            {
                return;
            }

            _service.UrlGenerator.OpenSolutionComponentInWeb((ComponentType)solutionComponent.ComponentType.Value, solutionComponent.ObjectId.Value);
        }

        private void MILinkedComponentOpenInstanceListInWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
               )
            {
                return;
            }

            var entityMetadata = _descriptor.MetadataSource.GetEntityMetadata(solutionComponent.ObjectId.Value);

            if (entityMetadata == null
                || string.IsNullOrEmpty(entityMetadata.LogicalName)
            )
            {
                return;
            }

            _service.ConnectionData.OpenEntityInstanceListInWeb(entityMetadata.LogicalName);
        }

        private void MILinkedComponentOpenExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
                || menuItem.Tag == null
                || !(menuItem.Tag is SolutionComponent solutionComponent)
                )
            {
                return;
            }

            if (!HasExplorer(solutionComponent.ComponentType?.Value))
            {
                return;
            }

            var componentType = (ComponentType)solutionComponent.ComponentType.Value;

            string parameter = string.Empty;

            if (componentType == ComponentType.EntityRelationship)
            {
                var relation = _descriptor.MetadataSource.GetRelationshipMetadata(solutionComponent.ObjectId.Value);

                if (relation != null)
                {
                    parameter = relation.GetType().Name;
                }
            }

            var name = _descriptor.GetName(solutionComponent);

            WindowHelper.OpenComponentExplorer(componentType, _iWriteToOutput, _service, _commonConfig, name, parameter);
        }

        private async void MILinkedComponentAddToCurrentSolution_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
                || menuItem.Tag == null
                || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            if (_solution == null
                || _solution.IsManaged.GetValueOrDefault()
                )
            {
                return;
            }

            await AddComponentsToSolution(false, _solution.UniqueName, new[] { solutionComponent });
        }

        private async void MILinkedComponentAddToSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                && menuItem.Parent is MenuItem menuItemParent
                && menuItemParent.Tag != null
                && menuItemParent.Tag is SolutionComponent solutionComponent
            )
            {
                await AddComponentsToSolution(false, solutionUniqueName, new[] { solutionComponent });
            }
        }

        private async void MILinkedComponentAddToSolution_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            await AddComponentsToSolution(true, null, new[] { solutionComponent });
        }

        private void MILinkedComponentOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , _service
                , _commonConfig
                , solutionComponent.ComponentType.Value
                , solutionComponent.ObjectId.Value
                , null
            );
        }

        private void MILinkedComponentOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            this._service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb((ComponentType)solutionComponent.ComponentType.Value, solutionComponent.ObjectId.Value);
        }

        private void MILinkedComponentOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenSolutionComponentDependenciesWindow(
                _iWriteToOutput
                , _service
                , _descriptor
                , _commonConfig
                , solutionComponent.ComponentType.Value
                , solutionComponent.ObjectId.Value
                , null);
        }

        private void cmBComponentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            FillDataGridColumns();

            ShowExistingSolutionComponents();
        }

        private void cmBSolutionComponentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ShowExistingSolutionComponents();
        }

        private void FillDataGridColumns()
        {
            int? category = null;

            cmBComponentType.Dispatcher.Invoke(() =>
            {
                if (cmBComponentType.SelectedItem is ComponentType comp)
                {
                    category = (int)comp;
                }
            });

            TupleList<string, string> columnsForComponentType = null;

            if (category.HasValue)
            {
                columnsForComponentType = _descriptor.GetComponentColumns(category.Value);
            }

            WindowSettings winConfig = this.GetWindowsSettings();

            foreach (var item in FindChildren<DataGrid>(this))
            {
                SaveDataGridColumnsWidths(item, winConfig);
            }

            lstVSolutionComponents.Columns.Clear();

            if (columnsForComponentType != null && columnsForComponentType.Any())
            {
                foreach (var item in columnsForComponentType)
                {
                    var column = new DataGridTextColumn()
                    {
                        Header = item.Item2,
                        Binding = new Binding()
                        {
                            Converter = this._converter,
                            ConverterParameter = item.Item1,
                        },
                        Width = 120,
                    };

                    lstVSolutionComponents.Columns.Add(column);
                }
            }
            else
            {
                //< DataGridTextColumn Header = "Name" Width = "120" Binding = "{Binding Name}" />
                //< DataGridTextColumn Header = "DisplayName" Width = "120" Binding = "{Binding DisplayName}" />
                //< DataGridTextColumn Header = "ComponentType" Width = "120" Binding = "{Binding ComponentType}" />
                //< DataGridTextColumn Header = "IsManaged" Width = "120" Binding = "{Binding IsManaged}" />
                //< DataGridTextColumn Header = "IsCustomizable" Width = "120" Binding = "{Binding IsCustomizable}" />

                string[] columns = { "Name", "DisplayName", "ComponentType", "Behavior", "IsManaged", "IsCustomizable" };

                foreach (var item in columns)
                {
                    var column = new DataGridTextColumn()
                    {
                        Header = item,
                        Binding = new Binding(item),
                        Width = 120,
                    };

                    lstVSolutionComponents.Columns.Add(column);
                }
            }

            foreach (var item in FindChildren<DataGrid>(this))
            {
                LoadDataGridColumnsWidths(item, winConfig);
            }
        }

        private void mIOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenSolutionComponentDependenciesWindow(
                _iWriteToOutput
                , _service
                , _descriptor
                , _commonConfig
                , entity.SolutionComponent.ComponentType.Value
                , entity.SolutionComponent.ObjectId.Value
                , null);
        }

        private void mIOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , _service
                , _commonConfig
                , entity.SolutionComponent.ComponentType.Value
                , entity.SolutionComponent.ObjectId.Value
                , null
            );
        }

        private void OpenSolutionInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                _service.ConnectionData.OpenSolutionInWeb(_solution.Id);
            }
        }

        private async void RemoveComponentFromSolution_Click(object sender, RoutedEventArgs e)
        {
            if (GetSolutionComponentsType() != SolutionComponentsType.SolutionComponents
                || this._solution == null
                || this._solution.IsManaged.GetValueOrDefault()
            )
            {
                return;
            }

            var componentsToRemove = lstVSolutionComponents.SelectedItems.OfType<SolutionComponentViewItem>().ToList();

            var solutionComponents = componentsToRemove.Select(en => en.SolutionComponent).ToList();

            if (!solutionComponents.Any())
            {
                return;
            }

            string question = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteComponentsFormat2, solutionComponents.Count, _solution.UniqueName);

            if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.RemovingSolutionComponentsFromSolutionFormat2, _service.ConnectionData.Name, _solution.UniqueName);

                _commonConfig.Save();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, _descriptor);

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        _service.ConnectionData.Name
                        , _solution.UniqueName
                        , "SolutionImage Components Backup before removing"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, _solution.Id);

                    this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Created backup Solution Components in '{0}': {1}", _solution.UniqueName, filePath);
                    this._iWriteToOutput.WriteToOutputFilePathUri(_service.ConnectionData, filePath);
                }

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        _service.ConnectionData.Name
                        , _solution.UniqueName
                        , "SolutionImage Components Backup before removing"
                        , "xml"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, _solution.Id, _solution.UniqueName);
                }

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        _service.ConnectionData.Name
                        , _solution.UniqueName
                        , "SolutionImage Removing Components"
                        , "xml"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateSolutionImageWithComponentsAsync(filePath, _solution.UniqueName, solutionComponents);
                }

                SolutionComponentRepository repository = new SolutionComponentRepository(this._service);

                await repository.RemoveSolutionComponentsAsync(_solution.UniqueName, solutionComponents);

                lstVSolutionComponents.Dispatcher.Invoke(() =>
                {
                    foreach (var item in componentsToRemove)
                    {
                        _itemsSource.Remove(item);
                    }
                });

                ToggleControls(true, Properties.WindowStatusStrings.RemovingSolutionComponentsFromSolutionCompletedFormat2, _service.ConnectionData.Name, _solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.RemovingSolutionComponentsFromSolutionFailedFormat2, _service.ConnectionData.Name, _solution.UniqueName);
            }
        }

        private async void miClearUnManagedSolution_Click(object sender, RoutedEventArgs e)
        {
            string question = string.Format(Properties.MessageBoxStrings.ClearSolutionFormat1, _solution.UniqueName);

            if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.ClearingSolutionFormat2, _service.ConnectionData.Name, _solution.UniqueName);

                _commonConfig.Save();

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, _descriptor);

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        _service.ConnectionData.Name
                        , _solution.UniqueName
                        , "Components Backup"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, _solution.Id);

                    this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Created backup Solution Components in '{0}': {1}", _solution.UniqueName, filePath);
                    this._iWriteToOutput.WriteToOutputFilePathUri(_service.ConnectionData, filePath);
                }

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        _service.ConnectionData.Name
                        , _solution.UniqueName
                        , "SolutionImage Backup before Clearing"
                        , "xml"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, _solution.Id, _solution.UniqueName);
                }

                SolutionComponentRepository repository = new SolutionComponentRepository(this._service);

                await repository.ClearSolutionAsync(_solution.UniqueName);

                lstVSolutionComponents.Dispatcher.Invoke(() =>
                {
                    _itemsSource.Clear();
                });

                ToggleControls(true, Properties.WindowStatusStrings.ClearingSolutionCompletedFormat2, _service.ConnectionData.Name, _solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.ClearingSolutionFailedFormat2, _service.ConnectionData.Name, _solution.UniqueName);
            }
        }

        private void miSelectAsLastSelected_Click(object sender, RoutedEventArgs e)
        {
            if (_solution.IsManaged.GetValueOrDefault())
            {
                return;
            }

            _service.ConnectionData.AddLastSelectedSolution(_solution.UniqueName);

            _iWriteToOutput.WriteToOutputSolutionUri(_service.ConnectionData, _solution.UniqueName, _solution.Id);
        }

        #region Кнопки открытия других форм с информация о сущности.

        private void btnCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, _service, _commonConfig);
        }

        private void btnEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, _service, _commonConfig, null);
        }

        private void btnEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, _service, _commonConfig, null);
        }

        private void btnEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, _service, _commonConfig, null);
        }

        private void btnEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, _service, _commonConfig, null);
        }

        private void miEntityPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, _service, _commonConfig);
        }

        private void miSecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, _service, _commonConfig);
        }

        private void btnGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , _service
                , _commonConfig
            );
        }

        private void btnSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, _service, _commonConfig);
        }

        private void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, _service, _commonConfig, null, string.Empty);
        }

        private void btnSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, _service, _commonConfig, null, string.Empty);
        }

        private void btnWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, _service, _commonConfig, null, string.Empty);
        }

        private void btnExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, _service, _commonConfig);
        }

        private void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, _service, _commonConfig, string.Empty, string.Empty, string.Empty);
        }

        private void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, _service, _commonConfig, null, string.Empty);
        }

        private void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, _service, _commonConfig);
        }

        private void btnSiteMap_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenExportSiteMapWindow(this._iWriteToOutput, _service, _commonConfig);
        }

        private void btnWebResources_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenWebResourceExplorerWindow(this._iWriteToOutput, _service, _commonConfig, string.Empty);
        }

        private void btnExportReport_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenReportExplorerWindow(this._iWriteToOutput, _service, _commonConfig, string.Empty);
        }

        private void btnPluginAssembly_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenPluginAssemblyWindow(this._iWriteToOutput, _service, _commonConfig, null);
        }

        private void btnPluginType_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenPluginTypeWindow(this._iWriteToOutput, _service, _commonConfig, null);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        private void ExecuteActionOnSingleSolution(Solution solution, Func<string, Solution, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (solution == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, _commonConfig.FolderForExport);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            try
            {
                action(_commonConfig.FolderForExport, solution);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private void mICreateSolutionEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformSolutionEntityDescription);
            }
        }

        private void mIUsedEntitiesInWorkflows_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformCreateFileWithUsedEntitiesInWorkflows);
            }
        }

        private void mIUsedNotExistsEntitiesInWorkflows_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformCreateFileWithUsedNotExistsEntitiesInWorkflows);
            }
        }

        private async Task PerformSolutionEntityDescription(string folder, Solution solution)
        {
            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityDescription);

            try
            {
                SolutionRepository repository = new SolutionRepository(_service);

                var solutionFull = await repository.GetSolutionByIdAsync(solution.Id);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    _service.ConnectionData.Name
                    , solution.UniqueName
                    , EntityFileNameFormatter.Headers.EntityDescription
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, solutionFull, EntityFileNameFormatter.WebResourceIgnoreFields, _service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData
                    , Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , _service.ConnectionData.Name
                    , solutionFull.LogicalName
                    , filePath
                );

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionFailed);
            }
        }

        private async Task PerformCreateFileWithUsedEntitiesInWorkflows(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingFileWithUsedEntitiesInWorkflowsFormat1, solution.UniqueName);

                var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, _service, _descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    _service.ConnectionData.Name
                    , solution.UniqueName
                    , "UsedEntitiesInWorkflows"
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var stringBuider = new StringBuilder();

                await workflowDescriptor.GetDescriptionWithUsedEntitiesInSolutionWorkflowsAsync(stringBuider, solution.Id);

                File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Solution Used Entities was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithUsedEntitiesInWorkflowsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithUsedEntitiesInWorkflowsFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformCreateFileWithUsedNotExistsEntitiesInWorkflows(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsFormat1, solution.UniqueName);

                var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, _service, _descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    _service.ConnectionData.Name
                    , solution.UniqueName
                    , "UsedNotExistsEntitiesInWorkflows"
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var stringBuider = new StringBuilder();

                await workflowDescriptor.GetDescriptionWithUsedNotExistsEntitiesInSolutionWorkflowsAsync(stringBuider, solution.Id);

                File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Solution Used Not Exists Entities was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithUsedNotExistsEntitiesInWorkflowsFailedFormat1, solution.UniqueName);
            }
        }

        private void mICreateSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformCreateSolutionImage);
            }
        }

        private void mICreateSolutionImageAndOpenOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformCreateSolutionImageAndOpenOrganizationComparer);
            }
        }

        private void mILoadSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformLoadFromSolutionImage);
            }
        }

        private void mILoadSolutionZip_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformLoadFromSolutionZipFile);
            }
        }

        private void mIComponentsIn_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformCreateFileWithSolutionComponents);
            }
        }

        private void mIMissingComponentsIn_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformShowingMissingDependencies);
            }
        }

        private void mIUninstallComponentsIn_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformShowingDependenciesForUninstall);
            }
        }

        private async Task PerformCreateSolutionImage(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingFileWithSolutionImageFormat1, solution.UniqueName);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, _descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    _service.ConnectionData.Name
                    , solution.UniqueName
                    , "SolutionImage"
                    , "xml"
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solution.Id, solution.UniqueName);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.ExportedSolutionImageForConnectionFormat2, _service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformCreateSolutionImageAndOpenOrganizationComparer(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingFileWithSolutionImageFormat1, solution.UniqueName);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, _descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    _service.ConnectionData.Name
                    , solution.UniqueName
                    , "SolutionImage"
                    , "xml"
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solution.Id, solution.UniqueName);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.ExportedSolutionImageForConnectionFormat2, _service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                _commonConfig.Save();

                WindowHelper.OpenOrganizationComparerWindow(_iWriteToOutput, _service.ConnectionData.ConnectionConfiguration, _commonConfig, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformLoadFromSolutionImage(string folder, Solution solution)
        {
            try
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
                        _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                    }
                });

                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (string.IsNullOrEmpty(selectedPath) || !File.Exists(selectedPath))
                {
                    return;
                }

                ToggleControls(false, Properties.WindowStatusStrings.LoadingComponentsFromSolutionImage);

                SolutionImage solutionImage = null;

                try
                {
                    solutionImage = await SolutionImage.LoadAsync(selectedPath);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                    solutionImage = null;
                }

                if (solutionImage == null)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.LoadingSolutionImageFailed);
                    return;
                }

                UpdateStatus(Properties.WindowStatusStrings.LoadedComponentsFromSolutionImageFormat1, solutionImage.Components.Count);

                if (solutionImage.Components.Count == 0)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.NoComponentsToAdd);
                    return;
                }

                var solutionComponents = await _descriptor.GetSolutionComponentsListAsync(solutionImage.Components);

                UpdateStatus(Properties.WindowStatusStrings.AddingComponentsToSolutionFormat3, _service.ConnectionData.Name, solutionComponents.Count, _solution.UniqueName);

                if (solutionComponents.Count == 0)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.NoComponentsToAdd);
                    return;
                }

                _commonConfig.Save();

                this._iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);

                await SolutionController.AddSolutionComponentsCollectionToSolution(_iWriteToOutput, _service, _descriptor, _commonConfig, _solution.UniqueName, solutionComponents, false);

                ToggleControls(true, Properties.WindowStatusStrings.LoadingComponentsFromSolutionImageCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.LoadingComponentsFromSolutionImageFailed);
            }
        }

        private async Task PerformLoadFromSolutionZipFile(string folder, Solution solution)
        {
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
                        _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                    }
                });

                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (string.IsNullOrEmpty(selectedPath) || !File.Exists(selectedPath))
                {
                    return;
                }

                ToggleControls(false, Properties.WindowStatusStrings.LoadingComponentsFromZipFile);

                List<SolutionComponent> solutionComponents = await _descriptor.LoadSolutionComponentsFromZipFileAsync(selectedPath);

                UpdateStatus(Properties.WindowStatusStrings.LoadedComponentsFromZipFileFormat1, solutionComponents.Count);

                if (solutionComponents.Count == 0)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.NoComponentsToAdd);
                    return;
                }

                UpdateStatus(Properties.WindowStatusStrings.AddingComponentsToSolutionFormat3, _service.ConnectionData.Name, solutionComponents.Count, _solution.UniqueName);

                this._iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);

                await SolutionController.AddSolutionComponentsCollectionToSolution(_iWriteToOutput, _service, _descriptor, _commonConfig, _solution.UniqueName, solutionComponents, false);

                ToggleControls(true, Properties.WindowStatusStrings.LoadingComponentsFromZipFileCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.LoadingComponentsFromZipFileFailed);
            }
        }

        private async Task PerformCreateFileWithSolutionComponents(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingTextFileWithComponentsFormat1, solution.UniqueName);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, _descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    _service.ConnectionData.Name
                    , solution.UniqueName
                    , "Components"
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solution.Id);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Solution Components was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithComponentsCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithComponentsFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformShowingMissingDependencies(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingTextFileWithMissingDependenciesFormat1, solution.UniqueName);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, _descriptor);

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
                    _service.ConnectionData.Name
                    , solution.UniqueName
                    , showString
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionMissingDependenciesAsync(filePath, solution.Id, showComponents, showString);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Solution {0} was export into file '{1}'", showString, filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithMissingDependenciesCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithMissingDependenciesFailedFormat1, solution.UniqueName);
            }
        }

        private async Task PerformShowingDependenciesForUninstall(string folder, Solution solution)
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.CreatingTextFileWithDependenciesForUninstallFormat1, solution.UniqueName);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, _descriptor);

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
                    _service.ConnectionData.Name
                    , solution.UniqueName
                    , showString
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await solutionDescriptor.CreateFileWithSolutionDependenciesForUninstallAsync(filePath, solution.Id, showComponents, showString);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Solution {0} was export into file '{1}'", showString, filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithDependenciesForUninstallCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingTextFileWithDependenciesForUninstallFailedFormat1, solution.UniqueName);
            }
        }

        private void mIOpenSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSolutionImageWindow(this._iWriteToOutput, _service.ConnectionData, _commonConfig);
        }

        private void mIOpenSolutionDifferenceImage_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSolutionDifferenceImageWindow(this._iWriteToOutput, _service.ConnectionData, _commonConfig);
        }

        private void mIOpenOrganizationDifferenceImage_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenOrganizationDifferenceImageWindow(this._iWriteToOutput, _service.ConnectionData, _commonConfig);
        }

        private void mIOpenExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            if (!HasExplorer(entity.SolutionComponent.ComponentType?.Value))
            {
                return;
            }

            var componentType = (ComponentType)entity.SolutionComponent.ComponentType.Value;

            string parameter = string.Empty;

            if (componentType == ComponentType.EntityRelationship)
            {
                var relation = _descriptor.MetadataSource.GetRelationshipMetadata(entity.SolutionComponent.ObjectId.Value);

                if (relation != null)
                {
                    parameter = relation.GetType().Name;
                }
            }

            WindowHelper.OpenComponentExplorer(componentType, _iWriteToOutput, _service, _commonConfig, entity.Name, parameter);
        }

        private bool HasExplorer(int? componentType)
        {
            if (!SolutionComponent.IsDefinedComponentType(componentType))
            {
                return false;
            }

            return WindowHelper.IsDefinedExplorer((ComponentType)componentType);
        }

        private void miDescriptionOptions_Click(object sender, RoutedEventArgs e)
        {
            this._optionsPopup.IsOpen = true;
            this._optionsPopup.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_optionsPopup.IsOpen)
            {
                _optionsPopup.IsOpen = false;
            }

            this.Focus();
        }

        private void miSolutionDescription_Click(object sender, RoutedEventArgs e)
        {
            if (_solution == null || string.IsNullOrEmpty(_solution.Description))
            {
                return;
            }

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var title = _solution.UniqueName + " Description";

                    var form = new WindowTextField(title, title, _solution.Description, true);

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        private void miOpenSolutionExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , _service
                , _commonConfig
                , null
                , null
                , null
            );
        }

        private void btnOpenInWebCustomization_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenCrmWebSite(OpenCrmWebSiteType.Customization);
        }

        private void btnOpenInWebSolutionList_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenCrmWebSite(OpenCrmWebSiteType.Solutions);
        }

        private void btnOpenInWebDefaultSolution_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenSolutionInWeb(Solution.Schema.InstancesUniqueId.DefaultId);
        }

        private void lstVSolutionComponents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true;

                RemoveComponentFromSolution_Click(null, null);
            }
        }
    }
}