using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportRibbon : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly Popup _optionsPopup;

        private readonly XmlOptionsControls _xmlOptions = XmlOptionsControls.RibbonFull;

        public WindowExportRibbon(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

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

            LoadFromConfig();

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this.DecreaseInit();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
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

        private async Task<IOrganizationServiceExtented> GetService()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData == null)
            {
                return null;
            }

            if (_connectionCache.ContainsKey(connectionData.ConnectionId))
            {
                return _connectionCache[connectionData.ConnectionId];
            }

            ToggleControls(connectionData, false, string.Empty);

            _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
            _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            try
            {
                var service = await QuickConnection.ConnectAsync(connectionData);

                if (service != null)
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;

                    return service;
                }
                else
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                }
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                ToggleControls(connectionData, true, string.Empty);
            }

            return null;
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

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection);
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void miCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void miEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void miEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void miEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void miEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void miGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
            );
        }

        private async void miSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void miSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty);
        }

        private async void miSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty);
        }

        private async void miWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty);
        }

        private async void miPluginTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty, string.Empty);
        }

        private async void miMessageTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty);
        }

        private async void miMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void miOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void miCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async Task ExecuteActionOnApplicationRibbonAsync(Func<Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await action();
        }

        private void miExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformExportApplicationRibbon);
        }

        private async Task PerformExportApplicationRibbon()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonFormat1, service.ConnectionData.Name);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingApplicationRibbon);

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                string ribbonXml = await repository.ExportApplicationRibbonAsync();

                ribbonXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonXml, _commonConfig, _xmlOptions
                    , ribbonEntityName: string.Empty
                    );

                {
                    string fileName = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name);
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedAppliationRibbonForConnectionFormat2, service.ConnectionData.Name, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingApplicationRibbonCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonFormat1, service.ConnectionData.Name);
        }

        private void miExportApplicationRibbonArchive_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformExportApplicationRibbonArchive);
        }

        private async Task PerformExportApplicationRibbonArchive()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonFormat1, service.ConnectionData.Name);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingApplicationRibbon);

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                var ribbonBody = await repository.ExportApplicationRibbonByteArrayAsync();

                {
                    string fileName = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name, "zip");
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, ribbonBody);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedAppliationRibbonForConnectionFormat2, service.ConnectionData.Name, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingApplicationRibbonCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonFormat1, service.ConnectionData.Name);
        }

        private void miExportApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformExportApplicationRibbonDiffXml);
        }

        private async Task PerformExportApplicationRibbonDiffXml()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonDiffXmlFormat1, service.ConnectionData.Name);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXml);

            var repositoryPublisher = new PublisherRepository(service);
            var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

            if (publisherDefault == null)
            {
                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.NotFoundedDefaultPublisher);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization == null)
            {
                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.NotFoundedApplicationRibbonRibbonCustomization);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            try
            {
                string ribbonDiffXml = await repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, null, ribbonCustomization);

                if (string.IsNullOrEmpty(ribbonDiffXml))
                {
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXmlFailed);
                    return;
                }

                ribbonDiffXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonDiffXml, _commonConfig, _xmlOptions
                    , schemaName: CommonExportXsdSchemasCommand.SchemaRibbonXml
                    , ribbonEntityName: string.Empty
                    );

                {
                    string fileName = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name);
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedAppliationRibbonDiffXmlForConnectionFormat2, service.ConnectionData.Name, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXmlCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXmlFailed);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonDiffXmlFormat1, service.ConnectionData.Name);
        }

        private void miUpdateApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformUpdateApplicationRibbonDiffXml);
        }

        private async Task PerformUpdateApplicationRibbonDiffXml()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.UpdatingApplicationRibbonDiffXmlFormat1, service.ConnectionData.Name);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXmlFormat1, service.ConnectionData.Name);

            var newText = string.Empty;
            {
                bool? dialogResult = false;

                var title = "Application RibbonDiffXml";

                this.Dispatcher.Invoke(() =>
                {
                    var form = new WindowTextField("Enter " + title, title, string.Empty);

                    dialogResult = form.ShowDialog();

                    newText = form.FieldText;
                });

                if (dialogResult.GetValueOrDefault() == false)
                {
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXmlCanceledFormat1, service.ConnectionData.Name);
                    return;
                }
            }

            newText = ContentCoparerHelper.RemoveAllCustomXmlAttributesAndNamespaces(newText);

            UpdateStatus(service.ConnectionData, Properties.WindowStatusStrings.ValidatingApplicationRibbonDiffXml);

            if (!ContentCoparerHelper.TryParseXmlDocument(newText, out var doc))
            {
                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.TextIsNotValidXml);

                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            bool validateResult = await RibbonCustomizationRepository.ValidateXmlDocumentAsync(service.ConnectionData, _iWriteToOutput, doc);

            if (!validateResult)
            {
                var dialogResult = MessageBoxResult.Cancel;

                this.Dispatcher.Invoke(() =>
                {
                    dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                });

                if (dialogResult != MessageBoxResult.OK)
                {
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ValidatingApplicationRibbonDiffXmlFailed);
                    return;
                }
            }

            var repositoryPublisher = new PublisherRepository(service);
            var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

            if (publisherDefault == null)
            {
                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.NotFoundedDefaultPublisher);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization == null)
            {
                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.NotFoundedApplicationRibbonRibbonCustomization);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            try
            {
                await repositoryRibbonCustomization.PerformUpdateRibbonDiffXml(_iWriteToOutput, _commonConfig, doc, null, ribbonCustomization);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXmlCompletedFormat1, service.ConnectionData.Name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXmlFailedFormat1, service.ConnectionData.Name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.UpdatingApplicationRibbonDiffXmlFormat1, service.ConnectionData.Name);
        }

        private async void miPublishApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingApplicationRibbonFormat1, service.ConnectionData.Name);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.PublishingApplicationRibbonFormat1, service.ConnectionData.Name);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishApplicationRibbonAsync();

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingApplicationRibbonCompletedFormat1, service.ConnectionData.Name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingApplicationRibbonFailedFormat1, service.ConnectionData.Name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingApplicationRibbonFormat1, service.ConnectionData.Name);
        }

        private void miApplicationRibbon_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            FillLastSolutionItems(connectionData, new[] { miAddApplicationTotSolutionLast }, true, AddApplicationRibbonToCrmSolutionLast_Click, "miAddApplicationTotSolutionLast");
        }

        private async void miApplicationRibbonOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            var repository = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repository.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization != null)
            {
                _commonConfig.Save();

                WindowHelper.OpenExplorerSolutionWindow(
                    _iWriteToOutput
                    , service
                    , _commonConfig
                    , (int)ComponentType.RibbonCustomization
                    , ribbonCustomization.Id
                    , null
                );
            }
        }

        private async void miApplicationRibbonOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            var repository = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repository.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization != null)
            {
                service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.RibbonCustomization, ribbonCustomization.Id);
            }
        }

        private async void miApplicationRibbonOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            var repository = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repository.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization != null)
            {
                WindowHelper.OpenSolutionComponentDependenciesWindow(
                    _iWriteToOutput
                    , service
                    , null
                    , _commonConfig
                    , (int)ComponentType.RibbonCustomization
                    , ribbonCustomization.Id
                    , null
                    );
            }
        }

        private async void AddApplicationRibbonToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddApplicationRibbonToSolution(true, null);
        }

        private async void AddApplicationRibbonToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                 && menuItem.Tag != null
                 && menuItem.Tag is string solutionUniqueName
                 )
            {
                await AddApplicationRibbonToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddApplicationRibbonToSolution(bool withSelect, string solutionUniqueName)
        {
            var service = await GetService();

            var repository = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repository.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization != null)
            {
                _commonConfig.Save();

                try
                {
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                    await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.RibbonCustomization, new[] { ribbonCustomization.Id }, null, withSelect);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
        }

        private void miExportEntityRibbonOptions_Click(object sender, RoutedEventArgs e)
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

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }
    }
}