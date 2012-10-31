using System;

namespace VSOptionsDialogResizer
{
    public class WindowPatcher : IWindowPatcher
    {
        readonly ICyclicWorker _cyclicWorker;
        readonly IWindowModifier _modifier;

        public WindowPatcher(ICyclicWorker cyclicWorker, IWindowModifier modifier)
        {
            _cyclicWorker = cyclicWorker;
            _modifier = modifier;
        }

        public void PatchUntilClose(IntPtr window)
        {
            _cyclicWorker.Start(20, () => {});
        }

        public void ExecuteModifier(IntPtr window)
        {
            _modifier.Modify(window);
        }
    }
}