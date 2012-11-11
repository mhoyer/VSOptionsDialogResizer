using System;

namespace VSOptionsDialogResizer
{
    public class OptionsDialogWatcher : IOptionsDialogWatcher
    {
        readonly IOptionsDialogFinder _optionsDialogFinder;
        readonly IWindowPatcher _windowPatcher;
        readonly ICyclicWorker _cyclicBackgroundWorker;

        public OptionsDialogWatcher(
            IOptionsDialogFinder optionsDialogFinder,
            IWindowPatcher windowPatcher,
            ICyclicWorker cyclicBackgroundWorker)
        {
            _optionsDialogFinder = optionsDialogFinder;
            _windowPatcher = windowPatcher;
            _cyclicBackgroundWorker = cyclicBackgroundWorker;
        }

        public void Listen(IntPtr mainWindow)
        {
            _cyclicBackgroundWorker.Start(200, () => FindOptionsDialog(mainWindow));
        }

        public void StopListen()
        {
        }

        public void FindOptionsDialog(IntPtr mainWindow)
        {
            var optionsDialogWindow = _optionsDialogFinder.Find(mainWindow);

            if (optionsDialogWindow != IntPtr.Zero)
                _windowPatcher.PatchUntilClose(optionsDialogWindow);
        }
    }
}