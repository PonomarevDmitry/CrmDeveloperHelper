using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class TeamTemplateRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public TeamTemplateRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<TeamTemplate>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        private List<TeamTemplate> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = TeamTemplate.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.None,

                Orders =
                {
                    new OrderExpression(TeamTemplate.Schema.Attributes.objecttypecode, OrderType.Ascending),
                    new OrderExpression(TeamTemplate.Schema.Attributes.teamtemplatename, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<TeamTemplate>(query);
        }

        public TeamTemplate FindByName(string name, ColumnSet columnSet)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 1,

                EntityName = TeamTemplate.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.None,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TeamTemplate.Schema.Attributes.teamtemplatename, ConditionOperator.Equal, name),
                    },
                },
            };

            var collection = _service.RetrieveMultiple(query);

            return collection.Entities.Select(e => e.ToEntity<TeamTemplate>()).FirstOrDefault();
        }
    }
}
