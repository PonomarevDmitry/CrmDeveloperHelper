using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class RibbonIntellisenseDataRepository : IDisposable
    {
        private const int _loadPeriodInSeconds = 45;

        private readonly ConcurrentDictionary<string, object> _syncObjectTaskGettingRibbonInformationCache = new ConcurrentDictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        private readonly ConnectionData _connectionData;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly ConcurrentDictionary<string, DateTime> _nextLoadDateTime = new ConcurrentDictionary<string, DateTime>();

        private readonly ConcurrentDictionary<string, Task> _taskGettingRibbonInformationCache = new ConcurrentDictionary<string, Task>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly ConcurrentDictionary<Guid, RibbonIntellisenseDataRepository> _staticCacheRepositories = new ConcurrentDictionary<Guid, RibbonIntellisenseDataRepository>();

        private RibbonIntellisenseDataRepository(ConnectionData connectionData)
        {
            this._connectionData = connectionData ?? throw new ArgumentNullException(nameof(connectionData));

            this._cancellationTokenSource = new CancellationTokenSource();
        }

        private object GetEntitySyncObject(string entityName)
        {
            entityName = entityName ?? string.Empty;

            if (!_syncObjectTaskGettingRibbonInformationCache.ContainsKey(entityName))
            {
                _syncObjectTaskGettingRibbonInformationCache.TryAdd(entityName, new object());
            }

            return _syncObjectTaskGettingRibbonInformationCache[entityName];
        }

        private async Task<IOrganizationServiceExtented> GetServiceAsync()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            IOrganizationServiceExtented service = null;

            try
            {
                service = await QuickConnection.ConnectAsync(this._connectionData);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);

                service = null;
            }

            return service;
        }

        public RibbonIntellisenseData GetRibbonIntellisenseData(string entityName)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            StartGettingRibbonAsync(entityName);

            if (_connectionData.RibbonIntellisense != null)
            {
                if (string.IsNullOrEmpty(entityName))
                {
                    return _connectionData.RibbonIntellisense.ApplicationRibbonData;
                }
                else
                {
                    if (_connectionData.RibbonIntellisense.EntitiesRibbonData != null
                        && _connectionData.RibbonIntellisense.EntitiesRibbonData.ContainsKey(entityName)
                    )
                    {
                        return _connectionData.RibbonIntellisense.EntitiesRibbonData[entityName];
                    }
                }
            }

            return null;
        }

        private void StartGettingRibbonAsync(string entityName)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            entityName = entityName ?? string.Empty;

            if (_nextLoadDateTime.ContainsKey(entityName) && DateTime.Now < _nextLoadDateTime[entityName])
            {
                return;
            }

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
            var service = await GetServiceAsync();

            if (service == null)
            {
                return;
            }

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                XDocument docRibbon = await repository.GetEntityRibbonAsync(entityName);

                this._nextLoadDateTime[entityName] = DateTime.Now.AddSeconds(_loadPeriodInSeconds);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
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

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            disposedValue = true;

            if (disposing)
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _cancellationTokenSource.Cancel();
                }

                _cancellationTokenSource.Dispose();
            }
        }

        ~RibbonIntellisenseDataRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}
