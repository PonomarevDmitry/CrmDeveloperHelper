using Microsoft.Crm.Sdk.Messages;
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
    public class PrivilegeRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public PrivilegeRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<Privilege>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        private List<Privilege> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = Privilege.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Orders =
                {
                    new OrderExpression(Privilege.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            var result = new List<Privilege>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Privilege>()));

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
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);
            }

            return result;
        }

        public Task<List<Privilege>> GetListWithEntityNameAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetListWithEntityName(columnSet));
        }

        private List<Privilege> GetListWithEntityName(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = Privilege.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    FilterOperator = LogicalOperator.Or,

                    Conditions =
                    {
                        new ConditionExpression(Privilege.Schema.Attributes.accessright, ConditionOperator.Equal, (int)AccessRights.None),
                        new ConditionExpression(PrivilegeObjectTypeCodes.EntityLogicalName, PrivilegeObjectTypeCodes.Schema.Attributes.objecttypecode, ConditionOperator.Equal, "none"),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Privilege.EntityLogicalName,
                        LinkFromAttributeName = Privilege.PrimaryIdAttribute,

                        LinkToEntityName = PrivilegeObjectTypeCodes.EntityLogicalName,
                        LinkToAttributeName = PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid,

                        JoinOperator = JoinOperator.LeftOuter,

                        EntityAlias = PrivilegeObjectTypeCodes.EntityLogicalName,

                        Columns = new ColumnSet(PrivilegeObjectTypeCodes.Schema.Attributes.objecttypecode),
                    },
                },

                Orders =
                {
                    new OrderExpression(Privilege.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            var result = new List<Privilege>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Privilege>()));

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
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);
            }

            return result;
        }

        public Task<List<Privilege>> GetListForRoleAsync(Guid idRole)
        {
            return Task.Run(() => GetListForRole(idRole));
        }

        private List<Privilege> GetListForRole(Guid idRole)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = Privilege.EntityLogicalName,

                NoLock = true,

                ColumnSet = new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Privilege.EntityLogicalName,
                        LinkFromAttributeName = Privilege.PrimaryIdAttribute,

                        LinkToEntityName = RolePrivileges.EntityLogicalName,
                        LinkToAttributeName = RolePrivileges.Schema.Attributes.privilegeid,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(RolePrivileges.Schema.Attributes.roleid, ConditionOperator.Equal, idRole),
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(Privilege.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            var result = new List<Privilege>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Privilege>()));

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
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);
            }

            return result;
        }
    }
}
