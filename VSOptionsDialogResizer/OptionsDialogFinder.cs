using System;

namespace VSOptionsDialogResizer
{
    public class OptionsDialogFinder : IOptionsDialogFinder
    {
        readonly IPInvoker _pinvoker;

        public OptionsDialogFinder(IPInvoker pinvoker)
        {
            _pinvoker = pinvoker;
        }

        public IntPtr Find(IntPtr devenvMainWindow)
        {
            _pinvoker.FindWindows("Options");
            return IntPtr.Zero;
        }
    }
}