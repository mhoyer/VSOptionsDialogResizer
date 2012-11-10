using System;

namespace VSOptionsDialogResizer
{
    public interface ICyclicWorker
    {
        void Start(int sleep, Action action);
    }
}