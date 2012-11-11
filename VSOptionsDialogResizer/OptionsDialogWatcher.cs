using System;

namespace VSOptionsDialogResizer
{
    public class OptionsDialogWatcher : IOptionsDialogWatcher
    {
        readonly IOptionsDialogFinder _optionsDialogFinder;
        readonly IWindowPatcher _windowPatcher;
        readonly ICyclicWorker _cyclicBackgroundWorker;
        private bool _stopWorker;

        public OptionsDialogWatcher(
            IOptionsDialogFinder optionsDialogFinder,
            IWindowPatcher windowPatcher,
            ICyclicWorker cyclicBackgroundWorker)
        {
            _optionsDialogFinder = optionsDialogFinder;
            _windowPatcher = windowPatcher;
            _cyclicBackgroundWorker = cyclicBackgroundWorker;
            _cyclicBackgroundWorker.StopAction = () => _stopWorker;
        }

        public void Listen(IntPtr mainWindow) 
        {
            _stopWorker = false;
            _cyclicBackgroundWorker.Start(200, () => FindOptionsDialog(mainWindow));
        }

        public void StopListen()
        {
            _stopWorker = true;
        }

        public void FindOptionsDialog(IntPtr mainWindow)
        {
            var optionsDialogWindow = _optionsDialogFinder.Find(mainWindow);

            if (optionsDialogWindow != IntPtr.Zero)
                _windowPatcher.PatchUntilClose(optionsDialogWindow);
        }
    }
}