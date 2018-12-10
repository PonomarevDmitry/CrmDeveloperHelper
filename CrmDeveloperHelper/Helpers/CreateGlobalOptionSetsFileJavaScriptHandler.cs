using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
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
    public class CreateGlobalOptionSetsFileJavaScriptHandler : CreateFileHandler
    {
        private IOrganizationServiceExtented _service;
        private bool _withDependentComponents;

        SolutionComponentDescriptor _descriptor;
        DependencyDescriptionHandler _descriptorHandler;
        DependencyRepository _dependencyRepository;

        private StringMapRepository _repositoryStringMap;

        private readonly IWriteToOutput _iWriteToOutput;

        public CreateGlobalOptionSetsFileJavaScriptHandler(
            IOrganizationServiceExtented service
            , IWriteToOutput iWriteToOutput
            , string tabSpacer
            , bool withDependentComponents
            ) : base(tabSpacer, true)
        {
            this._service = service;
            this._iWriteToOutput = iWriteToOutput;
            this._withDependentComponents = withDependentComponents;

            this._descriptor = new SolutionComponentDescriptor(_service, false);
            this._dependencyRepository = new DependencyRepository(this._service);
            this._descriptorHandler = new DependencyDescriptionHandler(this._descriptor);
            _repositoryStringMap = new StringMapRepository(_service);
        }

        public Task CreateFileAsync(string filePath, IEnumerable<OptionSetMetadata> optionSets)
        {
            return Task.Run(async () => await this.CreateFile(filePath, optionSets));
        }

        private async Task CreateFile(string filePath, IEnumerable<OptionSetMetadata> optionSets)
        {
            optionSets = optionSets
               .Where(e => e.Options.Any(o => o.Value.HasValue))
               .OrderBy(e => e.Name);

            StartWriting(filePath);

            WriteNameSpace();

            WriteLine();

            string tempNameSpace = !string.IsNullOrEmpty(this._service.ConnectionData.NameSpaceOptionSets) ? this._service.ConnectionData.NameSpaceOptionSets + "." : string.Empty;

            WriteLine(string.Format("{0}GlobalOptionSets = (new function () ", tempNameSpace) + "{");

            await WriteRegularOptionSets(optionSets);

            WriteLine();
            WriteLine("var pthis = this;");

            WriteLine();
            WriteLine("return pthis;");

            Write("}());");

            EndWriting();
        }

        private void WriteNameSpace()
        {
            if (string.IsNullOrEmpty(this._service.ConnectionData.NameSpaceOptionSets))
            {
                return;
            }

            string[] split = this._service.ConnectionData.NameSpaceOptionSets.Split('.');

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

        private async Task WriteRegularOptionSets(IEnumerable<OptionSetMetadata> optionSets)
        {
            foreach (var optionSetMetadata in optionSets)
            {
                await GenerateOptionSetEnums(optionSetMetadata);
            }
        }

        private async Task GenerateOptionSetEnums(OptionSetMetadata optionSet)
        {
            var dependent = await _dependencyRepository.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet.MetadataId.Value);

            string entityname = null;
            string attributename = null;
            List<StringMap> listStringmap = null;

            if (dependent.Any(e => e.DependentComponentType.Value == (int)ComponentType.Attribute))
            {
                var attr = dependent.FirstOrDefault(e => e.DependentComponentType.Value == (int)ComponentType.Attribute);

                var attributeMetadata = _descriptor.MetadataSource.GetAttributeMetadata(attr.DependentComponentObjectId.Value);

                if (attributeMetadata != null)
                {
                    entityname = attributeMetadata.EntityLogicalName;
                    attributename = attributeMetadata.LogicalName;

                    listStringmap = await _repositoryStringMap.GetListAsync(entityname);
                }
            }

            var options = CreateFileHandler.GetOptionItems(entityname, attributename, optionSet, listStringmap);

            if (!options.Any())
            {
                return;
            }

            WriteLine();

            if (this._withDependentComponents)
            {
                var desc = await _descriptorHandler.GetDescriptionDependentAsync(dependent);

                var split = desc.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in split)
                {
                    WriteLine("// {0}", item);
                }
            }

            if (CreateFileHandler.IgnoreGlobalOptionSet(optionSet.Name))
            {
                WriteLine("//var {0}Enum", optionSet.Name);
                return;
            }

            WriteLine(string.Format("var {0}Enum =", optionSet.Name) + " {");

            WriteLine();

            // Формируем значения
            foreach (var item in options)
            {
                WriteLine(item.MakeStringJS());
            }

            WriteLine("};");
        }
    }
}
