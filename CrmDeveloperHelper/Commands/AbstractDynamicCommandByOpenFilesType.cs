using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandByOpenFilesType : AbstractDynamicCommand<OpenFilesType>
    {
        protected static Dictionary<OpenFilesType, SelectedFileType> _selectedFileTypeForOpenFilesType = new Dictionary<OpenFilesType, SelectedFileType>()
        {
            { OpenFilesType.All, SelectedFileType.WebResource }

            , { OpenFilesType.NotExistsInCrmWithLink, SelectedFileType.WebResource }
            , { OpenFilesType.NotExistsInCrmWithoutLink, SelectedFileType.WebResource }

            , { OpenFilesType.NotEqualByText, SelectedFileType.WebResourceText }
            , { OpenFilesType.EqualByText, SelectedFileType.WebResourceText }

            , { OpenFilesType.WithInserts, SelectedFileType.WebResourceText }
            , { OpenFilesType.WithDeletes, SelectedFileType.WebResourceText }
            , { OpenFilesType.WithComplexChanges, SelectedFileType.WebResourceText }

            , { OpenFilesType.WithMirrorChanges, SelectedFileType.WebResourceText }
            , { OpenFilesType.WithMirrorInserts, SelectedFileType.WebResourceText }
            , { OpenFilesType.WithMirrorDeletes, SelectedFileType.WebResourceText }
            , { OpenFilesType.WithMirrorComplexChanges, SelectedFileType.WebResourceText }
        };

        protected static List<OpenFilesType> _typesOrdinal = new List<OpenFilesType>()
        {
            OpenFilesType.All
            , OpenFilesType.NotEqualByText
            , OpenFilesType.EqualByText
            , OpenFilesType.NotExistsInCrmWithLink
            , OpenFilesType.NotExistsInCrmWithoutLink
        };

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
            : base(commandService, PackageGuids.guidDynamicCommandSet, baseIdStart, sourceOpenTypes.Count)
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