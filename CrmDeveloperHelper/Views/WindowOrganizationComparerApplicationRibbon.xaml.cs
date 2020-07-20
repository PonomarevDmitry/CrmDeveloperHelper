using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.Collections.Generic;
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
    public partial class WindowOrganizationComparerApplicationRibbon : WindowWithConnectionList
    {
        private readonly Popup _optionsPopup;

        public WindowOrganizationComparerApplicationRibbon(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
        ) : base(iWriteToOutput, commonConfig, connection1)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.RibbonXmlOptions);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            this.Resources["ConnectionName1"] = connection1.Name;
            this.Resources["ConnectionName2"] = connection2.Name;

            LoadFromConfig();

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            FillExplorersMenuItems();

            this.DecreaseInit();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper1 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService1);
            var explorersHelper2 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService2);

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetConnection1(), GetConnection2()));

            explorersHelper1.FillExplorers(miExplorers1);
            explorersHelper2.FillExplorers(miExplorers2);

            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _commonConfig.Save();
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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            ToggleControls(enabled, statusFormat, args);
        }

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(tSProgressBar);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void mIDifferenceApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteDifferenceApplicationRibbon();
        }

        private async Task ExecuteDifferenceApplicationRibbon()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            var service1 = await GetService1();
            var service2 = await GetService2();

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.ExportingApplicationRibbonConnectionFormat2, service1.ConnectionData.Name, service1.ConnectionData.Name);

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceApplicationRibbons);

            try
            {
                string fileName1 = EntityFileNameFormatter.GetApplicationRibbonFileName(service1.ConnectionData.Name);
                string filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));

                string filePath2 = string.Empty;

                var repository1 = new RibbonCustomizationRepository(service1);

                Task<string> task1 = repository1.ExportApplicationRibbonAsync();
                Task<string> task2 = null;

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    var repository2 = new RibbonCustomizationRepository(service2);

                    task2 = repository2.ExportApplicationRibbonAsync();
                }

                if (task1 != null)
                {
                    string ribbonXml = await task1;

                    ribbonXml = ContentComparerHelper.FormatXmlByConfiguration(
                        ribbonXml
                        , _commonConfig
                        , XmlOptionsControls.RibbonXmlOptions
                        , entityName: string.Empty
                    );

                    string fileName = EntityFileNameFormatter.GetApplicationRibbonFileName(service1.ConnectionData.Name);
                    filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath1, ribbonXml, new UTF8Encoding(false));
                }

                if (task2 != null)
                {
                    string ribbonXml = await task2;

                    ribbonXml = ContentComparerHelper.FormatXmlByConfiguration(
                        ribbonXml
                        , _commonConfig
                        , XmlOptionsControls.RibbonXmlOptions
                        , entityName: string.Empty
                    );

                    string fileName = EntityFileNameFormatter.GetApplicationRibbonFileName(service2.ConnectionData.Name);
                    filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath2, ribbonXml, new UTF8Encoding(false));
                }

                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.ExportedAppliationRibbonForConnectionFormat2, service1.ConnectionData.Name, filePath1);

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.ExportedAppliationRibbonForConnectionFormat2, service2.ConnectionData.Name, filePath2);
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
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }

            ToggleControls(true, Properties.OutputStrings.ShowingDifferenceApplicationRibbonsCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.ExportingApplicationRibbonConnectionFormat2, service1.ConnectionData.Name, service1.ConnectionData.Name);
        }

        private async void mIDifferenceApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteDifferenceApplicationRibbonDiffXml();
        }

        private async Task ExecuteDifferenceApplicationRibbonDiffXml()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            var service1 = await GetService1();
            var service2 = await GetService2();

            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.ExportingApplicationRibbonDiffXmlConnectionFormat2, service1.ConnectionData.Name, service1.ConnectionData.Name);

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceApplicationRibbonsDiffXml);

            try
            {
                string filePath1 = string.Empty;
                string filePath2 = string.Empty;

                Task<string> task1 = null;
                Task<string> task2 = null;

                {
                    var repositoryRibbonCustomization = new RibbonCustomizationRepository(service1);
                    var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                    if (ribbonCustomization != null)
                    {
                        task1 = repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, null, ribbonCustomization);
                    }
                }

                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    var repositoryRibbonCustomization = new RibbonCustomizationRepository(service2);
                    var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                    if (ribbonCustomization != null)
                    {
                        task2 = repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, null, ribbonCustomization);
                    }
                }

                if (task1 != null)
                {
                    string ribbonDiffXml = await task1;

                    if (!string.IsNullOrEmpty(ribbonDiffXml))
                    {
                        ribbonDiffXml = ContentComparerHelper.FormatXmlByConfiguration(
                            ribbonDiffXml
                            , _commonConfig
                            , XmlOptionsControls.RibbonXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.RibbonSchema
                            , entityName: string.Empty
                        );

                        string fileName1 = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service1.ConnectionData.Name);
                        filePath1 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName1));

                        File.WriteAllText(filePath1, ribbonDiffXml, new UTF8Encoding(false));
                    }
                }

                if (task2 != null)
                {
                    string ribbonDiffXml = await task2;

                    if (!string.IsNullOrEmpty(ribbonDiffXml))
                    {
                        ribbonDiffXml = ContentComparerHelper.FormatXmlByConfiguration(
                            ribbonDiffXml
                            , _commonConfig
                            , XmlOptionsControls.RibbonXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.RibbonSchema
                            , entityName: string.Empty
                        );

                        string fileName2 = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service2.ConnectionData.Name);
                        filePath2 = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName2));

                        File.WriteAllText(filePath2, ribbonDiffXml, new UTF8Encoding(false));
                    }
                }

                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.ExportedAppliationRibbonDiffXmlForConnectionFormat2, service1.ConnectionData.Name, filePath1);
                if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.ExportedAppliationRibbonDiffXmlForConnectionFormat2, service2.ConnectionData.Name, filePath2);
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

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceApplicationRibbonsDiffXmlCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceApplicationRibbonsDiffXmlFailed);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.ExportingApplicationRibbonDiffXmlConnectionFormat2, service1.ConnectionData.Name, service1.ConnectionData.Name);
        }

        private async void mIConnection1ApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteCreatingApplicationRibbon(GetService1);
        }

        private async void mIConnection2ApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteCreatingApplicationRibbon(GetService2);
        }

        private async Task ExecuteCreatingApplicationRibbon(Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            var service = await getService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonFormat1, service.ConnectionData.Name);

            ToggleControls(false, Properties.OutputStrings.ExportingApplicationRibbon);

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                string ribbonXml = await repository.ExportApplicationRibbonAsync();

                ribbonXml = ContentComparerHelper.FormatXmlByConfiguration(
                    ribbonXml
                    , _commonConfig
                    , XmlOptionsControls.RibbonXmlOptions
                    , entityName: string.Empty
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

            ToggleControls(true, Properties.OutputStrings.ExportingApplicationRibbonCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonFormat1, service.ConnectionData.Name);
        }

        private async void mIConnection1ApplicationRibbonArchive_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteCreatingApplicationRibbonArchive(GetService1);
        }

        private async void mIConnection2ApplicationRibbonArchive_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteCreatingApplicationRibbonArchive(GetService2);
        }

        private async Task ExecuteCreatingApplicationRibbonArchive(Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            var service = await getService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonFormat1, service.ConnectionData.Name);

            ToggleControls(false, Properties.OutputStrings.ExportingApplicationRibbon);

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                var ribbonBody = await repository.ExportApplicationRibbonByteArrayAsync();

                {
                    string fileName = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name, FileExtension.zip);
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

            ToggleControls(true, Properties.OutputStrings.ExportingApplicationRibbonCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonFormat1, service.ConnectionData.Name);
        }

        private async void mIConnection1ApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteCreatingApplicationRibbonDiffXml(GetService1);
        }

        private async void mIConnection2ApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteCreatingApplicationRibbonDiffXml(GetService2);
        }

        private async Task ExecuteCreatingApplicationRibbonDiffXml(Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            var service = await getService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonDiffXmlFormat1, service.ConnectionData.Name);

            ToggleControls(false, Properties.OutputStrings.ExportingApplicationRibbonDiffXml);

            var repositoryPublisher = new PublisherRepository(service);
            var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

            if (publisherDefault == null)
            {
                ToggleControls(true, Properties.OutputStrings.NotFoundedDefaultPublisher);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization == null)
            {
                ToggleControls(true, Properties.OutputStrings.NotFoundedApplicationRibbonRibbonCustomization);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            try
            {
                string ribbonDiffXml = await repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, null, ribbonCustomization);

                if (string.IsNullOrEmpty(ribbonDiffXml))
                {
                    ToggleControls(true, Properties.OutputStrings.ExportingApplicationRibbonDiffXmlFailed);
                    return;
                }

                ribbonDiffXml = ContentComparerHelper.FormatXmlByConfiguration(
                    ribbonDiffXml
                    , _commonConfig
                    , XmlOptionsControls.RibbonXmlOptions
                    , schemaName: AbstractDynamicCommandXsdSchemas.RibbonSchema
                    , entityName: string.Empty
                );

                {
                    string fileName = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name);
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedAppliationRibbonDiffXmlForConnectionFormat2, service.ConnectionData.Name, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }

                ToggleControls(true, Properties.OutputStrings.ExportingApplicationRibbonDiffXmlCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.ExportingApplicationRibbonDiffXmlFailed);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingApplicationRibbonDiffXmlFormat1, service.ConnectionData.Name);
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;
                ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

                if (connection1 != null && connection2 != null)
                {
                    this.Resources["ConnectionName1"] = connection1.Name;
                    this.Resources["ConnectionName2"] = connection2.Name;
                }
            });
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
    }
}