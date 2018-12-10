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
    public class SystemFormDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SystemFormDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SystemForm)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SystemForm;

        public override int ComponentTypeValue => (int)ComponentType.SystemForm;

        public override string EntityLogicalName => SystemForm.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SystemForm.Schema.EntityPrimaryIdAttribute;

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

        public override string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<SystemForm>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return entity.ObjectTypeCode;
            }

            return base.GetLinkedEntityName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    {  SystemForm.Schema.Attributes.objecttypecode, "EntityName" }
                    , { SystemForm.Schema.Attributes.type, "FormType" }
                    , { SystemForm.Schema.Attributes.name, "Name" }
                    , { SystemForm.Schema.Attributes.uniquename, "UniqueName" }
                    , { SystemForm.Schema.Attributes.formactivationstate, "State" }
                    , { SystemForm.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { SystemForm.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}