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

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<SystemForm>(components.Select(c => c.ObjectId));

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

            handler.SetHeader("EntityName", "FormType", "FormName", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list
                .OrderBy(ent => ent.ObjectTypeCode)
                .ThenBy(ent => ent.Type.Value)
                .ThenBy(ent => ent.Name)
                )
            {
                string formTypeName = entity.FormattedValues[SystemForm.Schema.Attributes.type];

                handler.AddLine(entity.ObjectTypeCode
                    , string.Format("'{0}'", formTypeName)
                    , string.Format("'{0}'", entity.Name)
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
            var systemForm = GetEntity<SystemForm>(component.ObjectId.Value);

            if (systemForm != null)
            {
                string formName = systemForm.Name;
                string entityName = systemForm.ObjectTypeCode;

                string formTypeName = systemForm.FormattedValues[SystemForm.Schema.Attributes.type];

                return string.Format("SystemForm     {0}    '{1}'    '{2}'    IsManged {3}    SolutionName {4}"
                    , entityName
                    , formTypeName
                    , formName
                    , systemForm.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(systemForm, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
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