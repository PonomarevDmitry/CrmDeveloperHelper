namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public enum ComponentState
    {
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Published = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unpublished = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Deleted = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        DeletedUnpublished = 3,
    }
}
