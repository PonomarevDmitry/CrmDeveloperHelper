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

        public static IEnumerable<DataGridColumn> GetDataGridColumnOwner()
        {
            //<DataGridTextColumn Header="Name" Width="200" Binding="{Binding Name, Mode=OneTime}" />
            //<DataGridCheckBoxColumn Width="Auto" Binding="{Binding IsDefault, Mode=OneTime}">
            //    <DataGridCheckBoxColumn.Header>
            //        <Label Content="D" Margin="0" Padding="0" ToolTip="IsDefault" />
            //    </DataGridCheckBoxColumn.Header>
            //</DataGridCheckBoxColumn>
            //<DataGridTextColumn Header="BusinessUnit" Width="200" Binding="{Binding BusinessUnitId.Name, Mode=OneTime}" />

            return new List<DataGridColumn>()
            {
                new DataGridTextColumn()
                {
                    Header = "Name",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("Name"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridCheckBoxColumn()
                {
                    Header = new System.Windows.Controls.Label()
                    {
                        Content = "D",
                        Padding = new Thickness(0),
                        Margin = new Thickness(0),
                        ToolTip = "IsDefault",
                    },
                    Width = new DataGridLength(100, DataGridLengthUnitType.Auto),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("IsDefault"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "BusinessUnit",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("BusinessUnitId.Name"),
                        Mode = BindingMode.OneTime,
                    },
                },
            };
        }

        public static IEnumerable<DataGridColumn> GetDataGridColumn()
        {
            //<DataGridTextColumn Header="Name" Width="200" Binding="{Binding Name, Mode=OneTime}" />
            //<DataGridTextColumn Header="Type" Width="200" Binding="{Binding TeamTypeName, Mode=OneTime}" />
            //<DataGridCheckBoxColumn Width="Auto" Binding="{Binding IsDefault, Mode=OneTime}">
            //    <DataGridCheckBoxColumn.Header>
            //        <Label Content="D" Margin="0" Padding="0" ToolTip="IsDefault" />
            //    </DataGridCheckBoxColumn.Header>
            //</DataGridCheckBoxColumn>
            //<DataGridTextColumn Header="BusinessUnit" Width="200" Binding="{Binding BusinessUnitId.Name, Mode=OneTime}" />
            //<DataGridTextColumn Header="TeamTemplate" Width="200" Binding="{Binding TeamTemplateName, Mode=OneTime}" />
            //<DataGridTextColumn Header="RegardingObject" Width="200" Binding="{Binding RegardingObjectId.LogicalName, Mode=OneTime}" />

            return new List<DataGridColumn>()
            {
                new DataGridTextColumn()
                {
                    Header = "Name",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("Name"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "Type",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("TeamTypeName"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridCheckBoxColumn()
                {
                    Header = new System.Windows.Controls.Label()
                    {
                        Content = "D",
                        Padding = new Thickness(0),
                        Margin = new Thickness(0),
                        ToolTip = "IsDefault",
                    },
                    Width = new DataGridLength(100, DataGridLengthUnitType.Auto),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("IsDefault"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "BusinessUnit",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("BusinessUnitId.Name"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "TeamTemplate",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("TeamTemplateName"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "RegardingObject",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("RegardingObjectId.LogicalName"),
                        Mode = BindingMode.OneTime,
                    },
                },
            };
        }

        public Task<IEnumerable<Entity>> GetAvailableTeamsForRoleAsync(string filterTeam, Guid idRole, ColumnSet columnSet)
        {
            return Task.Run(() => GetAvailableTeamsForRole(filterTeam, idRole, columnSet));
        }

        private IEnumerable<Entity> GetAvailableTeamsForRole(string filterTeam, Guid idRole, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Team.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Team.Schema.Attributes.teamtype, ConditionOperator.Equal, (int)Team.Schema.OptionSets.teamtype.Owner_0),
                        new ConditionExpression(Role.EntityLogicalName, Role.Schema.Attributes.roleid, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Team.Schema.EntityLogicalName,
                        LinkFromAttributeName = Team.Schema.Attributes.businessunitid,

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

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Team.Schema.EntityLogicalName,
                        LinkFromAttributeName = Team.Schema.Attributes.teamid,

                        LinkToEntityName = TeamRoles.Schema.EntityLogicalName,
                        LinkToAttributeName = TeamRoles.Schema.Attributes.teamid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                JoinOperator = JoinOperator.LeftOuter,

                                LinkFromEntityName = TeamRoles.Schema.EntityLogicalName,
                                LinkFromAttributeName = TeamRoles.Schema.Attributes.roleid,

                                LinkToEntityName = Role.EntityLogicalName,
                                LinkToAttributeName = Role.Schema.Attributes.roleid,

                                EntityAlias = Role.EntityLogicalName,

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
                    new OrderExpression(Team.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(Team.Schema.Attributes.businessunitid, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filterTeam))
            {
                if (Guid.TryParse(filterTeam, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Team.Schema.Attributes.teamid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Team.Schema.Attributes.name, ConditionOperator.Like, "%" + filterTeam + "%"));
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
    }
}
