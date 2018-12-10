using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class FormDescriptionHandler
    {
        private EntityMetadata _entityMetadata;

        private SolutionComponentDescriptor _descriptor;

        private DependencyRepository _dependencyRepository;

        private DependencyDescriptionHandler _descriptorHandler;
        private const string tabSpacer = "    ";

        public bool WithManagedInfo { get; set; } = true;

        public bool WithDependentComponents { get; set; } = true;

        public FormDescriptionHandler(SolutionComponentDescriptor descriptor, DependencyRepository dependencyRepository)
        {
            this._descriptor = descriptor;
            this._dependencyRepository = dependencyRepository;
            this._descriptorHandler = new DependencyDescriptionHandler(_descriptor);
        }

        public Task<string> GetFormDescriptionAsync(XElement doc, string entityName, Guid formId, string name, string typeName)
        {
            return Task.Run(async () => await GetFormDescription(doc, entityName, formId, name, typeName));
        }

        private async Task<string> GetFormDescription(XElement doc, string entityName, Guid formId, string name, string typeName)
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(entityName) && entityName != "none")
            {
                try
                {
                    this._entityMetadata = _descriptor.MetadataSource.GetEntityMetadata(entityName);
                }
                catch (Exception)
                {
                    this._entityMetadata = null;

#if DEBUG
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif
                }
            }

            result.AppendFormat("Entity: {0}", entityName).AppendLine();
            result.AppendFormat("Form Type: {0}", typeName).AppendLine();
            result.AppendFormat("Form Name: {0}", name).AppendLine();
            result.AppendFormat("FormId: {0}", formId).AppendLine();

            SaveInfoFormLibraries(result, doc);

            SaveInfoEvents(result, doc);

            SaveInfoTabSection(result, doc);

            SaveInfoHiddenAttributes(result, doc);

            SaveInfoControlsWithoutAttributes(result, doc);

            SaveInfoAttributes(result, doc);

            await SaveDisplayConditions(result, doc);

            if (this.WithDependentComponents)
            {
                await SaveDependentComponents(result, formId);
            }

            return result.ToString();
        }

        private async Task SaveDependentComponents(StringBuilder result, Guid formId)
        {
            var coll = await _dependencyRepository.GetDependentComponentsAsync((int)ComponentType.SystemForm, formId);

            if (coll.Count > 0)
            {
                var desc = await _descriptorHandler.GetDescriptionDependentAsync(coll);

                result.AppendLine("DependentComponents:");

                result.AppendLine(desc);

                result.AppendLine().AppendLine().AppendLine();

                var businessRules = coll.Where(e => e.DependentComponentType?.Value == (int)ComponentType.Workflow);

                if (businessRules.Any())
                {
                    IEnumerable<Workflow> entities = _descriptor.GetEntities<Workflow>((int)ComponentType.Workflow, businessRules.Select(s => s.DependentComponentObjectId));

                    entities = entities.Where(e => e.Category?.Value == (int)Workflow.Schema.OptionSets.category.Business_Rule_2);

                    if (entities.Any())
                    {
                        foreach (var item in entities)
                        {
                            var required = await _dependencyRepository.GetRequiredComponentsAsync((int)ComponentType.Workflow, item.Id);

                            var descRequired = await _descriptor.GetSolutionComponentsDescriptionAsync(required.Select(d => d.RequiredToSolutionComponent()));

                            if (!string.IsNullOrEmpty(descRequired))
                            {
                                var businesDesc = _descriptor.GetComponentDescription((int)ComponentType.Workflow, item.Id);

                                result
                                    .AppendLine()
                                    .AppendLine(businesDesc)
                                    .AppendLine("RequiredComponents:")
                                    .AppendLine(descRequired)
                                    ;
                            }
                        }
                    }
                }
            }
        }

        private void SaveInfoHiddenAttributes(StringBuilder result, XElement doc)
        {
            var hiddenControls = doc.Element("hiddencontrols");

            if (hiddenControls != null)
            {
                var allData = hiddenControls.Descendants("data");

                if (allData.Any())
                {
                    if (result.Length > 0)
                    {
                        result.AppendLine().AppendLine().AppendLine();
                    }

                    result.AppendLine("Hidden Attributes:");

                    foreach (var nodeEvent in allData)
                    {
                        var datafieldname = (string)nodeEvent.Attribute("datafieldname");

                        result.AppendLine(datafieldname);
                    }
                }
            }
        }

        private void SaveInfoEvents(StringBuilder result, XElement doc)
        {
            string eventsDescription = GetEventsDescription(doc);

            if (!string.IsNullOrEmpty(eventsDescription))
            {
                if (result.Length > 0)
                {
                    result.AppendLine().AppendLine().AppendLine();
                }

                result.AppendLine("Events:");
                result.AppendLine(eventsDescription);
            }
        }

        private void SaveInfoFormLibraries(StringBuilder result, XElement doc)
        {
            string desc = GetFormLibrariesDescription(doc);

            if (!string.IsNullOrEmpty(desc))
            {
                if (result.Length > 0)
                {
                    result.AppendLine().AppendLine().AppendLine();
                }

                result.AppendLine("FormLibraries:");
                result.AppendLine(desc);
            }
        }

        private void SaveInfoTabSection(StringBuilder result, XElement doc)
        {
            FormTab header = GetHeader(doc);

            List<FormTab> tabs = GetFormTabs(doc);

            FormTab footer = GetFooter(doc);

            if (header != null || tabs.Any() || footer != null)
            {
                List<int> locales = GetLabelLocales(header, tabs, footer);

                locales.Sort(new LocaleComparer());

                result.AppendLine().AppendLine().AppendLine();
                result.AppendLine("Tabs and Sections");

                {
                    FormatTextTableHandler table = new FormatTextTableHandler();

                    {
                        List<string> fields = new List<string>() { "Name", "ShowLabel", "Visible", "Expanded" };
                        foreach (var lang in locales)
                        {
                            fields.Add("Label " + LanguageLocale.GetLocaleName(lang));
                        }

                        table.SetHeader(fields);
                    }

                    if (header != null)
                    {
                        List<string> fields = new List<string>() { header.Name, header.ShowLabel, header.Visible, "" };
                        AddLabels(header.Labels, locales, fields);
                        table.AddLine(fields);
                    }

                    foreach (var tab in tabs)
                    {
                        {
                            List<string> fields = new List<string>() { tab.Name, tab.ShowLabel, tab.Visible, tab.Expanded };
                            AddLabels(tab.Labels, locales, fields);
                            table.AddLine(fields);
                        }

                        foreach (var section in tab.Sections)
                        {
                            List<string> fields = new List<string>() { tabSpacer + section.Name, section.ShowLabel, section.Visible, "" };
                            AddLabels(section.Labels, locales, fields);
                            table.AddLine(fields);
                        }
                    }

                    if (footer != null)
                    {
                        List<string> fields = new List<string>() { footer.Name, footer.ShowLabel, footer.Visible, "" };
                        AddLabels(footer.Labels, locales, fields);
                        table.AddLine(fields);
                    }

                    table.GetFormatedLines(false).ForEach(s => result.AppendLine(tabSpacer + s));
                }

                result
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    ;

                result.AppendLine("Tabs, Sections and Controls");

                {
                    FormatTextTableHandler table = new FormatTextTableHandler();

                    {
                        List<string> fields = new List<string>() { "Name", "ControlType", "Attribute", "ShowLabel", "Visible", "Disabled" };
                        foreach (var lang in locales)
                        {
                            fields.Add("Label " + LanguageLocale.GetLocaleName(lang));
                        }

                        fields.AddRange(new[] { "AttributeType", "Type" });

                        foreach (var lang in locales)
                        {
                            fields.Add("DisplayName " + LanguageLocale.GetLocaleName(lang));
                        }

                        table.SetHeader(fields);
                    }

                    if (header != null)
                    {
                        {
                            List<string> fields = new List<string>() { header.Name, "Header", "", header.ShowLabel, header.Visible, "" };
                            AddLabels(header.Labels, locales, fields);
                            table.AddLine(fields);
                        }

                        foreach (var section in header.Sections)
                        {
                            foreach (var control in section.Controls)
                            {
                                List<string> fields = new List<string>() { tabSpacer + tabSpacer + control.Name, control.GetControlType(), control.Attribute, control.ShowLabel, control.Visible, control.Disabled };
                                AddLabels(control.Labels, locales, fields);
                                AddAttributeLabels(control.Attribute, locales, fields);
                                table.AddLine(fields);
                            }
                        }
                    }

                    foreach (var tab in tabs)
                    {
                        {
                            List<string> fields = new List<string>() { tab.Name, "Tab", "", tab.ShowLabel, tab.Visible, "" };
                            AddLabels(tab.Labels, locales, fields);
                            table.AddLine(fields);
                        }

                        foreach (var section in tab.Sections)
                        {
                            {
                                List<string> fields = new List<string>() { tabSpacer + section.Name, "Section", "", section.ShowLabel, section.Visible, "" };
                                AddLabels(section.Labels, locales, fields);
                                table.AddLine(fields);
                            }

                            foreach (var control in section.Controls)
                            {
                                List<string> fields = new List<string>() { tabSpacer + tabSpacer + control.Name, control.GetControlType(), control.Attribute, control.ShowLabel, control.Visible, control.Disabled };
                                AddLabels(control.Labels, locales, fields);
                                AddAttributeLabels(control.Attribute, locales, fields);
                                table.AddLine(fields);
                            }
                        }
                    }

                    if (footer != null)
                    {
                        {
                            List<string> fields = new List<string>() { footer.Name, "Footer", "", footer.ShowLabel, footer.Visible, "" };
                            AddLabels(footer.Labels, locales, fields);
                            table.AddLine(fields);
                        }

                        foreach (var section in footer.Sections)
                        {
                            foreach (var control in section.Controls)
                            {
                                List<string> fields = new List<string>() { tabSpacer + tabSpacer + control.Name, control.GetControlType(), control.Attribute, control.ShowLabel, control.Visible, control.Disabled };
                                AddLabels(control.Labels, locales, fields);
                                AddAttributeLabels(control.Attribute, locales, fields);
                                table.AddLine(fields);
                            }
                        }
                    }

                    table.GetFormatedLines(false).ForEach(s => result.AppendLine(tabSpacer + s));
                }
            }
        }

        private void AddAttributeLabels(string attribute, List<int> locales, List<string> fields)
        {
            if (string.IsNullOrEmpty(attribute) || _entityMetadata == null)
            {
                return;
            }

            var attr = _entityMetadata?.Attributes?.FirstOrDefault(a => a.LogicalName.Equals(attribute, StringComparison.OrdinalIgnoreCase));

            if (attr == null)
            {
                return;
            }

            fields.Add(attr.AttributeType.ToString());
            fields.Add(attr.GetType().Name);

            if (attr.DisplayName != null && attr.DisplayName.LocalizedLabels != null)
            {
                foreach (var lang in locales)
                {
                    var label = attr.DisplayName.LocalizedLabels.FirstOrDefault(l => l.LanguageCode == lang);

                    fields.Add(label?.Label ?? string.Empty);
                }
            }
        }

        private static void AddLabels(List<LabelString> labels, List<int> locales, List<string> fields)
        {
            foreach (var lang in locales)
            {
                var label = labels.FirstOrDefault(l => l.LanguageCode == lang);

                fields.Add(label?.Value ?? string.Empty);
            }
        }

        private List<int> GetLabelLocales(FormTab header, List<FormTab> tabs, FormTab footer)
        {
            HashSet<int> result = new HashSet<int>();

            if (header != null)
            {
                FillLocales(result, header);
            }

            foreach (var tab in tabs)
            {
                FillLocales(result, tab);
            }

            if (header != null)
            {
                FillLocales(result, header);
            }

            return result.ToList();
        }

        private void FillLocales(HashSet<int> result, FormTab tab)
        {
            if (tab == null)
            {
                return;
            }

            foreach (var label in tab.Labels)
            {
                result.Add(label.LanguageCode);
            }

            foreach (var section in tab.Sections)
            {
                foreach (var label in section.Labels)
                {
                    result.Add(label.LanguageCode);
                }

                foreach (var control in section.Controls)
                {
                    foreach (var label in control.Labels)
                    {
                        result.Add(label.LanguageCode);
                    }

                    if (!string.IsNullOrEmpty(control.Attribute) && _entityMetadata != null && _entityMetadata.Attributes != null)
                    {
                        var attr = _entityMetadata?.Attributes?.FirstOrDefault(a => a.LogicalName.Equals(control.Attribute, StringComparison.OrdinalIgnoreCase));

                        if (attr != null && attr.DisplayName != null && attr.DisplayName.LocalizedLabels != null)
                        {
                            foreach (var label in attr.DisplayName.LocalizedLabels.Where(l => !string.IsNullOrEmpty(l.Label)))
                            {
                                result.Add(label.LanguageCode);
                            }
                        }
                    }
                }
            }
        }

        private FormTab GetHeader(XElement doc)
        {
            FormTab result = null;

            var nodeHeader = doc.Descendants("header").FirstOrDefault();

            if (nodeHeader != null)
            {
                result = new FormTab()
                {
                    Id = (string)nodeHeader.Attribute("id"),
                    Name = "header",
                    ShowLabel = "true",
                    Visible = (string)nodeHeader.Attribute("visible") ?? "true",
                };

                var section = new FormSection();
                result.Sections.Add(section);

                var nodeAllCells = nodeHeader.Descendants("cell");

                if (nodeAllCells != null)
                {
                    foreach (var nodeCell in nodeAllCells)
                    {
                        FormControl control = GetControlFromNode("header", null, nodeCell);

                        if (control != null)
                        {
                            section.Controls.Add(control);
                        }
                    }
                }
            }

            return result;
        }

        private FormTab GetFooter(XElement doc)
        {
            FormTab result = null;

            var nodeFooter = doc.Descendants("footer").FirstOrDefault();

            if (nodeFooter != null)
            {
                result = new FormTab()
                {
                    Id = (string)nodeFooter.Attribute("id"),
                    Name = "footer",
                    ShowLabel = "true",
                    Visible = (string)nodeFooter.Attribute("visible") ?? "true",
                };

                var section = new FormSection();
                result.Sections.Add(section);

                var nodeAllCells = nodeFooter.Descendants("cell");

                if (nodeAllCells != null)
                {
                    foreach (var nodeCell in nodeAllCells)
                    {
                        FormControl control = GetControlFromNode("footer", null, nodeCell);

                        if (control != null)
                        {
                            section.Controls.Add(control);
                        }
                    }
                }
            }

            return result;
        }

        public List<FormTab> GetFormTabs(XElement doc)
        {
            List<FormTab> result = new List<FormTab>();

            var allTabs = doc.Descendants("tab");

            foreach (var nodeTab in allTabs)
            {
                var tab = new FormTab()
                {
                    Id = (string)nodeTab.Attribute("id"),
                    Name = (string)nodeTab.Attribute("name") ?? (string)nodeTab.Attribute("id"),
                    ShowLabel = (string)nodeTab.Attribute("showlabel") ?? "true",
                    Expanded = (string)nodeTab.Attribute("expanded") ?? "true",
                    Visible = (string)nodeTab.Attribute("visible") ?? "true",
                };

                result.Add(tab);

                {
                    var nodeLabels = nodeTab.Element("labels");

                    if (nodeLabels != null)
                    {
                        foreach (var label in nodeLabels.Elements("label"))
                        {
                            string description = (string)label.Attribute("description");
                            int languageCode = (int)label.Attribute("languagecode");

                            tab.Labels.Add(new LabelString() { Value = description, LanguageCode = languageCode });
                        }
                    }
                }

                var allSec = nodeTab.Descendants("section");

                foreach (var nodeSection in allSec)
                {
                    var secName = (string)nodeSection.Attribute("name");

                    var section = new FormSection()
                    {
                        Id = (string)nodeSection.Attribute("id"),
                        Name = (string)nodeSection.Attribute("name") ?? (string)nodeSection.Attribute("id"),
                        ShowLabel = (string)nodeSection.Attribute("showlabel") ?? "true",
                        Visible = (string)nodeSection.Attribute("visible") ?? "true",
                    };

                    tab.Sections.Add(section);

                    {
                        var nodeLabels = nodeSection.Element("labels");

                        if (nodeLabels != null)
                        {
                            foreach (var label in nodeLabels.Elements("label"))
                            {
                                string description = (string)label.Attribute("description");
                                int languageCode = (int)label.Attribute("languagecode");

                                section.Labels.Add(new LabelString() { Value = description, LanguageCode = languageCode });
                            }
                        }
                    }

                    var nodeAllCells = nodeSection.Descendants("cell");

                    if (nodeAllCells != null)
                    {
                        foreach (var nodeCell in nodeAllCells)
                        {
                            FormControl control = GetControlFromNode(tab.Name, section.Name, nodeCell);

                            if (control != null)
                            {
                                section.Controls.Add(control);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private FormControl GetControlFromNode(string tabName, string sectionName, XElement nodeCell)
        {
            FormControl control = null;

            var nodeControl = nodeCell.Element("control");

            if (nodeControl != null)
            {
                control = new FormControl
                {
                    Id = (string)nodeCell.Attribute("id"),
                    Name = (string)nodeControl.Attribute("id"),
                    ShowLabel = (string)nodeCell.Attribute("showlabel") ?? "true",
                    ClassId = (string)nodeControl.Attribute("classid"),
                    Attribute = (string)nodeControl.Attribute("datafieldname"),
                    Disabled = (string)nodeControl.Attribute("disabled") ?? "false",
                    Visible = (string)nodeControl.Attribute("visible") ?? "true",
                    Location = tabName + (!string.IsNullOrEmpty(sectionName) ? string.Format(".{0}", sectionName) : string.Empty),
                    IndicationOfSubgrid = (string)nodeControl.Attribute("indicationOfSubgrid")
                };

                {
                    var nodeLabels = nodeCell.Element("labels");

                    if (nodeLabels != null)
                    {
                        foreach (var label in nodeLabels.Elements("label"))
                        {
                            string description = (string)label.Attribute("description");
                            int languageCode = (int)label.Attribute("languagecode");

                            control.Labels.Add(new LabelString() { Value = description, LanguageCode = languageCode });
                        }
                    }
                }

                {
                    var nodeParameters = nodeControl.Element("parameters");

                    if (nodeParameters != null)
                    {
                        control.Parameters = GetParametersString(nodeParameters);
                    }
                }
            }

            return control;
        }

        private async Task SaveDisplayConditions(StringBuilder result, XElement doc)
        {
            List<Guid> list = GetRoles(doc);

            if (list.Any())
            {
                if (result.Length > 0)
                {
                    result.AppendLine().AppendLine().AppendLine();
                }

                var mess = await this._descriptor.GetSolutionComponentsDescriptionAsync(list.Select(id => new SolutionComponent()
                {
                    ObjectId = id,
                    ComponentType = new OptionSetValue((int)ComponentType.Role),
                }).ToList());

                if (!string.IsNullOrEmpty(mess))
                {
                    result.AppendLine("DisplayConditions:");

                    result.AppendLine(mess);
                }
            }
        }

        private List<Guid> GetRoles(XElement doc)
        {
            List<Guid> result = new List<Guid>();

            var formDisplayConditions = doc.Element("DisplayConditions");

            if (formDisplayConditions != null)
            {
                var allRoles = formDisplayConditions.Descendants("Role");

                if (allRoles.Any())
                {
                    foreach (var nodeRole in allRoles)
                    {
                        var attrId = (string)nodeRole.Attribute("Id");

                        if (!string.IsNullOrEmpty(attrId))
                        {
                            Guid tempId = Guid.Empty;

                            if (Guid.TryParse(attrId, out tempId))
                            {
                                result.Add(tempId);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private void SaveInfoAttributes(StringBuilder result, XElement doc)
        {
            IEnumerable<FormControl> list = GetFormControls(doc);

            list = list.Where(c => !string.IsNullOrEmpty(c.Attribute));

            if (list.Any())
            {
                if (result.Length > 0)
                {
                    result.AppendLine().AppendLine().AppendLine();
                }

                result.AppendLine("Attributes on form:");

                foreach (var gr in list.GroupBy(c => c.Attribute).OrderBy(gr => gr.Key))
                {
                    result.AppendLine(tabSpacer + gr.Key);
                }

                if (result.Length > 0)
                {
                    result.AppendLine().AppendLine().AppendLine();
                }

                result
                    .AppendLine("Attributes full information:")
                    .AppendLine()
                    .AppendLine();

                foreach (var gr in list.GroupBy(c => c.Attribute).OrderBy(gr => gr.Key))
                {
                    result.AppendLine(gr.Key);

                    if (_entityMetadata != null)
                    {
                        var attribute = _entityMetadata?.Attributes?.FirstOrDefault(e => e.LogicalName == gr.Key);

                        if (attribute != null)
                        {
                            List<string> lines = new List<string>();

                            CreateFileHandler.FillLabelDisplayNameAndDescription(lines, true, attribute.DisplayName, attribute.Description, tabSpacer);

                            var attributeDescription = CreateFileHandler.GetAttributeDescription(attribute, true, this.WithManagedInfo, this._descriptor);

                            if (attributeDescription.Count > 0)
                            {
                                if (lines.Count > 0)
                                {
                                    lines.Add(string.Empty);
                                }

                                lines.AddRange(attributeDescription);
                            }

                            foreach (var item in lines)
                            {
                                result.AppendLine(tabSpacer + "// " + item);
                            }

                            result.AppendLine();
                        }
                    }

                    foreach (var control in gr)
                    {
                        AppendInformationAboutControl(result, control);
                    }

                    result
                        .AppendLine()
                        .AppendLine(new string('-', 150))
                        .AppendLine()
                        ;
                }
            }
        }

        private static void AppendInformationAboutControl(StringBuilder result, FormControl control)
        {
            result.AppendFormat(tabSpacer + "ControlName=\"{0}\",    location=\"{1}\",    visible=\"{2}\",    disabled=\"{3}\",    ControlType=\"{4}\",    ClassId=\"{5}\",    CellId=\"{6}\""
                                        , control.Name
                                        , control.Location
                                        , control.Visible
                                        , control.Disabled
                                        , control.GetControlType()
                                        , control.ClassId
                                        , control.Id
                                        ).AppendLine();

            foreach (var label in control.Labels)
            {
                result.AppendFormat(tabSpacer + tabSpacer + "Label description=\"{0}\",    languagecode=\"{1}\""
                    , label.Value
                    , LanguageLocale.GetLocaleName(label.LanguageCode)
                    ).AppendLine();
            }

            if (!string.IsNullOrEmpty(control.Parameters))
            {
                var split = control.Parameters.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Any())
                {
                    foreach (var item in split)
                    {
                        result.AppendLine(tabSpacer + tabSpacer + tabSpacer + item);
                    }
                }
            }
        }

        private List<FormControl> GetFormControls(XElement doc)
        {
            List<FormControl> list = new List<FormControl>();

            var allControls = doc.Descendants("control");

            foreach (var nodeControl in allControls)
            {
                var nodeCell = nodeControl.Ancestors("cell").FirstOrDefault();

                if (nodeCell != null)
                {
                    string tabName = string.Empty;
                    string sectionName = string.Empty;

                    {
                        var nodeSection = nodeCell.Ancestors("section").FirstOrDefault();

                        if (nodeSection != null)
                        {
                            var nodeTab = nodeSection.Ancestors("tab").FirstOrDefault();

                            if (nodeTab != null)
                            {
                                tabName = (string)nodeTab.Attribute("name") ?? (string)nodeTab.Attribute("id");
                                sectionName = (string)nodeSection.Attribute("name") ?? (string)nodeSection.Attribute("id");
                            }
                        }
                    }

                    {
                        var node = nodeCell.Ancestors("header").FirstOrDefault();

                        if (node != null)
                        {
                            tabName = "header";
                        }
                    }

                    {
                        var node = nodeCell.Ancestors("footer").FirstOrDefault();

                        if (node != null)
                        {
                            tabName = "footer";
                        }
                    }

                    var control = GetControlFromNode(tabName, sectionName, nodeCell);

                    if (control != null)
                    {
                        list.Add(control);
                    }
                }
            }

            return list;
        }

        private void SaveInfoControlsWithoutAttributes(StringBuilder result, XElement doc)
        {
            IEnumerable<FormControl> list = GetFormControls(doc);

            list = list.Where(c => string.IsNullOrEmpty(c.Attribute));

            if (list.Any())
            {
                if (result.Length > 0)
                {
                    result.AppendLine().AppendLine().AppendLine();
                }

                result
                    .AppendLine("Controls without attributes:")
                    .AppendLine()
                    .AppendLine();

                foreach (var control in list)
                {
                    AppendInformationAboutControl(result, control);

                    result
                        .AppendLine()
                        .AppendLine(new string('-', 150))
                        .AppendLine()
                        ;
                }
            }
        }

        private string GetParametersString(XElement nodeParameters)
        {
            StringBuilder result = new StringBuilder();

            result.Append("Parameters");

            foreach (var child in nodeParameters.Elements())
            {
                if (string.Equals(child.Name.ToString(), "DefaultViewId", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(child.Name.ToString(), "AvailableViewIds", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(child.Name.ToString(), "ViewId", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(child.Name.ToString(), "ViewIds", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    StringBuilder str = new StringBuilder();

                    var split = child.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var id in split)
                    {
                        if (Guid.TryParse(id, out Guid idView))
                        {
                            if (str.Length > 0)
                            {
                                str.Append(", ");
                            }

                            var entity = this._descriptor.GetEntity<SavedQuery>((int)ComponentType.SavedQuery, idView);
                            if (entity != null)
                            {
                                string queryName = entity.Name;
                                string entityName = entity.ReturnedTypeCode;

                                str.AppendFormat("{0} '{1}' '{2}'", id, entityName, queryName);
                            }
                            else
                            {
                                str.Append(id);
                            }
                        }
                    }

                    result.AppendLine().AppendFormat("  {0} = {1}", child.Name, str.ToString());
                }
                else if (string.Equals(child.Name.ToString(), "VisualizationId", StringComparison.OrdinalIgnoreCase))
                {
                    StringBuilder str = new StringBuilder();

                    var split = child.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var id in split)
                    {
                        if (Guid.TryParse(id, out Guid idView))
                        {
                            if (str.Length > 0)
                            {
                                str.Append(", ");
                            }

                            var entity = this._descriptor.GetEntity<SavedQueryVisualization>((int)ComponentType.SavedQueryVisualization, idView);
                            if (entity != null)
                            {
                                string chartName = entity.Name;
                                string entityName = entity.PrimaryEntityTypeCode;

                                str.AppendFormat("{0} '{1}' '{2}'", id, entityName, chartName);
                            }
                            else
                            {
                                str.Append(id);
                            }
                        }
                    }

                    result.AppendLine().AppendFormat("  {0} = {1}", child.Name, str.ToString());
                }
                else if (string.Equals(child.Name.ToString(), "WebResourceId", StringComparison.OrdinalIgnoreCase))
                {
                    StringBuilder str = new StringBuilder();

                    var split = child.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var id in split)
                    {
                        if (Guid.TryParse(id, out Guid idView))
                        {
                            if (str.Length > 0)
                            {
                                str.Append(", ");
                            }

                            var entity = this._descriptor.GetEntity<WebResource>((int)ComponentType.WebResource, idView);
                            if (entity != null)
                            {
                                string name = entity.Name;

                                string webTypeName = entity.FormattedValues[WebResource.Schema.Attributes.webresourcetype];

                                str.AppendFormat("{0} '{1}' '{2}'", id, name, webTypeName);
                            }
                            else
                            {
                                str.Append(id);
                            }
                        }
                    }

                    result.AppendLine().AppendFormat("  {0} = {1}", child.Name, str.ToString());
                }
                else
                {
                    result.AppendLine().AppendFormat("  {0} = {1}", child.Name, child.Value);
                }
            }

            return result.ToString();
        }

        public string GetFormLibrariesDescription(XElement doc)
        {
            StringBuilder result = new StringBuilder();

            var formLibraries = doc.Element("formLibraries");

            if (formLibraries != null)
            {
                var allLibraries = formLibraries.Descendants("Library");

                if (allLibraries.Any())
                {
                    var list = new List<string>();

                    foreach (var nodeLibrary in allLibraries)
                    {
                        var name = (string)nodeLibrary.Attribute("name");

                        list.Add(name);
                    }

                    foreach (var item in list.OrderBy(s => s))
                    {
                        result.AppendFormat("    {0}", item).AppendLine();
                    }
                }
            }

            return result.ToString();
        }

        private class FormEvent
        {
            public string Attribute { get; set; }
            public string Name { get; set; }
            public string Application { get; set; }
            public string Active { get; set; }
            public string Path { get; set; }

            public List<FormEventHandler> Handlers { get; private set; }

            public FormEvent()
            {
                this.Handlers = new List<FormEventHandler>();
            }
        }

        private class FormEventHandler
        {
            public string LibraryName { get; set; }
            public string FunctionName { get; set; }
            public string Enabled { get; set; }
            public string PassExecutionContext { get; set; }
            public string Parameters { get; set; }
        }

        public string GetEventsDescription(XElement doc)
        {
            StringBuilder result = new StringBuilder();

            List<FormEvent> listEvents = new List<FormEvent>();

            var allEventsColl = doc.Descendants("events");

            FormatTextTableHandler formatEvent = new FormatTextTableHandler();

            foreach (var events in allEventsColl)
            {
                var allEvents = events.Descendants("event");

                if (allEvents.Any())
                {
                    foreach (var nodeEvent in allEvents)
                    {
                        var allHandlers = nodeEvent.Descendants("Handler");

                        if (allHandlers.Any())
                        {
                            FormEvent formEvent = new FormEvent();
                            listEvents.Add(formEvent);

                            {
                                var name = (string)nodeEvent.Attribute("name");
                                var application = (string)nodeEvent.Attribute("application");
                                var active = (string)nodeEvent.Attribute("active");
                                var attribute = (string)nodeEvent.Attribute("attribute");
                                var path = "//" + string.Join("/", nodeEvent.AncestorsAndSelf().Reverse().Select(n => n.Name.LocalName));

                                if (string.IsNullOrEmpty(attribute))
                                {
                                    var nodeCell = nodeEvent.Ancestors("cell").FirstOrDefault();

                                    if (nodeCell != null)
                                    {
                                        var nodeControl = nodeCell.Descendants("control").FirstOrDefault();

                                        if (nodeControl != null)
                                        {
                                            var attrFieldName = nodeControl.Attribute("datafieldname");

                                            if (attrFieldName != null)
                                            {
                                                attribute = (string)attrFieldName;
                                            }
                                        }
                                    }
                                }

                                formEvent.Name = name;
                                formEvent.Active = active;
                                formEvent.Application = application;
                                formEvent.Path = path;
                                formEvent.Attribute = attribute;

                                if (!string.IsNullOrEmpty(attribute))
                                {
                                    attribute = string.Format("attribute=\"{0}\"", attribute);
                                }
                                else
                                {
                                    attribute = "Form";
                                }

                                name = string.Format("name=\"{0}\"", name);
                                active = string.Format("active=\"{0}\"", active);
                                application = string.Format("application=\"{0}\"", application);
                                path = string.Format("XPath=\"{0}\"", path);

                                formatEvent.AddLine(attribute, name, active, application, path);
                            }

                            foreach (var nodeHandler in allHandlers)
                            {
                                FormEventHandler formHandler = new FormEventHandler();
                                formEvent.Handlers.Add(formHandler);

                                formHandler.LibraryName = (string)nodeHandler.Attribute("libraryName");
                                formHandler.FunctionName = (string)nodeHandler.Attribute("functionName");
                                formHandler.Enabled = (string)nodeHandler.Attribute("enabled");
                                formHandler.PassExecutionContext = (string)nodeHandler.Attribute("passExecutionContext");
                                var parameters = (string)nodeHandler.Attribute("parameters");

                                if (!string.IsNullOrEmpty(parameters))
                                {
                                    parameters = parameters.Replace("\r\n", " ");
                                    parameters = parameters.Replace("\r", " ");
                                    parameters = parameters.Replace("\n", " ");
                                }

                                formHandler.Parameters = parameters;
                            }
                        }
                    }
                }
            }

            foreach (var formEvent in listEvents
                .OrderBy(f => f.Attribute)
                .ThenBy(f => f.Name)
                .ThenBy(f => f.Path)
                )
            {
                var name = string.Format("name=\"{0}\"", formEvent.Name);
                var active = string.Format("active=\"{0}\"", formEvent.Active);
                var application = string.Format("application=\"{0}\"", formEvent.Application);
                var path = string.Format("XPath=\"{0}\"", formEvent.Path);

                var attribute = formEvent.Attribute;

                if (!string.IsNullOrEmpty(attribute))
                {
                    attribute = string.Format("attribute=\"{0}\"", attribute);
                }
                else
                {
                    attribute = "Form";
                }

                result.Append("    ");
                result.AppendLine(formatEvent.FormatLine(attribute, name, active, application, path));

                FormatTextTableHandler handler = new FormatTextTableHandler();

                handler.SetHeader("LibraryName", "FunctionName", "Enabled", "PassExecutionContext", "Parameters");

                foreach (var formHandler in formEvent.Handlers)
                {
                    handler.AddLine(formHandler.LibraryName
                            , formHandler.FunctionName
                            , formHandler.Enabled
                            , formHandler.PassExecutionContext
                            , formHandler.Parameters);
                }

                List<string> lines = handler.GetFormatedLines(true);

                foreach (var item in lines)
                {
                    result.AppendLine(tabSpacer + tabSpacer + item);
                }

                result.AppendLine();
            }

            return result.ToString();
        }

        public List<string> GetFormLibraries(XElement doc)
        {
            List<string> result = new List<string>();

            var formLibraries = doc.Element("formLibraries");

            if (formLibraries != null)
            {
                var allLibraries = formLibraries.Descendants("Library");

                if (allLibraries.Any())
                {
                    foreach (var nodeLibrary in allLibraries)
                    {
                        var name = (string)nodeLibrary.Attribute("name");

                        if (!string.IsNullOrEmpty(name))
                        {
                            result.Add(name);
                        }
                    }
                }
            }

            return result;
        }
    }
}