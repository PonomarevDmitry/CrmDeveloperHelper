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
    public class DuplicateRuleConditionDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public DuplicateRuleConditionDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.DuplicateRuleCondition)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.DuplicateRuleCondition;

        public override int ComponentTypeValue => (int)ComponentType.DuplicateRuleCondition;

        public override string EntityLogicalName => DuplicateRuleCondition.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => DuplicateRuleCondition.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    DuplicateRuleCondition.Schema.Attributes.regardingobjectid
                    , DuplicateRuleCondition.Schema.Attributes.baseattributename
                    , DuplicateRuleCondition.Schema.Attributes.matchingattributename
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<DuplicateRuleCondition>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("DuplicateRule", "BaseAttributeName", "MatchingAttributeName", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Url");

            foreach (var duplicateRuleCondition in list)
            {
                handler.AddLine(
                    duplicateRuleCondition.RegardingObjectId?.Name
                    , duplicateRuleCondition.BaseAttributeName
                    , duplicateRuleCondition.MatchingAttributeName
                    , EntityDescriptionHandler.GetAttributeString(duplicateRuleCondition, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(duplicateRuleCondition, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(duplicateRuleCondition, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(duplicateRuleCondition, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var duplicateRuleCondition = GetEntity<DuplicateRuleCondition>(component.ObjectId.Value);

            if (duplicateRuleCondition != null)
            {
                return string.Format("DuplicateRule     {0}    BaseAttributeName {1}    MatchingAttributeName {2}    SolutionName {3}"
                    , duplicateRuleCondition.RegardingObjectId?.Name
                    , duplicateRuleCondition.BaseAttributeName
                    , duplicateRuleCondition.MatchingAttributeName
                    , EntityDescriptionHandler.GetAttributeString(duplicateRuleCondition, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { DuplicateRuleCondition.Schema.Attributes.regardingobjectid, "DuplicateRule" }
                    , { DuplicateRuleCondition.Schema.Attributes.baseattributename, "BaseAttributeName" }
                    , { DuplicateRuleCondition.Schema.Attributes.matchingattributename, "MatchingAttributeName" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}