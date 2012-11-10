using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using VSOptionsDialogResizer.PInvoke;
using VSOptionsDialogResizer.WindowModifiers;

namespace VSOptionsDialogResizer.Specs.WindowModifiers
{
    public class when_arranging_the_ok_button_of_the_options_dialog : WithSubject<OkCancelButtonArranger>
    {
        Establish context = () =>
            {
                The<IPInvoker>()
                    .WhenToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "Button"))
                    .Return(new[] { _ok });
            };

        Because of = () => Subject.Modify(_optionsWindow, 400, 200);

        It should_determine_all_buttons =
            () => The<IPInvoker>().WasToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "Button"));

        It should_determine_ok_button =
            () => The<IPInvoker>().WasToldTo(p => p.GetWindowText(_ok));

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
            _pInvoker.GetWindowText(button);
        }
    }
}