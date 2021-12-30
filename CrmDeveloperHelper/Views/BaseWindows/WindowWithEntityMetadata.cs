using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowWithEntityMetadata : WindowWithSolutionComponentDescriptor
    {
        private readonly Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();

        protected WindowWithEntityMetadata(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
        ) : base(iWriteToOutput, commonConfig, service)
        {
        }

        protected WindowWithEntityMetadata(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connectionData
        ) : base(iWriteToOutput, commonConfig, connectionData)
        {
        }

        protected IEnumerable<EntityMetadata> GetEntityMetadataList(Guid connectionId)
        {
            if (_cacheEntityMetadata.ContainsKey(connectionId))
            {
                return _cacheEntityMetadata[connectionId];
            }

            return null;
        }

        protected async Task<IEnumerable<EntityMetadata>> GetEntityMetadataEnumerable(IOrganizationServiceExtented service)
        {
            if (!_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                var repository = new EntityMetadataRepository(service);

                var temp = await repository.GetEntitiesForEntityAttributeExplorerAsync(EntityFilters.Entity);

                _cacheEntityMetadata.Add(service.ConnectionData.ConnectionId, temp);
            }

            return _cacheEntityMetadata[service.ConnectionData.ConnectionId];
        }

        protected void RemoveEntityMetadataCache(Guid connectionId)
        {
            _cacheEntityMetadata.Remove(connectionId);
        }

        protected void ClearEntityMetadataCache()
        {
            _cacheEntityMetadata.Clear();
        }
    }
}
