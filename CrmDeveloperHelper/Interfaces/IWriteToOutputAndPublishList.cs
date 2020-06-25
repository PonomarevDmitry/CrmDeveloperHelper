using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    /// <summary>
    /// <see cref="Helpers.DTEHelper"/>
    /// </summary>
    public interface IWriteToOutputAndPublishList : IWriteToOutput
    {
        /// <summary>
        /// <see cref="Helpers.DTEHelper.AddToListForPublish(ConnectionData, IEnumerable{SelectedFile})"/>
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <returns></returns>
        IWriteToOutputAndPublishList AddToListForPublish(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles);

        /// <summary>
        /// <see cref="Helpers.DTEHelper.RemoveFromListForPublish(ConnectionData, List{SelectedFile})"/>
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="selectedFiles"></param>
        /// <returns></returns>
        IWriteToOutputAndPublishList RemoveFromListForPublish(ConnectionData connectionData, List<SelectedFile> selectedFiles);
    }
}