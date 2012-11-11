using System;
using Machine.Fakes;
using Machine.Specifications;
using VSOptionsDialogResizer.PInvoke;

namespace VSOptionsDialogResizer.Specs
{
    public class when_looking_for_the_options_dialog : WithSubject<OptionsDialogFinder>
    {
        Establish context = () =>
        {
            The<IPInvoker>()
                .WhenToldTo(p => p.FindAllWindowsByCaption("Options"))
                .Return(new[] { _optionsWindow, _otherOptionsWindow });
            
            The<IPInvoker>()
                .WhenToldTo(p => p.GetWindow(_optionsWindow, GetWindowCmd.GW_OWNER))
                .Return(_devenvMainWindow);

            The<IPInvoker>()
                .WhenToldTo(p => p.GetClassName(_optionsWindow))
                .Return("#32770");
        };

        Because of = () => _result = Subject.Find(_devenvMainWindow);

        It should_search_for_windows_with_Options_caption = 
            () => The<IPInvoker>().WasToldTo(p => p.FindAllWindowsByCaption("Options"));

        It should_check_if_class_name_matches =
            () => The<IPInvoker>().WasToldTo(p => p.GetClassName(_optionsWindow));

        It should_return_found_Options_window = () => _result.ShouldEqual(_optionsWindow);

        It should_compare_found_Options_windows_with_given_parent_window_handle = 
            () => The<IPInvoker>().WasToldTo(p => p.GetWindow(_optionsWindow, GetWindowCmd.GW_OWNER));

        static readonly IntPtr _devenvMainWindow = new IntPtr(1);
        static readonly IntPtr _optionsWindow = new IntPtr(2);
        static readonly IntPtr _otherOptionsWindow = new IntPtr(3);
        static IntPtr _result;
    }

    public class when_found_a_wrong_options_dialog : WithSubject<OptionsDialogFinder>
    {
        Establish context = () =>
        {
            The<IPInvoker>()
                .WhenToldTo(p => p.FindAllWindowsByCaption("Options"))
                .Return(new[] { _optionsWindow });

            The<IPInvoker>()
                .WhenToldTo(p => p.GetWindow(_optionsWindow, GetWindowCmd.GW_OWNER))
                .Return(_devenvMainWindow);

            The<IPInvoker>()
                .WhenToldTo(p => p.GetClassName(_optionsWindow))
                .Return("Any other then #32770");
        };

        Because of = () => _result = Subject.Find(_devenvMainWindow);

        It should_return_zero_if_class_name_of_found_Options_window_does_not_match = 
            () => _result.ShouldEqual(IntPtr.Zero);
        
        static readonly IntPtr _devenvMainWindow = new IntPtr(1);
        static readonly IntPtr _optionsWindow = new IntPtr(2);
        static IntPtr _result;
    }
}