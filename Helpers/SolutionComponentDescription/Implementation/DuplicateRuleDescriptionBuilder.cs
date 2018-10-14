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
    public class DuplicateRuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public DuplicateRuleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.DuplicateRule)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.DuplicateRule;

        public override int ComponentTypeValue => (int)ComponentType.DuplicateRule;

        public override string EntityLogicalName => DuplicateRule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => DuplicateRule.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    DuplicateRule.Schema.Attributes.name
                    , DuplicateRule.Schema.Attributes.baseentitytypecode
                    , DuplicateRule.Schema.Attributes.matchingentityname
                    , DuplicateRule.Schema.Attributes.statuscode
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<DuplicateRule>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("DuplicateRuleType", "BaseEntityTypeCode", "MatchingEntityName", "StatusCode", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Url");

            foreach (var duplicateRule in list)
            {
                handler.AddLine(
                    duplicateRule.Name
                    , duplicateRule.BaseEntityName
                    , duplicateRule.MatchingEntityName
                    , duplicateRule.FormattedValues[DuplicateRule.Schema.Attributes.statuscode]
                    , EntityDescriptionHandler.GetAttributeString(duplicateRule, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(duplicateRule, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(duplicateRule, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(duplicateRule, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var duplicateRule = GetEntity<DuplicateRule>(component.ObjectId.Value);

            if (duplicateRule != null)
            {
                return string.Format("DuplicateRule     {0}    BaseEntityName {1}    MatchingEntityName {2}    StatusCode {3}    SolutionName {4}"
                    , duplicateRule.Name
                    , duplicateRule.BaseEntityName
                    , duplicateRule.MatchingEntityName
                    , duplicateRule.FormattedValues[DuplicateRule.Schema.Attributes.statuscode]
                    , EntityDescriptionHandler.GetAttributeString(duplicateRule, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { DuplicateRule.Schema.Attributes.name, "Name" }
                    , { DuplicateRule.Schema.Attributes.baseentityname, "BaseEntityName" }
                    , { DuplicateRule.Schema.Attributes.matchingentityname, "MatchingEntityName" }
                    , { DuplicateRule.Schema.Attributes.statuscode, "StatusCode" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}