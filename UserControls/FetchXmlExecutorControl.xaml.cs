using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for FetchXmlExecutorControl.xaml
    /// </summary>
    public partial class FetchXmlExecutorControl : UserControl
    {
        private readonly object sysObjectConnections = new object();

        private ConnectionConfiguration _connectionConfig;

        private Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private bool _controlsEnabled = true;

        public event EventHandler<EventArgs> ConnectionChanged;

        private ContentEntityReferenceMultiValueConverter _contentConverter = new ContentEntityReferenceMultiValueConverter();
        private NavigateUriMultiValueConverter _navigateUriConverter = new NavigateUriMultiValueConverter();

        private void OnConnectionChanged()
        {
            this.ConnectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public string FilePath { get; private set; }

        public ConnectionData ConnectionData
        {
            get
            {
                return cmBCurrentConnection.SelectedItem as ConnectionData;
            }
        }

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

            ClearGridAndTextBox();

            this.dGrParameters.DataContext = cmBCurrentConnection;
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

            cmBCurrentConnection.ItemsSource = null;

            var settings = GetUserControlSettings();
            settings.DictDouble[paramColumnParametersWidth] = columnParameters.Width.Value;
            settings.DictDouble[paramColumnFetchTextWidth] = columnFetchText.Width.Value;
            settings.Save();

            _connectionConfig.Save();
        }

        public void SetSource(string filePath, ConnectionData connectionData)
        {
            this.FilePath = filePath;
            this._connectionConfig = connectionData.ConnectionConfiguration;

            var syncObject = new object();

            _syncCacheObjects.Add(connectionData.ConnectionId, syncObject);

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);
            BindingOperations.EnableCollectionSynchronization(connectionData.FetchXmlRequestParameterList, syncObject);

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = connectionData;
        }

        private void btnExecuteFetchXml_Click(object sender, RoutedEventArgs e)
        {
            this.Execute();
        }

        private void UpdateStatus(string msg)
        {
            this.statusBar.Dispatcher.Invoke(() =>
            {
                this.tSSLStatusMessage.Content = msg;
            });
        }


        public async Task Execute()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            ClearGridAndTextBox();

            if (!TryLoadFileText())
            {
                ToggleControls(true);
                return;
            }

            var connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData == null)
            {
                txtBErrorText.Text = "Connection is not selected.";

                tbErrorText.IsEnabled = true;
                tbErrorText.Visibility = Visibility.Visible;

                tbErrorText.IsSelected = true;
                tbErrorText.Focus();

                ToggleControls(true);

                UpdateStatus("Connection is not selected.");

                return;
            }

            var fileText = txtBFetchXml.Text.Trim();

            if (!ContentCoparerHelper.TryParseXml(fileText, out var exception, out var doc))
            {
                StringBuilder text = new StringBuilder();

                text.AppendLine("File text is not valid xml");
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

                ToggleControls(true);

                UpdateStatus("File text is not valid xml.");

                return;
            }

            txtBFetchXml.Text = doc.ToString();

            if (CheckParametersAndReturnHasNew(doc, connectionData))
            {
                UpdateStatus("Please, Fill New Parameters.");

                ToggleControls(true);

                return;
            }

            _connectionConfig.Save();

            FillParametersValues(doc, connectionData);

            UpdateStatus("Executing Fetch.....");

            await ExecuteFetchAsync(doc, connectionData);
        }

        private bool TryLoadFileText()
        {
            if (!File.Exists(this.FilePath))
            {
                txtBErrorText.Text = string.Format("File not exists: {0}", FilePath);

                tbErrorText.IsEnabled = true;
                tbErrorText.Visibility = Visibility.Visible;

                tbErrorText.IsSelected = true;
                tbErrorText.Focus();

                UpdateStatus("File not exists.");

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
            EntityCollection entityCollection = null;

            try
            {
                var service = await GetServiceAsync(connectionData);

                var request = new RetrieveMultipleRequest()
                {
                    Query = new FetchExpression(fetchXml.ToString()),
                };

                var response = (RetrieveMultipleResponse)service.Execute(request);

                entityCollection = response.EntityCollection;

                LoadData(connectionData, entityCollection, fetchXml);

                ToggleControls(true);

                UpdateStatus("Fetch Query executed successfully.");
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(() =>
                {
                    StringBuilder text = new StringBuilder();

                    text.AppendLine("Fetch Execution Error");
                    text.AppendLine();

                    var description = DTEHelper.GetExceptionDescription(ex);

                    text.AppendLine(description);

                    txtBErrorText.Text = text.ToString();

                    tbErrorText.IsEnabled = true;
                    tbErrorText.Visibility = Visibility.Visible;

                    tbErrorText.IsSelected = true;
                    tbErrorText.Focus();

                    ToggleControls(true);

                    UpdateStatus("Fetch Execution Error.");
                });

                return;
            }
        }

        private void LoadData(ConnectionData connectionData, EntityCollection entityCollection, XElement fetchXml)
        {
            var columnsInFetch = GetColumns(connectionData, fetchXml);

            Dictionary<string, string> columnMapping = null;

            var urlFormat = connectionData.GetEntityUrlFormat();

            var dataTable = EntityCollectionToDataTable(entityCollection, out columnMapping);

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

                    AddNewColumnInDataGrid(dataTable, attributeName, dataColumnName, urlFormat);
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

                    AddNewColumnInDataGrid(dataTable, attributeName, dataColumnName, urlFormat);
                }

                dGrResults.ItemsSource = dataTable.DefaultView;

                tbResults.IsEnabled = true;
                tbResults.Visibility = Visibility.Visible;

                tbResults.IsSelected = true;
                tbResults.Focus();
            });
        }

        private void AddNewColumnInDataGrid(DataTable dataTable, string attributeName, string dataColumnName, string urlFormat)
        {
            var dataColumn = dataTable.Columns[dataColumnName];

            if (dataColumn.DataType == typeof(EntityReference))
            {
                var columnDGT = new DataGridHyperlinkColumn()
                {
                    Header = attributeName.Replace("_", "__"),
                    Width = DataGridLength.Auto,

                    ContentBinding = new MultiBinding()
                    {
                        Mode = BindingMode.OneTime,

                        Converter = _contentConverter,

                        Bindings =
                        {
                            new Binding("[" + dataColumnName + "].Name"),
                            new Binding("[" + dataColumnName + "].LogicalName"),
                            new Binding("[" + dataColumnName + "].Id"),
                        },
                    },

                    Binding = new MultiBinding()
                    {
                        Mode = BindingMode.OneTime,

                        Converter = _navigateUriConverter,
                        ConverterParameter = urlFormat,

                        Bindings =
                        {
                            new Binding("[" + dataColumnName + "].LogicalName"),
                            new Binding("[" + dataColumnName + "].Id"),
                        },
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

        private class ContentEntityReferenceMultiValueConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (values == null || values.Length != 3)
                {
                    return Binding.DoNothing;
                }

                var name = values[0] as string;

                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }

                if (values[1] is string entityName && values[2] is Guid id)
                {
                    return string.Format("{0} - {1}", entityName, id);
                }

                return null;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
            {
                return new object[] { Binding.DoNothing };
            }
        }

        private class NavigateUriMultiValueConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (values == null || values.Length != 2)
                {
                    return Binding.DoNothing;
                }

                if (values[0] is string entityName && values[1] is Guid id && parameter is string format)
                {
                    var url = string.Format(format, entityName, id);

                    if (Uri.TryCreate(url, UriKind.Absolute, out var result))
                    {
                        return result;
                    }
                }

                return null;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
            {
                return new object[] { Binding.DoNothing };
            }
        }

        private List<string> GetColumns(ConnectionData connectionData, XElement fetchXml)
        {
            List<string> result = new List<string>();

            var linkElements = fetchXml.DescendantsAndSelf().Where(n => IsLinkElement(n)).ToList();

            {
                var entityElements = fetchXml.DescendantsAndSelf().Where(IsAttributeOrAllElement).ToList();

                foreach (var attributeNode in entityElements)
                {
                    if (IsAttributeElement(attributeNode))
                    {
                        var attrAlias = attributeNode.Attribute("alias");

                        if (attrAlias != null && !string.IsNullOrEmpty(attrAlias.Value))
                        {
                            result.Add(attrAlias.Value);
                        }
                        else
                        {
                            string name = attributeNode.Attribute("name").Value;

                            var parentElement = attributeNode.Ancestors().FirstOrDefault(IsEntityOrLinkElement);

                            if (parentElement != null)
                            {
                                var parentAlias = parentElement.Attribute("alias");

                                if (parentAlias != null && !string.IsNullOrEmpty(parentAlias.Value))
                                {
                                    name = parentAlias.Value + "." + name;
                                }
                                else if (IsLinkElement(parentElement))
                                {
                                    if (linkElements.Contains(parentElement))
                                    {
                                        var index = linkElements.IndexOf(parentElement) + 1;

                                        name = parentElement.Attribute("name")?.Value + index.ToString() + "." + name;
                                    }
                                }
                            }

                            result.Add(name);
                        }
                    }
                    else if (IsAllAttributesElement(attributeNode))
                    {
                        var parentElement = attributeNode.Ancestors().FirstOrDefault(IsEntityOrLinkElement);

                        if (parentElement != null)
                        {
                            var parentAlias = parentElement.Attribute("alias");
                            var parentEntityName = parentElement.Attribute("name");

                            string parentPrefixName = string.Empty;

                            if (parentAlias != null && !string.IsNullOrEmpty(parentAlias.Value))
                            {
                                parentPrefixName = parentAlias.Value + ".";
                            }
                            else if (IsLinkElement(parentElement))
                            {
                                if (linkElements.Contains(parentElement))
                                {
                                    var index = linkElements.IndexOf(parentElement) + 1;

                                    parentPrefixName = parentElement.Attribute("name")?.Value + index.ToString();
                                }
                            }

                            var intellisenseData = connectionData.IntellisenseData;

                            if (intellisenseData != null
                                && intellisenseData.Entities != null
                                && intellisenseData.Entities.ContainsKey(parentEntityName.Value)
                                && intellisenseData.Entities[parentEntityName.Value].Attributes != null
                                )
                            {
                                foreach (var attrName in intellisenseData.Entities[parentEntityName.Value].Attributes.Keys)
                                {
                                    string name = parentPrefixName + attrName;

                                    result.Add(name);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsEntityOrLinkElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "entity", StringComparison.OrdinalIgnoreCase)
                || string.Equals(element.Name.LocalName, "link-entity", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsLinkElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "link-entity", StringComparison.OrdinalIgnoreCase);
        }

        private const string _columnOriginalEntity = "columnOriginalEntity______";

        private static DataTable EntityCollectionToDataTable(EntityCollection entityCollection, out Dictionary<string, string> columnMapping)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(_columnOriginalEntity, typeof(Entity));

            columnMapping = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var entity in entityCollection.Entities)
            {
                DataRow row = dataTable.NewRow();

                row[_columnOriginalEntity] = entity;

                foreach (string attributeName in entity.Attributes.Keys)
                {
                    var value = entity.Attributes[attributeName];

                    if (value == null || EntityDescriptionHandler.GetUnderlyingValue(value) == null)
                    {
                        continue;
                    }

                    string columnName = string.Format("{0}___{1}", entity.LogicalName, attributeName.Replace(".", "_"));

                    if (value is AliasedValue aliasedValue)
                    {
                        columnName = string.Format("{0}___{1}___{2}", attributeName.Replace(".", "_"), aliasedValue.EntityLogicalName, aliasedValue.AttributeLogicalName);
                    }

                    value = EntityDescriptionHandler.GetUnderlyingValue(value);

                    if (value is Money money)
                    {
                        value = money.Value;
                    }

                    if (value is DateTime dateTime)
                    {
                        value = dateTime.ToLocalTime();
                    }

                    if (value is OptionSetValue optionSetValue)
                    {
                        value = (entity.FormattedValues != null && entity.FormattedValues.ContainsKey(attributeName) ? string.Format("{0} - ", entity.FormattedValues[attributeName]) : string.Empty) + optionSetValue.Value.ToString();
                    }

                    if (dataTable.Columns.IndexOf(columnName) == -1)
                    {
                        if (!columnMapping.ContainsKey(attributeName))
                        {
                            columnMapping.Add(attributeName, columnName);
                        }

                        dataTable.Columns.Add(columnName, value.GetType());
                    }

                    row[columnName] = value;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private async Task<IOrganizationServiceExtented> GetServiceAsync(ConnectionData connectionData)
        {
            if (connectionData != null)
            {
                if (!_connectionCache.ContainsKey(connectionData.ConnectionId))
                {
                    DTEHelper.Singleton?.WriteToOutput("Connection to CRM.");
                    DTEHelper.Singleton?.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    DTEHelper.Singleton?.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
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

        private static bool IsAttributeOrAllElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "attribute", StringComparison.OrdinalIgnoreCase)
                || string.Equals(element.Name.LocalName, "all-attributes", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsAttributeElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "attribute", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsAllAttributesElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "all-attributes", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsConditonElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "condition", StringComparison.OrdinalIgnoreCase);
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

            UpdateStatus(string.Empty);
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                ClearGridAndTextBox();

                var connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                if (connectionData != null)
                {
                    if (!_syncCacheObjects.ContainsKey(connectionData.ConnectionId))
                    {
                        var syncObject = new object();

                        _syncCacheObjects.Add(connectionData.ConnectionId, syncObject);

                        BindingOperations.EnableCollectionSynchronization(connectionData.FetchXmlRequestParameterList, syncObject);
                    }
                }
            });

            OnConnectionChanged();

            if (!_controlsEnabled)
            {
                return;
            }
        }

        private void dGrResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as DataRowView;

                if (item != null
                    && item[_columnOriginalEntity] != null
                    && item[_columnOriginalEntity] is Entity entity
                    )
                {
                    this.ConnectionData?.OpenEntityInWeb(entity.LogicalName, entity.Id);
                }
            }
        }

        private void ToggleControls(bool enabled)
        {
            this._controlsEnabled = enabled;

            ToggleControl(this.toolStrip, enabled);

            ToggleControl(this.dGrParameters, enabled);

            ToggleControl(cmBCurrentConnection, enabled);

            ToggleProgressBar(enabled);
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

        private async void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            var service = await GetServiceAsync(this.ConnectionData);

            this.ConnectionData.OpenEntityMetadataInWeb(entity.LogicalName);
        }

        private async void mIOpenEntityMetadataWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            var service = await GetServiceAsync(this.ConnectionData);

            var commonConfig = CommonConfiguration.Get();

            if (DTEHelper.Singleton != null)
            {
                Views.WindowHelper.OpenEntityMetadataWindow(DTEHelper.Singleton, service, commonConfig, entity.LogicalName, null, null);
            }
        }

        private void mICopyEntityId_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            Clipboard.SetText(entity.Id.ToString());
        }

        private void mICopyEntityName_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            Clipboard.SetText(entity.LogicalName);
        }

        private void mICopyEntityUrl_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            var url = this.ConnectionData.GetEntityUrl(entity.LogicalName, entity.Id);

            Clipboard.SetText(url);
        }

        private bool TryFindEntityFromDataRowView(RoutedEventArgs e, out Entity entity)
        {
            entity = null;

            DataRowView item = null;

            if (e.OriginalSource is FrameworkElement framework
                && framework.DataContext is DataRowView dataRowView
                )
            {
                item = dataRowView;
            }
            else if (e.OriginalSource is FrameworkContentElement frameworkContent
                && frameworkContent.DataContext is DataRowView dataRowView2
                )
            {
                item = dataRowView2;
            }

            if (item != null
                && item[_columnOriginalEntity] != null
                && item[_columnOriginalEntity] is Entity result
                )
            {
                entity = result;

                return true;
            }

            return false;
        }

        private async void mIOpenEntityReferenceCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityNameFromHyperlink(e, out string entityName))
            {
                return;
            }

            var service = await GetServiceAsync(this.ConnectionData);

            this.ConnectionData.OpenEntityMetadataInWeb(entityName);
        }

        private async void mIOpenEntityReferenceMetadataWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityNameFromHyperlink(e, out string entityName))
            {
                return;
            }

            var service = await GetServiceAsync(this.ConnectionData);

            var commonConfig = CommonConfiguration.Get();

            Views.WindowHelper.OpenEntityMetadataWindow(DTEHelper.Singleton, service, commonConfig, entityName, null, null);
        }

        private void mICopyEntityReferenceId_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityIdFromHyperlink(e, out var entityId))
            {
                return;
            }

            Clipboard.SetText(entityId.ToString());
        }

        private void mICopyEntityReferenceName_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityNameFromHyperlink(e, out string entityName))
            {
                return;
            }

            Clipboard.SetText(entityName);
        }

        private void mICopyEntityReferenceUrl_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindNavigateUriFromHyperlink(e, out var navigateUri))
            {
                return;
            }

            Clipboard.SetText(navigateUri.AbsoluteUri);
        }

        private bool TryFindEntityNameFromHyperlink(RoutedEventArgs e, out string entityName)
        {
            entityName = null;

            if (!TryFindNavigateUriFromHyperlink(e, out var navigateUri))
            {
                return false;
            }

            var query = HttpUtility.ParseQueryString(navigateUri.Query);

            if (!query.AllKeys.Contains("etn", StringComparer.InvariantCultureIgnoreCase))
            {
                return false;
            }

            entityName = query.Get("etn");

            if (string.IsNullOrEmpty(entityName))
            {
                return false;
            }

            return true;
        }

        private bool TryFindEntityIdFromHyperlink(RoutedEventArgs e, out Guid entityId)
        {
            entityId = Guid.Empty;

            if (!TryFindNavigateUriFromHyperlink(e, out var navigateUri))
            {
                return false;
            }

            var query = HttpUtility.ParseQueryString(navigateUri.Query);

            if (!query.AllKeys.Contains("id", StringComparer.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var entityIdString = query.Get("id");

            if (string.IsNullOrEmpty(entityIdString))
            {
                return false;
            }

            if (!Guid.TryParse(entityIdString, out entityId))
            {
                return false;
            }

            return true;
        }

        private bool TryFindNavigateUriFromHyperlink(RoutedEventArgs e, out Uri navigateUri)
        {
            navigateUri = null;

            MenuItem menuItem = e.OriginalSource as MenuItem;

            if (menuItem == null)
            {
                return false;
            }

            ContextMenu contextMenu = ItemsControl.ItemsControlFromItemContainer(menuItem) as ContextMenu;

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

            var jsCode = ContentCoparerHelper.FormatToJavaScript("fetchXml", fileText);

            Clipboard.SetText(jsCode);
        }
    }
}