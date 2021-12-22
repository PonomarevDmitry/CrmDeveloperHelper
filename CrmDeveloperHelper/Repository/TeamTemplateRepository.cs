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
    public class TeamTemplateRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public TeamTemplateRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<TeamTemplate>> GetListAsync(string filter, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filter, columnSet));
        }

        private List<TeamTemplate> GetList(string filter, ColumnSet columnSet)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = TeamTemplate.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.None,

                Orders =
                {
                    new OrderExpression(TeamTemplate.Schema.Attributes.objecttypecode, OrderType.Ascending),
                    new OrderExpression(TeamTemplate.Schema.Attributes.teamtemplatename, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filter))
            {
                if (Guid.TryParse(filter, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(TeamTemplate.Schema.Attributes.teamtemplateid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(TeamTemplate.Schema.Attributes.teamtemplatename, ConditionOperator.Like, "%" + filter + "%"));
                }
            }

            return _service.RetrieveMultipleAll<TeamTemplate>(query);
        }

        public Task<IEnumerable<TeamTemplate>> GetListForEntityAsync(int objectTypeCode, ColumnSet columnSet)
        {
            return Task.Run(() => GetListForEntity(string.Empty, objectTypeCode, columnSet));
        }

        public Task<IEnumerable<TeamTemplate>> GetListForEntityAsync(string filter, int objectTypeCode, ColumnSet columnSet)
        {
            return Task.Run(() => GetListForEntity(filter, objectTypeCode, columnSet));
        }

        private IEnumerable<TeamTemplate> GetListForEntity(string filter, int objectTypeCode, ColumnSet columnSet)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = TeamTemplate.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.None,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TeamTemplate.Schema.Attributes.objecttypecode, ConditionOperator.Equal, objectTypeCode),
                    },
                },

                Orders =
                {
                    new OrderExpression(TeamTemplate.Schema.Attributes.objecttypecode, OrderType.Ascending),
                    new OrderExpression(TeamTemplate.Schema.Attributes.teamtemplatename, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filter))
            {
                if (Guid.TryParse(filter, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(TeamTemplate.Schema.Attributes.teamtemplateid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(TeamTemplate.Schema.Attributes.teamtemplatename, ConditionOperator.Like, "%" + filter + "%"));
                }
            }

            return _service.RetrieveMultipleAll<TeamTemplate>(query);
        }

        public TeamTemplate FindByName(string name, ColumnSet columnSet)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 1,

                EntityName = TeamTemplate.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.None,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TeamTemplate.Schema.Attributes.teamtemplatename, ConditionOperator.Equal, name),
                    },
                },
            };

            var collection = _service.RetrieveMultiple(query);

            return collection.Entities.Select(e => e.ToEntity<TeamTemplate>()).FirstOrDefault();
        }

        public Task<TeamTemplate> GetByIdAsync(Guid idTeamTemplate, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idTeamTemplate, columnSet));
        }

        private TeamTemplate GetById(Guid idTeamTemplate, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = TeamTemplate.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TeamTemplate.EntityPrimaryIdAttribute, ConditionOperator.Equal, idTeamTemplate),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<TeamTemplate>()).SingleOrDefault() : null;
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
                    Header = nameof(TeamTemplate.TeamTemplateName),
                    Width = new DataGridLength(260),
                    Binding = new Binding
                    {
                        Path = new PropertyPath(nameof(TeamTemplate.TeamTemplateName)),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = nameof(TeamTemplate.ObjectTypeCode),
                    Width = new DataGridLength(120),
                    Binding = new Binding
                    {
                        Path = new PropertyPath(nameof(TeamTemplate.ObjectTypeCode)),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = nameof(TeamTemplate.DefaultAccessRightsMask),
                    Width = new DataGridLength(120),
                    Binding = new Binding
                    {
                        Path = new PropertyPath(nameof(TeamTemplate.DefaultAccessRightsMask)),
                        Mode = BindingMode.OneTime,
                    },
                },
            };
        }
    }
}
