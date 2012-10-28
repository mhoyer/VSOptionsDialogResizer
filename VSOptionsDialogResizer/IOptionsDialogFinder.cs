using System;

namespace VSOptionsDialogResizer
{
    public interface IOptionsDialogFinder
    {
        IntPtr Find(IntPtr devenvMainWindow);
    }
}
