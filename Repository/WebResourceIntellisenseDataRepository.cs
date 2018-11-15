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

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class WebResourceIntellisenseDataRepository : IDisposable
    {
        private readonly object _syncObjectService = new object();

        private readonly object _syncObjectTaskGettingSitemInformation = new object();
        private Task _taskGettingSitemInformation;

        private IOrganizationServiceExtented _service;

        private readonly ConnectionData _connectionData;

        private WebResourceIntellisenseData _WebResourceIntellisenseData = new WebResourceIntellisenseData();

        private CancellationTokenSource _cancellationTokenSource;

        private static ConcurrentDictionary<Guid, WebResourceIntellisenseDataRepository> _staticCacheRepositories = new ConcurrentDictionary<Guid, WebResourceIntellisenseDataRepository>();

        private WebResourceIntellisenseDataRepository(ConnectionData connectionData)
        {
            this._connectionData = connectionData ?? throw new ArgumentNullException(nameof(connectionData));

            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() => StartGettingSiteMaps(), _cancellationTokenSource.Token);
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

        public WebResourceIntellisenseData GetWebResourceIntellisenseData()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            if (!_WebResourceIntellisenseData.NextLoadFileDate.HasValue || _WebResourceIntellisenseData.NextLoadFileDate < DateTime.Now)
            {
                StartGettingSiteMapsAsync();
            }

            return _WebResourceIntellisenseData;
        }

        private void StartGettingSiteMapsAsync()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            lock (_syncObjectTaskGettingSitemInformation)
            {
                if (_taskGettingSitemInformation != null)
                {
                    if (_taskGettingSitemInformation.Status == TaskStatus.RanToCompletion)
                    {
                        _taskGettingSitemInformation = null;
                    }
                    else if (_taskGettingSitemInformation.Status == TaskStatus.Faulted)
                    {
                        DTEHelper.WriteExceptionToLog(_taskGettingSitemInformation.Exception);

                        _taskGettingSitemInformation = Task.Run(() => StartGettingSiteMaps(), _cancellationTokenSource.Token);
                    }
                }
                else
                {
                    _taskGettingSitemInformation = Task.Run(() => StartGettingSiteMaps(), _cancellationTokenSource.Token);
                }
            }
        }

        private async Task StartGettingSiteMaps()
        {
            try
            {
                var service = await GetServiceAsync();

                _WebResourceIntellisenseData.ClearData();

                {
                    var repository = new WebResourceRepository(service);

                    {
                        var listWebResources = await repository.GetListByTypesAsync(
                            new[] { (int)WebResource.Schema.OptionSets.webresourcetype.Webpage_HTML_1 }
                            , new ColumnSet
                            (
                                WebResource.Schema.Attributes.displayname
                                , WebResource.Schema.Attributes.name
                                , WebResource.Schema.Attributes.description
                            )
                        );

                        _WebResourceIntellisenseData.LoadWebResources(listWebResources, _WebResourceIntellisenseData.WebResourcesHtml);
                    }

                    {
                        var listWebResources = await repository.GetListByTypesAsync(
                            new[] { (int)WebResource.Schema.OptionSets.webresourcetype.Script_JScript_3 }
                            , new ColumnSet
                            (
                                WebResource.Schema.Attributes.displayname
                                , WebResource.Schema.Attributes.name
                                , WebResource.Schema.Attributes.description
                            )
                        );

                        _WebResourceIntellisenseData.LoadWebResources(listWebResources, _WebResourceIntellisenseData.WebResourcesJavaScript);
                    }

                    {
                        var listWebResources = await repository.GetListByTypesAsync(
                            new[]
                            {
                                (int)WebResource.Schema.OptionSets.webresourcetype.PNG_format_5
                                , (int)WebResource.Schema.OptionSets.webresourcetype.JPG_format_6
                                , (int)WebResource.Schema.OptionSets.webresourcetype.GIF_format_7
                                , (int)WebResource.Schema.OptionSets.webresourcetype.ICO_format_10
                                , (int)WebResource.Schema.OptionSets.webresourcetype.SVG_format_11
                            }
                            , new ColumnSet
                            (
                                WebResource.Schema.Attributes.displayname
                                , WebResource.Schema.Attributes.name
                                , WebResource.Schema.Attributes.webresourcetype
                                , WebResource.Schema.Attributes.content
                                , WebResource.Schema.Attributes.description
                            )
                        );

                        _WebResourceIntellisenseData.LoadWebResources(listWebResources, _WebResourceIntellisenseData.WebResourcesIcon);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
            finally
            {
                lock (_syncObjectTaskGettingSitemInformation)
                {
                    _taskGettingSitemInformation = null;
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
        #endregion
    }
}