using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlCommonUpdateCommand : AbstractSingleCommand
    {
        private CodeXmlCommonUpdateCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlCommonUpdateCommandId) { }

        public static CodeXmlCommonUpdateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlCommonUpdateCommand(commandService);
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
                                helper.HandleSiteMapUpdateCommand(null, doc, document.FullName);
                            }
                            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml, StringComparison.InvariantCultureIgnoreCase))
                            {
                                helper.HandleRibbonDiffXmlUpdateCommand(null, doc, document.FullName);
                            }
                            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootFetch, StringComparison.InvariantCultureIgnoreCase)
                                || string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootGrid, StringComparison.InvariantCultureIgnoreCase)
                                || string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootColumnSet, StringComparison.InvariantCultureIgnoreCase)
                            )
                            {
                                helper.HandleSavedQueryUpdateCommand(null, doc, document.FullName);
                            }
                            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootForm, StringComparison.InvariantCultureIgnoreCase))
                            {
                                helper.HandleSystemFormUpdateCommand(null, doc, document.FullName);
                            }
                            else if (doc.Root.Name == AbstractDynamicCommandXsdSchemas.RootActivity)
                            {
                                helper.HandleWorkflowUpdateCommand(null, doc, document.FullName);
                            }
                            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootWebResourceDependencies, StringComparison.InvariantCultureIgnoreCase))
                            {
                                helper.HandleWebResourceDependencyXmlUpdateCommand(null, doc, document.FullName);
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
                string nameCommand = Properties.CommandNames.CodeXmlSiteMapUpdateCommand;

                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

                if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlSiteMapUpdateByNameCommandFormat1, attribute.Value);
                }

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
            }
            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml, StringComparison.InvariantCultureIgnoreCase))
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName);

                if (attribute != null)
                {
                    string entityName = attribute.Value;

                    string nameCommand = Properties.CommandNames.CodeXmlRibbonDiffXmlApplicationUpdateCommand;

                    if (!string.IsNullOrEmpty(entityName))
                    {
                        nameCommand = string.Format(Properties.CommandNames.CodeXmlRibbonDiffXmlEntityUpdateCommandFormat1, entityName);
                    }

                    CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

                    CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
                }
            }
            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootFetch, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootGrid, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootColumnSet, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId);

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlSavedQueryUpdateCommand);

                if (attribute == null || !Guid.TryParse(attribute.Value, out _))
                {
                    menuCommand.Enabled = menuCommand.Visible = false;
                }
            }
            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootForm, StringComparison.InvariantCultureIgnoreCase))
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId);

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlSystemFormUpdateCommand);

                if (attribute == null || !Guid.TryParse(attribute.Value, out _))
                {
                    menuCommand.Enabled = menuCommand.Visible = false;
                }
            }
            else if (doc.Name == AbstractDynamicCommandXsdSchemas.RootActivity)
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWorkflowId);

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlWorkflowUpdateCommand);

                if (attribute == null || !Guid.TryParse(attribute.Value, out _))
                {
                    menuCommand.Enabled = menuCommand.Visible = false;
                }
            }
            else if (string.Equals(docRootName, AbstractDynamicCommandXsdSchemas.RootWebResourceDependencies, StringComparison.InvariantCultureIgnoreCase))
            {
                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWebResourceName);

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlWebResourceDependencyXmlUpdateCommand);

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