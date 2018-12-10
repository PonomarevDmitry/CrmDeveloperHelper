using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class PluginTypeDescriptionHandler
    {
        private const string tabSpacer = "    ";

        public static Task<string> CreateDescriptionAsync(
           Guid idPluginType
           , IEnumerable<SdkMessageProcessingStep> allSteps
           , IEnumerable<SdkMessageProcessingStepImage> queryImage
           , IEnumerable<SdkMessageProcessingStepSecureConfig> listSecure
           )
        {
            return Task.Run(() => CreateDescription(idPluginType, allSteps, queryImage, listSecure));
        }

        private static string CreateDescription(
            Guid idPluginType
            , IEnumerable<SdkMessageProcessingStep> allSteps
            , IEnumerable<SdkMessageProcessingStepImage> queryImage
            , IEnumerable<SdkMessageProcessingStepSecureConfig> listSecure
            )
        {
            var sortedSteps = allSteps
                .Where(s => s.EventHandler.Id == idPluginType)
                .OrderBy(ent => ent.PrimaryObjectTypeCodeName)
                .ThenBy(ent => ent.SdkMessageId?.Name ?? "Unknown", new MessageComparer())
                .ThenBy(s => s.Mode.Value)
                .ThenBy(s => s.Rank)
                .ThenBy(s => s.Name)
                ;

            if (!sortedSteps.Any())
            {
                return string.Empty;
            }

            StringBuilder description = new StringBuilder();

            //{
            //    string dateStr = GetDateString(pluginType.CreatedBy, pluginType.CreatedOn, pluginType.ModifiedBy, pluginType.ModifiedOn, pluginType.OverwriteTime);

            //    if (!string.IsNullOrEmpty(dateStr))
            //    {
            //        description.AppendLine(dateStr);
            //    }
            //}

            //description.AppendLine(connectionInfo).AppendLine();
            //description.AppendFormat("Plugin Steps for PluginType '{0}' at {1}", pluginType.TypeName, DateTime.Now.ToString(formatDateTime)).AppendLine();

            int numberStep = 1;

            foreach (var step in sortedSteps)
            {
                if (description.Length > 0)
                {
                    description.AppendLine();
                }

                description.AppendFormat("{0}. Step", numberStep).AppendLine();

                var stepImages = queryImage.Where(i => i.SdkMessageProcessingStepId.Id == step.SdkMessageProcessingStepId);

                SdkMessageProcessingStepSecureConfig entSecure = null;

                if (step.SdkMessageProcessingStepSecureConfigId != null)
                {
                    entSecure = listSecure.FirstOrDefault(s => s.SdkMessageProcessingStepSecureConfigId == step.SdkMessageProcessingStepSecureConfigId.Id);
                }

                description.AppendLine(GetStepDescription(step, entSecure, stepImages));

                numberStep++;
            }

            return description.ToString();
        }

        public static Task<string> GetStepDescriptionAsync(
            SdkMessageProcessingStep step
            , SdkMessageProcessingStepSecureConfig entSecure
            , IEnumerable<SdkMessageProcessingStepImage> stepImages
            )
        {
            return Task.Run(() => GetStepDescription(step, entSecure, stepImages));
        }

        private static string GetStepDescription(
            SdkMessageProcessingStep step
            , SdkMessageProcessingStepSecureConfig entSecure
            , IEnumerable<SdkMessageProcessingStepImage> stepImages
            )
        {
            StringBuilder description = new StringBuilder();

            var messageName = step.SdkMessageId?.Name ?? "Unknown";
            var primaryObjectTypeCode = step.PrimaryObjectTypeCodeName;
            var secondaryObjectTypeCode = step.SecondaryObjectTypeCodeName;

            description.AppendFormat("Primary Entity '{0}',   Secondary Entity '{1}'"
                , primaryObjectTypeCode
                , secondaryObjectTypeCode
                ).AppendLine();

            description.AppendFormat("Message '{0}',    Stage '{1}',    Mode '{2}',    Rank {3},    Status {4}"
                , messageName
                , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.mode]
                , step.Rank.ToString()
                , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                ).AppendLine();

            string contextUser = "Calling User";

            if (step.ImpersonatingUserId != null)
            {
                contextUser = step.ImpersonatingUserId.Name;
            }

            description.AppendFormat("RunInUserContext: {0}", contextUser).AppendLine();
            description.AppendFormat("AsyncAutoDelete: {0}", step.AsyncAutoDelete.GetValueOrDefault()).AppendLine();

            if (!string.IsNullOrEmpty(step.FilteringAttributes))
            {
                description.AppendFormat("Filtering Attributes: {0}", step.FilteringAttributesStringsSorted).AppendLine();
            }

            if (!string.IsNullOrEmpty(step.Name))
            {
                description.AppendFormat("Name: {0}", step.Name).AppendLine();
            }

            if (!string.IsNullOrEmpty(step.Description))
            {
                description.AppendFormat("Description: {0}", step.Description).AppendLine();
            }

            if (!string.IsNullOrEmpty(step.Configuration))
            {
                description.AppendFormat("Unsecure Configuration: {0}", step.Configuration).AppendLine();
            }

            if (entSecure != null)
            {
                if (!string.IsNullOrEmpty(entSecure.SecureConfig))
                {
                    description.AppendFormat("Secure Configuration: {0}", entSecure.SecureConfig).AppendLine();
                }
            }

            //{
            //    string dateStr = GetDateString(step.CreatedBy, step.CreatedOn, step.ModifiedBy, step.ModifiedOn, step.OverwriteTime);

            //    if (!string.IsNullOrEmpty(dateStr))
            //    {
            //        description.AppendLine(dateStr);
            //    }
            //}

            int numberImage = 1;

            foreach (var image in stepImages
                .OrderBy(im => im.ImageType.Value)
                .ThenBy(im => im.CreatedOn.Value)
                .ThenBy(im => im.Name)
                )
            {
                string imageDescription = GetImageDescription(numberImage, image);

                var coll = imageDescription.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in coll)
                {
                    description.AppendLine(tabSpacer + item);
                }

                numberImage++;
            }

            return description.ToString();
        }

        public static string GetImageDescription(int? numberImage, Entities.SdkMessageProcessingStepImage image)
        {
            StringBuilder result = new StringBuilder();

            string imageType = "Image";

            if (image.ImageType != null)
            {
                if (image.ImageType.Value == 0)
                {
                    imageType = "PreImage";
                }
                else if (image.ImageType.Value == 1)
                {
                    imageType = "PostImage";
                }
                else if (image.ImageType.Value == 2)
                {
                    imageType = "BothImage";
                }
            }

            if (numberImage.HasValue)
            {
                result.AppendFormat("{0}. ", numberImage);
            }

            result.AppendFormat("{0}", imageType).AppendLine();

            {
                StringBuilder temp = new StringBuilder();

                if (!string.IsNullOrEmpty(image.EntityAlias))
                {
                    if (temp.Length > 0) { temp.Append(",   "); }

                    temp.AppendFormat("EntityAlias '{0}'", image.EntityAlias);
                }

                if (!string.IsNullOrEmpty(image.Name))
                {
                    if (temp.Length > 0) { temp.Append(",   "); }

                    temp.AppendFormat("Name '{0}'", image.EntityAlias);
                }

                if (temp.Length > 0)
                {
                    result.AppendLine(temp.ToString());
                }
            }

            if (!string.IsNullOrEmpty(image.Attributes1))
            {
                result.AppendFormat("Attributes: {0}", image.Attributes1StringsSorted).AppendLine();
            }
            else
            {
                result.AppendLine("Attributes: All");
            }

            //{
            //    string dateStr = GetDateString(image.CreatedBy, image.CreatedOn, image.ModifiedBy, image.ModifiedOn, image.OverwriteTime);

            //    if (!string.IsNullOrEmpty(dateStr))
            //    {
            //        result.AppendLine(dateStr);
            //    }
            //}

            return result.ToString();
        }

        private static string GetDateString(EntityReference createdBy, DateTime? createdOn, EntityReference modifiedBy, DateTime? modifiedOn, DateTime? overwriteTime)
        {
            StringBuilder description = new StringBuilder();

            const string splitter = ",   ";

            if (createdBy != null)
            {
                if (description.Length > 0)
                {
                    description.Append(splitter);
                }

                description.AppendFormat("CreatedBy: '{0}'", createdBy.Name);
            }

            if (createdOn.HasValue)
            {
                if (description.Length > 0)
                {
                    description.Append(splitter);
                }

                description.AppendFormat("CreatedOn: {0}", createdOn.Value.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }

            if (modifiedBy != null)
            {
                if (description.Length > 0)
                {
                    description.Append(splitter);
                }

                description.AppendFormat("ModifiedBy: '{0}'", modifiedBy.Name);
            }

            if (modifiedOn.HasValue)
            {
                if (description.Length > 0)
                {
                    description.Append(splitter);
                }

                description.AppendFormat("ModifiedOn: {0}", modifiedOn.Value.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }

            if (overwriteTime.HasValue)
            {
                if (description.Length > 0)
                {
                    description.Append(splitter);
                }

                description.AppendFormat("OverwriteTime: {0}", overwriteTime.Value.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }

            return description.ToString();
        }

        public static Task<bool> CreateFileWithDescriptionAsync(string connectionInfo, string filePath, Guid idPluginType, string pluginTypeName, List<SdkMessageProcessingStep> allSteps, List<SdkMessageProcessingStepImage> queryImage, List<SdkMessageProcessingStepSecureConfig> listSecure)
        {
            return Task.Run(() => CreateFileWithDescription(connectionInfo, filePath, idPluginType, pluginTypeName, allSteps, queryImage, listSecure));
        }

        private static bool CreateFileWithDescription(string connectionInfo, string filePath, Guid idPluginType, string pluginTypeName, List<SdkMessageProcessingStep> allSteps, List<SdkMessageProcessingStepImage> queryImage, List<SdkMessageProcessingStepSecureConfig> listSecure)
        {
            string description = PluginTypeDescriptionHandler.CreateDescription(idPluginType, allSteps, queryImage, listSecure);

            if (string.IsNullOrEmpty(description))
            {
                return false;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(connectionInfo).AppendLine();
            content.AppendFormat("Plugin Steps for PluginType '{0}' at {1}", pluginTypeName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)).AppendLine();

            content.Append(description);

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return true;
        }
    }
}
