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
    public class ConnectionRoleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ConnectionRoleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ConnectionRole)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ConnectionRole;

        public override int ComponentTypeValue => (int)ComponentType.ConnectionRole;

        public override string EntityLogicalName => ConnectionRole.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ConnectionRole.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ConnectionRole.Schema.Attributes.name
                    , ConnectionRole.Schema.Attributes.ismanaged
                    , ConnectionRole.Schema.Attributes.iscustomizable
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<ConnectionRole>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("Name", "IsManaged", "IsCustomizable", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string name = entity.Name;

                table.AddLine(name
                    , entity.IsManaged.ToString()
                    , entity.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var connectionRole = GetEntity<ConnectionRole>(component.ObjectId.Value);

            if (connectionRole != null)
            {
                string name = connectionRole.Name;

                return string.Format("ConnectionRole {0}    IsManaged {1}    SolutionName {2}"
                    , name
                    , connectionRole.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(connectionRole, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    {  ConnectionRole.Schema.Attributes.name, "Name" }
                    , { ConnectionRole.Schema.Attributes.category, "Category" }
                    , { ConnectionRole.Schema.Attributes.statuscode, "StatusCode" }
                    , { ConnectionRole.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { ConnectionRole.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}