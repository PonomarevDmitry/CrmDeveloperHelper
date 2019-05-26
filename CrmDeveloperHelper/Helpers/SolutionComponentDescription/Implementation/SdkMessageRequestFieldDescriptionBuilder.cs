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
    public class SdkMessageRequestFieldDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SdkMessageRequestFieldDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessageRequestField)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessageRequestField;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessageRequestField;

        public override string EntityLogicalName => SdkMessageRequestField.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessageRequestField.EntityPrimaryIdAttribute;

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
                    , SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid
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
                        LinkToAttributeName = SdkMessageRequest.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid,

                        Columns = new ColumnSet(SdkMessageRequest.Schema.Attributes.name),

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                                LinkToEntityName = SdkMessagePair.EntityLogicalName,
                                LinkToAttributeName = SdkMessagePair.EntityPrimaryIdAttribute,

                                EntityAlias = SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                                LinkEntities =
                                {
                                    new LinkEntity()
                                    {
                                        LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                                        LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                                        LinkToEntityName = SdkMessage.EntityLogicalName,
                                        LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                                        EntityAlias = SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid,

                                        Columns = new ColumnSet(SdkMessage.Schema.Attributes.name, SdkMessage.Schema.Attributes.categoryname),
                                    },
                                },
                            },
                        },
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageRequestField.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequestField.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageRequestField.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequestField.Schema.Attributes.supportingsolutionid,

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
            handler.SetHeader("Message", "MessageCategory", "RequestName", "Position", "Name", "PublicName", "ClrParser", "Parser", "Optional", "CustomizationLevel", "Behavior");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SdkMessageRequestField>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.categoryname)
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.name)
                , entity.Position.ToString()
                , entity.Name
                , entity.PublicName
                , entity.ClrParser
                , entity.Parser
                , entity.Optional.ToString()
                , entity.CustomizationLevel.ToString()
                , behavior
            });

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name, "Message" }
                    , { SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.categoryname, "MessageCategory" }
                    , { SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.name, "RequestName" }
                    , { SdkMessageRequestField.Schema.Attributes.position, "Position" }
                    , { SdkMessageRequestField.Schema.Attributes.name, "Name" }
                    , { SdkMessageRequestField.Schema.Attributes.publicname, "PublicName" }
                    , { SdkMessageRequestField.Schema.Attributes.clrparser, "ClrParser" }
                    , { SdkMessageRequestField.Schema.Attributes.parser, "Parser" }
                    , { SdkMessageRequestField.Schema.Attributes.optional, "Optional" }
                    , { SdkMessageRequestField.Schema.Attributes.customizationlevel, "CustomizationLevel" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }

        public override IEnumerable<SolutionComponent> GetLinkedComponents(SolutionComponent solutionComponent)
        {
            var result = new List<SolutionComponent>();

            var entity = GetEntity<SdkMessageRequestField>(solutionComponent.ObjectId.Value);

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
            }

            return result;
        }
    }
}
