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
    public class ComplexControlDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ComplexControlDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ComplexControl)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ComplexControl;

        public override int ComponentTypeValue => (int)ComponentType.ComplexControl;

        public override string EntityLogicalName => ComplexControl.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ComplexControl.EntityPrimaryIdAttribute;

        public override List<T> GetEntities<T>(IEnumerable<Guid?> components)
        {
            List<T> result = new List<T>();

            foreach (var id in components.Where(c => c.HasValue).Select(c => c.Value))
            {
                var entity = GetEntity<T>(id);

                if (entity != null)
                {
                    result.Add(entity);
                }
            }

            return result;
        }

        public override T GetEntity<T>(Guid idEntity)
        {
            var columnSet = new ColumnSet
            (
                ComplexControl.Schema.Attributes.name
                , ComplexControl.Schema.Attributes.description
                , ComplexControl.Schema.Attributes.type
                , ComplexControl.Schema.Attributes.ismanaged
            );

            try
            {
                var result = _service.Retrieve(ComplexControl.EntityLogicalName, idEntity, columnSet);

                return result.ToEntity<T>();
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }

            return null;
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader(
                ComplexControl.Schema.Headers.name
                , ComplexControl.Schema.Headers.description
                , ComplexControl.Schema.Headers.type
                , "Behavior"
            );

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ComplexControl>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.Description
                , entity.FormattedValues[ComplexControl.Schema.Attributes.type]
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }
    }
}
