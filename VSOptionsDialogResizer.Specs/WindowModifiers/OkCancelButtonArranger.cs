using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using VSOptionsDialogResizer.PInvoke;
using VSOptionsDialogResizer.WindowModifiers;

namespace VSOptionsDialogResizer.Specs.WindowModifiers
{
    public class when_arranging_the_cancel_button_of_the_options_dialog : WithSubject<OkCancelButtonArranger>
    {
        Establish context = () =>
            {
                The<IPInvoker>()
                    .WhenToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "Button"))
                    .Return(new[] { _cancel });
            };

        Because of = () => Subject.Modify(_optionsWindow, 400, 200);

        It should_determine_cancel_button =
            () => The<IPInvoker>().WasToldTo(p => p.GetWindowText(_cancel));

        static readonly IntPtr _optionsWindow = new IntPtr(1);
        static readonly IntPtr _cancel = new IntPtr(3);
    }

    public class when_arranging_the_ok_button_of_the_options_dialog : WithSubject<OkCancelButtonArranger>
    {
        Establish context = () =>
            {
                The<IPInvoker>()
                    .WhenToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "Button"))
                    .Return(new[] { _ok });

                The<IPInvoker>()
                    .WhenToldTo(p => p.GetWindowText(_ok))
                    .Return("OK");

                The<IPInvoker>()
                    .WhenToldTo(p => p.GetWindowRect(_ok))
                    .Return(new Rect
                        {
                            X1 = 1000, Y1 = 1000,
                            X2 = 1200, Y2 = 1050
                        });
            };

        Because of = () => Subject.Modify(_optionsWindow, 400, 200);

        It should_determine_all_buttons =
            () => The<IPInvoker>().WasToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "Button"));

        It should_determine_ok_button =
            () => The<IPInvoker>().WasToldTo(p => p.GetWindowText(_ok));

        It should_determine_current_position_of_ok_button =
            () => The<IPInvoker>().WasToldTo(p => p.GetWindowRect(_ok));

        It should_set_new_position_of_ok_button_but_keep_the_width =
            () => The<IPInvoker>().WasToldTo(p => p.MoveWindow(_ok,
                                                               Param.IsAny<int>(),
                                                               Param.IsAny<int>(),
                                                               200, // = X2-X1
                                                               Param.IsAny<uint>(),
                                                               true));

        It should_set_new_position_of_ok_button_but_keep_the_height =
            () => The<IPInvoker>().WasToldTo(p => p.MoveWindow(_ok,
                                                               Param.IsAny<int>(),
                                                               Param.IsAny<int>(),
                                                               Param.IsAny<uint>(),
                                                               50, // = Y2-Y1
                                                               true));

        It should_set_new_position_of_ok_button_but_set_new_left_offset =
            () => The<IPInvoker>().WasToldTo(p => p.MoveWindow(_ok,
                                                               180, // newWidthOfOptionsDialog - widthOfOk - widthOfCancel - 20
                                                               Param.IsAny<int>(),
                                                               Param.IsAny<uint>(),
                                                               Param.IsAny<uint>(),
                                                               true));

        It should_set_new_position_of_ok_button_but_set_new_top_offset =
            () => The<IPInvoker>().WasToldTo(p => p.MoveWindow(_ok,
                                                               Param.IsAny<int>(),
                                                               140, // newHeightOfOptionsDialog - heightOfOk - 10
                                                               Param.IsAny<uint>(),
                                                               Param.IsAny<uint>(),
                                                               true));

        static readonly IntPtr _optionsWindow = new IntPtr(1);
        static readonly IntPtr _ok = new IntPtr(2);
    }

    public class OkCancelButtonArranger : IWindowModifier
    {
        readonly IPInvoker _pInvoker;

        public OkCancelButtonArranger(IPInvoker pInvoker)
        {
            _pInvoker = pInvoker;
        }

        public void Modify(IntPtr window, uint width, uint height)
        {
            var button = _pInvoker.FindAllChildrenByClassName(window, "Button").First();
            if (_pInvoker.GetWindowText(button) != "OK") return;
            var buttonRect = _pInvoker.GetWindowRect(button);
            _pInvoker.MoveWindow(button,
                                 (int) (width - buttonRect.Width - 20),
                                 (int) (height - 10 - buttonRect.Height),
                                 buttonRect.Width,
                                 buttonRect.Height,
                                 true);
        }
    }
}