using Microsoft.Xrm.Sdk;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerSolutionComponentDependencies : WindowBase
    {
        private readonly IWriteToOutput _iWriteToOutput;

        private IOrganizationServiceExtented _service;

        private readonly SolutionComponentDescriptor _descriptor;
        private SolutionComponentDescriptor GetDescriptor()
        {
            _descriptor.SetSettings(_commonConfig);
            return _descriptor;
        }

        private readonly SolutionComponentConverter _converter;

        private readonly CommonConfiguration _commonConfig;

        private readonly Popup _optionsPopup;

        public static readonly XmlOptionsControls _xmlOptions = XmlOptionsControls.SolutionComponentSettings;

        private readonly int _componentType;
        private readonly Guid _objectId;

        private readonly ObservableCollection<SolutionComponentViewItem> _itemsSource;

        public WindowExplorerSolutionComponentDependencies(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , int componentType
            , Guid objectId
            , string selection
        )
        {
            IncreaseInit();

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._commonConfig = commonConfig;
            this._componentType = componentType;
            this._objectId = objectId;
            this._descriptor = descriptor;

            if (this._descriptor == null)
            {
                this._descriptor = new SolutionComponentDescriptor(_service);
                this._descriptor.SetSettings(_commonConfig);
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

            txtBComponentDescription.Text = _descriptor.GetComponentDescription(componentType, objectId);

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
                ShowExistingSolutionComponents();
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
                        cmBComponentType.SelectedItem = item;

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

        private enum DependencyType
        {
            DependentComponents = 0,
            RequiredComponents = 1,
            DependenciesForDelete = 2,
        }

        private DependencyType GetDependencyType()
        {
            var result = DependencyType.DependentComponents;

            cmBDependencyType.Dispatcher.Invoke(() =>
            {
                if (cmBDependencyType.SelectedIndex != -1)
                {
                    result = (DependencyType)cmBDependencyType.SelectedIndex;
                }
            });

            return result;
        }

        private async Task ShowExistingSolutionComponents()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this._itemsSource.Clear();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            int? category = null;

            cmBComponentType.Dispatcher.Invoke(() =>
            {
                if (cmBComponentType.SelectedItem is ComponentType selected)
                {
                    category = (int)selected;
                }
            });

            var solutionComponents = GetDependencyType();

            var list = new List<SolutionComponent>();

            string formatResult = Properties.WindowStatusStrings.LoadingRequiredComponentsCompletedFormat1;

            try
            {
                var repository = new DependencyRepository(this._service);

                switch (solutionComponents)
                {
                    case DependencyType.RequiredComponents:
                    default:
                        {
                            ToggleControls(false, Properties.WindowStatusStrings.LoadingRequiredComponents);
                            formatResult = Properties.WindowStatusStrings.LoadingRequiredComponentsCompletedFormat1;

                            IEnumerable<Dependency> temp = await repository.GetRequiredComponentsAsync(_componentType, _objectId);

                            if (category.HasValue)
                            {
                                list.AddRange(temp.Select(en => en.RequiredToSolutionComponent()).Where(en => en.ComponentType?.Value == category.Value));
                            }
                            else
                            {
                                list.AddRange(temp.Select(en => en.RequiredToSolutionComponent()));
                            }
                        }
                        break;

                    case DependencyType.DependentComponents:
                        {
                            ToggleControls(false, Properties.WindowStatusStrings.LoadingDependentComponents);
                            formatResult = Properties.WindowStatusStrings.LoadingDependentComponentsCompletedFormat1;

                            IEnumerable<Dependency> temp = await repository.GetDependentComponentsAsync(_componentType, _objectId);

                            if (category.HasValue)
                            {
                                list.AddRange(temp.Select(en => en.DependentToSolutionComponent()).Where(en => en.ComponentType?.Value == category.Value));
                            }
                            else
                            {
                                list.AddRange(temp.Select(en => en.DependentToSolutionComponent()));
                            }
                        }
                        break;

                    case DependencyType.DependenciesForDelete:
                        {
                            ToggleControls(false, Properties.WindowStatusStrings.LoadingDependenciesForDelete);
                            formatResult = Properties.WindowStatusStrings.LoadingDependenciesForDeleteCompletedFormat1;

                            IEnumerable<Dependency> temp = await repository.GetDependenciesForDeleteAsync(_componentType, _objectId);

                            if (category.HasValue)
                            {
                                list.AddRange(temp.Select(en => en.DependentToSolutionComponent()).Where(en => en.ComponentType?.Value == category.Value));
                            }
                            else
                            {
                                list.AddRange(temp.Select(en => en.DependentToSolutionComponent()));
                            }
                        }
                        break;
                }

                await GetDescriptor().GetSolutionComponentsDescriptionAsync(list);
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

                var item = new SolutionComponentViewItem(entity, name, displayName, entity.ComponentTypeName, managed, customizable);

                convertedList.Add(item);
            }

            var enumerable = convertedList.AsEnumerable();

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

            ToggleControl(this.tSProgressBar, this.btnExportAll, this.tSDDBExportSolutionComponent, this.cmBComponentType, this.cmBDependencyType);

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

        private SolutionComponentViewItem GetSelectedEntity()
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
                var item = ((FrameworkElement)e.OriginalSource).DataContext as SolutionComponentViewItem;

                if (item != null)
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
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            await action(folder, item);
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

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
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformExportEntityDescription);
        }

        private async Task PerformExportEntityDescription(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(_service.ConnectionData, Properties.OperationNames.CreatingEntityDescriptionFormat1, _service.ConnectionData.Name);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityDescription);

            string fileName = GetDescriptor().GetFileName(_service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, "EntityDescription", "txt");

            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var stringBuilder = new StringBuilder();

            var desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(solutionComponentViewItem.SolutionComponent, null, _service.ConnectionData);

            if (!string.IsNullOrEmpty(desc))
            {
                if (stringBuilder.Length > 0) { stringBuilder.AppendLine().AppendLine().AppendLine().AppendLine(); }

                stringBuilder.AppendLine(desc);
            }

            var entity = GetDescriptor().GetEntity<Entity>(solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

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

            this._iWriteToOutput.WriteToOutputEndOperation(_service.ConnectionData, Properties.OperationNames.CreatingEntityDescriptionFormat1, _service.ConnectionData.Name);
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

        private void mIExportDependentComponents_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformCreatingFileWithDependentComponents);
        }

        private void mIExportRequiredComponents_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformCreatingFileWithRequiredComponents);
        }

        private void mIExportDependenciesForDelete_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformCreatingFileWithDependenciesForDelete);
        }

        private async Task PerformCreatingFileWithDependentComponents(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Starting downloading {0} {1}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);

            var dependencyRepository = new DependencyRepository(_service);

            var descriptorHandler = new DependencyDescriptionHandler(GetDescriptor());

            var coll = await dependencyRepository.GetDependentComponentsAsync(solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

            string description = await descriptorHandler.GetDescriptionDependentAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                var fileName = GetDescriptor().GetFileName(this._service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, "Dependent Components", "txt");

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "{0} {1} Dependent Components exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "{0} {1} has no Dependent Components.", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);
                this._iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
            }
        }

        private async Task PerformCreatingFileWithRequiredComponents(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Starting downloading {0} {1}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);

            var dependencyRepository = new DependencyRepository(_service);

            var descriptorHandler = new DependencyDescriptionHandler(GetDescriptor());

            var coll = await dependencyRepository.GetRequiredComponentsAsync(solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

            string description = await descriptorHandler.GetDescriptionRequiredAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                var fileName = GetDescriptor().GetFileName(this._service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, "Required Components", "txt");

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "{0} {1} Required Components exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "{0} {1} has no Required Components.", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);
                this._iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
            }
        }

        private async Task PerformCreatingFileWithDependenciesForDelete(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Starting downloading {0} {1}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);

            var dependencyRepository = new DependencyRepository(_service);

            var descriptorHandler = new DependencyDescriptionHandler(GetDescriptor());

            var coll = await dependencyRepository.GetDependenciesForDeleteAsync(solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

            string description = await descriptorHandler.GetDescriptionDependentAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                var fileName = GetDescriptor().GetFileName(this._service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, "Dependencies For Delete", "txt");

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "{0} {1} Dependencies For Delete exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "{0} {1} has no Dependencies For Delete.", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);
                this._iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
            }
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            this._service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb((ComponentType)entity.SolutionComponent.ComponentType.Value, entity.SolutionComponent.ObjectId.Value);
        }

        private void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _service.UrlGenerator.OpenSolutionComponentInWeb((ComponentType)entity.SolutionComponent.ComponentType.Value, entity.SolutionComponent.ObjectId.Value);
        }

        private async void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddIntoSolution(true, null);
        }

        private async void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var solutionComponents = lstVSolutionComponents.SelectedItems.OfType<SolutionComponentViewItem>().Select(en => en.SolutionComponent).ToList();

            if (!solutionComponents.Any())
            {
                return;
            }

            await AddComponentsIntoSolution(withSelect, solutionUniqueName, solutionComponents);
        }

        private async Task AddComponentsIntoSolution(bool withSelect, string solutionUniqueName, IEnumerable<SolutionComponent> solutionComponents)
        {
            if (!solutionComponents.Any())
            {
                return;
            }

            _commonConfig.Save();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);

                await SolutionController.AddSolutionComponentsCollectionIntoSolution(_iWriteToOutput, _service, GetDescriptor(), _commonConfig, solutionUniqueName, solutionComponents, withSelect);
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

            FillLastSolutionItems(_service.ConnectionData, items, true, AddIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");

            var entity = GetSelectedEntity();

            var hasExplorer = false;

            if (entity != null)
            {
                hasExplorer = HasExplorer(entity.SolutionComponent.ComponentType?.Value);

                if (hasExplorer)
                {
                    var componentType = (ComponentType)entity.SolutionComponent.ComponentType.Value;

                    string componentName = string.Format("{0} Explorer", componentType.ToString());

                    SetControlsName(items, componentName, "contMnExplorer");
                }
            }

            ActivateControls(items, hasExplorer, "contMnExplorer");

            var menuItemLinkedComponent = items.OfType<MenuItem>().FirstOrDefault(i => string.Equals(i.Uid, "contMnLinkedComponents", StringComparison.InvariantCultureIgnoreCase));

            if (menuItemLinkedComponent != null)
            {
                menuItemLinkedComponent.Items.Clear();

                if (entity != null)
                {
                    var linkedComponents = _descriptor.GetLinkedComponents(entity.SolutionComponent);

                    if (linkedComponents != null && linkedComponents.Any())
                    {
                        var componentsWithNames = linkedComponents.Select(c => new { Component = c, Name = _descriptor.GetName(c) }).OrderBy(c => c.Component.ComponentType?.Value).ThenBy(c => c.Name);

                        foreach (var item in componentsWithNames)
                        {
                            var menuItem = new MenuItem()
                            {
                                Header = string.Format("{0} - {1}", item.Component.ComponentTypeName, item.Name).Replace("_", "__"),
                            };

                            FillComponentActions(menuItem.Items, item.Component);

                            menuItemLinkedComponent.Items.Add(menuItem);
                        }
                    }
                }

                ActivateControls(items, menuItemLinkedComponent.Items.Count > 0, "contMnLinkedComponents");
            }
        }

        private void FillComponentActions(ItemCollection itemCollection, SolutionComponent solutionComponent)
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

            MenuItem mILinkedComponentAddIntoSolutionLast = new MenuItem()
            {
                Header = "Add into Last Crm Solution",
                Tag = solutionComponent,
                Uid = "mILinkedComponentAddIntoSolutionLast",
            };

            FillLastSolutionItems(_service.ConnectionData, new[] { mILinkedComponentAddIntoSolutionLast }, true, MILinkedComponentAddIntoSolutionLast_Click, "mILinkedComponentAddIntoSolutionLast");

            MenuItem mILinkedComponentAddIntoSolution = new MenuItem()
            {
                Header = "Add into Crm Solution",
                Tag = solutionComponent,
            };
            mILinkedComponentAddIntoSolution.Click += MILinkedComponentAddIntoSolution_Click;

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

            itemCollection.Add(mILinkedComponentAddIntoSolutionLast);
            itemCollection.Add(mILinkedComponentAddIntoSolution);

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

        private async void MILinkedComponentAddIntoSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                && menuItem.Parent is MenuItem menuItemParent
                && menuItemParent.Tag != null
                && menuItemParent.Tag is SolutionComponent solutionComponent
            )
            {
                await AddComponentsIntoSolution(false, solutionUniqueName, new[] { solutionComponent });
            }
        }

        private async void MILinkedComponentAddIntoSolution_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is SolutionComponent solutionComponent)
            )
            {
                return;
            }

            await AddComponentsIntoSolution(true, null, new[] { solutionComponent });
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
                , GetDescriptor()
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

        private void cmBDependencyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                if (cmBComponentType.SelectedItem is ComponentType selected)
                {
                    category = (int)selected;
                }
            });

            TupleList<string, string> columnsForComponentType = null;

            if (category.HasValue)
            {
                columnsForComponentType = GetDescriptor().GetComponentColumns(category.Value);
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

                string[] columns = { "Name", "DisplayName", "ComponentType", "IsManaged", "IsCustomizable" };

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
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenSolutionComponentDependenciesWindow(
                _iWriteToOutput
                , _service
                , GetDescriptor()
                , _commonConfig
                , entity.SolutionComponent.ComponentType.Value
                , entity.SolutionComponent.ObjectId.Value
                , null);
        }

        private void mIOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

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

        private void mIOpenExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

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
                var relation = GetDescriptor().MetadataSource.GetRelationshipMetadata(entity.SolutionComponent.ObjectId.Value);

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

        private async void btnOpenInWebDefaultSolution_Click(object sender, RoutedEventArgs e)
        {
            var repository = new SolutionRepository(_service);

            var solution = await repository.GetSolutionByUniqueNameAsync(Solution.InstancesUniqueNames.Default);

            if (solution != null)
            {
                _service.ConnectionData.OpenSolutionInWeb(solution.Id);
            }
        }
    }
}