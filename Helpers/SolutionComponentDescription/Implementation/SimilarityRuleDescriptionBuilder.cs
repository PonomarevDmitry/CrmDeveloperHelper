using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class SimilarityRuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SimilarityRuleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SimilarityRule)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SimilarityRule;

        public override int ComponentTypeValue => (int)ComponentType.SimilarityRule;

        public override string EntityLogicalName => SimilarityRule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SimilarityRule.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SimilarityRule.Schema.Attributes.baseentityname
                    , SimilarityRule.Schema.Attributes.name
                    , SimilarityRule.Schema.Attributes.matchingentityname
                    , SimilarityRule.Schema.Attributes.similarityruleid
                    , SimilarityRule.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<SimilarityRule>(components.Select(c => c.ObjectId));

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

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var similarityRule = GetEntity<SimilarityRule>(component.ObjectId.Value);

            if (similarityRule != null)
            {
                return string.Format("Entity {0}    Name {1}    MatchingEntityName {2}    Id {3}    IsManaged {4}    SolutionName {5}"
                    , similarityRule.BaseEntityName
                    , similarityRule.Name
                    , similarityRule.MatchingEntityName
                    , similarityRule.Id.ToString()
                    , similarityRule.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(similarityRule, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var fieldPermission = GetEntity<FieldPermission>(component.ObjectId.Value);

            if (fieldPermission != null)
            {

            }

            return component.ObjectId.ToString();
        }
    }
}