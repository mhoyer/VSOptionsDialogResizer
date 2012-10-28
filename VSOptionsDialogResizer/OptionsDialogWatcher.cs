using System;

namespace VSOptionsDialogResizer
{
    public class OptionsDialogWatcher : IOptionsDialogWatcher
    {
        readonly IOptionsDialogFinder _optionsDialogFinder;

        public OptionsDialogWatcher(IOptionsDialogFinder optionsDialogFinder)
        {
            _optionsDialogFinder = optionsDialogFinder;
        }

        public void Listen(IntPtr mainWindow)
        {
            _optionsDialogFinder.Find(mainWindow);
        }
    }
}