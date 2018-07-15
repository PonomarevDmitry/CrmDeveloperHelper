using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public static class CrmPathContentTypeDefinition
    {
        public const string CrmPathContentType = "CrmPath";

        [Export(typeof(ContentTypeDefinition))]
        [Name(CrmPathContentType)]
        [BaseDefinition("code")]
        [BaseDefinition("intellisense")]
        public static ContentTypeDefinition ICrmPathContentType { get; set; }

        [Export(typeof(FileExtensionToContentTypeDefinition))]
        [FileExtension(".crmpath")]
        [ContentType(CrmPathContentType)]
        public static FileExtensionToContentTypeDefinition CrmPathFileExtension { get; set; }
    }
}