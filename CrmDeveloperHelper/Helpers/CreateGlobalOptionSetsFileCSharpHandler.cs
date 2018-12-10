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
    public class CreateGlobalOptionSetsFileCSharpHandler : CreateFileHandler
    {
        private IOrganizationServiceExtented _service;
        private bool _withDependentComponents;

        private readonly bool _withManagedInfo;

        private string _fieldHeader;

        SolutionComponentDescriptor _descriptor;
        DependencyDescriptionHandler _descriptorHandler;
        DependencyRepository _dependencyRepository;

        private StringMapRepository _repositoryStringMap;

        private ConstantType _сonstantType;
        private OptionSetExportType _optionSetExportType;

        private readonly IWriteToOutput _iWriteToOutput;

        public CreateGlobalOptionSetsFileCSharpHandler(
            IOrganizationServiceExtented service
            , IWriteToOutput iWriteToOutput
            , string tabSpacer
            , ConstantType contantType
            , OptionSetExportType optionSetExportType
            , bool withDependentComponents
            , bool withManagedInfo
            , bool allDescriptions
            ) : base(tabSpacer, allDescriptions)
        {
            this._service = service;
            this._сonstantType = contantType;
            this._withDependentComponents = withDependentComponents;
            this._optionSetExportType = optionSetExportType;
            this._iWriteToOutput = iWriteToOutput;
            this._withManagedInfo = withManagedInfo;

            this._descriptor = new SolutionComponentDescriptor(_service, false);
            this._dependencyRepository = new DependencyRepository(this._service);
            this._descriptorHandler = new DependencyDescriptionHandler(this._descriptor);
            this._repositoryStringMap = new StringMapRepository(_service);
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

            if (this._сonstantType == Model.ConstantType.ReadOnlyField)
            {
                _fieldHeader = "static readonly";
            }
            else
            {
                _fieldHeader = "const";
            }

            StartWriting(filePath);

            WriteLine();

            WriteLine("namespace {0}", this._service.ConnectionData.NameSpaceOptionSets);
            WriteLine("{");

            await WriteRegularOptionSets(optionSets);

            Write("}");

            EndWriting();
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

            WriteLine();

            List<string> headers = new List<string>();

            string temp = string.Format("OptionSet Name: {0}      IsCustomOptionSet: {1}", optionSet.Name, optionSet.IsCustomOptionSet);
            if (this._withManagedInfo)
            {
                temp += string.Format("      IsManaged: {0}", optionSet.IsManaged);
            }
            headers.Add(temp);

            if (this._withDependentComponents)
            {
                var desc = await _descriptorHandler.GetDescriptionDependentAsync(dependent);

                var split = desc.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Any())
                {
                    headers.Add(string.Empty);

                    foreach (var item in split)
                    {
                        headers.Add(item);
                    }
                }
            }

            WriteSummary(optionSet.DisplayName, optionSet.Description, null, headers);

            var options = CreateFileHandler.GetOptionItems(entityname, attributename, optionSet, listStringmap);

            {
                bool ignore = CreateFileHandler.IgnoreGlobalOptionSet(optionSet.Name) || !options.Any();

                string str = string.Empty;

                if (ignore)
                {
                    str += "//";
                }

                if (_optionSetExportType == OptionSetExportType.Enums)
                {
                    str += string.Format("public enum {0}", optionSet.Name);
                }
                else
                {
                    str += string.Format("public static partial class {0}", optionSet.Name);
                }

                WriteLine(str);

                if (ignore)
                {
                    return;
                }
            }

            WriteLine("{");

            bool first = true;

            // Формируем значения
            foreach (var item in options)
            {
                if (first) { first = false; } else { WriteLine(); }

                List<string> header = new List<string>() { item.Value.ToString() };

                if (item.DisplayOrder.HasValue)
                {
                    header.Add(string.Format("DisplayOrder: {0}", item.DisplayOrder.Value));
                }

                WriteSummary(item.Label, item.Description, header, null);

                var str = item.MakeStrings();

                if (_optionSetExportType == OptionSetExportType.Enums)
                {
                    WriteLine("[System.Runtime.Serialization.EnumMemberAttribute()]");

                    WriteLine(str + ",");
                }
                else
                {
                    WriteLine("public {0} int {1};", _fieldHeader, str);
                }
            }

            WriteLine("}");
        }
    }
}
