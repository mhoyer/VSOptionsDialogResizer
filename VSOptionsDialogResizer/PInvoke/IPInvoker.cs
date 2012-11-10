using System;
using System.Collections.Generic;

namespace VSOptionsDialogResizer.PInvoke
{
    public interface IPInvoker
    {
        IEnumerable<IntPtr> FindAllWindowsByCaption(string caption);
        IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);
        uint GetWindowLong(IntPtr hWnd, GetWindowLong nIndex);
        uint SetWindowLong(IntPtr hWnd, GetWindowLong nIndex, uint dwNewLong);
        void ResizeWindow(IntPtr hWnd, uint width, uint height);
        Rect GetClientRect(IntPtr hWnd);
        IEnumerable<IntPtr> FindAllChildrenByClassName(IntPtr hWnd, string className);
    }
}
