using System;

namespace VSOptionsDialogResizer
{
    public class OptionsDialogModifier : IOptionsDialogModifier
    {
        readonly ICyclicWorker _cyclicWorker;

        public OptionsDialogModifier(ICyclicWorker cyclicWorker)
        {
            _cyclicWorker = cyclicWorker;
        }

        public void RefreshUntilClose(IntPtr optionsDialog)
        {
            _cyclicWorker.Start(0, null);
        }
    }
}