using System;
using System.Threading;

namespace VSOptionsDialogResizer
{
    public class CyclicWorker : ICyclicWorker
    {
        readonly Func<bool> _stopAction;

        public CyclicWorker(Func<bool> stopAction)
        {
            _stopAction = stopAction;
        }

        public void Start(int sleep, Action action)
        {
            while (true)
            {
                action.Invoke();
                if (_stopAction.Invoke()) break;
                Thread.Sleep(sleep);
            }
        }
    }

}