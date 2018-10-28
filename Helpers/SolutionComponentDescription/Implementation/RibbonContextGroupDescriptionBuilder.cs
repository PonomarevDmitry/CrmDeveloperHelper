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
    public class RibbonContextGroupDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RibbonContextGroupDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RibbonContextGroup)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RibbonContextGroup;

        public override int ComponentTypeValue => (int)ComponentType.RibbonContextGroup;

        public override string EntityLogicalName => RibbonContextGroup.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RibbonContextGroup.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RibbonContextGroup.Schema.Attributes.entity
                    , RibbonContextGroup.Schema.Attributes.contextgroupid
                    , RibbonContextGroup.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<RibbonContextGroup>(components.Select(c => c.ObjectId));

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

            table.SetHeader("Entity", "ContextGroupId", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string entityName = entity.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                string contextgroupid = entity.ContextGroupId;

                table.AddLine(entityName
                    , contextgroupid
                    , entity.IsManaged.ToString()
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
            var ribbonContextGroup = GetEntity<RibbonContextGroup>(component.ObjectId.Value);

            if (ribbonContextGroup != null)
            {
                string entityName = ribbonContextGroup.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                string contextgroupid = ribbonContextGroup.ContextGroupId;

                return string.Format("Entity {0}    ContextGroupId {1}    IsManaged {2}    SolutionName {3}"
                    , entityName
                    , contextgroupid
                    , ribbonContextGroup.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(ribbonContextGroup, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RibbonContextGroup>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.Entity ?? "ApplicationRibbon", entity.ContextGroupId);
            }

            return base.GetName(component);
        }

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            return;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { RibbonContextGroup.Schema.Attributes.entity, "Entity" }
                    , { RibbonContextGroup.Schema.Attributes.contextgroupid, "ContextGroupId" }
                    , { RibbonContextGroup.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}