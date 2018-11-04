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
            };

            return query;
        }

        protected override List<Entity> GetEntitiesByQuery(QueryExpression query)
        {
            var result = base.GetEntitiesByQuery(query);

            SdkMessageProcessingStepRepository.FullfillEntitiesSteps(result);

            return result;
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "IsCustomizable", "Behavior");

            action(handler, false, withManaged, withSolutionInfo);

            handler.AppendHeader("FilteringAttributes");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SdkMessageProcessingStep>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.EventHandler?.Name ?? "Unknown"
                , entity.PrimaryObjectTypeCodeName
                , entity.SecondaryObjectTypeCodeName
                , entity.SdkMessageId?.Name ?? "Unknown"
                , SdkMessageProcessingStepRepository.GetStageName(entity.Stage.Value, entity.Mode.Value)
                , entity.Rank.ToString()
                , entity.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            AppendIntoValues(values, entity, false, withManaged, withSolutionInfo);

            values.Add(entity.FilteringAttributesStringsSorted);

            return values;
        }

        public override string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<SdkMessageProcessingStep>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return entity.PrimaryObjectTypeCodeName;
            }

            return base.GetLinkedEntityName(solutionComponent);
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
                    , { SdkMessageProcessingStep.Schema.Attributes.filteringattributes, "FilteringAttributes" }
                };
        }
    }
}