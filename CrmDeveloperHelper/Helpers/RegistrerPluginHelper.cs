using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class RegistrerPluginHelper
    {
        private IOrganizationServiceExtented _service;

        public RegistrerPluginHelper(IOrganizationServiceExtented service)
        {
            this._service = service;
        }

        public async Task<string> RegisterPluginsForAssemblyAsync(string folder, PluginExtraction.PluginAssembly assembly)
        {
            if (_service.ConnectionData.IsReadOnly)
            {
                return null;
            }

            string fileName = string.Format("{0}.Plugin Register Operation at {1}.txt"
                , _service.ConnectionData.Name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            StringBuilder log = new StringBuilder();

            var repositoryAssembly = new PluginAssemblyRepository(_service);
            var repositoryType = new PluginTypeRepository(_service);
            var repositoryMessage = new SdkMessageRepository(_service);
            var repositoryFilter = new SdkMessageFilterRepository(_service);
            var repositorySystemUser = new SystemUserRepository(_service);

            var entAssembly = await repositoryAssembly.FindAssemblyAsync(assembly.Name);

            if (entAssembly != null)
            {
                log.AppendFormat("Assembly {0} founded in CRM with ID {1}", assembly.Name, entAssembly.Id).AppendLine();

                foreach (var pluginType in assembly.PluginTypes)
                {
                    var entPluginType = repositoryType.FindPluginType(pluginType.TypeName);

                    if (entPluginType == null)
                    {
                        log.AppendFormat("Plugin Type {0} not founded in CRM.", pluginType.TypeName).AppendLine();
                        continue;
                    }

                    log.AppendFormat("Plugin Type {0} founded in CRM with ID {1}", pluginType.TypeName, entPluginType.Id).AppendLine();

                    foreach (var step in pluginType.PluginSteps)
                    {
                        await RegisterSingleStep(log, repositoryMessage, repositoryFilter, repositorySystemUser, entPluginType, step);
                    }
                }
            }
            else
            {
                log.AppendFormat("Assembly {0} not founded in CRM.", assembly.Name).AppendLine();
            }

            File.WriteAllText(filePath, log.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        private async Task RegisterSingleStep(StringBuilder log, SdkMessageRepository repositoryMessage, SdkMessageFilterRepository repositoryFilter, SystemUserRepository repositorySystemUser, Entities.PluginType entPluginType, PluginExtraction.PluginStep step)
        {
            var entMessage = await repositoryMessage.FindMessageAsync(step.Message);

            if (entMessage == null)
            {
                log.AppendFormat("Message {0} not founded in CRM.", step.Message).AppendLine();
                return;
            }

            var refMessageFilter = await repositoryFilter.FindFilterAsync(entMessage.Id, step.PrimaryEntity, step.SecondaryEntity);

            EntityReference refSecure = null;
            EntityReference refSystemUser = null;

            if (!string.IsNullOrEmpty(step.SecureConfiguration))
            {
                var entSecure = new Entity(Entities.SdkMessageProcessingStepSecureConfig.EntityLogicalName);
                entSecure.Attributes[Entities.SdkMessageProcessingStepSecureConfig.Schema.Attributes.secureconfig] = step.SecureConfiguration;

                entSecure.Id = await _service.CreateAsync(entSecure);

                refSecure = entSecure.ToEntityReference();
            }

            if (!string.IsNullOrEmpty(step.RunInUserContext) && step.RunInUserContext != "Calling User")
            {
                refSystemUser = repositorySystemUser.FindUser(step.RunInUserContext);
            }

            var entStep = new Entity(Entities.SdkMessageProcessingStep.EntityLogicalName)
            {
                Id = step.Id
            };

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.asyncautodelete] = step.AsyncAutoDelete.GetValueOrDefault();
            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.name] = step.Name;
            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.description] = step.Description;
            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.rank] = step.ExecutionOrder;

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.stage] = new OptionSetValue((int)step.Stage);
            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.mode] = new OptionSetValue((int)step.ExecutionMode);

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.supporteddeployment] = new OptionSetValue((int)step.SupportedDeploymentCode);

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.configuration] = step.UnsecureConfiguration;

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.filteringattributes] = string.Join(",", step.FilteringAttributes.OrderBy(s => s));

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.plugintypeid] = entPluginType.ToEntityReference();

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.eventhandler] = entPluginType.ToEntityReference();

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.sdkmessageid] = entMessage.ToEntityReference();

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid] = refMessageFilter;

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.sdkmessageprocessingstepsecureconfigid] = refSecure;

            entStep.Attributes[Entities.SdkMessageProcessingStep.Schema.Attributes.impersonatinguserid] = refSystemUser;

            entStep.Id = await _service.CreateAsync(entStep);

            foreach (var image in step.PluginImages)
            {
                var entImage = new Entity(Entities.SdkMessageProcessingStepImage.EntityLogicalName)
                {
                    Id = image.Id
                };

                entImage.Attributes[Entities.SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid] = entStep.ToEntityReference();

                entImage.Attributes[Entities.SdkMessageProcessingStepImage.Schema.Attributes.imagetype] = new OptionSetValue(image.ImageType.Value);

                entImage.Attributes[Entities.SdkMessageProcessingStepImage.Schema.Attributes.name] = image.Name;
                entImage.Attributes[Entities.SdkMessageProcessingStepImage.Schema.Attributes.entityalias] = image.EntityAlias;

                entImage.Attributes[Entities.SdkMessageProcessingStepImage.Schema.Attributes.customizationlevel] = image.CustomizationLevel;
                entImage.Attributes[Entities.SdkMessageProcessingStepImage.Schema.Attributes.relatedattributename] = image.RelatedAttributeName;
                entImage.Attributes[Entities.SdkMessageProcessingStepImage.Schema.Attributes.messagepropertyname] = image.MessagePropertyName;

                entImage.Attributes[Entities.SdkMessageProcessingStepImage.Schema.Attributes.attributes] = string.Join(",", image.Attributes.OrderBy(s => s));

                entImage.Id = await _service.CreateAsync(entImage);
            }

            _service.Execute(new SetStateRequest()
            {
                EntityMoniker = entStep.ToEntityReference(),

                State = new OptionSetValue(step.StateCode.Value),
                Status = new OptionSetValue(step.StatusCode.Value),
            });
        }

        public async Task<string> RegisterPluginsForPluginTypeAsync(string folder, string assemblyName, Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction.PluginType pluginType)
        {
            if (_service.ConnectionData.IsReadOnly)
            {
                return null;
            }

            string fileName = string.Format("{0}.Plugin Register Operation at {1}.txt"
                , _service.ConnectionData.Name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            StringBuilder log = new StringBuilder();

            var repositoryAssembly = new PluginAssemblyRepository(_service);
            var repositoryType = new PluginTypeRepository(_service);
            var repositoryMessage = new SdkMessageRepository(_service);
            var repositoryFilter = new SdkMessageFilterRepository(_service);
            var repositorySystemUser = new SystemUserRepository(_service);

            var entAssembly = await repositoryAssembly.FindAssemblyAsync(assemblyName);

            if (entAssembly != null)
            {
                log.AppendFormat("Assembly {0} founded in CRM with ID {1}", assemblyName, entAssembly.Id).AppendLine();

                var entPluginType = repositoryType.FindPluginType(pluginType.TypeName);

                if (entPluginType != null)
                {
                    log.AppendFormat("Plugin Type {0} founded in CRM with ID {1}", pluginType.TypeName, entPluginType.Id).AppendLine();

                    foreach (var step in pluginType.PluginSteps)
                    {
                        await RegisterSingleStep(log, repositoryMessage, repositoryFilter, repositorySystemUser, entPluginType, step);
                    }
                }
                else
                {
                    log.AppendFormat("Plugin Type {0} not founded in CRM.", pluginType.TypeName).AppendLine();
                }
            }
            else
            {
                log.AppendFormat("Assembly {0} not founded in CRM.", assemblyName).AppendLine();
            }

            File.WriteAllText(filePath, log.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        public async Task<string> RegisterPluginsForPluginStepAsync(string folder, string assemblyName, string pluginType, Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction.PluginStep step)
        {
            if (_service.ConnectionData.IsReadOnly)
            {
                return null;
            }

            string fileName = string.Format("{0}.Plugin Register Operation at {1}.txt"
                , _service.ConnectionData.Name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            StringBuilder log = new StringBuilder();

            var repositoryAssembly = new PluginAssemblyRepository(_service);
            var repositoryType = new PluginTypeRepository(_service);
            var repositoryMessage = new SdkMessageRepository(_service);
            var repositoryFilter = new SdkMessageFilterRepository(_service);
            var repositorySystemUser = new SystemUserRepository(_service);

            var entAssembly = await repositoryAssembly.FindAssemblyAsync(assemblyName);

            if (entAssembly != null)
            {
                log.AppendFormat("Assembly {0} founded in CRM with ID {1}", assemblyName, entAssembly.Id).AppendLine();

                var entPluginType = repositoryType.FindPluginType(pluginType);

                if (entPluginType != null)
                {
                    log.AppendFormat("Plugin Type {0} founded in CRM with ID {1}", pluginType, entPluginType.Id).AppendLine();

                    await RegisterSingleStep(log, repositoryMessage, repositoryFilter, repositorySystemUser, entPluginType, step);
                }
                else
                {
                    log.AppendFormat("Plugin Type {0} not founded in CRM.", pluginType).AppendLine();
                }
            }
            else
            {
                log.AppendFormat("Assembly {0} not founded in CRM.", assemblyName).AppendLine();
            }

            File.WriteAllText(filePath, log.ToString(), new UTF8Encoding(false));

            return filePath;
        }
    }
}