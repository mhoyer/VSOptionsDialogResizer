using System;
using System.Linq;
using VSOptionsDialogResizer.PInvoke;

namespace VSOptionsDialogResizer.WindowModifiers
{
    public class MainTreeResizer : IWindowModifier
    {
        readonly IPInvoker _pInvoker;

        public MainTreeResizer(IPInvoker pInvoker)
        {
            _pInvoker = pInvoker;
        }

        public void Modify(IntPtr window, uint width, uint height)
        {
            var tree = _pInvoker.FindAllChildrenByClassName(window, "SysTreeView32").FirstOrDefault();
            if (tree == IntPtr.Zero) return;

            var treeRect = _pInvoker.GetWindowRect(tree);
            _pInvoker.MoveWindow(tree,
                                 10,
                                 10,
                                 treeRect.Width,
                                 height - 20, true);
        }
    }
}
