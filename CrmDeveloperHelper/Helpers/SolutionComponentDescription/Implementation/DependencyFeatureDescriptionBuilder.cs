using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class DependencyFeatureDescriptionBuilder : ISolutionComponentDescriptionBuilder
    {
        private const string formatSpacer = DefaultSolutionComponentDescriptionBuilder.formatSpacer;

        public DependencyFeatureDescriptionBuilder(IOrganizationServiceExtented service)
        {

        }

        public ComponentType? ComponentTypeEnum => ComponentType.DependencyFeature;

        public int ComponentTypeValue => (int)ComponentType.DependencyFeature;

        public string EntityLogicalName => DependencyFeature.Schema.EntityLogicalName;

        public string EntityPrimaryIdAttribute => DependencyFeature.Schema.EntityPrimaryIdAttribute;

        public List<T> GetEntities<T>(IEnumerable<Guid> components) where T : Entity
        {
            return new List<T>();
        }

        public List<T> GetEntities<T>(IEnumerable<Guid?> components) where T : Entity
        {
            return new List<T>();
        }

        public T GetEntity<T>(Guid idEntity) where T : Entity
        {
            return null;
        }

        public void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            result.Add(new SolutionImageComponent()
            {
                ObjectId = solutionComponent.ObjectId,
                ComponentType = (solutionComponent.ComponentType?.Value).GetValueOrDefault(),
                RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
            });
        }

        public virtual void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null || !solutionImageComponent.ObjectId.HasValue)
            {
                return;
            }

            FillSolutionComponentInternal(result, solutionImageComponent.ObjectId.Value, solutionImageComponent.RootComponentBehavior);
        }

        public void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {
            if (elementRootComponent == null)
            {
                return;
            }

            var id = DefaultSolutionComponentDescriptionBuilder.GetIdFromXml(elementRootComponent);

            if (id.HasValue)
            {
                int? behavior = DefaultSolutionComponentDescriptionBuilder.GetBehaviorFromXml(elementRootComponent);

                FillSolutionComponentInternal(result, id.Value, behavior);
            }
        }

        private void FillSolutionComponentInternal(ICollection<SolutionComponent> result, Guid objectId, int? behavior)
        {
            var component = new SolutionComponent()
            {
                ComponentType = new OptionSetValue(this.ComponentTypeValue),
                ObjectId = objectId,
                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
            };

            if (behavior.HasValue)
            {
                component.RootComponentBehavior = new OptionSetValue(behavior.Value);
            }

            result.Add(component);
        }

        public void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withManaged, bool withSolutionInfo, bool withUrls)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("DependencyFeatureId", "Behavior");

            foreach (var comp in components)
            {
                string behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior?.Value);

                handler.AddLine(comp.ObjectId.ToString(), string.Empty, behavior);
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public string GenerateDescriptionSingle(SolutionComponent component, bool withManaged, bool withSolutionInfo, bool withUrls)
        {
            return GenerateDescriptionSingleInternal(component.ObjectId.Value, withUrls, withManaged, withSolutionInfo);
        }

        private string GenerateDescriptionSingleInternal(Guid dependencyId, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            return string.Format("DependencyFeatureId {0}", dependencyId.ToString());
        }

        public string GetName(SolutionComponent component)
        {
            return component.ObjectId.Value.ToString();
        }

        public string GetDisplayName(SolutionComponent solutionComponent)
        {
            return null;
        }

        public string GetManagedName(SolutionComponent solutionComponent)
        {
            return null;
        }

        public string GetCustomizableName(SolutionComponent component)
        {
            return null;
        }

        public string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            return null;
        }

        public string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            return string.Format("{0}.DependencyFeature {1} - {2}.{3}", connectionName, objectId, fieldTitle, extension);
        }

        public TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>();
        }
    }
}