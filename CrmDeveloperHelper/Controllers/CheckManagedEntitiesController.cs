using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class CheckManagedEntitiesController : BaseController<IWriteToOutput>
    {
        private static string[] _entitiesWithManagedProperty =
        {
            "activitymimeattachment"

            , "appmodule"
            , "appmoduleroles"

            , "channelaccessprofile"
            , "channelaccessprofileentityaccesslevel"
            , "channelaccessprofilerule"
            , "channelaccessprofileruleitem"

            , "channelproperty"
            , "channelpropertygroup"

            , "connectionrole"

            , "convertrule"
            , "convertruleitem"

            , "customcontrol"
            , "customcontroldefaultconfig"
            , "customcontrolresource"

            //, "dependencyfeature"

            , "displaystring"
            , "displaystringmap"

            , "entitymap"
            , "attributemap"

            , "fieldsecurityprofile"
            , "fieldpermission"

            , "hierarchyrule"

            , "template"
            , "kbarticletemplate"
            , "contracttemplate"
            , "mailmergetemplate"

            , "knowledgesearchmodel"

            , "mobileofflineprofile"
            , "mobileofflineprofileitem"
            , "mobileofflineprofileitemassociation"

            , "organizationui"

            , "recommendationmodel"
            , "recommendationmodelmapping"

            , "report"
            , "reportcategory"
            , "reportentity"
            , "reportvisibility"

            , "ribboncommand"
            , "ribboncontextgroup"
            , "ribboncustomization"
            , "ribbondiff"
            , "ribbonrule"
            , "ribbontabtocommandmap"

            , "role"
            , "roleprivileges"

            , "routingrule"
            , "routingruleitem"

            , "systemform"
            , "savedquery"
            , "savedqueryvisualization"

            , "pluginassembly"
            , "plugintype"
            , "sdkmessageprocessingstep"
            , "sdkmessageprocessingstepimage"

            , "serviceendpoint"

            , "advancedsimilarityrule"
            , "similarityrule"

            , "sitemap"

            , "sla"
            , "slaitem"

            //, "syncattributemapping"
            //, "syncattributemappingprofile"

            , "textanalyticsentitymapping"
            , "topicmodelconfiguration"

            , "webresource"

            , "workflow"
            , "processtrigger"
        };

        public CheckManagedEntitiesController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        #region Проверка управляемых сущностей на неуправляемые изменения.

        public async Task ExecuteCheckingManagedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CheckingManagedEntitiesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckingManagedEntities(connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task CheckingManagedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var content = new StringBuilder();

                content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
                content.AppendLine(connectionData.GetConnectionDescription());
                content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

                var hasInfo = false;

                hasInfo |= CheckEntitiesMetadata(content, service);

                hasInfo |= CheckGlobalOptionSets(content, service);

                hasInfo |= await CheckManagedEntities(content, service);

                if (!hasInfo)
                {
                    content.AppendLine();
                    content.AppendFormat(Properties.OutputStrings.NoObjectsInCRMWereFounded).AppendLine();
                }

                commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

                string fileName = string.Format("{0}.Checking Managed Entities at {1}.txt"
                    , connectionData.Name
                    , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
        }

        private bool CheckGlobalOptionSets(StringBuilder content, IOrganizationServiceExtented service)
        {
            var request = new RetrieveAllOptionSetsRequest();

            var response = (RetrieveAllOptionSetsResponse)service.Execute(request);

            var wrongOptionSets = new Dictionary<OptionSetMetadataBase, List<string>>();

            {
                var optionSetList = response.OptionSetMetadata.Where(o => o.IsManaged.GetValueOrDefault() == true);

                var reporter = new ProgressReporter(_iWriteToOutput, optionSetList.Count(), 5, "Processing Global OptionSets");

                foreach (var optionSet in optionSetList.OrderBy(e => e.Name))
                {
                    reporter.Increase();

                    List<string> unmanagedProperties = null;

                    if (optionSet is BooleanOptionSetMetadata)
                    {
                        unmanagedProperties = GetUnmanagedProperties(optionSet as BooleanOptionSetMetadata);
                    }

                    if (optionSet is OptionSetMetadata)
                    {
                        unmanagedProperties = GetUnmanagedProperties(optionSet as OptionSetMetadata);
                    }

                    if (unmanagedProperties != null && unmanagedProperties.Any())
                    {
                        wrongOptionSets.Add(optionSet, unmanagedProperties);
                    }
                }
            }

            if (wrongOptionSets.Any())
            {
                string[] headers = { "Name", "OptionSetType", "IsCustomOptionSet", "IsManaged" };

                {
                    content
                        .AppendLine()
                        .AppendLine(new string('-', 150))
                        .AppendLine();

                    var table = new FormatTextTableHandler(true);
                    table.SetHeader(headers);

                    foreach (var optionSet in wrongOptionSets.Keys.OrderBy(a => a.Name))
                    {
                        table.AddLine(
                            optionSet.Name
                            , optionSet.OptionSetType.ToString()
                            , optionSet.IsCustomOptionSet.ToString()
                            , optionSet.IsManaged.ToString()
                            );
                    }

                    content.AppendLine();

                    content.AppendLine(string.Format("Global OptionSets with Unmanaged elements: {0}", wrongOptionSets.Count));

                    table.GetFormatedLines(false).ForEach(s => content.Append(_tabSpacer).AppendLine(s));
                }

                {
                    var list = wrongOptionSets.Where(s => s.Value.Any());

                    if (list.Any())
                    {
                        content.AppendLine();

                        content.AppendLine(string.Format("Global OptionSets with Unmanaged elements with details: {0}", list.Count()));

                        foreach (var optionSet in list.Select(s => s.Key).OrderBy(a => a.Name))
                        {
                            content
                                .AppendLine()
                                .AppendLine(new string('-', 150))
                                .AppendLine();

                            var table = new FormatTextTableHandler(true);
                            table.SetHeader(headers);

                            table.AddLine(
                                optionSet.Name
                                , optionSet.OptionSetType.ToString()
                                , optionSet.IsCustomOptionSet.ToString()
                                , optionSet.IsManaged.ToString()
                            );

                            table.GetFormatedLines(false).ForEach(s => content.Append(_tabSpacer).AppendLine(s));

                            var unmanagedProperties = wrongOptionSets[optionSet];

                            if (unmanagedProperties.Any())
                            {
                                foreach (var str in unmanagedProperties)
                                {
                                    content.Append(_tabSpacer).Append(_tabSpacer).AppendLine(str);
                                }
                            }
                        }
                    }
                }
            }

            return wrongOptionSets.Any();
        }

        private bool CheckEntitiesMetadata(StringBuilder content, IOrganizationServiceExtented service)
        {
            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression() { AllProperties = true },

                LabelQuery = new LabelQueryExpression(),

                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression() { AllProperties = true },
                },

                RelationshipQuery = new RelationshipQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression() { AllProperties = true },
                },

                Criteria =
                {
                    Conditions =
                    {
                        new MetadataConditionExpression("IsManaged", MetadataConditionOperator.Equals, true),
                    },
                },
            };

            var isEntityKeyExists = service.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

            if (isEntityKeyExists)
            {
                entityQueryExpression.KeyQuery = new EntityKeyQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression() { AllProperties = true }
                };
            }

            var response = (RetrieveMetadataChangesResponse)service.Execute(
                new RetrieveMetadataChangesRequest()
                {
                    ClientVersionStamp = null,
                    Query = entityQueryExpression,
                }
            );

            var wrongEntities = new Dictionary<EntityMetadata, List<string>>();

            {
                var reporter = new ProgressReporter(_iWriteToOutput, response.EntityMetadata.Count, 5, "Processing Entities Metadata");

                foreach (EntityMetadata currentEntity in response.EntityMetadata)
                {
                    reporter.Increase();

                    var unmanagedProperties = GetUnmanagedProperties(currentEntity);

                    if (unmanagedProperties.Any())
                    {
                        wrongEntities.Add(currentEntity, unmanagedProperties);
                    }
                }
            }

            if (wrongEntities.Any())
            {
                content
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine();

                string[] headers = { "LogicalName", "IsCustomEntity", "IsManaged" };

                {
                    var table = new FormatTextTableHandler(true);
                    table.SetHeader(headers);

                    foreach (var entity in wrongEntities.Keys.OrderBy(s => s.LogicalName))
                    {
                        table.AddLine(
                            entity.LogicalName
                            , entity.IsCustomEntity.ToString()
                            , entity.IsManaged.ToString()
                            );
                    }

                    content.AppendLine();

                    content.AppendLine(string.Format("Entities with Unmanaged elements: {0}", wrongEntities.Count));

                    table.GetFormatedLines(false).ForEach(s => content.Append(_tabSpacer).AppendLine(s));
                }

                {
                    var list = wrongEntities.Where(s => s.Value.Any());

                    if (list.Any())
                    {
                        content.AppendLine();

                        content.AppendLine(string.Format("Entities with Unmanaged elements with details: {0}", wrongEntities.Count));

                        foreach (var entity in wrongEntities.Keys.OrderBy(a => a.LogicalName))
                        {
                            content
                                .AppendLine()
                                .AppendLine(new string('-', 150))
                                .AppendLine();

                            var table = new FormatTextTableHandler(true);
                            table.SetHeader(headers);

                            table.AddLine(
                               entity.LogicalName
                               , entity.IsCustomEntity.ToString()
                               , entity.IsManaged.ToString()
                           );

                            table.GetFormatedLines(false).ForEach(s => content.Append(_tabSpacer).AppendLine(s));

                            var unmanagedProperties = wrongEntities[entity];

                            if (unmanagedProperties.Any())
                            {
                                foreach (var str in unmanagedProperties)
                                {
                                    content.Append(_tabSpacer).Append(_tabSpacer).AppendLine(str);
                                }
                            }
                        }
                    }
                }
            }

            return wrongEntities.Any();
        }

        private static void CompareAttributes(List<string> result, IEnumerable<AttributeMetadata> attributes)
        {
            var wrongEntityAttributes = new Dictionary<AttributeMetadata, List<string>>();

            foreach (var currentAttribute in attributes.Where(a => a.IsCustomAttribute == true && a.AttributeOf == null))
            {
                if (currentAttribute.IsManaged.GetValueOrDefault() == false)
                {
                    wrongEntityAttributes.Add(currentAttribute, new List<string>());
                }
                else
                {
                    List<string> unmanagedProperties = GetUnmanagedProperties(currentAttribute);

                    if (unmanagedProperties.Any())
                    {
                        wrongEntityAttributes.Add(currentAttribute, unmanagedProperties);
                    }
                }
            }

            if (wrongEntityAttributes.Any())
            {
                string[] headers = { "LogicalName", "TypeName", "AttributeType", "IsCustomAttribute", "IsManaged", "Target" };

                {
                    var table = new FormatTextTableHandler(true);
                    table.SetHeader(headers);

                    foreach (var attr in wrongEntityAttributes.Keys.OrderBy(s => s.LogicalName))
                    {
                        var listStr = new List<string>()
                        {
                            attr.LogicalName
                            , attr.GetType().Name
                            , attr.AttributeType.ToString()
                            , attr.IsCustomAttribute.ToString()
                            , attr.IsManaged.ToString()
                        };

                        if (attr is LookupAttributeMetadata)
                        {
                            listStr.Add(string.Join(",", (attr as LookupAttributeMetadata).Targets.OrderBy(s => s)));
                        }

                        table.AddLine(listStr.ToArray());
                    }

                    if (result.Count > 0) { result.Add(string.Empty); }

                    result.Add(string.Format("Attributes with Unmanaged elements: {0}", wrongEntityAttributes.Count));

                    table.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));
                }

                {
                    var list = wrongEntityAttributes.Where(s => s.Value.Any());

                    if (list.Any())
                    {
                        if (result.Count > 0) { result.Add(string.Empty); }

                        result.Add(string.Format("Attributes with Unmanaged elements with details: {0}", list.Count()));

                        foreach (var attr in list.Select(s => s.Key).OrderBy(a => a.LogicalName))
                        {
                            var table = new FormatTextTableHandler(true);
                            table.SetHeader(headers);

                            var listStr = new List<string>()
                            {
                                attr.LogicalName
                                , attr.GetType().Name
                                , attr.AttributeType.ToString()
                                , attr.IsCustomAttribute.ToString()
                                , attr.IsManaged.ToString()
                            };

                            if (attr is LookupAttributeMetadata)
                            {
                                listStr.Add(string.Join(",", (attr as LookupAttributeMetadata).Targets.OrderBy(s => s)));
                            }

                            table.AddLine(listStr.ToArray());

                            if (result.Count > 0) { result.Add(string.Empty); }

                            table.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));

                            var unmanagedProperties = wrongEntityAttributes[attr];

                            if (unmanagedProperties.Any())
                            {
                                foreach (var str in unmanagedProperties)
                                {
                                    result.Add(_tabSpacer + _tabSpacer + str);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void CompareOneToMany(List<string> result, IEnumerable<OneToManyRelationshipMetadata> relations, string className, string relationTypeName)
        {
            var wrongEntityRelationshipsManyToOne = new Dictionary<OneToManyRelationshipMetadata, List<string>>();

            foreach (var currentRelationship in relations.Where(r => r.IsCustomRelationship == true))
            {
                if (currentRelationship.IsManaged.GetValueOrDefault() == false)
                {
                    wrongEntityRelationshipsManyToOne.Add(currentRelationship, new List<string>());
                }
                else
                {
                    List<string> unmanagedProperties = GetUnmanagedProperties(currentRelationship);

                    if (unmanagedProperties.Any())
                    {
                        wrongEntityRelationshipsManyToOne.Add(currentRelationship, unmanagedProperties);
                    }
                }
            }

            if (wrongEntityRelationshipsManyToOne.Any())
            {
                string[] headers = { "SchemaName", "ReferencingEntity", "ReferencingAttribute", "ReferencedEntity", "ReferencedAttribute", "IsCustomRelationship", "IsManaged" };

                {
                    var table = new FormatTextTableHandler(true);
                    table.SetHeader(headers);

                    foreach (var relation in wrongEntityRelationshipsManyToOne.Keys.OrderBy(s => s.SchemaName))
                    {
                        table.AddLine(
                            relation.SchemaName
                            , relation.ReferencingEntity
                            , relation.ReferencingAttribute
                            , relation.ReferencedEntity
                            , relation.ReferencedAttribute
                            , relation.IsCustomRelationship.ToString()
                            , relation.IsManaged.ToString()
                            );
                    }

                    if (result.Count > 0) { result.Add(string.Empty); }

                    result.Add(string.Format("{0} - {1} with Unmanaged elements: {2}", className, relationTypeName, wrongEntityRelationshipsManyToOne.Count));

                    table.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));
                }

                {
                    var list = wrongEntityRelationshipsManyToOne.Where(s => s.Value.Any());

                    if (list.Any())
                    {
                        if (result.Count > 0) { result.Add(string.Empty); }

                        result.Add(string.Format("{0} - {1} with Unmanaged elements with details: {2}", className, relationTypeName, list.Count()));

                        foreach (var relation in list.Select(s => s.Key).OrderBy(a => a.SchemaName))
                        {
                            var table = new FormatTextTableHandler(true);
                            table.SetHeader(headers);

                            table.AddLine(relation.SchemaName
                                , relation.ReferencingEntity
                                , relation.ReferencingAttribute
                                , relation.ReferencedEntity
                                , relation.ReferencedAttribute
                                , relation.IsCustomRelationship.ToString()
                                , relation.IsManaged.ToString());

                            table.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));

                            var unmanagedProperties = wrongEntityRelationshipsManyToOne[relation];

                            if (unmanagedProperties.Any())
                            {
                                foreach (var str in unmanagedProperties)
                                {
                                    result.Add(_tabSpacer + _tabSpacer + str);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void CompareManyToMany(List<string> result, IEnumerable<ManyToManyRelationshipMetadata> relations)
        {
            var wrongEntityRelationshipsManyToMany = new Dictionary<ManyToManyRelationshipMetadata, List<string>>();

            foreach (var currentRelationship in relations.Where(r => r.IsCustomRelationship == true))
            {
                if (currentRelationship.IsManaged.GetValueOrDefault() == false)
                {
                    wrongEntityRelationshipsManyToMany.Add(currentRelationship, new List<string>());
                }
                else
                {
                    List<string> unmanagedProperties = GetUnmanagedProperties(currentRelationship);

                    if (unmanagedProperties.Any())
                    {
                        wrongEntityRelationshipsManyToMany.Add(currentRelationship, unmanagedProperties);
                    }
                }
            }

            if (wrongEntityRelationshipsManyToMany.Any())
            {
                string[] headers = { "SchemaName", "IntersectEntityName", "Entity1LogicalName", "Entity1IntersectAttribute", "Entity2LogicalName", "Entity2IntersectAttribute", "IsCustomRelationship", "IsManaged" };

                {
                    var table = new FormatTextTableHandler(true);
                    table.SetHeader(headers);

                    foreach (var relation in wrongEntityRelationshipsManyToMany.Keys.OrderBy(s => s.SchemaName))
                    {
                        table.AddLine(
                             relation.SchemaName
                            , relation.IntersectEntityName
                            , relation.Entity1LogicalName
                            , relation.Entity1IntersectAttribute
                            , relation.Entity2LogicalName
                            , relation.Entity2IntersectAttribute
                            , relation.IsCustomRelationship.ToString()
                            , relation.IsManaged.ToString()
                            );
                    }

                    if (result.Count > 0) { result.Add(string.Empty); }

                    result.Add(string.Format("ManyToMany with Unmanaged elements: {0}", wrongEntityRelationshipsManyToMany.Count));

                    table.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));
                }

                {
                    var list = wrongEntityRelationshipsManyToMany.Where(s => s.Value.Any());

                    if (list.Any())
                    {
                        if (result.Count > 0) { result.Add(string.Empty); }

                        result.Add(string.Format("ManyToMany with Unmanaged elements with details: {0}", list.Count()));

                        foreach (var relation in list.Select(s => s.Key).OrderBy(a => a.SchemaName))
                        {
                            var table = new FormatTextTableHandler(true);
                            table.SetHeader(headers);

                            table.AddLine(
                                relation.SchemaName
                               , relation.IntersectEntityName
                               , relation.Entity1LogicalName
                               , relation.Entity1IntersectAttribute
                               , relation.Entity2LogicalName
                               , relation.Entity2IntersectAttribute
                               , relation.IsCustomRelationship.ToString()
                               , relation.IsManaged.ToString()
                               );

                            table.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));

                            var unmanagedProperties = wrongEntityRelationshipsManyToMany[relation];

                            if (unmanagedProperties.Any())
                            {
                                foreach (var str in unmanagedProperties)
                                {
                                    result.Add(_tabSpacer + _tabSpacer + str);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void CompareKeys(List<string> result, IEnumerable<EntityKeyMetadata> keys)
        {
            var wrongEntityKeys = new Dictionary<EntityKeyMetadata, List<string>>();

            foreach (var currentKey in keys)
            {
                if (currentKey.IsManaged.GetValueOrDefault() == false)
                {
                    wrongEntityKeys.Add(currentKey, new List<string>());
                }
                else
                {
                    List<string> unmanagedProperties = GetUnmanagedProperties(currentKey);

                    if (unmanagedProperties.Any())
                    {
                        wrongEntityKeys.Add(currentKey, unmanagedProperties);
                    }
                }
            }

            if (wrongEntityKeys.Any())
            {
                string[] headers = { "LogicalName", "SchemaName", "IsManaged", "KeyAttributes" };

                {
                    var table = new FormatTextTableHandler(true);
                    table.SetHeader(headers);

                    foreach (var key in wrongEntityKeys.Keys.OrderBy(s => s.LogicalName))
                    {
                        table.AddLine(
                            key.LogicalName
                            , key.SchemaName
                            , key.IsManaged.ToString()
                            , string.Join(",", key.KeyAttributes.OrderBy(s => s))
                            );
                    }

                    if (result.Count > 0) { result.Add(string.Empty); }

                    result.Add(string.Format("Keys with Unmanaged elements: {0}", wrongEntityKeys.Count));

                    table.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));
                }

                {
                    var list = wrongEntityKeys.Where(s => s.Value.Any());

                    if (list.Any())
                    {
                        if (result.Count > 0) { result.Add(string.Empty); }

                        result.Add(string.Format("Keys with Unmanaged elements with details: {0}", list.Count()));

                        foreach (var key in list.Select(s => s.Key).OrderBy(a => a.LogicalName))
                        {
                            var table = new FormatTextTableHandler(true);
                            table.SetHeader(headers);

                            table.AddLine(
                                key.LogicalName
                                , key.SchemaName
                                , key.IsManaged.ToString()
                                , string.Join(",", key.KeyAttributes.OrderBy(s => s))
                                );

                            table.GetFormatedLines(false).ForEach(s => result.Add(_tabSpacer + s));

                            var unmanagedProperties = wrongEntityKeys[key];

                            if (unmanagedProperties.Any())
                            {
                                foreach (var str in unmanagedProperties)
                                {
                                    result.Add(_tabSpacer + _tabSpacer + str);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static List<string> CompareLabels(Label label, string name)
        {
            List<string> result = new List<string>();

            if (label == null)
            {
                return result;
            }

            var unmanagedLabels = label.LocalizedLabels.Where(l => !string.IsNullOrEmpty(l.Label) && l.IsManaged.GetValueOrDefault() == false);

            if (unmanagedLabels.Any())
            {
                FormatTextTableHandler table = new FormatTextTableHandler(true);
                table.SetHeader("LanguageCode", "Value", "IsManaged");

                foreach (var item in unmanagedLabels.OrderBy(l => l.LanguageCode, LocaleComparer.Comparer))
                {
                    table.AddLine(LanguageLocale.GetLocaleName(item.LanguageCode), item.Label, item.IsManaged.ToString());
                }

                result.Add(string.Format("{0} with unmanaged labels: {1}", name, table.Count));
                table.GetFormatedLines(false).ForEach(str => result.Add(_tabSpacer + str));
            }

            return result;
        }

        private static List<string> GetUnmanagedProperties(EntityMetadata currentEntity)
        {
            List<string> result = new List<string>();

            result.AddRange(CompareLabels(currentEntity.DisplayName, "DisplayName"));
            if (result.Count > 0) { result.Add(string.Empty); }
            result.AddRange(CompareLabels(currentEntity.Description, "Description"));
            if (result.Count > 0) { result.Add(string.Empty); }
            result.AddRange(CompareLabels(currentEntity.DisplayCollectionName, "DisplayCollectionName"));

            CompareAttributes(result, currentEntity.Attributes);

            CompareOneToMany(result, currentEntity.ManyToOneRelationships, "N:1", "ManyToOne");

            CompareOneToMany(result, currentEntity.OneToManyRelationships, "1:N", "OneToMany");

            CompareManyToMany(result, currentEntity.ManyToManyRelationships);

            if (currentEntity.Keys != null)
            {
                CompareKeys(result, currentEntity.Keys);
            }

            return result;
        }

        private static List<string> GetUnmanagedProperties(EntityKeyMetadata currentKey)
        {
            List<string> result = new List<string>();

            result.AddRange(CompareLabels(currentKey.DisplayName, "DisplayName"));

            return result;
        }

        private static List<string> GetUnmanagedProperties(ManyToManyRelationshipMetadata currentRelationship)
        {
            List<string> result = new List<string>();

            if (currentRelationship.Entity1AssociatedMenuConfiguration != null)
            {
                if (result.Count > 0) { result.Add(string.Empty); }
                result.AddRange(CompareLabels(currentRelationship.Entity1AssociatedMenuConfiguration.Label, "Entity1AssociatedMenuConfiguration.Label"));
            }

            if (currentRelationship.Entity2AssociatedMenuConfiguration != null)
            {
                if (result.Count > 0) { result.Add(string.Empty); }
                result.AddRange(CompareLabels(currentRelationship.Entity2AssociatedMenuConfiguration.Label, "Entity2AssociatedMenuConfiguration.Label"));
            }

            return result;
        }

        private static List<string> GetUnmanagedProperties(OneToManyRelationshipMetadata currentRelationship)
        {
            List<string> result = new List<string>();

            if (currentRelationship.AssociatedMenuConfiguration != null)
            {
                result.AddRange(CompareLabels(currentRelationship.AssociatedMenuConfiguration.Label, "AssociatedMenuConfiguration.Label"));
            }

            return result;
        }

        private static List<string> GetUnmanagedProperties(AttributeMetadata currentAttribute)
        {
            List<string> result = new List<string>();

            result.AddRange(CompareLabels(currentAttribute.DisplayName, "DiplayName"));
            if (result.Count > 0) { result.Add(string.Empty); }
            result.AddRange(CompareLabels(currentAttribute.Description, "Description"));

            if (currentAttribute is Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata)
            {
                var boolAttrib = currentAttribute as Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata;

                if (boolAttrib.OptionSet != null)
                {
                    if (!CreateFileHandler.IgnoreAttribute(boolAttrib.EntityLogicalName, boolAttrib.LogicalName))
                    {
                        if (boolAttrib.OptionSet.IsManaged.GetValueOrDefault() == false && boolAttrib.OptionSet.IsCustomOptionSet.GetValueOrDefault() == true)
                        {
                            FillOptionSetInfo(result, boolAttrib.OptionSet);
                        }
                        else
                        {
                            List<string> unmanagedProperties = GetUnmanagedProperties(boolAttrib.OptionSet);

                            if (unmanagedProperties.Any())
                            {
                                FillOptionSetInfo(result, boolAttrib.OptionSet);

                                foreach (var str in unmanagedProperties)
                                {
                                    result.Add(_tabSpacer + str);
                                }
                            }
                        }
                    }
                }
            }

            if (currentAttribute is EnumAttributeMetadata enumAttributeMetadata
                && enumAttributeMetadata.OptionSet != null
            )
            {
                if (!CreateFileHandler.IgnoreAttribute(enumAttributeMetadata.EntityLogicalName, enumAttributeMetadata.LogicalName))
                {
                    if (enumAttributeMetadata.OptionSet.IsManaged.GetValueOrDefault() == false && enumAttributeMetadata.OptionSet.IsCustomOptionSet.GetValueOrDefault() == true)
                    {
                        FillOptionSetInfo(result, enumAttributeMetadata.OptionSet);
                    }
                    else
                    {
                        List<string> unmanagedProperties = GetUnmanagedProperties(enumAttributeMetadata.OptionSet);

                        if (unmanagedProperties.Any())
                        {
                            FillOptionSetInfo(result, enumAttributeMetadata.OptionSet);

                            foreach (var str in unmanagedProperties)
                            {
                                result.Add(_tabSpacer + str);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static void FillOptionSetInfo(List<string> result, OptionSetMetadataBase optionSet)
        {
            result.Add(string.Format("{0} {1} {2} OptionSet {3}"
                , optionSet.IsGlobal.GetValueOrDefault() ? "Global" : "Local"
                , optionSet.IsCustomOptionSet.GetValueOrDefault() ? "Custom" : "System"
                , optionSet.IsManaged.GetValueOrDefault() ? "Managed" : "Unmanaged"
                , optionSet.Name
            ));
        }

        private static List<string> GetUnmanagedProperties(BooleanOptionSetMetadata optionSet)
        {
            List<string> result = new List<string>();

            result.AddRange(CompareLabels(optionSet.DisplayName, "DisplayName"));
            if (result.Count > 0) { result.Add(string.Empty); }
            result.AddRange(CompareLabels(optionSet.Description, "Description"));

            if (optionSet.FalseOption != null)
            {
                if (result.Count > 0) { result.Add(string.Empty); }
                result.AddRange(CompareLabels(optionSet.FalseOption.Label, "OptionSet.FalseOption.Label"));
            }

            if (optionSet.TrueOption != null)
            {
                if (result.Count > 0) { result.Add(string.Empty); }
                result.AddRange(CompareLabels(optionSet.TrueOption.Label, "OptionSet.TrueOption.Label"));
            }

            return result;
        }

        private static List<string> GetUnmanagedProperties(OptionSetMetadata optionSet)
        {
            List<string> result = new List<string>();

            result.AddRange(CompareLabels(optionSet.DisplayName, "DisplayName"));
            if (result.Count > 0) { result.Add(string.Empty); }
            result.AddRange(CompareLabels(optionSet.Description, "Description"));

            var wrongOptionValue = new Dictionary<OptionMetadata, List<string>>();

            foreach (var option in optionSet.Options)
            {
                if (option.IsManaged.GetValueOrDefault() == false)
                {
                    wrongOptionValue.Add(option, new List<string>());
                }
                else
                {
                    List<string> unmanagedProperties = GetUnmanagedProperties(option);

                    if (unmanagedProperties.Any())
                    {
                        wrongOptionValue.Add(option, unmanagedProperties);
                    }
                }
            }

            if (wrongOptionValue.Any())
            {
                var table = new FormatTextTableHandler(true)
                {
                    Separator = "        "
                };
                foreach (var option in wrongOptionValue.Keys)
                {
                    table.CalculateLineLengths(GetOptionSetValueInfo(option));
                }

                foreach (var option in wrongOptionValue.Keys.OrderBy(o => o.Value))
                {
                    if (result.Count > 0) { result.Add(string.Empty); }

                    result.Add(table.FormatLine(GetOptionSetValueInfo(option)));

                    var unmanagedProperties = wrongOptionValue[option];

                    foreach (var str in unmanagedProperties)
                    {
                        result.Add(_tabSpacer + str);
                    }
                }
            }

            return result;
        }

        private static string[] GetOptionSetValueInfo(OptionMetadata option)
        {
            string optionSetLabel = CreateFileHandler.GetLocalizedLabel(option.Label);

            string[] result =
            {
                string.Format("OptionSetValue {0}", option.Value)
                , string.Format("Label {0}", optionSetLabel)
                , string.Format("IsManaged {0}", option.IsManaged)
            };

            return result;
        }

        private static List<string> GetUnmanagedProperties(OptionMetadata option)
        {
            var result = new List<string>();

            result.AddRange(CompareLabels(option.Label, "Label"));
            if (result.Count > 0) { result.Add(string.Empty); }
            result.AddRange(CompareLabels(option.Description, "Description"));

            return result;
        }

        private async Task<bool> CheckManagedEntities(StringBuilder content, IOrganizationServiceExtented service)
        {
            bool hasInfo = false;

            SolutionComponentDescriptor descriptor = new SolutionComponentDescriptor(service);

            descriptor.MetadataSource.DownloadEntityMetadataForNames(_entitiesWithManagedProperty, true);

            var list = _entitiesWithManagedProperty.Where(n => descriptor.MetadataSource.GetEntityMetadata(n) != null);

            {
                var reporter = new ProgressReporter(_iWriteToOutput, list.Count(), 5, "Processing Managed Entities");

                foreach (var entityName in list)
                {
                    var entityMetadata = descriptor.MetadataSource.GetEntityMetadata(entityName);

                    if (entityMetadata == null)
                    {
                        continue;
                    }

                    reporter.Increase();

                    var entitiesList = GetEntities(service, entityName);

                    if (entitiesList.Count == 0)
                    {
                        continue;
                    }

                    hasInfo = true;


                    var componentTypeName = entityName;

                    if (string.Equals(componentTypeName, "template", StringComparison.InvariantCultureIgnoreCase))
                    {
                        componentTypeName = "EmailTemplate";
                    }

                    if (Enum.TryParse<ComponentType>(componentTypeName, true, out ComponentType componentType))
                    {
                        content
                            .AppendLine()
                            .AppendLine(new string('-', 150))
                            .AppendLine();

                        var message = await descriptor.GetSolutionComponentsDescriptionAsync(entitiesList.Select(e => new SolutionComponent()
                        {
                            ObjectId = e.Id,
                            ComponentType = new OptionSetValue((int)componentType),
                        }).ToList());

                        content.AppendLine(message);
                    }
                    else
                    {
                        var message = GetOtherEntityDescription(entitiesList, entityName, entityMetadata);

                        content.AppendLine(message);
                    }
                }
            }

            return hasInfo;
        }

        private static ConcurrentDictionary<string, string[]> _dictionary = new ConcurrentDictionary<string, string[]>
        (
            new Dictionary<string, string[]>(StringComparer.InvariantCultureIgnoreCase)
            {
                { ActivityMimeAttachment.EntityLogicalName, new string[] { ActivityMimeAttachment.Schema.Attributes.objecttypecode, ActivityMimeAttachment.Schema.Attributes.objectid, ActivityMimeAttachment.Schema.Attributes.mimetype, ActivityMimeAttachment.Schema.Attributes.activitysubject, ActivityMimeAttachment.Schema.Attributes.filename, ActivityMimeAttachment.Schema.Attributes.subject, ActivityMimeAttachment.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { AdvancedSimilarityRule.EntityLogicalName, new string[] { AdvancedSimilarityRule.Schema.Attributes.entity, AdvancedSimilarityRule.Schema.Attributes.name, AdvancedSimilarityRule.Schema.Attributes.advancedsimilarityruleid, AdvancedSimilarityRule.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }



                , { AppModule.EntityLogicalName, new string[] { AppModule.Schema.Attributes.uniquename, AppModule.Schema.Attributes.name, AppModule.Schema.Attributes.url, AppModule.Schema.Attributes.appmoduleversion, AppModule.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { AppModuleRoles.EntityLogicalName, new string[] { AppModuleRoles.Schema.Attributes.appmoduleid, AppModuleRoles.Schema.Attributes.roleid, AppModuleRoles.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }



                , { ChannelAccessProfile.EntityLogicalName, new string[] { ChannelAccessProfile.Schema.Attributes.name, ChannelAccessProfile.Schema.Attributes.channelaccessprofileid, ChannelAccessProfile.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { ChannelAccessProfileEntityAccessLevel.EntityLogicalName, new string[] { ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileentityaccesslevelid, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccessleveldepthmask, ChannelAccessProfileEntityAccessLevel.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { ChannelAccessProfileRule.EntityLogicalName, new string[] { ChannelAccessProfileRule.Schema.Attributes.name, ChannelAccessProfileRule.Schema.Attributes.workflowid, ChannelAccessProfileRule.Schema.Attributes.channelaccessprofileruleid, ChannelAccessProfileRule.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { ChannelAccessProfileRuleItem.EntityLogicalName, new string[] { ChannelAccessProfileRuleItem.Schema.Attributes.associatedchannelaccessprofile, ChannelAccessProfileRuleItem.Schema.Attributes.channelaccessprofileruleid, ChannelAccessProfileRuleItem.Schema.Attributes.name, ChannelAccessProfileRuleItem.Schema.Attributes.sequencenumber, ChannelAccessProfileRuleItem.Schema.Attributes.channelaccessprofileruleitemid, ChannelAccessProfileRuleItem.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }



                , { ChannelProperty.EntityLogicalName, new string[] { ChannelProperty.Schema.Attributes.applicationsource, ChannelProperty.Schema.Attributes.datatype, ChannelProperty.Schema.Attributes.name, ChannelProperty.Schema.Attributes.regardingobjectid, ChannelProperty.Schema.Attributes.statecode, ChannelProperty.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { ChannelPropertyGroup.EntityLogicalName, new string[] { ChannelPropertyGroup.Schema.Attributes.regardingtypecode, ChannelPropertyGroup.Schema.Attributes.name, ChannelPropertyGroup.Schema.Attributes.statecode, ChannelPropertyGroup.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }



                , { CustomControlResource.EntityLogicalName, new string[] { "customcontrol.name", CustomControlResource.Schema.Attributes.name, "webresource.name", CustomControlResource.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }



                , { KnowledgeSearchModel.EntityLogicalName, new string[] { KnowledgeSearchModel.Schema.Attributes.entity, KnowledgeSearchModel.Schema.Attributes.sourceentity, KnowledgeSearchModel.Schema.Attributes.name, KnowledgeSearchModel.Schema.Attributes.statecode, KnowledgeSearchModel.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { MobileOfflineProfileItemAssociation.EntityLogicalName, new string[] { MobileOfflineProfileItemAssociation.Schema.Attributes.mobileofflineprofileitemid, MobileOfflineProfileItemAssociation.Schema.Attributes.name, MobileOfflineProfileItemAssociation.Schema.Attributes.relationshipname, MobileOfflineProfileItemAssociation.Schema.Attributes.selectedrelationshipsschema, MobileOfflineProfileItemAssociation.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { OrganizationUI.EntityLogicalName, new string[] { OrganizationUI.Schema.Attributes.objecttypecode, OrganizationUI.Schema.Attributes.fieldxml, OrganizationUI.Schema.Attributes.formid, OrganizationUI.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { ProcessTrigger.EntityLogicalName, new string[] { ProcessTrigger.Schema.Attributes.primaryentitytypecode, ProcessTrigger.Schema.Attributes.controltype, ProcessTrigger.Schema.Attributes.controlname, ProcessTrigger.Schema.Attributes.formid, ProcessTrigger.Schema.Attributes.@event, ProcessTrigger.Schema.Attributes.processtriggerid, ProcessTrigger.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }



                , { "recommendationmodel", new string[] { "name", "productcatalogname", "statecode", "recommendationmodelid", "ismanaged"
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { "recommendationmodelmapping", new string[] { "entity", "accountfield", "mappingtype", "productfield", "entitydisplayname", "recommendationmodelmappingid", "ismanaged"
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }



                , { TextAnalyticsEntityMapping.EntityLogicalName, new string[] { TextAnalyticsEntityMapping.Schema.Attributes.advancedsimilarityruleid, TextAnalyticsEntityMapping.Schema.Attributes.similarityruleid, TextAnalyticsEntityMapping.Schema.Attributes.entity, TextAnalyticsEntityMapping.Schema.Attributes.field, TextAnalyticsEntityMapping.Schema.Attributes.relationshipname, TextAnalyticsEntityMapping.Schema.Attributes.textanalyticsentitymappingid, TextAnalyticsEntityMapping.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }

                , { TopicModelConfiguration.EntityLogicalName, new string[] { TopicModelConfiguration.Schema.Attributes.sourceentity, TopicModelConfiguration.Schema.Attributes.name, TopicModelConfiguration.Schema.Attributes.topicmodelconfigurationid, TopicModelConfiguration.Schema.Attributes.ismanaged
                    , "solution.uniquename", "solution.ismanaged", "suppsolution.uniquename", "suppsolution.ismanaged" } }
            }
            , StringComparer.InvariantCultureIgnoreCase
        );

        private string GetOtherEntityDescription(List<Entity> entitiesList, string entityName, EntityMetadata entityMetadata)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0}: {1}", entityName, entitiesList.Count).AppendLine();

            if (_dictionary.ContainsKey(entityName))
            {
                var listAttributes = _dictionary[entityName];

                var table = new FormatTextTableHandler();

                {
                    List<string> headers = new List<string>();

                    foreach (var attrName in listAttributes)
                    {
                        switch (attrName)
                        {
                            case "solution.uniquename":
                                headers.Add("SolutionName");
                                break;

                            case "solution.ismanaged":
                                headers.Add("SolutionIsManaged");
                                break;

                            case "suppsolution.uniquename":
                                headers.Add("SupportingName");
                                break;

                            case "suppsolution.ismanaged":
                                headers.Add("SupportinIsManaged");
                                break;

                            default:
                                {
                                    var attr = entityMetadata.Attributes.FirstOrDefault(a => string.Equals(attrName, a.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                                    if (attr != null)
                                    {
                                        headers.Add(attr.SchemaName);
                                    }
                                    else
                                    {
                                        headers.Add(attrName);
                                    }
                                }
                                break;
                        }
                    }

                    table.SetHeader(headers.ToArray());
                }

                foreach (var itemEntity in entitiesList)
                {
                    var listValue = new List<string>();

                    foreach (var attrName in listAttributes)
                    {
                        string value = EntityDescriptionHandler.GetAttributeStringShortEntityReference(itemEntity, attrName);
                        listValue.Add(value);
                    }

                    table.AddLine(listValue.ToArray());
                }

                table.GetFormatedLines(true).ForEach(str => result.Append(_tabSpacer).AppendLine(str));

                return result.ToString();
            }

            if (string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute))
            {
                var table = new FormatTextTableHandler();
                table.SetHeader("Id", "IsManaged", "SolutionName");

                foreach (var item in entitiesList)
                {
                    table.AddLine(
                        item.Id.ToString()
                        , item.GetAttributeValue<bool?>("ismanaged").ToString()
                        , item.GetAttributeValue<AliasedValue>("solution.uniquename").Value.ToString()
                        );
                }

                table.GetFormatedLines(true).ForEach(str => result.Append(_tabSpacer).AppendLine(str));

                return result.ToString();
            }

            {
                var table = new FormatTextTableHandler();
                table.SetHeader(entityMetadata.PrimaryNameAttribute, entityMetadata.PrimaryIdAttribute, "IsManaged", "SolutionName");

                foreach (var item in entitiesList)
                {
                    table.AddLine(
                        item.Attributes[entityMetadata.PrimaryNameAttribute].ToString()
                        , item.Attributes[entityMetadata.PrimaryIdAttribute].ToString()
                        , item.GetAttributeValue<bool?>("ismanaged").ToString()
                        , item.GetAttributeValue<AliasedValue>("solution.uniquename").Value.ToString()
                        );
                }

                table.GetFormatedLines(true).ForEach(str => result.Append(_tabSpacer).AppendLine(str));
            }

            return result.ToString();
        }

        private List<Entity> GetEntities(IOrganizationServiceExtented service, string entityName)
        {
            QueryExpression query = GetQuery(entityName);

            query.PageInfo = new PagingInfo()
            {
                PageNumber = 1,
                Count = 5000,
            };

            var result = new List<Entity>();

            try
            {
                while (true)
                {
                    var coll = service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities);

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, entityName);
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            return result;
        }

        private static QueryExpression GetQuery(string entityName)
        {
            if (string.Equals(entityName, CustomControlResource.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase))
            {
                var query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = entityName,

                    ColumnSet = ColumnSetInstances.AllColumns,

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(CustomControlResource.Schema.Attributes.ismanaged, ConditionOperator.Equal, true),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            LinkFromEntityName = entityName,
                            LinkFromAttributeName = CustomControlResource.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename),

                            LinkCriteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(Solution.Schema.Attributes.ismanaged, ConditionOperator.Equal, false),
                                },
                            },
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = entityName,
                            LinkFromAttributeName = CustomControlResource.Schema.Attributes.customcontrolid,

                            LinkToEntityName = CustomControl.Schema.EntityLogicalName,
                            LinkToAttributeName = CustomControl.Schema.EntityPrimaryIdAttribute,

                            EntityAlias = CustomControl.Schema.EntityLogicalName,

                            Columns = new ColumnSet(CustomControl.Schema.Attributes.name),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = entityName,
                            LinkFromAttributeName = WebResource.EntityPrimaryIdAttribute,

                            LinkToEntityName = WebResource.EntityLogicalName,
                            LinkToAttributeName = WebResource.EntityPrimaryIdAttribute,

                            EntityAlias = WebResource.EntityLogicalName,

                            Columns = new ColumnSet(WebResource.Schema.Attributes.name),
                        },
                    },
                };

                return query;
            }

            {
                var query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = entityName,

                    ColumnSet = ColumnSetInstances.AllColumns,

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression("ismanaged", ConditionOperator.Equal, true),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            LinkFromEntityName = entityName,
                            LinkFromAttributeName = "solutionid",

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename),

                            LinkCriteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(Solution.Schema.Attributes.ismanaged, ConditionOperator.Equal, false),
                                },
                            },
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = entityName,
                            LinkFromAttributeName = "supportingsolutionid",

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },
                };

                return query;
            }
        }

        #endregion Проверка управляемых сущностей на неуправляемые изменения.
    }
}