
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Dependency Type
    /// 
    /// Description:
    ///     (English - United States - 1033): The kind of dependency.
    /// 
    /// OptionSet Name: dependencytype      IsCustomOptionSet: False
    ///</summary>
    public enum dependencytype
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): None
        ///</summary>
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Solution Internal
        ///</summary>
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Solution_Internal_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Published
        ///</summary>
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Published_2 = 2,

        ///<summary>
        /// 4
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Unpublished
        ///</summary>
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unpublished_4 = 4,
    }
}