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
            _cyclicWorker.Start(20, () => ExecuteAllModifiers(optionsWindow));
        }

        public void ExecuteAllModifiers(IntPtr optionsWindow)
        {
            _modifiers.ToList().ForEach(m => m.Modify(optionsWindow));
        }
    }
}