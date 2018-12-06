using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class CheckPluginController
    {
        private const string tabSpacer = "    ";

        private IWriteToOutput _iWriteToOutput = null;

        public CheckPluginController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        private void DescribeImages(StringBuilder builder, int stepNumber, bool hasDuplicates, IEnumerable<IGrouping<string, Entities.SdkMessageProcessingStepImage>> images, string header)
        {
            if (hasDuplicates)
            {
                builder.AppendLine(header);

                int duplicateNumber = 1;

                foreach (var gr in images)
                {
                    builder.AppendFormat(tabSpacer + "{0}.{1}. {2}", stepNumber, duplicateNumber, gr.Key).AppendLine();

                    int imageNumber = 1;

                    foreach (var image in gr)
                    {
                        string imageDescription = GetImageDescription(string.Format("{0}.{1}.{2}", stepNumber, duplicateNumber, imageNumber), image);

                        var coll = imageDescription.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var item in coll)
                        {
                            builder.AppendLine(tabSpacer + item);
                        }

                        imageNumber++;
                    }

                    duplicateNumber++;
                }
            }
        }

        private string GetImageDescription(string imageNumber, Entities.SdkMessageProcessingStepImage image)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0}. ", imageNumber);

            if (image.ImageType != null)
            {
                if (image.ImageType.Value == 0)
                {
                    result.Append("PreImage");
                }
                else if (image.ImageType.Value == 1)
                {
                    result.Append("PostImage");
                }
                else if (image.ImageType.Value == 2)
                {
                    result.Append("BothImage");
                }
            }

            if (!string.IsNullOrEmpty(image.EntityAlias))
            {
                result.AppendFormat(",   EntityAlias '{0}'", image.EntityAlias);
            }

            if (!string.IsNullOrEmpty(image.Name))
            {
                result.AppendFormat(",   Name '{0}'", image.EntityAlias);
            }

            result.AppendLine();

            if (!string.IsNullOrEmpty(image.Attributes1))
            {
                result.AppendFormat("Attributes: {0}", image.Attributes1StringsSorted).AppendLine();
            }
            else
            {
                result.AppendLine("Attributes: All");
            }

            return result.ToString();
        }

        #region Проверка образов шагов плагинов на дубликаты.

        public async Task ExecuteCheckingPluginImages(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingPluginImagesDuplicatesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingPluginImages(connectionData, commonConfig);
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

        private async Task CheckingPluginImages(ConnectionData connectionData, CommonConfiguration commonConfig)
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

            var repository = new PluginSearchRepository(service);

            var search = await repository.FindAllAsync(null, string.Empty, string.Empty, string.Empty);

            var querySteps = search.SdkMessageProcessingStep
                            .OrderBy(ent => ent.EventHandler?.Name ?? "Unknown")
                            .ThenBy(ent => ent.PrimaryObjectTypeCodeName)
                            .ThenBy(ent => ent.SdkMessageId?.Name ?? "Unknown", new MessageComparer())
                            .ThenBy(ent => ent.Stage.Value)
                            .ThenBy(ent => ent.Mode.Value)
                            .ThenBy(ent => ent.Rank)
                            .ThenBy(ent => ent.Name)
                            ;

            int stepNumber = 1;

            bool hasInfo = false;

            foreach (var step in querySteps)
            {
                var queryImage = from image in search.SdkMessageProcessingStepImage
                                 where image.SdkMessageProcessingStepId != null
                                 where image.SdkMessageProcessingStepId.Id == step.Id
                                 orderby image.ImageType.Value, image.CreatedOn, image.Name
                                 select image;

                var preImages = queryImage.Where(im => im.ImageType.Value == 0 || im.ImageType.Value == 2);
                var postImages = queryImage.Where(im => im.ImageType.Value == 1 || im.ImageType.Value == 2);

                var preImagesByEntityAlias = preImages.GroupBy(im => im.EntityAlias).Where(gr => gr.Count() > 1);
                var preImagesByName = preImages.GroupBy(im => im.EntityAlias).Where(gr => gr.Count() > 1);

                var postImagesByEntityAlias = postImages.GroupBy(im => im.EntityAlias).Where(gr => gr.Count() > 1);
                var postImagesByName = postImages.GroupBy(im => im.EntityAlias).Where(gr => gr.Count() > 1);

                var hasDuplicatesPreImagesByEntityAlias = preImagesByEntityAlias.Count() > 0;
                var hasDuplicatesPreImagesByName = preImagesByName.Count() > 0;

                var hasDuplicatesPostImagesByEntityAlias = postImagesByEntityAlias.Count() > 0;
                var hasDuplicatesPostImagesByName = postImagesByName.Count() > 0;

                if (hasDuplicatesPreImagesByEntityAlias
                    || hasDuplicatesPreImagesByName
                    || hasDuplicatesPostImagesByEntityAlias
                    || hasDuplicatesPostImagesByName
                    )
                {
                    if (content.Length > 0)
                    {
                        content.AppendLine().AppendLine();
                    }

                    hasInfo = true;

                    content.AppendFormat("{0}. {1}", stepNumber, step.EventHandler?.Name ?? "Unknown").AppendLine();

                    content.AppendFormat("Entity '{0}',   Message '{1}',   Stage '{2}',   Rank {3},   Statuscode {4}"
                        , step.PrimaryObjectTypeCodeName
                        , step.SdkMessageId?.Name ?? "Unknown"
                        , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                        , step.Rank.ToString()
                        , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                        ).AppendLine();

                    DescribeImages(content, stepNumber, hasDuplicatesPreImagesByEntityAlias, preImagesByEntityAlias, "Pre images duplicates by EntityAlias:");

                    DescribeImages(content, stepNumber, hasDuplicatesPreImagesByName, preImagesByName, "Pre images duplicates by Name:");

                    DescribeImages(content, stepNumber, hasDuplicatesPostImagesByEntityAlias, postImagesByEntityAlias, "Post images duplicates by EntityAlias:");

                    DescribeImages(content, stepNumber, hasDuplicatesPostImagesByName, postImagesByName, "Post images duplicates by Name:");

                    stepNumber++;
                }
            }

            if (!hasInfo)
            {
                content.AppendLine(this._iWriteToOutput.WriteToOutput("No duplicates were found."));
            }

            string fileName = string.Format("{0}.Checking Plugin Images Duplicates at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (!Directory.Exists(commonConfig.FolderForExport))
            {
                Directory.CreateDirectory(commonConfig.FolderForExport);
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput("Created file with Checking Plugin Images Duplicates: {0}", filePath);

            this._iWriteToOutput.PerformAction(filePath);
        }

        #endregion Проверка образов шагов плагинов на дубликаты.

        #region Проверка шагов плагинов на дубликаты.

        public async Task ExecuteCheckingPluginSteps(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingPluginStepsDuplicatesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingPluginSteps(connectionData, commonConfig);
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

        private async Task CheckingPluginSteps(ConnectionData connectionData, CommonConfiguration commonConfig)
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

            var repository = new PluginSearchRepository(service);

            var search = await repository.FindAllAsync(null, string.Empty, string.Empty, string.Empty);

            var querySteps = search.SdkMessageProcessingStep
                            .OrderBy(ent => ent.EventHandler?.Name ?? "Unknown")
                            .ThenBy(ent => ent.PrimaryObjectTypeCodeName)
                            .ThenBy(ent => ent.SdkMessageId?.Name ?? "Unknown", new MessageComparer())
                            .ThenBy(ent => ent.Stage.Value)
                            .ThenBy(ent => ent.Mode.Value)
                            .ThenBy(ent => ent.Rank)
                            .ThenBy(ent => ent.Name)
                            ;

            var pluginTypesWithConflicts = querySteps.GroupBy(step => new
            {
                EventHandlerId = step.EventHandler.Id,
                Stage = step.Stage.Value,

                EntityName = step.PrimaryObjectTypeCodeName,
                Message = step.SdkMessageId?.Name ?? "Unknown",

                EventHandlerName = step.EventHandler?.Name ?? "Unknown",
                step.Configuration,
            }).Where(gr => gr.Count() > 1);

            int pluginTypeNumber = 1;

            bool hasInfo = false;

            foreach (var gr in pluginTypesWithConflicts)
            {
                hasInfo = true;

                if (content.Length > 0)
                {
                    content.AppendLine().AppendLine();
                }

                content.AppendFormat("{0}. {1}", pluginTypeNumber, gr.Key.EventHandlerName).AppendLine();

                content.AppendFormat("Entity '{0}',   Message '{1}',   Stage '{2}'"
                    , gr.Key.EntityName
                    , gr.Key.Message
                    , SdkMessageProcessingStepRepository.GetStageName(gr.Key.Stage, null)
                    ).AppendLine();

                if (!string.IsNullOrEmpty(gr.Key.Configuration))
                {
                    content.AppendFormat("Configuration: {0}", gr.Key.Configuration).AppendLine();
                }

                foreach (var step in gr)
                {
                    content.AppendFormat("Stage '{0}',   Rank {1},   Statuscode {2}"
                        , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                        , step.Rank.ToString()
                        , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                        ).AppendLine();

                    var queryImage = from image in search.SdkMessageProcessingStepImage
                                     where image.SdkMessageProcessingStepId != null
                                     where image.SdkMessageProcessingStepId.Id == step.Id
                                     orderby image.ImageType.Value, image.CreatedOn, image.Name
                                     select image;

                    int numberImage = 1;

                    foreach (var image in queryImage)
                    {
                        string imageDescription = GetImageDescription(numberImage.ToString(), image);

                        var coll = imageDescription.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var item in coll)
                        {
                            content.AppendLine(tabSpacer + tabSpacer + item);
                        }

                        numberImage++;
                    }
                }

                pluginTypeNumber++;
            }

            if (!hasInfo)
            {
                content.AppendLine(this._iWriteToOutput.WriteToOutput("No duplicates were found."));
            }

            string fileName = string.Format("{0}.Checking Plugin Steps Duplicates at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (!Directory.Exists(commonConfig.FolderForExport))
            {
                Directory.CreateDirectory(commonConfig.FolderForExport);
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput("Created file with Checking Plugin Steps Duplicates: {0}", filePath);

            this._iWriteToOutput.PerformAction(filePath);
        }

        #endregion Проверка шагов плагинов на дубликаты.

        #region Проверка шагов плагинов на необходимые компоненты.

        public async Task ExecuteCheckingPluginStepsRequiredComponents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingPluginStepsRequiredComponentsFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingPluginStepsRequiredComponents(connectionData, commonConfig);
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

        private async Task CheckingPluginStepsRequiredComponents(ConnectionData connectionData, CommonConfiguration commonConfig)
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

            var repository = new PluginSearchRepository(service);

            var search = await repository.FindAllAsync(new List<PluginStage>(), string.Empty, string.Empty, string.Empty);

            var querySteps = search.SdkMessageProcessingStep
                            .OrderBy(ent => ent.EventHandler.Name)
                            .ThenBy(ent => ent.PrimaryObjectTypeCodeName)
                            .ThenBy(ent => ent.SdkMessageId.Name, new MessageComparer())
                            .ThenBy(ent => ent.Stage.Value)
                            .ThenBy(ent => ent.Mode.Value)
                            .ThenBy(ent => ent.Rank)
                            .ThenBy(ent => ent.Name)
                            ;

            EntityMetadataRepository repositoryMetadata = new EntityMetadataRepository(service);

            DependencyRepository dependencyRepository = new DependencyRepository(service);

            var listMetaData = await repositoryMetadata.GetEntitiesWithAttributesAsync();

            var dictEntity = new Dictionary<Guid, EntityMetadata>();
            var dictAttribute = new Dictionary<Guid, AttributeMetadata>();

            bool hasInfo = false;

            foreach (var metaEntity in listMetaData)
            {
                dictEntity.Add(metaEntity.MetadataId.Value, metaEntity);

                foreach (var metaAttribute in metaEntity.Attributes)
                {
                    dictAttribute.Add(metaAttribute.MetadataId.Value, metaAttribute);
                }
            }

            foreach (var step in querySteps)
            {
                var listRequired = await dependencyRepository.GetRequiredComponentsAsync((int)ComponentType.SdkMessageProcessingStep, step.Id);

                var stepEntities = GetSetEntites(step);
                var stepAttributes = GetSetStepAttributes(step);

                var componentsEntities = GetSetComponentsEntites(listRequired, dictEntity);
                var componentsAttributes = GetSetComponentsAttributes(listRequired, dictAttribute);

                bool entitiesIsSame = stepEntities.SequenceEqual(componentsEntities);
                bool attributesIsSame = stepAttributes.SequenceEqual(componentsAttributes);

                if (!entitiesIsSame || !attributesIsSame)
                {
                    if (content.Length > 0)
                    {
                        content.AppendLine().AppendLine().AppendLine();
                    }

                    content.AppendFormat("{0}   Primary {1}   Secondary {2}   Message {3}   Stage {4}   Rank {5}   Status {6}   FilteringAttributes {7}",
                        step.EventHandler?.Name ?? "Unknown"
                        , step.PrimaryObjectTypeCodeName
                        , step.SecondaryObjectTypeCodeName
                        , step.SdkMessageId?.Name ?? "Unknown"
                        , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                        , step.Rank.ToString()
                        , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                        , step.FilteringAttributesStringsSorted
                    ).AppendLine();

                    if (!entitiesIsSame)
                    {
                        hasInfo = true;

                        content.AppendLine("Conflict in entites.");

                        content.Append("Entities in plugin step description");

                        if (stepEntities.Count > 0)
                        {
                            content.AppendLine(":");

                            foreach (var item in stepEntities)
                            {
                                content.AppendFormat("    {0}", item).AppendLine();
                            }
                        }
                        else
                        {
                            content.AppendLine(" are empty.");
                        }


                        content.Append("Entities in required components");

                        if (componentsEntities.Count > 0)
                        {
                            content.AppendLine(":");

                            foreach (var item in componentsEntities)
                            {
                                content.AppendFormat("    {0}", item).AppendLine();
                            }
                        }
                        else
                        {
                            content.AppendLine(" are empty.");
                        }
                    }

                    if (!attributesIsSame)
                    {
                        hasInfo = true;

                        content.AppendLine("Conflict in attributes.");

                        content.Append("Attributes in plugin step description");

                        if (componentsEntities.Count > 0)
                        {
                            content.AppendLine(":");

                            foreach (var item in stepAttributes)
                            {
                                content.AppendFormat("    {0}", item).AppendLine();
                            }
                        }
                        else
                        {
                            content.AppendLine(" are empty.");
                        }

                        content.Append("Attributes in required components");

                        if (componentsAttributes.Count > 0)
                        {
                            content.AppendLine(":");

                            foreach (var item in componentsAttributes)
                            {
                                content.AppendFormat("    {0}", item).AppendLine();
                            }
                        }
                        else
                        {
                            content.AppendLine(" are empty.");
                        }
                    }
                }
            }

            if (!hasInfo)
            {
                content.AppendLine(this._iWriteToOutput.WriteToOutput("No conflicts were found."));
            }

            string fileName = string.Format("{0}.Checking Plugin Steps Required Components at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (!Directory.Exists(commonConfig.FolderForExport))
            {
                Directory.CreateDirectory(commonConfig.FolderForExport);
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput("Created file with Checking Plugin Steps Required Components: {0}", filePath);

            this._iWriteToOutput.PerformAction(filePath);
        }

        private SortedSet<string> GetSetEntites(Entity entity)
        {
            SortedSet<string> result = new SortedSet<string>();

            if (entity.Contains(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode))
            {
                string primary = (string)entity.GetAttributeValue<AliasedValue>(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode).Value;

                if (!string.Equals(primary, "none", StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(primary);
                }
            }

            if (entity.Contains(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode))
            {
                string primary = (string)entity.GetAttributeValue<AliasedValue>(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode).Value;

                if (!string.Equals(primary, "none", StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(primary);
                }
            }

            return result;
        }

        private SortedSet<string> GetSetStepAttributes(SdkMessageProcessingStep step)
        {
            SortedSet<string> result = new SortedSet<string>();

            if (!string.IsNullOrEmpty(step.FilteringAttributes))
            {
                if (step.Contains(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode))
                {
                    string primary = (string)step.GetAttributeValue<AliasedValue>(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode).Value;

                    if (!string.Equals(primary, "none", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var item in step.FilteringAttributesStrings)
                        {
                            result.Add(string.Format("{0}.{1}", primary, item));
                        }
                    }
                }
            }

            return result;
        }

        #endregion Проверка шагов плагинов на необходимые компоненты.

        #region Проверка образов шагов плагинов на необходимые компоненты.

        public async Task ExecuteCheckingPluginImagesRequiredComponents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingPluginImagesRequiredComponentsFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await CheckingPluginImagesRequiredComponents(connectionData, commonConfig);
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

        private async Task CheckingPluginImagesRequiredComponents(ConnectionData connectionData, CommonConfiguration commonConfig)
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

            var repository = new PluginSearchRepository(service);
            var repositoryImage = new SdkMessageProcessingStepImageRepository(service);

            var listImages = await repositoryImage.GetAllImagesAsync();

            var queryImages = listImages
                            .OrderBy(image => image.Contains("sdkmessageprocessingstep.eventhandler") ? (image.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.eventhandler").Value as EntityReference).Name : "Null")
                            .ThenBy(image => image.PrimaryObjectTypeCodeName)
                            .ThenBy(image => image.SecondaryObjectTypeCodeName)
                            .ThenBy(image => image.Contains("sdkmessageprocessingstep.sdkmessageid") ? (image.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.sdkmessageid").Value as EntityReference).Name : "Null", new MessageComparer())
                            .ThenBy(image => image.Contains("sdkmessageprocessingstep.stage") ? (image.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.stage").Value as OptionSetValue).Value : 0)
                            .ThenBy(image => image.Contains("sdkmessageprocessingstep.mode") ? (image.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.mode").Value as OptionSetValue).Value : 0)
                            .ThenBy(image => image.Contains("sdkmessageprocessingstep.rank") ? (int)image.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.rank").Value : 0)
                            .ThenBy(image => image.FormattedValues.ContainsKey("sdkmessageprocessingstep.statuscode") ? image.FormattedValues["sdkmessageprocessingstep.statuscode"] : "")
                            .ThenBy(image => image.FormattedValues.ContainsKey(SdkMessageProcessingStepImage.Schema.Attributes.imagetype) ? image.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype] : "")
                            .ThenBy(image => image.Name)
                            .ThenBy(image => image.EntityAlias)
                            .ThenBy(image => image.Attributes1StringsSorted)
                            ;

            EntityMetadataRepository repositoryMetadata = new EntityMetadataRepository(service);
            var dependencyRepository = new DependencyRepository(service);

            var listMetaData = await repositoryMetadata.GetEntitiesWithAttributesAsync();

            var dictEntity = new Dictionary<Guid, EntityMetadata>();
            var dictAttribute = new Dictionary<Guid, AttributeMetadata>();

            foreach (var metaEntity in listMetaData)
            {
                dictEntity.Add(metaEntity.MetadataId.Value, metaEntity);

                foreach (var metaAttribute in metaEntity.Attributes)
                {
                    dictAttribute.Add(metaAttribute.MetadataId.Value, metaAttribute);
                }
            }

            bool hasInfo = false;

            foreach (var image in queryImages)
            {
                var listRequired = await dependencyRepository.GetRequiredComponentsAsync((int)ComponentType.SdkMessageProcessingStepImage, image.Id);

                var stepEntities = GetSetEntites(image);
                var stepAttributes = GetSetImageAttributes(image);

                var componentsEntities = GetSetComponentsEntites(listRequired, dictEntity);
                var componentsAttributes = GetSetComponentsAttributes(listRequired, dictAttribute);

                bool entitiesIsSame = stepEntities.SequenceEqual(componentsEntities);
                bool attributesIsSame = stepAttributes.SequenceEqual(componentsAttributes);

                string pluginType = image.Contains("sdkmessageprocessingstep.eventhandler") ? (image.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.eventhandler").Value as EntityReference).Name : "Null";

                string sdkMessage = image.Contains("sdkmessageprocessingstep.sdkmessageid") ? (image.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.sdkmessageid").Value as EntityReference).Name : "Null";
                int stage = image.Contains("sdkmessageprocessingstep.stage") ? (image.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.stage").Value as OptionSetValue).Value : 0;
                int mode = image.Contains("sdkmessageprocessingstep.mode") ? (image.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.mode").Value as OptionSetValue).Value : 0;
                int rank = image.Contains("sdkmessageprocessingstep.rank") ? (int)image.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.rank").Value : 0;
                string status = image.FormattedValues.ContainsKey("sdkmessageprocessingstep.statuscode") ? image.FormattedValues["sdkmessageprocessingstep.statuscode"] : "";

                if (!entitiesIsSame || !attributesIsSame)
                {
                    hasInfo = true;

                    if (content.Length > 0)
                    {
                        content.AppendLine().AppendLine().AppendLine();
                    }

                    //handler.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "ImageType", "Name", "EntityAlias", "Attributes");

                    content.AppendFormat("{0}   Primary {1}   Secondary {2}   Message {3}   Stage {4}   Rank {5}   Status {6}   ImageType {7}   Name {8}   EntityAlias {9}   Attributes {10}"
                        , pluginType
                        , image.PrimaryObjectTypeCodeName
                        , image.SecondaryObjectTypeCodeName
                        , sdkMessage
                        , SdkMessageProcessingStepRepository.GetStageName(stage, mode)
                        , rank.ToString()
                        , status
                        , image.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype]
                        , image.Name
                        , image.EntityAlias
                        , image.Attributes1StringsSorted
                    ).AppendLine();

                    if (!entitiesIsSame)
                    {
                        content.AppendLine("Conflict in entites.");

                        content.Append("Entities in plugin step description");

                        if (stepEntities.Count > 0)
                        {
                            content.AppendLine(":");

                            foreach (var item in stepEntities)
                            {
                                content.AppendFormat("    {0}", item).AppendLine();
                            }
                        }
                        else
                        {
                            content.AppendLine(" are empty.");
                        }


                        content.Append("Entities in required components");

                        if (componentsEntities.Count > 0)
                        {
                            content.AppendLine(":");

                            foreach (var item in componentsEntities)
                            {
                                content.AppendFormat("    {0}", item).AppendLine();
                            }
                        }
                        else
                        {
                            content.AppendLine(" are empty.");
                        }
                    }

                    if (!attributesIsSame)
                    {
                        content.AppendLine("Conflict in attributes.");

                        content.Append("Attributes in plugin step description");

                        if (componentsEntities.Count > 0)
                        {
                            content.AppendLine(":");

                            foreach (var item in stepAttributes)
                            {
                                content.AppendFormat("    {0}", item).AppendLine();
                            }
                        }
                        else
                        {
                            content.AppendLine(" are empty.");
                        }

                        content.Append("Attributes in required components");

                        if (componentsAttributes.Count > 0)
                        {
                            content.AppendLine(":");

                            foreach (var item in componentsAttributes)
                            {
                                content.AppendFormat("    {0}", item).AppendLine();
                            }
                        }
                        else
                        {
                            content.AppendLine(" are empty.");
                        }
                    }
                }
            }

            if (!hasInfo)
            {
                content.AppendLine(this._iWriteToOutput.WriteToOutput("No conflicts were found."));
            }

            string fileName = string.Format("{0}.Checking Plugin Images Required Components at {1}.txt", connectionData.Name, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (!Directory.Exists(commonConfig.FolderForExport))
            {
                Directory.CreateDirectory(commonConfig.FolderForExport);
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput("Created file with Checking Plugin Images Required Components: {0}", filePath);

            this._iWriteToOutput.PerformAction(filePath);
        }

        private SortedSet<string> GetSetImageAttributes(SdkMessageProcessingStepImage image)
        {
            SortedSet<string> result = new SortedSet<string>();

            if (!string.IsNullOrEmpty(image.Attributes1))
            {
                if (image.Contains(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode))
                {
                    string primary = (string)image.GetAttributeValue<AliasedValue>(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode).Value;

                    if (!string.Equals(primary, "none", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var item in image.Attributes1Strings)
                        {
                            result.Add(string.Format("{0}.{1}", primary, item));
                        }
                    }
                }
            }

            return result;
        }

        #endregion Проверка образов шагов плагинов на необходимые компоненты.

        private SortedSet<string> GetSetComponentsAttributes(List<Dependency> coll, Dictionary<Guid, AttributeMetadata> dictAttribute)
        {
            SortedSet<string> result = new SortedSet<string>();

            var list = coll.Where(ent => ent.RequiredComponentType.Value == (int)ComponentType.Attribute);

            foreach (var attribute in list)
            {
                var idAttribute = attribute.RequiredComponentObjectId.Value;

                if (dictAttribute.ContainsKey(idAttribute))
                {
                    var attributeMetadata = dictAttribute[idAttribute];

                    result.Add(string.Format("{0}.{1}", attributeMetadata.EntityLogicalName, attributeMetadata.LogicalName));
                }
            }

            return result;
        }

        private SortedSet<string> GetSetComponentsEntites(List<Dependency> coll, Dictionary<Guid, EntityMetadata> dictEntity)
        {
            SortedSet<string> result = new SortedSet<string>();

            var list = coll.Where(ent => ent.RequiredComponentType.Value == (int)ComponentType.Entity);

            foreach (var entity in list)
            {
                var idEntity = entity.RequiredComponentObjectId.Value;

                if (dictEntity.ContainsKey(idEntity))
                {
                    var metadata = dictEntity[idEntity];

                    result.Add(metadata.LogicalName);
                }
            }

            return result;
        }
    }
}
