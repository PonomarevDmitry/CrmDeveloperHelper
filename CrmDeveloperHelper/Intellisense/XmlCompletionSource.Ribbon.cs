using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public sealed partial class XmlCompletionSource
    {
        private static HashSet<string> _controlsWithImagesXmlElements = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "Button"
            , "CheckBox"
            , "ComboBox"
            , "Controls"
            , "DropDown"
            , "FlyoutAnchor"
            , "GalleryButton"
            , "Group"
            , "Label"
            , "MRUSplitButton"
            , "QAT"
            , "Spinner"
            , "SplitButton"
            , "TextBox"
            , "ToggleButton"
            //, "Button"
            //, "Button"
            //, "Button"
        };

        public static HashSet<string> ImagesXmlAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "Image16by16"
            , "Image32by32"
            , "ToolTipImage32by32"
            , "Image"
            , "Image32by32Popup"

            , "ImageUpArrow"
            , "ImageSideArrow"
            , "ImageDownArrow"

            , "ImageHover"
            , "ImageDown"
            , "ImageLeftSide"
            , "ImageLeftSideHover"
            , "ImageLeftSideDown"
            , "ImageRightSide"
            , "ImageRightSideHover"
            , "ImageRightSideDown"

            , "Image32by32GroupPopupDefault"
            , "ToolTipFooterImage16by16"
            , "ToolTipDisabledCommandImage16by16"
        };

        public static HashSet<string> LabelXmlAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "LabelText"
            , "Alt"
            , "Description"
            , "ToolTipTitle"
            , "ToolTipDescription"
            , "Title"

            , "ToolTipSelectedItemTitle"

            , "MenuSectionInitialTitle"
            , "MenuSectionTitle"

            , "ToolTipFooterText"
            , "ToolTipDisabledCommandDescription"
            , "ToolTipDisabledCommandTitle"
            , "ToolTipSelectedItemTitlePrefix"
            , "ATContextualTabText"
            , "ATTabPositionText"
            , "NavigationHelpText"

            , "AltArrow"
            , "MenuAlt"
            , "AltDownArrow"
            , "AltUpArrow"
            , "LayoutTitle"
        };

        public static HashSet<string> CommandXmlAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "Command"
            , "QueryCommand"
            , "CommandPreview"
            , "CommandRevert"

            , "CommandMenuOpen"
            , "CommandMenuClose"
            , "CommandPreview"
            , "CommandPreviewRevert"
            , "PopulateQueryCommand"

            , "MenuCommand"
            , "RootEventCommand"
            , "TabSwitchCommand"
            , "ScaleCommand"
        };

        private void FillSessionForRibbonDiffXmlCompletionSet(
            IList<CompletionSet> completionSets
            , XElement doc
            , ConnectionData connectionData
            , ConnectionIntellisenseDataRepository repositoryEntities
            , WebResourceIntellisenseDataRepository repositoryWebResource
            , RibbonIntellisenseDataRepository repositoryRibbon
            , XElement currentXmlNode
            , string currentNodeName
            , string currentAttributeName
            , ITrackingSpan applicableTo
        )
        {
            try
            {
                RibbonIntellisenseData ribbonIntellisenseData = null;

                var attrEntityName = doc.Attribute(IntellisenseContext.IntellisenseContextAttributeEntityName);

                if (attrEntityName != null)
                {
                    if (!string.IsNullOrEmpty(attrEntityName.Value))
                    {
                        var connectionIntellisense = repositoryEntities.GetEntitiesIntellisenseData();

                        if (connectionIntellisense != null
                            && connectionIntellisense.Entities != null
                            && connectionIntellisense.Entities.ContainsKey(attrEntityName.Value)
                        )
                        {
                            ribbonIntellisenseData = repositoryRibbon.GetRibbonIntellisenseData(attrEntityName.Value);
                        }
                    }
                    else
                    {
                        ribbonIntellisenseData = repositoryRibbon.GetRibbonIntellisenseData(string.Empty);
                    }
                }

                if (_controlsWithImagesXmlElements.Contains(currentNodeName)
                    && ImagesXmlAttributes.Contains(currentAttributeName)
                )
                {
#warning WebResourcesIcon
                    //FillWebResourcesIcons(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.WebResourcesIcon?.Values?.ToList(), "WebResources");

                    if (ribbonIntellisenseData != null)
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, ribbonIntellisenseData.Images, "Images");
                    }
                }

                if (string.Equals(currentAttributeName, "ModernImage", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (ribbonIntellisenseData != null)
                    {
                        FillIntellisenseBySet(completionSets, applicableTo, ribbonIntellisenseData.ModernImages, "ModernImages");
                    }
                }

                if (string.Equals(currentNodeName, "CustomAction", StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(currentAttributeName, "Location", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    if (ribbonIntellisenseData != null)
                    {
                        var sorted = new SortedSet<string>(ribbonIntellisenseData.Locations.Keys);

                        FillRibbonLocations(completionSets, applicableTo, sorted, "Locations");
                    }
                }

                if (string.Equals(currentAttributeName, "Sequence", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (ribbonIntellisenseData != null)
                    {
                        FillRibbonSequences(completionSets, applicableTo, currentXmlNode, ribbonIntellisenseData.Locations, "Locations");
                    }
                }

                if (string.Equals(currentAttributeName, "TemplateAlias", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (ribbonIntellisenseData != null)
                    {
                        FillRibbonTemplateAliases(completionSets, applicableTo, currentXmlNode, ribbonIntellisenseData.Locations, "TemplateAliases");
                    }
                }

                if (LabelXmlAttributes.Contains(currentAttributeName))
                {
                    bool isTitleElement = string.Equals(currentNodeName, "Title", StringComparison.InvariantCultureIgnoreCase)
                        && currentXmlNode.Parent != null
                        && string.Equals(currentXmlNode.Parent.Name.LocalName, "Titles", StringComparison.InvariantCultureIgnoreCase)
                        && currentXmlNode.Parent.Parent != null
                        && string.Equals(currentXmlNode.Parent.Parent.Name.LocalName, "LocLabel", StringComparison.InvariantCultureIgnoreCase);

                    if (!isTitleElement)
                    {
                        var localValues = FillLocLables(completionSets, applicableTo, doc, "LocLabels");

                        if (ribbonIntellisenseData != null)
                        {
                            var sorted = new SortedSet<string>(ribbonIntellisenseData.LabelTexts.Where(s => !localValues.Contains(s)));

                            FillIntellisenseBySet(completionSets, applicableTo, sorted, "Labels in Ribbon");
                        }
                    }
                }

                if (CommandXmlAttributes.Contains(currentAttributeName))
                {
                    var localValues = FillCommandsLocal(completionSets, applicableTo, doc, "Commands");

                    if (ribbonIntellisenseData != null)
                    {
                        var sorted = new SortedSet<string>(ribbonIntellisenseData.CommandDefinitions.Keys.Where(s => !localValues.Contains(s)));

                        FillIntellisenseBySet(completionSets, applicableTo, sorted, "Commands in Ribbon");
                    }
                }

                if (string.Equals(currentNodeName, "EnableRule", StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(currentAttributeName, "Id", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent != null
                    && string.Equals(currentXmlNode.Parent.Name.LocalName, "EnableRules", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent.Parent != null
                    && string.Equals(currentXmlNode.Parent.Parent.Name.LocalName, "CommandDefinition", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    var localValues = FillEnableRulesLocal(completionSets, applicableTo, doc, "EnableRules");

                    if (ribbonIntellisenseData != null)
                    {
                        var sorted = new SortedSet<string>(ribbonIntellisenseData.EnableRules.Keys.Where(s => !localValues.Contains(s)));

                        FillIntellisenseBySet(completionSets, applicableTo, sorted, "EnableRules in Ribbon");
                    }
                }

                if (string.Equals(currentNodeName, "DisplayRule", StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(currentAttributeName, "Id", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent != null
                    && string.Equals(currentXmlNode.Parent.Name.LocalName, "DisplayRules", StringComparison.InvariantCultureIgnoreCase)
                    && currentXmlNode.Parent.Parent != null
                    && string.Equals(currentXmlNode.Parent.Parent.Name.LocalName, "CommandDefinition", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    var localValues = FillDisplayRulesLocal(completionSets, applicableTo, doc, "DisplayRules");

                    if (ribbonIntellisenseData != null)
                    {
                        var sorted = new SortedSet<string>(ribbonIntellisenseData.DisplayRules.Keys.Where(s => !localValues.Contains(s)).Select(s => s));

                        FillIntellisenseBySet(completionSets, applicableTo, sorted, "DisplayRules in Ribbon");
                    }
                }

                if (string.Equals(currentAttributeName, "EntityName", StringComparison.InvariantCultureIgnoreCase)
                    &&
                    (
                        string.Equals(currentNodeName, "EntityRule", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(currentNodeName, "EntityPropertyRule", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(currentNodeName, "FormEntityContextRule", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(currentNodeName, "EntityPrivilegeRule", StringComparison.InvariantCultureIgnoreCase)
                    )
                )
                {
                    FillEntityNamesInList(completionSets, applicableTo, repositoryEntities, false, false);
                }

                if (string.Equals(currentAttributeName, IntellisenseContext.IntellisenseContextNamespacePrefix + ":" + IntellisenseContext.NameIntellisenseContextAttributeEntityName, StringComparison.InvariantCultureIgnoreCase)
                    &&
                    (
                        string.Equals(currentNodeName, Commands.AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml, StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(currentNodeName, Commands.AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions, StringComparison.InvariantCultureIgnoreCase)
                    )
                )
                {
                    FillEntityNamesInList(completionSets, applicableTo, repositoryEntities, false, false);
                }

                if (string.Equals(currentNodeName, "ValueRule", StringComparison.InvariantCultureIgnoreCase)
                   && string.Equals(currentAttributeName, "Field", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    if (attrEntityName != null
                        && !string.IsNullOrEmpty(attrEntityName.Value)
                    )
                    {
                        var entityIntellisenseData = repositoryEntities.GetEntitiesIntellisenseData();

                        if (entityIntellisenseData != null
                            && entityIntellisenseData.Entities.ContainsKey(attrEntityName.Value)
                        )
                        {
                            FillEntityIntellisenseDataAttributes(completionSets, applicableTo, entityIntellisenseData.Entities[attrEntityName.Value]);
                        }
                    }
                }

                if (string.Equals(currentNodeName, "ValueRule", StringComparison.InvariantCultureIgnoreCase)
                   && string.Equals(currentAttributeName, "Value", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    if (attrEntityName != null
                        && !string.IsNullOrEmpty(attrEntityName.Value)
                        && currentXmlNode.Attribute("Field") != null
                        && !string.IsNullOrEmpty(currentXmlNode.Attribute("Field").Value)
                    )
                    {
                        FillEntityAttributeValues(completionSets, applicableTo, repositoryEntities, attrEntityName.Value, currentXmlNode.Attribute("Field").Value);
                    }
                }

                if (string.Equals(currentNodeName, "CustomRule", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Library", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillWebResourcesTextWithWebResourcePrefix(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.GetJavaScriptWebResources()?.ToList(), "WebResources");

                        if (ribbonIntellisenseData != null)
                        {
                            FillIntellisenseBySet(completionSets, applicableTo, ribbonIntellisenseData.Libraries, "Library from Ribbon");
                        }
                    }
                    else if (string.Equals(currentAttributeName, "FunctionName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (ribbonIntellisenseData != null)
                        {
                            FillIntellisenseBySet(completionSets, applicableTo, ribbonIntellisenseData.FunctionsNames, "FunctionName from Ribbon");
                        }
                    }
                }
                else if (string.Equals(currentNodeName, "JavaScriptFunction", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Library", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillWebResourcesTextWithWebResourcePrefix(completionSets, applicableTo, repositoryWebResource.GetConnectionWebResourceIntellisenseData()?.GetJavaScriptWebResources()?.ToList(), "WebResources");

                        if (ribbonIntellisenseData != null)
                        {
                            FillIntellisenseBySet(completionSets, applicableTo, ribbonIntellisenseData.Libraries, "Library from Ribbon");
                        }
                    }
                    else if (string.Equals(currentAttributeName, "FunctionName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (ribbonIntellisenseData != null)
                        {
                            FillIntellisenseBySet(completionSets, applicableTo, ribbonIntellisenseData.FunctionsNames, "FunctionName from Ribbon");
                        }
                    }
                }
                else if (string.Equals(currentNodeName, "Title", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "languagecode", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillLCID(completionSets, applicableTo);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(connectionData, ex);
            }
        }

        private void FillRibbonSequences(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement currentXmlNode, Dictionary<string, RibbonLocation> locations, string nameCompletionSet)
        {
            if (locations == null || !locations.Any())
            {
                return;
            }

            List<RibbonLocationControl> controls = null;

            var customAction = currentXmlNode.AncestorsAndSelf().FirstOrDefault(e => string.Equals(e.Name.LocalName, "CustomAction", StringComparison.InvariantCultureIgnoreCase));

            if (customAction != null
                && customAction.Attribute("Location") != null
                )
            {
                var location = customAction.Attribute("Location").Value;

                if (!string.IsNullOrEmpty(location))
                {
                    if (location.EndsWith("._children", StringComparison.InvariantCultureIgnoreCase))
                    {
                        location = Regex.Replace(location, @"\._children", string.Empty, RegexOptions.IgnoreCase);
                    }

                    if (locations.ContainsKey(location))
                    {
                        controls = locations[location].Controls;
                    }
                }
            }

            if (controls != null && controls.Any())
            {
                List<CrmCompletion> list = new List<CrmCompletion>();

                foreach (var control in controls)
                {
                    StringBuilder str = new StringBuilder();
                    str.AppendFormat("ControlType:\t{0}", control.ControlType);

                    if (!string.IsNullOrEmpty(control.Sequence))
                    {
                        str.AppendLine().AppendFormat("Sequence:\t{0}", control.Sequence);
                    }

                    if (!string.IsNullOrEmpty(control.Id))
                    {
                        str.AppendLine().AppendFormat("Id:\t\t{0}", control.Id);
                    }

                    if (!string.IsNullOrEmpty(control.LabelText))
                    {
                        str.AppendLine().AppendFormat("LabelText:\t{0}", control.LabelText);
                    }

                    if (!string.IsNullOrEmpty(control.Command))
                    {
                        str.AppendLine().AppendFormat("Command:\t{0}", control.Command);
                    }

                    if (!string.IsNullOrEmpty(control.TemplateAlias))
                    {
                        str.AppendLine().AppendFormat("TemplateAlias:\t{0}", control.TemplateAlias);
                    }

                    list.Add(CreateCompletion(string.Format("{0} - {1} - {2}", control.Sequence, control.TemplateAlias, control.Id), control.Sequence, str.ToString(), _defaultGlyph, Enumerable.Empty<string>()));
                }

                completionSets.Add(new CrmCompletionSet(SourceNameMonikerRibbonSequences, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillRibbonTemplateAliases(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement currentXmlNode, Dictionary<string, RibbonLocation> locations, string nameCompletionSet)
        {
            if (locations == null || !locations.Any())
            {
                return;
            }

            RibbonLocation ribbonLocation = null;

            var customAction = currentXmlNode.AncestorsAndSelf().FirstOrDefault(e => string.Equals(e.Name.LocalName, "CustomAction", StringComparison.InvariantCultureIgnoreCase));

            if (customAction != null
                && customAction.Attribute("Location") != null
                )
            {
                var location = customAction.Attribute("Location").Value;

                if (!string.IsNullOrEmpty(location))
                {
                    if (location.EndsWith("._children", StringComparison.InvariantCultureIgnoreCase))
                    {
                        location = Regex.Replace(location, @"\._children", string.Empty, RegexOptions.IgnoreCase);
                    }

                    if (locations.ContainsKey(location))
                    {
                        ribbonLocation = locations[location];
                    }
                }
            }

            if (ribbonLocation != null && ribbonLocation.Template != null && ribbonLocation.Template.TemplateAliases.Any())
            {
                List<CrmCompletion> list = new List<CrmCompletion>();

                foreach (var alias in ribbonLocation.Template.TemplateAliases.OrderBy(s => s))
                {
                    list.Add(CreateCompletion(alias, alias, string.Empty, _defaultGlyph, Enumerable.Empty<string>()));
                }

                completionSets.Add(new CrmCompletionSet(SourceNameMonikerRibbonSequences, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillRibbonLocations(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, SortedSet<string> values, string nameCompletionSet)
        {
            if (values == null || !values.Any())
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var value in values)
            {
                list.Add(CreateCompletion(value, value + "._children", null, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerRibbonLocations, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private HashSet<string> FillDisplayRulesLocal(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement doc, string nameCompletionSet)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            List<CrmCompletion> list = new List<CrmCompletion>();

            var locLabelsList = doc.XPathSelectElements("./RuleDefinitions/DisplayRules/DisplayRule").Where(e => e.Attribute("Id") != null && !string.IsNullOrEmpty(e.Attribute("Id").Value));

            foreach (var label in locLabelsList.OrderBy(e => (string)e.Attribute("Id")))
            {
                string id = (string)label.Attribute("Id");

                result.Add(id);

                list.Add(CreateCompletion(id, id, string.Empty, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerRibbonDisplayRules, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));

            return result;
        }

        private HashSet<string> FillEnableRulesLocal(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement doc, string nameCompletionSet)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            List<CrmCompletion> list = new List<CrmCompletion>();

            var locLabelsList = doc.XPathSelectElements("./RuleDefinitions/EnableRules/EnableRule").Where(e => e.Attribute("Id") != null && !string.IsNullOrEmpty(e.Attribute("Id").Value));

            foreach (var label in locLabelsList.OrderBy(e => (string)e.Attribute("Id")))
            {
                string id = (string)label.Attribute("Id");

                result.Add(id);

                list.Add(CreateCompletion(id, id, string.Empty, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerRibbonEnableRules, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));

            return result;
        }

        private HashSet<string> FillCommandsLocal(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement doc, string nameCompletionSet)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            List<CrmCompletion> list = new List<CrmCompletion>();

            var locLabelsList = doc.XPathSelectElements("./CommandDefinitions/CommandDefinition").Where(e => e.Attribute("Id") != null && !string.IsNullOrEmpty(e.Attribute("Id").Value));

            foreach (var label in locLabelsList.OrderBy(e => (string)e.Attribute("Id")))
            {
                string id = (string)label.Attribute("Id");

                result.Add(id);

                list.Add(CreateCompletion(id, id, string.Empty, _defaultGlyph, Enumerable.Empty<string>()));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerRibbonCommands, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));

            return result;
        }

        private HashSet<string> FillLocLables(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, XElement doc, string nameCompletionSet)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            List<CrmCompletion> list = new List<CrmCompletion>();

            var locLabelsList = doc.XPathSelectElements("./LocLabels/LocLabel").Where(e => e.Attribute("Id") != null && !string.IsNullOrEmpty(e.Attribute("Id").Value));

            foreach (var label in locLabelsList.OrderBy(e => (string)e.Attribute("Id")))
            {
                string id = (string)label.Attribute("Id");

                List<string> compareValues = new List<string>() { id };

                var titlesList = label.XPathSelectElements("./Titles/Title").Where(e =>
                    e.Attribute("languagecode") != null
                    && int.TryParse(e.Attribute("languagecode").Value, out _)
                    && e.Attribute("description") != null
                    && !string.IsNullOrEmpty(e.Attribute("description").Value)
                );

                StringBuilder str = new StringBuilder(id);

                foreach (var title in titlesList.OrderBy(e => int.Parse(e.Attribute("languagecode").Value)))
                {
                    string description = (string)title.Attribute("description");
                    int languageCode = (int)title.Attribute("languagecode");

                    str.AppendLine();

                    if (LanguageLocale.KnownLocales.ContainsKey(languageCode))
                    {
                        str.AppendFormat("{0} - {1}: {2}", languageCode, LanguageLocale.KnownLocales[languageCode], description);
                    }
                    else
                    {
                        str.AppendFormat("{0}: {2}", languageCode, description);
                    }
                }

                string insertText = string.Format("$LocLabels:{0}", id);

                result.Add(insertText);

                list.Add(CreateCompletion(id, insertText, str.ToString(), _defaultGlyph, compareValues));
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerRibbonLocLables, nameCompletionSet, applicableTo, list, Enumerable.Empty<CrmCompletion>()));

            return result;
        }
    }
}