using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static partial class CommonHandlers
    {
        private static bool CheckInPublishListSingle(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            bool result = false;

            if (applicationObject != null)
            {
                try
                {
                    var helper = DTEHelper.Create(applicationObject);

                    var selectedFiles = helper.GetSelectedFilesFromListForPublish();

                    var count = selectedFiles.Where(f => checker(f.FilePath)).Take(2).Count();

                    result = count == 1;
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);

                    result = false;
                }
            }

            return result;
        }

        private static bool CheckInPublishListAny(EnvDTE80.DTE2 applicationObject, Func<string, bool> checker)
        {
            bool result = false;

            if (applicationObject != null)
            {
                try
                {
                    var helper = DTEHelper.Create(applicationObject);

                    var selectedFiles = helper.GetSelectedFilesFromListForPublish();

                    result = selectedFiles.Any(f => checker(f.FilePath));
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);

                    result = false;
                }
            }

            return result;
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceTextSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            if (!menuCommand.Enabled && !menuCommand.Visible)
            {
                return;
            }

            bool visible = CacheValue(nameof(ActionBeforeQueryStatusListForPublishWebResourceTextSingle), applicationObject, ActionBeforeQueryStatusListForPublishWebResourceTextSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusListForPublishWebResourceTextSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInPublishListSingle(applicationObject, FileOperations.SupportsWebResourceTextType);
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceTextAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            if (!menuCommand.Enabled && !menuCommand.Visible)
            {
                return;
            }

            bool visible = CacheValue(nameof(ActionBeforeQueryStatusListForPublishWebResourceTextAny), applicationObject, ActionBeforeQueryStatusListForPublishWebResourceTextAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusListForPublishWebResourceTextAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInPublishListAny(applicationObject, FileOperations.SupportsWebResourceTextType);
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceJavaScriptSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            if (!menuCommand.Enabled && !menuCommand.Visible)
            {
                return;
            }

            bool visible = CacheValue(nameof(ActionBeforeQueryStatusListForPublishWebResourceJavaScriptSingle), applicationObject, ActionBeforeQueryStatusListForPublishWebResourceJavaScriptSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusListForPublishWebResourceJavaScriptSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInPublishListSingle(applicationObject, FileOperations.SupportsJavaScriptType);
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceJavaScriptAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            if (!menuCommand.Enabled && !menuCommand.Visible)
            {
                return;
            }

            bool visible = CacheValue(nameof(ActionBeforeQueryStatusListForPublishWebResourceJavaScriptAny), applicationObject, ActionBeforeQueryStatusListForPublishWebResourceJavaScriptAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusListForPublishWebResourceJavaScriptAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInPublishListAny(applicationObject, FileOperations.SupportsJavaScriptType);
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceSingle(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            if (!menuCommand.Enabled && !menuCommand.Visible)
            {
                return;
            }

            bool visible = CacheValue(nameof(ActionBeforeQueryStatusListForPublishWebResourceSingle), applicationObject, ActionBeforeQueryStatusListForPublishWebResourceSingleInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusListForPublishWebResourceSingleInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInPublishListSingle(applicationObject, FileOperations.SupportsWebResourceType);
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceAny(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            if (!menuCommand.Enabled && !menuCommand.Visible)
            {
                return;
            }

            bool visible = CacheValue(nameof(ActionBeforeQueryStatusListForPublishWebResourceAny), applicationObject, ActionBeforeQueryStatusListForPublishWebResourceAnyInternal);

            if (visible == false)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }

        private static bool ActionBeforeQueryStatusListForPublishWebResourceAnyInternal(EnvDTE80.DTE2 applicationObject)
        {
            return CheckInPublishListAny(applicationObject, FileOperations.SupportsWebResourceType);
        }
    }
}
