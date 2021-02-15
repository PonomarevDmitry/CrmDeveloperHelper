using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceShowDifferenceThreeFileCommand : AbstractDynamicCommandConnectionPair
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly ShowDifferenceThreeFileType _differenceType;

        private WebResourceShowDifferenceThreeFileCommand(OleMenuCommandService commandService, int baseIdStart, string formatButtonName, ISourceSelectedFiles sourceSelectedFiles, ShowDifferenceThreeFileType differenceType)
            : base(commandService, baseIdStart, formatButtonName)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._differenceType = differenceType;
        }

        public static WebResourceShowDifferenceThreeFileCommand InstanceCodeOneByOne { get; private set; }

        public static WebResourceShowDifferenceThreeFileCommand InstanceCodeTwoConnections { get; private set; }

        public static WebResourceShowDifferenceThreeFileCommand InstanceCodeThreeWay { get; private set; }

        public static WebResourceShowDifferenceThreeFileCommand InstanceFileOneByOne { get; private set; }

        public static WebResourceShowDifferenceThreeFileCommand InstanceFileTwoConnections { get; private set; }

        public static WebResourceShowDifferenceThreeFileCommand InstanceFileThreeWay { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCodeOneByOne = new WebResourceShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.guidDynamicConnectionPairCommandSet.CodeWebResourceShowDifferenceOneByOneCommandId
                , Properties.CommandNames.ShowDifferenceOneByOneCommandFormat2
                , sourceCode
                , ShowDifferenceThreeFileType.OneByOne
            );

            InstanceCodeTwoConnections = new WebResourceShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.guidDynamicConnectionPairCommandSet.CodeWebResourceShowDifferenceTwoConnectionsCommandId
                , Properties.CommandNames.ShowDifferenceTwoConnectionsCommandFormat2
                , sourceCode
                , ShowDifferenceThreeFileType.TwoConnections
            );

            InstanceCodeThreeWay = new WebResourceShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.guidDynamicConnectionPairCommandSet.CodeWebResourceShowDifferenceThreeWayCommandId
                , Properties.CommandNames.ShowDifferenceThreeWayCommandFormat2
                , sourceCode
                , ShowDifferenceThreeFileType.ThreeWay
            );

            InstanceFileOneByOne = new WebResourceShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.guidDynamicConnectionPairCommandSet.FileWebResourceShowDifferenceOneByOneCommandId
                , Properties.CommandNames.ShowDifferenceOneByOneCommandFormat2
                , sourceFile
                , ShowDifferenceThreeFileType.OneByOne
            );

            InstanceFileTwoConnections = new WebResourceShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.guidDynamicConnectionPairCommandSet.FileWebResourceShowDifferenceTwoConnectionsCommandId
                , Properties.CommandNames.ShowDifferenceTwoConnectionsCommandFormat2
                , sourceFile
                , ShowDifferenceThreeFileType.TwoConnections
            );

            InstanceFileThreeWay = new WebResourceShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.guidDynamicConnectionPairCommandSet.FileWebResourceShowDifferenceThreeWayCommandId
                , Properties.CommandNames.ShowDifferenceThreeWayCommandFormat2
                , sourceFile
                , ShowDifferenceThreeFileType.ThreeWay
            );
        }

        protected override void CommandAction(DTEHelper helper, Tuple<ConnectionData, ConnectionData> connectionDataPair)
        {
            if (this._differenceType == ShowDifferenceThreeFileType.ThreeWay)
            {
                var commonConfig = CommonConfiguration.Get();

                if (!commonConfig.DifferenceThreeWayAvaliable())
                {
                    return;
                }
            }

            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceText).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleWebResourceThreeFileDifferenceCommand(connectionDataPair.Item1, connectionDataPair.Item2, selectedFiles.FirstOrDefault(), _differenceType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, Tuple<ConnectionData, ConnectionData> connectionDataPair, OleMenuCommand menuCommand)
        {
            if (this._differenceType == ShowDifferenceThreeFileType.ThreeWay)
            {
                var commonConfig = CommonConfiguration.Get();

                if (!commonConfig.DifferenceThreeWayAvaliable())
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    return;
                }
            }

            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceText);
        }
    }
}
