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
                .WhenToldTo(p => p.FindWindows("Options"))
                .Return(new[] { _optionsWindow, _otherOptionsWindow });
            
            The<IPInvoker>()
                .WhenToldTo(p => p.GetWindow(_optionsWindow, GetWindowCmd.GW_OWNER))
                .Return(_devenvMainWindow);
        };

        Because of = () => _result = Subject.Find(_devenvMainWindow);

        It should_search_for_windows_with_Options_caption = 
            () => The<IPInvoker>().WasToldTo(p => p.FindWindows("Options"));

        It should_return_found_Options_window = () => _result.ShouldEqual(_optionsWindow);

        It should_compare_found_Options_windows_with_given_parent_window_handle = 
            () => The<IPInvoker>().WasToldTo(p => p.GetWindow(_optionsWindow, GetWindowCmd.GW_OWNER));

        static readonly IntPtr _devenvMainWindow = new IntPtr(1);
        static readonly IntPtr _optionsWindow = new IntPtr(2);
        static readonly IntPtr _otherOptionsWindow = new IntPtr(3);
        static IntPtr _result;
    }
}