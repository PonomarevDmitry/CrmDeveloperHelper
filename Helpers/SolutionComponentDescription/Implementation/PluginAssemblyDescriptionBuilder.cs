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
    public class PluginAssemblyDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public PluginAssemblyDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.PluginAssembly)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.PluginAssembly;

        public override int ComponentTypeValue => (int)ComponentType.PluginAssembly;

        public override string EntityLogicalName => PluginAssembly.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => PluginAssembly.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
            (
                PluginAssembly.Schema.Attributes.name
                , PluginAssembly.Schema.Attributes.ismanaged
                , PluginAssembly.Schema.Attributes.iscustomizable
                , PluginAssembly.Schema.Attributes.culture
                , PluginAssembly.Schema.Attributes.version
                , PluginAssembly.Schema.Attributes.publickeytoken
            );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<PluginAssembly>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            var entity = GetEntity<PluginAssembly>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                result.Add(new SolutionImageComponent()
                {
                    ComponentType = (int)ComponentType.PluginAssembly,
                    SchemaName = string.Format("{0}, Version={1}, Culture={2}, PublicKeyToken={3}", entity.Name, entity.Version, entity.Culture, entity.PublicKeyToken),
                    RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
                });
            }
        }

        private static readonly Regex _regexSchemaName = new Regex(@"^([\w\.]+), Version=(\d+\.\d+\.\d+\.\d+), Culture=([\w]+), PublicKeyToken=([0-9A-Fa-f]{16,})$", RegexOptions.Compiled);

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

        private void FillSolutionComponentFromSchemaName(ICollection<SolutionComponent> result, string schemaName, int? behavior)
        {
            if (string.IsNullOrEmpty(schemaName))
            {
                return;
            }

            var match = _regexSchemaName.Match(schemaName);

            if (match.Success && match.Groups.Count == 5)
            {
                string name = match.Groups[1].Value;
                string versionString = match.Groups[2].Value;
                string cultureString = match.Groups[3].Value;
                string publicKeyTokenString = match.Groups[4].Value;

                if (!string.IsNullOrEmpty(name)
                    && !string.IsNullOrEmpty(versionString)
                    && !string.IsNullOrEmpty(cultureString)
                    && !string.IsNullOrEmpty(publicKeyTokenString)
                    )
                {
                    var repository = new PluginAssemblyRepository(_service);

                    var entity = repository.FindAssemblyByFullName(name, versionString, cultureString, publicKeyTokenString, new ColumnSet(false));

                    if (entity != null)
                    {
                        FillSolutionComponentInternal(result, entity.Id, behavior);

                        return;
                    }
                }
            }
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { PluginAssembly.Schema.Attributes.name, "Name" }
                    , { PluginAssembly.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { PluginAssembly.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}