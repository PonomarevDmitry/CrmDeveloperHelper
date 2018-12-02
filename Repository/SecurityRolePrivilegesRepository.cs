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
    public class SecurityRolePrivilegesRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SecurityRolePrivilegesRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<Privilege>> GetListAsync(IEnumerable<Guid> roles)
        {
            return Task.Run(() => GetList(roles));
        }

        private List<Privilege> GetList(IEnumerable<Guid> roles)
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

                        EntityAlias = RolePrivileges.EntityLogicalName,

                        Columns = new ColumnSet(RolePrivileges.Schema.Attributes.roleid, RolePrivileges.Schema.Attributes.privilegedepthmask),

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(RolePrivileges.Schema.Attributes.roleid, ConditionOperator.In, roles.ToArray()),
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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<List<RolePrivileges>> GetEntitySecurityRolesAsync(IEnumerable<Guid> roles, IEnumerable<Guid> privileges)
        {
            return Task.Run(() => GetEntitySecurityRoles(roles, privileges));
        }

        private List<RolePrivileges> GetEntitySecurityRoles(IEnumerable<Guid> roles, IEnumerable<Guid> privileges)
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

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<RolePrivileges>()));

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
                    return "User";

                case 2:
                    return "Business Unit";

                case 4:
                    return "Parent: Child Business Units";

                case 8:
                    return "Organization";

                default:
                    return string.Format("{0} - Unknown", mask);
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
    }
}
