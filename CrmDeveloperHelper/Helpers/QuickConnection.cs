using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    /// <summary>
    /// Подключение к CRM
    /// </summary>
    public static class QuickConnection
    {
        private static ConcurrentDictionary<Uri, IServiceManagement<IDiscoveryService>> _cacheDiscoveryServiceManagement = new ConcurrentDictionary<Uri, IServiceManagement<IDiscoveryService>>();
        private static ConcurrentDictionary<Uri, IServiceManagement<IOrganizationService>> _cacheOrganizationServiceManagement = new ConcurrentDictionary<Uri, IServiceManagement<IOrganizationService>>();

        private static IServiceManagement<IDiscoveryService> GetDiscoveryServiceConfiguration(ConnectionData connectionData, Uri uri)
        {
            if (_cacheDiscoveryServiceManagement.ContainsKey(uri))
            {
                return _cacheDiscoveryServiceManagement[uri];
            }

            if (!UrlIsAvailable(uri))
            {
                return null;
            }

            try
            {
                var serviceManagement = ServiceConfigurationFactory.CreateManagement<IDiscoveryService>(uri);

                if (!_cacheDiscoveryServiceManagement.ContainsKey(uri))
                {
                    _cacheDiscoveryServiceManagement.TryAdd(uri, serviceManagement);
                }

                return serviceManagement;
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(connectionData, ex);
            }

            return null;
        }

        private static IServiceManagement<IOrganizationService> GetOrganizationServiceConfiguration(ConnectionData connectionData, Uri uri)
        {
            if (_cacheOrganizationServiceManagement.ContainsKey(uri))
            {
                return _cacheOrganizationServiceManagement[uri];
            }

            if (!UrlIsAvailable(uri))
            {
                return null;
            }

            try
            {
                var management = ServiceConfigurationFactory.CreateManagement<IOrganizationService>(uri);

                if (!_cacheOrganizationServiceManagement.ContainsKey(uri))
                {
                    _cacheOrganizationServiceManagement.TryAdd(uri, management);
                }

                return management;
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(connectionData, ex);
            }

            return null;
        }

        public static bool UrlIsAvailable(Uri uri)
        {
            try
            {
                var request = WebRequest.Create(uri) as HttpWebRequest;
                request.Timeout = 5000;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    int statusCode = (int)response.StatusCode;
                    if (statusCode >= 100 && statusCode < 400)
                    {
                        return true;
                    }
                    else if (statusCode >= 500 && statusCode <= 510)
                    {
                        return false;
                    }
                }
            }
            catch (WebException ex)
            {
                DTEHelper.WriteExceptionToLog(ex);

                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }

            return false;
        }

        private const int _hoursOrganizationInformation = 20;

        public static Task<IOrganizationServiceExtented> ConnectAsync(ConnectionData connectionData)
        {
            return Task.Run(async () => await Connect(connectionData));
        }

        private static async Task<IOrganizationServiceExtented> Connect(ConnectionData connectionData)
        {
            bool withDiscoveryRequest = !connectionData.OrganizationInformationExpirationDate.HasValue || connectionData.OrganizationInformationExpirationDate.Value < DateTime.Now;

            try
            {
                var service = ConnectInternal(
                    connectionData
                    , withDiscoveryRequest
                    , out OrganizationDetail organizationDetail
                );

                if (service != null)
                {
                    var result = new OrganizationServiceExtentedProxy(service, connectionData);

                    await LoadOrganizationDataAsync(result, organizationDetail);

                    return result;
                }

                connectionData.OrganizationInformationExpirationDate = null;
                return null;
            }
            catch (Exception)
            {
                connectionData.OrganizationInformationExpirationDate = null;
                throw;
            }
        }

        public static Task<bool> TestConnectAsync(ConnectionData connectionData, IWriteToOutput iWriteToOutput)
        {
            return Task.Run(async () => await TestConnect(connectionData, iWriteToOutput));
        }

        private static async Task<bool> TestConnect(ConnectionData connectionData, IWriteToOutput iWriteToOutput)
        {
            iWriteToOutput.ActivateOutputWindow(connectionData);

            iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            try
            {
                var service = ConnectInternal(
                    connectionData
                    , true
                    , out OrganizationDetail organizationDetail
                );

                if (service != null)
                {
                    {
                        WhoAmIResponse whoresponse = (WhoAmIResponse)service.Execute(new WhoAmIRequest());

                        iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WhoAmIRequestExecutedSuccessfully);

                        iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.QuickConnectionOrganizationIdFormat1, whoresponse.OrganizationId);
                        iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.QuickConnectionBusinessUnitIdFormat1, whoresponse.BusinessUnitId);
                        iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.QuickConnectionUserIdFormat1, whoresponse.UserId);
                    }

                    var result = new OrganizationServiceExtentedProxy(service, connectionData);

                    await LoadOrganizationDataAsync(result, organizationDetail);

                    iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, result.CurrentServiceEndpoint);

                    iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SuccessfullyConnectedFormat1, connectionData.Name);

                    return true;
                }
                else
                {
                    iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);

                    return false;
                }
            }
            catch (Exception ex)
            {
                iWriteToOutput.WriteErrorToOutput(connectionData, ex);

                iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);

                return false;
            }
        }

        private static async Task LoadOrganizationDataAsync(OrganizationServiceExtentedProxy service, OrganizationDetail organizationDetail)
        {
            try
            {
                Guid? idOrganization = null;

                if (organizationDetail != null)
                {
                    idOrganization = organizationDetail.OrganizationId;

                    service.ConnectionData.OrganizationInformationExpirationDate = DateTime.Now.AddHours(_hoursOrganizationInformation);

                    service.ConnectionData.FriendlyName = organizationDetail.FriendlyName;
                    service.ConnectionData.OrganizationId = organizationDetail.OrganizationId;
                    service.ConnectionData.OrganizationVersion = organizationDetail.OrganizationVersion;
                    service.ConnectionData.OrganizationState = organizationDetail.State.ToString();
                    service.ConnectionData.UniqueOrgName = organizationDetail.UniqueName;
                    service.ConnectionData.UrlName = organizationDetail.UrlName;

                    if (organizationDetail.Endpoints.ContainsKey(EndpointType.OrganizationService))
                    {
                        var organizationUrlEndpoint = organizationDetail.Endpoints[EndpointType.OrganizationService];

                        if (string.IsNullOrEmpty(service.ConnectionData.OrganizationUrl)
                            && !string.IsNullOrEmpty(organizationUrlEndpoint)
                            )
                        {
                            service.ConnectionData.OrganizationUrl = organizationUrlEndpoint;
                        }
                    }

                    if (organizationDetail.Endpoints.ContainsKey(EndpointType.WebApplication))
                    {
                        var publicUrl = organizationDetail.Endpoints[EndpointType.WebApplication];

                        if (string.IsNullOrEmpty(service.ConnectionData.PublicUrl)
                            && !string.IsNullOrEmpty(publicUrl)
                            )
                        {
                            service.ConnectionData.PublicUrl = publicUrl;
                        }
                    }
                }

                if (!idOrganization.HasValue)
                {
                    WhoAmIResponse whoresponse = await service.ExecuteAsync<WhoAmIResponse>(new WhoAmIRequest());

                    idOrganization = whoresponse.OrganizationId;
                }

                service.ConnectionData.DefaultLanguage = string.Empty;
                service.ConnectionData.BaseCurrency = string.Empty;
                service.ConnectionData.DefaultLanguage = string.Empty;
                service.ConnectionData.InstalledLanguagePacks = string.Empty;

                if (idOrganization.HasValue)
                {
                    var organization = await service
                        .RetrieveAsync<Organization>(Organization.EntityLogicalName, idOrganization.Value, new ColumnSet(Organization.Schema.Attributes.languagecode, Organization.Schema.Attributes.basecurrencyid))
                        ;

                    if (organization.BaseCurrencyId != null)
                    {
                        service.ConnectionData.BaseCurrency = organization.BaseCurrencyId.Name;
                    }

                    var request = new RetrieveInstalledLanguagePacksRequest();
                    var response = await service.ExecuteAsync<RetrieveInstalledLanguagePacksResponse>(request);

                    var rep = new EntityMetadataRepository(service);

                    var isEntityExists = rep.IsEntityExists(LanguageLocale.EntityLogicalName);

                    if (isEntityExists)
                    {
                        var repository = new LanguageLocaleRepository(service);

                        if (organization.LanguageCode.HasValue)
                        {
                            var lang = (await repository.GetListAsync(organization.LanguageCode.Value)).FirstOrDefault();

                            if (lang != null)
                            {
                                service.ConnectionData.DefaultLanguage = lang.ToString();
                            }
                            else
                            {
                                service.ConnectionData.DefaultLanguage = LanguageLocale.GetLocaleName(organization.LanguageCode.Value);
                            }
                        }

                        if (response.RetrieveInstalledLanguagePacks != null && response.RetrieveInstalledLanguagePacks.Any())
                        {
                            var list = await repository.GetListAsync(response.RetrieveInstalledLanguagePacks);

                            service.ConnectionData.InstalledLanguagePacks = string.Join(",", list.OrderBy(s => s.LocaleId.Value, new LocaleComparer()).Select(l => l.ToString()));
                        }
                    }
                    else
                    {
                        if (organization.LanguageCode.HasValue)
                        {
                            service.ConnectionData.DefaultLanguage = LanguageLocale.GetLocaleName(organization.LanguageCode.Value);
                        }

                        if (response.RetrieveInstalledLanguagePacks != null && response.RetrieveInstalledLanguagePacks.Any())
                        {
                            service.ConnectionData.InstalledLanguagePacks = string.Join(",", response.RetrieveInstalledLanguagePacks.OrderBy(s => s, new LocaleComparer()).Select(l => LanguageLocale.GetLocaleName(l)));
                        }
                    }
                }

                if (string.IsNullOrEmpty(service.ConnectionData.PublicUrl)
                    && !string.IsNullOrEmpty(service.ConnectionData.OrganizationUrl)
                    )
                {
                    var orgUrl = service.ConnectionData.OrganizationUrl.TrimEnd('/');

                    if (orgUrl.EndsWith("/XRMServices/2011/Organization.svc", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var lastIndex = orgUrl.LastIndexOf("/XRMServices/2011/Organization.svc", StringComparison.InvariantCultureIgnoreCase);

                        var publicUrl = orgUrl.Substring(0, lastIndex + 1).TrimEnd('/');

                        service.ConnectionData.PublicUrl = publicUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
            }
        }

        private static OrganizationServiceProxy ConnectInternal(
            ConnectionData connectionData
            , bool withDiscoveryRequest
            , out OrganizationDetail organizationDetail
            )
        {
            organizationDetail = null;

            Uri orgUri = null;

            if ((withDiscoveryRequest || string.IsNullOrEmpty(connectionData.OrganizationUrl))
                && !string.IsNullOrEmpty(connectionData.DiscoveryUrl)
                && Uri.TryCreate(connectionData.DiscoveryUrl, UriKind.Absolute, out var discoveryUri)
                )
            {
                var disco = CreateDiscoveryService(connectionData, discoveryUri, connectionData.User?.Username, connectionData.User?.Password);

                if (disco != null)
                {
                    using (disco)
                    {
                        var repository = new DiscoveryServiceRepository(disco);

                        var orgs = repository.DiscoverOrganizations();

                        if (orgs.Count == 1)
                        {
                            organizationDetail = orgs[0];
                        }
                        else if (orgs.Count > 0)
                        {
                            organizationDetail = orgs
                                .FirstOrDefault(a => string.Equals(a.UniqueName, connectionData.UniqueOrgName, StringComparison.InvariantCultureIgnoreCase));

                            if (organizationDetail == null)
                            {

                            }
                        }

                        if (organizationDetail != null && organizationDetail.Endpoints.ContainsKey(EndpointType.OrganizationService))
                        {
                            orgUri = new Uri(organizationDetail.Endpoints[EndpointType.OrganizationService]);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(connectionData.OrganizationUrl)
                && Uri.TryCreate(connectionData.OrganizationUrl, UriKind.RelativeOrAbsolute, out Uri custromOrgUri)
            )
            {
                orgUri = custromOrgUri;
            }

            if (orgUri != null)
            {
                var serviceManagement = GetOrganizationServiceConfiguration(connectionData, orgUri);

                if (serviceManagement != null)
                {
                    var credentials = GetCredentials(serviceManagement, connectionData.User?.Username, connectionData.User?.Password);

                    OrganizationServiceProxy service = null;

                    if (serviceManagement.AuthenticationType != AuthenticationProviderType.ActiveDirectory
                           && serviceManagement.AuthenticationType != AuthenticationProviderType.None
                    )
                    {
                        AuthenticationCredentials tokenCredentials = serviceManagement.Authenticate(credentials);

                        service = new OrganizationServiceProxy(serviceManagement, tokenCredentials.SecurityTokenResponse);
                    }
                    else
                    {
                        service = new OrganizationServiceProxy(serviceManagement, credentials.ClientCredentials);
                    }

                    service.EnableProxyTypes();
                    service.Timeout = TimeSpan.FromMinutes(30);

                    return service;
                }
            }

            return null;
        }

        private static DiscoveryServiceProxy CreateDiscoveryService(ConnectionData connectionData, Uri discoveryUrl, string username, string password)
        {
            try
            {
                var serviceManagement = GetDiscoveryServiceConfiguration(connectionData, discoveryUrl);

                if (serviceManagement != null)
                {
                    return CreateDiscoveryService(serviceManagement, username, password);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }

            return null;
        }

        public static DiscoveryServiceProxy CreateDiscoveryService(IServiceManagement<IDiscoveryService> serviceManagement, string username, string password)
        {
            DiscoveryServiceProxy service = null;

            var credentials = GetCredentials(serviceManagement, username, password);

            if (serviceManagement.AuthenticationType != AuthenticationProviderType.ActiveDirectory
                && serviceManagement.AuthenticationType != AuthenticationProviderType.None
            )
            {
                AuthenticationCredentials tokenCredentials = serviceManagement.Authenticate(credentials);

                service = new DiscoveryServiceProxy(serviceManagement, tokenCredentials.SecurityTokenResponse);
            }
            else
            {
                service = new DiscoveryServiceProxy(serviceManagement, credentials.ClientCredentials);
            }

            service.Timeout = TimeSpan.FromMinutes(30);

            return service;
        }

        /// <summary>
        /// получение всех организаций
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private static OrganizationDetailCollection DiscoverOrganizations(IDiscoveryService service)
        {
            RetrieveOrganizationsRequest request = new RetrieveOrganizationsRequest();
            RetrieveOrganizationsResponse response = (RetrieveOrganizationsResponse)service.Execute(request);

            return response.Details;
        }

        private static AuthenticationCredentials GetCredentials<T>(IServiceManagement<T> serviceManagement, string username, string password)
        {
            var credentials = new AuthenticationCredentials();

            switch (serviceManagement.AuthenticationType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    {
                        var nc = CredentialCache.DefaultNetworkCredentials;

                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                        {
                            var domainAndUserName = username.Split('\\');

                            if (domainAndUserName.Length == 2)
                            {
                                nc = new NetworkCredential(domainAndUserName[1], password, domainAndUserName[0]);
                            }
                            else if (domainAndUserName.Length == 1)
                            {
                                nc = new NetworkCredential(username, password);
                            }
                        }

                        credentials.ClientCredentials.Windows.ClientCredential = nc;
                    }
                    break;

                case AuthenticationProviderType.LiveId:
                    {
                        credentials.ClientCredentials.UserName.UserName = username;
                        credentials.ClientCredentials.UserName.Password = password;

                        credentials.SupportingCredentials = new AuthenticationCredentials
                        {
                            ClientCredentials = Microsoft.Crm.Services.Utility.DeviceIdManager.LoadOrRegisterDevice()
                        };
                    }
                    break;

                case AuthenticationProviderType.Federation:
                case AuthenticationProviderType.OnlineFederation:
                    {
                        credentials.ClientCredentials.UserName.UserName = username;
                        credentials.ClientCredentials.UserName.Password = password;

                        if (serviceManagement.AuthenticationType == AuthenticationProviderType.OnlineFederation)
                        {
                            IdentityProvider provider = serviceManagement.GetIdentityProvider(credentials.ClientCredentials.UserName.UserName);
                            if (provider != null && provider.IdentityProviderType == IdentityProviderType.LiveId)
                            {
                                credentials.SupportingCredentials = new AuthenticationCredentials
                                {
                                    ClientCredentials = Microsoft.Crm.Services.Utility.DeviceIdManager.LoadOrRegisterDevice()
                                };
                            }
                        }
                    }
                    break;

                case AuthenticationProviderType.None:
                    break;
            }

            return credentials;
        }
    }
}