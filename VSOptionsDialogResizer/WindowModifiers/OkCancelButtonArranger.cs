using System;
using System.Linq;
using VSOptionsDialogResizer.PInvoke;

namespace VSOptionsDialogResizer.WindowModifiers
{
    public class OkCancelButtonArranger : IWindowModifier
    {
        readonly IPInvoker _pInvoker;

        public OkCancelButtonArranger(IPInvoker pInvoker)
        {
            _pInvoker = pInvoker;
        }

        public void Modify(IntPtr window, uint width, uint height)
        {
            var buttons = _pInvoker.FindAllChildrenByClassName(window, "Button");
            var okButton = buttons.FirstOrDefault(b => _pInvoker.GetWindowText(b) == "OK");
            var cancelButton = buttons.FirstOrDefault(b => _pInvoker.GetWindowText(b) == "Cancel");

            var cancelButtonRect = _pInvoker.GetWindowRect(cancelButton);
            _pInvoker.MoveWindow(cancelButton,
                                 (int)(width - cancelButtonRect.Width - 10),
                                 (int)(height - 10 - cancelButtonRect.Height),
                                 cancelButtonRect.Width,
                                 cancelButtonRect.Height,
                                 true);

            var okButtonRect = _pInvoker.GetWindowRect(okButton);
            _pInvoker.MoveWindow(okButton,
                                 (int)(width - okButtonRect.Width - cancelButtonRect.Width - 20),
                                 (int)(height - 10 - okButtonRect.Height),
                                 okButtonRect.Width,
                                 okButtonRect.Height,
                                 true);
        }
    }
}