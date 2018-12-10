using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class EntityAttributesDependentComponentsHandler
    {
        private DependencyRepository _dependencyRepository;
        private SolutionComponentDescriptor _descriptor;
        private DependencyDescriptionHandler _descriptorHandler;

        public EntityAttributesDependentComponentsHandler(DependencyRepository dependencyRepository, SolutionComponentDescriptor descriptor, DependencyDescriptionHandler descriptorHandler)
        {
            this._dependencyRepository = dependencyRepository;
            this._descriptor = descriptor;
            this._descriptorHandler = descriptorHandler;
        }

        public Task<string> CreateFileAsync(string entityName, string filePath, bool allComponents)
        {
            return Task.Run(async () => await CreateFile(entityName, filePath, allComponents));
        }

        private async Task<string> CreateFile(string entityName, string filePath, bool allComponents)
        {
            StringBuilder content = new StringBuilder();

            var metedata = _descriptor.MetadataSource.GetEntityMetadata(entityName);

            foreach (var currentAttribute in metedata.Attributes.OrderBy(attr => attr.LogicalName))
            {
                if (currentAttribute.AttributeOf == null)
                {
                    string name = string.Format("{0}.{1}", entityName, currentAttribute.LogicalName);

                    var coll = await _dependencyRepository.GetDependentComponentsAsync((int)ComponentType.Attribute, currentAttribute.MetadataId.Value);

                    if (!allComponents)
                    {
                        coll = coll.Where(ent =>
                        {
                            var val = ent.DependentComponentType.Value;

                            return val == (int)ComponentType.Workflow
                                || val == (int)ComponentType.SdkMessageProcessingStep
                                || val == (int)ComponentType.SdkMessageProcessingStepImage
                                ;

                            //return val == (int)ComponentType.SDKMessageProcessingStepImage
                            //    ;
                        }
                        ).ToList();
                    }

                    var desc = await _descriptorHandler.GetDescriptionDependentAsync(coll);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (content.Length > 0)
                        {
                            content
                                .AppendLine()
                                .AppendLine()
                                .AppendLine(new string('-', 100))
                                .AppendLine()
                                .AppendLine();
                        }

                        content.AppendFormat("Attribute {0} Dependent Components:", name).AppendLine();
                        content.Append(desc);
                    }
                }
            }

            string message = string.Empty;

            if (content.Length > 0)
            {
                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                message = string.Format("For entity '{0}' created file with attributes dependent components: {1}", entityName, filePath);
            }
            else
            {
                message = string.Format("No information about attributes dependent components for entity '{0}'.", entityName);
            }

            return message;
        }
    }
}
