using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    public class RibbonIntellisenseData
    {
        public string EntityLogicalName { get; private set; }

        public SortedSet<string> LabelTexts { get; private set; }

        public SortedSet<string> Images { get; private set; }

        public SortedSet<string> ModernImages { get; private set; }

        public ConcurrentDictionary<string, XElement> CommandDefinitions { get; private set; }

        public ConcurrentDictionary<string, XElement> EnableRules { get; private set; }

        public ConcurrentDictionary<string, XElement> DisplayRules { get; private set; }

        public SortedSet<string> Libraries { get; private set; }

        public SortedSet<string> FunctionsNames { get; private set; }

        public ConcurrentDictionary<string, RibbonLocation> Locations { get; private set; }

        public ConcurrentDictionary<string, RibbonGroupTemplate> Templates { get; private set; }

        public RibbonIntellisenseData()
        {
            ClearData();
        }

        public RibbonIntellisenseData(string entityName)
            : this()
        {
            this.EntityLogicalName = entityName;
        }

        private void CreateCollectionsIfNeeded()
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
                this.CommandDefinitions = new ConcurrentDictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (EnableRules == null)
            {
                this.EnableRules = new ConcurrentDictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (DisplayRules == null)
            {
                this.DisplayRules = new ConcurrentDictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);
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
                this.Locations = new ConcurrentDictionary<string, RibbonLocation>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (Templates == null)
            {
                this.Templates = new ConcurrentDictionary<string, RibbonGroupTemplate>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        private void ClearData()
        {
            this.LabelTexts = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.Images = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.ModernImages = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);

            this.CommandDefinitions = new ConcurrentDictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);
            this.EnableRules = new ConcurrentDictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);
            this.DisplayRules = new ConcurrentDictionary<string, XElement>(StringComparer.InvariantCultureIgnoreCase);

            this.Libraries = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.FunctionsNames = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);

            this.Locations = new ConcurrentDictionary<string, RibbonLocation>(StringComparer.InvariantCultureIgnoreCase);
            this.Templates = new ConcurrentDictionary<string, RibbonGroupTemplate>(StringComparer.InvariantCultureIgnoreCase);
        }

        public void LoadDataFromRibbon(XDocument docRibbon)
        {
            ClearData();

            if (docRibbon == null)
            {
                return;
            }

            {
                var elementsCommandDefinitions = docRibbon.XPathSelectElements("RibbonDefinitions/RibbonDefinition/CommandDefinitions/CommandDefinition");

                foreach (var command in elementsCommandDefinitions)
                {
                    var id = command.Attribute("Id")?.Value;

                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!this.CommandDefinitions.ContainsKey(id))
                        {
                            this.CommandDefinitions.TryAdd(id, command);
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
                            this.EnableRules.TryAdd(id, enableRule);
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
                            this.DisplayRules.TryAdd(id, displayRule);
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
                var elementsGroupTemplates = docRibbon.XPathSelectElements("RibbonDefinitions/RibbonDefinition/Templates/RibbonTemplates/GroupTemplate");

                foreach (var template in elementsGroupTemplates)
                {
                    var id = template.Attribute("Id")?.Value;

                    if (!string.IsNullOrEmpty(id) && !this.Templates.ContainsKey(id))
                    {
                        var newItem = new RibbonGroupTemplate()
                        {
                            Id = id,
                        };

                        this.Templates.TryAdd(id, newItem);

                        foreach (var item in template.Descendants("OverflowSection"))
                        {
                            string alias = item.Attribute("TemplateAlias")?.Value;

                            if (!string.IsNullOrEmpty(alias) && !newItem.TemplateAliases.Contains(alias))
                            {
                                newItem.TemplateAliases.Add(alias);
                            }
                        }
                    }
                }
            }

            {
                var elementsControls = docRibbon.Descendants("Controls");

                foreach (var controls in elementsControls)
                {
                    var id = controls.Attribute("Id")?.Value;

                    if (!string.IsNullOrEmpty(id) && !this.Locations.ContainsKey(id))
                    {
                        var newItem = new RibbonLocation()
                        {
                            Id = id,
                        };

                        var parentTemplateId = controls.AncestorsAndSelf().FirstOrDefault(e => e.Attribute("Template") != null)?.Attribute("Template")?.Value;

                        if (!string.IsNullOrEmpty(parentTemplateId) && this.Templates.ContainsKey(parentTemplateId))
                        {
                            newItem.Template = this.Templates[parentTemplateId];
                        }

                        this.Locations.TryAdd(id, newItem);

                        foreach (var item in controls.Elements())
                        {
                            newItem.Controls.Add(new RibbonLocationControl()
                            {
                                ControlType = item.Name.LocalName,
                                Id = item.Attribute("Id")?.Value,
                                Sequence = item.Attribute("Sequence")?.Value,
                                LabelText = item.Attribute("LabelText")?.Value,
                                Command = item.Attribute("Command")?.Value,

                                TemplateAlias = item.Attribute("TemplateAlias")?.Value,
                            });
                        }
                    }
                }
            }
        }

        public static RibbonIntellisenseData Get(string filePath, string entityName)
        {
            RibbonIntellisenseData result = null;

            if (File.Exists(filePath))
            {
                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        var arrayXml = File.ReadAllBytes(filePath);

                        arrayXml = FileOperations.UnzipRibbon(arrayXml);

                        using (var memStream = new MemoryStream())
                        {
                            memStream.Write(arrayXml, 0, arrayXml.Length);

                            memStream.Position = 0;

                            XDocument doc = XDocument.Load(memStream);

                            result = new RibbonIntellisenseData(entityName);

                            result.LoadDataFromRibbon(doc);
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToLog(ex);

                        FileOperations.CreateBackUpFile(filePath, ex);
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }

            return result;
        }
    }
}