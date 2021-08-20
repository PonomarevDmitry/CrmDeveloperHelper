namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public enum PrivilegeDepthExtended : int
    {
        None = -10,

        //
        // Summary:
        //     Indicates basic privileges. Users who have basic privileges can only use privileges
        //     to perform actions on objects that are owned by, or shared with, the user. Value
        //     = 0.
        Basic = 0,
        //
        // Summary:
        //     Indicates local privileges. Users who have local privileges can only use privileges
        //     to perform actions on data and objects that are in the user&#39;s current business
        //     unit. Value = 1.
        Local = 1,
        //
        // Summary:
        //     Indicates deep privileges. Users who have deep privileges can perform actions
        //     on all objects in the user&#39;s current business units and all objects down
        //     the hierarchy of business units. Value = 2.
        Deep = 2,
        //
        // Summary:
        //     Indicates global privileges. Users who have global privileges can perform actions
        //     on data and objects anywhere within the organization regardless of the business
        //     unit or user to which it belongs. Value = 3.
        Global = 3
    }
}
