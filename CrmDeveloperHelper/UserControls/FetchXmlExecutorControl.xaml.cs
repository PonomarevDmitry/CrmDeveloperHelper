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

        public event EventHandler<EventArgs> ConnectionChanged;

        private TabItem _selectedItem;

        private void OnConnectionChanged()
        {
            this.ConnectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public string FilePath { get; private set; }

        public ConnectionData ConnectionData => cmBCurrentConnection.SelectedItem as ConnectionData;

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
        }

        protected bool IsControlsEnabled => this._init == 0;

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

            this.dGrParameters.DataContext = cmBCurrentConnection;
        }

        private void btnExecuteFetchXml_Click(object sender, RoutedEventArgs e)
        {
            this.Execute();
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
            if (!IsControlsEnabled)
            {
                return;
            }

            ToggleControls(this.ConnectionData, false, Properties.OutputStrings.ExecutingFetch);

            ClearGridAndTextBox();

            if (!TryLoadFileText())
            {
                ToggleControls(this.ConnectionData, true, Properties.OutputStrings.FileNotExists);
                return;
            }

            if (!(cmBCurrentConnection.SelectedItem is ConnectionData connectionData))
            {
                txtBErrorText.Text = Properties.OutputStrings.ConnectionIsNotSelected;

                tbErrorText.IsEnabled = true;
                tbErrorText.Visibility = Visibility.Visible;

                tbErrorText.IsSelected = true;
                tbErrorText.Focus();

                this._selectedItem = tbErrorText;

                ToggleControls(this.ConnectionData, true, Properties.OutputStrings.ConnectionIsNotSelected);

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

                txtBErrorText.Text = text.ToString();

                tbErrorText.IsEnabled = true;
                tbErrorText.Visibility = Visibility.Visible;

                tbErrorText.IsSelected = true;
                tbErrorText.Focus();

                this._selectedItem = tbErrorText;

                ToggleControls(this.ConnectionData, true, Properties.OutputStrings.FileTextIsNotValidXml);

                return;
            }

            txtBFetchXml.Text = doc.ToString();

            if (CheckParametersAndReturnHasNew(doc, connectionData))
            {
                ToggleControls(this.ConnectionData, true, Properties.OutputStrings.FillNewParameters);

                return;
            }

            connectionData.Save();

            FillParametersValues(doc, connectionData);

            UpdateStatus(this.ConnectionData, Properties.OutputStrings.ExecutingFetch);

            try
            {
                await ExecuteFetchAsync(doc, connectionData);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
                _iWriteToOutput.ActivateOutputWindow(null);
            }
        }

        private bool TryLoadFileText()
        {
            if (!File.Exists(this.FilePath))
            {
                txtBErrorText.Text = string.Format(Properties.MessageBoxStrings.FileNotExistsFormat1, FilePath);

                tbErrorText.IsEnabled = true;
                tbErrorText.Visibility = Visibility.Visible;

                tbErrorText.IsSelected = true;
                tbErrorText.Focus();

                this._selectedItem = tbErrorText;

                UpdateStatus(this.ConnectionData, Properties.OutputStrings.FileNotExists);

                return false;
            }

            var fileText = File.ReadAllText(this.FilePath);

            txtBFetchXml.Text = fileText;

            return true;
        }

        private Task ExecuteFetchAsync(XElement fetchXml, ConnectionData connectionData)
        {
            return Task.Run(async () => await ExecuteFetch(fetchXml, connectionData));
        }

        private async Task ExecuteFetch(XElement fetchXml, ConnectionData connectionData)
        {
            this._entityCollection = null;

            var service = await GetServiceAsync(connectionData);

            if (service == null)
            {
                return;
            }

            try
            {
                var request = new RetrieveMultipleRequest()
                {
                    Query = new FetchExpression(fetchXml.ToString()),
                };

                var response = (RetrieveMultipleResponse)service.Execute(request);

                this._entityCollection = response.EntityCollection;

                LoadData(connectionData, this._entityCollection, fetchXml);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.FetchQueryExecutedSuccessfullyFormat1, this._entityCollection.Entities.Count);
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(() =>
                {
                    StringBuilder text = new StringBuilder();

                    text.AppendLine(Properties.OutputStrings.FetchExecutionError);
                    text.AppendLine();

                    var description = DTEHelper.GetExceptionDescription(ex);

                    text.AppendLine(description);

                    txtBErrorText.Text = text.ToString();

                    tbErrorText.IsEnabled = true;
                    tbErrorText.Visibility = Visibility.Visible;

                    tbErrorText.IsSelected = true;
                    tbErrorText.Focus();

                    this._selectedItem = tbErrorText;

                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.FetchExecutionError);
                });

                return;
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
                            attrValue.Value = parameter.Value;
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
            tbFetchXml.IsSelected = true;
            tbFetchXml.Focus();

            txtBErrorText.Text = string.Empty;

            tbErrorText.Visibility = tbResults.Visibility = Visibility.Collapsed;
            tbErrorText.IsEnabled = tbResults.IsEnabled = false;

            dGrResults.Columns.Clear();
            dGrResults.Items.DetachFromSourceCollection();
            dGrResults.ItemsSource = null;

            this._selectedItem = tbFetchXml;

            UpdateStatus(this.ConnectionData, string.Empty);
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
                    this.ConnectionData?.OpenEntityInstanceInWeb(entity.LogicalName, entity.Id);
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
                , this.tSProgressBar
                , this.btnExecuteFetchXml
                , this.btnExecuteFetchXml2
                , this.stBtnExecuteFetchXml
                , this.dGrParameters
            );

            ToggleControl(IsControlsEnabled
                && _entityCollection != null
                && !string.IsNullOrEmpty(_entityCollection.EntityName)
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

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            this.ConnectionData?.OpenEntityMetadataInWeb(entity.LogicalName);
        }

        private void mIOpenInstanceInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            this.ConnectionData?.OpenEntityInstanceInWeb(entity.LogicalName, entity.Id);
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            this.ConnectionData?.OpenEntityInstanceListInWeb(entity.LogicalName);
        }

        private async void mIOpenEntityExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                if (service == null)
                {
                    return;
                }

                var commonConfig = CommonConfiguration.Get();

                Views.WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entity.LogicalName);
            }
        }

        private void mICopyEntityId_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            ClipboardHelper.SetText(entity.Id.ToString());
        }

        private void mICopyEntityName_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            ClipboardHelper.SetText(entity.LogicalName);
        }

        private void mICopyEntityUrl_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            var url = this.ConnectionData?.GetEntityInstanceUrl(entity.LogicalName, entity.Id);

            ClipboardHelper.SetText(url);
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

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                if (service == null)
                {
                    return;
                }

                var entityFull = service.RetrieveByQuery<Entity>(entity.LogicalName, entity.Id, new ColumnSet(true));

                var commonConfig = CommonConfiguration.Get();

                string fileName = EntityFileNameFormatter.GetEntityName(service.ConnectionData.Name, entityFull, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, entityFull, null, service.ConnectionData);

                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , entityFull.LogicalName
                    , filePath);

                _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
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
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            this.ConnectionData?.OpenEntityMetadataInWeb(entityReferenceView.LogicalName);
        }

        private void mIOpenEntityReferenceInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            this.ConnectionData?.OpenEntityInstanceInWeb(entityReferenceView.LogicalName, entityReferenceView.Id);
        }

        private void mIOpenEntityReferenceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            this.ConnectionData?.OpenEntityInstanceListInWeb(entityReferenceView.LogicalName);
        }

        private async void mIOpenEntityReferenceExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                if (service == null)
                {
                    return;
                }

                var commonConfig = CommonConfiguration.Get();

                Views.WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entityReferenceView.LogicalName);
            }
        }

        private void mICopyEntityReferenceId_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            ClipboardHelper.SetText(entityReferenceView.Id.ToString());
        }

        private void mICopyEntityReferenceEntityName_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            ClipboardHelper.SetText(entityReferenceView.LogicalName);
        }

        private void mICopyEntityReferenceName_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            ClipboardHelper.SetText(entityReferenceView.Name);
        }

        private void mICopyEntityReferenceUrl_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindNavigateUriFromHyperlink(e, out var navigateUri))
            {
                return;
            }

            ClipboardHelper.SetText(navigateUri.AbsoluteUri);
        }

        private async void mICreateEntityReferenceDescription_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                if (service == null)
                {
                    return;
                }

                var entityFull = service.RetrieveByQuery<Entity>(entityReferenceView.LogicalName, entityReferenceView.Id, new ColumnSet(true));

                var commonConfig = CommonConfiguration.Get();

                string fileName = EntityFileNameFormatter.GetEntityName(service.ConnectionData.Name, entityFull, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, entityFull, null, service.ConnectionData);

                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , entityFull.LogicalName
                    , filePath);

                _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
        }

        private bool TryFindEntityReferenceViewFromRow(RoutedEventArgs e, out EntityReferenceView entityReferenceView)
        {
            entityReferenceView = null;

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

            if (!(dataRowView[cell.Column.SortMemberPath] is EntityReferenceView value))
            {
                return false;
            }

            entityReferenceView = value;

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

            string jsCode = ContentComparerHelper.FormatToJavaScript(Entities.SavedQuery.Schema.Attributes.fetchxml, fileText);

            ClipboardHelper.SetText(jsCode);
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

            var service = await GetServiceAsync(this.ConnectionData);

            if (service == null)
            {
                return;
            }

            var repository = new WorkflowRepository(service);

            Func<string, Task<IEnumerable<Workflow>>> getter = (string filter) => repository.GetListAsync(
                entityName
                , (int)Workflow.Schema.OptionSets.category.Workflow_0
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
                , Properties.OutputStrings.ExecutingWorkflowFormat4
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

            var service = await GetServiceAsync(this.ConnectionData);

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
                , Properties.OutputStrings.AssigningEntitiesToUserCompletedFormat4
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

            var service = await GetServiceAsync(this.ConnectionData);

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
                , Properties.OutputStrings.AssigningEntitiesToTeamFormat4
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
                , Properties.OutputStrings.AssigningEntitiesToTeamCompletedFormat4
                , service.ConnectionData.Name
                , entityName
                , listIds.Count
                , team.Name
            );

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        #endregion Assign to Team

        #region Create new Entity

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

            var service = await GetServiceAsync(this.ConnectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, _entityCollection.EntityName, Guid.Empty);
        }

        #endregion Create new Entity

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

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                if (service == null)
                {
                    return;
                }

                var commonConfig = CommonConfiguration.Get();

                WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, entity.LogicalName, entity.Id);
            }
        }

        private async void mIChangeEntityReferenceInEditor_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                if (service == null)
                {
                    return;
                }

                var commonConfig = CommonConfiguration.Get();

                WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, entityReferenceView.LogicalName, entityReferenceView.Id);
            }
        }

        #endregion Edit Entity

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

            var service = await GetServiceAsync(this.ConnectionData);

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

            var service = await GetServiceAsync(this.ConnectionData);

            if (service == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            var form = new WindowStatusSelect(_iWriteToOutput, service, commonConfig, entityName);

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
                , Properties.OutputStrings.SettingEntitiesStateFormat7
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
                , Properties.OutputStrings.SettingEntitiesStateCompletedFormat7
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

            var connectionData = this.ConnectionData;

            if (connectionData != null)
            {
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

            string question = string.Format(Properties.MessageBoxStrings.DeleteEntityCountFormat2, entityName, listIds.Count);

            if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            var service = await GetServiceAsync(this.ConnectionData);

            if (service == null)
            {
                return;
            }

            string operationName = string.Format(Properties.OperationNames.DeletingEntitiesSetFormat3, service.ConnectionData.Name, entityName, listIds.Count);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DeletingEntitiesSetFormat3, service.ConnectionData.Name, entityName, listIds.Count);

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

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DeletingEntitiesSetCompletedFormat3, service.ConnectionData.Name, entityName, listIds.Count);

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

            if (!TryFindEntityReferenceViewFromRow(e, out var entity))
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

            if (!TryFindEntityReferenceViewFromRow(e, out var entity))
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
            var commonConfig = CommonConfiguration.Get();

            var service = await GetServiceAsync(this.ConnectionData);

            if (service == null)
            {
                return;
            }

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

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

            bool hasSolutionComponentEntityReference = TryFindEntityReferenceViewFromRow(e, out var entityReferenceView);
            if (hasSolutionComponentEntityReference)
            {
                hasSolutionComponentEntityReference = SolutionComponent.GetEntityComponentType(entityReferenceView?.LogicalName, out _);
            }

            WindowBase.ActivateControls(items, hasSolutionComponentEntity, "contMnAddToSolution", "contMnAddToSolutionLast");
            WindowBase.FillLastSolutionItems(this.ConnectionData, items, hasSolutionComponentEntity, AddToCrmSolutionEntityLast_Click, "contMnAddToSolutionLast");

            WindowBase.ActivateControls(items, hasSolutionComponentEntityReference, "contMnAddToSolutionEntityReference", "contMnAddToSolutionEntityReferenceLast");
            WindowBase.FillLastSolutionItems(this.ConnectionData, items, hasSolutionComponentEntityReference, AddToCrmSolutionEntityReferenceLast_Click, "contMnAddToSolutionEntityReferenceLast");
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
            WindowBase.FillLastSolutionItems(this.ConnectionData, items, hasSolutionComponentEntity, AddToCrmSolutionSelectedEntitiesLast_Click, "contMnAddToSolutionLast");
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
            WindowBase.FillLastSolutionItems(this.ConnectionData, items, hasSolutionComponentEntity, AddToCrmSolutionAllEntitiesLast_Click, "contMnAddToSolutionLast");
        }
    }
}