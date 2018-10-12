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
    public class KbArticleTemplateDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public KbArticleTemplateDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.KbArticleTemplate)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.KbArticleTemplate;

        public override int ComponentTypeValue => (int)ComponentType.KbArticleTemplate;

        public override string EntityLogicalName => KbArticleTemplate.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => KbArticleTemplate.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    KbArticleTemplate.Schema.Attributes.title
                    , KbArticleTemplate.Schema.Attributes.ismanaged
                    , KbArticleTemplate.Schema.Attributes.iscustomizable
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<KbArticleTemplate>(components.Select(c => c.ObjectId));

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

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var kbArticleTemplate = GetEntity<KbArticleTemplate>(component.ObjectId.Value);

            if (kbArticleTemplate != null)
            {
                string title = kbArticleTemplate.Title;

                return string.Format("KBArticleTemplate {0}    IsManaged {1}    SolutionName {2}"
                    , title
                    , kbArticleTemplate.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(kbArticleTemplate, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<KbArticleTemplate>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return entity.Title;
            }

            return base.GetName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { KbArticleTemplate.Schema.Attributes.title, "Title" }
                    , { KbArticleTemplate.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { KbArticleTemplate.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}