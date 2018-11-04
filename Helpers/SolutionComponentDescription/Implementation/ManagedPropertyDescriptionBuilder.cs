using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class ManagedPropertyDescriptionBuilder : ISolutionComponentDescriptionBuilder
    {
        private const string formatSpacer = DefaultSolutionComponentDescriptionBuilder.formatSpacer;
        private const string unknowedMessage = DefaultSolutionComponentDescriptionBuilder.unknowedMessage;

        private readonly IOrganizationServiceExtented _service;

        public ManagedPropertyDescriptionBuilder(IOrganizationServiceExtented service)
        {
            this._service = service;
        }

        public ComponentType? ComponentTypeEnum => ComponentType.ManagedProperty;

        public int ComponentTypeValue => (int)ComponentType.ManagedProperty;

        public string EntityLogicalName => string.Empty;

        public string EntityPrimaryIdAttribute => string.Empty;

        private readonly object _syncObjectManagedPropertyMetadata = new object();

        private ConcurrentDictionary<Guid, ManagedPropertyMetadata> _allManagedProperties;

        private ConcurrentDictionary<Guid, ManagedPropertyMetadata> AllManagedProperties
        {
            get
            {
                lock (_syncObjectManagedPropertyMetadata)
                {
                    if (_allManagedProperties == null)
                    {
                        var request = new RetrieveAllManagedPropertiesRequest();
                        var response = (RetrieveAllManagedPropertiesResponse)this._service.Execute(request);

                        _allManagedProperties = new ConcurrentDictionary<Guid, ManagedPropertyMetadata>();

                        foreach (var item in response.ManagedPropertyMetadata)
                        {
                            if (!_allManagedProperties.ContainsKey(item.MetadataId.Value))
                            {
                                _allManagedProperties.TryAdd(item.MetadataId.Value, item);
                            }
                        }
                    }
                }

                return _allManagedProperties;
            }
        }

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
            if (this.AllManagedProperties != null && this.AllManagedProperties.Any())
            {
                if (this.AllManagedProperties.ContainsKey(solutionComponent.ObjectId.Value))
                {
                    var managedProperty = this.AllManagedProperties[solutionComponent.ObjectId.Value];

                    result.Add(new SolutionImageComponent()
                    {
                        ComponentType = (int)ComponentType.ManagedProperty,
                        ObjectId = solutionComponent.ObjectId.Value,
                        RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                        Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
                    });
                }
            }
        }

        public void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null || !solutionImageComponent.ObjectId.HasValue)
            {
                return;
            }

            if (this.AllManagedProperties != null
                && this.AllManagedProperties.Any()
                && this.AllManagedProperties.ContainsKey(solutionImageComponent.ObjectId.Value)
                )
            {
                FillSolutionComponentInternal(result, solutionImageComponent.ObjectId.Value, solutionImageComponent.RootComponentBehavior);
            }
        }

        public void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {
            if (elementRootComponent == null)
            {
                return;
            }

            var id = DefaultSolutionComponentDescriptionBuilder.GetIdFromXml(elementRootComponent);

            if (id.HasValue
                && this.AllManagedProperties != null
                && this.AllManagedProperties.Any()
                && this.AllManagedProperties.ContainsKey(id.Value)
                )
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
            handler.SetHeader(
                "LogicalName"
                , "DisplayName"
                , "Description"
                , "EnablesEntityName"
                , "EnablesAttributeName"
                , "ErrorCode"
                , "EvaluationPriority"
                , "IsPrivate"
                , "IsGlobalForOperation"
                , "ManagedPropertyType"
                , "Operation"
                , "Behavior"
                );

            //public Label Description { get; }
            //public Label DisplayName { get; }
            //public string EnablesAttributeName { get; }
            //public string EnablesEntityName { get; }
            //public int? ErrorCode { get; }
            //public ManagedPropertyEvaluationPriority? EvaluationPriority { get; }
            //public string IntroducedVersion { get; }
            //public bool? IsGlobalForOperation { get; }
            //public bool? IsPrivate { get; }
            //public string LogicalName { get; }
            //public ManagedPropertyType? ManagedPropertyType { get; }
            //public ManagedPropertyOperation? Operation { get; }

            foreach (var comp in components)
            {
                if (this.AllManagedProperties.ContainsKey(comp.ObjectId.Value))
                {
                    var behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior?.Value);

                    var managedProperty = this.AllManagedProperties[comp.ObjectId.Value];

                    handler.AddLine(
                        managedProperty.LogicalName
                        , CreateFileHandler.GetLocalizedLabel(managedProperty.DisplayName)
                        , CreateFileHandler.GetLocalizedLabel(managedProperty.Description)
                        , managedProperty.EnablesEntityName
                        , managedProperty.EnablesAttributeName
                        , managedProperty.ErrorCode.ToString()
                        , managedProperty.EvaluationPriority.ToString()
                        , managedProperty.IsPrivate.ToString()
                        , managedProperty.IsGlobalForOperation.ToString()
                        , managedProperty.ManagedPropertyType.ToString()
                        , managedProperty.Operation.ToString()
                        , behavior
                        );
                }
                else
                {
                    handler.AddLine(comp.ObjectId.ToString());
                }
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public string GenerateDescriptionSingle(SolutionComponent component, bool withManaged, bool withSolutionInfo, bool withUrls)
        {
            if (this.AllManagedProperties.Any())
            {
                if (this.AllManagedProperties.ContainsKey(component.ObjectId.Value))
                {
                    var managedProperty = this.AllManagedProperties[component.ObjectId.Value];

                    FormatTextTableHandler handler = new FormatTextTableHandler();
                    handler.SetHeader(
                        "LogicalName"
                        , "DisplayName"
                        , "Description"
                        , "EnablesEntityName"
                        , "EnablesAttributeName"
                        , "ErrorCode"
                        , "EvaluationPriority"
                        , "IsPrivate"
                        , "IsGlobalForOperation"
                        , "ManagedPropertyType"
                        , "Operation"
                        );

                    handler.AddLine(
                        managedProperty.LogicalName
                        , CreateFileHandler.GetLocalizedLabel(managedProperty.DisplayName)
                        , CreateFileHandler.GetLocalizedLabel(managedProperty.Description)
                        , managedProperty.EnablesEntityName
                        , managedProperty.EnablesAttributeName
                        , managedProperty.ErrorCode.ToString()
                        , managedProperty.EvaluationPriority.ToString()
                        , managedProperty.IsPrivate.ToString()
                        , managedProperty.IsGlobalForOperation.ToString()
                        , managedProperty.ManagedPropertyType.ToString()
                        , managedProperty.Operation.ToString()
                        );

                    var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                    return string.Format("{0} {1}", this.ComponentTypeEnum.ToString(), str);
                }
            }

            return component.ToString();
        }

        public string GetName(SolutionComponent solutionComponent)
        {
            if (this.AllManagedProperties.Any())
            {
                if (this.AllManagedProperties.ContainsKey(solutionComponent.ObjectId.Value))
                {
                    var managedProperty = this.AllManagedProperties[solutionComponent.ObjectId.Value];

                    return managedProperty.LogicalName;
                }
            }

            return solutionComponent.ObjectId.ToString();
        }

        public string GetDisplayName(SolutionComponent solutionComponent)
        {
            if (this.AllManagedProperties.Any())
            {
                if (this.AllManagedProperties.ContainsKey(solutionComponent.ObjectId.Value))
                {
                    var managedProperty = this.AllManagedProperties[solutionComponent.ObjectId.Value];

                    return managedProperty.DisplayName?.UserLocalizedLabel?.Label;
                }
            }

            return null;
        }

        public string GetCustomizableName(SolutionComponent solutionComponent)
        {
            return null;
        }

        public string GetManagedName(SolutionComponent solutionComponent)
        {
            return null;
        }

        public string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            return null;
        }

        public string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            if (this.AllManagedProperties.Any())
            {
                if (this.AllManagedProperties.ContainsKey(objectId))
                {
                    var managedProperty = this.AllManagedProperties[objectId];

                    return string.Format("{0}.ManagedProperty {1} - {2} - {3}.{4}", connectionName, managedProperty.LogicalName, objectId, fieldTitle, extension);
                }
            }

            return string.Format("{0}.ComponentType {1} - {2} - {3}.{4}", connectionName, this.ComponentTypeValue, objectId, fieldTitle, extension);
        }

        public TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>();
        }
    }
}