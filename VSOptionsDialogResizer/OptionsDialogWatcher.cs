using System;

namespace VSOptionsDialogResizer
{
    public class OptionsDialogWatcher : IOptionsDialogWatcher
    {
        readonly IOptionsDialogFinder _optionsDialogFinder;
        readonly IOptionsDialogModifier _optionsDialogModifier;
        readonly ICyclicWorker _cyclicBackgroundWorker;

        public OptionsDialogWatcher(
            IOptionsDialogFinder optionsDialogFinder,
            IOptionsDialogModifier optionsDialogModifier,
            ICyclicWorker cyclicBackgroundWorker)
        {
            _optionsDialogFinder = optionsDialogFinder;
            _optionsDialogModifier = optionsDialogModifier;
            _cyclicBackgroundWorker = cyclicBackgroundWorker;
        }

        public void Listen(IntPtr mainWindow)
        {
            _cyclicBackgroundWorker.Start(200, () => FindOptionsDialog(mainWindow));
        }

        public void FindOptionsDialog(IntPtr mainWindow)
        {
            var optionsDialogWindow = _optionsDialogFinder.Find(mainWindow);

            if (optionsDialogWindow != IntPtr.Zero)
                _optionsDialogModifier.RefreshUntilClose(optionsDialogWindow);
        }
    }
}