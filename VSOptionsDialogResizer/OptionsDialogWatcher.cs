using System;
using System.ComponentModel;

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
            using (var bw = new BackgroundWorker())
            {
                bw.DoWork += ListenForOptionsDialog;
                bw.RunWorkerAsync(mainWindow);
            }
        }

        void ListenForOptionsDialog(object sender, DoWorkEventArgs e)
        {
            var mainWindow = (IntPtr) e.Argument;

            while (true)
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