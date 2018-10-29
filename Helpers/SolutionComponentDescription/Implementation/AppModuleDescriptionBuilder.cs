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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withUrls, bool withManaged, bool withSolutionInfo, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "UniqueName", "URL", "AppModuleVersion", "Id");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, bool withUrls, bool withManaged, bool withSolutionInfo, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<AppModule>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.UniqueName
                , entity.URL
                , entity.AppModuleVersion
                , entity.Id.ToString()
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<AppModule>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.UniqueName ?? entity.Name ?? entity.Id.ToString();
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { AppModule.Schema.Attributes.name, "Name" }
                    , { AppModule.Schema.Attributes.uniquename, "UniqueName" }
                    , { AppModule.Schema.Attributes.url, "URL" }
                    , { AppModule.Schema.Attributes.appmoduleversion, "AppModuleVersion" }
                    , { AppModule.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}