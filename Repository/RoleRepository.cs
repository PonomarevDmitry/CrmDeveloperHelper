using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class RoleRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public RoleRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<Role>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<Role> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentroleid, ConditionOperator.Null),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<Role>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Role>()));

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
                Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Role FindRoleByTemplate(Guid roleTemplateId, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentroleid, ConditionOperator.Null),
                        new ConditionExpression(Role.Schema.Attributes.roletemplateid, ConditionOperator.Equal, roleTemplateId),
                    },
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<Role>()).SingleOrDefault();
        }
    }
}