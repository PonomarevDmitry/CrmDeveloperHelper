using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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

        //protected override ColumnSet GetColumnSet()
        //{
        //    return new ColumnSet
        //        (
        //            DuplicateRule.Schema.Attributes.name
        //            , DuplicateRule.Schema.Attributes.ismanaged
        //            , DuplicateRule.Schema.Attributes.ismanaged
        //            , DuplicateRule.Schema.Attributes.ismanaged
        //            , DuplicateRule.Schema.Attributes.ismanaged
        //            , DuplicateRule.Schema.Attributes.ismanaged
        //            , DuplicateRule.Schema.Attributes.ismanaged
        //            , DuplicateRule.Schema.Attributes.ismanaged
        //        );
        //}

        //public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        //{
        //    var list = GetEntities<DuplicateRule>(components.Select(c => c.ObjectId));

        //    {
        //        var hash = new HashSet<Guid>(list.Select(en => en.Id));
        //        var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
        //        if (notFinded.Any())
        //        {
        //            builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
        //            notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
        //        }
        //    }

        //    FormatTextTableHandler handler = new FormatTextTableHandler();
        //    handler.SetHeader("DuplicateRuleType", "Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Url");

        //    foreach (var DuplicateRule in list)
        //    {
        //        string webTypeName = string.Format("'{0}'", DuplicateRule.FormattedValues[DuplicateRule.Schema.Attributes.DuplicateRuletype]);

        //        string name = DuplicateRule.Name;

        //        handler.AddLine(webTypeName
        //            , name
        //            , DuplicateRule.IsManaged.ToString()
        //            , EntityDescriptionHandler.GetAttributeString(DuplicateRule, "solution.uniquename")
        //            , EntityDescriptionHandler.GetAttributeString(DuplicateRule, "solution.ismanaged")
        //            , EntityDescriptionHandler.GetAttributeString(DuplicateRule, "suppsolution.uniquename")
        //            , EntityDescriptionHandler.GetAttributeString(DuplicateRule, "suppsolution.ismanaged")
        //            , withUrls ? _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.DuplicateRule, DuplicateRule.Id, null, null) : string.Empty
        //            );
        //    }

        //    List<string> lines = handler.GetFormatedLines(true);

        //    lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        //}

        //public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        //{
        //    var duplicateRule = GetEntity<DuplicateRule>(component.ObjectId.Value);

        //    if (duplicateRule != null)
        //    {
        //        string webTypeName = string.Format("'{0}'", duplicateRule.FormattedValues[DuplicateRule.Schema.Attributes.DuplicateRuletype]);

        //        return string.Format("DuplicateRule     '{0}'    DuplicateRuleType '{1}'    IsManaged {2}    SolutionName {3}{4}"
        //            , duplicateRule.Name
        //            , webTypeName
        //            , duplicateRule.IsManaged.ToString()
        //            , EntityDescriptionHandler.GetAttributeString(duplicateRule, "solution.uniquename")
        //            , withUrls ? string.Format("    Url {0}", _service.ConnectionData.GetSolutionComponentUrl(ComponentType.DuplicateRule, duplicateRule.Id, null, null)) : string.Empty
        //            );
        //    }

        //    return component.ToString();
        //}

        //public TupleList<string, string> GetComponentColumns()
        //{

        //}
    }
}