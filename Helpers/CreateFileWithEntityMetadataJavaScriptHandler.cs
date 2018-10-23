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

        DependencyDescriptionHandler _descriptorHandler;
        DependencyRepository _dependencyRepository;

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
            RetrieveEntityRequest request = new RetrieveEntityRequest();
            request.LogicalName = _config.EntityName.ToLower();

            request.EntityFilters = EntityFilters.All;

            RetrieveEntityResponse response = (RetrieveEntityResponse)_service.Execute(request);
            this._entityMetadata = response.EntityMetadata;

            if (_config.WithDependentComponents)
            {
                this._dependencyRepository = new DependencyRepository(_service);
                this._descriptorHandler = new DependencyDescriptionHandler(new SolutionComponentDescriptor(_service, false));
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
            var pickLists = _entityMetadata.Attributes
                .OfType<PicklistAttributeMetadata>()
                .Where(e => e.OptionSet.Options.Any(o => o.Value.HasValue))
                .OrderBy(attr => attr.LogicalName);

            foreach (var attrib in pickLists)
            {
                await GenerateOptionSetEnums(attrib, attrib.OptionSet);
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

        private async Task GenerateOptionSetEnums(AttributeMetadata attrib, OptionSetMetadata optionSet)
        {
            var options = CreateFileHandler.GetOptionItems(attrib.EntityLogicalName, attrib.LogicalName, optionSet, this._listStringMap);

            if (!options.Any())
            {
                return;
            }

            WriteLine();

            if (optionSet.IsGlobal.GetValueOrDefault() && this._config.WithDependentComponents)
            {
                List<string> lines = new List<string>();

                var coll = await _dependencyRepository.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet.MetadataId.Value);

                var desc = await _descriptorHandler.GetDescriptionDependentAsync(coll);

                var split = desc.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Any())
                {
                    foreach (var item in split)
                    {
                        lines.Add(item);
                    }
                }

                WriteSummaryStrings(lines);
            }

            if (CreateFileHandler.IgnoreAttribute(_entityMetadata.LogicalName, attrib.LogicalName))
            {
                WriteLine("//var {0}Enum", attrib.LogicalName);
                return;
            }

            var enumName = attrib.LogicalName;

            if (optionSet.IsGlobal.GetValueOrDefault())
            {
                enumName = optionSet.Name;
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

        private void WriteSummaryStrings(List<string> lines)
        {
            if (lines.Count > 0)
            {
                foreach (var item in lines)
                {
                    WriteLine("// {0}", item);
                }
            }
        }
    }
}
