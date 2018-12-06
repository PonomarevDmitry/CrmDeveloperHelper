using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class CheckController
    {
        private const string tabSpacer = "    ";

        private IWriteToOutput _iWriteToOutput = null;

        public CheckController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Проверка имена на префикс.

        public async Task ExecuteCheckingEntitiesNames(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            prefix = prefix.TrimEnd(' ', '_').Trim();
            prefix = string.Format("{0}_", prefix);

            string operation = string.Format(Properties.OperationNames.CheckingCRMObjectsNamesForPrefixFormat2, connectionData?.Name, prefix);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingEntitiesNames(connectionData, commonConfig, prefix);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task CheckingEntitiesNames(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            List<string> wrongEntityNames = new List<string>();
            List<string> wrongEntityAttributes = new List<string>();
            List<string> wrongEntityRelationshipsManyToOne = new List<string>();
            List<string> wrongEntityRelationshipsManyToMany = new List<string>();

            var wrongWebResourceNames = new FormatTextTableHandler();
            wrongWebResourceNames.SetHeader("WebResourceType", "Name");

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    if (currentEntity.LogicalName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        wrongEntityNames.Add(currentEntity.LogicalName);
                    }

                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (currentAttribute.AttributeOf == null)
                        {
                            if (currentAttribute.LogicalName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                            {
                                wrongEntityAttributes.Add(string.Format("{0}.{1}", currentEntity.LogicalName, currentAttribute.LogicalName));
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToOneRelationships)
                    {
                        if (currentRelationship.SchemaName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        {
                            string name = string.Format("{0}.{1}", currentEntity.LogicalName, currentRelationship.SchemaName);

                            if (!wrongEntityRelationshipsManyToOne.Contains(name))
                            {
                                wrongEntityRelationshipsManyToOne.Add(name);
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToManyRelationships)
                    {
                        if (currentRelationship.SchemaName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        {
                            string name = string.Format("{0}.{1}", currentEntity.LogicalName, currentRelationship.SchemaName);

                            if (!wrongEntityRelationshipsManyToMany.Contains(name))
                            {
                                wrongEntityRelationshipsManyToMany.Add(name);
                            }
                        }
                    }
                }
            }

            {
                WebResourceRepository repositoryWebResource = new WebResourceRepository(service);

                var coll = await repositoryWebResource.GetListAllAsync(string.Empty);

                foreach (var webResource in coll)
                {
                    string name = webResource.Name;

                    if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        wrongWebResourceNames.AddLine(webResource.FormattedValues[WebResource.Schema.Attributes.webresourcetype], name);
                    }
                }
            }

            WriteToContentList(wrongEntityNames, content, "Entity names with prefix '" + prefix + "': {0}");

            WriteToContentList(wrongEntityAttributes, content, "Entity Attributes names with prefix '" + prefix + "': {0}");

            WriteToContentList(wrongEntityRelationshipsManyToOne, content, "Many to One Relationships names with prefix '" + prefix + "': {0}");

            WriteToContentList(wrongEntityRelationshipsManyToMany, content, "Many to Many Relationships names with prefix '" + prefix + "': {0}");

            WriteToContentList(wrongWebResourceNames.GetFormatedLines(true), content, "WebResounce names with prefix '" + prefix + "': {0}");

            int totalErrors =
                wrongEntityNames.Count
                + wrongEntityAttributes.Count
                + wrongEntityRelationshipsManyToOne.Count
                + wrongEntityRelationshipsManyToMany.Count
                + wrongWebResourceNames.Count
                ;

            if (totalErrors == 0)
            {
                content.AppendLine();
                content.AppendFormat("No Objects in CRM founded with prefix '{0}'.", prefix).AppendLine();
            }

            string filePath = string.Empty;

            if (content.Length > 0)
            {
                string fileName = EntityFileNameFormatter.GetCheckEntityNamesForPrefixFileName(connectionData.Name, prefix);

                filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoObjectsInCRMWereFounded);
            }
        }

        #endregion Проверка имена на префикс.

        #region Проверка имена на префикс и показ зависимых объектов.

        public async Task ExecuteCheckingEntitiesNamesAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            prefix = prefix.TrimEnd(' ', '_');
            prefix = string.Format("{0}_", prefix);

            string operation = string.Format(Properties.OperationNames.CheckingCRMObjectsNamesForPrefixAndShowDependentComponentsFormat2, connectionData?.Name, prefix);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingEntitiesNamesAndShowDependentComponents(connectionData, commonConfig, prefix);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task CheckingEntitiesNamesAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            Dictionary<string, string> wrongEntityNames = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Dictionary<string, string> wrongEntityAttributes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Dictionary<string, string> wrongEntityRelationshipsManyToOne = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Dictionary<string, string> wrongEntityRelationshipsManyToMany = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            Dictionary<string, string> wrongWebResourceNames = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            SolutionComponentDescriptor descriptor = new SolutionComponentDescriptor(service, true);

            DependencyRepository dependencyRepository = new DependencyRepository(service);

            DependencyDescriptionHandler descriptorHandler = new DependencyDescriptionHandler(descriptor);

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (currentAttribute.AttributeOf == null)
                        {
                            if (currentAttribute.LogicalName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                            {
                                string name = string.Format("{0}.{1}", currentEntity.LogicalName, currentAttribute.LogicalName);

                                var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.Attribute, currentAttribute.MetadataId.Value);

                                var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                                wrongEntityAttributes.Add(name, desc);
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToOneRelationships)
                    {
                        if (currentRelationship.SchemaName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        {
                            string name = string.Format("{0}.{1}", currentEntity.LogicalName, currentRelationship.SchemaName);

                            if (!wrongEntityRelationshipsManyToOne.ContainsKey(name))
                            {
                                var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.EntityRelationship, currentRelationship.MetadataId.Value);

                                var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                                wrongEntityRelationshipsManyToOne.Add(name, desc);
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToManyRelationships)
                    {
                        if (currentRelationship.SchemaName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        {
                            string name = string.Format("{0}.{1}", currentEntity.LogicalName, currentRelationship.SchemaName);

                            if (!wrongEntityRelationshipsManyToMany.ContainsKey(name))
                            {
                                var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.EntityRelationship, currentRelationship.MetadataId.Value);

                                var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                                wrongEntityRelationshipsManyToMany.Add(name, desc);
                            }
                        }
                    }

                    var wrongEntity = currentEntity.LogicalName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
                    if (wrongEntity)
                    {
                        var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.Entity, currentEntity.MetadataId.Value);

                        var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                        wrongEntityNames.Add(currentEntity.LogicalName, desc);
                    }
                }
            }

            {
                WebResourceRepository repositoryWebResource = new WebResourceRepository(service);

                var webResources = await repositoryWebResource.GetListAllAsync();

                foreach (var webResource in webResources)
                {
                    string name = webResource.Name;

                    if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        string longName = string.Format("'{1}' {0}", name, webResource.FormattedValues[WebResource.Schema.Attributes.webresourcetype]);

                        var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.WebResource, webResource.Id);

                        var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                        wrongWebResourceNames.Add(longName, desc);
                    }
                }
            }

            WriteToContentDictionary(content, wrongEntityNames, "Entity names with prefix '" + prefix + "': {0}");

            WriteToContentDictionary(content, wrongEntityAttributes, "Entity Attributes names with prefix '" + prefix + "': {0}");

            WriteToContentDictionary(content, wrongEntityRelationshipsManyToOne, "Many to One Relationships names with prefix '" + prefix + "': {0}");

            WriteToContentDictionary(content, wrongEntityRelationshipsManyToMany, "Many to Many Relationships names with prefix '" + prefix + "': {0}");

            WriteToContentDictionary(content, wrongWebResourceNames, "WebResounce names with prefix '" + prefix + "': {0}");

            int totalErrors = wrongEntityNames.Count
                + wrongEntityAttributes.Count
                + wrongEntityRelationshipsManyToOne.Count
                + wrongEntityRelationshipsManyToMany.Count
                + wrongWebResourceNames.Count
                ;

            if (totalErrors == 0)
            {
                content.AppendLine();
                content.AppendFormat("No Objects in CRM founded with prefix '{0}'.", prefix).AppendLine();
            }

            if (content.Length > 0)
            {
                string fileName = string.Format("{0}.CRM Objects names for prefix '{1}' and show dependent components at {2}.txt", connectionData.Name, prefix, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("Created file with CRM Objects names for prefix '{0}' and show dependent components: {1}", prefix, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("No information about web-resource dependent components.");
            }
        }

        #endregion Проверка имена на префикс и показ зависимых объектов.

        #region Проверка сущностей помеченных на удаление.

        public async Task ExecuteCheckingMarkedToDelete(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            string operation = string.Format(Properties.OperationNames.CheckingCRMObjectsMarkedToDeleteByAndShowDependentComponentsFormat2, connectionData?.Name, prefix);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingMarkedToDelete(connectionData, commonConfig, prefix);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task CheckingMarkedToDelete(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            Dictionary<string, string> wrongEntityNames = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Dictionary<string, string> wrongEntityAttributes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Dictionary<string, string> wrongEntityRelationshipsManyToOne = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Dictionary<string, string> wrongEntityRelationshipsManyToMany = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            Dictionary<string, string> wrongWebResourceNames = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            var descriptor = new SolutionComponentDescriptor(service, true);

            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            var dependencyRepository = new DependencyRepository(service);

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (currentAttribute.AttributeOf == null)
                        {
                            bool marked = IsMakedToDelete(prefix, currentAttribute.LogicalName, currentAttribute.DisplayName);

                            if (marked)
                            {
                                string name = string.Format("{0}.{1}", currentEntity.LogicalName, currentAttribute.LogicalName);

                                var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.Attribute, currentAttribute.MetadataId.Value);

                                var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                                wrongEntityAttributes.Add(name, desc);
                            }
                        }
                    }

                    var wrongEntity = IsMakedToDelete(prefix, currentEntity.LogicalName, currentEntity.DisplayName);

                    if (wrongEntity)
                    {
                        var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.Entity, currentEntity.MetadataId.Value);

                        var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                        wrongEntityNames.Add(currentEntity.LogicalName, desc);
                    }
                }
            }

            {
                WebResourceRepository repositoryWebResource = new WebResourceRepository(service);

                var collWebResources = await repositoryWebResource.GetListAllAsync(string.Empty);

                foreach (var webResource in collWebResources)
                {
                    string name = webResource.Name;

                    if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        string longName = string.Format("'{1}' {0}", name, webResource.FormattedValues[WebResource.Schema.Attributes.webresourcetype]);

                        var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.WebResource, webResource.Id);

                        var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                        wrongWebResourceNames.Add(longName, desc);
                    }
                }
            }

            WriteToContentDictionary(content, wrongEntityNames, "Entity names with prefix '" + prefix + "': {0}");

            WriteToContentDictionary(content, wrongEntityAttributes, "Entity Attributes names with prefix '" + prefix + "': {0}");

            WriteToContentDictionary(content, wrongEntityRelationshipsManyToOne, "Many to One Relationships names with prefix '" + prefix + "': {0}");

            WriteToContentDictionary(content, wrongEntityRelationshipsManyToMany, "Many to Many Relationships names with prefix '" + prefix + "': {0}");

            WriteToContentDictionary(content, wrongWebResourceNames, "WebResounce names with prefix '" + prefix + "': {0}");

            int totalErrors = wrongEntityAttributes.Count
                + wrongEntityNames.Count
                + wrongEntityRelationshipsManyToOne.Count
                + wrongEntityRelationshipsManyToMany.Count
                + wrongWebResourceNames.Count
                ;

            if (totalErrors == 0)
            {
                content.AppendLine();
                content.AppendFormat("No Objects in CRM founded with prefix '{0}'.", prefix).AppendLine();
            }

            if (content.Length > 0)
            {
                string fileName = string.Format("{0}.CRM Objects marked to delete by '{1}' and show dependent components at {2}.txt", connectionData.Name, prefix, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("Created file with CRM Objects marked to delete by '{0}' and show dependent components: {1}", prefix, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("No CRM Objects marked to delete by '{0}' and show dependent components.", prefix);
            }
        }

        private bool IsMakedToDelete(string prefix, string logicalName, Label label)
        {
            if (logicalName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (label != null)
            {
                foreach (var item in label.LocalizedLabels)
                {
                    if (item.Label.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion Проверка имена на префикс и показ зависимых объектов.

        #region Проверка уровня собственности сущностей.

        public async Task ExecuteCheckingEntitiesOwnership(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingCRMEntityOwnershipFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingEntitiesOwnership(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task CheckingEntitiesOwnership(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

            var allEntities = await repositoryEntity.GetEntitiesDisplayNameAsync();

            var groups = allEntities.GroupBy(ent => ent.OwnershipType).OrderBy(gr => gr.Key);

            foreach (var gr in groups)
            {
                content.AppendLine();

                int count = gr.Count();

                string name = "Null";

                if (gr.Key.HasValue)
                {
                    name = gr.Key.Value.ToString();
                }

                content.AppendFormat("Entities with Ownership {0}: {1}", name, count).AppendLine();

                gr.OrderBy(ent => ent.LogicalName).ToList().ForEach(ent => content.AppendLine(tabSpacer + ent.LogicalName));
            }

            string fileName = string.Format("{0}.Entities with Ownership at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (!Directory.Exists(commonConfig.FolderForExport))
            {
                Directory.CreateDirectory(commonConfig.FolderForExport);
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput("Created file with Entities with Ownership: {0}", filePath);

            this._iWriteToOutput.PerformAction(filePath);
        }

        #endregion Проверка уровня собственности сущностей.

        #region Проверка кодировки файлов.

        internal void ExecuteCheckingFilesEncoding(List<SelectedFile> selectedFiles)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CheckingFilesEncoding);

            try
            {
                CheckingFilesEncoding(this._iWriteToOutput, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CheckingFilesEncoding);
            }
        }

        public static void CheckingFilesEncoding(IWriteToOutput iWriteToOutput, IEnumerable<SelectedFile> selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding)
        {
            filesWithoutUTF8Encoding = new List<SelectedFile>();

            int countWithUTF8Encoding = 0;

            List<string> listNotExistsOnDisk = new List<string>();

            List<string> listNotHaveBOM = new List<string>();

            List<string> listWrongEncoding = new List<string>();

            List<string> listMultipleEncodingHasUTF8 = new List<string>();

            List<string> listMultipleEncodingHasNotUTF8 = new List<string>();

            foreach (var selectedFile in selectedFiles)
            {
                if (File.Exists(selectedFile.FilePath))
                {
                    var arrayFile = File.ReadAllBytes(selectedFile.FilePath);

                    var encodings = ContentCoparerHelper.GetFileEncoding(arrayFile);

                    if (encodings.Count > 0)
                    {
                        if (encodings.Count == 1)
                        {
                            if (encodings[0].CodePage == Encoding.UTF8.CodePage)
                            {
                                countWithUTF8Encoding++;
                            }
                            else
                            {
                                listWrongEncoding.Add(string.Format("{0} has encoding {1}", selectedFile.FriendlyFilePath, encodings[0].EncodingName));

                                filesWithoutUTF8Encoding.Add(selectedFile);
                            }
                        }
                        else
                        {
                            filesWithoutUTF8Encoding.Add(selectedFile);

                            bool hasUTF8 = false;

                            StringBuilder str = new StringBuilder();

                            foreach (var enc in encodings)
                            {
                                if (enc.CodePage == Encoding.UTF8.CodePage)
                                {
                                    hasUTF8 = true;
                                }

                                if (str.Length > 0)
                                {
                                    str.Append(", ");
                                    str.Append(enc.EncodingName);
                                }
                            }

                            if (hasUTF8)
                            {
                                listMultipleEncodingHasUTF8.Add(string.Format("{0} has encoding {1}", selectedFile.FriendlyFilePath, str.ToString()));
                            }
                            else
                            {
                                listMultipleEncodingHasNotUTF8.Add(string.Format("{0} has encoding {1}", selectedFile.FriendlyFilePath, str.ToString()));
                            }
                        }
                    }
                    else
                    {
                        listNotHaveBOM.Add(selectedFile.FriendlyFilePath);

                        filesWithoutUTF8Encoding.Add(selectedFile);
                    }
                }
                else
                {
                    listNotExistsOnDisk.Add(selectedFile.FilePath);
                }
            }

            if (listNotHaveBOM.Count > 0)
            {
                iWriteToOutput.WriteToOutput(string.Empty);
                iWriteToOutput.WriteToOutput("File does not have encoding: {0}", listNotHaveBOM.Count);

                listNotHaveBOM.Sort();

                listNotHaveBOM.ForEach(item => iWriteToOutput.WriteToOutput(item));
            }

            if (listWrongEncoding.Count > 0)
            {
                iWriteToOutput.WriteToOutput(string.Empty);
                iWriteToOutput.WriteToOutput("File has wrong Encoding: {0}", listWrongEncoding.Count);

                listWrongEncoding.Sort();

                listWrongEncoding.ForEach(item => iWriteToOutput.WriteToOutput(item));
            }

            if (listMultipleEncodingHasUTF8.Count > 0)
            {
                iWriteToOutput.WriteToOutput(string.Empty);
                iWriteToOutput.WriteToOutput("File complies multiple Encoding with UTF8 in list: {0}", listMultipleEncodingHasUTF8.Count);

                listMultipleEncodingHasUTF8.Sort();

                listMultipleEncodingHasUTF8.ForEach(item => iWriteToOutput.WriteToOutput(item));
            }

            if (listMultipleEncodingHasNotUTF8.Count > 0)
            {
                iWriteToOutput.WriteToOutput(string.Empty);
                iWriteToOutput.WriteToOutput("File complies multiple Encoding WITHOUT UTF8 in list: {0}", listMultipleEncodingHasNotUTF8.Count);

                listMultipleEncodingHasNotUTF8.Sort();

                listMultipleEncodingHasNotUTF8.ForEach(item => iWriteToOutput.WriteToOutput(item));
            }

            if (listNotExistsOnDisk.Count > 0)
            {
                iWriteToOutput.WriteToOutput(string.Empty);
                iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, listNotExistsOnDisk.Count);

                listNotExistsOnDisk.Sort();

                listNotExistsOnDisk.ForEach(item => iWriteToOutput.WriteToOutput(item));
            }

            if (countWithUTF8Encoding > 0)
            {
                if (countWithUTF8Encoding == selectedFiles.Count())
                {
                    iWriteToOutput.WriteToOutput(string.Empty);
                    iWriteToOutput.WriteToOutput("All Files has UTF8 Encoding: {0}", countWithUTF8Encoding);
                }
                else
                {
                    iWriteToOutput.WriteToOutput(string.Empty);
                    iWriteToOutput.WriteToOutput("Files has UTF8 Encoding: {0}", countWithUTF8Encoding);
                }
            }
        }

        public void ExecuteOpenFilesWithoutUTF8Encoding(IEnumerable<SelectedFile> selectedFiles)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.OpeningFilesWithoutUTF8Encoding);

            try
            {
                CheckingFilesEncoding(this._iWriteToOutput, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                if (filesWithoutUTF8Encoding.Count > 0)
                {
                    foreach (var item in filesWithoutUTF8Encoding)
                    {
                        this._iWriteToOutput.WriteToOutputFilePathUri(item.FilePath);
                        this._iWriteToOutput.OpenFileInVisualStudio(item.FilePath);
                    }
                }
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.OpeningFilesWithoutUTF8Encoding);
            }
        }

        #endregion Проверка кодировки файлов.

        #region Отображение зависимых компонентов веб-ресурсов.

        public async Task ExecuteShowingWebResourcesDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CheckingCRMObjectsNamesAndShowDependentComponents);

            try
            {
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OperationNames.CheckingFilesEncoding);

                    CheckController.CheckingFilesEncoding(this._iWriteToOutput, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                }

                await ShowingWebResourcesDependentComponents(connectionData, commonConfig, selectedFiles);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CheckingCRMObjectsNamesAndShowDependentComponents);
            }
        }

        private async Task ShowingWebResourcesDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            var descriptor = new SolutionComponentDescriptor(service, true);

            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            var dependencyRepository = new DependencyRepository(service);

            bool isconnectionDataDirty = false;

            List<string> listNotExistsOnDisk = new List<string>();
            List<string> listNotFoundedInCRMNoLink = new List<string>();
            List<string> listLastLinkEqualByContent = new List<string>();

            Dictionary<string, string> webResourceDescriptions = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            WebResourceRepository repositoryWebResource = new WebResourceRepository(service);

            FormatTextTableHandler tableWithoutDependenComponents = new FormatTextTableHandler();
            tableWithoutDependenComponents.SetHeader("FilePath", "Web Resource Name", "Web Resource Type");

            var groups = selectedFiles.GroupBy(sel => sel.Extension);

            foreach (var gr in groups)
            {
                var names = gr.Select(sel => sel.FriendlyFilePath).ToArray();

                var dict = repositoryWebResource.FindMultiple(gr.Key, names);

                foreach (var selectedFile in gr)
                {
                    if (File.Exists(selectedFile.FilePath))
                    {
                        string name = selectedFile.FriendlyFilePath.ToLower();

                        var webresource = WebResourceRepository.FindWebResourceInDictionary(dict, name, gr.Key);

                        if (webresource == null)
                        {
                            Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                            if (webId.HasValue)
                            {
                                webresource = await repositoryWebResource.FindByIdAsync(webId.Value);

                                if (webresource != null)
                                {
                                    listLastLinkEqualByContent.Add(selectedFile.FriendlyFilePath);
                                }
                            }
                        }

                        if (webresource != null)
                        {
                            // Запоминается файл
                            isconnectionDataDirty = true;
                            connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                            string webName = webresource.Name;

                            string longName = string.Format("{0}    and    '{1}'    '{2}'", selectedFile.FriendlyFilePath, webName, webresource.FormattedValues[WebResource.Schema.Attributes.webresourcetype]);

                            var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.WebResource, webresource.Id);

                            var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                            if (!string.IsNullOrEmpty(desc))
                            {
                                webResourceDescriptions.Add(longName, desc);
                            }
                            else
                            {
                                tableWithoutDependenComponents.AddLine(selectedFile.FriendlyFilePath, webName, "'" + webresource.FormattedValues[WebResource.Schema.Attributes.webresourcetype] + "'");
                            }
                        }
                        else
                        {
                            connectionData.RemoveMapping(selectedFile.FriendlyFilePath);

                            listNotFoundedInCRMNoLink.Add(selectedFile.FriendlyFilePath);
                        }
                    }
                    else
                    {
                        listNotExistsOnDisk.Add(selectedFile.FilePath);
                    }
                }
            }

            if (isconnectionDataDirty)
            {
                //Сохранение настроек после публикации
                connectionData.ConnectionConfiguration.Save();
            }

            WriteToContentList(listNotFoundedInCRMNoLink, content, "File NOT FOUNDED in CRM: {0}");

            WriteToContentList(listLastLinkEqualByContent, content, "Files NOT FOUNDED in CRM, but has Last Link: {0}");

            WriteToContentList(listNotExistsOnDisk, content, Properties.OutputStrings.FileNotExistsFormat1);

            WriteToContentList(tableWithoutDependenComponents.GetFormatedLines(true), content, "Files without dependent components: {0}");

            WriteToContentDictionary(content, webResourceDescriptions, "WebResource dependent components: {0}");

            if (content.Length > 0)
            {
                string fileName = string.Format("{0}.WebResourceDependent at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("Created file with web-resources dependent components: {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("No information about web-resource dependent components.");
            }
        }

        #endregion Отображение зависимых компонентов веб-ресурсов.

        #region Проверка глобальных OptionSet на дубликаты на сущности.

        public async Task ExecuteCheckingGlobalOptionSetDuplicates(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingGlobalOptionSetDuplicatesOnEntityFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingGlobalOptionSetDuplicates(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task CheckingGlobalOptionSetDuplicates(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            var entityMetadataSource = new SolutionComponentMetadataSource(service);

            var descriptor = new SolutionComponentDescriptor(service, true, entityMetadataSource);
            var dependencyRepository = new DependencyRepository(service);
            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            RetrieveAllOptionSetsRequest request = new RetrieveAllOptionSetsRequest();

            RetrieveAllOptionSetsResponse response = (RetrieveAllOptionSetsResponse)service.Execute(request);

            bool hasInfo = false;

            foreach (var optionSet in response.OptionSetMetadata.OfType<OptionSetMetadata>().OrderBy(e => e.Name))
            {
                var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.OptionSet, optionSet.MetadataId.Value);

                if (coll.Any())
                {
                    var filter = coll
                        .Where(c => c.DependentComponentType.Value == (int)ComponentType.Attribute)
                        .Select(c => new { Dependency = c, Attribute = entityMetadataSource.GetAttributeMetadata(c.DependentComponentObjectId.Value) })
                        .Where(c => c.Attribute != null)
                        .GroupBy(c => c.Attribute.EntityLogicalName)
                        .Where(gr => gr.Count() > 1)
                        .SelectMany(gr => gr.Select(c => c.Dependency))
                        .ToList()
                        ;

                    if (filter.Any())
                    {
                        var desc = await descriptorHandler.GetDescriptionDependentAsync(filter);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            if (content.Length > 0)
                            {
                                content
                                    .AppendLine(new string('-', 150))
                                    .AppendLine();
                            }

                            hasInfo = true;

                            content.AppendFormat("Global OptionSet Name {0}       IsCustomOptionSet {1}      IsManaged {2}", optionSet.Name, optionSet.IsCustomOptionSet, optionSet.IsManaged).AppendLine();

                            content.AppendLine(desc);
                        }
                    }
                }
            }

            if (!hasInfo)
            {
                content.AppendLine("No duplicates were found.");
            }

            if (content.Length > 0)
            {
                string fileName = string.Format("{0}.Checking Global OptionSet Duplicates on Entity at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("Created file with Checking Global OptionSet Duplicates on Entity: {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
        }

        #endregion Проверка глобальных OptionSet на дубликаты на сущности.

        #region Поиск элементов сущности с именем.

        public async Task ExecuteFindEntityElementsByName(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsNamesFormat2, connectionData?.Name, name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await FindindEntityElementsByName(connectionData, commonConfig, name);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task FindindEntityElementsByName(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            List<string> listEntityAttributes = new List<string>();
            List<string> listEntityRelationshipsManyToOne = new List<string>();
            List<string> listEntityRelationshipsManyToMany = new List<string>();

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (currentAttribute.AttributeOf == null)
                        {
                            if (string.Equals(currentAttribute.LogicalName, name, StringComparison.OrdinalIgnoreCase))
                            {
                                listEntityAttributes.Add(string.Format("{0}.{1}", currentEntity.LogicalName, currentAttribute.LogicalName));
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToOneRelationships)
                    {
                        if (string.Equals(currentRelationship.SchemaName, name, StringComparison.OrdinalIgnoreCase))
                        {
                            string elementName = string.Format("{0}.{1}", currentEntity.LogicalName, currentRelationship.SchemaName);

                            if (!listEntityRelationshipsManyToOne.Contains(elementName))
                            {
                                listEntityRelationshipsManyToOne.Add(elementName);
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToManyRelationships)
                    {
                        if (string.Equals(currentRelationship.SchemaName, name, StringComparison.OrdinalIgnoreCase))
                        {
                            string elementName = string.Format("{0}.{1}", currentEntity.LogicalName, currentRelationship.SchemaName);

                            if (!listEntityRelationshipsManyToMany.Contains(elementName))
                            {
                                listEntityRelationshipsManyToMany.Add(elementName);
                            }
                        }
                    }
                }
            }

            WriteToContentList(listEntityAttributes, content, "Entity Attributes names with name '" + name + "': {0}");

            WriteToContentList(listEntityRelationshipsManyToOne, content, "Many to One Relationships names with name '" + name + "': {0}");

            WriteToContentList(listEntityRelationshipsManyToMany, content, "Many to Many Relationships names with name '" + name + "': {0}");

            int totalErrors =
                listEntityAttributes.Count
                + listEntityRelationshipsManyToOne.Count
                + listEntityRelationshipsManyToMany.Count
                ;

            if (totalErrors == 0)
            {
                content.AppendLine();
                content.AppendFormat("No Objects in CRM founded with name '{0}'.", name).AppendLine();
            }

            string filePath = string.Empty;

            if (content.Length > 0)
            {
                string fileName = string.Format("{0}.Finding CRM Objects names for {1} at {2}.txt"
                , connectionData.Name
                , name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

                filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoObjectsInCRMWereFounded);
            }
        }

        #endregion Поиск элементов сущности с именем.

        #region Поиск элементов, содержащих строку.

        public async Task ExecuteFindEntityElementsContainsString(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectscontainsNameFormat2, connectionData?.Name, name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await FindindEntityElementsContainsString(connectionData, commonConfig, name);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task FindindEntityElementsContainsString(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            List<string> listEntityAttributes = new List<string>();
            List<string> listEntityRelationshipsManyToOne = new List<string>();
            List<string> listEntityRelationshipsManyToMany = new List<string>();

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (currentAttribute.AttributeOf == null)
                        {
                            if (Regex.IsMatch(currentAttribute.LogicalName, name, RegexOptions.IgnoreCase))
                            {
                                listEntityAttributes.Add(string.Format("{0}.{1}", currentEntity.LogicalName, currentAttribute.LogicalName));
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToOneRelationships)
                    {
                        if (Regex.IsMatch(currentRelationship.SchemaName, name, RegexOptions.IgnoreCase))
                        {
                            string elementName = string.Format("{0}.{1}", currentEntity.LogicalName, currentRelationship.SchemaName);

                            if (!listEntityRelationshipsManyToOne.Contains(elementName))
                            {
                                listEntityRelationshipsManyToOne.Add(elementName);
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToManyRelationships)
                    {
                        if (Regex.IsMatch(currentRelationship.SchemaName, name, RegexOptions.IgnoreCase))
                        {
                            string elementName = string.Format("{0}.{1}", currentEntity.LogicalName, currentRelationship.SchemaName);

                            if (!listEntityRelationshipsManyToMany.Contains(elementName))
                            {
                                listEntityRelationshipsManyToMany.Add(elementName);
                            }
                        }
                    }
                }
            }

            WriteToContentList(listEntityAttributes, content, "Entity Attributes names contains '" + name + "': {0}");

            WriteToContentList(listEntityRelationshipsManyToOne, content, "Many to One Relationships names contains '" + name + "': {0}");

            WriteToContentList(listEntityRelationshipsManyToMany, content, "Many to Many Relationships names contains '" + name + "': {0}");

            int totalErrors =
                listEntityAttributes.Count
                + listEntityRelationshipsManyToOne.Count
                + listEntityRelationshipsManyToMany.Count
                ;

            if (totalErrors == 0)
            {
                content.AppendLine();
                content.AppendFormat("No Objects in CRM founded that contains '{0}'.", name).AppendLine();
            }

            string filePath = string.Empty;

            if (content.Length > 0)
            {
                string fileName = string.Format("{0}.Finding CRM Objects names contains {1} at {2}.txt"
                , connectionData.Name
                , name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

                filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoObjectsInCRMWereFounded);
            }
        }

        #endregion Поиск элементов, содержащих строку.

        #region Поиск элементов по идентификатору.

        public async Task ExecuteFindEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsByIdEntityNameEntityTypeCodeFormat4, connectionData?.Name, entityId, entityName, entityTypeCode);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await FindEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task FindEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            EntityMetadataRepository repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.GetEntitiesPropertiesAsync(entityName, entityTypeCode, "LogicalName", "PrimaryIdAttribute", "IsIntersect", "Attributes");

            bool finded = false;

            foreach (var item in entityMetadataList.OrderBy(e => e.LogicalName))
            {
                var primaryAttr = item.Attributes.FirstOrDefault(a => string.Equals(a.LogicalName, item.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase));

                if (primaryAttr != null && primaryAttr.AttributeType == AttributeTypeCode.Uniqueidentifier)
                {
                    var generalRepository = new GenericRepository(service, item);

                    Entity entity = await generalRepository.GetEntityByIdAsync(entityId, new ColumnSet(true));

                    if (entity != null)
                    {
                        finded = true;

                        content
                            .AppendLine()
                            .AppendLine()
                            .AppendLine(new string('-', 150))
                            .AppendLine()
                            .AppendLine()
                            .AppendLine(await EntityDescriptionHandler.GetEntityDescriptionAsync(entity, null, connectionData))
                            ;
                    }
                }
            }

            if (finded)
            {
                string fileName = EntityFileNameFormatter.GetFindingCRMObjectsByIdFileName(connectionData.Name, entityId);

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));
                
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);
                
                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoObjectsInCRMWereFounded);
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        #endregion Поиск элементов по идентификатору.

        #region Поиск элементов по любому Guid.

        public async Task ExecuteFindEntityByUniqueidentifier(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsByUniqueidentifierEntityNameEntityTypeCodeFormat4, connectionData?.Name, entityId, entityName, entityTypeCode);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await FindEntityByUniqueidentifier(connectionData, commonConfig, entityName, entityTypeCode, entityId);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task FindEntityByUniqueidentifier(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            EntityMetadataRepository repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.GetEntitiesPropertiesAsync(entityName, entityTypeCode, "LogicalName", "PrimaryIdAttribute", "IsIntersect", "Attributes");

            bool finded = false;

            foreach (var item in entityMetadataList.OrderBy(e => e.LogicalName))
            {
                foreach (var field in item.Attributes.Where(a => a.AttributeType == AttributeTypeCode.Uniqueidentifier).OrderBy(a => a.LogicalName))
                {
                    var generalRepository = new GenericRepository(service, item);

                    var entityList = await generalRepository.GetEntitiesByFieldAsync(field.LogicalName, entityId, new ColumnSet(true));

                    if (entityList != null)
                    {
                        foreach (var entity in entityList)
                        {
                            finded = true;

                            content
                                .AppendLine()
                                .AppendLine()
                                .AppendLine(new string('-', 150))
                                .AppendLine()
                                .AppendLine()
                                .AppendLine(await EntityDescriptionHandler.GetEntityDescriptionAsync(entity, null, connectionData))
                                ;
                        }
                    }
                }
            }

            if (finded)
            {
                string fileName = EntityFileNameFormatter.GetFindingCRMObjectsByUniqueidentifierFileName(connectionData.Name, entityId);

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoObjectsInCRMWereFounded);
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        #endregion Поиск элементов по любому Guid.

        #region Поиск неизвестных типов компонентов.

        public async Task ExecuteCheckingComponentTypeEnum(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingComponentTypeEnumFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingComponentTypeEnum(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task CheckingComponentTypeEnum(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            var hash = new HashSet<Tuple<int, Guid>>();

            {
                var repository = new SolutionComponentRepository(service);

                var components = await repository.GetDistinctSolutionComponentsAsync();

                foreach (var item in components.Where(en => en.ComponentType != null && en.ObjectId.HasValue))
                {
                    if (!item.IsDefinedComponentType())
                    {
                        hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value));
                    }
                }
            }

            {
                var repository = new DependencyNodeRepository(service);

                var components = await repository.GetDistinctListUnknownComponentTypeAsync();

                foreach (var item in components.Where(en => en.ComponentType != null && en.ObjectId.HasValue))
                {
                    if (!item.IsDefinedComponentType())
                    {
                        hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value));
                    }
                }
            }

            {
                var repository = new InvalidDependencyRepository(service);

                var components = await repository.GetDistinctListAsync();

                foreach (var item in components.Where(en => en.MissingComponentType != null && en.MissingComponentId.HasValue))
                {
                    if (!item.IsDefinedMissingComponentType())
                    {
                        hash.Add(Tuple.Create(item.MissingComponentType.Value, item.MissingComponentId.Value));
                    }
                }

                foreach (var item in components.Where(en => en.ExistingComponentType != null && en.ExistingComponentId.HasValue))
                {
                    if (!item.IsDefinedExistingComponentType())
                    {
                        hash.Add(Tuple.Create(item.ExistingComponentType.Value, item.ExistingComponentId.Value));
                    }
                }
            }

            if (hash.Any())
            {
                var groups = hash.GroupBy(e => e.Item1);

                content.AppendLine().AppendLine();

                content.AppendFormat("ComponentTypes not founded in Enum: {0}", groups.Count());

                foreach (var gr in groups.OrderBy(e => e.Key))
                {
                    content.AppendLine().AppendLine();

                    foreach (var item in gr.OrderBy(e => e.Item2))
                    {
                        content.AppendFormat(tabSpacer + item.Item1.ToString() + tabSpacer + item.Item2.ToString()).AppendLine();
                    }
                }

                string fileName = string.Format("{0}.Checking ComponentType Enum at {1}.txt"
                    , connectionData.Name
                    , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("New ComponentTypes were exported to {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("No New ComponentTypes in CRM were founded.");
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        #endregion Поиск неизвестных типов компонентов.

        #region Описание всех dependencynode.

        public async Task ExecuteCreatingAllDependencyNodesDescription(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CreatingAllDependencyNodesDescriptionFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CreatingAllDependencyNodesDescription(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task CreatingAllDependencyNodesDescription(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            var hash = new HashSet<Tuple<int, Guid>>();

            {
                var repository = new DependencyNodeRepository(service);

                var components = await repository.GetDistinctListAsync();

                foreach (var item in components.Where(en => en.ComponentType != null && en.ObjectId.HasValue))
                {
                    hash.Add(Tuple.Create(item.ComponentType.Value, item.ObjectId.Value));
                }
            }

            if (hash.Any())
            {
                content.AppendLine().AppendLine();

                var solutionComponents = hash.Select(e => new SolutionComponent
                {
                    ComponentType = new OptionSetValue(e.Item1),
                    ObjectId = e.Item2,
                }).ToList();

                var descriptor = new SolutionComponentDescriptor(service, true);

                descriptor.MetadataSource.DownloadEntityMetadata();

                var desc = await descriptor.GetSolutionComponentsDescriptionAsync(solutionComponents);

                content.AppendLine(desc);

                string fileName = string.Format(
                    "{0}.Dependency Nodes Description at {1}.txt"
                    , connectionData.Name
                    , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(commonConfig.FolderForExport);
                }

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("Dependency Nodes Description were exported to {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
        }

        #endregion Описание всех dependencynode.

        public async Task ExecuteCheckingWorkflowsUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingWorkflowsUsedEntitiesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingWorkflowsUsedEntities(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task CheckingWorkflowsUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            string fileName = string.Format("{0}.Workflows Used Entities at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (!Directory.Exists(commonConfig.FolderForExport))
            {
                Directory.CreateDirectory(commonConfig.FolderForExport);
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, new SolutionComponentDescriptor(service, true));

            var stringBuider = new StringBuilder();

            await workflowDescriptor.GetDescriptionWithUsedEntitiesInAllWorkflowsAsync(stringBuider);

            File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput("Created file with Workflows Used Entities: {0}", filePath);

            this._iWriteToOutput.PerformAction(filePath);
        }

        public async Task ExecuteCheckingWorkflowsNotExistingUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingWorkflowsUsedNotExistingEntitiesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingWorkflowsNotExistingUsedEntities(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task CheckingWorkflowsNotExistingUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            content.AppendLine(this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            string fileName = string.Format("{0}.Workflows Used Not Existing Entities at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (!Directory.Exists(commonConfig.FolderForExport))
            {
                Directory.CreateDirectory(commonConfig.FolderForExport);
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, new SolutionComponentDescriptor(service, true));

            var stringBuider = new StringBuilder();

            await workflowDescriptor.GetDescriptionWithUsedNotExistsEntitiesInAllWorkflowsAsync(stringBuider);

            File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput("Created file with Workflows Used Not Existing Entities: {0}", filePath);

            this._iWriteToOutput.PerformAction(filePath);
        }

        private void WriteToContentDictionary(StringBuilder content, Dictionary<string, string> dict, string formatList)
        {
            if (dict.Count > 0)
            {
                if (content.Length > 0) { content.AppendLine(); }

                content.AppendFormat(formatList, dict.Count).AppendLine();

                var query = dict.Keys.OrderBy(s => s);

                foreach (var key in query)
                {
                    if (content.Length > 0) { content.AppendLine(); }

                    content.AppendLine(new string('-', 150));

                    var item = dict[key];

                    content.AppendLine(tabSpacer + key);

                    if (!string.IsNullOrEmpty(item))
                    {
                        content.AppendLine(tabSpacer + tabSpacer + item);
                    }
                }
            }
        }

        private static void WriteToContentList(List<string> list, StringBuilder content, string formatList)
        {
            if (list.Count > 0)
            {
                if (content.Length > 0) { content.AppendLine(); }

                content.AppendFormat(formatList, list.Count).AppendLine();

                list.Sort();

                list.ForEach(item => content.AppendLine(tabSpacer + item));
            }
        }
    }
}