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

        public void RetrieveEntities(IEnumerable<string> entityList)
        {
            var filteredEntities = entityList.Where(n => !_cache.ContainsKey(n)).ToList();

            if (!filteredEntities.Any())
            {
                return;
            }

            var list = _repository.GetEntityMetadataList(filteredEntities);

            foreach (var item in list)
            {
                _cache[item.LogicalName] = item;
            }
        }

        public void StoreEntityMetadata(params EntityMetadata[] entityMetadataList)
        {
            StoreEntityMetadataInternal(entityMetadataList);
        }

        public void StoreEntityMetadata(IEnumerable<EntityMetadata> entityMetadataList)
        {
            StoreEntityMetadataInternal(entityMetadataList);
        }

        private void StoreEntityMetadataInternal(IEnumerable<EntityMetadata> entityMetadataList)
        {
            if (!entityMetadataList.Any())
            {
                return;
            }

            foreach (var item in entityMetadataList)
            {
                if (!_cache.ContainsKey(item.LogicalName))
                {
                    _cache[item.LogicalName] = item;
                }
            }
        }
    }
}
