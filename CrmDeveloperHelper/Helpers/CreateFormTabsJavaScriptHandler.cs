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

        public Task WriteContentAsync(string entityLogicalName, SystemForm.Schema.OptionSets.type? formType, List<FormTab> tabs)
        {
            return Task.Run(() => WriteContent(entityLogicalName, formType, tabs));
        }

        private void WriteContent(string entityLogicalName, SystemForm.Schema.OptionSets.type? formType, List<FormTab> tabs)
        {
            var repository = new EntityMetadataRepository(_service);

            this._entityMetadata = repository.GetEntityMetadata(entityLogicalName);

            WriteNamespace();

            WriteLine();

            string tempNamespace = !string.IsNullOrEmpty(this._service.ConnectionData.NamespaceClassesJavaScript) ? this._service.ConnectionData.NamespaceClassesJavaScript + "." : string.Empty;

            GetTypeName(formType, out var formTypeName, out var formTypeConstructorName);

            string objectName = string.Format("{0}{1}_form_{2}", tempNamespace, _entityMetadata.LogicalName, formTypeName);
            string constructorName = string.Format("{0}Form{1}", _entityMetadata.LogicalName, formTypeConstructorName);

            string objectDeclaration = !string.IsNullOrEmpty(tempNamespace) ? objectName : "var " + objectName;

            WriteObjectStart(objectDeclaration, constructorName);

            WriteTabs(tabs);

            WriteSubgrids(tabs);

            WriteWebResources(tabs);

            WriteQuickViewForms(tabs);

            WriteIFrames(tabs);

            WriteConstantsAndFunctions(objectName);

            WriteObjectEnd(objectDeclaration, constructorName);
        }

        public static void GetTypeName(SystemForm.Schema.OptionSets.type? formType, out string formTypeName, out string formTypeConstructorName)
        {
            formTypeName = "unknown";
            formTypeConstructorName = "Unknown";

            if (!formType.HasValue)
            {
                return;
            }

            switch (formType.Value)
            {
                case SystemForm.Schema.OptionSets.type.Dashboard_0:
                    formTypeName = "dashboard";
                    formTypeConstructorName = "Dashboard";
                    break;

                case SystemForm.Schema.OptionSets.type.AppointmentBook_1:
                    formTypeName = "appointment_book";
                    formTypeConstructorName = "AppointmentBook";
                    break;

                case SystemForm.Schema.OptionSets.type.Main_2:
                    formTypeName = "main";
                    formTypeConstructorName = "Main";
                    break;

                case SystemForm.Schema.OptionSets.type.MiniCampaignBO_3:
                    formTypeName = "mini_campaign_bo";
                    formTypeConstructorName = "MiniCampaignBO";
                    break;

                case SystemForm.Schema.OptionSets.type.Preview_4:
                    formTypeName = "preview";
                    formTypeConstructorName = "Preview";
                    break;

                case SystemForm.Schema.OptionSets.type.Mobile_Express_5:
                    formTypeName = "mobile_express";
                    formTypeConstructorName = "MobileExpress";
                    break;

                case SystemForm.Schema.OptionSets.type.Quick_View_Form_6:
                    formTypeName = "quick_view";
                    formTypeConstructorName = "QuickView";
                    break;

                case SystemForm.Schema.OptionSets.type.Quick_Create_7:
                    formTypeName = "quick";
                    formTypeConstructorName = "Quick";
                    break;

                case SystemForm.Schema.OptionSets.type.Dialog_8:
                    formTypeName = "dialog";
                    formTypeConstructorName = "Dialog";
                    break;

                case SystemForm.Schema.OptionSets.type.Task_Flow_Form_9:
                    formTypeName = "task_flow";
                    formTypeConstructorName = "TaskFlow";
                    break;

                case SystemForm.Schema.OptionSets.type.InteractionCentricDashboard_10:
                    formTypeName = "interaction_centric_dashboard";
                    formTypeConstructorName = "InteractionCentricDashboard";
                    break;

                case SystemForm.Schema.OptionSets.type.Card_11:
                    formTypeName = "card";
                    formTypeConstructorName = "Card";
                    break;

                case SystemForm.Schema.OptionSets.type.Main_Interactive_experience_12:
                    formTypeName = "main_interactive_experience";
                    formTypeConstructorName = "MainInteractiveExperience";
                    break;

                case SystemForm.Schema.OptionSets.type.Other_100:
                    formTypeName = "other";
                    formTypeConstructorName = "Other";
                    break;

                case SystemForm.Schema.OptionSets.type.MainBackup_101:
                    formTypeName = "main_backup";
                    formTypeConstructorName = "MainBackup";
                    break;

                case SystemForm.Schema.OptionSets.type.AppointmentBookBackup_102:
                    formTypeName = "appointment_book_backup";
                    formTypeConstructorName = "AppointmentBookBackup";
                    break;

                case SystemForm.Schema.OptionSets.type.Power_BI_Dashboard_103:
                    formTypeName = "power_bi_dashboard";
                    formTypeConstructorName = "PowerBIDashboard";
                    break;
            }
        }

        public void WriteContentOnlyForm(List<FormTab> tabs)
        {
            WriteTabs(tabs);

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

        private void WriteWebResources(List<FormTab> tabs)
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

        private void WriteQuickViewForms(List<FormTab> tabs)
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

        private void WriteIFrames(List<FormTab> tabs)
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

        private void WriteSubgrids(List<FormTab> tabs)
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

        private void WriteTabs(List<FormTab> tabs)
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
                    WriteLine("'Sections': {");

                    foreach (var section in tab.Sections)
                    {
                        WriteLine("'{0}': {{", section.Name);

                        WriteLine("'Name': '{0}',", section.Name);

                        WriteLine("'DefaultShowLabel': {0},", section.ShowLabel);

                        WriteLine("'DefaultVisible': {0},", section.Visible);

                        foreach (var label in section.Labels)
                        {
                            WriteLine("'Label{0}': '{1}',", label.LanguageCode, label.GetValueJavaScript());
                        }

                        WriteLine("},");
                    }

                    WriteLine("},");
                }

                WriteLine("},");
            }

            WriteElementNameEnd();
        }

        private void WriteNamespace()
        {
            if (string.IsNullOrEmpty(this._service.ConnectionData.NamespaceClassesJavaScript))
            {
                return;
            }

            string[] split = this._service.ConnectionData.NamespaceClassesJavaScript.Split('.');

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
