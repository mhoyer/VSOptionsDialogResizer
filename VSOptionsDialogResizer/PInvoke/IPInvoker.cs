using System;
using System.Collections.Generic;

namespace VSOptionsDialogResizer.PInvoke
{
    public interface IPInvoker
    {
        IEnumerable<IntPtr> FindWindows(string caption);
        IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);
        uint GetWindowLong(IntPtr hWnd, GetWindowLong nIndex);
        uint SetWindowLong(IntPtr hWnd, GetWindowLong nIndex, uint dwNewLong);
        void ResizeWindow(IntPtr hWnd, uint width, uint height);
    }
}