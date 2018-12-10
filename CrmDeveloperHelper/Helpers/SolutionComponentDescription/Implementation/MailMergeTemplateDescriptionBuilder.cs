using Microsoft.Xrm.Sdk;
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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("TemplateTypeCode", "Name", "MailMergeType", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<MailMergeTemplate>();

            List<string> values = new List<string>();

            string typeName = entity.FormattedValues.ContainsKey(MailMergeTemplate.Schema.Attributes.mailmergetype) ? entity.FormattedValues[MailMergeTemplate.Schema.Attributes.mailmergetype] : string.Empty;

            values.AddRange(new[]
            {
                entity.TemplateTypeCode
                , entity.Name
                , typeName
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { MailMergeTemplate.Schema.Attributes.templatetypecode, "TemplateTypeCode" }
                    , { MailMergeTemplate.Schema.Attributes.name, "Name" }
                    , { MailMergeTemplate.Schema.Attributes.mailmergetype, "MailMergeType" }
                    , { MailMergeTemplate.Schema.Attributes.statuscode, "StatusCode" }
                    , { MailMergeTemplate.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { MailMergeTemplate.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}