using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
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
    public partial class WindowOrganizationComparerGlobalOptionSets : WindowWithConnectionList
    {
        private readonly Dictionary<Guid, SolutionComponentMetadataSource> _cacheMetadataSource = new Dictionary<Guid, SolutionComponentMetadataSource>();

        private readonly Dictionary<Guid, IEnumerable<OptionSetMetadata>> _cacheOptionSetMetadata = new Dictionary<Guid, IEnumerable<OptionSetMetadata>>();

        private readonly ObservableCollection<LinkedOptionSetMetadata> _itemsSource;

        private readonly Popup _optionsPopup;
        private readonly FileGenerationGlobalOptionSetMetadataOptionsControl _optionsControl;

        public WindowOrganizationComparerGlobalOptionSets(
          IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter
            , string filterEntityName
        ) : base(iWriteToOutput, commonConfig, connection1)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            LoadEntityNames(cmBEntityName, connection1, connection2);

            cmBEntityName.Text = filterEntityName;

            _optionsControl = new FileGenerationGlobalOptionSetMetadataOptionsControl();
            _optionsControl.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = _optionsControl,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            this.Resources["ConnectionName1"] = connection1.Name;
            this.Resources["ConnectionName2"] = connection2.Name;

            LoadFromConfig();

            txtBFilter.Text = filter;

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<LinkedOptionSetMetadata>();

            this.lstVwOptionSets.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingOptionSets();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper1 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService1
                , getEntityName: GetEntityName
                , getGlobalOptionSetName: GetGlobalOptionSetName
                , getGlobalOptionSetMetadataList: GetGlobalOptionSetMetadataList
            );

            var explorersHelper2 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService2
                , getEntityName: GetEntityName
                , getGlobalOptionSetName: GetGlobalOptionSetName
                , getGlobalOptionSetMetadataList: GetGlobalOptionSetMetadataList
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetConnection1(), GetConnection2())
                , getEntityName: GetEntityName
                , getGlobalOptionSetName: GetGlobalOptionSetName
            );

            explorersHelper1.FillExplorers(miExplorers1);
            explorersHelper2.FillExplorers(miExplorers2);

            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu listContextMenu
            )
            {
                explorersHelper1.FillExplorers(listContextMenu, "miExplorers1");

                explorersHelper1.FillExplorers(listContextMenu, "miExplorers2");

                compareWindowsHelper.FillCompareWindows(listContextMenu, "miCompareOrganizations");
            }
        }

        private string GetEntityName()
        {
            if (!string.IsNullOrEmpty(cmBEntityName.Text)
                && cmBEntityName.Items.Contains(cmBEntityName.Text)
            )
            {
                return cmBEntityName.Text.Trim().ToLower();
            }

            return null;
        }

        private string GetGlobalOptionSetName()
        {
            var entity = GetSelectedEntity();

            return entity?.Name ?? txtBFilter.Text.Trim();
        }

        private IEnumerable<OptionSetMetadata> GetGlobalOptionSetMetadataList(Guid connectionId)
        {
            if (_cacheOptionSetMetadata.ContainsKey(connectionId))
            {
                return _cacheOptionSetMetadata[connectionId];
            }

            return null;
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            FileGenerationConfiguration.SaveConfiguration();

            (cmBConnection1.SelectedItem as ConnectionData)?.Save();
            (cmBConnection2.SelectedItem as ConnectionData)?.Save();

            BindingOperations.ClearAllBindings(cmBConnection1);
            cmBConnection1.Items.DetachFromSourceCollection();
            cmBConnection1.DataContext = null;
            cmBConnection1.ItemsSource = null;

            BindingOperations.ClearAllBindings(cmBConnection2);
            cmBConnection2.Items.DetachFromSourceCollection();
            cmBConnection2.DataContext = null;
            cmBConnection2.ItemsSource = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private ConnectionData GetConnection1()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection1.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private ConnectionData GetConnection2()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection2.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private Task<IOrganizationServiceExtented> GetService1()
        {
            return GetOrganizationService(GetConnection1());
        }

        private Task<IOrganizationServiceExtented> GetService2()
        {
            return GetOrganizationService(GetConnection2());
        }

        private SolutionComponentMetadataSource GetMetadataSource(IOrganizationServiceExtented serivce)
        {
            if (serivce != null)
            {
                if (!_cacheMetadataSource.ContainsKey(serivce.ConnectionData.ConnectionId))
                {
                    var source = new SolutionComponentMetadataSource(serivce);

                    _cacheMetadataSource[serivce.ConnectionData.ConnectionId] = source;
                }

                return _cacheMetadataSource[serivce.ConnectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingOptionSets()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingOptionSets);

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

                        var task1List = await repository1.GetOptionSetsAsync();

                        _cacheOptionSetMetadata.Add(service1.ConnectionData.ConnectionId, task1List);
                    }

                    if (!_cacheOptionSetMetadata.ContainsKey(service2.ConnectionData.ConnectionId))
                    {
                        OptionSetRepository repository2 = new OptionSetRepository(service2);

                        var task2List = await repository2.GetOptionSetsAsync();

                        _cacheOptionSetMetadata.Add(service2.ConnectionData.ConnectionId, task2List);
                    }

                    IEnumerable<OptionSetMetadata> list1 = _cacheOptionSetMetadata[service1.ConnectionData.ConnectionId];
                    IEnumerable<OptionSetMetadata> list2 = _cacheOptionSetMetadata[service2.ConnectionData.ConnectionId];

                    string entityName = string.Empty;

                    this.Dispatcher.Invoke(() =>
                    {
                        if (!string.IsNullOrEmpty(cmBEntityName.Text)
                            && cmBEntityName.Items.Contains(cmBEntityName.Text)
                        )
                        {
                            entityName = cmBEntityName.Text.Trim().ToLower();
                        }
                    });

                    string filterEntity = null;

                    if (service1.ConnectionData.IsValidEntityName(entityName)
                        && service2.ConnectionData.IsValidEntityName(entityName)
                    )
                    {
                        filterEntity = entityName;
                    }

                    if (!string.IsNullOrEmpty(filterEntity))
                    {
                        {
                            var entityId = service1.ConnectionData.GetEntityMetadataId(filterEntity);

                            if (entityId.HasValue)
                            {
                                var source = GetMetadataSource(service1);

                                var entityMetadata = await source.GetEntityMetadataAsync(entityId.Value);

                                var entityOptionSets = new HashSet<Guid>(entityMetadata
                                    .Attributes
                                    .OfType<EnumAttributeMetadata>()
                                    .Where(a => a.OptionSet != null && a.OptionSet.IsGlobal.GetValueOrDefault())
                                    .Select(a => a.OptionSet.MetadataId.Value)
                                );

                                list1 = list1.Where(o => entityOptionSets.Contains(o.MetadataId.Value));
                            }
                        }

                        {
                            var entityId = service2.ConnectionData.GetEntityMetadataId(filterEntity);

                            if (entityId.HasValue)
                            {
                                var source = GetMetadataSource(service2);

                                var entityMetadata = await source.GetEntityMetadataAsync(entityId.Value);

                                var entityOptionSets = new HashSet<Guid>(entityMetadata
                                    .Attributes
                                    .OfType<EnumAttributeMetadata>()
                                    .Where(a => a.OptionSet != null && a.OptionSet.IsGlobal.GetValueOrDefault())
                                    .Select(a => a.OptionSet.MetadataId.Value)
                                );

                                list2 = list2.Where(o => entityOptionSets.Contains(o.MetadataId.Value));
                            }
                        }
                    }

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        foreach (var optionSet1 in list1)
                        {
                            var optionSet2 = list2.FirstOrDefault(e => string.Equals(e.Name, optionSet1.Name, StringComparison.InvariantCultureIgnoreCase));

                            if (optionSet2 == null)
                            {
                                continue;
                            }

                            temp.Add(new LinkedOptionSetMetadata(optionSet1.Name, optionSet1, optionSet2));
                        }
                    }
                    else
                    {
                        foreach (var optionSet1 in list1)
                        {
                            temp.Add(new LinkedOptionSetMetadata(optionSet1.Name, optionSet1, null));
                        }
                    }

                    list = temp;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadEntities(list);

            ToggleControls(true, Properties.OutputStrings.LoadingOptionSetsCompletedFormat1, list.Count());
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
                        ent.Name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1

                        ||
                        (
                            ent.OptionSetMetadata1 != null
                            && ent.OptionSetMetadata1.DisplayName != null
                            && ent.OptionSetMetadata1.DisplayName.LocalizedLabels != null
                            && ent.OptionSetMetadata1.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )

                        ||
                        (
                            ent.OptionSetMetadata2 != null
                            && ent.OptionSetMetadata2.DisplayName != null
                            && ent.OptionSetMetadata2.DisplayName.LocalizedLabels != null
                            && ent.OptionSetMetadata2.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )
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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            ToggleControls(enabled, statusFormat, args);
        }

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.cmBConnection1, this.cmBConnection2);

            UpdateButtonsEnable();
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(null, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private string CreateFileNameJavaScript(IEnumerable<OptionSetMetadata> optionSets, ConnectionData connection)
        {
            string fileName = CreateGlobalOptionSetsFileCSharpHandler.CreateFileNameJavaScript(connection, optionSets, false);

            return Path.Combine(_commonConfig.FolderForExport, fileName);
        }

        private string CreateFileNameCSharp(IEnumerable<OptionSetMetadata> optionSets, ConnectionData connection)
        {
            string fileName = CreateGlobalOptionSetsFileCSharpHandler.CreateFileNameCSharp(connection, optionSets, false);

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

            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            var service1 = await GetService1();
            var service2 = await GetService2();

            string optionSetsName = string.Join(",", optionSets1.Select(o => o.Name).OrderBy(s => s));

            if (service1 != null && service2 != null)
            {
                this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

                ToggleControls(false, Properties.OutputStrings.CreatingFileForOptionSetsForConnectionsFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);

                try
                {
                    string filePath1 = CreateFileNameCSharp(optionSets1, service1.ConnectionData);
                    string filePath2 = CreateFileNameCSharp(optionSets2, service2.ConnectionData);

                    var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                    var config = CreateFileCSharpConfiguration.CreateForSchemaGlobalOptionSet(fileGenerationOptions);

                    var stringBuilder1 = new StringBuilder();

                    using (var stringWriter1 = new StringWriter(stringBuilder1))
                    {
                        var descriptor1 = new SolutionComponentDescriptor(service1);

                        var handler1 = new CreateGlobalOptionSetsFileCSharpHandler(stringWriter1, service1, _iWriteToOutput, descriptor1, config);

                        var task1 = handler1.CreateFileAsync(optionSets1);

                        if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                        {
                            var stringBuilder2 = new StringBuilder();

                            using (var stringWriter2 = new StringWriter(stringBuilder2))
                            {
                                var descriptor2 = new SolutionComponentDescriptor(service2);

                                var handler2 = new CreateGlobalOptionSetsFileCSharpHandler(stringWriter2, service2, _iWriteToOutput, descriptor2, config);

                                await handler2.CreateFileAsync(optionSets2);
                            }

                            File.WriteAllText(filePath2, stringBuilder2.ToString(), new UTF8Encoding(false));
                        }

                        await task1;
                    }

                    File.WriteAllText(filePath1, stringBuilder1.ToString(), new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.InConnectionCreatedGlobalOptionSetMetadataFileFormat3, service1.ConnectionData.Name, optionSetsName, filePath1);

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.InConnectionCreatedGlobalOptionSetMetadataFileFormat3, service2.ConnectionData.Name, optionSetsName, filePath2);
                    }

                    if (File.Exists(filePath1) && File.Exists(filePath2))
                    {
                        await this._iWriteToOutput.ProcessStartProgramComparerAsync(service1.ConnectionData, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2), service2.ConnectionData);
                    }
                    else
                    {
                        this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                        this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                    }

                    ToggleControls(true, Properties.OutputStrings.CreatingFileForOptionSetsForConnectionsCompletedFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(null, ex);

                    ToggleControls(true, Properties.OutputStrings.CreatingFileForOptionSetsForConnectionsFailedFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);
                }

                this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
            }
        }

        private async void btnDifferenceJavaScriptFileJsonObject_Click(object sender, RoutedEventArgs e)
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

        private async void btnDifferenceJavaScriptFileSingleJsonObject_Click(object sender, RoutedEventArgs e)
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

            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            var service1 = await GetService1();
            var service2 = await GetService2();

            string optionSetsName = string.Join(",", optionSets1.Select(o => o.Name).OrderBy(s => s));

            if (service1 != null && service2 != null)
            {
                this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

                ToggleControls(false, Properties.OutputStrings.CreatingFileForOptionSetsForConnectionsFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);

                try
                {
                    string filePath1 = CreateFileNameJavaScript(optionSets1, service1.ConnectionData);
                    string filePath2 = CreateFileNameJavaScript(optionSets2, service2.ConnectionData);

                    var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                    var tabSpacer = fileGenerationOptions.GetTabSpacer();
                    var constantType = fileGenerationOptions.GenerateSchemaConstantType;
                    var namespaceJavascript = fileGenerationOptions.NamespaceGlobalOptionSetsJavaScript;

                    var withDependentComponents = fileGenerationOptions.GenerateSchemaGlobalOptionSetsWithDependentComponents;

                    var stringBuilder1 = new StringBuilder();

                    using (var stringWriter1 = new StringWriter(stringBuilder1))
                    {
                        var descriptor1 = new SolutionComponentDescriptor(service1);

                        var handler1 = new CreateGlobalOptionSetsFileJavaScriptHandler(stringWriter1, service1, descriptor1, _iWriteToOutput, tabSpacer, withDependentComponents, namespaceJavascript);

                        var task1 = handler1.CreateFileAsync(optionSets1);

                        if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                        {
                            var stringBuilder2 = new StringBuilder();

                            using (var stringWriter2 = new StringWriter(stringBuilder2))
                            {
                                var descriptor2 = new SolutionComponentDescriptor(service2);

                                var handler2 = new CreateGlobalOptionSetsFileJavaScriptHandler(stringWriter2, service2, descriptor2, _iWriteToOutput, tabSpacer, withDependentComponents, namespaceJavascript);

                                await handler2.CreateFileAsync(optionSets2);
                            }

                            File.WriteAllText(filePath2, stringBuilder2.ToString(), new UTF8Encoding(false));
                        }

                        await task1;
                    }

                    File.WriteAllText(filePath1, stringBuilder1.ToString(), new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.InConnectionCreatedGlobalOptionSetMetadataFileFormat3, service1.ConnectionData.Name, optionSetsName, filePath1);

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.InConnectionCreatedGlobalOptionSetMetadataFileFormat3, service2.ConnectionData.Name, optionSetsName, filePath2);
                    }

                    if (File.Exists(filePath1) && File.Exists(filePath2))
                    {
                        await this._iWriteToOutput.ProcessStartProgramComparerAsync(service1.ConnectionData, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2), service2.ConnectionData);
                    }
                    else
                    {
                        this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                        this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                    }

                    ToggleControls(true, Properties.OutputStrings.CreatingFileForOptionSetsForConnectionsCompletedFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(null, ex);

                    ToggleControls(true, Properties.OutputStrings.CreatingFileForOptionSetsForConnectionsFailedFormat3, service1.ConnectionData.Name, service2.ConnectionData.Name, optionSetsName);
                }

                this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
            }
        }

        private async void btnConnection1CSharp_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;

            if (connection1 != null && _cacheOptionSetMetadata.TryGetValue(connection1.ConnectionId, out var optionSets))
            {
                await CreateGlobalOptionSetsCSharpFile(GetService1, optionSets);
            }
        }

        private async void btnConnection2CSharp_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

            if (connection2 != null && _cacheOptionSetMetadata.TryGetValue(connection2.ConnectionId, out var optionSets))
            {
                await CreateGlobalOptionSetsCSharpFile(GetService2, optionSets);
            }
        }

        private async void btnConnection1CSharpSingle_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            var optionSets = new[] { link.OptionSetMetadata1 };

            await CreateGlobalOptionSetsCSharpFile(GetService1, optionSets);
        }

        private async void btnConnection2CSharpSingle_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            var optionSets = new[] { link.OptionSetMetadata2 };

            await CreateGlobalOptionSetsCSharpFile(GetService2, optionSets);
        }

        private async Task CreateGlobalOptionSetsCSharpFile(Func<Task<IOrganizationServiceExtented>> getService, IEnumerable<OptionSetMetadata> optionSets)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (optionSets == null)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

            ToggleControls(false, Properties.OutputStrings.CreatingFileForOptionSetsFormat1, optionSetsName);

            try
            {
                var service = await getService();

                string filePath = CreateFileNameCSharp(optionSets, service.ConnectionData);

                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                var config = CreateFileCSharpConfiguration.CreateForSchemaGlobalOptionSet(fileGenerationOptions);

                var stringBuilder = new StringBuilder();

                using (var stringWriter = new StringWriter(stringBuilder))
                {
                    var descriptor = new SolutionComponentDescriptor(service);

                    var handler = new CreateGlobalOptionSetsFileCSharpHandler(stringWriter, service, _iWriteToOutput, descriptor, config);

                    await handler.CreateFileAsync(optionSets);
                }

                File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionCreatedGlobalOptionSetMetadataFileFormat3, service.ConnectionData.Name, optionSetsName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(true, Properties.OutputStrings.CreatingFileForOptionSetsCompletedFormat1, optionSetsName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.CreatingFileForOptionSetsFailedFormat1, optionSetsName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
        }

        private async void btnConnection1JavaScriptJsonObject_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;

            if (connection1 != null && _cacheOptionSetMetadata.TryGetValue(connection1.ConnectionId, out var optionSets))
            {
                await CreateGlobalOptionSetsJavaScriptFile(GetService1, optionSets);
            }
        }

        private async void btnConnection2JavaScriptJsonObject_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

            if (connection2 != null && _cacheOptionSetMetadata.TryGetValue(connection2.ConnectionId, out var optionSets))
            {
                await CreateGlobalOptionSetsJavaScriptFile(GetService2, optionSets);
            }
        }

        private async void btnConnection1JavaScriptSingleJsonObject_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            var optionSets = new[] { link.OptionSetMetadata1 };

            await CreateGlobalOptionSetsJavaScriptFile(GetService1, optionSets);
        }

        private async void btnConnection2JavaScriptSingleJsonObject_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            var optionSets = new[] { link.OptionSetMetadata2 };

            await CreateGlobalOptionSetsJavaScriptFile(GetService2, optionSets);
        }

        private async Task CreateGlobalOptionSetsJavaScriptFile(Func<Task<IOrganizationServiceExtented>> getService, IEnumerable<OptionSetMetadata> optionSets)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (optionSets == null || !optionSets.Any())
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

            ToggleControls(false, Properties.OutputStrings.CreatingFileForOptionSetsFormat1, optionSetsName);

            try
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                string tabSpacer = fileGenerationOptions.GetTabSpacer();

                var withDependentComponents = fileGenerationOptions.GenerateSchemaGlobalOptionSetsWithDependentComponents;

                var namespaceJavaScript = fileGenerationOptions.NamespaceGlobalOptionSetsJavaScript;

                var service = await getService();

                string filePath = CreateFileNameJavaScript(optionSets, service.ConnectionData);

                var stringBuilder = new StringBuilder();

                using (var stringWriter = new StringWriter(stringBuilder))
                {
                    var descriptor = new SolutionComponentDescriptor(service);

                    var handler = new CreateGlobalOptionSetsFileJavaScriptHandler(stringWriter, service, descriptor, _iWriteToOutput, tabSpacer, withDependentComponents, namespaceJavaScript);

                    await handler.CreateFileAsync(optionSets);
                }

                File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

                var message = string.Empty;

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionCreatedGlobalOptionSetMetadataFileFormat3, service.ConnectionData.Name, optionSetsName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(true, Properties.OutputStrings.CreatingFileForOptionSetsCompletedFormat1, optionSetsName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.CreatingFileForOptionSetsFailedFormat1, optionSetsName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
        }

        private async void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingOptionSets();
            }
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingOptionSets();
        }

        protected override bool CanCloseWindow(KeyEventArgs e)
        {
            Popup[] _popupArray = new Popup[] { _optionsPopup };

            foreach (var popup in _popupArray)
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

        private async void lstVwOptionSets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                LinkedOptionSetMetadata item = GetItemFromRoutedDataContext<LinkedOptionSetMetadata>(e);

                if (item != null)
                {
                    var optionSets1 = new[] { item.OptionSetMetadata1 };
                    var optionSets2 = new[] { item.OptionSetMetadata2 };

                    await PerformComparingCSharpFiles(optionSets1, optionSets2);
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
                    bool enabled = this.IsControlsEnabled && this.lstVwOptionSets.SelectedItems.Count > 0;

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
            if (!this.IsControlsEnabled)
            {
                return;
            }

            foreach (var removed in e.RemovedItems.OfType<ConnectionData>())
            {
                removed.Save();
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();

                ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;
                ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

                if (connection1 != null && connection2 != null)
                {
                    this.Resources["ConnectionName1"] = connection1.Name;
                    this.Resources["ConnectionName2"] = connection2.Name;

                    LoadEntityNames(cmBEntityName, connection1, connection2);

                    UpdateButtonsEnable();

                    var task = ShowExistingOptionSets();
                }
            });
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            LinkedOptionSetMetadata linkedEntityMetadata = GetItemFromRoutedDataContext<LinkedOptionSetMetadata>(e);

            var items = contextMenu.Items.OfType<Control>();

            foreach (var menuContextDifference in items.Where(i =>
                string.Equals(i.Uid, "menuContextDifference", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(i.Uid, "miCompareOrganizations", StringComparison.InvariantCultureIgnoreCase)
            ))
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

            foreach (var menuContextConnection2 in items.Where(i =>
                string.Equals(i.Uid, "menuContextConnection2", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(i.Uid, "miExplorers2", StringComparison.InvariantCultureIgnoreCase)
            ))
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
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            this._optionsControl.BindFileGenerationOptions(fileGenerationOptions);

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

        private async void mIConnection1OpenSolutionComponentInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService1();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.OptionSet, entity.OptionSetMetadata1.MetadataId.Value);
            }
        }

        private async void mIConnection2OpenSolutionComponentInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService2();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.OptionSet, entity.OptionSetMetadata2.MetadataId.Value);
            }
        }
    }
}