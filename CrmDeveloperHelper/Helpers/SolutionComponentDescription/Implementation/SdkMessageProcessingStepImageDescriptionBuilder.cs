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
    public class SdkMessageProcessingStepImageDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SdkMessageProcessingStepImageDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessageProcessingStepImage)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessageProcessingStepImage;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessageProcessingStepImage;

        public override string EntityLogicalName => SdkMessageProcessingStepImage.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessageProcessingStepImage.Schema.EntityPrimaryIdAttribute;

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageProcessingStepImage.EntityLogicalName,

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

                        LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid,

                        LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStep.PrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.EntityLogicalName,

                        Columns = new ColumnSet(
                            SdkMessageProcessingStep.Schema.Attributes.sdkmessageid
                            , SdkMessageProcessingStep.Schema.Attributes.stage
                            , SdkMessageProcessingStep.Schema.Attributes.mode
                            , SdkMessageProcessingStep.Schema.Attributes.rank
                            , SdkMessageProcessingStep.Schema.Attributes.statuscode
                            , SdkMessageProcessingStep.Schema.Attributes.eventhandler
                        ),

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
                        },
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStepImage.Schema.Attributes.supportingsolutionid,

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
            handler.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "ImageType", "Name", "EntityAlias", "IsCustomizable", "Behavior");

            action(handler, false, withManaged, withSolutionInfo);

            handler.AppendHeader("Attributes");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SdkMessageProcessingStepImage>();

            List<string> values = new List<string>();

            int stage = entity.Contains("sdkmessageprocessingstep.stage") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.stage").Value as OptionSetValue).Value : 0;
            int mode = entity.Contains("sdkmessageprocessingstep.mode") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.mode").Value as OptionSetValue).Value : 0;

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, "sdkmessageprocessingstep.eventhandler")
                , entity.PrimaryObjectTypeCodeName
                , entity.SecondaryObjectTypeCodeName
                , EntityDescriptionHandler.GetAttributeString(entity, "sdkmessageprocessingstep.sdkmessageid")
                , SdkMessageProcessingStepRepository.GetStageName(stage, mode)
                , EntityDescriptionHandler.GetAttributeString(entity, "sdkmessageprocessingstep.rank")
                , EntityDescriptionHandler.GetAttributeString(entity, "sdkmessageprocessingstep.statuscode")
                , entity.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype]
                , entity.Name
                , entity.EntityAlias
                , entity.IsManaged.ToString()
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            AppendIntoValues(values, entity, false, withManaged, withSolutionInfo);

            values.Add(entity.Attributes1StringsSorted);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<SdkMessageProcessingStepImage>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.EntityAlias;
            }

            return base.GetName(component);
        }

        public override string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<SdkMessageProcessingStepImage>(solutionComponent.ObjectId.Value);

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
                    { "sdkmessageprocessingstep.eventhandler", "TypeName" }
                    , { SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode, "PrimaryObjectTypeCode" }
                    , { SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode, "SecondaryObjectTypeCode" }
                    , { "sdkmessageprocessingstep.sdkmessageid", "Message" }
                    , { "sdkmessageprocessingstep.stage", "Stage" }
                    , { "sdkmessageprocessingstep.mode", "Mode" }
                    , { "sdkmessageprocessingstep.rank", "Rank" }
                    , { "sdkmessageprocessingstep.statuscode", "StatusCode" }
                    , { SdkMessageProcessingStepImage.Schema.Attributes.imagetype, "ImageType" }
                    , { SdkMessageProcessingStepImage.Schema.Attributes.name, "Name" }
                    , { SdkMessageProcessingStepImage.Schema.Attributes.entityalias, "EntityAlias" }
                    , { SdkMessageProcessingStepImage.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { SdkMessageProcessingStepImage.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                    , { SdkMessageProcessingStepImage.Schema.Attributes.attributes, "" }
                };
        }
    }
}