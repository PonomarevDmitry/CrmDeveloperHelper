using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class SystemFormDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SystemFormDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SystemForm)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SystemForm;

        public override int ComponentTypeValue => (int)ComponentType.SystemForm;

        public override string EntityLogicalName => SystemForm.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SystemForm.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SystemForm.Schema.Attributes.objecttypecode
                    , SystemForm.Schema.Attributes.name
                    , SystemForm.Schema.Attributes.type
                    , SystemForm.Schema.Attributes.ismanaged
                    , SystemForm.Schema.Attributes.iscustomizable
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("EntityName", "FormType", "FormName", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SystemForm>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.ObjectTypeCode
                , entity.FormattedValues[SystemForm.Schema.Attributes.type]
                , entity.Name
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<SystemForm>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.ObjectTypeCode, entity.Name);
            }

            return base.GetName(component);
        }

        public override string GetDisplayName(SolutionComponent component)
        {
            var entity = GetEntity<SystemForm>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.FormattedValues[SystemForm.Schema.Attributes.type];
            }

            return base.GetDisplayName(component);
        }

        public override IEnumerable<SolutionComponent> GetLinkedComponents(SolutionComponent solutionComponent)
        {
            var result = new List<SolutionComponent>();

            var entity = GetEntity<SystemForm>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                var idEntityMetadata = _service.ConnectionData.GetEntityMetadataId(entity.ObjectTypeCode);

                if (idEntityMetadata.HasValue)
                {
                    result.Add(new SolutionComponent()
                    {
                        ObjectId = idEntityMetadata.Value,
                        ComponentType = new OptionSetValue((int)ComponentType.Entity),
                    });
                }
            }

            return result;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    {  SystemForm.Schema.Attributes.objecttypecode, SystemForm.Schema.Headers.objecttypecode }
                    , { SystemForm.Schema.Attributes.type, SystemForm.Schema.Headers.type }
                    , { SystemForm.Schema.Attributes.name, SystemForm.Schema.Headers.name }
                    , { SystemForm.Schema.Attributes.uniquename, SystemForm.Schema.Headers.uniquename }
                    , { SystemForm.Schema.Attributes.formactivationstate, SystemForm.Schema.Headers.formactivationstate }
                    , { SystemForm.Schema.Attributes.iscustomizable, SystemForm.Schema.Headers.iscustomizable }
                    , { SystemForm.Schema.Attributes.ismanaged, SystemForm.Schema.Headers.ismanaged }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}