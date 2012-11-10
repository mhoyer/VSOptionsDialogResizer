using System;
using System.Collections.Generic;
using System.Linq;
using VSOptionsDialogResizer.PInvoke;

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

        public void PatchUntilClose(IntPtr window)
        {
            _cyclicWorker.StopAction = () => _pInvoker.GetWindow(window, GetWindowCmd.GW_OWNER) == IntPtr.Zero;
            _cyclicWorker.Start(20, () => ExecuteAllModifiers(window));
        }

        public void ExecuteAllModifiers(IntPtr window)
        {
            _modifiers.ToList().ForEach(m => m.Modify(window));
        }
    }
}