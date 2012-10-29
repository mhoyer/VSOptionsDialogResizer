using System;
using System.Linq;
using VSOptionsDialogResizer.PInvoke;

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
            var optionsWindows = _pinvoker.FindWindows("Options");
            _pinvoker.GetWindow(optionsWindows.First(), GetWindowCmd.GW_OWNER);
            return IntPtr.Zero;
        }
    }
}