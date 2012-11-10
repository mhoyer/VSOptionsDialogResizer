using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using VSOptionsDialogResizer.PInvoke;
using VSOptionsDialogResizer.WindowModifiers;

namespace VSOptionsDialogResizer.Specs.WindowModifiers
{
    public class when_arranging_the_main_tree_of_the_options_dialog : WithSubject<MainTreeResizer>
    {
        Establish context = 
            () => The<IPInvoker>()
                .WhenToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "SysTreeView32"))
                .Return(new[]{_tree});

        Because of = () => Subject.Modify(_optionsWindow, 400, 200);

        It should_determine_the_main_tree = 
            () => The<IPInvoker>().WasToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "SysTreeView32"));

        It should_get_the_rect_of_the_main_tree = 
            () => The<IPInvoker>().WasToldTo(p => p.GetWindowRect(_tree));

        static IntPtr _optionsWindow = new IntPtr(1);
        static IntPtr _tree = new IntPtr(2);
    }

    public class MainTreeResizer : IWindowModifier
    {
        readonly IPInvoker _pInvoker;

        public MainTreeResizer(IPInvoker pInvoker)
        {
            _pInvoker = pInvoker;
        }

        public void Modify(IntPtr window, int width, int height)
        {
            var tree = _pInvoker.FindAllChildrenByClassName(window, "SysTreeView32").FirstOrDefault();
            if (tree == IntPtr.Zero) return;

            _pInvoker.GetWindowRect(tree);
        }
    }
}