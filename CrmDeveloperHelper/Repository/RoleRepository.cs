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

        public Task<IEnumerable<Role>> GetListAsync(string filterRole, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filterRole, null, null, null, columnSet));
        }

        public Task<IEnumerable<Role>> GetListAsync(string filterRole, bool? isCustomizable, bool? isManaged, bool? isTemplate, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filterRole, isCustomizable, isManaged, isTemplate, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private IEnumerable<Role> GetList(string filterRole, bool? isCustomizable, bool? isManaged, bool? isTemplate, ColumnSet columnSet)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentroleid, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roletemplateid,

                        LinkToEntityName = RoleTemplate.Schema.EntityLogicalName,
                        LinkToAttributeName = RoleTemplate.Schema.EntityPrimaryIdAttribute,

                        EntityAlias = Role.Schema.Attributes.roletemplateid,

                        Columns = new ColumnSet(RoleTemplate.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.businessunitid,

                        LinkToEntityName = BusinessUnit.EntityLogicalName,
                        LinkToAttributeName = BusinessUnit.EntityPrimaryIdAttribute,

                        EntityAlias = BusinessUnit.EntityLogicalName,

                        Columns = new ColumnSet(BusinessUnit.Schema.Attributes.parentbusinessunitid),
                    },
                },

                Orders =
                {
                    new OrderExpression(Role.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filterRole))
            {
                if (Guid.TryParse(filterRole, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roleid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.name, ConditionOperator.Like, "%" + filterRole + "%"));
                }
            }

            if (isManaged.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.ismanaged, ConditionOperator.Equal, isManaged.Value));
            }

            if (isCustomizable.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.iscustomizable, ConditionOperator.Equal, isCustomizable.Value));
            }

            if (isTemplate.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roletemplateid, isTemplate.Value ? ConditionOperator.NotNull : ConditionOperator.Null));
            }

            return _service.RetrieveMultipleAll<Role>(query);
        }

        public Task<List<Role>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<Role> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.roleid, ConditionOperator.In, ids.ToArray())
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roletemplateid,

                        LinkToEntityName = RoleTemplate.Schema.EntityLogicalName,
                        LinkToAttributeName = RoleTemplate.Schema.EntityPrimaryIdAttribute,

                        EntityAlias = Role.Schema.Attributes.roletemplateid,

                        Columns = new ColumnSet(RoleTemplate.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.businessunitid,

                        LinkToEntityName = BusinessUnit.EntityLogicalName,
                        LinkToAttributeName = BusinessUnit.EntityPrimaryIdAttribute,

                        EntityAlias = BusinessUnit.EntityLogicalName,

                        Columns = new ColumnSet(BusinessUnit.Schema.Attributes.parentbusinessunitid),
                    },
                },

                Orders =
                {
                    new OrderExpression(Role.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<Role>(query);
        }

        public Role FindRoleByTemplate(Guid roleTemplateId, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentroleid, ConditionOperator.Null),
                        new ConditionExpression(Role.Schema.Attributes.roletemplateid, ConditionOperator.Equal, roleTemplateId),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<Role>()).SingleOrDefault() : null;
        }

        public Task<List<Role>> GetUserRolesAsync(Guid idUser, string filterRole, bool? isCustomizable, bool? isManaged, bool? isTemplate, ColumnSet columnSet)
        {
            return Task.Run(() => GetUserRoles(idUser, filterRole, isCustomizable, isManaged, isTemplate, columnSet));
        }

        public List<Role> GetUserRoles(Guid idUser, string filterRole, bool? isCustomizable, bool? isManaged, bool? isTemplate, ColumnSet columnSet)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentroleid, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roletemplateid,

                        LinkToEntityName = RoleTemplate.Schema.EntityLogicalName,
                        LinkToAttributeName = RoleTemplate.Schema.EntityPrimaryIdAttribute,

                        EntityAlias = Role.Schema.Attributes.roletemplateid,

                        Columns = new ColumnSet(RoleTemplate.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roleid,

                        LinkToEntityName = Role.Schema.EntityLogicalName,
                        LinkToAttributeName = Role.Schema.Attributes.parentrootroleid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = Role.EntityLogicalName,
                                LinkFromAttributeName = Role.Schema.Attributes.roleid,

                                LinkToEntityName = SystemUserRoles.Schema.EntityLogicalName,
                                LinkToAttributeName = SystemUserRoles.Schema.Attributes.roleid,

                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(SystemUserRoles.Schema.Attributes.systemuserid, ConditionOperator.Equal, idUser),
                                    },
                                },
                            },
                        },
                    },
                },
            };

            if (!string.IsNullOrEmpty(filterRole))
            {
                if (Guid.TryParse(filterRole, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roleid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.name, ConditionOperator.Like, "%" + filterRole + "%"));
                }
            }

            if (isManaged.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.ismanaged, ConditionOperator.Equal, isManaged.Value));
            }

            if (isCustomizable.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.iscustomizable, ConditionOperator.Equal, isCustomizable.Value));
            }

            if (isTemplate.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roletemplateid, isTemplate.Value ? ConditionOperator.NotNull : ConditionOperator.Null));
            }

            return _service.RetrieveMultipleAll<Role>(query);
        }

        public Task<List<Role>> GetTeamRolesAsync(Guid idTeam, string filterRole, bool? isCustomizable, bool? isManaged, bool? isTemplate, ColumnSet columnSet)
        {
            return Task.Run(() => GetTeamRoles(idTeam, filterRole, isCustomizable, isManaged, isTemplate, columnSet));
        }

        public List<Role> GetTeamRoles(Guid idTeam, string filterRole, bool? isCustomizable, bool? isManaged, bool? isTemplate, ColumnSet columnSet)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentroleid, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roletemplateid,

                        LinkToEntityName = RoleTemplate.Schema.EntityLogicalName,
                        LinkToAttributeName = RoleTemplate.Schema.EntityPrimaryIdAttribute,

                        EntityAlias = Role.Schema.Attributes.roletemplateid,

                        Columns = new ColumnSet(RoleTemplate.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roleid,

                        LinkToEntityName = Role.Schema.EntityLogicalName,
                        LinkToAttributeName = Role.Schema.Attributes.parentrootroleid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = Role.EntityLogicalName,
                                LinkFromAttributeName = Role.Schema.Attributes.roleid,

                                LinkToEntityName = TeamRoles.Schema.EntityLogicalName,
                                LinkToAttributeName = TeamRoles.Schema.Attributes.roleid,

                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(TeamRoles.Schema.Attributes.teamid, ConditionOperator.Equal, idTeam),
                                    },
                                },
                            },
                        },
                    },
                },
            };

            if (!string.IsNullOrEmpty(filterRole))
            {
                if (Guid.TryParse(filterRole, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roleid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.name, ConditionOperator.Like, "%" + filterRole + "%"));
                }
            }

            if (isManaged.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.ismanaged, ConditionOperator.Equal, isManaged.Value));
            }

            if (isCustomizable.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.iscustomizable, ConditionOperator.Equal, isCustomizable.Value));
            }

            if (isTemplate.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roletemplateid, isTemplate.Value ? ConditionOperator.NotNull : ConditionOperator.Null));
            }

            return _service.RetrieveMultipleAll<Role>(query);
        }

        public Task<List<Role>> GetUserRolesByTeamsAsync(Guid idUser, string filterRole, bool? isCustomizable, bool? isManaged, bool? isTemplate, ColumnSet columnSet)
        {
            return Task.Run(() => GetUserRolesByTeams(idUser, filterRole, isCustomizable, isManaged, isTemplate, columnSet));
        }

        private List<Role> GetUserRolesByTeams(Guid idUser, string filterRole, bool? isCustomizable, bool? isManaged, bool? isTemplate, ColumnSet columnSet)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentroleid, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roletemplateid,

                        LinkToEntityName = RoleTemplate.Schema.EntityLogicalName,
                        LinkToAttributeName = RoleTemplate.Schema.EntityPrimaryIdAttribute,

                        EntityAlias = Role.Schema.Attributes.roletemplateid,

                        Columns = new ColumnSet(RoleTemplate.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roleid,

                        LinkToEntityName = Role.Schema.EntityLogicalName,
                        LinkToAttributeName = Role.Schema.Attributes.parentrootroleid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = Role.EntityLogicalName,
                                LinkFromAttributeName = Role.Schema.Attributes.roleid,

                                LinkToEntityName = TeamRoles.Schema.EntityLogicalName,
                                LinkToAttributeName = TeamRoles.Schema.Attributes.roleid,

                                LinkEntities =
                                {
                                    new LinkEntity()
                                    {
                                        LinkFromEntityName = TeamRoles.Schema.EntityLogicalName,
                                        LinkFromAttributeName = TeamRoles.Schema.Attributes.teamid,

                                        LinkToEntityName = Team.Schema.EntityLogicalName,
                                        LinkToAttributeName = Team.Schema.Attributes.teamid,

                                        EntityAlias = Team.Schema.Attributes.teamid,

                                        Columns = new ColumnSet(Team.Schema.Attributes.teamid, Team.Schema.Attributes.name, Team.Schema.Attributes.businessunitid, Team.Schema.Attributes.isdefault),

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
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };

            if (!string.IsNullOrEmpty(filterRole))
            {
                if (Guid.TryParse(filterRole, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roleid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.name, ConditionOperator.Like, "%" + filterRole + "%"));
                }
            }

            if (isManaged.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.ismanaged, ConditionOperator.Equal, isManaged.Value));
            }

            if (isCustomizable.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.iscustomizable, ConditionOperator.Equal, isCustomizable.Value));
            }

            if (isTemplate.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roletemplateid, isTemplate.Value ? ConditionOperator.NotNull : ConditionOperator.Null));
            }

            return _service.RetrieveMultipleAll<Role>(query);
        }

        public Task<IEnumerable<Role>> GetAvailableRolesForUserAsync(string filterRole, Guid idUser, ColumnSet columnSet)
        {
            return Task.Run(() => GetAvailableRolesForUser(filterRole, idUser, columnSet));
        }

        private IEnumerable<Role> GetAvailableRolesForUser(string filterRole, Guid idUser, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentroleid, ConditionOperator.Null),
                        new ConditionExpression(SystemUserRoles.Schema.EntityLogicalName, SystemUserRoles.Schema.Attributes.systemuserroleid, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roletemplateid,

                        LinkToEntityName = RoleTemplate.Schema.EntityLogicalName,
                        LinkToAttributeName = RoleTemplate.Schema.EntityPrimaryIdAttribute,

                        EntityAlias = Role.Schema.Attributes.roletemplateid,

                        Columns = new ColumnSet(RoleTemplate.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roleid,

                        LinkToEntityName = Role.Schema.EntityLogicalName,
                        LinkToAttributeName = Role.Schema.Attributes.parentrootroleid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = Role.EntityLogicalName,
                                LinkFromAttributeName = Role.Schema.Attributes.businessunitid,

                                LinkToEntityName = SystemUser.Schema.EntityLogicalName,
                                LinkToAttributeName = SystemUser.Schema.Attributes.businessunitid,

                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(SystemUser.Schema.Attributes.systemuserid, ConditionOperator.Equal, idUser),
                                    },
                                },
                            },

                            new LinkEntity()
                            {
                                JoinOperator = JoinOperator.LeftOuter,

                                LinkFromEntityName = Role.EntityLogicalName,
                                LinkFromAttributeName = Role.Schema.Attributes.roleid,

                                LinkToEntityName = SystemUserRoles.Schema.EntityLogicalName,
                                LinkToAttributeName = SystemUserRoles.Schema.Attributes.roleid,

                                EntityAlias = SystemUserRoles.Schema.EntityLogicalName,

                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(SystemUserRoles.Schema.Attributes.systemuserid, ConditionOperator.Equal, idUser),
                                    },
                                },
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(Role.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filterRole))
            {
                if (Guid.TryParse(filterRole, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roleid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.name, ConditionOperator.Like, "%" + filterRole + "%"));
                }
            }

            return _service.RetrieveMultipleAll<Role>(query);
        }

        public Task<IEnumerable<Role>> GetAvailableRolesForTeamAsync(string filterRole, Guid idTeam, ColumnSet columnSet)
        {
            return Task.Run(() => GetAvailableRolesForTeam(filterRole, idTeam, columnSet));
        }

        private IEnumerable<Role> GetAvailableRolesForTeam(string filterRole, Guid idTeam, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentroleid, ConditionOperator.Null),
                        new ConditionExpression(TeamRoles.Schema.EntityLogicalName, TeamRoles.Schema.Attributes.teamroleid, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roletemplateid,

                        LinkToEntityName = RoleTemplate.Schema.EntityLogicalName,
                        LinkToAttributeName = RoleTemplate.Schema.EntityPrimaryIdAttribute,

                        EntityAlias = Role.Schema.Attributes.roletemplateid,

                        Columns = new ColumnSet(RoleTemplate.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roleid,

                        LinkToEntityName = Role.Schema.EntityLogicalName,
                        LinkToAttributeName = Role.Schema.Attributes.parentrootroleid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = Role.EntityLogicalName,
                                LinkFromAttributeName = Role.Schema.Attributes.businessunitid,

                                LinkToEntityName = Team.Schema.EntityLogicalName,
                                LinkToAttributeName = Team.Schema.Attributes.businessunitid,

                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(Team.Schema.Attributes.teamid, ConditionOperator.Equal, idTeam),
                                    },
                                },
                            },

                            new LinkEntity()
                            {
                                JoinOperator = JoinOperator.LeftOuter,

                                LinkFromEntityName = Role.EntityLogicalName,
                                LinkFromAttributeName = Role.Schema.Attributes.roleid,

                                LinkToEntityName = TeamRoles.Schema.EntityLogicalName,
                                LinkToAttributeName = TeamRoles.Schema.Attributes.roleid,

                                EntityAlias = TeamRoles.Schema.EntityLogicalName,

                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(TeamRoles.Schema.Attributes.teamid, ConditionOperator.Equal, idTeam),
                                    },
                                },
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(Role.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filterRole))
            {
                if (Guid.TryParse(filterRole, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roleid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.name, ConditionOperator.Like, "%" + filterRole + "%"));
                }
            }

            return _service.RetrieveMultipleAll<Role>(query);
        }

        public Task<IEnumerable<Role>> GetRolesForNotAnotherAsync(string filterRole, Guid idRole, ColumnSet columnSet)
        {
            return Task.Run(() => GetRolesForNotAnother(filterRole, idRole, columnSet));
        }

        private IEnumerable<Role> GetRolesForNotAnother(string filterRole, Guid idRole, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentroleid, ConditionOperator.Null),
                        new ConditionExpression(Role.Schema.Attributes.roleid, ConditionOperator.NotEqual, idRole),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.roletemplateid,

                        LinkToEntityName = RoleTemplate.Schema.EntityLogicalName,
                        LinkToAttributeName = RoleTemplate.Schema.EntityPrimaryIdAttribute,

                        EntityAlias = Role.Schema.Attributes.roletemplateid,

                        Columns = new ColumnSet(RoleTemplate.Schema.Attributes.name),
                    },
                },

                Orders =
                {
                    new OrderExpression(Role.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filterRole))
            {
                if (Guid.TryParse(filterRole, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.roleid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Role.Schema.Attributes.name, ConditionOperator.Like, "%" + filterRole + "%"));
                }
            }

            return _service.RetrieveMultipleAll<Role>(query);
        }
    }
}