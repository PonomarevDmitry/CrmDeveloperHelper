using Microsoft.Xrm.Sdk.Query;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowTraceReader : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly object sysObjectTraceList = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private bool _controlsEnabled = true;

        private ObservableCollection<TraceRecord> _itemsSource;

        private List<TraceRecord> _loadedRecords;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private Dictionary<Guid, Dictionary<Guid, SystemUser>> _systemUserCache = new Dictionary<Guid, Dictionary<Guid, SystemUser>>();

        private Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        private int _init = 0;

        public WindowTraceReader(
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

            var syncObject = new object();
            _syncCacheObjects.Add(service.ConnectionData.ConnectionId, syncObject);

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.TraceReaderLastFilters, syncObject);
            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.TraceReaderLastFolders, syncObject);
            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.TraceReaderSelectedFolders, syncObject);

            InitializeComponent();

            LoadFromConfig();

            dPFileDate.SelectedDate = DateTime.Today.AddDays(-1);

            this._itemsSource = new ObservableCollection<TraceRecord>();

            BindingOperations.EnableCollectionSynchronization(_itemsSource, sysObjectTraceList);

            this.lstVwTraceRecords.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            cmBFilter.Focus();

            if (service != null)
            {
                OpenFilesInFolders();
            }
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            cmBFilter.DataContext = cmBCurrentConnection;
            cmBFolder.DataContext = cmBCurrentConnection;
            lstVwFolders.DataContext = cmBCurrentConnection;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);
            BindingOperations.ClearAllBindings(cmBFilter);
            BindingOperations.ClearAllBindings(cmBFolder);
            BindingOperations.ClearAllBindings(lstVwFolders);

            cmBFilter.Items.DetachFromSourceCollection();
            cmBCurrentConnection.Items.DetachFromSourceCollection();
            cmBFolder.Items.DetachFromSourceCollection();
            lstVwFolders.Items.DetachFromSourceCollection();

            cmBFilter.ItemsSource = null;
            cmBCurrentConnection.ItemsSource = null;
            cmBFolder.ItemsSource = null;
            lstVwFolders.ItemsSource = null;

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
                    _iWriteToOutput.WriteToOutput("Connection to CRM.");
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task LoadTraceRecords(IEnumerable<string> files)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Loading trace files...");

            this._itemsSource.Clear();

            var taskFiles = TraceRecord.ParseFilesAsync(files);

            var service = await GetService();

            if (service != null)
            {
                if (!_systemUserCache.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    var repository = new SystemUserRepository(service);

                    var list = await repository.GetListAsync(new ColumnSet(SystemUser.Schema.EntityPrimaryIdAttribute, SystemUser.Schema.Attributes.fullname));

                    _systemUserCache.Add(service.ConnectionData.ConnectionId, list.ToDictionary(e => e.Id));
                }
            }

            _loadedRecords = await taskFiles;

            ToggleControls(true);

            await FilterExistingTraceRecords();
        }

        private async Task FilterExistingTraceRecords()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Filtering trace records...");

            this._itemsSource.Clear();

            Guid connectionId = Guid.Empty;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                var connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                if (connectionData != null)
                {
                    connectionId = connectionData.ConnectionId;
                }
            });

            IEnumerable<TraceRecord> list = Enumerable.Empty<TraceRecord>();

            Dictionary<Guid, SystemUser> dictUsers = null;

            try
            {
                if (this._loadedRecords != null)
                {
                    list = this._loadedRecords;
                }

                if (_systemUserCache.ContainsKey(connectionId))
                {
                    dictUsers = _systemUserCache[connectionId];
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            this._iWriteToOutput.WriteToOutput("Found {0} trace records.", list.Count());

            string textName = string.Empty;

            DateTime? date = null;

            this.Dispatcher.Invoke(() =>
            {
                textName = cmBFilter.Text?.Trim().ToLower();

                var connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                if (connectionData != null)
                {
                    connectionData.AddTraceFilter(textName);
                }

                date = dPFilterDate.SelectedDate;
            });

            list = await FilterTraceRecordsAsync(list, textName, date);

            LoadTraceRecords(list, dictUsers);

            UpdateStatus("{0} trace records loaded.", list.Count());

            ToggleControls(true);
        }

        private Task<IEnumerable<TraceRecord>> FilterTraceRecordsAsync(IEnumerable<TraceRecord> list, string textName, DateTime? date)
        {
            if (string.IsNullOrEmpty(textName) && !date.HasValue)
            {
                return Task.FromResult(list);
            }

            return Task.Run(() => FilterTraceRecords(list, textName, date));
        }

        private IEnumerable<TraceRecord> FilterTraceRecords(IEnumerable<TraceRecord> list, string textName, DateTime? date)
        {
            var result = new List<TraceRecord>();

            var hash = new HashSet<Guid>();

            if (date.HasValue)
            {
                list = list.Where(e => e.Date >= date.Value);
            }

            if (!string.IsNullOrEmpty(textName))
            {
                foreach (var item in list)
                {
                    if (item.Description.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        result.Add(item);

                        if (item.RequestId != Guid.Empty)
                        {
                            hash.Add(item.RequestId);
                        }
                    }
                    else if (hash.Contains(item.RequestId))
                    {
                        result.Add(item);
                    }
                }
            }
            else
            {
                result.AddRange(list);
            }

            return result;
        }

        private void LoadTraceRecords(IEnumerable<TraceRecord> results, Dictionary<Guid, SystemUser> dictUsers)
        {
            this.lstVwTraceRecords.Dispatcher.Invoke(() =>
            {
                foreach (var item in results.OrderByDescending(ent => ent.Date).ThenByDescending(ent => ent.RecordNumber))
                {
                    if (item.UserId.HasValue)
                    {
                        if (item.UserId.Value == Guid.Empty)
                        {
                            item.User = SystemUser.EmptySystemUser;
                        }
                        else if (dictUsers != null && dictUsers.ContainsKey(item.UserId.Value))
                        {
                            item.User = dictUsers[item.UserId.Value];
                        }
                        else
                        {
                            item.User = new SystemUser() { Id = item.UserId.Value };
                        }
                    }

                    this._itemsSource.Add(item);
                }

                if (this._itemsSource.Any())
                {
                    tabControl.SelectedItem = tbITraces;
                }

                if (this.lstVwTraceRecords.Items.Count == 1)
                {
                    this.lstVwTraceRecords.SelectedItem = this.lstVwTraceRecords.Items[0];
                }
            });
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(bool enabled)
        {
            this._controlsEnabled = enabled;

            ToggleControl(cmBCurrentConnection, enabled);

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
            this.lstVwTraceRecords.Dispatcher.Invoke(() =>
            {
                try
                {
                    mIRemoveFolder.IsEnabled = this._controlsEnabled && lstVwFolders.SelectedItems.Count > 0;
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            });
        }

        private void cmBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterExistingTraceRecords();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                FilterExistingTraceRecords();
            }

            base.OnKeyDown(e);
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._loadedRecords.Clear();
                this._itemsSource.Clear();
            });

            if (!_controlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                if (connectionData != null)
                {
                    if (!_syncCacheObjects.ContainsKey(connectionData.ConnectionId))
                    {
                        var syncObject = new object();

                        _syncCacheObjects.Add(connectionData.ConnectionId, syncObject);

                        BindingOperations.EnableCollectionSynchronization(connectionData.TraceReaderLastFilters, syncObject);
                        BindingOperations.EnableCollectionSynchronization(connectionData.TraceReaderLastFolders, syncObject);
                        BindingOperations.EnableCollectionSynchronization(connectionData.TraceReaderSelectedFolders, syncObject);
                    }
                }
            });

            if (connectionData != null)
            {
                FilterExistingTraceRecords();
            }
        }

        private async void mIOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = @"CRM Trace files (*.log)|*.log",
                Multiselect = true,
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                await LoadTraceRecords(dialog.FileNames);
            }
        }

        private void mIOpenFilesInFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFilesInFolders();
        }

        private async void OpenFilesInFolders()
        {
            var folders = lstVwFolders.Items.OfType<string>().ToList();

            HashSet<FileInfo> hash = new HashSet<FileInfo>();

            foreach (var item in folders)
            {
                var di = new DirectoryInfo(item);

                var files = di.GetFiles("*.log");

                foreach (var file in files)
                {
                    hash.Add(file);
                }
            }

            var filter = hash.AsEnumerable();

            DateTime? date = dPFileDate.SelectedDate;

            if (date.HasValue)
            {
                filter = filter.Where(e => e.LastWriteTime >= date.Value);
            }

            LoadTraceRecords(filter.Select(f => f.FullName));
        }

        private void lstVwTraceRecords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as TraceRecord;

                if (item != null)
                {
                    OpenTraceRecordDescription(item);
                }
            }
        }

        private void OpenTraceRecordDescription(TraceRecord item)
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                string windowTitle = string.Format("TraceRecord: {0:yyyy.MM.dd HH:mm:ss.fff}", item.Date);

                try
                {
                    var form = new WindowTextField(windowTitle, "Description", item.Description, true);

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        private void mIAddFolderInList_Click(object sender, RoutedEventArgs e)
        {
            AddNewFolderInList();
        }

        private void AddNewFolderInList()
        {
            string folder = cmBFolder.Text?.Trim();

            if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
            {
                return;
            }

            var connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                if (lstVwFolders.ItemsSource != null
                    && lstVwFolders.ItemsSource is ObservableCollection<string> coll
                    )
                {
                    coll.Add(folder);
                }

                connectionData.AddTraceLastFolder(folder);
            }
        }

        private void mIRemoveFolderFromList_Click(object sender, RoutedEventArgs e)
        {
            var list = lstVwFolders.SelectedItems.OfType<string>().ToList();

            if (lstVwFolders.ItemsSource != null
                && lstVwFolders.ItemsSource is ObservableCollection<string> coll
                )
            {
                foreach (var item in list)
                {
                    coll.Remove(item);
                }
            }
        }

        private void lstVwFolders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void cmBFolder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddNewFolderInList();
            }
        }
    }
}