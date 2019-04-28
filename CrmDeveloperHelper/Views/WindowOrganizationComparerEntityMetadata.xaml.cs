using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.CodeDom.Compiler;
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

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly Popup _optionsPopup;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _cacheService = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly Dictionary<Guid, List<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, List<EntityMetadata>>();

        private readonly CommonConfiguration _commonConfig;

        private readonly ObservableCollection<LinkedEntityMetadata> _itemsSource;

        public WindowOrganizationComparerEntityMetadata(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string entityFilter
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;

            BindingOperations.EnableCollectionSynchronization(connection1.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            var child = new ExportEntityMetadataOptionsControl(_commonConfig);
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

            txtBFilterEnitity.Text = entityFilter;

            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;

            txtBFilterEnitity.Focus();

            this._itemsSource = new ObservableCollection<LinkedEntityMetadata>();

            this.lstVwEntities.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            this.DecreaseInit();

            ShowExistingEntities();
        }

        private void LoadFromConfig()
        {
            txtBNamespaceClasses1.DataContext = cmBConnection1;
            txtBNamespaceClasses2.DataContext = cmBConnection2;

            txtBNamespaceJavaScript1.DataContext = cmBConnection1;
            txtBNamespaceJavaScript2.DataContext = cmBConnection2;

            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();

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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _cacheService[connectionData.ConnectionId] = service;
                }

                return _cacheService[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingEntities()
        {
            if (!this.IsControlsEnabled)
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
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
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

            _iWriteToOutput.WriteToOutput(null, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.cmBConnection1, this.cmBConnection2);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwEntities.SelectedItems.Count > 0;

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
                    ExecuteDifferenceCSharpSchema(item);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void btnDifferenceCSharpFileSchema_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null
                || string.IsNullOrEmpty(entity.LogicalName)
            )
            {
                return;
            }

            ExecuteDifferenceCSharpSchema(entity);
        }

        private async Task ExecuteDifferenceCSharpSchema(LinkedEntityMetadata linkedEntityMetadata)
        {
            if (!this.IsControlsEnabled)
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

            var service1 = await GetService1();
            var service2 = await GetService2();

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityConnectionsFormat3, linkedEntityMetadata.LogicalName, service1.ConnectionData.Name, service2.ConnectionData.Name);

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceEntityMetadataCSharpForEntityFormat1, linkedEntityMetadata.LogicalName);

            CreateFileWithEntityMetadataCSharpConfiguration config = GetCSharpConfigSchema(linkedEntityMetadata.LogicalName);

            string fileName1 = string.Format("{0}.{1}.Generated.cs", service1.ConnectionData.Name, linkedEntityMetadata.EntityMetadata1.SchemaName);
            string fileName2 = string.Format("{0}.{1}.Generated.cs", service2.ConnectionData.Name, linkedEntityMetadata.EntityMetadata2.SchemaName);

            string filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));
            string filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName2));

            var repository1 = new EntityMetadataRepository(service1);
            var repository2 = new EntityMetadataRepository(service2);

            ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
            ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);

            INamingService namingService1 = new NamingService(service1.ConnectionData.ServiceContextName, config);
            INamingService namingService2 = new NamingService(service2.ConnectionData.ServiceContextName, config);
            ITypeMappingService typeMappingService1 = new TypeMappingService(service1.ConnectionData.NamespaceClasses);
            ITypeMappingService typeMappingService2 = new TypeMappingService(service2.ConnectionData.NamespaceClasses);
            IMetadataProviderService metadataProviderService1 = new MetadataProviderService(repository1);
            IMetadataProviderService metadataProviderService2 = new MetadataProviderService(repository2);

            ICodeGenerationServiceProvider codeGenerationServiceProvider1 = new CodeGenerationServiceProvider(typeMappingService1, codeGenerationService, codeWriterFilterService, metadataProviderService1, namingService1);
            ICodeGenerationServiceProvider codeGenerationServiceProvider2 = new CodeGenerationServiceProvider(typeMappingService2, codeGenerationService, codeWriterFilterService, metadataProviderService2, namingService2);

            using (var handler1 = new CreateFileWithEntityMetadataCSharpHandler(config, service1, _iWriteToOutput, codeGenerationServiceProvider1))
            {
                var task1 = handler1.CreateFileAsync(filePath1);

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    using (var handler2 = new CreateFileWithEntityMetadataCSharpHandler(config, service2, _iWriteToOutput, codeGenerationServiceProvider2))
                    {
                        await handler2.CreateFileAsync(filePath2);
                    }
                }

                await task1;
            }

            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service1.ConnectionData.Name, config.EntityName, filePath1);

            if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service2.ConnectionData.Name, config.EntityName, filePath2);
            }

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
            }
            else
            {
                this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceEntityMetadataCSharpForEntityCompletedFormat1, linkedEntityMetadata.LogicalName);

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityConnectionsFormat3, linkedEntityMetadata.LogicalName, service1.ConnectionData.Name, service2.ConnectionData.Name);
        }

        private void btnDifferenceCSharpFileProxyClass_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null
                || string.IsNullOrEmpty(entity.LogicalName)
            )
            {
                return;
            }

            ExecuteDifferenceCSharpProxyClass(entity);
        }

        private async Task ExecuteDifferenceCSharpProxyClass(LinkedEntityMetadata linkedEntityMetadata)
        {
            if (!this.IsControlsEnabled)
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

            var service1 = await GetService1();
            var service2 = await GetService2();

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityConnectionsFormat3, linkedEntityMetadata.LogicalName, service1.ConnectionData.Name, service2.ConnectionData.Name);

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceEntityMetadataCSharpForEntityFormat1, linkedEntityMetadata.LogicalName);

            CreateFileWithEntityMetadataCSharpConfiguration config = GetCSharpConfigProxyClass(linkedEntityMetadata.LogicalName);

            string fileName1 = string.Format("{0}.{1}.cs", service1.ConnectionData.Name, linkedEntityMetadata.EntityMetadata1.SchemaName);
            string fileName2 = string.Format("{0}.{1}.cs", service2.ConnectionData.Name, linkedEntityMetadata.EntityMetadata2.SchemaName);

            string filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));
            string filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName2));

            var repository1 = new EntityMetadataRepository(service1);
            var repository2 = new EntityMetadataRepository(service2);

            ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
            ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);

            INamingService namingService1 = new NamingService(service1.ConnectionData.ServiceContextName, config);
            INamingService namingService2 = new NamingService(service2.ConnectionData.ServiceContextName, config);
            ITypeMappingService typeMappingService1 = new TypeMappingService(service1.ConnectionData.NamespaceClasses);
            ITypeMappingService typeMappingService2 = new TypeMappingService(service2.ConnectionData.NamespaceClasses);
            IMetadataProviderService metadataProviderService1 = new MetadataProviderService(repository1);
            IMetadataProviderService metadataProviderService2 = new MetadataProviderService(repository2);

            ICodeGenerationServiceProvider codeGenerationServiceProvider1 = new CodeGenerationServiceProvider(typeMappingService1, codeGenerationService, codeWriterFilterService, metadataProviderService1, namingService1);
            ICodeGenerationServiceProvider codeGenerationServiceProvider2 = new CodeGenerationServiceProvider(typeMappingService2, codeGenerationService, codeWriterFilterService, metadataProviderService2, namingService2);

            var entityMetadataFull1 = await repository1.GetEntityMetadataAsync(linkedEntityMetadata.LogicalName);
            var entityMetadataFull2 = await repository2.GetEntityMetadataAsync(linkedEntityMetadata.LogicalName);

            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BlankLinesBetweenMembers = true,
                BracingStyle = "C",
                IndentString = config.TabSpacer,
                VerbatimOrder = true,
            };

            var task1 = codeGenerationService.WriteEntityFileAsync(entityMetadataFull1, "CSharp", filePath1, service1.ConnectionData.NamespaceClasses, options, codeGenerationServiceProvider1);

            if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
            {
                await codeGenerationService.WriteEntityFileAsync(entityMetadataFull2, "CSharp", filePath2, service2.ConnectionData.NamespaceClasses, options, codeGenerationServiceProvider2);
            }

            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service1.ConnectionData.Name, config.EntityName, filePath1);

            if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service2.ConnectionData.Name, config.EntityName, filePath2);
            }

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
            }
            else
            {
                this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceEntityMetadataCSharpForEntityCompletedFormat1, linkedEntityMetadata.LogicalName);

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityConnectionsFormat3, linkedEntityMetadata.LogicalName, service1.ConnectionData.Name, service2.ConnectionData.Name);
        }

        private CreateFileWithEntityMetadataCSharpConfiguration GetCSharpConfigSchema(string entityName)
        {
            string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

            var result = new CreateFileWithEntityMetadataCSharpConfiguration
            (
                entityName
                , tabSpacer
                , _commonConfig.GenerateAttributesSchema
                , _commonConfig.GenerateStatusOptionSetSchema
                , _commonConfig.GenerateLocalOptionSetSchema
                , _commonConfig.GenerateGlobalOptionSetSchema
                , _commonConfig.GenerateOneToManySchema
                , _commonConfig.GenerateManyToOneSchema
                , _commonConfig.GenerateManyToManySchema
                , _commonConfig.GenerateKeysSchema
                , _commonConfig.AllDescriptions
                , _commonConfig.EntityMetadaOptionSetDependentComponents
                , _commonConfig.GenerateIntoSchemaClass
                , _commonConfig.SolutionComponentWithManagedInfo
                , _commonConfig.ConstantType
                , _commonConfig.OptionSetExportType
                , _commonConfig.GenerateAttributesProxyClassWithNameOf
                , _commonConfig.GenerateProxyClassesWithDebuggerNonUserCode
                , _commonConfig.GenerateProxyClassesUseSchemaConstInCSharpAttributes
                , _commonConfig.GenerateProxyClassesWithoutObsoleteAttribute
                , _commonConfig.GenerateProxyClassesMakeAllPropertiesEditable
                , _commonConfig.GenerateProxyClassesAddConstructorWithAnonymousTypeObject
            );

            return result;
        }

        private CreateFileWithEntityMetadataCSharpConfiguration GetCSharpConfigProxyClass(string entityName)
        {
            string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

            var result = new CreateFileWithEntityMetadataCSharpConfiguration
            (
                entityName
                , tabSpacer
                , _commonConfig.GenerateAttributesProxyClass
                , _commonConfig.GenerateStatusOptionSetProxyClass
                , _commonConfig.GenerateLocalOptionSetProxyClass
                , _commonConfig.GenerateGlobalOptionSetProxyClass
                , _commonConfig.GenerateOneToManyProxyClass
                , _commonConfig.GenerateManyToOneProxyClass
                , _commonConfig.GenerateManyToManyProxyClass
                , false
                , _commonConfig.AllDescriptions
                , _commonConfig.EntityMetadaOptionSetDependentComponents
                , _commonConfig.GenerateIntoSchemaClass
                , _commonConfig.SolutionComponentWithManagedInfo
                , _commonConfig.ConstantType
                , _commonConfig.OptionSetExportType
                , _commonConfig.GenerateAttributesProxyClassWithNameOf
                , _commonConfig.GenerateProxyClassesWithDebuggerNonUserCode
                , _commonConfig.GenerateProxyClassesUseSchemaConstInCSharpAttributes
                , _commonConfig.GenerateProxyClassesWithoutObsoleteAttribute
                , _commonConfig.GenerateProxyClassesMakeAllPropertiesEditable
                , _commonConfig.GenerateProxyClassesAddConstructorWithAnonymousTypeObject
            );

            return result;
        }

        private void btnDifferenceJavaScriptFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null
                || string.IsNullOrEmpty(entity.LogicalName)
            )
            {
                return;
            }

            ExecuteDifferenceJavaScript(entity);
        }

        private async Task ExecuteDifferenceJavaScript(LinkedEntityMetadata linkedEntityMetadata)
        {
            if (!this.IsControlsEnabled)
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

            var service1 = await GetService1();
            var service2 = await GetService2();

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityConnectionsFormat3, linkedEntityMetadata.LogicalName, service1.ConnectionData.Name, service2.ConnectionData.Name);

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceEntityMetadataJavaScriptForEntityFormat1, linkedEntityMetadata.LogicalName);

            string filename1 = string.Format("{0}.{1}.entitymetadata.generated.js", service1.ConnectionData.Name, linkedEntityMetadata.LogicalName);
            string filename2 = string.Format("{0}.{1}.entitymetadata.generated.js", service2.ConnectionData.Name, linkedEntityMetadata.LogicalName);

            CreateFileWithEntityMetadataJavaScriptConfiguration config = GetJavaScriptConfig(linkedEntityMetadata.LogicalName);

            string filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(filename1));
            string filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(filename2));

            using (var handler1 = new CreateFileWithEntityMetadataJavaScriptHandler(config, service1, _iWriteToOutput))
            {
                var task1 = handler1.CreateFileAsync(filePath1);

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    using (var handler2 = new CreateFileWithEntityMetadataJavaScriptHandler(config, service2, _iWriteToOutput))
                    {
                        await handler2.CreateFileAsync(filePath2);
                    }
                }

                await task1;
            }

            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service1.ConnectionData.Name, config.EntityName, filePath1);

            if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service2.ConnectionData.Name, config.EntityName, filePath2);
            }

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
            }
            else
            {
                this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceEntityMetadataJavaScriptForEntityCompletedFormat1, linkedEntityMetadata.LogicalName);

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityConnectionsFormat3, linkedEntityMetadata.LogicalName, service1.ConnectionData.Name, service2.ConnectionData.Name);
        }

        private CreateFileWithEntityMetadataJavaScriptConfiguration GetJavaScriptConfig(string entityName)
        {
            var tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

            var result = new CreateFileWithEntityMetadataJavaScriptConfiguration(
                entityName
                , tabSpacer
                , _commonConfig.EntityMetadaOptionSetDependentComponents
                , _commonConfig.GenerateIntoSchemaClass
            );

            return result;
        }

        private void btnConnection1CSharpSchema_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null
                || string.IsNullOrEmpty(entity.LogicalName)
            )
            {
                return;
            }

            CreateEntityMetadataFileCSharpSchema(GetService1, entity.EntityMetadata1);
        }

        private void btnConnection2CSharpSchema_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null
                || string.IsNullOrEmpty(entity.LogicalName)
            )
            {
                return;
            }

            CreateEntityMetadataFileCSharpSchema(GetService2, entity.EntityMetadata2);
        }

        private async Task CreateEntityMetadataFileCSharpSchema(Func<Task<IOrganizationServiceExtented>> getService, EntityMetadata entityMetadata)
        {
            if (!this.IsControlsEnabled)
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

            var service = await getService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.LogicalName);

            string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

            var config = GetCSharpConfigSchema(entityMetadata.LogicalName);

            string fileName = string.Format("{0}.{1}.Generated.cs", service.ConnectionData.Name, entityMetadata.SchemaName);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                var repository = new EntityMetadataRepository(service);

                ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
                INamingService namingService = new NamingService(service.ConnectionData.ServiceContextName, config);
                ITypeMappingService typeMappingService = new TypeMappingService(service.ConnectionData.NamespaceClasses);
                ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);
                IMetadataProviderService metadataProviderService = new MetadataProviderService(repository);

                ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

                using (var handler = new CreateFileWithEntityMetadataCSharpHandler(config, service, _iWriteToOutput, codeGenerationServiceProvider))
                {
                    await handler.CreateFileAsync(filePath);
                }

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, config.EntityName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.LogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);
        }

        private void btnConnection1CSharpProxyClass_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null
                || string.IsNullOrEmpty(entity.LogicalName)
            )
            {
                return;
            }

            CreateEntityMetadataFileCSharpProxyClass(GetService1, entity.EntityMetadata1);
        }

        private void btnConnection2CSharpProxyClass_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null
                || string.IsNullOrEmpty(entity.LogicalName)
            )
            {
                return;
            }

            CreateEntityMetadataFileCSharpProxyClass(GetService2, entity.EntityMetadata2);
        }

        private async Task CreateEntityMetadataFileCSharpProxyClass(Func<Task<IOrganizationServiceExtented>> getService, EntityMetadata entityMetadata)
        {
            if (!this.IsControlsEnabled)
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

            var service = await getService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.LogicalName);

            string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

            var config = GetCSharpConfigProxyClass(entityMetadata.LogicalName);

            string fileName = string.Format("{0}.{1}.cs", service.ConnectionData.Name, entityMetadata.SchemaName);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                var repository = new EntityMetadataRepository(service);

                ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
                INamingService namingService = new NamingService(service.ConnectionData.ServiceContextName, config);
                ITypeMappingService typeMappingService = new TypeMappingService(service.ConnectionData.NamespaceClasses);
                ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);
                IMetadataProviderService metadataProviderService = new MetadataProviderService(repository);

                ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

                var entityMetadataFull = await repository.GetEntityMetadataAsync(entityMetadata.LogicalName);

                CodeGeneratorOptions options = new CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    IndentString = config.TabSpacer,
                    VerbatimOrder = true,
                };

                await codeGenerationService.WriteEntityFileAsync(entityMetadataFull, "CSharp", filePath, service.ConnectionData.NamespaceClasses, options, codeGenerationServiceProvider);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, config.EntityName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.LogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);
        }

        private void btnConnection1JavaScript_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (string.IsNullOrEmpty(entity?.LogicalName))
            {
                return;
            }

            CreateEntityMetadataFileJavaScript(GetService1, entity?.LogicalName);
        }

        private void btnConnection2JavaScript_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (string.IsNullOrEmpty(entity?.LogicalName))
            {
                return;
            }

            CreateEntityMetadataFileJavaScript(GetService2, entity?.LogicalName);
        }

        private async Task CreateEntityMetadataFileJavaScript(Func<Task<IOrganizationServiceExtented>> getService, string entityName)
        {
            if (!this.IsControlsEnabled)
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

            var service = await getService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat2, service.ConnectionData.Name, entityName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityName);

            var config = GetJavaScriptConfig(entityName);

            string filename = string.Format("{0}.{1}.EntityMetadata.Generated.js", service.ConnectionData.Name, entityName);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(filename));

            try
            {
                using (var handler = new CreateFileWithEntityMetadataJavaScriptHandler(config, service, _iWriteToOutput))
                {
                    await handler.CreateFileAsync(filePath);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, config.EntityName, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat2, service.ConnectionData.Name, entityName);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingEntities();
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
                        this.Focus();
                        e.Handled = true;
                    }
                }
            }

            base.OnKeyDown(e);
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

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData);
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

        private async void btnEntitySecurityRolesExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService1();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList);
        }

        private async void btnEntitySecurityRolesExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            _commonConfig.Save();

            var service = await GetService2();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList);
        }

        private async void btnSecurityRolesExplorer1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig, null, entityMetadataList, null);
        }

        private async void btnSecurityRolesExplorer2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig, null, entityMetadataList, null);
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

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entityMetadataList, entity?.LogicalName);
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
            var source = new SolutionComponentMetadataSource(service);

            var entityMetadata = source.GetEntityMetadata(entity.EntityMetadata1.MetadataId.Value);

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

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entityMetadataList, entity?.LogicalName);
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
            var source = new SolutionComponentMetadataSource(service);

            var entityMetadata = source.GetEntityMetadata(entity.EntityMetadata2.MetadataId.Value);

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
            if (!this.IsControlsEnabled)
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

            var service1 = await GetService1();
            var service2 = await GetService2();

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.ExportingEntityRibbonConnectionFormat3, entityName, service1.ConnectionData.Name, service1.ConnectionData.Name);

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceRibbonForEntityFormat1, entityName);

            try
            {
                string filePath1 = string.Empty;
                string filePath2 = string.Empty;

                var filter = _commonConfig.GetRibbonLocationFilters();

                var repository1 = new RibbonCustomizationRepository(service1);

                Task<string> task1 = repository1.ExportEntityRibbonAsync(entityName, filter);
                Task<string> task2 = null;

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    var repository2 = new RibbonCustomizationRepository(service2);

                    task2 = repository2.ExportEntityRibbonAsync(entityName, filter);
                }

                if (task1 != null)
                {
                    string ribbonXml = await task1;

                    ribbonXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonXml, _commonConfig, XmlOptionsControls.RibbonFull
                        , ribbonEntityName: entityName
                        );

                    string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service1.ConnectionData.Name, entityName);
                    filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath1, ribbonXml, new UTF8Encoding(false));
                }

                if (task2 != null)
                {
                    string ribbonXml = await task2;

                    ribbonXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonXml, _commonConfig, XmlOptionsControls.RibbonFull
                        , ribbonEntityName: entityName
                        );

                    string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service2.ConnectionData.Name, entityName);
                    filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath2, ribbonXml, new UTF8Encoding(false));
                }

                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service1.ConnectionData.Name, entityName, filePath1);
                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service2.ConnectionData.Name, entityName, filePath2);
                }

                if (File.Exists(filePath1) && File.Exists(filePath2))
                {
                    this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                }
                else
                {
                    this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                    this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                }

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonForEntityCompletedFormat1, entityName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonForEntityFailedFormat1, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.ExportingEntityRibbonConnectionFormat3, entityName, service1.ConnectionData.Name, service1.ConnectionData.Name);
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
            if (!this.IsControlsEnabled)
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

            var service1 = await GetService1();
            var service2 = await GetService2();

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.ExportingEntityRibbonDiffXmlConnectionFormat3, entity.LogicalName, service1.ConnectionData.Name, service1.ConnectionData.Name);

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceRibbonDiffXmlForEntityFormat1, entity.LogicalName);

            try
            {
                string filePath1 = string.Empty;
                string filePath2 = string.Empty;

                Task<string> task1 = null;
                Task<string> task2 = null;

                {
                    var repositoryRibbonCustomization = new RibbonCustomizationRepository(service1);

                    task1 = repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, entity.EntityMetadata1, null);
                }

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    var repositoryRibbonCustomization = new RibbonCustomizationRepository(service2);

                    task2 = repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, entity.EntityMetadata2, null);
                }

                if (task1 != null)
                {
                    string ribbonDiffXml = await task1;

                    if (!string.IsNullOrEmpty(ribbonDiffXml))
                    {
                        ribbonDiffXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonDiffXml, _commonConfig, XmlOptionsControls.RibbonFull
                            , schemaName: CommonExportXsdSchemasCommand.SchemaRibbonXml
                            , ribbonEntityName: entity.LogicalName
                            );

                        string fileName1 = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service1.ConnectionData.Name, entity.LogicalName);
                        filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));

                        File.WriteAllText(filePath1, ribbonDiffXml, new UTF8Encoding(false));
                    }
                }

                if (task2 != null)
                {
                    string ribbonDiffXml = await task2;

                    if (!string.IsNullOrEmpty(ribbonDiffXml))
                    {
                        ribbonDiffXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonDiffXml, _commonConfig, XmlOptionsControls.RibbonFull
                            , schemaName: CommonExportXsdSchemasCommand.SchemaRibbonXml
                            , ribbonEntityName: entity.LogicalName
                            );

                        string fileName2 = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service2.ConnectionData.Name, entity.LogicalName);
                        filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName2));

                        File.WriteAllText(filePath2, ribbonDiffXml, new UTF8Encoding(false));
                    }
                }

                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service1.ConnectionData.Name, entity.LogicalName, filePath1);
                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service2.ConnectionData.Name, entity.LogicalName, filePath2);
                }

                if (File.Exists(filePath1) && File.Exists(filePath2))
                {
                    this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                }
                else
                {
                    this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                    this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                }

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonDiffXmlForEntityCompletedFormat1, entity.LogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceRibbonDiffXmlForEntityFailedFormat1, entity.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.ExportingEntityRibbonDiffXmlConnectionFormat3, entity.LogicalName, service1.ConnectionData.Name, service1.ConnectionData.Name);
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
            if (!this.IsControlsEnabled)
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

            var service = await getService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingRibbonForEntityFormat2, service.ConnectionData.Name, entityName);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonForEntityFormat1, entityName);

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                string ribbonXml = await repository.ExportEntityRibbonAsync(entityName, _commonConfig.GetRibbonLocationFilters());

                ribbonXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonXml, _commonConfig, XmlOptionsControls.RibbonFull
                    , ribbonEntityName: entityName
                    );

                string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityName);
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service.ConnectionData.Name, entityName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityCompletedFormat1, entityName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityFailedFormat1, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingRibbonForEntityFormat2, service.ConnectionData.Name, entityName);
        }

        private void mIConnection1EntityRibbonArchive_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null || string.IsNullOrEmpty(entity.LogicalName))
            {
                return;
            }

            ExecuteCreatingEntityRibbonArchive(GetService1, entity.LogicalName);
        }

        private void mIConnection2EntityRibbonArchive_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null || string.IsNullOrEmpty(entity.LogicalName))
            {
                return;
            }

            ExecuteCreatingEntityRibbonArchive(GetService2, entity.LogicalName);
        }

        private async Task ExecuteCreatingEntityRibbonArchive(Func<Task<IOrganizationServiceExtented>> getService, string entityName)
        {
            if (!this.IsControlsEnabled)
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

            var service = await getService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingRibbonForEntityFormat2, service.ConnectionData.Name, entityName);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonForEntityFormat1, entityName);

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                var ribbonBody = await repository.ExportEntityRibbonByteArrayAsync(entityName, _commonConfig.GetRibbonLocationFilters());

                string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityName, "zip");
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllBytes(filePath, ribbonBody);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityRibbonArchiveForConnectionFormat3, service.ConnectionData.Name, entityName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityCompletedFormat1, entityName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityFailedFormat1, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingRibbonForEntityFormat2, service.ConnectionData.Name, entityName);
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

        private async Task ExecuteCreatingEntityRibbonDiffXml(Func<Task<IOrganizationServiceExtented>> getService, EntityMetadata entityMetadata)
        {
            if (!this.IsControlsEnabled)
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

            var service = await getService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFormat1, entityMetadata.LogicalName);

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            try
            {
                string ribbonDiffXml = await repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, entityMetadata, null);

                ribbonDiffXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonDiffXml, _commonConfig, XmlOptionsControls.RibbonFull
                    , schemaName: CommonExportXsdSchemasCommand.SchemaRibbonXml
                    , ribbonEntityName: entityMetadata.LogicalName
                    );

                {
                    string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entityMetadata.LogicalName);
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityCompletedFormat1, entityMetadata.LogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFailedFormat1, entityMetadata.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);
        }

        private void miExportEntityMetadataOptions_Click(object sender, RoutedEventArgs e)
        {
            _optionsPopup.IsOpen = true;
            _optionsPopup.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_optionsPopup.IsOpen)
            {
                _optionsPopup.IsOpen = false;
                this.Focus();
            }
        }

        private async void miConnection1OpenEntityMetadataInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null)
            {
                return;
            }

            var service = await GetService1();

            service.ConnectionData.OpenEntityMetadataInWeb(entity.EntityMetadata1.MetadataId.Value);
        }

        private async void miConnection2OpenEntityMetadataInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null)
            {
                return;
            }

            var service = await GetService2();

            service.ConnectionData.OpenEntityMetadataInWeb(entity.EntityMetadata2.MetadataId.Value);
        }

        private async void miConnection1OpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null)
            {
                return;
            }

            var service = await GetService1();

            service.ConnectionData.OpenEntityInstanceListInWeb(entity.LogicalName);
        }

        private async void miConnection2OpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedLinkedEntityMetadata();

            if (entity == null)
            {
                return;
            }

            var service = await GetService2();

            service.ConnectionData.OpenEntityInstanceListInWeb(entity.LogicalName);
        }
    }
}