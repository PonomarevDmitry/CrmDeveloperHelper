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
    public partial class WindowTraceReader : WindowWithConnectionList
    {
        private readonly object sysObjectTraceList = new object();

        private readonly ObservableCollection<TraceRecord> _itemsSource;

        private List<TraceRecord> _loadedRecords;

        private readonly Dictionary<Guid, Dictionary<Guid, SystemUser>> _systemUserCache = new Dictionary<Guid, Dictionary<Guid, SystemUser>>();

        private readonly Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        public WindowTraceReader(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            BindCollections(service.ConnectionData);

            InitializeComponent();

            LoadFromConfig();

            dPFileDate.SelectedDate = DateTime.Today.AddDays(-1);

            this._itemsSource = new ObservableCollection<TraceRecord>();

            BindingOperations.EnableCollectionSynchronization(_itemsSource, sysObjectTraceList);

            this.lstVwTraceRecords.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this.DecreaseInit();

            FocusOnComboBoxTextBox(cmBFilter);

            if (service != null)
            {
                OpenFilesInFoldersAsync();
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
            base.OnClosed(e);

            (cmBCurrentConnection.SelectedItem as ConnectionData)?.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);
            BindingOperations.ClearAllBindings(cmBFilter);
            BindingOperations.ClearAllBindings(cmBFolder);
            BindingOperations.ClearAllBindings(lstVwFolders);

            cmBFilter.Items.DetachFromSourceCollection();
            cmBCurrentConnection.Items.DetachFromSourceCollection();
            cmBFolder.Items.DetachFromSourceCollection();
            lstVwFolders.Items.DetachFromSourceCollection();

            cmBFilter.DataContext = null;
            cmBCurrentConnection.DataContext = null;
            cmBFolder.DataContext = null;

            cmBFilter.ItemsSource = null;
            cmBCurrentConnection.ItemsSource = null;
            cmBFolder.ItemsSource = null;
            lstVwFolders.ItemsSource = null;
        }

        private ConnectionData GetSelectedConnection()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private Task<IOrganizationServiceExtented> GetService()
        {
            return GetOrganizationService(GetSelectedConnection());
        }

        private async Task LoadTraceRecordsAsync(IEnumerable<string> files)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingTraceFiles);

            this._itemsSource.Clear();

            this.tabControl.Dispatcher.Invoke(() =>
            {
                tabControl.SelectedItem = tbITraces;
            });

            var taskFiles = TraceRecord.ParseFilesAsync(files);

            if (service != null)
            {
                if (!_systemUserCache.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    var repository = new SystemUserRepository(service);

                    var list = await repository.GetListAsync(null, new ColumnSet(SystemUser.Schema.EntityPrimaryIdAttribute, SystemUser.Schema.Attributes.fullname));

                    _systemUserCache.Add(service.ConnectionData.ConnectionId, list.ToDictionary(e => e.Id));
                }
            }

            _loadedRecords = await taskFiles;

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingTraceFilesCompletedFormat1, _loadedRecords.Count());

            await FilterExistingTraceRecords();
        }

        private async Task FilterExistingTraceRecords()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this._itemsSource.Clear();

            ConnectionData connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.FilteringTraceFiles);

            IEnumerable<TraceRecord> list = Enumerable.Empty<TraceRecord>();

            Dictionary<Guid, SystemUser> dictUsers = null;

            try
            {
                if (this._loadedRecords != null)
                {
                    list = this._loadedRecords;
                }

                if (connectionData != null && _systemUserCache.ContainsKey(connectionData.ConnectionId))
                {
                    dictUsers = _systemUserCache[connectionData.ConnectionId];
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            string textName = string.Empty;
            Guid? requestId = null;
            Guid? activityId = null;
            int? threadNumber = null;

            this.Dispatcher.Invoke(() =>
            {
                textName = cmBFilter.Text;

                connectionData?.AddTraceFilter(textName);

                cmBFilter.Text = textName;

                {
                    if (Guid.TryParse(txtBRequestId.Text, out var tempGuid)
                        && tempGuid != Guid.Empty
                    )
                    {
                        requestId = tempGuid;
                    }
                }

                {
                    if (Guid.TryParse(txtBActivityId.Text, out var tempGuid)
                        && tempGuid != Guid.Empty
                    )
                    {
                        activityId = tempGuid;
                    }
                }

                if (int.TryParse(txtBThread.Text, out var tempInt))
                {
                    threadNumber = tempInt;
                }
            });

            list = await FilterTraceRecordsAsync(list, textName, requestId, activityId, threadNumber);

            LoadTraceRecords(list, dictUsers);

            ToggleControls(connectionData, true, Properties.OutputStrings.FilteringTraceFilesCompletedFormat1, list.Count());
        }

        private Task<IEnumerable<TraceRecord>> FilterTraceRecordsAsync(IEnumerable<TraceRecord> list, string textName, Guid? requestId, Guid? activityId, int? threadNumber)
        {
            if (string.IsNullOrEmpty(textName)
                && !requestId.HasValue
                && !activityId.HasValue
                && !threadNumber.HasValue
            )
            {
                return Task.FromResult(list);
            }

            return Task.Run(() => FilterTraceRecords(list, textName, requestId, activityId, threadNumber));
        }

        private IEnumerable<TraceRecord> FilterTraceRecords(IEnumerable<TraceRecord> list, string textName, Guid? requestId, Guid? activityId, int? threadNumber)
        {
            if (requestId.HasValue)
            {
                list = list.Where(t => t.RequestId.HasValue && t.RequestId.Value == requestId.Value);
            }

            if (activityId.HasValue)
            {
                list = list.Where(t => t.ActivityId.HasValue && t.ActivityId.Value == activityId.Value);
            }

            if (threadNumber.HasValue)
            {
                list = list.Where(t => t.ThreadNumber == threadNumber.Value);
            }

            if (!string.IsNullOrEmpty(textName))
            {
                var hash = new HashSet<Guid>();

                foreach (var item in list)
                {
                    if (item.Description.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        if (item.RequestId.HasValue && item.RequestId != Guid.Empty)
                        {
                            hash.Add(item.RequestId.Value);
                        }
                    }
                }

                list = list.Where(t =>
                    (t.RequestId.HasValue && hash.Contains(t.RequestId.Value))
                    || t.Description.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                );
            }

            return list.ToList();
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
            });
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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, btnSetCurrentConnection, cmBCurrentConnection, this.miOpenFolder, this.miOpenFilesInFolders);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwTraceRecords.Dispatcher.Invoke(() =>
            {
                try
                {
                    mIRemoveFolder.IsEnabled = this.IsControlsEnabled && lstVwFolders.SelectedItems.Count > 0;
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(GetSelectedConnection(), ex);
                }
            });
        }

        private void cmBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (_loadedRecords.Count == 0)
                {
                    OpenFilesInFoldersAsync();
                }
                else
                {
                    FilterExistingTraceRecords();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            OpenFilesInFoldersAsync();
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._loadedRecords?.Clear();
                this._itemsSource?.Clear();
            });

            if (!this.IsControlsEnabled)
            {
                return;
            }

            foreach (var removed in e.RemovedItems.OfType<ConnectionData>())
            {
                removed.Save();
            }

            ConnectionData connectionData = GetSelectedConnection();

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                BindCollections(connectionData);
            });

            if (connectionData != null)
            {
                OpenFilesInFoldersAsync();
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

                BindingOperations.EnableCollectionSynchronization(connectionData.TraceReaderLastFilters, _syncCacheObjects[connectionData.ConnectionId]);
                BindingOperations.EnableCollectionSynchronization(connectionData.TraceReaderLastFolders, _syncCacheObjects[connectionData.ConnectionId]);
                BindingOperations.EnableCollectionSynchronization(connectionData.TraceReaderSelectedFolders, _syncCacheObjects[connectionData.ConnectionId]);
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
                await LoadTraceRecordsAsync(dialog.FileNames);
            }
        }

        private async void mIOpenFilesInFolder_Click(object sender, RoutedEventArgs e)
        {
            await OpenFilesInFoldersAsync();
        }

        private async Task OpenFilesInFoldersAsync()
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

            await LoadTraceRecordsAsync(filter.Select(f => f.FullName));
        }

        private void lstVwTraceRecords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                TraceRecord item = GetItemFromRoutedDataContext<TraceRecord>(e);

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
                try
                {
                    var form = new WindowTraceRecord();

                    form.SetTraceRecordInformation(item);
                    form.NextClicked += Form_NextClicked;
                    form.PreviousClicked += Form_PreviousClicked;

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(GetSelectedConnection(), ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        private void Form_PreviousClicked(object sender, EventArgs e)
        {
            if (!(sender is WindowTraceRecord form))
            {
                return;
            }

            if (form.TraceRecord == null)
            {
                return;
            }

            TraceRecord record = null;

            this.Dispatcher.Invoke(() =>
            {
                var index = this.lstVwTraceRecords.Items.IndexOf(form.TraceRecord);

                if (index != -1)
                {
                    index++;

                    if (index < this.lstVwTraceRecords.Items.Count)
                    {
                        record = this.lstVwTraceRecords.Items.GetItemAt(index) as TraceRecord;

                        if (record != null && this.lstVwTraceRecords.CurrentColumn != null)
                        {
                            this.lstVwTraceRecords.SelectedCells.Clear();
                            this.lstVwTraceRecords.SelectedCells.Add(new DataGridCellInfo(record, this.lstVwTraceRecords.CurrentColumn));
                        }
                    }
                }
            });

            if (record != null)
            {
                form.SetTraceRecordInformation(record);
            }
        }

        private void Form_NextClicked(object sender, EventArgs e)
        {
            if (!(sender is WindowTraceRecord form))
            {
                return;
            }

            if (form.TraceRecord == null)
            {
                return;
            }

            TraceRecord record = null;

            this.Dispatcher.Invoke(() =>
            {
                var index = this.lstVwTraceRecords.Items.IndexOf(form.TraceRecord);

                if (index != -1)
                {
                    index--;

                    if (index >= 0)
                    {
                        record = this.lstVwTraceRecords.Items.GetItemAt(index) as TraceRecord;

                        if (record != null && this.lstVwTraceRecords.CurrentColumn != null)
                        {
                            this.lstVwTraceRecords.SelectedCells.Clear();
                            this.lstVwTraceRecords.SelectedCells.Add(new DataGridCellInfo(record, this.lstVwTraceRecords.CurrentColumn));
                        }
                    }
                }
            });

            if (record != null)
            {
                form.SetTraceRecordInformation(record);
            }
        }

        private void btnAddFolderInList_Click(object sender, RoutedEventArgs e)
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

            ConnectionData connectionData = GetSelectedConnection();

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

        private TraceRecord GetSelectedTraceRecord()
        {
            TraceRecord result = null;

            if (this.lstVwTraceRecords.SelectedCells.Count == 1
                && this.lstVwTraceRecords.SelectedCells[0] != null
                && this.lstVwTraceRecords.SelectedCells[0].Item is TraceRecord
                )
            {
                result = this.lstVwTraceRecords.SelectedCells[0].Item as TraceRecord;
            }

            return result;
        }

        private void mIOpenTraceFile_Click(object sender, RoutedEventArgs e)
        {
            var record = GetSelectedTraceRecord();

            if (record == null || record.TraceFile == null)
            {
                return;
            }

            if (File.Exists(record.TraceFile.FilePath))
            {
                this._iWriteToOutput.PerformAction(null, record.TraceFile.FilePath);
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }
    }
}