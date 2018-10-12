using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            return query;
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<ChannelAccessProfileEntityAccessLevel>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("ChannelAccessProfileName", "EntityAccessLevelName", "EntityAccessLevelDepthMask", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(entity, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid + "." + ChannelAccessProfile.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid + "." + Privilege.Schema.Attributes.name)
                    , entity.EntityAccessLevelDepthMask.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var channelAccessProfileEntityAccessLevel = GetEntity<ChannelAccessProfileEntityAccessLevel>(component.ObjectId.Value);

            if (channelAccessProfileEntityAccessLevel != null)
            {
                return string.Format("ChannelAccessProfileName {0}    EntityAccessLevelName {1}    EntityAccessLevelDepthMask {2}    IsManaged {3}    SolutionName {4}"
                    , EntityDescriptionHandler.GetAttributeString(channelAccessProfileEntityAccessLevel, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid + "." + ChannelAccessProfile.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(channelAccessProfileEntityAccessLevel, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid + "." + Privilege.Schema.Attributes.name)
                    , channelAccessProfileEntityAccessLevel.EntityAccessLevelDepthMask
                    , channelAccessProfileEntityAccessLevel.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(channelAccessProfileEntityAccessLevel, "solution.uniquename")
                    );
            }

            return component.ToString();
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

            return component.ObjectId.ToString();
        }
    }
}