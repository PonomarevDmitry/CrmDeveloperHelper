using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class RibbonIntellisenseData
    {
        private const int _loadPeriodInMinutes = 5;

        public DateTime? NextLoadFileDate { get; set; }

        [DataMember]
        public Guid ConnectionId { get; private set; }

        [DataMember]
        public string EntityName { get; private set; }

        [DataMember]
        public SortedSet<string> LabelTexts { get; private set; }

        [DataMember]
        public SortedSet<string> Images { get; private set; }

        [DataMember]
        public SortedSet<string> ModernImages { get; private set; }

        [DataMember]
        public Dictionary<string, XElement> CommandDefinitions { get; private set; }

        [DataMember]
        public Dictionary<string, XElement> EnableRules { get; private set; }

        [DataMember]
        public Dictionary<string, XElement> DisplayRules { get; private set; }

        [DataMember]
        public SortedSet<string> Libraries { get; private set; }

        [DataMember]
        public SortedSet<string> FunctionsNames { get; private set; }

        [DataMember]
        public Dictionary<string, RibbonLocation> Locations { get; private set; }

        public RibbonIntellisenseData(Guid connectionId, string entityName)
        {
            this.ConnectionId = connectionId;
            this.EntityName = entityName ?? string.Empty;

            ClearData();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (LabelTexts == null)
            {
                this.LabelTexts = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (Images == null)
            {
                this.Images = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (ModernImages == null)
            {
                this.ModernImages = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (CommandDefinitions == null)
            {
                this.CommandDefinitions = new Dictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (EnableRules == null)
            {
                this.EnableRules = new Dictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (DisplayRules == null)
            {
                this.DisplayRules = new Dictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (Libraries == null)
            {
                this.Libraries = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (FunctionsNames == null)
            {
                this.FunctionsNames = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (Locations == null)
            {
                this.Locations = new Dictionary<string, RibbonLocation>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        public void ClearData()
        {
            this.LabelTexts = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.Images = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.ModernImages = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);

            this.CommandDefinitions = new Dictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);
            this.EnableRules = new Dictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);
            this.DisplayRules = new Dictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);

            this.Libraries = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.FunctionsNames = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.Locations = new Dictionary<string, RibbonLocation>(StringComparer.InvariantCultureIgnoreCase);
        }

        public void LoadDataFromRibbon(XDocument docRibbon)
        {
            if (docRibbon == null)
            {
                return;
            }

            this.NextLoadFileDate = DateTime.Now.AddMinutes(_loadPeriodInMinutes);

            {
                var elementsCommandDefinitions = docRibbon.XPathSelectElements("RibbonDefinitions/RibbonDefinition/CommandDefinitions/CommandDefinition");

                foreach (var command in elementsCommandDefinitions)
                {
                    var id = command.Attribute("Id")?.Value;

                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!this.CommandDefinitions.ContainsKey(id))
                        {
                            this.CommandDefinitions.Add(id, command);
                        }
                    }
                }
            }

            {
                var elementsEnableRules = docRibbon.XPathSelectElements("RibbonDefinitions/RibbonDefinition/RuleDefinitions/EnableRules/EnableRule");

                foreach (var enableRule in elementsEnableRules)
                {
                    var id = enableRule.Attribute("Id")?.Value;

                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!this.EnableRules.ContainsKey(id))
                        {
                            this.EnableRules.Add(id, enableRule);
                        }
                    }
                }
            }

            {
                var elementsDisplayRules = docRibbon.XPathSelectElements("RibbonDefinitions/RibbonDefinition/RuleDefinitions/DisplayRules/DisplayRule");

                foreach (var displayRule in elementsDisplayRules)
                {
                    var id = displayRule.Attribute("Id")?.Value;

                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!this.DisplayRules.ContainsKey(id))
                        {
                            this.DisplayRules.Add(id, displayRule);
                        }
                    }
                }
            }

            {
                var elementsLibraries = docRibbon.Descendants().Where(e => e.Attribute("Library") != null && !string.IsNullOrEmpty(e.Attribute("Library").Value));

                foreach (var library in elementsLibraries)
                {
                    var value = library.Attribute("Library")?.Value;

                    if (!string.IsNullOrEmpty(value)
                        && !value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                        )
                    {
                        this.Libraries.Add(value);
                    }
                }
            }

            {
                var elementsFunctionNames = docRibbon.Descendants().Where(e => e.Attribute("FunctionName") != null && !string.IsNullOrEmpty(e.Attribute("FunctionName").Value));

                foreach (var functionName in elementsFunctionNames)
                {
                    var value = functionName.Attribute("FunctionName")?.Value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        this.FunctionsNames.Add(value);
                    }
                }
            }

            {
                var elementsLables = docRibbon.Descendants().Where(e => e.Attributes().Any(a => XmlCompletionSource.LabelXmlAttributes.Contains(a.Name.LocalName) && !string.IsNullOrEmpty(a.Value)));

                foreach (var label in elementsLables)
                {
                    if (!string.Equals(label.Name.LocalName, "Layout", StringComparison.InvariantCultureIgnoreCase)
                        )
                    {
                        foreach (var attr in label.Attributes().Where(a => !string.IsNullOrEmpty(a.Value)))
                        {
                            if (XmlCompletionSource.LabelXmlAttributes.Contains(attr.Name.LocalName))
                            {
                                this.LabelTexts.Add(attr.Value);
                            }
                        }
                    }
                }
            }

            {
                var elementsImages = docRibbon.Descendants().Where(e => e.Attributes().Any(a => XmlCompletionSource.ImagesXmlAttributes.Contains(a.Name.LocalName)
                    && !string.IsNullOrEmpty(a.Value)
                    && !a.Value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                    ));

                foreach (var label in elementsImages)
                {
                    foreach (var attr in label.Attributes().Where(a => !string.IsNullOrEmpty(a.Value)))
                    {
                        if (XmlCompletionSource.ImagesXmlAttributes.Contains(attr.Name.LocalName)
                            && !attr.Value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                            )
                        {
                            this.Images.Add(attr.Value);
                        }
                    }
                }
            }

            {
                var elementsModernImages = docRibbon.Descendants().Where(e => e.Attribute("ModernImage") != null && !string.IsNullOrEmpty(e.Attribute("ModernImage").Value));

                foreach (var modernImage in elementsModernImages)
                {
                    var value = modernImage.Attribute("ModernImage")?.Value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        this.ModernImages.Add(value);
                    }
                }
            }

            {
                var elementsControls = docRibbon.Descendants("Controls");

                foreach (var controls in elementsControls)
                {
                    var id = controls.Attribute("Id")?.Value;

                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!this.Locations.ContainsKey(id))
                        {
                            var newItem = new RibbonLocation()
                            {
                                Id = id,
                            };

                            this.Locations.Add(id, newItem);

                            foreach (var item in controls.Elements())
                            {
                                newItem.Controls.Add(new RibbonLocationControl()
                                {
                                    ControlType = item.Name.LocalName,
                                    Id = item.Attribute("Id")?.Value,
                                    Sequence = item.Attribute("Sequence")?.Value,
                                    LabelText = item.Attribute("LabelText")?.Value,
                                    Command = item.Attribute("Command")?.Value,
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}
