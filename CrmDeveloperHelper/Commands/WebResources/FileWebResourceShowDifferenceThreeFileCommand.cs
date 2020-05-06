using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceShowDifferenceThreeFileCommand : AbstractDynamicCommandConnectionPair
    {
        private readonly ShowDifferenceThreeFileType _differenceType;

        private FileWebResourceShowDifferenceThreeFileCommand(OleMenuCommandService commandService, int baseIdStart, ShowDifferenceThreeFileType differenceType, string formatButtonName)
            : base(commandService, baseIdStart, formatButtonName)
        {
            this._differenceType = differenceType;
        }

        public static FileWebResourceShowDifferenceThreeFileCommand InstanceOneByOne { get; private set; }

        public static FileWebResourceShowDifferenceThreeFileCommand InstanceTwoConnections { get; private set; }

        public static FileWebResourceShowDifferenceThreeFileCommand InstanceThreeWay { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOneByOne = new FileWebResourceShowDifferenceThreeFileCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceShowDifferenceOneByOneCommandId, ShowDifferenceThreeFileType.OneByOne, Properties.CommandNames.ShowDifferenceOneByOneCommandFormat2);

            InstanceTwoConnections = new FileWebResourceShowDifferenceThreeFileCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceShowDifferenceTwoConnectionsCommandId, ShowDifferenceThreeFileType.TwoConnections, Properties.CommandNames.ShowDifferenceTwoConnectionsCommandFormat2);

            InstanceThreeWay = new FileWebResourceShowDifferenceThreeFileCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceShowDifferenceThreeWayCommandId, ShowDifferenceThreeFileType.ThreeWay, Properties.CommandNames.ShowDifferenceThreeWayCommandFormat2);
        }

        protected override void CommandAction(DTEHelper helper, Tuple<ConnectionData, ConnectionData> connectionDataPair)
        {
            if (this._differenceType == ShowDifferenceThreeFileType.ThreeWay)
            {
                var commonConfig = Model.CommonConfiguration.Get();

                if (!commonConfig.DifferenceThreeWayAvaliable())
                {
                    return;
                }
            }

            helper.HandleWebResourceThreeFileDifferenceCommand(connectionDataPair.Item1, connectionDataPair.Item2, _differenceType);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, Tuple<ConnectionData, ConnectionData> connectionDataPair, OleMenuCommand menuCommand)
        {
            if (this._differenceType == ShowDifferenceThreeFileType.ThreeWay)
            {
                var commonConfig = Model.CommonConfiguration.Get();

                if (!commonConfig.DifferenceThreeWayAvaliable())
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    return;
                }
            }

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingle(applicationObject, menuCommand);
        }
    }
}