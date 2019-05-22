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
    public class DuplicateRuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        private readonly SolutionComponentMetadataSource _source;

        public DuplicateRuleDescriptionBuilder(IOrganizationServiceExtented service, SolutionComponentMetadataSource source)
            : base(service, (int)ComponentType.DuplicateRule)
        {
            this._source = source;
        }

        public override ComponentType? ComponentTypeEnum => ComponentType.DuplicateRule;

        public override int ComponentTypeValue => (int)ComponentType.DuplicateRule;

        public override string EntityLogicalName => DuplicateRule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => DuplicateRule.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    DuplicateRule.Schema.Attributes.name
                    , DuplicateRule.Schema.Attributes.baseentityname
                    , DuplicateRule.Schema.Attributes.matchingentityname
                    , DuplicateRule.Schema.Attributes.statuscode
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("DuplicateRuleType", "BaseEntityName", "MatchingEntityName", "StatusCode", "Behavior");

            action(handler, withUrls, false, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<DuplicateRule>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.BaseEntityName
                , entity.MatchingEntityName
                , entity.FormattedValues[DuplicateRule.Schema.Attributes.statuscode]
                , behavior
            });

            AppendIntoValues(values, entity, withUrls, false, withSolutionInfo);

            return values;
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

        public override IEnumerable<SolutionComponent> GetLinkedComponents(SolutionComponent solutionComponent)
        {
            var result = new List<SolutionComponent>();

            var entity = GetEntity<DuplicateRule>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.BaseEntityName))
                {
                    var entityMetadata = _source.GetEntityMetadata(entity.BaseEntityName);

                    if (entityMetadata != null)
                    {
                        result.Add(new SolutionComponent()
                        {
                            ObjectId = entityMetadata.MetadataId,
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                        });
                    }
                }

                if (!string.IsNullOrEmpty(entity.MatchingEntityName)
                    && !string.Equals(entity.BaseEntityName, entity.MatchingEntityName, StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    var entityMetadata = _source.GetEntityMetadata(entity.MatchingEntityName);

                    if (entityMetadata != null)
                    {
                        result.Add(new SolutionComponent()
                        {
                            ObjectId = entityMetadata.MetadataId,
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                        });
                    }
                }
            }

            return result;
        }
    }
}