using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowOrganizationComparerGlobalOptionSets : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private Dictionary<Guid, IOrganizationServiceExtented> _cacheService = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, List<OptionSetMetadata>> _cacheOptionSetMetadata = new Dictionary<Guid, List<OptionSetMetadata>>();

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private ObservableCollection<LinkedOptionSetMetadata> _itemsSource;

        private Popup _optionsPopup;

        private bool _controlsEnabled = true;

        private int _init = 0;

        public WindowOrganizationComparerGlobalOptionSets(
          IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter
            )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = connection1.ConnectionConfiguration;

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            var child = new ExportGlobalOptionSetMetadataOptionsControl(_commonConfig);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            tSDDBConnection1.Header = string.Format(Properties.OperationNames.ExportFromConnectionFormat1, connection1.Name);
            tSDDBConnection2.Header = string.Format(Properties.OperationNames.ExportFromConnectionFormat1, connection2.Name);

            this.Resources["ConnectionName1"] = string.Format(Properties.OperationNames.CreateFromConnectionFormat1, connection1.Name);
            this.Resources["ConnectionName2"] = string.Format(Properties.OperationNames.CreateFromConnectionFormat1, connection2.Name);

            LoadFromConfig();

            txtBFilter.Text = filter;

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<LinkedOptionSetMetadata>();

            this.lstVwOptionSets.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = _connectionConfig.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = _connectionConfig.Connections;
            cmBConnection2.SelectedItem = connection2;

            _init--;

            ShowExistingOptionSets();
        }

        private void LoadFromConfig()
        {
            txtBNameSpace1.DataContext = cmBConnection1;
            txtBNameSpace2.DataContext = cmBConnection2;

            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

            BindingOperations.ClearAllBindings(cmBConnection1);
            cmBConnection1.Items.DetachFromSourceCollection();
            cmBConnection1.DataContext = null;
            cmBConnection1.ItemsSource = null;

            BindingOperations.ClearAllBindings(cmBConnection2);
            cmBConnection2.Items.DetachFromSourceCollection();
            cmBConnection2.DataContext = null;
            cmBConnection2.ItemsSource = null;

            base.OnClosed(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async Task<IOrganizationServiceExtented> GetService1()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection1.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_cacheService.ContainsKey(connectionData.ConnectionId))
                {
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _cacheService[connectionData.ConnectionId] = service;
                }

                return _cacheService[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task<IOrganizationServiceExtented> GetService2()
        {
            ConnectionData connectionData = null;

            cmBConnection2.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection2.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_cacheService.ContainsKey(connectionData.ConnectionId))
                {
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _cacheService[connectionData.ConnectionId] = service;
                }

                return _cacheService[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingOptionSets()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingOptionSets);

            this._itemsSource.Clear();

            IEnumerable<LinkedOptionSetMetadata> list = Enumerable.Empty<LinkedOptionSetMetadata>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var temp = new List<LinkedOptionSetMetadata>();

                    if (!_cacheOptionSetMetadata.ContainsKey(service1.ConnectionData.ConnectionId))
                    {
                        OptionSetRepository repository1 = new OptionSetRepository(service1);

                        var task1 = repository1.GetOptionSetsAsync();

                        _cacheOptionSetMetadata.Add(service1.ConnectionData.ConnectionId, await task1);
                    }

                    if (!_cacheOptionSetMetadata.ContainsKey(service2.ConnectionData.ConnectionId))
                    {
                        OptionSetRepository repository2 = new OptionSetRepository(service2);

                        var task2 = repository2.GetOptionSetsAsync();

                        _cacheOptionSetMetadata.Add(service2.ConnectionData.ConnectionId, await task2);
                    }

                    List<OptionSetMetadata> list1 = _cacheOptionSetMetadata[service1.ConnectionData.ConnectionId];
                    List<OptionSetMetadata> list2 = _cacheOptionSetMetadata[service2.ConnectionData.ConnectionId];

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        foreach (var entityMetadata1 in list1)
                        {
                            var entityMetadata2 = list2.FirstOrDefault(e => e.Name == entityMetadata1.Name);

                            if (entityMetadata2 == null)
                            {
                                continue;
                            }

                            temp.Add(new LinkedOptionSetMetadata(entityMetadata1.Name, entityMetadata1, entityMetadata2));
                        }
                    }
                    else
                    {
                        foreach (var entityMetadata1 in list1)
                        {
                            temp.Add(new LinkedOptionSetMetadata(entityMetadata1.Name, entityMetadata1, null));
                        }
                    }

                    list = temp;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadEntities(list);

            ToggleControls(true, Properties.WindowStatusStrings.LoadingOptionSetsCompletedFormat1, list.Count());
        }

        private static IEnumerable<LinkedOptionSetMetadata> FilterList(IEnumerable<LinkedOptionSetMetadata> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent =>
                        ent.OptionSetMetadata1?.MetadataId == tempGuid
                        || ent.OptionSetMetadata2?.MetadataId == tempGuid
                    );
                }
                else
                {
                    list = list
                    .Where(ent =>
                        ent.Name.ToLower().Contains(textName)

                        || (ent.OptionSetMetadata1 != null && ent.OptionSetMetadata1.DisplayName != null && ent.OptionSetMetadata1.DisplayName.LocalizedLabels
                            .Where(l => !string.IsNullOrEmpty(l.Label))
                            .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                        || (ent.OptionSetMetadata2 != null && ent.OptionSetMetadata2.DisplayName != null && ent.OptionSetMetadata2.DisplayName.LocalizedLabels
                            .Where(l => !string.IsNullOrEmpty(l.Label))
                            .Any(lbl => lbl.Label.ToLower().Contains(textName)))
                    );
                }
            }

            return list;
        }

        private void LoadEntities(IEnumerable<LinkedOptionSetMetadata> results)
        {
            this.lstVwOptionSets.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results)
                {
                    this._itemsSource.Add(entity);
                }

                if (this.lstVwOptionSets.Items.Count == 1)
                {
                    this.lstVwOptionSets.SelectedItem = this.lstVwOptionSets.Items[0];
                }
            });
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSDDBShowDifference, enabled);
            ToggleControl(this.tSDDBConnection1, enabled);
            ToggleControl(this.tSDDBConnection2, enabled);
            ToggleControl(this.cmBConnection1, enabled);
            ToggleControl(this.cmBConnection2, enabled);

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

        private string CreateFileName(IEnumerable<OptionSetMetadata> optionSets, ConnectionData connection, string extenstion)
        {
            string fileName = null;

            if (optionSets.Count() == 1)
            {
                fileName = string.Format("{0}.{1}.Generated.{2}", connection.Name, optionSets.First().Name, extenstion);
            }
            else
            {
                fileName = string.Format("{0}.GlobalOptionSets.{1}", connection.Name, extenstion);
            }

            return Path.Combine(_commonConfig.FolderForExport, fileName);
        }

        private async void btnDifferenceCSharpFile_Click(object sender, RoutedEventArgs e)
        {
            var service1 = await GetService1();
            var service2 = await GetService2();

            if (service1 != null && service2 != null && service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
            {
                if (_cacheOptionSetMetadata.TryGetValue(service1.ConnectionData.ConnectionId, out var optionSets1)
                    && _cacheOptionSetMetadata.TryGetValue(service2.ConnectionData.ConnectionId, out var optionSets2)
                    )
                {
                    await PerformComparingCSharpFiles(optionSets1, optionSets2);
                }
            }
        }

        private async void btnDifferenceCSharpFileSingle_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            var optionSets1 = new[] { link.OptionSetMetadata1 };
            var optionSets2 = new[] { link.OptionSetMetadata2 };

            await PerformComparingCSharpFiles(optionSets1, optionSets2);
        }

        private async Task PerformComparingCSharpFiles(IEnumerable<OptionSetMetadata> optionSets1, IEnumerable<OptionSetMetadata> optionSets2)
        {
            if (optionSets1 == null || optionSets2 == null)
            {
                return;
            }

            if (!optionSets1.Any() || !optionSets2.Any())
            {
                return;
            }

            if (_init > 0 || !_controlsEnabled)
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

            var service1 = await GetService1();
            var service2 = await GetService2();

            string optionSetsName = string.Join(",", optionSets1.Select(o => o.Name).OrderBy(s => s));

            if (service1 != null && service2 != null)
            {
                this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

                ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForOptionSetsForConnectionsFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);

                try
                {
                    string filePath1 = CreateFileName(optionSets1, service1.ConnectionData, "cs");
                    string filePath2 = CreateFileName(optionSets2, service2.ConnectionData, "cs");

                    string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);
                    var constantType = _commonConfig.ConstantType;
                    var optionSetExportType = _commonConfig.OptionSetExportType;

                    var nameSpace1 = txtBNameSpace1.Text.Trim();
                    var nameSpace2 = txtBNameSpace2.Text.Trim();

                    var withDependentComponents = _commonConfig.GlobalOptionSetsWithDependentComponents;
                    var allDescriptions = _commonConfig.AllDescriptions;
                    var withManagedInfo = _commonConfig.WithManagedInfo;

                    service1.ConnectionData.NameSpaceOptionSets = nameSpace1;
                    service2.ConnectionData.NameSpaceOptionSets = nameSpace2;

                    using (var handler1 = new CreateGlobalOptionSetsFileCSharpHandler(service1, _iWriteToOutput, tabSpacer, constantType, optionSetExportType, withDependentComponents, withManagedInfo, allDescriptions))
                    {
                        var task1 = handler1.CreateFileAsync(filePath1, optionSets1);

                        if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                        {
                            using (var handler2 = new CreateGlobalOptionSetsFileCSharpHandler(service2, _iWriteToOutput, tabSpacer, constantType, optionSetExportType, withDependentComponents, withManagedInfo, allDescriptions))
                            {
                                var task2 = handler2.CreateFileAsync(filePath2, optionSets2);

                                await task2;
                            }
                        }

                        await task1;
                    }
                    
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service1.ConnectionData.Name, optionSetsName, filePath1);

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service2.ConnectionData.Name, optionSetsName, filePath2);
                    }

                    if (File.Exists(filePath1) && File.Exists(filePath2))
                    {
                        this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                    }
                    else
                    {
                        this._iWriteToOutput.PerformAction(filePath1);

                        this._iWriteToOutput.PerformAction(filePath2);
                    }

                    ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsForConnectionsCompletedFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(ex);

                    ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsForConnectionsFailedFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);
                }

                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
            }
        }

        private async void btnDifferenceJavaScriptFile_Click(object sender, RoutedEventArgs e)
        {
            var service1 = await GetService1();
            var service2 = await GetService2();

            if (service1 != null && service2 != null && service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
            {
                if (_cacheOptionSetMetadata.TryGetValue(service1.ConnectionData.ConnectionId, out var optionSets1)
                    && _cacheOptionSetMetadata.TryGetValue(service2.ConnectionData.ConnectionId, out var optionSets2)
                    )
                {
                    await PerformComparingJavaScriptFile(optionSets1, optionSets2);
                }
            }
        }

        private async void btnDifferenceJavaScriptFileSingle_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            var optionSets1 = new[] { link.OptionSetMetadata1 };
            var optionSets2 = new[] { link.OptionSetMetadata2 };

            await PerformComparingJavaScriptFile(optionSets1, optionSets2);
        }

        private async Task PerformComparingJavaScriptFile(IEnumerable<OptionSetMetadata> optionSets1, IEnumerable<OptionSetMetadata> optionSets2)
        {
            if (optionSets1 == null || optionSets2 == null)
            {
                return;
            }

            if (!optionSets1.Any() || !optionSets2.Any())
            {
                return;
            }

            if (_init > 0 || !_controlsEnabled)
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

            var service1 = await GetService1();
            var service2 = await GetService2();

            string optionSetsName = string.Join(",", optionSets1.Select(o => o.Name).OrderBy(s => s));

            if (service1 != null && service2 != null)
            {
                this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

                ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForOptionSetsForConnectionsFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);

                try
                {
                    string filePath1 = CreateFileName(optionSets1, service1.ConnectionData, "js");
                    string filePath2 = CreateFileName(optionSets2, service2.ConnectionData, "js");

                    string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);
                    var constantType = _commonConfig.ConstantType;

                    var nameSpace1 = txtBNameSpace1.Text.Trim();
                    var nameSpace2 = txtBNameSpace2.Text.Trim();

                    var withDependentComponents = _commonConfig.GlobalOptionSetsWithDependentComponents;

                    service1.ConnectionData.NameSpaceOptionSets = nameSpace1;
                    service2.ConnectionData.NameSpaceOptionSets = nameSpace2;

                    using (var handler1 = new CreateGlobalOptionSetsFileJavaScriptHandler(service1, _iWriteToOutput, tabSpacer, withDependentComponents))
                    {
                        var task1 = handler1.CreateFileAsync(filePath1, optionSets1);

                        if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                        {
                            using (var handler2 = new CreateGlobalOptionSetsFileJavaScriptHandler(service2, _iWriteToOutput, tabSpacer, withDependentComponents))
                            {
                                var task2 = handler2.CreateFileAsync(filePath2, optionSets2);

                                await task2;
                            }
                        }

                        await task1;
                    }

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service1.ConnectionData.Name, optionSetsName, filePath1);

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service2.ConnectionData.Name, optionSetsName, filePath2);
                    }

                    if (File.Exists(filePath1) && File.Exists(filePath2))
                    {
                        this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                    }
                    else
                    {
                        this._iWriteToOutput.PerformAction(filePath1);

                        this._iWriteToOutput.PerformAction(filePath2);
                    }

                    ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsForConnectionsCompletedFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(ex);

                    ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsForConnectionsFailedFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);
                }

                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
            }
        }

        private void btnConnection1CSharp_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;

            if (connection1 != null && _cacheOptionSetMetadata.TryGetValue(connection1.ConnectionId, out var optionSets))
            {
                CreateGlobalOptionSetsCSharpFile(GetService1, txtBNameSpace1.Text.Trim(), optionSets);
            }
        }

        private void btnConnection2CSharp_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

            if (connection2 != null && _cacheOptionSetMetadata.TryGetValue(connection2.ConnectionId, out var optionSets))
            {
                CreateGlobalOptionSetsCSharpFile(GetService2, txtBNameSpace2.Text.Trim(), optionSets);
            }
        }

        private void btnConnection1CSharpSingle_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            var optionSets = new[] { link.OptionSetMetadata1 };

            CreateGlobalOptionSetsCSharpFile(GetService1, txtBNameSpace1.Text.Trim(), optionSets);
        }

        private void btnConnection2CSharpSingle_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            var optionSets = new[] { link.OptionSetMetadata2 };

            CreateGlobalOptionSetsCSharpFile(GetService2, txtBNameSpace2.Text.Trim(), optionSets);
        }

        private async Task CreateGlobalOptionSetsCSharpFile(Func<Task<IOrganizationServiceExtented>> getService, string nameSpace, IEnumerable<OptionSetMetadata> optionSets)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (optionSets == null)
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

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForOptionSetsFormat1, optionSetsName);

            try
            {
                string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);
                var constantType = _commonConfig.ConstantType;
                var optionSetExportType = _commonConfig.OptionSetExportType;

                var withDependentComponents = _commonConfig.GlobalOptionSetsWithDependentComponents;
                var allDescriptions = _commonConfig.AllDescriptions;
                var withManagedInfo = _commonConfig.WithManagedInfo;

                var service = await getService();

                string filePath = CreateFileName(optionSets, service.ConnectionData, "cs");

                service.ConnectionData.NameSpaceOptionSets = nameSpace;

                using (var handler = new CreateGlobalOptionSetsFileCSharpHandler(service, _iWriteToOutput, tabSpacer, constantType, optionSetExportType, withDependentComponents, withManagedInfo, allDescriptions))
                {
                    await handler.CreateFileAsync(filePath, optionSets);
                }

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, optionSetsName, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsCompletedFormat1, optionSetsName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsFailedFormat1, optionSetsName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
        }

        private void btnConnection1JavaScript_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;

            if (connection1 != null && _cacheOptionSetMetadata.TryGetValue(connection1.ConnectionId, out var optionSets))
            {
                CreateGlobalOptionSetsJavaScriptFile(GetService1, txtBNameSpace1.Text.Trim(), optionSets);
            }
        }

        private void btnConnection2JavaScript_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

            if (connection2 != null && _cacheOptionSetMetadata.TryGetValue(connection2.ConnectionId, out var optionSets))
            {
                CreateGlobalOptionSetsJavaScriptFile(GetService2, txtBNameSpace2.Text.Trim(), optionSets);
            }
        }

        private void btnConnection1JavaScriptSingle_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            var optionSets = new[] { link.OptionSetMetadata1 };

            CreateGlobalOptionSetsJavaScriptFile(GetService1, txtBNameSpace1.Text.Trim(), optionSets);
        }

        private void btnConnection2JavaScriptSingle_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            var optionSets = new[] { link.OptionSetMetadata2 };

            CreateGlobalOptionSetsJavaScriptFile(GetService2, txtBNameSpace2.Text.Trim(), optionSets);
        }

        private async Task CreateGlobalOptionSetsJavaScriptFile(Func<Task<IOrganizationServiceExtented>> getService, string nameSpace, IEnumerable<OptionSetMetadata> optionSets)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (optionSets == null || !optionSets.Any())
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

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForOptionSetsFormat1, optionSetsName);

            try
            {
                string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

                var withDependentComponents = _commonConfig.GlobalOptionSetsWithDependentComponents;

                var service = await getService();

                string filePath = CreateFileName(optionSets, service.ConnectionData, "js");

                service.ConnectionData.NameSpaceOptionSets = nameSpace;

                using (var handler = new CreateGlobalOptionSetsFileJavaScriptHandler(service, _iWriteToOutput, tabSpacer, withDependentComponents))
                {
                    await handler.CreateFileAsync(filePath, optionSets);
                }

                var message = string.Empty;

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, optionSetsName, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsCompletedFormat1, optionSetsName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsFailedFormat1, optionSetsName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingOptionSets();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingOptionSets();
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

        private void lstVwOptionSets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as LinkedOptionSetMetadata;

                if (item != null)
                {
                    var optionSets1 = new[] { item.OptionSetMetadata1 };
                    var optionSets2 = new[] { item.OptionSetMetadata2 };

                    PerformComparingCSharpFiles(optionSets1, optionSets2);
                }
            }
        }

        private void lstVwOptionSets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwOptionSets.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwOptionSets.SelectedItems.Count > 0;

                    var item = (this.lstVwOptionSets.SelectedItems[0] as LinkedOptionSetMetadata);

                    tSDDBShowDifference.IsEnabled = enabled && item.OptionSetMetadata1 != null && item.OptionSetMetadata2 != null;
                    tSDDBConnection1.IsEnabled = enabled && item.OptionSetMetadata1 != null;
                    tSDDBConnection2.IsEnabled = enabled && item.OptionSetMetadata2 != null;
                }
                catch (Exception)
                {
                }
            });
        }

        private LinkedOptionSetMetadata GetSelectedEntity()
        {
            return this.lstVwOptionSets.SelectedItems.OfType<LinkedOptionSetMetadata>().Count() == 1
                ? this.lstVwOptionSets.SelectedItems.OfType<LinkedOptionSetMetadata>().SingleOrDefault() : null;
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();

                ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;
                ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

                if (connection1 != null && connection2 != null)
                {
                    tSDDBConnection1.Header = string.Format(Properties.OperationNames.ExportFromConnectionFormat1, connection1.Name);
                    tSDDBConnection2.Header = string.Format(Properties.OperationNames.ExportFromConnectionFormat1, connection2.Name);

                    this.Resources["ConnectionName1"] = string.Format(Properties.OperationNames.CreateFromConnectionFormat1, connection1.Name);
                    this.Resources["ConnectionName2"] = string.Format(Properties.OperationNames.CreateFromConnectionFormat1, connection2.Name);

                    UpdateButtonsEnable();

                    ShowExistingOptionSets();
                }
            });
        }
        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, null);
        }

        private async void btnCompareApplicationRibbons_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, null);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, null);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, null);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, null);
        }

        private async void btnEntityAttributeExplorer1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntityAttributeExplorer2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntityRelationshipOneToManyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntityRelationshipOneToManyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntityRelationshipManyToManyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntityRelationshipManyToManyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntityKeyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntityKeyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void btnEntitySecurityRolesExplorer1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, null, null);
        }

        private async void btnEntitySecurityRolesExplorer2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, null, null);
        }

        private async void btnCreateMetadataFile1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, null, null, null);
        }

        private async void btnExportApplicationRibbon1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnGlobalOptionSets1_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService1();

            _commonConfig.Save();

            IEnumerable<OptionSetMetadata> optionSets = null;

            if (_cacheOptionSetMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                optionSets = _cacheOptionSetMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
                , optionSets
                , string.Empty
                , string.Empty
                );
        }

        private async void btnSystemForms1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnSavedQuery1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnSavedChart1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnWorkflows1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnPluginTree1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty, string.Empty);
        }

        private async void btnMessageTree1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnMessageRequestTree1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnCreateMetadataFile2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, null, null, null);
        }

        private async void btnExportApplicationRibbon2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnGlobalOptionSets2_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService2();

            _commonConfig.Save();

            IEnumerable<OptionSetMetadata> optionSets = null;

            if (_cacheOptionSetMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                optionSets = _cacheOptionSetMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
                , optionSets
                , string.Empty
                , string.Empty
                );
        }

        private async void btnSystemForms2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnSavedQuery2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnSavedChart2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnWorkflows2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnPluginTree2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty, string.Empty);
        }

        private async void btnMessageTree2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private async void btnMessageRequestTree2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, null, string.Empty);
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var linkedEntityMetadata = ((FrameworkElement)e.OriginalSource).DataContext as LinkedOptionSetMetadata;

            var items = contextMenu.Items.OfType<Control>();

            foreach (var menuContextDifference in items.Where(i => string.Equals(i.Uid, "menuContextDifference", StringComparison.InvariantCultureIgnoreCase)))
            {
                menuContextDifference.IsEnabled = false;
                menuContextDifference.Visibility = Visibility.Collapsed;

                if (linkedEntityMetadata != null
                     && linkedEntityMetadata.OptionSetMetadata1 != null
                     && linkedEntityMetadata.OptionSetMetadata2 != null
                     )
                {
                    menuContextDifference.IsEnabled = true;
                    menuContextDifference.Visibility = Visibility.Visible;
                }
            }

            foreach (var menuContextConnection2 in items.Where(i => string.Equals(i.Uid, "menuContextConnection2", StringComparison.InvariantCultureIgnoreCase)))
            {
                menuContextConnection2.IsEnabled = false;
                menuContextConnection2.Visibility = Visibility.Collapsed;

                if (linkedEntityMetadata != null
                     && linkedEntityMetadata.OptionSetMetadata2 != null
                     )
                {
                    menuContextConnection2.IsEnabled = true;
                    menuContextConnection2.Visibility = Visibility.Visible;
                }
            }
        }

        private void miOptions_Click(object sender, RoutedEventArgs e)
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
