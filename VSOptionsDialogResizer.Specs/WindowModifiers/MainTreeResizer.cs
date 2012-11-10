using System;
using Machine.Fakes;
using Machine.Specifications;
using VSOptionsDialogResizer.PInvoke;
using VSOptionsDialogResizer.WindowModifiers;

namespace VSOptionsDialogResizer.Specs.WindowModifiers
{
    public class when_arranging_the_main_tree_of_the_options_dialog : WithSubject<MainTreeResizer>
    {
        Because of = () => Subject.Modify(_optionsWindow, 400, 200);

        It should_determine_the_main_tree = 
            () => The<IPInvoker>().WasToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "SysTreeView32"));

        static IntPtr _optionsWindow = new IntPtr(1);
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
            _pInvoker.FindAllChildrenByClassName(window, "SysTreeView32");
        }
    }
}