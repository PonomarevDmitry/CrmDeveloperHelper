using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class CreateFileWithEntityMetadataJavaScriptHandler : CreateFileHandler
    {
        private EntityMetadata _entityMetadata;
        private readonly DependencyDescriptionHandler _descriptorHandler;
        private readonly DependencyRepository _dependencyRepository;
        private readonly SolutionComponentDescriptor _solutionComponentDescriptor;

        private IOrganizationServiceExtented _service;
        private CreateFileWithEntityMetadataJavaScriptConfiguration _config;

        private List<StringMap> _listStringMap;

        private readonly IWriteToOutput iWriteToOutput;

        private List<string> _attributesForPthis = new List<string>();

        public CreateFileWithEntityMetadataJavaScriptHandler(
            CreateFileWithEntityMetadataJavaScriptConfiguration config
            , IOrganizationServiceExtented service
            , IWriteToOutput outputWindow
        ) : base(config.TabSpacer, true)
        {
            this._config = config;
            this._service = service;
            this.iWriteToOutput = outputWindow;

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

        public Task CreateFileAsync(string filePath)
        {
            return Task.Run(async () => await CreateFile(filePath));
        }

        private async Task CreateFile(string filePath)
        {
            if (_config.EntityMetadata == null)
            {
                this._entityMetadata = this._solutionComponentDescriptor.MetadataSource.GetEntityMetadata(_config.EntityName);
            }
            else
            {
                this._entityMetadata = _config.EntityMetadata;

                this._solutionComponentDescriptor.MetadataSource.StoreEntityMetadata(_config.EntityMetadata);
            }

            var repositoryStringMap = new StringMapRepository(_service);
            this._listStringMap = await repositoryStringMap.GetListAsync(this._entityMetadata.LogicalName);

            StartWriting(filePath);

            WriteNamespace();

            WriteLine();

            string tempNamespace = !string.IsNullOrEmpty(this._service.ConnectionData.NamespaceJavaScript) ? this._service.ConnectionData.NamespaceJavaScript + "." : string.Empty;

            WriteLine(string.Format("{0}{1} = (new function () ", tempNamespace, _entityMetadata.LogicalName) + "{");

            WriteAttributesToFile();

            WriteStateStatusEnums();

            await WriteRegularOptionSets();

            WriteLine();
            WriteLine("var pthis = this;");

            if (_attributesForPthis.Any())
            {
                WriteLine();

                foreach (var item in _attributesForPthis)
                {
                    WriteLine(string.Format("pthis.{0} = {0};", item));
                }
            }

            WriteLine();
            WriteLine("return pthis;");

            Write("}());");

            EndWriting();
        }

        private void WriteNamespace()
        {
            if (string.IsNullOrEmpty(this._service.ConnectionData.NamespaceJavaScript))
            {
                return;
            }

            string[] split = this._service.ConnectionData.NamespaceJavaScript.Split('.');

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
                WriteLine("}");
            }
        }

        private void WriteAttributesToFile()
        {
            if (_entityMetadata.Attributes == null)
            {
                return;
            }

            var attributes = _entityMetadata.Attributes
                .Where(a => string.IsNullOrEmpty(a.AttributeOf));

            if (!attributes.Any())
            {
                return;
            }

            _attributesForPthis.Add("Attributes");

            WriteLine();
            WriteLine("var Attributes = {");

            foreach (AttributeMetadata attrib in attributes.OrderBy(attr => attr.LogicalName))
            {
                WriteLine("'{0}': '{0}',", attrib.LogicalName.ToLower());
            }

            WriteLine("};");
        }

        private async Task WriteRegularOptionSets()
        {
            var picklists = _entityMetadata.Attributes
                .OfType<PicklistAttributeMetadata>()
                .Where(e => e.OptionSet.Options.Any(o => o.Value.HasValue))
                ;

            foreach (var attrib in picklists
                    .Where(p => !p.OptionSet.IsGlobal.GetValueOrDefault())
                    .OrderBy(attr => attr.LogicalName)
            )
            {
                await GenerateOptionSetEnums(new[] { attrib }, attrib.OptionSet);
            }

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
            WriteLine();
            WriteLine("var StatusCodes = {");

            _attributesForPthis.Add("StatusCodes");

            var options = CreateFileHandler.GetStatusOptionItems(statusAttr, stateAttr, this._listStringMap);

            WriteLine();

            // Формируем значения
            foreach (var item in options.OrderBy(op => op.LinkedStateCode).ThenBy(op => op.Value))
            {
                WriteLine(item.MakeStringJS());
            }

            WriteLine("};");
        }

        private void GenerateStateEnums(StateAttributeMetadata stateAttr, StatusAttributeMetadata statusAttr)
        {
            WriteLine();
            WriteLine("var StateCodes = {");

            _attributesForPthis.Add("StateCodes");

            var options = CreateFileHandler.GetStateOptionItems(statusAttr, stateAttr, this._listStringMap);

            WriteLine();

            // Формируем значения
            foreach (var item in options)
            {
                WriteLine(item.MakeStringJS());
            }

            WriteLine("};");
        }

        private async Task GenerateOptionSetEnums(IEnumerable<AttributeMetadata> attributeList, OptionSetMetadata optionSet)
        {
            var options = CreateFileHandler.GetOptionItems(attributeList.First().EntityLogicalName, attributeList.First().LogicalName, optionSet, this._listStringMap);

            if (!options.Any())
            {
                return;
            }

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

            _attributesForPthis.Add(string.Format("{0}Enum", enumName));

            WriteLine(string.Format("var {0}Enum =", enumName) + " {");

            WriteLine();

            // Формируем значения
            foreach (var item in options)
            {
                WriteLine(item.MakeStringJS());
            }

            WriteLine("};");
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