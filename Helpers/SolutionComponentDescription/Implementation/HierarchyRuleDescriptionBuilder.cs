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
    public class HierarchyRuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public HierarchyRuleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.HierarchyRule)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.HierarchyRule;

        public override int ComponentTypeValue => (int)ComponentType.HierarchyRule;

        public override string EntityLogicalName => HierarchyRule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => HierarchyRule.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    HierarchyRule.Schema.Attributes.primaryentitylogicalname
                    , HierarchyRule.Schema.Attributes.name
                    , HierarchyRule.Schema.Attributes.description
                    , HierarchyRule.Schema.Attributes.ismanaged
                    , HierarchyRule.Schema.Attributes.iscustomizable
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<HierarchyRule>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            var table = new FormatTextTableHandler();
            table.SetHeader("PrimaryEntityLogicalName", "Name", "Description", "IsManaged", "IsCustomizable", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string entityName = entity.PrimaryEntityLogicalName;
                string name = entity.Name;

                string desc = entity.Description;

                table.AddLine(entityName
                    , name
                    , desc
                    , entity.IsManaged.ToString()
                    , entity.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var hierarchyRule = GetEntity<HierarchyRule>(component.ObjectId.Value);

            if (hierarchyRule != null)
            {
                StringBuilder title = new StringBuilder();

                string entityName = hierarchyRule.PrimaryEntityLogicalName;
                string name = hierarchyRule.Name;

                string desc = hierarchyRule.Description;

                title.AppendFormat("HierarchyRule {0}", entityName);

                if (!string.IsNullOrEmpty(name))
                {
                    title.AppendFormat("        Name '{0}'", name);
                }

                if (!string.IsNullOrEmpty(desc))
                {
                    title.AppendFormat("        Description '{0}'", desc);
                }

                title.AppendFormat("        IsManaged '{0}'", hierarchyRule.IsManaged.ToString());
                title.AppendFormat("        IsCustomizable '{0}'", hierarchyRule.IsCustomizable?.Value.ToString());

                title.AppendFormat("        SolutionName '{0}'", EntityDescriptionHandler.GetAttributeString(hierarchyRule, "solution.uniquename"));

                return title.ToString();
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { HierarchyRule.Schema.Attributes.primaryentitylogicalname, "Entity" }
                    , { HierarchyRule.Schema.Attributes.name, "Name" }
                    , { HierarchyRule.Schema.Attributes.description, "Description" }
                    , { HierarchyRule.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { HierarchyRule.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}