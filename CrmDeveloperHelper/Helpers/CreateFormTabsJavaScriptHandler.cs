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

        private readonly IOrganizationServiceExtented _service;
        private readonly CreateFileJavaScriptConfiguration _config;
        private readonly JavaScriptObjectType _javaScriptObjectType;

        public CreateFormTabsJavaScriptHandler(
            TextWriter writer
            , CreateFileJavaScriptConfiguration config
            , JavaScriptObjectType javaScriptObjectType
            , IOrganizationServiceExtented service
        ) : base(writer, config.TabSpacer, true)
        {
            this._config = config;
            this._service = service;
            this._javaScriptObjectType = javaScriptObjectType;
        }

        public Task WriteContentAsync(EntityMetadata entityMetadata, string objectName, string constructorName, IEnumerable<FormTab> tabs)
        {
            return Task.Run(() => WriteContent(entityMetadata, objectName, constructorName, tabs));
        }

        private void WriteContent(EntityMetadata entityMetadata, string objectName, string constructorName, IEnumerable<FormTab> tabs)
        {
            this._entityMetadata = entityMetadata;

            NewMethod(objectName, constructorName, tabs);
        }

        public Task WriteContentAsync(string entityLogicalName, string objectName, string constructorName, IEnumerable<FormTab> tabs)
        {
            return Task.Run(() => WriteContent(entityLogicalName, objectName, constructorName, tabs));
        }

        private void WriteContent(string entityLogicalName, string objectName, string constructorName, IEnumerable<FormTab> tabs)
        {
            var repository = new EntityMetadataRepository(_service);
            this._entityMetadata = repository.GetEntityMetadata(entityLogicalName);

            NewMethod(objectName, constructorName, tabs);
        }

        private void NewMethod(string objectName, string constructorName, IEnumerable<FormTab> tabs)
        {
            WriteNamespace();

            WriteLine();

            string tempNamespace = !string.IsNullOrEmpty(this._config.NamespaceClassesJavaScript) ? this._config.NamespaceClassesJavaScript + "." : string.Empty;

            string objectDeclaration = !string.IsNullOrEmpty(tempNamespace) ? tempNamespace + objectName : "var " + objectName;

            WriteObjectStart(objectDeclaration, constructorName);

            WriteTabs(tabs);

            WriteSections(tabs);

            WriteSubgrids(tabs);

            WriteWebResources(tabs);

            WriteQuickViewForms(tabs);

            WriteIFrames(tabs);

            WriteConstantsAndFunctions(objectName);

            WriteObjectEnd(objectDeclaration, constructorName);
        }

        public void WriteContentOnlyForm(IEnumerable<FormTab> tabs)
        {
            WriteTabs(tabs);

            WriteSections(tabs);

            WriteSubgrids(tabs);

            WriteWebResources(tabs);

            WriteQuickViewForms(tabs);

            WriteIFrames(tabs);
        }

        private void WriteConstantsAndFunctions(string objectName)
        {
            WriteLine();
            WriteElementNameStart("FormTypeEnum", "{");
            WriteLine(JavaScriptBodyFormTypeEnum);
            WriteElementNameEnd();

            WriteLine();
            WriteElementNameStart("RequiredLevelEnum", "{");
            WriteLine(JavaScriptBodyRequiredLevelEnum);
            WriteElementNameEnd();

            WriteLine();
            WriteElementNameStart("SubmitModeEnum", "{");
            WriteLine(JavaScriptBodySubmitModeEnum);
            WriteElementNameEnd();

            WriteLine();
            WriteElementNameStart("writeToConsoleInfo", "function (message) {");
            WriteLine(JavaScriptFunctionsWriteToConsoleInfo);
            WriteElementNameEnd();

            WriteLine();
            WriteElementNameStart("writeToConsoleError", "function (message) {");
            WriteLine(JavaScriptFunctionsWriteToConsoleError);
            WriteElementNameEnd();

            string functionUse = GetFunctionAddress(objectName, "writeToConsoleError");

            WriteLine();
            WriteElementNameStart("handleError", "function (e) {");
            WriteLine(JavaScriptFunctionsHandleError, functionUse);
            WriteElementNameEnd();
        }

        private string GetFunctionAddress(string objectName, string functionName)
        {
            switch (this._javaScriptObjectType)
            {
                case JavaScriptObjectType.JsonObject:
                    return string.Format("{0}.{1}", objectName, functionName);

                case JavaScriptObjectType.AnonymousConstructor:
                case JavaScriptObjectType.TypeConstructor:
                default:
                    return functionName;
            }
        }

        private void WriteObjectStart(string objectDeclaration, string constructorName)
        {
            switch (this._javaScriptObjectType)
            {
                case JavaScriptObjectType.TypeConstructor:
                    {
                        WriteLine(string.Format("var {0} = function () {{", constructorName));

                        WriteLine();
                        WriteLine("var pthis = this;");

                        WriteLine();
                        WriteLine(string.Format("pthis.__class_name = '{0}';", constructorName));
                        WriteLine(string.Format("pthis.constructor = {0};", constructorName));
                    }
                    break;

                case JavaScriptObjectType.JsonObject:
                    {
                        WriteLine(string.Format("{0} = {{", objectDeclaration));
                    }
                    break;

                case JavaScriptObjectType.AnonymousConstructor:
                default:
                    {
                        WriteLine(string.Format("{0} = (new function () {{", objectDeclaration));
                    }
                    break;
            }
        }

        private void WriteObjectEnd(string objectDeclaration, string constructorName)
        {
            switch (this._javaScriptObjectType)
            {
                case JavaScriptObjectType.TypeConstructor:
                    {
                        WriteLine("};");
                        WriteLine();
                        WriteLine("{0} = new {1}();", objectDeclaration, constructorName);
                    }
                    break;
                case JavaScriptObjectType.JsonObject:
                    {
                        Write("};");
                    }
                    break;

                case JavaScriptObjectType.AnonymousConstructor:
                default:
                    {
                        WriteLine();
                        WriteLine();
                        WriteLine("var pthis = this;");

                        WriteLine();
                        WriteLine("return pthis;");

                        Write("}());");
                    }
                    break;
            }
        }

        private void WriteWebResources(IEnumerable<FormTab> tabs)
        {
            var webResouces = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType() == FormControl.FormControlType.WebResource);

            if (!webResouces.Any())
            {
                return;
            }

            WriteLine();
            WriteElementNameStart("WebResources", "{");

            foreach (var control in webResouces)
            {
                WriteLine("'{0}': '{0}',", control.Name);
            }

            WriteElementNameEnd();
        }

        private void WriteQuickViewForms(IEnumerable<FormTab> tabs)
        {
            var webResouces = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType() == FormControl.FormControlType.QuickViewForm);

            if (!webResouces.Any())
            {
                return;
            }

            WriteLine();
            WriteElementNameStart("QuickViewForms", "{");

            foreach (var control in webResouces)
            {
                WriteLine("'{0}': '{0}',", control.Name);
            }

            WriteElementNameEnd();
        }

        private void WriteIFrames(IEnumerable<FormTab> tabs)
        {
            var webResouces = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType() == FormControl.FormControlType.IFrame);

            if (!webResouces.Any())
            {
                return;
            }

            WriteLine();
            WriteElementNameStart("IFrames", "{");

            foreach (var control in webResouces)
            {
                WriteLine("'{0}': '{0}',", control.Name);
            }

            WriteElementNameEnd();
        }

        private void WriteElementNameStart(string elementName, string elementExpression)
        {
            switch (this._javaScriptObjectType)
            {
                case JavaScriptObjectType.JsonObject:
                    {
                        WriteLine("'{0}': {1}", elementName, elementExpression);
                    }
                    break;

                case JavaScriptObjectType.AnonymousConstructor:
                case JavaScriptObjectType.TypeConstructor:
                default:
                    {
                        WriteLine("var {0} = {1}", elementName, elementExpression);
                    }
                    break;
            }
        }

        private void WriteElementNameEnd()
        {
            switch (this._javaScriptObjectType)
            {
                case JavaScriptObjectType.JsonObject:
                    {
                        WriteLine("},");
                    }
                    break;

                case JavaScriptObjectType.AnonymousConstructor:
                case JavaScriptObjectType.TypeConstructor:
                default:
                    {
                        WriteLine("};");
                    }
                    break;
            }
        }

        private void WriteSubgrids(IEnumerable<FormTab> tabs)
        {
            var subgrids = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType() == FormControl.FormControlType.SubGrid || c.GetControlType() == FormControl.FormControlType.EditableSubGrid);

            if (!subgrids.Any())
            {
                return;
            }

            WriteLine();
            WriteElementNameStart("SubGrids", "{");

            foreach (var control in subgrids)
            {
                WriteLine("'{0}': '{0}',", control.Name);
            }

            WriteElementNameEnd();
        }

        private void WriteTabs(IEnumerable<FormTab> tabs)
        {
            if (!tabs.Any())
            {
                return;
            }

            WriteLine();
            WriteElementNameStart("Tabs", "{");

            foreach (var tab in tabs)
            {
                WriteLine("'{0}': {{", tab.Name);

                WriteLine("'Name': '{0}',", tab.Name);

                WriteLine("'DefaultShowLabel': {0},", tab.ShowLabel);

                WriteLine("'DefaultExpanded': {0},", tab.Expanded);

                WriteLine("'DefaultVisible': {0},", tab.Visible);

                foreach (var label in tab.Labels)
                {
                    WriteLine("'Label{0}': '{1}',", label.LanguageCode, label.GetValueJavaScript());
                }

                if (tab.Sections.Any())
                {
                    WriteLine("'Sections': [");

                    foreach (var section in tab.Sections)
                    {
                        WriteLine("'{0}',", section.Name);
                    }

                    WriteLine("],");
                }

                WriteLine("},");
            }

            WriteElementNameEnd();
        }

        private void WriteSections(IEnumerable<FormTab> tabs)
        {
            var sections = tabs.SelectMany(t => t.Sections);

            if (!sections.Any())
            {
                return;
            }

            WriteLine();
            WriteElementNameStart("Sections", "{");

            foreach (var tab in tabs)
            {
                foreach (var section in tab.Sections)
                {
                    WriteLine("'{0}': {{", section.Name);

                    WriteLine("'Tab': '{0}',", tab.Name);

                    WriteLine("'Name': '{0}',", section.Name);

                    WriteLine("'DefaultShowLabel': {0},", section.ShowLabel);

                    WriteLine("'DefaultVisible': {0},", section.Visible);

                    foreach (var label in section.Labels)
                    {
                        WriteLine("'Label{0}': '{1}',", label.LanguageCode, label.GetValueJavaScript());
                    }

                    WriteLine("},");
                }
            }

            WriteElementNameEnd();
        }

        private void WriteNamespace()
        {
            if (string.IsNullOrEmpty(this._config.NamespaceClassesJavaScript))
            {
                return;
            }

            string[] split = this._config.NamespaceClassesJavaScript.Split('.');

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

        public const string JavaScriptTry =
@"try {";

        public const string JavaScriptCatch =
@"} catch (e) {
handleError(e);

throw e;
}";

        public const string JavaScriptBodyFormTypeEnum =
