using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class ConnectionConfiguration
    {
        private static readonly object _syncObject = new object();

        private static ConnectionConfiguration _singleton;

        private FileSystemWatcher _watcher = null;

        /// <summary>
        /// Путь к файлу конфигурации
        /// </summary>
        private string Path { get; set; }

        private VSSolutionConfiguration _currentSolutionConfiguration;

        /// <summary>
        /// Текущее подключение к CRM
        /// </summary>
        public ConnectionData CurrentConnectionData
        {
            get
            {
                ObjectCache cache = MemoryCache.Default;
                const string cacheName = nameof(CurrentConnectionData);

                ConnectionData result = null;

                if (cache.Contains(cacheName))
                {
                    result = cache.Get(cacheName) as ConnectionData;
                }
                else
                {
                    result = GetCurrentConnectionDataInternal();

                    if (result != null)
                    {
                        cache.Set(cacheName, result, new CacheItemPolicy()
                        {
                            AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheItemSpan),
                        });
                    }
                }

                return result;
            }
        }

        private ConnectionData GetCurrentConnectionDataInternal()
        {
            if (this._currentSolutionConfiguration != null && this._currentSolutionConfiguration.SelectedConnectionId.HasValue)
            {
                foreach (var item in this.Connections)
                {
                    item.IsCurrentConnection = false;
                }

                var connection = this.Connections.FirstOrDefault(c => c.ConnectionId == this._currentSolutionConfiguration.SelectedConnectionId.Value);

                if (connection == null)
                {
                    this._currentSolutionConfiguration.SelectedConnectionId = null;
                }
                else
                {
                    connection.IsCurrentConnection = true;
                }

                return connection;
            }

            return null;
        }

        [DataMember]
        public List<Guid> ConnectionsGuids { get; private set; }

        [DataMember]
        public List<Guid> ArchiveConnectionsGuids { get; private set; }

        [DataMember]
        public ObservableCollection<ConnectionUserData> Users { get; private set; }

        [DataMember]
        public VSSolutionConfiguration VSSolutionConfigurationWhenNoSolutionLoaded { get; private set; }

        [DataMember]
        public ObservableCollection<VSSolutionConfiguration> Solutions { get; private set; }

        public ObservableCollection<ConnectionData> Connections { get; private set; }

        public ObservableCollection<ConnectionData> ArchiveConnections { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ConnectionConfiguration()
        {
            this.VSSolutionConfigurationWhenNoSolutionLoaded = new VSSolutionConfiguration();

            this.Connections = new ObservableCollection<ConnectionData>();
            this.ArchiveConnections = new ObservableCollection<ConnectionData>();
            this.Users = new ObservableCollection<ConnectionUserData>();
            this.Solutions = new ObservableCollection<VSSolutionConfiguration>();

            this.ConnectionsGuids = new List<Guid>();
            this.ArchiveConnectionsGuids = new List<Guid>();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (this.VSSolutionConfigurationWhenNoSolutionLoaded == null)
            {
                this.VSSolutionConfigurationWhenNoSolutionLoaded = new VSSolutionConfiguration();
            }

            if (this.Connections == null)
            {
                this.Connections = new ObservableCollection<ConnectionData>();
            }

            if (this.ArchiveConnections == null)
            {
                this.ArchiveConnections = new ObservableCollection<ConnectionData>();
            }

            if (this.Users == null)
            {
                this.Users = new ObservableCollection<ConnectionUserData>();
            }

            if (this.Solutions == null)
            {
                this.Solutions = new ObservableCollection<VSSolutionConfiguration>();
            }

            if (this.ConnectionsGuids == null)
            {
                this.ConnectionsGuids = new List<Guid>();
            }

            if (this.ArchiveConnectionsGuids == null)
            {
                this.ArchiveConnectionsGuids = new List<Guid>();
            }
        }

        private void LoadConnectionsFromDisk()
        {
            List<ConnectionData> loadedConnections = new List<ConnectionData>();

            loadedConnections.AddRange(this.Connections);
            loadedConnections.AddRange(this.ArchiveConnections);

            foreach (var connection in loadedConnections)
            {
                connection.StopWatchFile();
            }

            this.Connections.Clear();
            this.ArchiveConnections.Clear();

            foreach (var connectionId in this.ConnectionsGuids)
            {
                if (!this.Connections.Any(c => c.ConnectionId == connectionId))
                {
                    ConnectionData connectionData = loadedConnections.FirstOrDefault(c => c.ConnectionId == connectionId);

                    if (connectionData == null)
                    {
                        connectionData = ConnectionData.Get(connectionId);
                    }

                    if (connectionData != null)
                    {
                        connectionData.ConnectionConfiguration = this;
                        connectionData.User = this.Users.FirstOrDefault(u => u.UserId == connectionData.UserId);

                        connectionData.LoadIntellisenseAsync();
                        connectionData.StartWatchFile();

                        this.Connections.Add(connectionData);
                    }
                }
            }

            foreach (var connectionId in this.ArchiveConnectionsGuids)
            {
                if (!this.ArchiveConnections.Any(c => c.ConnectionId == connectionId))
                {
                    ConnectionData connectionData = loadedConnections.FirstOrDefault(c => c.ConnectionId == connectionId);

                    if (connectionData == null)
                    {
                        connectionData = ConnectionData.Get(connectionId);
                    }

                    if (connectionData != null)
                    {
                        connectionData.ConnectionConfiguration = this;
                        connectionData.User = this.Users.FirstOrDefault(u => u.UserId == connectionData.UserId);

                        connectionData.LoadIntellisenseAsync();
                        connectionData.StartWatchFile();

                        this.ArchiveConnections.Add(connectionData);
                    }
                }
            }
        }

        /// <summary>
        /// Получение конфига из файла
        /// </summary>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public static ConnectionConfiguration Get()
        {
            lock (_syncObject)
            {
                if (_singleton != null)
                {
                    var applicationObject = CrmDeveloperHelperPackage.ServiceProvider?.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                    _singleton.SetCurrentSolution(applicationObject?.Solution?.FullName);

                    return _singleton;
                }

                string filePath = FileOperations.GetConnectionConfigFilePath();

                ConnectionConfiguration result = GetFromPath(filePath);

                result = result ?? new ConnectionConfiguration();

                result.Path = filePath;

                if (_singleton == null)
                {
                    _singleton = result;
                    _singleton.LoadConnectionsFromDisk();

                    var applicationObject = CrmDeveloperHelperPackage.ServiceProvider?.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                    result.SetCurrentSolution(applicationObject?.Solution?.FullName);
                }

                return _singleton;
            }
        }

        private static ConnectionConfiguration GetFromPath(string filePath)
        {
            ConnectionConfiguration result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(ConnectionConfiguration));

                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        using (var sr = File.OpenRead(filePath))
                        {
                            result = ser.ReadObject(sr) as ConnectionConfiguration;
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);

                        FileOperations.CreateBackUpFile(filePath, ex);

                        result = null;
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }

            return result;
        }

        private void StartWatchFile()
        {
            if (_watcher != null)
            {
                return;
            }

            _watcher = new FileSystemWatcher()
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                Path = System.IO.Path.GetDirectoryName(this.Path),
                Filter = System.IO.Path.GetFileName(this.Path),
            };

            _watcher.Changed += _watcher_Changed;

            _watcher.EnableRaisingEvents = true;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var diskData = GetFromPath(this.Path);

            LoadFromDisk(diskData);
        }

        private void LoadFromDisk(ConnectionConfiguration diskData)
        {
            this.ConnectionsGuids.Clear();
            this.ConnectionsGuids.AddRange(diskData.ConnectionsGuids);

            this.ArchiveConnectionsGuids.Clear();
            this.ArchiveConnectionsGuids.AddRange(diskData.ArchiveConnectionsGuids);

            this.Solutions.Clear();
            foreach (var item in diskData.Solutions)
            {
                this.Solutions.Add(item);
            }

            this.Users.Clear();
            foreach (var item in diskData.Users)
            {
                this.Users.Add(item);
            }

            this.VSSolutionConfigurationWhenNoSolutionLoaded = diskData.VSSolutionConfigurationWhenNoSolutionLoaded;

            LoadConnectionsFromDisk();
        }

        /// <summary>
        /// Сохранение конфигурации в папку
        /// </summary>
        /// <param name="filePath"></param>
        private void Save(string filePath)
        {
            this.Path = filePath;

            this.ConnectionsGuids.Clear();
            this.ConnectionsGuids.AddRange(this.Connections.Select(c => c.ConnectionId));

            this.ArchiveConnectionsGuids.Clear();
            this.ArchiveConnectionsGuids.AddRange(this.ArchiveConnections.Select(c => c.ConnectionId));

            DataContractSerializer ser = new DataContractSerializer(typeof(ConnectionConfiguration));

            byte[] fileBody = null;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true,
                        Encoding = Encoding.UTF8
                    };

                    using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings))
                    {
                        ser.WriteObject(xmlWriter, this);
                        xmlWriter.Flush();
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    fileBody = memoryStream.ToArray();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToLog(ex);

                    fileBody = null;
                }
            }

            if (fileBody != null)
            {
                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        if (_watcher != null)
                        {
                            _watcher.EnableRaisingEvents = false;
                        }

                        try
                        {
                            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                stream.Write(fileBody, 0, fileBody.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToLog(ex);
                        }

                        if (_watcher != null)
                        {
                            _watcher.EnableRaisingEvents = true;
                        }
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
        }

        public void Save()
        {
            this.Save(this.Path);
        }

        private void SetCurrentSolution(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
            {
                this.VSSolutionConfigurationWhenNoSolutionLoaded = this.VSSolutionConfigurationWhenNoSolutionLoaded ?? new VSSolutionConfiguration();
                this._currentSolutionConfiguration = this.VSSolutionConfigurationWhenNoSolutionLoaded;
                return;
            }

            var solution = this.Solutions.FirstOrDefault(s => s.SolutionPath.Equals(solutionPath, StringComparison.InvariantCultureIgnoreCase));

            if (solution != null)
            {
                if (this._currentSolutionConfiguration != solution)
                {
                    this._currentSolutionConfiguration = solution;
                }
            }
            else
            {
                solution = new VSSolutionConfiguration()
                {
                    SolutionPath = solutionPath,
                };

                this.Solutions.Add(solution);

                this._currentSolutionConfiguration = solution;
            }
        }

        public void SetCurrentConnection(Guid? selectedConnection)
        {
            foreach (var item in this.Connections)
            {
                item.IsCurrentConnection = false;
            }

            if (_currentSolutionConfiguration == null)
            {
                this.VSSolutionConfigurationWhenNoSolutionLoaded = this.VSSolutionConfigurationWhenNoSolutionLoaded ?? new VSSolutionConfiguration();

                _currentSolutionConfiguration = this.VSSolutionConfigurationWhenNoSolutionLoaded;
            }

            var connection = this.Connections.FirstOrDefault(c => c.ConnectionId == selectedConnection);

            if (connection != null)
            {
                connection.IsCurrentConnection = true;
                _currentSolutionConfiguration.SelectedConnectionId = selectedConnection;
            }
            else
            {
                _currentSolutionConfiguration.SelectedConnectionId = null;
            }
        }

        private static readonly TimeSpan _cacheItemSpan = TimeSpan.FromSeconds(1);

        public List<ConnectionData> GetConnectionsWithoutCurrent()
        {
            ObjectCache cache = MemoryCache.Default;
            const string cacheName = nameof(GetConnectionsWithoutCurrent);

            List<ConnectionData> result = null;

            if (cache.Contains(cacheName))
            {
                result = (List<ConnectionData>)cache.Get(cacheName);
            }
            else
            {
                result = GetConnectionsWithoutCurrentInternal();

                if (result != null)
                {
                    cache.Set(cacheName, result, new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheItemSpan),
                    });
                }
            }

            return result;
        }

        private List<ConnectionData> GetConnectionsWithoutCurrentInternal()
        {
            return this.Connections.Where(c => this.CurrentConnectionData?.ConnectionId != c.ConnectionId).ToList();
        }

        public List<ConnectionData> GetConnectionsByGroupWithoutCurrent()
        {
            ObjectCache cache = MemoryCache.Default;
            const string cacheName = nameof(GetConnectionsByGroupWithoutCurrent);

            List<ConnectionData> result = null;

            if (cache.Contains(cacheName))
            {
                result = (List<ConnectionData>)cache.Get(cacheName);
            }
            else
            {
                result = GetConnectionsByGroupWithoutCurrentInternal();

                if (result != null)
                {
                    cache.Set(cacheName, result, new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheItemSpan),
                    });
                }
            }

            return result;
        }

        private List<ConnectionData> GetConnectionsByGroupWithoutCurrentInternal()
        {
            if (this.CurrentConnectionData == null
                || string.IsNullOrEmpty(this.CurrentConnectionData.GroupName)
            )
            {
                return new List<ConnectionData>();
            }

            return this.Connections.Where(c =>
                    string.Equals(this.CurrentConnectionData.GroupName, c.GroupName, StringComparison.InvariantCultureIgnoreCase)
                    && this.CurrentConnectionData.ConnectionId != c.ConnectionId
                ).ToList();
        }

        public List<ConnectionData> GetConnectionsByGroupWithCurrent()
        {
            ObjectCache cache = MemoryCache.Default;
            const string cacheName = nameof(GetConnectionsByGroupWithCurrent);

            List<ConnectionData> result = null;

            if (cache.Contains(cacheName))
            {
                result = (List<ConnectionData>)cache.Get(cacheName);
            }
            else
            {
                result = GetConnectionsByGroupWithCurrentInternal();

                if (result != null)
                {
                    cache.Set(cacheName, result, new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheItemSpan),
                    });
                }
            }

            return result;
        }

        private List<ConnectionData> GetConnectionsByGroupWithCurrentInternal()
        {
            if (this.CurrentConnectionData == null)
            {
                return new List<ConnectionData>();
            }

            if (string.IsNullOrEmpty(this.CurrentConnectionData.GroupName))
            {
                return new List<ConnectionData>() { this.CurrentConnectionData };
            }

            return this.Connections.Where(c =>
                string.Equals(this.CurrentConnectionData.GroupName, c.GroupName, StringComparison.InvariantCultureIgnoreCase)
            ).ToList();
        }

        public TupleList<ConnectionData, ConnectionData> GetConnectionPairsByGroup()
        {
            ObjectCache cache = MemoryCache.Default;
            const string cacheName = nameof(GetConnectionPairsByGroup);

            TupleList<ConnectionData, ConnectionData> result = null;

            if (cache.Contains(cacheName))
            {
                result = (TupleList<ConnectionData, ConnectionData>)cache.Get(cacheName);
            }
            else
            {
                result = GetConnectionPairsByGroupInternal();

                if (result != null)
                {
                    cache.Set(cacheName, result, new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheItemSpan),
                    });
                }
            }

            return result;
        }

        private TupleList<ConnectionData, ConnectionData> GetConnectionPairsByGroupInternal()
        {
            var connectionsList = GetConnectionsByGroupWithCurrent();

            var result = new TupleList<ConnectionData, ConnectionData>();

            for (int index1 = 0; index1 < connectionsList.Count; index1++)
            {
                for (int index2 = index1 + 1; index2 < connectionsList.Count; index2++)
                {
                    result.Add(connectionsList[index1], connectionsList[index2]);
                }
            }

            return result;
        }
    }
}