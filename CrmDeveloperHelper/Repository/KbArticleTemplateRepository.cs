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
    public class KbArticleTemplateRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public KbArticleTemplateRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<KbArticleTemplate>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        private List<KbArticleTemplate> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = KbArticleTemplate.EntityLogicalName,

                NoLock = true,

                ColumnSet = ColumnSetInstances.AllColumns,

                Orders =
                {
                    new OrderExpression(KbArticleTemplate.Schema.Attributes.title, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<KbArticleTemplate>(query);
        }

        public Task<List<KbArticleTemplate>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<KbArticleTemplate> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = KbArticleTemplate.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(KbArticleTemplate.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(KbArticleTemplate.Schema.Attributes.title, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<KbArticleTemplate>(query);
        }
    }
}
