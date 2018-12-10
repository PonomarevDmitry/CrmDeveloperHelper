using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        private Solution _solution;

        private bool _controlsEnabled = true;

        private ObservableCollection<SolutionComponentViewItem> _itemsSource;

        private int _init = 0;

        public WindowExplorerSolutionComponents(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , string selection
            )
        {
            BeginLoadConfig();

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._commonConfig = commonConfig;
            this._descriptor = descriptor;

            this.Title = string.Format("{0} Solution Components", solutionUniqueName);

            if (this._descriptor == null)
            {
                this._descriptor = new SolutionComponentDescriptor(_service, true);
            }

            this._converter = new SolutionComponentConverter(this._descriptor);

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

            EndLoadConfig();

            if (_service != null)
            {
                ShowExistingSolutionComponents(solutionUniqueName);
            }
        }

        private void BeginLoadConfig()
        {
            ++_init;
        }

        private void EndLoadConfig()
        {
            --_init;
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
            if (_init > 0 || !_controlsEnabled)
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

                    this.Dispatcher.Invoke(() =>
                    {
                        miClearUnManagedSolution.IsEnabled = sepClearUnManagedSolution.IsEnabled = !isManaged;
                        miClearUnManagedSolution.Visibility = sepClearUnManagedSolution.Visibility = !isManaged ? Visibility.Visible : Visibility.Collapsed;

                        miSelectAsLastSelected.IsEnabled = sepClearUnManagedSolution2.IsEnabled = !isManaged;
                        miSelectAsLastSelected.Visibility = sepClearUnManagedSolution2.Visibility = !isManaged ? Visibility.Visible : Visibility.Collapsed;
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
                this._iWriteToOutput.WriteErrorToOutput(ex);
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

            ToggleControl(this.btnExportAll, enabled);
            ToggleControl(this.mISolutionInformation, enabled);
            ToggleControl(this.tSDDBExportSolutionComponent, enabled);

            ToggleControl(this.cmBComponentType, enabled);
            ToggleControl(this.cmBSolutionComponentsType, enabled);

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

            if (_init > 0 || !_controlsEnabled)
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
            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityDescription);

            string fileName = _descriptor.GetFileName(_service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, "EntityDescription", "txt");

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

            this._iWriteToOutput.WriteToOutput("{0} {1} Entity Description exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

            this._iWriteToOutput.PerformAction(filePath);

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
            this._iWriteToOutput.WriteToOutput("Starting downloading {0} {1}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);

            var dependencyRepository = new DependencyRepository(_service);

            var descriptorHandler = new DependencyDescriptionHandler(_descriptor);

            var coll = await dependencyRepository.GetDependentComponentsAsync(solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

            string description = await descriptorHandler.GetDescriptionDependentAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                var fileName = _descriptor.GetFileName(this._service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, "Dependent Components", "txt");

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("{0} {1} Dependent Components exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("{0} {1} has no Dependent Components.", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        private async Task PerformCreatingFileWithRequiredComponents(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            this._iWriteToOutput.WriteToOutput("Starting downloading {0} {1}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);

            var dependencyRepository = new DependencyRepository(_service);

            var descriptorHandler = new DependencyDescriptionHandler(_descriptor);

            var coll = await dependencyRepository.GetRequiredComponentsAsync(solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

            string description = await descriptorHandler.GetDescriptionRequiredAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                var fileName = _descriptor.GetFileName(this._service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, "Required Components", "txt");

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("{0} {1} Required Components exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("{0} {1} has no Required Components.", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        private async Task PerformCreatingFileWithDependenciesForDelete(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            this._iWriteToOutput.WriteToOutput("Starting downloading {0} {1}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);

            var dependencyRepository = new DependencyRepository(_service);

            var descriptorHandler = new DependencyDescriptionHandler(_descriptor);

            var coll = await dependencyRepository.GetDependenciesForDeleteAsync(solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

            string description = await descriptorHandler.GetDescriptionDependentAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                var fileName = _descriptor.GetFileName(this._service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, "Dependencies For Delete", "txt");

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("{0} {1} Dependencies For Delete exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("{0} {1} has no Dependencies For Delete.", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name);
                this._iWriteToOutput.ActivateOutputWindow();
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

        private async void AddIntoSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddIntoSolution(true, null);
        }

        private async void AddIntoSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
              && menuItem.Tag != null
              && menuItem.Tag is string solutionUniqueName
              )
            {
                await AddIntoSolution(false, solutionUniqueName);
            }
        }

        private async void AddIntoCurrentSolution_Click(object sender, RoutedEventArgs e)
        {
            if (GetSolutionComponentsType() == SolutionComponentsType.SolutionComponents
                || _solution == null
                || _solution.IsManaged.GetValueOrDefault()
                )
            {
                return;
            }

            await AddIntoSolution(false, _solution.UniqueName);
        }

        private async Task AddIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var solutionComponents = lstVSolutionComponents.SelectedItems.OfType<SolutionComponentViewItem>().Select(en => en.SolutionComponent).ToList();

            if (!solutionComponents.Any())
            {
                return;
            }

            _commonConfig.Save();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow();

                await SolutionController.AddSolutionComponentsCollectionIntoSolution(_iWriteToOutput, _service, _descriptor, _commonConfig, solutionUniqueName, solutionComponents, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        private async void mIAddEntityIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null);
        }

        private async void mIAddEntityIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName);
            }
        }

        private async void mIAddEntityIntoCurrentCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            if (_solution == null
                || _solution.IsManaged.GetValueOrDefault()
                )
            {
                return;
            }

            await AddEntityIntoSolution(false, _solution.UniqueName);
        }

        private async Task AddEntityIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            string entityName = _descriptor.GetLinkedEntityName(entity.SolutionComponent);

            if (string.IsNullOrEmpty(entityName)
                || string.Equals(entityName, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            var entityMetadataId = _service.ConnectionData.GetEntityMetadataId(entityName);

            if (entityMetadataId.HasValue)
            {
                _commonConfig.Save();

                try
                {
                    this._iWriteToOutput.ActivateOutputWindow();

                    await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, _service, _descriptor, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entityMetadataId.Value }, null, withSelect);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
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
                var enabledAdd = GetSolutionComponentsType() != SolutionComponentsType.SolutionComponents && this._solution != null && !this._solution.IsManaged.GetValueOrDefault();
                var enabledRemove = GetSolutionComponentsType() == SolutionComponentsType.SolutionComponents && this._solution != null && !this._solution.IsManaged.GetValueOrDefault();

                ActivateControls(items, enabledRemove, "sepRemoveFromSolution", "contMnRemoveFromSolution");

                ActivateControls(items, enabledAdd, "contMnAddIntoCurrentSolution");
            }

            FillLastSolutionItems(_service.ConnectionData, items, true, AddIntoSolutionLast_Click, "contMnAddIntoSolutionLast");

            bool hasLinkedEntity = false;

            var entity = GetSelectedEntity();

            if (entity != null)
            {
                string entityName = _descriptor.GetLinkedEntityName(entity.SolutionComponent);

                if (!string.IsNullOrEmpty(entityName)
                    && !string.Equals(entityName, "none", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    hasLinkedEntity = true;
                }
            }

            ActivateControls(items, hasLinkedEntity, "contMnAddEntityIntoSolution");
            ActivateControls(items, hasLinkedEntity && this._solution != null && !this._solution.IsManaged.GetValueOrDefault(), "contMnAddEntityIntoCurrentSolution");

            FillLastSolutionItems(_service.ConnectionData, items, hasLinkedEntity, mIAddEntityIntoCrmSolutionLast_Click, "contMnAddEntityIntoSolutionLast");
        }

        private void cmBComponentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            FillDataGridColumns();

            ShowExistingSolutionComponents();
        }

        private void cmBSolutionComponentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0 || !_controlsEnabled)
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
                , _descriptor
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

        private void OpenSolutionInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                _service.ConnectionData.OpenSolutionInWeb(_solution.Id);
            }
        }

        private async void RemoveComponentFromSolution_Click(object sender, RoutedEventArgs e)
        {
            if (GetSolutionComponentsType() != SolutionComponentsType.SolutionComponents)
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
                        , "Components Backup"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, _solution.Id);

                    this._iWriteToOutput.WriteToOutput("Created backup Solution Components in '{0}': {1}", _solution.UniqueName, filePath);
                    this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                }

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        _service.ConnectionData.Name
                        , _solution.UniqueName
                        , "SolutionImage Backup"
                        , "xml"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, _solution.Id);
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
                this._iWriteToOutput.WriteErrorToOutput(ex);

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

                    this._iWriteToOutput.WriteToOutput("Created backup Solution Components in '{0}': {1}", _solution.UniqueName, filePath);
                    this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                }

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        _service.ConnectionData.Name
                        , _solution.UniqueName
                        , "SolutionImage Backup"
                        , "xml"
                    );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, _solution.Id);
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
                this._iWriteToOutput.WriteErrorToOutput(ex);

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

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, _service, _commonConfig, null, null, null);
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
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, _service, _commonConfig, null);
        }

        private void btnEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, _service, _commonConfig, null);
        }

        private void miEntitySecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, _service, _commonConfig, null, null);
        }

        private void btnGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , _service
                , _commonConfig
                , null
                , string.Empty
                , string.Empty
                );
        }

        private void btnSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, _service, _commonConfig, null, string.Empty);
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

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, _service, _commonConfig, null, string.Empty);
        }

        private void btnSiteMap_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenExportSiteMapWindow(this._iWriteToOutput, _service, _commonConfig);
        }

        private void btnWebResources_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenExportWebResourcesWindow(this._iWriteToOutput, _service, _commonConfig, string.Empty);
        }

        private void btnExportReport_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenExportReportWindow(this._iWriteToOutput, _service, _commonConfig, string.Empty);
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
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (solution == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                return;
            }

            if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                return;
            }

            try
            {
                action(_commonConfig.FolderForExport, solution);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
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

        private void mICreateSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            if (_solution != null)
            {
                ExecuteActionOnSingleSolution(_solution, PerformCreateSolutionImage);
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

                await solutionDescriptor.CreateFileWithSolutionImageAsync(filePath, solution.Id);

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedSolutionImageForConnectionFormat2, _service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileWithSolutionImageCompletedFormat1, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

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
                
                ToggleControls(false, Properties.WindowStatusStrings.LoadingComponentsFromSolutionImage);

                SolutionImage solutionImage = null;

                try
                {
                    solutionImage = await SolutionImage.LoadAsync(selectedPath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);

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
                
                UpdateStatus(Properties.WindowStatusStrings.AddingComponentsIntoSolutionFormat3, _service.ConnectionData.Name, solutionComponents.Count, _solution.UniqueName);

                if (solutionComponents.Count == 0)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.NoComponentsToAdd);
                    return;
                }

                _commonConfig.Save();

                this._iWriteToOutput.ActivateOutputWindow();

                await SolutionController.AddSolutionComponentsCollectionIntoSolution(_iWriteToOutput, _service, _descriptor, _commonConfig, _solution.UniqueName, solutionComponents, false);

                ToggleControls(true, Properties.WindowStatusStrings.LoadingComponentsFromSolutionImageCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

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

                ToggleControls(false, Properties.WindowStatusStrings.LoadingComponentsFromZipFile);

                List<SolutionComponent> solutionComponents = await _descriptor.LoadSolutionComponentsFromZipFileAsync(selectedPath);
                
                UpdateStatus(Properties.WindowStatusStrings.LoadedComponentsFromZipFileFormat1, solutionComponents.Count);

                if (solutionComponents.Count == 0)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.NoComponentsToAdd);
                    return;
                }

                UpdateStatus(Properties.WindowStatusStrings.AddingComponentsIntoSolutionFormat3, _service.ConnectionData.Name, solutionComponents.Count, _solution.UniqueName);

                this._iWriteToOutput.ActivateOutputWindow();

                await SolutionController.AddSolutionComponentsCollectionIntoSolution(_iWriteToOutput, _service, _descriptor, _commonConfig, _solution.UniqueName, solutionComponents, false);

                ToggleControls(true, Properties.WindowStatusStrings.LoadingComponentsFromZipFileCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

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

        private void mIOpenSolutionImage_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSolutionImageWindow(this._iWriteToOutput, _service.ConnectionData, _commonConfig);
        }

        private void mIOpenOrganizationDifferenceImage_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenOrganizationDifferenceImageWindow(this._iWriteToOutput, _service.ConnectionData, _commonConfig);
        }

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            string entityName = _descriptor.GetLinkedEntityName(entity.SolutionComponent);

            if (string.IsNullOrEmpty(entityName)
                || string.Equals(entityName, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            var entityMetadataId = _service.ConnectionData.GetEntityMetadataId(entityName);

            if (entityMetadataId.HasValue)
            {
                _service.ConnectionData.OpenEntityMetadataInWeb(entityMetadataId.Value);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            string entityName = _descriptor.GetLinkedEntityName(entity.SolutionComponent);

            if (string.IsNullOrEmpty(entityName)
                || string.Equals(entityName, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            _service.ConnectionData.OpenEntityListInWeb(entityName);
        }

        private void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            string entityName = _descriptor.GetLinkedEntityName(entity.SolutionComponent);

            if (string.IsNullOrEmpty(entityName)
                || string.Equals(entityName, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ExecuteAction(entity, PublishEntityAsync);
        }

        private async Task PublishEntityAsync(string folder, SolutionComponentViewItem solutionComponentViewItem)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            string entityName = _descriptor.GetLinkedEntityName(solutionComponentViewItem.SolutionComponent);

            if (string.IsNullOrEmpty(entityName)
                || string.Equals(entityName, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.PublishingEntitiesFormat2, _service.ConnectionData.Name, entityName);

            ToggleControls(false, Properties.WindowStatusStrings.PublishingEntitiesFormat2, _service.ConnectionData.Name, entityName);

            try
            {
                var repository = new PublishActionsRepository(_service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat2, _service.ConnectionData.Name, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat2, _service.ConnectionData.Name, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.PublishingEntitiesFormat2, _service.ConnectionData.Name, entityName);
        }
    }
}