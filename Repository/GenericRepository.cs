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
            return Task.Run(async () => await GetEntityById(idEntity, columnSet));
        }

        private async Task<Entity> GetEntityById(Guid idEntity, ColumnSet columnSet)
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
                        new ConditionExpression(_entityMetadata.PrimaryIdAttribute, ConditionOperator.Equal, idEntity),
                    },
                },
            };

            try
            {
                return _service.RetrieveMultiple(query).Entities.SingleOrDefault();
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);

                return null;
            }
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

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<Entity>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Entity>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }

            return result;
        }
    }
}
