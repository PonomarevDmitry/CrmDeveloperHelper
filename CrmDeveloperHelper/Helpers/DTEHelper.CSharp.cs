using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleCSharpEntityMetadataFileUpdateSchema(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool selectEntity)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigOpenOptionsAndExecute(connectionData, (conn, commonConfig, openOptions) => Controller.StartCSharpEntityMetadataUpdatingFileWithSchema(conn, commonConfig, selectedFiles, selectEntity, openOptions));
        }

        public void HandleCSharpEntityMetadataFileUpdateProxyClassOrSchema(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool selectEntity)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigOpenOptionsAndExecute(connectionData, (conn, commonConfig, openOptions) => Controller.StartCSharpEntityMetadataUpdatingFileWithProxyClassOrSchema(conn, commonConfig, selectedFiles, selectEntity, openOptions));
        }

        public void HandleCSharpEntityMetadataFileUpdateProxyClass(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool selectEntity)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigOpenOptionsAndExecute(connectionData, (conn, commonConfig, openOptions) => Controller.StartCSharpEntityMetadataUpdatingFileWithProxyClass(conn, commonConfig, selectedFiles, selectEntity, openOptions));
        }

        public void HandleCSharpGlobalOptionSetsFileUpdateSchema(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withSelect)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigOpenOptionsAndExecute(connectionData, (conn, commonConfig, openOptions) => Controller.StartCSharpGlobalOptionSetsFileUpdatingSchema(conn, commonConfig, selectedFiles, withSelect, openOptions));
        }
    }
}
