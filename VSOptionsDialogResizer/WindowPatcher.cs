using System;
using System.Collections.Generic;
using System.Linq;
using VSOptionsDialogResizer.PInvoke;
using VSOptionsDialogResizer.WindowModifiers;

namespace VSOptionsDialogResizer
{
    public class WindowPatcher : IWindowPatcher
    {
        readonly ICyclicWorker _cyclicWorker;
        readonly IList<IWindowModifier> _modifiers;
        readonly IPInvoker _pInvoker;

        public WindowPatcher(ICyclicWorker cyclicWorker, IList<IWindowModifier> modifiers, IPInvoker pInvoker)
        {
            _cyclicWorker = cyclicWorker;
            _modifiers = modifiers;
            _pInvoker = pInvoker;
        }

        public void PatchUntilClose(IntPtr optionsWindow)
        {
            _cyclicWorker.StopAction = () => _pInvoker.GetWindow(optionsWindow, GetWindowCmd.GW_OWNER) == IntPtr.Zero;
            MakeWindowResizable(optionsWindow);
            _pInvoker.ResizeWindow(optionsWindow, 800, 768);

            _cyclicWorker.Start(20, () => ExecuteAllModifiers(optionsWindow));
        }

        public void ExecuteAllModifiers(IntPtr optionsWindow)
        {
            var clientRect = _pInvoker.GetClientRect(optionsWindow);

            _modifiers.ToList().ForEach(m => m.Modify(optionsWindow, clientRect.Width, clientRect.Height));
        }

        void MakeWindowResizable(IntPtr optionsWindow)
        {
            _pInvoker.SetWindowLong(optionsWindow,
                                    GetWindowLong.GWL_STYLE,
                                    (uint)(WindowStyles.WS_CAPTION |
                                            WindowStyles.WS_VISIBLE |
                                            WindowStyles.WS_THICKFRAME |
                                            WindowStyles.WS_POPUP));
        }
    }
}