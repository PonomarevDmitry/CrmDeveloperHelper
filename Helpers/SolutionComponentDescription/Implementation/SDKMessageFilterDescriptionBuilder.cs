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
        public SdkMessageFilterDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessageFilter)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessageFilter;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessageFilter;

        public override string EntityLogicalName => SdkMessageFilter.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessageFilter.Schema.EntityPrimaryIdAttribute;

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
                };
        }
    }
}
