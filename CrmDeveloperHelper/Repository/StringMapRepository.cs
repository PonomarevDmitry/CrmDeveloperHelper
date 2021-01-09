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
    public class StringMapRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public StringMapRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<StringMap>> GetListAsync(string entityName)
        {
            return Task.Run(() => GetList(entityName));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<StringMap> GetList(string entityName)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = StringMap.EntityLogicalName,

                ColumnSet = ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(StringMap.Schema.Attributes.displayorder, ConditionOperator.NotNull),
                    },
                },
            };

            if (!string.IsNullOrEmpty(entityName))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(StringMap.Schema.Attributes.objecttypecode, ConditionOperator.Equal, entityName));
            }

            return _service.RetrieveMultipleAll<StringMap>(query);
        }
    }
}
