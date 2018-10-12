using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class RibbonTabToCommandMapDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RibbonTabToCommandMapDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RibbonTabToCommandMap)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RibbonTabToCommandMap;

        public override int ComponentTypeValue => (int)ComponentType.RibbonTabToCommandMap;

        public override string EntityLogicalName => RibbonTabToCommandMap.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RibbonTabToCommandMap.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RibbonTabToCommandMap.Schema.Attributes.entity
                    , RibbonTabToCommandMap.Schema.Attributes.tabid
                    , RibbonTabToCommandMap.Schema.Attributes.controlid
                    , RibbonTabToCommandMap.Schema.Attributes.command
                    , RibbonTabToCommandMap.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<RibbonTabToCommandMap>(components.Select(c => c.ObjectId));

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

            table.SetHeader("Entity", "TabId", "ControlId", "Command", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string entityName = entity.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                table.AddLine(entityName
                    , entity.TabId
                    , entity.ControlId
                    , entity.Command
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
            var ribbonTabToCommandMap = GetEntity<RibbonTabToCommandMap>(component.ObjectId.Value);

            if (ribbonTabToCommandMap != null)
            {
                string entityName = ribbonTabToCommandMap.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                return string.Format("Entity {0}    TabId {1}    ControlId {2}    Command {3}    IsManaged {4}    SolutionName {5}"
                    , entityName
                    , ribbonTabToCommandMap.TabId
                    , ribbonTabToCommandMap.ControlId
                    , ribbonTabToCommandMap.Command
                    , ribbonTabToCommandMap.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(ribbonTabToCommandMap, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RibbonTabToCommandMap>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.Entity ?? "ApplicationRibbon", entity.TabId);
            }

            return component.ObjectId.ToString();
        }
    }
}