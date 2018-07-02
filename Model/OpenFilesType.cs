namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public enum OpenFilesType
    {
        All = 0,

        EqualByText,

        NotEqualByText,

        WithInserts,

        WithDeletes,

        WithComplex,

        WithMirror,

        WithMirrorInserts,

        WithMirrorDeletes,

        WithMirrorComplex,

        NotExistsInCrmWithoutLink,

        NotExistsInCrmWithLink
    }
}