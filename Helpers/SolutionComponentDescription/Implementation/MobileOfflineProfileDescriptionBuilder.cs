using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<MobileOfflineProfile>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("Name", "SelectedEntityMetadata", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.SelectedEntityMetadata
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
            var mobileOfflineProfile = GetEntity<MobileOfflineProfile>(component.ObjectId.Value);

            if (mobileOfflineProfile != null)
            {
                return string.Format("Name {0}    SelectedEntityMetadata {1}    Id {2}    IsManaged {3}    SolutionName {4}"
                    , mobileOfflineProfile.Name
                    , mobileOfflineProfile.SelectedEntityMetadata
                    , mobileOfflineProfile.Id.ToString()
                    , mobileOfflineProfile.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(mobileOfflineProfile, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<FieldPermission>(component.ObjectId.Value);

            if (entity != null)
            {

            }

            return component.ObjectId.ToString();
        }
    }
}