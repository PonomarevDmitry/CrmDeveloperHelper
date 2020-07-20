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
    public class SiteMapIntellisenseDataRepository : IDisposable
    {
        private const int _loadPeriodInMinutes = 1;

        private DateTime? _nextLoadFileDate;

        private readonly object _syncObjectTaskGettingSiteMapInformation = new object();

        private Task _taskGettingSiteMapInformation;

        private readonly ConnectionData _connectionData;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private static readonly ConcurrentDictionary<Guid, SiteMapIntellisenseDataRepository> _staticCacheRepositories = new ConcurrentDictionary<Guid, SiteMapIntellisenseDataRepository>();

        private SiteMapIntellisenseDataRepository(ConnectionData connectionData)
        {
            this._connectionData = connectionData ?? throw new ArgumentNullException(nameof(connectionData));

            this._cancellationTokenSource = new CancellationTokenSource();

            var task = Task.Run(() => StartGettingSiteMaps(), _cancellationTokenSource.Token);
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

        public SiteMapIntellisenseData GetSiteMapIntellisenseData()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            if (!_nextLoadFileDate.HasValue || _nextLoadFileDate < DateTime.Now)
            {
                StartGettingSiteMapsAsync();
            }

            return _connectionData.SiteMapIntellisenseData;
        }

        private void StartGettingSiteMapsAsync()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            lock (_syncObjectTaskGettingSiteMapInformation)
            {
                if (_taskGettingSiteMapInformation != null)
                {
                    if (_taskGettingSiteMapInformation.Status == TaskStatus.RanToCompletion)
                    {
                        _taskGettingSiteMapInformation = null;
                    }
                    else if (_taskGettingSiteMapInformation.Status == TaskStatus.Faulted)
                    {
                        DTEHelper.WriteExceptionToLog(_taskGettingSiteMapInformation.Exception);

                        _taskGettingSiteMapInformation = Task.Run(() => StartGettingSiteMaps(), _cancellationTokenSource.Token);
                    }
                }
                else
                {
                    _taskGettingSiteMapInformation = Task.Run(() => StartGettingSiteMaps(), _cancellationTokenSource.Token);
                }
            }
        }

        private async Task StartGettingSiteMaps()
        {
            try
            {
                var service = await GetServiceAsync();

                if (service == null)
                {
                    return;
                }

                _connectionData.SiteMapIntellisenseData.ClearData();

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

                    _connectionData.SiteMapIntellisenseData.LoadDataFromSiteMap(doc);

                    info.Stream.Dispose();
                }

                {
                    var repository = new SiteMapRepository(service);

                    var listSiteMaps = await repository.GetListAsync(new ColumnSet(SiteMap.Schema.Attributes.sitemapxml));

                    foreach (var item in listSiteMaps)
                    {
                        if (string.IsNullOrEmpty(item.SiteMapXml))
                        {
                            continue;
                        }

                        if (ContentComparerHelper.TryParseXmlDocument(item.SiteMapXml, out var doc))
                        {
                            _connectionData.SiteMapIntellisenseData.LoadDataFromSiteMap(doc);
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
                            if (ContentComparerHelper.TryParseXmlDocument(organization.ReferenceSiteMapXml, out var doc))
                            {
                                _connectionData.SiteMapIntellisenseData.LoadDataFromSiteMap(doc);
                            }
                        }

                        if (!string.IsNullOrEmpty(organization.SiteMapXml))
                        {
                            if (ContentComparerHelper.TryParseXmlDocument(organization.SiteMapXml, out var doc))
                            {
                                _connectionData.SiteMapIntellisenseData.LoadDataFromSiteMap(doc);
                            }
                        }
                    }
                }

                {
                    var repository = new SystemFormRepository(service);

                    var listSystemForms = await repository.GetListByTypeAsync((int)SystemForm.Schema.OptionSets.type.Dashboard_0
                        , new ColumnSet
                        (
                            SystemForm.Schema.EntityPrimaryIdAttribute
                            , SystemForm.Schema.Attributes.objecttypecode
                            , SystemForm.Schema.Attributes.name
                            , SystemForm.Schema.Attributes.description
                        )
                    );

                    _connectionData.SiteMapIntellisenseData.LoadDashboards(listSystemForms);
                }

                _connectionData.SiteMapIntellisenseData.Save();

                this._nextLoadFileDate = DateTime.Now.AddMinutes(_loadPeriodInMinutes);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }
            finally
            {
                lock (_syncObjectTaskGettingSiteMapInformation)
                {
                    _taskGettingSiteMapInformation = null;
                }
            }
        }

        static SiteMapIntellisenseDataRepository()
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

        public static SiteMapIntellisenseDataRepository GetRepository(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return null;
            }

            if (!_staticCacheRepositories.ContainsKey(connectionData.ConnectionId))
            {
                var repository = new SiteMapIntellisenseDataRepository(connectionData);

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

        ~SiteMapIntellisenseDataRepository()
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