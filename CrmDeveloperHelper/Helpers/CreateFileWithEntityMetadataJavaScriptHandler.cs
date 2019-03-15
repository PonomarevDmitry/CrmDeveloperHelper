using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
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
    public class CreateFileWithEntityMetadataJavaScriptHandler : CreateFileHandler
    {
        private EntityMetadata _entityMetadata;
        private DependencyDescriptionHandler _descriptorHandler;
        private DependencyRepository _dependencyRepository;

        public IOrganizationServiceExtented _service;
        private CreateFileWithEntityMetadataJavaScriptConfiguration _config;

        private List<StringMap> _listStringMap;

        private readonly IWriteToOutput iWriteToOutput;

        public CreateFileWithEntityMetadataJavaScriptHandler(
            CreateFileWithEntityMetadataJavaScriptConfiguration config
            , IOrganizationServiceExtented service
            , IWriteToOutput outputWindow
            ) : base(config.TabSpacer, true)
        {
            this._config = config;
            this._service = service;
            this.iWriteToOutput = outputWindow;
        }

        public Task<string> CreateFileAsync(string fileName = null)
        {
            return Task.Run(async () => await CreateFile(fileName));
        }

        private async Task<string> CreateFile(string fileName = null)
        {
            RetrieveEntityRequest request = new RetrieveEntityRequest
            {
                LogicalName = _config.EntityName.ToLower(),

                EntityFilters = EntityFilters.All
            };

            RetrieveEntityResponse response = (RetrieveEntityResponse)_service.Execute(request);
            this._entityMetadata = response.EntityMetadata;

            if (_config.WithDependentComponents)
            {
                this._dependencyRepository = new DependencyRepository(_service);
                this._descriptorHandler = new DependencyDescriptionHandler(new SolutionComponentDescriptor(_service));
            }

            var repositoryStringMap = new StringMapRepository(_service);
            this._listStringMap = await repositoryStringMap.GetListAsync(this._entityMetadata.LogicalName);

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = string.Format("{0}.{1}.Class.js", this._service.ConnectionData.Name, _entityMetadata.SchemaName);
            }

            var fileFilePath = Path.Combine(this._config.Folder, fileName);

            StartWriting(fileFilePath);

            WriteNameSpace();

            WriteLine();

            string tempNameSpace = !string.IsNullOrEmpty(this._service.ConnectionData.NameSpaceClasses) ? this._service.ConnectionData.NameSpaceClasses + "." : string.Empty;

            WriteLine(string.Format("{0}{1} = (new function () ", tempNameSpace, _entityMetadata.LogicalName) + "{");

            WriteAttributesToFile();

            WriteStateStatusEnums();

            await WriteRegularOptionSets();

            WriteLine();
            WriteLine(CreateFileHandler.JavaScriptCommonConstants);
            WriteLine();
            WriteLine(CreateFileHandler.JavaScriptCommonFunctions);
            WriteLine();

            WriteLine();
            WriteLine("var pthis = this;");

            WriteLine();
            WriteLine("return pthis;");

            Write("}());");

            EndWriting();

            return fileFilePath;
        }

        private void WriteNameSpace()
        {
            if (string.IsNullOrEmpty(this._service.ConnectionData.NameSpaceClasses))
            {
                return;
            }

            string[] split = this._service.ConnectionData.NameSpaceClasses.Split('.');

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

            WriteLine(string.Format("var {0}Enum =", enumName) + " {");

            WriteLine();

            // Формируем значения
            foreach (var item in options)
            {
                WriteLine(item.MakeStringJS());
            }

            WriteLine("};");
        }

        private void WriteSummaryStrings(IEnumerable<string> lines)
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
