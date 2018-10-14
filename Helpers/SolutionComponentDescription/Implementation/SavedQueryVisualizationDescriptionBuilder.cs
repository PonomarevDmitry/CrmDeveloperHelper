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

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<SavedQueryVisualization>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("PrimaryEntityTypeCode", "Name", "IsManaged", "IsCustomizable", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string chartName = entity.Name;
                string entityName = entity.PrimaryEntityTypeCode;

                handler.AddLine(entityName
                    , chartName
                    , entity.IsManaged.ToString()
                    , entity.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var savedQueryVisualization = GetEntity<SavedQueryVisualization>(component.ObjectId.Value);

            if (savedQueryVisualization != null)
            {
                return string.Format("SavedQueryVisualization (Chart)     {0}    Name '{1}'    IsManged {2}    IsManged {3}    SolutionName {4}"
                    , savedQueryVisualization.PrimaryEntityTypeCode
                    , savedQueryVisualization.Name
                    , savedQueryVisualization.IsManaged.ToString()
                    , savedQueryVisualization.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(savedQueryVisualization, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
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