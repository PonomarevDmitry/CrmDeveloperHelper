using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class OptionSetMetadataDescriptionBuilder : ISolutionComponentDescriptionBuilder
    {
        private const string formatSpacer = DefaultSolutionComponentDescriptionBuilder.formatSpacer;
        private const string unknowedMessage = DefaultSolutionComponentDescriptionBuilder.unknowedMessage;

        private readonly SolutionComponentMetadataSource _source;

        public int ComponentTypeValue => (int)ComponentType.OptionSet;

        public ComponentType? ComponentTypeEnum => ComponentType.OptionSet;

        public string EntityLogicalName => string.Empty;

        public string EntityPrimaryIdAttribute => string.Empty;

        public OptionSetMetadataDescriptionBuilder(SolutionComponentMetadataSource source)
        {
            this._source = source;
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
            if (solutionComponent == null || !solutionComponent.ObjectId.HasValue)
            {
                return;
            }

            if (!this._source.AllOptionSetMetadata.ContainsKey(solutionComponent.ObjectId.Value))
            {
                return;
            }

            var optionSet = this._source.AllOptionSetMetadata[solutionComponent.ObjectId.Value];

            result.Add(new SolutionImageComponent()
            {
                ComponentType = (int)ComponentType.OptionSet,
                SchemaName = optionSet.Name,
                RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
            });
        }

        public void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null
                || string.IsNullOrEmpty(solutionImageComponent.SchemaName)
                )
            {
                return;
            }

            if (this._source.AllOptionSetMetadata == null)
            {
                return;
            }

            var optionSet = this._source.AllOptionSetMetadata.Values.FirstOrDefault(o => string.Equals(o.Name, solutionImageComponent.SchemaName, StringComparison.InvariantCultureIgnoreCase));

            if (optionSet != null)
            {
                int? behavior = solutionImageComponent.RootComponentBehavior;

                FillSolutionComponentInternal(result, optionSet, behavior);
            }
        }

        public void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {
            if (elementRootComponent == null)
            {
                return;
            }

            var schemaName = DefaultSolutionComponentDescriptionBuilder.GetSchemaNameFromXml(elementRootComponent);

            if (!string.IsNullOrEmpty(schemaName))
            {
                var optionSet = this._source.AllOptionSetMetadata.Values.FirstOrDefault(o => string.Equals(o.Name, schemaName, StringComparison.InvariantCultureIgnoreCase));

                if (optionSet != null)
                {
                    int? behavior = DefaultSolutionComponentDescriptionBuilder.GetBehaviorFromXml(elementRootComponent);

                    FillSolutionComponentInternal(result, optionSet, behavior);
                }
            }
        }

        private void FillSolutionComponentInternal(ICollection<SolutionComponent> result, OptionSetMetadataBase optionSet, int? behavior)
        {
            var component = new SolutionComponent()
            {
                ComponentType = new OptionSetValue(this.ComponentTypeValue),
                ObjectId = optionSet.MetadataId.Value,
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
            FormatTextTableHandler handlerUnknowed = new FormatTextTableHandler();

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("OptionSetName", "IsCustomizable", "Behavior");

            if (withManaged)
            {
                handler.AppendHeader("IsManaged");
            }

            if (withUrls)
            {
                handler.AppendHeader("Url");
            }

            foreach (var comp in components)
            {
                string behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior?.Value);

                if (this._source.AllOptionSetMetadata.ContainsKey(comp.ObjectId.Value))
                {
                    var optionSet = this._source.AllOptionSetMetadata[comp.ObjectId.Value];

                    List<string> values = new List<string>();

                    values.AddRange(new[]
                    {
                        optionSet.Name
                        , optionSet.IsCustomizable?.Value.ToString()
                        , behavior
                    });

                    if (withManaged)
                    {
                        values.Add(optionSet.IsManaged.ToString());
                    }

                    if (withUrls)
                    {
                        values.Add(_source.Service.ConnectionData?.GetGlobalOptionSetUrl(optionSet.MetadataId.Value));
                    }

                    handler.AddLine(values);
                }
                else
                {
                    handlerUnknowed.AddLine(comp.ObjectId.ToString(), behavior);
                }
            }

            if (handlerUnknowed.Count > 0)
            {
                List<string> lines = handlerUnknowed.GetFormatedLines(true);
                builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            }

            {
                List<string> lines = handler.GetFormatedLines(true);

                lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            }
        }

        public string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withManaged, bool withSolutionInfo, bool withUrls)
        {
            if (this._source.AllOptionSetMetadata.Any())
            {
                if (this._source.AllOptionSetMetadata.ContainsKey(solutionComponent.ObjectId.Value))
                {
                    var optionSet = this._source.AllOptionSetMetadata[solutionComponent.ObjectId.Value];

                    string behavior = SolutionComponent.GetRootComponentBehaviorName(solutionComponent.RootComponentBehavior?.Value);

                    FormatTextTableHandler handler = new FormatTextTableHandler();
                    handler.SetHeader("OptionSetName", "IsCustomizable", "Behavior");

                    if (withManaged)
                    {
                        handler.AppendHeader("IsManaged");
                    }

                    if (withUrls)
                    {
                        handler.AppendHeader("Url");
                    }

                    List<string> values = new List<string>();

                    values.AddRange(new[]
                    {
                        optionSet.Name
                        , optionSet.IsCustomizable?.Value.ToString()
                        , behavior
                    });

                    if (withManaged)
                    {
                        values.Add(optionSet.IsManaged.ToString());
                    }

                    if (withUrls)
                    {
                        values.Add(_source.Service.ConnectionData?.GetGlobalOptionSetUrl(optionSet.MetadataId.Value));
                    }

                    handler.AddLine(values);

                    var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                    return string.Format("{0} {1}", this.ComponentTypeEnum.ToString(), str);
                }
            }

            return solutionComponent.ToString();
        }

        public string GetName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || !solutionComponent.ObjectId.HasValue)
            {
                return "null";
            }

            if (this._source.AllOptionSetMetadata.Any())
            {
                if (this._source.AllOptionSetMetadata.ContainsKey(solutionComponent.ObjectId.Value))
                {
                    var optionSet = this._source.AllOptionSetMetadata[solutionComponent.ObjectId.Value];

                    return optionSet.Name;
                }
            }

            return solutionComponent.ObjectId.ToString();
        }

        public string GetDisplayName(SolutionComponent solutionComponent)
        {
            if (this._source.AllOptionSetMetadata.Any())
            {
                if (this._source.AllOptionSetMetadata.ContainsKey(solutionComponent.ObjectId.Value))
                {
                    var optionSet = this._source.AllOptionSetMetadata[solutionComponent.ObjectId.Value];

                    return optionSet.DisplayName?.UserLocalizedLabel?.Label;
                }
            }

            return null;
        }

        public string GetCustomizableName(SolutionComponent solutionComponent)
        {
            if (this._source.AllOptionSetMetadata.Any())
            {
                if (this._source.AllOptionSetMetadata.ContainsKey(solutionComponent.ObjectId.Value))
                {
                    var optionSet = this._source.AllOptionSetMetadata[solutionComponent.ObjectId.Value];

                    return optionSet.IsCustomizable?.Value.ToString();
                }
            }

            return null;
        }

        public string GetManagedName(SolutionComponent solutionComponent)
        {
            if (this._source.AllOptionSetMetadata.Any())
            {
                if (this._source.AllOptionSetMetadata.ContainsKey(solutionComponent.ObjectId.Value))
                {
                    var optionSet = this._source.AllOptionSetMetadata[solutionComponent.ObjectId.Value];

                    return optionSet.IsManaged.ToString();
                }
            }

            return null;
        }

        public string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            return null;
        }

        public string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            if (this._source.AllOptionSetMetadata.Any())
            {
                if (this._source.AllOptionSetMetadata.ContainsKey(objectId))
                {
                    var optionSet = this._source.AllOptionSetMetadata[objectId];

                    return string.Format("{0}.OptionSet {1} - {2} - {3}.{4}", connectionName, optionSet.Name, objectId, fieldTitle, extension);
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