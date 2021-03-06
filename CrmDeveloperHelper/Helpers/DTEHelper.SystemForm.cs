﻿using EnvDTE;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleSystemFormDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormDifference(conn, commonConfig, selectedFile));
        }

        public void HandleSystemFormDifferenceCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormDifference(conn, commonConfig, doc, filePath));
        }

        public void HandleSystemFormUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            Func<ConnectionData, string> message = (conn) => string.Format(Properties.MessageBoxStrings.PublishSystemFormFormat2, selectedFile.FilePath, conn.GetDescriptionColumn());
            string title = Properties.MessageBoxStrings.ConfirmPublishSystemForm;

            GetConnectionConfigConfirmActionAndExecute(connectionData, message, title, (conn, commonConfig) => Controller.StartSystemFormUpdate(conn, commonConfig, selectedFile));
        }

        public void HandleSystemFormUpdateCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            Func<ConnectionData, string> message = (conn) => string.Format(Properties.MessageBoxStrings.PublishSystemFormFormat2, filePath, conn.GetDescriptionColumn());
            string title = Properties.MessageBoxStrings.ConfirmPublishSystemForm;

            GetConnectionConfigConfirmActionAndExecute(connectionData, message, title, (conn, commonConfig) => Controller.StartSystemFormUpdate(conn, commonConfig, doc, filePath));
        }

        public void HandleSystemFormOpenInWebCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormOpenInWeb(conn, commonConfig, selectedFile));
        }

        public void HandleSystemFormGetCurrentCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormGetCurrentFormXml(conn, commonConfig, selectedFile));
        }

        public void HandleSystemFormGetCurrentAttributeCommand(ConnectionData connectionData, Guid formId, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormGetCurrentAttribute(conn, commonConfig, formId, actionOnComponent, fieldName, fieldTitle));
        }

        public void HandleLinkedSystemFormAddingToSolutionCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, IEnumerable<Guid> formIdList)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartAddingLinkedSystemFormToSolution(conn, commonConfig, solutionUniqueName, withSelect, formIdList));
        }

        public void HandleLinkedSystemFormChangeInEntityEditorCommand(ConnectionData connectionData, string entityName, Guid formId, int formType)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartLinkedSystemFormChangeInEntityEditor(conn, commonConfig, entityName, formId, formType));
        }

        public void HandleSystemFormCopyToClipboardTabsAndSectionsCommand(ConnectionData connectionData, JavaScriptObjectType jsObjectType, string entityName, Guid formId, int formType)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormCopyToClipboardTabsAndSections(conn, commonConfig, jsObjectType, entityName, formId, formType));
        }

        public void HandleExplorerSystemForm()
        {
            string selection = GetSelectedText();

            HandleExplorerSystemForm(null, null, null, selection);
        }

        public void HandleExplorerSystemForm(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            HandleExplorerSystemForm(connectionData, null, null, selection);
        }

        public void HandleExplorerSystemForm(string selection)
        {
            HandleExplorerSystemForm(null, null, null, selection);
        }

        public void HandleExplorerSystemForm(string entityName, string selection)
        {
            HandleExplorerSystemForm(null, null, entityName, selection);
        }

        public void HandleExplorerSystemForm(ConnectionData connectionData, string selection)
        {
            HandleExplorerSystemForm(connectionData, null, null, selection);
        }

        public void HandleExplorerSystemForm(ConnectionData connectionData, SelectedItem selectedItem)
        {
            HandleExplorerSystemForm(connectionData, selectedItem, null, string.Empty);
        }

        private void HandleExplorerSystemForm(ConnectionData connectionData, SelectedItem selectedItem, string entityName, string selection)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerSystemForm(conn, commonConfig, entityName, selection, selectedItem));
        }

        public void HandleOpenSystemFormOrganizationComparerCommand(ConnectionData connectionData1, ConnectionData connectionData2, string filter)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.OpenSystemFormOrganizationComparer(connectionData1, connectionData2, commonConfig, filter);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }
    }
}
