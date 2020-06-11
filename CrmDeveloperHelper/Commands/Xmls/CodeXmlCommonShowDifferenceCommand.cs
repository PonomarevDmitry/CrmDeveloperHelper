using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlCommonShowDifferenceCommand : AbstractCommand
    {
        private CodeXmlCommonShowDifferenceCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlCommonShowDifferenceCommandId) { }

        public static CodeXmlCommonShowDifferenceCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlCommonShowDifferenceCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                var objTextDoc = document.Object(nameof(EnvDTE.TextDocument));
                if (objTextDoc != null
                    && objTextDoc is EnvDTE.TextDocument textDocument
                )
                {
                    string text = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                    if (!string.IsNullOrEmpty(text))
                    {
                        if (ContentComparerHelper.TryParseXmlDocument(text, out var doc))
                        {
                            string docRootName = doc.Root.Name.ToString();

                            if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootSiteMap, StringComparison.InvariantCultureIgnoreCase))
                            {
                                helper.HandleSiteMapDifferenceCommand(null, doc, document.FullName);
                            }
                            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml, StringComparison.InvariantCultureIgnoreCase))
                            {
                                helper.HandleRibbonDiffXmlDifferenceCommand(null, doc, document.FullName);
                            }
                            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions, StringComparison.InvariantCultureIgnoreCase))
                            {
                                helper.HandleRibbonDifferenceCommand(null, doc, document.FullName);
                            }
                            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootFetch, StringComparison.InvariantCultureIgnoreCase)
                                || string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootGrid, StringComparison.InvariantCultureIgnoreCase)
                                || string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootColumnSet, StringComparison.InvariantCultureIgnoreCase)
                            )
                            {
                                helper.HandleSavedQueryDifferenceCommand(null, doc, document.FullName);
                            }
                            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootForm, StringComparison.InvariantCultureIgnoreCase))
                            {
                                helper.HandleSystemFormDifferenceCommand(null, doc, document.FullName);
                            }
                            else if (doc.Root.Name == AbstractDynamicCommandXsdSchemas.RootActivity)
                            {
                                helper.HandleWorkflowDifferenceCommand(null, doc, document.FullName);
                            }
                            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootWebResourceDependencies, StringComparison.InvariantCultureIgnoreCase))
                            {
                                helper.HandleWebResourceDependencyXmlDifferenceCommand(null, doc, document.FullName);
                            }
                        }
                    }
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXml(applicationObject, menuCommand, out var doc);

            if (doc == null)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
                return;
            }

            string docRootName = doc.Name.ToString();

            if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootSiteMap, StringComparison.InvariantCultureIgnoreCase))
            {
                string nameCommand = Properties.CommandNames.CodeXmlSiteMapShowDifferenceCommand;

                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

                if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlSiteMapShowDifferenceByNameCommandFormat1, attribute.Value);
                }

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
            }
            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml, StringComparison.InvariantCultureIgnoreCase))
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName);

                if (attribute != null)
                {
                    string entityName = attribute.Value;

                    string nameCommand = Properties.CommandNames.CodeXmlRibbonDiffXmlApplicationShowDifferenceCommand;

                    if (!string.IsNullOrEmpty(entityName))
                    {
                        nameCommand = string.Format(Properties.CommandNames.CodeXmlRibbonDiffXmlEntityShowDifferenceCommandFormat1, entityName);
                    }

                    CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
                }
            }
            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions, StringComparison.InvariantCultureIgnoreCase))
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName);

                if (attribute != null)
                {
                    string entityName = attribute.Value;

                    string nameCommand = Properties.CommandNames.CodeXmlRibbonApplicationShowDifferenceCommand;

                    if (!string.IsNullOrEmpty(entityName))
                    {
                        nameCommand = string.Format(Properties.CommandNames.CodeXmlRibbonEntityShowDifferenceCommandFormat1, entityName);
                    }

                    CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
                }
            }
            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootFetch, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootGrid, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootColumnSet, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId);

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlSavedQueryShowDifferenceCommand);

                if (attribute == null || !Guid.TryParse(attribute.Value, out _))
                {
                    menuCommand.Enabled = menuCommand.Visible = false;
                }
            }
            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootForm, StringComparison.InvariantCultureIgnoreCase))
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId);

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlSystemFormShowDifferenceCommand);

                if (attribute == null || !Guid.TryParse(attribute.Value, out _))
                {
                    menuCommand.Enabled = menuCommand.Visible = false;
                }
            }
            else if (doc.Name == AbstractDynamicCommandXsdSchemas.RootActivity)
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWorkflowId);

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlWorkflowShowDifferenceCommand);

                if (attribute == null || !Guid.TryParse(attribute.Value, out _))
                {
                    menuCommand.Enabled = menuCommand.Visible = false;
                }
            }
            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootWebResourceDependencies, StringComparison.InvariantCultureIgnoreCase))
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWebResourceName);

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlWebResourceDependencyXmlShowDifferenceCommand);

                if (attribute == null || string.IsNullOrEmpty(attribute.Value))
                {
                    menuCommand.Enabled = menuCommand.Visible = false;
                }
            }
            else
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }



            //else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootSiteMap, StringComparison.InvariantCultureIgnoreCase))
            //{

            //}
            //else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootSiteMap, StringComparison.InvariantCultureIgnoreCase))
            //{

            //}
            //else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootSiteMap, StringComparison.InvariantCultureIgnoreCase))
            //{

            //}
        }


    }
}