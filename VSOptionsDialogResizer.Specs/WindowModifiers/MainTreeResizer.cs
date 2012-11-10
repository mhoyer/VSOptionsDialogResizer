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
        Establish context = () =>
            {
                The<IPInvoker>()
                    .WhenToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "SysTreeView32"))
                    .Return(new[] { _tree });

                The<IPInvoker>()
                    .WhenToldTo(p => p.GetWindowRect(_tree))
                    .Return(new Rect{ X1 = 10, Y1 = 10,
                                      X2 = 250, Y2 = 500});
            };

        Because of = () => Subject.Modify(_optionsWindow, 400, 200);

        It should_determine_the_main_tree = 
            () => The<IPInvoker>().WasToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "SysTreeView32"));

        It should_get_the_rect_of_the_main_tree = 
            () => The<IPInvoker>().WasToldTo(p => p.GetWindowRect(_tree));

        It should_move_the_tree_but_keep_the_x_offset =
            () => The<IPInvoker>().WasToldTo(p => p.MoveWindow(_tree,
                                                               10, // = X1
                                                               Param.IsAny<int>(),
                                                               Param.IsAny<uint>(),
                                                               Param.IsAny<uint>(),
                                                               true));

        static readonly IntPtr _optionsWindow = new IntPtr(1);
        static readonly IntPtr _tree = new IntPtr(2);
    }

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
            _pInvoker.MoveWindow(tree, treeRect.X1, 0, 0, 0, true);
        }
    }
}