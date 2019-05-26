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
    public class SdkMessageResponseDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SdkMessageResponseDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessageResponse)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessageResponse;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessageResponse;

        public override string EntityLogicalName => SdkMessageResponse.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessageResponse.EntityPrimaryIdAttribute;

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

                        LinkToEntityName = SdkMessageRequest.EntityLogicalName,
                        LinkToAttributeName = SdkMessageRequest.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid,

                        Columns = new ColumnSet(SdkMessageRequest.Schema.Attributes.name, SdkMessageRequest.Schema.Attributes.sdkmessagepairid),

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                                LinkToEntityName = SdkMessagePair.EntityLogicalName,
                                LinkToAttributeName = SdkMessagePair.EntityPrimaryIdAttribute,

                                EntityAlias = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid ,

                                Columns = new ColumnSet(SdkMessagePair.Schema.Attributes.@namespace, SdkMessagePair.Schema.Attributes.endpoint, SdkMessagePair.Schema.Attributes.sdkmessageid),

                                LinkEntities =
                                {
                                    new LinkEntity()
                                    {
                                        LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                                        LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                                        LinkToEntityName = SdkMessage.EntityLogicalName,
                                        LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                                        EntityAlias = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid,

                                        Columns = new ColumnSet(SdkMessage.Schema.Attributes.name, SdkMessage.Schema.Attributes.categoryname),
                                    },
                                },
                            },
                        },
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageResponse.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageResponse.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageResponse.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageResponse.Schema.Attributes.supportingsolutionid,

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
            handler.SetHeader("Message", "MessageCategory", "RequestName", "Namespace", "Endpoint", "CustomizationLevel", "Behavior");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SdkMessageResponse>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.categoryname)
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.name)
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.@namespace)
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.endpoint)
                , entity.CustomizationLevel.ToString()
                , behavior
            });

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name, "Message" }
                    , { SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.categoryname, "MessageCategory" }
                    , { SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.name, "RequestName" }
                    , { SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.@namespace, "Namespace" }
                    , { SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.endpoint, "Endpoint" }
                    , { SdkMessageResponse.Schema.Attributes.customizationlevel, "CustomizationLevel" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }

        public override IEnumerable<SolutionComponent> GetLinkedComponents(SolutionComponent solutionComponent)
        {
            var result = new List<SolutionComponent>();

            var entity = GetEntity<SdkMessageResponse>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                if (entity.SdkMessageRequestId != null)
                {
                    result.Add(new SolutionComponent()
                    {
                        ObjectId = entity.SdkMessageRequestId.Id,
                        ComponentType = new OptionSetValue((int)ComponentType.SdkMessageRequest),
                    });
                }

                {
                    string pairIdName = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid;

                    if (entity.Attributes.ContainsKey(pairIdName)
                        && entity.Attributes[pairIdName] != null
                        && entity.Attributes[pairIdName] is AliasedValue aliasedValue
                        && aliasedValue.Value != null
                        && aliasedValue.Value is EntityReference entityReference
                    )
                    {
                        result.Add(new SolutionComponent()
                        {
                            ObjectId = entityReference.Id,
                            ComponentType = new OptionSetValue((int)ComponentType.SdkMessagePair),
                        });
                    }
                }

                {
                    string messageIdName = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid;

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
            }

            return result;
        }
    }
}
