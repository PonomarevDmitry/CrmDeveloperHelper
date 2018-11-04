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
    public class ChannelAccessProfileEntityAccessLevelDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ChannelAccessProfileEntityAccessLevelDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ChannelAccessProfileEntityAccessLevel)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ChannelAccessProfileEntityAccessLevel;

        public override int ComponentTypeValue => (int)ComponentType.ChannelAccessProfileEntityAccessLevel;

        public override string EntityLogicalName => ChannelAccessProfileEntityAccessLevel.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ChannelAccessProfileEntityAccessLevel.Schema.EntityPrimaryIdAttribute;

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = this.EntityLogicalName,

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
                        LinkFromEntityName = ChannelAccessProfileEntityAccessLevel.EntityLogicalName,
                        LinkFromAttributeName = ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid,

                        LinkToEntityName = ChannelAccessProfile.EntityLogicalName,
                        LinkToAttributeName = ChannelAccessProfile.PrimaryIdAttribute,

                        EntityAlias = ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid,

                        Columns = new ColumnSet(ChannelAccessProfile.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = ChannelAccessProfileEntityAccessLevel.EntityLogicalName,
                        LinkFromAttributeName = ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid,

                        LinkToEntityName = Privilege.EntityLogicalName,
                        LinkToAttributeName = Privilege.PrimaryIdAttribute,

                        EntityAlias = ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid,

                        Columns = new ColumnSet(Privilege.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = AttributeMap.EntityLogicalName,
                        LinkFromAttributeName = AttributeMap.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = AttributeMap.EntityLogicalName,
                        LinkFromAttributeName = AttributeMap.Schema.Attributes.supportingsolutionid,

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
            handler.SetHeader("ChannelAccessProfileName", "EntityAccessLevelName", "EntityAccessLevelDepthMask", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ChannelAccessProfileEntityAccessLevel>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid + "." + ChannelAccessProfile.Schema.Attributes.name)
                , EntityDescriptionHandler.GetAttributeString(entity, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid + "." + Privilege.Schema.Attributes.name)
                , entity.EntityAccessLevelDepthMask.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<ChannelAccessProfileEntityAccessLevel>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1} - {2}"
                    , EntityDescriptionHandler.GetAttributeString(entity, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid + "." + ChannelAccessProfile.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid + "." + Privilege.Schema.Attributes.name)
                    , entity.EntityAccessLevelDepthMask
                );
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid + "." + ChannelAccessProfile.Schema.Attributes.name, "ChannelAccessProfileName" }
                    , { ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid + "." + Privilege.Schema.Attributes.name, "EntityAccessLevel" }
                    , { ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccessleveldepthmask, "EntityAccessLevelDepthMask" }
                    , { ChannelAccessProfileEntityAccessLevel.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}