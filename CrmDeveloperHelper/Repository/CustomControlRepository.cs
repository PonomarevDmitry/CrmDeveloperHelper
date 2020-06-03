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
    public class CustomControlRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public CustomControlRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<CustomControl>> GetListAsync(string filter, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filter, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<CustomControl> GetList(string filter, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = CustomControl.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(CustomControl.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                    },
                },

                Orders =
                {
                    new OrderExpression(CustomControl.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(CustomControl.Schema.Attributes.compatibledatatypes, OrderType.Ascending),
                    new OrderExpression(CustomControl.Schema.Attributes.customcontrolid, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filter))
            {
                if (Guid.TryParse(filter, out Guid tempGuid))
                {
                    query.Criteria.Filters.Add(new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new ConditionExpression(CustomControl.Schema.Attributes.customcontrolid, ConditionOperator.Equal, tempGuid),
                            new ConditionExpression(CustomControl.Schema.Attributes.customcontrolidunique, ConditionOperator.Equal, tempGuid),
                        },
                    });
                }
                else
                {
                    query.Criteria.AddCondition(CustomControl.Schema.Attributes.name, ConditionOperator.Like, "%" + filter + "%");
                }
            }

            return _service.RetrieveMultipleAll<CustomControl>(query);
        }

        public Task<CustomControl> GetByIdAsync(Guid idCustomControl, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idCustomControl, columnSet));
        }

        private CustomControl GetById(Guid idCustomControl, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = CustomControl.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(CustomControl.EntityPrimaryIdAttribute, ConditionOperator.Equal, idCustomControl),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<CustomControl>()).SingleOrDefault() : null;
        }
    }
}
