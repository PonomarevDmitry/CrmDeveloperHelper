using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class DependencyFeatureDescriptionBuilder : ISolutionComponentDescriptionBuilder
    {
        private const string formatSpacer = DefaultSolutionComponentDescriptionBuilder.formatSpacer;

        private readonly IOrganizationServiceExtented _service;

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

                Description = GenerateDescriptionSingle(solutionComponent, false, true, true),
            });
        }

        public virtual void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null || !solutionImageComponent.ObjectId.HasValue)
            {
                return;
            }

            var component = new SolutionComponent()
            {
                ComponentType = new OptionSetValue(this.ComponentTypeValue),
                ObjectId = solutionImageComponent.ObjectId.Value,
                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
            };

            if (solutionImageComponent.RootComponentBehavior.HasValue)
            {
                component.RootComponentBehavior = new OptionSetValue(solutionImageComponent.RootComponentBehavior.Value);
            }

            result.Add(component);
        }

        public void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("DependencyFeatureId", "Behaviour");

            foreach (var comp in components)
            {
                string behaviorName = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behaviorName = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                handler.AddLine(comp.ObjectId.ToString(), string.Empty, behaviorName);
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public string GenerateDescriptionSingle(SolutionComponent component, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            return string.Format("DependencyFeatureId {0}", component.ObjectId.Value.ToString());
        }

        public string GetName(SolutionComponent component)
        {
            return string.Format("DependencyFeatureId {0}", component.ObjectId.Value.ToString());
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