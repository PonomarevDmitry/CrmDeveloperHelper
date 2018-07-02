using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class OptionSetRepository
    {
        private IOrganizationServiceExtented _service;

        public OptionSetRepository(IOrganizationServiceExtented service)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<OptionSetMetadata>> GetOptionSetsAsync()
        {
            return Task.Run(() => GetOptionSets());
        }

        private List<OptionSetMetadata> GetOptionSets()
        {
            RetrieveAllOptionSetsRequest request = new RetrieveAllOptionSetsRequest();

            RetrieveAllOptionSetsResponse retrieve = (RetrieveAllOptionSetsResponse)_service.Execute(request);

            return retrieve
                .OptionSetMetadata
                .OfType<OptionSetMetadata>()
                .Where(e => e.Options.Any(o => o.Value.HasValue))
                .OrderBy(e => e.Name)
                .ToList();
        }
    }
}
