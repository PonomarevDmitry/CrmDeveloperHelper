using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public class MetadataProviderService : IMetadataProviderService
    {
        private readonly Dictionary<string, EntityMetadata> _cache = new Dictionary<string, EntityMetadata>(StringComparer.InvariantCultureIgnoreCase);

        private readonly EntityMetadataRepository _repository;

        public MetadataProviderService(EntityMetadataRepository repository)
        {
            this._repository = repository;
        }

        public EntityMetadata GetEntityMetadata(string entityName)
        {
            if (_cache.ContainsKey(entityName))
            {
                return _cache[entityName];
            }

            var result = _repository.GetEntityMetadata(entityName);

            _cache[entityName] = result;

            return result;
        }

        public void StoreEntities(IEnumerable<string> entityList)
        {
            if (!entityList.Any())
            {
                return;
            }

            var list = _repository.GetEntityMetadataList(entityList);

            foreach (var item in list)
            {
                _cache[item.LogicalName] = item;
            }
        }
    }
}
