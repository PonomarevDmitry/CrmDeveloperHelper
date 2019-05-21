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
    public class OrganizationRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public OrganizationRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<Organization>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<Organization> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = Organization.EntityLogicalName,
                NoLock = true,
                ColumnSet = new ColumnSet(true),
            };

            return _service.RetrieveMultipleAll<Organization>(query);
        }

        public Task<Organization> GetByIdAsync(Guid idOrganization, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idOrganization, columnSet));
        }

        private Organization GetById(Guid idOrganization, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = Organization.EntityLogicalName,

                NoLock = true,

                TopCount = 2,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Organization.EntityPrimaryIdAttribute, ConditionOperator.Equal, idOrganization),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<Organization>()).SingleOrDefault() : null;
        }
    }
}
