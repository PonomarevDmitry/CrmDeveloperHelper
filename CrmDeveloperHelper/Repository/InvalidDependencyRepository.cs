using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class InvalidDependencyRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public InvalidDependencyRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<InvalidDependency>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        private List<InvalidDependency> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = InvalidDependency.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,
            };

            return _service.RetrieveMultipleAll<InvalidDependency>(query);
        }

        public Task<List<InvalidDependency>> GetDistinctListAsync()
        {
            return Task.Run(() => GetDistinctList());
        }

        private List<InvalidDependency> GetDistinctList()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = InvalidDependency.EntityLogicalName,

                ColumnSet = new ColumnSet
                (
                    InvalidDependency.Schema.Attributes.existingcomponentid
                    , InvalidDependency.Schema.Attributes.existingcomponenttype
                    , InvalidDependency.Schema.Attributes.missingcomponentid
                    , InvalidDependency.Schema.Attributes.missingcomponenttype
                ),
            };

            return _service.RetrieveMultipleAll<InvalidDependency>(query);
        }
    }
}