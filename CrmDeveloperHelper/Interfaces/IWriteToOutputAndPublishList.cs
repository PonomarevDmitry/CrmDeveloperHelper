using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IWriteToOutputAndPublishList : IWriteToOutput
    {
        void AddToListForPublish(IEnumerable<SelectedFile> selectedFiles);
    }
}