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
    public class ConvertRuleItemDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ConvertRuleItemDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ConvertRuleItem)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ConvertRuleItem;

        public override int ComponentTypeValue => (int)ComponentType.ConvertRuleItem;

        public override string EntityLogicalName => ConvertRuleItem.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ConvertRuleItem.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ConvertRuleItem.Schema.Attributes.convertruleid
                    , ConvertRuleItem.Schema.Attributes.name
                    , ConvertRuleItem.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<ConvertRuleItem>(components.Select(c => c.ObjectId));

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

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var convertRuleItem = GetEntity<ConvertRuleItem>(component.ObjectId.Value);

            if (convertRuleItem != null)
            {
                string convertRuleName = convertRuleItem.ConvertRuleId.Name;
                string name = convertRuleItem.Name;

                return string.Format("ConvertRuleName {0}    Name {1}    IsManaged {2}    SolutionName {3}"
                    , convertRuleName
                    , name
                    , convertRuleItem.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(convertRuleItem, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { ConvertRuleItem.Schema.Attributes.convertruleid, "ConvertRuleName" }
                    , { ConvertRuleItem.Schema.Attributes.name, "Name" }
                    , { ConvertRuleItem.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}