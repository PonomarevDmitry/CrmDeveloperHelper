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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("TemplateTypeCode", "Title", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<Template>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.TemplateTypeCode
                , entity.Title
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<Template>(solutionComponent.ObjectId.Value);

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
                    { Template.Schema.Attributes.templatetypecode, "TemplateTypeCode" }
                    , { Template.Schema.Attributes.title, "Title" }
                    , { Template.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { Template.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}