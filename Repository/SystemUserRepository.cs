using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
                    new OrderExpression(SystemUser.Schema.Attributes.domainname, OrderType.Ascending),
                    new OrderExpression(SystemUser.Schema.Attributes.businessunitid, OrderType.Ascending),
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
                    query.Criteria.Conditions.Add(new ConditionExpression(SystemUser.Schema.Attributes.systemuserid, ConditionOperator.Equal, id));
                }
                else
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
                    new OrderExpression(SystemUser.Schema.Attributes.domainname, OrderType.Ascending),
                    new OrderExpression(SystemUser.Schema.Attributes.businessunitid, OrderType.Ascending),
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
                    query.Criteria.Conditions.Add(new ConditionExpression(SystemUser.Schema.Attributes.systemuserid, ConditionOperator.Equal, id));
                }
                else
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
                    new OrderExpression(SystemUser.Schema.Attributes.domainname, OrderType.Ascending),
                    new OrderExpression(SystemUser.Schema.Attributes.businessunitid, OrderType.Ascending),
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
                    query.Criteria.Conditions.Add(new ConditionExpression(SystemUser.Schema.Attributes.systemuserid, ConditionOperator.Equal, id));
                }
                else
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

        public Task<IEnumerable<SystemUser>> GetAvailableUsersForRoleAsync(string filterUser, Guid idRole, ColumnSet columnSet)
        {
            return Task.Run(() => GetAvailableUsersForRole(filterUser, idRole, columnSet));
        }

        private IEnumerable<SystemUser> GetAvailableUsersForRole(string filterUser, Guid idRole, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SystemUser.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUser.Schema.Attributes.isdisabled, ConditionOperator.Equal, false),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SystemUser.Schema.EntityLogicalName,
                        LinkFromAttributeName = SystemUser.Schema.Attributes.businessunitid,

                        LinkToEntityName = Role.EntityLogicalName,
                        LinkToAttributeName = Role.Schema.Attributes.businessunitid,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(Role.Schema.Attributes.parentrootroleid, ConditionOperator.Equal, idRole),
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(SystemUser.Schema.Attributes.fullname, OrderType.Ascending),
                    new OrderExpression(SystemUser.Schema.Attributes.businessunitid, OrderType.Ascending),
                    new OrderExpression(SystemUser.Schema.Attributes.domainname, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filterUser))
            {
                if (Guid.TryParse(filterUser, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(SystemUser.Schema.Attributes.systemuserid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.AddFilter(new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new ConditionExpression(SystemUser.Schema.Attributes.fullname, ConditionOperator.Like, "%" + filterUser + "%"),
                            new ConditionExpression(SystemUser.Schema.Attributes.domainname, ConditionOperator.Like, "%" + filterUser + "%"),
                        },
                    });
                }
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
                Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            {
                var users = GetUsersByRole(idRole, null, new ColumnSet(false));

                var hashSetUsersWithRole = new HashSet<Guid>(users.Select(t => t.Id));

                var usersToRemove = result.Where(t => hashSetUsersWithRole.Contains(t.Id)).ToList();

                foreach (var team in usersToRemove)
                {
                    result.Remove(team);
                }
            }

            return result;
        }

        public Task<IEnumerable<SystemUser>> GetAvailableUsersForTeamAsync(string filterUser, Guid idTeam, ColumnSet columnSet)
        {
            return Task.Run(() => GetAvailableUsersForTeam(filterUser, idTeam, columnSet));
        }

        private IEnumerable<SystemUser> GetAvailableUsersForTeam(string filterUser, Guid idTeam, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SystemUser.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUser.Schema.Attributes.isdisabled, ConditionOperator.Equal, false),
                        new ConditionExpression(TeamMembership.Schema.EntityLogicalName, TeamMembership.Schema.Attributes.teammembershipid, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SystemUser.Schema.EntityLogicalName,
                        LinkFromAttributeName = SystemUser.Schema.Attributes.systemuserid,

                        LinkToEntityName = TeamMembership.Schema.EntityLogicalName,
                        LinkToAttributeName = TeamMembership.Schema.Attributes.systemuserid,

                        EntityAlias = TeamMembership.Schema.EntityLogicalName,

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
                    new OrderExpression(SystemUser.Schema.Attributes.businessunitid, OrderType.Ascending),
                    new OrderExpression(SystemUser.Schema.Attributes.domainname, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filterUser))
            {
                if (Guid.TryParse(filterUser, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(SystemUser.Schema.Attributes.systemuserid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.AddFilter(new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new ConditionExpression(SystemUser.Schema.Attributes.fullname, ConditionOperator.Like, "%" + filterUser + "%"),
                            new ConditionExpression(SystemUser.Schema.Attributes.domainname, ConditionOperator.Like, "%" + filterUser + "%"),
                        },
                    });
                }
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
                Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public static IEnumerable<DataGridColumn> GetDataGridColumn()
        {
            //<DataGridTextColumn Header="Domain Name" Width="120" Binding="{Binding DomainName, Mode=OneTime}" />

            //<DataGridTextColumn Header="FullName" Width="260" Binding="{Binding FullName, Mode=OneTime}" />

            //<DataGridTextColumn Header="BusinessUnit" Width="120" Binding="{Binding BusinessUnitId.Name, Mode=OneTime}" />

            //<DataGridCheckBoxColumn Width="Auto" Binding="{Binding IsDisabled, Mode=OneTime}">
            //    <DataGridCheckBoxColumn.Header>
            //        <Label Content="Dis" Margin="0" Padding="0" ToolTip="IsDisabled" />
            //    </DataGridCheckBoxColumn.Header>
            //</DataGridCheckBoxColumn>

            return new List<DataGridColumn>()
            {
                new DataGridTextColumn()
                {
                    Header = "Domain Name",
                    Width = new DataGridLength(120),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("DomainName"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "FullName",
                    Width = new DataGridLength(260),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("FullName"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "BusinessUnit",
                    Width = new DataGridLength(120),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("BusinessUnitId.Name"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridCheckBoxColumn()
                {
                    Header = new System.Windows.Controls.Label()
                    {
                        Content = "Dis",
                        Padding = new Thickness(0),
                        Margin = new Thickness(0),
                        ToolTip = "IsDisabled",
                    },
                    Width = new DataGridLength(100, DataGridLengthUnitType.Auto),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("IsDisabled"),
                        Mode = BindingMode.OneTime,
                    },
                },
            };
        }

        public Task<List<SystemUser>> GetActiveUsersAsync(string filter, ColumnSet columnSet)
        {
            return Task.Run(() => GetActiveUsers(filter, columnSet));
        }

        private List<SystemUser> GetActiveUsers(string filter, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SystemUser.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUser.Schema.Attributes.isdisabled, ConditionOperator.Equal, false),
                    },
                },

                Orders =
                {
                    new OrderExpression(SystemUser.Schema.Attributes.fullname, OrderType.Ascending),
                    new OrderExpression(SystemUser.Schema.Attributes.domainname, OrderType.Ascending),
                    new OrderExpression(SystemUser.Schema.Attributes.businessunitid, OrderType.Ascending),
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
                    query.Criteria.Conditions.Add(new ConditionExpression(SystemUser.Schema.Attributes.systemuserid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Filters.Add(new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new ConditionExpression(SystemUser.Schema.Attributes.fullname, ConditionOperator.Like, "%" + filter + "%"),
                            new ConditionExpression(SystemUser.Schema.Attributes.domainname, ConditionOperator.Like, "%" + filter + "%"),
                        },
                    });
                }
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