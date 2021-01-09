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

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Orders =
                {
                    new OrderExpression(Privilege.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            var result = _service.RetrieveMultipleAll<Privilege>(query);

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
            };

            return _service.RetrieveMultipleAll<PrivilegeObjectTypeCodes>(query);
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

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Privilege.Schema.Attributes.accessright, ConditionOperator.Equal, (int)AccessRights.None),
                    },
                },

                Orders =
                {
                    new OrderExpression(Privilege.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            result.AddRange(_service.RetrieveMultipleAll<Privilege>(query));

            FillLinkedEntities(result);
        }

        private void FillPrivilegesWithNoneEntity(ColumnSet columnSet, List<Privilege> result, HashSet<Guid> hashPrivilegeIds)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Privilege.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Privilege.EntityLogicalName,
                        LinkFromAttributeName = Privilege.EntityPrimaryIdAttribute,

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
            };

            var temp = _service.RetrieveMultipleAll<Privilege>(query);

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

                ColumnSet = ColumnSetInstances.AllColumns,

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Privilege.EntityLogicalName,
                        LinkFromAttributeName = Privilege.EntityPrimaryIdAttribute,

                        LinkToEntityName = PrivilegeObjectTypeCodes.EntityLogicalName,
                        LinkToAttributeName = PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid,

                        JoinOperator = JoinOperator.LeftOuter,

                        EntityAlias = PrivilegeObjectTypeCodes.EntityLogicalName,

                        Columns = new ColumnSet(PrivilegeObjectTypeCodes.Schema.Attributes.objecttypecode),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = Privilege.EntityLogicalName,
                        LinkFromAttributeName = Privilege.EntityPrimaryIdAttribute,

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
            };

            var result = _service.RetrieveMultipleAll<Privilege>(query);

            FillLinkedEntities(result);

            return result;
        }

        public Task<List<Privilege>> GetListByIdsAsync(IEnumerable<Guid> idPrivilegeArr)
        {
            if (!idPrivilegeArr.Any())
            {
                return Task.FromResult(new List<Privilege>());
            }

            return Task.Run(() => GetListByIds(idPrivilegeArr));
        }

        private List<Privilege> GetListByIds(IEnumerable<Guid> idPrivilegeArr)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = Privilege.EntityLogicalName,

                ColumnSet = ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Privilege.Schema.Attributes.privilegeid, ConditionOperator.In, idPrivilegeArr.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(Privilege.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            var result = _service.RetrieveMultipleAll<Privilege>(query);

            FillLinkedEntities(result);

            return result;
        }
    }
}
