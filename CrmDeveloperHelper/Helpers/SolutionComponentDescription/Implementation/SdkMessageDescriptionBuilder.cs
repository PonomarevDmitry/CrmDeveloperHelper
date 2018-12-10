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
            };

            return query;
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Message", "Category", "Template", "WorkflowSdkStepEnabled", "CustomizationLevel", "AutoTransact", "Expand", "IsActive", "IsPrivate", "IsReadOnly", "IsValidForExecuteAsync", "Behavior");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SdkMessage>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
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
                , behavior
            });

            return values;
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