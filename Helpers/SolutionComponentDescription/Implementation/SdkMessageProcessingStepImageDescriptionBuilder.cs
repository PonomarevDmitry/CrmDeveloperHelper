using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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
            var list = GetEntities<SdkMessageProcessingStepImage>(components.Select(c => c.ObjectId));

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

            handler.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "ImageType", "Name", "EntityAlias", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Attributes");

            foreach (var entity in list.Select(ent => ent.ToEntity<SdkMessageProcessingStepImage>()))
            {
                string pluginType = entity.Contains("sdkmessageprocessingstep.eventhandler") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.eventhandler").Value as EntityReference).Name : "Null";

                string sdkMessage = entity.Contains("sdkmessageprocessingstep.sdkmessageid") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.sdkmessageid").Value as EntityReference).Name : "Null";
                int stage = entity.Contains("sdkmessageprocessingstep.stage") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.stage").Value as OptionSetValue).Value : 0;
                int mode = entity.Contains("sdkmessageprocessingstep.mode") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.mode").Value as OptionSetValue).Value : 0;
                int rank = entity.Contains("sdkmessageprocessingstep.rank") ? (int)entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.rank").Value : 0;
                string status = entity.FormattedValues.ContainsKey("sdkmessageprocessingstep.statuscode") ? entity.FormattedValues["sdkmessageprocessingstep.statuscode"] : "";

                handler.AddLine(
                    pluginType
                    , entity.PrimaryObjectTypeCodeName
                    , entity.SecondaryObjectTypeCodeName
                    , sdkMessage
                    , SdkMessageProcessingStepRepository.GetStageName(stage, mode)
                    , rank.ToString()
                    , status
                    , entity.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype]
                    , entity.Name
                    , entity.EntityAlias
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , entity.Attributes1StringsSorted
                );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var sdkMessageProcessingStepImage = GetEntity<SdkMessageProcessingStepImage>(component.ObjectId.Value);

            if (sdkMessageProcessingStepImage != null)
            {
                FormatTextTableHandler handler = new FormatTextTableHandler();

                string pluginType = sdkMessageProcessingStepImage.Contains("sdkmessageprocessingstep.eventhandler") ? (sdkMessageProcessingStepImage.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.eventhandler").Value as EntityReference).Name : "Null";

                string sdkMessage = sdkMessageProcessingStepImage.Contains("sdkmessageprocessingstep.sdkmessageid") ? (sdkMessageProcessingStepImage.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.sdkmessageid").Value as EntityReference).Name : "Null";
                int stage = sdkMessageProcessingStepImage.Contains("sdkmessageprocessingstep.stage") ? (sdkMessageProcessingStepImage.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.stage").Value as OptionSetValue).Value : 0;
                int mode = sdkMessageProcessingStepImage.Contains("sdkmessageprocessingstep.mode") ? (sdkMessageProcessingStepImage.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.mode").Value as OptionSetValue).Value : 0;
                int rank = sdkMessageProcessingStepImage.Contains("sdkmessageprocessingstep.rank") ? (int)sdkMessageProcessingStepImage.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.rank").Value : 0;
                string status = sdkMessageProcessingStepImage.FormattedValues.ContainsKey("sdkmessageprocessingstep.statuscode") ? sdkMessageProcessingStepImage.FormattedValues["sdkmessageprocessingstep.statuscode"] : "";

                handler.AddLine(
                    pluginType
                    , sdkMessageProcessingStepImage.PrimaryObjectTypeCodeName
                    , sdkMessageProcessingStepImage.SecondaryObjectTypeCodeName
                    , sdkMessage
                    , SdkMessageProcessingStepRepository.GetStageName(stage, mode)
                    , rank.ToString()
                    , status
                    , sdkMessageProcessingStepImage.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype]
                    , sdkMessageProcessingStepImage.Name
                    , sdkMessageProcessingStepImage.EntityAlias
                    , sdkMessageProcessingStepImage.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(sdkMessageProcessingStepImage, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(sdkMessageProcessingStepImage, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(sdkMessageProcessingStepImage, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(sdkMessageProcessingStepImage, "suppsolution.ismanaged")
                    , sdkMessageProcessingStepImage.Attributes1StringsSorted
                );

                var str = handler.GetFormatedLines(false).FirstOrDefault();

                return string.Format("SdkMessageProcessingStepImage {0}", str);
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var fieldPermission = GetEntity<FieldPermission>(component.ObjectId.Value);

            if (fieldPermission != null)
            {

            }

            return component.ObjectId.ToString();
        }
    }
}