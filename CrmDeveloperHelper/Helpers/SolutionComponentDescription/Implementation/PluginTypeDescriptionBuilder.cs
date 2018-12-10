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
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class PluginTypeDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public PluginTypeDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.PluginType)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.PluginType;

        public override int ComponentTypeValue => (int)ComponentType.PluginType;

        public override string EntityLogicalName => PluginType.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => PluginType.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
            (
                PluginType.Schema.Attributes.assemblyname
                , PluginType.Schema.Attributes.typename
                , PluginType.Schema.Attributes.culture
                , PluginType.Schema.Attributes.version
                , PluginType.Schema.Attributes.publickeytoken
                , PluginType.Schema.Attributes.ismanaged
            );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("PluginType", "PluginAssembly", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<PluginType>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.TypeName
                , entity.AssemblyName
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<PluginType>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0}, {1}", entity.TypeName, entity.AssemblyName);
            }

            return base.GetName(component);
        }

        public override void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            var entity = GetEntity<PluginType>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                result.Add(new SolutionImageComponent()
                {
                    ComponentType = (int)ComponentType.PluginType,
                    SchemaName = string.Format("{0}, {1}, Version={2}, Culture={3}, PublicKeyToken={4}", entity.TypeName, entity.AssemblyName, entity.Version, entity.Culture, entity.PublicKeyToken),
                    RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
                });
            }
        }

        private static readonly Regex _regexSchemaName = new Regex(@"^([\w\.]+), ([\w\.]+), Version=(\d+\.\d+\.\d+\.\d+), Culture=([\w]+), PublicKeyToken=([0-9A-Fa-f]{16,})$", RegexOptions.Compiled);

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null)
            {
                return;
            }

            string schemaName = solutionImageComponent.SchemaName;
            int? behavior = solutionImageComponent.RootComponentBehavior;

            FillSolutionComponentFromSchemaName(result, schemaName, behavior);
        }

        public override void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {
            var schemaName = GetSchemaNameFromXml(elementRootComponent);
            var behavior = GetBehaviorFromXml(elementRootComponent);

            FillSolutionComponentFromSchemaName(result, schemaName, behavior);
        }

        private void FillSolutionComponentFromSchemaName(ICollection<SolutionComponent> result, string pluginTypeName, int? behavior)
        {
            if (string.IsNullOrEmpty(pluginTypeName))
            {
                return;
            }

            var match = _regexSchemaName.Match(pluginTypeName);

            if (match.Success && match.Groups.Count == 6)
            {
                string name = match.Groups[1].Value;
                string assemblyName = match.Groups[2].Value;
                string versionString = match.Groups[3].Value;
                string cultureString = match.Groups[4].Value;
                string publicKeyTokenString = match.Groups[5].Value;

                if (!string.IsNullOrEmpty(name)
                    && !string.IsNullOrEmpty(versionString)
                    && !string.IsNullOrEmpty(cultureString)
                    && !string.IsNullOrEmpty(publicKeyTokenString)
                    )
                {
                    var repository = new PluginTypeRepository(_service);

                    var entity = repository.FindTypeByFullName(name, assemblyName, versionString, cultureString, publicKeyTokenString, new ColumnSet(false));

                    if (entity != null)
                    {
                        FillSolutionComponentInternal(result, entity.Id, behavior);
                    }
                }
            }
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { PluginType.Schema.Attributes.assemblyname, "AssemblyName" }
                    , { PluginType.Schema.Attributes.typename, "TypeName" }
                    , { PluginType.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}