using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    [Export(typeof(IVsTextViewCreationListener))]
    [Name("CrmXmlHandler")]
    [ContentType("XML")]
    [ContentType(CrmPathContentTypeDefinition.CrmPathContentType)]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class CrmTextViewCreationListener : IVsTextViewCreationListener
    {
        [Import(typeof(IVsEditorAdaptersFactoryService))]
        public IVsEditorAdaptersFactoryService AdapterService { get; set; }

        [Import(typeof(ICompletionBroker))]
        public ICompletionBroker CompletionBroker { get; set; }

        [Import(typeof(SVsServiceProvider))]
        public SVsServiceProvider ServiceProvider { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);

            if (textView == null)
            {
                return;
            }

            Func<CrmCompletionCommandHandler> createCommandHandler = delegate () { return new CrmCompletionCommandHandler(textViewAdapter, textView, this); };
            textView.Properties.GetOrCreateSingletonProperty(createCommandHandler);
        }
    }
}