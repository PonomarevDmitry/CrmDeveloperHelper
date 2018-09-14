using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class OrganizationComparer
    {
        public Task<string> CheckEntitiesAsync()
        {
            return Task.Run(() => CheckEntities());
        }

        private async Task<string> CheckEntities()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_writeToOutput, content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Entities started at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            var listEntityMetadata1 = _comparerSource.GetEntityMetadataCollection1();

            content.AppendLine(_writeToOutput.WriteToOutput("Entities in {0}: {1}", _comparerSource.Connection1.Name, listEntityMetadata1.Count()));

            var listEntityMetadata2 = _comparerSource.GetEntityMetadataCollection2();

            content.AppendLine(_writeToOutput.WriteToOutput("Entities in {0}: {1}", _comparerSource.Connection2.Name, listEntityMetadata2.Count()));

            var entityMetadataOnlyExistsIn1 = new FormatTextTableHandler();
            entityMetadataOnlyExistsIn1.SetHeader("EntityName", "IsManaged");

            var entityMetadataOnlyExistsIn2 = new FormatTextTableHandler();
            entityMetadataOnlyExistsIn2.SetHeader("EntityName", "IsManaged");

            Dictionary<string, List<string>> entityMetadataDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            var commonEntityMetadata = new List<LinkedEntities<EntityMetadata>>();

            foreach (var entityMetadata1 in listEntityMetadata1.OrderBy(e => e.LogicalName))
            {
                {
                    var entityMetadata2 = listEntityMetadata2.FirstOrDefault(e => e.LogicalName == entityMetadata1.LogicalName);

                    if (entityMetadata2 != null)
                    {
                        commonEntityMetadata.Add(new LinkedEntities<EntityMetadata>(entityMetadata1, entityMetadata2));
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn1.AddLine(entityMetadata1.LogicalName, entityMetadata1.IsManaged.ToString());
            }

            foreach (var entityMetadata2 in listEntityMetadata2.OrderBy(e => e.LogicalName))
            {
                {
                    var entityMetadata1 = listEntityMetadata1.FirstOrDefault(e => e.LogicalName == entityMetadata2.LogicalName);

                    if (entityMetadata1 != null)
                    {
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn2.AddLine(entityMetadata2.LogicalName, entityMetadata2.IsManaged.ToString());
            }

            var listNotExists = new HashSet<string>(listEntityMetadata1.Select(e => e.LogicalName).Union(listEntityMetadata2.Select(e => e.LogicalName)), StringComparer.OrdinalIgnoreCase);

            content.AppendLine(_writeToOutput.WriteToOutput("Common Entities in {0} and {1}: {2}", _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, commonEntityMetadata.Count()));

            var optionSetComparer = new OptionSetComparer(tabSpacer, Connection1.Name, Connection2.Name, new StringMapRepository(_comparerSource.Service1), new StringMapRepository(_comparerSource.Service2));

            EntityMetadataComparer comparer = new EntityMetadataComparer(tabSpacer, _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, optionSetComparer, listNotExists);

            {
                var reporter = new ProgressReporter(_writeToOutput, commonEntityMetadata.Count, 5, "Processing Common Entities");

                foreach (var commonEntity in commonEntityMetadata.OrderBy(e => e.Entity1.LogicalName))
                {
                    reporter.Increase();

                    List<string> strDifference = comparer.GetDifference(commonEntity.Entity1, commonEntity.Entity2);

                    if (strDifference.Count > 0)
                    {
                        entityMetadataDifference.Add(commonEntity.Entity1.LogicalName, strDifference.Select(s => tabSpacer + s).ToList());
                    }
                }
            }

            if (entityMetadataOnlyExistsIn1.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat("Entities ONLY EXISTS in {0}: {1}", _comparerSource.Connection1.Name, entityMetadataOnlyExistsIn1.Count);

                entityMetadataOnlyExistsIn1.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Entities ONLY EXISTS in {0}: {1}", _comparerSource.Connection2.Name, entityMetadataOnlyExistsIn2.Count);

                entityMetadataOnlyExistsIn2.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataDifference.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat("Entities DIFFERENT in {0} and {1}: {2}", _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, entityMetadataDifference.Count);

                foreach (var item in entityMetadataDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());
                }

                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Entities DIFFERENT Details in {0} and {1}: {2}", _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, entityMetadataDifference.Count);

                foreach (var item in entityMetadataDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());

                    foreach (var str in item.Value)
                    {
                        content.AppendLine().Append((tabSpacer + str).TrimEnd());
                    }

                    content
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(new string('-', 150));
                }
            }

            if (entityMetadataOnlyExistsIn2.Count == 0
                && entityMetadataOnlyExistsIn1.Count == 0
                && entityMetadataDifference.Count == 0
                )
            {
                content.AppendLine("No difference in Entities.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Entities ended at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            string fileName = string.Format("OrgCompare {0} at {1} Entities.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        public Task<string> CheckEntityLabelsAsync()
        {
            return Task.Run(() => CheckEntityLabels());
        }

        private async Task<string> CheckEntityLabels()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_writeToOutput, content);

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Entity Labels started at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            var listEntityMetadata1 = this._comparerSource.GetEntityMetadataCollection1();

            content.AppendLine(_writeToOutput.WriteToOutput("Entities in {0}: {1}", _comparerSource.Connection1.Name, listEntityMetadata1.Count()));

            var listEntityMetadata2 = this._comparerSource.GetEntityMetadataCollection2();

            content.AppendLine(_writeToOutput.WriteToOutput("Entities in {0}: {1}", _comparerSource.Connection2.Name, listEntityMetadata2.Count()));

            var entityMetadataOnlyExistsIn1 = new FormatTextTableHandler();
            entityMetadataOnlyExistsIn1.SetHeader("EntityName", "IsManaged");

            var entityMetadataOnlyExistsIn2 = new FormatTextTableHandler();
            entityMetadataOnlyExistsIn2.SetHeader("EntityName", "IsManaged");

            Dictionary<string, List<string>> entityMetadataDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            var commonEntityMetadata = new List<LinkedEntities<EntityMetadata>>();

            foreach (var entityMetadata1 in listEntityMetadata1.OrderBy(e => e.LogicalName))
            {
                {
                    var entityMetadata2 = listEntityMetadata2.FirstOrDefault(e => e.LogicalName == entityMetadata1.LogicalName);

                    if (entityMetadata2 != null)
                    {
                        commonEntityMetadata.Add(new LinkedEntities<EntityMetadata>(entityMetadata1, entityMetadata2));
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn1.AddLine(entityMetadata1.LogicalName, entityMetadata1.IsManaged.ToString());
            }

            foreach (var entityMetadata2 in listEntityMetadata2.OrderBy(e => e.LogicalName))
            {
                {
                    var entityMetadata1 = listEntityMetadata1.FirstOrDefault(e => e.LogicalName == entityMetadata2.LogicalName);

                    if (entityMetadata1 != null)
                    {
                        continue;
                    }
                }

                entityMetadataOnlyExistsIn2.AddLine(entityMetadata2.LogicalName, entityMetadata2.IsManaged.ToString());
            }

            var listNotExists = new HashSet<string>(listEntityMetadata1.Select(e => e.LogicalName).Union(listEntityMetadata2.Select(e => e.LogicalName)), StringComparer.OrdinalIgnoreCase);

            content.AppendLine(_writeToOutput.WriteToOutput("Common Entities in {0} and {1}: {2}", _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, commonEntityMetadata.Count()));

            var optionSetComparer = new OptionSetComparer(tabSpacer, Connection1.Name, Connection2.Name, new StringMapRepository(_comparerSource.Service1), new StringMapRepository(_comparerSource.Service2));

            var comparer = new EntityLabelComparer(tabSpacer, _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, optionSetComparer, listNotExists);

            {
                var reporter = new ProgressReporter(_writeToOutput, commonEntityMetadata.Count, 5, "Processing Common Entities");

                foreach (var commonEntity in commonEntityMetadata.OrderBy(e => e.Entity1.LogicalName))
                {
                    List<string> strDifference = comparer.GetDifference(commonEntity.Entity1, commonEntity.Entity2);

                    if (strDifference.Count > 0)
                    {
                        entityMetadataDifference.Add(commonEntity.Entity1.LogicalName, strDifference.Select(s => tabSpacer + s).ToList());
                    }
                }
            }

            if (entityMetadataOnlyExistsIn1.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat("Entities ONLY EXISTS in {0}: {1}", _comparerSource.Connection1.Name, entityMetadataOnlyExistsIn1.Count);

                entityMetadataOnlyExistsIn1.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Entities ONLY EXISTS in {0}: {1}", _comparerSource.Connection2.Name, entityMetadataOnlyExistsIn2.Count);

                entityMetadataOnlyExistsIn2.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (entityMetadataDifference.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Entities DIFFERENT in {0} and {1}: {2}", _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, entityMetadataDifference.Count);

                foreach (var item in entityMetadataDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());
                }

                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Entities DIFFERENT Details in {0} and {1}: {2}", _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, entityMetadataDifference.Count);

                foreach (var item in entityMetadataDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());

                    foreach (var str in item.Value)
                    {
                        content.AppendLine().Append((tabSpacer + str).TrimEnd());
                    }

                    content
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(new string('-', 150));
                }
            }

            if (entityMetadataOnlyExistsIn2.Count == 0
                && entityMetadataOnlyExistsIn1.Count == 0
                && entityMetadataDifference.Count == 0
                )
            {
                content.AppendLine("No difference in Entities.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Entity Labels ended at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            string fileName = string.Format("OrgCompare {0} at {1} Entity Labels.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        public Task<string> CheckEntityMapsAsync()
        {
            return Task.Run(async () => await CheckEntityMaps());
        }

        private async Task<string> CheckEntityMaps()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_writeToOutput, content);

            content.AppendLine(_writeToOutput.WriteToOutput("Entity Maps started at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            var task1 = _comparerSource.GetEntityMap1Async();
            var task2 = _comparerSource.GetEntityMap2Async();

            var taskAttr1 = _comparerSource.GetAttributeMap1Async();
            var taskAttr2 = _comparerSource.GetAttributeMap2Async();

            var list1 = await task1;

            content.AppendLine(_writeToOutput.WriteToOutput("Entity Maps in {0}: {1}", _comparerSource.Connection1.Name, list1.Count()));

            var list2 = await task2;

            content.AppendLine(_writeToOutput.WriteToOutput("Entity Maps in {0}: {1}", _comparerSource.Connection2.Name, list2.Count()));

            //new OrderExpression("sourceentityname", OrderType.Ascending),
            //new OrderExpression("targetentityname", OrderType.Ascending),

            var listPermission1 = await taskAttr1;

            content.AppendLine(_writeToOutput.WriteToOutput("Attribute Maps in {0}: {1}", _comparerSource.Connection1.Name, listPermission1.Count()));

            var listPermission2 = await taskAttr2;

            content.AppendLine(_writeToOutput.WriteToOutput("Attribute Maps in {0}: {1}", _comparerSource.Connection2.Name, listPermission2.Count()));

            var group1 = listPermission1.GroupBy(e => e.EntityMapId.Id);
            var group2 = listPermission2.GroupBy(e => e.EntityMapId.Id);

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Source", "", "Target", "IsManaged");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Source", "", "Target", "IsManaged");

            var dictDifference = new Dictionary<Tuple<string, string>, List<string>>();

            foreach (var entityMap1 in list1)
            {
                var source1 = entityMap1.SourceEntityName;
                var target1 = entityMap1.TargetEntityName;

                {
                    var entityMap2 = list2.FirstOrDefault(entityMap =>
                    {
                        var source2 = entityMap.SourceEntityName;
                        var target2 = entityMap.TargetEntityName;

                        return source1 == source2 && target1 == target2;
                    });

                    if (entityMap2 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn1.AddLine(source1, "->", target1, entityMap1.IsManaged.ToString());
            }

            foreach (var entityMap2 in list2)
            {
                var source2 = entityMap2.SourceEntityName;
                var target2 = entityMap2.TargetEntityName;

                {
                    var entityMap1 = list1.FirstOrDefault(entityMap =>
                    {
                        var source1 = entityMap.SourceEntityName;
                        var target1 = entityMap.TargetEntityName;

                        return source1 == source2 && target1 == target2;
                    });

                    if (entityMap1 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn2.AddLine(source2, "->", target2, entityMap2.IsManaged.ToString());
            }

            foreach (var entityMap1 in list1)
            {
                var source1 = entityMap1.SourceEntityName;
                var target1 = entityMap1.TargetEntityName;

                var entityMap2 = list2.FirstOrDefault(entityMap =>
                {
                    var source2 = entityMap.SourceEntityName;
                    var target2 = entityMap.TargetEntityName;

                    return source1 == source2 && target1 == target2;
                });

                if (entityMap2 == null)
                {
                    continue;
                }

                IEnumerable<AttributeMap> enumerable1 = group1.FirstOrDefault(g => g.Key == entityMap1.Id);
                IEnumerable<AttributeMap> enumerable2 = group2.FirstOrDefault(g => g.Key == entityMap2.Id);

                List<string> diff = CompareAttributeMap(enumerable1, enumerable2, tabSpacer);

                if (diff.Count > 0)
                {
                    dictDifference.Add(Tuple.Create(source1, target1), diff);
                }
            }

            if (tableOnlyExistsIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Entity Maps ONLY EXISTS in {0}: {1}", _comparerSource.Connection1.Name, tableOnlyExistsIn1.Count);

                tableOnlyExistsIn1.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Entity Maps ONLY EXISTS in {0}: {1}", _comparerSource.Connection2.Name, tableOnlyExistsIn2.Count);

                tableOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (dictDifference.Count > 0)
            {
                var order = dictDifference.OrderBy(s => s.Key.Item1).ThenBy(s => s.Key.Item2);

                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat("Entity Maps DIFFERENT in {0} and {1}: {2}", _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, dictDifference.Count);

                {
                    var table = new FormatTextTableHandler();

                    foreach (var item in order)
                    {
                        table.AddLine(item.Key.Item1, "->", item.Key.Item2);
                    }

                    table.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
                }


                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat("Entity Maps DIFFERENT Details in {0} and {1}: {2}", _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, dictDifference.Count);

                foreach (var item in order)
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + string.Format("{0} -> {1}", item.Key.Item1, item.Key.Item2)).TrimEnd());

                    foreach (var str in item.Value)
                    {
                        content.AppendLine().Append((tabSpacer + tabSpacer + str).TrimEnd());
                    }

                    content
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(new string('-', 150));
                }
            }

            if (tableOnlyExistsIn2.Count == 0
                && tableOnlyExistsIn1.Count == 0
                && dictDifference.Count == 0
                )
            {
                content.AppendLine("No difference in Entity Maps.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Entity Maps ended at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)));

            string fileName = string.Format("OrgCompare {0} at {1} Entity Maps.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        private List<string> CompareAttributeMap(IEnumerable<AttributeMap> enumerable1, IEnumerable<AttributeMap> enumerable2, string tabSpacer)
        {
            List<string> diff = new List<string>();

            FormatTextTableHandler tableOnlyIn1 = new FormatTextTableHandler();
            tableOnlyIn1.SetHeader("SourceAttribute", "", "TargetAttribute", "IsSystem", "IsManaged");

            FormatTextTableHandler tableOnlyIn2 = new FormatTextTableHandler();
            tableOnlyIn2.SetHeader("SourceAttribute", "", "TargetAttribute", "IsSystem", "IsManaged");

            if (enumerable1 != null)
            {
                foreach (var item1 in enumerable1)
                {
                    var source1 = item1.SourceAttributeName;
                    var target1 = item1.TargetAttributeName;

                    if (enumerable2 != null)
                    {
                        var item2 = enumerable2.FirstOrDefault(i =>
                            i.SourceAttributeName == source1
                            && i.TargetAttributeName == target1
                            );

                        if (item2 != null)
                        {
                            continue;
                        }
                    }

                    tableOnlyIn1.AddLine(source1, "->", target1
                        , item1.IsSystem.ToString()
                        , item1.IsManaged.ToString()
                        );

                    tableOnlyIn2.CalculateLineLengths(source1, "->", target1
                        , item1.IsSystem.ToString()
                        , item1.IsManaged.ToString()
                        );
                }
            }

            if (enumerable2 != null)
            {
                foreach (var item2 in enumerable2)
                {
                    var source2 = item2.SourceAttributeName;
                    var target2 = item2.TargetAttributeName;

                    if (enumerable1 != null)
                    {
                        var item1 = enumerable1.FirstOrDefault(i =>
                            i.SourceAttributeName == source2
                            && i.TargetAttributeName == target2
                            );

                        if (item1 != null)
                        {
                            continue;
                        }
                    }

                    tableOnlyIn2.AddLine(source2, "->", target2
                        , item2.IsSystem.ToString()
                        , item2.IsManaged.ToString()
                        );

                    tableOnlyIn1.CalculateLineLengths(source2, "->", target2
                        , item2.IsSystem.ToString()
                        , item2.IsManaged.ToString()
                        );
                }
            }

            if (tableOnlyIn1.Count > 0)
            {
                diff.Add(string.Format("Attribute Maps ONLY in {0}: {1}", _comparerSource.Connection1.Name, tableOnlyIn1.Count));
                tableOnlyIn1.GetFormatedLines(true).ForEach(s => diff.Add(tabSpacer + s));
            }

            if (tableOnlyIn2.Count > 0)
            {
                diff.Add(string.Format("Attribute Maps ONLY in {0}: {1}", _comparerSource.Connection2.Name, tableOnlyIn2.Count));
                tableOnlyIn2.GetFormatedLines(true).ForEach(s => diff.Add(tabSpacer + s));
            }

            return diff;
        }

        public Task<string> CheckRibbonsAsync(bool withDetails)
        {
            return Task.Run(() => CheckRibbons(withDetails));
        }

        private async Task<string> CheckRibbons(bool withDetails)
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_writeToOutput, content);

            var withDetailsName = withDetails ? " with details" : string.Empty;

            content.AppendLine(_writeToOutput.WriteToOutput("Checking Ribbons{0} started at {1}"
                , withDetailsName
                , DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture))
                );

            var task1 = _comparerSource.GetEntitiesWithRibbonCustomization1Async();
            var task2 = _comparerSource.GetEntitiesWithRibbonCustomization2Async();

            var listRibbon1 = await task1;

            content.AppendLine(_writeToOutput.WriteToOutput("Ribbons in {0}: {1}", _comparerSource.Connection1.Name, listRibbon1.Count()));

            var listRibbon2 = await task2;

            content.AppendLine(_writeToOutput.WriteToOutput("Ribbons in {0}: {1}", _comparerSource.Connection2.Name, listRibbon2.Count()));

            var entities1 = this._comparerSource.GetEntityMetadataCollection1();

            content.AppendLine(_writeToOutput.WriteToOutput("Enitites in {0}: {1}", _comparerSource.Connection1.Name, entities1.Count()));

            var entities2 = this._comparerSource.GetEntityMetadataCollection2();

            content.AppendLine(_writeToOutput.WriteToOutput("Enitites in {0}: {1}", _comparerSource.Connection2.Name, entities2.Count()));

            //var list1 = entities1.Select(e => e.LogicalName);
            //var list2 = entities2.Select(e => e.LogicalName);

            var list = listRibbon1.Union(listRibbon2).Distinct().OrderBy(s => s).ToList();

            content.AppendLine(_writeToOutput.WriteToOutput("Common Ribbons in {0} and {1}: {2}", _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, list.Count()));

            {
                var request = new RetrieveApplicationRibbonRequest();

                string xml1 = string.Empty;
                string xml2 = string.Empty;

                try
                {
                    var response1 = (RetrieveApplicationRibbonResponse)_comparerSource.Service1.Execute(request);
                    var response2 = (RetrieveApplicationRibbonResponse)_comparerSource.Service2.Execute(request);

                    var array1 = FileOperations.UnzipRibbon(response1.CompressedApplicationRibbonXml);
                    var array2 = FileOperations.UnzipRibbon(response2.CompressedApplicationRibbonXml);

                    xml1 = Encoding.UTF8.GetString(array1);
                    xml2 = Encoding.UTF8.GetString(array2);

                    xml1 = ContentCoparerHelper.RemoveDiacritics(xml1);
                    xml2 = ContentCoparerHelper.RemoveDiacritics(xml2);
                }
                catch (Exception ex)
                {
                    this._writeToOutput.WriteErrorToOutput(ex);
                }

                var compare = ContentCoparerHelper.CompareXML(xml1, xml2, withDetails);

                if (!compare.IsEqual)
                {
                    content.AppendLine().AppendLine("Application Ribbons are DIFFERENT.");

                    if (withDetails)
                    {
                        content.AppendFormat("Inserts {0}   InsertLength {1}   Deletes {2}    DeleteLength {3}    {4}"
                           , string.Format("+{0}", compare.Inserts)
                           , string.Format("(+{0})", compare.InsertLength)
                           , string.Format("-{0}", compare.Deletes)
                           , string.Format("(-{0})", compare.DeleteLength)
                           , compare.GetDescription()
                           );
                    }
                }
                else
                {
                    content.AppendLine().AppendLine("Application Ribbons are equal.");
                }
            }

            content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

            List<string> listOnlyExistsIn1 = new List<string>();

            List<string> listOnlyExistsIn2 = new List<string>();

            FormatTextTableHandler tableDifferent = new FormatTextTableHandler();
            if (withDetails)
            {
                tableDifferent.SetHeader("EntityName", "+Inserts", "(+Length)", "-Deletes", "(-Length)", "Note");
            }

            {
                var reporter = new ProgressReporter(_writeToOutput, list.Count, 5, "Processing Common Ribbons");

                foreach (var entityName in list)
                {
                    var entity1 = entities1.FirstOrDefault(e => e.LogicalName == entityName);
                    var entity2 = entities2.FirstOrDefault(e => e.LogicalName == entityName);

                    if (entity1 == null)
                    {
                        listOnlyExistsIn2.Add(entityName);
                    }

                    if (entity2 == null)
                    {
                        listOnlyExistsIn1.Add(entityName);
                    }

                    if (entity1 != null && entity2 != null)
                    {
                        reporter.Increase();

                        var request = new RetrieveEntityRibbonRequest()
                        {
                            RibbonLocationFilter = RibbonLocationFilters.All,
                            EntityName = entityName,
                        };

                        string xml1 = string.Empty;
                        string xml2 = string.Empty;

                        try
                        {
                            var response1 = (RetrieveEntityRibbonResponse)_comparerSource.Service1.Execute(request);
                            var response2 = (RetrieveEntityRibbonResponse)_comparerSource.Service2.Execute(request);

                            var array1 = FileOperations.UnzipRibbon(response1.CompressedEntityXml);
                            var array2 = FileOperations.UnzipRibbon(response2.CompressedEntityXml);

                            xml1 = Encoding.UTF8.GetString(array1);
                            xml2 = Encoding.UTF8.GetString(array2);

                            xml1 = ContentCoparerHelper.RemoveDiacritics(xml1);
                            xml2 = ContentCoparerHelper.RemoveDiacritics(xml2);
                        }
                        catch (Exception ex)
                        {
                            this._writeToOutput.WriteErrorToOutput(ex);
                        }

                        var compare = ContentCoparerHelper.CompareXML(xml1, xml2, withDetails);

                        if (!compare.IsEqual)
                        {
                            if (withDetails)
                            {
                                tableDifferent.AddLine(entityName
                                    , string.Format("+{0}", compare.Inserts)
                                    , string.Format("(+{0})", compare.InsertLength)
                                    , string.Format("-{0}", compare.Deletes)
                                    , string.Format("(-{0})", compare.DeleteLength)
                                    , compare.GetDescription()
                                    );
                            }
                            else
                            {
                                tableDifferent.AddLine(entityName);
                            }
                        }
                    }
                }
            }

            if (listOnlyExistsIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Entity Ribbon ONLY EXISTS in {0}: {1}", _comparerSource.Connection1.Name, listOnlyExistsIn1.Count);

                listOnlyExistsIn1.ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (listOnlyExistsIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Entity Ribbon ONLY EXISTS in {0}: {1}", _comparerSource.Connection2.Name, listOnlyExistsIn2.Count);

                listOnlyExistsIn2.ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableDifferent.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendLine().AppendLine().AppendFormat("Entity Ribbon DIFFERENT in {0} and {1}: {2}", _comparerSource.Connection1.Name, _comparerSource.Connection2.Name, tableDifferent.Count);

                tableDifferent.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (listOnlyExistsIn1.Count == 0
                && listOnlyExistsIn2.Count == 0
                && tableDifferent.Count == 0
                )
            {
                content.AppendLine("No difference in Entity Ribbon.");
            }

            content.AppendLine().AppendLine().AppendLine(_writeToOutput.WriteToOutput("Checking Ribbons{0} ended at {1}"
                , withDetailsName
                , DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture))
                );

            string fileName = string.Format("OrgCompare {0} at {1} Ribbons{2}.txt"
                , this._OrgOrgName
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                , withDetailsName
                );

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }
    }
}
