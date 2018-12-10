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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Title", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<KbArticleTemplate>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Title
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
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