using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class SolutionComponentDescriptor
    {
        private void GenerateDescriptionHierarchyRule(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<HierarchyRule>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("PrimaryEntityLogicalName", "Name", "Description", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string entityName = entity.PrimaryEntityLogicalName;
                string name = entity.Name;

                string desc = entity.Description;

                table.AddLine(entityName
                    , name
                    , desc
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionHierarchyRuleSingle(HierarchyRule entity, SolutionComponent component)
        {
            if (entity != null)
            {
                StringBuilder title = new StringBuilder();

                string entityName = entity.PrimaryEntityLogicalName;
                string name = entity.Name;

                string desc = entity.Description;

                title.AppendFormat("HierarchyRule {0}", entityName);

                if (!string.IsNullOrEmpty(name))
                {
                    title.AppendFormat("        Name '{0}'", name);
                }

                if (!string.IsNullOrEmpty(desc))
                {
                    title.AppendFormat("        Description '{0}'", desc);
                }

                title.AppendFormat("        IsManaged '{0}'", entity.IsManaged.ToString());

                title.AppendFormat("        SolutionName '{0}'", EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename"));

                return title.ToString();
            }

            return component.ToString();
        }

        private void GenerateDescriptionRoutingRule(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<RoutingRule>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("Name", "Workflow", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var name = entity.Name;

                EntityReference workflow = entity.WorkflowId;

                string workflowName = string.Empty;

                if (workflow != null)
                {
                    workflowName = workflow.Name;
                }

                table.AddLine(name
                    , workflowName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionRoutingRuleSingle(RoutingRule entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string name = entity.Name;
                EntityReference workflow = entity.WorkflowId;

                StringBuilder title = new StringBuilder();

                title.AppendFormat("RoutingRule {0}", name);

                if (workflow != null)
                {
                    title.AppendFormat("    Workflow '{0}'", workflow.Name);
                }

                title.AppendFormat("    IsManaged '{0}'", entity.IsManaged.ToString());

                title.AppendFormat("    SolutionName '{0}'", EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename"));

                return title.ToString();
            }

            return component.ToString();
        }

        private void GenerateDescriptionRoutingRuleItem(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<RoutingRuleItem>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("RoutingRuleName", "Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string ruleName = entity.RoutingRuleId.Name;
                string name = entity.Name;

                table.AddLine(
                    ruleName
                    , name
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionRoutingRuleItemSingle(RoutingRuleItem entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string ruleName = entity.RoutingRuleId.Name;
                string name = entity.Name;

                return string.Format("RoutingRuleName {0}    Name {1}    IsManaged {2}    SolutionName {3}"
                    , ruleName
                    , name
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionConvertRule(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<ConvertRule>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string name = entity.Name;

                table.AddLine(name
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionConvertRuleSingle(ConvertRule entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string name = entity.Name;

                return string.Format("ConvertRule {0}    IsManaged {1}    SolutionName {2}", name
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionConvertRuleItem(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<ConvertRuleItem>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("ConvertRuleName", "Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string convertRuleName = entity.ConvertRuleId.Name;
                string name = entity.Name;

                table.AddLine(
                    convertRuleName
                    , name
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionConvertRuleItemSingle(ConvertRuleItem entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string convertRuleName = entity.ConvertRuleId.Name;
                string name = entity.Name;

                return string.Format("ConvertRuleName {0}    Name {1}    IsManaged {2}    SolutionName {3}"
                    , convertRuleName
                    , name
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionSimilarityRule(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<SimilarityRule>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            handler.SetHeader("Entity", "Name", "MatchingEntityName", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.BaseEntityName
                    , entity.Name
                    , entity.MatchingEntityName
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

        private string GenerateDescriptionSimilarityRuleSingle(SimilarityRule entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("Entity {0}    Name {1}    MatchingEntityName {2}    Id {3}    IsManaged {4}    SolutionName {5}"
                    , entity.BaseEntityName
                    , entity.Name
                    , entity.MatchingEntityName
                    , entity.Id.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }
    }
}