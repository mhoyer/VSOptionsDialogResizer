using System;

namespace VSOptionsDialogResizer
{
    public interface IWindowPatcher
    {
        void PatchUntilClose(IntPtr window);
    }
}