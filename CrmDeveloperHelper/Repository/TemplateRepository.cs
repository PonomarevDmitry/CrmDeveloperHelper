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
    public class TemplateRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public TemplateRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<Template>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        private List<Template> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Template.EntityLogicalName,

                ColumnSet = ColumnSetInstances.AllColumns,

                Orders =
                {
                    new OrderExpression(Template.Schema.Attributes.templatetypecode, OrderType.Ascending),
                    new OrderExpression(Template.Schema.Attributes.title, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<Template>(query);
        }

        public Task<List<Template>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<Template> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Template.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Template.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(Template.Schema.Attributes.templatetypecode, OrderType.Ascending),
                    new OrderExpression(Template.Schema.Attributes.title, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<Template>(query);
        }
    }
}
