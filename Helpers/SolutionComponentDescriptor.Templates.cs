using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class SolutionComponentDescriptor
    {
        private void GenerateDescriptionContractTemplate(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<ContractTemplate>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string title = entity.Name;

                table.AddLine(title
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionContractTemplateSingle(ContractTemplate entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string title = entity.Name;

                return string.Format("ContractTemplate {0}    IsManaged {1}    SolutionName {2}"
                    , title
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionKBArticleTemplate(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<KbArticleTemplate>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("Title", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string title = entity.Title;

                table.AddLine(title
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionKBArticleTemplateSingle(KbArticleTemplate entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string title = entity.Title;

                return string.Format("KBArticleTemplate {0}    IsManaged {1}    SolutionName {2}"
                    , title
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionMailMergeTemplate(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<MailMergeTemplate>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            handler.SetHeader("TemplateTypeCode", "Name", "MailMergeType", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string entityName = entity.TemplateTypeCode;

                string name = entity.Name;

                string typeName = entity.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.mailmergetype) ? entity.FormattedValues[MailMergeTemplate.Schema.Attributes.mailmergetype] : string.Empty;

                handler.AddLine(entityName
                    , name
                    , typeName
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

        private string GenerateDescriptionMailMergeTemplateSingle(MailMergeTemplate entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string name = entity.Name;

                string entityName = entity.TemplateTypeCode;

                string typeName = string.Empty;

                if (entity.Contains(MailMergeTemplate.Schema.Attributes.mailmergetype) && entity[MailMergeTemplate.Schema.Attributes.mailmergetype] != null)
                {
                    typeName = entity.FormattedValues[MailMergeTemplate.Schema.Attributes.mailmergetype];
                }

                return string.Format("MailMergeTemplate {0}    TemplateTypeCode {1}    MailMergeType {2}    IsManaged {3}    SolutionName {4}"
                    , name
                    , entityName
                    , typeName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionEmailTemplate(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<Template>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            handler.SetHeader("TemplateTypeCode", "Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string entityName = entity.TemplateTypeCode;

                string name = entity.Title;

                handler.AddLine(entityName
                    , name
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

        private string GenerateDescriptionEmailTemplateSingle(Template entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string name = entity.Title;

                string type = entity.TemplateTypeCode;

                return string.Format("EmailTemplate {0}    TemplateTypeCode {1}    IsManaged {2}    SolutionName {3}", type, name
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }
    }
}