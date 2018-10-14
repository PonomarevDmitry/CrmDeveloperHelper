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
    public class SdkMessageRequestFieldDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SdkMessageRequestFieldDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessageRequestField)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessageRequestField;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessageRequestField;

        public override string EntityLogicalName => SdkMessageRequestField.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessageRequestField.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SdkMessageRequestField.Schema.Attributes.position
                    , SdkMessageRequestField.Schema.Attributes.name
                    , SdkMessageRequestField.Schema.Attributes.publicname
                    , SdkMessageRequestField.Schema.Attributes.clrparser
                    , SdkMessageRequestField.Schema.Attributes.parser
                    , SdkMessageRequestField.Schema.Attributes.optional
                    , SdkMessageRequestField.Schema.Attributes.customizationlevel
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
                        LinkFromEntityName = SdkMessageRequestField.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid,

                        LinkToEntityName = SdkMessageRequest.EntityLogicalName,
                        LinkToAttributeName = SdkMessageRequest.PrimaryIdAttribute,

                        EntityAlias = SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid,

                        Columns = new ColumnSet(SdkMessageRequest.Schema.Attributes.name),

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                                LinkToEntityName = SdkMessagePair.EntityLogicalName,
                                LinkToAttributeName = SdkMessagePair.PrimaryIdAttribute,

                                EntityAlias = SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                                LinkEntities =
                                {
                                    new LinkEntity()
                                    {
                                        LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                                        LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                                        LinkToEntityName = SdkMessage.EntityLogicalName,
                                        LinkToAttributeName = SdkMessage.PrimaryIdAttribute,

                                        EntityAlias = SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid,

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
            var list = GetEntities<SdkMessageRequestField>(components.Select(c => c.ObjectId));

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

            handler.SetHeader("Message", "RequestName", "Position", "Name", "PublicName", "ClrParser", "Parser", "Optional", "CustomizationLevel");

            foreach (var entity in list.Select(ent => ent.ToEntity<SdkMessageRequestField>()))
            {
                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                    , entity.Position.ToString()
                    , entity.Name
                    , entity.PublicName
                    , entity.ClrParser
                    , entity.Parser
                    , entity.Optional.ToString()
                    , entity.CustomizationLevel.ToString()
                );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var sdkMessageRequestField = GetEntity<SdkMessageRequestField>(component.ObjectId.Value);

            if (sdkMessageRequestField != null)
            {
                FormatTextTableHandler handler = new FormatTextTableHandler();

                handler.SetHeader("Message", "RequestName", "Position", "Name", "PublicName", "ClrParser", "Parser", "Optional", "CustomizationLevel");

                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(sdkMessageRequestField, SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(sdkMessageRequestField, SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.name)
                    , sdkMessageRequestField.Position.ToString()
                    , sdkMessageRequestField.Name
                    , sdkMessageRequestField.PublicName
                    , sdkMessageRequestField.ClrParser
                    , sdkMessageRequestField.Parser
                    , sdkMessageRequestField.Optional.ToString()
                    , sdkMessageRequestField.CustomizationLevel.ToString()
                );

                var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                return string.Format("SdkMessageRequestField {0}", str);
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name, "Message" }
                    , { SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.name, "RequestName" }
                    , { SdkMessageRequestField.Schema.Attributes.position, "Position" }
                    , { SdkMessageRequestField.Schema.Attributes.name, "Name" }
                    , { SdkMessageRequestField.Schema.Attributes.publicname, "PublicName" }
                    , { SdkMessageRequestField.Schema.Attributes.clrparser, "ClrParser" }
                    , { SdkMessageRequestField.Schema.Attributes.parser, "Parser" }
                    , { SdkMessageRequestField.Schema.Attributes.optional, "Optional" }
                    , { SdkMessageRequestField.Schema.Attributes.customizationlevel, "CustomizationLevel" }
                };
        }
    }
}
