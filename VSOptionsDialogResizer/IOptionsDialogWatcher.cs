using System;

namespace VSOptionsDialogResizer
{
    public interface IOptionsDialogWatcher
    {
        void Listen(IntPtr mainWindow);
        void StopListen();
    }
}