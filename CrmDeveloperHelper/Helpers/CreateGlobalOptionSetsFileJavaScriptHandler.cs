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
    public class CreateGlobalOptionSetsFileJavaScriptHandler : CreateFileHandler
    {
        private readonly IOrganizationServiceExtented _service;
        private readonly bool _withDependentComponents;

        private readonly SolutionComponentDescriptor _descriptor;
        private readonly DependencyDescriptionHandler _descriptorHandler;
        private readonly DependencyRepository _dependencyRepository;

        private readonly StringMapRepository _repositoryStringMap;

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly string _namespaceGlobalOptionSetsJavaScript;

        public CreateGlobalOptionSetsFileJavaScriptHandler(
            TextWriter writer
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , IWriteToOutput iWriteToOutput
            , string tabSpacer
            , bool withDependentComponents
            , string namespaceGlobalOptionSetsJavaScript
        ) : base(writer, tabSpacer, true)
        {
            this._service = service;
            this._iWriteToOutput = iWriteToOutput;
            this._withDependentComponents = withDependentComponents;

            this._namespaceGlobalOptionSetsJavaScript = namespaceGlobalOptionSetsJavaScript;

            this._descriptor = descriptor;
            this._dependencyRepository = new DependencyRepository(this._service);
            this._descriptorHandler = new DependencyDescriptionHandler(this._descriptor);

            this._repositoryStringMap = new StringMapRepository(_service);
        }

        public Task CreateFileAsync(IEnumerable<OptionSetMetadata> optionSets)
        {
            return Task.Run(() => this.CreateFile(optionSets));
        }

        private async Task CreateFile(IEnumerable<OptionSetMetadata> optionSets)
        {
            optionSets = optionSets
               .Where(e => e.Options.Any(o => o.Value.HasValue))
               .OrderBy(e => e.Name);

            if (optionSets.Count() == 1)
            {
                WriteCrmDeveloperAttribute(optionSets.First().Name);
            }

            WriteNamespace();

            string tempNamespace = !string.IsNullOrEmpty(this._namespaceGlobalOptionSetsJavaScript) ? this._namespaceGlobalOptionSetsJavaScript + "." : string.Empty;

            await WriteRegularOptionSets(tempNamespace, optionSets);
        }

        public void WriteCrmDeveloperAttribute(string optionSetName)
        {
            StringBuilder systemInfo = new StringBuilder();

            if (!string.IsNullOrEmpty(optionSetName))
            {
                systemInfo.AppendFormat(@" globaloptionsetname=""{0}""", optionSetName);
            }

            if (systemInfo.Length > 0)
            {
                WriteLine($@"/// <crmdeveloperhelper{systemInfo.ToString()} />");
                WriteLine();
            }
        }

        private void WriteNamespace()
        {
            if (string.IsNullOrEmpty(this._namespaceGlobalOptionSetsJavaScript))
            {
                return;
            }

            string[] split = this._namespaceGlobalOptionSetsJavaScript.Split('.');

            StringBuilder str = new StringBuilder();

            foreach (var item in split)
            {
                if (str.Length > 0)
                {
                    str.Append(".");
                    WriteLine();
                    WriteLine();
                }

                str.Append(item);

                WriteLine("if (typeof (" + str.ToString() + ") == 'undefined') {");
                WriteLine(str.ToString() + " = { __namespace: true };");
                Write("}");
            }
        }

        private async Task WriteRegularOptionSets(string tempNamespace, IEnumerable<OptionSetMetadata> optionSets)
        {
            foreach (var optionSetMetadata in optionSets.OrderBy(o => o.Name))
            {
                await GenerateOptionSetEnums(tempNamespace, optionSetMetadata);
            }
        }

        private async Task GenerateOptionSetEnums(string tempNamespace, OptionSetMetadata optionSet)
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

            string objectName = string.Format("{0}{1}Enum", tempNamespace, optionSet.Name);

            if (CreateFileHandler.IgnoreGlobalOptionSet(optionSet.Name))
            {
                WriteLine("// {0}", objectName);
                return;
            }

            WriteLine(string.Format("{0} =", objectName) + " {");

            WriteLine();

            bool first = true;

            // Формируем значения
            foreach (var item in options)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Write(",");
                    WriteLine();
                }

                Write(item.MakeStringJS());
            }

            WriteLine();
            Write("};");
        }
    }
}
