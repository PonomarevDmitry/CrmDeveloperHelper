using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
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
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportRibbon : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private Popup _optionsEntityRibbon;

        private bool _controlsEnabled = true;

        private int _init = 0;

        public WindowExportRibbon(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            this._optionsEntityRibbon = new Popup
            {
                Child = new ExportEntityRibbonOptionsControl(_commonConfig),

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
            };

            LoadFromConfig();

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
        }

        private async Task<IOrganizationServiceExtented> GetService()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_connectionCache.ContainsKey(connectionData.ConnectionId))
                {
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task<SolutionComponentDescriptor> GetDescriptor()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_descriptorCache.ContainsKey(connectionData.ConnectionId))
                {
                    var service = await GetService();

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);
                }

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
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

            ToggleControl(cmBCurrentConnection, enabled);
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

        #region Кнопки открытия других форм с информация о сущности.

        private async void miCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, string.Empty, null, null);
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
            var descriptor = await GetDescriptor();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
                , null
                , string.Empty
                , string.Empty
                );
        }

        private async void miSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty);
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

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty);
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

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, string.Empty);
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

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, string.Empty);
        }

        private async void miCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, string.Empty);
        }

        private async void miCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, string.Empty);
        }

        private async void miCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, string.Empty);
        }

        private async void miCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, string.Empty);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async Task ExecuteActionOnApplicationRibbonAsync(Func<Task> action)
        {
            string folder = txtBFolder.Text.Trim();

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

            await action();
        }

        private void miExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformExportApplicationRibbon);
        }

        private async Task PerformExportApplicationRibbon()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingApplicationRibbon);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingApplicationRibbon);

            var service = await GetService();

            string fileName = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                await repository.ExportApplicationRibbonAsync(filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput("Application Ribbon exported to {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ExportingApplicationRibbonCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingApplicationRibbon);
        }

        private void miExportApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformExportApplicationRibbonDiffXml);
        }

        private async Task PerformExportApplicationRibbonDiffXml()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingApplicationRibbonDiffXml);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXml);

            var service = await GetService();

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

                    UpdateStatus(Properties.WindowStatusStrings.CreatingNewSolutionFormat1, solutionUniqueName);

                    solution.Id = service.Create(solution);

                    UpdateStatus(Properties.WindowStatusStrings.AddingInSolutionApplicationRibbonFormat1, solutionUniqueName);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.RibbonCustomization),
                            ObjectId = ribbonCustomization.Id,
                            RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                        }});
                    }

                    UpdateStatus(Properties.WindowStatusStrings.ExportingSolutionAndExtractingApplicationRibbonDiffXmlFormat1, solutionUniqueName);

                    var repository = new ExportSolutionHelper(service);

                    string ribbonDiffXml = await repository.ExportSolutionAndGetApplicationRibbonDiffAsync(solutionUniqueName);

                    if (_commonConfig.SetXmlSchemasDuringExport)
                    {
                        var schemasResources = CommonExportXsdSchemasCommand.ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, "RibbonXml", StringComparison.InvariantCultureIgnoreCase));

                        if (schemasResources != null)
                        {
                            string schemas = ContentCoparerHelper.HandleExportXsdSchemaIntoSchamasFolder(schemasResources.Item2);

                            if (!string.IsNullOrEmpty(schemas))
                            {
                                ribbonDiffXml = ContentCoparerHelper.ReplaceXsdSchema(ribbonDiffXml, schemas);
                            }
                        }
                    }

                    if (_commonConfig.SetIntellisenseContext)
                    {
                        ribbonDiffXml = ContentCoparerHelper.SetRibbonDiffXmlIntellisenseContextEntityName(ribbonDiffXml, string.Empty);
                    }

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    {
                        string fileName = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name);
                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput("Application RibbonDiffXml exported to {0}", filePath);

                        this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                    }

                    UpdateStatus(Properties.WindowStatusStrings.DeletingSolutionFormat1, solutionUniqueName);

                    await service.DeleteAsync(solution.LogicalName, solution.Id);

                    ToggleControls(true, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXmlCompleted);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXmlFailed);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingApplicationRibbonDiffXml);
        }

        private void miUpdateApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformUpdateApplicationRibbonDiffXml);
        }

        private async Task PerformUpdateApplicationRibbonDiffXml()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.UpdatingApplicationRibbonDiffXml);

            ToggleControls(false, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXml);

            var service = await GetService();

            try
            {
                var newText = string.Empty;
                bool? dialogResult = false;

                var title = "Application RibbonDiffXml for entity {0}";

                this.Dispatcher.Invoke(() =>
                {
                    var form = new WindowTextField("Enter " + title, title, string.Empty);

                    dialogResult = form.ShowDialog();

                    newText = form.FieldText;
                });

                if (dialogResult.GetValueOrDefault() == false)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXmlCanceled);
                    return;
                }

                ContentCoparerHelper.ClearXsdSchema(newText, out newText);

                UpdateStatus(Properties.WindowStatusStrings.ValidatingApplicationRibbonDiffXml);

                if (!ContentCoparerHelper.TryParseXmlDocument(newText, out var doc))
                {
                    ToggleControls(true, Properties.WindowStatusStrings.TextIsNotValidXml);

                    _iWriteToOutput.ActivateOutputWindow();
                    return;
                }

                bool validateResult = await ValidateXmlDocumentAsync(doc);

                if (!validateResult)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.ValidatingApplicationRibbonDiffXmlFailed);
                    return;
                }




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

                    UpdateStatus(Properties.WindowStatusStrings.CreatingNewSolutionFormat1, solutionUniqueName);

                    solution.Id = service.Create(solution);

                    UpdateStatus(Properties.WindowStatusStrings.AddingInSolutionApplicationRibbonFormat1, solutionUniqueName);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.RibbonCustomization),
                            ObjectId = ribbonCustomization.Id,
                            RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                        }});
                    }

                    UpdateStatus(Properties.WindowStatusStrings.ExportingSolutionAndExtractingApplicationRibbonDiffXmlFormat1, solutionUniqueName);

                    var repository = new ExportSolutionHelper(service);

                    var solutionBodyBinary = await repository.ExportSolutionAndGetBodyBinaryAsync(solutionUniqueName);

                    string ribbonDiffXml = ExportSolutionHelper.GetApplicationRibbonDiffXmlFromSolutionBody(solutionBodyBinary);

                    {
                        string fileName = string.Format("{0}.{1}_{2} Solution Backup at {3}.zip"
                            , service.ConnectionData.Name
                            , solution.UniqueName
                            , solution.Version
                            , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                            );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllBytes(filePath, solutionBodyBinary);

                        this._iWriteToOutput.WriteToOutput("Solution {0} Backup exported to {1}", solution.UniqueName, filePath);

                        this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                    }

                    if (_commonConfig.SetXmlSchemasDuringExport)
                    {
                        var schemasResources = CommonExportXsdSchemasCommand.ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, "RibbonXml", StringComparison.InvariantCultureIgnoreCase));

                        if (schemasResources != null)
                        {
                            string schemas = ContentCoparerHelper.HandleExportXsdSchemaIntoSchamasFolder(schemasResources.Item2);

                            if (!string.IsNullOrEmpty(schemas))
                            {
                                ribbonDiffXml = ContentCoparerHelper.ReplaceXsdSchema(ribbonDiffXml, schemas);
                            }
                        }
                    }

                    if (_commonConfig.SetIntellisenseContext)
                    {
                        ribbonDiffXml = ContentCoparerHelper.SetRibbonDiffXmlIntellisenseContextEntityName(ribbonDiffXml, string.Empty);
                    }

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    {
                        string fileName = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name, "BackUp", "xml");
                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput("Application RibbonDiffXml BackUp exported to {0}", filePath);

                        this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                    }

                    solutionBodyBinary = ExportSolutionHelper.ReplaceApplicationRibbonDiffXmlInSolutionBody(solutionBodyBinary, doc.Root);

                    UpdateStatus(Properties.WindowStatusStrings.ImportingSolutionFormat1, solutionUniqueName);

                    {
                        string fileName = string.Format("{0}.{1}_{2} Changed Solution Backup at {3}.zip"
                            , service.ConnectionData.Name
                            , solution.UniqueName
                            , solution.Version
                            , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                            );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllBytes(filePath, solutionBodyBinary);

                        this._iWriteToOutput.WriteToOutput("Changed Solution {0} Backup exported to {1}", solution.UniqueName, filePath);

                        this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                    }

                    await repository.ImportSolutionAsync(solutionBodyBinary);

                    UpdateStatus(Properties.WindowStatusStrings.DeletingSolutionFormat1, solutionUniqueName);

                    await service.DeleteAsync(solution.LogicalName, solution.Id);

                    UpdateStatus(Properties.WindowStatusStrings.PublishingApplicationRibbon);

                    {
                        var repositoryPublish = new PublishActionsRepository(service);

                        await repositoryPublish.PublishApplicationRibbonAsync();
                    }
                }

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXmlCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXmlFailed);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.UpdatingApplicationRibbonDiffXml);
        }

        private async void miPublishApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.PublishingApplicationRibbon);

            ToggleControls(false, Properties.WindowStatusStrings.PublishingApplicationRibbon);

            try
            {
                var service = await GetService();

                var repository = new PublishActionsRepository(service);

                await repository.PublishApplicationRibbonAsync();

                ToggleControls(true, Properties.WindowStatusStrings.PublishingApplicationRibbonCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.PublishingApplicationRibbonFailed);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.PublishingApplicationRibbon);
        }

        private Task<bool> ValidateXmlDocumentAsync(XDocument doc)
        {
            return Task.Run(() => ValidateXmlDocument(doc));
        }

        private bool ValidateXmlDocument(XDocument doc)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            {
                var schemasResources = CommonExportXsdSchemasCommand.ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, "RibbonXml", StringComparison.InvariantCultureIgnoreCase));

                if (schemasResources != null)
                {
                    foreach (var fileName in schemasResources.Item2)
                    {
                        Uri uri = FileOperations.GetSchemaResourceUri(fileName);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        using (StreamReader reader = new StreamReader(info.Stream))
                        {
                            schemas.Add("", XmlReader.Create(reader));
                        }
                    }
                }
            }

            List<ValidationEventArgs> errors = new List<ValidationEventArgs>();

            doc.Validate(schemas, (o, e) =>
            {
                errors.Add(e);
            });

            if (errors.Count > 0)
            {
                _iWriteToOutput.WriteToOutput("RibbonDiff is not valid.");

                foreach (var item in errors)
                {
                    _iWriteToOutput.WriteToOutput(string.Empty);
                    _iWriteToOutput.WriteToOutput(string.Empty);
                    _iWriteToOutput.WriteToOutput("Severity: {0}      Message: {1}", item.Severity, item.Message);
                    _iWriteToOutput.WriteErrorToOutput(item.Exception);
                }

                _iWriteToOutput.ActivateOutputWindow();
            }

            return errors.Count == 0;
        }

        private void miApplicationRibbon_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            FillLastSolutionItems(connectionData, new[] { miAddApplicationIntoLastSolution }, true, AddApplicationRibbonIntoCrmSolutionLast_Click, "miAddApplicationIntoLastSolution");
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
                var descriptor = await GetDescriptor();

                WindowHelper.OpenSolutionComponentDependenciesWindow(
                    _iWriteToOutput
                    , service
                    , descriptor
                    , _commonConfig
                    , (int)ComponentType.RibbonCustomization
                    , ribbonCustomization.Id
                    , null
                    );
            }
        }

        private async void AddApplicationRibbonIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddApplicationRibbonIntoSolution(true, null);
        }

        private async void AddApplicationRibbonIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                 && menuItem.Tag != null
                 && menuItem.Tag is string solutionUniqueName
                 )
            {
                await AddApplicationRibbonIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddApplicationRibbonIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var service = await GetService();

            var repository = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repository.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization != null)
            {
                _commonConfig.Save();

                var descriptor = await GetDescriptor();

                try
                {
                    this._iWriteToOutput.ActivateOutputWindow();

                    await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.RibbonCustomization, new[] { ribbonCustomization.Id }, null, withSelect);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
        }

        private void miExportEntityRibbonOptions_Click(object sender, RoutedEventArgs e)
        {
            _optionsEntityRibbon.Focus();
            _optionsEntityRibbon.IsOpen = true;
        }
    }
}