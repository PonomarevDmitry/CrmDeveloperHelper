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

        public override string EntityPrimaryIdAttribute => SdkMessageRequest.EntityPrimaryIdAttribute;

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
                        LinkToAttributeName = SdkMessagePair.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                        Columns = new ColumnSet(),

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                                LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                                LinkToEntityName = SdkMessage.EntityLogicalName,
                                LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                                EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid,

                                Columns = new ColumnSet(SdkMessage.Schema.Attributes.name, SdkMessage.Schema.Attributes.categoryname),
                            },
                        },
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.supportingsolutionid,

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
            handler.SetHeader("Message", "MessageCategory", "Namespace", "Endpoint", "Name", "PrimaryObjectTypeCode", "CustomizationLevel", "Behavior");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SdkMessageRequest>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.categoryname)
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.@namespace )
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.endpoint)
                , entity.Name
                , entity.PrimaryObjectTypeCode
                , entity.CustomizationLevel.ToString()
                , behavior
            });

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
            {
                { SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name, "Message" }
                , { SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.categoryname, "MessageCategory" }
                , { SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.@namespace, "Namespace" }
                , { SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.endpoint, "Endpoint" }
                , { SdkMessageRequest.Schema.Attributes.name, "Name" }
                , { SdkMessageRequest.Schema.Attributes.primaryobjecttypecode, "PrimaryObjectTypeCode" }
                , { SdkMessageRequest.Schema.Attributes.customizationlevel, "CustomizationLevel" }
                , { "solution.uniquename", "SolutionName" }
                , { "solution.ismanaged", "SolutionIsManaged" }
                , { "suppsolution.uniquename", "SupportingName" }
                , { "suppsolution.ismanaged", "SupportingIsManaged" }
            };
        }

        public override IEnumerable<SolutionComponent> GetLinkedComponents(SolutionComponent solutionComponent)
        {
            var result = new List<SolutionComponent>();

            var entity = GetEntity<SdkMessageRequest>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                if (entity.SdkMessagePairId != null)
                {
                    result.Add(new SolutionComponent()
                    {
                        ObjectId = entity.SdkMessagePairId.Id,
                        ComponentType = new OptionSetValue((int)ComponentType.SdkMessagePair),
                    });
                }

                string messageIdName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid;

                if (entity.Attributes.ContainsKey(messageIdName)
                    && entity.Attributes[messageIdName] != null
                    && entity.Attributes[messageIdName] is AliasedValue aliasedValue
                    && aliasedValue.Value != null
                    && aliasedValue.Value is EntityReference entityReference
                )
                {
                    result.Add(new SolutionComponent()
                    {
                        ObjectId = entityReference.Id,
                        ComponentType = new OptionSetValue((int)ComponentType.SdkMessage),
                    });
                }
            }

            return result;
        }
    }
}
