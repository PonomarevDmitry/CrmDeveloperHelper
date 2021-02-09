using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class WindowSolutionDifferenceImage : WindowWithSolutionComponentDescriptor
    {
        private SolutionDifferenceImage _SolutionDifferenceImage = null;

        private readonly ObservableCollection<SolutionImageComponent> _itemsSource;

        public WindowSolutionDifferenceImage(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connectionData
            , string filePath
        ) : base(iWriteToOutput, commonConfig, connectionData)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            cmBComponentType.ItemsSource = new EnumBindingSourceExtension(typeof(ComponentType?))
            {
                SortByName = true,
            }.ProvideValue(null) as IEnumerable;

            LoadFromConfig();

            LoadConfiguration();

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<SolutionImageComponent>();

            this.lstVwComponents.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = connectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = connectionData;

            this.DecreaseInit();

            if (!string.IsNullOrEmpty(filePath))
            {
                var task = LoadSolutionDifferenceImage(filePath);
            }
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        private const string paramComponentType = "ComponentType";
        private const string paramComponentLocation = "ComponentLocation";

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
                    }
                }
            }

            {
                var categoryValue = winConfig.GetValueInt(paramComponentLocation);

                if (categoryValue.HasValue && 0 <= categoryValue && categoryValue < cmBComponentLocation.Items.Count)
                {
                    cmBComponentLocation.SelectedIndex = categoryValue.Value;
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            var categoryValue = -1;

            if (cmBComponentType.SelectedItem != null
                && cmBComponentType.SelectedItem is ComponentType selected
                )
            {
                categoryValue = (int)selected;
            }

            winConfig.DictInt[paramComponentType] = categoryValue;
            winConfig.DictInt[paramComponentLocation] = cmBComponentLocation.SelectedIndex;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;
        }

        private enum ComponentLocation
        {
            CommonComponents = 0,
            OnlyInFirst = 1,
            OnlyInSecond = 2,
        }

        private ComponentLocation GetComponentLocation()
        {
            var result = ComponentLocation.CommonComponents;

            cmBComponentLocation.Dispatcher.Invoke(() =>
            {
                if (cmBComponentLocation.SelectedIndex != -1)
                {
                    result = (ComponentLocation)cmBComponentLocation.SelectedIndex;
                }
            });

            return result;
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
            return GetOrganizationService(GetSelectedConnection());
        }

        private async Task LoadSolutionDifferenceImage(string filePath)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();

                txtBFilePath.Text = string.Empty;
            });

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            ToggleControls(null, false, Properties.OutputStrings.LoadingSolutionDifferenceImage);

            try
            {
                this._SolutionDifferenceImage = await SolutionDifferenceImage.LoadAsync(filePath);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);

                this._SolutionDifferenceImage = null;
            }

            txtBFilePath.Dispatcher.Invoke(() =>
            {
                if (this._SolutionDifferenceImage != null)
                {
                    txtBFilePath.Text = filePath;
                }
                else
                {
                    txtBFilePath.Text = string.Empty;
                }
            });

            if (this._SolutionDifferenceImage == null)
            {
                ToggleControls(null, true, Properties.OutputStrings.LoadingSolutionDifferenceImageFailed);
                return;
            }

            ToggleControls(null, true, Properties.OutputStrings.LoadingSolutionDifferenceImageCompleted);

            FilteringSolutionDifferenceImageComponents();
        }

        private void FilteringSolutionDifferenceImageComponents()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            ToggleControls(null, false, Properties.OutputStrings.FilteringSolutionDifferenceImageComponents);

            IEnumerable<SolutionImageComponent> filter = Enumerable.Empty<SolutionImageComponent>();

            if (this._SolutionDifferenceImage != null)
            {
                var location = GetComponentLocation();

                switch (location)
                {
                    case ComponentLocation.OnlyInFirst:
                        if (this._SolutionDifferenceImage.OnlySolution1Components != null)
                        {
                            filter = this._SolutionDifferenceImage.OnlySolution1Components;
                        }
                        break;

                    case ComponentLocation.OnlyInSecond:
                        if (this._SolutionDifferenceImage.OnlySolution2Components != null)
                        {
                            filter = this._SolutionDifferenceImage.OnlySolution2Components;
                        }
                        break;

                    case ComponentLocation.CommonComponents:
                    default:
                        if (this._SolutionDifferenceImage.CommonComponents != null)
                        {
                            filter = this._SolutionDifferenceImage.CommonComponents;
                        }
                        break;
                }
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });


            if (!string.IsNullOrEmpty(textName))
            {
                filter = filter.Where(s => !string.IsNullOrEmpty(s.Description) && s.Description.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1);
            }

            int? category = null;

            cmBComponentType.Dispatcher.Invoke(() =>
            {
                if (cmBComponentType.SelectedItem != null
                    && cmBComponentType.SelectedItem is ComponentType comp
                    )
                {
                    category = (int)comp;
                }
            });

            if (category.HasValue)
            {
                filter = filter.Where(e => e.ComponentType == category);
            }

            foreach (var component in filter)
            {
                _itemsSource?.Add(component);
            }

            ToggleControls(null, true, Properties.OutputStrings.FilteringSolutionDifferenceImageComponentsCompletedFormat1, filter.Count());
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

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, this.tSBLoadSolutionDifferenceImage, this.cmBComponentLocation, this.cmBComponentType);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwComponents.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwComponents.SelectedItems.Count > 0;

                    UIElement[] list = { };

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
                FilteringSolutionDifferenceImageComponents();
            }
        }

        private SolutionImageComponent GetSelectedEntity()
        {
            return this.lstVwComponents.SelectedItems.OfType<SolutionImageComponent>().Count() == 1
                ? this.lstVwComponents.SelectedItems.OfType<SolutionImageComponent>().SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                //SolutionImageComponent item = GetItemFromRoutedDataContext<SolutionImageComponent>(e);

                //if (item != null)
                //{
                //    ExecuteAction(item, PerformExportAllXml);
                //}
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(SolutionImageComponent entity, Func<string, SolutionImageComponent, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, entity);
        }

        private async void tSBLoadSolutionDifferenceImage_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = string.Empty;
            var thread = new Thread(() =>
            {
                try
                {
                    var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
                    {
                        Filter = "Solution Difference Image (.xml)|*.xml",
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

            if (!string.IsNullOrEmpty(selectedPath))
            {
                await LoadSolutionDifferenceImage(selectedPath);
            }
        }

        protected override Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            FilteringSolutionDifferenceImageComponents();

            return base.OnRefreshList(e);
        }

        private void cmBComponentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilteringSolutionDifferenceImageComponents();
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            ConnectionData connectionData = GetSelectedConnection();

            FillLastSolutionItems(connectionData, items, true, AddToSolutionLast_Click, "contMnAddToSolutionLast");
        }

        private async void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var descriptor = GetSolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(new[] { entity });

            if (!solutionComponents.Any())
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionComponentNotFoundInConnectionFormat1, service.ConnectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            foreach (var item in solutionComponents)
            {
                service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb((ComponentType)item.ComponentType.Value, item.ObjectId.Value);
            }
        }

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var descriptor = GetSolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(new[] { entity });

            if (!solutionComponents.Any())
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionComponentNotFoundInConnectionFormat1, service.ConnectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            _commonConfig.Save();

            foreach (var item in solutionComponents)
            {
                WindowHelper.OpenSolutionComponentDependenciesExplorer(
                    _iWriteToOutput
                    , service
                    , descriptor
                    , _commonConfig
                    , item.ComponentType.Value
                    , item.ObjectId.Value
                    , null);
            }
        }

        private async void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var descriptor = GetSolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(new[] { entity });

            if (!solutionComponents.Any())
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionComponentNotFoundInConnectionFormat1, service.ConnectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            foreach (var item in solutionComponents)
            {
                if (SolutionComponent.IsDefinedComponentType(item.ComponentType?.Value))
                {
                    service.UrlGenerator.OpenSolutionComponentInWeb((ComponentType)item.ComponentType.Value, item.ObjectId.Value);
                }
            }
        }

        private async void mIOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var descriptor = GetSolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(new[] { entity });

            if (!solutionComponents.Any())
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionComponentNotFoundInConnectionFormat1, service.ConnectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            _commonConfig.Save();

            foreach (var item in solutionComponents)
            {
                WindowHelper.OpenExplorerSolutionExplorer(
                    _iWriteToOutput
                    , service
                    , _commonConfig
                    , item.ComponentType.Value
                    , item.ObjectId.Value
                    , null
                );
            }
        }

        private async void AddToSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolutionAsync(true, null);
        }

        private async void AddToSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
              && menuItem.Tag != null
              && menuItem.Tag is string solutionUniqueName
              )
            {
                await AddToSolutionAsync(false, solutionUniqueName);
            }
        }

        private async Task AddToSolutionAsync(bool withSelect, string solutionUniqueName)
        {
            var solutionImageComponents = lstVwComponents.SelectedItems.OfType<SolutionImageComponent>().ToList();

            if (!solutionImageComponents.Any())
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var descriptor = GetSolutionComponentDescriptor(service);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(solutionImageComponents);

            if (!solutionComponents.Any())
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SolutionComponentNotFoundInConnectionFormat1, service.ConnectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            _commonConfig.Save();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsCollectionToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, solutionComponents, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }
    }
}