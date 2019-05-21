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
    public class RolePrivilegesRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public RolePrivilegesRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<RolePrivileges>> GetListAsync(IEnumerable<Guid> roles)
        {
            if (!roles.Any())
            {
                return Task.FromResult(new List<RolePrivileges>());
            }

            return Task.Run(() => GetList(roles));
        }

        private List<RolePrivileges> GetList(IEnumerable<Guid> roles)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = RolePrivileges.EntityLogicalName,

                NoLock = true,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RolePrivileges.Schema.Attributes.roleid, ConditionOperator.In, roles.ToArray()),
                    },
                },
            };

            return _service.RetrieveMultipleAll<RolePrivileges>(query);
        }

        public Task<List<RolePrivileges>> GetEntityPrivilegesAsync(IEnumerable<Guid> roles, IEnumerable<Guid> privileges)
        {
            return Task.Run(() => GetEntityPrivileges(roles, privileges));
        }

        private List<RolePrivileges> GetEntityPrivileges(IEnumerable<Guid> roles, IEnumerable<Guid> privileges)
        {
            var result = new List<RolePrivileges>();

            if (roles == null || !roles.Any())
            {
                return result;
            }

            if (privileges == null || !privileges.Any())
            {
                return result;
            }

            QueryExpression query = new QueryExpression()
            {
                EntityName = RolePrivileges.EntityLogicalName,

                NoLock = true,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RolePrivileges.Schema.Attributes.roleid, ConditionOperator.In, roles.ToArray()),
                        new ConditionExpression(RolePrivileges.Schema.Attributes.privilegeid, ConditionOperator.In, privileges.ToArray()),
                    },
                },
            };

            return _service.RetrieveMultipleAll<RolePrivileges>(query);
        }

        public static string GetPrivilegeDepthMaskName(int mask)
        {
            //Value Scope
            //1 Basic (User)
            //2 Local (Business Unit)
            //4 Deep (Parent: Child Business Units)
            //8 Global (Organization)

            switch (mask)
            {
                case 1:
                    return "Basic (User)";

                case 2:
                    return "Local (Business Unit)";

                case 4:
                    return "Deep (Parent: Child Business Units)";

                case 8:
                    return "Global (Organization)";

                default:
                    return string.Format("{0} - Unknown", mask);
            }
        }

        public static string GetPrivilegeDepthMaskName(PrivilegeDepth depth)
        {
            switch (depth)
            {
                case PrivilegeDepth.Basic:
                    return "Basic (User)";

                case PrivilegeDepth.Local:
                    return "Local (Business Unit)";

                case PrivilegeDepth.Deep:
                    return "Deep (Parent: Child Business Units)";

                case PrivilegeDepth.Global:
                    return "Global (Organization)";

                default:
                    return string.Empty;
            }
        }

        public static PrivilegeDepth? ConvertMaskToPrivilegeDepth(int value)
        {
            switch (value)
            {
                case 1:
                    return PrivilegeDepth.Basic;

                case 2:
                    return PrivilegeDepth.Local;

                case 4:
                    return PrivilegeDepth.Deep;

                case 8:
                    return PrivilegeDepth.Global;
            }

            return null;
        }

        public Task<IEnumerable<RolePrivilege>> GetUserPrivilegesAsync(Guid idUser)
        {
            return Task.Run(() => GetUserPrivileges(idUser));
        }

        private IEnumerable<RolePrivilege> GetUserPrivileges(Guid idUser)
        {
            var request = new RetrieveUserPrivilegesRequest()
            {
                UserId = idUser,
            };

            var response = (RetrieveUserPrivilegesResponse)_service.Execute(request);

            return response.RolePrivileges;
        }

        public Task<IEnumerable<RolePrivilege>> GetTeamPrivilegesAsync(Guid idTeam)
        {
            return Task.Run(() => GetTeamPrivileges(idTeam));
        }

        private IEnumerable<RolePrivilege> GetTeamPrivileges(Guid idTeam)
        {
            var request = new RetrieveTeamPrivilegesRequest()
            {
                TeamId = idTeam,
            };

            var response = (RetrieveTeamPrivilegesResponse)_service.Execute(request);

            return response.RolePrivileges;
        }

        public Task<IEnumerable<RolePrivilege>> GetRolePrivilegesAsync(Guid idRole)
        {
            return Task.Run(() => GetRolePrivileges(idRole));
        }

        private IEnumerable<RolePrivilege> GetRolePrivileges(Guid idRole)
        {
            var request = new RetrieveRolePrivilegesRoleRequest()
            {
                RoleId = idRole,
            };

            var response = (RetrieveRolePrivilegesRoleResponse)_service.Execute(request);

            return response.RolePrivileges;
        }

        public Task RemoveRolesFromUserAsync(Guid idUser, IEnumerable<Guid> idRoles)
        {
            return Task.Run(() => RemoveRolesFromUser(idUser, idRoles));
        }

        private void RemoveRolesFromUser(Guid idUser, IEnumerable<Guid> idRoles)
        {
            if (idRoles == null || !idRoles.Any())
            {
                return;
            }

            var roleArray = idRoles.ToArray();

            var roles = GetAssociatedRolesForUser(idUser, roleArray);

            if (roles.Any())
            {
                _service.Disassociate(SystemUser.EntityLogicalName, idUser, new Relationship(SystemUser.Schema.ManyToMany.systemuserroles_association.Name), new EntityReferenceCollection(roles));
            }
        }

        private IList<EntityReference> GetAssociatedRolesForUser(Guid idUser, Guid[] parentRolesArray)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = new ColumnSet(false),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentrootroleid, ConditionOperator.In, parentRolesArray),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.EntityPrimaryIdAttribute,

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
            };

            var result = new List<EntityReference>(_service.RetrieveMultipleAll<Role>(query).Select(e => e.ToEntityReference()));

            return result;
        }

        public Task RemoveRolesFromTeamAsync(Guid idTeam, IEnumerable<Guid> idRoles)
        {
            return Task.Run(() => RemoveRolesFromTeam(idTeam, idRoles));
        }

        private void RemoveRolesFromTeam(Guid idTeam, IEnumerable<Guid> idRoles)
        {
            if (idRoles == null || !idRoles.Any())
            {
                return;
            }

            var roleArray = idRoles.ToArray();

            var roles = GetAssociatedRolesForTeam(idTeam, roleArray);

            if (roles.Any())
            {
                _service.Disassociate(Team.EntityLogicalName, idTeam, new Relationship(Team.Schema.ManyToMany.teamroles_association.Name), new EntityReferenceCollection(roles));
            }
        }

        private IList<EntityReference> GetAssociatedRolesForTeam(Guid idTeam, Guid[] parentRolesArray)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = new ColumnSet(false),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentrootroleid, ConditionOperator.In, parentRolesArray),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.EntityPrimaryIdAttribute,

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

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            var result = new List<EntityReference>(_service.RetrieveMultipleAll<Role>(query).Select(e => e.ToEntityReference()));

            return result;
        }

        public Task AssignRolesToUserAsync(Guid idUser, IEnumerable<Guid> idRoles)
        {
            return Task.Run(() => AssignRolesToUser(idUser, idRoles));
        }

        private void AssignRolesToUser(Guid idUser, IEnumerable<Guid> idRoles)
        {
            if (idRoles == null || !idRoles.Any())
            {
                return;
            }

            var roleArray = idRoles.ToArray();

            var roles = GetRolesForUserBusinessUnit(idUser, roleArray);

            if (roles.Any())
            {
                _service.Associate(SystemUser.EntityLogicalName, idUser, new Relationship(SystemUser.Schema.ManyToMany.systemuserroles_association.Name), new EntityReferenceCollection(roles));
            }
        }

        private IList<EntityReference> GetRolesForUserBusinessUnit(Guid idUser, Guid[] parentRolesArray)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = new ColumnSet(false),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentrootroleid, ConditionOperator.In, parentRolesArray),
                        new ConditionExpression(SystemUserRoles.Schema.EntityLogicalName, SystemUserRoles.Schema.Attributes.systemuserroleid, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.EntityPrimaryIdAttribute,

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
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            var result = new List<EntityReference>(_service.RetrieveMultipleAll<Role>(query).Select(e => e.ToEntityReference()));

            return result;
        }

        public Task AssignRolesToTeamAsync(Guid idTeam, IEnumerable<Guid> idRoles)
        {
            return Task.Run(() => AssignRolesToTeam(idTeam, idRoles));
        }

        private void AssignRolesToTeam(Guid idTeam, IEnumerable<Guid> idRoles)
        {
            if (idRoles == null || !idRoles.Any())
            {
                return;
            }

            var roleArray = idRoles.ToArray();

            var roles = GetRolesForTeamBusinessUnit(idTeam, roleArray);

            if (roles.Any())
            {
                _service.Associate(Team.EntityLogicalName, idTeam, new Relationship(Team.Schema.ManyToMany.teamroles_association.Name), new EntityReferenceCollection(roles));
            }
        }

        private IList<EntityReference> GetRolesForTeamBusinessUnit(Guid idTeam, Guid[] parentRolesArray)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = new ColumnSet(false),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Schema.Attributes.parentrootroleid, ConditionOperator.In, parentRolesArray),
                        new ConditionExpression(TeamRoles.Schema.EntityLogicalName, TeamRoles.Schema.Attributes.teamroleid, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.EntityPrimaryIdAttribute,

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
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            var result = new List<EntityReference>(_service.RetrieveMultipleAll<Role>(query).Select(e => e.ToEntityReference()));

            return result;
        }

        public Task ModifyRolePrivilegesAsync(Guid idRole
            , IEnumerable<RolePrivilege> privilegesAdd
            , IEnumerable<RolePrivilege> privilegesRemove
            )
        {
            return Task.Run(() => ModifyRolePrivileges(idRole, privilegesAdd, privilegesRemove));
        }

        private void ModifyRolePrivileges(Guid idRole
            , IEnumerable<RolePrivilege> privilegesAdd
            , IEnumerable<RolePrivilege> privilegesRemove
            )
        {
            if (privilegesAdd != null && privilegesAdd.Any())
            {
                AddPrivilegesRoleRequest request = new AddPrivilegesRoleRequest()
                {
                    RoleId = idRole,
                    Privileges = privilegesAdd.ToArray(),
                };

                _service.Execute(request);
            }

            if (privilegesRemove != null && privilegesRemove.Any())
            {

                foreach (var priv in privilegesRemove)
                {
                    RemovePrivilegeRoleRequest request = new RemovePrivilegeRoleRequest()
                    {
                        RoleId = idRole,
                        PrivilegeId = priv.PrivilegeId
                    };

                    _service.Execute(request);
                }
            }
        }

        public Task ReplaceRolePrivilegesAsync(Guid idRole, IEnumerable<RolePrivilege> privileges)
        {
            return Task.Run(() => ReplaceRolePrivilege(idRole, privileges));
        }

        private void ReplaceRolePrivilege(Guid idRole, IEnumerable<RolePrivilege> privileges)
        {
            if (privileges != null)
            {
                ReplacePrivilegesRoleRequest request = new ReplacePrivilegesRoleRequest()
                {
                    RoleId = idRole,
                    Privileges = privileges.ToArray(),
                };

                _service.Execute(request);
            }
        }
    }
}