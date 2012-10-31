using System;
using System.Collections.Generic;
using System.Linq;

namespace VSOptionsDialogResizer
{
    public class WindowPatcher : IWindowPatcher
    {
        readonly ICyclicWorker _cyclicWorker;
        readonly IList<IWindowModifier> _modifiers;

        public WindowPatcher(ICyclicWorker cyclicWorker, IList<IWindowModifier> modifiers)
        {
            _cyclicWorker = cyclicWorker;
            _modifiers = modifiers;
        }

        public void PatchUntilClose(IntPtr window)
        {
            _cyclicWorker.Start(20, () => {});
        }

        public void ExecuteAllModifiers(IntPtr window)
        {
            _modifiers.ToList().ForEach(m => m.Modify(window));
        }
    }
}