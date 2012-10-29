using System;
using System.Collections.Generic;
using VSOptionsDialogResizer.PInvoke;

namespace VSOptionsDialogResizer
{
    public interface IPInvoker
    {
        IEnumerable<IntPtr> FindWindows(string caption);

        IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);
    }
}