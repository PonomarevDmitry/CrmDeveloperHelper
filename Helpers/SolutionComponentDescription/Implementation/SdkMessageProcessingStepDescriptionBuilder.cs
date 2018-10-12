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

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class SdkMessageProcessingStepDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SdkMessageProcessingStepDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessageProcessingStep)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessageProcessingStep;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessageProcessingStep;

        public override string EntityLogicalName => SdkMessageProcessingStep.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessageProcessingStep.Schema.EntityPrimaryIdAttribute;

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
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkToAttributeName = SdkMessageFilter.PrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        Columns = new ColumnSet(SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.supportingsolutionid,

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

        protected override List<Entity> GetEntitiesByQuery(QueryExpression query)
        {
            var result = base.GetEntitiesByQuery(query);

            SdkMessageProcessingStepRepository.FullfillEntitiesSteps(result);

            return result;
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<SdkMessageProcessingStep>(components.Select(c => c.ObjectId));

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

            handler.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "FilteringAttributes");

            foreach (var entity in list.Select(ent => ent.ToEntity<SdkMessageProcessingStep>()))
            {
                handler.AddLine(
                    entity.EventHandler?.Name ?? "Unknown"
                    , entity.PrimaryObjectTypeCodeName
                    , entity.SecondaryObjectTypeCodeName
                    , entity.SdkMessageId?.Name ?? "Unknown"
                    , SdkMessageProcessingStepRepository.GetStageName(entity.Stage.Value, entity.Mode.Value)
                    , entity.Rank.ToString()
                    , entity.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , entity.FilteringAttributesStringsSorted
                );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var sdkMessageProcessingStep = GetEntity<SdkMessageProcessingStep>(component.ObjectId.Value);

            if (sdkMessageProcessingStep != null)
            {
                FormatTextTableHandler handler = new FormatTextTableHandler();

                handler.AddLine(
                    sdkMessageProcessingStep.EventHandler?.Name ?? "Unknown"
                    , sdkMessageProcessingStep.PrimaryObjectTypeCodeName
                    , sdkMessageProcessingStep.SecondaryObjectTypeCodeName
                    , sdkMessageProcessingStep.SdkMessageId?.Name ?? "Unknown"
                    , SdkMessageProcessingStepRepository.GetStageName(sdkMessageProcessingStep.Stage.Value, sdkMessageProcessingStep.Mode.Value)
                    , sdkMessageProcessingStep.Rank.ToString()
                    , sdkMessageProcessingStep.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                    , sdkMessageProcessingStep.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(sdkMessageProcessingStep, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(sdkMessageProcessingStep, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(sdkMessageProcessingStep, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(sdkMessageProcessingStep, "suppsolution.ismanaged")
                    , sdkMessageProcessingStep.FilteringAttributesStringsSorted
                );

                var str = handler.GetFormatedLines(false).FirstOrDefault();

                return string.Format("SdkMessageProcessingStep {0}", str);
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SdkMessageProcessingStep.Schema.Attributes.eventhandler, "TypeName" }
                    , { SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode, "PrimaryObjectTypeCode" }
                    , { SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode, "SecondaryObjectTypeCode" }
                    , { SdkMessageProcessingStep.Schema.Attributes.sdkmessageid, "Message" }
                    , { SdkMessageProcessingStep.Schema.Attributes.stage, "Stage" }
                    , { SdkMessageProcessingStep.Schema.Attributes.mode, "Mode" }
                    , { SdkMessageProcessingStep.Schema.Attributes.rank, "Rank" }
                    , { SdkMessageProcessingStep.Schema.Attributes.statuscode, "StatusCode" }
                    , { SdkMessageProcessingStep.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { SdkMessageProcessingStep.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                    , { SdkMessageProcessingStep.Schema.Attributes.filteringattributes, "" }
                };
        }
    }
}