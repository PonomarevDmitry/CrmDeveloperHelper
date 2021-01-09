using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class SdkMessagePairRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessagePairRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessagePair>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        private List<SdkMessagePair> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessagePair.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,
            };

            return _service.RetrieveMultipleAll<SdkMessagePair>(query);
        }

        public Task<List<SdkMessagePair>> GetListByMessageAsync(Guid idMessage, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByRequest(idMessage, columnSet));
        }

        private List<SdkMessagePair> GetListByRequest(Guid idMessage, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessagePair.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessagePair.Schema.Attributes.sdkmessageid, ConditionOperator.Equal, idMessage),
                    },
                },
            };

            return _service.RetrieveMultipleAll<SdkMessagePair>(query);
        }

        public Task<SdkMessagePair> GetByIdAsync(Guid idSdkMessagePair, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSdkMessagePair, columnSet));
        }

        private SdkMessagePair GetById(Guid idSdkMessagePair, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessagePair.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessagePair.EntityPrimaryIdAttribute, ConditionOperator.Equal, idSdkMessagePair),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessagePair>()).SingleOrDefault() : null;
        }
    }
}
