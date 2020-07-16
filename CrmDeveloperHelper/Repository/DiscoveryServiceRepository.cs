using Microsoft.Xrm.Sdk.Discovery;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class DiscoveryServiceRepository
    {
        private readonly IDiscoveryService _service;

        public DiscoveryServiceRepository(IDiscoveryService service)
        {
            this._service = service;
        }

        public Task<OrganizationDetailCollection> DiscoverOrganizationsAsync()
        {
            return Task.Run(() => DiscoverOrganizations());
        }

        private OrganizationDetailCollection DiscoverOrganizations()
        {
            var request = new RetrieveOrganizationsRequest();
            var response = (RetrieveOrganizationsResponse)_service.Execute(request);

            return response.Details;
        }
    }
}