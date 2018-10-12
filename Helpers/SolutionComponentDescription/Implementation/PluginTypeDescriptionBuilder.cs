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
    public class PluginTypeDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public PluginTypeDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.PluginType)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.PluginType;

        public override int ComponentTypeValue => (int)ComponentType.PluginType;

        public override string EntityLogicalName => PluginType.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => PluginType.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    PluginType.Schema.Attributes.assemblyname
                    , PluginType.Schema.Attributes.typename
                    , PluginType.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<PluginType>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            var table = new FormatTextTableHandler();
            table.SetHeader("PluginType", "PluginAssembly", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                table.AddLine(
                    entity.TypeName
                    , entity.AssemblyName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var pluginType = GetEntity<PluginType>(component.ObjectId.Value);

            if (pluginType != null)
            {
                return string.Format("PluginType \"{0}, {1}\"   IsManged {2}    SolutionName {3}"
                    , pluginType.AssemblyName
                    , pluginType.TypeName
                    , pluginType.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(pluginType, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<PluginType>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0}, {1}", entity.TypeName, entity.AssemblyName);
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { PluginType.Schema.Attributes.assemblyname, "AssemblyName" }
                    , { PluginType.Schema.Attributes.typename, "TypeName" }
                    , { PluginType.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}