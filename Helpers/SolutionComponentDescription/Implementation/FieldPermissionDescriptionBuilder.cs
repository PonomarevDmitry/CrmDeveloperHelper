using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class FieldPermissionDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public FieldPermissionDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.FieldPermission)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.FieldPermission;

        public override int ComponentTypeValue => (int)ComponentType.FieldPermission;

        public override string EntityLogicalName => FieldPermission.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => FieldPermission.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    FieldPermission.Schema.Attributes.entityname
                    , FieldPermission.Schema.Attributes.attributelogicalname
                    , FieldPermission.Schema.Attributes.ismanaged
                );
        }

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = FieldPermission.EntityLogicalName,

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

                        LinkFromEntityName = FieldPermission.EntityLogicalName,
                        LinkFromAttributeName = FieldPermission.Schema.Attributes.fieldsecurityprofileid,

                        LinkToEntityName = FieldSecurityProfile.EntityLogicalName,
                        LinkToAttributeName = FieldSecurityProfile.PrimaryIdAttribute,

                        EntityAlias = FieldSecurityProfile.EntityLogicalName,

                        Columns = new ColumnSet(FieldSecurityProfile.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = FieldPermission.EntityLogicalName,
                        LinkFromAttributeName = FieldPermission.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = FieldPermission.EntityLogicalName,
                        LinkFromAttributeName = FieldPermission.Schema.Attributes.supportingsolutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = SupportingSolutionAlias,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },
                },
            };

            return query;
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("FieldSecurityProfileName", "Attribute", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<FieldPermission>();

            List<string> values = new List<string>();

            string attr = string.Format("{0}.{1}", entity.EntityName, entity.AttributeLogicalName);

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, "fieldsecurityprofile.name")
                , attr
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<FieldPermission>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0}.{1}", entity.EntityName, entity.AttributeLogicalName);
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { "fieldsecurityprofile.name", "FieldSecurityProfile" }
                    , { FieldPermission.Schema.Attributes.entityname, "Entity" }
                    , { FieldPermission.Schema.Attributes.attributelogicalname, "Attribute" }
                    , { FieldPermission.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}