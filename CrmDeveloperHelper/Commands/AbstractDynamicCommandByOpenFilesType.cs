using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandByOpenFilesType : AbstractDynamicCommand<OpenFilesType>
    {
        protected static Dictionary<OpenFilesType, WebResourceType> _comparersForOpenFilesType = new Dictionary<OpenFilesType, WebResourceType>()
        {
            { OpenFilesType.All, WebResourceType.Ordinal }

            , { OpenFilesType.NotExistsInCrmWithLink, WebResourceType.Ordinal }
            , { OpenFilesType.NotExistsInCrmWithoutLink, WebResourceType.Ordinal }

            , { OpenFilesType.NotEqualByText, WebResourceType.SupportsText }
            , { OpenFilesType.EqualByText, WebResourceType.SupportsText }

            , { OpenFilesType.WithInserts, WebResourceType.SupportsText }
            , { OpenFilesType.WithDeletes, WebResourceType.SupportsText }
            , { OpenFilesType.WithComplexChanges, WebResourceType.SupportsText }

            , { OpenFilesType.WithMirrorChanges, WebResourceType.SupportsText }
            , { OpenFilesType.WithMirrorInserts, WebResourceType.SupportsText }
            , { OpenFilesType.WithMirrorDeletes, WebResourceType.SupportsText }
            , { OpenFilesType.WithMirrorComplexChanges, WebResourceType.SupportsText }
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