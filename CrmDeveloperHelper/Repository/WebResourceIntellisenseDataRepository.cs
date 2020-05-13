using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class WebResourceIntellisenseDataRepository : IDisposable
    {
        private const int _loadPeriodInSeconds = 45;

        private readonly object _syncObjectService = new object();

        private readonly object _syncObjectTaskGettingWebResources = new object();

        private readonly ConnectionData _connectionData;

        private Task _taskGettingWebResources;

        private DateTime? _nextLoadFromCrmDate;

        private IOrganizationServiceExtented _service;

        private CancellationTokenSource _cancellationTokenSource;

        private static ConcurrentDictionary<Guid, WebResourceIntellisenseDataRepository> _staticCacheRepositories = new ConcurrentDictionary<Guid, WebResourceIntellisenseDataRepository>();

        private WebResourceIntellisenseDataRepository(ConnectionData connectionData)
        {
            this._connectionData = connectionData ?? throw new ArgumentNullException(nameof(connectionData));

            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() => StartGettingWebResources(), _cancellationTokenSource.Token);
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

                return localService;
            }
        }

        public ConnectionWebResourceIntellisenseData GetConnectionWebResourceIntellisenseData()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            StartGettingWebResourcesAsync();

            return _connectionData.WebResourceIntellisenseData;
        }

        private void StartGettingWebResourcesAsync()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            if (this._nextLoadFromCrmDate.HasValue && DateTime.Now < this._nextLoadFromCrmDate)
            {
                return;
            }

            lock (_syncObjectTaskGettingWebResources)
            {
                if (_taskGettingWebResources != null)
                {
                    if (_taskGettingWebResources.Status == TaskStatus.RanToCompletion)
                    {
                        _taskGettingWebResources = null;
                    }
                    else if (_taskGettingWebResources.Status == TaskStatus.Faulted)
                    {
                        DTEHelper.WriteExceptionToLog(_taskGettingWebResources.Exception);

                        _taskGettingWebResources = Task.Run(() => StartGettingWebResources(), _cancellationTokenSource.Token);
                    }
                }
                else
                {
                    _taskGettingWebResources = Task.Run(() => StartGettingWebResources(), _cancellationTokenSource.Token);
                }
            }
        }

        private async Task StartGettingWebResources()
        {
            try
            {
                var service = await GetServiceAsync();

                if (service == null)
                {
                    return;
                }

                var repository = new WebResourceRepository(service);

                var listWebResources = await repository.GetListAllAsync(
                    null
                    , new ColumnSet
                    (
                        WebResource.Schema.Attributes.name
                        , WebResource.Schema.Attributes.displayname
                        , WebResource.Schema.Attributes.description
                        , WebResource.Schema.Attributes.webresourcetype
                        , WebResource.Schema.Attributes.languagecode
                    )
                );

                LoadWebResources(listWebResources, _connectionData.WebResourceIntellisenseData.WebResourcesAll);

                this._nextLoadFromCrmDate = DateTime.Now.AddSeconds(_loadPeriodInSeconds);

                _connectionData.WebResourceIntellisenseData.SaveIntellisenseDataByTime();

                //{
                //    var listWebResources = await repository.GetListByTypesAsync(
                //        new[]
                //        {
                //            (int)WebResource.Schema.OptionSets.webresourcetype.PNG_format_5
                //            , (int)WebResource.Schema.OptionSets.webresourcetype.JPG_format_6
                //            , (int)WebResource.Schema.OptionSets.webresourcetype.GIF_format_7
                //            , (int)WebResource.Schema.OptionSets.webresourcetype.ICO_format_10
                //            , (int)WebResource.Schema.OptionSets.webresourcetype.Vector_format_SVG_11
                //        }
                //        , new ColumnSet
                //        (
                //            WebResource.Schema.Attributes.name
                //            , WebResource.Schema.Attributes.displayname
                //            , WebResource.Schema.Attributes.description
                //            , WebResource.Schema.Attributes.webresourcetype
                //            , WebResource.Schema.Attributes.languagecode
                //            , WebResource.Schema.Attributes.content
                //        )
                //    );

                //    LoadWebResources(listWebResources, _WebResourceIntellisenseData.WebResourcesIcon);

                //    _WebResourceIntellisenseData.NextLoadFileDate = DateTime.Now.AddMinutes(_loadPeriodInMinutes);
                //}
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }
            finally
            {
                lock (_syncObjectTaskGettingWebResources)
                {
                    _taskGettingWebResources = null;
                }
            }
        }

        static WebResourceIntellisenseDataRepository()
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

        public static WebResourceIntellisenseDataRepository GetRepository(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return null;
            }

            if (!_staticCacheRepositories.ContainsKey(connectionData.ConnectionId))
            {
                var repository = new WebResourceIntellisenseDataRepository(connectionData);

                _staticCacheRepositories.TryAdd(connectionData.ConnectionId, repository);
            }

            return _staticCacheRepositories[connectionData.ConnectionId];
        }

        private static void LoadWebResources(IEnumerable<WebResource> webResources, ConcurrentDictionary<string, WebResourceIntellisenseData> concurrentDictionary)
        {
            concurrentDictionary.Clear();

            if (!webResources.Any())
            {
                return;
            }

            foreach (var item in webResources.OrderBy(e => e.Name))
            {
                if (!concurrentDictionary.ContainsKey(item.Name))
                {
                    concurrentDictionary.TryAdd(item.Name, new WebResourceIntellisenseData());
                }

                concurrentDictionary[item.Name].LoadData(item);
            }
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

        ~WebResourceIntellisenseDataRepository()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}