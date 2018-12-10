using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
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
    public class CreateFormTabsJavaScriptHandler : CreateFileHandler
    {
        private EntityMetadata _entityMetadata;

        public IOrganizationServiceExtented _service;
        private CreateFileWithEntityMetadataJavaScriptConfiguration _config;

        public CreateFormTabsJavaScriptHandler(
            CreateFileWithEntityMetadataJavaScriptConfiguration config
            , IOrganizationServiceExtented service
            ) : base(config.TabSpacer, true)
        {
            this._config = config;
            this._service = service;
        }

        public Task<string> CreateFileAsync(string fileName, List<FormTab> tabs)
        {
            return Task.Run(() => CreateFile(fileName, tabs));
        }

        private string CreateFile(string fileName, List<FormTab> tabs)
        {
            RetrieveEntityRequest request = new RetrieveEntityRequest();
            request.LogicalName = _config.EntityName.ToLower();

            request.EntityFilters = EntityFilters.All;

            RetrieveEntityResponse response = (RetrieveEntityResponse)_service.Execute(request);
            this._entityMetadata = response.EntityMetadata;

            var fileFilePath = Path.Combine(this._config.Folder, fileName);

            StartWriting(fileFilePath);

            WriteNameSpace();

            WriteLine();

            string tempNameSpace = !string.IsNullOrEmpty(this._service.ConnectionData.NameSpaceClasses) ? this._service.ConnectionData.NameSpaceClasses + "." : string.Empty;

            WriteLine(string.Format("{0}{1} = (new function () ", tempNameSpace, _entityMetadata.LogicalName) + "{");

            WriteTabs(tabs);

            WriteSubgrids(tabs);

            WriteWebResources(tabs);

            WriteLine();
            WriteLine("var pthis = this;");

            WriteLine();
            WriteLine("return pthis;");

            Write("}());");

            EndWriting();

            return fileFilePath;
        }

        private void WriteWebResources(List<FormTab> tabs)
        {
            var webResouces = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType().Equals("WebResource", StringComparison.InvariantCultureIgnoreCase));

            if (!webResouces.Any())
            {
                return;
            }

            WriteLine("var WebResources = {");

            foreach (var control in webResouces)
            {
                WriteLine("'{0}': '{0}',", control.Name);
            }

            WriteLine("};");
        }

        private void WriteSubgrids(List<FormTab> tabs)
        {
            var subgrids = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType().Equals("SubGrid", StringComparison.InvariantCultureIgnoreCase));

            if (!subgrids.Any())
            {
                return;
            }

            WriteLine("var SubGrids = {");

            foreach (var control in subgrids)
            {
                WriteLine("'{0}': '{0}',", control.Name);
            }

            WriteLine("};");
        }

        private void WriteTabs(List<FormTab> tabs)
        {
            if (!tabs.Any())
            {
                return;
            }

            WriteLine("var Tabs = {");

            foreach (var tab in tabs)
            {
                WriteLine((string.Format("'{0}': ", tab.Name) + "{"));

                WriteLine("'Name': '{0}',", tab.Name);

                WriteLine("'DefaultShowLabel': {0},", tab.ShowLabel);

                WriteLine("'DefaultExpanded': {0},", tab.Expanded);

                WriteLine("'DefaultVisible': {0},", tab.Visible);

                foreach (var label in tab.Labels)
                {
                    WriteLine("'Label{0}': '{1}',", label.LanguageCode, label.Value);
                }

                if (tab.Sections.Any())
                {
                    WriteLine("'Sections': {");

                    foreach (var section in tab.Sections)
                    {
                        WriteLine((string.Format("'{0}': ", section.Name) + "{"));

                        WriteLine("'Name': '{0}',", section.Name);

                        WriteLine("'DefaultShowLabel': {0},", section.ShowLabel);

                        WriteLine("'DefaultVisible': {0},", section.Visible);

                        foreach (var label in section.Labels)
                        {
                            WriteLine("'Label{0}': '{1}',", label.LanguageCode, label.Value);
                        }

                        WriteLine("},");
                    }

                    WriteLine("},");
                }

                WriteLine("},");
            }

            WriteLine("};");
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
