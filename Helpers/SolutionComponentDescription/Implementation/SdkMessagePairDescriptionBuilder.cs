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
            };

            return query;
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Message", "Namespace", "Endpoint", "SdkMessageBindingInformation", "CustomizationLevel", "Behavior");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SdkMessagePair>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.SdkMessageId?.Name
                , entity.Namespace
                , entity.Endpoint
                , entity.SdkMessageBindingInformation
                , entity.CustomizationLevel.ToString()
                , behavior
            });

            return values;
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
