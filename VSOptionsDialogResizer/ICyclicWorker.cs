using System;

namespace VSOptionsDialogResizer
{
    public interface ICyclicWorker
    {
        Func<bool> StopAction { get; set; }
        void Start(int sleep, Action action);
    }
}