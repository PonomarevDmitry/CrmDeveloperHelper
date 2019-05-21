using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class GenericRepository
    {
        private readonly IOrganizationServiceExtented _service;
        private readonly EntityMetadata _entityMetadata;

        public GenericRepository(IOrganizationServiceExtented service, EntityMetadata entityMetadata)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
            this._entityMetadata = entityMetadata;
        }
        public Task<Entity> GetEntityByIdAsync(Guid idEntity, ColumnSet columnSet)
        {
            if (string.IsNullOrEmpty(_entityMetadata.PrimaryIdAttribute))
            {
                return Task.FromResult<Entity>(null);
            }

            return Task.Run(async () => await GetEntityById(idEntity, columnSet));
        }

        private async Task<Entity> GetEntityById(Guid idEntity, ColumnSet columnSet)
        {
            var repository = new SdkMessageFilterRepository(_service);

            var messageFilter = await repository.FindByEntityAndMessageAsync(_entityMetadata.LogicalName, SdkMessage.Instances.RetrieveMultiple, new ColumnSet(false));

            if (messageFilter != null)
            {
                var query = new QueryExpression()
                {
                    NoLock = true,

                    TopCount = 2,

                    EntityName = _entityMetadata.LogicalName,

                    ColumnSet = columnSet,

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(_entityMetadata.PrimaryIdAttribute, ConditionOperator.Equal, idEntity),
                        },
                    },
                };

                try
                {
                    var coll = _service.RetrieveMultiple(query);

                    if (coll.Entities.Count == 1)
                    {
                        return coll.Entities.First();
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToLog(ex);
                }
            }

            messageFilter = await repository.FindByEntityAndMessageAsync(_entityMetadata.LogicalName, SdkMessage.Instances.Retrieve, new ColumnSet(false));

            if (messageFilter != null)
            {
                try
                {
                    var result = _service.Retrieve(_entityMetadata.LogicalName, idEntity, columnSet);

                    return result;
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToLog(ex);
                }
            }

            return null;
        }

        public Task<List<Entity>> GetEntitiesByFieldAsync(string fieldName, Guid idEntity, ColumnSet columnSet)
        {
            return Task.Run(async () => await GetEntityById(fieldName, idEntity, columnSet));
        }

        private async Task<List<Entity>> GetEntityById(string fieldName, Guid idEntity, ColumnSet columnSet)
        {
            {
                var repository = new SdkMessageFilterRepository(_service);

                var messageFilter = await repository.FindByEntityAndMessageAsync(_entityMetadata.LogicalName, SdkMessage.Instances.RetrieveMultiple, new ColumnSet(false));

                if (messageFilter == null)
                {
                    return null;
                }
            }

            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = _entityMetadata.LogicalName,

                ColumnSet = columnSet,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(fieldName, ConditionOperator.Equal, idEntity),
                    },
                },
            };

            return _service.RetrieveMultipleAll<Entity>(query);
        }

        public Task<Entity> GetEntityByNameFieldAsync(string name, ColumnSet columnSet)
        {
            if (string.IsNullOrEmpty(_entityMetadata.PrimaryNameAttribute))
            {
                return Task.FromResult<Entity>(null);
            }

            return Task.Run(async () => await GetEntityByNameField(name, columnSet));
        }

        private async Task<Entity> GetEntityByNameField(string name, ColumnSet columnSet)
        {
            {
                var repository = new SdkMessageFilterRepository(_service);

                var messageFilter = await repository.FindByEntityAndMessageAsync(_entityMetadata.LogicalName, SdkMessage.Instances.RetrieveMultiple, new ColumnSet(false));

                if (messageFilter == null)
                {
                    return null;
                }
            }

            var query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = _entityMetadata.LogicalName,

                ColumnSet = columnSet,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(_entityMetadata.PrimaryNameAttribute, ConditionOperator.Equal, name),
                    },
                },
            };

            try
            {
                var coll = _service.RetrieveMultiple(query);

                if (coll.Entities.Count == 1)
                {
                    return coll.Entities.First();
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }

            return null;
        }
    }
}
