using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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
                string formName = entity.Name;
                string entityName = entity.ObjectTypeCode;

                string formTypeName = entity.FormattedValues[SystemForm.Schema.Attributes.type];

                handler.AddLine(entityName
                    , string.Format("'{0}'", formTypeName)
                    , string.Format("'{0}'", formName)
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

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<SystemForm>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.ObjectTypeCode, entity.Name);
            }

            return component.ObjectId.ToString();
        }

        public override string GetDisplayName(SolutionComponent component)
        {
            var entity = GetEntity<SystemForm>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.FormattedValues[SystemForm.Schema.Attributes.type];
            }

            return component.ObjectId.ToString();
        }
    }
}