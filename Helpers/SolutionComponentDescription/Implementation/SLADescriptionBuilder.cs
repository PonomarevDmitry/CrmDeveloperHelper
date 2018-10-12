using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class SLADescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SLADescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SLA)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SLA;

        public override int ComponentTypeValue => (int)ComponentType.SLA;

        public override string EntityLogicalName => SLA.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SLA.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SLA.Schema.Attributes.objecttypecode
                    , SLA.Schema.Attributes.name
                    , SLA.Schema.Attributes.slaid
                    , SLA.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<SLA>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("Entity", "Name", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.FormattedValues.ContainsKey(SLA.Schema.Attributes.objecttypecode) ? entity.FormattedValues[SLA.Schema.Attributes.objecttypecode] : entity.ObjectTypeCode?.Value.ToString()
                    , entity.Name
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
            var sla = GetEntity<SLA>(component.ObjectId.Value);

            if (sla != null)
            {
                return string.Format("Entity {0}    Name {1}    Id {2}    IsManaged {3}    SolutionName {4}"
                    , sla.FormattedValues.ContainsKey(SLA.Schema.Attributes.objecttypecode) ? sla.FormattedValues[SLA.Schema.Attributes.objecttypecode] : sla.ObjectTypeCode?.Value.ToString()
                    , sla.Name
                    , sla.Id.ToString()
                    , sla.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(sla, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var fieldPermission = GetEntity<FieldPermission>(component.ObjectId.Value);

            if (fieldPermission != null)
            {

            }

            return component.ObjectId.ToString();
        }
    }
}