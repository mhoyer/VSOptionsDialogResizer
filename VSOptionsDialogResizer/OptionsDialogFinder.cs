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
            var optionsWindow = _pinvoker
                .FindAllWindowsByCaption("Options")
                .FirstOrDefault(o => _pinvoker.GetWindow(o, GetWindowCmd.GW_OWNER) == devenvMainWindow);

            if (_pinvoker.GetClassName(optionsWindow) == "#32770") return optionsWindow;
            
            return IntPtr.Zero;
        }
    }
}