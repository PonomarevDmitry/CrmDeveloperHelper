using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class ServiceEndpointDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ServiceEndpointDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ServiceEndpoint)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ServiceEndpoint;

        public override int ComponentTypeValue => (int)ComponentType.ServiceEndpoint;

        public override string EntityLogicalName => ServiceEndpoint.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ServiceEndpoint.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ServiceEndpoint.Schema.Attributes.name
                    , ServiceEndpoint.Schema.Attributes.connectionmode
                    , ServiceEndpoint.Schema.Attributes.contract
                    , ServiceEndpoint.Schema.Attributes.messageformat
                    , ServiceEndpoint.Schema.Attributes.ismanaged
                    , ServiceEndpoint.Schema.Attributes.iscustomizable
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<ServiceEndpoint>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("Name", "ConnectionMode", "Contract", "MessageFormat", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.connectionmode]
                    , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.contract]
                    , entity.FormattedValues[ServiceEndpoint.Schema.Attributes.messageformat]
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
            var serviceEndpoint = GetEntity<ServiceEndpoint>(component.ObjectId.Value);

            if (serviceEndpoint != null)
            {
                return string.Format("Name {0}    ConnectionMode {1}    Contract {2}    MessageFormat {3}    IsManaged {4}    SolutionName {5}"
                    , serviceEndpoint.Name
                    , serviceEndpoint.FormattedValues[ServiceEndpoint.Schema.Attributes.connectionmode]
                    , serviceEndpoint.FormattedValues[ServiceEndpoint.Schema.Attributes.contract]
                    , serviceEndpoint.FormattedValues[ServiceEndpoint.Schema.Attributes.messageformat]
                    , serviceEndpoint.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(serviceEndpoint, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var fieldPermission = GetEntity<FieldPermission>(component.ObjectId.Value);

            if (fieldPermission != null)
            {

            }

            return component.ObjectId.ToString();
        }
    }
}