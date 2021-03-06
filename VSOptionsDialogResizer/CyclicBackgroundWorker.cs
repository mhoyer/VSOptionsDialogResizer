using System;
using System.ComponentModel;
using System.Threading;

namespace VSOptionsDialogResizer
{
    public class CyclicBackgroundWorker : ICyclicWorker
    {
        readonly BackgroundWorker _worker;

        public Func<bool> StopAction { get; set; }

        public CyclicBackgroundWorker()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += DoTheJob;
            _worker.WorkerSupportsCancellation = true;

            StopAction = () => false; // abort loop after one pass
        }

        public void Start(int sleep, Action action)
        {
            if (_worker.IsBusy) return;

            _worker.RunWorkerAsync(Delayed(sleep, action));
        }

        void DoTheJob(object sender, DoWorkEventArgs e)
        {
            var action = (Action) e.Argument;
            var backgroundWorker = (BackgroundWorker) sender;

            while (true)
            {
                action.Invoke();

                if (backgroundWorker.CancellationPending ||
                    StopAction.Invoke())
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        public void Stop()
        {
            if (_worker.IsBusy) _worker.CancelAsync();
        }

        Action Delayed(int sleep, Action action)
        {
            return () =>
                {
                    Thread.Sleep(sleep);
                    action.Invoke();
                };
        }
    }
}