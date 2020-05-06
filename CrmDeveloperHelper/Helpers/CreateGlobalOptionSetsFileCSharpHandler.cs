using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
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
        private readonly IOrganizationServiceExtented _service;
        private readonly CreateFileCSharpConfiguration _config;

        private readonly string _fieldHeader;

        private readonly SolutionComponentDescriptor _descriptor;
        private readonly DependencyDescriptionHandler _descriptorHandler;
        private readonly DependencyRepository _dependencyRepository;

        private readonly StringMapRepository _repositoryStringMap;

        private readonly IWriteToOutput _iWriteToOutput;

        public CreateGlobalOptionSetsFileCSharpHandler(
            TextWriter writer
            , IOrganizationServiceExtented service
            , IWriteToOutput iWriteToOutput
            , SolutionComponentDescriptor descriptor
            , CreateFileCSharpConfiguration config
        ) : base(writer, config.TabSpacer, config.AllDescriptions)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
            this._iWriteToOutput = iWriteToOutput ?? throw new ArgumentNullException(nameof(iWriteToOutput));

            this._config = config;

            this._descriptor = descriptor;
            descriptor.WithManagedInfo = config.WithManagedInfo;

            this._dependencyRepository = new DependencyRepository(this._service);
            this._descriptorHandler = new DependencyDescriptionHandler(this._descriptor);
            this._repositoryStringMap = new StringMapRepository(_service);

            if (config.ConstantType == ConstantType.ReadOnlyField)
            {
                _fieldHeader = "static readonly";
            }
            else
            {
                _fieldHeader = "const";
            }
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

            WriteLine();

            WriteLine("namespace {0}", _config.NamespaceGlobalOptionSets);
            WriteLine("{");

            await WriteRegularOptionSets(optionSets);

            Write("}");
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
            var dependentComponentsList = await _dependencyRepository.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet.MetadataId.Value);

            string entityname = null;
            string attributename = null;
            List<StringMap> listStringmap = null;

            if (dependentComponentsList.Any(e => e.DependentComponentType.Value == (int)ComponentType.Attribute))
            {
                var attr = dependentComponentsList.FirstOrDefault(e => e.DependentComponentType.Value == (int)ComponentType.Attribute);

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
            if (this._config.WithManagedInfo)
            {
                temp += string.Format("      IsManaged: {0}", optionSet.IsManaged);
            }
            headers.Add(temp);

            if (this._config.WithDependentComponents)
            {
                var desc = await _descriptorHandler.GetDescriptionDependentAsync(dependentComponentsList);

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

                if (!ignore)
                {
                    if (this._config.AddDescriptionAttribute)
                    {
                        string description = CreateFileHandler.GetLocalizedLabel(optionSet.DisplayName);

                        if (string.IsNullOrEmpty(description))
                        {
                            description = CreateFileHandler.GetLocalizedLabel(optionSet.Description);
                        }

                        if (!string.IsNullOrEmpty(description))
                        {
                            WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(description));
                        }
                    }

                    if (this._config.OptionSetExportType == OptionSetExportType.Enums && _config.AddTypeConverterAttributeForEnums && !string.IsNullOrEmpty(_config.TypeConverterName))
                    {
                        WriteLine("[System.ComponentModel.TypeConverterAttribute({0})]", ToCSharpLiteral(_config.TypeConverterName));
                    }
                }

                StringBuilder str = new StringBuilder();

                if (ignore)
                {
                    str.Append("// ");
                }

                if (this._config.OptionSetExportType == OptionSetExportType.Enums)
                {
                    str.AppendFormat("public enum {0}", optionSet.Name);
                }
                else
                {
                    str.AppendFormat("public static partial class {0}", optionSet.Name);
                }

                WriteLine(str.ToString());

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

                if (this._config.AddDescriptionAttribute)
                {
                    string description = CreateFileHandler.GetLocalizedLabel(item.Label);

                    if (string.IsNullOrEmpty(description))
                    {
                        description = CreateFileHandler.GetLocalizedLabel(item.Description);
                    }

                    if (!string.IsNullOrEmpty(description))
                    {
                        WriteLine("[System.ComponentModel.DescriptionAttribute({0})]", ToCSharpLiteral(description));
                    }
                }

                var str = item.MakeStrings();

                if (this._config.OptionSetExportType == OptionSetExportType.Enums)
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

        public static string CreateFileNameJavaScript(ConnectionData connectionData, IEnumerable<OptionSetMetadata> optionSets, bool withoutConnectionName)
        {
            string fileName = null;

            if (optionSets.Count() == 1)
            {
                fileName = string.Format("{0}.generated.js", optionSets.First().Name);
            }
            else
            {
                fileName = "globaloptionsets.js";
            }

            if (!withoutConnectionName)
            {
                fileName = string.Format("{0}.{1}", connectionData.Name, fileName);
            }

            return fileName;
        }

        public static string CreateFileNameCSharp(ConnectionData connectionData, IEnumerable<OptionSetMetadata> optionSets, bool withoutConnectionName)
        {
            string fileName = null;

            if (optionSets.Count() == 1)
            {
                fileName = string.Format("{0}.Schema.cs", optionSets.First().Name);
            }
            else
            {
                fileName = "GlobalOptionSets.cs";
            }

            if (!withoutConnectionName)
            {
                fileName = string.Format("{0}.{1}", connectionData.Name, fileName);
            }

            return fileName;
        }
    }
}
