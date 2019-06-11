using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CodeReportShowDifferenceThreeFileCommand : AbstractDynamicCommandConnectionPair
    {
        private readonly string _fieldName;
        private readonly string _fieldTitle;
        private readonly ShowDifferenceThreeFileType _differenceType;

        private CodeReportShowDifferenceThreeFileCommand(
            OleMenuCommandService commandService
            , int baseIdStart
            , string fieldName
            , string fieldTitle
            , ShowDifferenceThreeFileType differenceType
            , string formatButtonName
        ) : base(
                commandService
                , baseIdStart
                , formatButtonName
            )
        {
            this._differenceType = differenceType;
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        public static CodeReportShowDifferenceThreeFileCommand InstanceOneByOneOriginalBodyText { get; private set; }

        public static CodeReportShowDifferenceThreeFileCommand InstanceTwoConnectionsOriginalBodyText { get; private set; }

        public static CodeReportShowDifferenceThreeFileCommand InstanceThreeWayOriginalBodyText { get; private set; }

        public static CodeReportShowDifferenceThreeFileCommand InstanceOneByOneBodyText { get; private set; }

        public static CodeReportShowDifferenceThreeFileCommand InstanceTwoConnectionsBodyText { get; private set; }

        public static CodeReportShowDifferenceThreeFileCommand InstanceThreeWayBodyText { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOneByOneOriginalBodyText = new CodeReportShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.CodeReportShowDifferenceOriginalBodyTextOneByOneCommandId
                , Report.Schema.Attributes.originalbodytext
                , Report.Schema.Headers.originalbodytext
                , ShowDifferenceThreeFileType.OneByOne
                , Properties.CommandNames.ShowDifferenceOneByOneCommandFormat2
            );

            InstanceTwoConnectionsOriginalBodyText = new CodeReportShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.CodeReportShowDifferenceOriginalBodyTextTwoConnectionsCommandId
                , Report.Schema.Attributes.originalbodytext
                , Report.Schema.Headers.originalbodytext
                , ShowDifferenceThreeFileType.TwoConnections
                , Properties.CommandNames.ShowDifferenceTwoConnectionsCommandFormat2
            );

            InstanceThreeWayOriginalBodyText = new CodeReportShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.CodeReportShowDifferenceOriginalBodyTextThreeWayCommandId
                , Report.Schema.Attributes.originalbodytext
                , Report.Schema.Headers.originalbodytext
                , ShowDifferenceThreeFileType.ThreeWay
                , Properties.CommandNames.ShowDifferenceThreeWayCommandFormat2
            );

            InstanceOneByOneBodyText = new CodeReportShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.CodeReportShowDifferenceBodyTextOneByOneCommandId
                , Report.Schema.Attributes.bodytext
                , Report.Schema.Headers.bodytext
                , ShowDifferenceThreeFileType.OneByOne
                , Properties.CommandNames.ShowDifferenceOneByOneCommandFormat2
            );

            InstanceTwoConnectionsBodyText = new CodeReportShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.CodeReportShowDifferenceBodyTextTwoConnectionsCommandId
                , Report.Schema.Attributes.bodytext
                , Report.Schema.Headers.bodytext
                , ShowDifferenceThreeFileType.TwoConnections
                , Properties.CommandNames.ShowDifferenceTwoConnectionsCommandFormat2
            );

            InstanceThreeWayBodyText = new CodeReportShowDifferenceThreeFileCommand(
                commandService
                , PackageIds.CodeReportShowDifferenceBodyTextThreeWayCommandId
                , Report.Schema.Attributes.bodytext
                , Report.Schema.Headers.bodytext
                , ShowDifferenceThreeFileType.ThreeWay
                , Properties.CommandNames.ShowDifferenceThreeWayCommandFormat2
            );
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

            helper.HandleReportThreeFileDifferenceCommand(connectionDataPair.Item1, connectionDataPair.Item2, _fieldName, _fieldTitle, _differenceType);
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

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(applicationObject, menuCommand);
        }
    }
}