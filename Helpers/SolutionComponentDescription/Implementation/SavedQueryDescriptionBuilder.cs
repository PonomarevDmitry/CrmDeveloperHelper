using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class SavedQueryDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SavedQueryDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SavedQuery)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SavedQuery;

        public override int ComponentTypeValue => (int)ComponentType.SavedQuery;

        public override string EntityLogicalName => SavedQuery.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SavedQuery.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SavedQuery.Schema.Attributes.returnedtypecode
                    , SavedQuery.Schema.Attributes.querytype
                    , SavedQuery.Schema.Attributes.name
                    , SavedQuery.Schema.Attributes.ismanaged
                    , SavedQuery.Schema.Attributes.iscustomizable
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<SavedQuery>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("EntityName", "Name", "QueryType", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string queryName = entity.Name;
                string entityName = entity.ReturnedTypeCode;

                handler.AddLine(entityName
                    , queryName
                    , Repository.SavedQueryRepository.GetQueryTypeName(entity.QueryType.GetValueOrDefault())
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
            var savedQuery = GetEntity<SavedQuery>(component.ObjectId.Value);

            if (savedQuery != null)
            {
                string queryName = savedQuery.Name;
                string entityName = savedQuery.ReturnedTypeCode;

                return string.Format("SavedQuery {0} - '{1}'    QueryType {2}    IsManaged {3}    SolutionName {4}"
                    , entityName
                    , queryName
                    , Repository.SavedQueryRepository.GetQueryTypeName(savedQuery.QueryType.GetValueOrDefault())
                    , savedQuery.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(savedQuery, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<SavedQuery>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.ReturnedTypeCode, entity.Name);
            }

            return base.GetName(component);
        }

        public override string GetDisplayName(SolutionComponent component)
        {
            var entity = GetEntity<SavedQuery>(component.ObjectId.Value);

            if (entity != null)
            {
                return SavedQueryRepository.GetQueryTypeName(entity.QueryType.Value);
            }

            return base.GetDisplayName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    {  SavedQuery.Schema.Attributes.returnedtypecode, "EntityName" }
                    , { SavedQuery.Schema.Attributes.name, "Name" }
                    , { SavedQuery.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { SavedQuery.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}