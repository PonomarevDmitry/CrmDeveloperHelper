using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    public class UrlCommandFilterProvider : IVsTextViewCreationListener
    {
        [ImportingConstructor]
        public UrlCommandFilterProvider(SVsServiceProvider serviceProvider, IVsEditorAdaptersFactoryService editorAdaptersFactoryService, IViewTagAggregatorFactoryService viewTagAggregatorFactoryService)
        {
            this.ServiceProvider = serviceProvider;
            this.EditorAdaptersFactoryService = editorAdaptersFactoryService;
            this.ViewTagAggregatorFactoryService = viewTagAggregatorFactoryService;
        }

        public SVsServiceProvider ServiceProvider
        {
            get;
            private set;
        }

        public IVsEditorAdaptersFactoryService EditorAdaptersFactoryService
        {
            get;
            private set;
        }

        public IViewTagAggregatorFactoryService ViewTagAggregatorFactoryService
        {
            get;
            private set;
        }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = EditorAdaptersFactoryService.GetWpfTextView(textViewAdapter);
            if (textView == null)
                return;

            Func<UrlCommandFilter> createCommandHandler = delegate () { return new UrlCommandFilter(textViewAdapter, textView, this); };
            textView.Properties.GetOrCreateSingletonProperty(createCommandHandler);
        }
    }
}
