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
            while(true)
            {
                var optionsDialogWindow = _optionsDialogFinder.Find(mainWindow);
                
                if (optionsDialogWindow != IntPtr.Zero)
                {
                    break;
                }
            }
        }
    }
}