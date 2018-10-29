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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withUrls, bool withManaged, bool withSolutionInfo, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Message", "Name", "PrimaryObjectTypeCode", "CustomizationLevel");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, bool withUrls, bool withManaged, bool withSolutionInfo, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SdkMessageRequest>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                , entity.Name
                , entity.PrimaryObjectTypeCode
                , entity.CustomizationLevel.ToString()
            });

            return values;
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
