using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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

        //protected override ColumnSet GetColumnSet()
        //{
        //    return new ColumnSet
        //        (
        //            Workflow.Schema.Attributes.name
        //            , Workflow.Schema.Attributes.ismanaged
        //            , Workflow.Schema.Attributes.ismanaged
        //            , Workflow.Schema.Attributes.ismanaged
        //            , Workflow.Schema.Attributes.ismanaged
        //            , Workflow.Schema.Attributes.ismanaged
        //            , Workflow.Schema.Attributes.ismanaged
        //            , Workflow.Schema.Attributes.ismanaged
        //        );
        //}

        //public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        //{
        //    var list = GetEntities<DuplicateRuleCondition>(components.Select(c => c.ObjectId));

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
        //    handler.SetHeader("DuplicateRuleConditionType", "Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Url");

        //    foreach (var DuplicateRuleCondition in list)
        //    {
        //        string webTypeName = string.Format("'{0}'", DuplicateRuleCondition.FormattedValues[DuplicateRuleCondition.Schema.Attributes.DuplicateRuleConditiontype]);

        //        string name = DuplicateRuleCondition.Name;

        //        handler.AddLine(webTypeName
        //            , name
        //            , DuplicateRuleCondition.IsManaged.ToString()
        //            , EntityDescriptionHandler.GetAttributeString(DuplicateRuleCondition, "solution.uniquename")
        //            , EntityDescriptionHandler.GetAttributeString(DuplicateRuleCondition, "solution.ismanaged")
        //            , EntityDescriptionHandler.GetAttributeString(DuplicateRuleCondition, "suppsolution.uniquename")
        //            , EntityDescriptionHandler.GetAttributeString(DuplicateRuleCondition, "suppsolution.ismanaged")
        //            , withUrls ? _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.DuplicateRuleCondition, DuplicateRuleCondition.Id, null, null) : string.Empty
        //            );
        //    }

        //    List<string> lines = handler.GetFormatedLines(true);

        //    lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        //}

        //public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        //{
        //    var duplicateRuleCondition = GetEntity<DuplicateRuleCondition>(component.ObjectId.Value);

        //    if (duplicateRuleCondition != null)
        //    {
        //        string webTypeName = string.Format("'{0}'", duplicateRuleCondition.FormattedValues[DuplicateRuleCondition.Schema.Attributes.DuplicateRuleConditiontype]);

        //        return string.Format("DuplicateRuleCondition     '{0}'    DuplicateRuleConditionType '{1}'    IsManaged {2}    SolutionName {3}{4}"
        //            , duplicateRuleCondition.Name
        //            , webTypeName
        //            , duplicateRuleCondition.IsManaged.ToString()
        //            , EntityDescriptionHandler.GetAttributeString(duplicateRuleCondition, "solution.uniquename")
        //            , withUrls ? string.Format("    Url {0}", _service.ConnectionData.GetSolutionComponentUrl(ComponentType.DuplicateRuleCondition, duplicateRuleCondition.Id, null, null)) : string.Empty
        //            );
        //    }

        //    return component.ToString();
        //}

        //public TupleList<string, string> GetComponentColumns()
        //{

        //}
    }
}