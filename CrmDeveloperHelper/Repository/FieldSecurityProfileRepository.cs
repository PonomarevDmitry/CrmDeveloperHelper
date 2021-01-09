using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class FieldSecurityProfileRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public FieldSecurityProfileRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<FieldSecurityProfile>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        private List<FieldSecurityProfile> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = FieldSecurityProfile.EntityLogicalName,

                NoLock = true,

                ColumnSet = ColumnSetInstances.AllColumns,

                Orders =
                {
                    new OrderExpression(FieldSecurityProfile.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<FieldSecurityProfile>(query);
        }

        public Task<List<FieldSecurityProfile>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<FieldSecurityProfile> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = FieldSecurityProfile.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(FieldSecurityProfile.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(FieldSecurityProfile.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<FieldSecurityProfile>(query);
        }
    }
}
