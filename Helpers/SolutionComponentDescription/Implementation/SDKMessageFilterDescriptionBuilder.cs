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
            var list = GetEntities<SdkMessageFilter>(components.Select(c => c.ObjectId));

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

            handler.SetHeader("Message", "PrimaryObjectTypeCode", "SecondaryObjectTypeCode", "WorkflowSdkStepEnabled", "IsCustomProcessingStepAllowed", "IsVisible", "CustomizationLevel");

            foreach (var entity in list.Select(ent => ent.ToEntity<SdkMessageFilter>()))
            {
                handler.AddLine(
                    entity.SdkMessageId?.Name
                    , entity.PrimaryObjectTypeCode
                    , entity.SecondaryObjectTypeCode
                    , entity.WorkflowSdkStepEnabled.ToString()
                    , entity.IsCustomProcessingStepAllowed.ToString()
                    , entity.IsVisible.ToString()
                    , entity.CustomizationLevel.ToString()
                );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var sdkMessageFilter = GetEntity<SdkMessageFilter>(component.ObjectId.Value);

            if (sdkMessageFilter != null)
            {
                FormatTextTableHandler handler = new FormatTextTableHandler();

                handler.SetHeader("Message", "PrimaryObjectTypeCode", "SecondaryObjectTypeCode", "WorkflowSdkStepEnabled", "IsCustomProcessingStepAllowed", "IsVisible", "CustomizationLevel");

                handler.AddLine(
                    sdkMessageFilter.SdkMessageId?.Name
                    , sdkMessageFilter.PrimaryObjectTypeCode
                    , sdkMessageFilter.SecondaryObjectTypeCode
                    , sdkMessageFilter.WorkflowSdkStepEnabled.ToString()
                    , sdkMessageFilter.IsCustomProcessingStepAllowed.ToString()
                    , sdkMessageFilter.IsVisible.ToString()
                    , sdkMessageFilter.CustomizationLevel.ToString()
                );

                var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                return string.Format("SdkMessageFilter {0}", str);
            }

            return base.GenerateDescriptionSingle(component, withUrls);
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
