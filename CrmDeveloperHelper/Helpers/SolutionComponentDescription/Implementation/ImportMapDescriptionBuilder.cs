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
    public class ImportMapDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ImportMapDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ImportMap)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ImportMap;

        public override int ComponentTypeValue => (int)ComponentType.ImportMap;

        public override string EntityLogicalName => ImportMap.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ImportMap.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ImportMap.Schema.Attributes.name
                    , ImportMap.Schema.Attributes.description
                    , ImportMap.Schema.Attributes.importmaptype
                    , ImportMap.Schema.Attributes.source
                    , ImportMap.Schema.Attributes.sourcetype
                    , ImportMap.Schema.Attributes.entitiesperfile
                    , ImportMap.Schema.Attributes.isvalidforimport
                    , ImportMap.Schema.Attributes.iswizardcreated
                    , ImportMap.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader(
                ImportMap.Schema.Headers.name
                , ImportMap.Schema.Headers.description
                , ImportMap.Schema.Headers.importmaptype
                , ImportMap.Schema.Headers.source
                , ImportMap.Schema.Headers.sourcetype
                , ImportMap.Schema.Headers.entitiesperfile
                , ImportMap.Schema.Headers.isvalidforimport
                , ImportMap.Schema.Headers.iswizardcreated
                , "Behavior"
            );

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ImportMap>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.Description
                , entity.FormattedValues[ImportMap.Schema.Attributes.importmaptype]
                , entity.Source
                , entity.FormattedValues[ImportMap.Schema.Attributes.sourcetype]
                , entity.FormattedValues[ImportMap.Schema.Attributes.entitiesperfile]
                , entity.IsValidForImport.ToString()
                , entity.IsWizardCreated.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetDisplayName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<ImportMap>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return entity.Description;
            }

            return base.GetDisplayName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
            {
                { ImportMap.Schema.Headers.name, ImportMap.Schema.Headers.name }
                , { ImportMap.Schema.Headers.description, ImportMap.Schema.Headers.description }
                , { ImportMap.Schema.Headers.importmaptype, ImportMap.Schema.Headers.importmaptype }
                , { ImportMap.Schema.Headers.source, ImportMap.Schema.Headers.source }
                , { ImportMap.Schema.Headers.sourcetype, ImportMap.Schema.Headers.sourcetype }
                , { ImportMap.Schema.Headers.entitiesperfile, ImportMap.Schema.Headers.entitiesperfile }
                , { ImportMap.Schema.Headers.isvalidforimport, ImportMap.Schema.Headers.isvalidforimport }
                , { ImportMap.Schema.Headers.iswizardcreated, ImportMap.Schema.Headers.iswizardcreated }
                , { ImportMap.Schema.Headers.ismanaged, ImportMap.Schema.Headers.ismanaged }
                , { "solution.uniquename", "SolutionName" }
                , { "solution.ismanaged", "SolutionIsManaged" }
                , { "suppsolution.uniquename", "SupportingName" }
                , { "suppsolution.ismanaged", "SupportingIsManaged" }
            };
        }
    }
}
