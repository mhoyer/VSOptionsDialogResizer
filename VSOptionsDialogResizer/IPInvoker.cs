using System;
using System.Collections.Generic;

namespace VSOptionsDialogResizer
{
    public interface IPInvoker
    {
        IEnumerable<IntPtr> FindWindows(string caption);
    }
}