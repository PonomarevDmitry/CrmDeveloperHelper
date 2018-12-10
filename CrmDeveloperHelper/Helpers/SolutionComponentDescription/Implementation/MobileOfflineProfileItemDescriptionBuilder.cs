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
    public class MobileOfflineProfileItemDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public MobileOfflineProfileItemDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.MobileOfflineProfileItem)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.MobileOfflineProfileItem;

        public override int ComponentTypeValue => (int)ComponentType.MobileOfflineProfileItem;

        public override string EntityLogicalName => MobileOfflineProfileItem.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => MobileOfflineProfileItem.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    MobileOfflineProfileItem.Schema.Attributes.name
                    , MobileOfflineProfileItem.Schema.Attributes.selectedentitytypecode
                    , MobileOfflineProfileItem.Schema.Attributes.entityobjecttypecode
                    , MobileOfflineProfileItem.Schema.Attributes.mobileofflineprofileitemid
                    , MobileOfflineProfileItem.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "SelectedEntityTypeCode", "EntityObjectTypeCode", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<MobileOfflineProfileItem>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.SelectedEntityTypeCode
                , entity.EntityObjectTypeCode.ToString()
                , entity.Id.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { MobileOfflineProfileItem.Schema.Attributes.name, "Name" }
                    , { MobileOfflineProfileItem.Schema.Attributes.selectedentitytypecode, "SelectedEntityTypeCode" }
                    , { MobileOfflineProfileItem.Schema.Attributes.entityobjecttypecode, "EntityObjectTypeCode" }
                    , { MobileOfflineProfileItem.Schema.EntityPrimaryIdAttribute, "Id" }
                    , { MobileOfflineProfileItem.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}