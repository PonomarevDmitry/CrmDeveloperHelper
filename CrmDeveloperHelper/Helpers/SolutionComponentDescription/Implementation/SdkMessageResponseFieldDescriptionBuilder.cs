﻿using Microsoft.Xrm.Sdk;
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
    public class SdkMessageResponseFieldDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SdkMessageResponseFieldDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SdkMessageResponseField)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SdkMessageResponseField;

        public override int ComponentTypeValue => (int)ComponentType.SdkMessageResponseField;

        public override string EntityLogicalName => SdkMessageResponseField.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SdkMessageResponseField.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SdkMessageResponseField.Schema.Attributes.position
                    , SdkMessageResponseField.Schema.Attributes.name
                    , SdkMessageResponseField.Schema.Attributes.sdkmessageresponseid
                    , SdkMessageResponseField.Schema.Attributes.publicname
                    , SdkMessageResponseField.Schema.Attributes.value
                    , SdkMessageResponseField.Schema.Attributes.clrformatter
                    , SdkMessageResponseField.Schema.Attributes.formatter
                    , SdkMessageResponseField.Schema.Attributes.parameterbindinginformation
                    , SdkMessageResponseField.Schema.Attributes.customizationlevel
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
                        LinkFromEntityName = SdkMessageResponseField.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageResponseField.Schema.Attributes.sdkmessageresponseid,

                        LinkToEntityName = SdkMessageResponse.EntityLogicalName,
                        LinkToAttributeName = SdkMessageResponse.EntityPrimaryIdAttribute,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageResponse.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid,

                                LinkToEntityName = SdkMessageRequest.EntityLogicalName,
                                LinkToAttributeName = SdkMessageRequest.EntityPrimaryIdAttribute,

                                EntityAlias = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid,

                                Columns = new ColumnSet(SdkMessageRequest.Schema.Attributes.name),

                                LinkEntities =
                                {
                                    new LinkEntity()
                                    {
                                        LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                                        LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                                        LinkToEntityName = SdkMessagePair.EntityLogicalName,
                                        LinkToAttributeName = SdkMessagePair.EntityPrimaryIdAttribute,

                                        LinkEntities =
                                        {
                                            new LinkEntity()
                                            {
                                                LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                                                LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                                                LinkToEntityName = SdkMessage.EntityLogicalName,
                                                LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                                                EntityAlias = SdkMessagePair.Schema.Attributes.sdkmessageid,

                                                Columns = new ColumnSet(SdkMessage.Schema.Attributes.name, SdkMessage.Schema.Attributes.categoryname),
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageResponseField.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageResponseField.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageResponseField.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageResponseField.Schema.Attributes.supportingsolutionid,

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
            handler.SetHeader("Message", "MessageCategory", "RequestName", "Position", "Name", "PublicName", "Value", "ClrFormatter", "Formatter", "ParameterBindingInformation", "CustomizationLevel", "Behavior");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SdkMessageResponseField>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name)
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.categoryname)
                , EntityDescriptionHandler.GetAttributeString(entity, SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.name)
                , entity.Position.ToString()
                , entity.Name
                , entity.PublicName
                , entity.Value
                , entity.ClrFormatter
                , entity.Formatter
                , entity.ParameterBindingInformation
                , entity.CustomizationLevel.ToString()
                , behavior
            });

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name, "Message" }
                    , { SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.categoryname, "MessageCategory" }
                    , { SdkMessageResponse.Schema.Attributes.sdkmessagerequestid + "." + SdkMessageRequest.Schema.Attributes.name, "RequestName" }
                    , { SdkMessageResponseField.Schema.Attributes.position, "Position" }
                    , { SdkMessageResponseField.Schema.Attributes.name, "Name" }
                    , { SdkMessageResponseField.Schema.Attributes.publicname, "PublicName" }
                    , { SdkMessageResponseField.Schema.Attributes.value, "Value" }
                    , { SdkMessageResponseField.Schema.Attributes.clrformatter, "ClrFormatter" }
                    , { SdkMessageResponseField.Schema.Attributes.formatter, "Formatter" }
                    , { SdkMessageResponseField.Schema.Attributes.parameterbindinginformation, "ParameterBindingInformation" }
                    , { SdkMessageResponseField.Schema.Attributes.customizationlevel, "CustomizationLevel" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }

        public override IEnumerable<SolutionComponent> GetLinkedComponents(SolutionComponent solutionComponent)
        {
            var result = new List<SolutionComponent>();

            var entity = GetEntity<SdkMessageResponseField>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                if (entity.SdkMessageResponseId != null)
                {
                    result.Add(new SolutionComponent()
                    {
                        ObjectId = entity.SdkMessageResponseId.Id,
                        ComponentType = new OptionSetValue((int)ComponentType.SdkMessageResponse),
                    });
                }
            }

            return result;
        }
    }
}
