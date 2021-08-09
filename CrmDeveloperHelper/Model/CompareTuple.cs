using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class CompareTuple
    {
        public SelectedFile SelectedFile { get; private set; }

        public WebResource WebResource { get; private set; }

        public ContentCompareResult CompareResult { get; set; }

        public CompareTuple(SelectedFile selectedFile, WebResource webResource)
            : this(selectedFile, webResource, null)
        {
        }

        public CompareTuple(SelectedFile selectedFile, WebResource webResource, ContentCompareResult compareResult)
        {
            this.SelectedFile = selectedFile;
            this.WebResource = webResource;
            this.CompareResult = compareResult;
        }
    }
}
