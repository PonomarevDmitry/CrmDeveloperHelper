using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandByOpenFilesType : AbstractDynamicCommand<OpenFilesType>
    {
        //{ PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
        //, { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
        //, { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesEqualByTextCommandId, OpenFilesType.EqualByText }

        protected static List<OpenFilesType> _typesExistsOrHasLink = new List<OpenFilesType>()
        {
            OpenFilesType.NotEqualByText
            , OpenFilesType.EqualByText
            , OpenFilesType.NotExistsInCrmWithLink
        };

        protected static List<OpenFilesType> _typesChanges = new List<OpenFilesType>()
        {
            OpenFilesType.WithInserts
            , OpenFilesType.WithDeletes
            , OpenFilesType.WithComplexChanges
        };

        protected static List<OpenFilesType> _typesMirror = new List<OpenFilesType>()
        {
            OpenFilesType.WithMirrorChanges
            , OpenFilesType.WithMirrorInserts
            , OpenFilesType.WithMirrorDeletes
            , OpenFilesType.WithMirrorComplexChanges
        };

        private readonly IList<OpenFilesType> _sourceOpenTypes;

        protected AbstractDynamicCommandByOpenFilesType(OleMenuCommandService commandService, int baseIdStart, IList<OpenFilesType> sourceOpenTypes)
            : base(commandService, baseIdStart, sourceOpenTypes.Count)
        {
            this._sourceOpenTypes = sourceOpenTypes;
        }

        protected override string GetElementName(OpenFilesType openFilesType)
        {
            return Helpers.EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(openFilesType);
        }

        protected override IList<OpenFilesType> GetElementSourceCollection()
        {
            return _sourceOpenTypes;
        }
    }
}