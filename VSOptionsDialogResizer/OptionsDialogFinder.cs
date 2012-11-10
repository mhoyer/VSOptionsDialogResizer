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
            var optionsWindows = _pinvoker.FindAllWindowsByCaption("Options");
            return optionsWindows.FirstOrDefault(o => _pinvoker.GetWindow(o, GetWindowCmd.GW_OWNER) == devenvMainWindow);
        }
    }
}