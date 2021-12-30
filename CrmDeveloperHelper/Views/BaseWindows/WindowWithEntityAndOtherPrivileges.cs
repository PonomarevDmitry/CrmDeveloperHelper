using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowWithEntityAndOtherPrivileges : WindowWithSolutionComponentDescriptor
    {
        private readonly Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();
        private readonly Dictionary<Guid, IEnumerable<Privilege>> _cachePrivileges = new Dictionary<Guid, IEnumerable<Privilege>>();

        protected readonly IEnumerable<PrivilegeType> _privielgeTypesAll = Enum.GetValues(typeof(PrivilegeType)).Cast<PrivilegeType>().ToList();

        protected WindowWithEntityAndOtherPrivileges(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
        ) : base(iWriteToOutput, commonConfig, service)
        {
        }

        protected WindowWithEntityAndOtherPrivileges(
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

                var temp = await repository.GetEntitiesDisplayNameWithPrivilegesAsync();

                _cacheEntityMetadata.Add(service.ConnectionData.ConnectionId, temp);
            }

            return _cacheEntityMetadata[service.ConnectionData.ConnectionId];
        }

        protected IEnumerable<Privilege> GetOtherPrivilegesList(Guid connectionId)
        {
            if (_cachePrivileges.ContainsKey(connectionId))
            {
                return _cachePrivileges[connectionId];
            }

            return null;
        }

        protected async Task<IEnumerable<Privilege>> GetOtherPrivilegesEnumerable(IOrganizationServiceExtented service)
        {
            if (!_cachePrivileges.ContainsKey(service.ConnectionData.ConnectionId))
            {
                var repository = new PrivilegeRepository(service);

                var temp = await repository.GetListOtherPrivilegeAsync(new ColumnSet(
                    Privilege.Schema.Attributes.privilegeid
                    , Privilege.Schema.Attributes.name
                    , Privilege.Schema.Attributes.accessright

                    , Privilege.Schema.Attributes.canbebasic
                    , Privilege.Schema.Attributes.canbelocal
                    , Privilege.Schema.Attributes.canbedeep
                    , Privilege.Schema.Attributes.canbeglobal

                    , Privilege.Schema.Attributes.canbeentityreference
                    , Privilege.Schema.Attributes.canbeparententityreference
                ));

                _cachePrivileges.Add(service.ConnectionData.ConnectionId, temp);
            }

            return _cachePrivileges[service.ConnectionData.ConnectionId];
        }

        protected void RemoveEntityMetadataCache(Guid connectionId)
        {
            _cacheEntityMetadata.Remove(connectionId);
        }

        protected void RemoveOtherPrivilegeCache(Guid connectionId)
        {
            _cachePrivileges.Remove(connectionId);
        }

        protected void ClearEntityMetadataCache()
        {
            _cacheEntityMetadata.Clear();
        }

        protected void ClearOtherPrivilegeCache()
        {
            _cachePrivileges.Clear();
        }

        protected void StoreEntityMetadataCache(Guid connectionId, IEnumerable<EntityMetadata> entityMetadataList)
        {
            if (entityMetadataList != null && entityMetadataList.Any(e => e.Privileges != null && e.Privileges.Any()))
            {
                _cacheEntityMetadata[connectionId] = entityMetadataList;
            }
        }

        protected void StoreOtherPrivilegeCache(Guid connectionId, IEnumerable<Privilege> privileges)
        {
            if (privileges != null)
            {
                _cachePrivileges[connectionId] = privileges;
            }
        }
    }
}
