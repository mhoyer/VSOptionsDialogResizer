using System;

namespace VSOptionsDialogResizer
{
    public interface ICyclicBackgroundWorker
    {
        void Start(int sleep, Action action);
        void Stop();
    }
}