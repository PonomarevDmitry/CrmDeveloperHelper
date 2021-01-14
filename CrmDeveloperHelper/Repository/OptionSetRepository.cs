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
            var request = new RetrieveAllOptionSetsRequest();

            var response = (RetrieveAllOptionSetsResponse)_service.Execute(request);

            return response
                .OptionSetMetadata
                .OfType<OptionSetMetadata>()
                .Where(e => e.Options.Any(o => o.Value.HasValue))
                .OrderBy(e => e.Name)
                .ToList();
        }

        public Task<List<OptionSetMetadata>> GetListByIdListAsync(IEnumerable<Guid> ids)
        {
            return Task.Run(() => GetListByIdList(ids));
        }

        private List<OptionSetMetadata> GetListByIdList(IEnumerable<Guid> ids)
        {
            var request = new RetrieveAllOptionSetsRequest();

            var response = (RetrieveAllOptionSetsResponse)_service.Execute(request);

            var hash = new HashSet<Guid>(ids);

            return response
                .OptionSetMetadata
                .OfType<OptionSetMetadata>()
                .Where(e => e.Options.Any(o => o.Value.HasValue) && hash.Contains(e.MetadataId.Value))
                .OrderBy(e => e.Name)
                .ToList();
        }

        public Task<OptionSetMetadata> GetOptionSetByNameAsync(string optionSetName)
        {
            return Task.Run(() => GetOptionSetByName(optionSetName));
        }

        private OptionSetMetadata GetOptionSetByName(string optionSetName)
        {
            var request = new RetrieveOptionSetRequest()
            {
                Name = optionSetName,
            };

            try
            {
                var response = (RetrieveOptionSetResponse)_service.Execute(request);

                if (response.OptionSetMetadata != null && response.OptionSetMetadata is OptionSetMetadata result)
                {
                    return result;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}
