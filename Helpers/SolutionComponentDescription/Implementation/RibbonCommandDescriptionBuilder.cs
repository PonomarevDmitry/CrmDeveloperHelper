using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class RibbonCommandDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RibbonCommandDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RibbonCommand)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RibbonCommand;

        public override int ComponentTypeValue => (int)ComponentType.RibbonCommand;

        public override string EntityLogicalName => RibbonCommand.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RibbonCommand.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RibbonCommand.Schema.Attributes.entity
                    , RibbonCommand.Schema.Attributes.command
                    , RibbonCommand.Schema.Attributes.ribboncommandid
                    , RibbonCommand.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<RibbonCommand>(components.Select(c => c.ObjectId));

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

            table.SetHeader("Entity", "Command", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Guid");

            foreach (var entity in list)
            {
                string entityName = entity.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                string command = entity.Command;

                table.AddLine(entityName
                    , command
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , entity.Id.ToString()
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var ribbonCommand = GetEntity<RibbonCommand>(component.ObjectId.Value);

            if (ribbonCommand != null)
            {
                string entityName = ribbonCommand.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                string command = ribbonCommand.Command;

                return string.Format("Entity {0}    Command {1}    IsManaged {2}    SolutionName {3}"
                    , entityName
                    , command
                    , ribbonCommand.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(ribbonCommand, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RibbonCommand>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.Entity ?? "ApplicationRibbon", entity.Command);
            }

            return component.ObjectId.ToString();
        }
    }
}