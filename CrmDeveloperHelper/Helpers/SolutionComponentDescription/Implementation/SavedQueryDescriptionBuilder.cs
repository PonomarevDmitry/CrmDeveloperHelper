using Microsoft.Xrm.Sdk;
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

        public override string EntityPrimaryIdAttribute => SavedQuery.EntityPrimaryIdAttribute;

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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("EntityName", "Name", "QueryType", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<SavedQuery>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.ReturnedTypeCode
                , entity.Name
                , Repository.SavedQueryRepository.GetQueryTypeName(entity.QueryType.GetValueOrDefault())
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
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
                    {  SavedQuery.Schema.Attributes.returnedtypecode, SavedQuery.Schema.Headers.returnedtypecode }
                    , { SavedQuery.Schema.Attributes.name, SavedQuery.Schema.Headers.name }
                    , { SavedQuery.Schema.Attributes.iscustomizable, SavedQuery.Schema.Headers.iscustomizable }
                    , { SavedQuery.Schema.Attributes.ismanaged, SavedQuery.Schema.Headers.ismanaged }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }

        public override IEnumerable<SolutionComponent> GetLinkedComponents(SolutionComponent solutionComponent)
        {
            var result = new List<SolutionComponent>();

            var entity = GetEntity<SavedQuery>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.ReturnedTypeCode)
                    && _service.ConnectionData.IntellisenseData != null
                    && _service.ConnectionData.IntellisenseData.Entities != null
                    && _service.ConnectionData.IntellisenseData.Entities.ContainsKey(entity.ReturnedTypeCode)
                )
                {
                    result.Add(new SolutionComponent()
                    {
                        ObjectId = _service.ConnectionData.IntellisenseData.Entities[entity.ReturnedTypeCode].MetadataId,
                        ComponentType = new OptionSetValue((int)ComponentType.Entity),
                    });
                }
            }

            return result;
        }
    }
}