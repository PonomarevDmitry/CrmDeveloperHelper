using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class EmailTemplateDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public EmailTemplateDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.EmailTemplate)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.EmailTemplate;

        public override int ComponentTypeValue => (int)ComponentType.EmailTemplate;

        public override string EntityLogicalName => Template.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => Template.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    Template.Schema.Attributes.templatetypecode
                    , Template.Schema.Attributes.title
                    , Template.Schema.Attributes.ismanaged
                    , Template.Schema.Attributes.iscustomizable
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<Template>(components.Select(c => c.ObjectId));

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

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var template = GetEntity<Template>(component.ObjectId.Value);

            if (template != null)
            {
                string name = template.Title;

                string type = template.TemplateTypeCode;

                return string.Format("EmailTemplate {0}    TemplateTypeCode {1}    IsManaged {2}    SolutionName {3}", type, name
                    , template.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(template, "solution.uniquename")
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