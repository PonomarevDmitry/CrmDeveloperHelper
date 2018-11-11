using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowOrganizationComparerRibbon : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private Dictionary<Guid, IOrganizationServiceExtented> _cacheService = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, List<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, List<EntityMetadata>>();
        private Dictionary<Guid, HashSet<string>> _cacheRibbonCustomization = new Dictionary<Guid, HashSet<string>>();

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private ObservableCollection<LinkedEntityMetadata> _itemsSource;

        private bool _controlsEnabled = true;

        private int _init = 0;

        public WindowOrganizationComparerRibbon(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = connection1.ConnectionConfiguration;

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            tSDDBConnection1.Header = string.Format("Export from {0}", connection1.Name);
            tSDDBConnection2.Header = string.Format("Export from {0}", connection2.Name);

            this.Resources["ConnectionName1"] = string.Format("Create from {0}", connection1.Name);
            this.Resources["ConnectionName2"] = string.Format("Create from {0}", connection2.Name);

            LoadFromConfig();

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
            chBForm.DataContext = _commonConfig;
            chBHomepageGrid.DataContext = _commonConfig;
            chBSubGrid.DataContext = _commonConfig;

            cmBFileAction.DataContext = _commonConfig;

            chBXmlAttributeOnNewLine.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _commonConfig.Save();
        }

        private RibbonLocationFilters GetRibbonLocationFilters()
        {
            RibbonLocationFilters filter = RibbonLocationFilters.All;

            chBForm.Dispatcher.Invoke(() =>
            {

                if (chBForm.IsChecked.GetValueOrDefault() || chBHomepageGrid.IsChecked.GetValueOrDefault() || chBSubGrid.IsChecked.GetValueOrDefault())
                {
                    filter = 0;

                    if (chBForm.IsChecked.GetValueOrDefault())
                    {
                        filter |= RibbonLocationFilters.Form;
                    }

                    if (chBHomepageGrid.IsChecked.GetValueOrDefault())
                    {
                        filter |= RibbonLocationFilters.HomepageGrid;
                    }

                    if (chBSubGrid.IsChecked.GetValueOrDefault())
                    {
                        filter |= RibbonLocationFilters.SubGrid;
                    }
                }
            });

            return filter;
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
                    _iWriteToOutput.WriteToOutput("Connection to CRM.");
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

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
                    _iWriteToOutput.WriteToOutput("Connection to CRM.");
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

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

            ToggleControls(false, Properties.WindowStatusStrings.LoadingEntitiesFormat);

            this._itemsSource.Clear();

            IEnumerable<LinkedEntityMetadata> list = Enumerable.Empty<LinkedEntityMetadata>();
            HashSet<string> hash = new HashSet<string>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
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

                    if (!_cacheRibbonCustomization.ContainsKey(service1.ConnectionData.ConnectionId))
                    {
                        var repository = new RibbonCustomizationRepository(service1);

                        var task = repository.GetEntitiesWithRibbonCustomizationAsync();

                        _cacheRibbonCustomization.Add(service1.ConnectionData.ConnectionId, await task);
                    }

                    if (!_cacheRibbonCustomization.ContainsKey(service2.ConnectionData.ConnectionId))
                    {
                        var repository = new RibbonCustomizationRepository(service2);

                        var task = repository.GetEntitiesWithRibbonCustomizationAsync();

                        _cacheRibbonCustomization.Add(service2.ConnectionData.ConnectionId, await task);
                    }

                    List<EntityMetadata> list1 = _cacheEntityMetadata[service1.ConnectionData.ConnectionId];

                    var temp = new List<LinkedEntityMetadata>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        List<EntityMetadata> list2 = _cacheEntityMetadata[service2.ConnectionData.ConnectionId];

                        foreach (var entityMetadata1 in list1)
                        {
                            var entityMetadata2 = list2.FirstOrDefault(e => e.LogicalName == entityMetadata1.LogicalName);

                            if (entityMetadata2 == null)
                            {
                                continue;
                            }

                            temp.Add(new LinkedEntityMetadata(entityMetadata1.LogicalName, entityMetadata1, entityMetadata2));
                        }

                        if (_cacheRibbonCustomization.ContainsKey(service1.ConnectionData.ConnectionId)
                            && _cacheRibbonCustomization.ContainsKey(service2.ConnectionData.ConnectionId)
                            )
                        {
                            var hash1 = _cacheRibbonCustomization[service1.ConnectionData.ConnectionId];
                            var hash2 = _cacheRibbonCustomization[service2.ConnectionData.ConnectionId];

                            hash = new HashSet<string>(hash1.Intersect(hash2), StringComparer.InvariantCultureIgnoreCase);
                        }
                    }
                    else
                    {
                        foreach (var entityMetadata1 in list1)
                        {
                            temp.Add(new LinkedEntityMetadata(entityMetadata1.LogicalName, entityMetadata1, null));
                        }

                        if (_cacheRibbonCustomization.ContainsKey(service1.ConnectionData.ConnectionId))
                        {
                            hash = _cacheRibbonCustomization[service1.ConnectionData.ConnectionId];
                        }
                    }

                    list = temp;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            var textName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterEnitity.Text.Trim().ToLower();
            });

            list = FilterList(list, textName, hash);

            LoadEntities(list);
        }

        private static IEnumerable<LinkedEntityMetadata> FilterList(IEnumerable<LinkedEntityMetadata> list, string textName, HashSet<string> hash)
        {
            list = list.Where(ent => !(ent.EntityMetadata1?.IsIntersect).GetValueOrDefault() && !(ent.EntityMetadata2?.IsIntersect).GetValueOrDefault());

            if (hash != null && hash.Any())
            {
                list = list.Where(e => hash.Contains(e.LogicalName));
            }

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

                            || (ent.EntityMetadata1 != null && ent.EntityMetadata1.DisplayName != null && ent.EntityMetadata1.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.ToLower().Contains(textName)))

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
                    this._itemsSource.Add(entity);
                }

                if (this.lstVwEntities.Items.Count == 1)
                {
                    this.lstVwEntities.SelectedItem = this.lstVwEntities.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingEntitiesCompletedFormat, results.Count());
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

        private LinkedEntityMetadata GetSelectedEntity()
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
                    ExecuteDifferenceEntityRibbon(item.LogicalName);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void mIDifferenceApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            ExecuteDifferenceApplicationRibbon();
        }

        private async Task ExecuteDifferenceApplicationRibbon()
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

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceApplicationRibbons);

            this._iWriteToOutput.WriteToOutput("Start exporting files with Application Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                var filter = GetRibbonLocationFilters();

                string fileName1 = EntityFileNameFormatter.GetApplicationRibbonFileName(service1.ConnectionData.Name);
                string filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));

                string filePath2 = string.Empty;

                var repository1 = new RibbonCustomizationRepository(service1);

                var task1 = repository1.ExportApplicationRibbon(filter, filePath1, _commonConfig);

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    var repository2 = new RibbonCustomizationRepository(service2);

                    string fileName2 = EntityFileNameFormatter.GetApplicationRibbonFileName(service2.ConnectionData.Name);
                    filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName2));

                    var task2 = repository2.ExportApplicationRibbon(filter, filePath2, _commonConfig);

                    await task2;
                }

                await task1;

                this._iWriteToOutput.WriteToOutput("{0} ApplicationRibbon Xml exported to {1}", service1.ConnectionData.Name, filePath1);

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    this._iWriteToOutput.WriteToOutput("{0} ApplicationRibbon Xml exported to {1}", service2.ConnectionData.Name, filePath2);
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
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            this._iWriteToOutput.WriteToOutput("End exporting files with Application Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceApplicationRibbonsCompleted);
        }

        private void mIDifferenceApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            ExecuteDifferenceApplicationRibbonDiffXml();
        }

        private async Task ExecuteDifferenceApplicationRibbonDiffXml()
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

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceApplicationRibbonsDiffXml);

            this._iWriteToOutput.WriteToOutput("Start exporting files with Entity Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var solutionUniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());
                solutionUniqueName = solutionUniqueName.Replace("-", "_");

                var service1 = await GetService1();
                var service2 = await GetService2();

                string filePath1 = string.Empty;
                string filePath2 = string.Empty;

                Solution solution1 = null;
                Solution solution2 = null;

                Task<string> task1 = null;
                Task<string> task2 = null;

                {
                    var repositoryPublisher1 = new PublisherRepository(service1);
                    var publisherDefault1 = await repositoryPublisher1.GetDefaultPublisherAsync();

                    var repositoryRibbonCustomization = new RibbonCustomizationRepository(service1);
                    var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                    if (publisherDefault1 != null && ribbonCustomization != null)
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
                                ComponentType = new OptionSetValue((int)ComponentType.RibbonCustomization),
                                ObjectId = ribbonCustomization.Id,
                                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                            }});
                        }

                        var repository = new ExportSolutionHelper(service1);

                        task1 = repository.ExportSolutionAndGetApplicationRibbonDiffAsync(solutionUniqueName);
                    }
                }

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    var repositoryPublisher2 = new PublisherRepository(service2);
                    var publisherDefault2 = await repositoryPublisher2.GetDefaultPublisherAsync();

                    var repositoryRibbonCustomization = new RibbonCustomizationRepository(service2);
                    var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                    if (publisherDefault2 != null && ribbonCustomization != null)
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
                                ComponentType = new OptionSetValue((int)ComponentType.RibbonCustomization),
                                ObjectId = ribbonCustomization.Id,
                                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                            }});
                        }

                        var repository = new ExportSolutionHelper(service2);

                        task2 = repository.ExportSolutionAndGetApplicationRibbonDiffAsync(solutionUniqueName);
                    }
                }

                if (task1 != null)
                {
                    string ribbonDiffXml = await task1;

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    string fileName1 = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service1.ConnectionData.Name);
                    filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));

                    File.WriteAllText(filePath1, ribbonDiffXml, new UTF8Encoding(false));

                    if (solution1 != null)
                    {
                        service1.Delete(solution1.LogicalName, solution1.Id);
                    }
                }

                if (task2 != null)
                {
                    string ribbonDiffXml = await task2;

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    string fileName2 = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service2.ConnectionData.Name);
                    filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName2));

                    File.WriteAllText(filePath2, ribbonDiffXml, new UTF8Encoding(false));

                    if (solution2 != null)
                    {
                        service2.Delete(solution2.LogicalName, solution2.Id);
                    }
                }

                this._iWriteToOutput.WriteToOutput("{0} Application RibbonDiffXml exported to {1}", service1.ConnectionData.Name, filePath1);
                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    this._iWriteToOutput.WriteToOutput("{0} Application RibbonDiffXml exported to {1}", service2.ConnectionData.Name, filePath2);
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

                this._iWriteToOutput.WriteToOutput("End exporting files with Entity Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceApplicationRibbonsDiffXmlCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceApplicationRibbonsDiffXmlFailed);
            }
        }

        private void mIDifferenceEntityRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

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

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceRibbonForEntityFormat, entityName);

            this._iWriteToOutput.WriteToOutput("Start exporting files with Entity Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                string fileName1 = EntityFileNameFormatter.GetEntityRibbonFileName(service1.ConnectionData.Name, entityName);
                string filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));

                string filePath2 = string.Empty;

                var filter = GetRibbonLocationFilters();

                var repository1 = new RibbonCustomizationRepository(service1);

                var task1 = repository1.ExportEntityRibbon(entityName, filter, filePath1, _commonConfig);

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    var repository2 = new RibbonCustomizationRepository(service2);

                    string fileName2 = EntityFileNameFormatter.GetEntityRibbonFileName(service2.ConnectionData.Name, entityName);
                    filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName2));

                    var task2 = repository2.ExportEntityRibbon(entityName, filter, filePath2, _commonConfig);

                    await task2;
                }

                await task1;

                this._iWriteToOutput.WriteToOutput("{0} Entity {1} Ribbon Xml exported to {2}", service1.ConnectionData.Name, entityName, filePath1);
                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    this._iWriteToOutput.WriteToOutput("{0} Entity {1} Ribbon Xml exported to {2}", service2.ConnectionData.Name, entityName, filePath2);
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

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonForEntityCompletedFormat, entityName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonForEntityFailedFormat, entityName);
            }

            this._iWriteToOutput.WriteToOutput("End exporting files with Entity Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
        }

        private void mIDifferenceEntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

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

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceRibbonDiffXmlForEntityFormat, entity.LogicalName);

            this._iWriteToOutput.WriteToOutput("Start exporting files with Entity Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var solutionUniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());
                solutionUniqueName = solutionUniqueName.Replace("-", "_");

                var service1 = await GetService1();
                var service2 = await GetService2();

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

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    string fileName1 = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service1.ConnectionData.Name, entity.LogicalName);
                    filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));

                    File.WriteAllText(filePath1, ribbonDiffXml, new UTF8Encoding(false));

                    if (solution1 != null)
                    {
                        service1.Delete(solution1.LogicalName, solution1.Id);
                    }
                }

                if (task2 != null)
                {
                    string ribbonDiffXml = await task2;

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    string fileName2 = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service2.ConnectionData.Name, entity.LogicalName);
                    filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName2));

                    File.WriteAllText(filePath2, ribbonDiffXml, new UTF8Encoding(false));

                    if (solution2 != null)
                    {
                        service2.Delete(solution2.LogicalName, solution2.Id);
                    }
                }

                this._iWriteToOutput.WriteToOutput("{0} Entity {1} Ribbon Xml exported to {2}", service1.ConnectionData.Name, entity.LogicalName, filePath1);
                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    this._iWriteToOutput.WriteToOutput("{0} Entity {1} Ribbon Xml exported to {2}", service2.ConnectionData.Name, entity.LogicalName, filePath2);
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

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonDiffXmlForEntityCompletedFormat, entity.LogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonDiffXmlForEntityFailedFormat, entity.LogicalName);
            }

            this._iWriteToOutput.WriteToOutput("End exporting files with Entity Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
        }

        private void mIConnection1ApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCreatingApplicationRibbon(GetService1);
        }

        private void mIConnection2ApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCreatingApplicationRibbon(GetService2);
        }

        private async Task ExecuteCreatingApplicationRibbon(Func<Task<IOrganizationServiceExtented>> getService)
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

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForEntityFormat);

            this._iWriteToOutput.WriteToOutput("Start exporting file with Application Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var service = await getService();

                string fileName = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name);
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new RibbonCustomizationRepository(service);

                await repository.ExportApplicationRibbon(filters, filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput("{0} Application Ribbon Xml exported to {1}", service.ConnectionData.Name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            this._iWriteToOutput.WriteToOutput("End exporting file with Application Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat);
        }

        private void mIConnection1ApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCreatingApplicationRibbonDiffXml(GetService1);
        }

        private void mIConnection2ApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCreatingApplicationRibbonDiffXml(GetService2);
        }

        private async Task ExecuteCreatingApplicationRibbonDiffXml(Func<Task<IOrganizationServiceExtented>> getService)
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

            ToggleControls(false, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXml);

            var service = await getService();

            try
            {
                var repositoryPublisher = new PublisherRepository(service);

                var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

                var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

                var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                if (publisherDefault != null && ribbonCustomization != null)
                {
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

                    UpdateStatus(Properties.WindowStatusStrings.CreatingNewSolutionFormat, solutionUniqueName);

                    solution.Id = service.Create(solution);

                    UpdateStatus(Properties.WindowStatusStrings.AddingInSolutionApplicationRibbonFormat);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.RibbonCustomization),
                            ObjectId = ribbonCustomization.Id,
                            RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                        }});
                    }

                    UpdateStatus(Properties.WindowStatusStrings.ExportingSolutionAndExtractingApplicationRibbonDiffXmlFormat);

                    var repository = new ExportSolutionHelper(service);

                    string ribbonDiffXml = await repository.ExportSolutionAndGetApplicationRibbonDiffAsync(solutionUniqueName);

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    {
                        string fileName = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name);
                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput("Application RibbonDiffXml exported to {0}", filePath);

                        this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                    }

                    UpdateStatus(Properties.WindowStatusStrings.DeletingSolutionFormat, solutionUniqueName);

                    service.Delete(solution.LogicalName, solution.Id);

                    ToggleControls(true, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXmlCompleted);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXmlFailed);
            }
        }

        private void mIConnection1EntityRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || string.IsNullOrEmpty(entity.LogicalName))
            {
                return;
            }

            ExecuteCreatingEntityRibbon(GetService1, entity.LogicalName);
        }

        private void mIConnection2EntityRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

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

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonForEntityFormat, entityName);

            this._iWriteToOutput.WriteToOutput("Start exporting file with Entity Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var service = await getService();

                string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityName);
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new RibbonCustomizationRepository(service);

                await repository.ExportEntityRibbon(entityName, filters, filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput("Ribbon Xml exported to {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityCompletedFormat, entityName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityFailedFormat, entityName);
            }

            this._iWriteToOutput.WriteToOutput("End exporting file with Entity Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
        }

        private void mIConnection1EntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || string.IsNullOrEmpty(entity.LogicalName))
            {
                return;
            }

            ExecuteCreatingEntityRibbonDiffXml(GetService1, entity.EntityMetadata1);
        }

        private void mIConnection2EntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

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

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFormat, entity.LogicalName);

            this._iWriteToOutput.WriteToOutput("Start exporting file with Entity RibbonDiffXml at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var service = await getService();

                var repositoryPublisher = new PublisherRepository(service);

                var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

                if (publisherDefault != null)
                {
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

                    UpdateStatus(Properties.WindowStatusStrings.CreatingNewSolutionFormat, solutionUniqueName);

                    solution.Id = service.Create(solution);

                    UpdateStatus(Properties.WindowStatusStrings.AddingInSolutionEntityFormat, solutionUniqueName, entity.LogicalName);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                            ObjectId = entity.MetadataId.Value,
                            RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                        }});
                    }

                    UpdateStatus(Properties.WindowStatusStrings.ExportingSolutionAndExtractingRibbonDiffXmlForEntityFormat, entity.LogicalName);

                    var repository = new ExportSolutionHelper(service);

                    string ribbonDiffXml = await repository.ExportSolutionAndGetRibbonDiffAsync(solutionUniqueName, entity.LogicalName);

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    {
                        string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entity.LogicalName);
                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput("RibbonDiff Xml exported to {0}", filePath);

                        this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                    }

                    UpdateStatus(Properties.WindowStatusStrings.DeletingSolutionFormat, solutionUniqueName);

                    service.Delete(solution.LogicalName, solution.Id);

                    ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityCompletedFormat, entity.LogicalName);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFailedFormat, entity.LogicalName);
            }

            this._iWriteToOutput.WriteToOutput("End exporting file with Entity Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
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
                    tSDDBConnection1.Header = string.Format("Export from {0}", connection1.Name);
                    tSDDBConnection2.Header = string.Format("Export from {0}", connection2.Name);

                    this.Resources["ConnectionName1"] = string.Format("Create from {0}", connection1.Name);
                    this.Resources["ConnectionName2"] = string.Format("Create from {0}", connection2.Name);

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

        private async void btnCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.LogicalName);
        }

        private async void btnEntityAttributeExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityAttributeExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityRelationshipOneToManyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityRelationshipOneToManyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityRelationshipManyToManyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityRelationshipManyToManyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityKeyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityKeyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnCreateMetadataFile1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList, null);
        }

        private async void btnExportRibbon1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntityRibbonWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList);
        }

        private async void btnGlobalOptionSets1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

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
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnSavedQuery1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnSavedChart1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnWorkflows1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnAttributesDependentComponent1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenAttributesDependentComponentWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList);
        }

        private async void btnPluginTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty, string.Empty);
        }

        private async void btnMessageTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnMessageRequestTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnCreateMetadataFile2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList, null);
        }

        private async void btnExportRibbon2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntityRibbonWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList);
        }

        private async void btnGlobalOptionSets2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

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
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnSavedQuery2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnSavedChart2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnWorkflows2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnAttributesDependentComponent2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenAttributesDependentComponentWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList);
        }

        private async void btnPluginTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty, string.Empty);
        }

        private async void btnMessageTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnMessageRequestTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

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
    }
}