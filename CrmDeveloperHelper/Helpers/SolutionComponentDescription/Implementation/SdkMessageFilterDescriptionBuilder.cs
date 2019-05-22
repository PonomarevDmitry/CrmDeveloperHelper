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
    public class SdkMessageFilterDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        private readonly SolutionComponentMetadataSource _source;

        public SdkMessageFilterDescriptionBuilder(IOrganizationServiceExtented service, SolutionComponentMetadataSource source)
            : base(service, (int)ComponentType.SdkMessageFilter)
        {
            this._source = source;
        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessageFilter;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessageFilter;

        public override string EntityLogicalName => SdkMessageFilter.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessageFilter.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SdkMessageFilter.Schema.Attributes.sdkmessageid
                    , SdkMessageFilter.Schema.Attributes.primaryobjecttypecode
                    , SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode
                    , SdkMessageFilter.Schema.Attributes.workflowsdkstepenabled
                    , SdkMessageFilter.Schema.Attributes.iscustomprocessingstepallowed
                    , SdkMessageFilter.Schema.Attributes.customizationlevel
                    , SdkMessageFilter.Schema.Attributes.isvisible
                );
        }

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

                        LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageFilter.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageFilter.Schema.Attributes.supportingsolutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

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
            handler.SetHeader("Message", "PrimaryObjectTypeCode", "SecondaryObjectTypeCode", "WorkflowSdkStepEnabled", "IsCustomProcessingStepAllowed", "IsVisible", "CustomizationLevel", "Behavior");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SdkMessageFilter>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.SdkMessageId?.Name
                , entity.PrimaryObjectTypeCode
                , entity.SecondaryObjectTypeCode
                , entity.WorkflowSdkStepEnabled.ToString()
                , entity.IsCustomProcessingStepAllowed.ToString()
                , entity.IsVisible.ToString()
                , entity.CustomizationLevel.ToString()
                , behavior
            });

            return values;
        }

        public override string GetName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<SdkMessageFilter>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}"
                    , entity.SdkMessageId?.Name
                    , entity.PrimaryObjectTypeCode
                );
            }

            return base.GetName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SdkMessageFilter.Schema.Attributes.sdkmessageid, "Message" }
                    , { SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, "PrimaryObjectTypeCode" }
                    , { SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode, "SecondaryObjectTypeCode" }
                    , { SdkMessageFilter.Schema.Attributes.workflowsdkstepenabled, "WorkflowSdkStepEnabled" }
                    , { SdkMessageFilter.Schema.Attributes.iscustomprocessingstepallowed, "IsCustomProcessingStepAllowed" }
                    , { SdkMessageFilter.Schema.Attributes.isvisible, "IsVisible" }
                    , { SdkMessageFilter.Schema.Attributes.customizationlevel, "CustomizationLevel" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }

        public override IEnumerable<SolutionComponent> GetLinkedComponents(SolutionComponent solutionComponent)
        {
            var result = new List<SolutionComponent>();

            var entity = GetEntity<SdkMessageFilter>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                if (entity.SdkMessageId != null)
                {
                    result.Add(new SolutionComponent()
                    {
                        ObjectId = entity.SdkMessageId.Id,
                        ComponentType = new OptionSetValue((int)ComponentType.SdkMessage),
                    });
                }

                if (!string.IsNullOrEmpty(entity.PrimaryObjectTypeCode))
                {
                    var entityMetadata = _source.GetEntityMetadata(entity.PrimaryObjectTypeCode);

                    if (entityMetadata != null)
                    {
                        result.Add(new SolutionComponent()
                        {
                            ObjectId = entityMetadata.MetadataId,
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                        });
                    }
                }

                if (!string.IsNullOrEmpty(entity.SecondaryObjectTypeCode)
                    && !string.Equals(entity.PrimaryObjectTypeCode, entity.SecondaryObjectTypeCode, StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    var entityMetadata = _source.GetEntityMetadata(entity.SecondaryObjectTypeCode);

                    if (entityMetadata != null)
                    {
                        result.Add(new SolutionComponent()
                        {
                            ObjectId = entityMetadata.MetadataId,
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                        });
                    }
                }
            }

            return result;
        }
    }
}
