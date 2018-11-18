using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class OrganizationCustomizationTransfer
    {
        public ConnectionData ConnectionSource { get; private set; }

        public ConnectionData ConnectionTarget { get; private set; }

        private string _folder;

        private const string _tabSpacer = "    ";

        private IWriteToOutput _iWriteToOutput;

        private IOrganizationServiceExtented _serviceSource;
        private IOrganizationServiceExtented _serviceTarget;

        public OrganizationCustomizationTransfer(ConnectionData connectionSource, ConnectionData connectionTarget, IWriteToOutput iWriteToOutput, string folder)
        {
            this.ConnectionSource = connectionSource;
            this.ConnectionTarget = connectionTarget;
            this._iWriteToOutput = iWriteToOutput;
            this._folder = folder;
        }

        public async Task InitializeConnection(StringBuilder content)
        {
            bool service1IsNull = this._serviceSource == null;
            bool service2IsNull = this._serviceTarget == null;

            {
                var mess1 = "Connection CRM Source.";
                var mess2 = ConnectionSource.GetConnectionDescription();

                if (service1IsNull)
                {
                    _iWriteToOutput.WriteToOutput(mess1);
                    _iWriteToOutput.WriteToOutput(mess2);
                    _serviceSource = await QuickConnection.ConnectAsync(ConnectionSource);
                }

                var mess3 = string.Format(Properties.OutputStrings.CurrentServiceEndpointFormat1, _serviceSource.CurrentServiceEndpoint);

                if (service1IsNull)
                {
                    _iWriteToOutput.WriteToOutput(mess3);
                }

                if (content != null)
                {
                    content.AppendLine(mess1);
                    content.AppendLine(mess2);
                    content.AppendLine(mess3);
                }
            }

            if (service1IsNull)
            {
                _iWriteToOutput.WriteToOutput(string.Empty);
            }

            if (content != null)
            {
                content.AppendLine();
            }

            {
                var mess1 = "Connection CRM Target.";
                var mess2 = ConnectionTarget.GetConnectionDescription();

                if (service2IsNull)
                {
                    _iWriteToOutput.WriteToOutput(mess1);
                    _iWriteToOutput.WriteToOutput(mess2);
                    _serviceTarget = await QuickConnection.ConnectAsync(ConnectionTarget);
                }

                var mess3 = string.Format(Properties.OutputStrings.CurrentServiceEndpointFormat1, _serviceTarget.CurrentServiceEndpoint);

                if (service2IsNull)
                {
                    _iWriteToOutput.WriteToOutput(mess3);
                }

                if (content != null)
                {
                    content.AppendLine(mess1);
                    content.AppendLine(mess2);
                    content.AppendLine(mess3);
                }
            }

            if (content != null)
            {
                content.AppendLine();
            }
        }

        public Task<string> TrasnferAuditAsync()
        {
            return Task.Run(async () => await TrasnferAudit());
        }

        private async Task<string> TrasnferAudit()
        {
            StringBuilder content = new StringBuilder();

            await InitializeConnection(content);

            string operation = string.Format(Properties.OperationNames.TransferingAuditFormat2, ConnectionSource.Name, ConnectionTarget.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var repositorySource = new EntityMetadataRepository(_serviceSource);
            var repositoryTarget = new EntityMetadataRepository(_serviceTarget);

            var taskSource = repositorySource.GetEntitiesWithAttributesForAuditAsync();
            var taskTarget = repositoryTarget.GetEntitiesWithAttributesForAuditAsync();

            var listEntityMetadataSource = await taskSource;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Entities in {0}: {1}", ConnectionSource.Name, listEntityMetadataSource.Count()));

            var listEntityMetadataTarget = await taskTarget;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Entities in {0}: {1}", ConnectionTarget.Name, listEntityMetadataTarget.Count()));

            var commonEntityMetadata = new List<LinkedEntities<EntityMetadata>>();

            foreach (var entityMetadata1 in listEntityMetadataSource.OrderBy(e => e.LogicalName))
            {
                {
                    var entityMetadata2 = listEntityMetadataTarget.FirstOrDefault(e => string.Equals(e.LogicalName, entityMetadata1.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                    if (entityMetadata2 != null)
                    {
                        commonEntityMetadata.Add(new LinkedEntities<EntityMetadata>(entityMetadata1, entityMetadata2));
                        continue;
                    }
                }
            }

            HashSet<string> entitiesToPublish = new HashSet<string>();

            var entitiesToEnableAudit = commonEntityMetadata.Where(
                e =>
                e.Entity1.IsAuditEnabled != null
                && e.Entity1.IsAuditEnabled.Value
                && e.Entity2.IsAuditEnabled != null
                && e.Entity2.IsAuditEnabled.CanBeChanged
                && e.Entity2.IsAuditEnabled.Value == false
                ).ToList();

            if (entitiesToEnableAudit.Any())
            {
                content
                    .AppendLine()
                    .AppendFormat("Enabling Audit on Entities: {0}", entitiesToEnableAudit.Count)
                    .AppendLine();

                foreach (var entityLink in entitiesToEnableAudit.OrderBy(e => e.Entity1.LogicalName))
                {
                    content.AppendLine(_tabSpacer + entityLink.Entity1.LogicalName);

                    entitiesToPublish.Add(entityLink.Entity1.LogicalName);

                    try
                    {
                        entityLink.Entity2.IsAuditEnabled.Value = true;

                        await repositoryTarget.UpdateEntityMetadataAsync(entityLink.Entity2);
                    }
                    catch (Exception ex)
                    {
                        var desc = DTEHelper.GetExceptionDescription(ex);

                        content.AppendLine(desc);
                    }
                }
            }

            bool first = true;

            foreach (var entityLink in commonEntityMetadata.OrderBy(e => e.Entity1.LogicalName))
            {
                var query = from source in entityLink.Entity1.Attributes
                            join target in entityLink.Entity2.Attributes on source.LogicalName equals target.LogicalName
                            where source.IsAuditEnabled != null
                                    && string.IsNullOrEmpty(source.AttributeOf)
                                    && string.IsNullOrEmpty(target.AttributeOf)
                                    && source.IsAuditEnabled.Value
                                    && target.IsAuditEnabled != null
                                    && target.IsAuditEnabled.CanBeChanged
                                    && target.IsAuditEnabled.Value == false
                            orderby target.LogicalName
                            select target;

                foreach (var attribute in query)
                {
                    if (first)
                    {
                        content
                            .AppendLine()
                            .AppendLine("Enabling Audit on Attributes:")
                            .AppendLine();

                        first = false;
                    }

                    content
                        .AppendFormat(_tabSpacer + "{0}.{1}", attribute.EntityLogicalName, attribute.LogicalName)
                        .AppendLine();

                    entitiesToPublish.Add(attribute.EntityLogicalName);

                    try
                    {
                        attribute.IsAuditEnabled.Value = true;

                        await repositoryTarget.UpdateAttributeMetadataAsync(attribute);
                    }
                    catch (Exception ex)
                    {
                        var desc = DTEHelper.GetExceptionDescription(ex);

                        content.AppendLine(desc);
                    }
                }
            }

            if (entitiesToPublish.Any())
            {
                content
                    .AppendLine()
                    .AppendFormat("Publish Entities: {0}", entitiesToPublish.Count)
                    .AppendLine();

                foreach (var item in entitiesToPublish.OrderBy(s => s))
                {
                    content.AppendLine(_tabSpacer + item);
                }

                var repositoryPublish = new PublishActionsRepository(_serviceTarget);

                try
                {
                    await repositoryPublish.PublishEntitiesAsync(entitiesToPublish);
                }
                catch (Exception ex)
                {
                    var desc = DTEHelper.GetExceptionDescription(ex);

                    content.AppendLine(desc);
                }
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = string.Format("OrgTransfer Audit from {0} to {1} at {2}.txt"
                , this.ConnectionSource.Name
                , this.ConnectionTarget.Name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }
    }
}
