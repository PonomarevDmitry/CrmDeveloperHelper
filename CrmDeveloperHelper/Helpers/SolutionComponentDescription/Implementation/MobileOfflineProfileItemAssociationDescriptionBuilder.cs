using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class MobileOfflineProfileItemAssociationDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public MobileOfflineProfileItemAssociationDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.MobileOfflineProfileItemAssociation)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.MobileOfflineProfileItemAssociation;

        public override int ComponentTypeValue => (int)ComponentType.MobileOfflineProfileItemAssociation;

        public override string EntityLogicalName => MobileOfflineProfileItemAssociation.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => MobileOfflineProfileItemAssociation.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
            (
                MobileOfflineProfileItemAssociation.Schema.Attributes.mobileofflineprofileitemid
                , MobileOfflineProfileItemAssociation.Schema.Attributes.name
                , MobileOfflineProfileItemAssociation.Schema.Attributes.relationshipname
                , MobileOfflineProfileItemAssociation.Schema.Attributes.relationshipdisplayname
                , MobileOfflineProfileItemAssociation.Schema.Attributes.isvalidated
                , MobileOfflineProfileItemAssociation.Schema.Attributes.ismanaged
            );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader(
                MobileOfflineProfileItemAssociation.Schema.Headers.mobileofflineprofileitemid
                , MobileOfflineProfileItemAssociation.Schema.Headers.name
                , MobileOfflineProfileItemAssociation.Schema.Headers.relationshipname
                , MobileOfflineProfileItemAssociation.Schema.Headers.relationshipdisplayname
                , MobileOfflineProfileItemAssociation.Schema.Headers.isvalidated
                , "Behavior"
            );

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<MobileOfflineProfileItemAssociation>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.MobileOfflineProfileItemId?.Name
                , entity.Name
                , entity.RelationshipName
                , entity.RelationshipDisplayName
                , entity.FormattedValues[MobileOfflineProfileItemAssociation.Schema.Attributes.isvalidated]
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
            {
                { MobileOfflineProfileItemAssociation.Schema.Attributes.mobileofflineprofileitemid, MobileOfflineProfileItemAssociation.Schema.Headers.mobileofflineprofileitemid }
                , { MobileOfflineProfileItemAssociation.Schema.Attributes.name, MobileOfflineProfileItemAssociation.Schema.Headers.name }
                , { MobileOfflineProfileItemAssociation.Schema.Attributes.relationshipname, MobileOfflineProfileItemAssociation.Schema.Headers.relationshipname }
                , { MobileOfflineProfileItemAssociation.Schema.Attributes.relationshipdisplayname, MobileOfflineProfileItemAssociation.Schema.Headers.relationshipdisplayname }
                , { MobileOfflineProfileItemAssociation.Schema.Attributes.isvalidated, MobileOfflineProfileItemAssociation.Schema.Headers.isvalidated }
                , { MobileOfflineProfileItemAssociation.Schema.Attributes.ismanaged, MobileOfflineProfileItemAssociation.Schema.Headers.ismanaged }
                , { "solution.uniquename", "SolutionName" }
                , { "solution.ismanaged", "SolutionIsManaged" }
                , { "suppsolution.uniquename", "SupportingName" }
                , { "suppsolution.ismanaged", "SupportingIsManaged" }
            };
        }
    }
}
