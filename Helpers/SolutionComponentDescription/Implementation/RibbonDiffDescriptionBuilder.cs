using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class RibbonDiffDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RibbonDiffDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RibbonDiff)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RibbonDiff;

        public override int ComponentTypeValue => (int)ComponentType.RibbonDiff;

        public override string EntityLogicalName => RibbonDiff.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RibbonDiff.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RibbonDiff.Schema.Attributes.entity
                    , RibbonDiff.Schema.Attributes.diffid
                    , RibbonDiff.Schema.Attributes.tabid
                    , RibbonDiff.Schema.Attributes.contextgroupid
                    , RibbonDiff.Schema.Attributes.sequence
                    , RibbonDiff.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<RibbonDiff>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler table = new FormatTextTableHandler();

            table.SetHeader("Entity", "DiffId", "DiffType", "TabId", "ContextGroupId", "Sequence", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string entityName = entity.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }


                entity.FormattedValues.TryGetValue(RibbonDiff.Schema.Attributes.difftype, out string difftype);

                table.AddLine(entityName
                    , entity.DiffId
                    , difftype
                    , entity.TabId
                    , entity.ContextGroupId?.ToString()
                    , entity.Sequence?.ToString()
                    , entity.IsManaged?.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var ribbonDiff = GetEntity<RibbonDiff>(component.ObjectId.Value);

            if (ribbonDiff != null)
            {
                string entityName = ribbonDiff.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                return string.Format("Entity {0}    DiffId {1}    DiffType {2}    TabId {3}    ContextGroupId {4}    Sequence {5}    IsManaged {6}    SolutionName {7}"
                    , entityName
                    , ribbonDiff.DiffId
                    , ribbonDiff.FormattedValues[RibbonDiff.Schema.Attributes.difftype]
                    , ribbonDiff.TabId
                    , ribbonDiff.ContextGroupId.ToString()
                    , ribbonDiff.Sequence.ToString()
                    , ribbonDiff.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(ribbonDiff, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RibbonDiff>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.Entity ?? "ApplicationRibbon", entity.DiffId);
            }

            return component.ObjectId.ToString();
        }
    }
}