using Microsoft.Xrm.Sdk.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public OrganizationDetailCollection DiscoverOrganizations()
        {
            RetrieveOrganizationsRequest request = new RetrieveOrganizationsRequest();
            RetrieveOrganizationsResponse response = (RetrieveOrganizationsResponse)_service.Execute(request);

            return response.Details;
        }
    }
}
