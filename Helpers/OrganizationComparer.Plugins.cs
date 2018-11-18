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
        public Task<string> CheckPluginAssembliesAsync()
        {
            return Task.Run(async () => await CheckPluginAssemblies());
        }

        private async Task<string> CheckPluginAssemblies()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingPluginAssembliesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetPluginAssembly1Async();
            var task2 = _comparerSource.GetPluginAssembly2Async();

            var taskTypes1 = _comparerSource.GetPluginType1Async();
            var taskTypes2 = _comparerSource.GetPluginType2Async();

            var list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Assemblies in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Assemblies in {0}: {1}", Connection2.Name, list2.Count()));

            var listTypes1 = await taskTypes1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Types in {0}: {1}", Connection1.Name, listTypes1.Count()));

            var listTypes2 = await taskTypes2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Types in {0}: {1}", Connection2.Name, listTypes2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Name", "IsHidden", "IsManaged");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Name", "IsHidden", "IsManaged");

            Dictionary<string, List<string>> assemblyDifference = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var assem1 in list1)
            {
                {
                    var assem2 = list2.FirstOrDefault(e => e.Name == assem1.Name);

                    if (assem2 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn1.AddLine(assem1.Name, assem1.IsHidden.ToString(), assem1.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.PluginAssembly, assem1.Id);
            }

            foreach (var assem2 in list2)
            {
                {
                    var assem1 = list1.FirstOrDefault(e => e.Name == assem2.Name);

                    if (assem1 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn2.AddLine(assem2.Name, assem2.IsHidden.ToString(), assem2.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.PluginAssembly, assem2.Id);
            }

            foreach (var assem1 in list1)
            {
                var assem2 = list2.FirstOrDefault(e => e.Name == assem1.Name);

                if (assem2 == null)
                {
                    continue;
                }

                var types1 = listTypes1.Where(t => t.PluginAssemblyId.Id == assem1.PluginAssemblyId.Value);
                var types2 = listTypes2.Where(t => t.PluginAssemblyId.Id == assem2.PluginAssemblyId.Value);

                if (!types1.Select(t => t.TypeName).SequenceEqual(types2.Select(t => t.TypeName)))
                {
                    var only1Types = new HashSet<string>(types1.Select(t => t.TypeName).Except(types2.Select(t => t.TypeName)), StringComparer.OrdinalIgnoreCase);
                    var only2Types = new HashSet<string>(types2.Select(t => t.TypeName).Except(types1.Select(t => t.TypeName)), StringComparer.OrdinalIgnoreCase);

                    var only1 = types1.Where(plugType => only1Types.Contains(plugType.TypeName));
                    var only2 = types2.Where(plugType => only2Types.Contains(plugType.TypeName));

                    List<string> diff = new List<string>();

                    if (only1.Any())
                    {
                        diff.Add(string.Format("Plugin Types ONLY EXISTS in {0}: {1}", Connection1.Name, only1.Count()));

                        FormatTextTableHandler table = new FormatTextTableHandler();
                        table.SetHeader("Name", "IsManaged");

                        foreach (var item in only1.OrderBy(s => s.TypeName))
                        {
                            table.AddLine(item.TypeName, item.IsManaged.ToString());
                        }

                        table.GetFormatedLines(false).ForEach(item => diff.Add(tabSpacer + item));
                    }

                    if (only2.Any())
                    {
                        diff.Add(string.Format("Plugin Types ONLY EXISTS in {0}: {1}", Connection2.Name, only2.Count()));

                        FormatTextTableHandler table = new FormatTextTableHandler();
                        table.SetHeader("Name", "IsManaged");

                        foreach (var item in only2.OrderBy(s => s.TypeName))
                        {
                            table.AddLine(item.TypeName, item.IsManaged.ToString());
                        }

                        table.GetFormatedLines(false).ForEach(item => diff.Add(tabSpacer + item));
                    }

                    assemblyDifference.Add(assem1.Name, diff);

                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.PluginAssembly, assem1.Id, assem2.Id, string.Join(Environment.NewLine, diff));
                }
            }

            if (tableOnlyExistsIn1.Count > 0)
            {
                content.AppendLine().AppendLine().AppendFormat("Plugin Assemblies ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

                tableOnlyExistsIn1.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableOnlyExistsIn2.Count > 0)
            {
                content.AppendLine().AppendLine().AppendFormat("Plugin Assemblies ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

                tableOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (assemblyDifference.Count > 0)
            {
                content
                     .AppendLine()
                     .AppendLine()
                     .AppendLine()
                     .AppendLine(new string('-', 150))
                     .AppendLine()
                     .AppendLine();

                content.AppendFormat("Plugin Assemblies DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, assemblyDifference.Count);

                foreach (var item in assemblyDifference.OrderBy(s => s.Key))
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

                content.AppendFormat("Plugin Assemblies DIFFERENT Details in {0} and {1}: {2}", Connection1.Name, Connection2.Name, assemblyDifference.Count);

                foreach (var item in assemblyDifference.OrderBy(s => s.Key))
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + item.Key).TrimEnd());

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
                && assemblyDifference.Count == 0
                )
            {
                content.AppendLine("No difference in Plugin Assemblies.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Plugin Assemblies");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        public Task<string> CheckPluginTypesAsync()
        {
            return Task.Run(async () => await CheckPluginTypes());
        }

        private async Task<string> CheckPluginTypes()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingPluginTypesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetPluginType1Async();
            var task2 = _comparerSource.GetPluginType2Async();

            var list1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Types in {0}: {1}", Connection1.Name, list1.Count()));

            var list2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Types in {0}: {1}", Connection2.Name, list2.Count()));

            FormatTextTableHandler tableOnlyExistsIn1 = new FormatTextTableHandler();
            tableOnlyExistsIn1.SetHeader("Name", "IsManaged");

            FormatTextTableHandler tableOnlyExistsIn2 = new FormatTextTableHandler();
            tableOnlyExistsIn2.SetHeader("Name", "IsManaged");

            foreach (var plugType1 in list1.OrderBy(e => e.TypeName))
            {
                {
                    var plugType2 = list2.FirstOrDefault(e => e.TypeName == plugType1.TypeName);

                    if (plugType2 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn1.AddLine(plugType1.TypeName, plugType1.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.PluginType, plugType1.Id);
            }

            foreach (var plugType2 in list2.OrderBy(e => e.TypeName))
            {
                {
                    var plugType1 = list1.FirstOrDefault(e => e.TypeName == plugType2.TypeName);

                    if (plugType1 != null)
                    {
                        continue;
                    }
                }

                tableOnlyExistsIn2.AddLine(plugType2.TypeName, plugType2.IsManaged.ToString());

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.PluginAssembly, plugType2.Id);
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

                content.AppendLine().AppendLine().AppendFormat("Plugin Types ONLY EXISTS in {0}: {1}", Connection1.Name, tableOnlyExistsIn1.Count);

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

                content.AppendLine().AppendLine().AppendFormat("Plugin Types ONLY EXISTS in {0}: {1}", Connection2.Name, tableOnlyExistsIn2.Count);

                tableOnlyExistsIn2.GetFormatedLines(true).ForEach(e => content.AppendLine().Append((tabSpacer + e).TrimEnd()));
            }

            if (tableOnlyExistsIn2.Count == 0
                && tableOnlyExistsIn1.Count == 0
                )
            {
                content.AppendLine("No difference in Plugin Types.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Plugin Types");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        private class LineWithSublines
        {
            public string[] Line { get; set; }

            public List<string[]> SubLines { get; private set; }

            public LineWithSublines()
            {
                SubLines = new List<string[]>();
            }
        }

        private struct PluginTypeStep
        {
            public string PluginTypeName { get; set; }
            public string Message { get; set; }
            public string EntityName { get; set; }
            public string SecondaryName { get; set; }
            public int Stage { get; set; }
            public int Mode { get; set; }
        }

        public Task<string> CheckPluginStepsByPluginTypeNamesAsync()
        {
            return Task.Run(async () => await CheckPluginStepsByPluginTypeNames());
        }

        private async Task<string> CheckPluginStepsByPluginTypeNames()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingPluginStepsByPluginTypeNamesFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetSdkMessageProcessingStep1Async();
            var task2 = _comparerSource.GetSdkMessageProcessingStep2Async();

            var taskImages1 = _comparerSource.GetSdkMessageProcessingStepImage1Async();
            var taskImages2 = _comparerSource.GetSdkMessageProcessingStepImage2Async();

            var listSteps1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Steps in {0}: {1}", Connection1.Name, listSteps1.Count()));

            var listSteps2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Steps in {0}: {1}", Connection2.Name, listSteps2.Count()));

            var listImages1 = await taskImages1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Steps Images in {0}: {1}", Connection1.Name, listImages1.Count()));

            var listImages2 = await taskImages2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Steps Images in {0}: {1}", Connection2.Name, listImages2.Count()));

            var groups1 = listSteps1.GroupBy(e => new PluginTypeStep()
            {
                PluginTypeName = e.EventHandler?.Name ?? "Unknown",
                Message = e.SdkMessageId?.Name ?? "Unknown",
                EntityName = e.PrimaryObjectTypeCodeName,
                SecondaryName = e.SecondaryObjectTypeCodeName,
                Stage = e.Stage.Value,
                Mode = e.Mode.Value,
            });

            var groups2 = listSteps2.GroupBy(e => new PluginTypeStep()
            {
                PluginTypeName = e.EventHandler?.Name ?? "Unknown",
                Message = e.SdkMessageId?.Name ?? "Unknown",
                EntityName = e.PrimaryObjectTypeCodeName,
                SecondaryName = e.SecondaryObjectTypeCodeName,
                Stage = e.Stage.Value,
                Mode = e.Mode.Value,
            });

            List<LineWithSublines> stepsOnlyIn1 = new List<LineWithSublines>();
            List<LineWithSublines> stepsOnlyIn2 = new List<LineWithSublines>();

            Dictionary<PluginTypeStep, List<string>> stepDifference = new Dictionary<PluginTypeStep, List<string>>();

            foreach (var gr1 in groups1
                .OrderBy(s => s.Key.PluginTypeName)
                .ThenBy(s => s.Key.EntityName)
                .ThenBy(s => s.Key.SecondaryName)
                .ThenBy(s => s.Key.Message, new MessageComparer())
                .ThenBy(s => s.Key.Stage)
                .ThenBy(s => s.Key.Mode)
                )
            {
                {
                    var gr2 = groups2.FirstOrDefault(g =>
                        g.Key.Message == gr1.Key.Message
                        && g.Key.PluginTypeName == gr1.Key.PluginTypeName
                        && g.Key.EntityName == gr1.Key.EntityName
                        && g.Key.SecondaryName == gr1.Key.SecondaryName
                        && g.Key.Stage == gr1.Key.Stage
                        && g.Key.Mode == gr1.Key.Mode
                        );

                    if (gr2 != null)
                    {
                        continue;
                    }
                }

                foreach (var step in gr1)
                {
                    var item = new LineWithSublines
                    {
                        Line = new string[]
                    {
                        step.EventHandler?.Name ?? "Unknown"
                        , step.PrimaryObjectTypeCodeName
                        , step.SecondaryObjectTypeCodeName
                        , step.SdkMessageId?.Name ?? "Unknown"
                        , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                        , step.Rank.ToString()
                        , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                        , step.IsHidden.ToString()
                        , step.IsManaged.ToString()
                        , step.FilteringAttributesStringsSorted
                    }
                    };

                    var images = listImages1.Where(i => i.SdkMessageProcessingStepId.Id == step.SdkMessageProcessingStepId.Value);

                    if (images.Any())
                    {
                        foreach (var image in images
                            .OrderBy(i => i.ImageType.Value)
                            .ThenBy(i => i.Name)
                            .ThenBy(i => i.EntityAlias)
                            .ThenBy(i => i.Attributes1StringsSorted)
                            )
                        {
                            item.SubLines.Add(new string[] { image.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype], image.Name, image.EntityAlias, image.Attributes1StringsSorted });
                        }
                    }

                    stepsOnlyIn1.Add(item);
                }
            }

            foreach (var gr2 in groups2
                .OrderBy(s => s.Key.PluginTypeName)
                .ThenBy(s => s.Key.EntityName)
                .ThenBy(s => s.Key.SecondaryName)
                .ThenBy(s => s.Key.Message, new MessageComparer())
                .ThenBy(s => s.Key.Stage)
                .ThenBy(s => s.Key.Mode)
                )
            {
                {
                    var gr1 = groups1.FirstOrDefault(g =>
                        g.Key.Message == gr2.Key.Message
                        && g.Key.PluginTypeName == gr2.Key.PluginTypeName
                        && g.Key.EntityName == gr2.Key.EntityName
                        && g.Key.SecondaryName == gr2.Key.SecondaryName
                        && g.Key.Stage == gr2.Key.Stage
                        && g.Key.Mode == gr2.Key.Mode
                        );

                    if (gr1 != null)
                    {
                        continue;
                    }
                }

                foreach (var step in gr2)
                {
                    var item = new LineWithSublines
                    {
                        Line = new string[]
                    {
                        step.EventHandler?.Name ?? "Unknown"
                        , step.PrimaryObjectTypeCodeName
                        , step.SecondaryObjectTypeCodeName
                        , step.SdkMessageId?.Name ?? "Unknown"
                        , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                        , step.Rank.ToString()
                        , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                        , step.IsHidden.ToString()
                        , step.IsManaged.ToString()
                        , step.FilteringAttributesStringsSorted
                    }
                    };

                    var images = listImages2.Where(i => i.SdkMessageProcessingStepId.Id == step.SdkMessageProcessingStepId.Value);

                    if (images.Any())
                    {
                        foreach (var image in images
                            .OrderBy(i => i.ImageType.Value)
                            .ThenBy(i => i.Name)
                            .ThenBy(i => i.EntityAlias)
                            .ThenBy(i => i.Attributes1StringsSorted)
                            )
                        {
                            item.SubLines.Add(new string[] { image.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype], image.Name, image.EntityAlias, image.Attributes1StringsSorted });
                        }
                    }

                    stepsOnlyIn2.Add(item);
                }
            }

            var stepComparer = new StepComparer();

            foreach (var gr1 in groups1
                .OrderBy(s => s.Key.PluginTypeName)
                .ThenBy(s => s.Key.EntityName)
                .ThenBy(s => s.Key.Message, new MessageComparer())
                .ThenBy(s => s.Key.Stage)
                .ThenBy(s => s.Key.Mode)
                )
            {
                var gr2 = groups2.FirstOrDefault(g =>
                    g.Key.Message == gr1.Key.Message
                    && g.Key.PluginTypeName == gr1.Key.PluginTypeName
                    && g.Key.EntityName == gr1.Key.EntityName
                    && g.Key.SecondaryName == gr1.Key.SecondaryName
                    && g.Key.Stage == gr1.Key.Stage
                    && g.Key.Mode == gr1.Key.Mode
                    );

                if (gr2 == null)
                {
                    continue;
                }

                var grSteps1 = gr1.ToList();
                var grSteps2 = gr2.ToList();

                List<string> diff = new List<string>();

                var only1 = grSteps1.Except(grSteps2, stepComparer);
                var only2 = grSteps2.Except(grSteps1, stepComparer);

                var intersect = grSteps1.Intersect(grSteps2, stepComparer);

                if (only1.Any())
                {
                    foreach (var step in only1)
                    {
                        FormatTextTableHandler tableStep = new FormatTextTableHandler();
                        tableStep.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "FilteringAttributes", "Configuration");
                        tableStep.AddLine(
                            step.EventHandler?.Name ?? "Unknown"
                            , step.PrimaryObjectTypeCodeName
                            , step.SecondaryObjectTypeCodeName
                            , step.SdkMessageId?.Name ?? "Unknown"
                            , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                            , step.Rank.ToString()
                            , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                            , step.FilteringAttributesStringsSorted
                            , step.Configuration ?? string.Empty
                        );

                        diff.Add(string.Format("Steps ONLY in {0}", Connection1.Name));
                        tableStep.GetFormatedLines(true).ForEach(s => diff.Add(tabSpacer + s));

                        var images = listImages1.Where(i => i.SdkMessageProcessingStepId.Id == step.SdkMessageProcessingStepId.Value);

                        if (images.Any())
                        {
                            FormatTextTableHandler tableImage = new FormatTextTableHandler(true);
                            tableImage.SetHeader("ImageType", "Name", "EntityAlias", "Attributes");

                            foreach (var image in images
                                .OrderBy(i => i.ImageType.Value)
                                .ThenBy(i => i.Name)
                                .ThenBy(i => i.EntityAlias)
                                .ThenBy(i => i.Attributes1StringsSorted)
                                )
                            {
                                tableImage.AddLine(image.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype], image.Name, image.EntityAlias, image.Attributes1StringsSorted);
                            }

                            tableImage.GetFormatedLines(true).ForEach(l => diff.Add(tabSpacer + tabSpacer + l));
                        }
                    }
                }

                if (only2.Any())
                {
                    foreach (var step in only2)
                    {
                        FormatTextTableHandler tableStep = new FormatTextTableHandler();
                        tableStep.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "FilteringAttributes", "Configuration");
                        tableStep.AddLine(
                            step.EventHandler?.Name ?? "Unknown"
                            , step.PrimaryObjectTypeCodeName
                            , step.SecondaryObjectTypeCodeName
                            , step.SdkMessageId?.Name ?? "Unknown"
                            , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                            , step.Rank.ToString()
                            , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                            , step.FilteringAttributesStringsSorted
                            , step.Configuration ?? string.Empty
                        );

                        diff.Add(string.Format("Steps ONLY in {0}", Connection2.Name));
                        tableStep.GetFormatedLines(true).ForEach(s => diff.Add(tabSpacer + s));

                        var images = listImages2.Where(i => i.SdkMessageProcessingStepId.Id == step.SdkMessageProcessingStepId.Value);

                        if (images.Any())
                        {
                            FormatTextTableHandler tableImage = new FormatTextTableHandler(true);
                            tableImage.SetHeader("ImageType", "Name", "EntityAlias", "Attributes");

                            foreach (var image in images
                                .OrderBy(i => i.ImageType.Value)
                                .ThenBy(i => i.Name)
                                .ThenBy(i => i.EntityAlias)
                                .ThenBy(i => i.Attributes1StringsSorted)
                                )
                            {
                                tableImage.AddLine(image.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype], image.Name, image.EntityAlias, image.Attributes1StringsSorted);
                            }

                            tableImage.GetFormatedLines(true).ForEach(l => diff.Add(tabSpacer + tabSpacer + l));
                        }
                    }
                }

                if (intersect.Any())
                {
                    foreach (var step1 in intersect)
                    {
                        var step2 = grSteps2.FirstOrDefault(
                            i => i.Rank == step1.Rank
                            && i.StateCode.Value == step1.StateCode.Value
                            && i.StatusCode.Value == step1.StatusCode.Value
                            && i.FilteringAttributesStrings.SequenceEqual(step1.FilteringAttributesStrings)
                            //&& string.Equals(i.FilteringAttributes, step1.FilteringAttributes)
                            && string.Equals(i.Configuration, step1.Configuration)
                        );

                        FormatTextTableHandler tableDifference = new FormatTextTableHandler(true);
                        tableDifference.SetHeader("Property", Connection1.Name, Connection2.Name);

                        if (step1.Rank != step2.Rank)
                        {
                            tableDifference.AddLine("Rank", step1.Rank.ToString(), step2.Rank.ToString());
                        }

                        if (step1.StateCode.Value != step2.StateCode.Value)
                        {
                            tableDifference.AddLine("StateCode", step1.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statecode], step2.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statecode]);
                        }

                        if (step1.StatusCode.Value != step2.StatusCode.Value)
                        {
                            tableDifference.AddLine("StatusCode", step1.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode], step2.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]);
                        }

                        if (!step1.FilteringAttributesStrings.SequenceEqual(step2.FilteringAttributesStrings))
                        {
                            tableDifference.AddLine("FilteringAttributes"
                                , step1.FilteringAttributesStringsSorted
                                , step2.FilteringAttributesStringsSorted
                                );
                        }

                        if (step1.Configuration != step2.Configuration)
                        {
                            tableDifference.AddLine("Configuration", step1.Configuration, step2.Configuration);
                        }

                        if (tableDifference.Count > 0)
                        {
                            diff.AddRange(tableDifference.GetFormatedLines(false));
                        }

                        {
                            var compareImages = CompareImages(
                                tabSpacer
                                , listImages1.Where(i => i.SdkMessageProcessingStepId.Id == step1.SdkMessageProcessingStepId.Value)
                                , listImages2.Where(i => i.SdkMessageProcessingStepId.Id == step2.SdkMessageProcessingStepId.Value)
                                );

                            if (compareImages.Count > 0)
                            {
                                diff.AddRange(compareImages);
                            }
                        }
                    }
                }

                if (diff.Count > 0)
                {
                    stepDifference.Add(gr1.Key, diff);
                }
            }

            if (stepsOnlyIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Plugin Steps ONLY EXISTS in {0}: {1}", Connection1.Name, stepsOnlyIn1.Count);

                foreach (var step in stepsOnlyIn1)
                {
                    FormatTextTableHandler tableStep = new FormatTextTableHandler(true);
                    tableStep.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "FilteringAttributes");

                    tableStep.AddLine(step.Line);

                    tableStep.GetFormatedLines(true).ForEach(l => content.AppendLine().Append(tabSpacer + l));

                    if (step.SubLines.Count > 0)
                    {
                        FormatTextTableHandler tableImage = new FormatTextTableHandler(true);
                        tableImage.SetHeader("ImageType", "Name", "EntityAlias", "Attributes");

                        step.SubLines.ForEach(l => tableImage.AddLine(l));

                        tableImage.GetFormatedLines(true).ForEach(l => content.AppendLine().Append(tabSpacer + tabSpacer + l));
                    }

                    content
                        .AppendLine();
                }
            }

            if (stepsOnlyIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Plugin Steps ONLY EXISTS in {0}: {1}", Connection2.Name, stepsOnlyIn2.Count);

                foreach (var step in stepsOnlyIn2)
                {
                    FormatTextTableHandler tableStep = new FormatTextTableHandler(true);
                    tableStep.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "FilteringAttributes");

                    tableStep.AddLine(step.Line);

                    tableStep.GetFormatedLines(true).ForEach(l => content.AppendLine().Append(tabSpacer + l));

                    if (step.SubLines.Count > 0)
                    {
                        FormatTextTableHandler tableImage = new FormatTextTableHandler(true);
                        tableImage.SetHeader("ImageType", "Name", "EntityAlias", "Attributes");

                        step.SubLines.ForEach(l => tableImage.AddLine(l));

                        tableImage.GetFormatedLines(true).ForEach(l => content.AppendLine().Append(tabSpacer + tabSpacer + l));
                    }

                    content
                        .AppendLine();
                }
            }

            if (stepDifference.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Plugin Steps DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, stepDifference.Count);

                FormatTextTableHandler tableStep = new FormatTextTableHandler();
                tableStep.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage");

                foreach (var item in stepDifference
                    .OrderBy(s => s.Key.PluginTypeName)
                    .ThenBy(s => s.Key.EntityName)
                    .ThenBy(s => s.Key.Message)
                    .ThenBy(s => s.Key.Stage)
                    .ThenBy(s => s.Key.Mode)
                    )
                {
                    tableStep.AddLine(item.Key.PluginTypeName, item.Key.EntityName, item.Key.SecondaryName, item.Key.Message, SdkMessageProcessingStepRepository.GetStageName(item.Key.Stage, item.Key.Mode));
                }

                tableStep.GetFormatedLines(false).ForEach(s => content.AppendLine().Append(tabSpacer + s));

                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Plugin Steps DIFFERENT Details in {0} and {1}: {2}", Connection1.Name, Connection2.Name, stepDifference.Count);

                foreach (var item in stepDifference
                    .OrderBy(s => s.Key.PluginTypeName)
                    .ThenBy(s => s.Key.EntityName)
                    .ThenBy(s => s.Key.Message)
                    .ThenBy(s => s.Key.Stage)
                    .ThenBy(s => s.Key.Mode)
                    )
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + tableStep.FormatLine(item.Key.PluginTypeName, item.Key.EntityName, item.Key.SecondaryName, item.Key.Message, SdkMessageProcessingStepRepository.GetStageName(item.Key.Stage, item.Key.Mode))).TrimEnd());

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

            if (stepsOnlyIn1.Count == 0
                && stepsOnlyIn2.Count == 0
                && stepDifference.Count == 0
                )
            {
                content.AppendLine("No difference in Plugin Steps by Plugin Type Names.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Plugin Steps by Plugin Type Names");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        public class StepComparer : IEqualityComparer<SdkMessageProcessingStep>
        {
            public bool Equals(SdkMessageProcessingStep x, SdkMessageProcessingStep y)
            {
                if (x == null && y == null)
                {
                    return true;
                }
                else if (x == null || y == null)
                {
                    return false;
                }
                else if (
                    x.Rank == y.Rank
                    && x.StateCode.Value == y.StateCode.Value
                    && x.StatusCode.Value == y.StatusCode.Value
                    && x.FilteringAttributesStrings.SequenceEqual(y.FilteringAttributesStrings)
                    && string.Equals(x.Configuration ?? string.Empty, y.Configuration ?? string.Empty)
                    )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(SdkMessageProcessingStep obj)
            {
                return 0;
            }
        }

        public class StepImageComparer : IEqualityComparer<SdkMessageProcessingStepImage>
        {
            public bool Equals(SdkMessageProcessingStepImage x, SdkMessageProcessingStepImage y)
            {
                if (x == null && y == null)
                {
                    return true;
                }
                else if (x == null || y == null)
                {
                    return false;
                }
                else if (x.ImageType.Value == y.ImageType.Value
                    && string.Equals(x.Name ?? string.Empty, y.Name ?? string.Empty)
                    && string.Equals(x.EntityAlias ?? string.Empty, y.EntityAlias ?? string.Empty)
                    //&& string.Equals(x.Attributes1 ?? string.Empty, y.Attributes1 ?? string.Empty)
                    && x.Attributes1Strings.SequenceEqual(y.Attributes1Strings)
                    )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(SdkMessageProcessingStepImage obj)
            {
                return 0;
            }
        }

        private List<string> CompareImages(string tabSpacer, IEnumerable<SdkMessageProcessingStepImage> enumerable1, IEnumerable<SdkMessageProcessingStepImage> enumerable2)
        {
            List<string> result = new List<string>();

            var comparer = new StepImageComparer();

            var list1 = enumerable1.Except(enumerable2, comparer);
            var list2 = enumerable2.Except(enumerable1, comparer);

            if (list1.Any())
            {
                FormatTextTableHandler tableImage = new FormatTextTableHandler(true);
                tableImage.SetHeader("ImageType", "Name", "EntityAlias", "Attributes");

                foreach (var item in list1
                    .OrderBy(i => i.ImageType.Value)
                    .ThenBy(i => i.Name)
                    .ThenBy(i => i.EntityAlias)
                    .ThenBy(i => i.Attributes1StringsSorted)
                    )
                {
                    tableImage.AddLine(item.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype], item.Name, item.EntityAlias, item.Attributes1StringsSorted);
                }

                result.Add(string.Format("Images ONLY EXISTS in {0}: {1}", Connection1.Name, list1.Count()));
                tableImage.GetFormatedLines(true).ForEach(l => result.Add(tabSpacer + l));
            }

            if (list2.Any())
            {
                FormatTextTableHandler tableImage = new FormatTextTableHandler(true);
                tableImage.SetHeader("ImageType", "Name", "EntityAlias", "Attributes");

                foreach (var item in list2
                    .OrderBy(i => i.ImageType.Value)
                    .ThenBy(i => i.Name)
                    .ThenBy(i => i.EntityAlias)
                    .ThenBy(i => i.Attributes1StringsSorted)
                    )
                {
                    tableImage.AddLine(item.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype], item.Name, item.EntityAlias, item.Attributes1StringsSorted);
                }

                result.Add(string.Format("Images ONLY EXISTS in {0}: {1}", Connection2.Name, list2.Count()));
                tableImage.GetFormatedLines(true).ForEach(l => result.Add(tabSpacer + l));
            }

            return result;
        }

        public Task<string> CheckPluginStepsByIdsAsync()
        {
            return Task.Run(async () => await CheckPluginStepsByIds());
        }

        private async Task<string> CheckPluginStepsByIds()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content);

            string operation = string.Format(Properties.OperationNames.CheckingPluginStepsByIdsFormat2, Connection1.Name, Connection2.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(operation));

            var task1 = _comparerSource.GetSdkMessageProcessingStep1Async();
            var task2 = _comparerSource.GetSdkMessageProcessingStep2Async();

            var taskImages1 = _comparerSource.GetSdkMessageProcessingStepImage1Async();
            var taskImages2 = _comparerSource.GetSdkMessageProcessingStepImage2Async();

            var listSteps1 = await task1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Steps in {0}: {1}", Connection1.Name, listSteps1.Count()));

            var listSteps2 = await task2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Steps in {0}: {1}", Connection2.Name, listSteps2.Count()));

            var listImages1 = await taskImages1;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Steps Images in {0}: {1}", Connection1.Name, listImages1.Count()));

            var listImages2 = await taskImages2;

            content.AppendLine(_iWriteToOutput.WriteToOutput("Plugin Steps Images in {0}: {1}", Connection2.Name, listImages2.Count()));

            var stepsOnlyIn1 = new List<LineWithSublines>();
            var stepsOnlyIn2 = new List<LineWithSublines>();

            var commonStepsList = new List<LinkedEntities<SdkMessageProcessingStep>>();

            var stepDifference = new Dictionary<LinkedEntities<SdkMessageProcessingStep>, List<string>>();

            foreach (var step1 in listSteps1
                .OrderBy(s => s.EventHandler?.Name ?? "Unknown")
                .ThenBy(s => s.PrimaryObjectTypeCodeName)
                .ThenBy(s => s.SecondaryObjectTypeCodeName)
                .ThenBy(s => s.SdkMessageId?.Name ?? "Unknown", new MessageComparer())
                .ThenBy(s => s.Stage.Value)
                .ThenBy(s => s.Mode.Value)
                )
            {
                {
                    var step2 = listSteps2.FirstOrDefault(g => g.Id == step1.Id);

                    if (step2 != null)
                    {
                        commonStepsList.Add(new LinkedEntities<SdkMessageProcessingStep>(step1, step2));

                        continue;
                    }
                }

                var item = new LineWithSublines
                {
                    Line = new string[]
                {
                     step1.EventHandler?.Name ?? "Unknown"
                    , step1.PrimaryObjectTypeCodeName
                    , step1.SecondaryObjectTypeCodeName
                    , step1.SdkMessageId?.Name ?? "Unknown"
                    , SdkMessageProcessingStepRepository.GetStageName(step1.Stage.Value, step1.Mode.Value)
                    , step1.Rank.ToString()
                    , step1.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                    , step1.IsHidden?.Value.ToString()
                    , step1.IsManaged.ToString()
                    , step1.FilteringAttributesStringsSorted
                }
                };

                var images = listImages1.Where(i => i.SdkMessageProcessingStepId.Id == step1.SdkMessageProcessingStepId.Value);

                if (images.Any())
                {
                    foreach (var image in images
                        .OrderBy(i => i.ImageType.Value)
                        .ThenBy(i => i.Name)
                        .ThenBy(i => i.EntityAlias)
                        .ThenBy(i => i.Attributes1StringsSorted)
                        )
                    {
                        item.SubLines.Add(new string[] { image.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype], image.Name, image.EntityAlias, image.Attributes1StringsSorted });
                    }
                }

                stepsOnlyIn1.Add(item);

                this.ImageBuilder.AddComponentSolution1((int)ComponentType.SdkMessageProcessingStep, step1.Id);
            }

            foreach (var step2 in listSteps2
                .OrderBy(s => s.EventHandler?.Name ?? "Unknown")
                .ThenBy(s => s.PrimaryObjectTypeCodeName)
                .ThenBy(s => s.SecondaryObjectTypeCodeName)
                .ThenBy(s => s.SdkMessageId?.Name ?? "Unknown", new MessageComparer())
                .ThenBy(s => s.Stage.Value)
                .ThenBy(s => s.Mode.Value)
                )
            {
                {
                    var step1 = listSteps1.FirstOrDefault(g => g.Id == step2.Id);

                    if (step1 != null)
                    {
                        continue;
                    }
                }

                var item = new LineWithSublines
                {
                    Line = new string[]
                {
                    step2.EventHandler?.Name ?? "Unknown"
                    , step2.PrimaryObjectTypeCodeName
                    , step2.SecondaryObjectTypeCodeName
                    , step2.SdkMessageId?.Name ?? "Unknown"
                    , SdkMessageProcessingStepRepository.GetStageName(step2.Stage.Value, step2.Mode.Value)
                    , step2.Rank.ToString()
                    , step2.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                    , step2.IsHidden?.Value.ToString()
                    , step2.IsManaged.ToString()
                    , step2.FilteringAttributesStringsSorted
                }
                };

                var images = listImages2.Where(i => i.SdkMessageProcessingStepId.Id == step2.SdkMessageProcessingStepId.Value);

                if (images.Any())
                {
                    foreach (var image in images
                        .OrderBy(i => i.ImageType.Value)
                        .ThenBy(i => i.Name)
                        .ThenBy(i => i.EntityAlias)
                        .ThenBy(i => i.Attributes1StringsSorted)
                        )
                    {
                        item.SubLines.Add(new string[] { image.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype], image.Name, image.EntityAlias, image.Attributes1StringsSorted });
                    }
                }

                stepsOnlyIn2.Add(item);

                this.ImageBuilder.AddComponentSolution2((int)ComponentType.SdkMessageProcessingStep, step2.Id);
            }

            content.AppendLine(_iWriteToOutput.WriteToOutput("Common Plugin Steps by Ids in {0} and {1}: {2}", Connection1.Name, Connection2.Name, commonStepsList.Count()));

            foreach (var commonStep in commonStepsList)
            {
                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                {
                    List<string> fieldsToCompare = new List<string>()
                    {
                        SdkMessageProcessingStep.Schema.Attributes.asyncautodelete
                        , SdkMessageProcessingStep.Schema.Attributes.canusereadonlyconnection
                        , SdkMessageProcessingStep.Schema.Attributes.componentstate
                        , SdkMessageProcessingStep.Schema.Attributes.configuration
                        //, SdkMessageProcessingStep.Schema.Attributes.createdby
                        //, SdkMessageProcessingStep.Schema.Attributes.createdon
                        //, SdkMessageProcessingStep.Schema.Attributes.createdonbehalfby
                        , SdkMessageProcessingStep.Schema.Attributes.customizationlevel
                        , SdkMessageProcessingStep.Schema.Attributes.description
                        //, SdkMessageProcessingStep.Schema.Attributes.eventhandler
                        , SdkMessageProcessingStep.Schema.Attributes.filteringattributes
                        //, SdkMessageProcessingStep.Schema.Attributes.impersonatinguserid
                        //, SdkMessageProcessingStep.Schema.Attributes.introducedversion
                        , SdkMessageProcessingStep.Schema.Attributes.invocationsource
                        , SdkMessageProcessingStep.Schema.Attributes.iscustomizable
                        , SdkMessageProcessingStep.Schema.Attributes.ishidden
                        //, SdkMessageProcessingStep.Schema.Attributes.ismanaged
                        , SdkMessageProcessingStep.Schema.Attributes.mode
                        //, SdkMessageProcessingStep.Schema.Attributes.modifiedby
                        //, SdkMessageProcessingStep.Schema.Attributes.modifiedon
                        //, SdkMessageProcessingStep.Schema.Attributes.modifiedonbehalfby
                        , SdkMessageProcessingStep.Schema.Attributes.name
                        //, SdkMessageProcessingStep.Schema.Attributes.organizationid
                        //, SdkMessageProcessingStep.Schema.Attributes.overwritetime
                        //, SdkMessageProcessingStep.Schema.Attributes.plugintypeid
                        , SdkMessageProcessingStep.Schema.Attributes.rank
                        //, SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid
                        //, SdkMessageProcessingStep.Schema.Attributes.sdkmessageid
                        //, SdkMessageProcessingStep.Schema.Attributes.sdkmessageprocessingstepid
                        //, SdkMessageProcessingStep.Schema.Attributes.sdkmessageprocessingstepidunique
                        //, SdkMessageProcessingStep.Schema.Attributes.sdkmessageprocessingstepsecureconfigid
                        //, SdkMessageProcessingStep.Schema.Attributes.solutionid
                        , SdkMessageProcessingStep.Schema.Attributes.stage
                        , SdkMessageProcessingStep.Schema.Attributes.statecode
                        , SdkMessageProcessingStep.Schema.Attributes.statuscode
                        , SdkMessageProcessingStep.Schema.Attributes.supporteddeployment
                        //, SdkMessageProcessingStep.Schema.Attributes.supportingsolutionid
                        //, SdkMessageProcessingStep.Schema.Attributes.versionnumber
                    };

                    foreach (var fieldName in fieldsToCompare)
                    {
                        if (ContentCoparerHelper.IsEntityDifferentInField(commonStep.Entity1, commonStep.Entity2, fieldName))
                        {
                            var str1 = EntityDescriptionHandler.GetAttributeString(commonStep.Entity1, fieldName, Connection1);
                            var str2 = EntityDescriptionHandler.GetAttributeString(commonStep.Entity2, fieldName, Connection2);

                            tabDiff.AddLine(fieldName, Connection1.Name, str1);
                            tabDiff.AddLine(fieldName, Connection2.Name, str2);
                        }
                    }
                }

                if (!commonStep.Entity1.FilteringAttributesStrings.SequenceEqual(commonStep.Entity2.FilteringAttributesStrings))
                {
                    tabDiff.AddLine(SdkMessageProcessingStep.Schema.Attributes.filteringattributes, Connection1.Name, commonStep.Entity1.FilteringAttributesStringsSorted);
                    tabDiff.AddLine(SdkMessageProcessingStep.Schema.Attributes.filteringattributes, Connection2.Name, commonStep.Entity2.FilteringAttributesStringsSorted);
                }

                List<string> diff = new List<string>();

                if (tabDiff.Count > 0)
                {
                    diff.AddRange(tabDiff.GetFormatedLines(false));
                }

                var compareImages = CompareImagesByIds(
                    tabSpacer
                    , listImages1.Where(i => i.SdkMessageProcessingStepId.Id == commonStep.Entity1.SdkMessageProcessingStepId.Value)
                    , listImages2.Where(i => i.SdkMessageProcessingStepId.Id == commonStep.Entity2.SdkMessageProcessingStepId.Value)
                    );

                if (compareImages.Count > 0)
                {
                    diff.AddRange(compareImages);
                }

                if (diff.Count > 0)
                {
                    stepDifference.Add(commonStep, diff);

                    this.ImageBuilder.AddComponentDifferent((int)ComponentType.SdkMessageProcessingStep, commonStep.Entity1.Id, commonStep.Entity2.Id, string.Join(Environment.NewLine, diff));
                }
            }

            if (stepsOnlyIn1.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Plugin Steps ONLY EXISTS in {0}: {1}", Connection1.Name, stepsOnlyIn1.Count);

                foreach (var step in stepsOnlyIn1)
                {
                    FormatTextTableHandler tableStep = new FormatTextTableHandler(true);
                    tableStep.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "IsHidden", "IsManaged", "FilteringAttributes");

                    tableStep.AddLine(step.Line);

                    tableStep.GetFormatedLines(true).ForEach(l => content.AppendLine().Append(tabSpacer + l));

                    if (step.SubLines.Count > 0)
                    {
                        FormatTextTableHandler tableImage = new FormatTextTableHandler(true);
                        tableImage.SetHeader("ImageType", "Name", "EntityAlias", "Attributes");

                        step.SubLines.ForEach(l => tableImage.AddLine(l));

                        tableImage.GetFormatedLines(true).ForEach(l => content.AppendLine().Append(tabSpacer + tabSpacer + l));
                    }

                    content
                        .AppendLine();
                }
            }

            if (stepsOnlyIn2.Count > 0)
            {
                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Plugin Steps ONLY EXISTS in {0}: {1}", Connection2.Name, stepsOnlyIn2.Count);

                foreach (var step in stepsOnlyIn2)
                {
                    FormatTextTableHandler tableStep = new FormatTextTableHandler(true);
                    tableStep.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "IsHidden", "IsManaged", "FilteringAttributes");

                    tableStep.AddLine(step.Line);

                    tableStep.GetFormatedLines(true).ForEach(l => content.AppendLine().Append(tabSpacer + l));

                    if (step.SubLines.Count > 0)
                    {
                        FormatTextTableHandler tableImage = new FormatTextTableHandler(true);
                        tableImage.SetHeader("ImageType", "Name", "EntityAlias", "Attributes");

                        step.SubLines.ForEach(l => tableImage.AddLine(l));

                        tableImage.GetFormatedLines(true).ForEach(l => content.AppendLine().Append(tabSpacer + tabSpacer + l));
                    }

                    content.AppendLine();
                }
            }

            if (stepDifference.Count > 0)
            {
                var order = stepDifference
                    .OrderBy(s => s.Key.Entity1.EventHandler?.Name ?? "Unknown")
                    .ThenBy(s => s.Key.Entity1.PrimaryObjectTypeCodeName)
                    .ThenBy(s => s.Key.Entity1.SecondaryObjectTypeCodeName)
                    .ThenBy(s => s.Key.Entity1.SdkMessageId?.Name)
                    .ThenBy(s => s.Key.Entity1.Stage.Value)
                    .ThenBy(s => s.Key.Entity1.Mode.Value);

                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Plugin Steps DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, stepDifference.Count);

                FormatTextTableHandler tableStep = new FormatTextTableHandler();
                tableStep.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage");

                foreach (var item in order)
                {
                    tableStep.AddLine(
                        item.Key.Entity1.EventHandler?.Name ?? "Unknown"
                        , item.Key.Entity1.PrimaryObjectTypeCodeName
                        , item.Key.Entity1.SecondaryObjectTypeCodeName
                        , item.Key.Entity1.SdkMessageId?.Name ?? "Unknown"
                        , SdkMessageProcessingStepRepository.GetStageName(item.Key.Entity1.Stage.Value, item.Key.Entity1.Mode.Value)
                        );
                }

                tableStep.GetFormatedLines(false).ForEach(s => content.AppendLine().Append(tabSpacer + s));

                content
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine();

                content.AppendFormat("Plugin Steps DIFFERENT Details in {0} and {1}: {2}", Connection1.Name, Connection2.Name, stepDifference.Count);

                foreach (var item in order)
                {
                    content
                        .AppendLine()
                        .AppendLine()
                        .Append((tabSpacer + tableStep.FormatLine(
                            item.Key.Entity1.EventHandler?.Name ?? "Unknown"
                            , item.Key.Entity1.PrimaryObjectTypeCodeName
                            , item.Key.Entity1.SecondaryObjectTypeCodeName
                            , item.Key.Entity1.SdkMessageId?.Name ?? "Unknown"
                            , SdkMessageProcessingStepRepository.GetStageName(item.Key.Entity1.Stage.Value, item.Key.Entity1.Mode.Value)
                            )).TrimEnd());

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

            if (stepsOnlyIn1.Count == 0
                && stepsOnlyIn2.Count == 0
                && stepDifference.Count == 0
                )
            {
                content.AppendLine("No difference in Plugin Steps by Ids.");
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(operation));

            string fileName = EntityFileNameFormatter.GetDifferenceConnectionsForFieldFileName(_OrgOrgName, "Plugin Steps by Ids");

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            await SaveOrganizationDifferenceImage();

            return filePath;
        }

        private List<string> CompareImagesByIds(string tabSpacer, IEnumerable<SdkMessageProcessingStepImage> enumerable1, IEnumerable<SdkMessageProcessingStepImage> enumerable2)
        {
            List<string> result = new List<string>();

            var imagesOnlyIn1 = new List<SdkMessageProcessingStepImage>();
            var imagesOnlyIn2 = new List<SdkMessageProcessingStepImage>();

            var commonImagesList = new List<LinkedEntities<SdkMessageProcessingStepImage>>();

            var imageDifference = new Dictionary<LinkedEntities<SdkMessageProcessingStepImage>, List<string>>();

            foreach (var image1 in enumerable1.OrderBy(i => i.Name))
            {
                {
                    var image2 = enumerable2.FirstOrDefault(g => g.Id == image1.Id);

                    if (image2 != null)
                    {
                        commonImagesList.Add(new LinkedEntities<SdkMessageProcessingStepImage>(image1, image2));

                        continue;
                    }
                }

                imagesOnlyIn1.Add(image1);
            }

            foreach (var image2 in enumerable1.OrderBy(i => i.Name))
            {
                {
                    var image1 = enumerable2.FirstOrDefault(g => g.Id == image2.Id);

                    if (image1 != null)
                    {
                        continue;
                    }
                }

                imagesOnlyIn2.Add(image2);
            }

            foreach (var commonImage in commonImagesList)
            {
                FormatTextTableHandler tabDiff = new FormatTextTableHandler();
                tabDiff.SetHeader("Attribute", "Organization", "Value");

                {
                    if (!commonImage.Entity1.Attributes1Strings.SequenceEqual(commonImage.Entity2.Attributes1Strings))
                    {
                        tabDiff.AddLine(SdkMessageProcessingStepImage.Schema.Attributes.attributes, Connection1.Name, commonImage.Entity1.Attributes1StringsSorted);
                        tabDiff.AddLine(SdkMessageProcessingStepImage.Schema.Attributes.attributes, Connection2.Name, commonImage.Entity2.Attributes1StringsSorted);
                    }

                    List<string> fieldsToCompare = new List<string>()
                    {
                        //SdkMessageProcessingStepImage.Schema.Attributes.attributes
                        SdkMessageProcessingStepImage.Schema.Attributes.componentstate
                        //, SdkMessageProcessingStepImage.Schema.Attributes.createdby
                        //, SdkMessageProcessingStepImage.Schema.Attributes.createdon
                        //, SdkMessageProcessingStepImage.Schema.Attributes.createdonbehalfby
                        , SdkMessageProcessingStepImage.Schema.Attributes.customizationlevel
                        , SdkMessageProcessingStepImage.Schema.Attributes.description
                        , SdkMessageProcessingStepImage.Schema.Attributes.entityalias
                        , SdkMessageProcessingStepImage.Schema.Attributes.imagetype
                        //, SdkMessageProcessingStepImage.Schema.Attributes.introducedversion
                        , SdkMessageProcessingStepImage.Schema.Attributes.iscustomizable
                        //, ismanaged
                        , SdkMessageProcessingStepImage.Schema.Attributes.messagepropertyname
                        //, modifiedby
                        //, modifiedon
                        //, modifiedonbehalfby
                        , SdkMessageProcessingStepImage.Schema.Attributes.name
                        //, organizationid
                        //, overwritetime
                        , SdkMessageProcessingStepImage.Schema.Attributes.relatedattributename
                        //, SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                        //, SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepimageid
                        //, SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepimageidunique
                        //, SdkMessageProcessingStepImage.Schema.Attributes.solutionid
                        //, SdkMessageProcessingStepImage.Schema.Attributes.supportingsolutionid
                        , SdkMessageProcessingStepImage.Schema.Attributes.versionnumber
                    };

                    foreach (var fieldName in fieldsToCompare)
                    {
                        if (ContentCoparerHelper.IsEntityDifferentInField(commonImage.Entity1, commonImage.Entity2, fieldName))
                        {
                            var str1 = EntityDescriptionHandler.GetAttributeString(commonImage.Entity1, fieldName, Connection1);
                            var str2 = EntityDescriptionHandler.GetAttributeString(commonImage.Entity2, fieldName, Connection2);

                            tabDiff.AddLine(fieldName, Connection1.Name, str1);
                            tabDiff.AddLine(fieldName, Connection2.Name, str2);
                        }
                    }
                }

                if (tabDiff.Count > 0)
                {
                    imageDifference.Add(commonImage, tabDiff.GetFormatedLines(false));
                }
            }

            if (imagesOnlyIn1.Any())
            {
                FormatTextTableHandler tableImage = new FormatTextTableHandler(true);
                tableImage.SetHeader("ImageType", "Name", "EntityAlias", "Attributes");

                foreach (var item in imagesOnlyIn1
                    .OrderBy(i => i.ImageType.Value)
                    .ThenBy(i => i.Name)
                    .ThenBy(i => i.EntityAlias)
                    .ThenBy(i => i.Attributes1StringsSorted)
                    )
                {
                    tableImage.AddLine(item.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype], item.Name, item.EntityAlias, item.Attributes1StringsSorted);
                }

                result.Add(string.Format("Images ONLY EXISTS in {0}: {1}", Connection1.Name, imagesOnlyIn1.Count()));
                tableImage.GetFormatedLines(true).ForEach(l => result.Add(tabSpacer + l));
            }

            if (imagesOnlyIn2.Any())
            {
                FormatTextTableHandler tableImage = new FormatTextTableHandler(true);
                tableImage.SetHeader("ImageType", "Name", "EntityAlias", "Attributes");

                foreach (var item in imagesOnlyIn2
                    .OrderBy(i => i.ImageType.Value)
                    .ThenBy(i => i.Name)
                    .ThenBy(i => i.EntityAlias)
                    .ThenBy(i => i.Attributes1StringsSorted)
                    )
                {
                    tableImage.AddLine(item.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype], item.Name, item.EntityAlias, item.Attributes1StringsSorted);
                }

                result.Add(string.Format("Images ONLY EXISTS in {0}: {1}", Connection2.Name, imagesOnlyIn2.Count()));
                tableImage.GetFormatedLines(true).ForEach(l => result.Add(tabSpacer + l));
            }

            if (imageDifference.Count > 0)
            {
                var order = imageDifference
                    .OrderBy(s => s.Key.Entity1.ImageType.Value)
                    .ThenBy(s => s.Key.Entity1.Name)
                    .ThenBy(s => s.Key.Entity1.EntityAlias)
                    .ThenBy(s => s.Key.Entity1.Id)
                    ;

                result.Add(string.Format("Images DIFFERENT in {0} and {1}: {2}", Connection1.Name, Connection2.Name, imageDifference.Count));

                FormatTextTableHandler tableImage = new FormatTextTableHandler();
                tableImage.CalculateLineLengths("ImageType1", "ImageType2", "Name1", "Name2", "EntityAlias1", "EntityAlias2");

                foreach (var item in order)
                {
                    tableImage.CalculateLineLengths(
                        item.Key.Entity1.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype]
                        , item.Key.Entity2.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype]
                        , item.Key.Entity1.Name
                        , item.Key.Entity2.Name
                        , item.Key.Entity1.EntityAlias
                        , item.Key.Entity2.EntityAlias
                        );
                }

                result.Add((tabSpacer + tableImage.FormatLine("ImageType1", "ImageType2", "Name1", "Name2", "EntityAlias1", "EntityAlias2")).TrimEnd());

                foreach (var item in order)
                {
                    result.Add((tabSpacer + tableImage.FormatLine(
                            item.Key.Entity1.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype]
                            , item.Key.Entity2.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype]
                            , item.Key.Entity1.Name
                            , item.Key.Entity2.Name
                            , item.Key.Entity1.EntityAlias
                            , item.Key.Entity2.EntityAlias
                            )).TrimEnd());

                    foreach (var str in item.Value)
                    {
                        result.Add((tabSpacer + tabSpacer + str).TrimEnd());
                    }
                }
            }

            return result;
        }
    }
}
