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

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<PluginAssembly>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            var table = new FormatTextTableHandler();
            table.SetHeader("Name", "IsManged", "IsCustomizable", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string name = entity.Name;

                table.AddLine(name
                    , entity.IsManaged.ToString()
                    , entity.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var pluginAssembly = GetEntity<PluginAssembly>(component.ObjectId.Value);

            if (pluginAssembly != null)
            {
                string name = pluginAssembly.Name;

                return string.Format("PluginAssembly {0}    IsManged {1}    IsManged {2}    SolutionName {3}"
                    , name
                    , pluginAssembly.IsManaged.ToString()
                    , pluginAssembly.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(pluginAssembly, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
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

                    Description = GenerateDescriptionSingle(solutionComponent, false),
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

            if (!String.IsNullOrEmpty(solutionImageComponent.SchemaName))
            {
                var match = _regexSchemaName.Match(solutionImageComponent.SchemaName);

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
                            var component = new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue(this.ComponentTypeValue),
                                ObjectId = entity.Id,
                                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                            };

                            if (solutionImageComponent.RootComponentBehavior.HasValue)
                            {
                                component.RootComponentBehavior = new OptionSetValue(solutionImageComponent.RootComponentBehavior.Value);
                            }

                            result.Add(component);

                            return;
                        }
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