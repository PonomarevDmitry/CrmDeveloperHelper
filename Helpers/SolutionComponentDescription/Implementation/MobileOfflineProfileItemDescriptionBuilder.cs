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

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<MobileOfflineProfileItem>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("Name", "SelectedEntityMetadata", "EntityObjectTypeCode", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.SelectedEntityTypeCode
                    , entity.EntityObjectTypeCode.ToString()
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var mobileOfflineProfileItem = GetEntity<MobileOfflineProfileItem>(component.ObjectId.Value);

            if (mobileOfflineProfileItem != null)
            {
                return string.Format("Name {0}    SelectedEntityMetadata {1}    EntityObjectTypeCode {2}    Id {3}    IsManaged {4}    SolutionName {5}"
                     , mobileOfflineProfileItem.Name
                     , mobileOfflineProfileItem.SelectedEntityTypeCode
                     , mobileOfflineProfileItem.EntityObjectTypeCode.ToString()
                     , mobileOfflineProfileItem.Id.ToString()
                     , mobileOfflineProfileItem.IsManaged.ToString()
                     , EntityDescriptionHandler.GetAttributeString(mobileOfflineProfileItem, "solution.uniquename")
                     );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
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