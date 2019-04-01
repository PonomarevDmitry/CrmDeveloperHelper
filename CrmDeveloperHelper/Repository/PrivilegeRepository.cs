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
                NoLock = true,

                EntityName = Privilege.EntityLogicalName,

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

            FillLinkedEntities(result);

            return result;
        }

        private void FillLinkedEntities(IEnumerable<Privilege> result)
        {
            var privilegesObjectTypes = GetPrivilegeObjectTypeCodes();

            var dictObjectTypes = privilegesObjectTypes.GroupBy(o => o.PrivilegeId.Id).ToDictionary(g => g.Key, g => g.Select(o => o.ObjectTypeCode).OrderBy(s => s).ToList());

            foreach (var item in result)
            {
                if (dictObjectTypes.ContainsKey(item.Id))
                {
                    item.LinkedEntities = dictObjectTypes[item.Id];
                }
            }
        }

        private List<PrivilegeObjectTypeCodes> GetPrivilegeObjectTypeCodes()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = PrivilegeObjectTypeCodes.EntityLogicalName,

                ColumnSet = new ColumnSet(PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid, PrivilegeObjectTypeCodes.Schema.Attributes.objecttypecode),

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            var result = new List<PrivilegeObjectTypeCodes>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<PrivilegeObjectTypeCodes>()));

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

        public Task<List<Privilege>> GetListOtherPrivilegeAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetListOtherPrivilege(columnSet));
        }

        private List<Privilege> GetListOtherPrivilege(ColumnSet columnSet)
        {
            var result = new List<Privilege>();

            FillPrivilegesWithNoneAccessType(columnSet, result);

            var hashPrivilegeIds = new HashSet<Guid>();

            FillPrivilegesWithNoneEntity(columnSet, result, hashPrivilegeIds);

            return result;
        }

        private void FillPrivilegesWithNoneAccessType(ColumnSet columnSet, List<Privilege> result)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Privilege.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    FilterOperator = LogicalOperator.Or,

                    Conditions =
                    {
                        new ConditionExpression(Privilege.Schema.Attributes.accessright, ConditionOperator.Equal, (int)AccessRights.None),
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

            var temp = new List<Privilege>();

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

            FillLinkedEntities(result);
        }

        private void FillPrivilegesWithNoneEntity(ColumnSet columnSet, List<Privilege> result, HashSet<Guid> hashPrivilegeIds)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Privilege.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Privilege.EntityLogicalName,
                        LinkFromAttributeName = Privilege.PrimaryIdAttribute,

                        LinkToEntityName = PrivilegeObjectTypeCodes.EntityLogicalName,
                        LinkToAttributeName = PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid,

                        EntityAlias = PrivilegeObjectTypeCodes.EntityLogicalName,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(PrivilegeObjectTypeCodes.EntityLogicalName, PrivilegeObjectTypeCodes.Schema.Attributes.objecttypecode, ConditionOperator.Equal, "none"),
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

            var temp = new List<Privilege>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    temp.AddRange(coll.Entities.Select(e => e.ToEntity<Privilege>()));

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

            foreach (var item in temp)
            {
                if (hashPrivilegeIds.Add(item.Id))
                {
                    item.LinkedEntities = new List<string>() { "none" };

                    result.Add(item);
                }
            }
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

                        LinkToEntityName = PrivilegeObjectTypeCodes.EntityLogicalName,
                        LinkToAttributeName = PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid,

                        JoinOperator = JoinOperator.LeftOuter,

                        EntityAlias = PrivilegeObjectTypeCodes.EntityLogicalName,

                        Columns = new ColumnSet(PrivilegeObjectTypeCodes.Schema.Attributes.objecttypecode),
                    },

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
