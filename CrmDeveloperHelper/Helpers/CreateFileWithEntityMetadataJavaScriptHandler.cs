using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public sealed class CreateFileWithEntityMetadataJavaScriptHandler : CreateFileHandler
    {
        private EntityMetadata _entityMetadata;

        private readonly DependencyDescriptionHandler _descriptorHandler;
        private readonly DependencyRepository _dependencyRepository;
        private readonly SolutionComponentDescriptor _solutionComponentDescriptor;
        private readonly IOrganizationServiceExtented _service;
        private readonly CreateFileJavaScriptConfiguration _config;

        private List<StringMap> _listStringMap;

        public CreateFileWithEntityMetadataJavaScriptHandler(
            TextWriter writer
            , CreateFileJavaScriptConfiguration config
            , IOrganizationServiceExtented service
        ) : base(writer, config.TabSpacer, true)
        {
            this._config = config;
            this._service = service;

            this._solutionComponentDescriptor = new SolutionComponentDescriptor(_service)
            {
                WithManagedInfo = false,
            };

            if (_config.WithDependentComponents)
            {
                this._dependencyRepository = new DependencyRepository(_service);
                this._descriptorHandler = new DependencyDescriptionHandler(_solutionComponentDescriptor);
            }
        }

        public Task CreateFileAsync(string entityLogicalName)
        {
            return Task.Run(() => CreateFile(entityLogicalName, null));
        }

        public Task CreateFileAsync(EntityMetadata entityMetadata)
        {
            return Task.Run(() => CreateFile(entityMetadata.LogicalName, entityMetadata));
        }

        private async Task CreateFile(string entityLogicalName, EntityMetadata entityMetadata)
        {
            if (entityMetadata == null)
            {
                this._entityMetadata = this._solutionComponentDescriptor.MetadataSource.GetEntityMetadata(entityLogicalName);
            }
            else
            {
                this._entityMetadata = entityMetadata;

                this._solutionComponentDescriptor.MetadataSource.StoreEntityMetadata(entityMetadata);
            }

            var repositoryStringMap = new StringMapRepository(_service);
            this._listStringMap = await repositoryStringMap.GetListAsync(this._entityMetadata.LogicalName);

            string jsNamespace = _config.NamespaceClassesJavaScript;
            string className = (!string.IsNullOrEmpty(jsNamespace) ? jsNamespace + "." : string.Empty) + _entityMetadata.LogicalName;

            if (_config.GenerateSchemaIntoSchemaClass)
            {
                jsNamespace = className;
                className += ".Schema";
            }

            WriteCrmDeveloperAttribute(_entityMetadata.LogicalName);

            WriteNamespace(jsNamespace);

            WriteLine("{0} = {{", className);

            WriteAttributesToFile();

            WriteStateStatusEnums();

            await WriteRegularOptionSets();

            WriteLine();
            Write("};");
        }

        public void WriteCrmDeveloperAttribute(string entityName)
        {
            StringBuilder systemInfo = new StringBuilder();

            if (!string.IsNullOrEmpty(entityName))
            {
                systemInfo.AppendFormat(@" entityname=""{0}""", entityName);
            }

            if (systemInfo.Length > 0)
            {
                WriteLine($@"/// <crmdeveloperhelper{systemInfo.ToString()} />");
                WriteLine();
            }
        }

        private void WriteNamespace(string jsNamespace)
        {
            if (string.IsNullOrEmpty(jsNamespace))
            {
                return;
            }

            string[] split = jsNamespace.Split('.');

            StringBuilder str = new StringBuilder();

            foreach (var item in split)
            {
                if (str.Length > 0)
                {
                    str.Append(".");
                    WriteLine();
                }

                str.Append(item);

                WriteLine("if (typeof (" + str.ToString() + ") == 'undefined') {");
                WriteLine(str.ToString() + " = { __namespace: true };");
                WriteLine("};");
            }

            WriteLine();
        }

        private void WriteAttributesToFile()
        {
            if (_entityMetadata.Attributes == null)
            {
                return;
            }

            var attributes = _entityMetadata.Attributes.Where(a => string.IsNullOrEmpty(a.AttributeOf));

            if (!attributes.Any())
            {
                return;
            }

            WriteLine();
            WriteLine("'Attributes': {");

            bool firstLine = true;

            if (!string.IsNullOrEmpty(_entityMetadata.PrimaryIdAttribute))
            {
                AttributeMetadata attributeMetadata = attributes.FirstOrDefault(e => string.Equals(e.LogicalName, _entityMetadata.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase));

                if (attributeMetadata != null
                    && attributeMetadata.IsPrimaryId.GetValueOrDefault()
                )
                {
                    WriteCommaIfNotFirstLine(ref firstLine);

                    Write("'{0}': '{0}'", attributeMetadata.LogicalName.ToLower());
                }
            }

            if (!string.IsNullOrEmpty(_entityMetadata.PrimaryNameAttribute))
            {
                AttributeMetadata attributeMetadata = attributes.FirstOrDefault(e => string.Equals(e.LogicalName, _entityMetadata.PrimaryNameAttribute, StringComparison.InvariantCultureIgnoreCase));

                if (attributeMetadata != null
                    && attributeMetadata.IsPrimaryName.GetValueOrDefault()
                )
                {
                    WriteCommaIfNotFirstLine(ref firstLine);

                    Write("'{0}': '{0}'", attributeMetadata.LogicalName.ToLower());
                }
            }

            var notPrimaryAttributes = attributes.Where(a => !string.Equals(a.LogicalName, _entityMetadata.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase)
                    && !string.Equals(a.LogicalName, _entityMetadata.PrimaryNameAttribute, StringComparison.InvariantCultureIgnoreCase));

            var oobAttributes = notPrimaryAttributes.Where(a => !a.IsCustomAttribute.GetValueOrDefault());
            var customAttributes = notPrimaryAttributes.Where(a => a.IsCustomAttribute.GetValueOrDefault());

            if (oobAttributes.Any())
            {
                WriteCommaIfNotFirstLine(ref firstLine);

                WriteLine();

                bool firstLineOOB = true;

                foreach (AttributeMetadata attrib in oobAttributes.OrderBy(attr => attr.LogicalName))
                {
                    WriteCommaIfNotFirstLine(ref firstLineOOB);

                    Write("'{0}': '{0}'", attrib.LogicalName.ToLower());
                }
            }

            if (customAttributes.Any())
            {
                if (!firstLine)
                {
                    WriteLine(",");
                }

                WriteLine();

                bool firstLineCustom = true;

                foreach (AttributeMetadata attrib in customAttributes.OrderBy(attr => attr.LogicalName))
                {
                    WriteCommaIfNotFirstLine(ref firstLineCustom);

                    Write("'{0}': '{0}'", attrib.LogicalName.ToLower());
                }
            }

            WriteLine();
            Write("}");
        }

        private async Task WriteRegularOptionSets()
        {
            var picklists = _entityMetadata.Attributes
                .Where(a => a is PicklistAttributeMetadata || a is MultiSelectPicklistAttributeMetadata)
                .OfType<EnumAttributeMetadata>()
                .Where(e => e.OptionSet != null && e.OptionSet.Options.Any(o => o.Value.HasValue))
                ;

            foreach (var attrib in picklists
                    .Where(p => !p.OptionSet.IsGlobal.GetValueOrDefault())
                    .OrderBy(attr => attr.LogicalName)
            )
            {
                await GenerateOptionSetEnums(new[] { attrib }, attrib.OptionSet);
            }

            if (_config.GenerateGlobalOptionSets)
            {
                var groups = picklists.Where(p => p.OptionSet.IsGlobal.GetValueOrDefault()).GroupBy(p => p.OptionSet.MetadataId, (k, g) => new { g.First().OptionSet, Attributes = g }).OrderBy(e => e.OptionSet.Name);

                foreach (var optionSet in groups)
                {
                    await GenerateOptionSetEnums(optionSet.Attributes.OrderBy(a => a.LogicalName), optionSet.OptionSet);
                }
            }
        }

        private void WriteStateStatusEnums()
        {
            StateAttributeMetadata stateAttr = null;
            StatusAttributeMetadata statusAttr = null;

            foreach (AttributeMetadata attrib in _entityMetadata.Attributes.OrderBy(attr => attr.LogicalName))
            {
                if (attrib is StatusAttributeMetadata)
                {
                    statusAttr = attrib as StatusAttributeMetadata;
                }
                else if (attrib is StateAttributeMetadata)
                {
                    stateAttr = attrib as StateAttributeMetadata;
                }
            }

            if (stateAttr != null && statusAttr != null)
            {
                GenerateStateEnums(stateAttr, statusAttr);

                GenerateStatusEnums(statusAttr, stateAttr);
            }
        }

        private void GenerateStatusEnums(StatusAttributeMetadata statusAttr, StateAttributeMetadata stateAttr)
        {
            WriteLine(",");
            WriteLine();
            WriteLine("'StatusCodes': {");

            var options = GetStatusOptionItems(statusAttr, stateAttr, this._listStringMap);

            WriteLine();

            bool first = true;

            // Формируем значения
            foreach (var item in options.OrderBy(op => op.LinkedStateCode).ThenBy(op => op.Value))
            {
                WriteCommaIfNotFirstLine(ref first);

                Write(item.MakeStringJS());
            }

            WriteLine();
            Write("}");
        }

        private void GenerateStateEnums(StateAttributeMetadata stateAttr, StatusAttributeMetadata statusAttr)
        {
            WriteLine(",");
            WriteLine();
            WriteLine("'StateCodes': {");

            var options = GetStateOptionItems(statusAttr, stateAttr, this._listStringMap);

            WriteLine();

            bool first = true;

            // Формируем значения
            foreach (var item in options)
            {
                WriteCommaIfNotFirstLine(ref first);

                Write(item.MakeStringJS());
            }

            WriteLine();
            Write("}");
        }

        private async Task GenerateOptionSetEnums(IEnumerable<AttributeMetadata> attributeList, OptionSetMetadata optionSet)
        {
            var options = GetOptionItems(attributeList.First().EntityLogicalName, attributeList.First().LogicalName, optionSet, this._listStringMap);

            if (!options.Any())
            {
                return;
            }

            WriteLine(",");
            WriteLine();

            WriteLine("// Attribute:");
            foreach (var attr in attributeList.OrderBy(a => a.LogicalName))
            {
                WriteLine("// " + _tabSpacer + attr.LogicalName);
            }

            if (optionSet.IsGlobal.GetValueOrDefault() && this._config.WithDependentComponents)
            {
                var coll = await _dependencyRepository.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet.MetadataId.Value);

                var desc = await _descriptorHandler.GetDescriptionDependentAsync(coll);

                var split = desc.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Any())
                {
                    WriteSummaryStrings(split);
                }
            }

            bool ignore = attributeList.Any(a => IgnoreAttribute(_entityMetadata.LogicalName, a.LogicalName));

            if (ignore)
            {
                foreach (var attr in attributeList.OrderBy(a => a.LogicalName))
                {
                    WriteLine("//var {0}Enum", attr.LogicalName);
                }
                return;
            }

            var enumName = string.Empty;

            if (optionSet.IsGlobal.GetValueOrDefault())
            {
                enumName = optionSet.Name;
            }
            else
            {
                enumName = attributeList.First().LogicalName;
            }

            WriteLine(string.Format("'{0}Enum':", enumName) + " {");

            WriteLine();

            bool first = true;

            // Формируем значения
            foreach (var item in options.OrderBy(o => o.Value))
            {
                WriteCommaIfNotFirstLine(ref first);

                Write(item.MakeStringJS());
            }

            WriteLine();
            Write("}");
        }

        protected override void WriteSummaryStrings(IEnumerable<string> lines)
        {
            if (lines.Any())
            {
                foreach (var item in lines)
                {
                    WriteLine("// {0}", item);
                }
            }
        }
    }
}
