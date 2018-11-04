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
    public class SavedQueryVisualizationDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SavedQueryVisualizationDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SavedQueryVisualization)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SavedQueryVisualization;

        public override int ComponentTypeValue => (int)ComponentType.SavedQueryVisualization;

        public override string EntityLogicalName => SavedQueryVisualization.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SavedQueryVisualization.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SavedQueryVisualization.Schema.Attributes.primaryentitytypecode
                    , SavedQueryVisualization.Schema.Attributes.name
                    , SavedQueryVisualization.Schema.Attributes.ismanaged
                    , SavedQueryVisualization.Schema.Attributes.iscustomizable
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("PrimaryEntityTypeCode", "Name", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SavedQueryVisualization>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.PrimaryEntityTypeCode
                , entity.Name
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<SavedQueryVisualization>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.PrimaryEntityTypeCode, entity.Name);
            }

            return base.GetName(component);
        }

        public override string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<SavedQueryVisualization>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return entity.PrimaryEntityTypeCode;
            }

            return base.GetLinkedEntityName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    {  SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, "PrimaryEntityTypeCode" }
                    , { SavedQueryVisualization.Schema.Attributes.name, "Name" }
                    , { SavedQueryVisualization.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { SavedQueryVisualization.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}