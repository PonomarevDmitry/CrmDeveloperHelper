using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class ManagedPropertyDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ManagedPropertyDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ManagedProperty)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ManagedProperty;

        public override int ComponentTypeValue => (int)ComponentType.ManagedProperty;

        public override string EntityLogicalName => string.Empty;

        public override string EntityPrimaryIdAttribute => string.Empty;

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

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
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
                //, "Description"
                //, "Description"
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
                        //, managedProperty.IsManaged.ToString()
                        //, managedProperty.IsManaged.ToString()
                        //, managedProperty.IsManaged.ToString()
                        //, managedProperty.IsManaged.ToString()
                        //, managedProperty.IsManaged.ToString()
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

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            if (this.AllManagedProperties.Any())
            {
                if (this.AllManagedProperties.ContainsKey(component.ObjectId.Value))
                {
                    var managedProperty = this.AllManagedProperties[component.ObjectId.Value];

                    return $"LogicalName {managedProperty.LogicalName}    DisplayName {CreateFileHandler.GetLocalizedLabel(managedProperty.DisplayName)}"
                        + $"    Description {CreateFileHandler.GetLocalizedLabel(managedProperty.Description)}    EnablesEntityName {managedProperty.EnablesEntityName}"
                        + $"    EnablesAttributeName {managedProperty.EnablesAttributeName}    ErrorCode {managedProperty.ErrorCode.ToString()}"
                        + $"    EvaluationPriority {managedProperty.EvaluationPriority.ToString()}    IsPrivate {managedProperty.IsPrivate.ToString()}"
                        + $"    IsGlobalForOperation {managedProperty.IsGlobalForOperation.ToString()}    ManagedPropertyType {managedProperty.ManagedPropertyType.ToString()}"
                        + $"    Operation {managedProperty.Operation.ToString()}"
                        //+ $"    IsManaged {1}    IsManaged {1}"
                        ;
                    //, managedProperty.EnablesAttributeName
                    //, managedProperty.ErrorCode.ToString()
                    //, managedProperty.EvaluationPriority.ToString()
                    //, managedProperty.IsPrivate.ToString()
                    //, managedProperty.IsGlobalForOperation.ToString()
                    //, managedProperty.ManagedPropertyType.ToString()
                    //, managedProperty.Operation.ToString()
                }
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent solutionComponent)
        {
            if (this.AllManagedProperties.Any())
            {
                if (this.AllManagedProperties.ContainsKey(solutionComponent.ObjectId.Value))
                {
                    var managedProperty = this.AllManagedProperties[solutionComponent.ObjectId.Value];

                    return managedProperty.LogicalName;
                }
            }

            return base.GetName(solutionComponent);
        }

        public override string GetDisplayName(SolutionComponent solutionComponent)
        {
            if (this.AllManagedProperties.Any())
            {
                if (this.AllManagedProperties.ContainsKey(solutionComponent.ObjectId.Value))
                {
                    var managedProperty = this.AllManagedProperties[solutionComponent.ObjectId.Value];

                    return managedProperty.DisplayName?.UserLocalizedLabel?.Label;
                }
            }

            return base.GetDisplayName(solutionComponent);
        }

        public override string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            if (this.AllManagedProperties.Any())
            {
                if (this.AllManagedProperties.ContainsKey(objectId))
                {
                    var managedProperty = this.AllManagedProperties[objectId];

                    return managedProperty.LogicalName;
                }
            }

            return base.GetFileName(connectionName, objectId, fieldTitle, extension);
        }
    }
}