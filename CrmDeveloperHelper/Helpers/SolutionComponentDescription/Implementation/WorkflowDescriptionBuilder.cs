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

        public override string EntityPrimaryIdAttribute => Workflow.Schema.EntityPrimaryIdAttribute;

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
            handler.SetHeader("Entity", "Category", "Name", "Type", "Scope", "Mode", "StatusCode", "IsCustomizable", "Behavior");

            AppendIntoTableHeader(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<Workflow>();

            List<string> values = new List<string>();

            string name = entity.Name;

            if (!string.IsNullOrEmpty(entity.UniqueName))
            {
                name += string.Format("    (UniqueName \"{0}\")", entity.UniqueName);
            }

            entity.FormattedValues.TryGetValue(Workflow.Schema.Attributes.category, out string category);
            entity.FormattedValues.TryGetValue(Workflow.Schema.Attributes.businessprocesstype, out string businessprocesstype);
            entity.FormattedValues.TryGetValue(Workflow.Schema.Attributes.scope, out string scope);
            entity.FormattedValues.TryGetValue(Workflow.Schema.Attributes.mode, out string mode);
            entity.FormattedValues.TryGetValue(Workflow.Schema.Attributes.statuscode, out string statuscode);

            values.AddRange(new[]
            {
                entity.PrimaryEntity
                , category
                , name
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

        public override string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<Workflow>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return entity.PrimaryEntity;
            }

            return base.GetLinkedEntityName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { Workflow.Schema.Attributes.primaryentity, "EntityName" }
                    , { Workflow.Schema.Attributes.category, "Category" }
                    , { Workflow.Schema.Attributes.name, "Name" }
                    , { Workflow.Schema.Attributes.uniquename, "UniqueName" }
                    , { Workflow.Schema.Attributes.businessprocesstype, "Type" }
                    , { Workflow.Schema.Attributes.scope, "Scope" }
                    , { Workflow.Schema.Attributes.mode, "Mode" }
                    , { Workflow.Schema.Attributes.statuscode, "StatusCode" }
                    , { Workflow.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { Workflow.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}