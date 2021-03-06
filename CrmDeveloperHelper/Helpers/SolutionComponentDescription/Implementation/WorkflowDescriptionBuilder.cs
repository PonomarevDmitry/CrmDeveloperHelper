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
    public class WorkflowDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public WorkflowDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.Workflow)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.Workflow;

        public override int ComponentTypeValue => (int)ComponentType.Workflow;

        public override string EntityLogicalName => Workflow.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => Workflow.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    Workflow.Schema.Attributes.primaryentity
                    , Workflow.Schema.Attributes.category
                    , Workflow.Schema.Attributes.name
                    , Workflow.Schema.Attributes.uniquename
                    , Workflow.Schema.Attributes.businessprocesstype
                    , Workflow.Schema.Attributes.scope
                    , Workflow.Schema.Attributes.mode
                    , Workflow.Schema.Attributes.statuscode
                    , Workflow.Schema.Attributes.ismanaged
                    , Workflow.Schema.Attributes.iscustomizable
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader(
                Workflow.Schema.Headers.primaryentity
                , Workflow.Schema.Headers.category
                , Workflow.Schema.Headers.name
                , Workflow.Schema.Headers.uniquename
                , Workflow.Schema.Headers.businessprocesstype
                , Workflow.Schema.Headers.scope
                , Workflow.Schema.Headers.mode
                , Workflow.Schema.Headers.statuscode
                , Workflow.Schema.Headers.iscustomizable
                , "Behavior"
            );

            AppendIntoTableHeader(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<Workflow>();

            List<string> values = new List<string>();

            entity.FormattedValues.TryGetValue(Workflow.Schema.Attributes.category, out string category);
            entity.FormattedValues.TryGetValue(Workflow.Schema.Attributes.businessprocesstype, out string businessprocesstype);
            entity.FormattedValues.TryGetValue(Workflow.Schema.Attributes.scope, out string scope);
            entity.FormattedValues.TryGetValue(Workflow.Schema.Attributes.mode, out string mode);
            entity.FormattedValues.TryGetValue(Workflow.Schema.Attributes.statuscode, out string statuscode);

            values.AddRange(new[]
            {
                entity.PrimaryEntity
                , category
                , entity.Name
                , entity.UniqueName
                , businessprocesstype
                , scope
                , mode
                , statuscode
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetDisplayName(SolutionComponent component)
        {
            var entity = GetEntity<Workflow>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.UniqueName;
            }

            return base.GetDisplayName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { Workflow.Schema.Attributes.primaryentity, Workflow.Schema.Headers.primaryentity }
                    , { Workflow.Schema.Attributes.category, Workflow.Schema.Headers.category }
                    , { Workflow.Schema.Attributes.name, Workflow.Schema.Headers.name }
                    , { Workflow.Schema.Attributes.uniquename, Workflow.Schema.Headers.uniquename }
                    , { Workflow.Schema.Attributes.businessprocesstype, Workflow.Schema.Headers.businessprocesstype }
                    , { Workflow.Schema.Attributes.scope, Workflow.Schema.Headers.scope }
                    , { Workflow.Schema.Attributes.mode, Workflow.Schema.Headers.mode }
                    , { Workflow.Schema.Attributes.statuscode, Workflow.Schema.Headers.statuscode }
                    , { Workflow.Schema.Attributes.iscustomizable, Workflow.Schema.Headers.iscustomizable }
                    , { Workflow.Schema.Attributes.ismanaged, Workflow.Schema.Headers.ismanaged }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }

        public override IEnumerable<SolutionComponent> GetLinkedComponents(SolutionComponent solutionComponent)
        {
            var result = new List<SolutionComponent>();

            var entity = GetEntity<Workflow>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                var idEntityMetadata = _service.ConnectionData.GetEntityMetadataId(entity.PrimaryEntity);

                if (idEntityMetadata.HasValue)
                {
                    result.Add(new SolutionComponent()
                    {
                        ObjectId = idEntityMetadata.Value,
                        ComponentType = new OptionSetValue((int)ComponentType.Entity),
                    });
                }
            }

            return result;
        }
    }
}