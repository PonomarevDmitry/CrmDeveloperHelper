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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Entity", "Name", "MatchingEntityName", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SimilarityRule>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.BaseEntityName
                , entity.Name
                , entity.MatchingEntityName
                , entity.Id.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { SimilarityRule.Schema.Attributes.baseentityname, "Entity" }
                    , { SimilarityRule.Schema.Attributes.name, "Name" }
                    , { SimilarityRule.Schema.Attributes.matchingentityname, "MatchingEntityName" }
                    , { SimilarityRule.Schema.EntityPrimaryIdAttribute, "Id" }
                    , { SimilarityRule.Schema.Attributes.statuscode, "StatusCode" }
                    , { SimilarityRule.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}