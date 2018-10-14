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
    public class SdkMessageRequestDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SdkMessageRequestDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessageRequest)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessageRequest;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessageRequest;

        public override string EntityLogicalName => SdkMessageRequest.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessageRequest.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SdkMessageRequest.Schema.Attributes.name
                    , SdkMessageRequest.Schema.Attributes.primaryobjecttypecode
                    , SdkMessageRequest.Schema.Attributes.customizationlevel
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
                        LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                        LinkToEntityName = SdkMessagePair.EntityLogicalName,
                        LinkToAttributeName = SdkMessagePair.PrimaryIdAttribute,

                        EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                                LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                                LinkToEntityName = SdkMessage.EntityLogicalName,
                                LinkToAttributeName = SdkMessage.PrimaryIdAttribute,

                                EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid,

                                Columns = new ColumnSet(SdkMessage.Schema.Attributes.name),
                            },
                        },
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
            var list = GetEntities<SdkMessageRequest>(components.Select(c => c.ObjectId));

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

            handler.SetHeader("Message", "Name", "PrimaryObjectTypeCode", "CustomizationLevel");

            foreach (var entity in list.Select(ent => ent.ToEntity<SdkMessageRequest>()))
            {
                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                    , entity.Name
                    , entity.PrimaryObjectTypeCode
                    , entity.CustomizationLevel.ToString()
                );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var sdkMessageRequest = GetEntity<SdkMessageRequest>(component.ObjectId.Value);

            if (sdkMessageRequest != null)
            {
                FormatTextTableHandler handler = new FormatTextTableHandler();

                handler.SetHeader("Message", "Name", "PrimaryObjectTypeCode", "CustomizationLevel");

                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(sdkMessageRequest, SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                    , sdkMessageRequest.Name
                    , sdkMessageRequest.PrimaryObjectTypeCode
                    , sdkMessageRequest.CustomizationLevel.ToString()
                );

                var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                return string.Format("SdkMessageRequest {0}", str);
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name, "Message" }
                    , { SdkMessageRequest.Schema.Attributes.name, "Name" }
                    , { SdkMessageRequest.Schema.Attributes.primaryobjecttypecode, "PrimaryObjectTypeCode" }
                    , { SdkMessageRequest.Schema.Attributes.customizationlevel, "CustomizationLevel" }
                };
        }
    }
}
