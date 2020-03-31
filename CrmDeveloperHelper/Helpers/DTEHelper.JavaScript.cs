using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleJavaScriptEntityMetadataFileUpdate(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool selectEntity)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigOpenOptionsAndExecute(connectionData, (conn, commonConfig, openOptions) => Controller.StartJavaScriptEntityMetadataFileUpdatingSchema(conn, commonConfig, selectedFiles, selectEntity, openOptions));
        }

        public void HandleJavaScriptGlobalOptionSetFileUpdateSingle(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool selectEntity)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigOpenOptionsAndExecute(connectionData, (conn, commonConfig, openOptions) => Controller.StartJavaScriptGlobalOptionSetFileUpdatingSingle(conn, commonConfig, selectedFiles, selectEntity, openOptions));
        }

        public void HandleJavaScriptUpdateGlobalOptionSetFileAll(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigOpenOptionsAndExecute(connectionData, (conn, commonConfig, openOptions) => Controller.StartJavaScriptGlobalOptionSetFileUpdatingAll(conn, commonConfig, selectedFile, openOptions));
        }
    }
}
