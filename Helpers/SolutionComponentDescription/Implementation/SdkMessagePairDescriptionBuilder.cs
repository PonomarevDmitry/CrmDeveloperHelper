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
   public class SdkMessagePairDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SdkMessagePairDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessagePair)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessagePair;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessagePair;

        public override string EntityLogicalName => SdkMessagePair.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessagePair.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SdkMessagePair.Schema.Attributes.sdkmessageid
                    , SdkMessagePair.Schema.Attributes.@namespace
                    , SdkMessagePair.Schema.Attributes.endpoint
                    , SdkMessagePair.Schema.Attributes.sdkmessagebindinginformation
                    , SdkMessagePair.Schema.Attributes.customizationlevel
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
            var list = GetEntities<SdkMessagePair>(components.Select(c => c.ObjectId));

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

            handler.SetHeader("Message", "Namespace", "Endpoint", "SdkMessageBindingInformation", "CustomizationLevel");

            foreach (var entity in list.Select(ent => ent.ToEntity<SdkMessagePair>()))
            {
                handler.AddLine(
                    entity.SdkMessageId?.Name
                    , entity.Namespace
                    , entity.Endpoint
                    , entity.SdkMessageBindingInformation
                    , entity.CustomizationLevel.ToString()
                );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var sdkMessagePair = GetEntity<SdkMessagePair>(component.ObjectId.Value);

            if (sdkMessagePair != null)
            {
                FormatTextTableHandler handler = new FormatTextTableHandler();

                handler.SetHeader("Message", "Namespace", "Endpoint", "SdkMessageBindingInformation", "CustomizationLevel");

                handler.AddLine(
                    sdkMessagePair.SdkMessageId?.Name
                    , sdkMessagePair.Namespace
                    , sdkMessagePair.Endpoint
                    , sdkMessagePair.SdkMessageBindingInformation
                    , sdkMessagePair.CustomizationLevel.ToString()
                );

                var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                return string.Format("SdkMessagePair {0}", str);
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SdkMessagePair.Schema.Attributes.sdkmessageid, "Message" }
                    , { SdkMessagePair.Schema.Attributes.@namespace, "Namespace" }
                    , { SdkMessagePair.Schema.Attributes.endpoint, "Endpoint" }
                    , { SdkMessagePair.Schema.Attributes.sdkmessagebindinginformation, "SdkMessageBindingInformation" }
                    , { SdkMessagePair.Schema.Attributes.customizationlevel, "CustomizationLevel" }
                };
        }
    }
}
