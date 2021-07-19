using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    public partial class FetchXmlExecutorControl : UserControl
    {
        private int _init = 0;

        private EntityCollection _entityCollection;

        private IWriteToOutput _iWriteToOutput;

        private readonly object sysObjectConnections = new object();

        private readonly Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly OrganizationServiceExtentedLocker _serviceLocker;

        public event EventHandler<EventArgs> ConnectionChanged;

        private TabItem _selectedItem;

        private void OnConnectionChanged()
        {
            this.ConnectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public string FilePath { get; private set; }

        public ConnectionData GetSelectedConnection() => cmBCurrentConnection.SelectedItem as ConnectionData;

        private const string paramColumnParametersWidth = "ColumnParametersWidth";
        private const string paramColumnFetchTextWidth = "ColumnFetchTextWidth";

        public FetchXmlExecutorControl()
        {
            InitializeComponent();

            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);

            var settings = GetUserControlSettings();

            if (settings.DictDouble.ContainsKey(paramColumnParametersWidth)
                && settings.DictDouble.ContainsKey(paramColumnFetchTextWidth)
            )
            {
                var widthParameters = settings.DictDouble[paramColumnParametersWidth];
                var widthFetchText = settings.DictDouble[paramColumnFetchTextWidth];

                if (widthParameters + widthFetchText > 0)
                {
                    var tempParamerts = widthParameters * 100 / (widthParameters + widthFetchText);
                    var tempFetchText = widthFetchText * 100 / (widthParameters + widthFetchText);

                    columnParameters.Width = new GridLength(tempParamerts, GridUnitType.Star);
                    columnFetchText.Width = new GridLength(tempFetchText, GridUnitType.Star);
                }
            }

            tabControl.Loaded += TabControl_Loaded;

            this._serviceLocker = new OrganizationServiceExtentedLocker();
        }

        protected bool IsControlsEnabled => this._init >= 0;

        protected void ChangeInitByEnabled(bool enabled)
        {
            if (enabled)
            {
                this._init++;
            }
            else
            {
                this._init--;
            }
        }

        protected void IncreaseInit()
        {
            this._init++;
        }

        protected void DecreaseInit()
        {
            this._init--;
        }

        private void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null)
            {
                tabControl.SelectedItem = _selectedItem;
            }
        }

        private UserControlSettings GetUserControlSettings()
        {
            var name = this.Name;

            if (string.IsNullOrEmpty(name))
            {
                name = this.GetType().Name;
            }

            return FileOperations.GetUserControlSettings(name);
        }

        public void DetachFromSourceCollection()
        {
            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;

            var settings = GetUserControlSettings();
            settings.DictDouble[paramColumnParametersWidth] = columnParameters.Width.Value;
            settings.DictDouble[paramColumnFetchTextWidth] = columnFetchText.Width.Value;
            settings.Save();

            this.Dispatcher.Invoke(() =>
            {
                this._connectionCache.Clear();
            });

            this._serviceLocker.Dispose();
        }

        public void SetSource(string filePath, ConnectionData connectionData, IWriteToOutput iWriteToOutput)
        {
            if (!string.IsNullOrEmpty(this.FilePath))
            {
                return;
            }

            this.FilePath = filePath;
            this._iWriteToOutput = iWriteToOutput;

            BindingOperations.EnableCollectionSynchronization(connectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            BindCollections(connectionData);

            cmBCurrentConnection.ItemsSource = connectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = connectionData;

            ClearGridAndTextBox();

            UpdateStatus(this.GetSelectedConnection(), string.Empty);

            this.dGrParameters.DataContext = cmBCurrentConnection;
        }

        private async void btnExecuteFetchXml_Click(object sender, RoutedEventArgs e)
        {
            await this.Execute();
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

        public async Task Execute()
        {
            ConnectionData connectionData = this.GetSelectedConnection();

            try
            {
                await this.Execute(connectionData);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                ToggleControls(connectionData, true, Properties.OutputStrings.FetchExecutionError);

                while (IsControlsEnabled)
                {
                    ChangeInitByEnabled(true);
                }
            }
        }

        private async Task Execute(ConnectionData connectionData)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            ToggleControls(connectionData, false, Properties.OutputStrings.PreparingFetchRequest);

            ClearGridAndTextBox();

            if (connectionData == null)
            {
                this._selectedItem = tbErrorText;

                this.Dispatcher.Invoke(() =>
                {
                    txtBErrorText.Text = Properties.OutputStrings.ConnectionIsNotSelected;

                    tbErrorText.IsEnabled = true;
                    tbErrorText.Visibility = Visibility.Visible;

                    tbErrorText.IsSelected = true;
                    tbErrorText.Focus();
                });

                ToggleControls(connectionData, true, Properties.OutputStrings.ConnectionIsNotSelected);

                return;
            }

            if (!TryLoadFileText(connectionData))
            {
                ToggleControls(connectionData, true, Properties.OutputStrings.FileNotExists);
                return;
            }

            var fileText = txtBFetchXml.Text.Trim();

            if (!ContentComparerHelper.TryParseXml(fileText, out var exception, out var doc))
            {
                StringBuilder text = new StringBuilder();

                text.AppendLine(Properties.OutputStrings.FileTextIsNotValidXml);
                text.AppendFormat("File: {0}", this.FilePath).AppendLine();
                text.AppendLine();
                text.AppendLine();

                var description = DTEHelper.GetExceptionDescription(exception);

                text.AppendLine(description);

                this._selectedItem = tbErrorText;

                this.Dispatcher.Invoke(() =>
                {
                    txtBErrorText.Text = text.ToString();

                    tbErrorText.IsEnabled = true;
                    tbErrorText.Visibility = Visibility.Visible;

                    tbErrorText.IsSelected = true;
                    tbErrorText.Focus();
                });

                ToggleControls(connectionData, true, Properties.OutputStrings.FileTextIsNotValidXml);

                return;
            }

            txtBFetchXml.Text = doc.ToString();

            if (CheckParametersAndReturnHasNew(doc, connectionData))
            {
                ToggleControls(connectionData, true, Properties.OutputStrings.FillNewParameters);

                return;
            }

            connectionData.Save();

            FillParametersValues(doc, connectionData);

            UpdateStatus(connectionData, Properties.OutputStrings.ExecutingFetch);

            try
            {
                await ExecuteFetchAsync(doc, connectionData);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                ToggleControls(connectionData, true, Properties.OutputStrings.FetchExecutionError);

                while (IsControlsEnabled)
                {
                    ChangeInitByEnabled(true);
                }
            }
        }

        private bool TryLoadFileText(ConnectionData connectionData)
        {
            if (!File.Exists(this.FilePath))
            {
                txtBErrorText.Text = string.Format(Properties.MessageBoxStrings.FileNotExistsFormat1, FilePath);

                tbErrorText.IsEnabled = true;
                tbErrorText.Visibility = Visibility.Visible;

                tbErrorText.IsSelected = true;
                tbErrorText.Focus();

                this._selectedItem = tbErrorText;

                UpdateStatus(connectionData, Properties.OutputStrings.FileNotExists);

                return false;
            }

            var fileText = File.ReadAllText(this.FilePath);

            txtBFetchXml.Text = fileText;

            return true;
        }

        private Task ExecuteFetchAsync(XElement fetchXml, ConnectionData connectionData)
        {
            return Task.Run(() => ExecuteFetch(fetchXml, connectionData));
        }

        private async Task ExecuteFetch(XElement fetchXml, ConnectionData connectionData)
        {
            this._entityCollection = null;

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                ToggleControls(connectionData, true, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            try
            {
                var request = new RetrieveMultipleRequest()
                {
                    Query = new FetchExpression(fetchXml.ToString()),
                };

                var response = await service.ExecuteAsync<RetrieveMultipleResponse>(request);

                this._entityCollection = response.EntityCollection;

                LoadData(connectionData, this._entityCollection, fetchXml);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.FetchQueryExecutedSuccessfullyFormat1, this._entityCollection.Entities.Count);
            }
            catch (Exception ex)
            {
                var errorTextBuilder = new StringBuilder();

                errorTextBuilder.AppendLine(Properties.OutputStrings.FetchExecutionError);
                errorTextBuilder.AppendLine();

                string description = DTEHelper.GetExceptionDescription(ex);

                errorTextBuilder.AppendLine(description);

                this.Dispatcher.Invoke(() =>
                {
                    txtBErrorText.Text = errorTextBuilder.ToString();

                    tbErrorText.IsEnabled = true;
                    tbErrorText.Visibility = Visibility.Visible;

                    tbErrorText.IsSelected = true;
                    tbErrorText.Focus();

                    this._selectedItem = tbErrorText;

                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.FetchExecutionError);
                });
            }
        }

        private void LoadData(ConnectionData connectionData, EntityCollection entityCollection, XElement fetchXml)
        {
            var columnsInFetch = EntityDescriptionHandler.GetColumnsFromFetch(connectionData, fetchXml);

            var dataTable = EntityDescriptionHandler.ConvertEntityCollectionToDataTable(connectionData, entityCollection, out Dictionary<string, string> columnMapping);

            this.Dispatcher.Invoke(() =>
            {
                dGrResults.Columns.Clear();

                foreach (var attributeName in columnsInFetch)
                {
                    if (!columnMapping.ContainsKey(attributeName))
                    {
                        continue;
                    }

                    var dataColumnName = columnMapping[attributeName];

                    if (!dataTable.Columns.Contains(dataColumnName))
                    {
                        continue;
                    }

                    AddNewColumnInDataGrid(dataTable, attributeName, dataColumnName);
                }

                foreach (var attributeName in columnMapping.Keys)
                {
                    if (columnsInFetch.Contains(attributeName))
                    {
                        continue;
                    }

                    var dataColumnName = columnMapping[attributeName];

                    if (!dataTable.Columns.Contains(dataColumnName))
                    {
                        continue;
                    }

                    AddNewColumnInDataGrid(dataTable, attributeName, dataColumnName);
                }

                dGrResults.ItemsSource = dataTable.DefaultView;

                tabControl.SelectedItem = tbResults;

                tbResults.IsEnabled = true;
                tbResults.Visibility = Visibility.Visible;

                tbResults.IsSelected = true;
                tbResults.Focus();

                this._selectedItem = tbResults;
            });
        }

        private void AddNewColumnInDataGrid(DataTable dataTable, string attributeName, string dataColumnName)
        {
            var dataColumn = dataTable.Columns[dataColumnName];

            if (dataColumn.DataType == typeof(EntityReferenceView)
                || dataColumn.DataType == typeof(PrimaryGuidView)
            )
            {
                var columnDGT = new DataGridHyperlinkColumn()
                {
                    Header = attributeName.Replace("_", "__"),
                    Width = DataGridLength.Auto,

                    SortMemberPath = dataColumnName,

                    ContentBinding = new Binding("[" + dataColumnName + "]")
                    {
                        Mode = BindingMode.OneTime,
                    },

                    Binding = new Binding("[" + dataColumnName + "].Url")
                    {
                        Mode = BindingMode.OneTime,
                    },
                };

                dGrResults.Columns.Add(columnDGT);
            }
            else
            {
                string format = null;

                if (dataColumn.DataType == typeof(decimal)
                   || dataColumn.DataType == typeof(double)
                   )
                {
                    format = "{0:F2}";
                }
                else if (dataColumn.DataType == typeof(DateTime))
                {
                    format = "{0:G}";
                }

                var columnDGT = new DataGridTextColumn()
                {
                    Header = attributeName.Replace("_", "__"),
                    Width = DataGridLength.Auto,

                    SortMemberPath = dataColumnName,

                    Binding = new Binding(dataColumnName)
                    {
                        Mode = BindingMode.OneTime,

                        StringFormat = format,
                        ConverterCulture = CultureInfo.CurrentCulture,
                    },
                };

                dGrResults.Columns.Add(columnDGT);
            }
        }

        private void dGrResults_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = e.OriginalSource as Hyperlink;

            if (link != null && link.NavigateUri != null && !string.IsNullOrEmpty(link.NavigateUri.AbsoluteUri))
            {
                System.Diagnostics.Process.Start(link.NavigateUri.AbsoluteUri);
            }
        }

        private async Task<IOrganizationServiceExtented> GetServiceAsync(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return null;
            }

            if (_connectionCache.ContainsKey(connectionData.ConnectionId))
            {
                return _connectionCache[connectionData.ConnectionId];
            }

            ToggleControls(connectionData, false, string.Empty);

            try
            {
                var service = await QuickConnection.ConnectAndWriteToOutputAsync(_iWriteToOutput, connectionData);

                if (service != null)
                {
                    _connectionCache[connectionData.ConnectionId] = service;

                    this._serviceLocker.Lock(service);
                }

                return service;
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

        private void FillParametersValues(XElement doc, ConnectionData connectionData)
        {
            var entityElements = doc.DescendantsAndSelf().Where(IsConditonElement).ToList();

            foreach (var entity in entityElements)
            {
                var attrValue = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "value", StringComparison.InvariantCultureIgnoreCase));

                if (attrValue != null && !string.IsNullOrEmpty(attrValue.Value))
                {
                    var valueText = attrValue.Value;

                    if (valueText.StartsWith("@")
                        || (valueText.StartsWith("{") && valueText.EndsWith("}") && !Guid.TryParse(valueText, out _))
                    )
                    {
                        var parameter = connectionData.FetchXmlRequestParameterList.FirstOrDefault(p => string.Equals(p.Name, valueText));

                        if (parameter != null)
                        {
                            attrValue.Value = parameter.Value ?? string.Empty;
                        }
                    }
                }
            }
        }

        private bool CheckParametersAndReturnHasNew(XElement doc, ConnectionData connectionData)
        {
            bool hasNew = false;

            var entityElements = doc.DescendantsAndSelf().Where(IsConditonElement).ToList();

            foreach (var entity in entityElements)
            {
                var attrValue = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "value", StringComparison.InvariantCultureIgnoreCase));

                if (attrValue != null && !string.IsNullOrEmpty(attrValue.Value))
                {
                    var valueText = attrValue.Value;

                    if (valueText.StartsWith("@")
                        || (valueText.StartsWith("{") && valueText.EndsWith("}") && !Guid.TryParse(valueText, out _))
                        )
                    {
                        var parameter = connectionData.FetchXmlRequestParameterList.FirstOrDefault(p => string.Equals(p.Name, valueText));

                        if (parameter == null)
                        {
                            var newParameter = new FetchXmlRequestParameter()
                            {
                                Name = valueText,
                            };

                            connectionData.FetchXmlRequestParameterList.Add(newParameter);

                            if (!hasNew)
                            {
                                hasNew = true;

                                dGrParameters.SelectedItem = newParameter;
                            }
                        }
                    }
                }
            }

            return hasNew;
        }

        private static bool IsConditonElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "condition", StringComparison.InvariantCultureIgnoreCase);
        }

        private void ClearGridAndTextBox()
        {
            this.Dispatcher.Invoke(() =>
            {
                tbFetchXml.IsSelected = true;
                tbFetchXml.Focus();

                txtBErrorText.Text = string.Empty;

                tbErrorText.Visibility = tbResults.Visibility = Visibility.Collapsed;
                tbErrorText.IsEnabled = tbResults.IsEnabled = false;

                dGrResults.Columns.Clear();
                dGrResults.Items.DetachFromSourceCollection();
                dGrResults.ItemsSource = null;

                this._selectedItem = tbFetchXml;
            });
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems.OfType<ConnectionData>())
            {
                removed.Save();
            }

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                ClearGridAndTextBox();

                UpdateStatus(this.GetSelectedConnection(), string.Empty);

                if (cmBCurrentConnection.SelectedItem is ConnectionData connectionData)
                {
                    BindCollections(connectionData);
                }
            });

            OnConnectionChanged();

            if (!IsControlsEnabled)
            {
                return;
            }
        }

        private void BindCollections(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return;
            }

            if (!_syncCacheObjects.ContainsKey(connectionData.ConnectionId))
            {
                _syncCacheObjects.Add(connectionData.ConnectionId, new object());

                BindingOperations.EnableCollectionSynchronization(connectionData.FetchXmlRequestParameterList, _syncCacheObjects[connectionData.ConnectionId]);
            }
        }

        private void dGrResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DataRowView item = WindowBase.GetItemFromRoutedDataContext<DataRowView>(e);

                if (item != null
                    && item[EntityDescriptionHandler.ColumnOriginalEntity] != null
                    && item[EntityDescriptionHandler.ColumnOriginalEntity] is Entity entity
                )
                {
                    this.GetSelectedConnection()?.OpenEntityInstanceInWeb(entity.LogicalName, entity.Id);
                }
            }
        }

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(IsControlsEnabled
                , this.tSProgressBar
                , this.cmBCurrentConnection
                , this.btnSetCurrentConnection
                , this.tSProgressBar
                , this.btnExecuteFetchXml
                , this.btnExecuteFetchXml2
                , this.stBtnExecuteFetchXml
                , this.dGrParameters
            );

            ToggleControl(IsControlsEnabled
                && _entityCollection != null
                && !string.IsNullOrEmpty(_entityCollection.EntityName)

                , this.btnOpenEntityCustomizationInWeb
                , this.btnOpenEntityExplorer
                , this.btnOpenEntityListInWeb
                , this.btnCreateEntityInstance
            );

            ToggleControl(IsControlsEnabled && _entityCollection != null && _entityCollection.Entities.Any(en => en.Id != Guid.Empty)
                , this.menuSelectedEntities
                , this.menuAllEntities
                , this.menuTransferToConnection
            );
        }

        protected void ToggleControl(bool enabled, params Control[] controlsArray)
        {
            if (controlsArray == null || !controlsArray.Any())
            {
                return;
            }

            foreach (var control in controlsArray)
            {
                if (control == null)
                {
                    continue;
                }

                control.Dispatcher.Invoke(() =>
                {
                    if (control is TextBox textBox)
                    {
                        textBox.IsReadOnly = !enabled;
                    }
                    else if (control is ProgressBar progressBar)
                    {
                        progressBar.IsIndeterminate = !enabled;
                    }
                    else
                    {
                        control.IsEnabled = enabled;
                    }
                });
            }
        }

        private void mIOpenEntityCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            this.GetSelectedConnection()?.OpenEntityMetadataInWeb(entity.LogicalName);
        }

        private void mIOpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var dialog = new WindowSelectEntityName(connectionData, "EntityName", entity.LogicalName);

            if (dialog.ShowDialog().GetValueOrDefault())
            {
                string entityName = dialog.EntityTypeName;

                var dialogConnectionData = dialog.GetConnectionData();

                var idEntityMetadata = dialogConnectionData.GetEntityMetadataId(entityName);

                if (idEntityMetadata.HasValue)
                {
                    var commonConfig = CommonConfiguration.Get();

                    this._iWriteToOutput.OpenFetchXmlFile(dialogConnectionData, commonConfig, entityName);
                }
            }
        }

        private void mIOpenInstanceInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            this.GetSelectedConnection()?.OpenEntityInstanceInWeb(entity.LogicalName, entity.Id);
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            this.GetSelectedConnection()?.OpenEntityInstanceListInWeb(entity.LogicalName);
        }

        private async void mIOpenEntityExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            Views.WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entity.LogicalName);
        }

        private async void miCreateNewEntityInstance_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, entity.LogicalName);
        }

        private void mICopyEntityId_Click(object sender, RoutedEventArgs e)
        {
            if (TryFindEntityFromDataRowView(e, out var entity))
            {
                ClipboardHelper.SetText(entity.Id.ToString());
            }
            else
            {
                ClipboardHelper.Clear();
            }
        }

        private void mICopyEntityName_Click(object sender, RoutedEventArgs e)
        {
            if (TryFindEntityFromDataRowView(e, out var entity))
            {
                ClipboardHelper.SetText(entity.LogicalName);
            }
            else
            {
                ClipboardHelper.Clear();
            }
        }

        private void mICopyEntityUrl_Click(object sender, RoutedEventArgs e)
        {
            if (TryFindEntityFromDataRowView(e, out var entity))
            {
                var url = this.GetSelectedConnection()?.GetEntityInstanceUrl(entity.LogicalName, entity.Id);

                ClipboardHelper.SetText(url);
            }
            else
            {
                ClipboardHelper.Clear();
            }
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var entityFull = await service.RetrieveByQueryAsync<Entity>(entity.LogicalName, entity.Id, ColumnSetInstances.AllColumns);

            var commonConfig = CommonConfiguration.Get();

            string fileName = EntityFileNameFormatter.GetEntityName(service.ConnectionData.Name, entityFull, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, entityFull, service.ConnectionData);

            _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityDescriptionFormat3
                , service.ConnectionData.Name
                , entityFull.LogicalName
                , filePath
            );

            _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        private bool TryFindEntityFromDataRowView(RoutedEventArgs e, out Entity entity)
        {
            entity = null;

            DataRowView item = WindowBase.GetItemFromRoutedDataContext<DataRowView>(e);

            if (item != null
                && item[EntityDescriptionHandler.ColumnOriginalEntity] != null
                && item[EntityDescriptionHandler.ColumnOriginalEntity] is Entity result
            )
            {
                entity = result;

                return true;
            }

            return false;
        }

        private void mIOpenEntityReferenceCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                return;
            }

            this.GetSelectedConnection()?.OpenEntityMetadataInWeb(entityReferenceView.LogicalName);
        }

        private void mIOpenEntityReferenceFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var dialog = new WindowSelectEntityName(connectionData, "EntityName", entityReferenceView.LogicalName);

            if (dialog.ShowDialog().GetValueOrDefault())
            {
                string entityName = dialog.EntityTypeName;

                var dialogConnectionData = dialog.GetConnectionData();

                var idEntityMetadata = dialogConnectionData.GetEntityMetadataId(entityName);

                if (idEntityMetadata.HasValue)
                {
                    var commonConfig = CommonConfiguration.Get();

                    this._iWriteToOutput.OpenFetchXmlFile(dialogConnectionData, commonConfig, entityName);
                }
            }
        }

        private void mIOpenEntityReferenceInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                return;
            }

            this.GetSelectedConnection()?.OpenEntityInstanceInWeb(entityReferenceView.LogicalName, entityReferenceView.Id);
        }

        private void mIOpenEntityReferenceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                return;
            }

            this.GetSelectedConnection()?.OpenEntityInstanceListInWeb(entityReferenceView.LogicalName);
        }

        private async void mIOpenEntityReferenceExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            Views.WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entityReferenceView.LogicalName);
        }

        private async void miCreateNewEntityReferenceInstance_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, entityReferenceView.LogicalName);
        }

        private void mICopyEntityReferenceId_Click(object sender, RoutedEventArgs e)
        {
            if (TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                ClipboardHelper.SetText(entityReferenceView.Id.ToString());
            }
            else
            {
                ClipboardHelper.Clear();
            }
        }

        private void mICopyEntityReferenceEntityName_Click(object sender, RoutedEventArgs e)
        {
            if (TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                ClipboardHelper.SetText(entityReferenceView.LogicalName);
            }
            else
            {
                ClipboardHelper.Clear();
            }
        }

        private void mICopyEntityReferenceName_Click(object sender, RoutedEventArgs e)
        {
            if (TryFindEntityReferenceViewFromRow(e, out EntityReferenceView entityReferenceView))
            {
                ClipboardHelper.SetText(entityReferenceView.Name);
            }
            else
            {
                ClipboardHelper.Clear();
            }
        }

        private void mICopyEntityReferenceUrl_Click(object sender, RoutedEventArgs e)
        {
            if (TryFindNavigateUriFromHyperlink(e, out var navigateUri))
            {
                ClipboardHelper.SetText(navigateUri.AbsoluteUri);
            }
            else
            {
                ClipboardHelper.Clear();
            }
        }

        private async void mICreateEntityReferenceDescription_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var entityFull = await service.RetrieveByQueryAsync<Entity>(entityReferenceView.LogicalName, entityReferenceView.Id, ColumnSetInstances.AllColumns);

            var commonConfig = CommonConfiguration.Get();

            string fileName = EntityFileNameFormatter.GetEntityName(service.ConnectionData.Name, entityFull, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, entityFull, service.ConnectionData);

            _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityDescriptionFormat3
                , service.ConnectionData.Name
                , entityFull.LogicalName
                , filePath
            );

            _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        private bool TryFindEntityReferenceViewFromRow<T>(RoutedEventArgs e, out T result) where T : PrimaryGuidView
        {
            result = null;

            ContextMenu contextMenu = null;

            if (e.OriginalSource is ContextMenu)
            {
                contextMenu = e.OriginalSource as ContextMenu;
            }
            else if (e.OriginalSource is MenuItem menuItem)
            {
                contextMenu = GetContextMenuFromMenuItem(menuItem);
            }

            if (contextMenu == null
                || contextMenu.PlacementTarget == null
                || !(contextMenu.PlacementTarget is TextBlock textBlock)
                || !(contextMenu.DataContext is DataRowView dataRowView)
                || !(textBlock.Parent is DataGridCell cell)
            )
            {
                return false;
            }

            if (string.IsNullOrEmpty(cell.Column.SortMemberPath))
            {
                return false;
            }

            if (!dataRowView.Row.Table.Columns.Contains(cell.Column.SortMemberPath))
            {
                return false;
            }

            if (!(dataRowView[cell.Column.SortMemberPath] is T value))
            {
                return false;
            }

            result = value;

            return true;
        }

        private ContextMenu GetContextMenuFromMenuItem(MenuItem menuItem)
        {
            ItemsControl result = menuItem;

            do
            {
                result = ItemsControl.ItemsControlFromItemContainer(result);

                if (result != null && result is ContextMenu contextMenu)
                {
                    return contextMenu;
                }

            } while (result != null);

            return null;
        }

        private bool TryFindNavigateUriFromHyperlink(RoutedEventArgs e, out Uri navigateUri)
        {
            navigateUri = null;

            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return false;
            }

            var contextMenu = GetContextMenuFromMenuItem(menuItem);

            if (contextMenu == null
                || contextMenu.PlacementTarget == null
                || !(contextMenu.PlacementTarget is TextBlock textBlock)
                )
            {
                return false;
            }

            var link = textBlock.Inlines.OfType<Hyperlink>().FirstOrDefault();

            if (link == null
               || link.NavigateUri == null
               || string.IsNullOrEmpty(link.NavigateUri.AbsoluteUri)
               )
            {
                return false;
            }

            navigateUri = link.NavigateUri;

            return true;
        }

        private void btnCopyJavaScriptToClipboardCode_Click(object sender, RoutedEventArgs e)
        {
            ConvertFetchXmlToJavaScriptCodeInternal();
        }

        private void ConvertFetchXmlToJavaScriptCodeInternal()
        {
            var fileText = txtBFetchXml.Text.Trim();

            string jsCode = ContentComparerHelper.FormatToJavaScript(SavedQuery.Schema.Variables.fetchxml, fileText);

            ClipboardHelper.SetText(jsCode);
        }

        private void btnSelectFileInFolder_Click(object sender, RoutedEventArgs e)
        {
            _iWriteToOutput.SelectFileInFolder(this.GetSelectedConnection(), FilePath);
        }

        private IEnumerable<Entity> GetSelectedEntities()
        {
            HashSet<Guid> hash = new HashSet<Guid>();

            var selectedCells = dGrResults.SelectedCells.ToList();

            foreach (var cell in selectedCells)
            {
                if (cell.Item is DataRowView item
                    && item[EntityDescriptionHandler.ColumnOriginalEntity] != null
                    && item[EntityDescriptionHandler.ColumnOriginalEntity] is Entity entity
                    && entity.Id != Guid.Empty
                )
                {
                    if (hash.Add(entity.Id))
                    {
                        yield return entity;
                    }
                }
            }
        }

        #region Execute Workflow

        private async void mIExecuteWorkflowOnEntity_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                await ExecuteWorkflowOnEntities(entity.LogicalName, new[] { entity.Id });
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miExecuteWorkflowOnAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            try
            {
                await ExecuteWorkflowOnEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miExecuteWorkflowOnSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await ExecuteWorkflowOnEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async Task ExecuteWorkflowOnEntities(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var listIds = entityIds.Where(e => e != Guid.Empty).Distinct().ToList();

            if (!listIds.Any())
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new WorkflowRepository(service);

            Func<string, Task<IEnumerable<Workflow>>> getter = (string filter) => repository.GetListAsync(
                entityName
                , Workflow.Schema.OptionSets.category.Workflow_0
                , null
                , null
                , new ColumnSet
                (
                    Workflow.Schema.Attributes.workflowid
                    , Workflow.Schema.Attributes.category
                    , Workflow.Schema.Attributes.name
                    , Workflow.Schema.Attributes.mode
                    , Workflow.Schema.Attributes.uniquename
                    , Workflow.Schema.Attributes.primaryentity
                    , Workflow.Schema.Attributes.iscustomizable
                    , Workflow.Schema.Attributes.statuscode
                )
            );

            IEnumerable<DataGridColumn> columns = WorkflowRepository.GetDataGridColumn();

            var form = new WindowEntitySelect<Workflow>(_iWriteToOutput, service.ConnectionData, Workflow.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            Workflow workflow = form.SelectedEntity;

            string operationName = string.Format(Properties.OperationNames.ExecutingWorkflowFormat4
                , service.ConnectionData.Name
                , workflow.Name
                , entityName
                , listIds.Count
            );

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false
                , Properties.OutputStrings.InConnectionExecutingWorkflowFormat4
                , service.ConnectionData.Name
                , workflow.Name
                , entityName
                , listIds.Count
            );

            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            var request = new ExecuteWorkflowRequest()
            {
                WorkflowId = workflow.Id,
            };

            int number = 1;

            foreach (var id in listIds)
            {
                try
                {
                    request.EntityId = id;

                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExecutingOnEntityWorkflowFormat3, workflow.Name, number, listIds.Count);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, entityName, id);

                    await service.ExecuteAsync<ExecuteWorkflowResponse>(request);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                number++;
            }

            ToggleControls(service.ConnectionData, true
                , Properties.OutputStrings.ExecutingWorkflowCompletedFormat4
                , service.ConnectionData.Name
                , workflow.Name
                , entityName
                , listIds.Count
            );

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        #endregion Execute Workflow

        #region Set Business Process Flow

        private async void mISetBusinessProcessFlow_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                await SetBusinessProcessFlowOnEntities(entity.LogicalName, new[] { entity.Id });
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miSetBusinessProcessFlowOnSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await SetBusinessProcessFlowOnEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miSetBusinessProcessFlowOnAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            try
            {
                await SetBusinessProcessFlowOnEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async Task SetBusinessProcessFlowOnEntities(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var listIds = entityIds.Where(e => e != Guid.Empty).Distinct().ToList();

            if (!listIds.Any())
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new WorkflowRepository(service);

            Func<string, Task<IEnumerable<Workflow>>> getter = (string filter) => repository.GetListAsync(
                entityName
                , Workflow.Schema.OptionSets.category.Business_Process_Flow_4
                , null
                , null
                , new ColumnSet
                (
                    Workflow.Schema.Attributes.workflowid
                    , Workflow.Schema.Attributes.category
                    , Workflow.Schema.Attributes.name
                    , Workflow.Schema.Attributes.mode
                    , Workflow.Schema.Attributes.uniquename
                    , Workflow.Schema.Attributes.primaryentity
                    , Workflow.Schema.Attributes.iscustomizable
                    , Workflow.Schema.Attributes.statuscode
                )
            );

            IEnumerable<DataGridColumn> columns = WorkflowRepository.GetDataGridColumn();

            var form = new WindowEntitySelect<Workflow>(_iWriteToOutput, service.ConnectionData, Workflow.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            Workflow workflow = form.SelectedEntity;

            string operationName = string.Format(Properties.OperationNames.SettingBusinessProcessFlowFormat4
                , service.ConnectionData.Name
                , workflow.Name
                , entityName
                , listIds.Count
            );

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false
                , Properties.OutputStrings.InConnectionSettingBusinessProcessFlowFormat4
                , service.ConnectionData.Name
                , workflow.Name
                , entityName
                , listIds.Count
            );

            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            var request = new SetProcessRequest()
            {
                NewProcess = workflow.ToEntityReference(),
            };

            int number = 1;

            foreach (var id in listIds)
            {
                try
                {
                    request.Target = new EntityReference(entityName, id);

                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SettingOnEntityBusinessProcessFlowFormat3, workflow.Name, number, listIds.Count);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, entityName, id);

                    await service.ExecuteAsync<SetProcessResponse>(request);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                number++;
            }

            ToggleControls(service.ConnectionData, true
                , Properties.OutputStrings.SettingBusinessProcessFlowCompletedFormat4
                , service.ConnectionData.Name
                , workflow.Name
                , entityName
                , listIds.Count
            );

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        #endregion Set Business Process Flow

        #region Associate Entities

        private async void mIAssociateEntity_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                await AssociateEntities(entity.LogicalName, new[] { entity.Id });
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miAssociateSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await AssociateEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miAssociateAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            try
            {
                await AssociateEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async Task AssociateEntities(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var listIds = entityIds.Where(e => e != Guid.Empty).Distinct().ToList();

            if (!listIds.Any())
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var form = new WindowSelectEntityAndRelationshipName(connectionData, entityName, "Entity and Relationship", string.Empty);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            string targenEntityName = form.EntityName;
            Guid targenEntityId = form.EntityId;
            string relationshipName = form.RelationshipName;

            if (string.IsNullOrEmpty(targenEntityName))
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.MessageBoxStrings.EntityNameIsEmpty);
                return;
            }

            if (string.IsNullOrEmpty(relationshipName))
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.MessageBoxStrings.RelationshipNameIsEmpty);
                return;
            }

            string operationName = string.Format(Properties.OperationNames.AssociatingEntitiesToEntityFormat6
                , service.ConnectionData.Name
                , entityName
                , listIds.Count

                , relationshipName
                , targenEntityName
                , targenEntityId
            );

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false
                , Properties.OutputStrings.InConnectionAssociatingEntitiesToEntityFormat6
                , service.ConnectionData.Name
                , entityName
                , listIds.Count

                , relationshipName
                , targenEntityName
                , targenEntityId
            );

            _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, targenEntityName, targenEntityId);

            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            try
            {
                var relatedEntities = listIds.Select(i => new EntityReference(entityName, i)).ToList();

                var request = new AssociateRequest()
                {
                    Target = new EntityReference(targenEntityName, targenEntityId),
                    Relationship =  new Relationship(relationshipName),

                    RelatedEntities = new EntityReferenceCollection(relatedEntities)
                };

                await service.ExecuteAsync<AssociateResponse>(request);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true
                , Properties.OutputStrings.InConnectionAssociatingEntitiesToEntityCompletedFormat6
                , service.ConnectionData.Name
                , entityName
                , listIds.Count

                , relationshipName
                , targenEntityName
                , targenEntityId
            );

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        #endregion Associate Entities

        #region Disassociate Entities

        private async void mIDisassociateEntity_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                await DisassociateEntities(entity.LogicalName, new[] { entity.Id });
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miDisassociateSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await DisassociateEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miDisassociateAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            try
            {
                await DisassociateEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async Task DisassociateEntities(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var listIds = entityIds.Where(e => e != Guid.Empty).Distinct().ToList();

            if (!listIds.Any())
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var form = new WindowSelectEntityAndRelationshipName(connectionData, entityName, "Entity and Relationship", string.Empty);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            string targenEntityName = form.EntityName;
            Guid targenEntityId = form.EntityId;
            string relationshipName = form.RelationshipName;

            if (string.IsNullOrEmpty(targenEntityName))
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.MessageBoxStrings.EntityNameIsEmpty);
                return;
            }

            if (string.IsNullOrEmpty(relationshipName))
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.MessageBoxStrings.RelationshipNameIsEmpty);
                return;
            }

            string operationName = string.Format(Properties.OperationNames.DisassociatingEntitiesToEntityFormat6
                , service.ConnectionData.Name
                , entityName
                , listIds.Count

                , relationshipName
                , targenEntityName
                , targenEntityId
            );

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false
                , Properties.OutputStrings.InConnectionDisassociatingEntitiesToEntityFormat6
                , service.ConnectionData.Name
                , entityName
                , listIds.Count

                , relationshipName
                , targenEntityName
                , targenEntityId
            );

            _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, targenEntityName, targenEntityId);
            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            try
            {
                var relatedEntities = listIds.Select(i => new EntityReference(entityName, i)).ToList();

                var request = new DisassociateRequest()
                {
                    Target = new EntityReference(targenEntityName, targenEntityId),
                    Relationship = new Relationship(relationshipName),

                    RelatedEntities = new EntityReferenceCollection(relatedEntities)
                };

                await service.ExecuteAsync<DisassociateResponse>(request);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true
                , Properties.OutputStrings.InConnectionDisassociatingEntitiesToEntityCompletedFormat6
                , service.ConnectionData.Name
                , entityName
                , listIds.Count

                , relationshipName
                , targenEntityName
                , targenEntityId
            );

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        #endregion Disassociate Entities

        #region Assign to User

        private async void mIAssignEntityToUser_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                await AssignEntitiesToUser(entity.LogicalName, new[] { entity.Id });
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miAssignToUserAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            try
            {
                await AssignEntitiesToUser(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miAssignToUserSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await AssignEntitiesToUser(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async Task AssignEntitiesToUser(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var listIds = entityIds.Where(e => e != Guid.Empty).Distinct().ToList();

            if (!listIds.Any())
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new SystemUserRepository(service);

            Func<string, Task<IEnumerable<SystemUser>>> getter = (string filter) => repository.GetUsersAsync(filter
                , new ColumnSet(
                    SystemUser.Schema.Attributes.domainname
                    , SystemUser.Schema.Attributes.fullname
                    , SystemUser.Schema.Attributes.businessunitid
                    , SystemUser.Schema.Attributes.isdisabled
                )
            );

            IEnumerable<DataGridColumn> columns = SystemUserRepository.GetDataGridColumn();

            var form = new WindowEntitySelect<SystemUser>(_iWriteToOutput, service.ConnectionData, SystemUser.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            SystemUser user = form.SelectedEntity;

            string operationName = string.Format(Properties.OperationNames.AssigningEntitiesToUserFormat4
                , service.ConnectionData.Name
                , entityName
                , listIds.Count
                , user.FullName
            );

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false
                , Properties.OutputStrings.InConnectionAssigningEntitiesToUserFormat4
                , service.ConnectionData.Name
                , entityName
                , listIds.Count
                , user.FullName
            );

            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            var request = new AssignRequest()
            {
                Assignee = user.ToEntityReference(),
            };

            int number = 1;

            foreach (var id in listIds)
            {
                try
                {
                    request.Target = new EntityReference(entityName, id);

                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.AssigningEntityToUserFormat3, number, listIds.Count, user.FullName);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, entityName, id);

                    await service.ExecuteAsync<AssignResponse>(request);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                number++;
            }

            ToggleControls(service.ConnectionData, true
                , Properties.OutputStrings.InConnectionAssigningEntitiesToUserCompletedFormat4
                , service.ConnectionData.Name
                , entityName
                , listIds.Count
                , user.FullName
            );

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        #endregion Assign to User

        #region Assign to Team

        private async void mIAssignEntityToTeam_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                await AssignEntitiesToTeam(entity.LogicalName, new[] { entity.Id });
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miAssignToTeamAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            try
            {
                await AssignEntitiesToTeam(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miAssignToTeamSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await AssignEntitiesToTeam(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async Task AssignEntitiesToTeam(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var listIds = entityIds.Where(e => e != Guid.Empty).Distinct().ToList();

            if (!entityIds.Any(id => id != Guid.Empty))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new TeamRepository(service);

            Func<string, Task<IEnumerable<Team>>> getter = (string filter) => repository.GetOwnerTeamsAsync(filter
                , new ColumnSet(
                    Team.Schema.Attributes.name
                    , Team.Schema.Attributes.businessunitid
                    , Team.Schema.Attributes.isdefault
                )
            );

            IEnumerable<DataGridColumn> columns = TeamRepository.GetDataGridColumnOwner();

            var form = new WindowEntitySelect<Team>(_iWriteToOutput, service.ConnectionData, Team.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            Team team = form.SelectedEntity;

            string operationName = string.Format(Properties.OperationNames.AssigningEntitiesToTeamFormat4
                , service.ConnectionData.Name
                , entityName
                , listIds.Count
                , team.Name
            );

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false
                , Properties.OutputStrings.InConnectionAssigningEntitiesToTeamFormat4
                , service.ConnectionData.Name
                , entityName
                , listIds.Count
                , team.Name
            );

            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            var request = new AssignRequest()
            {
                Assignee = team.ToEntityReference(),
            };

            int number = 1;

            foreach (var id in entityIds.Where(e => e != Guid.Empty).Distinct())
            {
                try
                {
                    request.Target = new EntityReference(entityName, id);

                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.AssigningEntityToTeamFormat3, number, listIds.Count, team.Name);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, entityName, id);

                    await service.ExecuteAsync<AssignResponse>(request);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                number++;
            }

            ToggleControls(service.ConnectionData, true
                , Properties.OutputStrings.InConnectionAssigningEntitiesToTeamCompletedFormat4
                , service.ConnectionData.Name
                , entityName
                , listIds.Count
                , team.Name
            );

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        #endregion Assign to Team

        #region Entity Actions

        private void btnOpenEntityCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (_entityCollection == null
                || string.IsNullOrEmpty(_entityCollection.EntityName)
            )
            {
                return;
            }

            this.GetSelectedConnection()?.OpenEntityMetadataInWeb(_entityCollection.EntityName);
        }

        private void btnOpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var dialog = new WindowSelectEntityName(connectionData, "EntityName", _entityCollection?.EntityName);

            if (dialog.ShowDialog().GetValueOrDefault())
            {
                string entityName = dialog.EntityTypeName;

                var dialogConnectionData = dialog.GetConnectionData();

                var idEntityMetadata = dialogConnectionData.GetEntityMetadataId(entityName);

                if (idEntityMetadata.HasValue)
                {
                    var commonConfig = CommonConfiguration.Get();

                    this._iWriteToOutput.OpenFetchXmlFile(dialogConnectionData, commonConfig, entityName);
                }
            }
        }

        private void btnOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (_entityCollection == null
                || string.IsNullOrEmpty(_entityCollection.EntityName)
            )
            {
                return;
            }

            this.GetSelectedConnection()?.OpenEntityInstanceListInWeb(_entityCollection.EntityName);
        }

        private async void btnOpenEntityExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (_entityCollection == null
                || string.IsNullOrEmpty(_entityCollection.EntityName)
            )
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            Views.WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, _entityCollection.EntityName);
        }

        private async void btnCreateEntityInstance_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || string.IsNullOrEmpty(_entityCollection.EntityName)
            )
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, _entityCollection.EntityName);
        }

        #endregion Entity Actions

        #region Edit Entity

        private async void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, entity.LogicalName, entity.Id);
        }

        private async void mIChangeEntityReferenceInEditor_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, entityReferenceView.LogicalName, entityReferenceView.Id);
        }

        #endregion Edit Entity

        #region Create Entity Copy

        private async void mICreateEntityCopyInEditor_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var entityFull = await service.RetrieveByQueryAsync<Entity>(entity.LogicalName, entity.Id, ColumnSetInstances.AllColumns);

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, entity.LogicalName, entityFull);
        }

        private async void mICreateEntityReferenceCopyInEditor_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var entityFull = await service.RetrieveByQueryAsync<Entity>(entityReferenceView.LogicalName, entityReferenceView.Id, ColumnSetInstances.AllColumns);

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, entityReferenceView.LogicalName, entityFull);
        }

        #endregion Create Entity Copy

        #region Bulk Edit Entities

        private async void miBulkEditAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            try
            {
                await BulkEditEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void miBulkEditSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await BulkEditEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async Task BulkEditEntities(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!entityIds.Any(id => id != Guid.Empty))
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var entitiesIds = entityIds.Where(i => i != Guid.Empty).Distinct().ToList();

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenEntityBulkEditor(_iWriteToOutput, service, commonConfig, entityName, entitiesIds);
        }

        #endregion Bulk Edit Entities

        #region SetState Entity

        private async void mISetStateEntity_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                await SetStateEntities(entity.LogicalName, new[] { entity.Id });
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void mISetStateSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await SetStateEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void mISetStateAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            try
            {
                await SetStateEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async Task SetStateEntities(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var listIds = entityIds.Where(e => e != Guid.Empty).Distinct().ToList();

            if (!listIds.Any())
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            var form = new WindowStatusSelect(_iWriteToOutput, commonConfig, service, entityName);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedStatusCodeViewItem == null)
            {
                return;
            }

            StatusCodeViewItem statusOption = form.SelectedStatusCodeViewItem;

            string operationName = string.Format(Properties.OperationNames.SettingEntitiesStateFormat7
                , service.ConnectionData.Name
                , entityName
                , listIds.Count
                , statusOption.StateCode
                , statusOption.StateCodeName
                , statusOption.StatusCode
                , statusOption.StatusCodeName
            );

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false
                , Properties.OutputStrings.InConnectionSettingEntitiesStateFormat7
                , service.ConnectionData.Name
                , entityName
                , listIds.Count
                , statusOption.StateCode
                , statusOption.StateCodeName
                , statusOption.StatusCode
                , statusOption.StatusCodeName
            );

            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            var request = new SetStateRequest()
            {
                State = new OptionSetValue(statusOption.StatusOptionMetadata.State.Value),
                Status = new OptionSetValue(statusOption.StatusOptionMetadata.Value.Value),
            };

            int number = 1;

            foreach (var id in listIds)
            {
                try
                {
                    request.EntityMoniker = new EntityReference(entityName, id);

                    _iWriteToOutput.WriteToOutput(service.ConnectionData
                        , Properties.OutputStrings.SettingEntityStateFormat6
                        , number
                        , listIds.Count

                        , statusOption.StateCode
                        , statusOption.StateCodeName

                        , statusOption.StatusCode
                        , statusOption.StatusCodeName
                    );

                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, entityName, id);

                    await service.ExecuteAsync<SetStateResponse>(request);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                number++;
            }

            ToggleControls(service.ConnectionData, true
                , Properties.OutputStrings.InConnectionSettingEntitiesStateCompletedFormat7
                , service.ConnectionData.Name
                , entityName
                , listIds.Count
                , statusOption.StateCode
                , statusOption.StateCodeName
                , statusOption.StatusCode
                , statusOption.StatusCodeName
            );

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        #endregion SetState Entity

        #region Transfer Entities

        private void mITransferToConnection_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            mITransferAllEntitiesToConnection.Items.Clear();
            mITransferSelectedEntitiesToConnection.Items.Clear();

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            var otherConnections = connectionData.ConnectionConfiguration.Connections.Where(c => c.ConnectionId != connectionData.ConnectionId).ToList();

            if (otherConnections.Any())
            {
                foreach (var connection in otherConnections)
                {
                    {
                        MenuItem menuItem = new MenuItem()
                        {
                            Header = connection.Name,
                            Tag = connection,
                        };
                        menuItem.Click += mITransferAllEntitiesToConnection_Click;

                        if (mITransferAllEntitiesToConnection.Items.Count > 0)
                        {
                            mITransferAllEntitiesToConnection.Items.Add(new Separator());
                        }

                        mITransferAllEntitiesToConnection.Items.Add(menuItem);
                    }

                    {
                        MenuItem menuItem = new MenuItem()
                        {
                            Header = connection.Name,
                            Tag = connection,
                        };
                        menuItem.Click += mITransferSelectedEntitiesToConnection_Click;

                        if (mITransferSelectedEntitiesToConnection.Items.Count > 0)
                        {
                            mITransferSelectedEntitiesToConnection.Items.Add(new Separator());
                        }

                        mITransferSelectedEntitiesToConnection.Items.Add(menuItem);
                    }
                }
            }
        }

        private async void mITransferAllEntitiesToConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is ConnectionData targetConnectionData)
            )
            {
                return;
            }

            try
            {
                await TransferEntities(targetConnectionData, _entityCollection.EntityName, _entityCollection.Entities.Where(en => en.Id != Guid.Empty));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void mITransferSelectedEntitiesToConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Entity> selectedEntities = GetSelectedEntities();

            if (!selectedEntities.Any())
            {
                return;
            }

            if (!(sender is MenuItem menuItem)
               || menuItem.Tag == null
               || !(menuItem.Tag is ConnectionData targetConnectionData)
            )
            {
                return;
            }

            try
            {
                await TransferEntities(targetConnectionData, _entityCollection.EntityName, selectedEntities);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async Task TransferEntities(ConnectionData targetConnectionData, string entityName, IEnumerable<Entity> entities)
        {
            var targetService = await GetServiceAsync(targetConnectionData);

            if (targetService == null)
            {
                return;
            }

            ToggleControls(targetService.ConnectionData, false, Properties.OutputStrings.GettingEntityMetadataFormat1, _entityCollection.EntityName);

            EntityMetadataRepository repository = new EntityMetadataRepository(targetService);

            var targetEntityMetadata = await repository.GetEntityMetadataAsync(_entityCollection.EntityName);

            ToggleControls(targetService.ConnectionData, true, Properties.OutputStrings.GettingEntityMetadataCompletedFormat1, _entityCollection.EntityName);

            if (targetEntityMetadata == null)
            {
                this._iWriteToOutput.WriteToOutput(targetService.ConnectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, targetService.ConnectionData.Name);
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenEntityBulkTransfer(_iWriteToOutput, targetService, commonConfig, targetEntityMetadata, entities);
        }

        #endregion Transfer Entities

        #region Delete Entities

        private async void mIDeleteEntity_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                await DeleteEntities(entity.LogicalName, new[] { entity.Id });
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void mIDeleteSelectedEntity_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await DeleteEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void mIDeleteAllEntity_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            try
            {
                await DeleteEntities(_entityCollection.EntityName, selectedEntityIds);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async Task DeleteEntities(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var listIds = entityIds.Where(e => e != Guid.Empty).Distinct().ToList();

            if (!listIds.Any())
            {
                return;
            }

            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            string question = string.Format(Properties.MessageBoxStrings.DeleteEntityCountFormat2, entityName, listIds.Count);

            if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            string operationName = string.Format(Properties.OperationNames.DeletingEntitiesSetFormat3, service.ConnectionData.Name, entityName, listIds.Count);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionDeletingEntitiesSetFormat3, service.ConnectionData.Name, entityName, listIds.Count);

            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            int number = 1;

            foreach (var id in listIds)
            {
                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntityInSetFormat2, number, listIds.Count);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, entityName, id);

                    await service.DeleteAsync(entityName, id);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                number++;
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionDeletingEntitiesSetCompletedFormat3, service.ConnectionData.Name, entityName, listIds.Count);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        #endregion Delete Entities

        #region Adding to Solution

        private async void AddToCrmSolutionEntity_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (!SolutionComponent.GetEntityComponentType(entity.LogicalName, out var componentType))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                await AddToSolution(componentType, new[] { entity.Id }, true, null);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void AddToCrmSolutionEntityLast_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (!SolutionComponent.GetEntityComponentType(entity.LogicalName, out var componentType))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
            )
            {
                try
                {
                    await AddToSolution(componentType, new[] { entity.Id }, false, solutionUniqueName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(null, ex);
                    _iWriteToOutput.ActivateOutputWindow(null);
                }
            }
        }

        private async void AddToCrmSolutionEntityReference_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entity))
            {
                return;
            }

            if (!SolutionComponent.GetEntityComponentType(entity.LogicalName, out var componentType))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                await AddToSolution(componentType, new[] { entity.Id }, true, null);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void AddToCrmSolutionEntityReferenceLast_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entity))
            {
                return;
            }

            if (!SolutionComponent.GetEntityComponentType(entity.LogicalName, out var componentType))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
            )
            {
                try
                {
                    await AddToSolution(componentType, new[] { entity.Id }, false, solutionUniqueName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(null, ex);
                    _iWriteToOutput.ActivateOutputWindow(null);
                }
            }
        }

        private async void AddToCrmSolutionSelectedEntities_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            if (!SolutionComponent.GetEntityComponentType(_entityCollection.EntityName, out var componentType))
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await AddToSolution(componentType, selectedEntityIds, true, null);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void AddToCrmSolutionSelectedEntitiesLast_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            if (!SolutionComponent.GetEntityComponentType(_entityCollection.EntityName, out var componentType))
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntities().Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
            )
            {
                try
                {
                    await AddToSolution(componentType, selectedEntityIds, false, solutionUniqueName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(null, ex);
                    _iWriteToOutput.ActivateOutputWindow(null);
                }
            }
        }

        private async void AddToCrmSolutionAllEntities_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            if (!SolutionComponent.GetEntityComponentType(_entityCollection.EntityName, out var componentType))
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            try
            {
                await AddToSolution(componentType, selectedEntityIds, true, null);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private async void AddToCrmSolutionAllEntitiesLast_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            if (!SolutionComponent.GetEntityComponentType(_entityCollection.EntityName, out var componentType))
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id);

            if (!selectedEntityIds.Any())
            {
                return;
            }

            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
            )
            {
                try
                {
                    await AddToSolution(componentType, selectedEntityIds, false, solutionUniqueName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(null, ex);
                    _iWriteToOutput.ActivateOutputWindow(null);
                }
            }
        }

        private async Task AddToSolution(ComponentType componentType, IEnumerable<Guid> entityIds, bool withSelect, string solutionUniqueName)
        {
            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                var commonConfig = CommonConfiguration.Get();

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, commonConfig, solutionUniqueName, componentType, entityIds, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        #endregion Adding to Solution

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            bool hasSolutionComponentEntity = TryFindEntityFromDataRowView(e, out var entity);
            if (hasSolutionComponentEntity)
            {
                hasSolutionComponentEntity = SolutionComponent.GetEntityComponentType(entity?.LogicalName, out _);
            }

            bool hasSolutionComponentEntityReference = TryFindEntityReferenceViewFromRow(e, out PrimaryGuidView entityReferenceView);
            if (hasSolutionComponentEntityReference)
            {
                hasSolutionComponentEntityReference = SolutionComponent.GetEntityComponentType(entityReferenceView?.LogicalName, out _);
            }

            WindowBase.ActivateControls(items, hasSolutionComponentEntity, "contMnAddToSolution", "contMnAddToSolutionLast");
            WindowBase.ActivateControls(items, hasSolutionComponentEntityReference, "contMnAddToSolutionEntityReference", "contMnAddToSolutionEntityReferenceLast");

            ConnectionData connectionData = this.GetSelectedConnection();

            WindowBase.FillLastSolutionItems(connectionData, items, hasSolutionComponentEntity, AddToCrmSolutionEntityLast_Click, "contMnAddToSolutionLast");
            WindowBase.FillLastSolutionItems(connectionData, items, hasSolutionComponentEntityReference, AddToCrmSolutionEntityReferenceLast_Click, "contMnAddToSolutionEntityReferenceLast");
        }

        private void SelectedEntities_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
            {
                return;
            }

            var items = menuItem.Items.OfType<Control>();

            bool hasSolutionComponentEntity = _entityCollection != null
                && SolutionComponent.GetEntityComponentType(_entityCollection.EntityName, out _)
                && GetSelectedEntities().Any(en => en.Id != Guid.Empty)
                ;

            WindowBase.ActivateControls(items, hasSolutionComponentEntity, "contMnAddToSolution", "contMnAddToSolutionLast");

            ConnectionData connectionData = this.GetSelectedConnection();

            WindowBase.FillLastSolutionItems(connectionData, items, hasSolutionComponentEntity, AddToCrmSolutionSelectedEntitiesLast_Click, "contMnAddToSolutionLast");
        }

        private void AllEntities_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
            {
                return;
            }

            var items = menuItem.Items.OfType<Control>();

            bool hasSolutionComponentEntity = _entityCollection != null
                && SolutionComponent.GetEntityComponentType(_entityCollection.EntityName, out _)
                && _entityCollection.Entities.Any(en => en.Id != Guid.Empty)
                ;

            WindowBase.ActivateControls(items, hasSolutionComponentEntity, "contMnAddToSolution", "contMnAddToSolutionLast");

            ConnectionData connectionData = this.GetSelectedConnection();

            WindowBase.FillLastSolutionItems(connectionData, items, hasSolutionComponentEntity, AddToCrmSolutionAllEntitiesLast_Click, "contMnAddToSolutionLast");
        }

        #region Connection Actions

        private void mIOpenConfigFolder_Click(object sender, RoutedEventArgs e)
        {
            var directoryPath = FileOperations.GetConfigurationFolder();

            this._iWriteToOutput.OpenFolder(null, directoryPath);
        }

        private void mIOpenConnectionInformationFolder_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsNotSelected);
                return;
            }

            string directoryPath = FileOperations.GetConnectionInformationFolderPath(connectionData.ConnectionId);

            this._iWriteToOutput.OpenFolder(connectionData, directoryPath);
        }

        private void mIOpenConnectionFetchXmlFolder_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = this.GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            string directoryPath = FileOperations.GetConnectionFetchXmlFolderPath(connectionData.ConnectionId);

            this._iWriteToOutput.OpenFolder(connectionData, directoryPath);
        }

        #endregion Connection Actions

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, this.GetSelectedConnection());
        }

        protected void SetCurrentConnection(IWriteToOutput iWriteToOutput, ConnectionData connectionData)
        {
            if (connectionData == null || connectionData.ConnectionConfiguration == null)
            {
                return;
            }

            if (connectionData.ConnectionConfiguration.CurrentConnectionData?.ConnectionId == connectionData.ConnectionId)
            {
                return;
            }

            connectionData.ConnectionConfiguration.SetCurrentConnection(connectionData.ConnectionId);
            connectionData.ConnectionConfiguration.Save();
            iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);
        }
    }
}