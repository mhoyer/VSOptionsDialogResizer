using System;
using System.ComponentModel;

namespace VSOptionsDialogResizer
{
    public class OptionsDialogWatcher : IOptionsDialogWatcher
    {
        readonly IOptionsDialogFinder _optionsDialogFinder;
        readonly IOptionsDialogModifier _optionsDialogModifier;
        BackgroundWorker _listenWorker;

        public OptionsDialogWatcher(
            IOptionsDialogFinder optionsDialogFinder,
            IOptionsDialogModifier optionsDialogModifier)
        {
            _optionsDialogFinder = optionsDialogFinder;
            _optionsDialogModifier = optionsDialogModifier;
        }

        public void Listen(IntPtr mainWindow)
        {
            if (_listenWorker == null)
            {
                _listenWorker = new BackgroundWorker();

                _listenWorker.DoWork += ListenForOptionsDialog;
                _listenWorker.WorkerSupportsCancellation = true;
            }

            if (_listenWorker.IsBusy) return;

            _listenWorker.RunWorkerAsync(mainWindow);
        }

        public void Stop()
        {
            if (_listenWorker == null) return;

            _listenWorker.CancelAsync();
        }

        void ListenForOptionsDialog(object sender, DoWorkEventArgs e)
        {
            var mainWindow = (IntPtr) e.Argument;
            var backgroundWorker = (BackgroundWorker) sender;

            while (true)
            {
                var optionsDialogWindow = _optionsDialogFinder.Find(mainWindow);

                if (optionsDialogWindow != IntPtr.Zero)
                {
                    _optionsDialogModifier.RefreshUntilClose(optionsDialogWindow);
                }

                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
            }
        }
    }
}