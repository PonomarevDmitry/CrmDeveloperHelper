using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class SiteMapIntellisenseDataRepository : IDisposable
    {
        private object _syncObjectService = new object();

        private object _syncObjectTaskGettingSitemInformation = new object();
        private Task _taskGettingSitemInformation;

        private IOrganizationServiceExtented _service;

        private ConnectionData _connectionData;

        private SiteMapIntellisenseData _siteMapIntellisenseData = new SiteMapIntellisenseData();

        private CancellationTokenSource _cancellationTokenSource;

        private static object _staticSyncObjectCommon = new object();
        private static ConcurrentDictionary<Guid, SiteMapIntellisenseDataRepository> _staticCacheRepositories = new ConcurrentDictionary<Guid, SiteMapIntellisenseDataRepository>();

        private SiteMapIntellisenseDataRepository(ConnectionData connectionData)
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

        public SiteMapIntellisenseData GetSiteMapIntellisenseData()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            if (!_siteMapIntellisenseData.NextLoadFileDate.HasValue || _siteMapIntellisenseData.NextLoadFileDate < DateTime.Now)
            {
                StartGettingSiteMapsAsync();
            }

            return _siteMapIntellisenseData;
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

                _siteMapIntellisenseData.ClearData();

                if (Version.TryParse(_connectionData.OrganizationVersion, out var organizationVersion))
                {
                    string version = "365.8.2";

                    switch (organizationVersion.Major)
                    {
                        case 5:
                            version = "2011";
                            break;

                        case 6:
                            version = "2013";
                            break;

                        case 7:
                            if (organizationVersion.Minor == 0)
                                version = "2015";
                            else
                                version = "2015SP1";
                            break;

                        case 8:
                            if (organizationVersion.Minor == 0)
                                version = "2016";
                            else if (organizationVersion.Minor == 1)
                                version = "2016SP1";
                            else
                                version = "365.8.2";
                            break;
                    }

                    Uri uri = FileOperations.GetSiteMapResourceUri(version);
                    StreamResourceInfo info = Application.GetResourceStream(uri);

                    var doc = XDocument.Load(info.Stream);

                    _siteMapIntellisenseData.LoadDataFromSiteMap(doc);

                    info.Stream.Dispose();
                }

                {
                    var repository = new SitemapRepository(service);

                    var listSiteMaps = await repository.GetListAsync(new ColumnSet(SiteMap.Schema.Attributes.sitemapxml));

                    foreach (var item in listSiteMaps)
                    {
                        if (string.IsNullOrEmpty(item.SiteMapXml))
                        {
                            continue;
                        }

                        if (ContentCoparerHelper.TryParseXmlDocument(item.SiteMapXml, out var doc))
                        {
                            _siteMapIntellisenseData.LoadDataFromSiteMap(doc);
                        }
                    }
                }

                if (service.ConnectionData.OrganizationId.HasValue)
                {
                    var repository = new OrganizationRepository(service);

                    var organization = await repository.GetByIdAsync(service.ConnectionData.OrganizationId.Value, new ColumnSet(Organization.Schema.Attributes.referencesitemapxml, Organization.Schema.Attributes.sitemapxml));

                    if (organization != null)
                    {
                        if (!string.IsNullOrEmpty(organization.ReferenceSiteMapXml))
                        {
                            if (ContentCoparerHelper.TryParseXmlDocument(organization.ReferenceSiteMapXml, out var doc))
                            {
                                _siteMapIntellisenseData.LoadDataFromSiteMap(doc);
                            }
                        }

                        if (!string.IsNullOrEmpty(organization.SiteMapXml))
                        {
                            if (ContentCoparerHelper.TryParseXmlDocument(organization.SiteMapXml, out var doc))
                            {
                                _siteMapIntellisenseData.LoadDataFromSiteMap(doc);
                            }
                        }
                    }
                }

                {
                    var repository = new SystemFormRepository(service);

                    var listSystemForms = await repository.GetListByTypeAsync((int)SystemForm.Schema.OptionSets.type.Dashboard_0, new ColumnSet(SystemForm.Schema.Attributes.objecttypecode, SystemForm.Schema.Attributes.name, SystemForm.Schema.Attributes.description));

                    _siteMapIntellisenseData.LoadDashboards(listSystemForms);
                }

                {
                    var repository = new WebResourceRepository(service);

                    {
                        var listWebResources = await repository.GetListByTypesAsync(new[]
                            {
                            (int)WebResource.Schema.OptionSets.webresourcetype.Webpage_HTML_1
                        }
                            , new ColumnSet(WebResource.Schema.Attributes.displayname, WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.description));

                        _siteMapIntellisenseData.LoadWebResourcesHtml(listWebResources);
                    }

                    {
                        var listWebResources = await repository.GetListByTypesAsync(new[]
                            {
                            (int)WebResource.Schema.OptionSets.webresourcetype.PNG_format_5
                            , (int)WebResource.Schema.OptionSets.webresourcetype.JPG_format_6
                            , (int)WebResource.Schema.OptionSets.webresourcetype.GIF_format_7
                            , (int)WebResource.Schema.OptionSets.webresourcetype.ICO_format_10
                            , (int)WebResource.Schema.OptionSets.webresourcetype.SVG_format_11
                        }
                            , new ColumnSet(WebResource.Schema.Attributes.displayname, WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.webresourcetype, WebResource.Schema.Attributes.content, WebResource.Schema.Attributes.description));

                        _siteMapIntellisenseData.LoadWebResourcesIcon(listWebResources);
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

        static SiteMapIntellisenseDataRepository()
        {
            Task.Run(() => LoadIntellisenseRepository());
        }

        public static void LoadStaticConstructor()
        {

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

        public static SiteMapIntellisenseDataRepository GetRepository(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return null;
            }

            lock (_staticSyncObjectCommon)
            {
                if (!_staticCacheRepositories.ContainsKey(connectionData.ConnectionId))
                {
                    var repository = new SiteMapIntellisenseDataRepository(connectionData);

                    _staticCacheRepositories.TryAdd(connectionData.ConnectionId, repository);
                }

                return _staticCacheRepositories[connectionData.ConnectionId];
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

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}