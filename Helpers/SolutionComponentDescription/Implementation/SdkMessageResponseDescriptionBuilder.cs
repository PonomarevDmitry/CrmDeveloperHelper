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
    public class SdkMessageResponseDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SdkMessageResponseDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessageResponse)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessageResponse;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessageResponse;

        public override string EntityLogicalName => SdkMessageResponse.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessageResponse.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SdkMessageResponse.Schema.Attributes.sdkmessagerequestid
                    , SdkMessageResponse.Schema.Attributes.customizationlevel
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
                        LinkFromEntityName = SdkMessageResponse.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid,

                        LinkToEntityName = SdkMessagePair.EntityLogicalName,
                        LinkToAttributeName = SdkMessagePair.PrimaryIdAttribute,

                        EntityAlias = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                                LinkToEntityName = SdkMessagePair.EntityLogicalName,
                                LinkToAttributeName = SdkMessagePair.PrimaryIdAttribute,

                                EntityAlias = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                                LinkEntities =
                                {
                                    new LinkEntity()
                                    {
                                        LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                                        LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                                        LinkToEntityName = SdkMessage.EntityLogicalName,
                                        LinkToAttributeName = SdkMessage.PrimaryIdAttribute,

                                        EntityAlias = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid,

                                        Columns = new ColumnSet(SdkMessage.Schema.Attributes.name),
                                    },
                                },
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
            var list = GetEntities<SdkMessageResponse>(components.Select(c => c.ObjectId));

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

            handler.SetHeader("Message", "RequestName", "CustomizationLevel");

            foreach (var entity in list.Select(ent => ent.ToEntity<SdkMessageResponse>()))
            {
                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(entity, SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                    , entity.SdkMessageRequestId?.Name
                    , entity.CustomizationLevel.ToString()
                );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var sdkMessageResponse = GetEntity<SdkMessageResponse>(component.ObjectId.Value);

            if (sdkMessageResponse != null)
            {
                FormatTextTableHandler handler = new FormatTextTableHandler();

                handler.SetHeader("Message", "RequestName", "CustomizationLevel");

                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(sdkMessageResponse, SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                    , sdkMessageResponse.SdkMessageRequestId?.Name
                    , sdkMessageResponse.CustomizationLevel.ToString()
                );

                var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                return string.Format("SdkMessageResponse {0}", str);
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name, "Message" }
                    , { SdkMessageResponse.Schema.Attributes.sdkmessagerequestid, "RequestName" }
                    , { SdkMessageResponse.Schema.Attributes.customizationlevel, "CustomizationLevel" }
                };
        }
    }
}
