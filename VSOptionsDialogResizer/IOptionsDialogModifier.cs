using System;

namespace VSOptionsDialogResizer
{
    public interface IOptionsDialogModifier
    {
        void RefreshUntilClose(IntPtr optionsDialog);
    }
}