using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class ConvertRuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ConvertRuleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ConvertRule)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ConvertRule;

        public override int ComponentTypeValue => (int)ComponentType.ConvertRule;

        public override string EntityLogicalName => ConvertRule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ConvertRule.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ConvertRule.Schema.Attributes.name
                    , ConvertRule.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<ConvertRule>(components.Select(c => c.ObjectId));

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

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var convertRule = GetEntity<ConvertRule>(component.ObjectId.Value);

            if (convertRule != null)
            {
                string name = convertRule.Name;

                return string.Format("ConvertRule {0}    IsManaged {1}    SolutionName {2}", name
                    , convertRule.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(convertRule, "solution.uniquename")
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