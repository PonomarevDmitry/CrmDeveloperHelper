using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
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
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerSolutionComponentDependencies : WindowBase
    {
        private IWriteToOutput _iWriteToOutput;

        private IOrganizationServiceExtented _service;

        private readonly SolutionComponentDescriptor _descriptor;

        private readonly SolutionComponentConverter _converter;

        private CommonConfiguration _commonConfig;

        private readonly int _componentType;
        private readonly Guid _objectId;

        private bool _controlsEnabled = true;

        private ObservableCollection<SolutionComponentViewItem> _itemsSource;

        private int _init = 0;

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
            BeginLoadConfig();

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
                this._descriptor = new SolutionComponentDescriptor(_iWriteToOutput, _service, true);
            }

            this._converter = new SolutionComponentConverter(this._descriptor);

            this.tSSLblConnectionName.Content = _service.ConnectionData.Name;

            txtBComponentDescription.Text = _descriptor.GetComponentDescription(componentType, objectId);

            FillComboBoxComponentType();

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
                ShowExistingSolutionComponents();
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

        private void FillComboBoxComponentType()
        {
            cmBComponentType.Items.Clear();

            cmBComponentType.Items.Add(string.Empty);

            var listComponentType = Enum.GetValues(typeof(ComponentType)).OfType<ComponentType>().ToList();

            foreach (var item in listComponentType.OrderBy(o => o.ToString()))
            {
                cmBComponentType.Items.Add(item);
            }

            FillDataGridColumns();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        private const string paramComponentType = "ComponentType";
        private const string paramDependencyType = "DependencyType";

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

            {
                var categoryValue = winConfig.GetValueInt(paramDependencyType);

                if (categoryValue.HasValue && 0 <= categoryValue && categoryValue < cmBDependencyType.Items.Count)
                {
                    cmBDependencyType.SelectedIndex = categoryValue.Value;
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

            winConfig.DictInt[paramDependencyType] = cmBDependencyType.SelectedIndex;
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

        private async void ShowExistingSolutionComponents()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            if (_init > 0)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Loading dependent components...");

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

            try
            {
                var repository = new DependencyRepository(this._service);

                switch (solutionComponents)
                {
                    case DependencyType.RequiredComponents:
                        {
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

                    default:
                        break;
                }

                await _descriptor.GetSolutionComponentsDescriptionAsync(list);
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
            this._iWriteToOutput.WriteToOutput("Found {0} dependent components.", results.Count());

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

            UpdateStatus(string.Format("{0} solution dependent components.", results.Count()));

            ToggleControls(true);
        }

        private void UpdateStatus(string msg)
        {
            this.statusBar.Dispatcher.Invoke(() =>
            {
                this.tSSLStatusMessage.Content = msg;
            });
        }

        private void ToggleControls(bool enabled)
        {
            this._controlsEnabled = enabled;

            ToggleControl(this.toolStrip, enabled);

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
            SolutionComponentViewItem result = null;

            if (this.lstVSolutionComponents.SelectedItems.Count == 1
                && this.lstVSolutionComponents.SelectedItems[0] != null
                && this.lstVSolutionComponents.SelectedItems[0] is SolutionComponentViewItem
                )
            {
                result = this.lstVSolutionComponents.SelectedItems[0] as SolutionComponentViewItem;
            }

            return result;
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

        private async void ExecuteAction(SolutionComponentViewItem item, Func<string, SolutionComponentViewItem, Task> action)
        {
            string folder = _commonConfig.FolderForExport;

            if (!_controlsEnabled)
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

        private Task<string> CreateFileAsync(string folder, string entityName, string category, string name, string fieldName, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, entityName, category, name, fieldName, xmlContent));
        }

        private string CreateFile(string folder, string entityName, string category, string name, string fieldName, string xmlContent)
        {
            string fileName = EntityFileNameFormatter.GetWorkflowFileName(_service.ConnectionData.Name, entityName, category, name, fieldName, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }

                    File.WriteAllText(filePath, xmlContent, Encoding.UTF8);

                    this._iWriteToOutput.WriteToOutput("{0} Workflow {1} {2} exported to {3}", _service.ConnectionData.Name, name, fieldName, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("Workflow {0} {1} is empty.", name, fieldName);
                this._iWriteToOutput.ActivateOutputWindow();
            }

            return filePath;
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
            ToggleControls(false);

            string fileName = _descriptor.GetFileName(_service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, "EntityDescription", "txt");

            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var stringBuilder = new StringBuilder();

            var desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(solutionComponentViewItem.SolutionComponent, null);

            if (!string.IsNullOrEmpty(desc))
            {
                if (stringBuilder.Length > 0) { stringBuilder.AppendLine().AppendLine().AppendLine().AppendLine(); }

                stringBuilder.AppendLine(desc);
            }

            var entity = _descriptor.GetEntity<Entity>(solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

            if (entity != null)
            {
                desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(entity, null);

                if (!string.IsNullOrEmpty(desc))
                {
                    if (stringBuilder.Length > 0) { stringBuilder.AppendLine().AppendLine().AppendLine().AppendLine(); }

                    stringBuilder.AppendLine(desc);
                }
            }

            File.WriteAllText(filePath, stringBuilder.ToString(), Encoding.UTF8);

            this._iWriteToOutput.WriteToOutput("{0} {1} Entity Description exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

            this._iWriteToOutput.PerformAction(filePath, _commonConfig);

            this._iWriteToOutput.WriteToOutput("End creating file at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

            UpdateStatus("Operation is completed.");

            ToggleControls(true);
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

            var coll = await dependencyRepository.GetDependentComponentsAsync((int)solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value);

            string description = await descriptorHandler.GetDescriptionDependentAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                var fileName = _descriptor.GetFileName(this._service.ConnectionData.Name, solutionComponentViewItem.SolutionComponent.ComponentType.Value, solutionComponentViewItem.SolutionComponent.ObjectId.Value, "Dependent Components", "txt");

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, Encoding.UTF8);

                this._iWriteToOutput.WriteToOutput("{0} {1} Dependent Components exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
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

                File.WriteAllText(filePath, description, Encoding.UTF8);

                this._iWriteToOutput.WriteToOutput("{0} {1} Required Components exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
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

                File.WriteAllText(filePath, description, Encoding.UTF8);

                this._iWriteToOutput.WriteToOutput("{0} {1} Dependencies For Delete exported to {2}", solutionComponentViewItem.ComponentType, solutionComponentViewItem.Name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
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

            _service.ConnectionData.OpenSolutionComponentInWeb((ComponentType)entity.SolutionComponent.ComponentType.Value, entity.SolutionComponent.ObjectId.Value, null, null);
        }

        private void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            AddIntoSolution(true, null);
        }

        private void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                AddIntoSolution(false, solutionUniqueName);
            }
        }

        private void AddIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var solutionComponents = lstVSolutionComponents.SelectedItems.OfType<SolutionComponentViewItem>().Select(en => en.SolutionComponent).ToList();

            if (!solutionComponents.Any())
            {
                return;
            }

            _commonConfig.Save();

            var backWorker = new Thread(() =>
            {
                try
                {
                    this._iWriteToOutput.ActivateOutputWindow();

                    var contr = new SolutionController(this._iWriteToOutput);

                    contr.ExecuteAddingComponentesIntoSolution(_service.ConnectionData, _commonConfig, solutionUniqueName, solutionComponents, withSelect);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            });

            backWorker.Start();
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                var lastSoluiton = items.FirstOrDefault(i => string.Equals(i.Uid, "contMnAddIntoSolutionLast", StringComparison.InvariantCultureIgnoreCase));

                if (lastSoluiton != null)
                {
                    lastSoluiton.Items.Clear();

                    bool addIntoSolutionLast = _service.ConnectionData.LastSelectedSolutionsUniqueName.Any();

                    lastSoluiton.IsEnabled = addIntoSolutionLast;
                    lastSoluiton.Visibility = addIntoSolutionLast ? Visibility.Visible : Visibility.Collapsed;

                    foreach (var uniqueName in _service.ConnectionData.LastSelectedSolutionsUniqueName)
                    {
                        var menuItem = new MenuItem()
                        {
                            Header = uniqueName.Replace("_", "__"),
                            Tag = uniqueName,
                        };

                        menuItem.Click += AddIntoCrmSolutionLast_Click;

                        lastSoluiton.Items.Add(menuItem);
                    }
                }
            }
        }

        private void cmBComponentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0)
            {
                return;
            }

            FillDataGridColumns();

            ShowExistingSolutionComponents();
        }

        private void cmBDependencyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0)
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
                columnsForComponentType = SolutionComponentDescriptor.GetComponentColumns(category.Value);
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
            );
        }
    }
}