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
    public class MobileOfflineProfileDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public MobileOfflineProfileDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.MobileOfflineProfile)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.MobileOfflineProfile;

        public override int ComponentTypeValue => (int)ComponentType.MobileOfflineProfile;

        public override string EntityLogicalName => MobileOfflineProfile.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => MobileOfflineProfile.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    MobileOfflineProfile.Schema.Attributes.name
                    , MobileOfflineProfile.Schema.Attributes.selectedentitymetadata
                    , MobileOfflineProfile.Schema.Attributes.mobileofflineprofileid
                    , MobileOfflineProfile.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "SelectedEntityMetadata", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<MobileOfflineProfile>();

            List<string> values = new List<string>();

            string typeName = entity.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.mailmergetype) ? entity.FormattedValues[MailMergeTemplate.Schema.Attributes.mailmergetype] : string.Empty;

            values.AddRange(new[]
            {
                entity.Name
                , entity.SelectedEntityMetadata
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
                    { MobileOfflineProfile.Schema.Attributes.name, "Name" }
                    , { MobileOfflineProfile.Schema.Attributes.selectedentitymetadata, "SelectedEntityMetadata" }
                    , { MobileOfflineProfile.Schema.EntityPrimaryIdAttribute, "Id" }
                    , { MobileOfflineProfile.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}