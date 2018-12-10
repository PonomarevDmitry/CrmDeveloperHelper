using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("text")]
    [TagType(typeof(UrlTag))]
    public class CustomUrlTagProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new CustomUrlTagger(buffer) as ITagger<T>;
        }
    }
}