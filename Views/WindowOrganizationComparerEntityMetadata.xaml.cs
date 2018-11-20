using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
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
    public partial class WindowOrganizationComparerEntityMetadata : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private Popup _optionsExportEntityMetadata;
        private Popup _optionsExportAttributesDependentComponents;
        private Popup _optionsEntityRibbon;

        private Dictionary<Guid, IOrganizationServiceExtented> _cacheService = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, List<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, List<EntityMetadata>>();

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private ObservableCollection<LinkedEntityMetadata> _itemsSource;

        private bool _controlsEnabled = true;

        private int _init = 0;

        public WindowOrganizationComparerEntityMetadata(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string entityFilter
        )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = connection1.ConnectionConfiguration;

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            this._optionsExportEntityMetadata = new Popup
            {
                Child = new ExportEntityMetadataOptionsControl(_commonConfig),

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
            };
            this._optionsExportAttributesDependentComponents = new Popup
            {
                Child = new ExportEntityAttributesDependentComponentsOptionsControl(_commonConfig),

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
            };
            this._optionsEntityRibbon = new Popup
            {
                Child = new ExportEntityRibbonOptionsControl(_commonConfig),

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
            };

            tSDDBConnection1.Header = string.Format(Properties.OperationNames.ExportFromConnectionFormat1, connection1.Name);
            tSDDBConnection2.Header = string.Format(Properties.OperationNames.ExportFromConnectionFormat1, connection2.Name);

            this.Resources["ConnectionName1"] = string.Format(Properties.OperationNames.CreateFromConnectionFormat1, connection1.Name);
            this.Resources["ConnectionName2"] = string.Format(Properties.OperationNames.CreateFromConnectionFormat1, connection2.Name);

            LoadFromConfig();

            txtBFilterEnitity.Text = entityFilter;

            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;

            txtBFilterEnitity.Focus();

            this._itemsSource = new ObservableCollection<LinkedEntityMetadata>();

            this.lstVwEntities.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = _connectionConfig.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = _connectionConfig.Connections;
            cmBConnection2.SelectedItem = connection2;

            _init--;

            ShowExistingEntities();
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

        private async Task ShowExistingEntities()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingEntities);

            this._itemsSource.Clear();

            IEnumerable<LinkedEntityMetadata> list = Enumerable.Empty<LinkedEntityMetadata>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var temp = new List<LinkedEntityMetadata>();

                    List<EntityMetadata> list1;
                    List<EntityMetadata> list2;

                    if (!_cacheEntityMetadata.ContainsKey(service1.ConnectionData.ConnectionId))
                    {
                        EntityMetadataRepository repository1 = new EntityMetadataRepository(service1);

                        var task1 = repository1.GetEntitiesDisplayNameAsync();

                        _cacheEntityMetadata.Add(service1.ConnectionData.ConnectionId, await task1);
                    }

                    if (!_cacheEntityMetadata.ContainsKey(service2.ConnectionData.ConnectionId))
                    {
                        EntityMetadataRepository repository2 = new EntityMetadataRepository(service2);

                        var task2 = repository2.GetEntitiesDisplayNameAsync();

                        _cacheEntityMetadata.Add(service2.ConnectionData.ConnectionId, await task2);
                    }

                    list1 = _cacheEntityMetadata[service1.ConnectionData.ConnectionId];
                    list2 = _cacheEntityMetadata[service2.ConnectionData.ConnectionId];

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        foreach (var entityMetadata1 in list1)
                        {
                            var entityMetadata2 = list2.FirstOrDefault(e => e.LogicalName == entityMetadata1.LogicalName);

                            if (entityMetadata2 == null)
                            {
                                continue;
                            }

                            temp.Add(new LinkedEntityMetadata(entityMetadata1.LogicalName, entityMetadata1, entityMetadata2));
                        }
                    }
                    else
                    {
                        foreach (var entityMetadata1 in list1)
                        {
                            temp.Add(new LinkedEntityMetadata(entityMetadata1.LogicalName, entityMetadata1, null));
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

            txtBFilterEnitity.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterEnitity.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadEntities(list);
        }

        private static IEnumerable<LinkedEntityMetadata> FilterList(IEnumerable<LinkedEntityMetadata> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (int.TryParse(textName, out int tempInt))
                {
                    list = list.Where(ent => ent.EntityMetadata1?.ObjectTypeCode == tempInt || ent.EntityMetadata2?.ObjectTypeCode == tempInt);
                }
                else
                {
                    if (Guid.TryParse(textName, out Guid tempGuid))
                    {
                        list = list.Where(ent =>
                            ent.EntityMetadata1?.MetadataId == tempGuid
                            || ent.EntityMetadata2?.MetadataId == tempGuid
                        );
                    }
                    else
                    {
                        list = list
                        .Where(ent =>
                            ent.LogicalName.ToLower().Contains(textName)

                            || (ent.EntityMetadata1 != null && ent.EntityMetadata1?.DisplayName != null && ent.EntityMetadata1.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.ToLower().Contains(textName))
                                )

                            //|| (ent.EntityMetadata1.Description != null && ent.EntityMetadata1.Description.LocalizedLabels
                            //    .Where(l => !string.IsNullOrEmpty(l.Label))
                            //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                            //|| (ent.EntityMetadata1.DisplayCollectionName != null && ent.EntityMetadata1.DisplayCollectionName.LocalizedLabels
                            //    .Where(l => !string.IsNullOrEmpty(l.Label))
                            //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                            || (ent.EntityMetadata2 != null && ent.EntityMetadata2.DisplayName != null && ent.EntityMetadata2.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                        //|| (ent.EntityMetadata2.Description != null && ent.EntityMetadata2.Description.LocalizedLabels
                        //    .Where(l => !string.IsNullOrEmpty(l.Label))
                        //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                        //|| (ent.EntityMetadata2.DisplayCollectionName != null && ent.EntityMetadata2.DisplayCollectionName.LocalizedLabels
                        //    .Where(l => !string.IsNullOrEmpty(l.Label))
                        //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))
                        );
                    }
                }
            }

            return list;
        }

        private void LoadEntities(IEnumerable<LinkedEntityMetadata> results)
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results)
                {
                    _itemsSource.Add(entity);
                }

                if (this.lstVwEntities.Items.Count == 1)
                {
                    this.lstVwEntities.SelectedItem = this.lstVwEntities.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingEntitiesCompletedFormat1, results.Count());
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
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwEntities.SelectedItems.Count > 0;

                    var item = (this.lstVwEntities.SelectedItems[0] as LinkedEntityMetadata);

                    tSDDBShowDifference.IsEnabled = enabled && item.EntityMetadata1 != null && item.EntityMetadata2 != null;
                    tSDDBConnection1.IsEnabled = enabled && item.EntityMetadata1 != null;
                    tSDDBConnection2.IsEnabled = enabled && item.EntityMetadata2 != null;
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
                ShowExistingEntities();
            }
        }

        private LinkedEntityMetadata GetSelectedLinkedEntityMetadata()
        {
            return this.lstVwEntities.SelectedItems.OfType<LinkedEntityMetadata>().Count() == 1
                ? this.lstVwEntities.SelectedItems.OfType<LinkedEntityMetadata>().SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as LinkedEntityMetadata;

                if (item != null)
                {
                    ExecuteDifferenceCSharp(item.LogicalName);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void btnDifferenceCSharpFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (string.IsNullOrEmpty(entity?.LogicalName))
            {
                return;
            }

            ExecuteDifferenceCSharp(entity?.LogicalName);
        }

        private async Task ExecuteDifferenceCSharp(string entityName)
        {
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

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat1, entityName);

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceEntityMetadataCSharpForEntityFormat1, entityName);

            CreateFileWithEntityMetadataCSharpConfiguration config = GetCSharpConfig(entityName);

            string filePath1 = string.Empty;
            string filePath2 = string.Empty;

            var service1 = await GetService1();
            var service2 = await GetService2();

            using (var handler1 = new CreateFileWithEntityMetadataCSharpHandler(config, service1, _iWriteToOutput))
            {
                var task1 = handler1.CreateFileAsync();

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    using (var handler2 = new CreateFileWithEntityMetadataCSharpHandler(config, service2, _iWriteToOutput))
                    {
                        filePath2 = await handler2.CreateFileAsync();
                    }
                }

                filePath1 = await task1;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service1.ConnectionData.Name, config.EntityName, filePath1);

            if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service2.ConnectionData.Name, config.EntityName, filePath2);
            }

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this._iWriteToOutput.ProcessStartProgramComparer(_commonConfig, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
            }
            else
            {
                this._iWriteToOutput.PerformAction(filePath1, _commonConfig);

                this._iWriteToOutput.PerformAction(filePath2, _commonConfig);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceEntityMetadataCSharpForEntityCompletedFormat1, entityName);

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat1, entityName);
        }

        private CreateFileWithEntityMetadataCSharpConfiguration GetCSharpConfig(string entityName)
        {
            string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

            var result = new CreateFileWithEntityMetadataCSharpConfiguration
            (
                entityName
                , _commonConfig.FolderForExport
                , tabSpacer
                , _commonConfig.GenerateAttributes
                , _commonConfig.GenerateStatus
                , _commonConfig.GenerateLocalOptionSet
                , _commonConfig.GenerateGlobalOptionSet
                , _commonConfig.GenerateOneToMany
                , _commonConfig.GenerateManyToOne
                , _commonConfig.GenerateManyToMany
                , _commonConfig.GenerateKeys
                , _commonConfig.AllDescriptions
                , _commonConfig.EntityMetadaOptionSetDependentComponents
                , _commonConfig.GenerateIntoSchemaClass
                , _commonConfig.WithManagedInfo
                , _commonConfig.ConstantType
                , _commonConfig.OptionSetExportType
            );

            return result;
        }

        private void btnDifferenceJavaScriptFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (string.IsNullOrEmpty(entity?.LogicalName))
            {
                return;
            }

            ExecuteDifferenceJavaScript(entity?.LogicalName);
        }

        private async Task ExecuteDifferenceJavaScript(string entityName)
        {
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

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat1, entityName);

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceEntityMetadataJavaScriptForEntityFormat1, entityName);

            CreateFileWithEntityMetadataJavaScriptConfiguration config = GetJavaScriptConfig(entityName);

            string filePath1 = string.Empty;
            string filePath2 = string.Empty;

            var service1 = await GetService1();
            var service2 = await GetService2();

            using (var handler1 = new CreateFileWithEntityMetadataJavaScriptHandler(config, service1, _iWriteToOutput))
            {
                var task1 = handler1.CreateFileAsync();

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    using (var handler2 = new CreateFileWithEntityMetadataJavaScriptHandler(config, service2, _iWriteToOutput))
                    {
                        filePath2 = await handler2.CreateFileAsync();
                    }
                }

                filePath1 = await task1;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service1.ConnectionData.Name, config.EntityName, filePath1);

            if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service2.ConnectionData.Name, config.EntityName, filePath2);
            }

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this._iWriteToOutput.ProcessStartProgramComparer(_commonConfig, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
            }
            else
            {
                this._iWriteToOutput.PerformAction(filePath1, _commonConfig);

                this._iWriteToOutput.PerformAction(filePath2, _commonConfig);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceEntityMetadataJavaScriptForEntityCompletedFormat1, entityName);

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat1, entityName);
        }

        private CreateFileWithEntityMetadataJavaScriptConfiguration GetJavaScriptConfig(string entityName)
        {
            var tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

            var result = new CreateFileWithEntityMetadataJavaScriptConfiguration(
                entityName
                , _commonConfig.FolderForExport
                , tabSpacer
                , _commonConfig.EntityMetadaOptionSetDependentComponents
                );

            return result;
        }

        private void btnConnection1CSharp_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (string.IsNullOrEmpty(entity?.LogicalName))
            {
                return;
            }

            CreateEntityMetadataFileCSharp(GetService1, entity?.LogicalName, txtBNameSpace1.Text.Trim());
        }

        private void btnConnection2CSharp_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (string.IsNullOrEmpty(entity?.LogicalName))
            {
                return;
            }

            CreateEntityMetadataFileCSharp(GetService2, entity?.LogicalName, txtBNameSpace2.Text.Trim());
        }

        private async Task CreateEntityMetadataFileCSharp(Func<Task<IOrganizationServiceExtented>> getService, string entityName, string nameSpace)
        {
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

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat1, entityName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityName);

            string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

            var config = GetCSharpConfig(entityName);

            try
            {
                var service = await getService();

                using (var handler = new CreateFileWithEntityMetadataCSharpHandler(config, service, _iWriteToOutput))
                {
                    string filePath = await handler.CreateFileAsync();

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, config.EntityName, filePath);

                    this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                }

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat1, entityName);
        }

        private void btnConnection1JavaScript_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (string.IsNullOrEmpty(entity?.LogicalName))
            {
                return;
            }

            CreateEntityMetadataFileJavaScript(GetService1, entity?.LogicalName, txtBNameSpace1.Text.Trim());
        }

        private void btnConnection2JavaScript_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (string.IsNullOrEmpty(entity?.LogicalName))
            {
                return;
            }

            CreateEntityMetadataFileJavaScript(GetService2, entity?.LogicalName, txtBNameSpace2.Text.Trim());
        }

        private async Task CreateEntityMetadataFileJavaScript(Func<Task<IOrganizationServiceExtented>> getService, string entityName, string nameSpace)
        {
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

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat1, entityName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityName);

            var config = GetJavaScriptConfig(entityName);

            try
            {
                var service = await getService();

                using (var handler = new CreateFileWithEntityMetadataJavaScriptHandler(config, service, _iWriteToOutput))
                {
                    string filePath = await handler.CreateFileAsync();

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, config.EntityName, filePath);

                    this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                }

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat1, entityName);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingEntities();
            }

            base.OnKeyDown(e);
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

                    ShowExistingEntities();
                }
            });
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareApplicationRibbons_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.LogicalName);
        }

        private async void btnEntityAttributeExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityAttributeExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityRelationshipOneToManyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityRelationshipOneToManyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityRelationshipManyToManyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityRelationshipManyToManyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityKeyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityKeyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnCreateMetadataFile1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList, null);
        }

        private async void btnExportApplicationRibbon1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnGlobalOptionSets1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null)
            {
                return;
            }

            var service = await GetService1();
            var descriptor = new SolutionComponentDescriptor(service, false);

            var entityMetadata = descriptor.MetadataSource.GetEntityMetadata(entity.EntityMetadata1.MetadataId.Value);

            IEnumerable<OptionSetMetadata> optionSets =
                entityMetadata
                ?.Attributes
                ?.OfType<PicklistAttributeMetadata>()
                .Where(a => a.OptionSet.IsGlobal.GetValueOrDefault())
                .Select(a => a.OptionSet)
                .GroupBy(o => o.MetadataId)
                .Select(g => g.FirstOrDefault())
                ?? new OptionSetMetadata[0]
                ;

            _commonConfig.Save();

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
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnSavedQuery1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnSavedChart1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnWorkflows1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnPluginTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty, string.Empty);
        }

        private async void btnMessageTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnMessageRequestTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnCreateMetadataFile2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList, null);
        }

        private async void btnExportApplicationRibbon2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnGlobalOptionSets2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null)
            {
                return;
            }

            var service = await GetService2();
            var descriptor = new SolutionComponentDescriptor(service, false);

            var entityMetadata = descriptor.MetadataSource.GetEntityMetadata(entity.EntityMetadata2.MetadataId.Value);

            IEnumerable<OptionSetMetadata> optionSets =
                entityMetadata
                ?.Attributes
                ?.OfType<PicklistAttributeMetadata>()
                .Where(a => a.OptionSet.IsGlobal.GetValueOrDefault())
                .Select(a => a.OptionSet)
                .GroupBy(o => o.MetadataId)
                .Select(g => g.FirstOrDefault())
                ?? new OptionSetMetadata[0]
                ;

            _commonConfig.Save();

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
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnSavedQuery2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnSavedChart2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnWorkflows2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnPluginTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty, string.Empty);
        }

        private async void btnMessageTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnMessageRequestTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var linkedEntityMetadata = ((FrameworkElement)e.OriginalSource).DataContext as LinkedEntityMetadata;

            var items = contextMenu.Items.OfType<Control>();

            foreach (var menuContextDifference in items.Where(i => string.Equals(i.Uid, "menuContextDifference", StringComparison.InvariantCultureIgnoreCase)))
            {
                menuContextDifference.IsEnabled = false;
                menuContextDifference.Visibility = Visibility.Collapsed;

                if (linkedEntityMetadata != null
                     && linkedEntityMetadata.EntityMetadata1 != null
                     && linkedEntityMetadata.EntityMetadata2 != null
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
                     && linkedEntityMetadata.EntityMetadata2 != null
                     )
                {
                    menuContextConnection2.IsEnabled = true;
                    menuContextConnection2.Visibility = Visibility.Visible;
                }
            }
        }

        private RibbonLocationFilters GetRibbonLocationFilters()
        {
            RibbonLocationFilters filter = RibbonLocationFilters.All;

            if (_commonConfig.ExportRibbonXmlForm || _commonConfig.ExportRibbonXmlHomepageGrid || _commonConfig.ExportRibbonXmlSubGrid)
            {
                filter = 0;

                if (_commonConfig.ExportRibbonXmlForm)
                {
                    filter |= RibbonLocationFilters.Form;
                }

                if (_commonConfig.ExportRibbonXmlHomepageGrid)
                {
                    filter |= RibbonLocationFilters.HomepageGrid;
                }

                if (_commonConfig.ExportRibbonXmlSubGrid)
                {
                    filter |= RibbonLocationFilters.SubGrid;
                }
            }

            return filter;
        }

        private void mIDifferenceEntityRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null || string.IsNullOrEmpty(entity.LogicalName))
            {
                return;
            }

            ExecuteDifferenceEntityRibbon(entity.LogicalName);
        }

        private async Task ExecuteDifferenceEntityRibbon(string entityName)
        {
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

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingEntityRibbonConnectionFormat3, entityName, service1.ConnectionData.Name, service1.ConnectionData.Name);

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceRibbonForEntityFormat1, entityName);

            try
            {
                string fileName1 = EntityFileNameFormatter.GetEntityRibbonFileName(service1.ConnectionData.Name, entityName);
                string filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));

                string filePath2 = string.Empty;

                var filter = GetRibbonLocationFilters();

                var repository1 = new RibbonCustomizationRepository(service1);

                var task1 = repository1.ExportEntityRibbonAsync(entityName, filter, filePath1, _commonConfig);

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    var repository2 = new RibbonCustomizationRepository(service2);

                    string fileName2 = EntityFileNameFormatter.GetEntityRibbonFileName(service2.ConnectionData.Name, entityName);
                    filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName2));

                    var task2 = repository2.ExportEntityRibbonAsync(entityName, filter, filePath2, _commonConfig);

                    await task2;
                }

                await task1;

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service1.ConnectionData.Name, entityName, filePath1);
                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service2.ConnectionData.Name, entityName, filePath2);
                }

                if (File.Exists(filePath1) && File.Exists(filePath2))
                {
                    this._iWriteToOutput.ProcessStartProgramComparer(this._commonConfig, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                }
                else
                {
                    this._iWriteToOutput.PerformAction(filePath1, _commonConfig);

                    this._iWriteToOutput.PerformAction(filePath2, _commonConfig);
                }

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonForEntityCompletedFormat1, entityName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonForEntityFailedFormat1, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingEntityRibbonConnectionFormat3, entityName, service1.ConnectionData.Name, service1.ConnectionData.Name);
        }

        private void mIDifferenceEntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null || string.IsNullOrEmpty(entity.LogicalName))
            {
                return;
            }

            ExecuteDifferenceEntityRibbonDiffXml(entity);
        }

        private async Task ExecuteDifferenceEntityRibbonDiffXml(LinkedEntityMetadata entity)
        {
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

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingEntityRibbonDiffXmlConnectionFormat3, entity.LogicalName, service1.ConnectionData.Name, service1.ConnectionData.Name);

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceRibbonDiffXmlForEntityFormat1, entity.LogicalName);

            try
            {
                var solutionUniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());
                solutionUniqueName = solutionUniqueName.Replace("-", "_");

                string filePath1 = string.Empty;
                string filePath2 = string.Empty;

                Solution solution1 = null;
                Solution solution2 = null;

                Task<string> task1 = null;
                Task<string> task2 = null;

                {
                    var repositoryPublisher1 = new PublisherRepository(service1);

                    var publisherDefault1 = await repositoryPublisher1.GetDefaultPublisherAsync();

                    if (publisherDefault1 != null)
                    {
                        solution1 = new Solution()
                        {
                            UniqueName = solutionUniqueName,
                            FriendlyName = solutionUniqueName,

                            Description = "Temporary solution for exporting RibbonDiffXml.",

                            PublisherId = publisherDefault1.ToEntityReference(),

                            Version = "1.0.0.0",
                        };

                        solution1.Id = service1.Create(solution1);

                        {
                            var repositorySolutionComponent = new SolutionComponentRepository(service1);

                            await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.Entity),
                                ObjectId = entity.EntityMetadata1.MetadataId.Value,
                                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                            }});
                        }

                        var repository = new ExportSolutionHelper(service1);

                        task1 = repository.ExportSolutionAndGetRibbonDiffAsync(solutionUniqueName, entity.LogicalName);
                    }
                }

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    var repositoryPublisher2 = new PublisherRepository(service2);

                    var publisherDefault2 = await repositoryPublisher2.GetDefaultPublisherAsync();

                    if (publisherDefault2 != null)
                    {
                        solution2 = new Solution()
                        {
                            UniqueName = solutionUniqueName,
                            FriendlyName = solutionUniqueName,

                            Description = "Temporary solution for exporting RibbonDiffXml.",

                            PublisherId = publisherDefault2.ToEntityReference(),

                            Version = "1.0.0.0",
                        };

                        solution2.Id = service2.Create(solution2);

                        {
                            var repositorySolutionComponent = new SolutionComponentRepository(service2);

                            await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.Entity),
                                ObjectId = entity.EntityMetadata2.MetadataId.Value,
                                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                            }});
                        }

                        var repository = new ExportSolutionHelper(service2);

                        task2 = repository.ExportSolutionAndGetRibbonDiffAsync(solutionUniqueName, entity.LogicalName);
                    }
                }

                if (task1 != null)
                {
                    string ribbonDiffXml = await task1;

                    if (_commonConfig.SetXmlSchemasDuringExport)
                    {
                        var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaRibbonXml);

                        if (schemasResources != null)
                        {
                            ribbonDiffXml = ContentCoparerHelper.ReplaceXsdSchema(ribbonDiffXml, schemasResources);
                        }
                    }

                    if (_commonConfig.SetIntellisenseContext)
                    {
                        ribbonDiffXml = ContentCoparerHelper.SetRibbonDiffXmlIntellisenseContextEntityName(ribbonDiffXml, entity.LogicalName);
                    }

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    string fileName1 = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service1.ConnectionData.Name, entity.LogicalName);
                    filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));

                    File.WriteAllText(filePath1, ribbonDiffXml, new UTF8Encoding(false));

                    if (solution1 != null)
                    {
                        await service1.DeleteAsync(solution1.LogicalName, solution1.Id);
                    }
                }

                if (task2 != null)
                {
                    string ribbonDiffXml = await task2;

                    if (_commonConfig.SetXmlSchemasDuringExport)
                    {
                        var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaRibbonXml);

                        if (schemasResources != null)
                        {
                            ribbonDiffXml = ContentCoparerHelper.ReplaceXsdSchema(ribbonDiffXml, schemasResources);
                        }
                    }

                    if (_commonConfig.SetIntellisenseContext)
                    {
                        ribbonDiffXml = ContentCoparerHelper.SetRibbonDiffXmlIntellisenseContextEntityName(ribbonDiffXml, entity.LogicalName);
                    }

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    string fileName2 = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service2.ConnectionData.Name, entity.LogicalName);
                    filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName2));

                    File.WriteAllText(filePath2, ribbonDiffXml, new UTF8Encoding(false));

                    if (solution2 != null)
                    {
                        await service2.DeleteAsync(solution2.LogicalName, solution2.Id);
                    }
                }

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service1.ConnectionData.Name, entity.LogicalName, filePath1);
                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service2.ConnectionData.Name, entity.LogicalName, filePath2);
                }

                if (File.Exists(filePath1) && File.Exists(filePath2))
                {
                    this._iWriteToOutput.ProcessStartProgramComparer(this._commonConfig, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                }
                else
                {
                    this._iWriteToOutput.PerformAction(filePath1, _commonConfig);

                    this._iWriteToOutput.PerformAction(filePath2, _commonConfig);
                }

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonDiffXmlForEntityCompletedFormat1, entity.LogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonDiffXmlForEntityFailedFormat1, entity.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingEntityRibbonDiffXmlConnectionFormat3, entity.LogicalName, service1.ConnectionData.Name, service1.ConnectionData.Name);
        }

        private void mIConnection1EntityRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null || string.IsNullOrEmpty(entity.LogicalName))
            {
                return;
            }

            ExecuteCreatingEntityRibbon(GetService1, entity.LogicalName);
        }

        private void mIConnection2EntityRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null || string.IsNullOrEmpty(entity.LogicalName))
            {
                return;
            }

            ExecuteCreatingEntityRibbon(GetService2, entity.LogicalName);
        }

        private async Task ExecuteCreatingEntityRibbon(Func<Task<IOrganizationServiceExtented>> getService, string entityName)
        {
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

            var filters = GetRibbonLocationFilters();

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingRibbonForEntityFormat1, entityName);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonForEntityFormat1, entityName);

            try
            {
                var service = await getService();

                string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityName);
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new RibbonCustomizationRepository(service);

                await repository.ExportEntityRibbonAsync(entityName, filters, filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service.ConnectionData.Name, entityName, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityCompletedFormat1, entityName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityFailedFormat1, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingRibbonForEntityFormat1, entityName);
        }

        private void mIConnection1EntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null || string.IsNullOrEmpty(entity.LogicalName))
            {
                return;
            }

            ExecuteCreatingEntityRibbonDiffXml(GetService1, entity.EntityMetadata1);
        }

        private void mIConnection2EntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null || string.IsNullOrEmpty(entity.LogicalName))
            {
                return;
            }

            ExecuteCreatingEntityRibbonDiffXml(GetService2, entity.EntityMetadata2);
        }

        private async Task ExecuteCreatingEntityRibbonDiffXml(Func<Task<IOrganizationServiceExtented>> getService, EntityMetadata entity)
        {
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

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat1, entity.LogicalName);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFormat1, entity.LogicalName);

            var service = await getService();

            var repositoryPublisher = new PublisherRepository(service);
            var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

            if (publisherDefault == null)
            {
                ToggleControls(true, Properties.WindowStatusStrings.NotFoundedDefaultPublisher);
                _iWriteToOutput.ActivateOutputWindow();
                return;
            }

            var solutionUniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());
            solutionUniqueName = solutionUniqueName.Replace("-", "_");

            var solution = new Solution()
            {
                UniqueName = solutionUniqueName,
                FriendlyName = solutionUniqueName,

                Description = "Temporary solution for exporting RibbonDiffXml.",

                PublisherId = publisherDefault.ToEntityReference(),

                Version = "1.0.0.0",
            };

            UpdateStatus(Properties.WindowStatusStrings.CreatingNewSolutionFormat1, solutionUniqueName);

            solution.Id = service.Create(solution);

            string finalStatus = string.Empty;

            try
            {
                UpdateStatus(Properties.WindowStatusStrings.AddingInSolutionEntityFormat2, solutionUniqueName, entity.LogicalName);

                {
                    var repositorySolutionComponent = new SolutionComponentRepository(service);

                    await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                    {
                        ComponentType = new OptionSetValue((int)ComponentType.Entity),
                        ObjectId = entity.MetadataId.Value,
                        RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                    }});
                }

                UpdateStatus(Properties.WindowStatusStrings.ExportingSolutionAndExtractingRibbonDiffXmlForEntityFormat2, solutionUniqueName, entity.LogicalName);

                var repository = new ExportSolutionHelper(service);

                string ribbonDiffXml = await repository.ExportSolutionAndGetRibbonDiffAsync(solutionUniqueName, entity.LogicalName);

                if (_commonConfig.SetXmlSchemasDuringExport)
                {
                    var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaRibbonXml);

                    if (schemasResources != null)
                    {
                        ribbonDiffXml = ContentCoparerHelper.ReplaceXsdSchema(ribbonDiffXml, schemasResources);
                    }
                }

                if (_commonConfig.SetIntellisenseContext)
                {
                    ribbonDiffXml = ContentCoparerHelper.SetRibbonDiffXmlIntellisenseContextEntityName(ribbonDiffXml, entity.LogicalName);
                }

                ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                {
                    string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entity.LogicalName);
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service.ConnectionData.Name, entity.LogicalName, filePath);

                    this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                }

                finalStatus = string.Format(Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityCompletedFormat1, entity.LogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                finalStatus = string.Format(Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFailedFormat1, entity.LogicalName);
            }
            finally
            {
                UpdateStatus(Properties.WindowStatusStrings.DeletingSolutionFormat1, solutionUniqueName);
                await service.DeleteAsync(solution.LogicalName, solution.Id);
            }

            ToggleControls(true, finalStatus);

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat1, entity.LogicalName);
        }

        private void miExportEntityMetadataOptions_Click(object sender, RoutedEventArgs e)
        {
            _optionsExportEntityMetadata.Focus();
            _optionsExportEntityMetadata.IsOpen = true;
        }

        private void miExportAttributesDependentComponentsOptions_Click(object sender, RoutedEventArgs e)
        {
            _optionsExportAttributesDependentComponents.Focus();
            _optionsExportAttributesDependentComponents.IsOpen = true;
        }

        private void miExportEntityRibbonOptions_Click(object sender, RoutedEventArgs e)
        {
            _optionsEntityRibbon.Focus();
            _optionsEntityRibbon.IsOpen = true;
        }
    }
}