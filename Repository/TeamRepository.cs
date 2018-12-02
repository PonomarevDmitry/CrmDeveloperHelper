using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class TeamRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public TeamRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<Team>> GetListAsync(string filter, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filter, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<Team> GetList(string filter, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Team.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Team.Schema.EntityLogicalName,
                        LinkFromAttributeName = Team.Schema.Attributes.teamtemplateid,

                        LinkToEntityName = TeamTemplate.Schema.EntityLogicalName,
                        LinkToAttributeName = TeamTemplate.Schema.Attributes.teamtemplateid,

                        EntityAlias = Team.Schema.Attributes.teamtemplateid,

                        Columns = new ColumnSet(TeamTemplate.Schema.Attributes.teamtemplatename),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filter))
            {
                if (Guid.TryParse(filter, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Team.Schema.Attributes.teamid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Team.Schema.Attributes.name, ConditionOperator.Like, "%" + filter + "%"));
                }
            }

            var result = new List<Team>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Team>()));

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

        public Task<List<Team>> GetUserTeamsAsync(Guid idUser, string filter, ColumnSet columnSet)
        {
            return Task.Run(() => GetUserTeams(idUser, filter, columnSet));
        }

        private List<Team> GetUserTeams(Guid idUser, string filter, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Team.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Team.Schema.EntityLogicalName,
                        LinkFromAttributeName = Team.Schema.Attributes.teamid,

                        LinkToEntityName = TeamMembership.Schema.EntityLogicalName,
                        LinkToAttributeName = TeamMembership.Schema.Attributes.teamid,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(TeamMembership.Schema.Attributes.systemuserid, ConditionOperator.Equal, idUser),
                            },
                        },
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Team.Schema.EntityLogicalName,
                        LinkFromAttributeName = Team.Schema.Attributes.teamtemplateid,

                        LinkToEntityName = TeamTemplate.Schema.EntityLogicalName,
                        LinkToAttributeName = TeamTemplate.Schema.Attributes.teamtemplateid,

                        EntityAlias = Team.Schema.Attributes.teamtemplateid,

                        Columns = new ColumnSet(TeamTemplate.Schema.Attributes.teamtemplatename),
                    },
                },

                Orders =
                {
                    new OrderExpression(Team.Schema.Attributes.teamtype, OrderType.Ascending),
                    new OrderExpression(Team.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filter))
            {
                if (Guid.TryParse(filter, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Team.Schema.Attributes.teamid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Team.Schema.Attributes.name, ConditionOperator.Like, "%" + filter + "%"));
                }
            }

            var result = new List<Team>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Team>()));

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

        public Task<List<Team>> GetTeamsByRoleAsync(Guid idRole, string filter, ColumnSet columnSet)
        {
            return Task.Run(() => GetTeamsByRole(idRole, filter, columnSet));
        }

        private List<Team> GetTeamsByRole(Guid idRole, string filter, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Team.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Team.EntityLogicalName,
                        LinkFromAttributeName = Team.PrimaryIdAttribute,

                        LinkToEntityName = TeamRoles.Schema.EntityLogicalName,
                        LinkToAttributeName = TeamRoles.Schema.Attributes.teamid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = TeamRoles.Schema.EntityLogicalName,
                                LinkFromAttributeName = TeamRoles.Schema.Attributes.roleid,

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
                if (Guid.TryParse(filter, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Team.Schema.Attributes.teamid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Team.Schema.Attributes.name, ConditionOperator.Like, "%" + filter + "%"));
                }
            }

            var result = new List<Team>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Team>()));

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
