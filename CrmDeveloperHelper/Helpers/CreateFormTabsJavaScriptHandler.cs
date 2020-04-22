using Microsoft.Xrm.Sdk.Metadata;
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

        private bool _isFirstElement = false;

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

        public Task WriteContentAsync(EntityMetadata entityMetadata, string objectName, string constructorName, IEnumerable<FormTab> tabs, Guid? formId, string formName, int? formType, string formTypeName)
        {
            return Task.Run(() => WriteContent(entityMetadata, objectName, constructorName, tabs, formId, formName, formType, formTypeName));
        }

        private void WriteContent(EntityMetadata entityMetadata, string objectName, string constructorName, IEnumerable<FormTab> tabs, Guid? formId, string formName, int? formType, string formTypeName)
        {
            this._entityMetadata = entityMetadata;

            WriteContentInternal(entityMetadata.LogicalName, objectName, constructorName, tabs, formId, formName, formType, formTypeName);
        }

        public Task WriteContentAsync(string entityLogicalName, string objectName, string constructorName, IEnumerable<FormTab> tabs, Guid? formId, string formName, int? formType, string formTypeName)
        {
            return Task.Run(() => WriteContent(entityLogicalName, objectName, constructorName, tabs, formId, formName, formType, formTypeName));
        }

        private void WriteContent(string entityLogicalName, string objectName, string constructorName, IEnumerable<FormTab> tabs, Guid? formId, string formName, int? formType, string formTypeName)
        {
            var repository = new EntityMetadataRepository(_service);
            this._entityMetadata = repository.GetEntityMetadata(entityLogicalName);

            WriteContentInternal(entityLogicalName, objectName, constructorName, tabs, formId, formName, formType, formTypeName);
        }

        private void WriteContentInternal(
            string entityLogicalName
            , string objectName
            , string constructorName
            , IEnumerable<FormTab> tabs
            , Guid? formId
            , string formName
            , int? formType
            , string formTypeName
        )
        {
            WriteFormProperties(entityLogicalName, formId, formName, formType, formTypeName);

            WriteNamespace();

            WriteLine();

            string tempNamespace = !string.IsNullOrEmpty(this._config.NamespaceClassesJavaScript) ? this._config.NamespaceClassesJavaScript + "." : string.Empty;

            string objectDeclaration = !string.IsNullOrEmpty(tempNamespace) ? tempNamespace + objectName : "var " + objectName;

            WriteObjectStart(objectDeclaration, constructorName);

            _isFirstElement = true;

            WriteConstantsAndFunctions(objectName);

            WriteTabs(tabs);

            WriteSections(tabs);

            WriteSubgrids(tabs);

            WriteWebResources(tabs);

            WriteQuickViewForms(tabs);

            WriteIFrames(tabs);

            WriteObjectEnd(objectDeclaration, constructorName);
        }

        public void WriteContentOnlyForm(IEnumerable<FormTab> tabs)
        {
            _isFirstElement = true;

            WriteTabs(tabs);

            WriteSections(tabs);

            WriteSubgrids(tabs);

            WriteWebResources(tabs);

            WriteQuickViewForms(tabs);

            WriteIFrames(tabs);

            if (this._javaScriptObjectType == JavaScriptObjectType.JsonObject)
            {
                Write(",");
            }
        }

        private void WriteConstantsAndFunctions(string objectName)
        {
            WriteElementNameStart("FormTypeEnum", "{");
            Write(JavaScriptBodyFormTypeEnum);
            WriteElementNameEnd();

            WriteElementNameStart("RequiredLevelEnum", "{");
            Write(JavaScriptBodyRequiredLevelEnum);
            WriteElementNameEnd();

            WriteElementNameStart("SubmitModeEnum", "{");
            Write(JavaScriptBodySubmitModeEnum);
            WriteElementNameEnd();

            WriteElementNameStart("writeToConsoleInfo", "function (message) {");
            Write(JavaScriptFunctionsWriteToConsoleInfo);
            WriteElementNameEnd();

            WriteElementNameStart("writeToConsoleError", "function (message) {");
            Write(JavaScriptFunctionsWriteToConsoleError);
            WriteElementNameEnd();

            string functionUse = GetFunctionAddress(objectName, "writeToConsoleError");

            WriteElementNameStart("handleError", "function (e) {");
            Write(string.Format(JavaScriptFunctionsHandleError, functionUse));
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
                default:
                case JavaScriptObjectType.TypeConstructor:
                    {
                        WriteLine(string.Format("var {0} = function () {{", constructorName));

                        WriteLine();
                        WriteLine("var pthis = this;");

                        WriteLine();
                        WriteLine(string.Format("pthis.__class_name = '{0}';", constructorName));
                        Write(string.Format("pthis.constructor = {0};", constructorName));
                    }
                    break;

                case JavaScriptObjectType.JsonObject:
                    {
                        Write(string.Format("{0} = {{", objectDeclaration));
                    }
                    break;

                case JavaScriptObjectType.AnonymousConstructor:
                    {
                        WriteLine(string.Format("{0} = (new function () {{", objectDeclaration));

                        WriteLine();
                        Write("var pthis = this;");
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
                        WriteLine();
                        WriteLine("};");
                        WriteLine();
                        Write("{0} = new {1}();", objectDeclaration, constructorName);
                    }
                    break;
                case JavaScriptObjectType.JsonObject:
                    {
                        WriteLine();
                        Write("};");
                    }
                    break;

                case JavaScriptObjectType.AnonymousConstructor:
                default:
                    {
                        WriteLine();
                        WriteLine();
                        WriteLine("return pthis;");

                        Write("}());");
                    }
                    break;
            }
        }

        private void WriteWebResources(IEnumerable<FormTab> tabs)
        {
            var webResoucesEnum = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType() == FormControl.FormControlType.WebResource);

            if (!webResoucesEnum.Any())
            {
                return;
            }

            WriteElementNameStart("WebResources", "{");

            string prefix = string.Empty;

            foreach (var control in webResoucesEnum)
            {
                WriteLine();
                Write($"{prefix}'{control.Name}': '{control.Name}'");

                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = ", ";
                }
            }

            WriteLine();
            WriteElementNameEnd();
        }

        private void WriteQuickViewForms(IEnumerable<FormTab> tabs)
        {
            var quickFormsEnum = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType() == FormControl.FormControlType.QuickViewForm);

            if (!quickFormsEnum.Any())
            {
                return;
            }

            WriteElementNameStart("QuickViewForms", "{");

            string prefix = string.Empty;

            foreach (var control in quickFormsEnum)
            {
                WriteLine();
                Write($"{prefix}'{control.Name}': '{control.Name}'");

                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = ", ";
                }
            }

            WriteLine();
            WriteElementNameEnd();
        }

        private void WriteIFrames(IEnumerable<FormTab> tabs)
        {
            var iframesEnum = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType() == FormControl.FormControlType.IFrame);

            if (!iframesEnum.Any())
            {
                return;
            }

            WriteElementNameStart("IFrames", "{");

            string prefix = string.Empty;

            foreach (var control in iframesEnum)
            {
                WriteLine();
                Write($"{prefix}'{control.Name}': '{control.Name}'");

                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = ", ";
                }
            }

            WriteElementNameEnd();
        }

        private void WriteElementNameStart(string elementName, string elementExpression)
        {
            if (_isFirstElement)
            {
                _isFirstElement = false;
            }
            else
            {
                if (this._javaScriptObjectType == JavaScriptObjectType.JsonObject)
                {
                    Write(",");
                }
            }

            WriteLine();
            WriteLine();

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
            WriteLine();

            switch (this._javaScriptObjectType)
            {
                case JavaScriptObjectType.JsonObject:
                    {
                        Write("}");
                    }
                    break;

                case JavaScriptObjectType.AnonymousConstructor:
                case JavaScriptObjectType.TypeConstructor:
                default:
                    {
                        Write("};");
                    }
                    break;
            }
        }

        private void WriteSubgrids(IEnumerable<FormTab> tabs)
        {
            var subgridsEnum = tabs.SelectMany(t => t.Sections).SelectMany(s => s.Controls).Where(c => c.GetControlType() == FormControl.FormControlType.SubGrid || c.GetControlType() == FormControl.FormControlType.EditableSubGrid);

            if (!subgridsEnum.Any())
            {
                return;
            }

            WriteElementNameStart("SubGrids", "{");

            string prefix = string.Empty;

            foreach (var control in subgridsEnum)
            {
                WriteLine();
                Write($"{prefix}'{control.Name}': '{control.Name}'");

                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = ", ";
                }
            }

            WriteElementNameEnd();
        }

        private void WriteTabs(IEnumerable<FormTab> tabs)
        {
            if (!tabs.Any())
            {
                return;
            }

            WriteElementNameStart("Tabs", "{");

            bool first = true;

            foreach (var tab in tabs)
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

                WriteLine();
                WriteLine("'{0}': {{", tab.Name);

                WriteLine();
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
                    Write("'Sections': [");

                    string prefix = string.Empty;

                    foreach (var section in tab.Sections)
                    {
                        WriteLine();
                        Write($"{_tabSpacer}{prefix}'{section.Name}'");

                        if (string.IsNullOrEmpty(prefix))
                        {
                            prefix = ", ";
                        }
                    }

                    WriteLine();
                    WriteLine("]");
                }

                Write("}");
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

            WriteElementNameStart("Sections", "{");

            bool first = true;

            foreach (var tab in tabs)
            {
                foreach (var section in tab.Sections)
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

                    WriteLine();
                    WriteLine("'{0}': {{", section.Name);

                    WriteLine();
                    WriteLine("'Name': '{0}',", section.Name);

                    WriteLine("'TabName': '{0}',", tab.Name);

                    WriteLine("'DefaultShowLabel': {0},", section.ShowLabel);

                    Write($"'DefaultVisible': {section.Visible}");

                    foreach (var label in section.Labels)
                    {
                        Write(",");
                        WriteLine();

                        Write($"'Label{label.LanguageCode}': '{label.GetValueJavaScript()}'");
                    }

                    WriteLine();
                    Write("}");
                }
            }

            WriteElementNameEnd();
        }

        public void WriteFormProperties(string entityName, Guid? formId, string formName, int? formType, string formTypeName)
        {
            StringBuilder systemInfo = new StringBuilder();

            if (!string.IsNullOrEmpty(entityName))
            {
                systemInfo.AppendFormat(@" entityname=""{0}""", entityName);
            }

            if (formType.HasValue)
            {
                systemInfo.AppendFormat(@" systemformtype=""{0}""", formType.Value);
            }

            if (formId.HasValue)
            {
                systemInfo.AppendFormat(@" systemformid=""{0:B}""", formId.Value);
            }

            if (systemInfo.Length > 0)
            {
                WriteLine($@"/// <crmdeveloperhelper{systemInfo.ToString()} />");
            }

            if (!string.IsNullOrEmpty(formTypeName) || !string.IsNullOrEmpty(formName))
            {
                WriteLine($@"/// <crmdeveloperhelper systemformtypename=""{formTypeName}"" systemformname=""{formName}"" />");
            }
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
'Bulk Edit': 6";

        public const string JavaScriptBodyRequiredLevelEnum =
@"'none': 'none',
'required': 'required',
'recommended': 'recommended'";

        public const string JavaScriptBodySubmitModeEnum =
@"'always': 'always',
'never': 'never',
'dirty': 'dirty'";

        public const string JavaScriptFunctionsHandleError =
@"if (e && !e.HandledByConsole) {{

if (typeof window != 'undefined' && typeof window.console != 'undefined' && typeof window.console.error == 'function') {{

{0}(e);

e.HandledByConsole = true;

debugger;

if (typeof e == 'string' && e != '') {{
var message = e;
}}

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
