using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class AppModuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public AppModuleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.AppModule)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.AppModule;

        public override int ComponentTypeValue => (int)ComponentType.AppModule;

        public override string EntityLogicalName => AppModule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => AppModule.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    AppModule.Schema.Attributes.name
                    , AppModule.Schema.Attributes.uniquename
                    , AppModule.Schema.Attributes.url
                    , AppModule.Schema.Attributes.appmoduleversion
                    , AppModule.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<AppModule>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("Name", "UniqueName", "URL", "AppModuleVersion", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.Name
                    , entity.UniqueName
                    , entity.URL
                    , entity.AppModuleVersion
                    , entity.Id.ToString()
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
            var appModule = GetEntity<AppModule>(component.ObjectId.Value);

            if (appModule != null)
            {
                return string.Format("Name {0}    UniqueName {1}    URL {2}    AppModuleVersion {3}    Id {4}    IsManaged {5}    SolutionName {6}"
                    , appModule.Name
                    , appModule.UniqueName
                    , appModule.URL
                    , appModule.AppModuleVersion
                    , appModule.Id.ToString()
                    , appModule.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(appModule, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<AppModule>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.UniqueName ?? entity.Name ?? entity.Id.ToString();
            }

            return component.ObjectId.ToString();
        }
    }
}