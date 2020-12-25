using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerComponents : WindowWithSingleConnection
    {
        private readonly CommonConfiguration _commonConfig;

        private readonly SolutionComponentDescriptor _descriptor;

        private readonly SolutionComponentConverter _converter;

        private readonly Popup _optionsPopup;

        private Solution _solution;

        private readonly ObservableCollection<SolutionComponentViewItem> _itemsSource;

        private readonly IEnumerable<SolutionComponent> _solutionComponents;

        private readonly string _header;

        public WindowExplorerComponents(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , IEnumerable<SolutionComponent> solutionComponents
            , string solutionUniqueName
            , string header
            , string selection
        ) : base(iWriteToOutput, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            cmBComponentType.ItemsSource = new EnumBindingSourceExtension(typeof(ComponentType?))
            { 
                SortByName = true,
            }.ProvideValue(null) as IEnumerable;

            SetInputLanguageEnglish();

            this._commonConfig = commonConfig;
            this._descriptor = descriptor;
            this._solutionComponents = solutionComponents;

            this._header = header;

            this.Title = string.Format("{0} {1}", solutionUniqueName, this._header);

            if (this._descriptor == null)
            {
                this._descriptor = new SolutionComponentDescriptor(_service);
            }

            this._converter = new SolutionComponentConverter(this._descriptor);

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

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingComponents(solutionUniqueName);
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, () => Task.FromResult(_service));

            explorersHelper.FillExplorers(miExplorers);
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

        private async Task ShowExistingComponents(string solutionUniqueName = null)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this._itemsSource.Clear();

            ToggleControls(false, Properties.OutputStrings.LoadingComponents);

            int? category = null;

            cmBComponentType.Dispatcher.Invoke(() =>
            {
                if (cmBComponentType.SelectedItem is ComponentType comp)
                {
                    category = (int)comp;
                }
            });

            IEnumerable<SolutionComponent> list = _solutionComponents;

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
                        miSolutionDescription.IsEnabled = sepSolutionDescription.IsEnabled = hasDescription;
                        miSolutionDescription.Visibility = sepSolutionDescription.Visibility = hasDescription ? Visibility.Visible : Visibility.Collapsed;

                        miSolutionDescription.ToolTip = description;
                    });
                }

                if (category.HasValue)
                {
                    list = list.Where(en => en.ComponentType?.Value == category.Value);
                }

                await _descriptor.GetSolutionComponentsDescriptionAsync(_solutionComponents);
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

            ToggleControls(true, Properties.OutputStrings.LoadingComponentsCompletedFormat1, enumerable.Count());
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
                        list = list.Where(ent => ent.SolutionComponent.ObjectId == tempGuid);
                    }
                    else
                    {
                        list = list.Where(ent =>
                        {
                            var name = ent.Name ?? string.Empty;
                            var nameUnique = ent.DisplayName ?? string.Empty;

                            return name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1 || nameUnique.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1;
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

        protected override void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.btnExportAll, this.tSDDBExportSolutionComponent, this.cmBComponentType, this.mISolutionInformation);

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

        private async void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingComponents();
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

        private async void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                SolutionComponentViewItem item = GetItemFromRoutedDataContext<SolutionComponentViewItem>(e);

                if (item != null)
                {
                    await ExecuteAction(item, PerformExportMouseDoubleClick);
                }
            }
        }

        private Task PerformExportMouseDoubleClick(string folder, SolutionComponentViewItem item)
        {
            return Task.Run(() => _service.UrlGenerator.OpenSolutionComponentInWeb((ComponentType)item.SolutionComponent.ComponentType.Value, item.SolutionComponent.ObjectId.Value));
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(SolutionComponentViewItem item, Func<string, SolutionComponentViewItem, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = _commonConfig.FolderForExport;

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, item);
        }

        private async void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity, PerformExportAll);
        }

        private async Task PerformExportAll(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            await PerformExportEntityDescription(folder, solutionComponentViewItem);
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity, PerformExportEntityDescription);
        }

        private async Task PerformExportEntityDescription(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            ToggleControls(false, Properties.OutputStrings.CreatingEntityDescription);

            string fileName = _descriptor.GetFileName(_service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);

            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var stringBuilder = new StringBuilder();

            var desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(solutionComponentViewItem.SolutionComponent, _service.ConnectionData);

            if (!string.IsNullOrEmpty(desc))
            {
                if (stringBuilder.Length > 0) { stringBuilder.AppendLine().AppendLine().AppendLine().AppendLine(); }

                stringBuilder.AppendLine(desc);
            }

            var entity = _descriptor.GetEntity<Entity>(solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

            if (entity != null)
            {
                desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(entity, _service.ConnectionData);

                if (!string.IsNullOrEmpty(desc))
                {
                    if (stringBuilder.Length > 0) { stringBuilder.AppendLine().AppendLine().AppendLine().AppendLine(); }

                    stringBuilder.AppendLine(desc);
                }
            }

            File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "{0} {1} Entity Description exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

            this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

            ToggleControls(true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
        }

        protected async override Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingComponents();
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

        private async void AddToCurrentSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(false, _solution.UniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void AddToCurrentSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(false, _solution.UniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
        }

        private async void AddToCurrentSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(false, _solution.UniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
        }

        private async void AddToCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void AddToCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
        }

        private async void AddToCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
        }

        private async void AddToCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
            }
        }

        private async void AddToCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
            }
        }

        private async void AddToCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
            }
        }

        private async Task AddToSolution(bool withSelect, string solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior behavior)
        {
            var solutionComponents = lstVSolutionComponents.SelectedItems.OfType<SolutionComponentViewItem>().Select(en => en.SolutionComponent).ToList();

            if (!solutionComponents.Any())
            {
                return;
            }

            await AddComponentsToSolution(withSelect, solutionUniqueName, solutionComponents, behavior);
        }

        private async Task AddComponentsToSolution(bool withSelect, string solutionUniqueName, IEnumerable<SolutionComponent> solutionComponents, SolutionComponent.Schema.OptionSets.rootcomponentbehavior behavior)
        {
            if (!solutionComponents.Any())
            {
                return;
            }

            foreach (var item in solutionComponents)
            {
                item.RootComponentBehaviorEnum = behavior;
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

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            {
                var enabledAdd = this._solution != null && !this._solution.IsManaged.GetValueOrDefault();

                ActivateControls(items, enabledAdd, "contMnAddToCurrentSolution");
            }

            var withBehaviour = lstVSolutionComponents.SelectedItems.OfType<SolutionComponentViewItem>().Any(en => SolutionComponent.IsComponentTypeWithBehaviour(en.SolutionComponent.ComponentType?.Value));

            ActivateControls(items, !withBehaviour, "contMnAddToSolution", "contMnAddToSolutionLast");
            ActivateControls(items, withBehaviour, "contMnAddToSolutionWithBehaviour", "contMnAddToSolutionWithBehaviourLast");

            if (withBehaviour)
            {
                FillLastSolutionItems(_service.ConnectionData, items, withBehaviour, AddToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddToSolutionWithBehaviourLastIncludeSubcomponents");

                FillLastSolutionItems(_service.ConnectionData, items, withBehaviour, AddToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddToSolutionWithBehaviourLastDoNotIncludeSubcomponents");

                FillLastSolutionItems(_service.ConnectionData, items, withBehaviour, AddToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddToSolutionWithBehaviourLastIncludeAsShellOnly");

                ActivateControls(items, withBehaviour && _service.ConnectionData.LastSelectedSolutionsUniqueName != null && _service.ConnectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddToSolutionWithBehaviour");
            }
            else
            {
                FillLastSolutionItems(_service.ConnectionData, items, true, AddToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddToSolutionLast");
            }

            var selectedSolutionComponent = GetSelectedSolutionComponent();

            var hasExplorer = false;

            if (selectedSolutionComponent != null)
            {
                hasExplorer = WindowHelper.HasExplorer(selectedSolutionComponent.SolutionComponent.ComponentType?.Value);

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

        #region Linked Solution Components

        private void FillLinkedSolutionComponentActions(ItemCollection itemCollection, SolutionComponent solutionComponent)
        {
            {
                MenuItem mILinkedComponentOpenInWeb = new MenuItem()
                {
                    Header = "Open in Browser",
                    Tag = solutionComponent,
                };
                mILinkedComponentOpenInWeb.Click += MILinkedComponentOpenInWeb_Click;

                itemCollection.Add(mILinkedComponentOpenInWeb);
            }

            if (solutionComponent.ComponentType?.Value == (int)ComponentType.Entity)
            {
                MenuItem mILinkedComponentOpenEntityListInWeb = new MenuItem()
                {
                    Header = "Open Entity List in Browser",
                    Tag = solutionComponent,
                };
                mILinkedComponentOpenEntityListInWeb.Click += mILinkedComponentOpenEntityListInWeb_Click;

                itemCollection.Add(new Separator());
                itemCollection.Add(mILinkedComponentOpenEntityListInWeb);
            }

            if (WindowHelper.HasExplorer(solutionComponent.ComponentType?.Value))
            {
                MenuItem mILinkedComponentOpenExplorer = new MenuItem()
                {
                    Header = "Open Explorer",
                    Tag = solutionComponent,
                };
                mILinkedComponentOpenExplorer.Click += MILinkedComponentOpenExplorer_Click;

                itemCollection.Add(new Separator());
                itemCollection.Add(mILinkedComponentOpenExplorer);
            }

            bool withBehaviour = SolutionComponent.IsComponentTypeWithBehaviour(solutionComponent.ComponentType?.Value);

            if (this._solution != null && !this._solution.IsManaged.GetValueOrDefault())
            {
                itemCollection.Add(new Separator());

                MenuItem mILinkedComponentAddToCurrentSolution = new MenuItem()
                {
                    Header = "Add to Current Solution",
                    Tag = solutionComponent,
                };
                mILinkedComponentAddToCurrentSolution.Click += MILinkedComponentAddToCurrentSolution_Click;

                itemCollection.Add(mILinkedComponentAddToCurrentSolution);
            }

            if (withBehaviour)
            {
                itemCollection.Add(new Separator());

                if (_service.ConnectionData.LastSelectedSolutionsUniqueName != null && _service.ConnectionData.LastSelectedSolutionsUniqueName.Any())
                {
                    MenuItem mILinkedComponentAddToSolutionLast = new MenuItem()
                    {
                        Header = "Add to Last Crm Solution",
                        Tag = solutionComponent,
                    };

                    MenuItem mILinkedComponentAddToSolutionIncludeSubcomponentsLast = new MenuItem()
                    {
                        Header = Helpers.EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0),
                        Tag = solutionComponent,
                        Uid = "mILinkedComponentAddToSolutionIncludeSubcomponentsLast",
                    };

                    MenuItem mILinkedComponentAddToSolutionDoNotIncludeSubcomponentsLast = new MenuItem()
                    {
                        Header = Helpers.EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1),
                        Tag = solutionComponent,
                        Uid = "mILinkedComponentAddToSolutionDoNotIncludeSubcomponentsLast",
                    };

                    MenuItem mILinkedComponentAddToSolutionIncludeAsShellOnlyLast = new MenuItem()
                    {
                        Header = Helpers.EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2),
                        Tag = solutionComponent,
                        Uid = "mILinkedComponentAddToSolutionIncludeAsShellOnlyLast",
                    };

                    mILinkedComponentAddToSolutionLast.Items.Add(mILinkedComponentAddToSolutionIncludeSubcomponentsLast);
                    mILinkedComponentAddToSolutionLast.Items.Add(new Separator());
                    mILinkedComponentAddToSolutionLast.Items.Add(mILinkedComponentAddToSolutionDoNotIncludeSubcomponentsLast);
                    mILinkedComponentAddToSolutionLast.Items.Add(new Separator());
                    mILinkedComponentAddToSolutionLast.Items.Add(mILinkedComponentAddToSolutionIncludeAsShellOnlyLast);

                    FillLastSolutionItems(_service.ConnectionData, new[] { mILinkedComponentAddToSolutionIncludeSubcomponentsLast }, true, mILinkedComponentAddToSolutionIncludeSubcomponentsLast_Click, "mILinkedComponentAddToSolutionIncludeSubcomponentsLast");
                    FillLastSolutionItems(_service.ConnectionData, new[] { mILinkedComponentAddToSolutionDoNotIncludeSubcomponentsLast }, true, mILinkedComponentAddToSolutionDoNotIncludeSubcomponentsLast_Click, "mILinkedComponentAddToSolutionDoNotIncludeSubcomponentsLast");
                    FillLastSolutionItems(_service.ConnectionData, new[] { mILinkedComponentAddToSolutionIncludeAsShellOnlyLast }, true, mILinkedComponentAddToSolutionIncludeAsShellOnlyLast_Click, "mILinkedComponentAddToSolutionIncludeAsShellOnlyLast");

                    itemCollection.Add(mILinkedComponentAddToSolutionLast);
                }

                {
                    MenuItem mILinkedComponentAddToSolution = new MenuItem()
                    {
                        Header = "Add to Crm Solution",
                    };

                    MenuItem mILinkedComponentAddToSolutionIncludeSubcomponents = new MenuItem()
                    {
                        Header = Helpers.EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0),
                        Tag = solutionComponent,
                    };
                    mILinkedComponentAddToSolutionIncludeSubcomponents.Click += mILinkedComponentAddToSolutionIncludeSubcomponents_Click;

                    MenuItem mILinkedComponentAddToSolutionDoNotIncludeSubcomponents = new MenuItem()
                    {
                        Header = Helpers.EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1),
                        Tag = solutionComponent,
                    };
                    mILinkedComponentAddToSolutionDoNotIncludeSubcomponents.Click += mILinkedComponentAddToSolutionDoNotIncludeSubcomponents_Click;

                    MenuItem mILinkedComponentAddToSolutionIncludeAsShellOnly = new MenuItem()
                    {
                        Header = Helpers.EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2),
                        Tag = solutionComponent,
                    };
                    mILinkedComponentAddToSolutionIncludeAsShellOnly.Click += mILinkedComponentAddToSolutionIncludeAsShellOnly_Click;

                    mILinkedComponentAddToSolution.Items.Add(mILinkedComponentAddToSolutionIncludeSubcomponents);
                    mILinkedComponentAddToSolution.Items.Add(new Separator());
                    mILinkedComponentAddToSolution.Items.Add(mILinkedComponentAddToSolutionDoNotIncludeSubcomponents);
                    mILinkedComponentAddToSolution.Items.Add(new Separator());
                    mILinkedComponentAddToSolution.Items.Add(mILinkedComponentAddToSolutionIncludeAsShellOnly);

                    itemCollection.Add(mILinkedComponentAddToSolution);
                }
            }
            else
            {
                itemCollection.Add(new Separator());

                if (_service.ConnectionData.LastSelectedSolutionsUniqueName != null && _service.ConnectionData.LastSelectedSolutionsUniqueName.Any())
                {
                    MenuItem mILinkedComponentAddToSolutionLast = new MenuItem()
                    {
                        Header = "Add to Last Crm Solution",
                        Tag = solutionComponent,
                        Uid = "mILinkedComponentAddToSolutionLast",
                    };

                    FillLastSolutionItems(_service.ConnectionData, new[] { mILinkedComponentAddToSolutionLast }, true, mILinkedComponentAddToSolutionIncludeSubcomponentsLast_Click, "mILinkedComponentAddToSolutionLast");

                    itemCollection.Add(mILinkedComponentAddToSolutionLast);
                }

                MenuItem mILinkedComponentAddToSolution = new MenuItem()
                {
                    Header = "Add to Crm Solution",
                    Tag = solutionComponent,
                };
                mILinkedComponentAddToSolution.Click += mILinkedComponentAddToSolutionIncludeSubcomponents_Click;
                itemCollection.Add(mILinkedComponentAddToSolution);
            }

            itemCollection.Add(new Separator());

            {
                MenuItem mILinkedComponentOpenSolutionsContainingComponentInExplorer = new MenuItem()
                {
                    Header = "Open Solutions Containing Component in Explorer",
                    Tag = solutionComponent,
                };
                mILinkedComponentOpenSolutionsContainingComponentInExplorer.Click += MILinkedComponentOpenSolutionsContainingComponentInExplorer_Click;

                itemCollection.Add(mILinkedComponentOpenSolutionsContainingComponentInExplorer);
            }

            itemCollection.Add(new Separator());

            {
                MenuItem mILinkedComponentOpenDependentComponentsInWeb = new MenuItem()
                {
                    Header = "Open Dependent Components in Browser",
                    Tag = solutionComponent,
                };
                mILinkedComponentOpenDependentComponentsInWeb.Click += MILinkedComponentOpenDependentComponentsInWeb_Click;

                itemCollection.Add(mILinkedComponentOpenDependentComponentsInWeb);
            }

            {
                MenuItem mILinkedComponentOpenDependentComponentsInExplorer = new MenuItem()
                {
                    Header = "Open Dependent Components in Explorer",
                    Tag = solutionComponent,
                };
                mILinkedComponentOpenDependentComponentsInExplorer.Click += MILinkedComponentOpenDependentComponentsInExplorer_Click;

                itemCollection.Add(mILinkedComponentOpenDependentComponentsInExplorer);
            }
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

        private void mILinkedComponentOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
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

            if (!WindowHelper.HasExplorer(solutionComponent.ComponentType?.Value))
            {
                return;
            }

            var componentType = (ComponentType)solutionComponent.ComponentType.Value;

            WindowHelper.OpenComponentExplorer(_iWriteToOutput, _service, _commonConfig, _descriptor, componentType, solutionComponent.ObjectId.Value);
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

            await AddComponentsToSolution(false, _solution.UniqueName, new[] { solutionComponent }, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void mILinkedComponentAddToSolutionIncludeSubcomponentsLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                && menuItem.Parent is MenuItem menuItemParent
                && menuItemParent.Tag != null
                && menuItemParent.Tag is SolutionComponent solutionComponent
            )
            {
                await AddComponentsToSolution(false, solutionUniqueName, new[] { solutionComponent }, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
            }
        }

        private async void mILinkedComponentAddToSolutionDoNotIncludeSubcomponentsLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                && menuItem.Parent is MenuItem menuItemParent
                && menuItemParent.Tag != null
                && menuItemParent.Tag is SolutionComponent solutionComponent
            )
            {
                await AddComponentsToSolution(false, solutionUniqueName, new[] { solutionComponent }, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
            }
        }

        private async void mILinkedComponentAddToSolutionIncludeAsShellOnlyLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                && menuItem.Parent is MenuItem menuItemParent
                && menuItemParent.Tag != null
                && menuItemParent.Tag is SolutionComponent solutionComponent
            )
            {
                await AddComponentsToSolution(false, solutionUniqueName, new[] { solutionComponent }, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
            }
        }

        private async void mILinkedComponentAddToSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            await AddComponentsToSolution(true, null, new[] { solutionComponent }, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void mILinkedComponentAddToSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            await AddComponentsToSolution(true, null, new[] { solutionComponent }, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
        }

        private async void mILinkedComponentAddToSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            await AddComponentsToSolution(true, null, new[] { solutionComponent }, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
        }

        private void MILinkedComponentOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerSolutionExplorer(
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

        private void MILinkedComponentOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , _service
                , _descriptor
                , _commonConfig
                , solutionComponent.ComponentType.Value
                , solutionComponent.ObjectId.Value
                , null
            );
        }

        #endregion Linked Solution Components

        private async void cmBComponentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            FillDataGridColumns();

            await ShowExistingComponents();
        }

        private async void cmBSolutionComponentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            await ShowExistingComponents();
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

        private void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , _service
                , _descriptor
                , _commonConfig
                , entity.SolutionComponent.ComponentType.Value
                , entity.SolutionComponent.ObjectId.Value
                , null
            );
        }

        private void mIOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedSolutionComponent();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerSolutionExplorer(
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

        private void miSelectAsLastSelected_Click(object sender, RoutedEventArgs e)
        {
            if (_solution.IsManaged.GetValueOrDefault())
            {
                return;
            }

            _service.ConnectionData.AddLastSelectedSolution(_solution.UniqueName);

            _iWriteToOutput.WriteToOutputSolutionUri(_service.ConnectionData, _solution.UniqueName, _solution.Id);
        }

        private void ExecuteActionOnSingleSolution(Func<string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (_solution == null)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            try
            {
                action(_commonConfig.FolderForExport);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private void mICreateSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(PerformCreateSolutionImage);
            }
        }

        private void mICreateSolutionImageAndOpenOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(PerformCreateSolutionImageAndOpenOrganizationComparer);
            }
        }

        private void mIComponentsIn_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(PerformCreateFileWithSolutionComponents);
            }
        }

        private async Task PerformCreateSolutionImage(string folder)
        {
            try
            {
                var list = _itemsSource.Select(e => e.SolutionComponent);

                if (!list.Any())
                {
                    return;
                }

                ToggleControls(false, Properties.OutputStrings.CreatingFileWithSolutionImageFormat1, _solution.UniqueName);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, _descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    _service.ConnectionData.Name
                    , _solution.UniqueName
                    , $"{_header}_SolutionImage"
                    , FileExtension.xml
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageWithComponentsAsync($"{_solution.UniqueName}_{_header}", list);

                await solutionImage.SaveAsync(filePath);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.InConnectionExportedSolutionImageFormat2, _service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.OutputStrings.CreatingFileWithSolutionImageCompletedFormat1, _solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.CreatingFileWithSolutionImageFailedFormat1, _solution.UniqueName);
            }
        }

        private async Task PerformCreateSolutionImageAndOpenOrganizationComparer(string folder)
        {
            try
            {
                var list = _itemsSource.Select(e => e.SolutionComponent);

                if (!list.Any())
                {
                    return;
                }

                ToggleControls(false, Properties.OutputStrings.CreatingFileWithSolutionImageFormat1, _solution.UniqueName);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, _descriptor);

                SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageWithComponentsAsync($"{_solution.UniqueName}_{_header}", list);

                _commonConfig.Save();

                WindowHelper.OpenOrganizationComparerWindow(_iWriteToOutput, _service.ConnectionData.ConnectionConfiguration, _commonConfig, solutionImage);

                ToggleControls(true, Properties.OutputStrings.CreatingFileWithSolutionImageCompletedFormat1, _solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.CreatingFileWithSolutionImageFailedFormat1, _solution.UniqueName);
            }
        }

        private async Task PerformCreateFileWithSolutionComponents(string folder)
        {
            try
            {
                var list = _itemsSource.Select(e => e.SolutionComponent);

                if (!list.Any())
                {
                    return;
                }

                ToggleControls(false, Properties.OutputStrings.CreatingTextFileWithMissingDependenciesFormat1, _solution.UniqueName);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, _descriptor);

                string fileName = EntityFileNameFormatter.GetSolutionFileName(
                    _service.ConnectionData.Name
                    , _solution.UniqueName
                    , _header
                    , FileExtension.txt
                );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageWithComponentsAsync($"{_solution.UniqueName}_{_header}", list);

                await solutionImage.SaveAsync(filePath);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Solution All Missing Dependencies was export into file '{0}'", filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.OutputStrings.CreatingTextFileWithMissingDependenciesCompletedFormat1, _solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.CreatingTextFileWithMissingDependenciesFailedFormat1, _solution.UniqueName);
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

            if (!WindowHelper.HasExplorer(entity.SolutionComponent.ComponentType?.Value))
            {
                return;
            }

            var componentType = (ComponentType)entity.SolutionComponent.ComponentType.Value;

            WindowHelper.OpenComponentExplorer(_iWriteToOutput, _service, _commonConfig, _descriptor, componentType, entity.SolutionComponent.ObjectId.Value);
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

            WindowHelper.OpenExplorerSolutionExplorer(
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

        #region Clipboard

        private void mIClipboardCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionComponentViewItem>(e, ent => ent.Name);
        }

        private void mIClipboardCopyDisplayName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionComponentViewItem>(e, ent => ent.DisplayName);
        }

        private void mIClipboardCopyObjectId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionComponentViewItem>(e, ent => ent.SolutionComponent.ObjectId.ToString());
        }

        private void mIClipboardCopyComponentTypeCode_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionComponentViewItem>(e, ent => ent.SolutionComponent.ComponentType?.Value.ToString());
        }

        private void mIClipboardCopyComponentTypeName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionComponentViewItem>(e, ent => ent.ComponentType);
        }

        private void mIClipboardCopyEntityId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionComponentViewItem>(e, ent => ent.SolutionComponent.Id.ToString());
        }

        #endregion Clipboard
    }
}