using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Threading;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public class CrmCompletionCommandHandler : IOleCommandTarget
    {
        private IOleCommandTarget _nextCommandHandler;
        private ITextView _textView;
        private CrmTextViewCreationListener _provider;
        private ICompletionSession _session;

        public CrmCompletionCommandHandler(IVsTextView textViewAdapter, ITextView textView, CrmTextViewCreationListener provider)
        {
            this._textView = textView;
            this._provider = provider;

            var hresult = textViewAdapter.AddCommandFilter(this, out _nextCommandHandler);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            var ret = _nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);

            if (pguidCmdGroup == VSConstants.VSStd2K)
            {
                var any = prgCmds.Any(e =>
                    e.cmdID == (uint)VSConstants.VSStd2KCmdID.AUTOCOMPLETE
                    || e.cmdID == (uint)VSConstants.VSStd2KCmdID.COMPLETEWORD
                    || e.cmdID == (uint)VSConstants.VSStd2KCmdID.SHOWMEMBERLIST
                    || e.cmdID == (uint)VSConstants.VSStd2KCmdID.NavigateForward
                    || e.cmdID == (uint)VSConstants.VSStd2KCmdID.COMPLETION_HIDE_ADVANCED
                    || e.cmdID == (uint)VSConstants.VSStd2KCmdID.QUICKINFO
                );

                if (any)
                {
                    return VSConstants.S_OK;
                }
            }

            return ret;
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (VsShellUtilities.IsInAutomationFunction(_provider.ServiceProvider))
            {
                return _nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            }

            //make a copy of this so we can look at it after forwarding some commands
            uint commandID = nCmdID;
            char typedChar = char.MinValue;

            //make sure the input is a char before getting it
            if (pguidCmdGroup == VSConstants.VSStd2K && nCmdID == (uint)VSConstants.VSStd2KCmdID.TYPECHAR)
            {
                typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);
            }

            //check for a commit character
            if (nCmdID == (uint)VSConstants.VSStd2KCmdID.RETURN
                || nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB
                || ((char.IsPunctuation(typedChar) && typedChar != '_' && typedChar != '-' && typedChar != '/' && typedChar != '\\'))
                )
            {
                //check for a a selection
                if (_session != null && !_session.IsDismissed)
                {
                    //if the selection is fully selected, commit the current session
                    if (_session.SelectedCompletionSet.SelectionStatus.IsSelected)
                    {
                        _session?.Commit();

                        //also, don't add the character to the buffer
                        return VSConstants.S_OK;
                    }
                    else
                    {
                        //if there is no selection, dismiss the session
                        _session?.Dismiss();
                    }
                }
            }

            //pass along the command so the char is added to the buffer
            int retVal = _nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            if (nCmdID == (uint)VSConstants.VSStd2KCmdID.AUTOCOMPLETE
                || nCmdID == (uint)VSConstants.VSStd2KCmdID.COMPLETEWORD
                || nCmdID == (uint)VSConstants.VSStd2KCmdID.SHOWMEMBERLIST
                || nCmdID == (uint)VSConstants.VSStd2KCmdID.COMPLETION_HIDE_ADVANCED
                || nCmdID == (uint)VSConstants.VSStd2KCmdID.QUICKINFO
                || (!typedChar.Equals(char.MinValue) 
                    && (char.IsLetterOrDigit(typedChar) 
                            || typedChar == ' ' 
                            || typedChar == '_' 
                            || typedChar == '-' 
                            || typedChar == '.' 
                            || typedChar == ','
                            || typedChar == '/'
                            || typedChar == '\\'
                            ))
                )
            {
                if (_session == null && _provider.CompletionBroker.IsCompletionActive(_textView))
                {
                    _session = _provider.CompletionBroker.GetSessions(_textView).FirstOrDefault();
                }

                if (_session == null || _session.IsDismissed)
                {
                    this.TriggerCompletion();
                }

                _session?.Filter();
            }
            else if (commandID == (uint)VSConstants.VSStd2KCmdID.BACKSPACE
                || commandID == (uint)VSConstants.VSStd2KCmdID.DELETE
                )
            {
                if (_session != null && !_session.IsDismissed)
                {
                    _session?.Filter();
                }
            }

            return retVal;
        }

        private bool TriggerCompletion()
        {
            SnapshotPoint? caretPoint = _textView.Caret.Position.Point.GetPoint(
                textBuffer => (!textBuffer.ContentType.IsOfType("projection"))
                , PositionAffinity.Predecessor);

            if (!caretPoint.HasValue)
            {
                return false;
            }

            var list = _provider.CompletionBroker.GetSessions(_textView).ToList();

            if (_provider.CompletionBroker.IsCompletionActive(_textView) || list.Any())
            {
                _session = list.FirstOrDefault();
                _session.Dismissed += this.OnSessionDismissed;

                return true;
            }

            _provider.CompletionBroker.DismissAllSessions(_textView);

            _session = _provider.CompletionBroker.CreateCompletionSession(
                _textView
                , caretPoint.Value.Snapshot.CreateTrackingPoint(caretPoint.Value.Position, PointTrackingMode.Positive)
                , true);

            if (_session != null)
            {
                //subscribe to the Dismissed event on the session 
                _session.Dismissed += this.OnSessionDismissed;

                _session.Start();
            }

            return true;
        }

        private void OnSessionDismissed(object sender, EventArgs e)
        {
            if (sender is ICompletionSession session)
            {
                session.Dismissed -= this.OnSessionDismissed;
            }

            _session.Dismissed -= this.OnSessionDismissed;
            _session = null;
        }
    }
}