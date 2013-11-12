using System;

namespace OdessaGUIProject.UI_Helpers
{
    internal class HighlightEventArgs : EventArgs
    {
        internal HighlightObject HighlightObject;

        internal HighlightEventArgs(HighlightObject highlightObject)
        {
            this.HighlightObject = highlightObject;
        }
    }
}