using System;
using Machine.Fakes;
using Machine.Specifications;
using VSOptionsDialogResizer.PInvoke;
using VSOptionsDialogResizer.WindowModifiers;

namespace VSOptionsDialogResizer.Specs.WindowModifiers
{
    public class when_arranging_the_ok_button_of_the_options_dialog : WithSubject<OkCancelButtonArranger>
    {
        Because of = () => Subject.Modify(_optionsWindow, 400, 200);

        It should_determine_ok_button =
            () => The<IPInvoker>().WasToldTo(p => p.FindAllChildrenByClassName(_optionsWindow, "Button"));

        static readonly IntPtr _optionsWindow = new IntPtr(1);
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
            _pInvoker.FindAllChildrenByClassName(window, "Button");
        }
    }
}