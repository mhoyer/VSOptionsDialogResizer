using System;
using System.Collections.Generic;

namespace VSOptionsDialogResizer.PInvoke
{
    public interface IPInvoker
    {
        IEnumerable<IntPtr> FindWindows(string caption);

        IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);
    }
}