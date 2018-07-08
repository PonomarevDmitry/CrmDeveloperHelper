using Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class PluginTypeConfigurationDescriptionHandler
    {
        private const string formatIndent = "    {0}";

        public Task<string> CreateDescriptionAsync(PluginType pluginType)
        {
            return Task.Run(() => CreateDescription(pluginType));
        }

        private string CreateDescription(PluginType pluginType)
        {
            var sortedSteps = pluginType.PluginSteps
                .OrderBy(ent => ent.PrimaryEntity)
                .ThenBy(ent => ent.Message, new MessageComparer())
                .ThenBy(s => s.ExecutionMode.Value)
                .ThenBy(s => s.ExecutionOrder)
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

                var messageName = step.Message;
                var primaryObjectTypeCode = step.PrimaryEntity;
                var secondaryObjectTypeCode = step.SecondaryEntity;

                description.AppendFormat("{0}. Step", numberStep).AppendLine();

                description.AppendFormat("Primary Entity '{0}',   Secondary Entity '{1}'"
                    , primaryObjectTypeCode
                    , secondaryObjectTypeCode
                    ).AppendLine();

                description.AppendFormat("Message '{0}',    Stage '{1}',    Mode '{2}',    Rank {3},    Status {4}"
                    , messageName
                    , Nav.Common.VSPackages.CrmDeveloperHelper.Repository.SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.ExecutionMode.Value)
                    , step.ExecutionModeName
                    , step.ExecutionOrder.ToString()
                    , step.StatusCodeName
                    ).AppendLine();

                string contextUser = step.RunInUserContext;

                description.AppendFormat("RunInUserContext: {0}", contextUser).AppendLine();
                description.AppendFormat("AsyncAutoDelete: {0}", step.AsyncAutoDelete.GetValueOrDefault()).AppendLine();

                if (step.FilteringAttributes.Count > 0)
                {
                    description.AppendFormat("Filtering Attributes: {0}", string.Join(",", step.FilteringAttributes.OrderBy(s => s))).AppendLine();
                }

                if (!string.IsNullOrEmpty(step.Name))
                {
                    description.AppendFormat("Name: {0}", step.Name).AppendLine();
                }

                if (!string.IsNullOrEmpty(step.Description))
                {
                    description.AppendFormat("Description: {0}", step.Description).AppendLine();
                }

                //{
                //    string dateStr = GetDateString(step.CreatedBy, step.CreatedOn, step.ModifiedBy, step.ModifiedOn, step.OverwriteTime);

                //    if (!string.IsNullOrEmpty(dateStr))
                //    {
                //        description.AppendLine(dateStr);
                //    }
                //}

                int numberImage = 1;

                foreach (var image in step.PluginImages
                    .OrderBy(im => im.ImageType.Value)
                    .ThenBy(im => im.CreatedOn.Value)
                    .ThenBy(im => im.Name)
                    )
                {
                    string imageDescription = GetImageDescription(numberImage, image);

                    var coll = imageDescription.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in coll)
                    {
                        description.AppendFormat(formatIndent, item).AppendLine();
                    }

                    numberImage++;
                }

                numberStep++;
            }

            return description.ToString();
        }

        private string GetImageDescription(int numberImage, PluginImage image)
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

            result.AppendFormat("{0}. {1}", numberImage, imageType).AppendLine();

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

            if (image.Attributes.Count > 0)
            {
                result.AppendFormat("Attributes: {0}", string.Join(",", image.Attributes.OrderBy(s => s))).AppendLine();
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

        private string GetDateString(string createdBy, DateTime? createdOn, string modifiedBy, DateTime? modifiedOn)
        {
            StringBuilder description = new StringBuilder();

            const string splitter = ",   ";

            if (createdBy != null)
            {
                if (description.Length > 0)
                {
                    description.Append(splitter);
                }

                description.AppendFormat("CreatedBy: '{0}'", createdBy);
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

                description.AppendFormat("ModifiedBy: '{0}'", modifiedBy);
            }

            if (modifiedOn.HasValue)
            {
                if (description.Length > 0)
                {
                    description.Append(splitter);
                }

                description.AppendFormat("ModifiedOn: {0}", modifiedOn.Value.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }

            //if (overwriteTime.HasValue)
            //{
            //    if (description.Length > 0)
            //    {
            //        description.Append(splitter);
            //    }

            //    description.AppendFormat("OverwriteTime: {0}", overwriteTime.Value.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            //}

            return description.ToString();
        }
    }
}
