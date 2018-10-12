using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class MailMergeTemplateDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public MailMergeTemplateDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.MailMergeTemplate)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.MailMergeTemplate;

        public override int ComponentTypeValue => (int)ComponentType.MailMergeTemplate;

        public override string EntityLogicalName => MailMergeTemplate.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => MailMergeTemplate.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    MailMergeTemplate.Schema.Attributes.templatetypecode
                    , MailMergeTemplate.Schema.Attributes.name
                    , MailMergeTemplate.Schema.Attributes.mailmergetype
                    , MailMergeTemplate.Schema.Attributes.ismanaged
                    , MailMergeTemplate.Schema.Attributes.iscustomizable
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<MailMergeTemplate>(components.Select(c => c.ObjectId));

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

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var mailMergeTemplate = GetEntity<MailMergeTemplate>(component.ObjectId.Value);

            if (mailMergeTemplate != null)
            {
                string name = mailMergeTemplate.Name;

                string entityName = mailMergeTemplate.TemplateTypeCode;

                string typeName = string.Empty;

                if (mailMergeTemplate.Contains(MailMergeTemplate.Schema.Attributes.mailmergetype) && mailMergeTemplate[MailMergeTemplate.Schema.Attributes.mailmergetype] != null)
                {
                    typeName = mailMergeTemplate.FormattedValues[MailMergeTemplate.Schema.Attributes.mailmergetype];
                }

                return string.Format("MailMergeTemplate {0}    TemplateTypeCode {1}    MailMergeType {2}    IsManaged {3}    SolutionName {4}"
                    , name
                    , entityName
                    , typeName
                    , mailMergeTemplate.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(mailMergeTemplate, "solution.uniquename")
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