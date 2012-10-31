using System;

namespace VSOptionsDialogResizer
{
    public class WindowPatcher : IWindowPatcher
    {
        readonly ICyclicWorker _cyclicWorker;

        public WindowPatcher(ICyclicWorker cyclicWorker)
        {
            _cyclicWorker = cyclicWorker;
        }

        public void PatchUntilClose(IntPtr window)
        {
            _cyclicWorker.Start(20, () => {});
        }
    }
}