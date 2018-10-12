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
    public class CustomControlDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public CustomControlDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.CustomControl)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.CustomControl;

        public override int ComponentTypeValue => (int)ComponentType.CustomControl;

        public override string EntityLogicalName => CustomControl.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => CustomControl.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    CustomControl.Schema.Attributes.name
                    , CustomControl.Schema.Attributes.compatibledatatypes
                    , CustomControl.Schema.Attributes.manifest
                    , CustomControl.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<CustomControl>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("Name", "CompatibleDataTypes", "Manifest", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.CompatibleDataTypes
                    , entity.Manifest
                    , entity.IsManaged.ToString()
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
            var customControl = GetEntity<CustomControl>(component.ObjectId.Value);

            if (customControl != null)
            {
                return string.Format("Name {0}    CompatibleDataTypes {1}    Manifest {2}    IsManaged {3}    SolutionName {4}"
                    , customControl.Name
                    , customControl.CompatibleDataTypes
                    , customControl.Manifest
                    , customControl.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(customControl, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { CustomControl.Schema.Attributes.name, "Name" }
                    , { CustomControl.Schema.Attributes.compatibledatatypes, "CompatibleDataTypes" }
                    , { CustomControl.Schema.Attributes.manifest, "Manifest" }
                    , { CustomControl.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}