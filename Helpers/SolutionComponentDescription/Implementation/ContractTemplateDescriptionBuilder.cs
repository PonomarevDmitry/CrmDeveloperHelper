using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class ContractTemplateDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ContractTemplateDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ContractTemplate)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ContractTemplate;

        public override int ComponentTypeValue => (int)ComponentType.ContractTemplate;

        public override string EntityLogicalName => ContractTemplate.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ContractTemplate.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ContractTemplate.Schema.Attributes.name
                    , ContractTemplate.Schema.Attributes.ismanaged
                    , ContractTemplate.Schema.Attributes.iscustomizable
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<ContractTemplate>(components.Select(c => c.ObjectId));

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
            table.SetHeader("Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string title = entity.Name;

                table.AddLine(title
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
            var contractTemplate = GetEntity<ContractTemplate>(component.ObjectId.Value);

            if (contractTemplate != null)
            {
                string title = contractTemplate.Name;

                return string.Format("ContractTemplate {0}    IsManaged {1}    SolutionName {2}"
                    , title
                    , contractTemplate.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(contractTemplate, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<FieldPermission>(component.ObjectId.Value);

            if (entity != null)
            {

            }

            return component.ObjectId.ToString();
        }
    }
}