@"'Undefined': 0,
'Create': 1,
'Update': 2,
'ReadOnly': 3,
'Disabled': 4,
'Bulk Edit': 6,";

        public const string JavaScriptBodyRequiredLevelEnum =
@"'none': 'none',
'required': 'required',
'recommended': 'recommended',";

        public const string JavaScriptBodySubmitModeEnum =
@"'always': 'always',
'never': 'never',
'dirty': 'dirty'";

        public const string JavaScriptFunctionsHandleError =
@"if (!e.HandledByConsole) {{

if (typeof window != 'undefined' && typeof window.console != 'undefined' && typeof window.console.error == 'function') {{

if (typeof e.name == 'string') {{ {0}(e.name); }}
if (typeof e.fileName == 'string') {{ {0}(e.fileName); }}
if (typeof e.lineNumber != 'undefined') {{ {0}(e.lineNumber); }}
if (typeof e.message == 'string') {{ {0}(e.message); }}
if (typeof e.description == 'string') {{ {0}(e.description); }}
if (typeof e.stack == 'string') {{ {0}(e.stack); }}

e.HandledByConsole = true;

debugger;

if (typeof e.message == 'string' && e.message != '') {{
var message = e.message;
}}

if (typeof e.description == 'string' && e.description != '') {{
var message = e.description;
}}

if (typeof message != 'undefined') {{
Xrm.Utility.alertDialog('Возникла ошибка: ' + message);
}}
}}
}}";

        public const string JavaScriptFunctionsWriteToConsoleInfo =
@"if (typeof window != 'undefined' && typeof window.console != 'undefined' && typeof window.console.info == 'function') {
window.console.info(message);
}";

        public const string JavaScriptFunctionsWriteToConsoleError =
@"if (typeof window != 'undefined' && typeof window.console != 'undefined' && typeof window.console.error == 'function') {
window.console.error(message);
}";
    }
}
