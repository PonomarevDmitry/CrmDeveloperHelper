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
    public class SystemUserRepository
    {
        private IOrganizationServiceExtented _service;

        public SystemUserRepository(IOrganizationServiceExtented _service)
        {
            this._service = _service;
        }

        internal EntityReference FindUser(string fullname)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                ColumnSet = new ColumnSet(false),

                EntityName = SystemUser.EntityLogicalName,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUser.Schema.Attributes.fullname, ConditionOperator.Equal, fullname),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntityReference()).SingleOrDefault() : null;
        }

        public Task<List<SystemUser>> GetListAsync(string filter, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filter, columnSet));
        }

        private List<SystemUser> GetList(string filter, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SystemUser.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Orders =
                {
                    new OrderExpression(SystemUser.Schema.Attributes.fullname, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filter))
            {
                query.Criteria = new FilterExpression(LogicalOperator.Or)
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUser.Schema.Attributes.fullname, ConditionOperator.Like, "%" + filter + "%"),
                        new ConditionExpression(SystemUser.Schema.Attributes.domainname, ConditionOperator.Like, "%" + filter + "%"),
                    },
                };
            }

            var result = new List<SystemUser>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SystemUser>()));

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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<List<SystemUser>> GetUsersByTeamAsync(Guid idTeam, string filter, ColumnSet columnSet)
        {
            return Task.Run(() => GetUsersByTeam(idTeam, filter, columnSet));
        }

        private List<SystemUser> GetUsersByTeam(Guid idTeam, string filter, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SystemUser.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SystemUser.EntityLogicalName,
                        LinkFromAttributeName = SystemUser.PrimaryIdAttribute,

                        LinkToEntityName = TeamMembership.Schema.EntityLogicalName,
                        LinkToAttributeName = TeamMembership.Schema.Attributes.systemuserid,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(TeamMembership.Schema.Attributes.teamid, ConditionOperator.Equal, idTeam),
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(SystemUser.Schema.Attributes.fullname, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filter))
            {
                query.Criteria = new FilterExpression(LogicalOperator.Or)
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUser.Schema.Attributes.fullname, ConditionOperator.Like, "%" + filter + "%"),
                        new ConditionExpression(SystemUser.Schema.Attributes.domainname, ConditionOperator.Like, "%" + filter + "%"),
                    },
                };
            }

            var result = new List<SystemUser>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SystemUser>()));

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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<List<SystemUser>> GetUsersByRoleAsync(Guid idRole, string filter, ColumnSet columnSet)
        {
            return Task.Run(() => GetUsersByRole(idRole, filter, columnSet));
        }

        private List<SystemUser> GetUsersByRole(Guid idRole, string filter, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SystemUser.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SystemUser.EntityLogicalName,
                        LinkFromAttributeName = SystemUser.PrimaryIdAttribute,

                        LinkToEntityName = SystemUserRoles.Schema.EntityLogicalName,
                        LinkToAttributeName = SystemUserRoles.Schema.Attributes.systemuserid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SystemUserRoles.Schema.EntityLogicalName,
                                LinkFromAttributeName = SystemUserRoles.Schema.Attributes.roleid,

                                LinkToEntityName = Role.Schema.EntityLogicalName,
                                LinkToAttributeName = Role.Schema.Attributes.roleid,

                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(Role.Schema.Attributes.parentrootroleid, ConditionOperator.Equal, idRole),
                                    },
                                },
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(SystemUser.Schema.Attributes.fullname, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filter))
            {
                query.Criteria = new FilterExpression(LogicalOperator.Or)
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUser.Schema.Attributes.fullname, ConditionOperator.Like, "%" + filter + "%"),
                        new ConditionExpression(SystemUser.Schema.Attributes.domainname, ConditionOperator.Like, "%" + filter + "%"),
                    },
                };
            }

            var result = new List<SystemUser>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SystemUser>()));

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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }
    }
}