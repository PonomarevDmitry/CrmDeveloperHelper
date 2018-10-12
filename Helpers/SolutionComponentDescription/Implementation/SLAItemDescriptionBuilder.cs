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
    public class SLAItemDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SLAItemDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SLAItem)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SLAItem;

        public override int ComponentTypeValue => (int)ComponentType.SLAItem;

        public override string EntityLogicalName => SLAItem.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SLAItem.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SLAItem.Schema.Attributes.slaid
                    , SLAItem.Schema.Attributes.name
                    , SLAItem.Schema.Attributes.relatedfield
                    , SLAItem.Schema.Attributes.slaitemid
                    , SLAItem.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<SLAItem>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("SLA", "Name", "RelatedField", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.SLAId.Name
                    , entity.Name
                    , entity.RelatedField
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
            var slaItem = GetEntity<SLAItem>(component.ObjectId.Value);

            if (slaItem != null)
            {
                return string.Format("SLA {0}    Name {1}    RelatedField {2}    Id {3}    IsManaged {4}    SolutionName {5}"
                    , slaItem.SLAId.Name
                    , slaItem.Name
                    , slaItem.RelatedField
                    , slaItem.Id.ToString()
                    , slaItem.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(slaItem, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SLAItem.Schema.Attributes.slaid, "SLA" }
                    , { SLAItem.Schema.Attributes.name, "Name" }
                    , { SLAItem.Schema.Attributes.relatedfield, "RelatedField" }
                    , { SLAItem.Schema.EntityPrimaryIdAttribute, "Id" }
                    , { SLAItem.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}