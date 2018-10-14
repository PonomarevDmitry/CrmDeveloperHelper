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
    public class SdkMessageDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SdkMessageDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessage)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessage;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessage;

        public override string EntityLogicalName => SdkMessage.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessage.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SdkMessage.Schema.Attributes.name
                    , SdkMessage.Schema.Attributes.categoryname
                    , SdkMessage.Schema.Attributes.template
                    , SdkMessage.Schema.Attributes.workflowsdkstepenabled
                    , SdkMessage.Schema.Attributes.autotransact
                    , SdkMessage.Schema.Attributes.customizationlevel
                    , SdkMessage.Schema.Attributes.expand
                    , SdkMessage.Schema.Attributes.isactive
                    , SdkMessage.Schema.Attributes.isprivate
                    , SdkMessage.Schema.Attributes.isreadonly
                    , SdkMessage.Schema.Attributes.isvalidforexecuteasync
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
            var list = GetEntities<SdkMessage>(components.Select(c => c.ObjectId));

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

            handler.SetHeader("Message", "Category", "Template", "WorkflowSdkStepEnabled", "CustomizationLevel", "AutoTransact", "Expand", "IsActive", "IsPrivate", "IsReadOnly", "IsValidForExecuteAsync");

            foreach (var entity in list.Select(ent => ent.ToEntity<SdkMessage>()))
            {
                handler.AddLine(
                    entity.Name
                    , entity.CategoryName
                    , entity.Template.ToString()
                    , entity.WorkflowSdkStepEnabled.ToString()
                    , entity.CustomizationLevel.ToString()
                    , entity.AutoTransact.ToString()
                    , entity.Expand.ToString()
                    , entity.IsActive.ToString()
                    , entity.IsPrivate.ToString()
                    , entity.IsReadOnly.ToString()
                    , entity.IsValidForExecuteAsync.ToString()
                );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var sdkMessage = GetEntity<SdkMessage>(component.ObjectId.Value);

            if (sdkMessage != null)
            {
                FormatTextTableHandler handler = new FormatTextTableHandler();

                handler.SetHeader("Message", "Category", "Template", "WorkflowSdkStepEnabled", "CustomizationLevel", "AutoTransact", "Expand", "IsActive", "IsPrivate", "IsReadOnly", "IsValidForExecuteAsync");

                handler.AddLine(
                    sdkMessage.Name
                    , sdkMessage.CategoryName
                    , sdkMessage.Template.ToString()
                    , sdkMessage.WorkflowSdkStepEnabled.ToString()
                    , sdkMessage.CustomizationLevel.ToString()
                    , sdkMessage.AutoTransact.ToString()
                    , sdkMessage.Expand.ToString()
                    , sdkMessage.IsActive.ToString()
                    , sdkMessage.IsPrivate.ToString()
                    , sdkMessage.IsReadOnly.ToString()
                    , sdkMessage.IsValidForExecuteAsync.ToString()
                );

                var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                return string.Format("SdkMessage {0}", str);
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SdkMessage.Schema.Attributes.name, "Message" }
                    , { SdkMessage.Schema.Attributes.categoryname, "Category" }
                    , { SdkMessage.Schema.Attributes.template, "Template" }
                    , { SdkMessage.Schema.Attributes.workflowsdkstepenabled, "WorkflowSdkStepEnabled" }
                    , { SdkMessage.Schema.Attributes.customizationlevel, "CustomizationLevel" }
                    , { SdkMessage.Schema.Attributes.autotransact, "AutoTransact" }
                    , { SdkMessage.Schema.Attributes.expand, "Expand" }
                    , { SdkMessage.Schema.Attributes.isactive, "IsActive" }
                    , { SdkMessage.Schema.Attributes.isprivate, "IsPrivate" }
                    , { SdkMessage.Schema.Attributes.isreadonly, "IsReadOnly" }
                    , { SdkMessage.Schema.Attributes.isvalidforexecuteasync, "IsValidForExecuteAsync" }
                };
        }
    }
}