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
    public class DisplayStringMapDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public DisplayStringMapDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.DisplayStringMap)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.DisplayStringMap;

        public override int ComponentTypeValue => (int)ComponentType.DisplayStringMap;

        public override string EntityLogicalName => DisplayStringMap.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => DisplayStringMap.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet(DisplayStringMap.Schema.Attributes.objecttypecode, DisplayStringMap.Schema.Attributes.ismanaged);
        }

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
           var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = DisplayStringMap.EntityLogicalName,

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

                        LinkFromEntityName = DisplayStringMap.EntityLogicalName,
                        LinkFromAttributeName = DisplayStringMap.Schema.Attributes.displaystringid,

                        LinkToEntityName = DisplayString.EntityLogicalName,
                        LinkToAttributeName = DisplayString.PrimaryIdAttribute,

                        EntityAlias = DisplayString.EntityLogicalName,

                        Columns = new ColumnSet(DisplayString.Schema.Attributes.displaystringkey),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = DisplayStringMap.EntityLogicalName,
                        LinkFromAttributeName = DisplayStringMap.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = DisplayStringMap.EntityLogicalName,
                        LinkFromAttributeName = DisplayStringMap.Schema.Attributes.supportingsolutionid,

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
            handler.SetHeader("DisplayStringKey", "ObjectTypeCode", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<DisplayStringMap>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, "displaystring.displaystringkey")
                , entity.ObjectTypeCode
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<DisplayStringMap>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}"
                    , EntityDescriptionHandler.GetAttributeString(entity, "displaystring.displaystringkey")
                    , entity.ObjectTypeCode
                    );
            }

            return base.GetName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { "displaystring.displaystringkey", "DisplayStringKey" }
                    , { DisplayStringMap.Schema.Attributes.objecttypecode, "Entity" }
                    , { DisplayStringMap.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}