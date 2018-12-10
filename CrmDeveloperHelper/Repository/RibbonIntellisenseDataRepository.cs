using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class RibbonIntellisenseDataRepository : IDisposable
    {
        private readonly object _syncObjectService = new object();

        private ConcurrentDictionary<string, object> _syncObjectTaskGettingRibbonInformationCache = new ConcurrentDictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        private object GetEntitySyncObject(string entityName)
        {
            entityName = entityName ?? string.Empty;

            if (!_syncObjectTaskGettingRibbonInformationCache.ContainsKey(entityName))
            {
                _syncObjectTaskGettingRibbonInformationCache.TryAdd(entityName, new object());
            }

            return _syncObjectTaskGettingRibbonInformationCache[entityName];
        }

        private ConcurrentDictionary<string, Task> _taskGettingRibbonInformationCache = new ConcurrentDictionary<string, Task>(StringComparer.InvariantCultureIgnoreCase);

        private IOrganizationServiceExtented _service;

        private readonly ConnectionData _connectionData;

        private ConcurrentDictionary<string, RibbonIntellisenseData> _RibbonIntellisenseDataCache = new ConcurrentDictionary<string, RibbonIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);

        private CancellationTokenSource _cancellationTokenSource;

        private static ConcurrentDictionary<Guid, RibbonIntellisenseDataRepository> _staticCacheRepositories = new ConcurrentDictionary<Guid, RibbonIntellisenseDataRepository>();

        private RibbonIntellisenseDataRepository(ConnectionData connectionData)
        {
            this._connectionData = connectionData ?? throw new ArgumentNullException(nameof(connectionData));

            _cancellationTokenSource = new CancellationTokenSource();
        }

        private async Task<IOrganizationServiceExtented> GetServiceAsync()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            if (_service != null)
            {
                return _service;
            }

            IOrganizationServiceExtented localService = null;

            try
            {
                localService = await QuickConnection.ConnectAsync(this._connectionData);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }

            lock (_syncObjectService)
            {
                if (localService != null && _service == null)
                {
                    _service = localService;
                }

                return _service;
            }
        }

        public RibbonIntellisenseData GetRibbonIntellisenseData(string entityName)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            entityName = entityName ?? string.Empty;

            if (!_RibbonIntellisenseDataCache.ContainsKey(entityName))
            {
                _RibbonIntellisenseDataCache.TryAdd(entityName, new RibbonIntellisenseData(this._connectionData.ConnectionId, entityName));
            }

            if (!_RibbonIntellisenseDataCache[entityName].NextLoadFileDate.HasValue || _RibbonIntellisenseDataCache[entityName].NextLoadFileDate < DateTime.Now)
            {
                StartGettingRibbonAsync(entityName);
            }

            return _RibbonIntellisenseDataCache[entityName];
        }

        private void StartGettingRibbonAsync(string entityName)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            entityName = entityName ?? string.Empty;

            var syncObject = GetEntitySyncObject(entityName);

            lock (syncObject)
            {
                if (_taskGettingRibbonInformationCache.ContainsKey(entityName))
                {
                    var task = _taskGettingRibbonInformationCache[entityName];

                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        _taskGettingRibbonInformationCache.TryRemove(entityName, out _);
                    }
                    else if (task.Status == TaskStatus.Faulted)
                    {
                        DTEHelper.WriteExceptionToLog(task.Exception);

                        _taskGettingRibbonInformationCache.TryAdd(entityName, Task.Run(() => StartGettingRibbon(entityName), _cancellationTokenSource.Token));
                    }
                }
                else
                {
                    _taskGettingRibbonInformationCache.TryAdd(entityName, Task.Run(() => StartGettingRibbon(entityName), _cancellationTokenSource.Token));
                }
            }
        }

        private async Task StartGettingRibbon(string entityName)
        {
            try
            {
                var service = await GetServiceAsync();

                var ribbonData = _RibbonIntellisenseDataCache[entityName];

                ribbonData.ClearData();

                var repository = new RibbonCustomizationRepository(service);

                XDocument docRibbon = await repository.GetEntityRibbonAsync(entityName);

                ribbonData.LoadDataFromRibbon(docRibbon);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
            finally
            {
                var syncObject = GetEntitySyncObject(entityName);

                lock (syncObject)
                {
                    _taskGettingRibbonInformationCache.TryRemove(entityName, out _);
                }
            }
        }

        static RibbonIntellisenseDataRepository()
        {
            Task.Run(() => LoadIntellisenseRepository());
        }

        private static void LoadIntellisenseRepository()
        {
            var connectionConfig = ConnectionConfiguration.Get();

            foreach (var connectionData in connectionConfig.Connections)
            {
                if (connectionData != connectionConfig.CurrentConnectionData)
                {
                    try
                    {
                        GetRepository(connectionData);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToLog(ex);
                    }
                }
            }
        }

        public static RibbonIntellisenseDataRepository GetRepository(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return null;
            }

            if (!_staticCacheRepositories.ContainsKey(connectionData.ConnectionId))
            {
                var repository = new RibbonIntellisenseDataRepository(connectionData);

                _staticCacheRepositories.TryAdd(connectionData.ConnectionId, repository);
            }

            return _staticCacheRepositories[connectionData.ConnectionId];
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        _cancellationTokenSource.Cancel();
                    }

                    _cancellationTokenSource.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}
