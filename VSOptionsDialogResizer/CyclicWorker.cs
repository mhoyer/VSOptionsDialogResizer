using System;
using System.Threading;

namespace VSOptionsDialogResizer
{
    public class CyclicWorker : ICyclicWorker
    {
        public Func<bool> StopAction { get; set; }

        public CyclicWorker()
        {
            StopAction = () => true; // abort loop after one pass
        }

        public void Start(int sleep, Action action)
        {
            while (true)
            {
                action.Invoke();
                if (StopAction.Invoke()) break;
                Thread.Sleep(sleep);
            }
        }
    }

}