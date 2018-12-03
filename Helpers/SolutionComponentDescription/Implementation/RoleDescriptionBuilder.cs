using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class RoleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RoleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.Role)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.Role;

        public override int ComponentTypeValue => (int)ComponentType.Role;

        public override string EntityLogicalName => Role.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => Role.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet(Role.Schema.Attributes.name, Role.Schema.Attributes.businessunitid, Role.Schema.Attributes.ismanaged, Role.Schema.Attributes.iscustomizable);
        }

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

                ColumnSet = GetColumnSet(),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(this.EntityPrimaryIdAttribute, ConditionOperator.In, idsNotCached.ToArray()),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.businessunitid,

                        LinkToEntityName = BusinessUnit.EntityLogicalName,
                        LinkToAttributeName = BusinessUnit.PrimaryIdAttribute,

                        EntityAlias = BusinessUnit.EntityLogicalName,

                        Columns = new ColumnSet(BusinessUnit.Schema.Attributes.parentbusinessunitid, BusinessUnit.Schema.Attributes.name),
                    },

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
                        LinkFromAttributeName = Role.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = Role.Schema.Attributes.supportingsolutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = SupportingSolutionAlias,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },
                },

                Orders =
                {
                    new OrderExpression(Role.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return query;
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "RoleTemplate", "BusinessUnit", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<Role>();

            List<string> values = new List<string>();

            var businessUnit = entity.BusinessUnitId.Name;

            if (entity.BusinessUnitParentBusinessUnit == null)
            {
                businessUnit = "Root Organization";
            }

            values.AddRange(new[]
            {
                entity.Name
                , entity.RoleTemplateName
                , businessUnit
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            if (solutionComponent == null
                || !solutionComponent.ObjectId.HasValue
                )
            {
                return;
            }

            var entity = GetEntity<Role>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                var imageComponent = new SolutionImageComponent()
                {
                    ComponentType = this.ComponentTypeValue,
                    RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
                };

                if (entity.RoleTemplateId != null)
                {
                    imageComponent.SchemaName = entity.RoleTemplateId.Id.ToString();
                }
                else
                {
                    imageComponent.ObjectId = entity.Id;
                }

                result.Add(imageComponent);
            }
        }

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null)
            {
                return;
            }

            if (FillSolutionComponentFromSchemaName(result, solutionImageComponent.SchemaName, solutionImageComponent.RootComponentBehavior))
            {
                return;
            }

            base.FillSolutionComponent(result, solutionImageComponent);
        }

        public override void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {
            var parentId = GetAttributeValue(elementRootComponent, "parentId");
            var behavior = GetBehaviorFromXml(elementRootComponent);

            if (FillSolutionComponentFromSchemaName(result, parentId, behavior))
            {
                return;
            }

            base.FillSolutionComponentFromXml(result, elementRootComponent, docCustomizations);
        }

        private bool FillSolutionComponentFromSchemaName(ICollection<SolutionComponent> result, string schemaName, int? behavior)
        {
            if (!string.IsNullOrEmpty(schemaName)
                && Guid.TryParse(schemaName, out var roleTemplateId))
            {
                var repository = new RoleRepository(_service);

                var entity = repository.FindRoleByTemplate(roleTemplateId, new ColumnSet(false));

                if (entity != null)
                {
                    FillSolutionComponentInternal(result, entity.Id, behavior);

                    return true;
                }
            }

            return false;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    {  Role.Schema.Attributes.name, "Name" }
                    , { Role.Schema.Attributes.roletemplateid + "." + RoleTemplate.Schema.Attributes.name, "RoleTemplate" }
                    , { Role.Schema.Attributes.businessunitid, "BusinessUnit" }
                    , { Role.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { Role.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }

        public static IEnumerable<DataGridColumn> GetDataGridColumn()
        {
            //<DataGridTextColumn Header="Role Name" Width="200" Binding="{Binding Name, Mode=OneTime}" />
            //<DataGridTextColumn Header="RoleTemplate" Width="200" Binding="{Binding RoleTemplateName, Mode=OneTime}" />
            //<DataGridTextColumn Header="BusinessUnit" Width="200" Binding="{Binding BusinessUnitId.Name, Mode=OneTime}" />

            return new List<DataGridColumn>()
            {
                new DataGridTextColumn()
                {
                    Header = "Role Name",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("Name"),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "RoleTemplate",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("RoleTemplateName"),
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
    }
}